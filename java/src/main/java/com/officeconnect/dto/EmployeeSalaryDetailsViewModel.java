package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class EmployeeSalaryDetailsViewModel {
    @JsonProperty("LoginId")
    @JsonAlias({"loginId", "LoginId"})
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("CompId")
    private Integer compId;

    @JsonProperty("LEId")
    private Integer leId;

    @JsonProperty("BUId")
    private Integer buId;

    @JsonProperty("LocId")
    private Integer locId;

    @JsonProperty("DeptId")
    private Integer deptId;

    @JsonProperty("DesignationId")
    private Integer designationId;

    @JsonProperty("ReportId")
    private Integer reportId;

    @JsonProperty("SalaryId")
    private Integer salaryId;

    @JsonProperty("FirstName")
    private String firstName;

    @JsonProperty("MiddleName")
    private String middleName;

    @JsonProperty("LastName")
    private String lastName;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("CTC")
    private java.math.BigDecimal ctc;

    @JsonProperty("MCTC")
    private java.math.BigDecimal mctc;

    @JsonProperty("IncrementPercent")
    private java.math.BigDecimal incrementPercent;

    @JsonProperty("EffectiveFromDate")
    private String effectiveFromDate;

    @JsonProperty("EffectiveToDate")
    private String effectiveToDate;

    @JsonProperty("IsAppraised")
    private Boolean isAppraised;

    @JsonProperty("CreatedBy")
    private Integer createdBy;

    @JsonProperty("CreatedDate")
    private String createdDate;

    @JsonProperty("LastUpdatedBy")
    private Integer lastUpdatedBy;

    @JsonProperty("LastUpdatedDate")
    private String lastUpdatedDate;

    @JsonProperty("IsActive")
    private Boolean isActive;

    @JsonProperty("IsUpdated")
    private Boolean isUpdated;

    @JsonProperty("IsDeleted")
    private Boolean isDeleted;

    @JsonProperty("Year")
    @JsonAlias({"year", "Year"})
    private Integer year;

    @JsonProperty("Month")
    @JsonAlias({"month", "Month"})
    private String month;

    @JsonProperty("MonthNo")
    @JsonAlias({"monthNo", "MonthNo"})
    private Integer monthNo;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }
    public Integer getLEId() { return leId; }
    public void setLEId(Integer leId) { this.leId = leId; }
    public Integer getBUId() { return buId; }
    public void setBUId(Integer buId) { this.buId = buId; }
    public Integer getLocId() { return locId; }
    public void setLocId(Integer locId) { this.locId = locId; }
    public Integer getDeptId() { return deptId; }
    public void setDeptId(Integer deptId) { this.deptId = deptId; }
    public Integer getDesignationId() { return designationId; }
    public void setDesignationId(Integer designationId) { this.designationId = designationId; }
    public Integer getReportId() { return reportId; }
    public void setReportId(Integer reportId) { this.reportId = reportId; }
    public Integer getSalaryId() { return salaryId; }
    public void setSalaryId(Integer salaryId) { this.salaryId = salaryId; }
    public String getFirstName() { return firstName; }
    public void setFirstName(String firstName) { this.firstName = firstName; }
    public String getMiddleName() { return middleName; }
    public void setMiddleName(String middleName) { this.middleName = middleName; }
    public String getLastName() { return lastName; }
    public void setLastName(String lastName) { this.lastName = lastName; }
    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
    public java.math.BigDecimal getCtc() { return ctc; }
    public void setCtc(java.math.BigDecimal ctc) { this.ctc = ctc; }
    public java.math.BigDecimal getMctc() { return mctc; }
    public void setMctc(java.math.BigDecimal mctc) { this.mctc = mctc; }
    public java.math.BigDecimal getIncrementPercent() { return incrementPercent; }
    public void setIncrementPercent(java.math.BigDecimal incrementPercent) { this.incrementPercent = incrementPercent; }
    public String getEffectiveFromDate() { return effectiveFromDate; }
    public void setEffectiveFromDate(String effectiveFromDate) { this.effectiveFromDate = effectiveFromDate; }
    public String getEffectiveToDate() { return effectiveToDate; }
    public void setEffectiveToDate(String effectiveToDate) { this.effectiveToDate = effectiveToDate; }
    public Boolean getIsAppraised() { return isAppraised; }
    public void setIsAppraised(Boolean isAppraised) { this.isAppraised = isAppraised; }
    public Integer getCreatedBy() { return createdBy; }
    public void setCreatedBy(Integer createdBy) { this.createdBy = createdBy; }
    public String getCreatedDate() { return createdDate; }
    public void setCreatedDate(String createdDate) { this.createdDate = createdDate; }
    public Integer getLastUpdatedBy() { return lastUpdatedBy; }
    public void setLastUpdatedBy(Integer lastUpdatedBy) { this.lastUpdatedBy = lastUpdatedBy; }
    public String getLastUpdatedDate() { return lastUpdatedDate; }
    public void setLastUpdatedDate(String lastUpdatedDate) { this.lastUpdatedDate = lastUpdatedDate; }
    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }
    public Boolean getIsUpdated() { return isUpdated; }
    public void setIsUpdated(Boolean isUpdated) { this.isUpdated = isUpdated; }
    public Boolean getIsDeleted() { return isDeleted; }
    public void setIsDeleted(Boolean isDeleted) { this.isDeleted = isDeleted; }
    public Integer getYear() { return year; }
    public void setYear(Integer year) { this.year = year; }
    public String getMonth() { return month; }
    public void setMonth(String month) { this.month = month; }
    public Integer getMonthNo() { return monthNo; }
    public void setMonthNo(Integer monthNo) { this.monthNo = monthNo; }
}
