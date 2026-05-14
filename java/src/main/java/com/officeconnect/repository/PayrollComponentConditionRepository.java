package com.officeconnect.repository;

import com.officeconnect.entity.PayrollComponentCondition;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface PayrollComponentConditionRepository extends JpaRepository<PayrollComponentCondition, Integer> {
    List<PayrollComponentCondition> findByComponentIdAndIsActiveTrueAndIsDeletedFalse(Integer componentId);
    List<PayrollComponentCondition> findByComponentId(Integer componentId);
    @Query("SELECT c FROM PayrollComponentCondition c WHERE c.componentId = ?1 AND c.sno = ?2")
    PayrollComponentCondition findByComponentIdAndSNo(Integer componentId, Integer sno);
}
