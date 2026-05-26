package com.officeconnect.repository;

import com.officeconnect.entity.PerGoal;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PerGoalRepository extends JpaRepository<PerGoal, Integer> {
    List<PerGoal> findByEmpIdAndIsDeleted(Integer empId, Boolean isDeleted);
    List<PerGoal> findByEmpIdAndIsActiveAndIsDeleted(Integer empId, Boolean isActive, Boolean isDeleted);
    List<PerGoal> findByEmpIdAndStatus(Integer empId, String status);
    PerGoal findByEmpIdAndGoalAndIsActiveAndIsDeleted(Integer empId, String goal, Boolean isActive, Boolean isDeleted);
    PerGoal findByGoalIdAndIsActiveAndIsDeleted(Integer goalId, Boolean isActive, Boolean isDeleted);
    PerGoal findByGoalIdAndEmpIdAndReviewedByEmpAndReviewedByManagerAndIsActiveAndIsDeleted(Integer goalId, Integer empId, Boolean reviewedByEmp, Boolean reviewedByManager, Boolean isActive, Boolean isDeleted);
}