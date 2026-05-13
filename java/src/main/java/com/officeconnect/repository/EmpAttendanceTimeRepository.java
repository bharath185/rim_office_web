package com.officeconnect.repository;

import com.officeconnect.entity.EmpAttendanceTime;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface EmpAttendanceTimeRepository extends JpaRepository<EmpAttendanceTime, Integer> {
    List<EmpAttendanceTime> findByLogDateBetween(Date startDate, Date endDate);
    List<EmpAttendanceTime> findByLogDateBetweenAndEmpCode(Date startDate, Date endDate, String empCode);
}
