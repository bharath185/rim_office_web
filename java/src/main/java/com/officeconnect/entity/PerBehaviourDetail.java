package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "Per_BehaviourDetail")
public class PerBehaviourDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "QId")
    private Integer qId;

    @Column(name = "PeriodId")
    private Integer periodId;

    @Column(name = "EmpId")
    private Integer empId;

    @Column(name = "BehaviourId")
    private Integer behaviourId;

    @Column(name = "Behaviour")
    private String behaviour;

    @Column(name = "Description")
    private String description;

    @Column(name = "Weightage")
    private String weightage;

    @Column(name = "EmpReview")
    private String empReview;

    @Column(name = "ManagerReview")
    private String managerReview;

    @Column(name = "EDescription")
    private String eDescription;

    @Column(name = "MDescription")
    private String mDescription;

    @Column(name = "ReviewedByEmp")
    private Boolean reviewedByEmp;

    @Column(name = "ReviewedByManager")
    private Boolean reviewedByManager;

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
    public Integer getQId() { return qId; }
    public void setQId(Integer qId) { this.qId = qId; }
    public Integer getPeriodId() { return periodId; }
    public void setPeriodId(Integer periodId) { this.periodId = periodId; }
    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }
    public Integer getBehaviourId() { return behaviourId; }
    public void setBehaviourId(Integer behaviourId) { this.behaviourId = behaviourId; }
    public String getBehaviour() { return behaviour; }
    public void setBehaviour(String behaviour) { this.behaviour = behaviour; }
    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }
    public String getWeightage() { return weightage; }
    public void setWeightage(String weightage) { this.weightage = weightage; }
    public String getEmpReview() { return empReview; }
    public void setEmpReview(String empReview) { this.empReview = empReview; }
    public String getManagerReview() { return managerReview; }
    public void setManagerReview(String managerReview) { this.managerReview = managerReview; }
    public String getEDescription() { return eDescription; }
    public void setEDescription(String eDescription) { this.eDescription = eDescription; }
    public String getMDescription() { return mDescription; }
    public void setMDescription(String mDescription) { this.mDescription = mDescription; }
    public Boolean getReviewedByEmp() { return reviewedByEmp; }
    public void setReviewedByEmp(Boolean reviewedByEmp) { this.reviewedByEmp = reviewedByEmp; }
    public Boolean getReviewedByManager() { return reviewedByManager; }
    public void setReviewedByManager(Boolean reviewedByManager) { this.reviewedByManager = reviewedByManager; }
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
