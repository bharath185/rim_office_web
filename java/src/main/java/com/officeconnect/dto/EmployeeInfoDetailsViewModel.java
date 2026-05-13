package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.math.BigDecimal;

public class EmployeeInfoDetailsViewModel {
    @JsonProperty("Name")
    private String name;

    @JsonProperty("Designation")
    private String designation;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("Location")
    private String location;

    @JsonProperty("PanNo")
    private String panNo;

    @JsonProperty("BankName")
    private String bankName;

    @JsonProperty("BranchName")
    private String branchName;

    @JsonProperty("IFSCCode")
    private String ifscCode;

    @JsonProperty("BankAccNo")
    private String bankAccNo;

    @JsonProperty("PFNo")
    private String pfNo;

    @JsonProperty("DaysPaid")
    private BigDecimal daysPaid;

    @JsonProperty("UANNo")
    private String uanNo;

    @JsonProperty("LOP")
    private BigDecimal lop;

    @JsonProperty("ESINo")
    private String esiNo;

    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getDesignation() { return designation; }
    public void setDesignation(String designation) { this.designation = designation; }
    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
    public String getLocation() { return location; }
    public void setLocation(String location) { this.location = location; }
    public String getPanNo() { return panNo; }
    public void setPanNo(String panNo) { this.panNo = panNo; }
    public String getBankName() { return bankName; }
    public void setBankName(String bankName) { this.bankName = bankName; }
    public String getBranchName() { return branchName; }
    public void setBranchName(String branchName) { this.branchName = branchName; }
    public String getIfscCode() { return ifscCode; }
    public void setIfscCode(String ifscCode) { this.ifscCode = ifscCode; }
    public String getBankAccNo() { return bankAccNo; }
    public void setBankAccNo(String bankAccNo) { this.bankAccNo = bankAccNo; }
    public String getPfNo() { return pfNo; }
    public void setPfNo(String pfNo) { this.pfNo = pfNo; }
    public BigDecimal getDaysPaid() { return daysPaid; }
    public void setDaysPaid(BigDecimal daysPaid) { this.daysPaid = daysPaid; }
    public String getUanNo() { return uanNo; }
    public void setUanNo(String uanNo) { this.uanNo = uanNo; }
    public BigDecimal getLop() { return lop; }
    public void setLop(BigDecimal lop) { this.lop = lop; }
    public String getEsiNo() { return esiNo; }
    public void setEsiNo(String esiNo) { this.esiNo = esiNo; }
}
