﻿using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Interview.Green.Web.Scraper.Service;

namespace Interview.Green.Web.Scraper
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
			JobSchedulerService.Start();
        }
    }
}