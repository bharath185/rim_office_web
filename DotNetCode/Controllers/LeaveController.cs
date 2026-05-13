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
using static OfficeConnect_Web.Models.LeaveModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class LeaveController : Controller
    {
        LeaveModel LM = new LeaveModel();

        // POST: Leave/GetAllLeaveType
        [Route("Leave/GetAllLeaveType")]
        [HttpPost]
        public ActionResult GetAllLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.GetAllLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetLeaveType
        [Route("Leave/GetLeaveType")]
        [HttpPost]
        public ActionResult GetLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.GetLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/AddLeaveType
        [Route("Leave/AddLeaveType")]
        [HttpPost]
        public ActionResult AddLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.AddLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/UpdateLeaveType
        [Route("Leave/UpdateLeaveType")]
        [HttpPost]
        public ActionResult UpdateLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.UpdateLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DeleteLeaveType
        [Route("Leave/DeleteLeaveType")]
        [HttpPost]
        public ActionResult DeleteLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.DeleteLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/ActivateLeaveType
        [Route("Leave/ActivateLeaveType")]
        [HttpPost]
        public ActionResult ActivateLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.ActivateLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DeactivateLeaveType
        [Route("Leave/DeactivateLeaveType")]
        [HttpPost]
        public ActionResult DeactivateLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                var leave = LM.DeactivateLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DDLeaveType
        [Route("Leave/DDLeaveType")]
        [HttpPost]
        public ActionResult DDLeaveType(DDLeaveTypePayloadViewModel model)
        {
            try
            {
                var leave = LM.GetDDLeaveType(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DDApproveManager
        [Route("Leave/DDApproveManager")]
        [HttpPost]
        public ActionResult DDApproveManager(DDComOffManager model)
        {
            try
            {
                var leave = LM.DDApproveManager(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/CompOffLeave
        [Route("Leave/CompOffLeave")]
        [HttpPost]
        public ActionResult CompOffLeave(CompOffRequestViewModel model)
        {
            try
            {
                var leave = LM.CompOffLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/CompOffHours
        [Route("Leave/CompOffHours")]
        [HttpPost]
        public ActionResult CompOffHours(CompOffRequestViewModel model)
        {
            try
            {
                var leave = LM.CompOffHours(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllEmpCompOffLeave
        [Route("Leave/GetAllEmpCompOffLeave")]
        [HttpPost]
        public ActionResult GetAllEmpCompOffLeave(CompOffRequestViewModel model)
        {
            try
            {
                var leave = LM.GetAllEmpCompOffLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllCompOffLeave
        [Route("Leave/GetAllCompOffLeave")]
        [HttpPost]
        public ActionResult GetAllCompOffLeave(CompOffRequestViewModel model)
        {
            try
            {
                var leave = LM.GetAllCompOffLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/ApproveCompOff
        [Route("Leave/ApproveCompOff")]
        [HttpPost]
        public ActionResult ApproveCompOff(ApproveCompOffViewModel model)
        {
            try
            {
                var leave = LM.ApproveCompOff(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/RejectCompOff
        [Route("Leave/RejectCompOff")]
        [HttpPost]
        public ActionResult RejectCompOff(ApproveCompOffViewModel model)
        {
            try
            {
                var leave = LM.RejectCompOff(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DraftLeave
        [Route("Leave/DraftLeave")]
        [HttpPost]
        public ActionResult DraftLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.DraftLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/ApplyLeave
        [Route("Leave/ApplyLeave")]
        [HttpPost]
        public ActionResult ApplyLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.ApplyLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DraftApplyLeave
        [Route("Leave/DraftApplyLeave")]
        [HttpPost]
        public ActionResult DraftApplyLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.DraftApplyLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/WithDrawLeave
        [Route("Leave/WithDrawLeave")]
        [HttpPost]
        public ActionResult WithDrawLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.WithDrawLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/CancelLeave
        [Route("Leave/CancelLeave")]
        [HttpPost]
        public ActionResult CancelLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.CancelLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/DeleteDraftLeave
        [Route("Leave/DeleteDraftLeave")]
        [HttpPost]
        public ActionResult DeleteDraftLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.DeleteDraftLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllLeave
        [Route("Leave/GetAllLeave")]
        [HttpPost]
        public ActionResult GetAllLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.GetAllLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllApplyManagerLeave
        [Route("Leave/GetAllApplyManagerLeave")]
        [HttpPost]
        public ActionResult GetAllApplyManagerLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.GetAllApplyManagerLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllManagerLeave
        [Route("Leave/GetAllManagerLeave")]
        [HttpPost]
        public ActionResult GetAllManagerLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.GetAllManagerLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/ApproveLeaveByManager
        [Route("Leave/ApproveLeaveByManager")]
        [HttpPost]
        public ActionResult ApproveLeaveByManager(ApproveLeaveViewModel model)
        {
            try
            {
                var leave = LM.ApproveLeaveByManager(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/RejectLeaveByManager
        [Route("Leave/RejectLeaveByManager")]
        [HttpPost]
        public ActionResult RejectLeaveByManager(ApproveLeaveViewModel model)
        {
            try
            {
                var leave = LM.RejectLeaveByManager(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllApplyHRLeave
        [Route("Leave/GetAllApplyHRLeave")]
        [HttpPost]
        public ActionResult GetAllApplyHRLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.GetAllApplyHRLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/GetAllHRLeave
        [Route("Leave/GetAllHRLeave")]
        [HttpPost]
        public ActionResult GetAllHRLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.GetAllHRLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/ApproveLeaveByHR
        [Route("Leave/ApproveLeaveByHR")]
        [HttpPost]
        public ActionResult ApproveLeaveByHR(ApproveLeaveViewModel model)
        {
            try
            {
                var leave = LM.ApproveLeaveByHR(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/RejectLeaveByHR
        [Route("Leave/RejectLeaveByHR")]
        [HttpPost]
        public ActionResult RejectLeaveByHR(ApproveLeaveViewModel model)
        {
            try
            {
                var leave = LM.RejectLeaveByHR(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/UploadFileLeave
        [Route("Leave/UploadFileLeave")]
        [HttpPost]
        public ActionResult UploadFileLeave(FileUploadAPIViewModel model)
        {
            try
            {
                var leave = LM.UploadFileLeave(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/IndividualLeaveCount
        [Route("Leave/IndividualLeaveCount")]
        [HttpPost]
        public ActionResult IndividualLeaveCount(EmpLeaveApplicationViewModel model)
        {
            try
            {
                var leave = LM.IndividualLeaveCount(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Leave/LeaveBalReport
        [Route("Leave/LeaveBalReport")]
        [HttpPost]
        public ActionResult LeaveBalReport(LeaveBalReportViewModel model)
        {
            try
            {
                var leave = LM.LeaveBalReport(model);
                return Json(leave, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}