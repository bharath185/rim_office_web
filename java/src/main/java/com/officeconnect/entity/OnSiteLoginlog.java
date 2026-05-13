package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "OnSiteLoginlog")
public class OnSiteLoginlog {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "EmpId")
    private Integer empId;

    @Column(name = "EmpCode")
    private String empCode;

    @Column(name = "Company")
    private String company;

    @Column(name = "LoginAddress")
    private String loginAddress;

    @Column(name = "LoginCity")
    private String loginCity;

    @Column(name = "LoginDate")
    @Temporal(TemporalType.DATE)
    private Date loginDate;

    @Column(name = "LoginLongitude")
    private String loginLongitude;

    @Column(name = "LoginLatitude")
    private String loginLatitude;

    @Column(name = "LogInTime")
    @Temporal(TemporalType.TIME)
    private Date logInTime;

    @Column(name = "LogoutAddress")
    private String logoutAddress;

    @Column(name = "LogoutCity")
    private String logoutCity;

    @Column(name = "Purpose")
    private String purpose;

    @Column(name = "Description")
    private String description;

    @Column(name = "LogoutDate")
    @Temporal(TemporalType.DATE)
    private Date logoutDate;

    @Column(name = "LogoutLongitude")
    private String logoutLongitude;

    @Column(name = "LogoutLatitude")
    private String logoutLatitude;

    @Column(name = "LogOutTime")
    @Temporal(TemporalType.TIME)
    private Date logOutTime;

    @Column(name = "ActiveHrs")
    @Temporal(TemporalType.TIME)
    private Date activeHrs;

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

    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getCompany() { return company; }
    public void setCompany(String company) { this.company = company; }

    public String getLoginAddress() { return loginAddress; }
    public void setLoginAddress(String loginAddress) { this.loginAddress = loginAddress; }

    public String getLoginCity() { return loginCity; }
    public void setLoginCity(String loginCity) { this.loginCity = loginCity; }

    public Date getLoginDate() { return loginDate; }
    public void setLoginDate(Date loginDate) { this.loginDate = loginDate; }

    public String getLoginLongitude() { return loginLongitude; }
    public void setLoginLongitude(String loginLongitude) { this.loginLongitude = loginLongitude; }

    public String getLoginLatitude() { return loginLatitude; }
    public void setLoginLatitude(String loginLatitude) { this.loginLatitude = loginLatitude; }

    public Date getLogInTime() { return logInTime; }
    public void setLogInTime(Date logInTime) { this.logInTime = logInTime; }

    public String getLogoutAddress() { return logoutAddress; }
    public void setLogoutAddress(String logoutAddress) { this.logoutAddress = logoutAddress; }

    public String getLogoutCity() { return logoutCity; }
    public void setLogoutCity(String logoutCity) { this.logoutCity = logoutCity; }

    public String getPurpose() { return purpose; }
    public void setPurpose(String purpose) { this.purpose = purpose; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public Date getLogoutDate() { return logoutDate; }
    public void setLogoutDate(Date logoutDate) { this.logoutDate = logoutDate; }

    public String getLogoutLongitude() { return logoutLongitude; }
    public void setLogoutLongitude(String logoutLongitude) { this.logoutLongitude = logoutLongitude; }

    public String getLogoutLatitude() { return logoutLatitude; }
    public void setLogoutLatitude(String logoutLatitude) { this.logoutLatitude = logoutLatitude; }

    public Date getLogOutTime() { return logOutTime; }
    public void setLogOutTime(Date logOutTime) { this.logOutTime = logOutTime; }

    public Date getActiveHrs() { return activeHrs; }
    public void setActiveHrs(Date activeHrs) { this.activeHrs = activeHrs; }

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
