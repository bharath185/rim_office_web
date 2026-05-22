package com.officeconnect.dto;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

@JsonInclude(JsonInclude.Include.ALWAYS)
public class AttendanceViewModel {

    @JsonProperty("WFHDetails")
    private String wfhDetails;

    @JsonProperty("EmpId")
    private Integer empId;

    @JsonProperty("CompId")
    private Integer compId;

    @JsonProperty("CompName")
    private String compName;

    @JsonProperty("Designation")
    private String designation;

    @JsonProperty("DeptName")
    private String deptName;

    @JsonProperty("DeptId")
    private Integer deptId;

    @JsonProperty("DesignationId")
    private Integer designationId;

    @JsonProperty("EmpCode")
    private String empCode;

    @JsonProperty("EmpName")
    private String empName;

    @JsonProperty("LogDate")
    private Object logDate;

    @JsonProperty("WorkingHours")
    private String workingHours;

    @JsonProperty("LogInTime")
    private String logInTime;

    @JsonProperty("LogOutTime")
    private String logOutTime;

    @JsonProperty("ESSLLogInTime")
    private String esslLogInTime;

    @JsonProperty("ESSLLogOutTime")
    private String esslLogOutTime;

    @JsonProperty("WFHLogInTime")
    private String wfhLogInTime;

    @JsonProperty("WFHLogOutTime")
    private String wfhLogOutTime;

    @JsonProperty("ONSITELogInTime")
    private String onsiteLogInTime;

    @JsonProperty("ONSITELogOutTime")
    private String onsiteLogOutTime;

    @JsonProperty("LoginLocation")
    private String loginLocation;

    @JsonProperty("LogoutLocation")
    private String logoutLocation;

    @JsonProperty("ActiveHours")
    private String activeHours;

    @JsonProperty("ESSLActiveHours")
    private String esslActiveHours;

    @JsonProperty("WFHActiveHours")
    private String wfhActiveHours;

    @JsonProperty("ONSITEActiveHours")
    private String onsiteActiveHours;

    @JsonProperty("ShiftName")
    private String shiftName;

    @JsonProperty("LeaveType")
    private String leaveType;

    @JsonProperty("BreakTime")
    private String breakTime;

    @JsonProperty("WorkType")
    private String workType;

    @JsonProperty("IsHoliday")
    private Boolean isHoliday;

    @JsonProperty("HolidayName")
    private String holidayName;

    @JsonProperty("PayDays")
    private Double payDays;

    @JsonProperty("clelcount")
    private Double clelcount;

    @JsonProperty("holirhcount")
    private Double holirhcount;

    @JsonProperty("weekendcount")
    private Double weekendcount;

    @JsonProperty("weekendcount1")
    private Double weekendcount1;

    @JsonProperty("dojsundayCount")
    private Double dojsundayCount;

    @JsonProperty("dojweekendDaysCount")
    private Double dojweekendDaysCount;

    @JsonProperty("totalpaydaycount")
    private Double totalpaydaycount;

    @JsonProperty("lopcount")
    private Double lopcount;

    @JsonProperty("DaysPresent")
    private Integer daysPresent;

    @JsonProperty("SalType")
    private String salType;

    @JsonProperty("Status")
    private String status;

    @JsonProperty("CheckIn")
    private String checkIn;

    @JsonProperty("CheckOut")
    private String checkOut;

    @JsonProperty("TotalHours")
    private String totalHours;

    public String getWfhDetails() { return wfhDetails; }
    public void setWfhDetails(String wfhDetails) { this.wfhDetails = wfhDetails; }

    public Integer getEmpId() { return empId; }
    public void setEmpId(Integer empId) { this.empId = empId; }

    public Integer getCompId() { return compId; }
    public void setCompId(Integer compId) { this.compId = compId; }

    public String getCompName() { return compName; }
    public void setCompName(String compName) { this.compName = compName; }

    public String getDesignation() { return designation; }
    public void setDesignation(String designation) { this.designation = designation; }

    public String getDeptName() { return deptName; }
    public void setDeptName(String deptName) { this.deptName = deptName; }

    public Integer getDeptId() { return deptId; }
    public void setDeptId(Integer deptId) { this.deptId = deptId; }

    public Integer getDesignationId() { return designationId; }
    public void setDesignationId(Integer designationId) { this.designationId = designationId; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }

    public String getEmpName() { return empName; }
    public void setEmpName(String empName) { this.empName = empName; }

    public Object getLogDate() { return logDate; }
    public void setLogDate(Object logDate) { this.logDate = logDate; }

    public String getWorkingHours() { return workingHours; }
    public void setWorkingHours(String workingHours) { this.workingHours = workingHours; }

    public String getLogInTime() { return logInTime; }
    public void setLogInTime(String logInTime) { this.logInTime = logInTime; }

    public String getLogOutTime() { return logOutTime; }
    public void setLogOutTime(String logOutTime) { this.logOutTime = logOutTime; }

    public String getEsslLogInTime() { return esslLogInTime; }
    public void setEsslLogInTime(String esslLogInTime) { this.esslLogInTime = esslLogInTime; }

    public String getEsslLogOutTime() { return esslLogOutTime; }
    public void setEsslLogOutTime(String esslLogOutTime) { this.esslLogOutTime = esslLogOutTime; }

    public String getWfhLogInTime() { return wfhLogInTime; }
    public void setWfhLogInTime(String wfhLogInTime) { this.wfhLogInTime = wfhLogInTime; }

    public String getWfhLogOutTime() { return wfhLogOutTime; }
    public void setWfhLogOutTime(String wfhLogOutTime) { this.wfhLogOutTime = wfhLogOutTime; }

    public String getOnsiteLogInTime() { return onsiteLogInTime; }
    public void setOnsiteLogInTime(String onsiteLogInTime) { this.onsiteLogInTime = onsiteLogInTime; }

    public String getOnsiteLogOutTime() { return onsiteLogOutTime; }
    public void setOnsiteLogOutTime(String onsiteLogOutTime) { this.onsiteLogOutTime = onsiteLogOutTime; }

    public String getLoginLocation() { return loginLocation; }
    public void setLoginLocation(String loginLocation) { this.loginLocation = loginLocation; }

    public String getLogoutLocation() { return logoutLocation; }
    public void setLogoutLocation(String logoutLocation) { this.logoutLocation = logoutLocation; }

    public String getActiveHours() { return activeHours; }
    public void setActiveHours(String activeHours) { this.activeHours = activeHours; }

    public String getEsslActiveHours() { return esslActiveHours; }
    public void setEsslActiveHours(String esslActiveHours) { this.esslActiveHours = esslActiveHours; }

    public String getWfhActiveHours() { return wfhActiveHours; }
    public void setWfhActiveHours(String wfhActiveHours) { this.wfhActiveHours = wfhActiveHours; }

    public String getOnsiteActiveHours() { return onsiteActiveHours; }
    public void setOnsiteActiveHours(String onsiteActiveHours) { this.onsiteActiveHours = onsiteActiveHours; }

    public String getShiftName() { return shiftName; }
    public void setShiftName(String shiftName) { this.shiftName = shiftName; }

    public String getLeaveType() { return leaveType; }
    public void setLeaveType(String leaveType) { this.leaveType = leaveType; }

    public String getBreakTime() { return breakTime; }
    public void setBreakTime(String breakTime) { this.breakTime = breakTime; }

    public String getWorkType() { return workType; }
    public void setWorkType(String workType) { this.workType = workType; }

    public Boolean getIsHoliday() { return isHoliday; }
    public void setIsHoliday(Boolean isHoliday) { this.isHoliday = isHoliday; }

    public String getHolidayName() { return holidayName; }
    public void setHolidayName(String holidayName) { this.holidayName = holidayName; }

    public Double getPayDays() { return payDays; }
    public void setPayDays(Double payDays) { this.payDays = payDays; }

    public Double getClelcount() { return clelcount; }
    public void setClelcount(Double clelcount) { this.clelcount = clelcount; }

    public Double getHolirhcount() { return holirhcount; }
    public void setHolirhcount(Double holirhcount) { this.holirhcount = holirhcount; }

    public Double getWeekendcount() { return weekendcount; }
    public void setWeekendcount(Double weekendcount) { this.weekendcount = weekendcount; }

    public Double getWeekendcount1() { return weekendcount1; }
    public void setWeekendcount1(Double weekendcount1) { this.weekendcount1 = weekendcount1; }

    public Double getDojsundayCount() { return dojsundayCount; }
    public void setDojsundayCount(Double dojsundayCount) { this.dojsundayCount = dojsundayCount; }

    public Double getDojweekendDaysCount() { return dojweekendDaysCount; }
    public void setDojweekendDaysCount(Double dojweekendDaysCount) { this.dojweekendDaysCount = dojweekendDaysCount; }

    public Double getTotalpaydaycount() { return totalpaydaycount; }
    public void setTotalpaydaycount(Double totalpaydaycount) { this.totalpaydaycount = totalpaydaycount; }

    public Double getLopcount() { return lopcount; }
    public void setLopcount(Double lopcount) { this.lopcount = lopcount; }

    public Integer getDaysPresent() { return daysPresent; }
    public void setDaysPresent(Integer daysPresent) { this.daysPresent = daysPresent; }

    public String getSalType() { return salType; }
    public void setSalType(String salType) { this.salType = salType; }

    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }

    public String getCheckIn() { return checkIn; }
    public void setCheckIn(String checkIn) { this.checkIn = checkIn; }

    public String getCheckOut() { return checkOut; }
    public void setCheckOut(String checkOut) { this.checkOut = checkOut; }

    public String getTotalHours() { return totalHours; }
    public void setTotalHours(String totalHours) { this.totalHours = totalHours; }
}
