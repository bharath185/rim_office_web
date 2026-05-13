package com.officeconnect.repository;

import com.officeconnect.entity.OnSiteLoginlog;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface OnSiteLoginlogRepository extends JpaRepository<OnSiteLoginlog, Integer> {
    List<OnSiteLoginlog> findByLoginDateBetween(Date startDate, Date endDate);
    List<OnSiteLoginlog> findByLoginDateBetweenAndEmpCode(Date startDate, Date endDate, String empCode);
    List<OnSiteLoginlog> findByEmpIdAndIsActiveAndIsDeletedOrderByLoginDateDesc(Integer empId, Boolean isActive, Boolean isDeleted);
    List<OnSiteLoginlog> findByEmpIdAndLogoutDateIsNullAndIsActiveAndIsDeletedOrderByCreatedDateDesc(Integer empId, Boolean isActive, Boolean isDeleted);
}
