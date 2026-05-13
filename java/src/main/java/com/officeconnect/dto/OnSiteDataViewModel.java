package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.Date;

@JsonInclude(JsonInclude.Include.ALWAYS)
public class OnSiteDataViewModel {

    @JsonProperty("LoginId")
    private Integer loginId;

    @JsonProperty("Id")
    private Integer id;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("Company")
    private String company;

    @JsonProperty("LoginAddress")
    private String loginAddress;

    @JsonProperty("LoginCity")
    private String loginCity;

    @JsonProperty("LoginDate")
    private Date loginDate;

    @JsonProperty("LoginLongitude")
    private String loginLongitude;

    @JsonProperty("LoginLatitude")
    private String loginLatitude;

    @JsonProperty("Purpose")
    private String purpose;

    @JsonProperty("Description")
    private String description;

    @JsonProperty("LogInTime")
    private Object logInTime;

    @JsonProperty("LogoutAddress")
    private String logoutAddress;

    @JsonProperty("LogoutCity")
    private String logoutCity;

    @JsonProperty("LogoutDate")
    private Date logoutDate;

    @JsonProperty("LogoutLongitude")
    private String logoutLongitude;

    @JsonProperty("LogoutLatitude")
    private String logoutLatitude;

    @JsonProperty("LogOutTime")
    private Object logOutTime;

    @JsonProperty("ActiveHrs")
    private Object activeHrs;

    @JsonProperty("WorkStatus")
    private String workStatus;

    @JsonProperty("CreatedBy")
    private Integer createdBy;

    @JsonProperty("CreatedDate")
    private Date createdDate;

    @JsonProperty("LastUpdatedBy")
    private Integer lastUpdatedBy;

    @JsonProperty("LastUpdatedDate")
    private Date lastUpdatedDate;

    @JsonProperty("IsActive")
    private Boolean isActive;

    @JsonProperty("IsUpdated")
    private Boolean isUpdated;

    @JsonProperty("IsDeleted")
    private Boolean isDeleted;

    @JsonProperty("msg")
    private String msg;

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

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

    public String getPurpose() { return purpose; }
    public void setPurpose(String purpose) { this.purpose = purpose; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public Object getLogInTime() { return logInTime; }
    public void setLogInTime(Object logInTime) { this.logInTime = logInTime; }

    public String getLogoutAddress() { return logoutAddress; }
    public void setLogoutAddress(String logoutAddress) { this.logoutAddress = logoutAddress; }

    public String getLogoutCity() { return logoutCity; }
    public void setLogoutCity(String logoutCity) { this.logoutCity = logoutCity; }

    public Date getLogoutDate() { return logoutDate; }
    public void setLogoutDate(Date logoutDate) { this.logoutDate = logoutDate; }

    public String getLogoutLongitude() { return logoutLongitude; }
    public void setLogoutLongitude(String logoutLongitude) { this.logoutLongitude = logoutLongitude; }

    public String getLogoutLatitude() { return logoutLatitude; }
    public void setLogoutLatitude(String logoutLatitude) { this.logoutLatitude = logoutLatitude; }

    public Object getLogOutTime() { return logOutTime; }
    public void setLogOutTime(Object logOutTime) { this.logOutTime = logOutTime; }

    public Object getActiveHrs() { return activeHrs; }
    public void setActiveHrs(Object activeHrs) { this.activeHrs = activeHrs; }

    public String getWorkStatus() { return workStatus; }
    public void setWorkStatus(String workStatus) { this.workStatus = workStatus; }

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

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}