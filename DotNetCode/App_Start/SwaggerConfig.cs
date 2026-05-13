using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OfficeConnect_Web.App_Start
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            //GlobalConfiguration.Configuration.EnableSwagger(c => c.SingleApiVersion("v1", "ErrorHandlingWebAPI")).EnableSwaggerUi();
        }
    }
}



////using Swashbuckle.Application;

////namespace OfficeConnect_Web.App_Start
////{
////    public class SwaggerConfig
////    {
////        public static void Register()
////        {
////            GlobalConfiguration.Configuration
////                .EnableSwagger(c =>
////                {
////                    c.SingleApiVersion("v1", "OfficeConnect API");
////                })
////                .EnableSwaggerUi();
////        }
////    }
////}