package com.officeconnect.repository;

import com.officeconnect.entity.VisitorInviteHistory;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface VisitorInviteHistoryRepository extends JpaRepository<VisitorInviteHistory, Integer> {
    List<VisitorInviteHistory> findByVisitIdAndIsActiveAndIsDeleted(Integer visitId, Boolean isActive, Boolean isDeleted);
}
