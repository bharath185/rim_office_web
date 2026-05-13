package com.officeconnect.repository;

import com.officeconnect.entity.PayrollSymbolMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PayrollSymbolMasterRepository extends JpaRepository<PayrollSymbolMaster, Integer> {

    List<PayrollSymbolMaster> findByIsActiveTrue();
}
