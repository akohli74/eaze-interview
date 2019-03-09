using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interview.Green.Web.Scraper.Controllers;
using Interview.Green.Web.Scraper.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Interview.Green.Web.Scrapper.Tests
{
	[TestClass]
	public class JobControllerTest
	{
		public JobControllerTest()
		{
		}

		[TestMethod]
		public async Task TestJobCreation()
		{
			JobSchedulerService.Start();
			var controller = new JobController();
			var result = await controller.Post("http://google.com", "search");
			Assert.IsFalse(result <= 0);
			JobSchedulerService.Stop();
		}

		[TestMethod]
		public async Task TestJobRetrieval()
		{
			JobSchedulerService.Start();
			var controller = new JobController();
			var id = await controller.Post("http://cnn.com", "Middle East");

			Thread.Sleep(5000);

			var result = controller.Get(id);
			Assert.IsNotNull(result);
			JobSchedulerService.Stop();
		}

		[TestMethod]
		public async Task TestGetAllJobs()
		{
			JobSchedulerService.Start();
			var controller = new JobController();
			await controller.Post("http://google.com", "search");
			await controller.Post("http://msn.com", "Middle East");
			await controller.Post("http://yahoo.com", "Middle East");

			Thread.Sleep(5000);

			var result = controller.Get();
			Assert.AreEqual(3, result.Count());

			JobSchedulerService.Stop();
		}

		[TestMethod]
		public async Task TestDeleteJob()
		{
			JobSchedulerService.Start();
			var controller = new JobController();
			await controller.Post("http://google.com", "search");
			var id2 = await controller.Post("http://msn.com", "Middle East");
			await controller.Post("http://yahoo.com", "Middle East");

			Thread.Sleep(5000);

			controller.Delete(id2);

			var result = controller.Get();
			Assert.AreEqual(2, result.Count());

			JobSchedulerService.Stop();
		}
	}
}
