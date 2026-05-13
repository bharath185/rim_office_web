package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class DDLocationViewModel {
    @JsonProperty("LocationId")
    @JsonAlias({"locationId", "LocationId"})
    private Integer locationId;

    @JsonProperty("Location")
    @JsonAlias({"location", "Location"})
    private String location;

    @JsonProperty("CompId")
    @JsonAlias({"compId", "CompId"})
    private Integer compId;

    @JsonProperty("LEId")
    @JsonAlias({"leId", "LeId", "LEId"})
    private Integer leId;

    @JsonProperty("BUId")
    @JsonAlias({"buId", "BuId", "BUId"})
    private Integer buId;

    @JsonProperty("EmpId")
    @JsonAlias({"empId", "EmpId"})
    private Integer empId;

    @JsonProperty("LoginId")
    @JsonAlias({"loginId", "LoginId"})
    private Integer loginId;

    @JsonProperty("AuthorisedEntity")
    @JsonAlias({"authorisedEntity", "AuthorisedEntity"})
    private String authorisedEntity;

    public Integer getLocationId() { return locationId; }
    public void setLocationId(Integer locationId) { this.locationId = locationId; }
    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }
    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }
    public Integer getLeId() { return leId; }
    public void setLeId(Integer leId) { this.leId = leId; }
    public Integer getBuId() { return buId; }
    public void setBuId(Integer buId) { this.buId = buId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }
    public String getAuthorisedEntity() { return authorisedEntity; }
    public void setAuthorisedEntity(String authorisedEntity) { this.authorisedEntity = authorisedEntity; }
}
