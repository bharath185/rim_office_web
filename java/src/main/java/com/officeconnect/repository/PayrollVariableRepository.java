package com.officeconnect.repository;

import com.officeconnect.entity.PayrollVariable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PayrollVariableRepository extends JpaRepository<PayrollVariable, Integer> {
    List<PayrollVariable> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}
