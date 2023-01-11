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
        private IndexWriter writer = null;

        public SWCLogging(SWCLoggingProvider swcLoggingProvider)
        {
            _swcLoggingProvider = swcLoggingProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
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
                {"LogLevel", logLevel.ToString() },
                {"EventId", eventId.ToString() },
                {"Message", state.ToString() },
                {"Exception", exception?.Message },
                {"StackTrace", exception != null ? exception.StackTrace : string.Empty },
                {"LogTime", DateTimeOffset.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff") },
            };

            var writer = CreateIndex();
            Add(writer, item);
            writer.Flush(true, true);
            //writer.Optimize();
            writer.Commit();
            writer.Dispose();
        }

        public string GetSearchPage()
        {
            var k = File.ReadAllText("index.html");
            return k;
        }

        public SWCSearchResponse SearchTextNew(SWCSearchRequest swcSearchRequest)
        {
            BooleanQuery rootquery = new BooleanQuery();

            // LogLevel search query
            AddLogLevelQuery(rootquery, swcSearchRequest);

            // Full text search query
            AddFullTextQuery(rootquery, swcSearchRequest);

            // Time range search query
            AddTimeRangeQuery(rootquery, swcSearchRequest);

            return GetLatestLog(rootquery, swcSearchRequest.NoOfRecordsToFetch);
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
                else if(swcSearchRequest.TimeRange.Contains("hour"))
                {
                    min = currentDateTime.AddHours(((int)Enum.Parse(typeof(TimeRange), swcSearchRequest.TimeRange) * -1)).AsYMDHMS();
                }
                else if(swcSearchRequest.TimeRange.Contains("day"))
                {
                    min = currentDateTime.AddDays(((int)Enum.Parse(typeof(TimeRange), swcSearchRequest.TimeRange) * -1)).AsYMDHMS();
                }
                else if(swcSearchRequest.TimeRange.Contains("month"))
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
                string[] k = new string[3] { "Message", "Exception", "StackTrace" };
                var analyzer = new StandardAnalyzer(SWCConstants.AppLuceneVersion);
                var parser = new MultiFieldQueryParser(SWCConstants.AppLuceneVersion, k, analyzer);
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
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", swcSearchRequest.LoggingType.Trim().ToLower())), Occur.MUST);
            }
            else
            {
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", LogType.Critical.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", LogType.Debug.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", LogType.Error.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", LogType.Information.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", LogType.Trace.ToString().ToLower())), Occur.SHOULD);
                logLevelQuery.Add(new TermQuery(new Term("LogLevel", LogType.Warning.ToString().ToLower())), Occur.SHOULD);
            }

            rootquery.Add(logLevelQuery, Occur.MUST);
        }

        private SWCSearchResponse GetLatestLog(Query query, int limit)
        {
            var writer = CreateIndex();

            DirectoryReader reader = writer.GetReader(true);

            //query = query == null ? new MatchAllDocsQuery() : query;
            var SWCSearchResults = new List<SWCSearchResult>();
            int totalScoreDocs = 0;

            //var indexDirectory = FSDirectory.Open(new DirectoryInfo(_swcLoggingProvider.Options.FolderPath));
            var searcher = new IndexSearcher(reader);

            var sort = new Sort(new SortField(SWCConstants.CreationDate, SortFieldType.INT64, true));
            var hits = searcher.Search(query, null, limit, sort);
            totalScoreDocs = hits.ScoreDocs.Count();

            Console.WriteLine(hits.TotalHits + " result(s) found for query: " + query.ToString());
            foreach (var scoreDoc in hits.ScoreDocs)
            {
                var doc = searcher.Doc(scoreDoc.Doc);
                SWCSearchResults.Add(
                    new SWCSearchResult
                    {
                        LogLevel = doc.Get(SWCConstants.LogLevel),
                        EventId = doc.Get(SWCConstants.EventId),
                        Message = doc.Get(SWCConstants.Message),
                        Exception = doc.Get(SWCConstants.Exception),
                        StackTrace = doc.Get(SWCConstants.StackTrace),
                        LogTime = doc.Get(SWCConstants.LogTime),
                        CreationDate = doc.Get(SWCConstants.CreationDate)
                    });
            }

            reader.Dispose();
            writer.Flush(false, true);
            writer.Dispose();
            //indexDirectory.Dispose();

            return new SWCSearchResponse { SWCSearchResults = SWCSearchResults, TotalScoreDocs = totalScoreDocs };
        }

        private List<SWCSearchResult> Search(IndexSearcher searcher, Dictionary<string, string> values)
        {
            var query = new BooleanQuery();
            foreach (var termQuery in values.Select(kvp => new TermQuery(new Term(kvp.Key, kvp.Value.ToLowerInvariant()))))
            {
                query.Add(new BooleanClause(termQuery, Occur.MUST));
            }

            return Search(searcher, query);
        }

        private List<SWCSearchResult> Search(IndexSearcher searcher, Query query)
        {
            var sortField = new SortField(SWCConstants.CreationDate, SortFieldType.INT64, true);
            var inverseSort = new Sort(sortField);

            var results = searcher.Search(query, null, 100, inverseSort); // exception thrown here

            List<SWCSearchResult> SWCSearchResult = new List<SWCSearchResult>();
            var matches = results.ScoreDocs;
            foreach (var item in matches)
            {
                var id = item.Doc;
                Document doc = searcher.Doc(id);
                SWCSearchResult.Add(new SWCSearchResult { LogLevel = doc.Get(SWCConstants.LogLevel) });
                SWCSearchResult.Add(new SWCSearchResult { EventId = doc.Get(SWCConstants.EventId) });
                SWCSearchResult.Add(new SWCSearchResult { Message = doc.Get(SWCConstants.Message) });
                SWCSearchResult.Add(new SWCSearchResult { Exception = doc.Get(SWCConstants.Exception) });
                SWCSearchResult.Add(new SWCSearchResult { StackTrace = doc.Get(SWCConstants.StackTrace) });
                SWCSearchResult.Add(new SWCSearchResult { LogTime = doc.Get(SWCConstants.LogTime) });
                SWCSearchResult.Add(new SWCSearchResult { CreationDate = doc.Get(SWCConstants.CreationDate) });
            }

            return SWCSearchResult;
        }

        IndexWriter CreateIndex()
        {
            if (writer == null)
            {
                //var directory = new RAMDirectory();
                Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(_swcLoggingProvider.Options.FolderPath));
                var analyzer = new StandardAnalyzer(SWCConstants.AppLuceneVersion);
                writer = new IndexWriter(directory, new IndexWriterConfig(SWCConstants.AppLuceneVersion, analyzer));
            }

            return writer;
        }

        void Add(IndexWriter writer, IDictionary<string, string> values)
        {
            var document = new Document();
            foreach (var kvp in values)
            {
                //document.Add(new Field(
                //    kvp.Key,
                //    kvp.Value == null ? string.Empty : kvp.Value?.ToLowerInvariant(),
                //    Field.Store.YES, Field.Index.ANALYZED));

                document.Add(new TextField(
                    kvp.Key,
                    kvp.Value == null ? string.Empty : kvp.Value?.ToLowerInvariant(),
                    Field.Store.YES));
            }

            document.Add(new Int64Field(SWCConstants.CreationDate, DateTime.UtcNow.AsYMDHMS(), Field.Store.YES));

            //document.Add(new NumericField("CreationDate", Field.Store.YES, true).SetLongValue(AsYMDHMS(DateTime.UtcNow)));

            writer.AddDocument(document);
        }
    }
}