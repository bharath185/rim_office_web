package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class WFHLoginlogFilterViewModel {

    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("CompId")
    private Integer compId;

    @JsonProperty("DeptId")
    private Integer deptId;

    @JsonProperty("DesignationId")
    private Integer designationId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("FromDate")
    private String fromDate;

    @JsonProperty("ToDate")
    private String toDate;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }

    public Integer getDeptId() { return deptId; }
    public void setDeptId(Integer deptId) { this.deptId = deptId; }

    public Integer getDesignationId() { return designationId; }
    public void setDesignationId(Integer designationId) { this.designationId = designationId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getFromDate() { return fromDate; }
    public void setFromDate(String fromDate) { this.fromDate = fromDate; }

    public String getToDate() { return toDate; }
    public void setToDate(String toDate) { this.toDate = toDate; }
}
