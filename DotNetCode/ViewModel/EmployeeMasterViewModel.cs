using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TokenId { get; set; }
        public string AuthKey { get; set; }
        public int RoleId { get; set; }
    }
    public class LoginDetailsViewModel
    {
        public int? LoginId { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Mode { get; set; }
        public int? EmpId { get; set; }
    }
    public class FRViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public string AuthKey { get; set; }
        public int RoleId { get; set; }
        public int? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string msg { get; set; }
    }
    public class CPwdViewModel
    {
        public int? LoginId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public Nullable<bool> CPwd { get; set; }
        public Nullable<bool> Expired { get; set; }
        public string msg { get; set; }
    }
    public class PassHistoryManagementViewModel
    {
        public int? LoginId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public Nullable<bool> FPwd { get; set; }
        public Nullable<bool> CPwd { get; set; }
        public Nullable<bool> Expired { get; set; }
        public string msg { get; set; }
    }

    public class WFHLoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TokenId { get; set; }
        public string AuthKey { get; set; }
        public int RoleId { get; set; }
        public string IPAddress { get; set; }
    }
    public class CheckAuthViewModel
    {
        public string UserName { get; set; }
        public string TokenId { get; set; }
        public string AuthKey { get; set; }
    }
    public class DDCompanyViewModel
    {
        public int CompId { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public int EmpId { get; set; }
        //public string AuthorisedEntity { get; set; }
    }
    public class DDLegalEntityViewModel
    {
        public int LEId { get; set; }
        public int? CompId { get; set; }
        public string LegalEntity { get; set; }
        public int EmpId { get; set; }
        public string AuthorisedEntity { get; set; }
    }
    public class DDAuthorisedEntityViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public string AuthorisedEntity { get; set; }
        public int LEId { get; set; }
        public string LegalEntity { get; set; }
    }
    public class DDBusinessUnitViewModel
    {
        public int BUId { get; set; }
        public int? LEId { get; set; }
        public int? CompId { get; set; }
        public string BusinessUnit { get; set; }
        public int EmpId { get; set; }
        public string AuthorisedEntity { get; set; }
    }
    public class DDLocationViewModel
    {
        public int LocationId { get; set; }
        public int? BUId { get; set; }
        public int? LEId { get; set; }
        public int? CompId { get; set; }
        public string Location { get; set; }
        public int EmpId { get; set; }
        public string AuthorisedEntity { get; set; }
    }
    public class NewDDCompanyViewModel
    {
        public int CompId { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public int LoginId { get; set; }
    }
    public class NewDDLegalEntityViewModel
    {
        public int LEId { get; set; }
        public int? CompId { get; set; }
        public string LegalEntity { get; set; }
        public int LoginId { get; set; }
    }
    public class NewDDBusinessUnitViewModel
    {
        public int BUId { get; set; }
        public int? LEId { get; set; }
        public int? CompId { get; set; }
        public string BusinessUnit { get; set; }
        public int LoginId { get; set; }
    }
    public class NewDDLocationViewModel
    {
        public int LocationId { get; set; }
        public int? LEId { get; set; }
        public int? CompId { get; set; }
        public string Location { get; set; }
        public int LoginId { get; set; }
    }
    public class DDSaluationViewModel
    {
        public int SalutationId { get; set; }
        public string Salutation { get; set; }
        public int EmpId { get; set; }
    }
    public class DDGenderViewModel
    {
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public int EmpId { get; set; }
    }
    public class DDEmpTypeViewModel
    {
        public int EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public string Description { get; set; }
        public int EmpId { get; set; }
    }
    public class DDApproverViewModel
    {
        public int ApproverId { get; set; }
        public string Approver { get; set; }
        public int CompId { get; set; }
        public int LEId { get; set; }
        public int BUId { get; set; }
        public int LocationId { get; set; }
        public int EmpId { get; set; }
        public string AuthorisedEntity { get; set; }
    }
    public class EmployeeMasterViewModel
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
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> DeptId { get; set; }
        public string DeptName { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string Designation { get; set; }
        public Nullable<int> ReportId { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public string Approver { get; set; }
        public string ReportEmpCode { get; set; }
        public string ReportEmpName { get; set; }
        public Nullable<bool> Authorised { get; set; }
        public string EmpCode { get; set; }
        public string TokenId { get; set; }
        public string UserAuth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public Nullable<int> SalutationId { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EmpStatus { get; set; }
        public string Reason { get; set; }
        public string EmpType { get; set; }
        public Nullable<int> EmpTypeId { get; set; }
        public Nullable<System.DateTime> CEndDate { get; set; }
        public Nullable<bool> CPwd { get; set; }
        public int? OnSiteLogInId { get; set; }
        public Nullable<System.DateTime> OnSiteLogInDate { get; set; }
        public Nullable<System.DateTime> OnSiteLogOutDate { get; set; }
        public Nullable<System.TimeSpan> OnSiteLogInTime { get; set; }
        public Nullable<System.TimeSpan> OnSiteLogOutTime { get; set; }
        public string OnSiteStatus { get; set; }
        public string AuthorisedEntity { get; set; }
        public string RelievedReason { get; set; }
        public Nullable<System.DateTime> RelievedDate { get; set; }
        public Nullable<System.DateTime> RelievedEffectiveDate { get; set; }
        public Nullable<bool> IsRelieved { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsProbation { get; set; }
        public Nullable<bool> IsProbationConfirm { get; set; }
        public string ProbationConfirmationEffectiveDate { get; set; }
        public string ProbationConfirmationDate { get; set; }
        public string ProbationRemarks { get; set; }
        public string ProbationConfirmationStatus { get; set; }
        public string msg { get; set; }
    }
    public class EmployeeDetailViewModel
    {
        public int? LoginId { get; set; }
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string AMobileNo { get; set; }
        public string PMailId { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string HusbandName { get; set; }
        public string FContactNo { get; set; }
        public string MContactNo { get; set; }
        public string HContactNo { get; set; }
        public string EContactNo { get; set; }
        public string EContactName { get; set; }
        public string EContactRelationship { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public Nullable<System.DateTime> DateOfAnniversary { get; set; }
        public string Disability { get; set; }
        public string ECActivities { get; set; }
        public string Sports { get; set; }
        public string Caste { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Nationality { get; set; }
        public string TotalExperience { get; set; }
        public string RelevantExperience { get; set; }
        public string PermanentDoorNumber { get; set; }
        public string PermanentBuildingName { get; set; }
        public string PermanentStreet { get; set; }
        public string PermanentLocation { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentState { get; set; }
        public string PermanentCountry { get; set; }
        public string PermanentPinCode { get; set; }
        public string CurrentDoorNumber { get; set; }
        public string CurrentBuildingName { get; set; }
        public string CurrentStreet { get; set; }
        public string CurrentLocation { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string CurrentCountry { get; set; }
        public string CurrentPinCode { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
        public string EContactNo1 { get; set; }
        public string EContactName1 { get; set; }
        public string EContactRelationship1 { get; set; }

        public string EContactNo2 { get; set; }
        public string EContactName2 { get; set; }
        public string EContactRelationship2 { get; set; }


    }
    public class FileUploadAPIViewModel
    {
        //public int ImgID { get; set; }
        //public string? Customers { get; set; }
        public IFormFile files { get; set; }
        //public string ImgName { get; set; }
        public string DocName { get; set; }
        public string msg { get; set; }
        public string path { get; set; }
        public int EmpId { get; set; }
        public string Visitor { get; set; }
        public string ImageType { get; set; }
    }
    // Fro WFH 
    public class WFHFileUploadAPIViewModel
    {
        public IFormFile file { get; set; }
        public string msg { get; set; }
        public string path { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
    }
    public class DDDocViewModel
    {
        public int DocId { get; set; }
        public int? EduId { get; set; }
        public string DocName { get; set; }
        public int EmpId { get; set; }
    }
    public class DocumentMasterViewModel
    {
        public int DocId { get; set; }
        public Nullable<int> EduId { get; set; }
        public string DocName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? LoginId { get; set; }
        public Nullable<int> EmpId { get; set; }
    }
    public class EmployeeEducationViewModel
    {
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> DocId { get; set; }
        public string Others { get; set; }
        public string DocName { get; set; }
        public string School { get; set; }
        public string DegreeId { get; set; }
        public string Filed { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? LoginId { get; set; }
        public string msg { get; set; }
    }
    public class EmployeeGovtDocViewModel
    {
        public int GovId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> DocId { get; set; }
        public string DocName { get; set; }
        public string Others { get; set; }
        public string Name { get; set; }
        public string DocNo { get; set; }
        public string IssuedDate { get; set; }
        public string ExpiredDate { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? LoginId { get; set; }
        public string msg { get; set; }
    }
    public class EmployeeAccDetailViewModel
    {
        public int AccId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccHolderName { get; set; }
        public string AccNo { get; set; }
        public string PFNo { get; set; }
        public string ESIInsuranceNo { get; set; }
        public string HealthInsuranceNo { get; set; }
        public string PANNo { get; set; }
        public string UANNo { get; set; }
        public string AadharNo { get; set; }
        public string MobileNo { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? LoginId { get; set; }
        public string msg { get; set; }
    }
    public class EmployeeCareerDetailViewModel
    {
        public int CareerId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Experience { get; set; }
        public string PMonth1 { get; set; }
        public string PaySlip1 { get; set; }
        public string PMonth2 { get; set; }
        public string PaySlip2 { get; set; }
        public string PMonth3 { get; set; }
        public string PaySlip3 { get; set; }
        public string OfferLetter { get; set; }
        public string SalaryLetter { get; set; }
        public string ExperienceLetter { get; set; }
        public string RelievingLetter { get; set; }
        public string ContactName { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactMobile { get; set; }
        public string CTC { get; set; }
        public string Reason { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? LoginId { get; set; }
        public string msg { get; set; }
    }
    public class FetchEmployeeViewModel
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
        public string TokenId { get; set; }
        public string UserAuth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public Nullable<int> Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EmpStatus { get; set; }
        public string Reason { get; set; }
        public string EmpType { get; set; }
        public Nullable<int> EmpTypeId { get; set; }
        public Nullable<System.DateTime> CEndDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }

        //CI
        public int Id { get; set; }
        public string AMobileNo { get; set; }
        public string PMailId { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string HusbandName { get; set; }
        public string FContactNo { get; set; }
        public string MContactNo { get; set; }
        public string HContactNo { get; set; }
        public string EContactNo { get; set; }
        public string EContactName { get; set; }
        public string EContactRelationship { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public Nullable<System.DateTime> DateOfAnniversary { get; set; }
        public string Disability { get; set; }
        public string ECActivities { get; set; }
        public string Sports { get; set; }
        public string Caste { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Nationality { get; set; }
        public string TotalExperience { get; set; }
        public string RelevantExperience { get; set; }
        public string PermanentDoorNumber { get; set; }
        public string PermanentBuildingName { get; set; }
        public string PermanentStreet { get; set; }
        public string PermanentLocation { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentState { get; set; }
        public string PermanentCountry { get; set; }
        public string PermanentPinCode { get; set; }
        public string CurrentDoorNumber { get; set; }
        public string CurrentBuildingName { get; set; }
        public string CurrentStreet { get; set; }
        public string CurrentLocation { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string CurrentCountry { get; set; }
        public string CurrentPinCode { get; set; }

        //Acc
        public int AccId { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string AccHolderName { get; set; }
        public string AccNo { get; set; }
        public string PFNo { get; set; }
        public Nullable<bool> AccStatus { get; set; }

        //Edu Doc
        public List<EmployeeEducationViewModel> lstEmpEduDoc { get; set; }
        //Govt Doc
        public List<EmployeeGovtDocViewModel> lstEmpGovtDoc { get; set; }
        //Career Doc
        public List<EmployeeCareerDetailViewModel> lstEmpCareerDoc { get; set; }
    }
    public class WorkTypeMasterViewModel
    {
        public int? LoginId { get; set; }
        public int? WorkTypeId { get; set; }
        public string WorkType { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Reason { get; set; }
        public string ApproverDescription { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> IsApprovedBy { get; set; }
        public string Approver { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<int> IsRejectedBy { get; set; }
        public string RApprover { get; set; }
        public Nullable<bool> IsEnd { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastupdatedDate { get; set; }
        public string msg { get; set; }
        public string Status { get;  set; }
    }
    public class DDEmployeeViewModel
    {
        public int LoginId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
    }
    public class WorkTypeFilterViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
    }
    public class WFHLoginlogViewModel
    {
        public int WFHId { get; set; }
        public Nullable<int> LoginId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> LoginTime { get; set; }
        public Nullable<System.TimeSpan> LogOutTime { get; set; }
        public Nullable<System.TimeSpan> Activehrs { get; set; }
        public Nullable<System.TimeSpan> AnalysisHr { get; set; }
        public Nullable<bool> IsLoggedIn { get; set; }
        public Nullable<bool> IsLoggedOut { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int? CompId { get; set; }
        public string CompName { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string msg { get; set; }
    }
    public class WFHLoginlogFilterViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public int? CompId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public string EmpCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class AttendaceDateViewModel
    {
        public string AttendaceDate { get; set; }
        //public decimal? PayDays { get; set; }
        public List<AttendanceViewModel> lstofAttendance { get; set; }
    }

    //public class AttendanceViewModel
    //{
    //    public int? EmpId { get; set; }
    //    public int? CompId { get; set; }
    //    public string CompName { get; set; }
    //    public string Designation { get; set; }
    //    public string DeptName { get; set; }
    //    public Nullable<int> DeptId { get; set; }
    //    public int? DesignationId { get; set; }
    //    public string EmpCode { get; set; }
    //    public string EmpName { get; set; }
    //    public string WorkType { get; set; }
    //    public DateTime LogDate { get; set; }
    //    public string WorkingHours { get; set; }
    //    public string LogInTime { get; set; }
    //    public string LogOutTime { get; set; }
    //    public bool IsWorkFromHome { get; set; }
    //    public WFHLoginlogViewModel WFHDetails { get; internal set; }
    //}

    //public class AttendanceFilterViewModel
    //{
    //    public int? EmpId { get; set; }
    //    public int? LoginId { get; set; }
    //    public int? CompId { get; set; }
    //    public Nullable<int> DeptId { get; set; }
    //    public int? DesignationId { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string EmpCode { get; set; }
    //    public string Empname { get; set; }
    //    public string WorkType { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public TimeSpan? WorkingHours { get; set; }
    //}

    //public class AttendanceViewModel
    //{
    //    //public WFHLoginlogViewModel WFHDetails { get; set; }
    //    public string WFHDetails { get; set; }
    //    public int? EmpId { get; set; }
    //    public int? CompId { get; set; }
    //    public string CompName { get; set; }
    //    public string Designation { get; set; }
    //    public string DeptName { get; set; }
    //    public Nullable<int> DeptId { get; set; }
    //    public int? DesignationId { get; set; }
    //    public string EmpCode { get; set; }
    //    public string EmpName { get; set; }
    //    public DateTime LogDate { get; set; }
    //    public string WorkingHours { get; set; }
    //    public string LogInTime { get; set; }
    //    public string LogOutTime { get; set; }
    //    public bool IsWorkFromHome { get; set; }
    //    public string ActiveHours { get; set; }
    //    public string OnSiteDetails { get; set; }
    //}

    public class TimeInterval
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }

    public class AttendanceViewModel
    {
        //public WFHLoginlogViewModel WFHDetails { get; set; }
        public string WFHDetails { get; set; }
        public int? EmpId { get; set; }
        public int? CompId { get; set; }
        public string CompName { get; set; }
        public string Designation { get; set; }
        public string DeptName { get; set; }
        public Nullable<int> DeptId { get; set; }
        public int? DesignationId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime LogDate { get; set; }
        public string WorkingHours { get; set; }
        public string LogInTime { get; set; }
        public string LogOutTime { get; set; }
        public string ESSLLogInTime { get; set; }
        public string ESSLLogOutTime { get; set; }
        public string WFHLogInTime { get; set; }
        public string WFHLogOutTime { get; set; }
        public string ONSITELogInTime { get; set; }
        public string ONSITELogOutTime { get; set; }
        public string LoginLocation { get; set; }
        public string LogoutLocation { get; set; }
        // public bool IsWorkFromHome { get; set; }
        public string ActiveHours { get; set; }
        public string ESSLActiveHours { get; set; }
        public string WFHActiveHours { get; set; }
        public string ONSITEActiveHours { get; set; }
        // public string OnSiteDetails { get; set; }
        //public int ShiftId { get; set; }  
        public string ShiftName { get; set; }
        public string LeaveType { get; set; }
        public string BreakTime { get; set; }
        public string WorkType { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
        public decimal? PayDays { get; set; }
        public decimal? clelcount { get; set; }
        public decimal? holirhcount { get; set; }
        public decimal? weekendcount { get; set; }
        public decimal? weekendcount1 { get; set; }
        public decimal? dojsundayCount { get; set; }
        public decimal? dojweekendDaysCount { get; set; }
        public decimal? totalpaydaycount { get; set; }
        public decimal? lopcount { get; set; }
        public int? DaysPresent { get; set; }
        public string SalType { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginatedResponse()
        {
            Data = new List<T>();
        }
    }

    public class LoginLogViewModel
    {
        public Nullable<int> LoginId { get; set; }
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string LoginAddress { get; set; }
        public string LoginCity { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public string LoginLongitude { get; set; }
        public string LoginLatitude { get; set; }
        public Nullable<System.TimeSpan> LogInTime { get; set; }
        public string LogoutAddress { get; set; }
        public string LogoutCity { get; set; }
        public Nullable<System.DateTime> LogoutDate { get; set; }
        public string LogoutLongitude { get; set; }
        public string LogoutLatitude { get; set; }
        public Nullable<System.TimeSpan> LogOutTime { get; set; }
        public string ActionType { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class LoginlogViewModel
    {
        public int? LoginId { get; set; }
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string LoginAddress { get; set; }
        public string LoginCity { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public string LoginLongitude { get; set; }
        public string LoginLatitude { get; set; }
        public Nullable<System.TimeSpan> LogInTime { get; set; }
        public string LogoutAddress { get; set; }
        public string LogoutCity { get; set; }
        public Nullable<System.DateTime> LogoutDate { get; set; }
        public string LogoutLongitude { get; set; }
        public string LogoutLatitude { get; set; }
        public Nullable<System.TimeSpan> LogOutTime { get; set; }
        public string ActionType { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }
    public class OnSiteDataViewModel
    {
        public Nullable<int> LoginId { get; set; }
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string Company { get; set; }
        public string LoginAddress { get; set; }
        public string LoginCity { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public string LoginLongitude { get; set; }
        public string LoginLatitude { get; set; }
        public string Purpose { get; set; }
        public string Description { get; set; }
        public Nullable<System.TimeSpan> LogInTime { get; set; }
        public string LogoutAddress { get; set; }
        public string LogoutCity { get; set; }
        public Nullable<System.DateTime> LogoutDate { get; set; }
        public string LogoutLongitude { get; set; }
        public string LogoutLatitude { get; set; }
        public Nullable<System.TimeSpan> LogOutTime { get; set; }
        public string WorkStatus { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
    }

    public class SelectEmployeeViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public Nullable<int> CompId { get; set; }
        public string Company { get; set; }

        public string DeptName { get; set; }
        public Nullable<int> ReportId { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string EmpName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string msg { get; set; }
        public DateTime StartDate { get; set; }
        public int TotalEmployeeCount { get; set; }
    }

    public class TotalEmployeeViewModel
    {
        public int? LoginId { get; set; }
        public int TotalEmployeeCount { get; set; }
    }

    public class AttendanceSourceViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LoginId { get; set; }
        public int DeviceCheckIns { get; set; }
        public int AppCheckIns { get; set; }
        public int OnSiteCheckIns { get; set; }
    }
    public class ddLocationViewModel
    {
        public int LocationId { get; set; }
        public string Location { get; set; }
        public int EmpId { get; set; }

    }
    public class DDSelectEmpViewModel
    {
        public int EmpId { get; set; }
        public int LocationId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
    }
    public class ConsolidatedAttendanceSummaryViewModel
    {
        public int? LoginId { get; set; }
        public double TotalWorkedHours { get; set; }
        public double MaxWorkingHours { get; set; }
        public int OfficeCount { get; set; }
        public int WorkFromHomeCount { get; set; }
        public int OnSiteCount { get; set; }
    }

    //public class AttendanceFilterViewModel
    //{
    //    public string WFHDetails { get; set; }
    //    public string OnSiteDetails { get; set; }

    //    public int? EmpId { get; set; }
    //    public int? LocationId { get; set; }

    //    public int? LoginId { get; set; }
    //    public int? CompId { get; set; }
    //    public Nullable<int> DeptId { get; set; }
    //    public int? DesignationId { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string EmpCode { get; set; }
    //    public string Empname { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public TimeSpan? WorkingHours { get; set; }
    //    public bool IsOverall { get; set; }
    //}
    public class DailyAttendanceSummaryViewModel
    {
        public string Date { get; set; }
        public string EmpCode { get; set; }
        public int OnTimeCheckInCount { get; set; }
        public int LateCheckInCount { get; set; }
        public int TotalEmployeeCount { get; set; }
        public string Location { get; internal set; }
    }

    public class OnTimeCheckInViewModel
    {
        public int? LoginId { get; set; }
        public DateTime? StartDate { get; set; }
        public string EmpCode { get; set; }
        public DateTime? EndDate { get; set; }
        public int OnTimeCheckInCount { get; set; }
        public int LateCheckInCount { get; set; }
        public string EmployeeName { get; set; }
        public int? LocationId { get; set; }
    }
    public class LogActivityResponseViewModel
    {
        public string Message { get; set; }
        public string EmpCode { get; set; }
        public string Action { get; set; }
        public string LogInTime { get; set; }
        public string LogOutTime { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }

    public class LogActivityViewModel
    {
        public string EmpCode { get; set; }
        public string Action { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string CreatedBy { get; set; }
    }
    public class AttendanceFilterViewModel
    {
        public string WFHDetails { get; set; }
        public string OnSiteDetails { get; set; }

        public int? EmpId { get; set; }
        //public int? LocationId { get; set; }

        public int? LoginId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocId { get; set; }

        public Nullable<int> DeptId { get; set; }
        public int? DesignationId { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Empname { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public TimeSpan? WorkingHours { get; set; }
        public bool IsOverall { get; set; }
        public List<EmployeeDetailsViewModel> Device { get; set; }
        public List<EmployeeDetailsViewModel> Site { get; set; }
        public List<EmployeeDetailsViewModel> WorkFromHome { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<int> LocationId { get; set; }

    }

    public class EmployeeDetailsViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public string DeptName { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string Date { get; set; }
    }


    public class ShiftViewModel
    {
        public int CompanyId { get; set; }
        public int BusinessUnitId { get; set; }
        public int LegalEntityId { get; set; }
        public int LocationId { get; set; }

        public TimeSpan GeneralShiftStart { get; set; }
        public TimeSpan GeneralShiftEnd { get; set; }
        public int MinWorkHoursDay { get; set; }
        public int MinWorkHoursWeek { get; set; }
        public int GraceTimeMinutes { get; set; }

        public int PayDay { get; set; }
        public int HalfDayLossHours { get; set; }
        public bool OvertimeAllowed { get; set; }
        public decimal SalaryDeductions { get; set; }

        public int CreatedBy { get; set; }
        public string msg { get; set; }
        public List<ShiftViewModel> ShiftList { get; set; }
        public int UpdatedBy { get; set; }
        public int SettingId { get; set; }
        public string Shift { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public int CompWrkHrs { get; set; }
    }

    public class WorkHoursViewModel
    {
        public int EmpId { get; set; }
        public string msg { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<WorkHoursDetail> WorkHoursData { get; set; }
        public string Date { get; set; }
        public double WorkHours { get; set; }
        public int HalfDayLossCount { get; set; }
        public string HalfDayLossReason { get; set; }
        public TimeSpan TotalWorkHours { get; set; }
        public TimeSpan RequiredDailyHours { get; set; }
        public TimeSpan RequiredWeeklyHours { get; set; }
        public string IsCompliant { get; set; }
        public string Reason { get; set; }
        public bool IsHalfDay { get; set; }
    }


    public class WorkHoursDetail
    {
        public string Date { get; set; }
        public double WorkHours { get; set; }
    }
    public class AccessControlViewModel
    {
        public int? AccessId { get; set; }
        public string AccessName { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public int? PageModuleId { get; set; }
        public string PageName { get; set; }
        public bool? AddAccess { get; set; }
        public bool? UpdateAccess { get; set; }
        public bool? DeleteAccess { get; set; }
        public bool? ViewAccess { get; set; }
        public int EmpId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string msg { get; set; }
    }

    public class CompanySettingViewModel
    {
        public string CompanyName { get; set; }
        public string BusinessUnitName { get; set; }
        public string LEName { get; set; }
        public string LocationName { get; set; }
        public int CompanyId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? LEId { get; set; }
        public int? LocationId { get; set; }
        public string Shift { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }
        public int? MinWorkHoursDay { get; set; }
        public int? MinWorkHoursWeek { get; set; }
        public int? PayDay { get; set; }
        public int? HalfDayLossHours { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> CompWrkHrs { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsUpdated { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int? ShiftId { get; set; }
        public string WeeklyHolidays { get; set; }
        public Nullable<int> WorkingHoursPerDay { get; set; }
        public Nullable<int> WeeklyWorkingHours { get; set; }
    }
    public class EmpHolidayListViewModel
    {
        public int LoginId { get; set; }
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
    public class HolidayViewModel
    {
        public List<int> Holiday_Id { get; set; }
        public string Title { get; set; }
        public List<string> Day { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; }
        public int Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public Nullable<int> Modify_By { get; set; }
        public Nullable<System.DateTime> Modify_Date { get; set; }
        public string Status { get; set; }
        public string msg { get; set; }
        public int LoginId { get; set; }
        public List<int> LocationId { get; set; }
        public List<int> HolidayLocationId { get; set; }
        public string HolidayType { get; set; }
        public int Year { get; set; }
        public List<string> Location { get; set; }
        public List<string> HolidayLocation { get; set; }
        public List<HolidayViewModel> UpdatedHolidays { get; set; }
    }
    public class WeekHolidayViewModel
    {
        public int LoginId { get; set; }
        public List<int> WeekDay_ID { get; set; }
        //public int WeekDay_ID { get; set; }
        //public string Day { get; set; }
        public List<string> Day { get; set; }
        public int Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        // public int LocationId { get; set; }
        public List<int> LocationId { get; set; }
        public string msg { get; set; }
        public string Title { get; set; }
        // public string Location { get; set; }
        public List<string> Location { get; set; }
        public string Description { get; set; }
        public List<WeekHolidayViewModel> UpdatedWeekHolidays { get; set; }
    }
    public class FinanceMasterViewModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
    }
    public class ProjectConnectUserViewModel
    {
        public Nullable<int> LoginId { get; set; }
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> IsTransffered { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class responseViewModel
    {
        public int status { get; set; }
        public string msg { get; set; }
    }
    public class AttWFHLoginlogViewModel
    {
        public int WFHId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public TimeSpan? LoginTime { get; set; }
        public TimeSpan? LogOutTime { get; set; }
        public TimeSpan? Activehrs { get; set; }
        public TimeSpan? AnalysisHr { get; set; }
        public Nullable<bool> IsLoggedIn { get; set; }
        public Nullable<bool> IsLoggedOut { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class AttLoginlogViewModel
    {
        public int Id { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string LoginAddress { get; set; }
        public string LoginCity { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public string LoginLongitude { get; set; }
        public string LoginLatitude { get; set; }
        public TimeSpan? LogInTime { get; set; }
        public string LogoutAddress { get; set; }
        public string LogoutCity { get; set; }
        public Nullable<System.DateTime> LogoutDate { get; set; }
        public string LogoutLongitude { get; set; }
        public string LogoutLatitude { get; set; }
        public TimeSpan? LogOutTime { get; set; }
        public string ActionType { get; set; }
        public TimeSpan? ActiveHrs { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class AttendanceUploadModel
    {
        public int LoginId { get; set; }
        public int EmpId { get; set; }

        // Optional – user-defined or original file name
        public string FileName { get; set; }

        // Actual uploaded file
        public HttpPostedFileBase File { get; set; }
    }
    public class UploadResult
    {
        public int TotalRecords { get; set; }
        public int InsertedRecords { get; set; }
        public int FailedRecords { get; set; }
        public List<AttendanceException> Exceptions { get; set; }
    }
    public class AttendanceException
    {
        public string EmpCode { get; set; }      // Employee code from Excel
        public string Date { get; set; }         // Date from Excel (string or DateTime)
        public string Time { get; set; }         // Worked hours / time from Excel
        public string Reason { get; set; }       // Why this row failed (e.g., "Invalid EmpCode", "Duplicate Attendance", "Invalid Date/Time")
    }
    public class UploadAttendanceSingleViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public string EmpCode { get; set; }      // Employee code from Excel
        public string Date { get; set; }         // Date from Excel (string or DateTime)
        public string Time { get; set; }         // Worked hours / time from Excel
        public string Status { get; set; }       // Why this row failed (e.g., "Invalid EmpCode", "Duplicate Attendance", "Invalid Date/Time")
    }
    public class ManualAttendanceViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string FullName { get; set; }
        public int? CompId { get; set; }
        public string Company { get; set; }
        public int? LEId { get; set; }
        public string LegalEntity { get; set; }
        public int? BUId { get; set; }
        public string BusinessUnit { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string WorkedHrs { get; set; }
        public string Status { get; set; }
    }
    public class DDEmpListViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
    }
    public class SPAttendanceViewModel
    {
        public int? LoginId { get; set; }
        public int? EmpId { get; set; }
        public string Date { get; set; }
        public string msg { get; set; }
    }
    // Create these classes at the top of your file or in a separate file
    public class EmployeeInfo
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public int? CategoryId { get; set; }
        public int? DesignationId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string CompName { get; set; }
        public string DesignationName { get; set; }
        public string DeptName { get; set; }
    }

    public class LeaveInfo
    {
        public int EmpId { get; set; }
        public int? LeaveTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Duration { get; set; }
    }
    public class AttendaceDeptReportViewModel
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public List<DepartmentAttendanceViewModel> lstofDept { get; set; } = new List<DepartmentAttendanceViewModel>();
    }
    // Helper class for stored procedure result
    public class DepartmentAttendanceResult
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public string Total { get; set; }  // Change from int to string
        public string OverAllAbsentPercentage { get; set; }
        public string Present { get; set; }  // Change from int to string
        public string Absent { get; set; }   // Change from int to string
        public string Leave { get; set; }    // Change from int to string
        public string AbsentPesent { get; set; }
        public string IsHoliday { get; set; }  // Change from bool to string
    }

    public class DepartmentAttendanceViewModel
    {
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public int Total { get; set; }
        public string OverAllAbsentPercentage { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Leave { get; set; }
        public string AbsentPesent { get; set; }
        public bool IsHoliday { get; set; }
    }
    // Helper class that matches the exact SQL result types
    public class DepartmentAttendanceRawResult
    {
        public string Date { get; set; }                  // VARCHAR/NVARCHAR
        public string Day { get; set; }                   // VARCHAR/NVARCHAR  
        public string DeptName { get; set; }              // VARCHAR/NVARCHAR
        public string DeptShortName { get; set; }         // VARCHAR/NVARCHAR
        public string Total { get; set; }                    // INT
        public string OverAllAbsentPercentage { get; set; } // DECIMAL
        public string Present { get; set; }                  // INT
        public string Absent { get; set; }                   // INT
        public string Leave { get; set; }                    // INT
        public string AbsentPesent { get; set; }         // DECIMAL
        public bool IsHoliday { get; set; }               // BIT
    }
    public class ContractViewModel
    {
        public int? LoginId { get; set; }
        public string MobileNo { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public int? ProjectId { get; set; }
        public int? VendorId { get; set; }
        public string Status { get; set; }

    }
    public class ContractApprovedViewModel
    {
        public int? LoginId { get; set; }
        public List<ContractAttendanceViewModel> lstofCantractIId { get; set; }
    }
    public class ContractAttendanceViewModel
    {
        public int? LoginId { get; set; }
        public int CId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Mobile { get; set; }
        public string Mail { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Skill { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<int> ERPVendorId { get; set; }
        public string Vendor { get; set; }
        public string VendorCode { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> ERPProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string Project { get; set; }
        public Nullable<int> SiteId { get; set; }
        public string Site { get; set; }
        public string SiteDetails { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string ManagerEmpCode { get; set; }
        public string ManagerName { get; set; }
        public Nullable<bool> Status { get; set; }
        public string LoginStatus { get; set; }
        public Nullable<bool> IsLogin { get; set; }
        public Nullable<bool> IsLogout { get; set; }
        public Nullable<System.TimeSpan> LoginTime { get; set; }
        public Nullable<System.TimeSpan> LogoutTime { get; set; }
        public Nullable<System.TimeSpan> Activehrs { get; set; }
        public Nullable<System.TimeSpan> Approvedhrs { get; set; }
        public string LoginAddress { get; set; }
        public string LoginLonqitude { get; set; }
        public string LoginLatitude { get; set; }
        public string LogoutAddress { get; set; }
        public string LogoutLonqitude { get; set; }
        public string LogoutLatitude { get; set; }
        public string Description { get; set; }
        public string ManPowerApproval { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsLogoutManager { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class DDVendorListViewModel
    {
        public int? LoginId { get; set; }
        public int? VendorId { get; set; }
        public int? ERPVendorId { get; set; }
        public string VendorCode { get; set; }
        public string Vendor { get; set; }
    }
    public class DDSiteListViewModel
    {
        public int? LoginId { get; set; }
        public int? SiteId { get; set; }
        public string Site { get; set; }
    }
    public class DDProjectListViewModel
    {
        public int? LoginId { get; set; }
        public int? ProjectId { get; set; }
        public int? ERPProjectId { get; set; }
        public string Project { get; set; }
        public string ProjectCode { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerCode { get; set; }
        public string ManagerName { get; set; }
        public int? SiteId { get; set; }
        public string Site { get; set; }
    }
    public class VendorListViewModel
    {
        public int? LoginId { get; set; }
        public int? VendorId { get; set; }
        public int? ERPVendorId { get; set; }
        public string VendorCode { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class ProjectListViewModel
    {
        public int? LoginId { get; set; }
        public int? ProjectId { get; set; }
        public int? ERPProjectId { get; set; }
        public string Project { get; set; }
        public string ProjectCode { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerCode { get; set; }
        public string ManagerName { get; set; }
        public int? SiteId { get; set; }
        public string Site { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class ProjectMasterViewModel
    {
        public int ProjectId { get; set; }
        public int? ERPProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public Nullable<int> SiteId { get; set; }
        public string Site { get; set; }
        public Nullable<int> ProjectManagerId { get; set; }
        public string ManagerCode { get; set; }
        public string ManagerName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class EmpProbationTrackingHistoryViewModel
    {
        public int? LoginId { get; set; }
        public int? LEId { get; set; }
        public int? BuId { get; set; }
        public int? LocId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public int? ReporterId { get; set; }
        public int EmpProbationId { get; set; } 
        public Nullable<int> EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<int> ProbationDays { get; set; }
        public Nullable<System.DateTime> ProbationEndDate { get; set; }
        public Nullable<int> ReportId { get; set; }
        public string ReportCode { get; set; }
        public Nullable<bool> IsProbation { get; set; }
        public Nullable<bool> IsPermanent { get; set; }
        public Nullable<bool> IsContract { get; set; }
        public Nullable<bool> IsConsultant { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public Nullable<int> ConfirmBy { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastupdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class EmpProbationTrackingHistoryListViewModel
    {
        public List<EmpProbationTrackingHistoryViewModel> ProbationHistoryList { get; set; }
        public List<EmpProbationTrackingHistoryViewModel> PendingProbationList { get; set; }
    }
    public class DDReporterListViewModel
    {
        public int ReporterId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public int? EmpId { get; set; }
        public string ReporterName { get; set; }
        public string ReporterCode { get; set; }
        public int? LoginId { get; set; }
    }
    public class DDEmployeeListViewModel
    {
        public int EmpId { get; set; }
        public int? ReporterId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public int? LoginId { get; set; }
    }
    public class EmployeeMasterLogViewModel
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
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> DeptId { get; set; }
        public string DeptName { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string Designation { get; set; }
        public Nullable<int> ReportId { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public string Approver { get; set; }
        public string ReportEmpCode { get; set; }
        public string ReportEmpName { get; set; }
        public Nullable<bool> Authorised { get; set; }
        public string EmpCode { get; set; }
        public string TokenId { get; set; }
        public string UserAuth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public Nullable<int> SalutationId { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EmpStatus { get; set; }
        public string Reason { get; set; }
        public string EmpType { get; set; }
        public Nullable<int> EmpTypeId { get; set; }
        public Nullable<System.DateTime> CEndDate { get; set; }
        public Nullable<bool> CPwd { get; set; }
        public int? OnSiteLogInId { get; set; }
        public Nullable<System.DateTime> OnSiteLogInDate { get; set; }
        public Nullable<System.DateTime> OnSiteLogOutDate { get; set; }
        public Nullable<System.TimeSpan> OnSiteLogInTime { get; set; }
        public Nullable<System.TimeSpan> OnSiteLogOutTime { get; set; }
        public string OnSiteStatus { get; set; }
        public string AuthorisedEntity { get; set; }
        public string RelievedReason { get; set; }
        public Nullable<System.DateTime> RelievedDate { get; set; }
        public Nullable<System.DateTime> RelievedEffectiveDate { get; set; }
        public Nullable<bool> IsRelieved { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsProbation { get; set; }
        public Nullable<bool> IsProbationConfirm { get; set; }
        public string ProbationConfirmationEffectiveDate { get; set; }
        public string ProbationConfirmationDate { get; set; }
        public string ProbationRemarks { get; set; }
        public string ProbationConfirmationStatus { get; set; }
        public string msg { get; set; }
    }
    public class HierarchyRequestViewModel
    {
        public int? EmpId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public int? ReporterId { get; set; }
        public int? GradeId { get; set; }
        public bool IncludeInactive { get; set; } = false;
    }

    public class HierarchyResponseViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public string EmployeeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int? GradeId { get; set; }
        public string GradeName { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public int? ReporterId { get; set; }
        public string ReporterName { get; set; }
        public int? EmpType { get; set; }
        public string EmpStatus { get; set; }
        public int HierarchyLevel { get; set; }
        public int ReporteesCount { get; set; }
        public List<HierarchyResponseViewModel> Reportees { get; set; }
    }

    public class HierarchyFinalResponse
    {
        public List<HierarchyResponseViewModel> Hierarchy { get; set; }
        public HierarchySummary Summary { get; set; }
        public DateTime GeneratedOn { get; set; }
    }

    public class HierarchySummary
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalDesignations { get; set; }
        public int TotalGrades { get; set; }
        public Dictionary<string, int> EmployeesByGrade { get; set; }
        public Dictionary<string, int> EmployeesByDepartment { get; set; }
        public Dictionary<string, int> EmployeesByDesignation { get; set; }
        public Dictionary<string, int> EmployeesByHierarchyLevel { get; set; }  // Changed to string key
    }
}