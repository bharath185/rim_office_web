package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "LeaveTypeMaster")
public class LeaveTypeMaster {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "LeaveTypeId")
    private Integer leaveTypeId;

    @Column(name = "LocationId")
    private String locationId;

    @Column(name = "YearType")
    private String yearType;

    @Column(name = "LeaveName")
    private String leaveName;

    @Column(name = "ShortName")
    private String shortName;

    @Column(name = "Description")
    private String description;

    @Column(name = "DurationType")
    private String durationType;

    @Column(name = "ApplicableTo")
    private String applicableTo;

    @Column(name = "EmpTypeId")
    private String empTypeId;

    @Column(name = "EmpLevel")
    private String empLevel;

    @Column(name = "CarryForward")
    private Boolean carryForward;

    @Column(name = "Credit")
    private Integer credit;

    @Column(name = "IsMonth")
    private Boolean isMonth;

    @Column(name = "IsYear")
    private Boolean isYear;

    @Column(name = "MaxCarryForward")
    private Integer maxCarryForward;

    @Column(name = "ResetYear")
    private Boolean resetYear;

    @Column(name = "Encashable")
    private Boolean encashable;

    @Column(name = "MaxPerMonth")
    private Integer maxPerMonth;

    @Column(name = "MaxPerYear")
    private Integer maxPerYear;

    @Column(name = "MaxApply")
    private Integer maxApply;

    @Column(name = "IsPaid")
    private Boolean isPaid;

    @Column(name = "ApplicableDuration")
    private Integer applicableDuration;

    @Column(name = "IsSingleApplication")
    private Boolean isSingleApplication;

    @Column(name = "MaxAllowedEvents")
    private Integer maxAllowedEvents;

    @Column(name = "WeekEndInclusive")
    private Boolean weekEndInclusive;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "CreatedBy")
    private Integer createdBy;

    @Column(name = "CreatedDate")
    @Temporal(TemporalType.DATE)
    private Date createdDate;

    @Column(name = "LastUpdatedBy")
    private Integer lastUpdatedBy;

    @Column(name = "LastUpdatedDate")
    @Temporal(TemporalType.DATE)
    private Date lastUpdatedDate;

    @Column(name = "IsUpdated")
    private Boolean isUpdated;

    @Column(name = "IsDeleted")
    private Boolean isDeleted;

    public Integer getLeaveTypeId() { return leaveTypeId; }
    public void setLeaveTypeId(Integer leaveTypeId) { this.leaveTypeId = leaveTypeId; }

    public String getLocationId() { return locationId; }
    public void setLocationId(String locationId) { this.locationId = locationId; }

    public String getYearType() { return yearType; }
    public void setYearType(String yearType) { this.yearType = yearType; }

    public String getLeaveName() { return leaveName; }
    public void setLeaveName(String leaveName) { this.leaveName = leaveName; }

    public String getShortName() { return shortName; }
    public void setShortName(String shortName) { this.shortName = shortName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getDurationType() { return durationType; }
    public void setDurationType(String durationType) { this.durationType = durationType; }

    public String getApplicableTo() { return applicableTo; }
    public void setApplicableTo(String applicableTo) { this.applicableTo = applicableTo; }

    public String getEmpTypeId() { return empTypeId; }
    public void setEmpTypeId(String empTypeId) { this.empTypeId = empTypeId; }

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

    public Integer getCreatedBy() { return createdBy; }
    public void setCreatedBy(Integer createdBy) { this.createdBy = createdBy; }

    public Date getCreatedDate() { return createdDate; }
    public void setCreatedDate(Date createdDate) { this.createdDate = createdDate; }

    public Integer getLastUpdatedBy() { return lastUpdatedBy; }
    public void setLastUpdatedBy(Integer lastUpdatedBy) { this.lastUpdatedBy = lastUpdatedBy; }

    public Date getLastUpdatedDate() { return lastUpdatedDate; }
    public void setLastUpdatedDate(Date lastUpdatedDate) { this.lastUpdatedDate = lastUpdatedDate; }

    public Boolean getIsUpdated() { return isUpdated; }
    public void setIsUpdated(Boolean isUpdated) { this.isUpdated = isUpdated; }

    public Boolean getIsDeleted() { return isDeleted; }
    public void setIsDeleted(Boolean isDeleted) { this.isDeleted = isDeleted; }
}