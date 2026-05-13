using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    //public class VisitorManagementViewModel
    //{
    //    public int VisitId { get; set; }
    //    public string RegNo { get; set; }
    //    public string QR { get; set; }
    //    public string Name { get; set; }
    //    public string InviteCode { get; set; }
    //    public string Designation { get; set; }
    //    public string Company { get; set; }
    //    public string Purpose { get; set; }
    //    public string PMail { get; set; }
    //    public string OMail { get; set; }
    //    public string Mobile { get; set; }
    //    public string AMobile { get; set; }
    //    public string Photo { get; set; }
    //    public string CompId { get; set; }
    //    public string CompName { get; set; }
    //    public int? WhomtoMeet { get; set; }
    //    public string WName { get; set; }
    //    public string WEmpCode { get; set; }
    //    public string otp { get; set; }
    //    public Nullable<System.DateTime> Date { get; set; }
    //    public string Time { get; set; }
    //    public Nullable<bool> Invited { get; set; }
    //    public Nullable<bool> Accept { get; set; }
    //    public Nullable<bool> Approved { get; set; }
    //    public Nullable<bool> Expired { get; set; }
    //    public string Accessories { get; set; }
    //    public Nullable<bool> DirectCheckIn { get; set; }
    //    public Nullable<System.DateTime> CheckIn { get; set; }
    //    public Nullable<System.DateTime> CheckOut { get; set; }
    //    public string IdCard { get; set; }
    //    public Nullable<bool> VisitorCheckIn { get; set; }
    //    public Nullable<bool> VisitorCheckOut { get; set; }
    //    public string Status { get; set; }
    //    public Nullable<int> CreatedBy { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    public Nullable<int> LastUpdatedBy { get; set; }
    //    public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    //    public Nullable<bool> IsActive { get; set; }
    //    public Nullable<bool> IsUpdated { get; set; }
    //    public Nullable<bool> IsDeleted { get; set; }
    //    public int EmpId { get; set; }
    //    public string msg { get; set; }
    //}

    public class VisitorManagementViewModel
    {
        public int VisitId { get; set; }
        public int LoginId { get; set; }
        public string RegNo { get; set; }
        public string QR { get; set; }
        public string Name { get; set; }
        public string InviteCode { get; set; }
        public string Designation { get; set; }
        public string Company { get; set; }
        public string Purpose { get; set; }
        public string PMail { get; set; }
        public string OMail { get; set; }
        public string Mobile { get; set; }
        public string AMobile { get; set; }
        public string Photo { get; set; }
        public string CompId { get; set; }
        public string CompName { get; set; }
        public int? WhomtoMeet { get; set; }
        public string WName { get; set; }
        public string WEmpCode { get; set; }
        public string otp { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Time { get; set; }
        public Nullable<bool> Invited { get; set; }
        public Nullable<bool> Accept { get; set; }
        public Nullable<bool> Approved { get; set; }
        public Nullable<bool> Expired { get; set; }
        public string Accessories { get; set; }
        public Nullable<bool> DirectCheckIn { get; set; }
        public Nullable<System.DateTime> CheckIn { get; set; }
        public Nullable<System.DateTime> CheckOut { get; set; }
        public string IdCard { get; set; }
        public Nullable<bool> VisitorCheckIn { get; set; }
        public Nullable<bool> VisitorCheckOut { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
        public string msg { get; set; }
        public List<EmployeeMasterViewModel> EmployeeDetails { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
        public class VisitorInviteHistoryViewModel
    {
        public int Id { get; set; }
        public Nullable<int> VisitorId { get; set; }
        public string InviteCode { get; set; }
        public Nullable<bool> Mail { get; set; }
        public Nullable<bool> Mobile { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class EmailSetUpViewModel
    {
        public int Id { get; set; }
        public int? CompId { get; set; }
        public string EmailId { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPPort { get; set; }
        public string SMTPMailId { get; set; }
        public string SMTPPassword { get; set; }
    }
    public class FilterViewModel
    {
        public int EmpId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
    }
}