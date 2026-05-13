using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static OfficeConnect_Web.Models.EmployeeMasterModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class EmployeeController : Controller
    {
        LoginModel LM = new LoginModel();
        EmployeeMasterModel EM = new EmployeeMasterModel();


        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login/Create
        //[Route("Employee/GetEmployee")]
        //[HttpPost]
        //public ActionResult GetEmployee(EmployeeMasterViewModel Emp)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        if (Emp == null || string.IsNullOrEmpty(Emp.UserName) || string.IsNullOrEmpty(Emp.Password))
        //        {
        //            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //            {
        //                Content = new StringContent(string.Format("No Employee found with ID = {0}", Emp)),
        //                ReasonPhrase = "Invalid input parameters"
        //            };

        //            throw new System.Web.Http.HttpResponseException(response);
        //        }
        //        else
        //        {
        //            var Employess = EM.GetEmployee(Emp);
        //            return Json(Emp, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new WebFaultException(ex);
        //    }
        //}

        // POST: Employee/DDCompany
        [Route("Employee/DDCompany")]
        [HttpPost]
        public ActionResult DDCompany(DDCompanyViewModel model)
        {
            try
            {
                var Comp = EM.GetDDCompany(model);
                return Json(Comp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDLegalEntity
        [Route("Employee/DDLegalEntity")]
        [HttpPost]
        public ActionResult DDLegalEntity(DDLegalEntityViewModel model)
        {
            try
            {
                var Ent = EM.GetDDLegalEntity(model);
                return Json(Ent, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDAuthorisedEntity
        [Route("Employee/DDAuthorisedEntity")]
        [HttpPost]
        public ActionResult DDAuthorisedEntity(DDAuthorisedEntityViewModel model)
        {
            try
            {
                var Ent = EM.DDAuthorisedEntity(model);
                return Json(Ent, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDBusinessUnit
        [Route("Employee/DDBusinessUnit")]
        [HttpPost]
        public ActionResult DDBusinessUnit(DDBusinessUnitViewModel model)
        {
            try
            {
                var bus = EM.GetDDBusinessUnit(model);
                return Json(bus, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDLocation
        [Route("Employee/DDLocation")]
        [HttpPost]
        public ActionResult DDLocation(DDLocationViewModel model)
        {
            try
            {
                var loc = EM.GetDDLocation(model);
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/NewDDCompany
        [Route("Employee/NewDDCompany")]
        [HttpPost]
        public ActionResult NewDDCompany(NewDDCompanyViewModel model)
        {
            try
            {
                var Comp = EM.GetNewDDCompany(model);
                return Json(Comp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/NewDDLegalEntity
        [Route("Employee/NewDDLegalEntity")]
        [HttpPost]
        public ActionResult NewDDLegalEntity(NewDDLegalEntityViewModel model)
        {
            try
            {
                var Ent = EM.GetNewDDLegalEntity(model);
                return Json(Ent, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/NewDDBusinessUnit
        [Route("Employee/NewDDBusinessUnit")]
        [HttpPost]
        public ActionResult NewDDBusinessUnit(NewDDBusinessUnitViewModel model)
        {
            try
            {
                var bus = EM.GetNewDDBusinessUnit(model);
                return Json(bus, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/NewDDLocation
        [Route("Employee/NewDDLocation")]
        [HttpPost]
        public ActionResult NewDDLocation(NewDDLocationViewModel model)
        {
            try
            {
                var loc = EM.GetNewDDLocation(model);
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDSalutation
        [Route("Employee/DDSalutation")]
        [HttpPost]
        public ActionResult DDSalutation(DDSaluationViewModel model)
        {
            try
            {
                var sal = EM.DDSalutation(model);
                return Json(sal, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDGender
        [Route("Employee/DDGender")]
        [HttpPost]
        public ActionResult DDGender(DDGenderViewModel model)
        {
            try
            {
                var gen = EM.DDGender(model);
                return Json(gen, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDEmpType
        [Route("Employee/DDEmpType")]
        [HttpPost]
        public ActionResult DDEmpType(DDEmpTypeViewModel model)
        {
            try
            {
                var Etyp = EM.DDEmpType(model);
                return Json(Etyp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDApprover
        [Route("Employee/DDApprover")]
        [HttpPost]
        public ActionResult DDApprover(DDApproverViewModel model)
        {
            try
            {
                var Etyp = EM.DDApprover(model);
                return Json(Etyp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/FetchEmployee
        [Route("Employee/FetchEmployee")]
        [HttpPost]
        public ActionResult FetchEmployee(FetchEmployeeViewModel model)
        {
            try
            {
                var emp = EM.FetchEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/PCGetAllEmployee
        [Route("Employee/PCGetAllEmployee")]
        [HttpPost]
        public ActionResult PCGetAllEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.PCGetAllEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/PCAddAllEmployee
        [Route("Employee/PCAddAllEmployee")]
        [HttpPost]
        public ActionResult PCAddAllEmployee(List<ProjectConnectUserViewModel> model)
        {
            try
            {
                var emp = EM.PCAddAllEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllEmployee
        [Route("Employee/GetAllEmployee")]
        [HttpPost]
        public ActionResult GetAllEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.GetAllEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetEmployee
        [Route("Employee/GetEmployee")]
        [HttpPost]
        public ActionResult GetEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.GetEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddEmployee
        [Route("Employee/AddEmployee")]
        [HttpPost]
        public ActionResult AddEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.AddEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateEmployee
        [Route("Employee/UpdateEmployee")]
        [HttpPost]
        public ActionResult UpdateEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.UpdateEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteEmployee
        [Route("Employee/DeleteEmployee")]
        [HttpPost]
        public ActionResult DeleteEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.DeleteEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ActiveEmployee
        [Route("Employee/ActiveEmployee")]
        [HttpPost]
        public ActionResult ActiveEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.ActiveEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeActiveEmployee
        [Route("Employee/DeActiveEmployee")]
        [HttpPost]
        public ActionResult DeActiveEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.DeActiveEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/RelievedEmployee
        [Route("Employee/RelievedEmployee")]
        [HttpPost]
        public ActionResult RelievedEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                var emp = EM.RelievedEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllEmployeeContactInformation
        [Route("Employee/GetAllEmployeeContactInformation")]
        [HttpPost]
        public ActionResult GetAllEmployeeContactInformation(EmployeeDetailViewModel model)
        {
            try
            {
                var emp = EM.GetAllEmployeeDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetEmployeeContactInformation
        [Route("Employee/GetEmployeeContactInformation")]
        [HttpPost]
        public ActionResult GetEmployeeContactInformation(EmployeeDetailViewModel model)
        {
            try
            {
                var emp = EM.GetEmployeeDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddEmployeeContactInformation
        [Route("Employee/AddEmployeeContactInformation")]
        [HttpPost]
        public ActionResult AddEmployeeContactInformation(EmployeeDetailViewModel model)
        {
            try
            {
                var emp = EM.AddEmployeeDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateEmployeeContactInformation
        [Route("Employee/UpdateEmployeeContactInformation")]
        [HttpPost]
        public ActionResult UpdateEmployeeContactInformation(EmployeeDetailViewModel model)
        {
            try
            {
                var emp = EM.UpdateEmployeeDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteEmployeeContactInformation
        [Route("Employee/DeleteEmployeeContactInformation")]
        [HttpPost]
        public ActionResult DeleteEmployeeContactInformation(EmployeeDetailViewModel model)
        {
            try
            {
                var emp = EM.DeleteEmployeeDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //// POST: Employee/UploadImage
        //[Route("Employee/UploadImage")]
        //[HttpPost]
        //public ActionResult UploadImage(HttpPostedFileBase file)
        //{
        //    // extract only the fielname            
        //    var imageName = Path.GetFileName(file.FileName);
        //    var imgsrc = Path.Combine(Server.MapPath("~/images/"), imageName);
        //    string filepathToSave = "images/" + imageName;
        //    file.SaveAs(imgsrc);
        //    ViewBag.ImagPath = filepathToSave;
        //    return View();
        //}
        // POST: Employee/UploadImage
        [Route("Employee/UploadImage")]
        [HttpPost]
        public ActionResult UploadImage(FileUploadAPIViewModel model)
        {
        
            try
            {
                var emp = EM.UploadImage(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/WFHUploadImage
        [Route("Employee/WFHUploadImage")]
        [HttpPost]
        public ActionResult WFHUploadImage(WFHFileUploadAPIViewModel model)
        {
            try
            {
                var emp = EM.WFHUploadImage(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UploadFileEducation
        [Route("Employee/UploadFileEducation")]
        [HttpPost]
        public ActionResult UploadFileEducation(FileUploadAPIViewModel model)
        {
            try
            {
                var emp = EM.UploadFileEducation(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UploadFileGovt
        [Route("Employee/UploadFileGovt")]
        [HttpPost]
        public ActionResult UploadFileGovt(FileUploadAPIViewModel model)
        {
            try
            {
                var emp = EM.UploadFileGovt(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDEducationDoc
        [Route("Employee/DDEducationDoc")]
        [HttpPost]
        public ActionResult DDEducationDoc(DDDocViewModel model)
        {
            try
            {
                var Dept = EM.GetDDEducationDoc(model);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDGovtDoc
        [Route("Employee/DDGovtDoc")]
        [HttpPost]
        public ActionResult DDGovtDoc(DDDocViewModel model)
        {
            try
            {
                var Dept = EM.GetDDGovtDoc(model);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllEducationDoc
        [Route("Employee/GetAllEducationDoc")]
        [HttpPost]
        public ActionResult GetAllEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                var emp = EM.GetAllEducationDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetEducationDoc
        [Route("Employee/GetEducationDoc")]
        [HttpPost]
        public ActionResult GetEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                var emp = EM.GetEducationDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddEducationDoc
        [Route("Employee/AddEducationDoc")]
        [HttpPost]
        public ActionResult AddEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                var emp = EM.AddEducationDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateEducationDoc
        [Route("Employee/UpdateEducationDoc")]
        [HttpPost]
        public ActionResult UpdateEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                var emp = EM.UpdateEducationDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteEducationDoc
        [Route("Employee/DeleteEducationDoc")]
        [HttpPost]
        public ActionResult DeleteEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                var emp = EM.DeleteEducationDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllGovtDoc
        [Route("Employee/GetAllGovtDoc")]
        [HttpPost]
        public ActionResult GetAllGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                var emp = EM.GetAllGovtDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetGovtDoc
        [Route("Employee/GetGovtDoc")]
        [HttpPost]
        public ActionResult GetGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                var emp = EM.GetGovtDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddGovtDoc
        [Route("Employee/AddGovtDoc")]
        [HttpPost]
        public ActionResult AddGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                var emp = EM.AddGovtDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateGovtDoc
        [Route("Employee/UpdateGovtDoc")]
        [HttpPost]
        public ActionResult UpdateGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                var emp = EM.UpdateGovtDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteGovtDoc
        [Route("Employee/DeleteGovtDoc")]
        [HttpPost]
        public ActionResult DeleteGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                var emp = EM.DeleteGovtDoc(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllEmpAccDetails
        [Route("Employee/GetAllEmpAccDetails")]
        [HttpPost]
        public ActionResult GetAllEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                var emp = EM.GetAllEmpAccDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetEmpAccDetails
        [Route("Employee/GetEmpAccDetails")]
        [HttpPost]
        public ActionResult GetEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                var emp = EM.GetEmpAccDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddEmpAccDetails
        [Route("Employee/AddEmpAccDetails")]
        [HttpPost]
        public ActionResult AddEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                var emp = EM.AddEmpAccDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateEmpAccDetails
        [Route("Employee/UpdateEmpAccDetails")]
        [HttpPost]
        public ActionResult UpdateEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                var emp = EM.UpdateEmpAccDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteEmpAccDetails
        [Route("Employee/DeleteEmpAccDetails")]
        [HttpPost]
        public ActionResult DeleteEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                var emp = EM.DeleteEmpAccDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UploadFileCareer
        [Route("Employee/UploadFileCareer")]
        [HttpPost]
        public ActionResult UploadFileCareer(FileUploadAPIViewModel model)
        {
            try
            {
                var emp = EM.UploadFileCareer(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllEmpCareerDetails
        [Route("Employee/GetAllEmpCareerDetails")]
        [HttpPost]
        public ActionResult GetAllEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                var emp = EM.GetAllEmpCareerDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetEmpCareerDetails
        [Route("Employee/GetEmpCareerDetails")]
        [HttpPost]
        public ActionResult GetEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                var emp = EM.GetEmpCareerDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddEmpCareerDetails
        [Route("Employee/AddEmpCareerDetails")]
        [HttpPost]
        public ActionResult AddEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                var emp = EM.AddEmpCareerDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateEmpCareerDetails
        [Route("Employee/UpdateEmpCareerDetails")]
        [HttpPost]
        public ActionResult UpdateEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                var emp = EM.UpdateEmpCareerDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteEmpCareerDetails
        [Route("Employee/DeleteEmpCareerDetails")]
        [HttpPost]
        public ActionResult DeleteEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                var emp = EM.DeleteEmpCareerDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllWorkType
        [Route("Employee/GetAllWorkType")]
        [HttpPost]
        public ActionResult GetAllWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.GetAllWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetWorkType
        [Route("Employee/GetWorkType")]
        [HttpPost]
        public ActionResult GetWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.GetWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddWorkType
        [Route("Employee/AddWorkType")]
        [HttpPost]
        public ActionResult AddWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.AddWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/UpdateWorkType
        [Route("Employee/UpdateWorkType")]
        [HttpPost]
        public ActionResult UpdateWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.UpdateWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DeleteWorkType
        [Route("Employee/DeleteWorkType")]
        [HttpPost]
        public ActionResult DeleteWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.DeleteWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllApproverWorkType
        [Route("Employee/GetAllApproverWorkType")]
        [HttpPost]
        public ActionResult GetAllApproverWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.GetAllApproverWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDEmployeeApprover
        [Route("Employee/DDEmployeeApprover")]
        [HttpPost]
        public ActionResult DDEmployeeApprover(DDEmployeeViewModel Empdd)
        {
            try
            {
                var Emp = EM.DDEmployeeApprover(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllWorkTypeFilter
        [Route("Employee/GetAllWorkTypeFilter")]
        [HttpPost]
        public ActionResult GetAllWorkTypeFilter(WorkTypeFilterViewModel model)
        {
            try
            {
                var Invite = EM.GetAllWorkTypeFilter(model);
                return Json(Invite, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ApproveWorkType
        [Route("Employee/ApproveWorkType")]
        [HttpPost]
        public ActionResult ApproveWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.ApproveWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/RejectWorkType
        [Route("Employee/RejectWorkType")]
        [HttpPost]
        public ActionResult RejectWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                var emp = EM.RejectWorkType(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/EmployeeAttendance
        [Route("Employee/EmployeeAttendance")]
        [HttpPost]
        public JsonResult EmployeeAttendance(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.EmployeeAttendance(model);
                return new LargeJsonResult { Data = emp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (CustomApiException ex)
            {
                return new LargeJsonResult { Data = new { StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        //POST: Employee/AttendanceFilter
        [Route("Employee/AttendanceFilter")]
        [HttpPost]
        public JsonResult AttendanceFilter(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.AttendanceFilter(model);
                return new LargeJsonResult { Data = emp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (CustomApiException ex)
            {
                return new LargeJsonResult { Data = new { StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        //POST: Employee/ReportingEmployeeAttendance
        [Route("Employee/ReportingEmployeeAttendance")]
        [HttpPost]
        public JsonResult ReportingEmployeeAttendance(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.ReportingEmployeeAttendance(model);
                return new LargeJsonResult { Data = emp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (CustomApiException ex)
            {
                return new LargeJsonResult { Data = new { StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        //POST: Employee/EachEmployeeAttendance
        [Route("Employee/EachEmployeeAttendance")]
        [HttpPost]
        public JsonResult EachEmployeeAttendance(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.EachEmployeeAttendance(model);
                return new LargeJsonResult { Data = emp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (CustomApiException ex)
            {
                return new LargeJsonResult { Data = new { StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        //////POST: Employee/ReportingEmployeeAttendance
        ////[Route("Employee/ReportingEmployeeAttendance")]
        ////[HttpPost]
        ////public JsonResult ReportingEmployeeAttendance(AttendanceFilterViewModel model)
        ////{
        ////    try
        ////    {
        ////        var emp = EM.ReportingEmployeeAttendance(model);
        ////        return new LargeJsonResult { Data = emp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        return new LargeJsonResult { Data = new { StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        ////    }
        ////}

        // POST: Employee/OnSiteLogin
        [Route("Employee/OnSiteLogin")]
        [HttpPost]
        public JsonResult OnSiteLogin(LoginLogViewModel model)
        {
            try
            {
                var emp = EM.OnSiteLogin(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        // POST: Employee/OnSiteLogout
        [Route("Employee/OnSiteLogout")]
        [HttpPost]
        public JsonResult OnSiteLogout(LoginLogViewModel model)
        {
            try
            {
                var emp = EM.OnSiteLogout(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        // POST: Employee/AddOnSiteData
        [Route("Employee/AddOnSiteData")]
        [HttpPost]
        public JsonResult AddOnSiteData(OnSiteDataViewModel model)
        {
            try
            {
                var emp = EM.AddOnSiteData(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllLoginLogs
        [Route("Employee/GetAllLoginLogs")]
        [HttpPost]
        public JsonResult GetAllLoginLogs(LoginlogViewModel model)
        {
            try
            {
                var emp = EM.GetAllLoginLogs(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetLoginLogs
        [Route("Employee/GetLoginLogs")]
        [HttpPost]
        public JsonResult GetLoginLogs(LoginlogViewModel model)
        {
            try
            {
                var emp = EM.GetLoginLogs(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetOnSiteData
        [Route("Employee/GetOnSiteData")]
        [HttpPost]
        public JsonResult GetOnSiteData(OnSiteDataViewModel model)
        {
            try
            {
                var emp = EM.GetOnSiteData(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/SelectEmployee
        [Route("Employee/SelectEmployee")]
        [HttpPost]
        public JsonResult SelectEmployee(SelectEmployeeViewModel model)
        {
            try
            {
                var emp = EM.SelectEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        // POST: Employee/GetTotalEmployeeCount
        [Route("Employee/GetTotalEmployeeCount")]
        [HttpPost]
        public JsonResult GetTotalEmployeeCount(TotalEmployeeViewModel model)
        {
            try
            {
                var emp = EM.GetTotalEmployeeCount(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }


        // POST: Employee/GetAttendanceSource
        [Route("Employee/GetAttendanceSource")]
        [HttpPost]
        public JsonResult GetAttendanceSource(AttendanceSourceViewModel model)
        {
            try
            {
                var emp = EM.GetAttendanceSource(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //// POST: Employee/GetOnTimeCheckInForAllEmployees
        //[Route("Employee/GetOnTimeCheckInForAllEmployees")]
        //[HttpPost]
        //public JsonResult GetOnTimeCheckInForAllEmployees(OnTimeCheckInViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.GetOnTimeCheckInForAllEmployees(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}
        // POST: Employee/DDGetLocation
        [Route("Employee/DDGetLocation")]
        [HttpPost]
        public JsonResult DDGetLocation(ddLocationViewModel model)
        {
            try
            {
                var emp = EM.DDGetLocation(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDselectEmployee
        [Route("Employee/DDselectEmployee")]
        [HttpPost]
        public JsonResult DDselectEmployee(DDSelectEmpViewModel model)
        {
            try
            {
                var emp = EM.DDselectEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetConsolidatedAttendanceData
        [Route("Employee/GetConsolidatedAttendanceData")]
        [HttpPost]
        public JsonResult GetConsolidatedAttendanceData(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.GetConsolidatedAttendanceData(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        ////POST: Employee/LogActivity
        //[Route("Employee/LogActivity")]
        //[HttpPost]
        //public JsonResult LogActivity(LogActivityViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.LogActivity(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}
        //// POST: Employee/GetOnTimeCheckInForEmployee
        //[Route("Employee/GetOnTimeCheckInForEmployee")]
        //[HttpPost]
        //public JsonResult GetOnTimeCheckInForEmployee(OnTimeCheckInViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.GetOnTimeCheckInForAllEmployees(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}

        //POST: Employee/DashboardEmployee
        [Route("Employee/DashboardEmployee")]
        [HttpPost]
        public JsonResult DashboardEmployee(SelectEmployeeViewModel model)
        {
            try
            {
                var emp = EM.DashboardEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/DashboardDetails
        [Route("Employee/DashboardDetails")]
        [HttpPost]
        public JsonResult DashboardDetails(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.DashboardDetails(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/CreateShift
        [Route("Employee/CreateShift")]
        [HttpPost]
        public JsonResult CreateShift(ShiftViewModel model)
        {
            try
            {
                var emp = EM.CreateShift(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/CreateCompanySetting
        [Route("Employee/CreateCompanySetting")]
        [HttpPost]
        public JsonResult CreateCompanySetting(CompanySettingViewModel model)
        {
            try
            {
                var emp = EM.CreateCompanySetting(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }


        //POST: Employee/CheckHalfDayLoss
        [Route("Employee/CheckHalfDayLoss")]
        [HttpPost]
        public JsonResult CheckHalfDayLoss(WorkHoursViewModel model)
        {
            try
            {
                DateTime date = !string.IsNullOrEmpty(model.Date) ? DateTime.Parse(model.Date) : DateTime.Today;

                var emp = EM.CheckHalfDayLoss(model, date);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }


        //POST: Employee/GetWorkHours
        [Route("Employee/GetWorkHours")]
        [HttpPost]
        public JsonResult GetWorkHours(WorkHoursViewModel model)
        {
            try
            {
                var emp = EM.GetWorkHours(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/GetAllPages
        [Route("Employee/GetAllPages")]
        [HttpPost]
        public JsonResult GetAllPages(PageAccessViewModel model)
        {
            try
            {
                var emp = EM.GetAllPages(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/SubmitAccessControls
        [Route("Employee/SubmitAccessControls")]
        [HttpPost]
        public JsonResult SubmitAccessControls(List<AccessViewModel> modelList)
        {
            try
            {
                var empList = EM.SubmitAccessControls(modelList);
                return Json(empList, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/GetPageById
        [Route("Employee/GetPageById")]
        [HttpPost]
        public JsonResult GetPageById(int pageModuleId)
        {
            try
            {
                var empList = EM.GetPageById(pageModuleId);
                return Json(empList, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/UpdatePageModule
        [Route("Employee/UpdatePageModules")]
        [HttpPost]
        public JsonResult UpdatePageModules(List<AccessViewModel> accessList)
        {
            try
            {
                var result = EM.UpdatePageModules(accessList);
                return Json(new { message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/DeletePageModule
        [Route("Employee/DeletePageModules")]
        [HttpPost]
        public JsonResult DeletePageModules(List<AccessViewModel> accessList)
        {
            try
            {
                var result = EM.DeletePageModules(accessList);
                return Json(new { message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        ////POST: Employee/CreateHoliday
        //[Route("Employee/CreateHoliday")]
        //[HttpPost]
        //public JsonResult CreateHoliday(HolidayViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.CreateHoliday(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}

        //POST: Employee/UpdateHoliday
        [Route("Employee/UpdateHoliday")]
        [HttpPost]
        public JsonResult UpdateHoliday(HolidayViewModel model)
        {
            try
            {
                var emp = EM.UpdateHoliday(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/DeleteHoliday
        [Route("Employee/DeleteHoliday")]
        [HttpPost]
        public JsonResult DeleteHoliday(HolidayViewModel model)
        {
            try
            {
                var emp = EM.DeleteHoliday(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/GetAllHolidays
        [Route("Employee/GetAllHolidays")]
        [HttpPost]
        public JsonResult GetAllHolidays(HolidayViewModel model)
        {
            try
            {
                var emp = EM.GetAllHolidays(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/GetEmpHolidays
        [Route("Employee/GetEmpHolidays")]
        [HttpPost]
        public JsonResult GetEmpHolidays(EmpHolidayListViewModel model)
        {
            try
            {
                var emp = EM.GetEmpHolidays(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        ////POST: Employee/GetHolidayById
        //[Route("Employee/GetHolidayById")]
        //[HttpPost]
        //public JsonResult GetHolidayById(HolidayViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.GetHolidayById(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}

        ////POST: Employee/CreateWeekHoliday
        //[Route("Employee/CreateWeekHoliday")]
        //[HttpPost]
        //public JsonResult CreateWeekHoliday(WeekHolidayViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.CreateWeekHoliday(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}

        //POST: Employee/UpdateWeekHoliday
        [Route("Employee/UpdateWeekHoliday")]
        [HttpPost]
        public JsonResult UpdateWeekHoliday(WeekHolidayViewModel model)
        {
            try
            {
                var emp = EM.UpdateWeekHoliday(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/DeleteWeekHoliday
        [Route("Employee/DeleteWeekHoliday")]
        [HttpPost]
        public JsonResult DeleteWeekHoliday(WeekHolidayViewModel model)
        {
            try
            {
                var emp = EM.DeleteWeekHoliday(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/GetAllWeekHolidays
        [Route("Employee/GetAllWeekHolidays")]
        [HttpPost]
        public JsonResult GetAllWeekHolidays(WeekHolidayViewModel model)
        {
            try
            {
                var emp = EM.GetAllWeekHolidays(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/GetWeekHolidayById
        //[Route("Employee/GetWeekHolidayById")]
        //[HttpPost]
        //public JsonResult GetWeekHolidayById(WeekHolidayViewModel model)
        //{
        //    try
        //    {
        //        var emp = EM.GetWeekHolidayById(model);
        //        return Json(emp, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        //    }
        //}


        //POST: Employee/GetAllFinanceMaster
        [Route("Employee/GetAllFinanceMaster")]
        [HttpPost]
        public JsonResult GetAllFinanceMaster(FinanceMasterViewModel model)
        {
            try
            {
                var emp = EM.GetAllFinanceMaster(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        //POST: Employee/AddHoliday
        [Route("Employee/AddHoliday")]
        [HttpPost]
        public JsonResult AddHoliday(HolidayViewModel model)
        {
            try
            {
                var emp = EM.AddHoliday(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/UploadAttendance
        [Route("Employee/UploadAttendance")]
        [HttpPost]
        public ActionResult UploadAttendance(AttendanceUploadModel model)
        {
            DB_Offc_ConEntities DB = new DB_Offc_ConEntities();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;  // Add this line - License

            if (model.File == null || model.File.ContentLength == 0)
                return Json(new { success = false, message = "Please upload a file" });

            // Only allow .xlsx files
            if (!Path.GetExtension(model.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return Json(new { success = false, message = "Only Excel (.xlsx) files allowed" });

            int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
            int? empId = (model.EmpId != 0) ? model.EmpId : 0;

            // Use provided file name or original file name
            string fileName = string.IsNullOrEmpty(model.FileName)
                                ? model.File.FileName
                                : model.FileName;

            // Ensure the file name is safe
            fileName = Path.GetFileName(fileName);

            // Define server folder path (make sure this folder exists in your project or server)
            string folderPath = Server.MapPath("~/Uploads/Attendance/Manual/");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Combine folder path + file name
            string fullPath = Path.Combine(folderPath, fileName);

            // Save the file to server
            model.File.SaveAs(fullPath);

            // Now you can read the Excel from the saved file path
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                var sheet = package.Workbook.Worksheets[0];
                int rowCount = sheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // skip header
                {
                    var empCode = sheet.Cells[row, 1].Text.Trim();
                    var date = sheet.Cells[row, 2].Text.Trim();
                    var workedHrs = sheet.Cells[row, 3].Text.Trim();
                    var status = sheet.Cells[row, 4].Text.Trim();

                    if (string.IsNullOrEmpty(empCode)) continue;

                    ////// Add to TempManualAttendance
                    DB.TempManualAttendances.Add(new TempManualAttendance
                    {
                        EmpCode = empCode,
                        Date = date,
                        Time = workedHrs,
                        Status = status
                    });
                }

                DB.SaveChanges();
            }

            // Process validation + insert
            var result = EM.UploadAttendance(loginId);

            return Json(new
            {
                success = true,
                result.TotalRecords,
                result.InsertedRecords,
                result.FailedRecords,
                result.Exceptions
            });

            ////return Json(new { success = true, message = "File uploaded and data saved successfully", filePath = fullPath });
        }
        //POST: Employee/UploadSingleAttendance
        [Route("Employee/UploadSingleAttendance")]
        [HttpPost]
        public ActionResult UploadSingleAttendance(UploadAttendanceSingleViewModel model)
        {
            try
            {
                var emp = EM.UploadSingleAttendance(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/UploadMultiAttendance
        [Route("Employee/UploadMultiAttendance")]
        [HttpPost]
        public ActionResult UploadMultiAttendance(List<UploadAttendanceSingleViewModel> model)
        {
            try
            {
                var emp = EM.UploadMultiAttendance(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/GetAllManualAttendance
        [Route("Employee/GetAllManualAttendance")]
        [HttpPost]
        public JsonResult GetAllManualAttendance(ManualAttendanceViewModel model)
        {
            try
            {
                var emp = EM.GetAllManualAttendance(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDEmpList
        [Route("Employee/DDEmpList")]
        [HttpPost]
        public ActionResult DDEmpList(DDEmpListViewModel Empdd)
        {
            try
            {
                var Emp = EM.DDEmpList(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/SPAttendance
        [Route("Employee/SPAttendance")]
        [HttpPost]
        public ActionResult SPAttendance(SPAttendanceViewModel model)
        {
            try
            {
                var Emp = EM.SPAttendance(model);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        //POST: Employee/AttendanceDeptReport
        [Route("Employee/AttendanceDeptReport")]
        [HttpPost]
        public JsonResult AttendanceDeptReport(AttendanceFilterViewModel model)
        {
            try
            {
                var emp = EM.AttendanceDeptReport(model);
                return new LargeJsonResult { Data = emp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (CustomApiException ex)
            {
                return new LargeJsonResult { Data = new { StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        // POST: Employee/ContractAttendanceChecking
        [Route("Employee/ContractAttendanceChecking")]
        [HttpPost]
        public ActionResult ContractAttendanceChecking(ContractViewModel Empdd)
        {
            try
            {
                var Emp = EM.ContractAttendanceChecking(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ContractAttendanceManager
        [Route("Employee/ContractAttendanceManager")]
        [HttpPost]
        public ActionResult ContractAttendanceManager(ContractViewModel Empdd)
        {
            try
            {
                var Emp = EM.ContractAttendanceManager(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ERPContractAttendanceDetails
        [Route("Employee/ERPContractAttendanceDetails")]
        [HttpPost]
        public ActionResult ERPContractAttendanceDetails(ContractViewModel Empdd)
        {
            try
            {
                var Emp = EM.ERPContractAttendanceManager(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ERPProjectmappingDetails
        [Route("Employee/ERPProjectmappingDetails")]
        [HttpPost]
        public ActionResult ERPProjectmappingDetails(ContractViewModel Empdd)
        {
            try
            {
                var Emp = EM.ERPProjectmappingDetails(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddContractAttendance
        [Route("Employee/AddContractAttendance")]
        [HttpPost]
        public ActionResult AddContractAttendance(ContractAttendanceViewModel Empdd)
        {
            try
            {
                var Emp = EM.AddContractAttendance(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/LogoutbyManager
        [Route("Employee/LogoutbyManager")]
        [HttpPost]
        public ActionResult LogoutbyManager(ContractAttendanceViewModel Empdd)
        {
            try
            {
                var Emp = EM.LogoutbyManager(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ApprovedHrbyManager
        [Route("Employee/ApprovedHrbyManager")]
        [HttpPost]
        public ActionResult ApprovedHrbyManager(ContractAttendanceViewModel Empdd)
        {
            try
            {
                var Emp = EM.ApprovedHrbyManager(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ApprovedbyManager
        [Route("Employee/ApprovedbyManager")]
        [HttpPost]
        public ActionResult ApprovedbyManager(ContractApprovedViewModel model)
        {
            try
            {
                var Emp = EM.ApprovedbyManager(model);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDVendorList
        [Route("Employee/DDVendorList")]
        [HttpPost]
        public ActionResult DDVendorList(DDVendorListViewModel Empdd)
        {
            try
            {
                var Emp = EM.DDVendorList(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDSiteList
        [Route("Employee/DDSiteList")]
        [HttpPost]
        public ActionResult DDSiteList(DDSiteListViewModel Empdd)
        {
            try
            {
                var Emp = EM.DDSiteList(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDProjectList
        [Route("Employee/DDProjectList")]
        [HttpPost]
        public ActionResult DDProjectList(DDProjectListViewModel Empdd)
        {
            try
            {
                var Emp = EM.DDProjectList(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddVendorList
        [Route("Employee/AddVendorList")]
        [HttpPost]
        public ActionResult AddVendorList(VendorListViewModel model)
        {
            try
            {
                var Emp = EM.AddVendorList(model);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/AddProjectList
        [Route("Employee/AddProjectList")]
        [HttpPost]
        public ActionResult AddProjectList(ProjectListViewModel model)
        {
            try
            {
                var Emp = EM.AddProjectList(model);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/FetchAttendance
        [Route("Employee/FetchAttendance")]
        [HttpPost]
        public ActionResult FetchAttendance()
        {
            try
            {
                // Call the static method to fetch attendance
                EmployeeMasterModel.FetchAttendance();

                return Json(new
                {
                    StatusCode = 200,
                    Message = "Attendance fetched successfully for yesterday!"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return Json(new
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching attendance.",
                    Error = ex.Message
                });
            }
        }
        // POST: Employee/CFLeaveCredits
        [Route("Employee/CFLeaveCredits")]
        [HttpPost]
        public ActionResult CFLeaveCredits()
        {
            try
            {
                // Call the static method to fetch attendance
                EmployeeMasterModel.CFLeaveCredits();

                return Json(new
                {
                    StatusCode = 200,
                    Message = "CL and EL Credited and Carry forwarded successfully for Today!"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return Json(new
                {
                    StatusCode = 500,
                    Message = "An error occurred while Crediting and Carry forwarding CL and EL.",
                    Error = ex.Message
                });
            }
        }
        // POST: Employee/GetAllEmpProbationTrackingHistory
        [Route("Employee/GetAllEmpProbationTrackingHistory")]
        [HttpPost]
        public ActionResult GetAllEmpProbationTrackingHistory(EmpProbationTrackingHistoryViewModel model)
        {
            try
            {
                var emp = EM.GetAllEmpProbationTrackingHistory(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/ConfirmProbation
        [Route("Employee/ConfirmProbation")]
        [HttpPost]
        public ActionResult ConfirmProbation(EmpProbationTrackingHistoryViewModel model)
        {
            try
            {
                var emp = EM.ConfirmProbation(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDReporterList
        [Route("Employee/DDReporterList")]
        [HttpPost]
        public ActionResult DDReporterList(DDReporterListViewModel model)
        {
            try
            {
                var emp = EM.GetDDReporterList(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/DDEmployeeList
        [Route("Employee/DDEmployeeList")]
        [HttpPost]
        public ActionResult DDEmployeeList(DDEmployeeListViewModel model)
        {
            try
            {
                var emp = EM.GetDDEmployeeList(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Employee/GetAllEmployeeLogHistory
        [Route("Employee/GetAllEmployeeLogHistory")]
        [HttpPost]
        public ActionResult GetAllEmployeeLogHistory(EmployeeMasterLogViewModel model)
        {
            try
            {
                var emp = EM.GetAllEmployeeLogHistory(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("Employee/GetDesignationHierarchy")]
        public ActionResult GetDesignationHierarchy(HierarchyRequestViewModel model)
        {
            try
            {
                var result = EM.GetDesignationHierarchy(model);

                // Transform the summary to avoid dictionary with int keys
                var transformedSummary = new
                {
                    result.Summary.TotalEmployees,
                    result.Summary.TotalDepartments,
                    result.Summary.TotalDesignations,
                    result.Summary.TotalGrades,
                    result.Summary.EmployeesByGrade,
                    result.Summary.EmployeesByDepartment,
                    result.Summary.EmployeesByDesignation,
                    EmployeesByHierarchyLevel = result.Summary.EmployeesByHierarchyLevel
                        .ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value)
                };

                return Json(new { Success = true, Data = new { result.Hierarchy, Summary = transformedSummary, result.GeneratedOn } }, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { Success = false, StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Route("Employee/GetGradeWiseHierarchy")]
        public ActionResult GetGradeWiseHierarchy(HierarchyRequestViewModel model)
        {
            try
            {
                var result = EM.GetDesignationHierarchy(model);

                // Group by Grade
                var gradeWiseData = new Dictionary<string, object>();

                foreach (var grade in result.Summary.EmployeesByGrade.Keys.OrderBy(g => g))
                {
                    var employeesInGrade = new List<HierarchyResponseViewModel>();
                    EM.FlattenHierarchy(result.Hierarchy, employeesInGrade);

                    gradeWiseData[grade] = employeesInGrade
                        .Where(e => e.GradeName == grade)
                        .OrderBy(e => e.HierarchyLevel)
                        .ToList();
                }

                return Json(new
                {
                    Success = true,
                    Data = new
                    {
                        GradeWise = gradeWiseData,
                        Summary = result.Summary
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { Success = false, StatusCode = ex.StatusCode, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}