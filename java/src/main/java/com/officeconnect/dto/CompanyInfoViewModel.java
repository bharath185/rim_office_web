package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class CompanyInfoViewModel {
    @JsonProperty("CompanyName")
    private String companyName;

    @JsonProperty("CompanyAddress")
    private String companyAddress;

    @JsonProperty("CompanyPhoneNo")
    private String companyPhoneNo;

    @JsonProperty("CompanyFax")
    private String companyFax;

    @JsonProperty("CompanyEmail")
    private String companyEmail;

    public String getCompanyName() { return companyName; }
    public void setCompanyName(String companyName) { this.companyName = companyName; }
    public String getCompanyAddress() { return companyAddress; }
    public void setCompanyAddress(String companyAddress) { this.companyAddress = companyAddress; }
    public String getCompanyPhoneNo() { return companyPhoneNo; }
    public void setCompanyPhoneNo(String companyPhoneNo) { this.companyPhoneNo = companyPhoneNo; }
    public String getCompanyFax() { return companyFax; }
    public void setCompanyFax(String companyFax) { this.companyFax = companyFax; }
    public String getCompanyEmail() { return companyEmail; }
    public void setCompanyEmail(String companyEmail) { this.companyEmail = companyEmail; }
}
