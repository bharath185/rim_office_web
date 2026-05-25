package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class UploadAttendanceSingleViewModel {
    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("Date")
    private String date;

    @JsonProperty("Time")
    private String time;

    @JsonProperty("Status")
    private String status;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getDate() { return date; }
    public void setDate(String date) { this.date = date; }

    public String getTime() { return time; }
    public void setTime(String time) { this.time = time; }

    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }
}
