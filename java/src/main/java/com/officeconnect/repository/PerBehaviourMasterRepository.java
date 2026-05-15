package com.officeconnect.repository;

import com.officeconnect.entity.PerBehaviourMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PerBehaviourMasterRepository extends JpaRepository<PerBehaviourMaster, Integer> {
    List<PerBehaviourMaster> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}
