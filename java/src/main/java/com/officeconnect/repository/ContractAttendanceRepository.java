package com.officeconnect.repository;

import com.officeconnect.entity.ContractAttendance;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import java.util.Date;
import java.util.List;
import java.util.Optional;

@Repository
public interface ContractAttendanceRepository extends JpaRepository<ContractAttendance, Integer> {
    List<ContractAttendance> findByIsDeleted(Boolean isDeleted);
    List<ContractAttendance> findByManagerIdAndIsDeleted(Integer managerId, Boolean isDeleted);

    @Query("SELECT c FROM ContractAttendance c WHERE c.mobile = :mobile AND c.date = :date AND c.isLogin = :isLogin AND c.isLogout = :isLogout AND c.isActive = :isActive AND c.isDeleted = :isDeleted")
    Optional<ContractAttendance> findByMobileAndDateAndFlags(
            @Param("mobile") String mobile,
            @Param("date") Date date,
            @Param("isLogin") Boolean isLogin,
            @Param("isLogout") Boolean isLogout,
            @Param("isActive") Boolean isActive,
            @Param("isDeleted") Boolean isDeleted);
}
