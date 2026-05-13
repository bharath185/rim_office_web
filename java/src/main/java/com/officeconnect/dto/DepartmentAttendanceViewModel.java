package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class DepartmentAttendanceViewModel {

    @JsonProperty("DeptName")
    private String deptName;

    @JsonProperty("DeptShortName")
    private String deptShortName;

    @JsonProperty("Total")
    private Integer total;

    @JsonProperty("OverAllAbsentPercentage")
    private String overAllAbsentPercentage;

    @JsonProperty("Present")
    private Integer present;

    @JsonProperty("Absent")
    private Integer absent;

    @JsonProperty("Leave")
    private Integer leave;

    @JsonProperty("AbsentPesent")
    private String absentPesent;

    @JsonProperty("IsHoliday")
    private Boolean isHoliday;

    public String getDeptName() { return deptName; }
    public void setDeptName(String deptName) { this.deptName = deptName; }

    public String getDeptShortName() { return deptShortName; }
    public void setDeptShortName(String deptShortName) { this.deptShortName = deptShortName; }

    public Integer getTotal() { return total; }
    public void setTotal(Integer total) { this.total = total; }

    public String getOverAllAbsentPercentage() { return overAllAbsentPercentage; }
    public void setOverAllAbsentPercentage(String overAllAbsentPercentage) { this.overAllAbsentPercentage = overAllAbsentPercentage; }

    public Integer getPresent() { return present; }
    public void setPresent(Integer present) { this.present = present; }

    public Integer getAbsent() { return absent; }
    public void setAbsent(Integer absent) { this.absent = absent; }

    public Integer getLeave() { return leave; }
    public void setLeave(Integer leave) { this.leave = leave; }

    public String getAbsentPesent() { return absentPesent; }
    public void setAbsentPesent(String absentPesent) { this.absentPesent = absentPesent; }

    public Boolean getIsHoliday() { return isHoliday; }
    public void setIsHoliday(Boolean isHoliday) { this.isHoliday = isHoliday; }
}
