package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

public class ApproveLeaveViewModel {
    @JsonProperty("EmpLeaveId")
    @JsonAlias({"empLeaveId", "EmpLeaveId"})
    private Integer empLeaveId;

    @JsonProperty("EmpId")
    @JsonAlias({"empId", "EmpId"})
    private Integer empId;

    @JsonProperty("Status")
    @JsonAlias({"status", "Status"})
    private String status;

    @JsonProperty("ApprovedBy")
    @JsonAlias({"approvedBy", "ApprovedBy"})
    private Integer approvedBy;

    @JsonProperty("msg")
    @JsonAlias({"msg", "Msg"})
    private String msg;

    @JsonProperty("lstofLevAppId")
    @JsonAlias({"lstofLevAppId", "LstofLevAppId"})
    private java.util.List<LeaveAppIdItem> lstofLevAppId;

    @JsonProperty("ApprovedIds")
    @JsonAlias({"approvedIds", "ApprovedIds"})
    private java.util.List<Integer> approvedIds;

    @JsonProperty("FailedIds")
    @JsonAlias({"failedIds", "FailedIds"})
    private java.util.List<Integer> failedIds;

    @JsonProperty("Errors")
    @JsonAlias({"errors", "Errors"})
    private java.util.List<String> errors;

    @JsonProperty("lstofCompOffReqId")
    @JsonAlias({"lstofCompOffReqId", "LstofCompOffReqId"})
    private java.util.List<CompOffReqIdItem> lstofCompOffReqId;

    public Integer getEmpLeaveId() { return empLeaveId; }
    public void setEmpLeaveId(Integer empLeaveId) { this.empLeaveId = empLeaveId; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }

    public Integer getApprovedBy() { return approvedBy; }
    public void setApprovedBy(Integer approvedBy) { this.approvedBy = approvedBy; }

    public String getMsg() { return msg; }
    public void setMsg(String msg) { this.msg = msg; }

    public java.util.List<LeaveAppIdItem> getLstofLevAppId() { return lstofLevAppId; }
    public void setLstofLevAppId(java.util.List<LeaveAppIdItem> lstofLevAppId) { this.lstofLevAppId = lstofLevAppId; }

    public java.util.List<Integer> getApprovedIds() { return approvedIds; }
    public void setApprovedIds(java.util.List<Integer> approvedIds) { this.approvedIds = approvedIds; }

    public java.util.List<Integer> getFailedIds() { return failedIds; }
    public void setFailedIds(java.util.List<Integer> failedIds) { this.failedIds = failedIds; }

    public java.util.List<String> getErrors() { return errors; }
    public void setErrors(java.util.List<String> errors) { this.errors = errors; }

    public java.util.List<CompOffReqIdItem> getLstofCompOffReqId() { return lstofCompOffReqId; }
    public void setLstofCompOffReqId(java.util.List<CompOffReqIdItem> lstofCompOffReqId) { this.lstofCompOffReqId = lstofCompOffReqId; }

    public static class LeaveAppIdItem {
        @JsonProperty("LeaveAppId")
        @JsonAlias({"leaveAppId", "LeaveAppId"})
        private Integer leaveAppId;

        @JsonProperty("Remarks")
        @JsonAlias({"remarks", "Remarks"})
        private String remarks;

        public Integer getLeaveAppId() { return leaveAppId; }
        public void setLeaveAppId(Integer leaveAppId) { this.leaveAppId = leaveAppId; }

        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    public static class CompOffReqIdItem {
        @JsonProperty("CompOffReqId")
        @JsonAlias({"compOffReqId", "CompOffReqId"})
        private Integer compOffReqId;

        @JsonProperty("Remarks")
        @JsonAlias({"remarks", "Remarks"})
        private String remarks;

        public Integer getCompOffReqId() { return compOffReqId; }
        public void setCompOffReqId(Integer compOffReqId) { this.compOffReqId = compOffReqId; }

        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }
}