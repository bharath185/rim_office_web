package com.officeconnect.repository;

import com.officeconnect.entity.EmpShiftDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface EmpShiftDetailRepository extends JpaRepository<EmpShiftDetail, Integer> {
    List<EmpShiftDetail> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
    List<EmpShiftDetail> findByIsActiveAndIsDeletedAndEmpCode(Boolean isActive, Boolean isDeleted, String empCode);
    List<EmpShiftDetail> findByEmpCode(String empCode);
    List<EmpShiftDetail> findByEmpIdAndCompIdAndLeIdAndBuIdAndLocationIdAndEndDateIsNullAndShiftStatusAndIsActiveAndIsDeleted(
        Integer empId, Integer compId, Integer leId, Integer buId, Integer locationId,
        Boolean shiftStatus, Boolean isActive, Boolean isDeleted);
    List<EmpShiftDetail> findByEmpIdAndCompIdAndLeIdAndBuIdAndLocationIdAndShiftIdAndEndDateIsNullAndShiftStatusAndIsActiveAndIsDeleted(
        Integer empId, Integer compId, Integer leId, Integer buId, Integer locationId, Integer shiftId,
        Boolean shiftStatus, Boolean isActive, Boolean isDeleted);
}
