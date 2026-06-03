package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

@JsonInclude(JsonInclude.Include.NON_NULL)
public class ScreenshotsViewModel {

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("Date")
    private String date;

    @JsonProperty("EmpName")
    private String empName;

    @JsonProperty("msg")
    private String msg;

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getDate() { return date; }
    public void setDate(String date) { this.date = date; }

    public String getEmpName() { return empName; }
    public void setEmpName(String empName) { this.empName = empName; }

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
