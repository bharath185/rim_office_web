package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.List;

public class PayrollResponseViewModel {
    @JsonProperty("Company")
    private CompanyInfoViewModel company;

    @JsonProperty("SalaryMonth")
    private String salaryMonth;

    @JsonProperty("Year")
    private String year;

    @JsonProperty("EmployeeDetails")
    private EmployeeInfoDetailsViewModel employeeDetails;

    @JsonProperty("PayslipSections")
    private List<SectionResponseViewModel> payslipSections;

    @JsonProperty("ArrearSections")
    private List<SectionResponseViewModel> arrearSections;

    @JsonProperty("VariableSections")
    private List<SalaryComponentViewModel> variableSections;

    @JsonProperty("DescriptionforArrear")
    private String descriptionforArrear;

    @JsonProperty("Status")
    private Integer status;

    @JsonProperty("msg")
    private String msg;

    public CompanyInfoViewModel getCompany() { return company; }
    public void setCompany(CompanyInfoViewModel company) { this.company = company; }
    public String getSalaryMonth() { return salaryMonth; }
    public void setSalaryMonth(String salaryMonth) { this.salaryMonth = salaryMonth; }
    public String getYear() { return year; }
    public void setYear(String year) { this.year = year; }
    public EmployeeInfoDetailsViewModel getEmployeeDetails() { return employeeDetails; }
    public void setEmployeeDetails(EmployeeInfoDetailsViewModel employeeDetails) { this.employeeDetails = employeeDetails; }
    public List<SectionResponseViewModel> getPayslipSections() { return payslipSections; }
    public void setPayslipSections(List<SectionResponseViewModel> payslipSections) { this.payslipSections = payslipSections; }
    public List<SectionResponseViewModel> getArrearSections() { return arrearSections; }
    public void setArrearSections(List<SectionResponseViewModel> arrearSections) { this.arrearSections = arrearSections; }
    public List<SalaryComponentViewModel> getVariableSections() { return variableSections; }
    public void setVariableSections(List<SalaryComponentViewModel> variableSections) { this.variableSections = variableSections; }
    public String getDescriptionforArrear() { return descriptionforArrear; }
    public void setDescriptionforArrear(String descriptionforArrear) { this.descriptionforArrear = descriptionforArrear; }
    public Integer getStatus() { return status; }
    public void setStatus(Integer status) { this.status = status; }
    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
