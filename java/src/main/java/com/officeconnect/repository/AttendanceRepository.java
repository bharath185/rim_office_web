package com.officeconnect.repository;

import com.officeconnect.entity.Attendance;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface AttendanceRepository extends JpaRepository<Attendance, Integer> {
    List<Attendance> findByTypeAndLogDateBetween(String type, Date startDate, Date endDate);
    List<Attendance> findByTypeAndLogDateBetweenAndEmpCode(String type, Date startDate, Date endDate, String empCode);
    List<Attendance> findByLogDateBetween(Date startDate, Date endDate);
}
