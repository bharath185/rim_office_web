package com.officeconnect.repository;

import com.officeconnect.entity.ManualAttendance;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface ManualAttendanceRepository extends JpaRepository<ManualAttendance, Integer> {
    List<ManualAttendance> findByDate(Date date);
    List<ManualAttendance> findByEmpCodeAndDate(String empCode, Date date);
    List<ManualAttendance> findByIsActiveAndIsDeletedOrderByCreatedDateDesc(Boolean isActive, Boolean isDeleted);
}
