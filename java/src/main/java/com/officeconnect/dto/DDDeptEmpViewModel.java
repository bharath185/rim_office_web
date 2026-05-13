package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class DDDeptEmpViewModel {

    @JsonProperty("DeptId")
    private Integer deptId;

    @JsonProperty("DesignationId")
    private Integer designationId;

    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("EmpName")
    private String empName;

    @JsonProperty("EmpCode")
    private String empCode;

    public Integer getDeptId() { return deptId; }
    public void setDeptId(Integer deptId) { this.deptId = deptId; }

    public Integer getDesignationId() { return designationId; }
    public void setDesignationId(Integer designationId) { this.designationId = designationId; }

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpName() { return empName; }
    public void setEmpName(String empName) { this.empName = empName; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
}
