package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.List;

public class ShiftGroupingMasterViewModel {
    @JsonProperty("SGId")
    private Integer sgId;
    @JsonProperty("CompId")
    private Integer compId;
    @JsonProperty("Company")
    private String company;
    @JsonProperty("LEId")
    private Integer leId;
    @JsonProperty("LegalEntity")
    private String legalEntity;
    @JsonProperty("BUId")
    private Integer buId;
    @JsonProperty("BusinessUnit")
    private String businessUnit;
    @JsonProperty("LocationId")
    private Integer locationId;
    @JsonProperty("Location")
    private String location;
    @JsonProperty("Status")
    private Boolean status;
    @JsonProperty("CreatedBy")
    private Integer createdBy;
    @JsonProperty("IsActive")
    private Boolean isActive;
    @JsonProperty("IsUpdated")
    private Boolean isUpdated;
    @JsonProperty("IsDeleted")
    private Boolean isDeleted;
    @JsonProperty("LoginId")
    private Integer loginId;
    @JsonProperty("msg")
    private String msg;
    @JsonProperty("lstOfShift")
    private List<SampleShiftMasterViewModel> lstOfShift;

    public Integer getSgId() { return sgId; }
    public void setSgId(Integer sgId) { this.sgId = sgId; }
    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }
    public String getCompany() { return company; }
    public void setCompany(String company) { this.company = company; }
    public Integer getLeId() { return leId; }
    public void setLeId(Integer leId) { this.leId = leId; }
    public String getLegalEntity() { return legalEntity; }
    public void setLegalEntity(String legalEntity) { this.legalEntity = legalEntity; }
    public Integer getBuId() { return buId; }
    public void setBuId(Integer buId) { this.buId = buId; }
    public String getBusinessUnit() { return businessUnit; }
    public void setBusinessUnit(String businessUnit) { this.businessUnit = businessUnit; }
    public Integer getLocationId() { return locationId; }
    public void setLocationId(Integer locationId) { this.locationId = locationId; }
    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }
    public Boolean getStatus() { return status; }
    public void setStatus(Boolean status) { this.status = status; }
    public Integer getCreatedBy() { return createdBy; }
    public void setCreatedBy(Integer createdBy) { this.createdBy = createdBy; }
    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }
    public Boolean getIsUpdated() { return isUpdated; }
    public void setIsUpdated(Boolean isUpdated) { this.isUpdated = isUpdated; }
    public Boolean getIsDeleted() { return isDeleted; }
    public void setIsDeleted(Boolean isDeleted) { this.isDeleted = isDeleted; }
    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }
    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
    public List<SampleShiftMasterViewModel> getLstOfShift() { return lstOfShift; }
    public void setLstOfShift(List<SampleShiftMasterViewModel> lstOfShift) { this.lstOfShift = lstOfShift; }
}
