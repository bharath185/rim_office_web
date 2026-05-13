package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class LeaveTypeViewModel {
    @JsonProperty("LeaveTypeId")
    @JsonAlias({"leaveTypeId", "LeaveTypeId"})
    private Integer leaveTypeId;

    @JsonProperty("LeaveType")
    @JsonAlias({"leaveType", "LeaveType"})
    private String leaveType;

    @JsonProperty("LeaveName")
    @JsonAlias({"leaveName", "LeaveName"})
    private String leaveName;

    @JsonProperty("ShortName")
    @JsonAlias({"shortName", "ShortName"})
    private String shortName;

    @JsonProperty("Description")
    @JsonAlias({"description", "Description"})
    private String description;

    @JsonProperty("LocationId")
    @JsonAlias({"locationId", "LocationId"})
    private String locationId;

    @JsonProperty("Location")
    @JsonAlias({"location", "Location"})
    private String location;

    @JsonProperty("YearType")
    @JsonAlias({"yearType", "YearType"})
    private String yearType;

    @JsonProperty("DurationType")
    @JsonAlias({"durationType", "DurationType"})
    private String durationType;

    @JsonProperty("ApplicableTo")
    @JsonAlias({"applicableTo", "ApplicableTo"})
    private String applicableTo;

    @JsonProperty("EmpTypeId")
    @JsonAlias({"empTypeId", "EmpTypeId"})
    private String empTypeId;

    @JsonProperty("EmpType")
    @JsonAlias({"empType", "EmpType"})
    private String empType;

    @JsonProperty("EmpLevel")
    @JsonAlias({"empLevel", "EmpLevel"})
    private String empLevel;

    @JsonProperty("CarryForward")
    @JsonAlias({"carryForward", "CarryForward"})
    private Boolean carryForward;

    @JsonProperty("Credit")
    @JsonAlias({"credit", "Credit"})
    private Integer credit;

    @JsonProperty("LeaveCount")
    @JsonAlias({"leaveCount", "LeaveCount", "leaveCount", "LeaveCount"})
    private Integer leaveCount;

    @JsonProperty("MaxApply")
    @JsonAlias({"maxApply", "MaxApply"})
    private Integer maxApply;

    @JsonProperty("IsPaid")
    @JsonAlias({"isPaid", "IsPaid"})
    private Boolean isPaid;

    @JsonProperty("ApplicableDuration")
    @JsonAlias({"applicableDuration", "ApplicableDuration"})
    private Integer applicableDuration;

    @JsonProperty("IsSingleApplication")
    @JsonAlias({"isSingleApplication", "IsSingleApplication"})
    private Boolean isSingleApplication;

    @JsonProperty("MaxAllowedEvents")
    @JsonAlias({"maxAllowedEvents", "MaxAllowedEvents"})
    private Integer maxAllowedEvents;

    @JsonProperty("WeekEndInclusive")
    @JsonAlias({"weekEndInclusive", "WeekEndInclusive"})
    private Boolean weekEndInclusive;

    @JsonProperty("IsActive")
    @JsonAlias({"isActive", "IsActive"})
    private Boolean isActive;

    @JsonProperty("IsUpdated")
    @JsonAlias({"isUpdated", "IsUpdated"})
    private Boolean isUpdated;

    @JsonProperty("IsDeleted")
    @JsonAlias({"isDeleted", "IsDeleted"})
    private Boolean isDeleted;

    @JsonProperty("IsMonth")
    @JsonAlias({"isMonth", "IsMonth"})
    private Boolean isMonth;

    @JsonProperty("IsYear")
    @JsonAlias({"isYear", "IsYear"})
    private Boolean isYear;

    @JsonProperty("MaxCarryForward")
    @JsonAlias({"maxCarryForward", "MaxCarryForward"})
    private Integer maxCarryForward;

    @JsonProperty("ResetYear")
    @JsonAlias({"resetYear", "ResetYear"})
    private Boolean resetYear;

    @JsonProperty("Encashable")
    @JsonAlias({"encashable", "Encashable"})
    private Boolean encashable;

    @JsonProperty("MaxPerMonth")
    @JsonAlias({"maxPerMonth", "MaxPerMonth"})
    private Integer maxPerMonth;

    @JsonProperty("MaxPerYear")
    @JsonAlias({"maxPerYear", "MaxPerYear"})
    private Integer maxPerYear;

    @JsonProperty("CreatedBy")
    @JsonAlias({"createdBy", "CreatedBy"})
    private Integer createdBy;

    @JsonProperty("CreatedDate")
    @JsonAlias({"createdDate", "CreatedDate"})
    private Object createdDate;

    @JsonProperty("LastUpdatedBy")
    @JsonAlias({"lastUpdatedBy", "LastUpdatedBy"})
    private Integer lastUpdatedBy;

    @JsonProperty("LastUpdatedDate")
    @JsonAlias({"lastUpdatedDate", "LastUpdatedDate"})
    private Object lastUpdatedDate;

    @JsonProperty("LoginId")
    @JsonAlias({"loginId", "LoginId"})
    private Integer loginId;

    @JsonProperty("msg")
    @JsonAlias({"msg", "Msg"})
    private String msg;

    // Getters and Setters
    public Integer getLeaveTypeId() { return leaveTypeId; }
    public void setLeaveTypeId(Integer leaveTypeId) { this.leaveTypeId = leaveTypeId; }

    public String getLeaveType() { return leaveType; }
    public void setLeaveType(String leaveType) { this.leaveType = leaveType; }

    public String getLeaveName() { return leaveName; }
    public void setLeaveName(String leaveName) { this.leaveName = leaveName; }

    public String getShortName() { return shortName; }
    public void setShortName(String shortName) { this.shortName = shortName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getLocationId() { return locationId; }
    public void setLocationId(String locationId) { this.locationId = locationId; }

    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }

    public String getYearType() { return yearType; }
    public void setYearType(String yearType) { this.yearType = yearType; }

    public String getDurationType() { return durationType; }
    public void setDurationType(String durationType) { this.durationType = durationType; }

    public String getApplicableTo() { return applicableTo; }
    public void setApplicableTo(String applicableTo) { this.applicableTo = applicableTo; }

    public String getEmpTypeId() { return empTypeId; }
    public void setEmpTypeId(String empTypeId) { this.empTypeId = empTypeId; }

    public String getEmpType() { return empType; }
    public void setEmpType(String empType) { this.empType = empType; }

    public String getEmpLevel() { return empLevel; }
    public void setEmpLevel(String empLevel) { this.empLevel = empLevel; }

    public Boolean getCarryForward() { return carryForward; }
    public void setCarryForward(Boolean carryForward) { this.carryForward = carryForward; }

    public Integer getCredit() { return credit; }
    public void setCredit(Integer credit) { this.credit = credit; }

    public Boolean getIsMonth() { return isMonth; }
    public void setIsMonth(Boolean isMonth) { this.isMonth = isMonth; }

    public Boolean getIsYear() { return isYear; }
    public void setIsYear(Boolean isYear) { this.isYear = isYear; }

    public Integer getMaxCarryForward() { return maxCarryForward; }
    public void setMaxCarryForward(Integer maxCarryForward) { this.maxCarryForward = maxCarryForward; }

    public Boolean getResetYear() { return resetYear; }
    public void setResetYear(Boolean resetYear) { this.resetYear = resetYear; }

    public Boolean getEncashable() { return encashable; }
    public void setEncashable(Boolean encashable) { this.encashable = encashable; }

    public Integer getMaxPerMonth() { return maxPerMonth; }
    public void setMaxPerMonth(Integer maxPerMonth) { this.maxPerMonth = maxPerMonth; }

    public Integer getMaxPerYear() { return maxPerYear; }
    public void setMaxPerYear(Integer maxPerYear) { this.maxPerYear = maxPerYear; }

    public Integer getMaxApply() { return maxApply; }
    public void setMaxApply(Integer maxApply) { this.maxApply = maxApply; }

    public Boolean getIsPaid() { return isPaid; }
    public void setIsPaid(Boolean isPaid) { this.isPaid = isPaid; }

    public Integer getApplicableDuration() { return applicableDuration; }
    public void setApplicableDuration(Integer applicableDuration) { this.applicableDuration = applicableDuration; }

    public Boolean getIsSingleApplication() { return isSingleApplication; }
    public void setIsSingleApplication(Boolean isSingleApplication) { this.isSingleApplication = isSingleApplication; }

    public Integer getMaxAllowedEvents() { return maxAllowedEvents; }
    public void setMaxAllowedEvents(Integer maxAllowedEvents) { this.maxAllowedEvents = maxAllowedEvents; }

    public Boolean getWeekEndInclusive() { return weekEndInclusive; }
    public void setWeekEndInclusive(Boolean weekEndInclusive) { this.weekEndInclusive = weekEndInclusive; }

    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }

    public Boolean getIsUpdated() { return isUpdated; }
    public void setIsUpdated(Boolean isUpdated) { this.isUpdated = isUpdated; }

    public Boolean getIsDeleted() { return isDeleted; }
    public void setIsDeleted(Boolean isDeleted) { this.isDeleted = isDeleted; }

    public Integer getCreatedBy() { return createdBy; }
    public void setCreatedBy(Integer createdBy) { this.createdBy = createdBy; }

    public Object getCreatedDate() { return createdDate; }
    public void setCreatedDate(Object createdDate) { this.createdDate = createdDate; }

    public Integer getLastUpdatedBy() { return lastUpdatedBy; }
    public void setLastUpdatedBy(Integer lastUpdatedBy) { this.lastUpdatedBy = lastUpdatedBy; }

    public Object getLastUpdatedDate() { return lastUpdatedDate; }
    public void setLastUpdatedDate(Object lastUpdatedDate) { this.lastUpdatedDate = lastUpdatedDate; }

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

    public Integer getLeaveCount() { return credit; }
    public void setLeaveCount(Integer leaveCount) { this.credit = leaveCount; this.leaveCount = leaveCount; }

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}