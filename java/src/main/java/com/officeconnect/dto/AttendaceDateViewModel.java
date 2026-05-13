package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.List;

public class AttendaceDateViewModel {

    @JsonProperty("AttendaceDate")
    private String attendaceDate;

    @JsonProperty("lstofAttendance")
    private List<AttendanceViewModel> lstofAttendance;

    public String getAttendaceDate() { return attendaceDate; }
    public void setAttendaceDate(String attendaceDate) { this.attendaceDate = attendaceDate; }

    public List<AttendanceViewModel> getLstofAttendance() { return lstofAttendance; }
    public void setLstofAttendance(List<AttendanceViewModel> lstofAttendance) { this.lstofAttendance = lstofAttendance; }
}
