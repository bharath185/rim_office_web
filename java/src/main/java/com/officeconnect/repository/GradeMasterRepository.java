package com.officeconnect.repository;

import com.officeconnect.entity.GradeMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface GradeMasterRepository extends JpaRepository<GradeMaster, Integer> {
    List<GradeMaster> findByIsActive(Boolean isActive);
}
