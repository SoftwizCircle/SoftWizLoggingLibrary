using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SWCLoggingLibrary
{
    public class SWCLogging : ILogger
    {
        private readonly SWCLoggingProvider _swcLoggingProvider;
        public string Category { get; private set; }
        public SWCLogging(SWCLoggingProvider swcLoggingProvider)
        {
            _swcLoggingProvider = swcLoggingProvider;
        }

        public SWCLogging(SWCLoggingProvider swcLoggingProvider, string category)
        {
            _swcLoggingProvider = swcLoggingProvider;
            this.Category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _swcLoggingProvider.ScopeProvider.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var item = new Dictionary<string, string>
            {
                {SWCConstants.LogLevel, logLevel.ToString() },
                {SWCConstants.EventId, eventId.ToString() },
                {SWCConstants.Message, state.ToString() },
                {SWCConstants.LogTime, DateTimeOffset.UtcNow.ToString(SWCConstants.DateFormat) }
            };

            if (exception!=null)
            {
                item.Add(SWCConstants.Exception, exception.Message);
                item.Add(SWCConstants.StackTrace, exception.StackTrace);
            }

            item.Add(SWCConstants.Category, this.Category);

            // well, you never know what it really is
            if (state is IEnumerable<KeyValuePair<string, object>> Properties)
            {
                foreach (KeyValuePair<string, object> props in Properties)
                {
                    item.Add(props.Key, props.Value.ToString());
                }
            }

            // gather info about scope(s), if any
            if (_swcLoggingProvider.ScopeProvider != null)
            {
                _swcLoggingProvider.ScopeProvider.ForEachScope((value, loggingProps) =>
                    {
                        if (value is string)
                        {
                            item.Add(SWCConstants.Scope, Convert.ToString(value));
                        }
                        else if (value is IEnumerable<KeyValuePair<string, object>> props)
                        {
                            foreach (var prop in props)
                            {
                                item.Add(prop.Key, Convert.ToString(prop.Value));
                            }
                        }
                    }, state);
            }

            Add(item);
        }

        /// <summary>
        /// Returns the html of the search page. You only need to place this on one of your UI page and voila! 
        /// you are ready with the log search page. 
        /// </summary>
        /// <returns>Complete HTML page</returns>
        public string GetSearchPage()
        {
            return SWCConstants.SearchPageHTML;
        }

        /// <summary>
        /// It searches the log as per sent request.
        /// </summary>
        /// <param name="swcSearchRequest">Request for the search</param>
        /// <returns>Receives response which will be processed by the search page to show the logs</returns>
        public SWCSearchResponse SearchLogs(SWCSearchRequest swcSearchRequest)
        {
            try
            {
                BooleanQuery rootquery = new BooleanQuery();

                // LogLevel search query
                AddLogLevelQuery(rootquery, swcSearchRequest);

                // Full text search query
                AddFullTextQuery(rootquery, swcSearchRequest);

                // Time range search query
                AddTimeRangeQuery(rootquery, swcSearchRequest);                

                return GetLatestLog(rootquery, swcSearchRequest.NoOfRcordsToFetch);
            }
            catch { }

            return new SWCSearchResponse();
        }

        private void AddTimeRangeQuery(BooleanQuery rootquery, SWCSearchRequest swcSearchRequest)
        {
            if (!string.IsNullOrEmpty(swcSearchRequest.TimeRange))
            {
                long min = 0;
                var currentDateTime = DateTime.UtcNow;

                if (swcSearchRequest.TimeRange.Contains("minute"))
                {
                    min = currentDateTime.AddMinutes(((int)Enum.Parse(typeof(TimeRange), swcSearchRequest.TimeRange) * -1)).AsYMDHMS();
                }
                else if (swcSearchRequest.TimeRange.Contains("hour"))
                {
                    min = currentDateTime.AddHours(((int)Enum.Parse(typeof(TimeRange), swcSearchRequest.TimeRange) * -1)).AsYMDHMS();
                }
                else if (swcSearchRequest.TimeRange.Contains("day"))
                {
                    min = currentDateTime.AddDays(((int)Enum.Parse(typeof(TimeRange), swcSearchRequest.TimeRange) * -1)).AsYMDHMS();
                }
                else if (swcSearchRequest.TimeRange.Contains("month"))
                {
                    min = currentDateTime.AddMonths(((int)Enum.Parse(typeof(TimeRange), swcSearchRequest.TimeRange) * -1)).AsYMDHMS();
                }

                NumericRangeQuery<long> numericRangeQuery = NumericRangeQuery.NewInt64Range(SWCConstants.CreationDate, min, currentDateTime.AsYMDHMS(), true, true);

                rootquery.Add(numericRangeQuery, Occur.MUST);
            }
        }

        private void AddFullTextQuery(BooleanQuery rootquery, SWCSearchRequest swcSearchRequest)
        {
            if (!string.IsNullOrEmpty(swcSearchRequest.TextToSearch))
            {
                //string newterm = string.Empty;
                //string[] tok = term.Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);
                //tok.ForEach(x => newterm += x.EnsureStartsWith(" *").EnsureEndsWith("* "));
                string[] fields = string.IsNullOrEmpty(swcSearchRequest.Fields)
                    ? new string[8] { SWCConstants.Message, SWCConstants.Exception, SWCConstants.StackTrace, SWCConstants.Scope, SWCConstants.EventId, SWCConstants.ActionName, SWCConstants.ActionId, SWCConstants.TraceId }
                    : new string[1] { swcSearchRequest.Fields };
                var analyzer = new StandardAnalyzer(SWCConstants.AppLuceneVersion);
                var parser = new MultiFieldQueryParser(SWCConstants.AppLuceneVersion, fields, analyzer);
                parser.DefaultOperator = QueryParser.AND_OPERATOR;
                parser.AllowLeadingWildcard = true;

                Query query = parser.Parse(swcSearchRequest.TextToSearch.Trim().ToLower());

                rootquery.Add(query, Occur.MUST);
            }
        }

        private void AddLogLevelQuery(BooleanQuery rootquery, SWCSearchRequest swcSearchRequest)
        {
            BooleanQuery logLevelQuery = new BooleanQuery();
            if (!string.IsNullOrEmpty(swcSearchRequest.LoggingType))
            {
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, swcSearchRequest.LoggingType.Trim().ToLower())), Occur.MUST);
            }
            else
            {
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, LogType.Critical.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, LogType.Debug.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, LogType.Error.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, LogType.Information.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, LogType.Trace.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term(SWCConstants.LogLevel, LogType.Warning.ToString().ToLower())), Occur.SHOULD);
            }

            rootquery.Add(logLevelQuery, Occur.MUST);
        }

        private SWCSearchResponse GetLatestLog(Query query, int limit)
        {
            SWCSearchResponse response = new SWCSearchResponse();
            int totalScoreDocs = 0;
            using (Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(_swcLoggingProvider.Options.FolderPath)))
            using (DirectoryReader reader = DirectoryReader.Open(directory))
            {
                var searcher = new IndexSearcher(reader);

                var sort = new Sort(new SortField(SWCConstants.CreationDate, SortFieldType.INT64, true));
                var hits = searcher.Search(query, null, limit, sort);
                totalScoreDocs = hits.ScoreDocs.Count();

                Console.WriteLine(hits.TotalHits + " result(s) found for query: " + query.ToString());
                foreach (var scoreDoc in hits.ScoreDocs)
                {
                    var doc = searcher.Doc(scoreDoc.Doc);

                    var fields = new Dictionary<string, string>();

                    foreach (var item in doc.Fields)
                    {
                        if (item.Name != SWCConstants.CreationDate && item.Name != "{OriginalFormat}")
                        {
                            fields.Add(item.Name, item?.GetStringValue());
                        }                        
                    }

                    response.SWCSearchResults.Add(fields);
                }
            }

            response.TotalScoreDocs = totalScoreDocs;

            return response;
        }

        void Add(IDictionary<string, string> values)
        {
            using(Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(_swcLoggingProvider.Options.FolderPath)))
            using (IndexWriter writer = new IndexWriter(directory, new IndexWriterConfig(SWCConstants.AppLuceneVersion, new StandardAnalyzer(SWCConstants.AppLuceneVersion))))
            {
                var document = new Document();
                foreach (var kvp in values)
                {
                    document.Add(new TextField(
                        kvp.Key,
                        kvp.Value == null ? string.Empty : kvp.Value?.ToLowerInvariant(),
                        Field.Store.YES));
                }

                document.Add(new Int64Field(SWCConstants.CreationDate, DateTime.UtcNow.AsYMDHMS(), Field.Store.YES));

                writer.AddDocument(document);
                writer.Flush(true, false);
                writer.Commit();
            }
        }
    }
}