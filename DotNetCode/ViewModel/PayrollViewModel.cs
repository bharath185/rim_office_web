using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class PayrollResponseViewModel
    {
        public int Status { get; set; }
        public string msg { get; set; }
    }
    public class PayrollPayoutTypeViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public string Frequency { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class DDPayrollPayoutTypeViewModel
    {
        public int PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public string Frequency { get; set; }
    }
    public class PayrollSegmentViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public Nullable<int> PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class DDPayrollSegmentViewModel
    {
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
    }
    ////public class PayrollALLComponentViewModel
    ////{
    ////    public int EmpId { get; set; }
    ////    public int LoginId { get; set; }
    ////    public string Year { get; set; }
    ////    public string Month { get; set; }
    ////    public int MonthNo { get; set; }
    ////    public int ComponentId { get; set; }
    ////    public string ComponentName { get; set; }
    ////    public string ComponentCode { get; set; }
    ////    public string ComponentValue { get; set; }
    ////    public Nullable<int> PayoutTypeId { get; set; }
    ////    public string PayoutTypeName { get; set; }
    ////    public Nullable<int> FrequencyId { get; set; }
    ////    public string Frequency { get; set; }
    ////    public Nullable<int> SegmentId { get; set; }
    ////    public string SegmentName { get; set; }
    ////    public int ConditionId { get; set; }
    ////    public string ConditionExpression { get; set; }
    ////    public Nullable<bool> ConditionResultPFESI { get; set; }
    ////    public int LogicId { get; set; }
    ////    public Nullable<decimal> Percentage { get; set; }
    ////    public Nullable<decimal> Value { get; set; }
    ////    public Nullable<int> ComponentId1 { get; set; }
    ////    public string ComponentName1 { get; set; }
    ////    public Nullable<System.DateTime> EffectiveFrom { get; set; }
    ////    public Nullable<System.DateTime> EffectiveTo { get; set; }
    ////    public Nullable<int> CreatedBy { get; set; }
    ////    public Nullable<System.DateTime> CreatedDate { get; set; }
    ////    public Nullable<int> LastUpdatedBy { get; set; }
    ////    public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    ////    public Nullable<bool> IsActive { get; set; }
    ////    public Nullable<bool> IsUpdated { get; set; }
    ////    public Nullable<bool> IsDeleted { get; set; }
    ////}
    public class PayrollALLComponentViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int MonthNo { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentValue { get; set; }
        public Nullable<int> PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public Nullable<int> FrequencyId { get; set; }
        public string Frequency { get; set; }
        public Nullable<int> SegmentId { get; set; }
        public string SegmentName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public List<PayrollALLComponentLogicConditionViewModel> lstofLC { get; set; }
    }
    public class PayrollALLComponentLogicConditionViewModel
    {
        public int ComponentId { get; set; }
        public int LogicId { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> ComponentId1 { get; set; }
        public string ComponentName1 { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public int ConditionId { get; set; }
        public string ConditionExpression { get; set; }
        public Nullable<bool> ConditionResultPFESI { get; set; }
    }
    public class PayrollALLFULLComponentCompactViewModel
    {
        public List<PayrollALLComponentCompactViewModel> lstofComponentDetails { get; set; }
        public List<PayrollALLComponentCompactViewModel> lstofArrearComponentDetails { get; set; }
    }
    public class PayrollALLComponentCompactViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int LoginId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentValue { get; set; }
        public Nullable<int> PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public Nullable<int> FrequencyId { get; set; }
        public string Frequency { get; set; }
        public Nullable<int> SegmentId { get; set; }
        public string SegmentName { get; set; }
        public int ConditionId { get; set; }
        public string ConditionExpression { get; set; }
        public Nullable<bool> ConditionResultPFESI { get; set; }
        public int LogicId { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> ComponentId1 { get; set; }
        public string ComponentName1 { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public int LCtrue { get; set; }
    }
    public class DDPayrollComponentViewModel
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
    }
    public class DDPayrollEmpListViewModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
    }
    public class PayrollComponentViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public Nullable<int> PayoutTypeId { get; set; }
        public Nullable<int> SegmentId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class PayrollComponentLogicViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int LogicId { get; set; }
        public Nullable<int> ComponentId { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> ComponentId1 { get; set; }
        public string ComponentName { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class PayrollComponentConditionViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int ConditionId { get; set; }
        public Nullable<int> ComponentId { get; set; }
        public string ConditionExpression { get; set; }
        public Nullable<bool> ConditionResultPFESI { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class PayrollSymbolMasterViewModel
    {
        public int SymbolId { get; set; }
        public string Symbol { get; set; }
    }
    public class PayrollFrequencyMasterViewModel
    {
        public int FrequencyId { get; set; }
        public string Frequency { get; set; }
    }
    public class PayrolAccessViewModel
    {
        public int LoginId { get; set; }
    }
    public class PayrollResponseModel
    {
        public int PayoutId { get; set; }
        public string PayoutName { get; set; }
        public List<SegmentResponseModel> Segments { get; set; }
    }

    public class SegmentResponseModel
    {
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public List<ComponentResponseModel> Components { get; set; }
    }

    public class ComponentResponseModel
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentValue { get; set; }
        public List<LogicConditionResponseModel> LogicConditions { get; set; }
    }
    public class LogicConditionResponseModel
    {
        public int ComponentId { get; set; }
        public int LogicId { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> ComponentId1 { get; set; }
        public string ComponentName1 { get; set; }
        public int ConditionId { get; set; }
        public string ConditionExpression { get; set; }
        public Nullable<bool> ConditionResultPFESI { get; set; }
    }
    public class DDPayslipSectionViewModel
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
    }
    public class PayslipSectionViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public Nullable<int> SequenceNo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class PayslipSectionComponentViewModel
    {
        public int EmpId { get; set; }
        public int LoginId { get; set; }
        public int SectionComponentId { get; set; }
        public Nullable<int> PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string SectionName { get; set; }
        public Nullable<int> ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public Nullable<int> SequenceNo { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<bool> RecordStatus { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public partial class EmployeeSalaryDetailViewModel
    {
        public int LoginId { get; set; }
        public int? EmpId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public int? ReportId { get; set; }
        public int SalaryId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmpCode { get; set; }
        public Nullable<decimal> CTC { get; set; }
        public Nullable<decimal> MCTC { get; set; }
        public Nullable<decimal> PerviousCTC { get; set; }
        public Nullable<decimal> IncrementPercent { get; set; }
        public Nullable<System.DateTime> EffectiveFromDate { get; set; }
        public Nullable<System.DateTime> EffectiveToDate { get; set; }
        public Nullable<bool> IsAppraised { get; set; }
        public Nullable<bool> RecordStatus { get; set; }
        public Nullable<bool> IsFixed { get; set; }
        public Nullable<bool> IsVariable { get; set; }
        public Nullable<int> Period { get; set; }
        public Nullable<int> VariableId { get; set; }
        public string VariableName { get; set; }
        public string VariableCode { get; set; }
        public string VariableAmt { get; set; }
        public Nullable<bool> IsArrear { get; set; }
        public string ArrearAmt { get; set; }
        public Nullable<bool> IsClearArrear { get; set; }
        public Nullable<int> PendingMonth { get; set; }
        public Nullable<int> ArrearYear { get; set; }
        public Nullable<int> ArrearMonth { get; set; }
        public string DescriptionforArrear { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class PayoutRequestViewModel
    {
        public int PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public List<SectionRequestViewModel> Sections { get; set; }
    }

    public class SectionRequestViewModel
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public List<ComponentRequestViewModel> Components { get; set; }
    }

    public class ComponentRequestViewModel
    {
        public Nullable<int> SectionComponentId { get; set; }
        public Nullable<int> ComponentId { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentName { get; set; }
        public Nullable<int> SequenceNo { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<bool> RecordStatus { get; set; }
    }
    public class PayslipPayloadRequest
    {
        public int PayoutTypeId { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
        public int LoginId { get; set; }
        public List<PayslipSectionRequest> Sections { get; set; }
    }
    public class PayslipSectionRequest
    {
        public string SectionName { get; set; }
        public List<PayslipComponentRequest> Components { get; set; }
    }
    public class PayslipComponentRequest
    {
        public int ComponentId { get; set; }
        public int SequenceNo { get; set; }
    }
    public class UpdatePayslipPayload
    {
        public int PayoutTypeId { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
        public int LoginId { get; set; }
        public List<UpdateSectionRequest> Sections { get; set; }
    }

    public class UpdateSectionRequest
    {
        public string SectionName { get; set; }
        public List<UpdateComponentRequest> Components { get; set; }
    }

    public class UpdateComponentRequest
    {
        public int SectionComponentId { get; set; }
        public int ComponentId { get; set; }
        public int SequenceNo { get; set; }
    }
    public class DeletePayslipPayload
    {
        public int PayoutTypeId { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
        public int LoginId { get; set; }
        public List<DeleteSectionRequest> Sections { get; set; }
    }

    public class DeleteSectionRequest
    {
        public string SectionName { get; set; }
        public List<DeleteComponentRequest> Components { get; set; }
    }

    public class DeleteComponentRequest
    {
        public int SectionComponentId { get; set; }
    }
    public class PayslipRequestViewModel
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public int MonthNo { get; set; }
        public int LoginId { get; set; }
        public string EmpCode { get; set; }
    }
    public class PayslipResponseViewModel
    {
        public CompanyInfoViewModel Company { get; set; }
        public string SalaryMonth { get; set; }
        public EmployeeInfoDetailsViewModel EmployeeDetails { get; set; }
        public List<SectionResponseViewModel>  PayslipSections { get; set; }
        public List<SectionResponseViewModel> ArrearSections { get; set; }
        public List<PayrollALLComponentCompactViewModel> VariableSections { get; set; }
        public string DescriptionforArrear { get; set; }
    }
    public class CompanyInfoViewModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhoneNo { get; set; }
        public string CompanyFax { get; set; }
        public string CompanyEmail { get; set; }
        //public string LogoUrl { get; set; }
    }
    public class EmployeeInfoDetailsViewModel
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string EmpCode { get; set; }
        public string Location { get; set; }
        public string PanNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string BankAccNo { get; set; }
        public string PFNo { get; set; }
        public decimal? DaysPaid { get; set; }
        public string UANNo { get; set; }
        public decimal? LOP { get; set; }
        public string ESINo { get; set; }
    }
    public class SectionResponseViewModel
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public List<SalaryComponentViewModel> Components { get; set; }
    }
    public class SalaryComponentViewModel
    {
        
        public int SectionComponentId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCode { get; set; }
        public int SequenceNo { get; set; }
        public string ComponentValue { get; set; }
    }
    public class PayoutMappingMasterViewModel
    {
        public int LoginId { get; set; }
        public int MapId { get; set; }
        public Nullable<int> GradeId { get; set; }
        public string Grade { get; set; }
        public Nullable<int> PayoutTypeId { get; set; }
        public string PayoutTypeName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class PayrollReportViewModel
    {
        public int? LoginId { get; set; }
        public string Month { get; set; }
        public int MonthNo { get; set; }
        public int Year { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int? CompId { get; set; }
        public string Company { get; set; }
        public int? LEId { get; set; }
        public string LegalEntity { get; set; }
        public int? BUId { get; set; }
        public string BusinessUnit { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public int? DeptId { get; set; }
        public string Department { get; set; }
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public decimal TotalDays { get; set; }
        public decimal WorkingDays { get; set; }
        public decimal PaidLeaveDaysEL { get; set; }
        public decimal PaidLeaveDaysCL { get; set; }
        public decimal LOPDays { get; set; }
        public decimal Arrears { get; set; }
        public decimal LOPAmt { get; set; }
    }
    public class DDLegalEntityPayrollViewModel
    {
        public int LEId { get; set; }
        public string LegalEntity { get; set; }
        public int? LoginId { get; set; }
    }
    public class DDLocationPayrollViewModel
    {
        public int LocationId { get; set; }
        public string Location { get; set; }
        public int? LoginId { get; set; }
        public string AuthorisedEntity { get; set; }
    }
    public class PayrollVariableViewModel
    {
        public int? LoginId { get; set; }
        public int VariableId { get; set; }
        public string VariableName { get; set; }
        public string VariableCode { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class DDPayrollVariableViewModel
    {
        public int VariableId { get; set; }
        public string VariableName { get; set; }
        public string VariableCode { get; set; }
        public int? LoginId { get; set; }
        public string AuthorisedEntity { get; set; }
    }
    public class VariableHistoryViewModel
    {
        public int? LoginId { get; set; }
        public int? CompId { get; set; }
        public int? LEId { get; set; }
        public int? BUId { get; set; }
        public int? LocationId { get; set; }
        public int? DeptId { get; set; }
        public int? DesignationId { get; set; }
        public int? ReporterId { get; set; }
        public int VariableHistoryId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpCTC { get; set; }
        public Nullable<int> VariableId { get; set; }
        public string VariableName { get; set; }
        public string VariableCode { get; set; }
        public string VariableAmt { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}