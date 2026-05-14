package com.officeconnect.repository;

import com.officeconnect.entity.VariableHistory;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface VariableHistoryRepository extends JpaRepository<VariableHistory, Integer> {
}
