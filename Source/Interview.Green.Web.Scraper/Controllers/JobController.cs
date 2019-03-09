using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Interview.Green.Web.Scraper.Interfaces;
using Interview.Green.Web.Scraper.Service;

namespace Interview.Green.Web.Scraper.Controllers
{
    public class JobController : ApiController
    {
	    private IJobSchedulerService _service;

	    public JobController()
	    {
		    _service = new JobSchedulerService();
	    }

        // GET: api/job
        public IEnumerable<int> Get()
        {
	        return _service.GetAllJobs();
        }

        // GET: api/job/5
        public IEnumerable<string> Get(int id)
        {

			return _service.GetJobData(id);
        }

        // POST: api/job
        public async Task<int> Post([FromBody] string siteUri, string tag)
        {
			var id = await _service.Schedule(siteUri, tag);

	        return id;
        }

        // PUT: api/job/5
        public void Put(int id, [FromBody] string value)
        {
	        _service.Reschedule(id, value);
        }

        // DELETE: api/job/5
        public void Delete(int id)
        {
	        _service.Delete(id);
        }
    }
}