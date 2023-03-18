﻿using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    public static class SWCConstants
    {
        public const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        public const string LogLevel = "LogLevel";
        public const string EventId = "EventId";
        public const string Message = "Message";
        public const string Exception = "Exception";
        public const string StackTrace = "StackTrace";
        public const string LogTime = "LogTime";
        public const string CreationDate = "CreationDate";
        public const string DateFormat = "dd-MM-yyyy HH:mm:ss.fff";
        public const string RequestId = "RequestId";
        public const string RequestPath = "RequestPath";
        public const string SpanId = "SpanId";
        public const string TraceId = "TraceId";
        public const string ParentId = "ParentId";
        public const string StateText = "StateText";
        public const string Scope = "Scope";
        public const string Category = "Category";
        public const string ActionName = "ActionName";
        public const string ActionId = "ActionId";

        public const string SearchPageHTML = headstart + style1 + style2 + style3 + style4 + style5 + style6 + style7 + style8 + headend + bodystart + body1 + body2 + body3formstart + body4formend + body5 + body6scriptstart + scriptend + end;

        private const string headstart = "<!DOCTYPE html><html lang='en'><head><meta charset='utf-8' /><meta name='viewport' content='width=device-width, initial-scale=1.0' /><title>SWC Logging</title><link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@3.3.7/dist/css/bootstrap.min.css' />";
        private const string style1 = "<style>html{position: relative;min-height: 100%;}body {/* Margin bottom by footer height */margin-bottom: 60px;}footer {position: absolute;bottom: 0;width: 100%;/* Set the fixed height of the footer here */height: 60px;background-color: #f5f5f5;}@import url('https://fonts.googleapis.com/css2?family=IBM+Plex+Sans:wght@300;400;500;600;700&display=swap');";
        private const string style2 = ":root{--color-bg:#EEEDEB;--color-title:#0E1C4E;--color-summary-1:#FFF6EE;--color-summary-1-highlight:#FFC48B;--color-summary-2:#FAFAFF;--color-summary-2-highlight:#B4B3FF;--color-summary-3:#FFF0F3;--color-summary-3-highlight:#FFB3C0;--font-ibm-plex-sans:'IBMPlexSans',sans-serif;}";
        private const string style3 = ".card {  background: white; margin-top: 40px; margin-bottom: 40px; border-radius: 4px; box-shadow: 0 8px 10px rgba(0, 0, 0, 0.1); width: 100%;}.card h1 {font-family: var(--font-ibm-plex-sans); font-style: normal; font-weight: bold; font-size: 20px;line-height: 1.2;color: var(--color-title); margin-bottom: 20px;}.card details {display: flex;border-radius: 5px;overflow: hidden;background: rgba(0, 0, 0, 0.05);border-left: 15px solid gray;padding: 1px;}.card details {margin-top: 3px;}.card details.warning {--highlight: var(--color-summary-1-highlight); background: var(--color-summary-1); border-left-color: var(--color-summary-1-highlight); }.card details.warning p {list-style-type: corona-warning;} .card details.info { --highlight: var(--color-summary-2-highlight); background: var(--color-summary-2); border-left-color: var(--color-summary-2-highlight);}";
        private const string style4 = ".card details.info p {list-style-type: corona-info;} .card details.alert {--highlight: var(--color-summary-3-highlight);background: var(--color-summary-3);border-left-color: var(--highlight);}.card details.alert p {list-style-type: corona-alert;}.card details summary, .card details p {position: relative;display: flex;flex-direction: row;align-content: center;justify-content: flex-start;font-family: var(--font-ibm-plex-sans);font-style: normal;font-weight: normal;color: var(--color-title);padding: 10px;cursor: pointer;}.card details summary::-webkit-details-marker, .card details p::-webkit-details-marker {display: none;}.card details summary:focus, .card details p:focus {outline: solid 3px var(--highlight);}.card details summary::-moz-selection, .card details p::-moz-selection {background-color: var(--highlight);}.card details summary::selection, .card details p::selection {background-color: var(--highlight);}.card details p {cursor: default;list-style-type: corona;}";
        private const string style5 = ".card details summary::before {cursor: pointer;position: absolute;display: inline-flex;width: 1rem;height: 1rem;left: 0rem;margin-right: 0.5rem; content: url(data:image/svg+xml,%3Csvg width='100%' height='100%' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M22.6066 12H1.3934' stroke='%23202842' stroke-width='1.875' stroke-linecap='round' stroke-linejoin='round'/%3E%3Cpath d='M12 1.39343V22.6066' stroke='%23202842' stroke-width='1.875' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E%0A);}";
        private const string style6 = ".card details[open] summary {font-weight: 700;}.card details[open] summary::before {transform: rotate(45deg);content: url(data:image/svg+xml,%3Csvg width='100%' height='100%' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M22.6066 12H1.3934' stroke='%23202842' stroke-width='3.6' stroke-linecap='round' stroke-linejoin='round'/%3E%3Cpath d='M12 1.39343V22.6066' stroke='%23202842' stroke-width='3.6' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E%0A);}";
        private const string style7 = ".accordianheaderspan{padding-right: 6px; font-family: fantasy;} .accordianheader {text-overflow: ellipsis;overflow: hidden;white-space: nowrap;display: inline-block !important;padding-left: 3px !important;}.odddetailsrow {background-color: #dcdcfc;margin-top: 0px;padding-bottom: 0px !important;padding-top: 0px !important;margin-bottom: 0px;display: grid !important;grid-template-columns: 1fr 13fr;}.evendetailsrow {background-color: lavender;padding-top: 0px !important;margin-top: 0px;padding-bottom: 0px !important;margin-bottom: 0px;display: grid !important;grid-template-columns: 1fr 13fr;}.detailsrowspan {font-family: emoji !important;font-weight: 600;}.criticallog {border-left-color: rgba(240,50,50,0.75) !important;} .debuglog { border-left-color: rgba(255,180,50,0.75) !important; } .errorlog { border-left-color: #ff9800 !important; } .informationlog { border-left-color: rgba(100,210,80,0.75) !important; } .tracelog{ border-left-color: rgba(170, 90, 240, 0.75)!important; } .warninglog{ border-left-color: rgba(90, 165, 255, 0.75)!important; }";
        private const string style8 = ".loader { position: fixed; width: 48px; height: 48px; display: block; top: 50%; left: 50%;z-index: 1500; } .loader:before, .loader:after {content: ''; display: block;border: 23px solid transparent;border-top-color: #ffc107;position: absolute;left: 0;top: 0;animation: weld-rotate 2s infinite ease-in;} .loader:before { border-color: transparent transparent transparent #FF3D00;animation-delay: 0.5s; } @keyframes weld-rotate { 0%, 25% {transform: rotate(0deg) } 50%, 75% {transform: rotate(180deg)} 100% {transform: rotate(360deg) } } .highlight {background-image: linear-gradient(to right, #f2712136, #e940573d, #8a238726); border-radius: 6px; padding: 3px 0px;}</style>";
        private const string headend = "</head>";

        private const string bodystart = "<body><span class='loader' id='spanloader'></span><!-- Fixed navbar --><div class='navbar navbar-default navbar-fixed-top' role='navigation' style='background-color: #c62b3f; '><div class='container'><div class='navbar-header'><button type='button' class='navbar-toggle' data-toggle='collapse' data-target='.navbar-collapse'><span class='sr-only'>Toggle navigation</span><span class='icon-bar'></span><span class='icon-bar'></span><span class='icon-bar'></span></button>";
        private const string body1 = "<a class='navbar-brand' style='color: white; background-image: url(data:image/png;base64,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); background-repeat: no-repeat; background-size: 50px 42px; padding-left: 51px; background-position-y: center; '>SWC Logging</a>";
        private const string body2 = "</div><div class='collapse navbar-collapse'></div><!--/.nav-collapse --></div></div><div class='container' style='margin-top:35px;'><main role='main' class='pb-3'><div id='row-detail'></div><div style='position: sticky; top: 35px; z-index: 1028; background-color: white;'><div gui-structure-top-panel='' class='gui-structure-top-panel gui-p-6 gui-border-b gui-border-b-solid'><div gui-search-bar='' class='gui-flex gui-items-center gui-h-full gui-w-3/5 gui-mr-auto gui-search-bar'><div gui-search-icon='' class='gui-search-icon gui-icon'><svg xmlns='http://www.w3.org/2000/svg' width='10.231' height='10.601' viewBox='0 0 10.231 10.601' class='gui-search-icon-svg'><line x2='1.77' y2='1.77' transform='translate(7.4 7.77)' fill='none' stroke-linecap='round' stroke-linejoin='round' stroke-width='1.5'></line><circle cx='4.02' cy='4.02' r='4.02' transform='translate(0.5 0.5)' stroke-width='1' stroke-linecap='round' stroke-linejoin='round' fill='none'></circle></svg></div>";

        private const string body3formstart = "<form id='searchForm' class='card' style='margin-top:0px; margin-bottom:15px;'><details class='info' style='border-bottom: 7px solid #dee2e6; border-left: 0;'><summary style='margin-top: 3px;'><div class='form-group' style='margin-bottom:0px;'><label for='demo_overview' style='font-size: smaller; position: absolute; top: -7px; margin-bottom: 0px; font-weight: 100;'>Logging type</label><select name='LoggingType' id='LoggingType' class='form-control' style='width:141px;'><option value=''>All</option><option value='Critical'>Critical</option><option value='Debug'>Debug</option><option value='Error'>Error</option><option value='Information'>Information</option><option value='Trace'>Trace</option><option value='Warning'>Warning</option></select></div><div class='form-group' style='margin-bottom:0px; margin-left:5px;'><label for='demo_overview' style='font-size: smaller; position: absolute; top: -7px; margin-bottom: 0px; font-weight: 100;'>Fields</label><select name='Fields' id='Fields' class='form-control' style='width:141px;'><option value=''>All</option><option value='Scope'>Scope</option><option value='Message'>Message</option><option value='Exception'>Exception</option><option value='StackTrace'>StackTrace</option><option value='EventId'>EventId</option><option value='ActionName'>ActionName</option><option value='ActionId'>ActionId</option><option value='TraceId'>TraceId</option></select></div><div style='width: 80%; padding-left: 5px; padding-right: 5px;'><div class='form-group' style='margin-bottom:0px;'><label for='demo_overview' style='font-size: smaller; position: absolute; top: -7px; margin-bottom: 0px; font-weight: 100;'>Text to search</label><input class='.gui-border-0 gui-w-full gui-h-full gui-py-5 gui-pr-5 gui-pl-21 ng-untouched ng-pristine ng-valid form-control' id='TextToSearch' name='TextToSearch' placeholder='Search logs' type='text' value='' />";
        private const string body4formend = "</div></div><div style='width: 20%; padding-left: 0px; padding-right: 5px;'><div class='form-group' style='margin-bottom:0px;'><label for='demo_overview' style='font-size: smaller; position: absolute; top: -7px; margin-bottom: 0px; font-weight: 100;'>Time range</label><select name='TimeRange' id='TimeRange' class='form-control'><option value=''>All</option><option value='Last_15_minutes'>Last 15 minutes</option><option value='Last_30_minutes'>Last 30 minutes</option><option value='Last_1_hour'>Last 1 hour</option><option value='Last_4_hours'>Last 4 hours</option><option value='Last_12_hours'>Last 12 hours</option><option value='Last_24_hours'>Last 24 hours</option><option value='Last_7_days'>Last 7 days</option><option value='Last_30_days'>Last 30 days</option><option value='Last_60_days'>Last 60 days</option><option value='Last_90_days'>Last 90 days</option><option value='Last_6_month'>Last 6 months</option><option value='Last_1_year'>Last 1 year</option><option value='Last_2_years'>Last 2 years</option><option value='Last_5_years'>Last 5 years</option></select></div></div><div class='icon-details-preview-rendering text-center'><button id='btnGo' class='btn btn-primary' type='button'>Go</button></div><div style='padding-left: 5px; font-size: x-large;'>+</div></summary><p style='padding-bottom: 0px; margin-bottom: 0px; padding-top: 0px; font-size: x-small;'><i>More refine search optoins to be released in future.</i></p></details><input type='hidden' name='PageNumber' id='PageNumber' /></form>";
        private const string body5 = "</div></div><div style='width:59%; float: left;'><h6 align='left' style='margin:0px 7px 0px 4px; float:left;'>Total Logs: <span class='label label-default totalScoreDocs'>0</span></h6><h6 align=right style='margin:0px 7px 0px 4px; float:right;'><table class='charts-css column multiple'><tr style='height: 15px; text-align: center; font-size: xx-small; font-weight: 700;'><td title = 'Critical' style='width: 35px; background-color: rgba(240,50,50,0.75);'>C</td><td title = 'Debug' style='width: 35px; background-color: rgba(255,180,50,0.75);'>D</td><td title = 'Error' style='width: 35px; background-color: #ff9800;'>E</td><td title = 'Information' style='width: 35px; background-color: rgba(100,210,80,0.75);'>I</td><td title = 'Trace' style='width: 35px; background-color: rgba(170,90,240,0.75);'>T</td><td title = 'Warning' style='width: 35px; background-color: rgba(90,165,255,0.75);'>W</td></tr></table></h6></div><div><h6 align='right' style='margin:0px 7px 0px 4px' ;>Showing: <span class='label label-default' id='currentShow'>0</span> Out of <span class='label label-default totalScoreDocs'>0</span></h6></div></div><div id='logContainer' class='card' style='margin-top:0px;'></div></main></div><footer class='footer'><div class='container' style='margin-top:27px;'><span class='text-muted'>&copy; <script>document.write(new Date().getFullYear())</script> - SoftWiz Circle</span></div></footer>";

        private const string body6scriptstart = "<script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.1/jquery.min.js'></script><script src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.2.3/js/bootstrap.bundle.min.js'></script>";
        private const string scriptend = "<script>var indexcounter=1,ajaxInprogress=!1;function GetAllLogsFromStart(){indexcounter=1,$(\"#PageNumber\").val(\"\"),$(\"#logContainer\").text(\"\"),GetAllLogs()}function GetAllLogs(){$(\"#spanloader\").show(),ajaxInprogress=!0,$.ajax({type:\"Post\",dataType:\"json\",url:\"/SWCL/GetLogs\",data:$(\"#searchForm\").serialize(),success:function(e){0==e.result.swcSearchResults.length&&(0==e.result.totalScoreDocs||$(\"details.warning\").length<e.result.totalScoreDocs)?(0==$(\"#logContainer #divNoLogFound\").length&&$(\"#logContainer\").text(\"\").append('<div id=divNoLogFound style=margin-bottom:0px; class=\"alert alert-secondary\" role=alert><center><b>No log found.</b> Please refine the search.</center></div>'),$(\"#currentShow\").text(0),$(\".totalScoreDocs\").text(0),$(\"#spanloader\").hide()):(appendLogsInHTML(e.result.swcSearchResults),$(\".totalScoreDocs\").text(e.result.totalScoreDocs),$(\"#spanloader\").hide()),$(\"#PageNumber\").val(e.result.pageNumber),$(\"#currentShow\").text(e.result.noOfVisibleRecords),ajaxInprogress=!1},error:function(e){ajaxInprogress=!1}})}function appendLogsInHTML(e){$(\"#logContainer #divNoLogFound\").length>0&&$(\"#logContainer\").text(\"\"),$.each(e,function(e,a){var t='<details class=\"warning '+a.LogLevel+'log\">';t+=\"<summary class=accordianheader> <span class=accordianheaderspan>\"+counter()+\"</span> \"+a.LogTime+\" : \"+highlightSearchedText(a.Message)+\"</summary >\";Object.entries(a).map(([e,a],o=entry)=>{t+='<p class=\"'+(o%2==0?\"evendetailsrow\":\"odddetailsrow\")+'\" ><span class=detailsrowspan>'+e+\"</span><span> \"+highlightSearchedText(a)+\" </span></p>\"}),t+=\"</details>\",$(\"#logContainer\").append(t)})}function counter(){return indexcounter++}function highlightSearchedText(e){var a=$(\"#TextToSearch\").val().trim();return\"\"==a?e:e.replace(RegExp(a,\"gi\"),'<span class=\"highlight\">'+a+\"</span>\")}$(document).ready(function(){$(\"#PageNumber\").val(1),$(\"#btnGo\").on(\"click\",GetAllLogsFromStart),$(\"#PageNumber\").val(\"\"),GetAllLogs(),$(window).scroll(function(){$(window).scrollTop()>$(document).height()-$(window).height()-20&&!1==ajaxInprogress&&($(\"#PageNumber\").val(parseInt($(\"#PageNumber\").val())+1),GetAllLogs())}),$(\"#TextToSearch\").on(\"keypress\",function(e){if(13==e.keyCode)return $(\"#btnGo\").click(),!1})});</script>";
        private const string end = "</body></html>";
    }
}