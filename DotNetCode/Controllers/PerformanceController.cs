using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class PerformanceController : Controller
    {
        PerformanceModel PM = new PerformanceModel();
        // GET: Performance
        public ActionResult Index()
        {
            return View();
        }
        // POST: Performance/DDFYear
        [Route("Performance/DDFYear")]
        [HttpPost]
        public ActionResult DDFYear(DDFinancialYear model)
        {
            try
            {
                var Goal = PM.DDFYear(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DDQuater
        [Route("Performance/DDQuater")]
        [HttpPost]
        public ActionResult DDQuater(DDQuater model)
        {
            try
            {
                var Quater = PM.DDQuater(model);
                return Json(Quater, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DDReviewStatus
        [Route("Performance/DDReviewStatus")]
        [HttpPost]
        public ActionResult DDReviewStatus(DDReviewStatus model)
        {
            try
            {
                var RS = PM.DDReviewStatus(model);
                return Json(RS, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetQuaterDetails
        [Route("Performance/GetQuaterDetails")]
        [HttpPost]
        public ActionResult GetQuaterDetails(QuaterMasterViewModel model)
        {
            try
            {
                var Goal = PM.GetQuaterDetails(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
                //return Json(Goal);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetFYearDetails
        [Route("Performance/GetFYearDetails")]
        [HttpPost]
        public ActionResult GetFYearDetails(FyearDetailsViewModel model)
        {
            try
            {
                var Goal = PM.GetFYearDetails(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/SubmitConfigSetup
        [Route("Performance/SubmitConfigSetup")]
        [HttpPost]
        public ActionResult SubmitConfigSetup(ConfigSetupViewmodel model)
        {
            try
            {
                var Config = PM.SubmitConfigSetup(model);
                return Json(Config, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/UpdateConfigSetup
        [Route("Performance/UpdateConfigSetup")]
        [HttpPost]
        public ActionResult UpdateConfigSetup(ConfigSetupViewmodel model)
        {
            try
            {
                var Goal = PM.UpdateConfigSetup(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllConfigSetup
        [Route("Performance/GetAllConfigSetup")]
        [HttpPost]
        public ActionResult GetAllConfigSetup(ConfigSetupViewmodel model)
        {
            try
            {
                var Goal = PM.GetAllConfigSetup(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetEmployeeDetails
        [Route("Performance/GetEmployeeDetails")]
        [HttpPost]
        public ActionResult GetEmployeeDetails(EmployeeMasterViewModel model)
        {
            try
            {
                var Goal = PM.GetEmployeeDetails(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/PerformanceReport
        [Route("Performance/PerformanceReport")]
        [HttpPost]
        public ActionResult PerformanceReport(PerreportViewModel model)
        {
            try
            {
                var PR = PM.PerformanceReport(model);
                return Json(PR, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllGoal
        [Route("Performance/GetAllGoal")]
        [HttpPost]
        public ActionResult GetAllGoal(Per_GoalViewModel model)
        {
            try
            {
                var Goal = PM.GetAllGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllGoalEmployee
        [Route("Performance/GetAllGoalEmployee")]
        [HttpPost]
        public ActionResult GetAllGoalEmployee(Per_GoalViewModel model)
        {
            try
            {
                var Goal = PM.GetAllGoalEmployee(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetGoal
        [Route("Performance/GetGoal")]
        [HttpPost]
        public ActionResult GetGoal(Per_GoalViewModel model)
        {
            try
            {
                var Goal = PM.GetGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/AddAllGoal
        [Route("Performance/AddAllGoal")]
        [HttpPost]
        public ActionResult AddAllGoal(Per_GoalListViewModel model)
        {
            try
            {
                var Goal = PM.AddAllGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/ApproveAllGoal
        [Route("Performance/ApproveAllGoal")]
        [HttpPost]
        public ActionResult ApproveAllGoal(Per_GoalListViewModel model)
        {
            try
            {
                var Goal = PM.ApproveAllGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/AddGoal
        [Route("Performance/AddGoal")]
        [HttpPost]
        public ActionResult AddGoal(Per_GoalViewModel model)
        {
            try
            {
                var Goal = PM.AddGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/UpdateGoal
        [Route("Performance/UpdateGoal")]
        [HttpPost]
        public ActionResult UpdateGoal(Per_GoalViewModel model)
        {
            try
            {
                var Goal = PM.UpdateGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DeleteGoal
        [Route("Performance/DeleteGoal")]
        [HttpPost]
        public ActionResult DeleteGoal(Per_GoalViewModel model)
        {
            try
            {
                var Goal = PM.DeleteGoal(model);
                return Json(Goal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllTask
        [Route("Performance/GetAllTask")]
        [HttpPost]
        public ActionResult GetAllTask(Per_TaskViewModel model)
        {
            try
            {
                var Task = PM.GetAllTask(model);
                return Json(Task, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetTask
        [Route("Performance/GetTask")]
        [HttpPost]
        public ActionResult GetTask(Per_TaskViewModel model)
        {
            try
            {
                var Task = PM.GetTask(model);
                return Json(Task, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Performance/AddTask
        [Route("Performance/AddTask")]
        [HttpPost]
        public ActionResult AddTask(Per_TaskViewModel model)
        {
            try
            {
                var Task = PM.AddTask(model);
                return Json(Task, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/UpdateTask
        [Route("Performance/UpdateTask")]
        [HttpPost]
        public ActionResult UpdateTask(Per_TaskViewModel model)
        {
            try
            {
                var Task = PM.UpdateTask(model);
                return Json(Task, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DeleteTask
        [Route("Performance/DeleteTask")]
        [HttpPost]
        public ActionResult DeleteTask(Per_TaskViewModel model)
        {
            try
            {
                var Task = PM.DeleteTask(model);
                return Json(Task, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllBehaviour
        [Route("Performance/GetAllBehaviour")]
        [HttpPost]
        public ActionResult GetAllBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                var Behaviour = PM.GetAllBehaviour(model);
                return Json(Behaviour, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetBehaviour
        [Route("Performance/GetBehaviour")]
        [HttpPost]
        public ActionResult GetBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                var Behaviour = PM.GetBehaviour(model);
                return Json(Behaviour, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/AddBehaviour
        [Route("Performance/AddBehaviour")]
        [HttpPost]
        public ActionResult AddBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                var Behaviour = PM.AddBehaviour(model);
                return Json(Behaviour, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/UpdateBehaviour
        [Route("Performance/UpdateBehaviour")]
        [HttpPost]
        public ActionResult UpdateBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                var Behaviour = PM.UpdateBehaviour(model);
                return Json(Behaviour, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DeleteBehaviour
        [Route("Performance/DeleteBehaviour")]
        [HttpPost]
        public ActionResult DeleteBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                var Behaviour = PM.DeleteBehaviour(model);
                return Json(Behaviour, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllBehaviourDetail
        [Route("Performance/GetAllBehaviourDetail")]
        [HttpPost]
        public ActionResult GetAllBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                var BehaviourDetail = PM.GetAllBehaviourDetail(model);
                return Json(BehaviourDetail, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetBehaviourDetail
        [Route("Performance/GetBehaviourDetail")]
        [HttpPost]
        public ActionResult GetBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                var BehaviourDetail = PM.GetBehaviourDetail(model);
                return Json(BehaviourDetail, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/AddBehaviourDetail
        [Route("Performance/AddBehaviourDetail")]
        [HttpPost]
        public ActionResult AddBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                var BehaviourDetail = PM.AddBehaviourDetail(model);
                return Json(BehaviourDetail, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/UpdateBehaviourDetail
        [Route("Performance/UpdateBehaviourDetail")]
        [HttpPost]
        public ActionResult UpdateBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                var BehaviourDetail = PM.UpdateBehaviourDetail(model);
                return Json(BehaviourDetail, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DeleteBehaviourDetail
        [Route("Performance/DeleteBehaviourDetail")]
        [HttpPost]
        public ActionResult DeleteBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                var BehaviourDetail = PM.DeleteBehaviourDetail(model);
                return Json(BehaviourDetail, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllSelfDevelopment
        [Route("Performance/GetAllSelfDevelopment")]
        [HttpPost]
        public ActionResult GetAllSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.GetAllSelfDevelopment(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetSelfDevelopment
        [Route("Performance/GetSelfDevelopment")]
        [HttpPost]
        public ActionResult GetSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.GetSelfDevelopment(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/AddSelfDevelopment
        [Route("Performance/AddSelfDevelopment")]
        [HttpPost]
        public ActionResult AddSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.AddSelfDevelopment(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/UpdateSelfDevelopment
        [Route("Performance/UpdateSelfDevelopment")]
        [HttpPost]
        public ActionResult UpdateSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.UpdateSelfDevelopment(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/DeleteSelfDevelopment
        [Route("Performance/DeleteSelfDevelopment")]
        [HttpPost]
        public ActionResult DeleteSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.DeleteSelfDevelopment(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/SaveEmployeeReview
        [Route("Performance/SaveEmployeeReview")]
        [HttpPost]
        public ActionResult SaveEmployeeReview(Per_EmployeeReviewViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.SaveEmployeeReview(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetAllEmployeeReviewList
        [Route("Performance/GetAllEmployeeReviewList")]
        [HttpPost]
        public ActionResult GetAllEmployeeReviewList(ReviewListViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.GetAllEmployeeReviewList(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/GetEmployeeReviewList
        [Route("Performance/GetEmployeeReviewList")]
        [HttpPost]
        public ActionResult GetEmployeeReviewList(ReviewListViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.GetEmployeeReviewList(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/SaveManagerReview
        [Route("Performance/SaveManagerReview")]
        [HttpPost]
        public ActionResult SaveManagerReview(Per_EmployeeReviewViewModel model)
        {
            try
            {
                var SelfDevelopment = PM.SaveManagerReview(model);
                return Json(SelfDevelopment, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Performance/ViewScreenShots
        [Route("Performance/ViewScreenShots")]
        [HttpPost]
        public ActionResult ViewScreenShots(ScreenshotsViewModel model)
        {
            //string basePath = @"http:\\192.168.2.61\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\";

            //string basePath = @"E:\Sundar\Sundar\Application\OfficeConnect_Web\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\";

            //string basePath = @"C:\inetpub\wwwroot\DealerDelight\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\"; //61 system path

            string basePath = @"E:\New Office Connect\Hosting\Production\BackEnd\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\"; // 100.22

            if (string.IsNullOrEmpty(model.EmpCode))
            {
                throw new CustomApiException(HttpStatusCode.NotFound, "Employee code is required.");
            }

            var currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            var empPath = Path.Combine(basePath, currentMonth, model.EmpCode);

            // Normalize the path
            empPath = Path.GetFullPath(empPath);

            // OPTIONAL: Validate path format
            if (empPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, "The constructed path is invalid.");
            }

            // Check directory exists
            if (!Directory.Exists(empPath))
            {
                throw new CustomApiException(HttpStatusCode.NotFound, $"Path not found: {empPath}");
            }

            // Return folder list if date not provided
            if (string.IsNullOrEmpty(model.Date))
            {
                var folders = Directory.GetDirectories(empPath)
                    .Select(folder => new ScreenshotsViewModel
                    {
                        EmpCode = model.EmpCode,
                        Date = Path.GetFileName(folder)
                    })
                    .ToList();

                return Json(folders, JsonRequestBehavior.AllowGet);
            }

            // If both EmpCode and Date are provided — return zip file
            var dateFolderPath = Path.Combine(empPath, model.Date);

            if (!Directory.Exists(dateFolderPath))
            {
                throw new CustomApiException(HttpStatusCode.NotFound, "Date folder not found.");
            }

            try
            {
                MemoryStream memoryStream1 = new MemoryStream();
                var memoryStream = memoryStream1;
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var filePath in Directory.GetFiles(dateFolderPath))
                    {
                        var fileName = Path.GetFileName(filePath);
                        archive.CreateEntryFromFile(filePath, fileName);
                    }
                }

                memoryStream.Position = 0;
                var zipFileName = $"{model.EmpCode}_{model.Date}.zip";

                // Set Content-Disposition to trigger download in Postman or browser
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + zipFileName);

                return File(memoryStream, "application/zip", zipFileName);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}