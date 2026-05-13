package com.officeconnect.repository;

import com.officeconnect.entity.TempManualAttendance;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface TempManualAttendanceRepository extends JpaRepository<TempManualAttendance, Integer> {
    List<TempManualAttendance> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}
