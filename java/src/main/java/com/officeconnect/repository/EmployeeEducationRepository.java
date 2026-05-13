package com.officeconnect.repository;

import com.officeconnect.entity.EmployeeEducation;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface EmployeeEducationRepository extends JpaRepository<EmployeeEducation, Integer> {
    List<EmployeeEducation> findByEmpIdAndIsActiveAndIsDeleted(Integer empId, Boolean isActive, Boolean isDeleted);
    List<EmployeeEducation> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);

    @Query("SELECT e FROM EmployeeEducation e WHERE e.empId = :empId AND e.docId = :docId AND e.isActive = :isActive AND e.isDeleted = :isDeleted")
    List<EmployeeEducation> findByEmpIdAndDocIdAndIsActiveAndIsDeleted(@Param("empId") Integer empId, @Param("docId") Integer docId, @Param("isActive") Boolean isActive, @Param("isDeleted") Boolean isDeleted);
}
