package com.officeconnect.repository;

import com.officeconnect.entity.CPwdManagement;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface CPwdManagementRepository extends JpaRepository<CPwdManagement, Integer> {

    List<CPwdManagement> findByEmpCodeIgnoreCaseAndCpwdAndExpiredAndIsActiveAndIsDeleted(String empCode, Boolean cpwd, Boolean expired, Boolean isActive, Boolean isDeleted);
    
    Optional<CPwdManagement> findOneByEmpCodeIgnoreCaseAndCpwdAndExpiredAndIsActiveAndIsDeleted(String empCode, Boolean cpwd, Boolean expired, Boolean isActive, Boolean isDeleted);
}