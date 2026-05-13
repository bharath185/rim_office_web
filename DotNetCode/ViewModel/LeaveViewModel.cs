using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class LeaveTypeViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string YearType { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DurationType { get; set; }
        public string ApplicableTo { get; set; }
        public string EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public string EmpLevel { get; set; }
        public Nullable<bool> CarryForward { get; set; }
        public Nullable<int> Credit { get; set; }
        public Nullable<bool> IsMonth { get; set; }
        public Nullable<bool> IsYear { get; set; }
        public Nullable<int> MaxCarryForward { get; set; }
        public Nullable<bool> Encashable { get; set; }
        public Nullable<int> MaxApply { get; set; }
        public Nullable<int> MaxPerMonth { get; set; }
        public Nullable<int> MaxPerYear { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<int> ApplicableDuration { get; set; }
        public Nullable<bool> IsSingleApplication { get; set; }
        public Nullable<int> MaxAllowedEvents { get; set; }
        public Nullable<bool> WeekEndInclusive { get; set; }
        public Nullable<bool> ResetYear { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    ////public class LeaveResponseViewModel
    ////{
    ////    public int Status { get; set; }
    ////    public string msg { get; set; }
    ////}
    ///
    public class LeaveResponseViewModel
    {
        public int Status { get; set; }
        public string msg { get; set; }
        public List<int> ApprovedIds { get; set; }
        public List<int> FailedIds { get; set; }
        public List<string> Errors { get; set; }
    }
    public class DDLeaveTypeViewModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string ShortName { get; set; }
        public int EmpId { get; set; }
        public int LoginId { get; set; }
    }
    public class DDLeaveTypePayloadViewModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
    public class EmpLeaveApplicationViewModel
    {
        public int LoginId { get; set; }
        public int LeaveAppId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> AppliedDate { get; set; }
        public Nullable<System.DateTime> CompOffDate { get; set; }
        public string CompOffReason { get; set; }
        public string DocName { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public string Approver { get; set; }
        public Nullable<bool> IsLOP { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> HRApproved { get; set; }
        public Nullable<System.DateTime> HRApprovedDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class LeaveApprovalViewModel
    {
        public List<EmpLeaveApplicationViewModel> AppliedList { get; set; }
        public List<EmpLeaveApplicationViewModel> AllList { get; set; }
    }
    public class ApproveLeaveViewModel
    {
        public int LoginId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public List<LeaveAppIdViewModel> lstofLevAppId { get; set; }
    }
    public class LeaveAppIdViewModel
    {
        public int LeaveAppId { get; set; }
        public string Remarks { get; set; }
    }
    public class LeaveCarryForwardMasterViewModel
    {
        public int CFId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public Nullable<int> LeaveYear { get; set; }
        public Nullable<int> LeaveMonth { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> Availed { get; set; }
        public Nullable<decimal> CarryForward { get; set; }
        public Nullable<decimal> Encashment { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class CarryForwardMasterViewModel
    {
        public int CFId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public Nullable<int> LeaveYear { get; set; }
        public Nullable<int> LeaveMonth { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> Availed { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
    }
    public class LeaveCountsViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public List<CarryForwardMasterViewModel> CasualCounts { get; set; }
        public List<CarryForwardMasterViewModel> ReservedHolidayCounts { get; set; }
        public List<CarryForwardMasterViewModel> EarnedLeaveCounts { get; set; }
        public List<CarryForwardMasterViewModel> CompOffCounts { get; set; }
        public List<CarryForwardMasterViewModel> MLCounts { get; set; }
        public List<CarryForwardMasterViewModel> PLCounts { get; set; }
    }
    public class CompOffRequestViewModel
    {
        public Nullable<int> LoginId { get; set; }
        public int CompOffReqId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string ManagerCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Project { get; set; }
        public Nullable<int> TaskId { get; set; }
        public string Task { get; set; }
        public Nullable<decimal> Hrs { get; set; }
        public string ActualHrs { get; set; }
        public string WorkMode { get; set; }
        public Nullable<bool> IsRequested { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public string Reason { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsUsed { get; set; }
    }
    public class CompOffHoursRequestViewModel
    {
        public Nullable<int> LoginId { get; set; }
        public int CompOffReqId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string ActualHrs { get; set; }
        public string WorkMode { get; set; }
    }
    public class DDComOffManager
    {
        public Nullable<int> LoginId { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerCode { get; set; }
    }
    public class ApproveCompOffViewModel
    {
        public int LoginId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public List<CompOffReqIdViewModel> lstofCompOffReqId { get; set; }
    }
    public class CompOffReqIdViewModel
    {
        public int CompOffReqId { get; set; }
        public string Remarks { get; set; }
    }
    public class LeaveBalReportViewModel
    {
        public int LoginId { get; set; }
        public Nullable<int> CompId { get; set; }
        public Nullable<int> LEId { get; set; }
        public Nullable<int> BUId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<int> CLLeaveTypeId { get; set; }
        public string CLLeaveType { get; set; }
        public Nullable<int> ELLeaveTypeId { get; set; }
        public string ELLeaveType { get; set; }
        public Nullable<int> RHLeaveTypeId { get; set; }
        public string RHLeaveType { get; set; }
        public Nullable<int> COMPOFFLeaveTypeId { get; set; }
        public string COMPOFFLeaveType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public Nullable<decimal> CLOpeningBalance { get; set; }
        public Nullable<decimal> CLAvailed { get; set; }
        public Nullable<decimal> CLCarryFroward { get; set; }
        public Nullable<decimal> CLColsingBalance { get; set; }
        public Nullable<decimal> ELOpeningBalance { get; set; }
        public Nullable<decimal> ELAvailed { get; set; }
        public Nullable<decimal> ELCarryFroward { get; set; }
        public Nullable<decimal> ELColsingBalance { get; set; }
        public Nullable<decimal> RHOpeningBalance { get; set; }
        public Nullable<decimal> RHAvailed { get; set; }
        public Nullable<decimal> RHCarryFroward { get; set; }
        public Nullable<decimal> RHColsingBalance { get; set; }
        public Nullable<decimal> COMPOFFOpeningBalance { get; set; }
        public Nullable<decimal> COMPOFFAvailed { get; set; }
        public Nullable<decimal> COMPOFFCarryFroward { get; set; }
        public Nullable<decimal> COMPOFFColsingBalance { get; set; }
    }
}