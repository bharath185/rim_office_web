package com.officeconnect.repository;

import com.officeconnect.entity.WFHLoginlog;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;

@Repository
public interface WFHLoginlogRepository extends JpaRepository<WFHLoginlog, Integer> {
    List<WFHLoginlog> findByDateBetween(Date startDate, Date endDate);
    List<WFHLoginlog> findByDateBetweenAndEmpCode(Date startDate, Date endDate, String empCode);
}
