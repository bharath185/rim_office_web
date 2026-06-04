package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class PassHistoryManagementViewModel {

    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("OldPassword")
    private String oldPassword;

    @JsonProperty("NewPassword")
    private String newPassword;

    @JsonProperty("FPwd")
    private Boolean fpwd;

    @JsonProperty("CPwd")
    private Boolean cpwd;

    @JsonProperty("Expired")
    private Boolean expired;

    @JsonProperty("msg")
    private String msg;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getOldPassword() { return oldPassword; }
    public void setOldPassword(String oldPassword) { this.oldPassword = oldPassword; }

    public String getNewPassword() { return newPassword; }
    public void setNewPassword(String newPassword) { this.newPassword = newPassword; }

    public Boolean getFpwd() { return fpwd; }
    public void setFpwd(Boolean fpwd) { this.fpwd = fpwd; }

    public Boolean getCpwd() { return cpwd; }
    public void setCpwd(Boolean cpwd) { this.cpwd = cpwd; }

    public Boolean getExpired() { return expired; }
    public void setExpired(Boolean expired) { this.expired = expired; }

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
