package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "WeekHoliday")
public class WeekHoliday {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "WeekDay_ID")
    private Integer weekDayId;

    @Column(name = "Day")
    private String day;

    @Column(name = "Created_By")
    private Integer createdBy;

    @Column(name = "Created_Date")
    @Temporal(TemporalType.TIMESTAMP)
    private Date createdDate;

    @Column(name = "Modified_By")
    private Integer modifiedBy;

    @Column(name = "Modified_Date")
    @Temporal(TemporalType.TIMESTAMP)
    private Date modifiedDate;

    @Column(name = "Status")
    private String status;

    @Column(name = "LocationId")
    private Integer locationId;

    @Column(name = "Title")
    private String title;

    @Column(name = "Location")
    private String location;

    @Column(name = "Description")
    private String description;

    @Column(name = "HolidayType")
    private String holidayType;

    @Column(name = "Year")
    private Integer year;

    public Integer getWeekDayId() { return weekDayId; }
    public void setWeekDayId(Integer weekDayId) { this.weekDayId = weekDayId; }

    public String getDay() { return day; }
    public void setDay(String day) { this.day = day; }

    public Integer getCreatedBy() { return createdBy; }
    public void setCreatedBy(Integer createdBy) { this.createdBy = createdBy; }

    public Date getCreatedDate() { return createdDate; }
    public void setCreatedDate(Date createdDate) { this.createdDate = createdDate; }

    public Integer getModifiedBy() { return modifiedBy; }
    public void setModifiedBy(Integer modifiedBy) { this.modifiedBy = modifiedBy; }

    public Date getModifiedDate() { return modifiedDate; }
    public void setModifiedDate(Date modifiedDate) { this.modifiedDate = modifiedDate; }

    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }

    public Integer getLocationId() { return locationId; }
    public void setLocationId(Integer locationId) { this.locationId = locationId; }

    public String getTitle() { return title; }
    public void setTitle(String title) { this.title = title; }

    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getHolidayType() { return holidayType; }
    public void setHolidayType(String holidayType) { this.holidayType = holidayType; }

    public Integer getYear() { return year; }
    public void setYear(Integer year) { this.year = year; }
}
