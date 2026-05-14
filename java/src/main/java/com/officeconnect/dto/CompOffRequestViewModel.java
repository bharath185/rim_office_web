package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import java.util.Date;

public class CompOffRequestViewModel {
    @JsonProperty("CompOffReqId")
    @JsonAlias({"compOffReqId", "CompOffReqId", "CompOffId", "compOffId"})
    private Integer compOffReqId;

    @JsonProperty("LoginId")
    @JsonAlias({"loginId", "LoginId"})
    private Integer loginId;

    @JsonProperty("EmpId")
    @JsonAlias({"empId", "EmpId"})
    private Integer empId;

    @JsonProperty("EmpCode")
    @JsonAlias({"empCode", "EmpCode"})
    private String empCode;

    @JsonProperty("ManagerId")
    @JsonAlias({"managerId", "ManagerId"})
    private Integer managerId;

    @JsonProperty("ManagerCode")
    @JsonAlias({"managerCode", "ManagerCode"})
    private String managerCode;

    @JsonProperty("Date")
    @JsonAlias({"date", "Date"})
    @JsonDeserialize(using = NetDateTimeDeserializer.class)
    private Date date;

    @JsonProperty("ProjectId")
    @JsonAlias({"projectId", "ProjectId"})
    private Integer projectId;

    @JsonProperty("Project")
    @JsonAlias({"project", "Project"})
    private String project;

    @JsonProperty("TaskId")
    @JsonAlias({"taskId", "TaskId"})
    private Integer taskId;

    @JsonProperty("Task")
    @JsonAlias({"task", "Task"})
    private String task;

    @JsonProperty("ActualHrs")
    @JsonAlias({"actualHrs", "ActualHrs"})
    private String actualHrs;

    @JsonProperty("Hrs")
    @JsonAlias({"hrs", "Hrs"})
    private Double hrs;

    @JsonProperty("WorkMode")
    @JsonAlias({"workMode", "WorkMode"})
    private String workMode;

    @JsonProperty("IsRequested")
    @JsonAlias({"isRequested", "IsRequested"})
    private Boolean isRequested;

    @JsonProperty("IsApproved")
    @JsonAlias({"isApproved", "IsApproved"})
    private Boolean isApproved;

    @JsonProperty("IsRejected")
    @JsonAlias({"isRejected", "IsRejected"})
    private Boolean isRejected;

    @JsonProperty("Reason")
    @JsonAlias({"reason", "Reason"})
    private String reason;

    @JsonProperty("IsActive")
    @JsonAlias({"isActive", "IsActive"})
    private Boolean isActive;

    @JsonProperty("CreatedDate")
    @JsonAlias({"createdDate", "CreatedDate", "AppliedDate"})
    private Date appliedDate;

    @JsonProperty("EmpName")
    @JsonAlias({"empName", "EmpName", "EmployeeName"})
    private String employeeName;

    @JsonProperty("msg")
    @JsonAlias({"msg", "Msg"})
    private String msg;

    // Getters and Setters
    public Integer getCompOffReqId() { return compOffReqId; }
    public void setCompOffReqId(Integer compOffReqId) { this.compOffReqId = compOffReqId; }

    public Integer getLoginId() { return loginId; }
    public void setLoginId(Integer loginId) { this.loginId = loginId; }

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

    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }

    public Date getAppliedDate() { return appliedDate; }
    public void setAppliedDate(Date appliedDate) { this.appliedDate = appliedDate; }

    public String getEmployeeName() { return employeeName; }
    public void setEmployeeName(String employeeName) { this.employeeName = employeeName; }

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }
}
