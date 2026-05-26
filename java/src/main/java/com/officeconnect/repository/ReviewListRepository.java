package com.officeconnect.repository;

import com.officeconnect.entity.ReviewList;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ReviewListRepository extends JpaRepository<ReviewList, Integer> {
    List<ReviewList> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);

    @Query("SELECT r FROM ReviewList r WHERE r.qId = :qId AND r.fYearId = :fYearId AND r.empId = :empId AND r.isActive = :isActive AND r.isDeleted = :isDeleted")
    ReviewList findByQIdAndFYearIdAndEmpIdAndIsActiveAndIsDeleted(
            @Param("qId") Integer qId,
            @Param("fYearId") Integer fYearId,
            @Param("empId") Integer empId,
            @Param("isActive") Boolean isActive,
            @Param("isDeleted") Boolean isDeleted);
}
