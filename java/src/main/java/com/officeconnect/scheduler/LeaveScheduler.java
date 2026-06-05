package com.officeconnect.scheduler;

import com.officeconnect.service.EmployeeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

@Component
public class LeaveScheduler {

    @Autowired
    private EmployeeService employeeService;

    @Scheduled(cron = "0 0 3 * * ?")
    public void processLeaveCredits() {
        employeeService.processLeaveCreditsScheduled();
    }

    @Scheduled(cron = "0 0 10 * * ?")
    public void fetchAttendance() {
        employeeService.fetchAttendanceScheduled();
    }
}
