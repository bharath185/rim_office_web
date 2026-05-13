using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static OfficeConnect_Web.Models.DashboardModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class DashboardController : Controller
    {
        DashboardModel DM = new DashboardModel();

        // POST: Dashboard/GetEmployeeEvents
        [Route("Dashboard/GetEmployeeEvents")]
        [HttpPost]
        public ActionResult GetEmployeeEvents(DashboardViewModel model)
        {
            try
            {
                var leave = DM.GetEmployeeEvents(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        [Route("Dashboard/GetAllHRcount")]
        [HttpPost]
        public ActionResult GetAllHRcount(VisitorManagementViewModel model)
        {
            try
            {
                var data = DM.GetAllHRCount(model);
                return Content(
            Newtonsoft.Json.JsonConvert.SerializeObject(
                data,
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                }
            ),
            "application/json"
        );
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message });
            }
        }
    }
}