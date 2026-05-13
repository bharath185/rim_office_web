using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class Per_GoalViewModel
    {
        public int GoalId { get; set; }
        public Nullable<int> QId { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string Type { get; set; }
        public string QName { get; set; }
        public string FYear { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public string Weightage { get; set; }
        public string EmpReview { get; set; }
        public string EDescription { get; set; }
        public string ManagerReview { get; set; }
        public string MDescription { get; set; }
        public string Status { get; set; }
        public Nullable<bool> FinalSubmit { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class PerreportViewModel
    {
        public int GoalId { get; set; }
        public Nullable<int> QId { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public Nullable<int> FYearId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string Type { get; set; }
        public string QName { get; set; }
        public string FYear { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public string Weightage { get; set; }
        public string EmpReview { get; set; }
        public string EDescription { get; set; }
        public string ManagerReview { get; set; }
        public string MDescription { get; set; }
        public string Status { get; set; }
        public string OverAllStatus { get; set; }
        public Nullable<bool> FinalSubmit { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class Per_GoalListViewModel
    {
        public List<Per_GoalViewModel> listofGoal { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }
    public class ResponseViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }
    public class Per_EmployeeReviewViewModel
    {
        public List<Per_GoalViewModel> listofGoal { get; set; }
        public List<Per_BehaviourViewModel> listofBehavior { get; set; }
        public Nullable<int> QId { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public string QReview { get; set; }
        public string Period { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string EmpName { get; set; }
        public string Status { get; set; }
        public string FYear { get; set; }
        public string msg { get; set; }
    }
    public class Per_TaskViewModel
    {
        public int TaskId { get; set; }
        public Nullable<int> QId1 { get; set; }
        public string Type { get; set; }
        public string QName { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public string FYear { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<int> GoalId { get; set; }
        public string Goal { get; set; }
        public Nullable<int> QId { get; set; }
        public string Task { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class Per_BehaviourViewModel
    {
        public int Id { get; set; }
        public Nullable<int> QId { get; set; }
        public string Type { get; set; }
        public string QName { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public string FYear { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Behaviour { get; set; }
        public string Description { get; set; }
        public string EmpReview { get; set; }
        public string EDescription { get; set; }
        public string ManagerReview { get; set; }
        public string MDescription { get; set; }
        public string Weightage { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class Per_BehaviourDetailViewModel
    {
        public int Id { get; set; }
        public Nullable<int> QId { get; set; }
        public string Type { get; set; }
        public string QName { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public string FYear { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Behaviour { get; set; }
        public int? BehaviourId { get; set; }
        public string Description { get; set; }
        public string Weightage { get; set; }
        public string EmpReview { get; set; }
        public string EDescription { get; set; }
        public string ManagerReview { get; set; }
        public string MDescription { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class Per_SelfDevelopmentViewModel
    {
        public int Id { get; set; }
        public Nullable<int> QId { get; set; }
        public string Type { get; set; }
        public string QName { get; set; }
        public Nullable<int> PeriodId { get; set; }
        public string FYear { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Activity { get; set; }
        public string ActionDescription { get; set; }
        public string ActionType { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class DDFinancialYear
    {
        public Nullable<int> YearId { get; set; }
        public string FinancialYear { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }
    public class DDQuater
    {
        public Nullable<int> QId { get; set; }
        public string Name { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }
    public class DDReviewStatus
    {
        public Nullable<int> Id { get; set; }
        public string OverAllStatus { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }

    public class FyearDetailsViewModel
    {
        public Nullable<int> FYearId { get; set; }
        public string FinancialYear { get; set; }
        public string FinancialDetails { get; set; }
        public Nullable<int> QId { get; set; }
        public string QName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }
    public class QuaterMasterViewModel
    {
        public int QId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }
    public class ConfigSetupViewmodel
    {
        public int ConfigSetupId { get; set; }
        public Nullable<int> FYearId { get; set; }
        public string FYear { get; set; }
        public string Type { get; set; }
        public Nullable<int> QId { get; set; }
        public string QName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreationDate { get; set; }
        public string ExtendCreationDate { get; set; }
        public string SubmitDate { get; set; }
        public string ExtendSubmitDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string msg { get; set; }
    }

    public class ReviewListViewModel
    {
        public int ReviewId { get; set; }
        public Nullable<int> FYearId { get; set; }
        public Nullable<int> QId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string QType { get; set; }
        public string Status { get; set; }
        public string FYear { get; set; }
        public string EmpName { get; set; }
        public string Period { get; set; }
        public Nullable<bool> ReviewedByEmp { get; set; }
        public Nullable<bool> ReviewedByManager { get; set; }
        public Nullable<bool> Completed { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class ScreenshotsViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string Date { get; set; }
        public string EmpName { get; set; }
        public string msg { get; set; }
    }
}