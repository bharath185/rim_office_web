package com.officeconnect.repository;

import com.officeconnect.entity.VisitorInviteHistory;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface VisitorInviteHistoryRepository extends JpaRepository<VisitorInviteHistory, Integer> {
}
