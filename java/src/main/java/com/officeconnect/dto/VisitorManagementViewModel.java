package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class VisitorManagementViewModel {
    @JsonProperty("VisitorId")
    private Integer visitorId;

    @JsonProperty("VisitId")
    private Integer visitId;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("Name")
    private String visitorName;

    @JsonProperty("Designation")
    private String designation;

    @JsonProperty("Company")
    private String company;

    @JsonProperty("Purpose")
    private String purpose;

    @JsonProperty("PMail")
    private String pMail;

    @JsonProperty("OMail")
    private String oMail;

    @JsonProperty("Mobile")
    private String mobile;

    @JsonProperty("AMobile")
    private String aMobile;

    @JsonProperty("Photo")
    private String photo;

    @JsonProperty("CompId")
    private String compId;

    @JsonProperty("CompName")
    private String compName;

    @JsonProperty("Accessories")
    private String accessories;

    @JsonProperty("WhomtoMeet")
    private Integer whomtoMeet;

    @JsonProperty("WName")
    private String wName;

    @JsonProperty("WEmpCode")
    private String wEmpCode;

    @JsonProperty("Date")
    private String visitDateStr;

    private java.util.Date visitDate;

    @JsonProperty("Time")
    private String visitTime;

    @JsonProperty("Invited")
    private Boolean invited;

    @JsonProperty("Accept")
    private Boolean accept;

    @JsonProperty("Approved")
    private Boolean approved;

    @JsonProperty("Expired")
    private Boolean expired;

    @JsonProperty("DirectCheckIn")
    private Boolean directCheckIn;

    @JsonProperty("CheckIn")
    private java.util.Date checkIn;

    @JsonProperty("CheckOut")
    private java.util.Date checkOut;

    @JsonProperty("IdCard")
    private String idCard;

    @JsonProperty("Status")
    private String status;

    @JsonProperty("IsActive")
    private Boolean isActive;

    @JsonProperty("msg")
    private String msg;

    public Integer getVisitorId() { return visitorId; }
    public void setVisitorId(Integer visitorId) { this.visitorId = visitorId; }
    public Integer getVisitId() { return visitId; }
    public void setVisitId(Integer visitId) { this.visitId = visitId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
    public String getVisitorName() { return visitorName; }
    public void setVisitorName(String visitorName) { this.visitorName = visitorName; }
    public String getDesignation() { return designation; }
    public void setDesignation(String designation) { this.designation = designation; }
    public String getCompany() { return company; }
    public void setCompany(String company) { this.company = company; }
    public String getPurpose() { return purpose; }
    public void setPurpose(String purpose) { this.purpose = purpose; }
    public String getPMail() { return pMail; }
    public void setPMail(String pMail) { this.pMail = pMail; }
    public String getOMail() { return oMail; }
    public void setOMail(String oMail) { this.oMail = oMail; }
    public String getMobile() { return mobile; }
    public void setMobile(String mobile) { this.mobile = mobile; }
    public String getAMobile() { return aMobile; }
    public void setAMobile(String aMobile) { this.aMobile = aMobile; }
    public String getPhoto() { return photo; }
    public void setPhoto(String photo) { this.photo = photo; }
    public String getCompId() { return compId; }
    public void setCompId(String compId) { this.compId = compId; }
    public String getCompName() { return compName; }
    public void setCompName(String compName) { this.compName = compName; }
    public String getAccessories() { return accessories; }
    public void setAccessories(String accessories) { this.accessories = accessories; }
    public Integer getWhomtoMeet() { return whomtoMeet; }
    public void setWhomtoMeet(Integer whomtoMeet) { this.whomtoMeet = whomtoMeet; }
    public String getWName() { return wName; }
    public void setWName(String wName) { this.wName = wName; }
    public String getWEmpCode() { return wEmpCode; }
    public void setWEmpCode(String wEmpCode) { this.wEmpCode = wEmpCode; }
    public String getVisitDateStr() { return visitDateStr; }
    public void setVisitDateStr(String visitDateStr) { this.visitDateStr = visitDateStr; }

    public java.util.Date getVisitDate() { return visitDate; }
    public void setVisitDate(java.util.Date visitDate) { this.visitDate = visitDate; }
    public String getVisitTime() { return visitTime; }
    public void setVisitTime(String visitTime) { this.visitTime = visitTime; }
    public Boolean getInvited() { return invited; }
    public void setInvited(Boolean invited) { this.invited = invited; }
    public Boolean getAccept() { return accept; }
    public void setAccept(Boolean accept) { this.accept = accept; }
    public Boolean getApproved() { return approved; }
    public void setApproved(Boolean approved) { this.approved = approved; }
    public Boolean getExpired() { return expired; }
    public void setExpired(Boolean expired) { this.expired = expired; }
    public Boolean getDirectCheckIn() { return directCheckIn; }
    public void setDirectCheckIn(Boolean directCheckIn) { this.directCheckIn = directCheckIn; }
    public java.util.Date getCheckIn() { return checkIn; }
    public void setCheckIn(java.util.Date checkIn) { this.checkIn = checkIn; }
    public java.util.Date getCheckOut() { return checkOut; }
    public void setCheckOut(java.util.Date checkOut) { this.checkOut = checkOut; }
    public String getIdCard() { return idCard; }
    public void setIdCard(String idCard) { this.idCard = idCard; }
    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }
    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }
    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
