package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class PayrolAccessViewModel {
    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
}
