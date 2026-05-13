package com.officeconnect.repository;

import com.officeconnect.entity.SiteMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface SiteMasterRepository extends JpaRepository<SiteMaster, Integer> {
    List<SiteMaster> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}