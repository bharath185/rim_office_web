package com.officeconnect.repository;

import com.officeconnect.entity.LeaveTypeMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface LeaveTypeMasterRepository extends JpaRepository<LeaveTypeMaster, Integer> {
    List<LeaveTypeMaster> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
    List<LeaveTypeMaster> findByShortNameAndIsActiveAndIsDeleted(String shortName, Boolean isActive, Boolean isDeleted);
    List<LeaveTypeMaster> findByLeaveNameAndIsActiveTrueAndIsDeletedFalse(String leaveName);

    @org.springframework.data.jpa.repository.Query("SELECT l.leaveTypeId FROM LeaveTypeMaster l WHERE l.shortName = ?1 AND l.isActive = true AND l.isDeleted = false")
    Integer findLeaveTypeIdByShortName(String shortName);
}