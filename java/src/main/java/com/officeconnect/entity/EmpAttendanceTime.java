package com.officeconnect.entity;

import jakarta.persistence.*;
import java.util.Date;

@Entity
@Table(name = "Emp_AttendanceTime")
public class EmpAttendanceTime {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "EmpAttendance")
    private Integer empAttendance;

    @Column(name = "LogId")
    private Integer logId;

    @Column(name = "LogDate")
    @Temporal(TemporalType.DATE)
    private Date logDate;

    @Column(name = "Duration")
    @Temporal(TemporalType.TIMESTAMP)
    private Date duration;

    @Column(name = "AttendHours")
    private Integer attendHours;

    @Column(name = "AttendMins")
    private Integer attendMins;

    @Column(name = "AttendSec")
    private Integer attendSec;

    @Column(name = "SalType")
    private String salType;

    @Column(name = "EmpCode")
    private String empCode;

    public Integer getEmpAttendance() { return empAttendance; }
    public void setEmpAttendance(Integer empAttendance) { this.empAttendance = empAttendance; }

    public Integer getLogId() { return logId; }
    public void setLogId(Integer logId) { this.logId = logId; }

    public Date getLogDate() { return logDate; }
    public void setLogDate(Date logDate) { this.logDate = logDate; }

    public Date getDuration() { return duration; }
    public void setDuration(Date duration) { this.duration = duration; }

    public Integer getAttendHours() { return attendHours; }
    public void setAttendHours(Integer attendHours) { this.attendHours = attendHours; }

    public Integer getAttendMins() { return attendMins; }
    public void setAttendMins(Integer attendMins) { this.attendMins = attendMins; }

    public Integer getAttendSec() { return attendSec; }
    public void setAttendSec(Integer attendSec) { this.attendSec = attendSec; }

    public String getSalType() { return salType; }
    public void setSalType(String salType) { this.salType = salType; }

    public String getEmpCode() { return empCode; }
    public void setEmpCode(String empCode) { this.empCode = empCode; }
}
