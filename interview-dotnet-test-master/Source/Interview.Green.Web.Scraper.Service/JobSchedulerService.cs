using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Interview.Green.Web.Scraper.Interfaces;
using Interview.Green.Web.Scrapper.Service;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Interview.Green.Web.Scraper.Service
{
    public class JobSchedulerService : IJobSchedulerService
    {
	    private static IScheduler _scheduler;
	    private static Dictionary<int, IJobDetail> Jobs;

	    static JobSchedulerService()
	    {
			Jobs = new Dictionary<int, IJobDetail>();
	    }

	    public JobSchedulerService()
	    {
	    }

	    public static void Start()
	    {
			_scheduler = StdSchedulerFactory.GetDefaultScheduler();
			_scheduler.Start();
		}

	    public static void Stop()
	    {
		    _scheduler.Shutdown(false);
			Jobs = new Dictionary<int, IJobDetail>();
	    }

	    public async Task<int> Schedule(string siteUri, string tag)
	    {
		    int id = (Jobs.Keys.Any() ? Jobs.Keys.Last() + 1 : 1);

			IJobDetail job = JobBuilder.Create<WebScrapingJob>().Build();
		    job.JobDataMap.Add("siteUri", siteUri);
		    job.JobDataMap.Add("tag", tag);
		    job.JobDataMap.Add("result", new List<string>());

			ITrigger trigger = TriggerBuilder.Create()
			.WithIdentity("trigger" + id, "group1")
			.StartNow()
			.WithSimpleSchedule(x => x
			.WithIntervalInSeconds(50)
			.RepeatForever())
			.Build();

		    Jobs.Add(id, job);

			await Task.Run(delegate { _scheduler.ScheduleJob(job, trigger); });

			return id;
	    }

	    public IEnumerable<string> GetJobData(int id)
	    {
		    var job = _scheduler.GetJobDetail(Jobs[id].Key);
			return job.JobDataMap["result"] as IEnumerable<string>;
	    }

	    public IEnumerable<int> GetAllJobs()
	    {
		    return Jobs.Keys;
	    }

	    public void Reschedule(int id, string siteUri)
	    {
		    Jobs[id].JobDataMap["siteUri"] = siteUri;
	    }

	    public void Delete(int id)
	    {
		    _scheduler.DeleteJob(Jobs[id].Key);
		    Jobs.Remove(id);
	    }
    }
}