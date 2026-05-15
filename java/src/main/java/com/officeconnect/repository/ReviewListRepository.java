package com.officeconnect.repository;

import com.officeconnect.entity.ReviewList;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ReviewListRepository extends JpaRepository<ReviewList, Integer> {
    List<ReviewList> findByIsActiveAndIsDeleted(Boolean isActive, Boolean isDeleted);
}
