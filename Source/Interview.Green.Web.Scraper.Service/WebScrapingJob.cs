using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Common.Logging.Simple;
using HtmlAgilityPack;
using Quartz;

namespace Interview.Green.Web.Scrapper.Service
{
	[PersistJobDataAfterExecution]
	public class WebScrapingJob : IJob
	{
		private string _tag = string.Empty;
		private IJobExecutionContext _context;

		public void Execute(IJobExecutionContext context)
		{
			try
			{
				_context = context;
				var siteUri = context.MergedJobDataMap["siteUri"]?.ToString();
				_tag = context.MergedJobDataMap["tag"]?.ToString();

				if (siteUri != null && _tag != null)
				{
					WebClient wc = new WebClient();
					wc.DownloadStringCompleted += DownloadStringCompletedHanlder;
					wc.DownloadStringAsync(new Uri(siteUri));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void DownloadStringCompletedHanlder(object sender, DownloadStringCompletedEventArgs e)
		{
			_tag = "\"" + _tag + "\"";

			var doc = new HtmlDocument();
			doc.LoadHtml(e.Result);
			var nav = doc.CreateNavigator();
			var elements = nav.Select("//a[contains(.," + _tag + ")]");
			var results = new List<string>();
			while (elements.MoveNext())
			{
				results.Add(elements.Current.OuterXml + elements.Current.InnerXml + "</a>");
			}
			var jobResults = (List<string>) _context.JobDetail.JobDataMap["result"];
			jobResults.AddRange(results);
			_context.JobDetail.JobDataMap["result"] = jobResults;
		}
	}
}
