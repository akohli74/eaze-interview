using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;

namespace Interview.Green.Web.Scraper.Interfaces
{
    public interface IJobSchedulerService
    {
	    Task<int> Schedule(string siteUri, string tag);

		IEnumerable<string> GetJobData(int id);

		IEnumerable<int> GetAllJobs();

		void Reschedule(int id, string siteUri);

		void Delete(int id);
	}
}