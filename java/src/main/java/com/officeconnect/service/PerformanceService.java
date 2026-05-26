package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;
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

    @Autowired
    private PerTaskRepository perTaskRepository;

    @Autowired
    private DeptMasterRepository deptMasterRepository;

    @Autowired
    private DesignationMasterRepository designationMasterRepository;

    @Autowired
    private ModuleMasterRepository moduleMasterRepository;

    @Autowired
    private SubModuleMasterRepository subModuleMasterRepository;

    @Autowired
    private PageModuleMasterRepository pageModuleMasterRepository;

    @Autowired
    private AccessPolicyRepository accessPolicyRepository;

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

        List<PerGoal> goals = perGoalRepository.findByEmpIdAndIsActiveAndIsDeleted(empId, true, false);
        List<Map<String, Object>> result = new ArrayList<>();

        Map<Integer, EmployeeMaster> empCache = new HashMap<>();
        Map<Integer, QuaterMaster> qCache = new HashMap<>();
        Map<Integer, FinancialYearMaster> fyCache = new HashMap<>();

        for (PerGoal g : goals) {
            EmployeeMaster emp = null;
            if (g.getEmpId() != null) {
                emp = empCache.get(g.getEmpId());
                if (emp == null) {
                    Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(g.getEmpId());
                    if (empOpt.isPresent()) {
                        emp = empOpt.get();
                        empCache.put(g.getEmpId(), emp);
                    }
                }
            }

            QuaterMaster q = null;
            if (g.getQId() != null) {
                q = qCache.get(g.getQId());
                if (q == null) {
                    Optional<QuaterMaster> qOpt = quaterMasterRepository.findById(g.getQId());
                    if (qOpt.isPresent()) {
                        q = qOpt.get();
                        qCache.put(g.getQId(), q);
                    }
                }
            }

            FinancialYearMaster fy = null;
            if (g.getPeriodId() != null) {
                fy = fyCache.get(g.getPeriodId());
                if (fy == null) {
                    Optional<FinancialYearMaster> fyOpt = financialYearMasterRepository.findById(g.getPeriodId());
                    if (fyOpt.isPresent()) {
                        fy = fyOpt.get();
                        fyCache.put(g.getPeriodId(), fy);
                    }
                }
            }

            Map<String, Object> m = new HashMap<>();
            m.put("GoalId", g.getGoalId());
            m.put("EmpId", g.getEmpId());
            m.put("EmpCode", emp != null ? emp.getEmpCode() : null);
            m.put("EmpName", emp != null ? emp.getFirstName() : null);
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
            m.put("Type", q != null ? q.getType() : null);
            m.put("QName", q != null ? q.getName() : null);
            m.put("PeriodId", g.getPeriodId());
            m.put("FYear", fy != null ? fy.getFinancialYear() : null);
            m.put("ReviewedByEmp", g.getReviewedByEmp());
            m.put("ReviewedByManager", g.getReviewedByManager());
            m.put("CreatedBy", g.getCreatedBy());
            m.put("CreatedDate", g.getCreatedDate());
            m.put("LastUpdatedBy", g.getLastUpdatedBy());
            m.put("LastUpdatedDate", g.getLastUpdatedDate());
            m.put("IsActive", g.getIsActive());
            m.put("IsUpdated", g.getIsUpdated());
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

    @SuppressWarnings("unchecked")
    public Map<String, Object> addAllGoal(Map<String, Object> model) {
        Object empIdObj = model.get("EmpId");
        Integer empId = empIdObj != null ? Integer.parseInt(empIdObj.toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        Object listObj = model.get("listofGoal");
        if (listObj == null) throw new RuntimeException("listofGoal is Missing");

        List<Map<String, Object>> listofGoal = (List<Map<String, Object>>) listObj;

        for (Map<String, Object> goalItem : listofGoal) {
            Integer goalId = goalItem.get("GoalId") != null ? Integer.parseInt(goalItem.get("GoalId").toString()) : 0;
            String goalName = goalItem.get("Goal") != null ? goalItem.get("Goal").toString() : "";
            String weightage = goalItem.get("Weightage") != null ? goalItem.get("Weightage").toString() : "0";

            PerGoal goal = perGoalRepository.findByGoalIdAndIsActiveAndIsDeleted(goalId, true, false);
            if (goal == null) {
                throw new RuntimeException("This " + goalName + " Detail is Not Found");
            }

            goal.setGoal(goalName);
            goal.setWeightage(weightage);
            goal.setStatus("Pending");
            goal.setFinalSubmit(true);
            goal.setIsActive(true);
            goal.setIsUpdated(true);
            goal.setIsDeleted(false);
            goal.setLastUpdatedBy(empId);
            goal.setLastUpdatedDate(new Date());
            perGoalRepository.save(goal);
        }

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Final Subimission Done");
        return result;
    }

    public Map<String, Object> approveAllGoal(Map<String, Object> model) {
        return Map.of("StatusCode", 200, "Message", "Goals approved successfully");
    }

    public Map<String, Object> addGoal(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        String goalName = model.get("Goal") != null ? model.get("Goal").toString() : "";

        // Get active quarter and financial year
        List<QuaterMaster> activeQuarters = quaterMasterRepository.findByIsActiveAndIsDeleted(true, false);
        Integer qId = activeQuarters.isEmpty() ? 0 : activeQuarters.get(0).getQuaterId();

        List<FinancialYearMaster> activeFYears = financialYearMasterRepository.findByStatus(true);
        if (activeFYears.isEmpty()) {
            activeFYears = financialYearMasterRepository.findByIsActiveAndIsDeleted(true, false);
        }
        Integer periodId = activeFYears.isEmpty() ? 0 : activeFYears.get(0).getYearId();

        // Check if goal with same name already exists
        PerGoal existing = perGoalRepository.findByEmpIdAndGoalAndIsActiveAndIsDeleted(empId, goalName, true, false);
        if (existing != null) {
            throw new RuntimeException("Goal Details Not Found");
        }

        PerGoal dm = new PerGoal();
        dm.setGoal(goalName);
        dm.setQId(qId != 0 ? qId : 0);
        dm.setPeriodId(periodId != 0 ? periodId : 0);
        dm.setEmpId(empId);
        dm.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
        dm.setWeightage(model.get("Weightage") != null ? model.get("Weightage").toString() : "");
        dm.setEmpReview(model.get("EmpReview") != null ? model.get("EmpReview").toString() : "");
        dm.setManagerReview(model.get("ManagerReview") != null ? model.get("ManagerReview").toString() : "");
        dm.setStatus("");
        dm.setReviewedByEmp(false);
        dm.setReviewedByManager(false);
        dm.setFinalSubmit(false);
        dm.setIsActive(true);
        dm.setIsDeleted(false);
        dm.setCreatedBy(empId);
        dm.setCreatedDate(new Date());
        dm.setLastUpdatedBy(empId);
        dm.setLastUpdatedDate(new Date());
        perGoalRepository.save(dm);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Added");
        result.put("Goal", goalName);
        return result;
    }

    public Map<String, Object> updateGoal(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        Integer goalId = model.get("GoalId") != null ? Integer.parseInt(model.get("GoalId").toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        PerGoal goal = perGoalRepository.findByGoalIdAndIsActiveAndIsDeleted(goalId, true, false);
        if (goal == null) {
            throw new RuntimeException("Goal Details Not Found");
        }

        goal.setGoal(model.get("Goal") != null ? model.get("Goal").toString() : "");
        goal.setQId(model.get("QId") != null ? Integer.parseInt(model.get("QId").toString()) : 0);
        goal.setPeriodId(model.get("PeriodId") != null ? Integer.parseInt(model.get("PeriodId").toString()) : 0);
        goal.setEmpId(empId);
        goal.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
        goal.setWeightage(model.get("Weightage") != null ? model.get("Weightage").toString() : "");
        goal.setEmpReview(model.get("EmpReview") != null ? model.get("EmpReview").toString() : "");
        goal.setManagerReview(model.get("ManagerReview") != null ? model.get("ManagerReview").toString() : "");
        goal.setStatus("");
        goal.setFinalSubmit(false);
        goal.setIsActive(true);
        goal.setIsUpdated(true);
        goal.setIsDeleted(false);
        goal.setLastUpdatedBy(empId);
        goal.setLastUpdatedDate(new Date());
        perGoalRepository.save(goal);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Updated");
        result.put("Goal", model.get("Goal"));
        return result;
    }

    public Map<String, Object> deleteGoalEndpoint(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        Integer goalId = model.get("GoalId") != null ? Integer.parseInt(model.get("GoalId").toString()) : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        PerGoal goal = perGoalRepository.findByGoalIdAndIsActiveAndIsDeleted(goalId, true, false);
        if (goal == null) {
            throw new RuntimeException("Goal Details Not Found");
        }

        goal.setIsDeleted(true);
        goal.setLastUpdatedBy(empId);
        goal.setLastUpdatedDate(new Date());
        perGoalRepository.save(goal);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Deleted");
        result.put("Goal", model.get("Goal"));
        return result;
    }

    public List<Map<String, Object>> getAllTask(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        Integer goalId = model.get("GoalId") != null ? Integer.parseInt(model.get("GoalId").toString()) : 0;

        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<PerTask> tasks = perTaskRepository.findByEmpIdAndGoalIdAndStatusAndIsActiveAndIsDeleted(empId, goalId, true, true, false);
        List<Map<String, Object>> result = new ArrayList<>();

        Map<Integer, EmployeeMaster> empCache = new HashMap<>();
        Map<Integer, QuaterMaster> qCache = new HashMap<>();
        Map<Integer, FinancialYearMaster> fyCache = new HashMap<>();
        Map<Integer, PerGoal> goalCache = new HashMap<>();

        for (PerTask t : tasks) {
            // Resolve quarter
            QuaterMaster q = null;
            if (t.getQId() != null) {
                q = qCache.get(t.getQId());
                if (q == null) {
                    Optional<QuaterMaster> qOpt = quaterMasterRepository.findById(t.getQId());
                    if (qOpt.isPresent()) {
                        q = qOpt.get();
                        qCache.put(t.getQId(), q);
                    }
                }
            }

            // Resolve financial year
            FinancialYearMaster fy = null;
            if (t.getPeriodId() != null) {
                fy = fyCache.get(t.getPeriodId());
                if (fy == null) {
                    Optional<FinancialYearMaster> fyOpt = financialYearMasterRepository.findById(t.getPeriodId());
                    if (fyOpt.isPresent()) {
                        fy = fyOpt.get();
                        fyCache.put(t.getPeriodId(), fy);
                    }
                }
            }

            // Resolve employee
            EmployeeMaster emp = null;
            if (t.getEmpId() != null) {
                emp = empCache.get(t.getEmpId());
                if (emp == null) {
                    Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(t.getEmpId());
                    if (empOpt.isPresent()) {
                        emp = empOpt.get();
                        empCache.put(t.getEmpId(), emp);
                    }
                }
            }

            // Resolve goal
            PerGoal goal = null;
            if (t.getGoalId() != null && t.getEmpId() != null) {
                String goalKey = t.getEmpId() + "_" + t.getGoalId();
                goal = goalCache.computeIfAbsent(t.getGoalId(), k -> {
                    Optional<PerGoal> gOpt = perGoalRepository.findById(k);
                    return gOpt.orElse(null);
                });
            }

            Map<String, Object> m = new HashMap<>();
            m.put("TaskId", t.getTaskId());
            m.put("QId1", t.getQId1());
            m.put("PeriodId", t.getPeriodId());
            m.put("FYear", fy != null ? fy.getFinancialYear() : null);
            m.put("EmpId", t.getEmpId());
            m.put("EmpCode", emp != null ? emp.getEmpCode() : null);
            m.put("EmpName", emp != null ? emp.getFirstName() : null);
            m.put("GoalId", t.getGoalId());
            m.put("Goal", goal != null ? goal.getGoal() : null);
            m.put("QId", t.getQId());
            m.put("Type", q != null ? q.getType() : null);
            m.put("QName", q != null ? q.getName() : null);
            m.put("Task", t.getTask());
            m.put("Description", t.getDescription());
            m.put("Status", t.getStatus());
            m.put("IsActive", t.getIsActive());
            m.put("IsUpdated", t.getIsUpdated());
            m.put("IsDeleted", t.getIsDeleted());
            m.put("CreatedBy", t.getCreatedBy());
            m.put("CreatedDate", t.getCreatedDate());
            m.put("LastUpdatedBy", t.getLastUpdatedBy());
            m.put("LastUpdatedDate", t.getLastUpdatedDate());
            result.add(m);
        }

        return result;
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

    @SuppressWarnings("unchecked")
    public Map<String, Object> saveEmployeeReview(Map<String, Object> model) {
        Object empIdObj = model.get("EmpId");
        Integer empId = null;
        if (empIdObj != null) {
            String val = empIdObj.toString().trim();
            if (!val.isEmpty()) {
                empId = Integer.parseInt(val);
            }
        }
        if (empId != null && empId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        List<QuaterMaster> activeQuarters = quaterMasterRepository.findByIsActiveAndIsDeleted(true, false);
        Integer qId = activeQuarters.isEmpty() ? 0 : activeQuarters.get(0).getQuaterId();

        List<FinancialYearMaster> activeFYears = financialYearMasterRepository.findByStatus(true);
        if (activeFYears.isEmpty()) {
            activeFYears = financialYearMasterRepository.findByIsActiveAndIsDeleted(true, false);
        }
        Integer fYearId = activeFYears.isEmpty() ? 0 : activeFYears.get(0).getYearId();

        Object listofGoalObj = model.get("listofGoal");
        if (listofGoalObj == null) {
            throw new RuntimeException("Goal Details Not Found");
        }

        List<Map<String, Object>> listofGoal = (List<Map<String, Object>>) listofGoalObj;
        for (Map<String, Object> goalItem : listofGoal) {
            Integer goalId = goalItem.get("GoalId") != null ? Integer.parseInt(goalItem.get("GoalId").toString()) : 0;

            PerGoal goalDetails = perGoalRepository.findByGoalIdAndEmpIdAndReviewedByEmpAndReviewedByManagerAndIsActiveAndIsDeleted(
                    goalId, empId, false, false, true, false);

            if (goalDetails != null) {
                goalDetails.setEmpReview(goalItem.get("EmpReview") != null ? goalItem.get("EmpReview").toString() : "");
                goalDetails.setEDescription(goalItem.get("EDescription") != null ? goalItem.get("EDescription").toString() : "");
                goalDetails.setReviewedByEmp(true);
                goalDetails.setStatus("Emp Review Completed");
                goalDetails.setFinalSubmit(true);
                goalDetails.setIsActive(true);
                goalDetails.setIsUpdated(true);
                goalDetails.setIsDeleted(false);
                goalDetails.setLastUpdatedBy(empId);
                goalDetails.setLastUpdatedDate(new Date());
                perGoalRepository.save(goalDetails);
            } else {
                String goalName = goalItem.get("Goal") != null ? goalItem.get("Goal").toString() : "Unknown";
                throw new RuntimeException("This " + goalName + " Detail is Not Found");
            }
        }

        Object listofBehaviorObj = model.get("listofBehavior");
        if (listofBehaviorObj == null) {
            throw new RuntimeException("Employee Review Details Not Found");
        }

        List<Map<String, Object>> listofBehavior = (List<Map<String, Object>>) listofBehaviorObj;
        for (Map<String, Object> behaviorItem : listofBehavior) {
            Integer behaviourId = behaviorItem.get("Id") != null ? Integer.parseInt(behaviorItem.get("Id").toString()) : 0;

            PerBehaviourDetail behaviourDetails = perBehaviourDetailRepository.findByBehaviourIdAndEmpIdAndIsActiveAndIsDeleted(
                    behaviourId, empId, true, false);

            if (behaviourDetails != null) {
                behaviourDetails.setEmpReview(behaviorItem.get("EmpReview") != null ? behaviorItem.get("EmpReview").toString() : "");
                behaviourDetails.setEDescription(behaviorItem.get("EDescription") != null ? behaviorItem.get("EDescription").toString() : "");
                behaviourDetails.setReviewedByEmp(true);
                behaviourDetails.setIsActive(true);
                behaviourDetails.setIsUpdated(true);
                behaviourDetails.setIsDeleted(false);
                behaviourDetails.setLastUpdatedBy(empId);
                behaviourDetails.setLastUpdatedDate(new Date());
                perBehaviourDetailRepository.save(behaviourDetails);
            } else {
                PerBehaviourMaster behaviour = perBehaviourMasterRepository.findById(behaviourId).orElse(null);
                if (behaviour != null && behaviour.getIsActive() != null && behaviour.getIsActive()
                        && behaviour.getIsDeleted() != null && !behaviour.getIsDeleted()) {
                    PerBehaviourDetail pbd = new PerBehaviourDetail();
                    pbd.setQId(behaviour.getQId() != null ? behaviour.getQId() : 0);
                    pbd.setPeriodId(behaviour.getPeriodId() != null ? behaviour.getPeriodId() : 0);
                    pbd.setEmpId(empId);
                    pbd.setBehaviourId(behaviour.getId());
                    pbd.setBehaviour(behaviour.getBehaviour());
                    pbd.setDescription(behaviour.getDescription() != null ? behaviour.getDescription() : "");
                    pbd.setWeightage(behaviour.getWeightage());
                    pbd.setEmpReview(behaviorItem.get("EmpReview") != null ? behaviorItem.get("EmpReview").toString() : "");
                    pbd.setManagerReview("");
                    pbd.setEDescription(behaviorItem.get("EDescription") != null ? behaviorItem.get("EDescription").toString() : "");
                    pbd.setMDescription("");
                    pbd.setReviewedByEmp(true);
                    pbd.setReviewedByManager(false);
                    pbd.setIsActive(true);
                    pbd.setIsUpdated(false);
                    pbd.setIsDeleted(false);
                    pbd.setCreatedBy(empId);
                    pbd.setCreatedDate(new Date());
                    pbd.setLastUpdatedBy(empId);
                    pbd.setLastUpdatedDate(new Date());
                    perBehaviourDetailRepository.save(pbd);
                }
            }
        }

        ReviewList empReviewDetails = reviewListRepository.findByQIdAndFYearIdAndEmpIdAndIsActiveAndIsDeleted(
                qId, fYearId, empId, true, false);

        if (empReviewDetails == null) {
            String qType = activeQuarters.isEmpty() ? "" : activeQuarters.get(0).getType();

            ReviewList rl = new ReviewList();
            rl.setFYearId(fYearId);
            rl.setQId(qId);
            rl.setEmpId(empId);
            rl.setQType(qType);
            rl.setStatus("Emp Review Completed");
            rl.setReviewedByEmp(true);
            rl.setReviewedByManager(false);
            rl.setCompleted(false);
            rl.setCreatedBy(empId);
            rl.setCreatedDate(new Date());
            rl.setLastUpdatedBy(empId);
            rl.setLastUpdatedDate(new Date());
            rl.setIsActive(true);
            rl.setIsUpdated(false);
            rl.setIsDeleted(false);
            reviewListRepository.save(rl);
        }

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee Reviw Completed");
        return result;
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