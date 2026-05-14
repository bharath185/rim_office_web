package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "VariableHistory")
public class VariableHistory {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "VariableHistoryId")
    private Integer variableHistoryId;

    @Column(name = "EmpId")
    private Integer empId;

    @Column(name = "EmpCode")
    private String empCode;

    @Column(name = "EmpCTC")
    private String empCtc;

    @Column(name = "VariableId")
    private Integer variableId;

    @Column(name = "VariableName")
    private String variableName;

    @Column(name = "VariableCode")
    private String variableCode;

    @Column(name = "VariableAmt")
    private String variableAmt;

    @Column(name = "Year")
    private Integer year;

    @Column(name = "Month")
    private Integer month;

    @Column(name = "Status")
    private Boolean status;

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

    public Integer getVariableHistoryId() { return variableHistoryId; }
    public void setVariableHistoryId(Integer variableHistoryId) { this.variableHistoryId = variableHistoryId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
    public String getEmpCtc() { return empCtc; }
    public void setEmpCtc(String empCtc) { this.empCtc = empCtc; }
    public Integer getVariableId() { return variableId; }
    public void setVariableId(Integer variableId) { this.variableId = variableId; }
    public String getVariableName() { return variableName; }
    public void setVariableName(String variableName) { this.variableName = variableName; }
    public String getVariableCode() { return variableCode; }
    public void setVariableCode(String variableCode) { this.variableCode = variableCode; }
    public String getVariableAmt() { return variableAmt; }
    public void setVariableAmt(String variableAmt) { this.variableAmt = variableAmt; }
    public Integer getYear() { return year; }
    public void setYear(Integer year) { this.year = year; }
    public Integer getMonth() { return month; }
    public void setMonth(Integer month) { this.month = month; }
    public Boolean getStatus() { return status; }
    public void setStatus(Boolean status) { this.status = status; }
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
