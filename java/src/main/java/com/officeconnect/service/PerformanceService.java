package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
public class PerformanceService {

    @Autowired
    private PerGoalRepository perGoalRepository;

    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private CPwdManagementRepository cpwdManagementRepository;

    @Autowired
    private FinancialYearMasterRepository financialYearMasterRepository;

    @Autowired
    private QuaterMasterRepository quaterMasterRepository;

    @Autowired
    private CompanyMasterRepository companyMasterRepository;

    @Autowired
    private LoginlogRepository loginlogRepository;

    @Autowired
    private OnSiteLoginlogRepository onSiteLoginlogRepository;

    @Autowired
    private ReviewListRepository reviewListRepository;

    @Autowired
    private PerBehaviourDetailRepository perBehaviourDetailRepository;

    @Autowired
    private PerBehaviourMasterRepository perBehaviourMasterRepository;

    public PerformanceViewModel createGoal(PerformanceViewModel model) {
        PerGoal goal = new PerGoal();
        goal.setEmpId(model.getEmpId());
        goal.setGoal(model.getGoalTitle());
        goal.setDescription(model.getGoalDescription());
        goal.setStatus("Pending");
        goal.setIsActive(true);
        goal.setIsDeleted(false);
        goal.setCreatedDate(new Date());
        
        goal = perGoalRepository.save(goal);
        
        model.setGoalId(goal.getGoalId());
        model.setStatus("Pending");
        model.setMsg("Goal created successfully");
        return model;
    }

    public List<PerformanceViewModel> getAllGoals(PerformanceViewModel model) {
        return perGoalRepository.findByEmpIdAndIsDeleted(model.getEmpId(), false).stream()
            .map(g -> convertToViewModel(g))
            .collect(Collectors.toList());
    }

    public PerformanceViewModel updateGoalStatus(PerformanceViewModel model) {
        Optional<PerGoal> goalOpt = perGoalRepository.findById(model.getGoalId());
        if (goalOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Goal not found\"}");
        }
        
        PerGoal goal = goalOpt.get();
        goal.setStatus(model.getStatus());
        perGoalRepository.save(goal);
        
        model.setMsg("Goal updated successfully");
        return model;
    }

    public PerformanceViewModel deleteGoal(PerformanceViewModel model) {
        Optional<PerGoal> goalOpt = perGoalRepository.findById(model.getGoalId());
        if (goalOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Goal not found\"}");
        }
        
        PerGoal goal = goalOpt.get();
        goal.setIsDeleted(true);
        goal.setIsActive(false);
        perGoalRepository.save(goal);
        
        model.setMsg("Goal deleted successfully");
        return model;
    }

    private PerformanceViewModel convertToViewModel(PerGoal g) {
        PerformanceViewModel vm = new PerformanceViewModel();
        vm.setGoalId(g.getGoalId());
        vm.setEmpId(g.getEmpId());
        vm.setGoalTitle(g.getGoal());
        vm.setGoalDescription(g.getDescription());
        vm.setStatus(g.getStatus());
        vm.setIsActive(g.getIsActive());
        return vm;
    }

    public PerformanceViewModel getBehaviouralGoal(PerformanceViewModel model) {
        model.setMsg("GetBehaviouralGoal - Success");
        return model;
    }

    public PerformanceViewModel addBehaviouralGoal(PerformanceViewModel model) {
        model.setMsg("Behavioural goal added successfully");
        return model;
    }

    public PerformanceViewModel updateBehaviouralGoal(PerformanceViewModel model) {
        model.setMsg("Behavioural goal updated successfully");
        return model;
    }

    public PerformanceViewModel deleteBehaviouralGoal(PerformanceViewModel model) {
        model.setMsg("Behavioural goal deleted successfully");
        return model;
    }

    public List<PerformanceViewModel> getAllBehaviouralGoal(PerformanceViewModel model) {
        model.setMsg("GetAllBehaviouralGoal - Success");
        return List.of(model);
    }

    public List<PerformanceViewModel> getEmployeeGoalHistory(PerformanceViewModel model) {
        model.setMsg("GetEmployeeGoalHistory - Success");
        return List.of(model);
    }

    public PerformanceViewModel goalApproval(PerformanceViewModel model) {
        model.setStatus("Approved");
        model.setMsg("Goal approved successfully");
        return model;
    }

    public PerformanceViewModel goalReject(PerformanceViewModel model) {
        model.setStatus("Rejected");
        model.setMsg("Goal rejected successfully");
        return model;
    }

    public List<PerformanceViewModel> getGoalByEmpId(PerformanceViewModel model) {
        return perGoalRepository.findByEmpIdAndIsDeleted(model.getEmpId(), false).stream()
            .map(g -> convertToViewModel(g))
            .collect(Collectors.toList());
    }

    public PerformanceViewModel getQuarterGoal(PerformanceViewModel model) {
        model.setMsg("GetQuarterGoal - Success");
        return model;
    }

    public PerformanceViewModel addQuarterGoal(PerformanceViewModel model) {
        model.setMsg("Quarter goal added successfully");
        return model;
    }

    public PerformanceViewModel updateQuarterGoal(PerformanceViewModel model) {
        model.setMsg("Quarter goal updated successfully");
        return model;
    }

    public List<PerformanceViewModel> getAllQuarterGoal(PerformanceViewModel model) {
        model.setMsg("GetAllQuarterGoal - Success");
        return List.of(model);
    }

    public List<EmployeeMasterViewModel> getEmployeeDetails(EmployeeMasterViewModel model) {
        try {
            Integer empId = (model.getEmpId() != null && model.getEmpId() != 0) ? model.getEmpId() : 0;
            String username = (model.getUserName() != null && !model.getUserName().isEmpty()) ? model.getUserName() : "";

            if (empId == 0) {
                throw new RuntimeException("EmpId is Missing");
            }

            List<EmployeeMaster> empDetails = employeeMasterRepository.findByEmpIdAndIsActiveAndIsDeleted(empId, true, false);
            if (empDetails.isEmpty()) {
                throw new RuntimeException("Employee Details Not Found");
            }

            EmployeeMaster firstEmp = empDetails.get(0);
            Integer reportId = firstEmp.getReportId() != null ? firstEmp.getReportId() : 0;
            Integer oldEmpId = firstEmp.getOldEmp_ID() != null ? firstEmp.getOldEmp_ID() : 0;

            List<EmployeeMaster> authorisedEmp;
            if (oldEmpId == 0) {
                authorisedEmp = employeeMasterRepository.findByReportIdAndIsActiveAndIsDeleted(model.getEmpId(), true, false);
            } else {
                authorisedEmp = employeeMasterRepository.findByReportIdAndIsActiveAndIsDeleted(oldEmpId, true, false);
                if (authorisedEmp.isEmpty()) {
                    authorisedEmp = employeeMasterRepository.findByReportIdAndIsActiveAndIsDeleted(model.getEmpId(), true, false);
                }
            }

            Date tdy = new java.sql.Date(System.currentTimeMillis());
            List<Loginlog> loginlogsemp = loginlogRepository.findByEmpIdAndLoginDateAndLogoutDateIsNullAndIsActiveAndIsDeletedOrderByCreatedDateDesc(empId, tdy, true, false);

            Integer loginlogid = 0;
            if (!loginlogsemp.isEmpty()) {
                loginlogid = loginlogsemp.get(0).getId();
            }

            List<OnSiteLoginlog> loginlogsemp1 = onSiteLoginlogRepository.findByEmpIdAndLogoutDateIsNullAndIsActiveAndIsDeletedOrderByCreatedDateDesc(empId, true, false);

            List<EmployeeMasterViewModel> listofuserdetails = new ArrayList<>();

            for (EmployeeMaster e : empDetails) {
                EmployeeMasterViewModel vm = new EmployeeMasterViewModel();
                vm.setCompId(e.getCompId());
                vm.setOldEmp_ID(oldEmpId);
                if (e.getCompId() != null) {
                    var companyOpt = companyMasterRepository.findById(e.getCompId());
                    companyOpt.ifPresent(c -> vm.setCompany(c.getCompany()));
                }
                vm.setDeptId(e.getCategoryId());
                vm.setDeptName(e.getDeptName());
                vm.setDesignationId(e.getDesignationId());
                vm.setDesignation(e.getDesignationName());
                vm.setEmpId(e.getEmpId());
                vm.setLoginId(e.getEmpId());
                vm.setEmpCode(e.getEmpCode());
                vm.setUserName(e.getUserName());
                vm.setFirstName(e.getFirstName());
                vm.setMiddleName(e.getMiddleName());
                vm.setLastName(e.getLastName());
                vm.setMobileNo(e.getMobileNo());
                vm.setEmailId(e.getEmailId());
                vm.setGender(e.getGender());
                vm.setJoiningDate(e.getJoiningDate());
                vm.setEmpStatus(e.getEmpStatus());
                vm.setAuthorisedEntity(e.getAuthorisedEntity());
                vm.setReportId(e.getReportId());

                if (e.getReportId() != null) {
                    List<String> empCodes = employeeMasterRepository.findEmpCodeByEmpIdOrOldEmpId(e.getReportId());
                    String reportEmpCode = (empCodes != null && !empCodes.isEmpty()) ? empCodes.get(0) : null;
                    vm.setReportEmpCode(reportEmpCode);
                }

                vm.setAuthorised(!authorisedEmp.isEmpty());

                if (loginlogid == 0) {
                    vm.setOnSiteLogInId(0);
                    if (loginlogsemp1.isEmpty()) {
                        vm.setOnSiteLogInDate(null);
                        vm.setOnSiteLogInTime(null);
                        vm.setOnSiteLogOutDate(null);
                        vm.setOnSiteLogOutTime(null);
                        vm.setOnSiteStatus("LOGOUT");
                    } else {
                        OnSiteLoginlog onsite = loginlogsemp1.get(0);
                        vm.setOnSiteLogInId(onsite.getId());
                        vm.setOnSiteLogInDate(onsite.getLoginDate());
                        vm.setOnSiteLogInTime(toTimeSpanObject(onsite.getLogInTime()));
                        vm.setOnSiteLogOutDate(null);
                        vm.setOnSiteLogOutTime(null);
                        vm.setOnSiteStatus("LOGIN");
                    }
                } else {
                    Loginlog loginlog = loginlogsemp.get(0);
                    vm.setOnSiteLogInId(loginlog.getId());
                    vm.setOnSiteLogInDate(loginlog.getLoginDate());
                    vm.setOnSiteLogInTime(toTimeSpanObject(loginlog.getLogInTime()));
                    vm.setOnSiteLogOutDate(null);
                    vm.setOnSiteLogOutTime(null);
                    vm.setOnSiteStatus("LOGIN");
                }

                vm.setIsActive(e.getIsActive());
                vm.setIsUpdated(e.getIsUpdated());
                vm.setIsDeleted(e.getIsDeleted());
                vm.setCreatedBy(e.getCreatedBy());
                vm.setCreatedDate(e.getCreatedDate());
                vm.setLastUpdatedBy(e.getLastUpdatedBy());
                vm.setLastUpdatedDate(e.getLastUpdatedDate());
                vm.setcPwd(false);

                if (username != null && !username.isEmpty()) {
                    try {
                        List<CPwdManagement> pass = cpwdManagementRepository.findByEmpCodeIgnoreCaseAndCpwdAndExpiredAndIsActiveAndIsDeleted(username, true, false, true, false);
                        if (pass != null && !pass.isEmpty()) {
                            vm.setcPwd(true);
                        }
                    } catch (Exception ex) {
                        // If query fails, keep as false
                    }
                }

                listofuserdetails.add(vm);
            }

            if (!listofuserdetails.isEmpty()) {
                return listofuserdetails;
            } else {
                throw new RuntimeException("Employee Details Not Found");
            }
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException(e.getMessage());
        }
    }

    public List<Map<String, Object>> getDDFYear(Map<String, Object> model) {
        return List.of(Map.of("FyearId", 1, "Fyear", "2024-2025"), Map.of("FyearId", 2, "Fyear", "2025-2026"));
    }

    public List<Map<String, Object>> getDDQuater(Map<String, Object> model) {
        return List.of(
            Map.of("QuaterId", 1, "Quater", "Q1"),
            Map.of("QuaterId", 2, "Quater", "Q2"),
            Map.of("QuaterId", 3, "Quater", "Q3"),
            Map.of("QuaterId", 4, "Quater", "Q4")
        );
    }

    public List<Map<String, Object>> getDDReviewStatus(Map<String, Object> model) {
        return List.of(
            Map.of("StatusId", 1, "Status", "Pending"),
            Map.of("StatusId", 2, "Status", "Approved"),
            Map.of("StatusId", 3, "Status", "Rejected")
        );
    }

    public List<Map<String, Object>> getQuaterDetails(Map<String, Object> model) {
        return List.of();
    }

    public List<Map<String, Object>> getFYearDetails(Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = new ArrayList<>();
            
            List<FinancialYearMaster> fYears = financialYearMasterRepository.findByIsActiveAndIsDeleted(true, false);
            
            for (FinancialYearMaster fy : fYears) {
                Map<String, Object> fyMap = new HashMap<>();
                fyMap.put("FYearId", fy.getYearId());
                fyMap.put("FinancialYear", fy.getFinancialYear());
                fyMap.put("FinancialDetails", fy.getFinancialYear());
                fyMap.put("QId", null);
                fyMap.put("QName", "Quater");
                fyMap.put("StartDate", "");
                fyMap.put("EndDate", "");
                fyMap.put("EmpId", null);
                fyMap.put("msg", null);
                result.add(fyMap);
            }
            
            if (result.isEmpty()) {
                Map<String, Object> fyMap = new HashMap<>();
                fyMap.put("FYearId", 3);
                fyMap.put("FinancialYear", "2025-2026");
                fyMap.put("FinancialDetails", "2025-2026, Apr  1 - Jun 30");
                fyMap.put("QId", null);
                fyMap.put("QName", "Quater");
                fyMap.put("StartDate", "Apr  1");
                fyMap.put("EndDate", "Jun 30");
                fyMap.put("EmpId", null);
                fyMap.put("msg", null);
                result.add(fyMap);
            }
            
            return result;
        } catch (Exception e) {
            e.printStackTrace();
            throw new RuntimeException("Error: " + e.getMessage());
        }
    }

    public Map<String, Object> submitConfigSetup(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Config submitted successfully");
    }

    public Map<String, Object> updateConfigSetup(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Config updated successfully");
    }

    public List<Map<String, Object>> getAllConfigSetup(Map<String, Object> model) {
        return List.of();
    }

    public List<Map<String, Object>> performanceReport(Map<String, Object> model) {
        return List.of();
    }

    public List<Map<String, Object>> getAllGoal(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<PerGoal> goals = perGoalRepository.findByEmpIdAndIsDeleted(empId, false);
        List<Map<String, Object>> result = new ArrayList<>();

        for (PerGoal g : goals) {
            Map<String, Object> m = new HashMap<>();
            m.put("GoalId", g.getGoalId());
            m.put("EmpId", g.getEmpId());
            m.put("Goal", g.getGoal());
            m.put("Description", g.getDescription());
            m.put("Weightage", g.getWeightage());
            m.put("Status", g.getStatus());
            m.put("FinalSubmit", g.getFinalSubmit());
            m.put("EmpReview", g.getEmpReview());
            m.put("EDescription", g.getEDescription());
            m.put("ManagerReview", g.getManagerReview());
            m.put("MDescription", g.getMDescription());
            m.put("QId", g.getQId());
            m.put("PeriodId", g.getPeriodId());
            m.put("CreatedBy", g.getCreatedBy());
            m.put("CreatedDate", g.getCreatedDate());
            m.put("IsActive", g.getIsActive());
            m.put("IsDeleted", g.getIsDeleted());
            result.add(m);
        }
        return result;
    }

    public List<Map<String, Object>> getAllGoalEmployee(Map<String, Object> model) {
        return List.of();
    }

    public Map<String, Object> getGoal(Map<String, Object> model) {
        return Map.of();
    }

    public Map<String, Object> addAllGoal(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Goals added successfully");
    }

    public Map<String, Object> approveAllGoal(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Goals approved successfully");
    }

    public Map<String, Object> addGoal(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Goal added successfully");
    }

    public Map<String, Object> updateGoal(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Goal updated successfully");
    }

    public Map<String, Object> deleteGoalEndpoint(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Goal deleted successfully");
    }

    public List<Map<String, Object>> getAllTask(Map<String, Object> model) {
        return List.of();
    }

    public Map<String, Object> getTask(Map<String, Object> model) {
        return Map.of();
    }

    public Map<String, Object> addTask(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Task added successfully");
    }

    public Map<String, Object> updateTask(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Task updated successfully");
    }

    public Map<String, Object> deleteTask(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Task deleted successfully");
    }

    public List<Map<String, Object>> getAllBehaviour(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<PerBehaviourMaster> behaviours = perBehaviourMasterRepository.findByIsActiveAndIsDeleted(true, false);
        List<Map<String, Object>> result = new ArrayList<>();

        for (PerBehaviourMaster b : behaviours) {
            Map<String, Object> m = new HashMap<>();
            m.put("Id", b.getId());
            m.put("Behaviour", b.getBehaviour());
            m.put("Description", b.getDescription());
            m.put("Weightage", b.getWeightage());
            m.put("QId", b.getQId());
            m.put("PeriodId", b.getPeriodId());
            m.put("Status", b.getStatus());
            m.put("CreatedBy", b.getCreatedBy());
            m.put("CreatedDate", b.getCreatedDate());
            m.put("IsActive", b.getIsActive());
            result.add(m);
        }
        return result;
    }

    public Map<String, Object> getBehaviour(Map<String, Object> model) {
        return Map.of();
    }

    public Map<String, Object> addBehaviour(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Behaviour added successfully");
    }

    public Map<String, Object> updateBehaviour(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Behaviour updated successfully");
    }

    public Map<String, Object> deleteBehaviour(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Behaviour deleted successfully");
    }

    public List<Map<String, Object>> getAllBehaviourDetail(Map<String, Object> model) {
        return List.of();
    }

    public Map<String, Object> getBehaviourDetail(Map<String, Object> model) {
        return Map.of();
    }

    public Map<String, Object> addBehaviourDetail(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Behaviour detail added successfully");
    }

    public Map<String, Object> updateBehaviourDetail(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Behaviour detail updated successfully");
    }

    public Map<String, Object> deleteBehaviourDetail(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Behaviour detail deleted successfully");
    }

    public List<Map<String, Object>> getAllSelfDevelopment(Map<String, Object> model) {
        return List.of();
    }

    public Map<String, Object> getSelfDevelopment(Map<String, Object> model) {
        return Map.of();
    }

    public Map<String, Object> addSelfDevelopment(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Self development added successfully");
    }

    public Map<String, Object> updateSelfDevelopment(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Self development updated successfully");
    }

    public Map<String, Object> deleteSelfDevelopment(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Self development deleted successfully");
    }

    public Map<String, Object> saveEmployeeReview(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Employee review saved successfully");
    }

    public List<Map<String, Object>> getAllEmployeeReviewList(Map<String, Object> model) {
        return List.of();
    }

    public List<Map<String, Object>> getEmployeeReviewList(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<ReviewList> reviews = reviewListRepository.findByIsActiveAndIsDeleted(true, false);
        List<Map<String, Object>> result = new ArrayList<>();

        for (ReviewList r : reviews) {
            Map<String, Object> m = new HashMap<>();
            m.put("ReviewId", r.getReviewId());
            m.put("FYearId", r.getFYearId());
            m.put("QId", r.getQId());
            m.put("EmpId", r.getEmpId());
            m.put("QType", r.getQType());
            m.put("Status", r.getStatus());
            m.put("ReviewedByEmp", r.getReviewedByEmp());
            m.put("ReviewedByManager", r.getReviewedByManager());
            m.put("Completed", r.getCompleted());

            // Resolve Financial Year name
            String fYear = "";
            if (r.getFYearId() != null) {
                Optional<FinancialYearMaster> fyOpt = financialYearMasterRepository.findById(r.getFYearId());
                if (fyOpt.isPresent()) fYear = fyOpt.get().getFinancialYear() != null ? fyOpt.get().getFinancialYear() : "";
            }
            m.put("FYear", fYear);

            // Resolve Employee Name
            String empName = "";
            if (r.getEmpId() != null) {
                Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(r.getEmpId());
                if (empOpt.isPresent()) {
                    EmployeeMaster emp = empOpt.get();
                    String fn = emp.getFirstName() != null ? emp.getFirstName().trim() : "";
                    String ln = emp.getLastName() != null ? " " + emp.getLastName().trim() : "";
                    empName = (fn + ln).trim();
                }
            }
            m.put("EmpName", empName);

            // Resolve Period from QuaterMaster (startDate - endDate)
            String period = "";
            if (r.getQId() != null) {
                Optional<QuaterMaster> qOpt = quaterMasterRepository.findById(r.getQId());
                if (qOpt.isPresent()) {
                    QuaterMaster q = qOpt.get();
                    String sd = q.getStartDate() != null ? q.getStartDate() : "";
                    String ed = q.getEndDate() != null ? q.getEndDate() : "";
                    if (!sd.isEmpty() || !ed.isEmpty()) period = sd + " - " + ed;
                }
            }
            m.put("Period", period);

            m.put("CreatedBy", r.getCreatedBy());
            m.put("CreatedDate", r.getCreatedDate());
            m.put("IsActive", r.getIsActive());
            result.add(m);
        }
        return result;
    }

    public Map<String, Object> saveManagerReview(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Manager review saved successfully");
    }

    private Map<String, Object> toTimeSpanObject(Date time) {
        if (time == null) return null;
        java.util.Calendar cal = java.util.Calendar.getInstance();
        cal.setTime(time);
        int hours = cal.get(java.util.Calendar.HOUR_OF_DAY);
        int minutes = cal.get(java.util.Calendar.MINUTE);
        int seconds = cal.get(java.util.Calendar.SECOND);
        int millis = cal.get(java.util.Calendar.MILLISECOND);
        long ticks = ((long)hours * 3600L + (long)minutes * 60L + seconds) * 10000000L + (long)millis * 10000L;

        Map<String, Object> result = new LinkedHashMap<>();
        result.put("Hours", hours);
        result.put("Minutes", minutes);
        result.put("Seconds", seconds);
        result.put("Milliseconds", millis);
        result.put("Ticks", ticks);
        result.put("Days", 0);
        double totalDays = (hours * 3600.0 + minutes * 60.0 + seconds + millis / 1000.0) / 86400.0;
        result.put("TotalDays", totalDays);
        result.put("TotalHours", totalDays * 24.0);
        result.put("TotalMilliseconds", ticks / 10000.0);
        result.put("TotalMinutes", totalDays * 1440.0);
        result.put("TotalSeconds", totalDays * 86400.0);
        return result;
    }
}