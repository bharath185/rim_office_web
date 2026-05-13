using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class WFHLoginController : Controller
    {
        LoginModel LM = new LoginModel();
        EmployeeMasterModel EM = new EmployeeMasterModel();

        // POST: WFHLogin/WFHLogin
        [Route("WFHLogin/WFHLogin")]
        [HttpPost]
        public ActionResult WFHLogin(WFHLoginViewModel LoginUser)
        {
            try
            {
                // TODO: Add insert logic here
                if (LoginUser == null || string.IsNullOrEmpty(LoginUser.UserName) || string.IsNullOrEmpty(LoginUser.Password))
                {
                    //var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("No Employee found with ID = {0}", LoginUser)),
                    //    ReasonPhrase = "Invalid input parameters"
                    //};

                    //throw new System.Web.Http.HttpResponseException(response);
                    throw new CustomApiException(HttpStatusCode.NotFound, "Invalid Input Parameters");
                }
                else
                {
                    EmployeeMasterViewModel EM = new EmployeeMasterViewModel();
                    var Emp = LM.WFHCheckLogin(LoginUser);
                    return Json(Emp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        // POST: WFHLogin/WFHLogOut
        [Route("WFHLogin/WFHLogOut")]
        [HttpPost]
        public ActionResult WFHLogOut(WFHLoginViewModel LoginUser)
        {
            try
            {
                // TODO: Add insert logic here
                if (LoginUser == null || string.IsNullOrEmpty(LoginUser.UserName))
                {
                    //var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("No Employee found with ID = {0}", LoginUser)),
                    //    ReasonPhrase = "Invalid input parameters"
                    //};

                    //throw new System.Web.Http.HttpResponseException(response);

                    throw new CustomApiException(HttpStatusCode.NotFound, "Not Found");
                }
                else
                {
                    EmployeeMasterViewModel EM = new EmployeeMasterViewModel();
                    var Emp = LM.WFHCheckLogOut(LoginUser);
                    return Json(Emp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: WFHLogin/GetAllWFHDetails
        [Route("WFHLogin/GetAllWFHDetails")]
        [HttpPost]
        public ActionResult GetAllWFHDetails(WFHLoginlogViewModel model)
        {
            try
            {
                var Invite = EM.GetAllWFHDetails(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: WFHLogin/GetAllWFHFilterDetails
        [Route("WFHLogin/GetAllWFHFilterDetails")]
        [HttpPost]
        public ActionResult GetAllWFHFilterDetails(WFHLoginlogFilterViewModel model)
        {
            try
            {
                var Invite = EM.GetAllWFHFilterDetails(model);
                //return Json(Invite, JsonRequestBehavior.AllowGet);
                return new JsonResult
                {
                    Data = Invite,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: WFHLogin/ViewScreenShots
        [Route("WFHLogin/ViewScreenShots")]
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
        // POST: WFHLogin/SaveWFHAnalysis
        [Route("WFHLogin/SaveWFHAnalysis")]
        [HttpPost]
        public ActionResult SaveWFHAnalysis(WFHLoginlogViewModel model)
        {
            try
            {
                var emp = EM.SaveWFHAnalysis(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: WFHLogin/GetAllWFHAnalysis
        [Route("WFHLogin/GetAllWFHAnalysis")]
        [HttpPost]
        public ActionResult GetAllWFHAnalysis(WFHLoginlogViewModel model)
        {
            try
            {
                var Invite = EM.GetAllWFHAnalysis(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: WFHLogin/WFHEmpList
        [Route("WFHLogin/WFHEmpList")]
        [HttpPost]
        public ActionResult WFHEmpList(ScreenshotsViewModel model)
        {
            //string basePath = @"http:\\192.168.2.61\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\";

            //string basePath = @"E:\Sundar\Sundar\Application\OfficeConnect_Web\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\";

            //string basePath = @"C:\inetpub\wwwroot\DealerDelight\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\"; //61 system path

            string basePath = @"E:\New Office Connect\Hosting\Production\BackEnd\OfficeConnect_Web\Uploads\Images\WorkFromHome\ScreenShot\"; // 100.22

            //if (string.IsNullOrEmpty(model.EmpCode))
            //{
            //    throw new CustomApiException(HttpStatusCode.NotFound, "Employee code is required.");
            //}

            var currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            var empPath = Path.Combine(basePath, currentMonth);

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
            var folders = Directory.GetDirectories(empPath)
                    .Select(folder => new ScreenshotsViewModel
                    {
                        EmpCode = Path.GetFileName(folder),
                    })
                    .ToList();

            return Json(folders, JsonRequestBehavior.AllowGet);
        }
    }
}