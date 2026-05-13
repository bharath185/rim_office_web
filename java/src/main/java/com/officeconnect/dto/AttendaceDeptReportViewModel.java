package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.List;

public class AttendaceDeptReportViewModel {

    @JsonProperty("Date")
    private String date;

    @JsonProperty("Day")
    private String day;

    @JsonProperty("lstofDept")
    private List<DepartmentAttendanceViewModel> lstofDept;

    public String getDate() { return date; }
    public void setDate(String date) { this.date = date; }

    public String getDay() { return day; }
    public void setDay(String day) { this.day = day; }

    public List<DepartmentAttendanceViewModel> getLstofDept() { return lstofDept; }
    public void setLstofDept(List<DepartmentAttendanceViewModel> lstofDept) { this.lstofDept = lstofDept; }
}
