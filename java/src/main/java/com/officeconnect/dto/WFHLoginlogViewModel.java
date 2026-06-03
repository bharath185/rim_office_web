package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonInclude;

@JsonInclude(JsonInclude.Include.NON_NULL)
public class WFHLoginlogViewModel {

    @JsonProperty("WFHId")
    private Integer wfhId;

    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("EmpName")
    private String empName;

    @JsonProperty("IPAddress")
    private String ipAddress;

    @JsonProperty("Date")
    private Object date;

    @JsonProperty("LoginTime")
    private Object loginTime;

    @JsonProperty("LogOutTime")
    private Object logOutTime;

    @JsonProperty("Activehrs")
    private Object activehrs;

    @JsonProperty("AnalysisHr")
    private Object analysisHr;

    @JsonProperty("IsLoggedIn")
    private Boolean isLoggedIn;

    @JsonProperty("IsLoggedOut")
    private Boolean isLoggedOut;

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

    @JsonProperty("CompId")
    private Integer compId;

    @JsonProperty("CompName")
    private String compName;

    @JsonProperty("DeptId")
    private Integer deptId;

    @JsonProperty("DeptName")
    private String deptName;

    @JsonProperty("DesignationId")
    private Integer designationId;

    @JsonProperty("Designation")
    private String designation;

    @JsonProperty("FromDate")
    private String fromDate;

    @JsonProperty("ToDate")
    private String toDate;

    @JsonProperty("msg")
    private String msg;

    public Integer getWfhId() { return wfhId; }
    public void setWfhId(Integer wfhId) { this.wfhId = wfhId; }

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getEmpName() { return empName; }
    public void setEmpName(String empName) { this.empName = empName; }

    public String getIpAddress() { return ipAddress; }
    public void setIpAddress(String ipAddress) { this.ipAddress = ipAddress; }

    public Object getDate() { return date; }
    public void setDate(Object date) { this.date = date; }

    public Object getLoginTime() { return loginTime; }
    public void setLoginTime(Object loginTime) { this.loginTime = loginTime; }

    public Object getLogOutTime() { return logOutTime; }
    public void setLogOutTime(Object logOutTime) { this.logOutTime = logOutTime; }

    public Object getActivehrs() { return activehrs; }
    public void setActivehrs(Object activehrs) { this.activehrs = activehrs; }

    public Object getAnalysisHr() { return analysisHr; }
    public void setAnalysisHr(Object analysisHr) { this.analysisHr = analysisHr; }

    public Boolean getIsLoggedIn() { return isLoggedIn; }
    public void setIsLoggedIn(Boolean isLoggedIn) { this.isLoggedIn = isLoggedIn; }

    public Boolean getIsLoggedOut() { return isLoggedOut; }
    public void setIsLoggedOut(Boolean isLoggedOut) { this.isLoggedOut = isLoggedOut; }

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

    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }

    public String getCompName() { return compName; }
    public void setCompName(String compName) { this.compName = compName; }

    public Integer getDeptId() { return deptId; }
    public void setDeptId(Integer deptId) { this.deptId = deptId; }

    public String getDeptName() { return deptName; }
    public void setDeptName(String deptName) { this.deptName = deptName; }

    public Integer getDesignationId() { return designationId; }
    public void setDesignationId(Integer designationId) { this.designationId = designationId; }

    public String getDesignation() { return designation; }
    public void setDesignation(String designation) { this.designation = designation; }

    public String getFromDate() { return fromDate; }
    public void setFromDate(String fromDate) { this.fromDate = fromDate; }

    public String getToDate() { return toDate; }
    public void setToDate(String toDate) { this.toDate = toDate; }

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
