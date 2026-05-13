package com.officeconnect.repository;

import com.officeconnect.entity.VendorMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface VendorMasterRepository extends JpaRepository<VendorMaster, Integer> {
    List<VendorMaster> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}
