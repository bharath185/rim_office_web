package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

@JsonInclude(JsonInclude.Include.NON_NULL)
public class ShiftEmployeeMasterViewModel {
    @JsonProperty("LoginId")     private Integer loginId;
    @JsonProperty("EmpId")       private Integer empId;
    @JsonProperty("OldEmp_ID")   private Integer oldEmpId;
    @JsonProperty("CompId")      private Integer compId;
    @JsonProperty("Company")     private String company;
    @JsonProperty("LEId")        private Integer leId;
    @JsonProperty("LegalEntity") private String legalEntity;
    @JsonProperty("BUId")        private Integer buId;
    @JsonProperty("BusinessUnit") private String businessUnit;
    @JsonProperty("LocationId")  private Integer locationId;
    @JsonProperty("Location")    private String location;
    @JsonProperty("ShiftId")     private Integer shiftId;
    @JsonProperty("ShiftName")   private String shiftName;
    @JsonProperty("CategoryId")  private Integer categoryId;
    @JsonProperty("DeptId")      private Integer deptId;
    @JsonProperty("DeptName")    private String deptName;
    @JsonProperty("DesignationId") private Integer designationId;
    @JsonProperty("Designation") private String designation;
    @JsonProperty("ReportId")    private Integer reportId;
    @JsonProperty("ApproverId")  private Integer approverId;
    @JsonProperty("Approver")    private String approver;
    @JsonProperty("EmpCode")     private String empCode;
    @JsonProperty("FirstName")   private String firstName;
    @JsonProperty("MiddleName")  private String middleName;
    @JsonProperty("LastName")    private String lastName;
    @JsonProperty("msg")         private String msg;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public Integer getOldEmpId() { return oldEmpId; }
    public void setOldEmpId(Integer oldEmpId) { this.oldEmpId = oldEmpId; }
    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }
    public String getCompany() { return company; }
    public void setCompany(String company) { this.company = company; }
    public Integer getLeId() { return leId; }
    public void setLeId(Integer leId) { this.leId = leId; }
    public String getLegalEntity() { return legalEntity; }
    public void setLegalEntity(String legalEntity) { this.legalEntity = legalEntity; }
    public Integer getBuId() { return buId; }
    public void setBuId(Integer buId) { this.buId = buId; }
    public String getBusinessUnit() { return businessUnit; }
    public void setBusinessUnit(String businessUnit) { this.businessUnit = businessUnit; }
    public Integer getLocationId() { return locationId; }
    public void setLocationId(Integer locationId) { this.locationId = locationId; }
    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }
    public Integer getShiftId() { return shiftId; }
    public void setShiftId(Integer shiftId) { this.shiftId = shiftId; }
    public String getShiftName() { return shiftName; }
    public void setShiftName(String shiftName) { this.shiftName = shiftName; }
    public Integer getCategoryId() { return categoryId; }
    public void setCategoryId(Integer categoryId) { this.categoryId = categoryId; }
    public Integer getDeptId() { return deptId; }
    public void setDeptId(Integer deptId) { this.deptId = deptId; }
    public String getDeptName() { return deptName; }
    public void setDeptName(String deptName) { this.deptName = deptName; }
    public Integer getDesignationId() { return designationId; }
    public void setDesignationId(Integer designationId) { this.designationId = designationId; }
    public String getDesignation() { return designation; }
    public void setDesignation(String designations) { this.designation = designations; }
    public Integer getReportId() { return reportId; }
    public void setReportId(Integer reportId) { this.reportId = reportId; }
    public Integer getApproverId() { return approverId; }
    public void setApproverId(Integer approverId) { this.approverId = approverId; }
    public String getApprover() { return approver; }
    public void setApprover(String approver) { this.approver = approver; }
    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
    public String getFirstName() { return firstName; }
    public void setFirstName(String firstName) { this.firstName = firstName; }
    public String getMiddleName() { return middleName; }
    public void setMiddleName(String middleName) { this.middleName = middleName; }
    public String getLastName() { return lastName; }
    public void setLastName(String lastName) { this.lastName = lastName; }
    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
