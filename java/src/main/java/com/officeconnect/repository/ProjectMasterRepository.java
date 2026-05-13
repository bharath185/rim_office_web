package com.officeconnect.repository;

import com.officeconnect.entity.ProjectMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface ProjectMasterRepository extends JpaRepository<ProjectMaster, Integer> {
    List<ProjectMaster> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}
