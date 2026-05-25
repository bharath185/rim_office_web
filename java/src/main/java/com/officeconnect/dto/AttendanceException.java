package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class AttendanceException {
    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("Date")
    private String date;

    @JsonProperty("Time")
    private String time;

    @JsonProperty("Reason")
    private String reason;

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getDate() { return date; }
    public void setDate(String date) { this.date = date; }

    public String getTime() { return time; }
    public void setTime(String time) { this.time = time; }

    public String getReason() { return reason; }
    public void setReason(String reason) { this.reason = reason; }
}
