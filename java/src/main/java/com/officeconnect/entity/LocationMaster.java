package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "LocationMaster")
public class LocationMaster {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "LocationId")
    private Integer locationId;

    @Column(name = "BUId")
    private Integer buId;

    @Column(name = "LEId")
    private Integer leId;

    @Column(name = "CompId")
    private Integer compId;

    @Column(name = "Location")
    private String location;

    @Column(name = "Description")
    private String description;

    @Column(name = "LocationMap")
    private String locationMap;

    @Column(name = "Address")
    private String address;

    @Column(name = "City")
    private String city;

    @Column(name = "State")
    private String state;

    @Column(name = "Country")
    private String country;

    @Column(name = "PostalCode")
    private String postalCode;

    @Column(name = "TimeZone")
    private String timeZone;

    @Column(name = "ProbationPeriod")
    private Integer probationPeriod;

    @Column(name = "WeeklyHoliday")
    private String weeklyHoliday;

    @Column(name = "CompanyRegNo")
    private String companyRegNo;

    @Column(name = "DateofReg")
    private String dateofReg;

    @Column(name = "PFNo")
    private String pfNo;

    @Column(name = "ESINo")
    private String esiNo;

    @Column(name = "TANNo")
    private String tanNo;

    @Column(name = "VATNo")
    private String vatNo;

    @Column(name = "PANNo")
    private String panNo;

    @Column(name = "ServiceTaxNo")
    private String serviceTaxNo;

    @Column(name = "GSTNo")
    private String gstNo;

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

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "IsUpdated")
    private Boolean isUpdated;

    @Column(name = "IsDeleted")
    private Boolean isDeleted;

    public Integer getLocationId() { return locationId; }
    public void setLocationId(Integer locationId) { this.locationId = locationId; }

    public Integer getBuId() { return buId; }
    public void setBuId(Integer buId) { this.buId = buId; }

    public Integer getLeId() { return leId; }
    public void setLeId(Integer leId) { this.leId = leId; }

    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }

    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getLocationMap() { return locationMap; }
    public void setLocationMap(String locationMap) { this.locationMap = locationMap; }

    public String getAddress() { return address; }
    public void setAddress(String address) { this.address = address; }

    public String getCity() { return city; }
    public void setCity(String city) { this.city = city; }

    public String getState() { return state; }
    public void setState(String state) { this.state = state; }

    public String getCountry() { return country; }
    public void setCountry(String country) { this.country = country; }

    public String getPostalCode() { return postalCode; }
    public void setPostalCode(String postalCode) { this.postalCode = postalCode; }

    public String getTimeZone() { return timeZone; }
    public void setTimeZone(String timeZone) { this.timeZone = timeZone; }

    public Integer getProbationPeriod() { return probationPeriod; }
    public void setProbationPeriod(Integer probationPeriod) { this.probationPeriod = probationPeriod; }

    public String getWeeklyHoliday() { return weeklyHoliday; }
    public void setWeeklyHoliday(String weeklyHoliday) { this.weeklyHoliday = weeklyHoliday; }

    public String getCompanyRegNo() { return companyRegNo; }
    public void setCompanyRegNo(String companyRegNo) { this.companyRegNo = companyRegNo; }

    public String getDateofReg() { return dateofReg; }
    public void setDateofReg(String dateofReg) { this.dateofReg = dateofReg; }

    public String getPfNo() { return pfNo; }
    public void setPfNo(String pfNo) { this.pfNo = pfNo; }

    public String getEsiNo() { return esiNo; }
    public void setEsiNo(String esiNo) { this.esiNo = esiNo; }

    public String getTanNo() { return tanNo; }
    public void setTanNo(String tanNo) { this.tanNo = tanNo; }

    public String getVatNo() { return vatNo; }
    public void setVatNo(String vatNo) { this.vatNo = vatNo; }

    public String getPanNo() { return panNo; }
    public void setPanNo(String panNo) { this.panNo = panNo; }

    public String getServiceTaxNo() { return serviceTaxNo; }
    public void setServiceTaxNo(String serviceTaxNo) { this.serviceTaxNo = serviceTaxNo; }

    public String getGstNo() { return gstNo; }
    public void setGstNo(String gstNo) { this.gstNo = gstNo; }

    public Integer getCreatedBy() { return createdBy; }
    public void setCreatedBy(Integer createdBy) { this.createdBy = createdBy; }

    public Date getCreatedDate() { return createdDate; }
    public void setCreatedDate(Date createdDate) { this.createdDate = createdDate; }

    public Integer getLastUpdatedBy() { return lastUpdatedBy; }
    public void setLastUpdatedBy(Integer lastUpdatedBy) { this.lastUpdatedBy = lastUpdatedBy; }

    public Date getLastUpdatedDate() { return lastUpdatedDate; }
    public void setLastUpdatedDate(Date lastUpdatedDate) { this.lastUpdatedDate = lastUpdatedDate; }

    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }

    public Boolean getIsUpdated() { return isUpdated; }
    public void setIsUpdated(Boolean isUpdated) { this.isUpdated = isUpdated; }

    public Boolean getIsDeleted() { return isDeleted; }
    public void setIsDeleted(Boolean isDeleted) { this.isDeleted = isDeleted; }
}
