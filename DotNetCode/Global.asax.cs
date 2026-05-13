using OfficeConnect_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OfficeConnect_Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Timer _timer;
        private static Timer _timer10AM;
        private static Timer _timer3AM;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ScheduleDailyJob();
        }
        private void ScheduleDailyJob()
        {
            // Timer for 10 AM (RunDailyApi)
            DateTime now = DateTime.Now;
            DateTime nextRun10AM = DateTime.Today.AddHours(10); // 10 AM

            if (now > nextRun10AM)
            {
                nextRun10AM = nextRun10AM.AddDays(1);
            }

            TimeSpan initialDelay10AM = nextRun10AM - now;

            _timer10AM = new Timer(
                RunDailyApi,
                null,
                initialDelay10AM,
                TimeSpan.FromDays(1)
            );

            // Timer for 3 AM (RunLeaveCredits)
            DateTime now1 = DateTime.Now;
            DateTime nextRun3AM = DateTime.Today.AddHours(3); // 3 AM

            if (now1 > nextRun3AM)
            {
                nextRun3AM = nextRun3AM.AddDays(1);
            }

            TimeSpan initialDelay3AM = nextRun3AM - now1;

            _timer3AM = new Timer(
                RunLeaveCredits,
                null,
                initialDelay3AM,
                TimeSpan.FromDays(1)
            );

            //////// every 5 mins - testing 
            ////TimeSpan startDelay = TimeSpan.Zero;            // start now
            ////TimeSpan interval = TimeSpan.FromMinutes(5);    // every 5 minutes

            ////_timer = new Timer(
            ////    RunDailyApi,
            ////    null,
            ////    startDelay,
            ////    interval
            ////);
        }

        private void RunDailyApi(object state)
        {
            // Call your API logic here
            EmployeeMasterModel.FetchAttendance();
        }

        private void RunLeaveCredits(object state)
        {
            // Call your API logic here
            EmployeeMasterModel.CFLeaveCredits();
            EmployeeMasterModel.EmployeeConfirmationMail();
        }
        public void StopScheduler()
        {
            _timer10AM?.Dispose();
            _timer3AM?.Dispose();
            _timer?.Dispose();
        }
        protected void Application_End()
        {
            _timer?.Dispose();
        }
        ////protected void Application_BeginRequest(object sender, EventArgs e)
        ////{
        ////    var context = HttpContext.Current;
        ////    var response = context.Response;
        ////    var request = context.Request;

        ////    // CORS Headers
        ////    response.AddHeader("Access-Control-Allow-Origin", "http://192.168.2.61");
        ////    response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        ////    response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
        ////    response.AddHeader("Access-Control-Allow-Credentials", "true"); // if cookies are used

        ////    // Security Headers
        ////    response.AddHeader("Strict-Transport-Security", "max-age=31536000");
        ////    response.AddHeader("Content-Security-Policy", "default-src 'self'");
        ////    response.AddHeader("X-Content-Type-Options", "nosniff");
        ////    response.AddHeader("X-Frame-Options", "DENY");
        ////    response.AddHeader("X-Xss-Protection", "1; mode=block");
        ////    response.AddHeader("Referrer-Policy", "strict-origin-when-cross-origin");
        ////    response.Headers.Remove("Server");

        ////    // Handle OPTIONS request
        ////    if (request.HttpMethod == "OPTIONS")
        ////    {
        ////        response.StatusCode = 200;
        ////        response.SuppressContent = true;
        ////        response.End();
        ////    }
        ////}
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var response = context.Response;

            // enable CORS  
            response.AddHeader("Access-Control-Allow-Origin", "*");
            // response.AddHeader("x-frame-options", "DENY");

            //// enable CORS  
            //response.AddHeader("Strict-Transport-Security", "max-age=31536000");
            //response.AddHeader("Content-Security-Policy", "default-src 'self'");
            //response.AddHeader("X-Content-Type-Options", "nosniff");
            //response.AddHeader("X-Frame-Options", "DENY");
            //response.AddHeader("X-Xss-Protection", "1; mode=block");
            //response.AddHeader("Referrer-Policy", "strict-origin-when-cross-origin");
            //response.Headers.Remove("Server");

            if (context.Request.HttpMethod == "OPTIONS")
            {
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.AddHeader("Access-Control-Allow-Headers", "*");
                response.End();
            }
        }
    }
}
