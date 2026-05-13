using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class DashboardViewModel
    {
        public Nullable<int> LoginId { get; set; }
        public Nullable<int> LEId { get; set; }
        public ListBirthdayViewModel lstofbirthday { get; set; }
        public List<HolidayListViewModel> lstofholiday { get; set; }
        public List<EmployeeListViewModel> lstofemp { get; set; }
    }
    public class ListBirthdayViewModel
    {
        public List<BirthdayViewModel> lstofdaybirthday { get; set; }
        public Nullable<int> daycount { get; set; }
        public List<BirthdayViewModel> lstofweekbirthday { get; set; }
        public Nullable<int> weekcount { get; set; }
        public List<BirthdayViewModel> lstofmonthbirthday { get; set; }
        public Nullable<int> monthcount { get; set; }
    }
    public class BirthdayViewModel
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Day { get; set; }
        public string Gender { get; set; }
    }
    public class HolidayListViewModel
    {
        ////public int CompanyId { get; set; }
        ////public string Company { get; set; }
        ////public int LEId { get; set; }
        ////public string LegalEntity { get; set; }
        ////public int BUId { get; set; }
        ////public string BusinessUnit { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Location { get; set; }
        public int HolidayId { get; set; }
        public Nullable<int> Year { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string HolidayType { get; set; }

    }
    public class EmployeeListViewModel
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> DeptId { get; set; }
        public string Department { get; set; }
        public Nullable<int> DesigId { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
    }
    public class HRCountViewModel
    {
        //public IEnumerable<object> GetvisitorToday { get; set; }
        //public object CurrentmonthemployeeList { get; set; }


        public List<VisitorManagementViewModel> GetvisitorToday { get; set; }
        public List<EmployeeMasterViewModel> CurrentmonthemployeeList { get; set; }

        //   public int PendingLeaveCount { get; set; }

        public List<EmpLeaveApplicationViewModel> PendingLeaves { get; set; }

        public List<EmpLeaveApplicationViewModel> AllLeaves { get; set; }

        public List<CompOffRequestViewModel> CompOffList { get; set; }
    }
}