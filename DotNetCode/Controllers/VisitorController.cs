using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class VisitorController : Controller
    {
        VisitorModel VM = new VisitorModel();
        // GET: Visitor
        public ActionResult Index() 
        {
            return View();
        }

        // POST: Visitor/UploadFileVisitor
        [Route("Visitor/UploadFileVisitor")]
        [HttpPost]
        public ActionResult UploadFileVisitor(FileUploadAPIViewModel model)
        {
            try
            {
                var emp = VM.UploadFileVisitor(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/InviteVisit
        [Route("Visitor/InviteVisit")]
        [HttpPost]
        public ActionResult InviteVisit(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.InviteVisit(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VerifyOTP
        [Route("Visitor/VerifyOTP")]
        [HttpPost]
        public ActionResult VerifyOTP(VisitorManagementViewModel model)
        {
            try
            {
                var Verify = VM.VerifyOTP(model);
                return Json(Verify, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/AcceptInvite
        [Route("Visitor/AcceptInvite")]
        [HttpPost]
        public ActionResult AcceptInvite(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.AcceptInvite(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/GetAllInvite
        [Route("Visitor/GetAllInvite")]
        [HttpPost]
        public ActionResult GetAllInvite(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.GetAllInvite(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/GetAllEmployeeInvite
        [Route("Visitor/GetAllEmployeeInvite")]
        [HttpPost]
        public ActionResult GetAllEmployeeInvite(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.GetAllEmployeeInvite(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/CancelInvite
        [Route("Visitor/CancelInvite")]
        [HttpPost]
        public ActionResult CancelInvite(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.CancelInvite(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/DirectCheckIn
        [Route("Visitor/DirectCheckIn")]
        [HttpPost]
        public ActionResult DirectCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.DirectCheckIn(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/CheckIn
        [Route("Visitor/CheckIn")]
        [HttpPost]
        public ActionResult CheckIn(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.CheckIn(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/CheckOut
        [Route("Visitor/CheckOut")]
        [HttpPost]
        public ActionResult CheckOut(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.CheckOut(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VisitorCheckIn
        [Route("Visitor/VisitorCheckIn")]
        [HttpPost]
        public ActionResult VisitorCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.VisitorCheckIn(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VisitorCheckOut
        [Route("Visitor/VisitorCheckOut")]
        [HttpPost]
        public ActionResult VisitorCheckOut(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.VisitorCheckOut(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VerifyOTPCheckIn
        [Route("Visitor/VerifyOTPCheckIn")]
        [HttpPost]
        public ActionResult VerifyOTPCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.VerifyOTPCheckIn(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VisitFilter
        [Route("Visitor/VisitFilter")]
        [HttpPost]
        public ActionResult VisitFilter(FilterViewModel model)
        {
            try
            {
                var Invite = VM.VisitFilter(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/DDCompany
        [Route("Visitor/DDCompany")]
        [HttpPost]
        public ActionResult DDCompany(DDCompViewModel Deptdd)
        {
            try
            {
                var Comp = VM.DDCompany(Deptdd);
                return Json(Comp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/DDEmployee
        [Route("Visitor/DDEmployee")]
        [HttpPost]
        public ActionResult DDEmployee(DDEmpViewModel Empdd)
        {
            try
            {
                var Emp = VM.DDEmployee(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VisitorDirectCheckIn
        [Route("Visitor/VisitorDirectCheckIn")]
        [HttpPost]
        public ActionResult VisitorDirectCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                var Invite = VM.VisitorDirectCheckIn(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Visitor/VisitExportCSV
        [Route("Visitor/VisitExportCSV")]
        [HttpPost]
        public ActionResult VisitExportCSV(FilterViewModel model)
        {
            try
            {
                var visitors = VM.VisitExport(model);
                // Convert visitor data to CSV
                var csv = new StringBuilder();

                // Add CSV headers
                csv.AppendLine("VisitId,Name,Designation,Company,Purpose,Mail,Mobile,Photo,CompName,Accessories,WhomtoMeet,EmpCode,Date,Time,IdCard,CheckIn,CheckOut");

                // Add data rows
                foreach (var visitor in visitors)
                {
                    string base64Photo = ImageToBase64(visitor.Photo);
                    csv.AppendLine($"{visitor.VisitId},{visitor.Name},{visitor.Designation},{visitor.Company},{visitor.Purpose},{visitor.OMail},{visitor.Mobile},{base64Photo},{visitor.CompName},{visitor.Accessories},{visitor.WName},{visitor.WEmpCode},{visitor.Date},{visitor.Time},{visitor.IdCard},{visitor.CheckIn},{visitor.CheckOut}");
                }

                // Convert CSV string to byte array
                byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());

                // Return the file result
                return File(buffer, "text/csv", "VisitorsData.csv");
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // Helper function to convert image to base64
        private string ImageToBase64(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageArray);
            }
            return string.Empty;
        }
        // POST: Visitor/VisitExportExcel
        [Route("Visitor/VisitExportExcel")]
        [HttpPost]
        public ActionResult VisitExportExcel(FilterViewModel model)
        {
            try
            {
                // Get the visitor data from your service or database
                var visitors = VM.VisitExport(model);

                // Create an Excel package
                using (var package = new ExcelPackage())
                {
                    // Add a worksheet
                    var worksheet = package.Workbook.Worksheets.Add("Visitors");

                    // Set headers
                    // Set headers (including Serial No.)
                    worksheet.Cells[1, 1].Value = "Serial No.";  // Adding Serial No.
                    worksheet.Cells[1, 2].Value = "Name";
                    worksheet.Cells[1, 3].Value = "Designation";
                    worksheet.Cells[1, 4].Value = "Company";
                    worksheet.Cells[1, 5].Value = "Purpose";
                    worksheet.Cells[1, 6].Value = "Mail";
                    worksheet.Cells[1, 7].Value = "Mobile";
                    worksheet.Cells[1, 8].Value = "Photo";
                    worksheet.Cells[1, 9].Value = "CompName";
                    worksheet.Cells[1, 10].Value = "Accessories";
                    worksheet.Cells[1, 11].Value = "WhomtoMeet";
                    worksheet.Cells[1, 12].Value = "EmpCode";
                    worksheet.Cells[1, 13].Value = "Date";
                    worksheet.Cells[1, 14].Value = "Time";
                    worksheet.Cells[1, 15].Value = "IdCard";
                    worksheet.Cells[1, 16].Value = "CheckIn";
                    worksheet.Cells[1, 17].Value = "CheckOut";

                    // Populate the sheet with visitor data
                    for (int i = 0; i < visitors.Count; i++)
                    {
                        var visitor = visitors[i];
                        worksheet.Cells[i + 2, 1].Value = i + 1; // Serial No. (Starts from 1)
                        worksheet.Cells[i + 2, 2].Value = visitor.Name;
                        worksheet.Cells[i + 2, 3].Value = visitor.Designation;
                        worksheet.Cells[i + 2, 4].Value = visitor.Company;
                        worksheet.Cells[i + 2, 5].Value = visitor.Purpose;
                        worksheet.Cells[i + 2, 6].Value = visitor.OMail;
                        worksheet.Cells[i + 2, 7].Value = visitor.Mobile;
                        //worksheet.Cells[i + 2, 8].Value = visitor.Photo;
                        // Check if the file path (visitor.Photo) is valid and file exists
                        if (!string.IsNullOrEmpty(visitor.Photo) && System.IO.File.Exists(visitor.Photo))
                        {
                            ////// Load the image from file
                            ////var image = System.Drawing.Image.FromFile(visitor.Photo);

                            ////// Add the image to the worksheet
                            ////var excelImage = worksheet.Drawings.AddPicture("Photo" + i, FileInfo);

                            ////// Position the image in the appropriate cell (column 8 for "Photo")
                            ////excelImage.SetPosition(i + 1, 0, 7, 0); // Row, Row Offset, Column, Column Offset
                            ////excelImage.SetSize(50, 50); // Adjust the size of the image
                            

                            ///// Add the image directly from the file path
                            var excelImage = worksheet.Drawings.AddPicture("Photo" + i, new FileInfo(visitor.Photo));

                            // Position the image in the appropriate cell (column 8 for "Photo")
                            excelImage.SetPosition(i + 1, 0, 7, 0); // Row, Row Offset, Column, Column Offset
                            excelImage.SetSize(50, 50); // Adjust the size of the image
                        }
                        else
                        {
                            // If no valid image, add placeholder text or leave empty
                            worksheet.Cells[i + 2, 8].Value = "No Image";
                        }
                        worksheet.Cells[i + 2, 9].Value = visitor.CompName;
                        worksheet.Cells[i + 2, 10].Value = visitor.Accessories;
                        worksheet.Cells[i + 2, 11].Value = visitor.WName;
                        worksheet.Cells[i + 2, 12].Value = visitor.WEmpCode;
                        worksheet.Cells[i + 2, 13].Value = visitor.Date;
                        worksheet.Cells[i + 2, 14].Value = visitor.Time;
                        worksheet.Cells[i + 2, 15].Value = visitor.IdCard;
                        worksheet.Cells[i + 2, 16].Value = visitor.CheckIn;
                        worksheet.Cells[i + 2, 17].Value = visitor.CheckOut;
                    }

                    // Set response headers and content type
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    string excelName = $"Visitors-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}