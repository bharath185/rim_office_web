package com.officeconnect.repository;

import com.officeconnect.entity.EmployeeAccDetails;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface EmployeeAccDetailsRepository extends JpaRepository<EmployeeAccDetails, Integer> {
    List<EmployeeAccDetails> findByEmpIdAndIsActiveAndIsDeleted(Integer empId, Boolean isActive, Boolean isDeleted);
}
