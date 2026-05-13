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
using static OfficeConnect_Web.Models.BusinessEntityModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class BusinessEntityController : Controller
    {
        BusinessEntityModel BEM = new BusinessEntityModel();
        // POST: BusinessEntity/GetAllCompany
        [Route("BusinessEntity/GetAllCompany")]
        [HttpPost]
        public ActionResult GetAllCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetAllCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetCompany
        [Route("BusinessEntity/GetCompany")]
        [HttpPost]
        public ActionResult GetCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/AddCompany
        [Route("BusinessEntity/AddCompany")]
        [HttpPost]
        public ActionResult AddCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.AddCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/UpdateCompany
        [Route("BusinessEntity/UpdateCompany")]
        [HttpPost]
        public ActionResult UpdateCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.UpdateCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeleteCompany
        [Route("BusinessEntity/DeleteCompany")]
        [HttpPost]
        public ActionResult DeleteCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeleteCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/ActivateCompany
        [Route("BusinessEntity/ActivateCompany")]
        [HttpPost]
        public ActionResult ActivateCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.ActivateCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeActivateCompany
        [Route("BusinessEntity/DeActivateCompany")]
        [HttpPost]
        public ActionResult DeActivateCompany(CompanyMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeActivateCompany(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetAllLegalEntity
        [Route("BusinessEntity/GetAllLegalEntity")]
        [HttpPost]
        public ActionResult GetAllLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetAllLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetLegalEntity
        [Route("BusinessEntity/GetLegalEntity")]
        [HttpPost]
        public ActionResult GetLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/AddLegalEntity
        [Route("BusinessEntity/AddLegalEntity")]
        [HttpPost]
        public ActionResult AddLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.AddLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/UpdateLegalEntity
        [Route("BusinessEntity/UpdateLegalEntity")]
        [HttpPost]
        public ActionResult UpdateLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.UpdateLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeleteLegalEntity
        [Route("BusinessEntity/DeleteLegalEntity")]
        [HttpPost]
        public ActionResult DeleteLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeleteLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/ActivateLegalEntity
        [Route("BusinessEntity/ActivateLegalEntity")]
        [HttpPost]
        public ActionResult ActivateLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.ActivateLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeActivateLegalEntity
        [Route("BusinessEntity/DeActivateLegalEntity")]
        [HttpPost]
        public ActionResult DeActivateLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeActivateLegalEntity(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetAllBusinessUnit
        [Route("BusinessEntity/GetAllBusinessUnit")]
        [HttpPost]
        public ActionResult GetAllBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetAllBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetBusinessUnit
        [Route("BusinessEntity/GetBusinessUnit")]
        [HttpPost]
        public ActionResult GetBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/AddBusinessUnit
        [Route("BusinessEntity/AddBusinessUnit")]
        [HttpPost]
        public ActionResult AddBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.AddBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/UpdateBusinessUnit
        [Route("BusinessEntity/UpdateBusinessUnit")]
        [HttpPost]
        public ActionResult UpdateBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.UpdateBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeleteBusinessUnit
        [Route("BusinessEntity/DeleteBusinessUnit")]
        [HttpPost]
        public ActionResult DeleteBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeleteBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/ActivateBusinessUnit
        [Route("BusinessEntity/ActivateBusinessUnit")]
        [HttpPost]
        public ActionResult ActivateBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.ActivateBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeActivateBusinessUnit
        [Route("BusinessEntity/DeActivateBusinessUnit")]
        [HttpPost]
        public ActionResult DeActivateBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeActivateBusinessUnit(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetAllLocation
        [Route("BusinessEntity/GetAllLocation")]
        [HttpPost]
        public ActionResult GetAllLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetAllLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetLocation
        [Route("BusinessEntity/GetLocation")]
        [HttpPost]
        public ActionResult GetLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/AddLocation
        [Route("BusinessEntity/AddLocation")]
        [HttpPost]
        public ActionResult AddLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.AddLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/UpdateLocation
        [Route("BusinessEntity/UpdateLocation")]
        [HttpPost]
        public ActionResult UpdateLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.UpdateLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeleteLocation
        [Route("BusinessEntity/DeleteLocation")]
        [HttpPost]
        public ActionResult DeleteLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeleteLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/ActivateLocation
        [Route("BusinessEntity/ActivateLocation")]
        [HttpPost]
        public ActionResult ActivateLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.ActivateLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeActivateLocation
        [Route("BusinessEntity/DeActivateLocation")]
        [HttpPost]
        public ActionResult DeActivateLocation(LocationMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeActivateLocation(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetAllCalendarYear 
        [Route("BusinessEntity/GetAllCalendarYear")]
        [HttpPost]
        public ActionResult GetAllCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetAllCalendarYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetCalendarYear
        [Route("BusinessEntity/GetCalendarYear")]
        [HttpPost]
        public ActionResult GetCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetCalendarYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/AddCalendarYear
        [Route("BusinessEntity/AddCalendarYear")]
        [HttpPost]
        public ActionResult AddCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.AddCalendarYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/UpdateCalendarYear
        [Route("BusinessEntity/UpdateCalendarYear")]
        [HttpPost]
        public ActionResult UpdateCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.UpdateCalendarYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeleteCalendarYear
        [Route("BusinessEntity/DeleteCalendarYear")]
        [HttpPost]
        public ActionResult DeleteCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeleteCalendarYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetAllFinancialYear 
        [Route("BusinessEntity/GetAllFinancialYear")]
        [HttpPost]
        public ActionResult GetAllFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetAllFinancialYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/GetFinancialYear
        [Route("BusinessEntity/GetFinancialYear")]
        [HttpPost]
        public ActionResult GetFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.GetFinancialYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/AddFinancialYear
        [Route("BusinessEntity/AddFinancialYear")]
        [HttpPost]
        public ActionResult AddFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.AddFinancialYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/UpdateFinancialYear
        [Route("BusinessEntity/UpdateFinancialYear")]
        [HttpPost]
        public ActionResult UpdateFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.UpdateFinancialYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: BusinessEntity/DeleteFinancialYear
        [Route("BusinessEntity/DeleteFinancialYear")]
        [HttpPost]
        public ActionResult DeleteFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                var Entity = BEM.DeleteFinancialYear(model);
                return Json(Entity, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}