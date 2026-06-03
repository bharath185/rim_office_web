package com.officeconnect.repository;

import com.officeconnect.entity.WFHLoginlog;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface WFHLoginlogRepository extends JpaRepository<WFHLoginlog, Integer> {
    List<WFHLoginlog> findByDateBetween(Date startDate, Date endDate);
    List<WFHLoginlog> findByDateBetweenAndEmpCode(Date startDate, Date endDate, String empCode);
    List<WFHLoginlog> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
    @Query("SELECT w FROM WFHLoginlog w WHERE UPPER(w.empCode) = UPPER(?1) AND w.date = ?2 AND w.isActive = ?3 AND w.isDeleted = ?4")
    List<WFHLoginlog> findByEmpCodeIgnoreCaseAndDateAndIsActiveAndIsDeleted(String empCode, Date date, Boolean isActive, Boolean isDeleted);
    @Query("SELECT w FROM WFHLoginlog w WHERE w.analysisHr IS NOT NULL AND w.isActive = true AND w.isDeleted = false")
    List<WFHLoginlog> findByAnalysisHrIsNotNullAndIsActiveAndIsDeleted();
    @Query("SELECT w FROM WFHLoginlog w WHERE UPPER(w.empCode) = UPPER(?1) AND w.empId = ?2 AND w.date = ?3 AND w.isLoggedIn = true AND w.isLoggedOut = false AND w.isActive = true AND w.isDeleted = false ORDER BY w.createdDate DESC")
    List<WFHLoginlog> findTodayActiveLogin(String empCode, Integer empId, Date date);
}
