package com.officeconnect.repository;

import com.officeconnect.entity.PerBehaviourDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface PerBehaviourDetailRepository extends JpaRepository<PerBehaviourDetail, Integer> {
}
