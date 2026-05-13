package com.officeconnect.repository;

import com.officeconnect.entity.WeekHoliday;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface WeekHolidayRepository extends JpaRepository<WeekHoliday, Integer> {
    List<WeekHoliday> findByYearAndStatus(Integer year, String status);
    List<WeekHoliday> findByYearAndStatusAndLocationId(Integer year, String status, Integer locationId);
}
