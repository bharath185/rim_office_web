package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.Map;

@JsonInclude(JsonInclude.Include.NON_NULL)
public class SampleShiftMasterViewModel {
    @JsonProperty("ShiftId")
    private Integer shiftId;
    @JsonProperty("ShiftName")
    private String shiftName;
    @JsonProperty("ClkHrs")
    private String clkHrs;
    @JsonProperty("Days")
    private String days;

    private String startTime;
    private String endTime;

    public Integer getShiftId() { return shiftId; }
    public void setShiftId(Integer shiftId) { this.shiftId = shiftId; }
    public String getShiftName() { return shiftName; }
    public void setShiftName(String shiftName) { this.shiftName = shiftName; }

    @JsonIgnore
    public String getStartTime() { return startTime; }
    @JsonProperty("StartTime")
    public void setStartTime(String startTime) { this.startTime = startTime; }

    @JsonIgnore
    public String getEndTime() { return endTime; }
    @JsonProperty("EndTime")
    public void setEndTime(String endTime) { this.endTime = endTime; }

    @JsonProperty("StartTime")
    public Map<String, Integer> getStartTimeObject() {
        if (startTime == null) return null;
        String[] parts = startTime.split(":");
        return Map.of("Hours", Integer.parseInt(parts[0]), "Minutes", Integer.parseInt(parts[1]));
    }

    @JsonProperty("EndTime")
    public Map<String, Integer> getEndTimeObject() {
        if (endTime == null) return null;
        String[] parts = endTime.split(":");
        return Map.of("Hours", Integer.parseInt(parts[0]), "Minutes", Integer.parseInt(parts[1]));
    }

    public String getClkHrs() { return clkHrs; }
    public void setClkHrs(String clkHrs) { this.clkHrs = clkHrs; }
    public String getDays() { return days; }
    public void setDays(String days) { this.days = days; }
}
