using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class ShiftMasterViewModel
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string ClkHrs { get; set; }
        public string Days { get; set; }
        public Nullable<bool> Status { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int LoginId { get; set; }
        public string msg { get; set; }
    }
    public class ShiftGroupingViewModel
    {
        public int SGId { get; set; }
        public int? CompId { get; set; }
        public string Company { get; set; }
        public int? LEId { get; set; }
        public string LegalEntity { get; set; }
        public int? BUId { get; set; }
        public string BusinessUnit { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public Nullable<bool> Status { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int LoginId { get; set; }
        public string msg { get; set; }
        public List<SampleShiftMasterViewModel> lstOfShift { get; set; }
    }
    public class SampleShiftMasterViewModel
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string ClkHrs { get; set; }
        public string Days { get; set; }
    }
    public class GetShiftGroupingViewModel
    {
        public int SGId { get; set; }
        public int? CompId { get; set; }
        public string Company { get; set; }
        public int? LEId { get; set; }
        public string LegalEntity { get; set; }
        public int? BUId { get; set; }
        public string BusinessUnit { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string ClkHrs { get; set; }
        public string Days { get; set; }
        public Nullable<bool> Status { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int LoginId { get; set; }
        public string msg { get; set; }
    }
    public class DDShiftViewModel
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
    }
    public class ShiftEmployeeListViewModel
    {
        public List<ShiftEmployeeMasterViewModel> ShiftEmployee { get; set; }
        public List<ShiftEmployeeMasterViewModel> NonShiftEmployee { get; set; }
    }
    public class ShiftEmployeeMasterViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public Nullable<int> OldEmp_ID { get; set; }
        public Nullable<int> CompId { get; set; }
        public string Company { get; set; }
        public Nullable<int> LEId { get; set; }
        public string LegalEntity { get; set; }
        public Nullable<int> BUId { get; set; }
        public string BusinessUnit { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Location { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public string ShiftName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> DeptId { get; set; }
        public string DeptName { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string Designation { get; set; }
        public Nullable<int> ReportId { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public string Approver { get; set; }
        public string ReportEmpCode { get; set; }
        public Nullable<bool> Authorised { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string msg { get; set; }
    }
    public class ShiftEmployeeMappingMasterViewModel
    {
        public int? LoginId { get; set; }
        public Nullable<int> CompId { get; set; }
        public string Company { get; set; }
        public Nullable<int> LEId { get; set; }
        public string LegalEntity { get; set; }
        public Nullable<int> BUId { get; set; }
        public string BusinessUnit { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Location { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string msg { get; set; }
        public List<ShiftEmployeeViewModel> EmpList { get; set; }
    }
    public class ShiftEmployeeViewModel
    {
        public int? EmpId { get; set; }
        public Nullable<int> OldEmp_ID { get; set; }
        public string EmpCode { get; set; }
    }
    public class NewResponseViewModel
    {
        public int? LoginId { get; set; }
        public string msg { get; set; }
    }
}