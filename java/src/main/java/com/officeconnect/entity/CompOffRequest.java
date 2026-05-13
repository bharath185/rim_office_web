package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "CompOffRequest")
public class CompOffRequest {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "CompOffReqId")
    private Integer compOffReqId;

    @Column(name = "EmpId")
    private Integer empId;

    @Column(name = "EmpCode")
    private String empCode;

    @Column(name = "ManagerId")
    private Integer managerId;

    @Column(name = "ManagerCode")
    private String managerCode;

    @Column(name = "Date")
    @Temporal(TemporalType.DATE)
    private Date date;

    @Column(name = "ProjectId")
    private Integer projectId;

    @Column(name = "Project")
    private String project;

    @Column(name = "TaskId")
    private Integer taskId;

    @Column(name = "Task")
    private String task;

    @Column(name = "ActualHrs")
    private String actualHrs;

    @Column(name = "Hrs")
    private Double hrs;

    @Column(name = "WorkMode")
    private String workMode;

    @Column(name = "IsRequested")
    private Boolean isRequested;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "IsRejected")
    private Boolean isRejected;

    @Column(name = "Reason")
    private String reason;

    @Column(name = "CreatedBy")
    private Integer createdBy;

    @Column(name = "CreatedDate")
    @Temporal(TemporalType.DATE)
    private Date createdDate;

    @Column(name = "LastUpdatedBy")
    private Integer lastUpdatedBy;

    @Column(name = "LastUpdatedDate")
    @Temporal(TemporalType.DATE)
    private Date lastUpdatedDate;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "IsUpdated")
    private Boolean isUpdated;

    @Column(name = "IsDeleted")
    private Boolean isDeleted;

    @Column(name = "IsUsed")
    private Boolean isUsed;

    // Getters and Setters
    public Integer getCompOffReqId() { return compOffReqId; }
    public void setCompOffReqId(Integer compOffReqId) { this.compOffReqId = compOffReqId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public Integer getManagerId() { return managerId; }
    public void setManagerId(Integer managerId) { this.managerId = managerId; }

    public String getManagerCode() { return managerCode; }
    public void setManagerCode(String managerCode) { this.managerCode = managerCode; }

    public Date getDate() { return date; }
    public void setDate(Date date) { this.date = date; }

    public Integer getProjectId() { return projectId; }
    public void setProjectId(Integer projectId) { this.projectId = projectId; }

    public String getProject() { return project; }
    public void setProject(String project) { this.project = project; }

    public Integer getTaskId() { return taskId; }
    public void setTaskId(Integer taskId) { this.taskId = taskId; }

    public String getTask() { return task; }
    public void setTask(String task) { this.task = task; }

    public String getActualHrs() { return actualHrs; }
    public void setActualHrs(String actualHrs) { this.actualHrs = actualHrs; }

    public Double getHrs() { return hrs; }
    public void setHrs(Double hrs) { this.hrs = hrs; }

    public String getWorkMode() { return workMode; }
    public void setWorkMode(String workMode) { this.workMode = workMode; }

    public Boolean getIsRequested() { return isRequested; }
    public void setIsRequested(Boolean isRequested) { this.isRequested = isRequested; }

    public Boolean getIsApproved() { return isApproved; }
    public void setIsApproved(Boolean isApproved) { this.isApproved = isApproved; }

    public Boolean getIsRejected() { return isRejected; }
    public void setIsRejected(Boolean isRejected) { this.isRejected = isRejected; }

    public String getReason() { return reason; }
    public void setReason(String reason) { this.reason = reason; }

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

    public Boolean getIsUsed() { return isUsed; }
    public void setIsUsed(Boolean isUsed) { this.isUsed = isUsed; }
}