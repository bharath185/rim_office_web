using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class CompanyMasterViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> LoginId { get; set; }
        public int CompId { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public string LocationMap { get; set; }
        public string Address { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class LegalEntityMasterViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> LoginId { get; set; }
        public int LEId { get; set; }
        public Nullable<int> CompId { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public string LegalEntity { get; set; }
        public string Description { get; set; }
        public string CompanyType { get; set; }
        public string Logo { get; set; }
        public string LogoWithAddress { get; set; }
        public string WebAppLogo { get; set; }
        public string Website { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class BusinessUnitMasterViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> LoginId { get; set; }
        public int BUId { get; set; }
        public Nullable<int> CompId { get; set; }
        public Nullable<int> LEId { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public string LegalEntity { get; set; }
        public string BusinessUnit { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class LocationMasterViewModel
    {
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> LoginId { get; set; }
        public int LocationId { get; set; }
        public Nullable<int> CompId { get; set; }
        public Nullable<int> LEId { get; set; }
        public Nullable<int> BUId { get; set; }
        public string Company { get; set; }
        public string CompanyCode { get; set; }
        public string LegalEntity { get; set; }
        public string BusinessUnit { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string LocationMap { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string TimeZone { get; set; }
        public Nullable<int> ProbationPeriod { get; set; }
        public string WeeklyHoliday { get; set; }
        public string CompanyRegNo { get; set; }
        public string DateofReg { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }
        public string TANNo { get; set; }
        public string VATNo { get; set; }
        public string PANNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public string GSTNo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class BusinessEntityResponseViewModel
    {
        public int Status { get; set; }
        public string msg { get; set; }
    }
    public class CalendarYearMasterViewModel
    {
        public int? LoginId { get; set; }
        public int Id { get; set; }
        public int Year { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public partial class FinancialYearMasterViewModel
    {
        public int? LoginId { get; set; }
        public int YearId { get; set; }
        public string FinancialYear { get; set; }
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