package com.officeconnect.repository;

import com.officeconnect.entity.PerTask;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PerTaskRepository extends JpaRepository<PerTask, Integer> {
    List<PerTask> findByEmpIdAndGoalIdAndStatusAndIsActiveAndIsDeleted(Integer empId, Integer goalId, Boolean status, Boolean isActive, Boolean isDeleted);
    List<PerTask> findByEmpIdAndIsActiveAndIsDeleted(Integer empId, Boolean isActive, Boolean isDeleted);
}
