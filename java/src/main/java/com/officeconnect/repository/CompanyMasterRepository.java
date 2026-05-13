package com.officeconnect.repository;

import com.officeconnect.entity.CompanyMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface CompanyMasterRepository extends JpaRepository<CompanyMaster, Integer> {
    List<CompanyMaster> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}