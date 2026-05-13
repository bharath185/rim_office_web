package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "WFHLoginlog")
public class WFHLoginlog {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "WFHId")
    private Integer wfhId;

    @Column(name = "EmpId")
    private Integer empId;

    @Column(name = "EmpCode")
    private String empCode;

    @Column(name = "IPAddress")
    private String ipAddress;

    @Column(name = "Date")
    @Temporal(TemporalType.DATE)
    private Date date;

    @Column(name = "LoginTime")
    @Temporal(TemporalType.TIME)
    private Date loginTime;

    @Column(name = "LogOutTime")
    @Temporal(TemporalType.TIME)
    private Date logOutTime;

    @Column(name = "Activehrs")
    @Temporal(TemporalType.TIME)
    private Date activehrs;

    @Column(name = "AnalysisHr")
    @Temporal(TemporalType.TIME)
    private Date analysisHr;

    @Column(name = "IsLoggedIn")
    private Boolean isLoggedIn;

    @Column(name = "IsLoggedOut")
    private Boolean isLoggedOut;

    @Column(name = "CreatedBy")
    private Integer createdBy;

    @Column(name = "CreatedDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date createdDate;

    @Column(name = "LastUpdatedBy")
    private Integer lastUpdatedBy;

    @Column(name = "LastUpdatedDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date lastUpdatedDate;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "IsUpdated")
    private Boolean isUpdated;

    @Column(name = "IsDeleted")
    private Boolean isDeleted;

    public Integer getWfhId() { return wfhId; }
    public void setWfhId(Integer wfhId) { this.wfhId = wfhId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getIpAddress() { return ipAddress; }
    public void setIpAddress(String ipAddress) { this.ipAddress = ipAddress; }

    public Date getDate() { return date; }
    public void setDate(Date date) { this.date = date; }

    public Date getLoginTime() { return loginTime; }
    public void setLoginTime(Date loginTime) { this.loginTime = loginTime; }

    public Date getLogOutTime() { return logOutTime; }
    public void setLogOutTime(Date logOutTime) { this.logOutTime = logOutTime; }

    public Date getActivehrs() { return activehrs; }
    public void setActivehrs(Date activehrs) { this.activehrs = activehrs; }

    public Date getAnalysisHr() { return analysisHr; }
    public void setAnalysisHr(Date analysisHr) { this.analysisHr = analysisHr; }

    public Boolean getIsLoggedIn() { return isLoggedIn; }
    public void setIsLoggedIn(Boolean isLoggedIn) { this.isLoggedIn = isLoggedIn; }

    public Boolean getIsLoggedOut() { return isLoggedOut; }
    public void setIsLoggedOut(Boolean isLoggedOut) { this.isLoggedOut = isLoggedOut; }

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
