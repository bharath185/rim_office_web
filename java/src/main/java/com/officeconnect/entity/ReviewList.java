package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "ReviewList")
public class ReviewList {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ReviewId")
    private Integer reviewId;

    @Column(name = "FYearId")
    private Integer fYearId;

    @Column(name = "QId")
    private Integer qId;

    @Column(name = "EmpId")
    private Integer empId;

    @Column(name = "QType")
    private String qType;

    @Column(name = "Status")
    private String status;

    @Column(name = "ReviewedByEmp")
    private Boolean reviewedByEmp;

    @Column(name = "ReviewedByManager")
    private Boolean reviewedByManager;

    @Column(name = "Completed")
    private Boolean completed;

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

    public Integer getReviewId() { return reviewId; }
    public void setReviewId(Integer reviewId) { this.reviewId = reviewId; }
    public Integer getFYearId() { return fYearId; }
    public void setFYearId(Integer fYearId) { this.fYearId = fYearId; }
    public Integer getQId() { return qId; }
    public void setQId(Integer qId) { this.qId = qId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public String getQType() { return qType; }
    public void setQType(String qType) { this.qType = qType; }
    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }
    public Boolean getReviewedByEmp() { return reviewedByEmp; }
    public void setReviewedByEmp(Boolean reviewedByEmp) { this.reviewedByEmp = reviewedByEmp; }
    public Boolean getReviewedByManager() { return reviewedByManager; }
    public void setReviewedByManager(Boolean reviewedByManager) { this.reviewedByManager = reviewedByManager; }
    public Boolean getCompleted() { return completed; }
    public void setCompleted(Boolean completed) { this.completed = completed; }
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
