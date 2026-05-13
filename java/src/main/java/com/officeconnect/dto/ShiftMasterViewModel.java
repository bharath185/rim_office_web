package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.Map;

@JsonInclude(JsonInclude.Include.NON_NULL)
public class ShiftMasterViewModel {
    @JsonProperty("ShiftId")
    private Integer shiftId;
    @JsonProperty("ShiftName")
    private String shiftName;
    @JsonProperty("ClkHrs")
    private String clkHrs;
    @JsonProperty("Days")
    private String days;
    @JsonProperty("Status")
    private Boolean status;
    @JsonProperty("CreatedBy")
    private int createdBy;
    @JsonProperty("CreatedDate")
    private String createdDate;
    @JsonProperty("LastUpdatedBy")
    private Integer lastUpdatedBy;
    @JsonProperty("LastUpdatedDate")
    private String lastUpdatedDate;
    @JsonProperty("IsActive")
    private Boolean isActive;
    @JsonProperty("IsUpdated")
    private Boolean isUpdated;
    @JsonProperty("IsDeleted")
    private Boolean isDeleted;
    @JsonProperty("LoginId")
    private int loginId;
    @JsonProperty("msg")
    private String msg;

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
    public Boolean getStatus() { return status; }
    public void setStatus(Boolean status) { this.status = status; }
    public int getCreatedBy() { return createdBy; }
    public void setCreatedBy(int createdBy) { this.createdBy = createdBy; }
    public String getCreatedDate() { return createdDate; }
    public void setCreatedDate(String createdDate) { this.createdDate = createdDate; }
    public Integer getLastUpdatedBy() { return lastUpdatedBy; }
    public void setLastUpdatedBy(Integer lastUpdatedBy) { this.lastUpdatedBy = lastUpdatedBy; }
    public String getLastUpdatedDate() { return lastUpdatedDate; }
    public void setLastUpdatedDate(String lastUpdatedDate) { this.lastUpdatedDate = lastUpdatedDate; }
    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }
    public Boolean getIsUpdated() { return isUpdated; }
    public void setIsUpdated(Boolean isUpdated) { this.isUpdated = isUpdated; }
    public Boolean getIsDeleted() { return isDeleted; }
    public void setIsDeleted(Boolean isDeleted) { this.isDeleted = isDeleted; }
    public int getLoginId() { return loginId; }
    public void setLoginId(int loginId) { this.loginId = loginId; }
    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
