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
using static OfficeConnect_Web.Models.PayrollModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class PayrollController : Controller
    {
        PayrollModel PM = new PayrollModel();

        // POST: Payroll/DDPayrollSymbols
        [Route("Payroll/DDPayrollSymbols")]
        [HttpPost]
        public ActionResult DDPayrollSymbols(PayrolAccessViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayrollSymbols(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayrollFrequency
        [Route("Payroll/DDPayrollFrequency")]
        [HttpPost]
        public ActionResult DDPayrollFrequency(PayrolAccessViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayrollFrequency(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayrollPayoutType
        [Route("Payroll/DDPayrollPayoutType")]
        [HttpPost]
        public ActionResult DDPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllPayrollPayoutType
        [Route("Payroll/GetAllPayrollPayoutType")]
        [HttpPost]
        public ActionResult GetAllPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetPayrollPayoutType
        [Route("Payroll/GetPayrollPayoutType")]
        [HttpPost]
        public ActionResult GetPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.GetPayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayrollPayoutType
        [Route("Payroll/AddPayrollPayoutType")]
        [HttpPost]
        public ActionResult AddPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.AddPayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayrollPayoutType
        [Route("Payroll/UpdatePayrollPayoutType")]
        [HttpPost]
        public ActionResult UpdatePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.UpdatePayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayrollPayoutType
        [Route("Payroll/DeletePayrollPayoutType")]
        [HttpPost]
        public ActionResult DeletePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.DeletePayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/ActivatePayrollPayoutType
        [Route("Payroll/ActivatePayrollPayoutType")]
        [HttpPost]
        public ActionResult ActivatePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.ActivatePayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeactivatePayrollPayoutType
        [Route("Payroll/DeactivatePayrollPayoutType")]
        [HttpPost]
        public ActionResult DeactivatePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                var Payroll = PM.DeactivatePayrollPayoutType(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayrollSegment
        [Route("Payroll/DDPayrollSegment")]
        [HttpPost]
        public ActionResult DDPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayrollSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllPayrollPayoutTypeSegment
        [Route("Payroll/GetAllPayrollPayoutTypeSegment")]
        [HttpPost]
        public ActionResult GetAllPayrollPayoutTypeSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayrollPayoutTypeSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllPayrollSegment
        [Route("Payroll/GetAllPayrollSegment")]
        [HttpPost]
        public ActionResult GetAllPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayrollSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetPayrollSegment
        [Route("Payroll/GetPayrollSegment")]
        [HttpPost]
        public ActionResult GetPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.GetPayrollSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayrollSegment
        [Route("Payroll/AddPayrollSegment")]
        [HttpPost]
        public ActionResult AddPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.AddPayrollSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayrollSegment
        [Route("Payroll/UpdatePayrollSegment")]
        [HttpPost]
        public ActionResult UpdatePayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.UpdatePayrollSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayrollSegment
        [Route("Payroll/DeletePayrollSegment")]
        [HttpPost]
        public ActionResult DeletePayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                var Payroll = PM.DeletePayrollSegment(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayrollEmpList
        [Route("Payroll/DDPayrollEmpList")]
        [HttpPost]
        public ActionResult DDPayrollEmpList(PayrollALLComponentViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayrollEmpList(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayrollComponent
        [Route("Payroll/DDPayrollComponent")]
        [HttpPost]
        public ActionResult DDPayrollComponent(PayrollALLComponentViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayrollComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddComponent
        [Route("Payroll/AddComponent")]
        [HttpPost]
        public ActionResult AddComponent(PayrollALLComponentViewModel model)
        {
            try
            {
                var Payroll = PM.AddComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllComponentDetails
        [Route("Payroll/GetAllComponentDetails")]
        [HttpPost]
        public ActionResult GetAllComponentDetails(PayrollALLComponentViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllComponentDetails(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/EmpCTCCalculation
        [Route("Payroll/EmpCTCCalculation")]
        [HttpPost]
        public ActionResult EmpCTCCalculation(PayrollALLComponentViewModel model)
        {
            try
            {
                var Payroll = PM.EmpCTCCalculation(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/EmpPayslipGeneration
        [Route("Payroll/EmpPayslipGeneration")]
        [HttpPost]
        public ActionResult EmpPayslipGeneration(PayslipRequestViewModel model)
        {
            try
            {
                var Payroll = PM.EmpPayslipGeneration(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayslipSection
        [Route("Payroll/DDPayslipSection")]
        [HttpPost]
        public ActionResult DDPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                var Payroll = PM.DDPayslipSection(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        public ActionResult GetAllPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayslipSection(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetPayslipSection
        [Route("Payroll/GetPayslipSection")]
        [HttpPost]
        public ActionResult GetPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                var Payroll = PM.GetPayslipSection(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayslipSection
        [Route("Payroll/AddPayslipSection")]
        [HttpPost]
        public ActionResult AddPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                var Payroll = PM.AddPayslipSection(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayslipSection
        [Route("Payroll/UpdatePayslipSection")]
        [HttpPost]
        public ActionResult UpdatePayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                var Payroll = PM.UpdatePayslipSection(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayslipSection
        [Route("Payroll/DeletePayslipSection")]
        [HttpPost]
        public ActionResult DeletePayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                var Payroll = PM.DeletePayslipSection(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllPayslipSectionComponent
        [Route("Payroll/GetAllPayslipSectionComponent")]
        [HttpPost]
        public ActionResult GetAllPayslipSectionComponent(PayslipSectionComponentViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayslipSectionComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetPayslipSectionComponent
        [Route("Payroll/GetPayslipSectionComponent")]
        [HttpPost]
        public ActionResult GetPayslipSectionComponent(PayslipSectionComponentViewModel model)
        {
            try
            {
                var Payroll = PM.GetPayslipSectionComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayslipSectionComponent
        [Route("Payroll/AddPayslipSectionComponent")]
        [HttpPost]
        public ActionResult AddPayslipSectionComponent(PayslipPayloadRequest model)
        {
            try
            {
                var Payroll = PM.AddPayslipSectionComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayslipSectionComponent
        [Route("Payroll/UpdatePayslipSectionComponent")]
        [HttpPost]
        public ActionResult UpdatePayslipSectionComponent(UpdatePayslipPayload model)
        {
            try
            {
                var Payroll = PM.UpdatePayslipSectionComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayslipSectionComponent
        [Route("Payroll/DeletePayslipSectionComponent")]
        [HttpPost]
        public ActionResult DeletePayslipSectionComponent(DeletePayslipPayload model)
        {
            try
            {
                var Payroll = PM.DeletePayslipSectionComponent(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllEmployeeSalaryDetails
        [Route("Payroll/GetAllEmployeeSalaryDetails")]
        [HttpPost]
        public ActionResult GetAllEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllEmployeeSalaryDetails(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetEmployeeSalaryDetails
        [Route("Payroll/GetEmployeeSalaryDetails")]
        [HttpPost]
        public ActionResult GetEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                var Payroll = PM.GetEmployeeSalaryDetails(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddEmployeeSalaryDetails
        [Route("Payroll/AddEmployeeSalaryDetails")]
        [HttpPost]
        public ActionResult AddEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                var Payroll = PM.AddEmployeeSalaryDetails(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdateEmployeeSalaryDetails
        [Route("Payroll/UpdateEmployeeSalaryDetails")]
        [HttpPost]
        public ActionResult UpdateEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                var Payroll = PM.UpdateEmployeeSalaryDetails(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeleteEmployeeSalaryDetails
        [Route("Payroll/DeleteEmployeeSalaryDetails")]
        [HttpPost]
        public ActionResult DeleteEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                var Payroll = PM.DeleteEmployeeSalaryDetails(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllPayoutMappingMaster
        [Route("Payroll/GetAllPayoutMappingMaster")]
        public ActionResult GetAllPayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayoutMappingMaster(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetPayoutMappingMaster
        [Route("Payroll/GetPayoutMappingMaster")]
        [HttpPost]
        public ActionResult GetPayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                var Payroll = PM.GetPayoutMappingMaster(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayoutMappingMaster
        [Route("Payroll/AddPayoutMappingMaster")]
        [HttpPost]
        public ActionResult AddPayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                var Payroll = PM.AddPayoutMappingMaster(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayoutMappingMaster
        [Route("Payroll/UpdatePayoutMappingMaster")]
        [HttpPost]
        public ActionResult UpdatePayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                var Payroll = PM.UpdatePayoutMappingMaster(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayoutMappingMaster
        [Route("Payroll/DeletePayoutMappingMaster")]
        [HttpPost]
        public ActionResult DeletePayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                var Payroll = PM.DeletePayoutMappingMaster(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/PayrollReportforALL
        [Route("Payroll/PayrollReportforALL")]
        [HttpPost]
        public ActionResult PayrollReportforALL(PayrollReportViewModel model)
        {
            try
            {
                var Payroll = PM.PayrollReportforALL(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDLegalEntity
        [Route("Payroll/DDLegalEntity")]
        [HttpPost]
        public ActionResult DDLegalEntity(DDLegalEntityPayrollViewModel model)
        {
            try
            {
                var Payroll = PM.GetDDLegalEntity(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDLocation
        [Route("Payroll/DDLocation")]
        [HttpPost]
        public ActionResult DDLocation(DDLocationPayrollViewModel model)
        {
            try
            {
                var Payroll = PM.GetDDLocation(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetAllPayrollVariable
        [Route("Payroll/GetAllPayrollVariable")]
        [HttpPost]
        public ActionResult GetAllPayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                var Payroll = PM.GetAllPayrollVariable(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/GetPayrollVariable
        [Route("Payroll/GetPayrollVariable")]
        [HttpPost]
        public ActionResult GetPayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                var Payroll = PM.GetPayrollVariable(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayrollVariable
        [Route("Payroll/AddPayrollVariable")]
        [HttpPost]
        public ActionResult AddPayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                var Payroll = PM.AddPayrollVariable(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayrollVariable
        [Route("Payroll/UpdatePayrollVariable")]
        [HttpPost]
        public ActionResult UpdatePayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                var Payroll = PM.UpdatePayrollVariable(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayrollVariable
        [Route("Payroll/DeletePayrollVariable")]
        [HttpPost]
        public ActionResult DeletePayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                var Payroll = PM.DeletePayrollVariable(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DDPayrollVariable
        [Route("Payroll/DDPayrollVariable")]
        [HttpPost]
        public ActionResult DDPayrollVariable(DDPayrollVariableViewModel model)
        {
            try
            {
                var Payroll = PM.GetDDPayrollVariable(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/PayrollVariableHistory
        [Route("Payroll/PayrollVariableHistory")]
        [HttpPost]
        public ActionResult PayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                var Payroll = PM.PayrollVariableHistory(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/AddPayrollVariableHistory
        [Route("Payroll/AddPayrollVariableHistory")]
        [HttpPost]
        public ActionResult AddPayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                var Payroll = PM.AddPayrollVariableHistory(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/UpdatePayrollVariableHistory
        [Route("Payroll/UpdatePayrollVariableHistory")]
        [HttpPost]
        public ActionResult UpdatePayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                var Payroll = PM.UpdatePayrollVariableHistory(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Payroll/DeletePayrollVariableHistory
        [Route("Payroll/DeletePayrollVariableHistory")]
        [HttpPost]
        public ActionResult DeletePayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                var Payroll = PM.DeletePayrollVariableHistory(model);
                return Json(Payroll, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}