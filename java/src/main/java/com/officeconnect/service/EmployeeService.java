package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.io.File;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.text.SimpleDateFormat;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Base64;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.Objects;
import java.util.stream.Collectors;
import java.util.LinkedHashMap;
import java.util.GregorianCalendar;
import java.util.TimeZone;
import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;
import jakarta.persistence.Query;

@Service
@Transactional
public class EmployeeService {

    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private CompanyMasterRepository companyMasterRepository;

    @Autowired
    private LegalEntityMasterRepository legalEntityMasterRepository;

    @Autowired
    private BusinessUnitMasterRepository businessUnitMasterRepository;

    @Autowired
    private LocationMasterRepository locationMasterRepository;

    @Autowired
    private HolidayRepository holidayRepository;

    @Autowired
    private ShiftMasterRepository shiftMasterRepository;

    @Autowired
    private CPwdManagementRepository cpwdManagementRepository;

    @Autowired
    private PageModuleMasterRepository pageModuleMasterRepository;

    @Autowired
    private ModuleMasterRepository moduleMasterRepository;

    @Autowired
    private SubModuleMasterRepository subModuleMasterRepository;

    @Autowired
    private AccessPolicyRepository accessPolicyRepository;

    @Autowired
    private SalutationMasterRepository salutationMasterRepository;

    @Autowired
    private EmpTypeMasterRepository empTypeMasterRepository;

    @Autowired
    private GenderMasterRepository genderMasterRepository;

    @Autowired
    private WorkTypeMasterRepository workTypeMasterRepository;

    @Autowired
    private EmpProbationTrackingHistoryRepository empProbationTrackingHistoryRepository;

    @Autowired
    private EmployeeMasterLogRepository employeeMasterLogRepository;

    @Autowired
    private LeaveCarryForwardMasterRepository leaveCarryForwardMasterRepository;

    @Autowired
    private LeaveTypeMasterRepository leaveTypeMasterRepository;

    @Autowired
    private EmployeeCareerDetailRepository employeeCareerDetailRepository;

    @Autowired
    private DocumentMasterRepository documentMasterRepository;

    @Autowired
    private EmployeeEducationRepository employeeEducationRepository;

    @Autowired
    private LoginlogRepository loginlogRepository;

    @Autowired
    private EmployeeDetailRepository employeeDetailRepository;

    @Autowired
    private EmployeeAccDetailRepository employeeAccDetailRepository;

    @Autowired
    private EmployeeGovtDocRepository employeeGovtDocRepository;

    @Autowired
    private AttendanceRepository attendanceRepository;

    @Autowired
    private EmpAttendanceTimeRepository empAttendanceTimeRepository;

    @Autowired
    private EmpShiftDetailRepository empShiftDetailRepository;

    @Autowired
    private WFHLoginlogRepository wfhLoginlogRepository;

    @Autowired
    private OnSiteLoginlogRepository onSiteLoginlogRepository;

    @Autowired
    private WeekHolidayRepository weekHolidayRepository;

    @Autowired
    private EmpLeaveApplicationRepository empLeaveApplicationRepository;

    @Autowired
    private DeptMasterRepository deptMasterRepository;

    @Autowired
    private DesignationMasterRepository designationMasterRepository;

    @Autowired
    private ManualAttendanceRepository manualAttendanceRepository;

    @Autowired
    private ContractAttendanceRepository contractAttendanceRepository;

    @Autowired
    private VendorMasterRepository vendorMasterRepository;

    @Autowired
    private SiteMasterRepository siteMasterRepository;

    @Autowired
    private ProjectMasterRepository projectMasterRepository;

    @Autowired
    private TempManualAttendanceRepository tempManualAttendanceRepository;

    @PersistenceContext
    private EntityManager entityManager;

    @Autowired
    private FinancialYearMasterRepository financialYearMasterRepository;

    public List<DDCompanyViewModel> getDDCompany(DDCompanyViewModel model) {
        return companyMasterRepository.findAll().stream()
            .filter(c -> c.getIsActive() != null && c.getIsActive() && c.getIsDeleted() != null && !c.getIsDeleted())
            .map(c -> {
                DDCompanyViewModel vm = new DDCompanyViewModel();
                vm.setCompId(c.getCompId());
                vm.setCompany(c.getCompany());
                vm.setCompanyCode(c.getCompanyCode());
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<DDLegalEntityViewModel> getDDLegalEntity(DDLegalEntityViewModel model) {
        Integer empId = model.getEmpId() != null && model.getEmpId() != 0 ? model.getEmpId() : 0;
        Integer compId = model.getCompId() != null && model.getCompId() != 0 ? model.getCompId() : 0;
        String authorisedEntityStr = model.getAuthorisedEntity();

        List<Integer> authorisedEntities = parseAuthorisedEntities(authorisedEntityStr);

        List<LegalEntityMaster> allEntities = legalEntityMasterRepository.findByIsActiveAndIsDeleted(true, false);

        List<DDLegalEntityViewModel> result = allEntities.stream()
            .filter(le -> le.getLeId() != null && authorisedEntities.contains(le.getLeId()))
            .map(le -> {
                DDLegalEntityViewModel vm = new DDLegalEntityViewModel();
                vm.setCompId(le.getCompId());
                vm.setLeId(le.getLeId());
                vm.setLegalEntity(le.getLegalEntity());
                return vm;
            })
            .collect(Collectors.toList());

        if (compId != 0) {
            result = allEntities.stream()
                .filter(le -> le.getLeId() != null && authorisedEntities.contains(le.getLeId()))
                .filter(le -> le.getCompId() != null && le.getCompId().equals(compId))
                .map(le -> {
                    DDLegalEntityViewModel vm = new DDLegalEntityViewModel();
                    vm.setCompId(le.getCompId());
                    vm.setLeId(le.getLeId());
                    vm.setLegalEntity(le.getLegalEntity());
                    return vm;
                })
                .collect(Collectors.toList());
        }

        if (empId != 0) {
            if (result == null || result.isEmpty()) {
                throw new RuntimeException("Legal Entity Details Not Found");
            }
            return result;
        } else {
            throw new RuntimeException("EmpId is Missing");
        }
    }

    public List<DDAuthorisedEntityViewModel> getAuthorizedEntity(DDAuthorisedEntityViewModel model) {
        return legalEntityMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(le -> {
                DDAuthorisedEntityViewModel vm = new DDAuthorisedEntityViewModel();
                vm.setEmpId(0);
                vm.setLoginId(0);
                vm.setLeId(le.getLeId());
                vm.setLegalEntity(le.getLegalEntity());
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<DDBusinessUnitViewModel> getDDBusinessUnit(DDBusinessUnitViewModel model) {
        Integer empId = model.getEmpId() != null && model.getEmpId() != 0 ? model.getEmpId() : 0;
        Integer compId = model.getCompId() != null && model.getCompId() != 0 ? model.getCompId() : 0;
        Integer leId = model.getLeId() != null && model.getLeId() != 0 ? model.getLeId() : 0;
        List<Integer> authorisedEntities = parseAuthorisedEntities(model.getAuthorisedEntity());

        List<BusinessUnitMaster> allBUs = businessUnitMasterRepository.findByIsActiveAndIsDeleted(true, false);

        List<DDBusinessUnitViewModel> result = allBUs.stream()
            .filter(bu -> bu.getLeId() != null && authorisedEntities.contains(bu.getLeId()))
            .map(bu -> {
                DDBusinessUnitViewModel vm = new DDBusinessUnitViewModel();
                vm.setCompId(bu.getCompId());
                vm.setLeId(bu.getLeId());
                vm.setBuId(bu.getBuId());
                vm.setBusinessUnit(bu.getBusinessUnit());
                return vm;
            })
            .collect(Collectors.toList());

        if (compId != 0) {
            result = allBUs.stream()
                .filter(bu -> bu.getLeId() != null && authorisedEntities.contains(bu.getLeId()))
                .filter(bu -> bu.getCompId() != null && bu.getCompId().equals(compId))
                .filter(bu -> leId == 0 || (bu.getLeId() != null && bu.getLeId().equals(leId)))
                .map(bu -> {
                    DDBusinessUnitViewModel vm = new DDBusinessUnitViewModel();
                    vm.setCompId(bu.getCompId());
                    vm.setLeId(bu.getLeId());
                    vm.setBuId(bu.getBuId());
                    vm.setBusinessUnit(bu.getBusinessUnit());
                    return vm;
                })
                .collect(Collectors.toList());
        }

        if (empId != 0) {
            return result;
        } else {
            throw new RuntimeException("EmpId is Missing");
        }
    }

    public List<DDLocationViewModel> getDDLocation(DDLocationViewModel model) {
        Integer empId = model.getEmpId() != null && model.getEmpId() != 0 ? model.getEmpId() : 0;
        Integer compId = model.getCompId() != null && model.getCompId() != 0 ? model.getCompId() : 0;
        Integer leId = model.getLeId() != null && model.getLeId() != 0 ? model.getLeId() : 0;
        Integer buId = model.getBuId() != null && model.getBuId() != 0 ? model.getBuId() : 0;
        List<Integer> authorisedEntities = parseAuthorisedEntities(model.getAuthorisedEntity());

        List<DDLocationViewModel> result = locationMasterRepository.findAll().stream()
            .filter(lm -> lm.getIsActive() != null && lm.getIsActive() && lm.getIsDeleted() != null && !lm.getIsDeleted())
            .filter(lm -> authorisedEntities.isEmpty() || (lm.getLeId() != null && authorisedEntities.contains(lm.getLeId())))
            .map(lm -> {
                DDLocationViewModel vm = new DDLocationViewModel();
                vm.setCompId(lm.getCompId());
                vm.setLeId(lm.getLeId());
                vm.setBuId(lm.getBuId());
                vm.setLocationId(lm.getLocationId());
                vm.setLocation(lm.getLocation());
                return vm;
            })
            .collect(Collectors.toList());

        if (compId != 0) {
            result = result.stream()
                .filter(vm -> vm.getCompId() != null && vm.getCompId().equals(compId))
                .collect(Collectors.toList());
        }
        if (leId != 0) {
            result = result.stream()
                .filter(vm -> vm.getLeId() != null && vm.getLeId().equals(leId))
                .collect(Collectors.toList());
        }
        if (buId != 0) {
            result = result.stream()
                .filter(vm -> vm.getBuId() != null && vm.getBuId().equals(buId))
                .collect(Collectors.toList());
        }

        if (empId != 0) {
            return result;
        } else {
            throw new RuntimeException("EmpId is Missing");
        }
    }

    public List<NewDDCompanyViewModel> getNewDDCompany(NewDDCompanyViewModel model) {
        return companyMasterRepository.findAll().stream()
            .filter(c -> c.getIsActive() != null && c.getIsActive() && c.getIsDeleted() != null && !c.getIsDeleted())
            .map(c -> {
                NewDDCompanyViewModel vm = new NewDDCompanyViewModel();
                vm.setCompId(c.getCompId());
                vm.setCompany(c.getCompany());
                vm.setCompanyCode(c.getCompanyCode());
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<NewDDLegalEntityViewModel> getNewDDLegalEntity(NewDDLegalEntityViewModel model) {
        Integer loginId = model.getLoginId();
        if (loginId == null || loginId == 0) throw new RuntimeException("EmpId is Missing");

        Integer compId = model.getCompId();
        if (compId != null && compId != 0) {
            return legalEntityMasterRepository.findByCompIdAndIsActiveAndIsDeleted(compId, true, false).stream()
                .map(le -> {
                    NewDDLegalEntityViewModel vm = new NewDDLegalEntityViewModel();
                    vm.setLeId(le.getLeId());
                    vm.setCompId(le.getCompId());
                    vm.setLegalEntity(le.getLegalEntity());
                    return vm;
                })
                .collect(Collectors.toList());
        }
        return legalEntityMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(le -> {
                NewDDLegalEntityViewModel vm = new NewDDLegalEntityViewModel();
                vm.setLeId(le.getLeId());
                vm.setCompId(le.getCompId());
                vm.setLegalEntity(le.getLegalEntity());
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<NewDDBusinessUnitViewModel> getNewDDBusinessUnit(NewDDBusinessUnitViewModel model) {
        Integer loginId = model.getLoginId();
        if (loginId == null || loginId == 0) throw new RuntimeException("EmpId is Missing");

        Integer compId = model.getCompId();
        Integer leId = model.getLeId();

        List<NewDDBusinessUnitViewModel> result = businessUnitMasterRepository.findByIsActiveAndIsDeleted(true, false)
            .stream()
            .filter(bu -> compId == null || compId == 0 || (bu.getCompId() != null && bu.getCompId().equals(compId)))
            .filter(bu -> leId == null || leId == 0 || (bu.getLeId() != null && bu.getLeId().equals(leId)))
            .map(bu -> {
                NewDDBusinessUnitViewModel vm = new NewDDBusinessUnitViewModel();
                vm.setBuId(bu.getBuId());
                vm.setLeId(bu.getLeId());
                vm.setCompId(bu.getCompId());
                vm.setBusinessUnit(bu.getBusinessUnit());
                return vm;
            })
            .collect(Collectors.toList());

        if (result.isEmpty()) {
            throw new RuntimeException("Business Unit Details Not Found");
        }
        return result;
    }

    public List<NewDDLocationViewModel> getNewDDLocation(NewDDLocationViewModel model) {
        Integer loginId = model.getLoginId();
        if (loginId == null || loginId == 0) throw new RuntimeException("EmpId is Missing");

        Integer compId = model.getCompId();
        Integer leId = model.getLeId();

        List<NewDDLocationViewModel> result = locationMasterRepository.findByIsActiveAndIsDeleted(true, false)
            .stream()
            .filter(loc -> compId == null || compId == 0 || (loc.getCompId() != null && loc.getCompId().equals(compId)))
            .filter(loc -> leId == null || leId == 0 || (loc.getLeId() != null && loc.getLeId().equals(leId)))
            .map(loc -> {
                NewDDLocationViewModel vm = new NewDDLocationViewModel();
                vm.setLocationId(loc.getLocationId());
                vm.setLeId(loc.getLeId());
                vm.setCompId(loc.getCompId());
                vm.setLocation(loc.getLocation());
                return vm;
            })
            .collect(Collectors.toList());

        if (result.isEmpty()) {
            throw new RuntimeException("Location Details Not Found");
        }
        return result;
    }

    public EmployeeMasterViewModel saveEmployee(EmployeeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<EmployeeMaster> existing = employeeMasterRepository.findAll().stream()
            .filter(e -> e.getEmpCode() != null && e.getEmpCode().equals(model.getEmpCode())
                      && Boolean.TRUE.equals(e.getIsActive()) && Boolean.FALSE.equals(e.getIsDeleted()))
            .collect(Collectors.toList());

        if (!existing.isEmpty()) throw new RuntimeException("Employee with EmpCode " + model.getEmpCode() + " already exists");

        EmployeeMaster emp = new EmployeeMaster();
        emp.setOldEmp_ID(0);
        emp.setCompId(model.getCompId());
        emp.setLeId(model.getLeId() != null ? model.getLeId() : 0);
        emp.setBuId(model.getBuId() != null ? model.getBuId() : 0);
        emp.setLocationId(model.getLocationId() != null ? model.getLocationId() : 0);
        emp.setCategoryId(model.getDeptId());
        emp.setDeptName(model.getDeptName());
        emp.setDesignationId(model.getDesignationId());
        emp.setDesignationName(model.getDesignation());
        emp.setReportId(model.getReportId());

        String reportCode = "";
        if (model.getReportId() != null && model.getReportId() > 0) {
            Optional<EmployeeMaster> reporter = employeeMasterRepository.findById(model.getReportId());
            if (reporter.isPresent() && reporter.get().getEmpCode() != null) {
                reportCode = reporter.get().getEmpCode();
            }
        }
        emp.setReportName(reportCode);
        emp.setEmpCode(model.getEmpCode());
        emp.setUserName(model.getEmpCode());

        String defaultPassword = "password";
        String encodedPassword = Base64.getEncoder().encodeToString(defaultPassword.getBytes(StandardCharsets.UTF_16));
        if (model.getPassword() != null && !model.getPassword().isEmpty()) {
            encodedPassword = Base64.getEncoder().encodeToString(model.getPassword().getBytes(StandardCharsets.UTF_16));
        }
        emp.setPassword(encodedPassword);
        emp.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        emp.setSalutation(model.getSalutationId());
        emp.setFirstName(model.getFirstName());
        emp.setMiddleName(model.getMiddleName() != null ? model.getMiddleName() : "");
        emp.setLastName(model.getLastName());
        emp.setDob(parseDate(model.getDob()));
        emp.setMobileNo(model.getMobileNo());
        emp.setEmailId(model.getEmailId());
        emp.setBloodGroup(model.getBloodGroup());
        emp.setMaritalStatus(model.getMaritalStatus());
        emp.setGender(model.getGender());
        emp.setInterviewDate(parseDate(model.getInterviewDate()));
        emp.setJoiningDate(parseDate(model.getJoiningDate()));
        emp.setEmpType(model.getEmpTypeId() != null ? model.getEmpTypeId() : 0);
        emp.setAuthorisedEntity(model.getAuthorisedEntity());
        emp.setIsRelieved(false);
        emp.setcEndDate(model.getcEndDate() != null ? parseDate(model.getcEndDate().toString()) : null);
        emp.setEmpStatus("Active");
        emp.setIsActive(true);
        emp.setIsUpdated(false);
        emp.setIsDeleted(false);
        emp.setCreatedBy(loginId);
        emp.setCreatedDate(new Date());
        emp.setLastUpdatedBy(loginId);
        emp.setLastUpdatedDate(new Date());

        emp = employeeMasterRepository.save(emp);
        Integer empId = emp.getEmpId();

        EmployeeMasterLog eml = new EmployeeMasterLog();
        eml.setEmpId(empId);
        eml.setOldEmp_ID(0);
        eml.setCompId(model.getCompId());
        eml.setLeId(model.getLeId() != null ? model.getLeId() : 0);
        eml.setBuId(model.getBuId() != null ? model.getBuId() : 0);
        eml.setLocationId(model.getLocationId() != null ? model.getLocationId() : 0);
        eml.setCategoryId(model.getDeptId());
        eml.setDeptName(model.getDeptName());
        eml.setDesignationId(model.getDesignationId());
        eml.setDesignationName(model.getDesignation());
        eml.setReportId(model.getReportId());
        eml.setReportName(reportCode);
        eml.setEmpCode(model.getEmpCode());
        eml.setUserName(model.getEmpCode());
        eml.setPassword(encodedPassword);
        eml.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        eml.setSalutation(model.getSalutationId());
        eml.setFirstName(model.getFirstName());
        eml.setMiddleName(model.getMiddleName() != null ? model.getMiddleName() : "");
        eml.setLastName(model.getLastName());
        eml.setDob(parseDate(model.getDob()));
        eml.setMobileNo(model.getMobileNo());
        eml.setEmailId(model.getEmailId());
        eml.setBloodGroup(model.getBloodGroup());
        eml.setMaritalStatus(model.getMaritalStatus());
        eml.setGender(model.getGender());
        eml.setJoiningDate(parseDate(model.getJoiningDate()));
        eml.setEmpType(model.getEmpTypeId() != null ? model.getEmpTypeId() : 0);
        eml.setAuthorisedEntity(model.getAuthorisedEntity());
        eml.setCEndDate(model.getcEndDate() != null ? parseDate(model.getcEndDate().toString()) : null);
        eml.setEmpStatus("Active");
        eml.setIsActive(true);
        eml.setIsUpdated(false);
        eml.setIsDeleted(false);
        eml.setCreatedBy(loginId);
        eml.setCreatedDate(new Date());
        eml.setLastUpdatedBy(loginId);
        eml.setLastUpdatedDate(new Date());
        employeeMasterLogRepository.save(eml);

        CPwdManagement cpm = new CPwdManagement();
        cpm.setEmpId(empId);
        cpm.setEmpCode(model.getEmpCode());
        cpm.setCpwd(true);
        cpm.setExpired(false);
        cpm.setCreatedBy(loginId);
        cpm.setCreatedDate(new Date());
        cpm.setLastUpdatedBy(loginId);
        cpm.setLastUpdatedDate(new Date());
        cpm.setIsActive(true);
        cpm.setIsUpdated(false);
        cpm.setIsDeleted(false);
        cpwdManagementRepository.save(cpm);

        if (model.getEmpTypeId() != null && model.getEmpTypeId() > 0 && Boolean.TRUE.equals(model.getIsProbation())) {
            Optional<LocationMaster> locOpt = locationMasterRepository.findById(model.getLocationId() != null ? model.getLocationId() : 0);
            int probationDays = locOpt.isPresent() && locOpt.get().getProbationPeriod() != null ? locOpt.get().getProbationPeriod() : 90;

            Date joiningDate = parseDate(model.getJoiningDate());
            if (joiningDate == null) joiningDate = new Date();

            Calendar cal = Calendar.getInstance();
            cal.setTime(joiningDate);
            cal.add(Calendar.DAY_OF_MONTH, probationDays);
            Date probationEndDate = cal.getTime();

            EmpProbationTrackingHistory epth = new EmpProbationTrackingHistory();
            epth.setEmpId(empId);
            epth.setJoiningDate(joiningDate);
            epth.setProbationDays(probationDays);
            epth.setProbationEndDate(probationEndDate);
            epth.setReportId(model.getReportId());
            epth.setReportCode(reportCode);
            epth.setIsProbation(true);
            epth.setIsPermanent(false);
            epth.setIsContract(false);
            epth.setIsConsultant(false);
            epth.setRemarks("");
            epth.setCreatedBy(loginId);
            epth.setCreatedDate(new Date());
            epth.setLastUpdatedBy(loginId);
            epth.setLastUpdatedDate(new Date());
            epth.setIsActive(true);
            epth.setIsUpdated(false);
            epth.setIsDeleted(false);
            empProbationTrackingHistoryRepository.save(epth);
        }

        List<LeaveTypeMaster> leaveTypes = leaveTypeMasterRepository.findAll().stream()
            .filter(lt -> Boolean.TRUE.equals(lt.getIsActive()) && Boolean.FALSE.equals(lt.getIsDeleted()))
            .collect(Collectors.toList());

        Date today = new Date();
        Calendar cal = Calendar.getInstance();
        cal.setTime(today);
        int currentYear = cal.get(Calendar.YEAR);
        int currentMonth = cal.get(Calendar.MONTH) + 1;

        boolean hasCompletedOneYear = false;
        boolean isEligibleForCLThisMonth = true;
        if (model.getJoiningDate() != null) {
            Date joiningDate = parseDate(model.getJoiningDate());
            if (joiningDate != null) {
                Calendar joinCal = Calendar.getInstance();
                joinCal.setTime(joiningDate);
                joinCal.add(Calendar.YEAR, 1);
                hasCompletedOneYear = today.after(joinCal.getTime());
                if (currentMonth > 10) isEligibleForCLThisMonth = false;
            }
        }

        for (LeaveTypeMaster lt : leaveTypes) {
            if ("CL".equalsIgnoreCase(lt.getLeaveName())) {
                if (!hasCompletedOneYear && isEligibleForCLThisMonth) {
                    LeaveCarryForwardMaster lcfm = new LeaveCarryForwardMaster();
                    lcfm.setEmpId(empId);
                    lcfm.setLeaveTypeId(lt.getLeaveTypeId());
                    lcfm.setOpeningBalance(lt.getCredit() != null ? lt.getCredit().doubleValue() : 0.0);
                    lcfm.setLeaveYear(currentYear);
                    lcfm.setLeaveMonth(currentMonth);
                    lcfm.setCreatedBy(loginId);
                    lcfm.setCreatedDate(new Date());
                    lcfm.setLastUpdatedBy(loginId);
                    lcfm.setLastUpdatedDate(new Date());
                    lcfm.setIsActive(true);
                    lcfm.setIsUpdated(false);
                    lcfm.setIsDeleted(false);
                    leaveCarryForwardMasterRepository.save(lcfm);
                }
            } else if ("EL".equalsIgnoreCase(lt.getLeaveName()) && hasCompletedOneYear) {
                LeaveCarryForwardMaster lcfm = new LeaveCarryForwardMaster();
                lcfm.setEmpId(empId);
                lcfm.setLeaveTypeId(lt.getLeaveTypeId());
                lcfm.setOpeningBalance(lt.getCredit() != null ? lt.getCredit().doubleValue() : 0.0);
                lcfm.setLeaveYear(currentYear);
                lcfm.setCreatedBy(loginId);
                lcfm.setCreatedDate(new Date());
                lcfm.setLastUpdatedBy(loginId);
                lcfm.setLastUpdatedDate(new Date());
                lcfm.setIsActive(true);
                lcfm.setIsUpdated(false);
                lcfm.setIsDeleted(false);
                leaveCarryForwardMasterRepository.save(lcfm);
            }
        }

        model.setEmpId(empId);
        model.setMsg("Added");
        return model;
    }

    private Date parseDate(Object dateObj) {
        if (dateObj == null) return null;
        if (dateObj instanceof Date) return (Date) dateObj;
        if (dateObj instanceof String) {
            try {
                String dateStr = (String) dateObj;
                if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                    return new SimpleDateFormat("dd-MM-yyyy").parse(dateStr);
                } else {
                    return new SimpleDateFormat("yyyy-MM-dd").parse(dateStr);
                }
            } catch (Exception e) {
                return null;
            }
        }
        return null;
    }

    public EmployeeMasterViewModel updateEmployee(EmployeeMasterViewModel model) {
        int loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(model.getEmpId());
        if (empOpt.isEmpty()) throw new RuntimeException("Employee not found");

        EmployeeMaster emp = empOpt.get();

        String reportName = "";
        if (model.getReportId() != null && model.getReportId() != 0) {
            Optional<EmployeeMaster> reportOpt = employeeMasterRepository.findById(model.getReportId());
            if (reportOpt.isPresent() && reportOpt.get().getEmpCode() != null) {
                reportName = reportOpt.get().getEmpCode();
            }
        }

        emp.setCompId(model.getCompId());
        emp.setLeId(model.getLeId() != null ? model.getLeId() : emp.getLeId());
        emp.setBuId(model.getBuId() != null ? model.getBuId() : emp.getBuId());
        emp.setLocationId(model.getLocationId() != null ? model.getLocationId() : 0);
        emp.setCategoryId(model.getDeptId() != null ? model.getDeptId() : emp.getCategoryId());
        emp.setDeptName(model.getDeptName() != null ? model.getDeptName() : emp.getDeptName());
        emp.setDesignationId(model.getDesignationId() != null ? model.getDesignationId() : emp.getDesignationId());
        emp.setDesignationName(model.getDesignation() != null ? model.getDesignation() : emp.getDesignationName());
        emp.setReportId(model.getReportId() != null ? model.getReportId() : emp.getReportId());
        emp.setReportName(reportName);
        emp.setEmpCode(model.getEmpCode());
        emp.setUserName(model.getEmpCode());
        emp.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        emp.setSalutation(model.getSalutationId());
        emp.setFirstName(model.getFirstName());
        emp.setMiddleName(model.getMiddleName() != null ? model.getMiddleName() : "");
        emp.setLastName(model.getLastName());
        emp.setDob(parseDateFromObject(model.getDob()));
        emp.setMobileNo(model.getMobileNo());
        emp.setEmailId(model.getEmailId());
        emp.setBloodGroup(model.getBloodGroup());
        emp.setMaritalStatus(model.getMaritalStatus());
        emp.setGender(model.getGender());
        emp.setInterviewDate(parseDateFromObject(model.getInterviewDate()));
        emp.setJoiningDate(parseDateFromObject(model.getJoiningDate()));
        emp.setEmpType(model.getEmpTypeId());
        emp.setcEndDate(parseDateFromObject(model.getcEndDate()));
        emp.setAuthorisedEntity(model.getAuthorisedEntity());
        emp.setIsUpdated(true);
        emp.setLastUpdatedBy(loginId);
        emp.setLastUpdatedDate(new Date());

        employeeMasterRepository.save(emp);

        // Create log entry
        EmployeeMasterLog eml = new EmployeeMasterLog();
        eml.setEmpId(emp.getEmpId());
        eml.setOldEmp_ID(emp.getOldEmp_ID() != null ? emp.getOldEmp_ID() : 0);
        eml.setCompId(emp.getCompId());
        eml.setLeId(emp.getLeId() != null ? emp.getLeId() : 0);
        eml.setBuId(emp.getBuId() != null ? emp.getBuId() : 0);
        eml.setLocationId(emp.getLocationId() != null ? emp.getLocationId() : 0);
        eml.setCategoryId(emp.getCategoryId());
        eml.setDeptName(emp.getDeptName());
        eml.setDesignationId(emp.getDesignationId());
        eml.setDesignationName(emp.getDesignationName());
        eml.setReportId(emp.getReportId());
        eml.setReportName(reportName);
        eml.setEmpCode(emp.getEmpCode());
        eml.setUserName(emp.getUserName());
        eml.setPassword(emp.getPassword());
        eml.setPhoto(emp.getPhoto() != null ? emp.getPhoto() : "");
        eml.setSalutation(emp.getSalutation());
        eml.setFirstName(emp.getFirstName());
        eml.setMiddleName(emp.getMiddleName());
        eml.setLastName(emp.getLastName());
        eml.setDob(emp.getDob());
        eml.setMobileNo(emp.getMobileNo());
        eml.setEmailId(emp.getEmailId());
        eml.setBloodGroup(emp.getBloodGroup());
        eml.setMaritalStatus(emp.getMaritalStatus());
        eml.setGender(emp.getGender());
        eml.setJoiningDate(emp.getJoiningDate());
        eml.setEmpType(emp.getEmpType());
        eml.setEmpStatus(emp.getEmpStatus());
        eml.setAuthorisedEntity(emp.getAuthorisedEntity());
        eml.setIsRelieved(emp.getIsRelieved());
        eml.setCEndDate(emp.getcEndDate());
        eml.setIsActive(emp.getIsActive());
        eml.setIsUpdated(true);
        eml.setIsDeleted(false);
        eml.setCreatedBy(loginId);
        eml.setCreatedDate(new Date());
        eml.setLastUpdatedBy(loginId);
        eml.setLastUpdatedDate(new Date());
        employeeMasterLogRepository.save(eml);

        if (model.getEmpTypeId() != null && model.getEmpTypeId() > 0 && Boolean.TRUE.equals(model.getIsProbation())) {
            List<EmpProbationTrackingHistory> pthList = empProbationTrackingHistoryRepository
                .findByEmpIdAndIsProbationAndIsActiveAndIsDeleted(model.getEmpId(), true, true, false);
            EmpProbationTrackingHistory pthdetails = pthList.isEmpty() ? null : pthList.get(0);

            if (Boolean.TRUE.equals(model.getIsProbationConfirm()) && pthdetails != null) {
                pthdetails.setIsProbation(false);
                pthdetails.setConfirmBy(loginId);
                pthdetails.setConfirmDate(parseDateFromObject(model.getProbationConfirmationDate()));
                pthdetails.setIsPermanent(true);
                pthdetails.setRemarks(model.getProbationRemarks());
                pthdetails.setLastUpdatedBy(loginId);
                pthdetails.setLastUpdatedDate(new Date());
                pthdetails.setIsUpdated(true);
                empProbationTrackingHistoryRepository.save(pthdetails);
            }
        }

        EmployeeMasterViewModel result = new EmployeeMasterViewModel();
        result.setMsg("Updated");
        return result;
    }

    public List<EmployeeMasterViewModel> getAllEmployees(EmployeeMasterViewModel model) {
        Integer loginId = model.getLoginId();
        
        if (loginId == null || loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }
        
        Optional<EmployeeMaster> loginEmpOpt = employeeMasterRepository.findById(loginId);
        if (loginEmpOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        
        final Integer compId = model.getCompId() == null ? 0 : model.getCompId();
        final Integer leId = model.getLeId() == null ? 0 : model.getLeId();
        final Integer buId = model.getBuId() == null ? 0 : model.getBuId();
        final Integer locId = model.getLocationId() == null ? 0 : model.getLocationId();
        final Integer deptId = model.getDeptId() == null ? 0 : model.getDeptId();
        final Integer designationId = model.getDesignationId() == null ? 0 : model.getDesignationId();
        final Integer empId = model.getEmpId() == null ? 0 : model.getEmpId();
        final Integer empTypeId = model.getEmpTypeId() == null ? 0 : model.getEmpTypeId();
        
        final String status = model.getStatus() == null ? "" : model.getStatus();
        
        List<EmployeeMaster> empdetails = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false);
        
        if (compId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getCompId() != null && e.getCompId().equals(compId)).collect(Collectors.toList());
        }
        if (leId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getLeId() != null && e.getLeId().equals(leId)).collect(Collectors.toList());
        }
        if (buId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getBuId() != null && e.getBuId().equals(buId)).collect(Collectors.toList());
        }
        if (locId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getLocationId() != null && e.getLocationId().equals(locId)).collect(Collectors.toList());
        }
        if (deptId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getCategoryId() != null && e.getCategoryId().equals(deptId)).collect(Collectors.toList());
        }
        if (designationId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getDesignationId() != null && e.getDesignationId().equals(designationId)).collect(Collectors.toList());
        }
        if (empTypeId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getEmpType() != null && e.getEmpType().equals(empTypeId)).collect(Collectors.toList());
        }
        if (empId != 0) {
            empdetails = empdetails.stream().filter(e -> e.getEmpId() != null && e.getEmpId().equals(empId)).collect(Collectors.toList());
        }
        
        if (status.equalsIgnoreCase("JOINED")) {
            empdetails = empdetails.stream().filter(e -> e.getJoiningDate() != null).collect(Collectors.toList());
        } else if (status.equalsIgnoreCase("RELIEVED")) {
            empdetails = empdetails.stream().filter(e -> e.getRelievedDate() != null).collect(Collectors.toList());
        }
        
        if (empdetails.isEmpty()) {
            throw new RuntimeException("Employees Detail Not Found");
        }
        
        // Batch load all lookup data to avoid N+1 queries
        List<Integer> compIds = empdetails.stream().map(EmployeeMaster::getCompId).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        List<Integer> leIds = empdetails.stream().map(EmployeeMaster::getLeId).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        List<Integer> buIds = empdetails.stream().map(EmployeeMaster::getBuId).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        List<Integer> locIds = empdetails.stream().map(EmployeeMaster::getLocationId).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        List<Integer> reportIds = empdetails.stream().map(EmployeeMaster::getReportId).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        List<Integer> salutationIds = empdetails.stream().map(EmployeeMaster::getSalutation).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        List<Integer> empTypeIds = empdetails.stream().map(EmployeeMaster::getEmpType).filter(Objects::nonNull).filter(id -> id != 0).distinct().collect(Collectors.toList());
        
        Map<Integer, String> compMap = companyMasterRepository.findAllById(compIds).stream().collect(Collectors.toMap(CompanyMaster::getCompId, c -> c.getCompany() != null ? c.getCompany() : ""));
        Map<Integer, String> leMap = legalEntityMasterRepository.findAllById(leIds).stream().collect(Collectors.toMap(LegalEntityMaster::getLeId, l -> l.getLegalEntity() != null ? l.getLegalEntity() : ""));
        Map<Integer, String> buMap = businessUnitMasterRepository.findAllById(buIds).stream().collect(Collectors.toMap(BusinessUnitMaster::getBuId, b -> b.getBusinessUnit() != null ? b.getBusinessUnit() : ""));
        Map<Integer, String> locMap = locationMasterRepository.findAllById(locIds).stream().collect(Collectors.toMap(LocationMaster::getLocationId, l -> l.getLocation() != null ? l.getLocation() : ""));
        Map<Integer, String> salutationMap = salutationMasterRepository.findAllById(salutationIds).stream().collect(Collectors.toMap(SalutationMaster::getSalutationId, s -> s.getSalutation() != null ? s.getSalutation() : ""));
        Map<Integer, String> empTypeMap = empTypeMasterRepository.findAllById(empTypeIds).stream().collect(Collectors.toMap(EmpTypeMaster::getEmpTypId, e -> e.getEmpType() != null ? e.getEmpType() : ""));
        
        List<EmployeeMaster> allReporters = employeeMasterRepository.findAllById(reportIds);
        Map<Integer, String> reporterNameMap = new HashMap<>();
        Map<Integer, String> reporterCodeMap = new HashMap<>();
        for (EmployeeMaster r : allReporters) {
            String fullName = (r.getFirstName() != null ? r.getFirstName() : "") + " " + (r.getMiddleName() != null ? r.getMiddleName() : "") + " " + (r.getLastName() != null ? r.getLastName() : "");
            reporterNameMap.put(r.getEmpId(), fullName.trim());
            reporterCodeMap.put(r.getEmpId(), r.getEmpCode() != null ? r.getEmpCode() : "");
        }
        
        List<EmployeeMasterViewModel> lstofEmp = new ArrayList<>();
        
        for (EmployeeMaster emp : empdetails) {
            EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
            emvm.setEmpId(emp.getEmpId());
            emvm.setOldEmp_ID(emp.getOldEmp_ID() != null ? emp.getOldEmp_ID() : 0);
            emvm.setCompId(emp.getCompId());
            emvm.setCompany(emp.getCompId() != null && emp.getCompId() != 0 ? compMap.getOrDefault(emp.getCompId(), "") : "");
            
            emvm.setLeId(emp.getLeId() != null ? emp.getLeId() : 0);
            emvm.setLegalEntity(emp.getLeId() != null && emp.getLeId() != 0 ? leMap.getOrDefault(emp.getLeId(), "") : "");
            
            emvm.setBuId(emp.getBuId() != null ? emp.getBuId() : 0);
            emvm.setBusinessUnit(emp.getBuId() != null && emp.getBuId() != 0 ? buMap.getOrDefault(emp.getBuId(), "") : "");
            
            emvm.setLocationId(emp.getLocationId() != null ? emp.getLocationId() : 0);
            emvm.setLocation(emp.getLocationId() != null && emp.getLocationId() != 0 ? locMap.getOrDefault(emp.getLocationId(), "") : "");
            
            emvm.setCategoryId(emp.getCategoryId());
            emvm.setDeptId(emp.getCategoryId());
            emvm.setDeptName(emp.getDeptName());
            emvm.setDesignationId(emp.getDesignationId());
            emvm.setDesignation(emp.getDesignationName());
            
            emvm.setReportId(emp.getReportId());
            emvm.setApproverId(emp.getReportId());
            
            Integer reportId = emp.getReportId();
            if (reportId != null && reportId != 0) {
                String reporterName = reporterNameMap.getOrDefault(reportId, "");
                String reporterCode = reporterCodeMap.getOrDefault(reportId, "");
                emvm.setApprover(reporterName + " - " + reporterCode);
                emvm.setReportEmpCode(reporterName);
                emvm.setReportEmpName(reporterCode);
            } else {
                emvm.setReportEmpCode("");
                emvm.setReportEmpName("");
                emvm.setApprover("");
            }
            
            emvm.setEmpCode(emp.getEmpCode());
            emvm.setUserName(emp.getUserName());
            
            String photo = emp.getPhoto();
            emvm.setPhoto(photo != null ? photo : "");
            if (photo != null && !photo.isEmpty() && photo.contains("Uploads")) {
                String[] parts = photo.split("Uploads");
                if (parts.length > 1) {
                    emvm.setPhoto("Uploads" + parts[1]);
                }
            }
            
            Integer salutationId = emp.getSalutation();
            emvm.setSalutationId(salutationId != null ? salutationId : 0);
            emvm.setSalutation(salutationId != null && salutationId != 0 ? salutationMap.getOrDefault(salutationId, "") : "");
            
            emvm.setFirstName(emp.getFirstName());
            emvm.setMiddleName(emp.getMiddleName());
            emvm.setLastName(emp.getLastName());
            emvm.setDob(convertToJsonDate(emp.getDob()));
            emvm.setMobileNo(emp.getMobileNo());
            emvm.setEmailId(emp.getEmailId());
            emvm.setBloodGroup(emp.getBloodGroup());
            emvm.setMaritalStatus(emp.getMaritalStatus());
            emvm.setGender(emp.getGender());
            emvm.setInterviewDate(null);
            emvm.setJoiningDate(convertToJsonDate(emp.getJoiningDate()));
            emvm.setRelievedDate(convertToJsonDate(emp.getRelievedDate()));
            emvm.setRelievedReason(emp.getRelievedReason());
            emvm.setRelievedEffectiveDate(convertToJsonDate(emp.getRelievedEffectiveDate()));
            emvm.setIsRelieved(emp.getIsRelieved());
            emvm.setEndDate(convertToJsonDate(emp.getEndDate()));
            emvm.setEmpStatus(emp.getEmpStatus());
            emvm.setAuthorisedEntity(emp.getAuthorisedEntity());
            emvm.setReason(emp.getReason());
            
            Integer empType = emp.getEmpType();
            emvm.setEmpTypeId(empType != null ? empType : 0);
            emvm.setEmpType(empType != null && empType != 0 ? empTypeMap.getOrDefault(empType, "") : "");
            
            emvm.setcEndDate(convertToJsonDate(emp.getcEndDate()));
            emvm.setIsActive(emp.getIsActive());
            emvm.setIsUpdated(emp.getIsUpdated());
            emvm.setIsDeleted(emp.getIsDeleted());
            emvm.setCreatedBy(emp.getCreatedBy());
            emvm.setCreatedDate(convertToJsonDate(emp.getCreatedDate()));
            emvm.setLastUpdatedBy(emp.getLastUpdatedBy());
            emvm.setLastUpdatedDate(convertToJsonDate(emp.getLastUpdatedDate()));
            
            lstofEmp.add(emvm);
        }
        
        lstofEmp.sort((e1, e2) -> {
            if (e1.getEmpStatus() == null && e2.getEmpStatus() == null) return 0;
            if (e1.getEmpStatus() == null) return 1;
            if (e2.getEmpStatus() == null) return -1;
            return e1.getEmpStatus().compareTo(e2.getEmpStatus());
        });
        
        return lstofEmp;
    }
    
    private String convertToJsonDate(Date date) {
        if (date == null) return null;
        return "/Date(" + date.getTime() + ")/";
    }

    public EmployeeMasterViewModel getEmployeeById(EmployeeMasterViewModel model) {
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(model.getEmpId());
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        
        EmployeeMaster e = empOpt.get();
        EmployeeMasterViewModel vm = new EmployeeMasterViewModel();
        vm.setEmpId(e.getEmpId());
        vm.setCompId(e.getCompId());
        vm.setLeId(e.getLeId());
        vm.setBuId(e.getBuId());
        vm.setLocationId(e.getLocationId());
        vm.setCategoryId(e.getCategoryId());
        vm.setDeptName(e.getDeptName());
        vm.setDesignationId(e.getDesignationId());
        vm.setDesignation(e.getDesignationName());
        vm.setReportId(e.getReportId());
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
        vm.setIsActive(e.getIsActive());
        vm.setAuthorisedEntity(e.getAuthorisedEntity());
        
        // Check CPwd from CPwdManagement table
        vm.setcPwd(false); // Default
        try {
            List<CPwdManagement> cPwdList = cpwdManagementRepository.findByEmpCodeIgnoreCaseAndCpwdAndExpiredAndIsActiveAndIsDeleted(e.getEmpCode(), true, false, true, false);
            if (cPwdList != null && !cPwdList.isEmpty()) {
                vm.setcPwd(true);
            }
        } catch (Exception ex) {
            // If query fails, keep as false
        }
        
        return vm;
    }

    public EmployeeMasterViewModel deleteEmployee(EmployeeMasterViewModel model) {
        Integer loginId = model.getLoginId();
        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }
        Integer empId = model.getEmpId();
        if (empId == null || empId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }
        
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }

        EmployeeMaster emp = empOpt.get();
        emp.setIsDeleted(true);
        emp.setIsActive(false);
        emp.setReason(model.getReason());
        emp.setLastUpdatedBy(loginId);
        emp.setLastUpdatedDate(new java.util.Date());
        employeeMasterRepository.save(emp);

        model.setMsg("Deleted");
        return model;
    }

    public List<DDSalutationViewModel> getDDSalutation(Integer empId) {
        List<SalutationMaster> salutationMasters = salutationMasterRepository.findByIsActiveAndIsDeleted(true, false);
        if (salutationMasters.isEmpty()) {
            return new ArrayList<>();
        }

        return salutationMasters.stream()
            .map(sm -> {
                DDSalutationViewModel vm = new DDSalutationViewModel();
                vm.setSalutationId(sm.getSalutationId());
                vm.setSalutation(sm.getSalutation());
                vm.setEmpId(empId != null ? empId : 0);
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<DDGenderViewModel> getDDGender(Integer empId) {
        List<GenderMaster> genderMasters = genderMasterRepository.findByIsActiveAndIsDeleted(true, false);
        if (genderMasters.isEmpty()) {
            return new ArrayList<>();
        }

        return genderMasters.stream()
            .map(gm -> {
                DDGenderViewModel vm = new DDGenderViewModel();
                vm.setGenderId(gm.getGenderId());
                vm.setGender(gm.getGender());
                vm.setEmpId(0);
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<DDEmpTypeViewModel> getDDEmpType() {
        return empTypeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(et -> {
                DDEmpTypeViewModel vm = new DDEmpTypeViewModel();
                vm.setEmpTypeId(et.getEmpTypId());
                vm.setEmpType(et.getEmpType());
                vm.setDescription(et.getDescription());
                vm.setEmpId(0);
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<DDApproverViewModel> getDDApprover(DDApproverViewModel model) {
        int empId = (model.getEmpId() != null && model.getEmpId() != 0) ? model.getEmpId() : 0;
        int compId = (model.getCompId() != null && model.getCompId() != 0) ? model.getCompId() : 0;
        int leId = (model.getLEId() != null && model.getLEId() != 0) ? model.getLEId() : 0;
        int buId = (model.getBUId() != null && model.getBUId() != 0) ? model.getBUId() : 0;
        int locationId = (model.getLocationId() != null && model.getLocationId() != 0) ? model.getLocationId() : 0;

        if (empId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        final int fCompId = compId;
        List<DDApproverViewModel> approverDetails = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(em -> em.getEmpStatus() != null && em.getEmpStatus().equalsIgnoreCase("ACTIVE"))
            .filter(em -> fCompId == 0 || (em.getCompId() != null && em.getCompId() == fCompId))
            .map(em -> {
                DDApproverViewModel vm = new DDApproverViewModel();
                vm.setApproverId(em.getEmpId());
                String firstName = em.getFirstName() != null ? em.getFirstName() : "";
                String middleName = em.getMiddleName() != null ? em.getMiddleName() : "";
                String lastName = em.getLastName() != null ? em.getLastName() : "";
                String empCode = em.getEmpCode() != null ? em.getEmpCode() : "";
                vm.setApprover((firstName + " " + middleName + " " + lastName + " - " + empCode).trim());
                vm.setCompId(0);
                vm.setLEId(0);
                vm.setBUId(0);
                vm.setLocationId(0);
                vm.setEmpId(0);
                vm.setAuthorisedEntity(null);
                return vm;
            })
            .collect(Collectors.toList());

        if (approverDetails.isEmpty()) {
            throw new RuntimeException("Location Details Not Found");
        }

        return approverDetails;
    }

    public List<EmployeeMasterViewModel> fetchEmployee(EmployeeMasterViewModel model) {
        return employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(e -> {
                EmployeeMasterViewModel vm = new EmployeeMasterViewModel();
                vm.setEmpId(e.getEmpId());
                vm.setEmpCode(e.getEmpCode());
                vm.setFirstName(e.getFirstName());
                vm.setLastName(e.getLastName());
                vm.setUserName(e.getUserName());
                vm.setEmailId(e.getEmailId());
                vm.setMobileNo(e.getMobileNo());
                vm.setEmpStatus(e.getEmpStatus());
                return vm;
            })
            .collect(Collectors.toList());
    }

    public EmployeeMasterViewModel activeEmployee(EmployeeMasterViewModel model) {
        Integer loginId = model.getLoginId();
        Integer empId = model.getEmpId();
        if (loginId == null || loginId == 0) throw new RuntimeException("LoginId is Missing");
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        EmployeeMaster emp = empOpt.get();
        emp.setEmpStatus("Active");
        emp.setReason(model.getReason());
        emp.setIsActive(true);
        emp.setIsUpdated(true);
        emp.setIsDeleted(false);
        emp.setLastUpdatedBy(loginId);
        emp.setLastUpdatedDate(new java.util.Date());
        employeeMasterRepository.save(emp);
        model.setMsg("Actived");
        return model;
    }

    public EmployeeMasterViewModel deActiveEmployee(EmployeeMasterViewModel model) {
        Integer loginId = model.getLoginId();
        Integer empId = model.getEmpId();
        if (loginId == null || loginId == 0) throw new RuntimeException("LoginId is Missing");
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        EmployeeMaster emp = empOpt.get();
        emp.setEndDate(new java.util.Date());
        emp.setEmpStatus("Deactive");
        emp.setReason(model.getReason());
        emp.setIsActive(true);
        emp.setIsUpdated(true);
        emp.setIsDeleted(false);
        emp.setLastUpdatedBy(loginId);
        emp.setLastUpdatedDate(new java.util.Date());
        employeeMasterRepository.save(emp);
        model.setMsg("Deactived");
        return model;
    }

    public List<EmployeeSelectViewModel> selectEmployee(EmployeeMasterViewModel model) {
        Integer empId = model.getEmpId();
        Integer compId = model.getCompId();
        
        List<EmployeeMaster> employees = new ArrayList<>();
        
        if (empId != null && empId != 0) {
            EmployeeMaster emp = employeeMasterRepository.findById(empId)
                .orElseThrow(() -> new RuntimeException("Employee not found with ID: " + empId));
            employees.add(emp);
        } else if (compId != null && compId != 0) {
            employees = employeeMasterRepository.findByCompIdAndIsActiveAndIsDeleted(compId, true, false);
        } else {
            employees = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false);
        }
        
        return employees.stream()
            .map(e -> {
                EmployeeSelectViewModel vm = new EmployeeSelectViewModel();
                vm.setLoginId(null);
                vm.setEmpId(e.getEmpId());
                vm.setCompId(e.getCompId());
                vm.setCompany(e.getCompId() != null ? companyMasterRepository.findById(e.getCompId()).orElse(null).getCompany() : null);
                vm.setDeptName(e.getDeptName());
                vm.setReportId(e.getReportId());
                vm.setEmpCode(e.getEmpCode());
                vm.setFirstName(e.getFirstName());
                vm.setMiddleName(e.getMiddleName());
                vm.setLastName(e.getLastName());
                vm.setIsActive(e.getIsActive());
                vm.setIsUpdated(e.getIsUpdated());
                vm.setIsDeleted(e.getIsDeleted());
                vm.setStartDate(convertToJsonDate(e.getJoiningDate()));
                vm.setTotalEmployeeCount(0);
                vm.setMsg(null);
                vm.setEmpName(null);
                return vm;
            })
            .collect(Collectors.toList());
    }

    public Map<String, Object> getTotalEmployeeCount() {
        Map<String, Object> result = new HashMap<>();
        long count = employeeMasterRepository.count();
        result.put("total", count);
        result.put("active", employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).size());
        return result;
    }

    public List<Map<String, Object>> getDDGetLocation() {
        return locationMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(loc -> {
                Map<String, Object> m = new HashMap<>();
                m.put("LocationId", loc.getLocationId());
                m.put("Location", loc.getLocation());
                // TODO: Get actual employee count for this location
                m.put("EmpId", 0);
                return m;
            })
            .collect(Collectors.toList());
    }

    public List<Map<String, Object>> getDDselectEmployee() {
        return employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(emp -> {
                Map<String, Object> m = new HashMap<>();
                m.put("id", emp.getEmpId());
                m.put("name", emp.getFirstName() + " " + emp.getLastName());
                return m;
            })
            .collect(Collectors.toList());
    }

    public List<Map<String, Object>> getDDEmployeeList() {
        return employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(emp -> {
                Map<String, Object> m = new HashMap<>();
                m.put("EmpId", emp.getEmpId());
                m.put("EmpCode", emp.getEmpCode());
                m.put("FirstName", emp.getFirstName());
                m.put("LastName", emp.getLastName());
                m.put("DisplayName", emp.getFirstName() + " " + emp.getLastName());
                return m;
            })
            .collect(Collectors.toList());
    }

    public List<Map<String, Object>> getDDEmpList(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Integer compId = parseInteger(model.get("CompId"));
        Integer leId = parseInteger(model.get("LEId"));
        Integer buId = parseInteger(model.get("BUId"));
        Integer locationId = parseInteger(model.get("LocationId"));

        return employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(emp -> "ACTIVE".equalsIgnoreCase(emp.getEmpStatus()))
            .filter(emp -> compId == null || compId <= 0 || (emp.getCompId() != null && emp.getCompId().equals(compId)))
            .filter(emp -> leId == null || leId <= 0 || (emp.getLeId() != null && emp.getLeId().equals(leId)))
            .filter(emp -> buId == null || buId <= 0 || (emp.getBuId() != null && emp.getBuId().equals(buId)))
            .filter(emp -> locationId == null || locationId <= 0 || (emp.getLocationId() != null && emp.getLocationId().equals(locationId)))
            .map(emp -> {
                Map<String, Object> m = new HashMap<>();
                m.put("EmpId", emp.getEmpId());
                String empName = (emp.getFirstName() != null ? emp.getFirstName() : "")
                    + " " + (emp.getMiddleName() != null ? emp.getMiddleName() : "")
                    + " " + (emp.getLastName() != null ? emp.getLastName() : "");
                m.put("EmpName", empName.trim().replaceAll("\\s+", " "));
                m.put("EmpCode", emp.getUserName());
                return m;
            })
            .sorted((a, b) -> {
                String na = (String) a.getOrDefault("EmpName", "");
                String nb = (String) b.getOrDefault("EmpName", "");
                return na.compareToIgnoreCase(nb);
            })
            .collect(Collectors.toList());
    }

    public List<EmployeeMasterViewModel> dashboardEmployee(EmployeeMasterViewModel model) {
        if (model.getLoginId() == null || model.getLoginId() == 0) {
            throw new RuntimeException("LoginId is Missing");
        }
        
        return employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .sorted((e1, e2) -> Integer.compare(e2.getEmpId(), e1.getEmpId()))
            .map(e -> {
                EmployeeMasterViewModel vm = new EmployeeMasterViewModel();
                vm.setEmpId(e.getEmpId());
                vm.setCompId(e.getCompId());
                vm.setCompany(e.getCompId() != null ? companyMasterRepository.findById(e.getCompId()).orElse(null).getCompany() : null);
                vm.setDeptName(e.getDeptName());
                vm.setReportId(e.getReportId());
                vm.setEmpCode(e.getEmpCode());
                String empName = e.getFirstName();
                if (e.getMiddleName() != null && !e.getMiddleName().isEmpty()) empName += " " + e.getMiddleName();
                if (e.getLastName() != null && !e.getLastName().isEmpty()) empName += " " + e.getLastName();
                vm.setFirstName(empName.trim());
                vm.setIsActive(e.getIsActive());
                vm.setIsUpdated(e.getIsUpdated());
                vm.setIsDeleted(e.getIsDeleted());
                return vm;
            })
            .collect(Collectors.toList());
    }

    private String getEmployeeFullName(Integer empId) {
        if (empId == null || empId == 0) return "N/A";
        EmployeeMaster emp = employeeMasterRepository.findById(empId).orElse(null);
        if (emp == null) return "N/A";
        String fullName = "";
        if (emp.getFirstName() != null) fullName += emp.getFirstName();
        if (emp.getMiddleName() != null) fullName += " " + emp.getMiddleName();
        if (emp.getLastName() != null) fullName += " " + emp.getLastName();
        return fullName.trim();
    }

    public List<WorkTypeMasterViewModel> getAllWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        List<WorkTypeMaster> workdetails = workTypeMasterRepository.findByEmpIdAndActiveAndNotDeleted(loginId);

        if (workdetails == null || workdetails.isEmpty()) {
            throw new RuntimeException("Work Type Details Not Found");
        }

        List<WorkTypeMasterViewModel> lstofWork = new ArrayList<>();

        for (WorkTypeMaster work : workdetails) {
            WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
            wtvm.setWorkTypeId(work.getWorkTypeId());
            wtvm.setWorkType(work.getWorkType());
            wtvm.setEmpId(work.getEmpId());
            wtvm.setEmpCode(work.getEmpCode());
            wtvm.setEmpName(getEmployeeFullName(work.getEmpId()));
            wtvm.setStartDate(convertToJsonDateObj(work.getStartDate()));
            wtvm.setEndDate(convertToJsonDateObj(work.getEndDate()));
            wtvm.setReason(work.getReason());
            wtvm.setApproverDescription(work.getApproverDescription());
            wtvm.setIsApproved(work.getIsApproved());
            wtvm.setIsApprovedBy(work.getIsApprovedBy());
            wtvm.setIsRejected(work.getIsRejected());
            wtvm.setIsRejectedBy(work.getIsRejectedBy());
            wtvm.setIsEnd(work.getIsEnd());
            wtvm.setIsActive(work.getIsActive());
            wtvm.setIsUpdated(work.getIsUpdated());
            wtvm.setIsDeleted(work.getIsDeleted());
            wtvm.setCreatedBy(work.getCreatedBy());
            wtvm.setCreatedDate(convertToJsonDateObj(work.getCreatedDate()));
            wtvm.setLastUpdatedBy(work.getLastUpdatedBy());
            wtvm.setLastupdatedDate(convertToJsonDateObj(work.getLastupdatedDate()));
            wtvm.setStatus(Boolean.TRUE.equals(work.getIsApproved()) ? "Approved" : Boolean.TRUE.equals(work.getIsRejected()) ? "Rejected" : "Applied");
            wtvm.setApprover(getEmployeeFullName(work.getIsApprovedBy()));
            lstofWork.add(wtvm);
        }

        return lstofWork;
    }

    public WorkTypeMasterViewModel getWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer empId = (model.getEmpId() != null && model.getEmpId() != 0) ? model.getEmpId() : 0;
        Integer wid = (model.getWorkTypeId() != null && model.getWorkTypeId() != 0) ? model.getWorkTypeId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        WorkTypeMaster workdetails = workTypeMasterRepository.findByEmpIdAndWorkTypeIdAndActiveAndNotDeleted(empId, wid);

        if (workdetails != null) {
            WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
            wtvm.setWorkTypeId(workdetails.getWorkTypeId());
            wtvm.setWorkType(workdetails.getWorkType());
            wtvm.setEmpId(workdetails.getEmpId());
            wtvm.setEmpCode(workdetails.getEmpCode());
            wtvm.setEmpName(getEmployeeFullName(workdetails.getEmpId()));
            wtvm.setStartDate(convertToJsonDateObj(workdetails.getStartDate()));
            wtvm.setEndDate(convertToJsonDateObj(workdetails.getEndDate()));
            wtvm.setReason(workdetails.getReason());
            wtvm.setApproverDescription(workdetails.getApproverDescription());
            wtvm.setIsApproved(workdetails.getIsApproved());
            wtvm.setIsApprovedBy(workdetails.getIsApprovedBy());
            wtvm.setApprover(workdetails.getIsApprovedBy() != null && workdetails.getIsApprovedBy() != 0 ? getEmployeeFullName(workdetails.getIsApprovedBy()) : "");
            wtvm.setIsRejected(workdetails.getIsRejected());
            wtvm.setIsRejectedBy(workdetails.getIsRejectedBy());
            wtvm.setRApprover(workdetails.getIsRejectedBy() != null && workdetails.getIsRejectedBy() != 0 ? getEmployeeFullName(workdetails.getIsRejectedBy()) : "");
            wtvm.setIsEnd(workdetails.getIsEnd());
            wtvm.setIsActive(workdetails.getIsActive());
            wtvm.setIsUpdated(workdetails.getIsUpdated());
            wtvm.setIsDeleted(workdetails.getIsDeleted());
            wtvm.setCreatedBy(workdetails.getCreatedBy());
            wtvm.setCreatedDate(convertToJsonDateObj(workdetails.getCreatedDate()));
            wtvm.setLastUpdatedBy(workdetails.getLastUpdatedBy());
            wtvm.setLastupdatedDate(convertToJsonDateObj(workdetails.getLastupdatedDate()));
            return wtvm;
        } else {
            throw new RuntimeException("Work Type Details Not Found");
        }
    }

    public WorkTypeMasterViewModel addWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        List<WorkTypeMaster> workdetails = workTypeMasterRepository.findActiveApprovedWorkTypeByEmpId(loginId);

        if (workdetails.isEmpty()) {
            WorkTypeMaster wt = new WorkTypeMaster();
            wt.setWorkType(model.getWorkType());
            wt.setEmpId(model.getEmpId());
            wt.setEmpCode(model.getEmpCode());
            wt.setStartDate(model.getStartDate() != null ? parseDate(model.getStartDate()) : null);
            wt.setEndDate(model.getEndDate() != null ? parseDate(model.getEndDate()) : null);
            wt.setReason(model.getReason());
            wt.setApproverDescription("");
            wt.setIsApproved(false);
            wt.setIsApprovedBy(0);
            wt.setIsRejected(false);
            wt.setIsRejectedBy(0);
            wt.setIsEnd(model.getIsEnd());
            wt.setIsActive(true);
            wt.setIsUpdated(false);
            wt.setIsDeleted(false);
            wt.setCreatedBy(loginId);
            wt.setCreatedDate(new Date());
            wt.setLastUpdatedBy(loginId);
            wt.setLastupdatedDate(new Date());
            workTypeMasterRepository.save(wt);

            WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
            wtmvm.setEmpId(loginId);
            wtmvm.setMsg("Added");
            return wtmvm;
        } else {
            throw new RuntimeException("Work Type Details Already Exists");
        }
    }

    public WorkTypeMasterViewModel updateWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer wid = (model.getWorkTypeId() != null && model.getWorkTypeId() != 0) ? model.getWorkTypeId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        WorkTypeMaster workdetails = workTypeMasterRepository.findByWorkTypeIdAndActiveAndNotDeleted(wid);

        if (workdetails != null) {
            workdetails.setWorkType(model.getWorkType());
            workdetails.setEmpId(model.getEmpId());
            workdetails.setEmpCode(model.getEmpCode());
            workdetails.setStartDate(model.getStartDate() != null ? parseDate(model.getStartDate()) : null);
            workdetails.setEndDate(model.getEndDate() != null ? parseDate(model.getEndDate()) : null);
            workdetails.setReason(model.getReason());
            workdetails.setApproverDescription("");
            workdetails.setIsApproved(false);
            workdetails.setIsApprovedBy(0);
            workdetails.setIsRejected(false);
            workdetails.setIsRejectedBy(0);
            workdetails.setIsEnd(model.getIsEnd());
            workdetails.setIsUpdated(true);
            workdetails.setLastUpdatedBy(loginId);
            workdetails.setLastupdatedDate(new Date());
            workTypeMasterRepository.save(workdetails);

            WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
            wtmvm.setMsg("Updated");
            return wtmvm;
        } else {
            throw new RuntimeException("Work Type Details Not Found");
        }
    }

    public WorkTypeMasterViewModel deleteWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer wid = (model.getWorkTypeId() != null && model.getWorkTypeId() != 0) ? model.getWorkTypeId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        WorkTypeMaster workdetails = workTypeMasterRepository.findByWorkTypeIdAndActiveAndNotDeleted(wid);

        if (workdetails != null) {
            workdetails.setReason(model.getReason());
            workdetails.setIsActive(true);
            workdetails.setIsUpdated(true);
            workdetails.setIsDeleted(true);
            workdetails.setLastUpdatedBy(model.getLoginId());
            workdetails.setLastupdatedDate(new Date());
            workTypeMasterRepository.save(workdetails);

            WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
            wtmvm.setMsg("Deleted");
            return wtmvm;
        } else {
            throw new RuntimeException("Work Type Details Not Found");
        }
    }

    public List<WorkTypeMasterViewModel> getAllApproverWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        EmployeeMaster currentEmp = employeeMasterRepository.findByEmpIdAndActive(loginId);
        if (currentEmp == null) {
            throw new RuntimeException("Employee Details Not Found");
        }

        Integer oldEmpId = currentEmp.getOldEmp_ID();

        List<EmployeeMaster> empdetails = employeeMasterRepository.findByReportIdOrOldEmpId(loginId, oldEmpId).stream()
            .sorted((a, b) -> Integer.compare(b.getEmpId(), a.getEmpId()))
            .collect(Collectors.toList());

        if (empdetails == null || empdetails.isEmpty()) {
            throw new RuntimeException("Employee Details Not Found");
        }

        List<WorkTypeMasterViewModel> lstofWork = new ArrayList<>();

        // .NET code fetches ALL work types (EmpId filter is commented out)
        List<WorkTypeMaster> workdetails = workTypeMasterRepository.findAllActiveAndNotDeleted().stream()
            .sorted((a, b) -> {
                if (a.getCreatedDate() == null) return 1;
                if (b.getCreatedDate() == null) return -1;
                return b.getCreatedDate().compareTo(a.getCreatedDate());
            })
            .collect(Collectors.toList());

        if (workdetails == null || workdetails.isEmpty()) {
            throw new RuntimeException("Work Type Details Not Found");
        }

        for (WorkTypeMaster work : workdetails) {
            WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
            wtvm.setWorkTypeId(work.getWorkTypeId());
            wtvm.setWorkType(work.getWorkType());
            wtvm.setEmpId(work.getEmpId());
            wtvm.setEmpCode(work.getEmpCode());
            wtvm.setEmpName(getEmployeeFullName(work.getEmpId()));
            wtvm.setStartDate(convertToJsonDateObj(work.getStartDate()));
            wtvm.setEndDate(convertToJsonDateObj(work.getEndDate()));
            wtvm.setReason(work.getReason());
            wtvm.setApproverDescription(work.getApproverDescription());
            wtvm.setIsApproved(work.getIsApproved());
            wtvm.setIsApprovedBy(work.getIsApprovedBy());
            wtvm.setApprover(work.getIsApprovedBy() != null && work.getIsApprovedBy() != 0 ? getEmployeeFullName(work.getIsApprovedBy()) : "");
            wtvm.setIsRejected(work.getIsRejected());
            wtvm.setIsRejectedBy(work.getIsRejectedBy());
            wtvm.setRApprover(work.getIsRejectedBy() != null && work.getIsRejectedBy() != 0 ? getEmployeeFullName(work.getIsRejectedBy()) : "");
            wtvm.setIsEnd(work.getIsEnd());
            wtvm.setIsActive(work.getIsActive());
            wtvm.setIsUpdated(work.getIsUpdated());
            wtvm.setIsDeleted(work.getIsDeleted());
            wtvm.setCreatedBy(work.getCreatedBy());
            wtvm.setCreatedDate(convertToJsonDateObj(work.getCreatedDate()));
            wtvm.setLastUpdatedBy(work.getLastUpdatedBy());
            wtvm.setLastupdatedDate(convertToJsonDateObj(work.getLastupdatedDate()));
            wtvm.setStatus(Boolean.TRUE.equals(work.getIsApproved()) ? "Approved" : Boolean.TRUE.equals(work.getIsRejected()) ? "Rejected" : "Applied");
            lstofWork.add(wtvm);
        }

        return lstofWork;
    }

    public List<DDEmployeeViewModel> ddEmployeeApprover(DDEmployeeViewModel empdd) {
        Integer loginId = (empdd.getLoginId() != null && empdd.getLoginId() != 0) ? empdd.getLoginId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        EmployeeMaster currentEmp = employeeMasterRepository.findByEmpIdAndActive(loginId);
        if (currentEmp == null) {
            throw new RuntimeException("Employee Details Not Found");
        }

        Integer oldempid = currentEmp.getOldEmp_ID();

        List<EmployeeMaster> empdetails = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(e -> (e.getReportId() != null && (e.getReportId().equals(loginId) || e.getReportId().equals(oldempid))))
            .sorted((a, b) -> Integer.compare(b.getEmpId(), a.getEmpId()))
            .collect(Collectors.toList());

        if (empdetails == null || empdetails.isEmpty()) {
            return new ArrayList<>();
        }

        List<DDEmployeeViewModel> lstofDDEmp = new ArrayList<>();

        for (EmployeeMaster emp : empdetails) {
            DDEmployeeViewModel devm = new DDEmployeeViewModel();
            devm.setEmpId(emp.getEmpId());
            devm.setEmpName(emp.getFirstName() + " " + emp.getMiddleName() + " " + emp.getLastName());
            devm.setEmpCode(emp.getUserName());
            lstofDDEmp.add(devm);
        }

        return lstofDDEmp;
    }

    public WorkTypeMasterViewModel approveWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer wid = (model.getWorkTypeId() != null && model.getWorkTypeId() != 0) ? model.getWorkTypeId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        WorkTypeMaster workdetails = workTypeMasterRepository.findPendingApprovalByWorkTypeId(wid);

        if (workdetails != null) {
            workdetails.setApproverDescription(model.getApproverDescription());
            workdetails.setIsApproved(true);
            workdetails.setIsApprovedBy(loginId);
            workdetails.setIsRejected(false);
            workdetails.setIsRejectedBy(0);
            workdetails.setIsEnd(model.getIsEnd());
            workdetails.setIsUpdated(true);
            workdetails.setLastUpdatedBy(loginId);
            workdetails.setLastupdatedDate(new Date());
            workTypeMasterRepository.save(workdetails);

            WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
            wtmvm.setMsg("Approved");
            return wtmvm;
        } else {
            throw new RuntimeException("Work Type Details Not Found");
        }
    }

    public WorkTypeMasterViewModel rejectWorkType(WorkTypeMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer wid = (model.getWorkTypeId() != null && model.getWorkTypeId() != 0) ? model.getWorkTypeId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        WorkTypeMaster workdetails = workTypeMasterRepository.findPendingApprovalByWorkTypeId(wid);

        if (workdetails != null) {
            workdetails.setApproverDescription(model.getApproverDescription());
            workdetails.setIsApproved(false);
            workdetails.setIsApprovedBy(0);
            workdetails.setIsRejected(true);
            workdetails.setIsRejectedBy(loginId);
            workdetails.setIsEnd(model.getIsEnd());
            workdetails.setIsUpdated(true);
            workdetails.setLastUpdatedBy(loginId);
            workdetails.setLastupdatedDate(new Date());
            workTypeMasterRepository.save(workdetails);

            WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
            wtmvm.setMsg("Rejected");
            return wtmvm;
        } else {
            throw new RuntimeException("Work Type Details Not Found");
        }
    }

    public List<WorkTypeMasterViewModel> getAllWorkTypeFilter(WorkTypeFilterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer empId = (model.getEmpId() != null && model.getEmpId() != 0) ? model.getEmpId() : 0;

        String fromDate = model.getFromDate();
        String toDate = model.getToDate();
        if (fromDate == null) fromDate = "";
        if (toDate == null) toDate = "";

        String statusInput = model.getStatus();
        boolean approved = false, rejected = false, active = false, end = false;
        String statusFinal = "";

        if (statusInput != null) {
            String upperStatus = statusInput.toUpperCase();
            if (upperStatus.equals("APPROVED")) {
                active = true; approved = true; statusFinal = upperStatus;
            } else if (upperStatus.equals("REJECTED")) {
                active = true; rejected = true; statusFinal = upperStatus;
            } else if (upperStatus.equals("COMPLETED")) {
                active = true; approved = true; end = true; statusFinal = upperStatus;
            } else if (upperStatus.equals("APPLIED")) {
                active = true; approved = false; end = false; statusFinal = upperStatus;
            } else {
                statusFinal = "";
            }
        } else {
            statusFinal = "";
        }

        final boolean fApproved = approved;
        final boolean fRejected = rejected;
        final boolean fEnd = end;
        final Date fdate = (!fromDate.isEmpty() && !toDate.isEmpty()) ? parseDate(fromDate) : null;
        final Date tdate = (!fromDate.isEmpty() && !toDate.isEmpty()) ? parseDate(toDate) : null;
        final boolean hasDateFilter = !fromDate.isEmpty() && !toDate.isEmpty();
        final boolean hasStatusFilter = !statusFinal.isEmpty();

        List<WorkTypeMaster> workdetails = workTypeMasterRepository.findAllActiveAndNotDeleted();

        if (empId != 0) {
            List<WorkTypeMaster> list = workdetails.stream()
                .filter(w -> w.getEmpId() != null && w.getEmpId().equals(empId))
                .collect(Collectors.toList());

            if (hasDateFilter) {
                if (hasStatusFilter) {
                    list = list.stream()
                        .filter(w -> w.getStartDate() != null && !w.getStartDate().before(fdate)
                            && w.getEndDate() != null && !w.getEndDate().after(tdate)
                            && Boolean.TRUE.equals(w.getIsApproved()) == fApproved
                            && Boolean.TRUE.equals(w.getIsRejected()) == fRejected
                            && Boolean.TRUE.equals(w.getIsEnd()) == fEnd)
                        .collect(Collectors.toList());
                } else {
                    list = list.stream()
                        .filter(w -> w.getStartDate() != null && !w.getStartDate().before(fdate)
                            && w.getEndDate() != null && !w.getEndDate().after(tdate))
                        .collect(Collectors.toList());
                }
            } else {
                if (hasStatusFilter) {
                    list = list.stream()
                        .filter(w -> Boolean.TRUE.equals(w.getIsApproved()) == fApproved
                            && Boolean.TRUE.equals(w.getIsRejected()) == fRejected
                            && Boolean.TRUE.equals(w.getIsEnd()) == fEnd)
                        .collect(Collectors.toList());
                }
            }
            workdetails = list;
        } else {
            List<WorkTypeMaster> list = new ArrayList<>(workdetails);

            if (hasDateFilter) {
                if (hasStatusFilter) {
                    list = list.stream()
                        .filter(w -> w.getStartDate() != null && !w.getStartDate().before(fdate)
                            && w.getEndDate() != null && !w.getEndDate().after(tdate)
                            && Boolean.TRUE.equals(w.getIsApproved()) == fApproved
                            && Boolean.TRUE.equals(w.getIsRejected()) == fRejected
                            && Boolean.TRUE.equals(w.getIsEnd()) == fEnd)
                        .collect(Collectors.toList());
                } else {
                    list = list.stream()
                        .filter(w -> w.getStartDate() != null && !w.getStartDate().before(fdate)
                            && w.getEndDate() != null && !w.getEndDate().after(tdate))
                        .collect(Collectors.toList());
                }
            } else {
                if (hasStatusFilter) {
                    list = list.stream()
                        .filter(w -> Boolean.TRUE.equals(w.getIsApproved()) == fApproved
                            && Boolean.TRUE.equals(w.getIsRejected()) == fRejected
                            && Boolean.TRUE.equals(w.getIsEnd()) == fEnd)
                        .collect(Collectors.toList());
                }
            }
            workdetails = list;
        }

        if (workdetails != null && !workdetails.isEmpty()) {
            List<WorkTypeMasterViewModel> lstofWork = new ArrayList<>();

            for (WorkTypeMaster work : workdetails) {
                WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
                wtvm.setWorkTypeId(work.getWorkTypeId());
                wtvm.setWorkType(work.getWorkType());
                wtvm.setEmpId(work.getEmpId());
                wtvm.setEmpCode(work.getEmpCode());
                wtvm.setEmpName(getEmployeeFullName(work.getEmpId()));
                wtvm.setStartDate(convertToJsonDateObj(work.getStartDate()));
                wtvm.setEndDate(convertToJsonDateObj(work.getEndDate()));
                wtvm.setReason(work.getReason());
                wtvm.setApproverDescription(work.getApproverDescription());
                wtvm.setIsApproved(work.getIsApproved());
                wtvm.setIsApprovedBy(work.getIsApprovedBy());
                wtvm.setApprover(work.getIsApprovedBy() != null && work.getIsApprovedBy() != 0 ? getEmployeeFullName(work.getIsApprovedBy()) : "");
                wtvm.setIsRejected(work.getIsRejected());
                wtvm.setIsRejectedBy(work.getIsRejectedBy());
                wtvm.setRApprover(work.getIsRejectedBy() != null && work.getIsRejectedBy() != 0 ? getEmployeeFullName(work.getIsRejectedBy()) : "");
                wtvm.setIsEnd(work.getIsEnd());
                wtvm.setIsActive(work.getIsActive());
                wtvm.setIsUpdated(work.getIsUpdated());
                wtvm.setIsDeleted(work.getIsDeleted());
                wtvm.setCreatedBy(work.getCreatedBy());
                wtvm.setCreatedDate(convertToJsonDateObj(work.getCreatedDate()));
                wtvm.setLastUpdatedBy(work.getLastUpdatedBy());
                wtvm.setLastupdatedDate(convertToJsonDateObj(work.getLastupdatedDate()));
                lstofWork.add(wtvm);
            }

            return lstofWork;
        } else {
            throw new RuntimeException("Work Type Details Not Found");
        }
    }

    private Date parseDate(String dateStr) {
        try {
            if (dateStr.contains("T")) {
                return new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(dateStr);
            } else if (dateStr.contains("/")) {
                return new SimpleDateFormat("MM/dd/yyyy").parse(dateStr);
            } else if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                return new SimpleDateFormat("dd-MM-yyyy").parse(dateStr);
            } else {
                return new SimpleDateFormat("yyyy-MM-dd").parse(dateStr);
            }
        } catch (Exception e) {
            return new Date();
        }
    }

    public List<Map<String, Object>> getDDEmployeeApprover(DDEmployeeViewModel empdd) {
        Integer loginId = (empdd.getLoginId() != null && empdd.getLoginId() != 0) ? empdd.getLoginId() : 0;

        if (loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        EmployeeMaster currentEmp = employeeMasterRepository.findByEmpIdAndActive(loginId);
        if (currentEmp == null) {
            throw new RuntimeException("Employee Details Not Found");
        }

        Integer oldEmpId = currentEmp.getOldEmp_ID();

        List<EmployeeMaster> empdetails = employeeMasterRepository.findByReportIdOrOldEmpId(loginId, oldEmpId).stream()
            .filter(e -> Boolean.TRUE.equals(e.getIsActive()) && Boolean.FALSE.equals(e.getIsDeleted()))
            .sorted((a, b) -> Integer.compare(b.getEmpId(), a.getEmpId()))
            .collect(Collectors.toList());

        if (empdetails == null || empdetails.isEmpty()) {
            return new ArrayList<>();
        }

        return empdetails.stream()
            .map(emp -> {
                Map<String, Object> m = new HashMap<>();
                m.put("EmpId", emp.getEmpId());
                m.put("EmpName", getEmployeeFullName(emp.getEmpId()));
                m.put("EmpCode", emp.getUserName());
                return m;
            })
            .collect(Collectors.toList());
    }

    public List<Map<String, Object>> employeeAttendance() {
        List<Map<String, Object>> result = new ArrayList<>();
        
        Map<String, Object> attendanceSource = new HashMap<>();
        attendanceSource.put("DeviceCheckInCount", 0);
        attendanceSource.put("OnSiteCount", 0);
        attendanceSource.put("WFHCount", 0);
        addToResult(result, "AttendanceSource", attendanceSource);
        
        Map<String, Object> currentMonthWorkedHours = new HashMap<>();
        currentMonthWorkedHours.put("TotalWH", "00:00:00");
        currentMonthWorkedHours.put("MaxWH", "00:00:00");
        addToResult(result, "CurrentMonthWorkedHours", currentMonthWorkedHours);
        
        List<Map<String, Object>> onTimeCheckIn = new ArrayList<>();
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.DATE, -7);
        for (int i = 0; i < 8; i++) {
            Map<String, Object> entry = new HashMap<>();
            entry.put("Date", sdf.format(cal.getTime()));
            entry.put("OnTimeCheckInCount", 0);
            entry.put("LateCheckInCount", 0);
            onTimeCheckIn.add(entry);
            cal.add(Calendar.DATE, 1);
        }
        addToResult(result, "OnTimeCheckIn", onTimeCheckIn);
        
        addToResult(result, "GetvisitorToday", new ArrayList<>());
        addToResult(result, "CurrentmonthemployeeList", new ArrayList<>());
        addToResult(result, "PendingLeaves", new ArrayList<>());
        addToResult(result, "AllLeaves", new ArrayList<>());
        addToResult(result, "CompOffList", new ArrayList<>());
        
        return result;
    }

    public List<AttendaceDateViewModel> attendanceFilter(Map<String, Object> model) {
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId == 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            Date startDate;
            Date endDate;

            Object startDateObj = model.get("StartDate");
            Object endDateObj = model.get("EndDate");

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            Calendar today = Calendar.getInstance();
            today.set(Calendar.HOUR_OF_DAY, 0);
            today.set(Calendar.MINUTE, 0);
            today.set(Calendar.SECOND, 0);
            today.set(Calendar.MILLISECOND, 0);

            if (startDateObj != null && !startDateObj.toString().isEmpty()) {
                startDate = sdf.parse(startDateObj.toString());
            } else {
                startDate = new Date(today.getTimeInMillis());
                startDate.setDate(1);
            }

            if (endDateObj != null && !endDateObj.toString().isEmpty()) {
                endDate = sdf.parse(endDateObj.toString());
            } else {
                Calendar yesterday = (Calendar) today.clone();
                yesterday.add(Calendar.DATE, -1);
                endDate = yesterday.getTime();
            }

            Integer compId = parseInteger(model.get("CompId"));
            Integer leId = parseInteger(model.get("LEId"));
            Integer buId = parseInteger(model.get("BUId"));
            Integer locId = parseInteger(model.get("LocId"));
            Integer deptId = parseInteger(model.get("DeptId"));
            Integer designationId = parseInteger(model.get("DesignationId"));
            Integer empId = parseInteger(model.get("EmpId"));
            Integer empTypeId = parseInteger(model.get("EmpTypeId"));

            Query spQuery = entityManager.createNativeQuery(
                "EXEC sp_GetAttendanceReport22 @LoginId = :loginId, @CompId = :compId, @LEId = :leId, @BUId = :buId, @LocId = :locId, @DeptId = :deptId, @DesignationId = :designationId, @EmpId = :empId, @StartDate = :startDate, @EndDate = :endDate");
            spQuery.setParameter("loginId", loginId);
            spQuery.setParameter("compId", compId != null ? compId : 0);
            spQuery.setParameter("leId", leId != null ? leId : 0);
            spQuery.setParameter("buId", buId != null ? buId : 0);
            spQuery.setParameter("locId", locId != null ? locId : 0);
            spQuery.setParameter("deptId", deptId != null ? deptId : 0);
            spQuery.setParameter("designationId", designationId != null ? designationId : 0);
            spQuery.setParameter("empId", empId != null ? empId : 0);
            spQuery.setParameter("startDate", new java.sql.Date(startDate.getTime()));
            spQuery.setParameter("endDate", new java.sql.Date(endDate.getTime()));

            List<Object[]> spResult = spQuery.getResultList();
            Map<String, List<AttendanceViewModel>> grouped = new LinkedHashMap<>();
            SimpleDateFormat logDateFormat = new SimpleDateFormat("yyyy-MM-dd");

            for (Object[] row : spResult) {
                if (row.length < 35) continue;
                AttendanceViewModel avm = new AttendanceViewModel();
                int c = 0;
                avm.setEmpId(row[c] != null ? ((Number) row[c]).intValue() : null); c++;
                avm.setEmpCode(row[c] != null ? row[c].toString() : ""); c++;
                avm.setEmpName(row[c] != null ? row[c].toString() : ""); c++;
                Date rowDate = row[c] != null ? (Date) row[c] : null; c++;
                c++; // DayName
                c++; // IsWeekend
                avm.setIsHoliday(row[c] != null && ((row[c] instanceof Boolean) ? (Boolean) row[c] : ((Number) row[c]).intValue() == 1)); c++;
                String li = row[c] != null ? row[c].toString() : "00:00:00"; c++;
                avm.setLogInTime(li);
                avm.setCheckIn(li);
                String lo = row[c] != null ? row[c].toString() : "00:00:00"; c++;
                avm.setLogOutTime(lo);
                avm.setCheckOut(lo);
                avm.setEsslLogInTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setEsslLogOutTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setWfhLogInTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setWfhLogOutTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setOnsiteLogInTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setOnsiteLogOutTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                String wh = row[c] != null ? row[c].toString() : "00:00:00"; c++;
                avm.setWorkingHours(wh);
                avm.setTotalHours(wh);
                c++; // DailyPay
                avm.setPayDays(row[c] != null ? ((Number) row[c]).doubleValue() : 0.0); c++;
                c++; // WorkingHoursDecimal
                avm.setCompId(row[c] != null ? ((Number) row[c]).intValue() : null); c++;
                avm.setCompName(row[c] != null ? row[c].toString() : ""); c++;
                avm.setDesignation(row[c] != null ? row[c].toString() : ""); c++;
                avm.setDeptName(row[c] != null ? row[c].toString() : ""); c++;
                avm.setDeptId(row[c] != null ? ((Number) row[c]).intValue() : null); c++;
                avm.setDesignationId(row[c] != null ? ((Number) row[c]).intValue() : null); c++;
                avm.setLeaveType(row[c] != null ? row[c].toString() : ""); c++;
                String wt = row[c] != null ? row[c].toString() : ""; c++;
                avm.setWorkType(wt);
                avm.setDaysPresent(row[c] != null ? ((Number) row[c]).intValue() : 0); c++;
                avm.setActiveHours(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setEsslActiveHours(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setWfhActiveHours(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setOnsiteActiveHours(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                c++; // MANUALActiveHours
                avm.setShiftName(row[c] != null ? row[c].toString() : "No Shift"); c++;
                avm.setBreakTime(row[c] != null ? row[c].toString() : "00:00:00"); c++;
                avm.setLoginLocation(row[c] != null ? row[c].toString() : ""); c++;
                avm.setLogoutLocation(row[c] != null ? row[c].toString() : ""); c++;

                avm.setStatus(wt == null || wt.isEmpty() || "HOLIDAY".equals(wt) || "WEEKEND".equals(wt) || "RELIEVED".equals(wt) || "NOT JOINED".equals(wt) ? "Absent" : "Present");
                avm.setLogDate(rowDate);

                String dateKey = logDateFormat.format(rowDate);
                grouped.computeIfAbsent(dateKey, k -> new ArrayList<>()).add(avm);
            }

            if (empTypeId != null && empTypeId > 0) {
                List<Integer> validEmpIds = employeeMasterRepository
                    .findByEmpTypeAndIsActiveAndIsDeleted(empTypeId, true, false)
                    .stream()
                    .map(EmployeeMaster::getEmpId)
                    .collect(Collectors.toList());
                grouped.forEach((dateKey, attendanceList) ->
                    attendanceList.removeIf(avm -> !validEmpIds.contains(avm.getEmpId())));
                grouped.values().removeIf(List::isEmpty);
            }

            List<AttendaceDateViewModel> result = new ArrayList<>();
            for (Map.Entry<String, List<AttendanceViewModel>> entry : grouped.entrySet()) {
                AttendaceDateViewModel advm = new AttendaceDateViewModel();
                advm.setAttendaceDate(entry.getKey());
                advm.setLstofAttendance(entry.getValue());
                result.add(advm);
            }
            return result;
        } catch (Exception ex) {
            throw new RuntimeException("Error: " + ex.getMessage());
        }
    }

    public List<AttendaceDateViewModel> eachEmployeeAttendance(Map<String, Object> model) {
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId == 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            Calendar today = Calendar.getInstance();
            today.set(Calendar.HOUR_OF_DAY, 0);
            today.set(Calendar.MINUTE, 0);
            today.set(Calendar.SECOND, 0);
            today.set(Calendar.MILLISECOND, 0);

            Date startDate;
            Date endDate;

            Object startDateObj = model.get("StartDate");
            Object endDateObj = model.get("EndDate");

            if (startDateObj != null && !startDateObj.toString().isEmpty()) {
                startDate = sdf.parse(startDateObj.toString());
            } else {
                startDate = new Date(today.getTimeInMillis());
                startDate.setDate(1);
            }

            if (endDateObj != null && !endDateObj.toString().isEmpty()) {
                endDate = sdf.parse(endDateObj.toString());
            } else {
                Calendar yesterday = (Calendar) today.clone();
                yesterday.add(Calendar.DATE, -1);
                endDate = yesterday.getTime();
            }

            int weekendCount = 0;
            int weekendCount1 = 0;
            Calendar cal = Calendar.getInstance();
            cal.setTime(startDate);
            while (!cal.getTime().after(endDate)) {
                int dayOfWeek = cal.get(Calendar.DAY_OF_WEEK);
                if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) {
                    weekendCount++;
                }
                if (dayOfWeek == Calendar.SUNDAY) {
                    weekendCount1++;
                }
                cal.add(Calendar.DATE, 1);
            }

            Integer clId = leaveTypeMasterRepository.findLeaveTypeIdByShortName("CL");
            Integer elId = leaveTypeMasterRepository.findLeaveTypeIdByShortName("EL");
            Integer rhId = leaveTypeMasterRepository.findLeaveTypeIdByShortName("RH");

            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(loginId);
            if (empOpt.isEmpty() || !Boolean.TRUE.equals(empOpt.get().getIsActive()) ||
                !"ACTIVE".equalsIgnoreCase(empOpt.get().getEmpStatus())) {
                return new ArrayList<>();
            }

            EmployeeMaster empMaster = empOpt.get();
            String empCode = empMaster.getEmpCode() != null ? empMaster.getEmpCode().toUpperCase() : "";

            Optional<CompanyMaster> compOpt = companyMasterRepository.findById(empMaster.getCompId());
            String compName = compOpt.map(CompanyMaster::getCompany).orElse("");

            Optional<DesignationMaster> desigOpt = designationMasterRepository.findById(empMaster.getDesignationId());
            String designationName = desigOpt.map(DesignationMaster::getDesignation).orElse("");

            Optional<DeptMaster> deptOpt = deptMasterRepository.findById(empMaster.getCategoryId());
            String deptName = deptOpt.map(DeptMaster::getDeptName).orElse("");

            String empName = (empMaster.getFirstName() != null ? empMaster.getFirstName() : "") + " " +
                           (empMaster.getMiddleName() != null ? empMaster.getMiddleName() : "") + " " +
                           (empMaster.getLastName() != null ? empMaster.getLastName() : "");

            List<Attendance> logInData = attendanceRepository.findByTypeAndLogDateBetweenAndEmpCode("IN", startDate, endDate, empCode);
            List<Attendance> logOutData = attendanceRepository.findByTypeAndLogDateBetweenAndEmpCode("OUT", startDate, endDate, empCode);
            // Emp_AttendanceTime table does not exist in database, active hours computed from login/logout times below
            List<EmpAttendanceTime> attendanceTimes = new ArrayList<>();
            List<WFHLoginlog> wfhData = wfhLoginlogRepository.findByDateBetweenAndEmpCode(startDate, endDate, empCode);
            List<OnSiteLoginlog> onsiteData = onSiteLoginlogRepository.findByLoginDateBetweenAndEmpCode(startDate, endDate, empCode);
            List<EmpShiftDetail> shiftDetails = empShiftDetailRepository.findByIsActiveAndIsDeletedAndEmpCode(true, false, empCode);

            Integer locationId = empMaster.getLocationId() != null ? empMaster.getLocationId() : 0;

            List<Holiday> holidays = holidayRepository.findByLocationIdAndDateBetween(locationId, startDate, endDate);
            Map<Date, String> holidayDict = new HashMap<>();
            for (Holiday h : holidays) {
                if ("Active".equalsIgnoreCase(h.getStatus())) {
                    holidayDict.put(h.getDate(), h.getTitle());
                }
            }

            List<WeekHoliday> weekHolidays = weekHolidayRepository.findByYearAndStatus(startDate.getYear() + 1900, "Active");
            Map<String, List<Integer>> weeklyHolidayDict = new HashMap<>();
            for (WeekHoliday wh : weekHolidays) {
                if (wh.getLocationId() != null && wh.getLocationId().equals(locationId)) {
                    weeklyHolidayDict.computeIfAbsent(wh.getDay(), k -> new ArrayList<>()).add(wh.getLocationId());
                }
            }

            // Fetch leave applications for date range for leave type computation
            List<EmpLeaveApplication> empLeaves = empLeaveApplicationRepository
                .findByEmpIdAndIsDeleted(empMaster.getEmpId(), false).stream()
                .filter(l -> l.getFromDate() != null && l.getToDate() != null
                    && !l.getToDate().before(startDate) && !l.getFromDate().after(endDate)
                    && Boolean.TRUE.equals(l.getIsActive())
                    && !"CANCELLED".equalsIgnoreCase(l.getStatus())
                    && !"WITHDRAWN".equalsIgnoreCase(l.getStatus()))
                .collect(Collectors.toList());

            List<AttendaceDateViewModel> lstOfDate = new ArrayList<>();

            Calendar dateCal = Calendar.getInstance();
            dateCal.setTime(startDate);
            while (!dateCal.getTime().after(endDate)) {
                Date currentDate = dateCal.getTime();
                String dateStr = sdf.format(currentDate);

                AttendaceDateViewModel advm = new AttendaceDateViewModel();
                advm.setAttendaceDate(dateStr);

                String dayName = new SimpleDateFormat("EEEE").format(currentDate);
                boolean isHoliday = holidayDict.containsKey(currentDate);
                String holidayReason = null;

                if (!isHoliday && weeklyHolidayDict.containsKey(dayName)) {
                    isHoliday = true;
                    holidayReason = dayName;
                } else if (isHoliday) {
                    holidayReason = holidayDict.get(currentDate);
                }

                AttendanceViewModel avm = new AttendanceViewModel();
                avm.setEmpId(empMaster.getEmpId());
                avm.setEmpCode(empMaster.getEmpCode());
                avm.setEmpName(empName.trim());
                avm.setLogDate(currentDate);
                avm.setCompId(empMaster.getCompId());
                avm.setCompName(compName);
                avm.setDesignation(designationName);
                avm.setDeptName(deptName);
                avm.setDeptId(empMaster.getCategoryId());
                avm.setDesignationId(empMaster.getDesignationId());
                avm.setIsHoliday(isHoliday);
                avm.setHolidayName(holidayReason);

                // Determine leave type for this date
                String leaveTypeForDate = "";
                if (isHoliday) {
                    leaveTypeForDate = "Holiday";
                } else {
                    for (EmpLeaveApplication leave : empLeaves) {
                        if (!currentDate.before(leave.getFromDate()) && !currentDate.after(leave.getToDate())) {
                            Integer ltId = leave.getLeaveTypeId();
                            if (ltId != null && ltId == 0) {
                                leaveTypeForDate = "LOP";
                            } else if (ltId != null) {
                                if (ltId.equals(clId)) leaveTypeForDate = "CL";
                                else if (ltId.equals(elId)) leaveTypeForDate = "EL";
                                else if (ltId.equals(rhId)) leaveTypeForDate = "RH";
                                else leaveTypeForDate = "Leave";
                            }
                            break;
                        }
                    }
                }
                avm.setLeaveType(leaveTypeForDate);

                String esslLogInTime = "00:00:00";
                String esslLogOutTime = "00:00:00";
                String logInTime = "00:00:00";
                String logOutTime = "00:00:00";
                String activeHours = "00:00:00";
                String wfhDetails = "";
                String onsiteDetails = "";
                String workType = "";
                String wfhLogInTime = "00:00:00";
                String wfhLogOutTime = "00:00:00";
                String onsiteLogInTime = "00:00:00";
                String onsiteLogOutTime = "00:00:00";
                String wfhActiveHours = "00:00:00";
                String onsiteActiveHours = "00:00:00";
                String esslActiveHours = "00:00:00";

                if (!isHoliday) {
                    Attendance logInEntry = null;
                    Attendance logOutEntry = null;
                    for (Attendance a : logInData) {
                        if (a.getLogDate() != null && sdf.format(a.getLogDate()).equals(dateStr)) {
                            logInEntry = a;
                            break;
                        }
                    }
                    for (Attendance a : logOutData) {
                        if (a.getLogDate() != null && sdf.format(a.getLogDate()).equals(dateStr)) {
                            logOutEntry = a;
                            break;
                        }
                    }

                    if (logInEntry != null && logInEntry.getLogTime() != null) {
                        esslLogInTime = new SimpleDateFormat("HH:mm:ss").format(logInEntry.getLogTime());
                    }
                    if (logOutEntry != null && logOutEntry.getLogTime() != null) {
                        esslLogOutTime = new SimpleDateFormat("HH:mm:ss").format(logOutEntry.getLogTime());
                    }

                    if (esslLogInTime.equals("00:00:00") && esslLogOutTime.equals("00:00:00")) {
                        List<WFHLoginlog> wfhEntries = wfhData.stream()
                            .filter(w -> w.getDate() != null && sdf.format(w.getDate()).equals(dateStr))
                            .collect(Collectors.toList());

                        if (!wfhEntries.isEmpty()) {
                            wfhEntries.sort((a, b) -> {
                                if (a.getLoginTime() == null) return 1;
                                if (b.getLoginTime() == null) return -1;
                                return a.getLoginTime().compareTo(b.getLoginTime());
                            });

                            Date firstLogin = null;
                            Date lastLogout = null;
                            long totalWfhMs = 0;

                            for (int i = 0; i < wfhEntries.size(); i++) {
                                WFHLoginlog entry = wfhEntries.get(i);
                                if (entry.getLoginTime() == null) continue;

                                if (firstLogin == null || entry.getLoginTime().before(firstLogin)) {
                                    firstLogin = entry.getLoginTime();
                                }

                                Date logOut;
                                if (entry.getLogOutTime() != null) {
                                    logOut = entry.getLogOutTime();
                                } else if (i + 1 < wfhEntries.size() && wfhEntries.get(i + 1).getLoginTime() != null) {
                                    logOut = wfhEntries.get(i + 1).getLoginTime();
                                } else {
                                    Calendar defaultLogout = Calendar.getInstance();
                                    defaultLogout.set(Calendar.HOUR_OF_DAY, 18);
                                    defaultLogout.set(Calendar.MINUTE, 35);
                                    defaultLogout.set(Calendar.SECOND, 0);
                                    logOut = defaultLogout.getTime();
                                }

                                if (lastLogout == null || logOut.after(lastLogout)) {
                                    lastLogout = logOut;
                                }

                                if (logOut.after(entry.getLoginTime())) {
                                    totalWfhMs += logOut.getTime() - entry.getLoginTime().getTime();
                                }
                            }

                            if (firstLogin != null) {
                                wfhLogInTime = new SimpleDateFormat("HH:mm:ss").format(firstLogin);
                            }
                            if (lastLogout != null) {
                                wfhLogOutTime = new SimpleDateFormat("HH:mm:ss").format(lastLogout);
                            }
                            logInTime = wfhLogInTime;
                            logOutTime = wfhLogOutTime;
                            wfhDetails = "1";
                            workType = "WFH";

                            if (totalWfhMs > 0) {
                                long hours = totalWfhMs / (1000 * 60 * 60);
                                long mins = (totalWfhMs % (1000 * 60 * 60)) / (1000 * 60);
                                long secs = (totalWfhMs % (1000 * 60)) / 1000;
                                wfhActiveHours = String.format("%02d:%02d:%02d", hours, mins, secs);
                                activeHours = wfhActiveHours;
                            }
                        } else {
                            List<OnSiteLoginlog> onsiteEntries = onsiteData.stream()
                                .filter(o -> o.getLoginDate() != null && sdf.format(o.getLoginDate()).equals(dateStr))
                                .collect(Collectors.toList());

                            if (!onsiteEntries.isEmpty()) {
                                onsiteEntries.sort((a, b) -> {
                                    if (a.getLogInTime() == null) return 1;
                                    if (b.getLogInTime() == null) return -1;
                                    return a.getLogInTime().compareTo(b.getLogInTime());
                                });

                                Date onsiteFirstLogin = null;
                                Date onsiteLastLogout = null;
                                long totalOnsiteMs = 0;

                                for (int i = 0; i < onsiteEntries.size(); i++) {
                                    OnSiteLoginlog entry = onsiteEntries.get(i);
                                    if (entry.getLogInTime() == null) continue;

                                    if (onsiteFirstLogin == null || entry.getLogInTime().before(onsiteFirstLogin)) {
                                        onsiteFirstLogin = entry.getLogInTime();
                                    }

                                    Date logOut;
                                    if (entry.getLogOutTime() != null) {
                                        logOut = entry.getLogOutTime();
                                    } else if (i + 1 < onsiteEntries.size() && onsiteEntries.get(i + 1).getLogInTime() != null) {
                                        logOut = onsiteEntries.get(i + 1).getLogInTime();
                                    } else {
                                        Calendar defaultLogout = Calendar.getInstance();
                                        defaultLogout.set(Calendar.HOUR_OF_DAY, 18);
                                        defaultLogout.set(Calendar.MINUTE, 36);
                                        defaultLogout.set(Calendar.SECOND, 0);
                                        logOut = defaultLogout.getTime();
                                    }

                                    if (onsiteLastLogout == null || logOut.after(onsiteLastLogout)) {
                                        onsiteLastLogout = logOut;
                                    }

                                    if (logOut.after(entry.getLogInTime())) {
                                        totalOnsiteMs += logOut.getTime() - entry.getLogInTime().getTime();
                                    }
                                }

                                if (onsiteFirstLogin != null) {
                                    onsiteLogInTime = new SimpleDateFormat("HH:mm:ss").format(onsiteFirstLogin);
                                }
                                if (onsiteLastLogout != null) {
                                    onsiteLogOutTime = new SimpleDateFormat("HH:mm:ss").format(onsiteLastLogout);
                                }
                                logInTime = onsiteLogInTime;
                                logOutTime = onsiteLogOutTime;
                                onsiteDetails = "2";
                                workType = "OnSite";

                                if (totalOnsiteMs > 0) {
                                    long hours = totalOnsiteMs / (1000 * 60 * 60);
                                    long mins = (totalOnsiteMs % (1000 * 60 * 60)) / (1000 * 60);
                                    long secs = (totalOnsiteMs % (1000 * 60)) / 1000;
                                    onsiteActiveHours = String.format("%02d:%02d:%02d", hours, mins, secs);
                                    activeHours = onsiteActiveHours;
                                }
                            }
                        }
                    } else {
                        logInTime = esslLogInTime;
                        logOutTime = esslLogOutTime;
                        workType = "ESSL";

                        EmpAttendanceTime attTime = null;
                        for (EmpAttendanceTime at : attendanceTimes) {
                            if (at.getLogDate() != null && sdf.format(at.getLogDate()).equals(dateStr)) {
                                if (attTime == null || (at.getAttendHours() != null && attTime.getAttendHours() != null && at.getAttendHours() > attTime.getAttendHours())) {
                                    attTime = at;
                                }
                            }
                        }

                        if (attTime != null && attTime.getDuration() != null) {
                            activeHours = new SimpleDateFormat("HH:mm:ss").format(attTime.getDuration());
                        } else if (logInEntry != null && logInEntry.getLogTime() != null && logOutEntry != null && logOutEntry.getLogTime() != null) {
                            long diffMs = logOutEntry.getLogTime().getTime() - logInEntry.getLogTime().getTime();
                            long hours = diffMs / (1000 * 60 * 60);
                            long mins = (diffMs % (1000 * 60 * 60)) / (1000 * 60);
                            long secs = (diffMs % (1000 * 60)) / 1000;
                            activeHours = String.format("%02d:%02d:%02d", hours, mins, secs);
                        }
                    }

                    avm.setEsslLogInTime(esslLogInTime);
                    avm.setEsslLogOutTime(esslLogOutTime);
                    avm.setWfhLogInTime(wfhLogInTime);
                    avm.setWfhLogOutTime(wfhLogOutTime);
                    avm.setOnsiteLogInTime(onsiteLogInTime);
                    avm.setOnsiteLogOutTime(onsiteLogOutTime);
                    avm.setEsslActiveHours(esslActiveHours);
                    avm.setWfhActiveHours(wfhActiveHours);
                    avm.setOnsiteActiveHours(onsiteActiveHours);
                }

                avm.setLogInTime(logInTime);
                avm.setLogOutTime(logOutTime);
                avm.setWorkingHours(activeHours);
                avm.setActiveHours(activeHours);
                avm.setWorkType(workType);

                // Set DaysPresent for valid attendance (matching .NET behavior)
                int daysPresent = isHoliday ? 0 : (!logInTime.equals("00:00:00") || !logOutTime.equals("00:00:00") ? 1 : 0);
                avm.setDaysPresent(daysPresent);

                EmpShiftDetail empShift = null;
                for (EmpShiftDetail shift : shiftDetails) {
                    if (shift.getStartDate() != null && shift.getEndDate() != null) {
                        if (!currentDate.before(shift.getStartDate()) && !currentDate.after(shift.getEndDate())) {
                            empShift = shift;
                            break;
                        }
                    }
                }
                avm.setShiftName(empShift != null ? empShift.getShiftName() : "No Shift");

                List<EmpLeaveApplication> lopApplications = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndStartDateBetweenAndEndDateBetween(
                    empMaster.getEmpId(), 0, startDate, endDate);
                double lopDuration = 0;
                for (EmpLeaveApplication app : lopApplications) {
                    if (app.getNoOfDays() != null) {
                        lopDuration += app.getNoOfDays();
                    }
                }

                long diffDays = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24);
                double totalDays = (double) diffDays;
                double workingDays = totalDays - lopDuration;
                avm.setPayDays(workingDays);

                List<AttendanceViewModel> lstOfAtt = new ArrayList<>();
                lstOfAtt.add(avm);
                advm.setLstofAttendance(lstOfAtt);
                lstOfDate.add(advm);

                dateCal.add(Calendar.DATE, 1);
            }

            // Sort by date descending to match .NET behavior
            lstOfDate.sort((a, b) -> b.getAttendaceDate().compareTo(a.getAttendaceDate()));
            return lstOfDate;
        } catch (RuntimeException ex) {
            throw ex;
        } catch (Exception ex) {
            throw new RuntimeException("Error: " + ex.getMessage());
        }
    }

    public List<AttendaceDateViewModel> reportingEmployeeAttendance(Map<String, Object> model) {
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId == 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            SimpleDateFormat timeFormat = new SimpleDateFormat("HH:mm");
            Calendar today = Calendar.getInstance();
            today.set(Calendar.HOUR_OF_DAY, 0);
            today.set(Calendar.MINUTE, 0);
            today.set(Calendar.SECOND, 0);
            today.set(Calendar.MILLISECOND, 0);

            Date startDate;
            Date endDate;

            Object startDateObj = model.get("StartDate");
            Object endDateObj = model.get("EndDate");

            if (startDateObj != null && !startDateObj.toString().isEmpty()) {
                startDate = sdf.parse(startDateObj.toString());
            } else {
                startDate = new Date(today.getTimeInMillis());
                startDate.setDate(1);
            }

            if (endDateObj != null && !endDateObj.toString().isEmpty()) {
                endDate = sdf.parse(endDateObj.toString());
            } else {
                Calendar yesterday = (Calendar) today.clone();
                yesterday.add(Calendar.DATE, -1);
                endDate = yesterday.getTime();
            }

            Integer compId = parseInteger(model.get("CompId"));
            Integer deptId = parseInteger(model.get("DeptId"));
            Integer designationId = parseInteger(model.get("DesignationId"));
            Integer empId = parseInteger(model.get("EmpId"));

            List<EmployeeMaster> employees = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
                .filter(e -> "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
                .filter(e -> compId == null || compId == 0 || (e.getCompId() != null && e.getCompId().equals(compId)))
                .filter(e -> deptId == null || deptId == 0 || (e.getCategoryId() != null && e.getCategoryId().equals(deptId)))
                .filter(e -> designationId == null || designationId == 0 || (e.getDesignationId() != null && e.getDesignationId().equals(designationId)))
                .filter(e -> empId == null || empId == 0 || e.getEmpId().equals(empId))
                .sorted((a, b) -> {
                    String nameA = (a.getFirstName() != null ? a.getFirstName() : "") + " " + (a.getLastName() != null ? a.getLastName() : "");
                    String nameB = (b.getFirstName() != null ? b.getFirstName() : "") + " " + (b.getLastName() != null ? b.getLastName() : "");
                    return nameA.trim().compareToIgnoreCase(nameB.trim());
                })
                .collect(Collectors.toList());

            if (employees.isEmpty()) {
                return new ArrayList<>();
            }

            List<String> empCodes = employees.stream()
                .map(e -> e.getEmpCode() != null ? e.getEmpCode().toUpperCase() : "")
                .filter(c -> !c.isEmpty())
                .collect(Collectors.toList());

            List<Attendance> allLogInData = attendanceRepository.findByTypeAndLogDateBetween("IN", startDate, endDate);
            List<Attendance> allLogOutData = attendanceRepository.findByTypeAndLogDateBetween("OUT", startDate, endDate);
            List<WFHLoginlog> allWFHData = wfhLoginlogRepository.findByDateBetween(startDate, endDate);
            List<OnSiteLoginlog> allOnsiteData = onSiteLoginlogRepository.findByLoginDateBetween(startDate, endDate);

            Map<String, EmployeeMaster> empCodeMap = employees.stream()
                .filter(e -> e.getEmpCode() != null)
                .collect(Collectors.toMap(e -> e.getEmpCode().toUpperCase(), e -> e, (a, b) -> a));

            Calendar dateCal = Calendar.getInstance();
            dateCal.setTime(startDate);
            List<AttendaceDateViewModel> lstOfDate = new ArrayList<>();

            while (!dateCal.getTime().after(endDate)) {
                Date currentDate = dateCal.getTime();
                String dateStr = sdf.format(currentDate);

                AttendaceDateViewModel advm = new AttendaceDateViewModel();
                advm.setAttendaceDate(dateStr);

                List<AttendanceViewModel> lstOfAtt = new ArrayList<>();

                for (EmployeeMaster emp : employees) {
                    String empCode = emp.getEmpCode() != null ? emp.getEmpCode().toUpperCase() : "";
                    String empName = (emp.getFirstName() != null ? emp.getFirstName() : "") + " " +
                        (emp.getMiddleName() != null ? emp.getMiddleName() : "") + " " +
                        (emp.getLastName() != null ? emp.getLastName() : "");

                    Attendance logInEntry = null;
                    Attendance logOutEntry = null;

                    for (Attendance a : allLogInData) {
                        if (a.getEmpCode() != null && a.getEmpCode().toUpperCase().equals(empCode) && a.getLogDate() != null) {
                            Calendar logCal = Calendar.getInstance();
                            logCal.setTime(a.getLogDate());
                            String logDateStr = sdf.format(a.getLogDate());
                            if (logDateStr.equals(dateStr)) {
                                logInEntry = a;
                                break;
                            }
                        }
                    }

                    for (Attendance a : allLogOutData) {
                        if (a.getEmpCode() != null && a.getEmpCode().toUpperCase().equals(empCode) && a.getLogDate() != null) {
                            Calendar logCal = Calendar.getInstance();
                            logCal.setTime(a.getLogDate());
                            String logDateStr = sdf.format(a.getLogDate());
                            if (logDateStr.equals(dateStr)) {
                                logOutEntry = a;
                                break;
                            }
                        }
                    }

                    String checkIn = "00:00";
                    String checkOut = "00:00";
                    String totalHours = "00:00";
                    String status = "Absent";
                    String workType = "";

                    if (logInEntry != null) {
                        checkIn = timeFormat.format(logInEntry.getLogTime());
                        if (logOutEntry != null) {
                            checkOut = timeFormat.format(logOutEntry.getLogTime());
                            long diffMs = logOutEntry.getLogTime().getTime() - logInEntry.getLogTime().getTime();
                            if (diffMs > 0) {
                                long hours = diffMs / (1000 * 60 * 60);
                                long mins = (diffMs % (1000 * 60 * 60)) / (1000 * 60);
                                totalHours = String.format("%02d:%02d", hours, mins);
                                status = "Present";
                            }
                        } else {
                            status = "Half Day";
                            Calendar defaultOut = Calendar.getInstance();
                            defaultOut.setTime(logInEntry.getLogTime());
                            defaultOut.add(Calendar.HOUR_OF_DAY, 9);
                            checkOut = timeFormat.format(defaultOut.getTime());
                            totalHours = "09:00";
                        }
                    } else {
                        List<WFHLoginlog> wfhEntries = allWFHData.stream()
                            .filter(w -> w.getEmpCode() != null && w.getEmpCode().toUpperCase().equals(empCode) && w.getDate() != null && sdf.format(w.getDate()).equals(dateStr))
                            .collect(Collectors.toList());

                        if (!wfhEntries.isEmpty()) {
                            workType = "WFH";
                            Date firstLogin = null;
                            Date lastLogout = null;
                            for (WFHLoginlog w : wfhEntries) {
                                if (w.getLoginTime() != null) {
                                    if (firstLogin == null || w.getLoginTime().before(firstLogin)) {
                                        firstLogin = w.getLoginTime();
                                    }
                                }
                                if (w.getLogOutTime() != null) {
                                    if (lastLogout == null || w.getLogOutTime().after(lastLogout)) {
                                        lastLogout = w.getLogOutTime();
                                    }
                                }
                            }
                            if (firstLogin != null) {
                                checkIn = timeFormat.format(firstLogin);
                            }
                            if (lastLogout != null) {
                                checkOut = timeFormat.format(lastLogout);
                                long diffMs = lastLogout.getTime() - firstLogin.getTime();
                                if (diffMs > 0) {
                                    long hours = diffMs / (1000 * 60 * 60);
                                    long mins = (diffMs % (1000 * 60 * 60)) / (1000 * 60);
                                    totalHours = String.format("%02d:%02d", hours, mins);
                                }
                            }
                            status = "Present";
                        } else {
                            List<OnSiteLoginlog> onsiteEntries = allOnsiteData.stream()
                                .filter(o -> o.getEmpCode() != null && o.getEmpCode().toUpperCase().equals(empCode) && o.getLoginDate() != null && sdf.format(o.getLoginDate()).equals(dateStr))
                                .collect(Collectors.toList());

                            if (!onsiteEntries.isEmpty()) {
                                workType = "OnSite";
                                Date firstLogin = null;
                                Date lastLogout = null;
                                for (OnSiteLoginlog o : onsiteEntries) {
                                    if (o.getLogInTime() != null) {
                                        if (firstLogin == null || o.getLogInTime().before(firstLogin)) {
                                            firstLogin = o.getLogInTime();
                                        }
                                    }
                                    if (o.getLogOutTime() != null) {
                                        if (lastLogout == null || o.getLogOutTime().after(lastLogout)) {
                                            lastLogout = o.getLogOutTime();
                                        }
                                    }
                                }
                                if (firstLogin != null) {
                                    checkIn = timeFormat.format(firstLogin);
                                }
                                if (lastLogout != null) {
                                    checkOut = timeFormat.format(lastLogout);
                                    long diffMs = lastLogout.getTime() - firstLogin.getTime();
                                    if (diffMs > 0) {
                                        long hours = diffMs / (1000 * 60 * 60);
                                        long mins = (diffMs % (1000 * 60 * 60)) / (1000 * 60);
                                        totalHours = String.format("%02d:%02d", hours, mins);
                                    }
                                }
                                status = "Present";
                            }
                        }
                    }

                    AttendanceViewModel avm = new AttendanceViewModel();
                    avm.setEmpId(emp.getEmpId());
                    avm.setEmpCode(emp.getEmpCode());
                    avm.setEmpName(empName.trim());
                    avm.setLogInTime(checkIn);
                    avm.setLogOutTime(checkOut);
                    avm.setWorkingHours(totalHours);
                    avm.setWorkType(workType);
                    avm.setStatus(status);
                    avm.setCheckIn(checkIn);
                    avm.setCheckOut(checkOut);
                    avm.setTotalHours(totalHours);

                    lstOfAtt.add(avm);
                }

                advm.setLstofAttendance(lstOfAtt);
                lstOfDate.add(advm);

                dateCal.add(Calendar.DATE, 1);
            }

            return lstOfDate;
        } catch (Exception ex) {
            throw new RuntimeException("Error: " + ex.getMessage());
        }
    }

    public Map<String, Object> onSiteLogin(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("LoginId"));
        if (empId == 0) throw new RuntimeException("EmpId is Mismatching");

        List<Loginlog> existingLogins = loginlogRepository.findByEmpIdAndActionTypeAndIsActiveAndIsDeletedOrderByCreatedDateDesc(empId, "LOGIN", true, false);
        if (!existingLogins.isEmpty()) throw new RuntimeException("User Already Logged In");

        Loginlog log = new Loginlog();
        log.setEmpId(empId);
        log.setEmpCode(parseString(model.get("EmpCode")));
        log.setLoginAddress(parseString(model.get("LoginAddress")));
        log.setLoginCity(parseString(model.get("LoginCity")));
        log.setLoginDate(new Date());
        log.setLoginLongitude(parseString(model.get("LoginLongitude")));
        log.setLoginLatitude(parseString(model.get("LoginLatitude")));
        log.setActionType("LOGIN");
        log.setLogoutAddress("");
        log.setLogoutCity("");
        log.setLogoutLongitude("");
        log.setLogoutLatitude("");
        log.setLogoutDate(null);
        log.setLogInTime(new Date());
        log.setCreatedBy(empId);
        log.setCreatedDate(new Date());
        log.setIsActive(true);
        log.setIsUpdated(false);
        log.setIsDeleted(false);
        loginlogRepository.save(log);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Login");
        result.put("Id", log.getId());
        return result;
    }

    public Map<String, Object> onSiteLogout(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("LoginId"));
        Integer id = parseInteger(model.get("Id"));
        if (empId == 0) throw new RuntimeException("EmpId is Mismatching");

        Optional<Loginlog> opt = loginlogRepository.findByEmpIdAndActionTypeAndIdAndIsActiveAndIsDeleted(empId, "LOGIN", id, true, false);
        if (opt.isEmpty()) throw new RuntimeException("Login record not found");

        Loginlog log = opt.get();
        log.setLogoutAddress(parseString(model.get("LogoutAddress")));
        log.setLogoutCity(parseString(model.get("LogoutCity")));
        log.setLogoutDate(new Date());
        log.setLogoutLongitude(parseString(model.get("LogoutLongitude")));
        log.setLogoutLatitude(parseString(model.get("LogoutLatitude")));
        log.setLogOutTime(new Date());
        log.setActionType("LOGOUT");
        log.setIsUpdated(true);
        log.setLastUpdatedBy(empId);
        log.setLastUpdatedDate(new Date());
        loginlogRepository.save(log);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Logout");
        return result;
    }

    public List<Map<String, Object>> getAllLoginLogs() {
        List<Map<String, Object>> result = new ArrayList<>();
        List<Loginlog> logs = loginlogRepository.findByIsActiveAndIsDeletedOrderByCreatedDateDesc(true, false);
        for (Loginlog log : logs) {
            Map<String, Object> m = new HashMap<>();
            m.put("Id", log.getId());
            m.put("EmpId", log.getEmpId());
            m.put("EmpCode", log.getEmpCode());
            m.put("LoginTime", log.getLogInTime());
            m.put("LogoutTime", log.getLogOutTime());
            m.put("LoginDate", log.getLoginDate());
            m.put("LoginAddress", log.getLoginAddress());
            m.put("LoginCity", log.getLoginCity());
            m.put("LoginLatitude", log.getLoginLatitude());
            m.put("LoginLongitude", log.getLoginLongitude());
            m.put("LogoutAddress", log.getLogoutAddress());
            m.put("LogoutCity", log.getLogoutCity());
            m.put("LogoutLatitude", log.getLogoutLatitude());
            m.put("LogoutLongitude", log.getLogoutLongitude());
            m.put("ActionType", log.getActionType());
            result.add(m);
        }
        return result;
    }

    public Map<String, Object> uploadImage(Integer empId, String imageType, MultipartFile file) {
        if (imageType == null || imageType.isEmpty()) throw new RuntimeException("Invalid Image Type");
        String folderName = switch (imageType.toUpperCase()) {
            case "PROFILEPIC" -> "ProfilePic";
            case "LOGO" -> "Logo";
            case "LOGOWITHADDRESS" -> "LogoWithAddress";
            case "WEBAPPLOGO" -> "WebAppLogo";
            default -> throw new RuntimeException("Invalid Image Type");
        };
        String uploadDir = "Uploads/Images/" + folderName;
        return saveFile(uploadDir, folderName + "_" + empId, file, false);
    }

    public Map<String, Object> uploadFileEducation(Integer empId, String docName, MultipartFile file) {
        String uploadDir = "Uploads/File/Education";
        String cleanName = docName.toUpperCase().replace(" ", "");

        if ("GRADUATE".equals(cleanName)) {
            uploadDir = "Uploads/File/Education/Graduate";
        } else if ("POSTGRADUATE".equals(cleanName)) {
            uploadDir = "Uploads/File/Education/PostGraduate";
        } else if ("HSC".equals(cleanName)) {
            uploadDir = "Uploads/File/Education/HSC";
        } else if ("SSLC".equals(cleanName)) {
            uploadDir = "Uploads/File/Education/SSLC";
        } else if ("OTHERS".equals(cleanName)) {
            uploadDir = "Uploads/File/Education/Others";
        }
        return saveFile(uploadDir, cleanName + "_" + empId, file, true);
    }

    public Map<String, Object> uploadFileGovt(Integer empId, String docName, MultipartFile file) {
        String uploadDir = "Uploads/File/Govt";
        String cleanName = docName.toUpperCase().replace(" ", "");

        if ("AADHARCARD".equals(cleanName)) {
            uploadDir = "Uploads/File/Govt/Aadharcard";
        } else if ("PANCARD".equals(cleanName)) {
            uploadDir = "Uploads/File/Govt/Pancard";
        } else if ("VOTERID".equals(cleanName)) {
            uploadDir = "Uploads/File/Govt/VoterId";
        } else if ("DRIVINGLISENCE".equals(cleanName)) {
            uploadDir = "Uploads/File/Govt/Drivinglisence";
        } else if ("OTHERS".equals(cleanName)) {
            uploadDir = "Uploads/File/Govt/Others";
        }
        return saveFile(uploadDir, cleanName + "_" + empId, file, true);
    }

    public Map<String, Object> uploadFileCareer(Integer empId, String docName, MultipartFile file) {
        String uploadDir = "Uploads/File/Career";
        String fileNamePrefix = docName.toUpperCase().replace(" ", "");

        String cleanName = docName.toUpperCase().replace(" ", "");
        if ("EXPERIENCELETTER".equals(cleanName)) {
            uploadDir = "Uploads/File/Career/ExperienceLetter";
        } else if ("OFFERLETTER".equals(cleanName)) {
            uploadDir = "Uploads/File/Career/OfferLetter";
        } else if ("PAYSLIP".equals(cleanName) || cleanName.startsWith("PAYSLIP")) {
            uploadDir = "Uploads/File/Career/PaySlip";
        } else if ("RELIEVINGLETTER".equals(cleanName)) {
            uploadDir = "Uploads/File/Career/RelievingLetter";
        } else if ("SALARYINCREMENTLETTER".equals(cleanName)) {
            uploadDir = "Uploads/File/Career/SalaryIncrementLetter";
        }
        return saveFile(uploadDir, fileNamePrefix + "_" + empId, file, true);
    }

    private Map<String, Object> saveFile(String uploadDir, String filePrefix, MultipartFile file, boolean keepExtension) {
        try {
            if (file.isEmpty()) throw new RuntimeException("No file uploaded");
            String ext = "";
            String originalName = file.getOriginalFilename();
            if (originalName != null && originalName.contains(".")) {
                ext = originalName.substring(originalName.lastIndexOf("."));
            }
            String timestamp = java.time.LocalDateTime.now().format(java.time.format.DateTimeFormatter.ofPattern("yyyyMMddHHmmss"));
            String fileName = filePrefix + "_" + timestamp + ext;
            Path targetPath = Paths.get(uploadDir).resolve(fileName);
            Files.createDirectories(targetPath.getParent());
            Files.copy(file.getInputStream(), targetPath, StandardCopyOption.REPLACE_EXISTING);
            String relativePath = uploadDir + "/" + fileName;
            Map<String, Object> result = new HashMap<>();
            result.put("msg", uploadDir.substring(uploadDir.lastIndexOf("/") + 1) + " Uploaded");
            result.put("path", relativePath);
            return result;
        } catch (IOException e) {
            throw new RuntimeException("File upload failed: " + e.getMessage());
        }
    }

    public List<Map<String, Object>> ddEducationDoc(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<DocumentMaster> docs = documentMasterRepository.findByIsActiveAndIsDeletedAndEduId(true, false, 1);
        if (docs.isEmpty()) return new ArrayList<>();

        return docs.stream().map(doc -> {
            Map<String, Object> vm = new HashMap<>();
            vm.put("DocId", doc.getDocId());
            vm.put("EduId", doc.getEduId());
            vm.put("DocName", doc.getDocName());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> ddGovtDoc(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<DocumentMaster> docs = documentMasterRepository.findByIsActiveAndIsDeletedAndEduId(true, false, 2);
        if (docs.isEmpty()) return new ArrayList<>();

        return docs.stream().map(doc -> {
            Map<String, Object> vm = new HashMap<>();
            vm.put("DocId", doc.getDocId());
            vm.put("EduId", doc.getEduId());
            vm.put("DocName", doc.getDocName());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getAllEducationDoc() {
        List<EmployeeEducation> educations = employeeEducationRepository.findByIsActiveAndIsDeleted(true, false);
        if (educations.isEmpty()) return new ArrayList<>();

        List<Integer> docIds = educations.stream().map(EmployeeEducation::getDocId).filter(Objects::nonNull).distinct().collect(Collectors.toList());
        Map<Integer, String> docNames = docIds.isEmpty() ? new HashMap<>() :
            documentMasterRepository.findAllById(docIds).stream()
                .collect(Collectors.toMap(d -> d.getDocId(), d -> d.getDocName() != null ? d.getDocName() : ""));

        return educations.stream().map(edu -> {
            Map<String, Object> vm = new HashMap<>();
            vm.put("Id", edu.getId());
            vm.put("EmpId", edu.getEmpId());
            vm.put("DocId", edu.getDocId());
            vm.put("DocName", docNames.getOrDefault(edu.getDocId(), ""));
            vm.put("Others", edu.getOthers());
            vm.put("School", edu.getSchool());
            vm.put("DegreeId", edu.getDegreeId());
            vm.put("Filed", edu.getFiled());
            vm.put("StartDate", convertToJsonDate(edu.getStartDate()));
            vm.put("EndDate", convertToJsonDate(edu.getEndDate()));
            vm.put("Grade", edu.getGrade());
            vm.put("Description", edu.getDescription());
            String path = edu.getPath();
            if (path != null && !path.isEmpty() && path.contains("Uploads")) {
                String[] parts = path.split("Uploads", 2);
                if (parts.length > 1) path = "Uploads" + parts[1];
            }
            vm.put("Path", path != null ? path : "");
            vm.put("CreatedBy", edu.getCreatedBy());
            vm.put("CreatedDate", edu.getCreatedDate());
            vm.put("LastUpdatedBy", edu.getLastUpdatedBy());
            vm.put("LastUpdatedDate", edu.getLastUpdatedDate());
            vm.put("IsActive", edu.getIsActive());
            vm.put("IsUpdated", edu.getIsUpdated());
            vm.put("IsDeleted", edu.getIsDeleted());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getAllGovtDoc() {
        List<EmployeeGovtDoc> govtDocs = employeeGovtDocRepository.findByIsActiveAndIsDeleted(true, false);
        if (govtDocs.isEmpty()) return new ArrayList<>();

        List<Integer> docIds = govtDocs.stream().map(EmployeeGovtDoc::getDocId).filter(Objects::nonNull).distinct().collect(Collectors.toList());
        Map<Integer, String> docNames = docIds.isEmpty() ? new HashMap<>() :
            documentMasterRepository.findAllById(docIds).stream()
                .collect(Collectors.toMap(d -> d.getDocId(), d -> d.getDocName() != null ? d.getDocName() : ""));

        return govtDocs.stream().map(govt -> {
            Map<String, Object> vm = new HashMap<>();
            vm.put("GovId", govt.getGovId());
            vm.put("EmpId", govt.getEmpId());
            vm.put("DocId", govt.getDocId());
            vm.put("DocName", docNames.getOrDefault(govt.getDocId(), ""));
            vm.put("Others", govt.getOthers());
            vm.put("DocNo", govt.getDocNo());
            vm.put("IssuedDate", convertToGovtDate(govt.getIssuedDate()));
            vm.put("ExpiredDate", convertToGovtDate(govt.getExpiredDate()));
            String path = govt.getPath();
            if (path != null && !path.isEmpty() && path.contains("Uploads")) {
                String[] parts = path.split("Uploads", 2);
                if (parts.length > 1) path = "Uploads" + parts[1];
            }
            vm.put("Path", path != null ? path : "");
            vm.put("CreatedBy", govt.getCreatedBy());
            vm.put("CreatedDate", govt.getCreatedDate());
            vm.put("LastUpdatedBy", govt.getLastUpdatedBy());
            vm.put("LastUpdatedDate", govt.getLastUpdatedDate());
            vm.put("IsActive", govt.getIsActive());
            vm.put("IsUpdated", govt.getIsUpdated());
            vm.put("IsDeleted", govt.getIsDeleted());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getEducationDoc(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == 0) throw new RuntimeException("EmpId is Missing");
        if (empId == 0) return new ArrayList<>();

        List<EmployeeEducation> educations = employeeEducationRepository.findByEmpIdAndIsActiveAndIsDeleted(empId, true, false);
        if (educations.isEmpty()) return new ArrayList<>();

        List<Integer> docIds = educations.stream().map(EmployeeEducation::getDocId).filter(Objects::nonNull).distinct().collect(Collectors.toList());
        Map<Integer, String> docNames = docIds.isEmpty() ? new HashMap<>() :
            documentMasterRepository.findAllById(docIds).stream()
                .collect(Collectors.toMap(d -> d.getDocId(), d -> d.getDocName() != null ? d.getDocName() : ""));

        List<Map<String, Object>> result = new ArrayList<>();
        for (EmployeeEducation edu : educations) {
            Map<String, Object> vm = new HashMap<>();
            vm.put("Id", edu.getId());
            vm.put("EmpId", edu.getEmpId());
            vm.put("DocId", edu.getDocId());
            vm.put("Others", edu.getOthers());
            vm.put("School", edu.getSchool());
            vm.put("DegreeId", edu.getDegreeId());
            vm.put("Filed", edu.getFiled());
            vm.put("StartDate", convertToJsonDate(edu.getStartDate()));
            vm.put("EndDate", convertToJsonDate(edu.getEndDate()));
            vm.put("Grade", edu.getGrade());
            vm.put("Description", edu.getDescription());

            String path = edu.getPath();
            if (path != null && !path.isEmpty() && path.contains("Uploads")) {
                String[] parts = path.split("Uploads", 2);
                if (parts.length > 1) path = "Uploads" + parts[1];
            }
            vm.put("Path", path != null ? path : "");

            vm.put("CreatedBy", edu.getCreatedBy());
            vm.put("CreatedDate", edu.getCreatedDate());
            vm.put("LastUpdatedBy", edu.getLastUpdatedBy());
            vm.put("LastUpdatedDate", edu.getLastUpdatedDate());
            vm.put("IsActive", edu.getIsActive());
            vm.put("IsUpdated", edu.getIsUpdated());
            vm.put("IsDeleted", edu.getIsDeleted());
            result.add(vm);
        }
        return result;
    }

    public Map<String, Object> addEducationDoc(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer empId = parseInteger(model.get("EmpId"));
        Integer docId = parseInteger(model.get("DocId"));

        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<EmployeeEducation> existing = employeeEducationRepository.findByEmpIdAndDocIdAndIsActiveAndIsDeleted(empId, docId, true, false);
        if (!existing.isEmpty()) throw new RuntimeException("Education document already exists");

        EmployeeEducation edu = new EmployeeEducation();
        edu.setEmpId(empId);
        edu.setDocId(docId);
        edu.setOthers("");
        edu.setSchool(parseString(model.get("School")));
        edu.setDegreeId(parseString(model.get("DocName")));
        if ("OTHERS".equalsIgnoreCase(parseString(model.get("DocName")))) {
            edu.setOthers(parseString(model.get("Others")));
        }
        edu.setFiled(parseString(model.get("Filed")));
        edu.setStartDate(parseDateFromObject(model.get("StartDate")));
        edu.setEndDate(parseDateFromObject(model.get("EndDate")));
        edu.setGrade(parseString(model.get("Grade")));
        edu.setDescription(parseString(model.get("Description")));
        edu.setPath(parseString(model.get("Path")));
        edu.setCreatedBy(loginId);
        edu.setCreatedDate(new Date());
        edu.setLastUpdatedBy(loginId);
        edu.setLastUpdatedDate(new Date());
        edu.setIsActive(true);
        edu.setIsUpdated(false);
        edu.setIsDeleted(false);
        employeeEducationRepository.save(edu);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateEducationDoc(Map<String, Object> model) {
        Integer id = parseInteger(model.get("Id"));
        if (id == 0) throw new RuntimeException("Id is Missing");

        Optional<EmployeeEducation> opt = employeeEducationRepository.findById(id);
        if (opt.isEmpty()) throw new RuntimeException("Education document not found");

        EmployeeEducation edu = opt.get();
        if (model.containsKey("School")) edu.setSchool(parseString(model.get("School")));
        if (model.containsKey("Filed")) edu.setFiled(parseString(model.get("Filed")));
        if (model.containsKey("StartDate")) edu.setStartDate(parseDateFromObject(model.get("StartDate")));
        if (model.containsKey("EndDate")) edu.setEndDate(parseDateFromObject(model.get("EndDate")));
        if (model.containsKey("Grade")) edu.setGrade(parseString(model.get("Grade")));
        if (model.containsKey("Description")) edu.setDescription(parseString(model.get("Description")));
        if (model.containsKey("Path")) edu.setPath(parseString(model.get("Path")));
        if (model.containsKey("Others")) edu.setOthers(parseString(model.get("Others")));
        edu.setIsUpdated(true);
        edu.setLastUpdatedDate(new Date());
        employeeEducationRepository.save(edu);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteEducationDoc(Map<String, Object> model) {
        Integer id = parseInteger(model.get("Id"));
        if (id == 0) throw new RuntimeException("Id is Missing");

        Optional<EmployeeEducation> opt = employeeEducationRepository.findById(id);
        if (opt.isEmpty()) throw new RuntimeException("Education document not found");

        EmployeeEducation edu = opt.get();
        edu.setIsDeleted(true);
        edu.setIsUpdated(true);
        edu.setLastUpdatedDate(new Date());
        employeeEducationRepository.save(edu);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Deleted");
        return result;
    }

    public List<Map<String, Object>> getGovtDoc(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == 0) throw new RuntimeException("EmpId is Missing");
        if (empId == 0) return new ArrayList<>();

        List<EmployeeGovtDoc> govtDocs = employeeGovtDocRepository.findByEmpIdAndIsActiveAndIsDeleted(empId, true, false);
        if (govtDocs.isEmpty()) return new ArrayList<>();

        return govtDocs.stream().map(gd -> {
            Map<String, Object> vm = new HashMap<>();
            vm.put("GovId", gd.getGovId());
            vm.put("EmpId", gd.getEmpId());
            vm.put("DocId", gd.getDocId());
            vm.put("DocName", gd.getDocName());
            vm.put("Others", gd.getOthers());
            vm.put("Name", gd.getName());
            vm.put("DocNo", gd.getDocNo());
            vm.put("IssuedDate", convertToGovtDate(gd.getIssuedDate()));
            vm.put("ExpiredDate", convertToGovtDate(gd.getExpiredDate()));
            vm.put("Description", gd.getDescription());

            String path = gd.getPath();
            if (path != null && !path.isEmpty() && path.contains("Uploads")) {
                String[] parts = path.split("Uploads", 2);
                if (parts.length > 1) path = "Uploads" + parts[1];
            }
            vm.put("Path", path != null ? path : "");

            vm.put("CreatedBy", gd.getCreatedBy());
            vm.put("CreatedDate", gd.getCreatedDate());
            vm.put("LastUpdatedBy", gd.getLastUpdatedBy());
            vm.put("LastUpdatedDate", gd.getLastUpdatedDate());
            vm.put("IsActive", gd.getIsActive());
            vm.put("IsUpdated", gd.getIsUpdated());
            vm.put("IsDeleted", gd.getIsDeleted());
            return vm;
        }).collect(Collectors.toList());
    }

    public Map<String, Object> addGovtDoc(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer empId = parseInteger(model.get("EmpId"));
        Integer docId = parseInteger(model.get("DocId"));

        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<EmployeeGovtDoc> existing = employeeGovtDocRepository.findByEmpIdAndDocIdAndIsActiveAndIsDeleted(empId, docId, true, false);
        if (!existing.isEmpty()) throw new RuntimeException("Government document already exists");

        EmployeeGovtDoc gd = new EmployeeGovtDoc();
        gd.setEmpId(empId);
        gd.setDocId(docId);
        gd.setDocName(parseString(model.get("DocName")));
        gd.setName(parseString(model.get("Name")));
        gd.setOthers("");
        if ("OTHERS".equalsIgnoreCase(parseString(model.get("DocName")))) {
            gd.setOthers(parseString(model.get("Others")));
        }
        gd.setDocNo(parseString(model.get("DocNo")));
        gd.setIssuedDate(parseString(model.get("IssuedDate")));
        gd.setExpiredDate(parseString(model.get("ExpiredDate")));
        gd.setDescription(parseString(model.get("Description")));
        gd.setPath(parseString(model.get("Path")));
        gd.setCreatedBy(loginId);
        gd.setCreatedDate(new Date());
        gd.setLastUpdatedBy(loginId);
        gd.setLastUpdatedDate(new Date());
        gd.setIsActive(true);
        gd.setIsUpdated(false);
        gd.setIsDeleted(false);
        employeeGovtDocRepository.save(gd);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateGovtDoc(Map<String, Object> model) {
        Integer govId = parseInteger(model.get("GovId"));
        if (govId == 0) throw new RuntimeException("GovId is Missing");

        Optional<EmployeeGovtDoc> opt = employeeGovtDocRepository.findById(govId);
        if (opt.isEmpty()) throw new RuntimeException("Government document not found");

        EmployeeGovtDoc gd = opt.get();
        if (model.containsKey("DocName")) gd.setDocName(parseString(model.get("DocName")));
        if (model.containsKey("DocNo")) gd.setDocNo(parseString(model.get("DocNo")));
        if (model.containsKey("IssuedDate")) gd.setIssuedDate(parseString(model.get("IssuedDate")));
        if (model.containsKey("ExpiredDate")) gd.setExpiredDate(parseString(model.get("ExpiredDate")));
        if (model.containsKey("Description")) gd.setDescription(parseString(model.get("Description")));
        if (model.containsKey("Path")) gd.setPath(parseString(model.get("Path")));
        if (model.containsKey("Others")) gd.setOthers(parseString(model.get("Others")));
        gd.setIsUpdated(true);
        gd.setLastUpdatedDate(new Date());
        employeeGovtDocRepository.save(gd);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteGovtDoc(Map<String, Object> model) {
        Integer govId = parseInteger(model.get("GovId"));
        if (govId == 0) throw new RuntimeException("GovId is Missing");

        Optional<EmployeeGovtDoc> opt = employeeGovtDocRepository.findById(govId);
        if (opt.isEmpty()) throw new RuntimeException("Government document not found");

        EmployeeGovtDoc gd = opt.get();
        gd.setIsDeleted(true);
        gd.setIsUpdated(true);
        gd.setLastUpdatedDate(new Date());
        employeeGovtDocRepository.save(gd);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Deleted");
        return result;
    }

    private Integer parseInteger(Object val) {
        if (val == null) return 0;
        if (val instanceof Number) return ((Number) val).intValue();
        try { return Integer.parseInt(val.toString().trim()); } catch (Exception e) { return 0; }
    }

    private String parseString(Object val) {
        return val != null ? val.toString().trim() : "";
    }

    private Date parseDateFromObject(Object dateObj) {
        return parseStringDate(dateObj);
    }

    private Date parseStringDate(Object dateObj) {
        if (dateObj == null) return null;
        if (dateObj instanceof Date) return (Date) dateObj;
        String dateStr = dateObj.toString().trim();
        if (dateStr.isEmpty()) return null;
        try {
            if (dateStr.startsWith("/Date(") && dateStr.endsWith(")/")) {
                long millis = Long.parseLong(dateStr.substring(6, dateStr.length() - 2));
                return new Date(millis);
            }
            if (dateStr.contains("-") && dateStr.split("-")[0].length() == 4) {
                return new SimpleDateFormat("yyyy-MM-dd").parse(dateStr);
            } else if (dateStr.contains("-")) {
                return new SimpleDateFormat("dd-MM-yyyy").parse(dateStr);
            } else {
                return new SimpleDateFormat("yyyy-MM-dd").parse(dateStr);
            }
        } catch (Exception e) {
            return null;
        }
    }

    private String convertToJsonDate(String dateStr) {
        if (dateStr == null || dateStr.isEmpty()) return null;
        try {
            Date d = parseStringDate(dateStr);
            if (d != null) return "/Date(" + d.getTime() + ")/";
        } catch (Exception e) {}
        return dateStr;
    }

    private String convertToJsonDateObj(Date dateObj) {
        if (dateObj == null) return null;
        return "/Date(" + dateObj.getTime() + ")/";
    }

    private String convertToGovtDate(String dateStr) {
        if (dateStr == null || dateStr.isEmpty()) return null;
        try {
            Date d = parseStringDate(dateStr);
            if (d != null) {
                SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
                return sdf.format(d);
            }
        } catch (Exception e) {}
        return dateStr;
    }

    public List<Map<String, Object>> getEmpCareerDetails(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");
        if (empId == 0) return new ArrayList<>();

        List<EmployeeCareerDetail> careers = employeeCareerDetailRepository.findByEmpIdAndIsActiveAndIsDeletedOrderByCareerIdDesc(empId, true, false);
        if (careers.isEmpty()) return new ArrayList<>();

        return careers.stream().map(c -> {
            Map<String, Object> vm = new HashMap<>();
            vm.put("CareerId", c.getCareerId());
            vm.put("EmpId", c.getEmpId());
            vm.put("Company", c.getCompany());
            vm.put("Designation", c.getDesignation());
            vm.put("FromDate", convertToJsonDate(c.getFromDate()));
            vm.put("ToDate", convertToJsonDate(c.getToDate()));
            vm.put("Experience", c.getExperience());
            vm.put("PMonth1", c.getPMonth1());
            vm.put("PaySlip1", normalizeUploadsPath(c.getPaySlip1()));
            vm.put("PMonth2", c.getPMonth2());
            vm.put("PaySlip2", normalizeUploadsPath(c.getPaySlip2()));
            vm.put("PMonth3", c.getPMonth3());
            vm.put("PaySlip3", normalizeUploadsPath(c.getPaySlip3()));
            vm.put("OfferLetter", normalizeUploadsPath(c.getOfferLetter()));
            vm.put("SalaryLetter", normalizeUploadsPath(c.getSalaryLetter()));
            vm.put("ExperienceLetter", normalizeUploadsPath(c.getExperienceLetter()));
            vm.put("RelievingLetter", normalizeUploadsPath(c.getRelievingLetter()));
            vm.put("ContactName", c.getContactName());
            vm.put("ContactDesignation", c.getContactDesignation());
            vm.put("ContactEmail", c.getContactEmail());
            vm.put("ContactMobile", c.getContactMobile());
            vm.put("CTC", c.getCtc());
            vm.put("Reason", c.getReason());
            vm.put("CreatedBy", c.getCreatedBy());
            vm.put("CreatedDate", c.getCreatedDate());
            vm.put("LastUpdatedBy", c.getLastUpdatedBy());
            vm.put("LastUpdatedDate", c.getLastUpdatedDate());
            vm.put("IsActive", c.getIsActive());
            vm.put("IsUpdated", c.getIsUpdated());
            vm.put("IsDeleted", c.getIsDeleted());
            return vm;
        }).collect(Collectors.toList());
    }

    public Map<String, Object> addEmpCareerDetails(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer empId = parseInteger(model.get("EmpId"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        EmployeeCareerDetail career = new EmployeeCareerDetail();
        career.setEmpId(empId);
        career.setCompany(parseString(model.get("Company")));
        career.setDesignation(parseString(model.get("Designation")));
        career.setFromDate(parseDateFromObject(model.get("FromDate")));
        career.setToDate(parseDateFromObject(model.get("ToDate")));
        career.setExperience(parseString(model.get("Experience")));
        career.setPMonth1(parseString(model.get("PMonth1")));
        career.setPaySlip1(parseString(model.get("PaySlip1")));
        career.setPMonth2(parseString(model.get("PMonth2")));
        career.setPaySlip2(parseString(model.get("PaySlip2")));
        career.setPMonth3(parseString(model.get("PMonth3")));
        career.setPaySlip3(parseString(model.get("PaySlip3")));
        career.setOfferLetter(parseString(model.get("OfferLetter")));
        career.setSalaryLetter(parseString(model.get("SalaryLetter")));
        career.setExperienceLetter(parseString(model.get("ExperienceLetter")));
        career.setRelievingLetter(parseString(model.get("RelievingLetter")));
        career.setContactName(parseString(model.get("ContactName")));
        career.setContactDesignation(parseString(model.get("ContactDesignation")));
        career.setContactEmail(parseString(model.get("ContactEmail")));
        career.setContactMobile(parseString(model.get("ContactMobile")));
        career.setCtc(parseString(model.get("CTC")));
        career.setReason(parseString(model.get("Reason")));
        career.setCreatedBy(loginId);
        career.setCreatedDate(new Date());
        career.setLastUpdatedBy(loginId);
        career.setLastUpdatedDate(new Date());
        career.setIsActive(true);
        career.setIsUpdated(false);
        career.setIsDeleted(false);
        employeeCareerDetailRepository.save(career);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateEmpCareerDetails(Map<String, Object> model) {
        Integer careerId = parseInteger(model.get("CareerId"));
        if (careerId == 0) throw new RuntimeException("CareerId is Missing");

        Optional<EmployeeCareerDetail> opt = employeeCareerDetailRepository.findById(careerId);
        if (opt.isEmpty()) throw new RuntimeException("Career detail not found");

        EmployeeCareerDetail c = opt.get();
        if (model.containsKey("Company")) c.setCompany(parseString(model.get("Company")));
        if (model.containsKey("Designation")) c.setDesignation(parseString(model.get("Designation")));
        if (model.containsKey("FromDate")) c.setFromDate(parseDateFromObject(model.get("FromDate")));
        if (model.containsKey("ToDate")) c.setToDate(parseDateFromObject(model.get("ToDate")));
        if (model.containsKey("Experience")) c.setExperience(parseString(model.get("Experience")));
        if (model.containsKey("PMonth1")) c.setPMonth1(parseString(model.get("PMonth1")));
        if (model.containsKey("PaySlip1")) c.setPaySlip1(parseString(model.get("PaySlip1")));
        if (model.containsKey("PMonth2")) c.setPMonth2(parseString(model.get("PMonth2")));
        if (model.containsKey("PaySlip2")) c.setPaySlip2(parseString(model.get("PaySlip2")));
        if (model.containsKey("PMonth3")) c.setPMonth3(parseString(model.get("PMonth3")));
        if (model.containsKey("PaySlip3")) c.setPaySlip3(parseString(model.get("PaySlip3")));
        if (model.containsKey("OfferLetter")) c.setOfferLetter(parseString(model.get("OfferLetter")));
        if (model.containsKey("SalaryLetter")) c.setSalaryLetter(parseString(model.get("SalaryLetter")));
        if (model.containsKey("ExperienceLetter")) c.setExperienceLetter(parseString(model.get("ExperienceLetter")));
        if (model.containsKey("RelievingLetter")) c.setRelievingLetter(parseString(model.get("RelievingLetter")));
        if (model.containsKey("ContactName")) c.setContactName(parseString(model.get("ContactName")));
        if (model.containsKey("ContactDesignation")) c.setContactDesignation(parseString(model.get("ContactDesignation")));
        if (model.containsKey("ContactEmail")) c.setContactEmail(parseString(model.get("ContactEmail")));
        if (model.containsKey("ContactMobile")) c.setContactMobile(parseString(model.get("ContactMobile")));
        if (model.containsKey("CTC")) c.setCtc(parseString(model.get("CTC")));
        if (model.containsKey("Reason")) c.setReason(parseString(model.get("Reason")));
        c.setIsUpdated(true);
        c.setLastUpdatedDate(new Date());
        employeeCareerDetailRepository.save(c);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteEmpCareerDetails(Map<String, Object> model) {
        Integer careerId = parseInteger(model.get("CareerId"));
        if (careerId == 0) throw new RuntimeException("CareerId is Missing");

        Optional<EmployeeCareerDetail> opt = employeeCareerDetailRepository.findById(careerId);
        if (opt.isEmpty()) throw new RuntimeException("Career detail not found");

        EmployeeCareerDetail c = opt.get();
        c.setIsDeleted(true);
        c.setIsUpdated(true);
        c.setLastUpdatedDate(new Date());
        employeeCareerDetailRepository.save(c);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Deleted");
        return result;
    }

    private String normalizeUploadsPath(String path) {
        if (path == null || path.isEmpty()) return "";
        if (path.contains("Uploads")) {
            String[] parts = path.split("Uploads", 2);
            if (parts.length > 1) return "Uploads" + parts[1];
        }
        return path;
    }

    public EmployeeMasterViewModel relievedEmployee(EmployeeMasterViewModel model) {
        Integer loginId = model.getLoginId();
        Integer empId = model.getEmpId();
        if (loginId == null || loginId == 0) throw new RuntimeException("LoginId is Missing");
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        EmployeeMaster emp = empOpt.get();
        emp.setEmpStatus("Relieved");
        emp.setRelievedReason(model.getRelievedReason());
        
        Object relievedDateObj = model.getRelievedDate();
        if (relievedDateObj instanceof java.util.Date) {
            emp.setRelievedDate((java.util.Date) relievedDateObj);
        } else if (relievedDateObj instanceof String) {
            try {
                String dateStr = (String) relievedDateObj;
                if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                    emp.setRelievedDate(new java.text.SimpleDateFormat("dd-MM-yyyy").parse(dateStr));
                } else {
                    emp.setRelievedDate(new java.text.SimpleDateFormat("yyyy-MM-dd").parse(dateStr));
                }
            } catch (Exception e) {
                emp.setRelievedDate(null);
            }
        }
        
        Object relievedEffectiveDateObj = model.getRelievedEffectiveDate();
        if (relievedEffectiveDateObj instanceof java.util.Date) {
            emp.setRelievedEffectiveDate((java.util.Date) relievedEffectiveDateObj);
        } else if (relievedEffectiveDateObj instanceof String) {
            try {
                String dateStr = (String) relievedEffectiveDateObj;
                if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                    emp.setRelievedEffectiveDate(new java.text.SimpleDateFormat("dd-MM-yyyy").parse(dateStr));
                } else {
                    emp.setRelievedEffectiveDate(new java.text.SimpleDateFormat("yyyy-MM-dd").parse(dateStr));
                }
            } catch (Exception e) {
                emp.setRelievedEffectiveDate(null);
            }
        }
        
        emp.setIsRelieved(model.getIsRelieved());
        emp.setIsActive(true);
        emp.setIsUpdated(true);
        emp.setIsDeleted(false);
        emp.setLastUpdatedBy(loginId);
        emp.setLastUpdatedDate(new java.util.Date());
        employeeMasterRepository.save(emp);
        model.setMsg("Relieved");
        return model;
    }

    public List<Map<String, Object>> getAttendanceSource() {
        List<Map<String, Object>> result = new ArrayList<>();
        Map<String, Object> m1 = new HashMap<>(); m1.put("id", 1); m1.put("name", "ESSL");
        Map<String, Object> m2 = new HashMap<>(); m2.put("id", 2); m2.put("name", "Manual");
        result.add(m1); result.add(m2);
        return result;
    }

    public List<Map<String, Object>> getConsolidatedAttendanceData() {
        List<Map<String, Object>> result = new ArrayList<>();

        // 1. AttendanceSource
        Map<String, Object> attendanceSource = new HashMap<>();
        long totalEmployeeCount = employeeMasterRepository.findAll().stream()
            .filter(e -> e.getIsDeleted() == null || !e.getIsDeleted())
            .count();
        attendanceSource.put("TotalEmployeeCount", totalEmployeeCount);
        attendanceSource.put("DeviceCheckInCount", 0L);
        attendanceSource.put("OnSiteCount", 0L);
        attendanceSource.put("WFHCount", 0L);

        Map<String, Object> attendanceSourceWrapper = new HashMap<>();
        attendanceSourceWrapper.put("AttendanceSource", attendanceSource);
        result.add(attendanceSourceWrapper);

        // 2. YesterdayAttendanceDetails
        Map<String, Object> yesterdayAttendance = new HashMap<>();
        yesterdayAttendance.put("PresentYesterday", 0L);
        yesterdayAttendance.put("AbsentYesterday", totalEmployeeCount);
        yesterdayAttendance.put("OnLeaveYesterday", 0L);
        yesterdayAttendance.put("WFHYesterday", 0L);
        yesterdayAttendance.put("ONSITEYesterday", 0L);

        Map<String, Object> yesterdayWrapper = new HashMap<>();
        yesterdayWrapper.put("YesterdayAttendanceDetails", yesterdayAttendance);
        result.add(yesterdayWrapper);

        // 3. CurrentMonthWorkedHours
        Map<String, Object> workedHours = new HashMap<>();
        workedHours.put("TotalWH", "00:00:00");
        workedHours.put("MaxWH", "00:00:00");

        Map<String, Object> workedHoursWrapper = new HashMap<>();
        workedHoursWrapper.put("CurrentMonthWorkedHours", workedHours);
        result.add(workedHoursWrapper);

        // 4. OnTimeCheckIn - Last 7 days
        List<Map<String, Object>> onTimeCheckInList = new ArrayList<>();
        java.time.LocalDate today = java.time.LocalDate.now();
        java.time.format.DateTimeFormatter formatter = java.time.format.DateTimeFormatter.ofPattern("yyyy-MM-dd");
        for (int i = 7; i >= 0; i--) {
            java.time.LocalDate date = today.minusDays(i);
            Map<String, Object> onTimeCheckIn = new HashMap<>();
            onTimeCheckIn.put("Date", date.format(formatter));
            onTimeCheckIn.put("OnTimeCheckInCount", 0L);
            onTimeCheckIn.put("LateCheckInCount", 0L);
            onTimeCheckInList.add(onTimeCheckIn);
        }

        Map<String, Object> onTimeCheckInWrapper = new HashMap<>();
        onTimeCheckInWrapper.put("OnTimeCheckIn", onTimeCheckInList);
        result.add(onTimeCheckInWrapper);

        // 5. GetvisitorToday
        Map<String, Object> visitorWrapper = new HashMap<>();
        visitorWrapper.put("GetvisitorToday", new ArrayList<>());
        result.add(visitorWrapper);

        // 6. CurrentmonthemployeeList
        Map<String, Object> employeeListWrapper = new HashMap<>();
        employeeListWrapper.put("CurrentmonthemployeeList", new ArrayList<>());
        result.add(employeeListWrapper);

        // 7. PendingLeaves
        Map<String, Object> pendingLeavesWrapper = new HashMap<>();
        pendingLeavesWrapper.put("PendingLeaves", new ArrayList<>());
        result.add(pendingLeavesWrapper);

        // 8. AllLeaves
        Map<String, Object> allLeavesWrapper = new HashMap<>();
        allLeavesWrapper.put("AllLeaves", new ArrayList<>());
        result.add(allLeavesWrapper);

        // 9. CompOffList
        Map<String, Object> compOffWrapper = new HashMap<>();
        compOffWrapper.put("CompOffList", new ArrayList<>());
        result.add(compOffWrapper);

        // 10. ShiftManagement
        List<Map<String, Object>> shiftManagement = new ArrayList<>();
        Map<String, Object> shift1 = new HashMap<>();
        shift1.put("ShiftId", 7);
        shift1.put("Shift", "General Shift");
        shift1.put("ShiftClkHrs", "00:00");
        shift1.put("Shiftdays", "5");
        shift1.put("ShiftTime", "09:30 - 18:30");
        shift1.put("ShiftEmpCount", 0L);
        shift1.put("ShiftStartTime", "09:30");
        shift1.put("ShiftEndTime", "18:30");
        shiftManagement.add(shift1);

        Map<String, Object> shift2 = new HashMap<>();
        shift2.put("ShiftId", 12);
        shift2.put("Shift", "SPCL SHIFT");
        shift2.put("ShiftClkHrs", "00:00");
        shift2.put("Shiftdays", "6");
        shift2.put("ShiftTime", "09:00 - 05:30");
        shift2.put("ShiftEmpCount", 0L);
        shift2.put("ShiftStartTime", "09:00");
        shift2.put("ShiftEndTime", "05:30");
        shiftManagement.add(shift2);

        Map<String, Object> shiftManagementWrapper = new HashMap<>();
        shiftManagementWrapper.put("ShiftManagement", shiftManagement);
        result.add(shiftManagementWrapper);

        return result;
    }
    
    private void addToResult(List<Map<String, Object>> result, String key, Object value) {
        Map<String, Object> item = new HashMap<>();
        item.put(key, value);
        result.add(item);
    }

    public Map<String, Object> dashboardDetails() {
        Map<String, Object> result = new HashMap<>();
        result.put("totalEmployees", employeeMasterRepository.count());
        result.put("activeEmployees", employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).size());
        return result;
    }

    public Map<String, Object> createShift(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Shift created successfully");
        return result;
    }

    public Map<String, Object> createCompanySetting(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Company setting created successfully");
        return result;
    }

    public Map<String, Object> checkHalfDayLoss(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("lossHours", 0);
        return result;
    }

    public Map<String, Object> getWorkHours() {
        Map<String, Object> result = new HashMap<>();
        result.put("workHours", 9);
        return result;
    }

    public List<Map<String, Object>> getAllPages(Map<String, Object> model) {
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        Integer deptId = model.get("DeptId") != null ? Integer.parseInt(model.get("DeptId").toString()) : 0;
        Integer roleId = model.get("RoleId") != null ? Integer.parseInt(model.get("RoleId").toString()) : 0;

        List<PageModuleMaster> pageModules = pageModuleMasterRepository.findByIsDeleted(false);
        List<ModuleMaster> modules = moduleMasterRepository.findByIsDeleted(false);
        List<SubModuleMaster> subModules = subModuleMasterRepository.findByIsDeleted(false);

        // If both DeptId and RoleId provided, get existing access policies for this combination
        List<AccessPolicy> existingPolicies = new ArrayList<>();
        if (deptId != 0 && roleId != 0) {
            existingPolicies = accessPolicyRepository.findByDeptIdAndRoleIdAndIsDeleted(deptId, roleId, false);
        }

        List<Map<String, Object>> result = new ArrayList<>();
        for (PageModuleMaster pm : pageModules) {
            ModuleMaster module = modules.stream()
                .filter(mod -> mod.getModuleId().equals(pm.getModuleId()))
                .findFirst().orElse(null);
            SubModuleMaster subModule = subModules.stream()
                .filter(sm -> sm.getSubModuleId().equals(pm.getSubModuleId()))
                .findFirst().orElse(null);

            // Find matching access policy for this page module
            AccessPolicy policy = existingPolicies.stream()
                .filter(p -> pm.getPageModuleId().equals(p.getPageModuleId()))
                .findFirst().orElse(null);

            Map<String, Object> m = new HashMap<>();
            m.put("DeptId", deptId != 0 ? deptId : null);
            m.put("DeptName", null);
            m.put("PageAccess", policy != null || (deptId == 0 || roleId == 0));
            m.put("RoleId", roleId != 0 ? roleId : null);
            m.put("PageModuleId", pm.getPageModuleId());
            m.put("ModuleId", pm.getModuleId());
            m.put("ModuleName", module != null ? module.getModuleName() : "");
            m.put("SubModuleId", pm.getSubModuleId());
            m.put("SubModuleName", subModule != null ? subModule.getSubModuleName() : "");
            m.put("PageName", pm.getPageName());
            m.put("PageModuleName", null);
            m.put("AddAccess", policy != null && Boolean.TRUE.equals(policy.getAddAccess()));
            m.put("UpdateAccess", policy != null && Boolean.TRUE.equals(policy.getUpdateAccess()));
            m.put("DeleteAccess", policy != null && Boolean.TRUE.equals(policy.getDeleteAccess()));
            m.put("ViewAccess", policy != null && Boolean.TRUE.equals(policy.getViewAccess()));
            m.put("msg", null);
            m.put("CreatedBy", pm.getCreatedBy());
            m.put("CreatedDate", pm.getCreatedDate() != null ? "\\/Date(" + pm.getCreatedDate().getTime() + ")\\/" : null);
            m.put("LastUpdatedBy", pm.getLastUpdatedBy());
            m.put("LastUpdatedDate", pm.getLastUpdatedDate() != null ? "\\/Date(" + pm.getLastUpdatedDate().getTime() + ")\\/" : null);
            m.put("IsActive", pm.getIsActive());
            m.put("IsUpdated", null);
            m.put("IsDeleted", null);
            m.put("EmpId", 0);

            result.add(m);
        }
        return result;
    }

    public Map<String, Object> submitAccessControls(List<Map<String, Object>> models) {
        Map<String, Object> result = new HashMap<>();

        if (models == null || models.isEmpty()) {
            result.put("StatusCode", 400);
            result.put("Message", "Access list cannot be empty.");
            return result;
        }

        // Get first record to determine DeptId and RoleId
        Map<String, Object> firstRecord = models.get(0);
        Integer deptId = null;
        Integer roleId = null;
        Integer empId = null;

        if (firstRecord.containsKey("DeptId") && firstRecord.get("DeptId") != null && !firstRecord.get("DeptId").toString().trim().isEmpty()) {
            try { deptId = Integer.valueOf(firstRecord.get("DeptId").toString().trim()); } catch (Exception e) {}
        }

        if (firstRecord.containsKey("RoleId") && firstRecord.get("RoleId") != null && !firstRecord.get("RoleId").toString().trim().isEmpty()) {
            try { roleId = Integer.valueOf(firstRecord.get("RoleId").toString().trim()); } catch (Exception e) {}
        }

        if (firstRecord.containsKey("EmpId") && firstRecord.get("EmpId") != null && !firstRecord.get("EmpId").toString().trim().isEmpty()) {
            try { empId = Integer.valueOf(firstRecord.get("EmpId").toString().trim()); } catch (Exception e) {}
        }

        // Delete existing access controls for same DeptId and RoleId
        if (deptId != null && roleId != null) {
            List<AccessPolicy> existingPolicies = accessPolicyRepository.findByDeptIdAndRoleIdAndIsDeleted(deptId, roleId, false);
            for (AccessPolicy policy : existingPolicies) {
                policy.setIsDeleted(true);
                policy.setLastUpdatedBy(empId != null ? empId : 1);
                policy.setLastUpdatedDate(new Date());
                accessPolicyRepository.save(policy);
            }
        }

        // Insert new access controls
        for (Map<String, Object> model : models) {
            AccessPolicy policy = new AccessPolicy();

            if (model.containsKey("DeptId") && model.get("DeptId") != null && !model.get("DeptId").toString().trim().isEmpty()) {
                try { policy.setDeptId(Integer.valueOf(model.get("DeptId").toString().trim())); } catch (Exception e) {}
            }

            if (model.containsKey("RoleId") && model.get("RoleId") != null && !model.get("RoleId").toString().trim().isEmpty()) {
                try { policy.setRoleId(Integer.valueOf(model.get("RoleId").toString().trim())); } catch (Exception e) {}
            }

            if (model.containsKey("ModuleId") && model.get("ModuleId") != null && !model.get("ModuleId").toString().trim().isEmpty()) {
                try { policy.setModuleId(Integer.valueOf(model.get("ModuleId").toString().trim())); } catch (Exception e) {}
            }

            if (model.containsKey("SubModuleId") && model.get("SubModuleId") != null && !model.get("SubModuleId").toString().trim().isEmpty()) {
                try { policy.setSubModuleId(Integer.valueOf(model.get("SubModuleId").toString().trim())); } catch (Exception e) {}
            }

            if (model.containsKey("PageModuleId") && model.get("PageModuleId") != null && !model.get("PageModuleId").toString().trim().isEmpty()) {
                try { policy.setPageModuleId(Integer.valueOf(model.get("PageModuleId").toString().trim())); } catch (Exception e) {}
            }

            if (model.containsKey("ViewAccess") && model.get("ViewAccess") != null && !model.get("ViewAccess").toString().trim().isEmpty()) {
                try { policy.setViewAccess(Boolean.valueOf(model.get("ViewAccess").toString().trim().toLowerCase())); } catch (Exception e) {}
            }

            if (model.containsKey("AddAccess") && model.get("AddAccess") != null && !model.get("AddAccess").toString().trim().isEmpty()) {
                try { policy.setAddAccess(Boolean.valueOf(model.get("AddAccess").toString().trim().toLowerCase())); } catch (Exception e) {}
            }

            if (model.containsKey("UpdateAccess") && model.get("UpdateAccess") != null && !model.get("UpdateAccess").toString().trim().isEmpty()) {
                try { policy.setUpdateAccess(Boolean.valueOf(model.get("UpdateAccess").toString().trim().toLowerCase())); } catch (Exception e) {}
            }

            if (model.containsKey("DeleteAccess") && model.get("DeleteAccess") != null && !model.get("DeleteAccess").toString().trim().isEmpty()) {
                try { policy.setDeleteAccess(Boolean.valueOf(model.get("DeleteAccess").toString().trim().toLowerCase())); } catch (Exception e) {}
            }

            if (model.containsKey("CreatedBy") && model.get("CreatedBy") != null && !model.get("CreatedBy").toString().trim().isEmpty()) {
                try { policy.setCreatedBy(Integer.valueOf(model.get("CreatedBy").toString().trim())); } catch (Exception e) {}
            } else {
                policy.setCreatedBy(empId != null ? empId : 1);
            }

            policy.setCreatedDate(new Date());

            if (model.containsKey("LastUpdatedBy") && model.get("LastUpdatedBy") != null && !model.get("LastUpdatedBy").toString().trim().isEmpty()) {
                try { policy.setLastUpdatedBy(Integer.valueOf(model.get("LastUpdatedBy").toString().trim())); } catch (Exception e) {}
            } else {
                policy.setLastUpdatedBy(empId != null ? empId : 1);
            }

            policy.setLastUpdatedDate(new Date());
            policy.setIsActive(true);
            policy.setIsDeleted(false);

            accessPolicyRepository.save(policy);
        }

        result.put("msg", "Access controls submitted successfully");
        return result;
    }

    public List<Map<String, Object>> getAllEmployeeContactInformation() {
        List<Map<String, Object>> result = new ArrayList<>();
        return result;
    }

    public Map<String, Object> getEmployeeContactInformation(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == 0) throw new RuntimeException("EmpId is Missing");
        if (empId == 0) return new HashMap<>();

        Optional<EmployeeDetail> opt = employeeDetailRepository.findByEmpIdAndIsActiveAndIsDeleted(empId, true, false);
        if (opt.isEmpty()) return new HashMap<>();

        EmployeeDetail ed = opt.get();
        Map<String, Object> result = new HashMap<>();
        result.put("Id", ed.getId());
        result.put("EmpId", ed.getEmpId());
        result.put("AMobileNo", ed.getAMobileNo());
        result.put("PMailId", ed.getPMailId());
        result.put("FatherName", ed.getFatherName());
        result.put("MotherName", ed.getMotherName());
        result.put("HusbandName", ed.getHusbandName());
        result.put("FContactNo", ed.getFContactNo());
        result.put("MContactNo", ed.getMContactNo());
        result.put("HContactNo", ed.getHContactNo());
        result.put("EContactNo", ed.getEContactNo());
        result.put("EContactName", ed.getEContactName());
        result.put("EContactRelationship", ed.getEContactRelationship());
        result.put("EContactNo1", ed.getEContactNo1());
        result.put("EContactName1", ed.getEContactName1());
        result.put("EContactRelationship1", ed.getEContactRelationship1());
        result.put("EContactNo2", ed.getEContactNo2());
        result.put("EContactName2", ed.getEContactName2());
        result.put("EContactRelationship2", ed.getEContactRelationship2());
        result.put("Height", ed.getHeight());
        result.put("Weight", ed.getWeight());
        result.put("DateOfAnniversary", convertToJsonDateObj(ed.getDateOfAnniversary()));
        result.put("Disability", ed.getDisability());
        result.put("TotalExperience", ed.getTotalExperience());
        result.put("RelevantExperience", ed.getRelevantExperience());
        result.put("ECActivities", ed.getEcActivities());
        result.put("Sports", ed.getSports());
        result.put("CurrentDoorNumber", ed.getCurrentDoorNumber());
        result.put("CurrentBuildingName", ed.getCurrentBuildingName());
        result.put("CurrentStreet", ed.getCurrentStreet());
        result.put("CurrentLocation", ed.getCurrentLocation());
        result.put("CurrentCity", ed.getCurrentCity());
        result.put("CurrentState", ed.getCurrentState());
        result.put("CurrentCountry", ed.getCurrentCountry());
        result.put("CurrentPinCode", ed.getCurrentPinCode());
        result.put("PermanentDoorNumber", ed.getPermanentDoorNumber());
        result.put("PermanentBuildingName", ed.getPermanentBuildingName());
        result.put("PermanentStreet", ed.getPermanentStreet());
        result.put("PermanentLocation", ed.getPermanentLocation());
        result.put("PermanentCity", ed.getPermanentCity());
        result.put("PermanentState", ed.getPermanentState());
        result.put("PermanentCountry", ed.getPermanentCountry());
        result.put("PermanentPinCode", ed.getPermanentPinCode());
        result.put("Caste", ed.getCaste());
        result.put("Region", ed.getRegion());
        result.put("Country", ed.getCountry());
        result.put("Nationality", ed.getNationality());
        result.put("CreatedBy", ed.getCreatedBy());
        result.put("CreatedDate", ed.getCreatedDate());
        result.put("LastUpdatedBy", ed.getLastUpdatedBy());
        result.put("LastUpdatedDate", ed.getLastUpdatedDate());
        result.put("IsActive", ed.getIsActive());
        result.put("IsUpdated", ed.getIsUpdated());
        result.put("IsDeleted", ed.getIsDeleted());
        return result;
    }

    public Map<String, Object> addEmployeeContactInformation(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer empId = parseInteger(model.get("EmpId"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        EmployeeDetail ed = new EmployeeDetail();
        ed.setEmpId(empId);
        ed.setAMobileNo(parseString(model.get("AMobileNo")));
        ed.setPMailId(parseString(model.get("PMailId")));
        ed.setFatherName(parseString(model.get("FatherName")));
        ed.setMotherName(parseString(model.get("MotherName")));
        ed.setHusbandName(parseString(model.get("HusbandName")));
        ed.setFContactNo(parseString(model.get("FContactNo")));
        ed.setMContactNo(parseString(model.get("MContactNo")));
        ed.setHContactNo(parseString(model.get("HContactNo")));
        ed.setEContactNo(parseString(model.get("EContactNo")));
        ed.setEContactName(parseString(model.get("EContactName")));
        ed.setEContactRelationship(parseString(model.get("EContactRelationship")));
        ed.setEContactNo1(parseString(model.get("EContactNo1")));
        ed.setEContactName1(parseString(model.get("EContactName1")));
        ed.setEContactRelationship1(parseString(model.get("EContactRelationship1")));
        ed.setEContactNo2(parseString(model.get("EContactNo2")));
        ed.setEContactName2(parseString(model.get("EContactName2")));
        ed.setEContactRelationship2(parseString(model.get("EContactRelationship2")));
        ed.setHeight(parseString(model.get("Height")));
        ed.setWeight(parseString(model.get("Weight")));
        ed.setDateOfAnniversary(parseStringDate(model.get("DateOfAnniversary")));
        ed.setDisability(parseString(model.get("Disability")));
        ed.setTotalExperience(parseString(model.get("TotalExperience")));
        ed.setRelevantExperience(parseString(model.get("RelevantExperience")));
        ed.setEcActivities(parseString(model.get("ECActivities")));
        ed.setSports(parseString(model.get("Sports")));
        ed.setCurrentDoorNumber(parseString(model.get("CurrentDoorNumber")));
        ed.setCurrentBuildingName(parseString(model.get("CurrentBuildingName")));
        ed.setCurrentStreet(parseString(model.get("CurrentStreet")));
        ed.setCurrentLocation(parseString(model.get("CurrentLocation")));
        ed.setCurrentCity(parseString(model.get("CurrentCity")));
        ed.setCurrentState(parseString(model.get("CurrentState")));
        ed.setCurrentCountry(parseString(model.get("CurrentCountry")));
        ed.setCurrentPinCode(parseString(model.get("CurrentPinCode")));
        ed.setPermanentDoorNumber(parseString(model.get("PermanentDoorNumber")));
        ed.setPermanentBuildingName(parseString(model.get("PermanentBuildingName")));
        ed.setPermanentStreet(parseString(model.get("PermanentStreet")));
        ed.setPermanentLocation(parseString(model.get("PermanentLocation")));
        ed.setPermanentCity(parseString(model.get("PermanentCity")));
        ed.setPermanentState(parseString(model.get("PermanentState")));
        ed.setPermanentCountry(parseString(model.get("PermanentCountry")));
        ed.setPermanentPinCode(parseString(model.get("PermanentPinCode")));
        ed.setCaste(parseString(model.get("Caste")));
        ed.setRegion(parseString(model.get("Region")));
        ed.setCountry(parseString(model.get("Country")));
        ed.setNationality(parseString(model.get("Nationality")));
        ed.setCreatedBy(loginId);
        ed.setCreatedDate(new Date());
        ed.setLastUpdatedBy(loginId);
        ed.setLastUpdatedDate(new Date());
        ed.setIsActive(true);
        ed.setIsUpdated(false);
        ed.setIsDeleted(false);
        employeeDetailRepository.save(ed);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateEmployeeContactInformation(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer id = parseInteger(model.get("Id"));
        Integer empId = parseInteger(model.get("EmpId"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        if (id == 0) {
            EmployeeDetail ed = new EmployeeDetail();
            ed.setEmpId(empId);
            ed.setAMobileNo(parseString(model.get("AMobileNo")));
            ed.setPMailId(parseString(model.get("PMailId")));
            ed.setFatherName(parseString(model.get("FatherName")));
            ed.setMotherName(parseString(model.get("MotherName")));
            ed.setHusbandName(parseString(model.get("HusbandName")));
            ed.setFContactNo(parseString(model.get("FContactNo")));
            ed.setMContactNo(parseString(model.get("MContactNo")));
            ed.setHContactNo(parseString(model.get("HContactNo")));
            ed.setEContactNo(parseString(model.get("EContactNo")));
            ed.setEContactName(parseString(model.get("EContactName")));
            ed.setEContactRelationship(parseString(model.get("EContactRelationship")));
            ed.setEContactNo1(parseString(model.get("EContactNo1")));
            ed.setEContactName1(parseString(model.get("EContactName1")));
            ed.setEContactRelationship1(parseString(model.get("EContactRelationship1")));
            ed.setEContactNo2(parseString(model.get("EContactNo2")));
            ed.setEContactName2(parseString(model.get("EContactName2")));
            ed.setEContactRelationship2(parseString(model.get("EContactRelationship2")));
            ed.setHeight(parseString(model.get("Height")));
            ed.setWeight(parseString(model.get("Weight")));
            ed.setDateOfAnniversary(parseStringDate(model.get("DateOfAnniversary")));
            ed.setDisability(parseString(model.get("Disability")));
            ed.setTotalExperience(parseString(model.get("TotalExperience")));
            ed.setRelevantExperience(parseString(model.get("RelevantExperience")));
            ed.setEcActivities(parseString(model.get("ECActivities")));
            ed.setSports(parseString(model.get("Sports")));
            ed.setCurrentDoorNumber(parseString(model.get("CurrentDoorNumber")));
            ed.setCurrentBuildingName(parseString(model.get("CurrentBuildingName")));
            ed.setCurrentStreet(parseString(model.get("CurrentStreet")));
            ed.setCurrentLocation(parseString(model.get("CurrentLocation")));
            ed.setCurrentCity(parseString(model.get("CurrentCity")));
            ed.setCurrentState(parseString(model.get("CurrentState")));
            ed.setCurrentCountry(parseString(model.get("CurrentCountry")));
            ed.setCurrentPinCode(parseString(model.get("CurrentPinCode")));
            ed.setPermanentDoorNumber(parseString(model.get("PermanentDoorNumber")));
            ed.setPermanentBuildingName(parseString(model.get("PermanentBuildingName")));
            ed.setPermanentStreet(parseString(model.get("PermanentStreet")));
            ed.setPermanentLocation(parseString(model.get("PermanentLocation")));
            ed.setPermanentCity(parseString(model.get("PermanentCity")));
            ed.setPermanentState(parseString(model.get("PermanentState")));
            ed.setPermanentCountry(parseString(model.get("PermanentCountry")));
            ed.setPermanentPinCode(parseString(model.get("PermanentPinCode")));
            ed.setCaste(parseString(model.get("Caste")));
            ed.setRegion(parseString(model.get("Region")));
            ed.setCountry(parseString(model.get("Country")));
            ed.setNationality(parseString(model.get("Nationality")));
            ed.setCreatedBy(loginId);
            ed.setCreatedDate(new Date());
            ed.setLastUpdatedBy(loginId);
            ed.setLastUpdatedDate(new Date());
            ed.setIsActive(true);
            ed.setIsUpdated(false);
            ed.setIsDeleted(false);
            employeeDetailRepository.save(ed);

            Map<String, Object> result = new HashMap<>();
            result.put("msg", "Added");
            return result;
        }

        Optional<EmployeeDetail> opt = employeeDetailRepository.findById(id);
        if (opt.isEmpty()) throw new RuntimeException("Employee Details Not Found");

        EmployeeDetail ed = opt.get();
        if (model.containsKey("AMobileNo")) ed.setAMobileNo(parseString(model.get("AMobileNo")));
        if (model.containsKey("PMailId")) ed.setPMailId(parseString(model.get("PMailId")));
        if (model.containsKey("FatherName")) ed.setFatherName(parseString(model.get("FatherName")));
        if (model.containsKey("MotherName")) ed.setMotherName(parseString(model.get("MotherName")));
        if (model.containsKey("HusbandName")) ed.setHusbandName(parseString(model.get("HusbandName")));
        if (model.containsKey("FContactNo")) ed.setFContactNo(parseString(model.get("FContactNo")));
        if (model.containsKey("MContactNo")) ed.setMContactNo(parseString(model.get("MContactNo")));
        if (model.containsKey("HContactNo")) ed.setHContactNo(parseString(model.get("HContactNo")));
        if (model.containsKey("EContactNo")) ed.setEContactNo(parseString(model.get("EContactNo")));
        if (model.containsKey("EContactName")) ed.setEContactName(parseString(model.get("EContactName")));
        if (model.containsKey("EContactRelationship")) ed.setEContactRelationship(parseString(model.get("EContactRelationship")));
        if (model.containsKey("EContactNo1")) ed.setEContactNo1(parseString(model.get("EContactNo1")));
        if (model.containsKey("EContactName1")) ed.setEContactName1(parseString(model.get("EContactName1")));
        if (model.containsKey("EContactRelationship1")) ed.setEContactRelationship1(parseString(model.get("EContactRelationship1")));
        if (model.containsKey("EContactNo2")) ed.setEContactNo2(parseString(model.get("EContactNo2")));
        if (model.containsKey("EContactName2")) ed.setEContactName2(parseString(model.get("EContactName2")));
        if (model.containsKey("EContactRelationship2")) ed.setEContactRelationship2(parseString(model.get("EContactRelationship2")));
        if (model.containsKey("Height")) ed.setHeight(parseString(model.get("Height")));
        if (model.containsKey("Weight")) ed.setWeight(parseString(model.get("Weight")));
        if (model.containsKey("DateOfAnniversary")) ed.setDateOfAnniversary(parseStringDate(model.get("DateOfAnniversary")));
        if (model.containsKey("Disability")) ed.setDisability(parseString(model.get("Disability")));
        if (model.containsKey("TotalExperience")) ed.setTotalExperience(parseString(model.get("TotalExperience")));
        if (model.containsKey("RelevantExperience")) ed.setRelevantExperience(parseString(model.get("RelevantExperience")));
        if (model.containsKey("ECActivities")) ed.setEcActivities(parseString(model.get("ECActivities")));
        if (model.containsKey("Sports")) ed.setSports(parseString(model.get("Sports")));
        if (model.containsKey("CurrentDoorNumber")) ed.setCurrentDoorNumber(parseString(model.get("CurrentDoorNumber")));
        if (model.containsKey("CurrentBuildingName")) ed.setCurrentBuildingName(parseString(model.get("CurrentBuildingName")));
        if (model.containsKey("CurrentStreet")) ed.setCurrentStreet(parseString(model.get("CurrentStreet")));
        if (model.containsKey("CurrentLocation")) ed.setCurrentLocation(parseString(model.get("CurrentLocation")));
        if (model.containsKey("CurrentCity")) ed.setCurrentCity(parseString(model.get("CurrentCity")));
        if (model.containsKey("CurrentState")) ed.setCurrentState(parseString(model.get("CurrentState")));
        if (model.containsKey("CurrentCountry")) ed.setCurrentCountry(parseString(model.get("CurrentCountry")));
        if (model.containsKey("CurrentPinCode")) ed.setCurrentPinCode(parseString(model.get("CurrentPinCode")));
        if (model.containsKey("PermanentDoorNumber")) ed.setPermanentDoorNumber(parseString(model.get("PermanentDoorNumber")));
        if (model.containsKey("PermanentBuildingName")) ed.setPermanentBuildingName(parseString(model.get("PermanentBuildingName")));
        if (model.containsKey("PermanentStreet")) ed.setPermanentStreet(parseString(model.get("PermanentStreet")));
        if (model.containsKey("PermanentLocation")) ed.setPermanentLocation(parseString(model.get("PermanentLocation")));
        if (model.containsKey("PermanentCity")) ed.setPermanentCity(parseString(model.get("PermanentCity")));
        if (model.containsKey("PermanentState")) ed.setPermanentState(parseString(model.get("PermanentState")));
        if (model.containsKey("PermanentCountry")) ed.setPermanentCountry(parseString(model.get("PermanentCountry")));
        if (model.containsKey("PermanentPinCode")) ed.setPermanentPinCode(parseString(model.get("PermanentPinCode")));
        if (model.containsKey("Caste")) ed.setCaste(parseString(model.get("Caste")));
        if (model.containsKey("Region")) ed.setRegion(parseString(model.get("Region")));
        if (model.containsKey("Country")) ed.setCountry(parseString(model.get("Country")));
        if (model.containsKey("Nationality")) ed.setNationality(parseString(model.get("Nationality")));
        ed.setIsUpdated(true);
        ed.setLastUpdatedDate(new Date());
        employeeDetailRepository.save(ed);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteEmployeeContactInformation(Map<String, Object> model) {
        Integer id = parseInteger(model.get("Id"));
        if (id == 0) throw new RuntimeException("Id is Missing");

        Optional<EmployeeDetail> opt = employeeDetailRepository.findById(id);
        if (opt.isEmpty()) throw new RuntimeException("Employee Details Not Found");

        EmployeeDetail ed = opt.get();
        ed.setIsDeleted(true);
        ed.setIsUpdated(true);
        ed.setLastUpdatedDate(new Date());
        employeeDetailRepository.save(ed);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Deleted");
        return result;
    }

    public List<Map<String, Object>> getOnSiteData(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            // Verify employee exists and is active
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(loginId);
            if (empOpt.isEmpty() || !Boolean.TRUE.equals(empOpt.get().getIsActive()) || Boolean.TRUE.equals(empOpt.get().getIsDeleted())) {
                throw new RuntimeException("Employee onsite details Not Found");
            }

            List<OnSiteLoginlog> onSiteLogs = onSiteLoginlogRepository.findByEmpIdAndIsActiveAndIsDeletedOrderByLoginDateDesc(loginId, true, false);

            if (onSiteLogs.isEmpty()) {
                throw new RuntimeException("Employee onsite details Not Found");
            }

            // Sort by Id descending to match dotnet OrderByDescending(x => x.Id)
            onSiteLogs.sort((a, b) -> b.getId().compareTo(a.getId()));

            for (OnSiteLoginlog log : onSiteLogs) {
                Map<String, Object> record = new LinkedHashMap<>();
                record.put("Id", log.getId());
                record.put("EmpId", log.getEmpId());
                record.put("EmpCode", log.getEmpCode());
                record.put("Company", log.getCompany());
                record.put("LoginAddress", log.getLoginAddress());
                record.put("LoginCity", log.getLoginCity());
                record.put("LoginDate", log.getLoginDate());
                record.put("LoginLongitude", log.getLoginLongitude());
                record.put("LoginLatitude", log.getLoginLatitude());
                record.put("Purpose", log.getPurpose());
                record.put("Description", log.getDescription());
                record.put("LogoutAddress", log.getLogoutAddress());
                record.put("LogoutCity", log.getLogoutCity());
                record.put("LogoutDate", log.getLogoutDate());
                record.put("LogoutLongitude", log.getLogoutLongitude());
                record.put("LogoutLatitude", log.getLogoutLatitude());
                record.put("LogInTime", timeToTimeSpan(log.getLogInTime(), false));
                record.put("LogOutTime", timeToTimeSpan(log.getLogOutTime(), false));
                record.put("ActiveHrs", timeToTimeSpan(log.getActiveHrs(), true));
                record.put("CreatedBy", log.getCreatedBy());
                record.put("CreatedDate", log.getCreatedDate());
                record.put("LastUpdatedBy", log.getLastUpdatedBy());
                record.put("LastUpdatedDate", log.getLastUpdatedDate());
                record.put("IsActive", log.getIsActive());
                record.put("IsUpdated", log.getIsUpdated());
                record.put("IsDeleted", log.getIsDeleted());
                // Aliases for frontend filter (uses row.Address, row.City)
                record.put("Address", log.getLoginAddress());
                record.put("City", log.getLoginCity());
                result.add(record);
            }
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error fetching onsite data: " + e.getMessage());
        }
        return result;
    }

    public OnSiteDataViewModel addOnSiteData(Map<String, Object> model) {
        try {
            Integer empid = model.get("LoginId") != null ? Integer.valueOf(model.get("LoginId").toString()) : 0;
            empid = (empid != 0) ? empid : 0;

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            String loginDateStr = model.get("LoginDate") != null ? model.get("LoginDate").toString() : null;
            Date tdy = loginDateStr != null && !loginDateStr.isEmpty() ? sdf.parse(loginDateStr) : new Date();

            if (empid != 0) {
                List<WorkTypeMaster> workTypeList = workTypeMasterRepository.findActiveApprovedWorkTypeByEmpId(empid);
                WorkTypeMaster workTypeDetails = null;
                for (WorkTypeMaster wt : workTypeList) {
                    if (wt.getStartDate() != null && wt.getEndDate() != null
                            && !wt.getStartDate().after(tdy) && !wt.getEndDate().before(tdy)) {
                        workTypeDetails = wt;
                        break;
                    }
                }

                if (workTypeDetails != null) {
                    return processOnSiteLoginLogout(model, empid, loginDateStr, tdy, sdf);
                } else {
                    return processOnSiteLoginLogout(model, empid, loginDateStr, tdy, sdf);
                }
            }

            OnSiteDataViewModel result = new OnSiteDataViewModel();
            result.setMsg("Added");
            return result;
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException(e.getMessage());
        }
    }

    private OnSiteDataViewModel processOnSiteLoginLogout(Map<String, Object> model, Integer empid,
                                                          String loginDateStr, Date tdy, SimpleDateFormat sdf) throws Exception {
        String workStatus = (String) model.get("WorkStatus");
        Integer loginId = model.get("LoginId") != null ? Integer.valueOf(model.get("LoginId").toString()) : 0;

        if ("Login".equals(workStatus)) {
            OnSiteLoginlog osd = new OnSiteLoginlog();
            osd.setEmpId(empid);
            osd.setEmpCode((String) model.get("EmpCode"));
            osd.setCompany((String) model.get("Company"));
            osd.setLoginAddress((String) model.get("LoginAddress"));
            osd.setLoginCity((String) model.get("LoginCity"));
            if (loginDateStr != null && !loginDateStr.isEmpty()) {
                osd.setLoginDate(tdy);
            }
            osd.setLogInTime(new Date(System.currentTimeMillis()));
            osd.setLoginLongitude((String) model.get("LoginLongitude"));
            osd.setLoginLatitude((String) model.get("LoginLatitude"));
            osd.setPurpose((String) model.get("Purpose"));
            osd.setDescription((String) model.get("Description"));
            osd.setLogoutAddress((String) model.get("LogoutAddress"));
            osd.setLogoutCity((String) model.get("LogoutCity"));
            String logoutDateStr = (String) model.get("LogoutDate");
            if (logoutDateStr != null && !logoutDateStr.isEmpty()) {
                osd.setLogoutDate(sdf.parse(logoutDateStr));
            }
            osd.setLogoutLongitude((String) model.get("LogoutLongitude"));
            osd.setLogoutLatitude((String) model.get("LogoutLatitude"));
            osd.setCreatedBy(loginId);
            osd.setCreatedDate(new Date());
            osd.setIsActive(true);
            osd.setIsUpdated(false);
            osd.setIsDeleted(false);

            onSiteLoginlogRepository.save(osd);

            OnSiteDataViewModel result = new OnSiteDataViewModel();
            result.setMsg("Added");
            return result;
        } else {
            Integer id = model.get("Id") != null ? Integer.valueOf(model.get("Id").toString()) : 0;
            if (id != 0) {
                Optional<OnSiteLoginlog> onSiteOpt = onSiteLoginlogRepository.findById(id);
                if (onSiteOpt.isPresent()) {
                    OnSiteLoginlog onsitedetails = onSiteOpt.get();
                    onsitedetails.setDescription((String) model.get("Description"));
                    onsitedetails.setLogoutAddress((String) model.get("LogoutAddress"));
                    onsitedetails.setLogoutCity((String) model.get("LogoutCity"));
                    String logoutDateStr = (String) model.get("LogoutDate");
                    if (logoutDateStr != null && !logoutDateStr.isEmpty()) {
                        onsitedetails.setLogoutDate(sdf.parse(logoutDateStr));
                    }
                    onsitedetails.setLogoutLongitude((String) model.get("LogoutLongitude"));
                    onsitedetails.setLogoutLatitude((String) model.get("LogoutLatitude"));
                    onsitedetails.setLogOutTime(new Date(System.currentTimeMillis()));

                    if (onsitedetails.getLogInTime() != null && onsitedetails.getLogOutTime() != null) {
                        long diffMs = onsitedetails.getLogOutTime().getTime() - onsitedetails.getLogInTime().getTime();
                        onsitedetails.setActiveHrs(new Date(diffMs));
                    }

                    onsitedetails.setIsUpdated(true);
                    onsitedetails.setLastUpdatedBy(loginId);
                    onsitedetails.setLastUpdatedDate(new Date());

                    onSiteLoginlogRepository.save(onsitedetails);

                    OnSiteDataViewModel result = new OnSiteDataViewModel();
                    result.setMsg("Updated");
                    return result;
                } else {
                    throw new RuntimeException("Login details not found");
                }
            } else {
                throw new RuntimeException("Onsite Id is missing");
            }
        }
    }

    public EmployeeMasterViewModel getEmployee(EmployeeMasterViewModel model) {
        int loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        int empId = (model.getEmpId() != null && model.getEmpId() != 0) ? model.getEmpId() : 0;

        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
        if (empOpt.isEmpty()) throw new RuntimeException("Employee Details Not Found");

        EmployeeMaster emp = empOpt.get();
        if (emp.getIsActive() == null || !emp.getIsActive() || emp.getIsDeleted() != null && emp.getIsDeleted()) {
            throw new RuntimeException("Employee Details Not Found");
        }

        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
        emvm.setEmpId(emp.getEmpId());
        emvm.setOldEmp_ID(emp.getOldEmp_ID());
        emvm.setCompId(emp.getCompId());

        if (emp.getCompId() != null) {
            Optional<CompanyMaster> compOpt = companyMasterRepository.findById(emp.getCompId());
            emvm.setCompany(compOpt.map(CompanyMaster::getCompany).orElse(""));
        }

        emvm.setLeId(emp.getLeId() != null ? emp.getLeId() : 0);
        if (emvm.getLeId() != null && emvm.getLeId() != 0) {
            Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(emvm.getLeId());
            emvm.setLegalEntity(leOpt.map(LegalEntityMaster::getLegalEntity).orElse(""));
        } else {
            emvm.setLegalEntity("");
        }

        emvm.setBuId(emp.getBuId() != null ? emp.getBuId() : 0);
        if (emvm.getBuId() != null && emvm.getBuId() != 0) {
            Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(emvm.getBuId());
            emvm.setBusinessUnit(buOpt.map(BusinessUnitMaster::getBusinessUnit).orElse(""));
        } else {
            emvm.setBusinessUnit("");
        }

        emvm.setLocationId(emp.getLocationId() != null ? emp.getLocationId() : 0);
        if (emvm.getLocationId() != 0) {
            Optional<LocationMaster> locOpt = locationMasterRepository.findById(emvm.getLocationId());
            emvm.setLocation(locOpt.map(LocationMaster::getLocation).orElse(""));
        }

        emvm.setCategoryId(emp.getCategoryId());
        emvm.setDeptId(emp.getCategoryId());
        emvm.setDeptName(emp.getDeptName());
        emvm.setDesignationId(emp.getDesignationId());
        emvm.setDesignation(emp.getDesignationName());
        emvm.setReportId(emp.getReportId());
        emvm.setApproverId(emp.getReportId());
        emvm.setAuthorisedEntity(emp.getAuthorisedEntity());
        emvm.setApprover("");

        if (emp.getReportId() != null && emp.getReportId() != 0) {
            Optional<EmployeeMaster> approverOpt = employeeMasterRepository.findById(emp.getReportId());
            if (approverOpt.isPresent()) {
                EmployeeMaster approver = approverOpt.get();
                String firstName = approver.getFirstName() != null ? approver.getFirstName() : "";
                String middleName = approver.getMiddleName() != null ? approver.getMiddleName() : "";
                String lastName = approver.getLastName() != null ? approver.getLastName() : "";
                String empCode = approver.getEmpCode() != null ? approver.getEmpCode() : "";
                emvm.setApprover(firstName + " " + middleName + " " + lastName + " - " + empCode);
            }
        }

        emvm.setEmpCode(emp.getEmpCode());
        emvm.setUserName(emp.getUserName());
        emvm.setPhoto(emp.getPhoto());
        if (emvm.getPhoto() != null && !emvm.getPhoto().isEmpty() && emvm.getPhoto().contains("Uploads")) {
            String[] parts = emvm.getPhoto().split("Uploads", 2);
            if (parts.length > 1) {
                emvm.setPhoto("Uploads" + parts[1]);
            }
        }

        emvm.setSalutationId(emp.getSalutation());
        if (emvm.getSalutationId() != null && emvm.getSalutationId() != 0) {
            Optional<SalutationMaster> salOpt = salutationMasterRepository.findById(emvm.getSalutationId());
            emvm.setSalutation(salOpt.map(SalutationMaster::getSalutation).orElse(""));
        }

        emvm.setFirstName(emp.getFirstName());
        emvm.setMiddleName(emp.getMiddleName());
        emvm.setLastName(emp.getLastName());
        emvm.setDob(convertToJsonDateObj(emp.getDob()));
        emvm.setMobileNo(emp.getMobileNo());
        emvm.setEmailId(emp.getEmailId());
        emvm.setBloodGroup(emp.getBloodGroup());
        emvm.setMaritalStatus(emp.getMaritalStatus());
        emvm.setGender(emp.getGender());
        emvm.setJoiningDate(convertToJsonDateObj(emp.getJoiningDate()));
        emvm.setInterviewDate(convertToJsonDateObj(emp.getInterviewDate()));
        emvm.setEndDate(convertToJsonDateObj(emp.getEndDate()));
        emvm.setEmpStatus(emp.getEmpStatus() != null ? emp.getEmpStatus().toUpperCase() : "");
        emvm.setReason(emp.getReason());
        emvm.setEmpTypeId(emp.getEmpType());
        if (emvm.getEmpTypeId() != null && emvm.getEmpTypeId() != 0) {
            Optional<EmpTypeMaster> etOpt = empTypeMasterRepository.findById(emvm.getEmpTypeId());
            emvm.setEmpType(etOpt.map(EmpTypeMaster::getEmpType).orElse(""));
        }
        emvm.setcEndDate(convertToJsonDateObj(emp.getcEndDate()));

        List<EmpProbationTrackingHistory> probationList;
        try {
            probationList = empProbationTrackingHistoryRepository.findByEmpIdAndIsActiveAndIsDeletedOrderByCreatedDateDesc(empId, true, false);
        } catch (Exception e) {
            probationList = new ArrayList<>();
        }
        EmpProbationTrackingHistory probation = probationList.isEmpty() ? null : probationList.get(0);

        if (probation != null) {
            emvm.setIsProbation(probation.getIsProbation());
            if (Boolean.TRUE.equals(probation.getIsProbation())) {
                emvm.setProbationConfirmationStatus("Probation");
                if (probation.getProbationEndDate() != null) {
                    SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
                    emvm.setProbationConfirmationEffectiveDate(sdf.format(probation.getProbationEndDate()));
                }
                if (probation.getConfirmDate() != null) {
                    SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
                    emvm.setProbationConfirmationDate(sdf.format(probation.getConfirmDate()));
                }
            } else {
                emvm.setProbationConfirmationStatus("Permanent");
                if (probation.getProbationEndDate() != null) {
                    SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
                    emvm.setProbationConfirmationEffectiveDate(sdf.format(probation.getProbationEndDate()));
                }
                if (probation.getConfirmDate() != null) {
                    SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
                    emvm.setProbationConfirmationDate(sdf.format(probation.getConfirmDate()));
                }
            }
        } else {
            emvm.setIsProbation(false);
            emvm.setProbationConfirmationStatus("No Status");
            emvm.setProbationConfirmationEffectiveDate("");
            emvm.setProbationConfirmationDate("");
        }

        emvm.setIsActive(emp.getIsActive());
        emvm.setIsUpdated(emp.getIsUpdated());
        emvm.setIsDeleted(emp.getIsDeleted());
        emvm.setCreatedBy(emp.getCreatedBy());
        emvm.setCreatedDate(convertToJsonDateObj(emp.getCreatedDate()));
        emvm.setLastUpdatedBy(emp.getLastUpdatedBy());
        emvm.setLastUpdatedDate(convertToJsonDateObj(emp.getLastUpdatedDate()));
        emvm.setMsg("GetEmployee - Success");

        return emvm;
    }

    public EmployeeMasterViewModel addEmployee(EmployeeMasterViewModel model) {
        int loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<EmployeeMaster> existing = employeeMasterRepository.findByUserNameIgnoreCaseAndIsActiveAndIsDeleted(
            model.getEmpCode() != null ? model.getEmpCode() : "", true, false);
        if (!existing.isEmpty()) throw new RuntimeException("Employee with this EmpCode already exists");

        String reportName = "";
        if (model.getReportId() != null && model.getReportId() != 0) {
            Optional<EmployeeMaster> reportOpt = employeeMasterRepository.findById(model.getReportId());
            if (reportOpt.isPresent() && reportOpt.get().getEmpCode() != null) {
                reportName = reportOpt.get().getEmpCode();
            }
        }

        String password = "password";
        String encodedPassword;
        try {
            encodedPassword = Base64.getEncoder().encodeToString(password.getBytes(StandardCharsets.UTF_16LE));
        } catch (Exception e) {
            encodedPassword = password;
        }

        EmployeeMaster em = new EmployeeMaster();
        em.setOldEmp_ID(0);
        em.setCompId(model.getCompId());
        em.setLeId(model.getLeId() != null ? model.getLeId() : 0);
        em.setBuId(model.getBuId() != null ? model.getBuId() : 0);
        em.setLocationId(model.getLocationId() != null ? model.getLocationId() : 0);
        em.setCategoryId(model.getDeptId());
        em.setDeptName(model.getDeptName());
        em.setDesignationId(model.getDesignationId());
        em.setDesignationName(model.getDesignation());
        em.setReportId(model.getReportId());
        em.setReportName(reportName);
        em.setEmpCode(model.getEmpCode());
        em.setUserName(model.getEmpCode());
        em.setPassword(encodedPassword);
        em.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        em.setSalutation(model.getSalutationId());
        em.setFirstName(model.getFirstName());
        em.setMiddleName(model.getMiddleName() != null ? model.getMiddleName() : "");
        em.setLastName(model.getLastName());
        em.setDob(parseDateFromObject(model.getDob()));
        em.setMobileNo(model.getMobileNo());
        em.setEmailId(model.getEmailId());
        em.setBloodGroup(model.getBloodGroup());
        em.setMaritalStatus(model.getMaritalStatus());
        em.setGender(model.getGender());
        em.setInterviewDate(parseDateFromObject(model.getInterviewDate()));
        em.setJoiningDate(parseDateFromObject(model.getJoiningDate()));
        em.setEmpType(model.getEmpTypeId());
        em.setEmpStatus("Active");
        em.setAuthorisedEntity(model.getAuthorisedEntity());
        em.setIsRelieved(false);
        em.setcEndDate(parseDateFromObject(model.getcEndDate()));
        em.setIsActive(true);
        em.setIsUpdated(false);
        em.setIsDeleted(false);
        em.setCreatedBy(loginId);
        em.setCreatedDate(new Date());
        em.setLastUpdatedBy(loginId);
        em.setLastUpdatedDate(new Date());

        employeeMasterRepository.save(em);

        // Create log entry
        EmployeeMasterLog eml = new EmployeeMasterLog();
        eml.setEmpId(em.getEmpId());
        eml.setOldEmp_ID(0);
        eml.setCompId(model.getCompId());
        eml.setLeId(model.getLeId() != null ? model.getLeId() : 0);
        eml.setBuId(model.getBuId() != null ? model.getBuId() : 0);
        eml.setLocationId(model.getLocationId() != null ? model.getLocationId() : 0);
        eml.setCategoryId(model.getDeptId());
        eml.setDeptName(model.getDeptName());
        eml.setDesignationId(model.getDesignationId());
        eml.setDesignationName(model.getDesignation());
        eml.setReportId(model.getReportId());
        eml.setReportName(reportName);
        eml.setEmpCode(model.getEmpCode());
        eml.setUserName(model.getEmpCode());
        eml.setPassword(encodedPassword);
        eml.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        eml.setSalutation(model.getSalutationId());
        eml.setFirstName(model.getFirstName());
        eml.setMiddleName(model.getMiddleName() != null ? model.getMiddleName() : "");
        eml.setLastName(model.getLastName());
        eml.setDob(parseDateFromObject(model.getDob()));
        eml.setMobileNo(model.getMobileNo());
        eml.setEmailId(model.getEmailId());
        eml.setBloodGroup(model.getBloodGroup());
        eml.setMaritalStatus(model.getMaritalStatus());
        eml.setGender(model.getGender());
        eml.setJoiningDate(parseDateFromObject(model.getJoiningDate()));
        eml.setEmpType(model.getEmpTypeId());
        eml.setEmpStatus("Active");
        eml.setAuthorisedEntity(model.getAuthorisedEntity());
        eml.setIsRelieved(false);
        eml.setCEndDate(parseDateFromObject(model.getcEndDate()));
        eml.setIsActive(true);
        eml.setIsUpdated(false);
        eml.setIsDeleted(false);
        eml.setCreatedBy(loginId);
        eml.setCreatedDate(new Date());
        eml.setLastUpdatedBy(loginId);
        eml.setLastUpdatedDate(new Date());
        employeeMasterLogRepository.save(eml);

        model.setEmpId(em.getEmpId());
        model.setMsg("Added");
        return model;
    }

    public List<EmployeeMasterViewModel> pcGetAllEmployee(EmployeeMasterViewModel model) {
        List<Map<String, Object>> result = new ArrayList<>();
        return new ArrayList<>();
    }

    public List<EmployeeMasterViewModel> pcAddAllEmployee(EmployeeMasterViewModel model) {
        List<Map<String, Object>> result = new ArrayList<>();
        return new ArrayList<>();
    }

    public List<Map<String, Object>> getLoginLogs() {
        List<Map<String, Object>> result = new ArrayList<>();
        return result;
    }

    public Map<String, Object> createHoliday(Map<String, Object> model) {
        Holiday holiday = new Holiday();
        Object holidayDate = model.get("holidayDate");
        Object holidayName = model.get("holidayName");
        Object day = model.get("day");
        Object locationId = model.get("locationId");
        
        if (holidayName != null) {
            holiday.setTitle(holidayName.toString());
        }
        if (day != null) {
            holiday.setHolidayType(day.toString());
        }
        if (locationId != null) {
            holiday.setLocationId(Integer.parseInt(locationId.toString()));
        }
        holiday.setCreatedDate(new Date());
        
        holiday = holidayRepository.save(holiday);
        
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Holiday created successfully");
        result.put("id", holiday.getHolidayId());
        return result;
    }

    @SuppressWarnings("unchecked")
    public Map<String, Object> updateHoliday(Map<String, Object> model) {
        Object modifyByObj = model.get("Modify_By");
        if (modifyByObj == null || Integer.parseInt(modifyByObj.toString()) == 0) {
            throw new RuntimeException("Invalid ModifiedBy ID.");
        }
        Integer modifyBy = Integer.parseInt(modifyByObj.toString());

        Object holidayIdObj = model.get("Holiday_Id");
        if (holidayIdObj == null) {
            throw new RuntimeException("Invalid Holiday ID list.");
        }
        List<Integer> holidayIdList = new ArrayList<>();
        if (holidayIdObj instanceof List) {
            for (Object o : (List<Object>) holidayIdObj) {
                if (o instanceof Number) {
                    holidayIdList.add(((Number) o).intValue());
                } else {
                    holidayIdList.add(Integer.parseInt(o.toString()));
                }
            }
        } else {
            holidayIdList.add(Integer.parseInt(holidayIdObj.toString()));
        }
        if (holidayIdList.isEmpty()) {
            throw new RuntimeException("Invalid Holiday ID list.");
        }

        Object holidayLocationIdObj = model.get("HolidayLocationId");
        if (holidayLocationIdObj == null) {
            throw new RuntimeException("HolidayLocationId list is required.");
        }
        List<Integer> selectedLocationIds = new ArrayList<>();
        if (holidayLocationIdObj instanceof List) {
            for (Object o : (List<Object>) holidayLocationIdObj) {
                if (o instanceof Number) {
                    selectedLocationIds.add(((Number) o).intValue());
                } else {
                    selectedLocationIds.add(Integer.parseInt(o.toString()));
                }
            }
        } else {
            selectedLocationIds.add(Integer.parseInt(holidayLocationIdObj.toString()));
        }

        List<Object> holidayLocationList = new ArrayList<>();
        Object holidayLocationObj = model.get("HolidayLocation");
        if (holidayLocationObj instanceof List) {
            holidayLocationList = (List<Object>) holidayLocationObj;
        }

        List<Holiday> existingHolidays = holidayRepository.findAllById(holidayIdList);

        List<Holiday> holidaysToRemove = existingHolidays.stream()
            .filter(h -> h.getLocationId() != null && !selectedLocationIds.contains(h.getLocationId()))
            .collect(Collectors.toList());

        for (Holiday h : holidaysToRemove) {
            holidayRepository.delete(h);
        }

        String dateStr = model.get("Date") != null ? model.get("Date").toString() : "";
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
        Date holidayDate;
        try {
            holidayDate = dateStr.isEmpty() ? null : sdf.parse(dateStr);
        } catch (Exception e) {
            throw new RuntimeException("Invalid date format: " + dateStr);
        }

        for (int i = 0; i < selectedLocationIds.size(); i++) {
            int locationId = selectedLocationIds.get(i);
            String locationName = (i < holidayLocationList.size()) ? holidayLocationList.get(i).toString() : "";

            Holiday existingHoliday = existingHolidays.stream()
                .filter(h -> h.getLocationId() != null && h.getLocationId() == locationId)
                .findFirst().orElse(null);

            if (existingHoliday != null) {
                existingHoliday.setTitle(model.get("Title") != null ? model.get("Title").toString() : "");
                if (holidayDate != null) existingHoliday.setDate(holidayDate);
                existingHoliday.setYear(model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : null);
                existingHoliday.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
                existingHoliday.setLocation(locationName);
                existingHoliday.setModifyBy(modifyBy);
                existingHoliday.setModifyDate(new Date());
                existingHoliday.setStatus(model.get("Status") != null ? model.get("Status").toString() : "Active");
                existingHoliday.setHolidayType(model.get("HolidayType") != null ? model.get("HolidayType").toString() : "");
                holidayRepository.save(existingHoliday);
            } else {
                Holiday newHoliday = new Holiday();
                newHoliday.setTitle(model.get("Title") != null ? model.get("Title").toString() : "");
                if (holidayDate != null) newHoliday.setDate(holidayDate);
                newHoliday.setYear(model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : null);
                newHoliday.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
                newHoliday.setLocationId(locationId);
                newHoliday.setLocation(locationName);
                newHoliday.setCreatedBy(model.get("Created_By") != null ? Integer.parseInt(model.get("Created_By").toString()) : 0);
                newHoliday.setCreatedDate(new Date());
                newHoliday.setModifyBy(modifyBy);
                newHoliday.setModifyDate(new Date());
                newHoliday.setStatus(model.get("Status") != null ? model.get("Status").toString() : "Active");
                newHoliday.setHolidayType(model.get("HolidayType") != null ? model.get("HolidayType").toString() : "");
                holidayRepository.save(newHoliday);
            }
        }

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Holiday(s) updated successfully.");
        return result;
    }

    @SuppressWarnings("unchecked")
    public Map<String, Object> deleteHoliday(Map<String, Object> model) {
        Object holidayIdObj = model.get("Holiday_Id");
        if (holidayIdObj == null) {
            throw new RuntimeException("Invalid Holiday_Id list.");
        }

        List<Integer> holidayIdList = new ArrayList<>();
        if (holidayIdObj instanceof List) {
            for (Object o : (List<Object>) holidayIdObj) {
                if (o instanceof Number) {
                    holidayIdList.add(((Number) o).intValue());
                } else {
                    holidayIdList.add(Integer.parseInt(o.toString()));
                }
            }
        } else {
            holidayIdList.add(Integer.parseInt(holidayIdObj.toString()));
        }
        if (holidayIdList.isEmpty()) {
            throw new RuntimeException("Invalid Holiday_Id list.");
        }

        List<Holiday> holidays = holidayRepository.findAllById(holidayIdList).stream()
            .filter(h -> "Active".equals(h.getStatus()))
            .collect(Collectors.toList());

        if (holidays.isEmpty()) {
            throw new RuntimeException("No matching Holidays found.");
        }

        Object modifyByObj = model.get("Modify_By");
        Integer modifyBy = (modifyByObj != null) ? Integer.parseInt(modifyByObj.toString()) : 0;
        if (modifyBy == 0) {
            Object loginIdObj = model.get("LoginId");
            if (loginIdObj != null) {
                modifyBy = Integer.parseInt(loginIdObj.toString());
            }
        }

        for (Holiday holiday : holidays) {
            holiday.setStatus("Inactive");
            holiday.setModifyBy(modifyBy);
            holiday.setModifyDate(new Date());
            holidayRepository.save(holiday);
        }

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Holiday(s) Deleted Successfully");
        return result;
    }

    public Map<String, Object> getAllHolidayEMP(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        List<Holiday> holidays = holidayRepository.findAll();
        for (Holiday h : holidays) {
            Map<String, Object> m = new HashMap<>();
            m.put("Holiday_Id", h.getHolidayId());
            m.put("Title", h.getTitle());
            m.put("Date", h.getDate());
            m.put("HolidayType", h.getHolidayType());
            m.put("locationId", h.getLocationId());
            m.put("Description", h.getDescription());
            m.put("Year", h.getYear());
            m.put("Status", h.getStatus());
            m.put("Location", h.getLocation());
            result.add(m);
        }
        Map<String, Object> response = new HashMap<>();
        response.put("holidays", result);
        response.put("count", holidays.size());
        return response;
    }

    public Map<String, Object> getPageById(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("pageId", 1);
        result.put("pageName", "Dashboard");
        return result;
    }

    public Map<String, Object> updatePageModules(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Page modules updated successfully");
        return result;
    }

    public Map<String, Object> deletePageModules(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Page modules deleted successfully");
        return result;
    }

    public Map<String, Object> getBehaviouralGoal(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("goalId", 1);
        result.put("goalName", "Leadership");
        return result;
    }

    public Map<String, Object> addBehaviouralGoal(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Behavioral goal added successfully");
        return result;
    }

    public Map<String, Object> updateBehaviouralGoal(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Behavioral goal updated successfully");
        return result;
    }

    public Map<String, Object> deleteBehaviouralGoal(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Behavioral goal deleted successfully");
        return result;
    }

    public List<Map<String, Object>> getAllBehaviouralGoal() {
        List<Map<String, Object>> result = new ArrayList<>();
        Map<String, Object> m1 = new HashMap<>(); m1.put("goalId", 1); m1.put("goalName", "Leadership");
        Map<String, Object> m2 = new HashMap<>(); m2.put("goalId", 2); m2.put("goalName", "Communication");
        result.add(m1); result.add(m2);
        return result;
    }

    public List<Map<String, Object>> getEmployeeGoalHistory() {
        List<Map<String, Object>> result = new ArrayList<>();
        return result;
    }

    public Map<String, Object> getEmployeeQuarterDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("quarter", "Q1");
        result.put("year", 2024);
        return result;
    }

    public Map<String, Object> getEmployeeSalaryDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("salaryId", 1);
        result.put("basicSalary", 50000);
        return result;
    }

    public Map<String, Object> addEmployeeSalaryDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee salary details added successfully");
        return result;
    }

    public Map<String, Object> updateEmployeeSalaryDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee salary details updated successfully");
        return result;
    }

    public Map<String, Object> deleteEmployeeSalaryDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee salary details deleted successfully");
        return result;
    }

    public List<Map<String, Object>> getAllEmployeeSalaryDetails() {
        List<Map<String, Object>> result = new ArrayList<>();
        return result;
    }

    public List<Map<String, Object>> getAllEmployeeNomineeDetails() {
        List<Map<String, Object>> result = new ArrayList<>();
        return result;
    }

    public Map<String, Object> addEmployeeNomineeDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee nominee details added successfully");
        return result;
    }

    public Map<String, Object> getEmployeeNomineeDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("nomineeId", 1);
        result.put("nomineeName", "John Doe");
        return result;
    }

    public Map<String, Object> updateEmployeeNomineeDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee nominee details updated successfully");
        return result;
    }

    public Map<String, Object> deleteEmployeeNomineeDetails(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee nominee details deleted successfully");
        return result;
    }

    public List<Map<String, Object>> getAllCompany(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        return companyMasterRepository.findAll().stream()
            .filter(c -> c.getIsDeleted() == null || !c.getIsDeleted())
            .map(c -> {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("LoginId", loginId);
                m.put("CompId", c.getCompId());
                m.put("Company", c.getCompany());
                m.put("CompanyCode", c.getCompanyCode());
                m.put("LocationMap", c.getLocationMap());
                m.put("Address", c.getAddress());
                m.put("CreatedBy", c.getCreatedBy());
                m.put("CreatedDate", c.getCreatedDate() != null ? "\\/Date(" + c.getCreatedDate().getTime() + ")\\/" : null);
                m.put("LastUpdatedBy", c.getLastUpdatedBy());
                m.put("LastUpdatedDate", c.getLastUpdatedDate() != null ? "\\/Date(" + c.getLastUpdatedDate().getTime() + ")\\/" : null);
                m.put("IsActive", c.getIsActive());
                m.put("IsUpdated", c.getIsUpdated());
                m.put("IsDeleted", c.getIsDeleted());
                return m;
            })
            .collect(Collectors.toList());
    }

    // =================== Company CRUD ===================

    public Map<String, Object> getCompany(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer compId = model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<CompanyMaster> compOpt = companyMasterRepository.findById(compId);
        if (compOpt.isEmpty() || (compOpt.get().getIsDeleted() != null && compOpt.get().getIsDeleted())) {
            throw new RuntimeException("Company Details Not Found");
        }

        CompanyMaster c = compOpt.get();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("LoginId", loginId);
        m.put("CompId", c.getCompId());
        m.put("Company", c.getCompany());
        m.put("CompanyCode", c.getCompanyCode());
        m.put("LocationMap", c.getLocationMap());
        m.put("Address", c.getAddress());
        m.put("CreatedBy", c.getCreatedBy());
        m.put("CreatedDate", c.getCreatedDate() != null ? "\\/Date(" + c.getCreatedDate().getTime() + ")\\/" : null);
        m.put("LastUpdatedBy", c.getLastUpdatedBy());
        m.put("LastUpdatedDate", c.getLastUpdatedDate() != null ? "\\/Date(" + c.getLastUpdatedDate().getTime() + ")\\/" : null);
        m.put("IsActive", c.getIsActive());
        m.put("IsUpdated", c.getIsUpdated());
        m.put("IsDeleted", c.getIsDeleted());
        return m;
    }

    public Map<String, Object> addCompany(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        String company = model.get("Company") != null ? model.get("Company").toString() : "";
        String companyCode = model.get("CompanyCode") != null ? model.get("CompanyCode").toString() : "";

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        boolean exists = companyMasterRepository.findAll().stream()
            .anyMatch(c -> c.getCompany() != null && c.getCompany().equals(company)
                && c.getCompanyCode() != null && c.getCompanyCode().equals(companyCode)
                && (c.getIsDeleted() == null || !c.getIsDeleted()));

        if (exists) {
            throw new RuntimeException("Company Details Already Exists");
        }

        CompanyMaster cm = new CompanyMaster();
        cm.setCompany(company);
        cm.setCompanyCode(companyCode);
        cm.setLocationMap(model.get("LocationMap") != null ? model.get("LocationMap").toString() : null);
        cm.setAddress(model.get("Address") != null ? model.get("Address").toString() : null);
        cm.setCreatedBy(loginId);
        cm.setCreatedDate(new Date());
        cm.setLastUpdatedBy(loginId);
        cm.setLastUpdatedDate(new Date());
        cm.setIsActive(true);
        cm.setIsUpdated(false);
        cm.setIsDeleted(false);
        companyMasterRepository.save(cm);

        result.put("Status", 200);
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateCompany(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer compId = model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        Optional<CompanyMaster> compOpt = companyMasterRepository.findById(compId);
        if (compOpt.isEmpty() || (compOpt.get().getIsDeleted() != null && compOpt.get().getIsDeleted())) {
            throw new RuntimeException("Company Details Not Found");
        }

        CompanyMaster cm = compOpt.get();
        cm.setCompany(model.get("Company") != null ? model.get("Company").toString() : cm.getCompany());
        cm.setCompanyCode(model.get("CompanyCode") != null ? model.get("CompanyCode").toString() : cm.getCompanyCode());
        cm.setLocationMap(model.get("LocationMap") != null ? model.get("LocationMap").toString() : cm.getLocationMap());
        cm.setAddress(model.get("Address") != null ? model.get("Address").toString() : cm.getAddress());
        cm.setLastUpdatedBy(loginId);
        cm.setLastUpdatedDate(new Date());
        cm.setIsUpdated(true);
        companyMasterRepository.save(cm);

        result.put("Status", 200);
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteCompany(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer compId = model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<CompanyMaster> compOpt = companyMasterRepository.findById(compId);
        if (compOpt.isEmpty() || (compOpt.get().getIsDeleted() != null && compOpt.get().getIsDeleted())) {
            throw new RuntimeException("Company Details Not Found");
        }

        CompanyMaster cm = compOpt.get();
        cm.setIsUpdated(true);
        cm.setIsDeleted(true);
        cm.setLastUpdatedBy(loginId);
        cm.setLastUpdatedDate(new Date());
        companyMasterRepository.save(cm);

        result.put("Status", 200);
        result.put("msg", "Deleted");
        return result;
    }

    public Map<String, Object> activateCompany(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer compId = model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<CompanyMaster> compOpt = companyMasterRepository.findById(compId);
        if (compOpt.isEmpty() || (compOpt.get().getIsDeleted() != null && compOpt.get().getIsDeleted())) {
            throw new RuntimeException("Company Details Not Found");
        }

        CompanyMaster cm = compOpt.get();
        cm.setIsActive(true);
        cm.setIsUpdated(true);
        cm.setLastUpdatedBy(loginId);
        cm.setLastUpdatedDate(new Date());
        companyMasterRepository.save(cm);

        result.put("Status", 200);
        result.put("msg", "Activated");
        return result;
    }

    public Map<String, Object> deActivateCompany(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer compId = model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<CompanyMaster> compOpt = companyMasterRepository.findById(compId);
        if (compOpt.isEmpty() || (compOpt.get().getIsDeleted() != null && compOpt.get().getIsDeleted())) {
            throw new RuntimeException("Company Details Not Found");
        }

        CompanyMaster cm = compOpt.get();
        cm.setIsActive(false);
        cm.setIsUpdated(true);
        cm.setLastUpdatedBy(loginId);
        cm.setLastUpdatedDate(new Date());
        companyMasterRepository.save(cm);

        result.put("Status", 200);
        result.put("msg", "Deactivated");
        return result;
    }

    // =================== Legal Entity CRUD ===================

    public List<Map<String, Object>> getAllLegalEntity(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        return legalEntityMasterRepository.findAll().stream()
            .filter(le -> le.getIsDeleted() == null || !le.getIsDeleted())
            .map(le -> {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("LoginId", loginId);
                m.put("CompId", le.getCompId());

                String company = "";
                String companyCode = "";
                if (le.getCompId() != null) {
                    Optional<CompanyMaster> compOpt = companyMasterRepository.findById(le.getCompId());
                    if (compOpt.isPresent()) {
                        company = compOpt.get().getCompany() != null ? compOpt.get().getCompany() : "";
                        companyCode = compOpt.get().getCompanyCode() != null ? compOpt.get().getCompanyCode() : "";
                    }
                }
                m.put("Company", company);
                m.put("CompanyCode", companyCode);
                m.put("LEId", le.getLeId());
                m.put("LegalEntity", le.getLegalEntity());
                m.put("Description", le.getDescription());
                m.put("CompanyType", le.getCompanyType());
                m.put("Logo", le.getLogo());
                m.put("LogoWithAddress", le.getLogoWithAddress());
                m.put("WebAppLogo", le.getWebAppLogo());
                m.put("Website", le.getWebsite());
                m.put("CreatedBy", le.getCreatedBy());
                m.put("CreatedDate", le.getCreatedDate() != null ? "\\/Date(" + le.getCreatedDate().getTime() + ")\\/" : null);
                m.put("LastUpdatedBy", le.getLastUpdatedBy());
                m.put("LastUpdatedDate", le.getLastUpdatedDate() != null ? "\\/Date(" + le.getLastUpdatedDate().getTime() + ")\\/" : null);
                m.put("IsActive", le.getIsActive());
                m.put("IsUpdated", le.getIsUpdated());
                m.put("IsDeleted", le.getIsDeleted());
                return m;
            })
            .collect(Collectors.toList());
    }

    // =================== LegalEntity CRUD ===================

    public Map<String, Object> getLegalEntity(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer leId = model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(leId);
        if (leOpt.isEmpty() || (leOpt.get().getIsDeleted() != null && leOpt.get().getIsDeleted())) {
            throw new RuntimeException("Leagal Entity Details Not Found");
        }

        LegalEntityMaster le = leOpt.get();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("LoginId", loginId);
        m.put("CompId", le.getCompId());

        String company = "";
        String companyCode = "";
        if (le.getCompId() != null) {
            Optional<CompanyMaster> compOpt = companyMasterRepository.findById(le.getCompId());
            if (compOpt.isPresent()) {
                company = compOpt.get().getCompany() != null ? compOpt.get().getCompany() : "";
                companyCode = compOpt.get().getCompanyCode() != null ? compOpt.get().getCompanyCode() : "";
            }
        }
        m.put("Company", company);
        m.put("CompanyCode", companyCode);
        m.put("LEId", le.getLeId());
        m.put("LegalEntity", le.getLegalEntity());
        m.put("Description", le.getDescription());
        m.put("CompanyType", le.getCompanyType());
        m.put("Logo", le.getLogo());
        m.put("LogoWithAddress", le.getLogoWithAddress());
        m.put("WebAppLogo", le.getWebAppLogo());
        m.put("Website", le.getWebsite());
        m.put("CreatedBy", le.getCreatedBy());
        m.put("CreatedDate", le.getCreatedDate() != null ? "\\/Date(" + le.getCreatedDate().getTime() + ")\\/" : null);
        m.put("LastUpdatedBy", le.getLastUpdatedBy());
        m.put("LastUpdatedDate", le.getLastUpdatedDate() != null ? "\\/Date(" + le.getLastUpdatedDate().getTime() + ")\\/" : null);
        m.put("IsActive", le.getIsActive());
        m.put("IsUpdated", le.getIsUpdated());
        m.put("IsDeleted", le.getIsDeleted());
        return m;
    }

    public Map<String, Object> addLegalEntity(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        String legalEntity = model.get("LegalEntity") != null ? model.get("LegalEntity").toString() : "";

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        boolean exists = legalEntityMasterRepository.findAll().stream()
            .anyMatch(le -> le.getLegalEntity() != null && le.getLegalEntity().equalsIgnoreCase(legalEntity)
                && (le.getIsDeleted() == null || !le.getIsDeleted()));

        if (exists) {
            throw new RuntimeException("Leagal Entity Details Already Exists");
        }

        LegalEntityMaster lem = new LegalEntityMaster();
        lem.setCompId(model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : null);
        lem.setLegalEntity(legalEntity);
        lem.setDescription(model.get("Description") != null ? model.get("Description").toString() : null);
        lem.setCompanyType(model.get("CompanyType") != null ? model.get("CompanyType").toString() : null);
        lem.setLogo(model.get("LOGO") != null ? model.get("LOGO").toString() : null);
        lem.setLogoWithAddress(model.get("LOGOWITHADDRESS") != null ? model.get("LOGOWITHADDRESS").toString() : null);
        lem.setWebAppLogo(model.get("WEBAPPLOGO") != null ? model.get("WEBAPPLOGO").toString() : null);
        lem.setWebsite(model.get("Website") != null ? model.get("Website").toString() : null);
        lem.setCreatedBy(loginId);
        lem.setCreatedDate(new Date());
        lem.setLastUpdatedBy(loginId);
        lem.setLastUpdatedDate(new Date());
        lem.setIsActive(true);
        lem.setIsUpdated(false);
        lem.setIsDeleted(false);
        legalEntityMasterRepository.save(lem);

        result.put("Status", 200);
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateLegalEntity(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer leId = model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(leId);
        if (leOpt.isEmpty() || (leOpt.get().getIsDeleted() != null && leOpt.get().getIsDeleted())) {
            throw new RuntimeException("Leagal Entity Details Not Found");
        }

        LegalEntityMaster lem = leOpt.get();
        lem.setCompId(model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : lem.getCompId());
        lem.setLegalEntity(model.get("LegalEntity") != null ? model.get("LegalEntity").toString() : lem.getLegalEntity());
        lem.setDescription(model.get("Description") != null ? model.get("Description").toString() : lem.getDescription());
        lem.setCompanyType(model.get("CompanyType") != null ? model.get("CompanyType").toString() : lem.getCompanyType());
        lem.setLogo(model.get("LOGO") != null ? model.get("LOGO").toString() : lem.getLogo());
        lem.setLogoWithAddress(model.get("LOGOWITHADDRESS") != null ? model.get("LOGOWITHADDRESS").toString() : lem.getLogoWithAddress());
        lem.setWebAppLogo(model.get("WEBAPPLOGO") != null ? model.get("WEBAPPLOGO").toString() : lem.getWebAppLogo());
        lem.setWebsite(model.get("Website") != null ? model.get("Website").toString() : lem.getWebsite());
        lem.setLastUpdatedBy(loginId);
        lem.setLastUpdatedDate(new Date());
        lem.setIsUpdated(true);
        legalEntityMasterRepository.save(lem);

        result.put("Status", 200);
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteLegalEntity(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer leId = model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(leId);
        if (leOpt.isEmpty() || (leOpt.get().getIsDeleted() != null && leOpt.get().getIsDeleted())) {
            throw new RuntimeException("Leagal Entity Details Not Found");
        }

        LegalEntityMaster lem = leOpt.get();
        lem.setIsUpdated(true);
        lem.setIsDeleted(true);
        lem.setLastUpdatedBy(loginId);
        lem.setLastUpdatedDate(new Date());
        legalEntityMasterRepository.save(lem);

        result.put("Status", 200);
        result.put("msg", "Deleted");
        return result;
    }

    public Map<String, Object> activateLegalEntity(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer leId = model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(leId);
        if (leOpt.isEmpty() || (leOpt.get().getIsDeleted() != null && leOpt.get().getIsDeleted())) {
            throw new RuntimeException("Leagal Entity Details Not Found");
        }

        LegalEntityMaster lem = leOpt.get();
        lem.setIsActive(true);
        lem.setIsUpdated(true);
        lem.setLastUpdatedBy(loginId);
        lem.setLastUpdatedDate(new Date());
        legalEntityMasterRepository.save(lem);

        result.put("Status", 200);
        result.put("msg", "Activated");
        return result;
    }

    public Map<String, Object> deActivateLegalEntity(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer leId = model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(leId);
        if (leOpt.isEmpty() || (leOpt.get().getIsDeleted() != null && leOpt.get().getIsDeleted())) {
            throw new RuntimeException("Leagal Entity Details Not Found");
        }

        LegalEntityMaster lem = leOpt.get();
        lem.setIsActive(false);
        lem.setIsUpdated(true);
        lem.setLastUpdatedBy(loginId);
        lem.setLastUpdatedDate(new Date());
        legalEntityMasterRepository.save(lem);

        result.put("Status", 200);
        result.put("msg", "Deactivated");
        return result;
    }

    // =================== BusinessUnit CRUD ===================

    public List<Map<String, Object>> getAllBusinessUnit(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        return businessUnitMasterRepository.findAll().stream()
            .filter(bu -> bu.getIsDeleted() == null || !bu.getIsDeleted())
            .map(bu -> {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("LoginId", loginId);
                m.put("CompId", bu.getCompId());

                String company = "";
                String companyCode = "";
                if (bu.getCompId() != null) {
                    Optional<CompanyMaster> compOpt = companyMasterRepository.findById(bu.getCompId());
                    if (compOpt.isPresent()) {
                        company = compOpt.get().getCompany() != null ? compOpt.get().getCompany() : "";
                        companyCode = compOpt.get().getCompanyCode() != null ? compOpt.get().getCompanyCode() : "";
                    }
                }
                m.put("Company", company);
                m.put("CompanyCode", companyCode);
                m.put("LEId", bu.getLeId());

                String legalEntity = "";
                if (bu.getLeId() != null) {
                    Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(bu.getLeId());
                    if (leOpt.isPresent()) {
                        legalEntity = leOpt.get().getLegalEntity() != null ? leOpt.get().getLegalEntity() : "";
                    }
                }
                m.put("LegalEntity", legalEntity);
                m.put("BUId", bu.getBuId());
                m.put("BusinessUnit", bu.getBusinessUnit());
                m.put("Description", bu.getDescription());
                m.put("CreatedBy", bu.getCreatedBy());
                m.put("CreatedDate", bu.getCreatedDate() != null ? "\\/Date(" + bu.getCreatedDate().getTime() + ")\\/" : null);
                m.put("LastUpdatedBy", bu.getLastUpdatedBy());
                m.put("LastUpdatedDate", bu.getLastUpdatedDate() != null ? "\\/Date(" + bu.getLastUpdatedDate().getTime() + ")\\/" : null);
                m.put("IsActive", bu.getIsActive());
                m.put("IsUpdated", bu.getIsUpdated());
                m.put("IsDeleted", bu.getIsDeleted());
                return m;
            })
            .collect(Collectors.toList());
    }

    public Map<String, Object> getBusinessUnit(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer buId = model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(buId);
        if (buOpt.isEmpty() || (buOpt.get().getIsDeleted() != null && buOpt.get().getIsDeleted())) {
            throw new RuntimeException("Business Unit Details Not Found");
        }

        BusinessUnitMaster bu = buOpt.get();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("LoginId", loginId);
        m.put("CompId", bu.getCompId());

        String company = "";
        String companyCode = "";
        if (bu.getCompId() != null) {
            Optional<CompanyMaster> compOpt = companyMasterRepository.findById(bu.getCompId());
            if (compOpt.isPresent()) {
                company = compOpt.get().getCompany() != null ? compOpt.get().getCompany() : "";
                companyCode = compOpt.get().getCompanyCode() != null ? compOpt.get().getCompanyCode() : "";
            }
        }
        m.put("Company", company);
        m.put("CompanyCode", companyCode);
        m.put("LEId", bu.getLeId());

        String legalEntity = "";
        if (bu.getLeId() != null) {
            Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(bu.getLeId());
            if (leOpt.isPresent()) {
                legalEntity = leOpt.get().getLegalEntity() != null ? leOpt.get().getLegalEntity() : "";
            }
        }
        m.put("LegalEntity", legalEntity);
        m.put("BUId", bu.getBuId());
        m.put("BusinessUnit", bu.getBusinessUnit());
        m.put("Description", bu.getDescription());
        m.put("CreatedBy", bu.getCreatedBy());
        m.put("CreatedDate", bu.getCreatedDate() != null ? "\\/Date(" + bu.getCreatedDate().getTime() + ")\\/" : null);
        m.put("LastUpdatedBy", bu.getLastUpdatedBy());
        m.put("LastUpdatedDate", bu.getLastUpdatedDate() != null ? "\\/Date(" + bu.getLastUpdatedDate().getTime() + ")\\/" : null);
        m.put("IsActive", bu.getIsActive());
        m.put("IsUpdated", bu.getIsUpdated());
        m.put("IsDeleted", bu.getIsDeleted());
        return m;
    }

    public Map<String, Object> addBusinessUnit(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        String businessUnitRaw = model.get("BusinessUnit") != null ? model.get("BusinessUnit").toString() : "";
        if (businessUnitRaw.isEmpty()) {
            businessUnitRaw = model.get("Businessunit") != null ? model.get("Businessunit").toString() : "";
        }
        final String businessUnit = businessUnitRaw;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        boolean exists = businessUnitMasterRepository.findAll().stream()
            .anyMatch(bu -> bu.getBusinessUnit() != null && bu.getBusinessUnit().equalsIgnoreCase(businessUnit)
                && (bu.getIsDeleted() == null || !bu.getIsDeleted()));

        if (exists) {
            throw new RuntimeException("Business Unit Details Already Exists");
        }

        BusinessUnitMaster bum = new BusinessUnitMaster();
        bum.setCompId(model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : null);
        bum.setLeId(model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : null);
        bum.setBusinessUnit(businessUnit);
        bum.setDescription(model.get("Description") != null ? model.get("Description").toString() : null);
        bum.setCreatedBy(loginId);
        bum.setCreatedDate(new Date());
        bum.setLastUpdatedBy(loginId);
        bum.setLastUpdatedDate(new Date());
        bum.setIsActive(true);
        bum.setIsUpdated(false);
        bum.setIsDeleted(false);
        businessUnitMasterRepository.save(bum);

        result.put("Status", 200);
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateBusinessUnit(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer buId = model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(buId);
        if (buOpt.isEmpty() || (buOpt.get().getIsDeleted() != null && buOpt.get().getIsDeleted())) {
            throw new RuntimeException("Business Unit Details Not Found");
        }

        BusinessUnitMaster bum = buOpt.get();
        bum.setCompId(model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : bum.getCompId());
        bum.setLeId(model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : bum.getLeId());
        bum.setBusinessUnit(model.get("BusinessUnit") != null ? model.get("BusinessUnit").toString() : bum.getBusinessUnit());
        bum.setDescription(model.get("Description") != null ? model.get("Description").toString() : bum.getDescription());
        bum.setLastUpdatedBy(loginId);
        bum.setLastUpdatedDate(new Date());
        bum.setIsUpdated(true);
        businessUnitMasterRepository.save(bum);

        result.put("Status", 200);
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteBusinessUnit(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer buId = model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(buId);
        if (buOpt.isEmpty() || (buOpt.get().getIsDeleted() != null && buOpt.get().getIsDeleted())) {
            throw new RuntimeException("Business Unit Details Not Found");
        }

        BusinessUnitMaster bum = buOpt.get();
        bum.setIsUpdated(true);
        bum.setIsDeleted(true);
        bum.setLastUpdatedBy(loginId);
        bum.setLastUpdatedDate(new Date());
        businessUnitMasterRepository.save(bum);

        result.put("Status", 200);
        result.put("msg", "Deleted");
        return result;
    }

    public Map<String, Object> activateBusinessUnit(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer buId = model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(buId);
        if (buOpt.isEmpty() || (buOpt.get().getIsDeleted() != null && buOpt.get().getIsDeleted())) {
            throw new RuntimeException("Business Unit Details Not Found");
        }

        BusinessUnitMaster bum = buOpt.get();
        bum.setIsActive(true);
        bum.setIsUpdated(true);
        bum.setLastUpdatedBy(loginId);
        bum.setLastUpdatedDate(new Date());
        businessUnitMasterRepository.save(bum);

        result.put("Status", 200);
        result.put("msg", "Activated");
        return result;
    }

    public Map<String, Object> deActivateBusinessUnit(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer buId = model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(buId);
        if (buOpt.isEmpty() || (buOpt.get().getIsDeleted() != null && buOpt.get().getIsDeleted())) {
            throw new RuntimeException("Business Unit Details Not Found");
        }

        BusinessUnitMaster bum = buOpt.get();
        bum.setIsActive(false);
        bum.setIsUpdated(true);
        bum.setLastUpdatedBy(loginId);
        bum.setLastUpdatedDate(new Date());
        businessUnitMasterRepository.save(bum);

        result.put("Status", 200);
        result.put("msg", "Deactivated");
        return result;
    }

    public List<Map<String, Object>> getAllLocationBE(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        return locationMasterRepository.findAll().stream()
            .filter(loc -> loc.getIsDeleted() == null || !loc.getIsDeleted())
            .map(loc -> {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("LoginId", loginId);
                m.put("CompId", loc.getCompId());

                String company = "";
                String companyCode = "";
                if (loc.getCompId() != null) {
                    Optional<CompanyMaster> compOpt = companyMasterRepository.findById(loc.getCompId());
                    if (compOpt.isPresent()) {
                        company = compOpt.get().getCompany() != null ? compOpt.get().getCompany() : "";
                        companyCode = compOpt.get().getCompanyCode() != null ? compOpt.get().getCompanyCode() : "";
                    }
                }
                m.put("Company", company);
                m.put("CompanyCode", companyCode);
                m.put("LEId", loc.getLeId());

                String legalEntity = "";
                if (loc.getLeId() != null) {
                    Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(loc.getLeId());
                    if (leOpt.isPresent()) {
                        legalEntity = leOpt.get().getLegalEntity() != null ? leOpt.get().getLegalEntity() : "";
                    }
                }
                m.put("LegalEntity", legalEntity);
                m.put("BUId", loc.getBuId());

                String businessUnit = "";
                if (loc.getBuId() != null) {
                    Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(loc.getBuId());
                    if (buOpt.isPresent()) {
                        businessUnit = buOpt.get().getBusinessUnit() != null ? buOpt.get().getBusinessUnit() : "";
                    }
                }
                m.put("BusinessUnit", businessUnit);
                m.put("LocationId", loc.getLocationId());
                m.put("Location", loc.getLocation());
                m.put("Description", loc.getDescription());
                m.put("LocationMap", loc.getLocationMap());
                m.put("Address", loc.getAddress());
                m.put("City", loc.getCity());
                m.put("State", loc.getState());
                m.put("Country", loc.getCountry());
                m.put("PostalCode", loc.getPostalCode());
                m.put("TimeZone", loc.getTimeZone());
                m.put("ProbationPeriod", loc.getProbationPeriod());
                m.put("WeeklyHoliday", loc.getWeeklyHoliday());
                m.put("CompanyRegNo", loc.getCompanyRegNo());
                m.put("DateofReg", loc.getDateofReg());
                m.put("PFNo", loc.getPfNo());
                m.put("ESINo", loc.getEsiNo());
                m.put("TANNo", loc.getTanNo());
                m.put("VATNo", loc.getVatNo());
                m.put("PANNo", loc.getPanNo());
                m.put("ServiceTaxNo", loc.getServiceTaxNo());
                m.put("GSTNo", loc.getGstNo());
                m.put("CreatedBy", loc.getCreatedBy());
                m.put("CreatedDate", loc.getCreatedDate() != null ? "\\/Date(" + loc.getCreatedDate().getTime() + ")\\/" : null);
                m.put("LastUpdatedBy", loc.getLastUpdatedBy());
                m.put("LastUpdatedDate", loc.getLastUpdatedDate() != null ? "\\/Date(" + loc.getLastUpdatedDate().getTime() + ")\\/" : null);
                m.put("IsActive", loc.getIsActive());
                m.put("IsUpdated", loc.getIsUpdated());
                m.put("IsDeleted", loc.getIsDeleted());
                return m;
            })
            .collect(Collectors.toList());
    }

    public Map<String, Object> addLocationBE(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        String location = model.get("Location") != null ? model.get("Location").toString() : "";

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        boolean exists = locationMasterRepository.findAll().stream()
            .anyMatch(loc -> loc.getLocation() != null && loc.getLocation().equals(location)
                && (loc.getIsDeleted() == null || !loc.getIsDeleted()));

        if (exists) {
            throw new RuntimeException("Location Details Already Exists");
        }

        LocationMaster lm = new LocationMaster();
        lm.setCompId(model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : null);
        lm.setLeId(model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : null);
        lm.setBuId(model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : null);
        lm.setLocation(location);
        lm.setDescription(model.get("Description") != null ? model.get("Description").toString() : null);
        lm.setLocationMap(model.get("LocationMap") != null ? model.get("LocationMap").toString() : null);
        lm.setAddress(model.get("Address") != null ? model.get("Address").toString() : null);
        lm.setCity(model.get("City") != null ? model.get("City").toString() : null);
        lm.setState(model.get("State") != null ? model.get("State").toString() : null);
        lm.setCountry(model.get("Country") != null ? model.get("Country").toString() : null);
        lm.setPostalCode(model.get("PostalCode") != null ? model.get("PostalCode").toString() : null);
        lm.setTimeZone(model.get("TimeZone") != null ? model.get("TimeZone").toString() : null);
        lm.setProbationPeriod(model.get("ProbationPeriod") != null ? Integer.parseInt(model.get("ProbationPeriod").toString()) : null);
        lm.setWeeklyHoliday(model.get("WeeklyHoliday") != null ? model.get("WeeklyHoliday").toString() : null);
        lm.setCompanyRegNo(model.get("CompanyRegNo") != null ? model.get("CompanyRegNo").toString() : null);
        lm.setDateofReg(model.get("DateofReg") != null ? model.get("DateofReg").toString() : null);
        lm.setPfNo(model.get("PFNo") != null ? model.get("PFNo").toString() : null);
        lm.setEsiNo(model.get("ESINo") != null ? model.get("ESINo").toString() : null);
        lm.setTanNo(model.get("TANNo") != null ? model.get("TANNo").toString() : null);
        lm.setVatNo(model.get("VATNo") != null ? model.get("VATNo").toString() : null);
        lm.setPanNo(model.get("PANNo") != null ? model.get("PANNo").toString() : null);
        lm.setServiceTaxNo(model.get("ServiceTaxNo") != null ? model.get("ServiceTaxNo").toString() : null);
        lm.setGstNo(model.get("GSTNo") != null ? model.get("GSTNo").toString() : null);
        lm.setCreatedBy(loginId);
        lm.setCreatedDate(new Date());
        lm.setLastUpdatedBy(loginId);
        lm.setLastUpdatedDate(new Date());
        lm.setIsActive(true);
        lm.setIsUpdated(false);
        lm.setIsDeleted(false);
        locationMasterRepository.save(lm);

        result.put("Status", 200);
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateLocationBE(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer locationId = model.get("LocationId") != null ? Integer.parseInt(model.get("LocationId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        Optional<LocationMaster> locOpt = locationMasterRepository.findById(locationId);
        if (locOpt.isEmpty()) {
            throw new RuntimeException("Location Details Not Found");
        }

        LocationMaster lm = locOpt.get();
        lm.setCompId(model.get("CompId") != null ? Integer.parseInt(model.get("CompId").toString()) : null);
        lm.setLeId(model.get("LEId") != null ? Integer.parseInt(model.get("LEId").toString()) : null);
        lm.setBuId(model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : null);
        lm.setLocation(model.get("Location") != null ? model.get("Location").toString() : null);
        lm.setDescription(model.get("Description") != null ? model.get("Description").toString() : null);
        lm.setLocationMap(model.get("LocationMap") != null ? model.get("LocationMap").toString() : null);
        lm.setAddress(model.get("Address") != null ? model.get("Address").toString() : null);
        lm.setCity(model.get("City") != null ? model.get("City").toString() : null);
        lm.setState(model.get("State") != null ? model.get("State").toString() : null);
        lm.setCountry(model.get("Country") != null ? model.get("Country").toString() : null);
        lm.setPostalCode(model.get("PostalCode") != null ? model.get("PostalCode").toString() : null);
        lm.setTimeZone(model.get("TimeZone") != null ? model.get("TimeZone").toString() : null);
        lm.setProbationPeriod(model.get("ProbationPeriod") != null ? Integer.parseInt(model.get("ProbationPeriod").toString()) : null);
        lm.setWeeklyHoliday(model.get("WeeklyHoliday") != null ? model.get("WeeklyHoliday").toString() : null);
        lm.setCompanyRegNo(model.get("CompanyRegNo") != null ? model.get("CompanyRegNo").toString() : null);
        lm.setDateofReg(model.get("DateofReg") != null ? model.get("DateofReg").toString() : null);
        lm.setPfNo(model.get("PFNo") != null ? model.get("PFNo").toString() : null);
        lm.setEsiNo(model.get("ESINo") != null ? model.get("ESINo").toString() : null);
        lm.setTanNo(model.get("TANNo") != null ? model.get("TANNo").toString() : null);
        lm.setVatNo(model.get("VATNo") != null ? model.get("VATNo").toString() : null);
        lm.setPanNo(model.get("PANNo") != null ? model.get("PANNo").toString() : null);
        lm.setServiceTaxNo(model.get("ServiceTaxNo") != null ? model.get("ServiceTaxNo").toString() : null);
        lm.setGstNo(model.get("GSTNo") != null ? model.get("GSTNo").toString() : null);
        lm.setLastUpdatedBy(loginId);
        lm.setLastUpdatedDate(new Date());
        lm.setIsUpdated(true);
        locationMasterRepository.save(lm);

        result.put("Status", 200);
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteLocationBE(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer locationId = model.get("LocationId") != null ? Integer.parseInt(model.get("LocationId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LocationMaster> locOpt = locationMasterRepository.findById(locationId);
        if (locOpt.isEmpty()) {
            throw new RuntimeException("Location Details Not Found");
        }

        LocationMaster lm = locOpt.get();
        lm.setIsUpdated(true);
        lm.setIsDeleted(true);
        lm.setLastUpdatedBy(loginId);
        lm.setLastUpdatedDate(new Date());
        locationMasterRepository.save(lm);

        result.put("Status", 200);
        result.put("msg", "Deleted");
        return result;
    }

    public Map<String, Object> activateLocationBE(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer locationId = model.get("LocationId") != null ? Integer.parseInt(model.get("LocationId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LocationMaster> locOpt = locationMasterRepository.findById(locationId);
        if (locOpt.isEmpty()) {
            throw new RuntimeException("Location Details Not Found");
        }

        LocationMaster lm = locOpt.get();
        lm.setIsActive(true);
        lm.setIsUpdated(true);
        lm.setLastUpdatedBy(loginId);
        lm.setLastUpdatedDate(new Date());
        locationMasterRepository.save(lm);

        result.put("Status", 200);
        result.put("msg", "Activated");
        return result;
    }

    public Map<String, Object> deActivateLocationBE(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer locationId = model.get("LocationId") != null ? Integer.parseInt(model.get("LocationId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<LocationMaster> locOpt = locationMasterRepository.findById(locationId);
        if (locOpt.isEmpty()) {
            throw new RuntimeException("Location Details Not Found");
        }

        LocationMaster lm = locOpt.get();
        lm.setIsActive(false);
        lm.setIsUpdated(true);
        lm.setLastUpdatedBy(loginId);
        lm.setLastUpdatedDate(new Date());
        locationMasterRepository.save(lm);

        result.put("Status", 200);
        result.put("msg", "Deactivated");
        return result;
    }

    private String extractTime(String fullTime) {
        if (fullTime == null) return "09:30";
        if (fullTime.contains(".")) {
            String[] parts = fullTime.split("\\.");
            if (parts.length > 0 && parts[0].length() >= 5) {
                return parts[0].substring(0, 5);
            }
        }
        if (fullTime.length() > 5) {
            return fullTime.substring(0, 5);
        }
        return fullTime;
    }

    public List<DDReporterListViewModel> getDDReporterList(DDReporterListViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer leId = (model != null && model.getLEId() != null && model.getLEId() != 0) ? model.getLEId() : 0;
        Integer buId = (model != null && model.getBUId() != null && model.getBUId() != 0) ? model.getBUId() : 0;
        Integer locationId = (model != null && model.getLocationId() != null && model.getLocationId() != 0) ? model.getLocationId() : 0;
        Integer deptId = (model != null && model.getDeptId() != null && model.getDeptId() != 0) ? model.getDeptId() : 0;
        Integer designationId = (model != null && model.getDesignationId() != null && model.getDesignationId() != 0) ? model.getDesignationId() : 0;

        // Get active employees with ReportId != null
        List<EmployeeMaster> employees = employeeMasterRepository.findAll().stream()
            .filter(e -> "ACTIVE".equalsIgnoreCase(e.getEmpStatus()) && Boolean.TRUE.equals(e.getIsActive()) && Boolean.FALSE.equals(e.getIsDeleted()) && e.getReportId() != null)
            .collect(Collectors.toList());

        // Group by ReportId, take first employee per group
        Map<Integer, EmployeeMaster> reporterMap = new java.util.LinkedHashMap<>();
        for (EmployeeMaster e : employees) {
            reporterMap.putIfAbsent(e.getReportId(), e);
        }

        // Get reporter details (employees whose EmpId is in the ReportId set)
        List<Integer> reporterIds = new ArrayList<>(reporterMap.keySet());
        List<EmployeeMaster> reporters = employeeMasterRepository.findAllById(reporterIds);

        // Map to ViewModel
        List<DDReporterListViewModel> result = reporters.stream().map(r -> {
            DDReporterListViewModel vm = new DDReporterListViewModel();
            vm.setCompId(r.getCompId());
            vm.setLEId(r.getLeId());
            vm.setBUId(r.getBuId());
            vm.setLocationId(r.getLocationId());
            vm.setDeptId(r.getCategoryId());
            vm.setDesignationId(r.getDesignationId());
            vm.setReporterId(r.getEmpId());
            vm.setReporterName(
                (r.getFirstName() != null ? r.getFirstName() : "") + " " +
                (r.getMiddleName() != null ? r.getMiddleName() : "") + " " +
                (r.getLastName() != null ? r.getLastName() : "")
            );
            vm.setReporterCode(r.getEmpCode());
            vm.setLoginId(loginId);
            return vm;
        }).collect(Collectors.toList());

        // Apply filters
        if (leId != 0) {
            result = result.stream().filter(x -> leId.equals(x.getLEId())).collect(Collectors.toList());
        }
        if (buId != 0) {
            result = result.stream().filter(x -> buId.equals(x.getBUId())).collect(Collectors.toList());
        }
        if (locationId != 0) {
            result = result.stream().filter(x -> locationId.equals(x.getLocationId())).collect(Collectors.toList());
        }
        if (deptId != 0) {
            result = result.stream().filter(x -> deptId.equals(x.getDeptId())).collect(Collectors.toList());
        }
        if (designationId != 0) {
            result = result.stream().filter(x -> designationId.equals(x.getDesignationId())).collect(Collectors.toList());
        }

        if (loginId != 0) {
            if (result == null || result.isEmpty()) {
                throw new RuntimeException("Reporter Details Not Found");
            }
            return result;
        }

        return result;
    }

    public EmpProbationTrackingHistoryListViewModel getAllEmpProbationTrackingHistory(EmpProbationTrackingHistoryViewModel model) {
        if (model.getLoginId() == null || model.getLoginId() == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        Integer loginId = model.getLoginId();
        Integer leId = model.getLeId();

        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(loginId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee with ID " + loginId + " not found");
        }

        Integer deptId = empOpt.get().getCategoryId();

        List<EmpProbationTrackingHistory> allProbationRecords;
        try {
            allProbationRecords = empProbationTrackingHistoryRepository
                .findByIsActiveAndIsDeletedOrderByCreatedDateDesc(true, false);
        } catch (Exception e) {
            // Table doesn't exist in UAT - return empty result
            EmpProbationTrackingHistoryListViewModel emptyResult = new EmpProbationTrackingHistoryListViewModel();
            emptyResult.setPendingProbationList(new ArrayList<>());
            emptyResult.setProbationHistoryList(new ArrayList<>());
            return emptyResult;
        }

        List<Map<String, Object>> matchedRecords = new ArrayList<>();

        for (EmpProbationTrackingHistory pt : allProbationRecords) {
            if (pt.getEmpId() == null) continue;

            Optional<EmployeeMaster> empMatch = employeeMasterRepository.findById(pt.getEmpId());
            if (empMatch.isEmpty() || !Boolean.TRUE.equals(empMatch.get().getIsActive()) ||
                Boolean.TRUE.equals(empMatch.get().getIsDeleted()) || !"ACTIVE".equalsIgnoreCase(empMatch.get().getEmpStatus())) {
                continue;
            }

            EmployeeMaster em = empMatch.get();

            if (leId != null && leId > 0 && !leId.equals(em.getLeId())) continue;

            if (model.getBuId() != null && model.getBuId() > 0 && !model.getBuId().equals(em.getBuId())) continue;
            if (model.getLocId() != null && model.getLocId() > 0 && !model.getLocId().equals(em.getLocationId())) continue;
            if (model.getDeptId() != null && model.getDeptId() > 0 && !model.getDeptId().equals(em.getCategoryId())) continue;
            if (model.getDesignationId() != null && model.getDesignationId() > 0 && !model.getDesignationId().equals(em.getDesignationId())) continue;
            if (model.getReporterId() != null && model.getReporterId() > 0 && !model.getReporterId().equals(em.getReportId())) continue;
            if (model.getEmpId() != null && model.getEmpId() > 0 && !model.getEmpId().equals(em.getEmpId())) continue;

            Integer HR_DEPT_ID = 1;
            if (!HR_DEPT_ID.equals(deptId) && !loginId.equals(pt.getReportId())) continue;

            Map<String, Object> record = new HashMap<>();
            record.put("LoginId", pt.getEmpId());
            record.put("EmpProbationId", pt.getEmpProbationId());
            record.put("EmpId", pt.getEmpId());
            record.put("EmpName", (em.getFirstName() != null ? em.getFirstName() : "") + " " + 
                (em.getMiddleName() != null ? em.getMiddleName() : "") + " " + 
                (em.getLastName() != null ? em.getLastName() : ""));
            record.put("EmpCode", em.getEmpCode());
            record.put("JoiningDate", pt.getJoiningDate());
            record.put("ProbationDays", pt.getProbationDays());
            record.put("ProbationEndDate", pt.getProbationEndDate());
            record.put("ReportId", pt.getReportId());
            record.put("ReportCode", pt.getReportCode());
            record.put("IsProbation", pt.getIsProbation());
            record.put("IsPermanent", pt.getIsPermanent());
            record.put("IsContract", pt.getIsContract());
            record.put("IsConsultant", pt.getIsConsultant());
            record.put("ConfirmDate", pt.getConfirmDate());
            record.put("ConfirmBy", pt.getConfirmBy());
            record.put("Remarks", pt.getRemarks());
            record.put("CreatedBy", pt.getCreatedBy());
            record.put("CreatedDate", pt.getCreatedDate());
            record.put("LastUpdatedBy", pt.getLastUpdatedBy());
            record.put("LastUpdatedDate", pt.getLastUpdatedDate());
            record.put("IsActive", pt.getIsActive());
            record.put("IsUpdated", pt.getIsUpdated());
            record.put("IsDeleted", pt.getIsDeleted());

            matchedRecords.add(record);
        }

        List<Map<String, Object>> pendingList = matchedRecords.stream()
            .filter(x -> Boolean.TRUE.equals(x.get("IsProbation")))
            .collect(Collectors.toList());

        List<Map<String, Object>> historyList = matchedRecords.stream()
            .filter(x -> !Boolean.TRUE.equals(x.get("IsProbation")))
            .collect(Collectors.toList());

        EmpProbationTrackingHistoryListViewModel result = new EmpProbationTrackingHistoryListViewModel();
        result.setPendingProbationList(convertToViewModelList(pendingList));
        result.setProbationHistoryList(convertToViewModelList(historyList));

        return result;
    }

    private List<EmpProbationTrackingHistoryViewModel> convertToViewModelList(List<Map<String, Object>> records) {
        return records.stream().map(record -> {
            EmpProbationTrackingHistoryViewModel vm = new EmpProbationTrackingHistoryViewModel();
            vm.setLoginId((Integer) record.get("LoginId"));
            vm.setEmpProbationId((Integer) record.get("EmpProbationId"));
            vm.setEmpId((Integer) record.get("EmpId"));
            vm.setEmpName((String) record.get("EmpName"));
            vm.setEmpCode((String) record.get("EmpCode"));
            vm.setJoiningDate(record.get("JoiningDate"));
            vm.setProbationDays((Integer) record.get("ProbationDays"));
            vm.setProbationEndDate(record.get("ProbationEndDate"));
            vm.setReportId((Integer) record.get("ReportId"));
            vm.setReportCode((String) record.get("ReportCode"));
            vm.setIsProbation((Boolean) record.get("IsProbation"));
            vm.setIsPermanent((Boolean) record.get("IsPermanent"));
            vm.setIsContract((Boolean) record.get("IsContract"));
            vm.setIsConsultant((Boolean) record.get("IsConsultant"));
            vm.setConfirmDate(record.get("ConfirmDate"));
            vm.setConfirmBy((Integer) record.get("ConfirmBy"));
            vm.setRemarks((String) record.get("Remarks"));
            vm.setCreatedBy((Integer) record.get("CreatedBy"));
            vm.setCreatedDate(record.get("CreatedDate"));
            vm.setLastUpdatedBy((Integer) record.get("LastUpdatedBy"));
            vm.setLastUpdatedDate(record.get("LastUpdatedDate"));
            vm.setIsActive((Boolean) record.get("IsActive"));
            vm.setIsUpdated((Boolean) record.get("IsUpdated"));
            vm.setIsDeleted((Boolean) record.get("IsDeleted"));
            return vm;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getAllEmployeeLogHistory(EmployeeMasterViewModel model) {
        if (model.getLoginId() == null || model.getLoginId() == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        Integer loginId = model.getLoginId();
        Integer leId = model.getLeId();

        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(loginId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee with ID " + loginId + " not found");
        }

        Integer deptId = empOpt.get().getCategoryId();
        Integer HR_DEPT_ID = 1;

        List<EmployeeMasterLog> allRecords = employeeMasterLogRepository.findByIsActiveAndIsDeleted(true, false);

        List<EmployeeMasterLog> filteredRecords = new ArrayList<>();
        for (EmployeeMasterLog record : allRecords) {
            if (leId != null && leId > 0 && !leId.equals(record.getLeId())) continue;
            if (model.getBuId() != null && model.getBuId() > 0 && !model.getBuId().equals(record.getBuId())) continue;
            if (model.getLocationId() != null && model.getLocationId() > 0 && !model.getLocationId().equals(record.getLocationId())) continue;
            if (model.getCategoryId() != null && model.getCategoryId() > 0 && !model.getCategoryId().equals(record.getCategoryId())) continue;
            if (model.getDesignationId() != null && model.getDesignationId() > 0 && !model.getDesignationId().equals(record.getDesignationId())) continue;
            if (model.getReportId() != null && model.getReportId() > 0 && !model.getReportId().equals(record.getReportId())) continue;
            if (model.getEmpId() != null && model.getEmpId() > 0 && !model.getEmpId().equals(record.getEmpId())) continue;

            if (!HR_DEPT_ID.equals(deptId) && !loginId.equals(record.getReportId())) continue;

            filteredRecords.add(record);
        }

        if (filteredRecords.isEmpty()) {
            return new ArrayList<>();
        }

        filteredRecords.sort((a, b) -> {
            if (a.getCreatedDate() == null && b.getCreatedDate() == null) return 0;
            if (a.getCreatedDate() == null) return 1;
            if (b.getCreatedDate() == null) return -1;
            return b.getCreatedDate().compareTo(a.getCreatedDate());
        });

        List<Integer> compIds = filteredRecords.stream().map(EmployeeMasterLog::getCompId)
            .filter(Objects::nonNull).distinct().collect(Collectors.toList());
        List<Integer> leIds = filteredRecords.stream().map(EmployeeMasterLog::getLeId)
            .filter(Objects::nonNull).filter(id -> id > 0).distinct().collect(Collectors.toList());
        List<Integer> buIds = filteredRecords.stream().map(EmployeeMasterLog::getBuId)
            .filter(Objects::nonNull).filter(id -> id > 0).distinct().collect(Collectors.toList());
        List<Integer> locationIds = filteredRecords.stream().map(EmployeeMasterLog::getLocationId)
            .filter(Objects::nonNull).filter(id -> id > 0).distinct().collect(Collectors.toList());
        List<Integer> reportIds = filteredRecords.stream().map(EmployeeMasterLog::getReportId)
            .filter(Objects::nonNull).filter(id -> id > 0).distinct().collect(Collectors.toList());
        List<Integer> salutationIds = filteredRecords.stream().map(EmployeeMasterLog::getSalutation)
            .filter(Objects::nonNull).filter(id -> id > 0).distinct().collect(Collectors.toList());
        List<Integer> empTypeIds = filteredRecords.stream().map(EmployeeMasterLog::getEmpType)
            .filter(Objects::nonNull).filter(id -> id > 0).distinct().collect(Collectors.toList());

        Map<Integer, String> companies = compIds.isEmpty() ? new HashMap<>() :
            companyMasterRepository.findAllById(compIds).stream()
                .collect(Collectors.toMap(c -> c.getCompId(), c -> c.getCompany()));

        Map<Integer, String> legalEntities = leIds.isEmpty() ? new HashMap<>() :
            legalEntityMasterRepository.findAllById(leIds).stream()
                .collect(Collectors.toMap(l -> l.getLeId(), l -> l.getLegalEntity()));

        Map<Integer, String> businessUnits = buIds.isEmpty() ? new HashMap<>() :
            businessUnitMasterRepository.findAllById(buIds).stream()
                .collect(Collectors.toMap(b -> b.getBuId(), b -> b.getBusinessUnit()));

        Map<Integer, String> locations = locationIds.isEmpty() ? new HashMap<>() :
            locationMasterRepository.findAllById(locationIds).stream()
                .collect(Collectors.toMap(l -> l.getLocationId(), l -> l.getLocation()));

        Map<Integer, Map<String, String>> employees = reportIds.isEmpty() ? new HashMap<>() :
            employeeMasterRepository.findAllById(reportIds).stream()
                .collect(Collectors.toMap(e -> e.getEmpId(), e -> {
                    Map<String, String> map = new HashMap<>();
                    map.put("FirstName", e.getFirstName() != null ? e.getFirstName() : "");
                    map.put("MiddleName", e.getMiddleName() != null ? e.getMiddleName() : "");
                    map.put("LastName", e.getLastName() != null ? e.getLastName() : "");
                    map.put("EmpCode", e.getEmpCode() != null ? e.getEmpCode() : "");
                    return map;
                }));

        Map<Integer, String> salutations = salutationIds.isEmpty() ? new HashMap<>() :
            salutationMasterRepository.findAllById(salutationIds).stream()
                .collect(Collectors.toMap(s -> s.getSalutationId(), s -> s.getSalutation()));

        Map<Integer, String> empTypes = empTypeIds.isEmpty() ? new HashMap<>() :
            empTypeMasterRepository.findAllById(empTypeIds).stream()
                .collect(Collectors.toMap(e -> e.getEmpTypId(), e -> e.getEmpType()));

        List<Map<String, Object>> result = new ArrayList<>();
        for (EmployeeMasterLog record : filteredRecords) {
            Map<String, Object> vm = new HashMap<>();
            vm.put("EmpId", record.getEmpId());
            vm.put("OldEmp_ID", record.getOldEmp_ID());
            vm.put("CompId", record.getCompId() != null ? record.getCompId() : 0);
            vm.put("Company", (record.getCompId() != null && companies.containsKey(record.getCompId())) ? companies.get(record.getCompId()) : "");
            vm.put("LEId", record.getLeId() != null && record.getLeId() > 0 ? record.getLeId() : 0);
            vm.put("LegalEntity", (record.getLeId() != null && record.getLeId() > 0 && legalEntities.containsKey(record.getLeId())) ? legalEntities.get(record.getLeId()) : "");
            vm.put("BUId", record.getBuId() != null && record.getBuId() > 0 ? record.getBuId() : 0);
            vm.put("BusinessUnit", (record.getBuId() != null && record.getBuId() > 0 && businessUnits.containsKey(record.getBuId())) ? businessUnits.get(record.getBuId()) : "");
            vm.put("LocationId", record.getLocationId() != null && record.getLocationId() > 0 ? record.getLocationId() : 0);
            vm.put("Location", (record.getLocationId() != null && record.getLocationId() > 0 && locations.containsKey(record.getLocationId())) ? locations.get(record.getLocationId()) : "");
            vm.put("CategoryId", record.getCategoryId() != null ? record.getCategoryId() : 0);
            vm.put("DeptId", record.getCategoryId() != null ? record.getCategoryId() : 0);
            vm.put("DeptName", record.getDeptName() != null ? record.getDeptName() : "");
            vm.put("DesignationId", record.getDesignationId() != null ? record.getDesignationId() : 0);
            vm.put("Designation", record.getDesignationName() != null ? record.getDesignationName() : "");
            vm.put("ReportId", record.getReportId() != null ? record.getReportId() : 0);
            vm.put("ApproverId", record.getReportId() != null ? record.getReportId() : 0);
            vm.put("AuthorisedEntity", record.getAuthorisedEntity() != null ? record.getAuthorisedEntity() : "");
            vm.put("EmpCode", record.getEmpCode() != null ? record.getEmpCode() : "");
            vm.put("UserName", record.getUserName() != null ? record.getUserName() : "");
            vm.put("Photo", processPhotoPath(record.getPhoto()));
            vm.put("SalutationId", record.getSalutation() != null ? record.getSalutation() : 0);
            vm.put("Salutation", (record.getSalutation() != null && record.getSalutation() > 0 && salutations.containsKey(record.getSalutation())) ? salutations.get(record.getSalutation()) : "");
            vm.put("FirstName", record.getFirstName() != null ? record.getFirstName() : "");
            vm.put("MiddleName", record.getMiddleName() != null ? record.getMiddleName() : "");
            vm.put("LastName", record.getLastName() != null ? record.getLastName() : "");
            vm.put("DOB", record.getDob());
            vm.put("MobileNo", record.getMobileNo() != null ? record.getMobileNo() : "");
            vm.put("EmailId", record.getEmailId() != null ? record.getEmailId() : "");
            vm.put("BloodGroup", record.getBloodGroup() != null ? record.getBloodGroup() : "");
            vm.put("MaritalStatus", record.getMaritalStatus() != null ? record.getMaritalStatus() : "");
            vm.put("Gender", record.getGender() != null ? record.getGender() : "");
            vm.put("JoiningDate", record.getJoiningDate());
            vm.put("EndDate", record.getEndDate());
            vm.put("EmpStatus", record.getEmpStatus() != null ? record.getEmpStatus().toUpperCase() : "");
            vm.put("Reason", record.getReason() != null ? record.getReason() : "");
            vm.put("EmpTypeId", record.getEmpType() != null ? record.getEmpType() : 0);
            vm.put("EmpType", (record.getEmpType() != null && record.getEmpType() > 0 && empTypes.containsKey(record.getEmpType())) ? empTypes.get(record.getEmpType()) : "");
            vm.put("CEndDate", record.getCEndDate());

            String approver = "";
            if (record.getReportId() != null && record.getReportId() > 0 && employees.containsKey(record.getReportId())) {
                Map<String, String> approverInfo = employees.get(record.getReportId());
                approver = (approverInfo.get("FirstName") + " " + approverInfo.get("MiddleName") + " " + approverInfo.get("LastName") + " - " + approverInfo.get("EmpCode")).trim();
                approver = approver.replaceAll("\\s+", " ");
            }
            vm.put("Approver", approver);

            vm.put("IsActive", Boolean.TRUE.equals(record.getIsActive()));
            vm.put("IsUpdated", Boolean.TRUE.equals(record.getIsUpdated()));
            vm.put("IsDeleted", Boolean.TRUE.equals(record.getIsDeleted()));
            vm.put("CreatedBy", record.getCreatedBy());
            vm.put("CreatedDate", record.getCreatedDate());
            vm.put("LastUpdatedBy", record.getLastUpdatedBy());
            vm.put("LastUpdatedDate", record.getLastUpdatedDate());

            result.add(vm);
        }

        return result;
    }

    private Map<String, Object> timeToTimeSpan(Date time, boolean isDuration) {
        if (time == null) return null;
        Calendar cal;
        if (isDuration) {
            // ActiveHrs is a duration stored as Date(millis) — interpret in UTC
            cal = Calendar.getInstance(TimeZone.getTimeZone("UTC"));
        } else {
            // LogInTime/LogOutTime are clock times — use local timezone
            cal = Calendar.getInstance();
        }
        cal.setTime(time);
        int hours = cal.get(Calendar.HOUR_OF_DAY);
        int minutes = cal.get(Calendar.MINUTE);
        int seconds = cal.get(Calendar.SECOND);
        int millis = cal.get(Calendar.MILLISECOND);
        long ticks = ((long) hours * 3600L + (long) minutes * 60L + seconds) * 1000L + millis;
        ticks *= 10000L;
        Map<String, Object> map = new LinkedHashMap<>();
        map.put("Days", 0);
        map.put("Hours", hours);
        map.put("Minutes", minutes);
        map.put("Seconds", seconds);
        map.put("Milliseconds", millis);
        map.put("Ticks", ticks);
        return map;
    }

    private String processPhotoPath(String photo) {
        if (photo == null || photo.isEmpty()) return "";
        if (photo.contains("Uploads")) {
            String[] parts = photo.split("Uploads", 2);
            if (parts.length > 1) return "Uploads" + parts[1];
        }
        return photo;
    }

    // ========== MISSING EMPLOYEE ENDPOINTS ==========

    public Map<String, Object> uploadSingleAttendance(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            String empCode = model.get("EmpCode") != null ? model.get("EmpCode").toString() : "";
            String dateStr = model.get("Date") != null ? model.get("Date").toString() : "";
            String time = model.get("Time") != null ? model.get("Time").toString() : "";
            String status = model.get("Status") != null ? model.get("Status").toString() : "Active";

            if (empCode.isEmpty()) {
                throw new RuntimeException("EmpCode is required");
            }
            if (dateStr.isEmpty()) {
                throw new RuntimeException("Date is required");
            }

            List<EmployeeMaster> empList = employeeMasterRepository.findByEmpCodeAndIsActiveAndIsDeleted(empCode, true, false);
            if (empList.isEmpty()) {
                throw new RuntimeException("Employee with EmpCode '" + empCode + "' not found");
            }

            TempManualAttendance temp = new TempManualAttendance();
            temp.setEmpCode(empCode);
            temp.setDate(dateStr);
            temp.setTime(time);
            temp.setStatus(status);
            tempManualAttendanceRepository.save(temp);

            long totalRecords = tempManualAttendanceRepository.count();

            SimpleDateFormat sdf;
            if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                sdf = new SimpleDateFormat("dd-MM-yyyy");
            } else {
                sdf = new SimpleDateFormat("yyyy-MM-dd");
            }
            Date date = sdf.parse(dateStr);

            ManualAttendance manualAtt = new ManualAttendance();
            manualAtt.setEmpCode(empCode);
            manualAtt.setDate(date);
            // Parse time string (e.g., "09:30" or "09:30:00") into a Date object for the Time column
            if (time != null && !time.isEmpty()) {
                String timeStr = time.trim();
                if (!timeStr.contains(":")) {
                    throw new RuntimeException("Invalid time format: " + timeStr);
                }
                // Pad to HH:mm:ss if needed
                String[] parts = timeStr.split(":");
                String paddedTime;
                if (parts.length == 2) {
                    paddedTime = String.format("%02d:%02d:00", Integer.parseInt(parts[0]), Integer.parseInt(parts[1]));
                } else if (parts.length == 3) {
                    paddedTime = String.format("%02d:%02d:%02d", Integer.parseInt(parts[0]), Integer.parseInt(parts[1]), Integer.parseInt(parts[2]));
                } else {
                    paddedTime = timeStr;
                }
                SimpleDateFormat timeSdf = new SimpleDateFormat("HH:mm:ss");
                Date timeDate = timeSdf.parse(paddedTime);
                manualAtt.setTime(timeDate);
            }
            manualAtt.setStatus(status);
            manualAtt.setRecordStatus(true);
            manualAtt.setCreatedBy(loginId);
            manualAtt.setCreatedDate(new Date());
            manualAtt.setIsActive(true);
            manualAtt.setIsUpdated(false);
            manualAtt.setIsDeleted(false);
            manualAttendanceRepository.save(manualAtt);

            tempManualAttendanceRepository.deleteAll();

            result.put("TotalRecords", (int) totalRecords);
            result.put("InsertedRecords", 1);
            result.put("FailedRecords", 0);
            result.put("Exceptions", new ArrayList<>());
            result.put("StatusCode", 200);
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error uploading attendance: " + e.getMessage());
        }
        return result;
    }

    @Transactional(rollbackFor = Exception.class)
    public UploadResult uploadMultiAttendance(List<UploadAttendanceSingleViewModel> model) {
        if (model == null || model.isEmpty()) {
            throw new RuntimeException("No attendance data provided");
        }

        Integer loginId = model.get(0).getLoginId();
        if (loginId == null || loginId <= 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        int totalRecords = 0;
        int insertedRecords = 0;
        int updatedRecords = 0;
        List<AttendanceException> exceptions = new ArrayList<>();

        for (UploadAttendanceSingleViewModel item : model) {
            String empCode = item.getEmpCode();
            if (empCode == null || empCode.trim().isEmpty()) continue;

            totalRecords++;

            List<EmployeeMaster> empList = employeeMasterRepository.findByEmpCodeAndIsActiveAndIsDeleted(empCode.trim(), true, false);
            if (empList.isEmpty()) {
                AttendanceException ex = new AttendanceException();
                ex.setEmpCode(empCode.trim());
                ex.setDate(item.getDate());
                ex.setTime(item.getTime());
                ex.setReason("Invalid Employee Code");
                exceptions.add(ex);
                continue;
            }

            String dateStr = item.getDate();
            if (dateStr == null || dateStr.isEmpty()) {
                AttendanceException ex = new AttendanceException();
                ex.setEmpCode(empCode.trim());
                ex.setDate(item.getDate());
                ex.setTime(item.getTime());
                ex.setReason("Invalid Date or Time format");
                exceptions.add(ex);
                continue;
            }

            String formattedDate = null;
            try {
                SimpleDateFormat sdf;
                if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                    sdf = new SimpleDateFormat("dd-MM-yyyy");
                } else {
                    sdf = new SimpleDateFormat("yyyy-MM-dd");
                }
                sdf.setLenient(false);
                Date parsed = sdf.parse(dateStr);
                SimpleDateFormat outputFormat = new SimpleDateFormat("yyyy-MM-dd");
                formattedDate = outputFormat.format(parsed);
            } catch (Exception e) {
                AttendanceException ex = new AttendanceException();
                ex.setEmpCode(empCode.trim());
                ex.setDate(item.getDate());
                ex.setTime(item.getTime());
                ex.setReason("Invalid Date or Time format");
                exceptions.add(ex);
                continue;
            }

            String formattedTime = null;
            String timeStr = item.getTime();
            if (timeStr != null && !timeStr.isEmpty()) {
                String[] parts = timeStr.split(":");
                if (parts.length < 2) {
                    AttendanceException ex = new AttendanceException();
                    ex.setEmpCode(empCode.trim());
                    ex.setDate(item.getDate());
                    ex.setTime(item.getTime());
                    ex.setReason("Invalid Date or Time format");
                    exceptions.add(ex);
                    continue;
                }
                String h = String.format("%02d", Integer.parseInt(parts[0]));
                String m = String.format("%02d", Integer.parseInt(parts[1]));
                String s = parts.length >= 3 ? String.format("%02d", Integer.parseInt(parts[2])) : "00";
                formattedTime = h + ":" + m + ":" + s;
            }

            Query findQuery = entityManager.createNativeQuery(
                "SELECT Id FROM ManualAttendance WHERE EmpCode = :empCode AND Date = CAST(:dateStr AS date)");
            findQuery.setParameter("empCode", empCode.trim());
            findQuery.setParameter("dateStr", formattedDate);
            List<?> existingIds = findQuery.getResultList();

            if (!existingIds.isEmpty()) {
                Integer existingId = (Integer) existingIds.get(0);
                if (formattedTime != null) {
                    entityManager.createNativeQuery(
                        "UPDATE ManualAttendance SET Time = CAST(:time AS time), Status = 'Active', LastUpdatedBy = :loginId, LastUpdatedDate = GETDATE(), IsUpdated = 1, IsActive = 1, IsDeleted = 0 WHERE Id = :id")
                        .setParameter("time", formattedTime)
                        .setParameter("loginId", loginId)
                        .setParameter("id", existingId)
                        .executeUpdate();
                } else {
                    entityManager.createNativeQuery(
                        "UPDATE ManualAttendance SET Status = 'Active', LastUpdatedBy = :loginId, LastUpdatedDate = GETDATE(), IsUpdated = 1, IsActive = 1, IsDeleted = 0 WHERE Id = :id")
                        .setParameter("loginId", loginId)
                        .setParameter("id", existingId)
                        .executeUpdate();
                }
                updatedRecords++;
            } else {
                entityManager.createNativeQuery(
                    "INSERT INTO ManualAttendance (EmpCode, Date, Time, Status, RecordStatus, CreatedBy, CreatedDate, IsActive, IsUpdated, IsDeleted) " +
                    "VALUES (:empCode, CAST(:dateStr AS date), CAST(:time AS time), 'Active', 1, :loginId, GETDATE(), 1, 0, 0)")
                    .setParameter("empCode", empCode.trim())
                    .setParameter("dateStr", formattedDate)
                    .setParameter("time", formattedTime != null ? formattedTime : "00:00:00")
                    .setParameter("loginId", loginId)
                    .executeUpdate();
                insertedRecords++;
            }
        }

        UploadResult result = new UploadResult();
        result.setTotalRecords(totalRecords);
        result.setInsertedRecords(insertedRecords + updatedRecords);
        result.setFailedRecords(exceptions.size());
        result.setExceptions(exceptions);

        return result;
    }

    public List<Map<String, Object>> getAllManualAttendance(Map<String, Object> model) {
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            List<ManualAttendance> allManual = manualAttendanceRepository.findByIsActiveAndIsDeletedOrderByCreatedDateDesc(true, false);

            List<Map<String, Object>> result = new ArrayList<>();
            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            SimpleDateFormat timeFormat = new SimpleDateFormat("HH:mm:ss");

            for (ManualAttendance ma : allManual) {
                if (!"Active".equalsIgnoreCase(ma.getStatus()) && !"Present".equalsIgnoreCase(ma.getStatus())) {
                    continue;
                }

                List<EmployeeMaster> empList = employeeMasterRepository.findByEmpCodeAndIsActiveAndIsDeleted(ma.getEmpCode(), true, false);
                if (empList.isEmpty()) continue;

                EmployeeMaster emp = empList.get(0);
                if (!"ACTIVE".equalsIgnoreCase(emp.getEmpStatus())) continue;

                Map<String, Object> record = new LinkedHashMap<>();
                record.put("EmpCode", ma.getEmpCode());

                String firstName = emp.getFirstName() != null ? emp.getFirstName() : "";
                String middleName = emp.getMiddleName() != null ? emp.getMiddleName() : "";
                String lastName = emp.getLastName() != null ? emp.getLastName() : "";
                record.put("FullName", (firstName + " " + middleName + " " + lastName).trim());

                record.put("CompId", emp.getCompId());
                if (emp.getCompId() != null) {
                    Optional<CompanyMaster> compOpt = companyMasterRepository.findById(emp.getCompId());
                    record.put("Company", compOpt.map(CompanyMaster::getCompany).orElse(""));
                } else {
                    record.put("Company", "");
                }

                record.put("LEId", emp.getLeId());
                if (emp.getLeId() != null && emp.getLeId() > 0) {
                    Optional<LegalEntityMaster> leOpt = legalEntityMasterRepository.findById(emp.getLeId());
                    record.put("LegalEntity", leOpt.map(LegalEntityMaster::getLegalEntity).orElse(""));
                } else {
                    record.put("LegalEntity", "");
                }

                record.put("BUId", emp.getBuId());
                if (emp.getBuId() != null && emp.getBuId() > 0) {
                    Optional<BusinessUnitMaster> buOpt = businessUnitMasterRepository.findById(emp.getBuId());
                    record.put("BusinessUnit", buOpt.map(BusinessUnitMaster::getBusinessUnit).orElse(""));
                } else {
                    record.put("BusinessUnit", "");
                }

                record.put("LocationId", emp.getLocationId());
                if (emp.getLocationId() != null && emp.getLocationId() > 0) {
                    Optional<LocationMaster> locOpt = locationMasterRepository.findById(emp.getLocationId());
                    record.put("Location", locOpt.map(LocationMaster::getLocation).orElse(""));
                } else {
                    record.put("Location", "");
                }

                record.put("Date", ma.getDate() != null ? sdf.format(ma.getDate()) : "");
                record.put("WorkedHrs", ma.getTime() != null ? timeFormat.format(ma.getTime()) : "");
                record.put("Status", ma.getStatus());

                result.add(record);
            }

            if (result.isEmpty()) {
                throw new RuntimeException("Manual Attendance is not Found");
            }

            return result;
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error fetching manual attendance: " + e.getMessage());
        }
    }

    public Map<String, Object> spAttendance(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            String dateStr = model.get("Date") != null ? model.get("Date").toString() : "";
            if (dateStr.isEmpty()) {
                throw new RuntimeException("Please select the Date!!");
            }

            SimpleDateFormat sdf;
            if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                sdf = new SimpleDateFormat("dd-MM-yyyy");
            } else {
                sdf = new SimpleDateFormat("yyyy-MM-dd");
            }
            Date spDate = sdf.parse(dateStr);

            Calendar dateCal = Calendar.getInstance();
            dateCal.setTime(spDate);
            Date dayStart = dateCal.getTime();
            dateCal.add(Calendar.DATE, 1);
            Date dayEnd = dateCal.getTime();

            // Soft-delete existing ManualAttendance records for this date first (dotnet pattern)
            List<ManualAttendance> existingRecords = manualAttendanceRepository.findByDate(spDate);
            for (ManualAttendance existing : existingRecords) {
                existing.setIsDeleted(true);
                existing.setLastUpdatedBy(loginId);
                existing.setLastUpdatedDate(new Date());
            }
            manualAttendanceRepository.saveAll(existingRecords);

            List<EmployeeMaster> activeEmps = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
                .filter(e -> e.getEmpStatus() != null && "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
                .collect(Collectors.toList());

            List<Loginlog> loginLogs = loginlogRepository.findByLoginDateBetween(dayStart, dayEnd);
            List<WFHLoginlog> wfhLogs = wfhLoginlogRepository.findByDateBetween(dayStart, dayEnd);
            List<OnSiteLoginlog> onsiteLogs = onSiteLoginlogRepository.findByLoginDateBetween(dayStart, dayEnd);
            List<Attendance> attendanceLogs = attendanceRepository.findByLogDateBetween(dayStart, dayEnd);

            for (EmployeeMaster emp : activeEmps) {
                String empCode = emp.getEmpCode();
                if (empCode == null || empCode.isEmpty()) continue;

                Date firstLogin = null;
                Date lastLogout = null;
                boolean hasRecord = false;

                // Check ALL sources for this employee (no priority chain)
                for (Loginlog log : loginLogs) {
                    if (empCode.equalsIgnoreCase(log.getEmpCode())) {
                        if (log.getLogInTime() != null) {
                            if (firstLogin == null || log.getLogInTime().before(firstLogin)) {
                                firstLogin = log.getLogInTime();
                            }
                        }
                        if (log.getLogOutTime() != null) {
                            if (lastLogout == null || log.getLogOutTime().after(lastLogout)) {
                                lastLogout = log.getLogOutTime();
                            }
                        }
                        hasRecord = true;
                    }
                }

                for (OnSiteLoginlog onsite : onsiteLogs) {
                    if (empCode.equalsIgnoreCase(onsite.getEmpCode())) {
                        if (onsite.getLogInTime() != null) {
                            if (firstLogin == null || onsite.getLogInTime().before(firstLogin)) {
                                firstLogin = onsite.getLogInTime();
                            }
                        }
                        if (onsite.getLogOutTime() != null) {
                            if (lastLogout == null || onsite.getLogOutTime().after(lastLogout)) {
                                lastLogout = onsite.getLogOutTime();
                            }
                        }
                        hasRecord = true;
                    }
                }

                for (Attendance att : attendanceLogs) {
                    if (empCode.equalsIgnoreCase(att.getEmpCode())) {
                        if (att.getLogTime() != null) {
                            if ("IN".equalsIgnoreCase(att.getType())) {
                                if (firstLogin == null || att.getLogTime().before(firstLogin)) {
                                    firstLogin = att.getLogTime();
                                }
                            } else if ("OUT".equalsIgnoreCase(att.getType())) {
                                if (lastLogout == null || att.getLogTime().after(lastLogout)) {
                                    lastLogout = att.getLogTime();
                                }
                            }
                        }
                        hasRecord = true;
                    }
                }

                for (WFHLoginlog wfh : wfhLogs) {
                    if (empCode.equalsIgnoreCase(wfh.getEmpCode())) {
                        if (wfh.getLoginTime() != null) {
                            if (firstLogin == null || wfh.getLoginTime().before(firstLogin)) {
                                firstLogin = wfh.getLoginTime();
                            }
                        }
                        if (wfh.getLogOutTime() != null) {
                            if (lastLogout == null || wfh.getLogOutTime().after(lastLogout)) {
                                lastLogout = wfh.getLogOutTime();
                            }
                        }
                        hasRecord = true;
                    }
                }

                if (hasRecord && firstLogin != null) {
                    ManualAttendance manualAtt = new ManualAttendance();
                    manualAtt.setEmpCode(empCode);
                    manualAtt.setDate(spDate);
                    manualAtt.setTime(firstLogin);
                    manualAtt.setStatus("Present");
                    manualAtt.setRecordStatus(true);
                    manualAtt.setCreatedBy(loginId);
                    manualAtt.setCreatedDate(new Date());
                    manualAtt.setIsActive(true);
                    manualAtt.setIsUpdated(false);
                    manualAtt.setIsDeleted(false);
                    manualAttendanceRepository.save(manualAtt);
                }

                if (hasRecord && lastLogout != null && !lastLogout.equals(firstLogin)) {
                    ManualAttendance manualAtt = new ManualAttendance();
                    manualAtt.setEmpCode(empCode);
                    manualAtt.setDate(spDate);
                    manualAtt.setTime(lastLogout);
                    manualAtt.setStatus("Present");
                    manualAtt.setRecordStatus(true);
                    manualAtt.setCreatedBy(loginId);
                    manualAtt.setCreatedDate(new Date());
                    manualAtt.setIsActive(true);
                    manualAtt.setIsUpdated(false);
                    manualAtt.setIsDeleted(false);
                    manualAttendanceRepository.save(manualAtt);
                }
            }

            String formattedDate = new SimpleDateFormat("yyyy-MM-dd").format(spDate);
            result.put("msg", "Attendance for " + formattedDate + " loaded successfully!");
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error processing attendance: " + e.getMessage());
        }
        return result;
    }

    public List<Map<String, Object>> attendanceDeptReport(Map<String, Object> model) {
        try {
            String startDateStr = model.get("StartDate") != null ? model.get("StartDate").toString() : "";
            String endDateStr = model.get("EndDate") != null ? model.get("EndDate").toString() : "";

            if (startDateStr.isEmpty() || endDateStr.isEmpty()) {
                throw new RuntimeException("StartDate and EndDate are required");
            }

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            Date startDate = sdf.parse(startDateStr);
            Date endDate = sdf.parse(endDateStr);

            Integer compId = parseInteger(model.get("CompId"));
            Integer leId = parseInteger(model.get("LEId"));
            Integer buId = parseInteger(model.get("BUId"));
            Integer locId = parseInteger(model.get("LocId"));
            Integer deptId = parseInteger(model.get("DeptId"));

            List<EmployeeMaster> allEmps = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
                .filter(e -> e.getEmpStatus() != null && "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
                .collect(Collectors.toList());

            List<EmployeeMaster> filteredEmps = new ArrayList<>();
            for (EmployeeMaster emp : allEmps) {
                if (compId != null && compId > 0 && !compId.equals(emp.getCompId())) continue;
                if (leId != null && leId > 0 && !leId.equals(emp.getLeId())) continue;
                if (buId != null && buId > 0 && !buId.equals(emp.getBuId())) continue;
                if (locId != null && locId > 0 && !locId.equals(emp.getLocationId())) continue;
                if (deptId != null && deptId > 0 && !deptId.equals(emp.getCategoryId())) continue;
                if (emp.getEmpCode() == null || emp.getEmpCode().isEmpty()) continue;
                if (emp.getDeptName() == null || emp.getDeptName().isEmpty()) continue;
                filteredEmps.add(emp);
            }

            List<Holiday> holidays = holidayRepository.findAll().stream()
                .filter(h -> "ACTIVE".equalsIgnoreCase(h.getStatus()) || "1".equals(h.getStatus()))
                .collect(Collectors.toList());
            List<WeekHoliday> weekHolidays = weekHolidayRepository.findAll().stream()
                .filter(wh -> "ACTIVE".equalsIgnoreCase(wh.getStatus()) || "1".equals(wh.getStatus()))
                .collect(Collectors.toList());

            List<Map<String, Object>> result = new ArrayList<>();

            Calendar cal = Calendar.getInstance();
            cal.setTime(startDate);

            while (!cal.getTime().after(endDate)) {
                Date currentDate = cal.getTime();
                Calendar dayCal = Calendar.getInstance();
                dayCal.setTime(currentDate);
                dayCal.add(Calendar.DATE, 1);
                Date nextDay = dayCal.getTime();

                String dayName = new SimpleDateFormat("EEEE").format(currentDate);
                String dateStr = sdf.format(currentDate);

                boolean isWeekend = false;
                for (WeekHoliday wh : weekHolidays) {
                    if (dayName.equalsIgnoreCase(wh.getDay())) {
                        isWeekend = true;
                        break;
                    }
                }

                boolean isHoliday = false;
                for (Holiday h : holidays) {
                    if (h.getDate() != null && sdf.format(h.getDate()).equals(dateStr)) {
                        isHoliday = true;
                        break;
                    }
                }

                if (isWeekend) isHoliday = true;

                List<Loginlog> loginLogs = loginlogRepository.findByLoginDateBetween(currentDate, nextDay);
                List<Attendance> attendanceLogs = attendanceRepository.findByLogDateBetween(currentDate, nextDay);
                List<WFHLoginlog> wfhLogs = wfhLoginlogRepository.findByDateBetween(currentDate, nextDay);
                List<OnSiteLoginlog> onsiteLogs = onSiteLoginlogRepository.findByLoginDateBetween(currentDate, nextDay);
                List<EmpLeaveApplication> leaveApps = empLeaveApplicationRepository.findAll().stream()
                    .filter(l -> l.getFromDate() != null && l.getToDate() != null &&
                           !currentDate.before(l.getFromDate()) && !currentDate.after(l.getToDate()) &&
                           Boolean.TRUE.equals(l.getIsActive()) && Boolean.FALSE.equals(l.getIsDeleted()))
                    .collect(Collectors.toList());

                Map<String, List<EmployeeMaster>> deptEmpMap = new LinkedHashMap<>();
                for (EmployeeMaster emp : filteredEmps) {
                    String deptName = emp.getDeptName() != null ? emp.getDeptName() : "Unknown";
                    deptEmpMap.computeIfAbsent(deptName, k -> new ArrayList<>()).add(emp);
                }

                List<Map<String, Object>> lstofDept = new ArrayList<>();

                for (Map.Entry<String, List<EmployeeMaster>> entry : deptEmpMap.entrySet()) {
                    String deptName = entry.getKey();
                    List<EmployeeMaster> deptEmps = entry.getValue();
                    int total = deptEmps.size();
                    int present = 0;
                    int absent = 0;
                    int leave = 0;

                    for (EmployeeMaster emp : deptEmps) {
                        String empCode = emp.getEmpCode();
                        boolean isPresent = false;
                        boolean isOnLeave = false;

                        for (Loginlog log : loginLogs) {
                            if (empCode.equalsIgnoreCase(log.getEmpCode()) && log.getLogInTime() != null) {
                                isPresent = true;
                                break;
                            }
                        }

                        if (!isPresent) {
                            for (Attendance att : attendanceLogs) {
                                if (empCode.equalsIgnoreCase(att.getEmpCode()) && "IN".equalsIgnoreCase(att.getType())) {
                                    isPresent = true;
                                    break;
                                }
                            }
                        }

                        if (!isPresent) {
                            for (WFHLoginlog wfh : wfhLogs) {
                                if (empCode.equalsIgnoreCase(wfh.getEmpCode()) && wfh.getLoginTime() != null) {
                                    isPresent = true;
                                    break;
                                }
                            }
                        }

                        if (!isPresent) {
                            for (OnSiteLoginlog onsite : onsiteLogs) {
                                if (empCode.equalsIgnoreCase(onsite.getEmpCode()) && onsite.getLogInTime() != null) {
                                    isPresent = true;
                                    break;
                                }
                            }
                        }

                        for (EmpLeaveApplication leaveApp : leaveApps) {
                            if (leaveApp.getEmpId() != null && leaveApp.getEmpId().equals(emp.getEmpId())) {
                                isOnLeave = true;
                                break;
                            }
                        }

                        if (isOnLeave) {
                            leave++;
                        } else if (isPresent) {
                            present++;
                        } else {
                            absent++;
                        }
                    }

                    String absentPercent = total > 0 ? String.format("%.2f", (absent * 100.0) / total) : "0.00";
                    String overAllAbsentPercent = total > 0 ? String.format("%.2f", ((absent + leave) * 100.0) / total) : "0.00";

                    Map<String, Object> deptData = new LinkedHashMap<>();
                    deptData.put("DeptName", deptName);
                    deptData.put("DeptShortName", deptName.length() > 3 ? deptName.substring(0, 3) : deptName);
                    deptData.put("Total", total);
                    deptData.put("OverAllAbsentPercentage", overAllAbsentPercent);
                    deptData.put("Present", present);
                    deptData.put("Absent", absent);
                    deptData.put("Leave", leave);
                    deptData.put("AbsentPesent", absentPercent);
                    deptData.put("IsHoliday", isHoliday);

                    lstofDept.add(deptData);
                }

                Map<String, Object> dateGroup = new LinkedHashMap<>();
                dateGroup.put("Date", dateStr);
                dateGroup.put("Day", dayName);
                dateGroup.put("lstofDept", lstofDept);

                result.add(dateGroup);

                cal.add(Calendar.DATE, 1);
            }

            return result;
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error generating department attendance report: " + e.getMessage());
        }
    }

    public Map<String, Object> contractAttendanceChecking(Map<String, Object> model) {
        try {
            // Extract parameters from model
            Integer loginId = parseInteger(model.get("LoginId"));
            String mobileNo = (String) model.get("MobileNo");
            
            // Validate LoginId (like in dotnet)
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }
            
            // Get today's date without time (like dotnet DateTime.Now.Date)
            Date today = new Date();
            // Set time to 00:00:00 to compare dates only
            java.util.Calendar cal = java.util.Calendar.getInstance();
            cal.setTime(today);
            cal.set(java.util.Calendar.HOUR_OF_DAY, 0);
            cal.set(java.util.Calendar.MINUTE, 0);
            cal.set(java.util.Calendar.SECOND, 0);
            cal.set(java.util.Calendar.MILLISECOND, 0);
            Date todayDateOnly = cal.getTime();
            
            // Find contract attendance record for today with login=true, logout=false
            Optional<ContractAttendance> attendanceOpt = contractAttendanceRepository.findByMobileAndDateAndFlags(
                    mobileNo, todayDateOnly, true, false, true, false);
            
            Map<String, Object> result = new HashMap<>();
            
            if (attendanceOpt.isPresent()) {
                ContractAttendance attendance = attendanceOpt.get();
                result.put("LoginStatus", "LOGIN");
                result.put("CId", attendance.getcId());
                result.put("Date", attendance.getDate());
                result.put("Mobile", attendance.getMobile());
                result.put("Mail", attendance.getMail());
                result.put("EmpCode", attendance.getEmpCode());
                result.put("EmpName", attendance.getEmpName());
                result.put("Skill", attendance.getSkill());
                result.put("VendorId", attendance.getVendorId());
                // Note: ERPVendorId field doesn't exist in Java entity, skipping
                result.put("VendorCode", attendance.getVendorCode());
                result.put("Vendor", attendance.getVendor());
                result.put("ProjectId", attendance.getProjectId());
                // Note: ERPProjectId field doesn't exist in Java entity, skipping
                result.put("ProjectCode", attendance.getProjectCode());
                result.put("Project", attendance.getProject());
                result.put("SiteId", attendance.getSiteId());
                result.put("Site", attendance.getSite());
                result.put("SiteDetails", attendance.getSiteDetails());
                result.put("ManagerId", attendance.getManagerId());
                result.put("ManagerEmpCode", attendance.getManagerEmpCode());
                result.put("ManagerName", attendance.getManagerName());
                result.put("Status", attendance.getStatus());
                result.put("IsLogin", attendance.getIsLogin());
                result.put("IsLogout", attendance.getIsLogout());
                result.put("LoginTime", parseTimeToObject(attendance.getLoginTime()));
                result.put("LogoutTime", parseTimeToObject(attendance.getLogoutTime()));
                result.put("Activehrs", parseTimeToObject(attendance.getActivehrs()));
                result.put("Approvedhrs", parseTimeToObject(attendance.getApprovedhrs()));
                result.put("LoginAddress", attendance.getLoginAddress());
                result.put("LoginLonqitude", attendance.getLoginLonqitude());
                result.put("LoginLatitude", attendance.getLoginLatitude());
                result.put("LogoutAddress", attendance.getLogoutAddress());
                result.put("LogoutLonqitude", attendance.getLogoutLonqitude());
                result.put("LogoutLatitude", attendance.getLogoutLatitude());
                result.put("Description", attendance.getDescription());
                result.put("ManPowerApproval", attendance.getManPowerApproval());
                result.put("IsApproved", attendance.getIsApproved());
                result.put("IsLogoutManager", attendance.getIsLogoutManager());
                result.put("CreatedBy", attendance.getCreatedBy());
                result.put("CreatedDate", attendance.getCreatedDate());
                result.put("LastUpdatedBy", attendance.getLastUpdatedBy());
                result.put("LastUpdatedDate", attendance.getLastUpdatedDate());
                result.put("IsActive", attendance.getIsActive());
                result.put("IsUpdated", attendance.getIsUpdated());
                result.put("IsDeleted", attendance.getIsDeleted());
            } else {
                // No data found - return minimal object with status "No Data"
                result.put("LoginStatus", "No Data");
            }
            
            return result;
        } catch (RuntimeException ex) {
            throw ex; // Re-throw runtime exceptions as-is
        } catch (Exception ex) {
            throw new RuntimeException("An error occurred while processing your request: " + ex.getMessage());
        }
    }

    public Map<String, Object> addContractAttendance(Map<String, Object> model) {
        try {
            // Extract parameters from model
            Integer loginId = parseInteger(model.get("LoginId"));
            String mobileNo = (String) model.get("Mobile");
            String mail = (String) model.get("Mail");
            String empCode = (String) model.get("EmpCode");
            String empName = (String) model.get("EmpName");
            String skill = (String) model.get("Skill");
            Integer vendorId = parseInteger(model.get("VendorId"));
            // Note: ERPVendorId field doesn't exist in Java entity, skipping
            String vendorCode = (String) model.get("VendorCode");
            String vendor = (String) model.get("Vendor");
            Integer projectId = parseInteger(model.get("ProjectId"));
            // Note: ERPProjectId field doesn't exist in Java entity, skipping
            String projectCode = (String) model.get("ProjectCode");
            String project = (String) model.get("Project");
            Integer siteId = parseInteger(model.get("SiteId"));
            String site = (String) model.get("Site");
            String siteDetails = (String) model.get("SiteDetails");
            Integer managerId = parseInteger(model.get("ManagerId"));
            String managerEmpCode = (String) model.get("ManagerEmpCode");
            String managerName = (String) model.get("ManagerName");
            String loginStatus = (String) model.get("LoginStatus");
            String approvedHrs = (String) model.get("Approvedhrs");
            String loginAddress = (String) model.get("LoginAddress");
            String loginLonqitude = (String) model.get("LoginLonqitude");
            String loginLatitude = (String) model.get("LoginLatitude");
            String logoutAddress = (String) model.get("LogoutAddress");
            String logoutLonqitude = (String) model.get("LogoutLonqitude");
            String logoutLatitude = (String) model.get("LogoutLatitude");
            
            // Validate LoginId (like in dotnet)
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }
            
            // Get today's date without time (like dotnet DateTime.Now.Date)
            Date today = new Date();
            // Set time to 00:00:00 to compare dates only
            java.util.Calendar cal = java.util.Calendar.getInstance();
            cal.setTime(today);
            cal.set(java.util.Calendar.HOUR_OF_DAY, 0);
            cal.set(java.util.Calendar.MINUTE, 0);
            cal.set(java.util.Calendar.SECOND, 0);
            cal.set(java.util.Calendar.MILLISECOND, 0);
            Date todayDateOnly = cal.getTime();
            
            // Get current time for login/logout (like dotnet DateTime.Now.ToString("HH:mm:ss"))
            java.util.Calendar timeCal = java.util.Calendar.getInstance();
            String currentTime = String.format("%02d:%02d:%02d", 
                timeCal.get(java.util.Calendar.HOUR_OF_DAY),
                timeCal.get(java.util.Calendar.MINUTE),
                timeCal.get(java.util.Calendar.SECOND));
            
            // Find contract attendance record for today with login=true, logout=false
            Optional<ContractAttendance> attendanceOpt = contractAttendanceRepository.findByMobileAndDateAndFlags(
                    mobileNo, todayDateOnly, true, false, true, false);
            
            Map<String, Object> result = new HashMap<>();
            
            if ("NO DATA".equalsIgnoreCase(loginStatus)) {
                // Login case - create new record if none exists for today
                if (attendanceOpt.isPresent()) {
                    // Already has a login record for today
                    result.put("msg", "Already logged in today");
                    result.put("StatusCode", 400);
                } else {
                    // Create new login record
                    ContractAttendance attendance = new ContractAttendance();
                    attendance.setDate(todayDateOnly);
                    attendance.setMobile(mobileNo);
                    attendance.setMail(mail);
                    attendance.setEmpCode(empCode);
                    attendance.setEmpName(empName);
                    attendance.setSkill(skill);
                    attendance.setVendorId(vendorId);
                    // Note: ERPVendorId field doesn't exist in Java entity
                    attendance.setVendorCode(vendorCode);
                    attendance.setVendor(vendor);
                    attendance.setProjectId(projectId);
                    // Note: ERPProjectId field doesn't exist in Java entity
                    attendance.setProjectCode(projectCode);
                    attendance.setProject(project);
                    attendance.setSiteId(siteId);
                    attendance.setSite(site);
                    attendance.setSiteDetails(siteDetails);
                    attendance.setManagerId(managerId);
                    attendance.setManagerEmpCode(managerEmpCode);
                    attendance.setManagerName(managerName);
                    attendance.setStatus(true);
                    attendance.setIsLogin(true);
                    attendance.setIsLogout(false);
                    attendance.setLoginTime(currentTime);
                    // Logout time will be empty initially
                    attendance.setLogoutTime("");
                    attendance.setActivehrs("00:00:00");
                    attendance.setApprovedhrs(approvedHrs != null ? approvedHrs : "00:00:00");
                    attendance.setLoginAddress(loginAddress);
                    attendance.setLoginLonqitude(loginLonqitude);
                    attendance.setLoginLatitude(loginLatitude);
                    // Logout address/coordinates will be empty initially
                    attendance.setLogoutAddress("");
                    attendance.setLogoutLonqitude("");
                    attendance.setLogoutLatitude("");
                    attendance.setIsLogoutManager(false);
                    attendance.setIsApproved(false);
                    attendance.setDescription("");
                    attendance.setManPowerApproval("");
                    attendance.setCreatedBy(loginId); // Using loginId as CreatedBy
                    attendance.setCreatedDate(new Date());
                    attendance.setLastUpdatedBy(loginId); // Using loginId as LastUpdatedBy
                    attendance.setLastUpdatedDate(new Date());
                    attendance.setIsActive(true);
                    attendance.setIsUpdated(false);
                    attendance.setIsDeleted(false);
                    
                    contractAttendanceRepository.save(attendance);
                    result.put("msg", "Login Successfully");
                    result.put("StatusCode", 200);
                }
            } else {
                // Logout case - update existing record
                if (attendanceOpt.isPresent()) {
                    ContractAttendance attendance = attendanceOpt.get();
                    // Update logout time and calculate active hours
                    attendance.setIsLogin(false);
                    attendance.setIsLogout(true);
                    attendance.setLogoutTime(currentTime);
                    
                    // Calculate active hours (like dotnet: activehrs = logoutTime - loginTime)
                    String loginTimeStr = attendance.getLoginTime();
                    if (loginTimeStr != null && !loginTimeStr.isEmpty()) {
                        try {
                            java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat("HH:mm:ss");
                            Date loginDt = sdf.parse(cleanTimeString(loginTimeStr));
                            Date logoutDt = sdf.parse(currentTime);
                            long diffMillis = logoutDt.getTime() - loginDt.getTime();
                            if (diffMillis < 0) diffMillis += 24 * 60 * 60 * 1000;
                            attendance.setActivehrs(String.format("%02d:%02d:%02d",
                                (diffMillis / (60 * 60 * 1000)) % 24,
                                (diffMillis / (60 * 1000)) % 60,
                                (diffMillis / 1000) % 60));
                        } catch (Exception e) {
                            attendance.setActivehrs("00:00:00");
                        }
                    } else {
                        attendance.setActivehrs("00:00:00");
                    }
                    
                    attendance.setLogoutAddress(logoutAddress);
                    attendance.setLogoutLonqitude(logoutLonqitude);
                    attendance.setLogoutLatitude(logoutLatitude);
                    attendance.setLastUpdatedBy(loginId); // Using loginId as LastUpdatedBy
                    attendance.setLastUpdatedDate(new Date());
                    
                    contractAttendanceRepository.save(attendance);
                    result.put("msg", "Logout Successfully");
                    result.put("StatusCode", 200);
                } else {
                    // No login record found for today
                    result.put("msg", "No login record found for today");
                    result.put("StatusCode", 400);
                }
            }
            
            return result;
        } catch (RuntimeException ex) {
            throw ex; // Re-throw runtime exceptions as-is
        } catch (Exception ex) {
            throw new RuntimeException("An error occurred while processing your request: " + ex.getMessage());
        }
    }

    public List<Map<String, Object>> contractAttendanceManager(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <=0) {
                throw new RuntimeException("LoginId is Missing");
            }

            // Get employee details
            Optional<EmployeeMaster> currentEmployeeOpt = employeeMasterRepository.findById(loginId);
            if (currentEmployeeOpt.isEmpty() || !Boolean.TRUE.equals(currentEmployeeOpt.get().getIsActive()) || Boolean.TRUE.equals(currentEmployeeOpt.get().getIsDeleted())) {
                throw new RuntimeException("Employee not found");
            }
            
            EmployeeMaster currentEmployee = currentEmployeeOpt.get();
            
            // Check if employee is HR
            boolean isHR = currentEmployee.getDeptName() != null && 
                     (currentEmployee.getDeptName().toUpperCase().contains("HUMAN RESOURCE") || 
                      currentEmployee.getDeptName().toUpperCase().contains("HR"));

            // Build query - Get all non-deleted records first
            List<ContractAttendance> attendanceList = contractAttendanceRepository.findByIsDeleted(false);
            
            // If not HR, filter by ManagerId
            if (!isHR) {
                attendanceList = attendanceList.stream()
                    .filter(ca -> loginId.equals(ca.getManagerId()))
                    .collect(Collectors.toList());
            }

            // Apply date filter (only if both dates are provided and not empty)
            if (model.get("FromDate") != null && model.get("ToDate") != null) {
                String fromDateStr = model.get("FromDate").toString().trim();
                String toDateStr = model.get("ToDate").toString().trim();
                
                if (!fromDateStr.isEmpty() && !toDateStr.isEmpty()) {
                    SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
                    Date fromDate = sdf.parse(fromDateStr);
                    Date toDate = sdf.parse(toDateStr);
                    Calendar cal = Calendar.getInstance();
                    cal.setTime(toDate);
                    cal.add(Calendar.DATE, 1);
                    Date to = cal.getTime();
                    
                    attendanceList = attendanceList.stream()
                        .filter(ca -> ca.getDate() != null && !ca.getDate().before(fromDate) && ca.getDate().before(to))
                        .collect(Collectors.toList());
                }
            }

            // Apply project filter
            if (model.get("ProjectId") != null) {
                Integer projectId = parseInteger(model.get("ProjectId"));
                if (projectId != null && projectId >0) {
                    attendanceList = attendanceList.stream()
                        .filter(ca -> projectId.equals(ca.getProjectId()))
                        .collect(Collectors.toList());
                }
            }

            // Apply vendor filter
            if (model.get("VendorId") != null) {
                Integer vendorId = parseInteger(model.get("VendorId"));
                if (vendorId != null && vendorId >0) {
                    attendanceList = attendanceList.stream()
                        .filter(ca -> vendorId.equals(ca.getVendorId()))
                        .collect(Collectors.toList());
                }
            }

            // Apply status filter
            if (model.get("Status") != null) {
                String status = model.get("Status").toString().trim().toUpperCase();
                if (!status.isEmpty() && !status.equals("ALL")) {
                    if (status.equals("APPROVED")) {
                        attendanceList = attendanceList.stream()
                            .filter(ca -> Boolean.TRUE.equals(ca.getIsApproved()))
                            .collect(Collectors.toList());
                    } else if (status.equals("REJECTED")) {
                        // ContractAttendance doesn't have IsRejected, skip or add logic if needed
                    } else if (status.equals("COMPLETED")) {
                        attendanceList = attendanceList.stream()
                            .filter(ca -> Boolean.TRUE.equals(ca.getIsApproved()) && Boolean.TRUE.equals(ca.getIsLogout()))
                            .collect(Collectors.toList());
                    } else if (status.equals("APPLIED")) {
                        attendanceList = attendanceList.stream()
                            .filter(ca -> !Boolean.TRUE.equals(ca.getIsApproved()))
                            .collect(Collectors.toList());
                    }
                }
            }

            // Sort by created date descending
            attendanceList.sort((a, b) -> {
                if (a.getCreatedDate() == null && b.getCreatedDate() == null) return 0;
                if (a.getCreatedDate() == null) return 1;
                if (b.getCreatedDate() == null) return -1;
                return b.getCreatedDate().compareTo(a.getCreatedDate());
            });

              for (ContractAttendance ca : attendanceList) {
                  Map<String, Object> record = new LinkedHashMap<>();
                  record.put("CId", ca.getcId());
                  
                  SimpleDateFormat dateSdf = new SimpleDateFormat("yyyy-MM-dd");
                  SimpleDateFormat timestampSdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
                  
                  // Format Date in .NET JSON format "/Date(timestamp)/" for frontend parsing
                  record.put("Date", ca.getDate() != null ? "/Date(" + ca.getDate().getTime() + ")/" : null);
                  record.put("Mobile", ca.getMobile());
                 record.put("Mail", ca.getMail());
                 record.put("EmpCode", ca.getEmpCode());
                 record.put("EmpName", ca.getEmpName());
                 record.put("Skill", ca.getSkill());
                 record.put("VendorId", ca.getVendorId());
                 record.put("Vendor", ca.getVendor());
                 record.put("VendorCode", ca.getVendorCode());
                 record.put("ProjectId", ca.getProjectId());
                 record.put("ProjectCode", ca.getProjectCode());
                 record.put("Project", ca.getProject());
                 record.put("SiteId", ca.getSiteId());
                 record.put("Site", ca.getSite());
                 record.put("SiteDetails", ca.getSiteDetails());
                 record.put("ManagerId", ca.getManagerId());
                 record.put("ManagerEmpCode", ca.getManagerEmpCode());
                 record.put("ManagerName", ca.getManagerName());
                 record.put("Status", ca.getStatus());
                 
                 String loginStatus = "UNKNOWN";
                 if (Boolean.TRUE.equals(ca.getIsLogin()) && Boolean.FALSE.equals(ca.getIsLogout())) {
                     loginStatus = "LOGIN";
                 } else if (Boolean.TRUE.equals(ca.getIsLogin()) && Boolean.TRUE.equals(ca.getIsLogout())) {
                     loginStatus = "LOGOUT";
                 }
                 record.put("LoginStatus", loginStatus);
                 record.put("IsLogin", ca.getIsLogin());
                 record.put("IsLogout", ca.getIsLogout());
                 
                  // Clean time fields to remove extra decimal precision (e.g., "18:56:36.0000000" -> "18:56:36")
                  record.put("LoginTime", parseTimeToObject(ca.getLoginTime()));
                  record.put("LogoutTime", parseTimeToObject(ca.getLogoutTime()));
                  record.put("Activehrs", parseTimeToObject(ca.getActivehrs()));
                  record.put("Approvedhrs", parseTimeToObject(ca.getApprovedhrs()));
                 
                 record.put("LoginAddress", ca.getLoginAddress());
                 record.put("LoginLongitude", ca.getLoginLonqitude());
                 record.put("LoginLatitude", ca.getLoginLatitude());
                 record.put("LogoutAddress", ca.getLogoutAddress());
                 record.put("LogoutLongitude", ca.getLogoutLonqitude());
                 record.put("LogoutLatitude", ca.getLogoutLatitude());
                 record.put("Description", ca.getDescription());
                 record.put("ManPowerApproval", ca.getManPowerApproval());
                 record.put("IsApproved", ca.getIsApproved());
                 record.put("IsLogoutManager", ca.getIsLogoutManager());
                 record.put("CreatedBy", ca.getCreatedBy());
                 record.put("CreatedDate", ca.getCreatedDate() != null ? timestampSdf.format(ca.getCreatedDate()) : null);
                 record.put("LastUpdatedBy", ca.getLastUpdatedBy());
                 record.put("LastUpdatedDate", ca.getLastUpdatedDate() != null ? timestampSdf.format(ca.getLastUpdatedDate()) : null);
                 record.put("IsActive", ca.getIsActive());
                 record.put("IsUpdated", ca.getIsUpdated());
                 record.put("IsDeleted", ca.getIsDeleted());
                 result.add(record);
             }

            if (result.isEmpty()) {
                throw new RuntimeException("Contract Attendance Details Not Found");
            }
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("An error occurred while processing your request: " + e.getMessage());
        }
        return result;
    }

    public Map<String, Object> approvedbyManager(Map<String, Object> model) {
        Map<String, Object> response = new HashMap<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is required");
            }
            
            Object lstObj = model.get("lstofCantractIId");
            if (lstObj == null) {
                throw new RuntimeException("CFIds missing");
            }
            
            List<?> rawList = (List<?>) lstObj;
            if (rawList.isEmpty()) {
                throw new RuntimeException("CFIds missing");
            }
            
            for (Object itemObj : rawList) {
                Map<String, Object> item = (Map<String, Object>) itemObj;
                Integer cId = parseInteger(item.get("CId"));
                if (cId == null || cId <= 0) continue;
                
                Optional<ContractAttendance> attOpt = contractAttendanceRepository.findById(cId);
                if (attOpt.isPresent()) {
                    ContractAttendance att = attOpt.get();
                    att.setIsApproved(true);
                    att.setLastUpdatedBy(loginId);
                    att.setLastUpdatedDate(new Date());
                    att.setIsActive(true);
                    att.setIsUpdated(true);
                    contractAttendanceRepository.save(att);
                }
            }
            
            response.put("msg", "Approved Successfully");
            response.put("Status", 200);
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("An error occurred while processing your request: " + e.getMessage());
        }
        return response;
    }

    public Map<String, Object> logoutbyManager(Map<String, Object> model) {
        Map<String, Object> response = new HashMap<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is required");
            }
            
            Integer cId = parseInteger(model.get("CId"));
            if (cId == null || cId <= 0) {
                throw new RuntimeException("CId is required");
            }
            
            Optional<ContractAttendance> attOpt = contractAttendanceRepository.findById(cId);
            if (attOpt.isEmpty()) {
                throw new RuntimeException("Attendance record not found for logout");
            }
            
            ContractAttendance att = attOpt.get();
            
            // Get current time for logout
            Date now = new Date();
            java.text.SimpleDateFormat timeFormat = new java.text.SimpleDateFormat("HH:mm:ss");
            String logoutTimeStr = timeFormat.format(now);
            
            // Calculate active hours if login time exists
            if (att.getLoginTime() != null) {
                try {
                    java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat("HH:mm:ss");
                    Date loginTime = sdf.parse(att.getLoginTime());
                    Date logoutTime = sdf.parse(logoutTimeStr);
                    long diffMillis = logoutTime.getTime() - loginTime.getTime();
                    if (diffMillis < 0) diffMillis += 24 * 60 * 60 * 1000; // Handle midnight crossover
                    String activeHrs = String.format("%02d:%02d:%02d.0000000", 
                        (diffMillis / (60 * 60 * 1000)) % 24,
                        (diffMillis / (60 * 1000)) % 60,
                        (diffMillis / 1000) % 60);
                    att.setActivehrs(activeHrs);
                } catch (Exception e) {
                    // Ignore time calculation errors
                }
            }
            
            // Update logout details
            att.setLogoutTime(logoutTimeStr);
            att.setLogoutAddress((String) model.getOrDefault("LogoutAddress", null));
            att.setLogoutLonqitude((String) model.getOrDefault("LogoutLonqitude", null));
            att.setLogoutLatitude((String) model.getOrDefault("LogoutLatitude", null));
            att.setDescription((String) model.getOrDefault("Description", att.getDescription()));
            att.setIsLogout(true);
            att.setIsLogoutManager(true);
            att.setLastUpdatedBy(loginId);
            att.setLastUpdatedDate(now);
            att.setIsActive(true);
            att.setIsUpdated(true);
            
            contractAttendanceRepository.save(att);
            
            response.put("msg", "Logout Successfully");
            response.put("StatusCode", 200);
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("An error occurred while processing your request: " + e.getMessage());
        }
        return response;
    }

    public Map<String, Object> approvedHrbyManager(Map<String, Object> model) {
        Map<String, Object> response = new HashMap<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is required");
            }

            Integer cId = parseInteger(model.get("CId"));
            if (cId == null || cId <= 0) {
                throw new RuntimeException("CId is required");
            }

            Optional<ContractAttendance> attOpt = contractAttendanceRepository.findById(cId);
            if (attOpt.isEmpty()) {
                throw new RuntimeException("Attendance record not found for logout");
            }

            ContractAttendance att = attOpt.get();
            String approvedHrs = (String) model.get("Approvedhrs");
            if (approvedHrs != null && !approvedHrs.isEmpty()) {
                att.setApprovedhrs(approvedHrs);
            }
            att.setLastUpdatedBy(loginId);
            att.setLastUpdatedDate(new Date());
            att.setIsActive(true);
            att.setIsUpdated(true);

            contractAttendanceRepository.save(att);

            response.put("msg", "Approved hours added Successfully");
            response.put("StatusCode", 200);
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("An error occurred while processing your request: " + e.getMessage());
        }
        return response;
    }

    public List<Map<String, Object>> ddVendorList(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            List<VendorMaster> vendors = vendorMasterRepository.findByIsActiveAndIsDeleted(true, false);
            
            vendors.sort((a, b) -> {
                if (a.getVendorId() == null) return 1;
                if (b.getVendorId() == null) return -1;
                return b.getVendorId().compareTo(a.getVendorId());
            });

            for (VendorMaster emp : vendors) {
                Map<String, Object> record = new LinkedHashMap<>();
                record.put("VendorId", emp.getVendorId());
                record.put("ERPVendorId", emp.getErpVendorId());
                record.put("VendorCode", emp.getVendorCode() != null ? emp.getVendorCode() : "");
                record.put("Vendor", emp.getVendor());
                result.add(record);
            }

            if (result.isEmpty()) {
                throw new RuntimeException("Vendor Details Not Found");
            }
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error fetching vendor list: " + e.getMessage());
        }
        return result;
    }

    public List<Map<String, Object>> ddSiteList(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            List<SiteMaster> sites = siteMasterRepository.findByIsActiveAndIsDeleted(true, false);
            
            // Log the sites found for debugging
            System.out.println("DDSiteList: Found " + sites.size() + " active and not deleted sites.");
            for (SiteMaster site : sites) {
                System.out.println("DDSiteList: SiteId=" + site.getSiteId() + ", Site=" + site.getSite());
            }
            
            sites.sort((a, b) -> {
                if (a.getSiteId() == null) return 1;
                if (b.getSiteId() == null) return -1;
                return b.getSiteId().compareTo(a.getSiteId());
            });

            for (SiteMaster site : sites) {
                Map<String, Object> record = new LinkedHashMap<>();
                record.put("SiteId", site.getSiteId());
                record.put("Site", site.getSite());
                result.add(record);
            }

            if (result.isEmpty()) {
                throw new RuntimeException("Site Details Not Found");
            }
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error fetching site list: " + e.getMessage());
        }
        return result;
    }

    public List<Map<String, Object>> ddProjectList(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        try {
            Integer loginId = parseInteger(model.get("LoginId"));
            if (loginId == null || loginId <= 0) {
                throw new RuntimeException("LoginId is Missing");
            }

            List<ProjectMaster> projects = projectMasterRepository.findByIsActiveAndIsDeleted(true, false);
            
            projects.sort((a, b) -> {
                if (a.getProjectId() == null) return 1;
                if (b.getProjectId() == null) return -1;
                return b.getProjectId().compareTo(a.getProjectId());
            });

            for (ProjectMaster emp : projects) {
                Map<String, Object> record = new LinkedHashMap<>();
                record.put("ProjectId", emp.getProjectId());
                record.put("ProjectCode", emp.getProjectCode());
                record.put("Project", emp.getProject());
                record.put("ManagerId", emp.getProjectManagerId());
                record.put("ManagerCode", emp.getManagerCode());
                record.put("ManagerName", emp.getManagerName());
                record.put("SiteId", emp.getSiteId());
                record.put("Site", emp.getSite());
                result.add(record);
            }

            if (result.isEmpty()) {
                throw new RuntimeException("Project Details Not Found");
            }
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("Error fetching project list: " + e.getMessage());
        }
        return result;
    }

    public Map<String, Object> confirmProbation(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? ((Number) model.get("LoginId")).intValue() : 0;
        if (loginId == 0) throw new RuntimeException("Login Id is mismatching");

        Integer empId = model.get("EmpId") != null ? ((Number) model.get("EmpId")).intValue() : 0;
        if (empId == 0) throw new RuntimeException("EmpId is Missing");

        List<EmpProbationTrackingHistory> probationRecords = empProbationTrackingHistoryRepository.findAll().stream()
            .filter(p -> empId.equals(p.getEmpId()) && Boolean.TRUE.equals(p.getIsProbation())
                      && Boolean.TRUE.equals(p.getIsActive()) && Boolean.FALSE.equals(p.getIsDeleted()))
            .collect(Collectors.toList());

        if (probationRecords.isEmpty()) throw new RuntimeException("Probation details is not found");

        EmpProbationTrackingHistory pth = probationRecords.get(0);

        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
        if (empOpt.isEmpty() || !"ACTIVE".equalsIgnoreCase(empOpt.get().getEmpStatus())
            || !Boolean.TRUE.equals(empOpt.get().getIsActive()) || Boolean.TRUE.equals(empOpt.get().getIsDeleted())) {
            throw new RuntimeException("Employee is not active");
        }

        pth.setIsProbation(false);
        pth.setConfirmBy(loginId);
        pth.setConfirmDate(new Date());
        pth.setIsPermanent(true);
        if (model.get("Remarks") != null) pth.setRemarks((String) model.get("Remarks"));
        pth.setLastUpdatedBy(loginId);
        pth.setLastUpdatedDate(new Date());
        pth.setIsUpdated(true);
        empProbationTrackingHistoryRepository.save(pth);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Employee Permanented");
        result.put("status", 200);
        result.put("StatusCode", 200);
        return result;
    }

    public Map<String, Object> getDesignationHierarchy(Map<String, Object> model) {
        Integer compId = parseInteger(model.get("CompId"));
        Integer leId = parseInteger(model.get("LEId"));
        Integer buId = parseInteger(model.get("BUId"));
        Integer locationId = parseInteger(model.get("LocationId"));
        Integer deptId = parseInteger(model.get("DeptId"));
        Integer designationId = parseInteger(model.get("DesignationId"));
        Integer reporterId = parseInteger(model.get("ReporterId"));
        Integer gradeId = parseInteger(model.get("GradeId"));
        Integer empId = parseInteger(model.get("EmpId"));

        // Get all active employees with designation and department info
        List<EmployeeMaster> allEmp = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(e -> "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
            .filter(e -> compId == null || compId <= 0 || (e.getCompId() != null && e.getCompId().equals(compId)))
            .filter(e -> leId == null || leId <= 0 || (e.getLeId() != null && e.getLeId().equals(leId)))
            .filter(e -> buId == null || buId <= 0 || (e.getBuId() != null && e.getBuId().equals(buId)))
            .filter(e -> locationId == null || locationId <= 0 || (e.getLocationId() != null && e.getLocationId().equals(locationId)))
            .filter(e -> deptId == null || deptId <= 0 || (e.getCategoryId() != null && e.getCategoryId().equals(deptId)))
            .filter(e -> designationId == null || designationId <= 0 || (e.getDesignationId() != null && e.getDesignationId().equals(designationId)))
            .filter(e -> reporterId == null || reporterId <= 0 || (e.getReportId() != null && e.getReportId().equals(reporterId)))
            .filter(e -> empId == null || empId <= 0 || e.getEmpId().equals(empId))
            .collect(Collectors.toList());

        // Preload designation and grade info
        List<DesignationMaster> allDesigs = designationMasterRepository.findAll().stream()
            .filter(d -> Boolean.TRUE.equals(d.getIsActive()) && (d.getIsDeleted() == null || !d.getIsDeleted()))
            .collect(Collectors.toList());
        Map<Integer, DesignationMaster> desigMap = allDesigs.stream()
            .collect(Collectors.toMap(DesignationMaster::getDesignationId, d -> d));

        // Build hierarchy nodes
        List<Map<String, Object>> allNodes = new ArrayList<>();
        Map<Integer, Map<String, Object>> nodeMap = new HashMap<>();

        for (EmployeeMaster emp : allEmp) {
            Map<String, Object> node = new LinkedHashMap<>();
            node.put("EmpId", emp.getEmpId());
            node.put("EmpCode", emp.getEmpCode());
            String fn = emp.getFirstName() != null ? emp.getFirstName().trim() : "";
            String mn = emp.getMiddleName() != null ? " " + emp.getMiddleName().trim() : "";
            String ln = emp.getLastName() != null ? " " + emp.getLastName().trim() : "";
            node.put("EmployeeName", (fn + mn + ln).trim());

            DesignationMaster desig = emp.getDesignationId() != null ? desigMap.get(emp.getDesignationId()) : null;
            node.put("DesignationName", desig != null ? desig.getDesignation() : "");
            node.put("GradeId", desig != null ? desig.getGradeId() : null);
            node.put("GradeName", desig != null && desig.getGrade() != null ? desig.getGrade() : "");
            node.put("DeptName", "");
            node.put("DeptShortName", "");
            if (emp.getCategoryId() != null) {
                deptMasterRepository.findById(emp.getCategoryId()).ifPresent(dept -> {
                    node.put("DeptName", dept.getDeptName() != null ? dept.getDeptName() : "");
                    node.put("DeptShortName", dept.getDeptShortName() != null ? dept.getDeptShortName() : "");
                });
            }
            node.put("LocationId", emp.getLocationId());
            node.put("ReporterId", emp.getReportId());
            node.put("ReporteesCount", 0);
            node.put("Reportees", new ArrayList<Map<String, Object>>());
            node.put("HierarchyLevel", 0);
            allNodes.add(node);
            nodeMap.put(emp.getEmpId(), node);
        }

        // Build tree: find root employees and assign reportees
        List<Map<String, Object>> roots = new ArrayList<>();
        for (Map<String, Object> node : allNodes) {
            Integer repId = (Integer) node.get("ReporterId");
            Integer empIdVal = (Integer) node.get("EmpId");
            if (repId == null || repId == 0 || repId.equals(empIdVal) || !nodeMap.containsKey(repId)) {
                roots.add(node);
            }
        }

        // Assign reportees
        for (Map<String, Object> node : allNodes) {
            Integer repId = (Integer) node.get("ReporterId");
            if (repId != null && repId > 0 && nodeMap.containsKey(repId) && !repId.equals(node.get("EmpId"))) {
                Map<String, Object> parent = nodeMap.get(repId);
                @SuppressWarnings("unchecked")
                List<Map<String, Object>> reportees = (List<Map<String, Object>>) parent.get("Reportees");
                reportees.add(node);
                parent.put("ReporteesCount", reportees.size());
            }
        }

        // Set hierarchy levels via BFS
        for (Map<String, Object> root : roots) {
            setLevels(root, 0);
        }

        // Build summary
        Map<String, Object> summary = new HashMap<>();
        summary.put("TotalEmployees", allNodes.size());
        summary.put("TotalDepartments", (int) allNodes.stream().map(n -> n.get("DeptName")).distinct().count());
        summary.put("TotalDesignations", (int) allNodes.stream().map(n -> n.get("DesignationName")).distinct().count());
        summary.put("TotalGrades", (int) allNodes.stream().map(n -> n.get("GradeName")).filter(g -> g != null && !g.toString().isEmpty()).distinct().count());

        Map<String, Object> data = new HashMap<>();
        data.put("Hierarchy", roots);
        data.put("Summary", summary);
        data.put("GeneratedOn", new Date());

        Map<String, Object> response = new HashMap<>();
        response.put("Success", true);
        response.put("Data", data);
        return response;
    }

    @SuppressWarnings("unchecked")
    private void setLevels(Map<String, Object> node, int level) {
        node.put("HierarchyLevel", level);
        List<Map<String, Object>> reportees = (List<Map<String, Object>>) node.get("Reportees");
        for (Map<String, Object> child : reportees) {
            setLevels(child, level + 1);
        }
    }

    @SuppressWarnings("unchecked")
    public Map<String, Object> addHoliday(Map<String, Object> model) {
        String holidayType = model.get("HolidayType") != null ? model.get("HolidayType").toString() : "";

        if ("Weekly Holidays".equals(holidayType)) {
            Object dayObj = model.get("Day");
            Object locationIdObj = model.get("LocationId");
            if (dayObj == null || locationIdObj == null) {
                throw new RuntimeException("Day and LocationId are required for weekly holidays.");
            }

            List<Object> dayList;
            if (dayObj instanceof List) {
                dayList = (List<Object>) dayObj;
            } else {
                dayList = List.of(dayObj);
            }

            List<Object> locationIdList;
            if (locationIdObj instanceof List) {
                locationIdList = (List<Object>) locationIdObj;
            } else {
                locationIdList = List.of(locationIdObj);
            }

            String combinedDays = String.join(", ", dayList.stream().map(Object::toString).toArray(String[]::new));

            Object locationListObj = model.get("Location");

            for (int i = 0; i < locationIdList.size(); i++) {
                int locId = Integer.parseInt(locationIdList.get(i).toString());
                String locName = "";
                if (locationListObj instanceof List) {
                    List<Object> locationList = (List<Object>) locationListObj;
                    if (i < locationList.size()) {
                        locName = locationList.get(i).toString();
                    }
                }

                WeekHoliday exists = weekHolidayRepository.findByDayAndLocationIdAndStatus(combinedDays, locId, "Active");
                if (exists != null) {
                    throw new RuntimeException("Weekly holiday for '" + combinedDays + "' already exists at location '" + locName + "'.");
                }

                WeekHoliday weekHoliday = new WeekHoliday();
                weekHoliday.setDay(combinedDays);
                weekHoliday.setCreatedBy(model.get("Created_By") != null ? Integer.parseInt(model.get("Created_By").toString()) : 0);
                weekHoliday.setCreatedDate(new Date());
                weekHoliday.setStatus(model.get("Status") != null ? model.get("Status").toString() : "Active");
                weekHoliday.setLocationId(locId);
                weekHoliday.setTitle(model.get("Title") != null ? model.get("Title").toString() : "");
                weekHoliday.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
                weekHoliday.setLocation(locName);
                weekHoliday.setHolidayType(holidayType);
                weekHoliday.setYear(model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : null);
                weekHolidayRepository.save(weekHoliday);
            }

            Map<String, Object> result = new HashMap<>();
            result.put("msg", "Weekly Holidays Created Successfully");
            return result;
        } else {
            Object holidayLocationIdObj = model.get("HolidayLocationId");
            if (holidayLocationIdObj == null) {
                throw new RuntimeException("HolidayLocationId is required.");
            }

            List<Object> holidayLocationIdList;
            if (holidayLocationIdObj instanceof List) {
                holidayLocationIdList = (List<Object>) holidayLocationIdObj;
            } else {
                holidayLocationIdList = List.of(holidayLocationIdObj);
            }

            Object holidayLocationObj = model.get("HolidayLocation");
            String dateStr = model.get("Date") != null ? model.get("Date").toString() : "";

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            Date holidayDate;
            try {
                holidayDate = sdf.parse(dateStr);
            } catch (Exception e) {
                throw new RuntimeException("Invalid date format: " + dateStr);
            }

            for (int i = 0; i < holidayLocationIdList.size(); i++) {
                int locId = Integer.parseInt(holidayLocationIdList.get(i).toString());
                String locName = "";
                if (holidayLocationObj instanceof List) {
                    List<Object> holidayLocationList = (List<Object>) holidayLocationObj;
                    if (i < holidayLocationList.size()) {
                        locName = holidayLocationList.get(i).toString();
                    }
                }

                List<Holiday> existingHolidays = holidayRepository.findByDateAndLocationIdAndHolidayTypeAndStatus(
                        holidayDate, locId, holidayType, "Active");

                if (!existingHolidays.isEmpty()) {
                    if (existingHolidays.size() == 1) {
                        Holiday existing = existingHolidays.get(0);
                        throw new RuntimeException("Records already exists");
                    } else {
                        throw new RuntimeException("Records already exists");
                    }
                }

                Holiday newHoliday = new Holiday();
                newHoliday.setTitle(model.get("Title") != null ? model.get("Title").toString() : "");
                newHoliday.setDate(holidayDate);
                newHoliday.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
                newHoliday.setLocationId(locId);
                newHoliday.setCreatedBy(model.get("Created_By") != null ? Integer.parseInt(model.get("Created_By").toString()) : 0);
                newHoliday.setCreatedDate(new Date());
                newHoliday.setStatus(model.get("Status") != null ? model.get("Status").toString() : "Active");
                newHoliday.setYear(model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : null);
                newHoliday.setHolidayType(holidayType);
                newHoliday.setLocation(locName);
                holidayRepository.save(newHoliday);
            }

            Map<String, Object> result = new HashMap<>();
            result.put("msg", " Holidays Created Successfully");
            return result;
        }
    }

    public Map<String, Object> getHolidayById(Map<String, Object> model) {
        return Map.of("HolidayId", model.get("HolidayId"), "HolidayName", "Sample Holiday", "StatusCode", 200);
    }

    public Map<String, Object> createWeekHoliday(Map<String, Object> model) {
        return Map.of("msg", "Added", "StatusCode", 200);
    }

    @SuppressWarnings("unchecked")
    public Map<String, Object> updateWeekHoliday(Map<String, Object> model) {
        Object modifiedByObj = model.get("Modified_By");
        Integer modifiedBy = modifiedByObj != null ? Integer.parseInt(modifiedByObj.toString()) : 0;
        if (modifiedBy == 0) {
            throw new RuntimeException("Invalid Modified_By ID.");
        }

        Object locationIdObj = model.get("LocationId");
        if (locationIdObj == null) {
            throw new RuntimeException("LocationId list is required.");
        }
        List<Integer> selectedLocationIds = new ArrayList<>();
        if (locationIdObj instanceof List) {
            for (Object o : (List<Object>) locationIdObj) {
                selectedLocationIds.add(o instanceof Number ? ((Number) o).intValue() : Integer.parseInt(o.toString()));
            }
        } else {
            selectedLocationIds.add(Integer.parseInt(locationIdObj.toString()));
        }
        if (selectedLocationIds.isEmpty()) {
            throw new RuntimeException("LocationId list is required.");
        }

        Object dayObj = model.get("Day");
        if (dayObj == null) {
            throw new RuntimeException("Day list is required.");
        }
        List<Object> dayList;
        if (dayObj instanceof List) {
            dayList = (List<Object>) dayObj;
        } else {
            dayList = List.of(dayObj);
        }
        if (dayList.isEmpty()) {
            throw new RuntimeException("Day list is required.");
        }

        List<String> distinctDays = dayList.stream()
            .map(Object::toString)
            .distinct()
            .collect(Collectors.toList());
        String combinedDays = String.join(", ", distinctDays);

        Object locationListObj = model.get("Location");
        List<Object> locationList = locationListObj instanceof List ? (List<Object>) locationListObj : new ArrayList<>();

        Integer year = model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : null;
        String title = model.get("Title") != null ? model.get("Title").toString() : "";

        List<WeekHoliday> existingWeekHolidays = weekHolidayRepository.findAll().stream()
            .filter(w -> year != null && year.equals(w.getYear()))
            .filter(w -> title.equals(w.getTitle() != null ? w.getTitle() : ""))
            .collect(Collectors.toList());

        List<WeekHoliday> holidaysToRemove = existingWeekHolidays.stream()
            .filter(w -> w.getLocationId() != null && !selectedLocationIds.contains(w.getLocationId()))
            .collect(Collectors.toList());

        for (WeekHoliday h : holidaysToRemove) {
            weekHolidayRepository.delete(h);
        }

        for (int i = 0; i < selectedLocationIds.size(); i++) {
            int locId = selectedLocationIds.get(i);
            String locName = i < locationList.size() ? locationList.get(i).toString() : "";

            WeekHoliday existing = existingWeekHolidays.stream()
                .filter(w -> w.getLocationId() != null && w.getLocationId() == locId)
                .findFirst().orElse(null);

            if (existing != null) {
                existing.setDay(combinedDays);
                existing.setTitle(title);
                existing.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
                existing.setYear(year);
                existing.setStatus(model.get("Status") != null ? model.get("Status").toString() : "Active");
                existing.setModifiedBy(modifiedBy);
                existing.setModifiedDate(new Date());
                existing.setLocation(locName);
                weekHolidayRepository.save(existing);
            } else {
                WeekHoliday newWeekHoliday = new WeekHoliday();
                newWeekHoliday.setDay(combinedDays);
                newWeekHoliday.setLocationId(locId);
                newWeekHoliday.setLocation(locName);
                newWeekHoliday.setTitle(title);
                newWeekHoliday.setDescription(model.get("Description") != null ? model.get("Description").toString() : "");
                newWeekHoliday.setYear(year);
                newWeekHoliday.setStatus(model.get("Status") != null ? model.get("Status").toString() : "Active");
                newWeekHoliday.setCreatedBy(model.get("Created_By") != null ? Integer.parseInt(model.get("Created_By").toString()) : 0);
                newWeekHoliday.setCreatedDate(new Date());
                newWeekHoliday.setModifiedBy(modifiedBy);
                newWeekHoliday.setModifiedDate(new Date());
                weekHolidayRepository.save(newWeekHoliday);
            }
        }

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Week Holidays updated successfully");
        return result;
    }

    @SuppressWarnings("unchecked")
    public Map<String, Object> deleteWeekHoliday(Map<String, Object> model) {
        Object weekDayIdObj = model.get("WeekDay_ID");
        if (weekDayIdObj == null) {
            throw new RuntimeException("Invalid WeekDay_ID list.");
        }

        List<Integer> weekDayIdList = new ArrayList<>();
        if (weekDayIdObj instanceof List) {
            for (Object o : (List<Object>) weekDayIdObj) {
                weekDayIdList.add(o instanceof Number ? ((Number) o).intValue() : Integer.parseInt(o.toString()));
            }
        } else {
            weekDayIdList.add(Integer.parseInt(weekDayIdObj.toString()));
        }
        if (weekDayIdList.isEmpty()) {
            throw new RuntimeException("Invalid WeekDay_ID list.");
        }

        List<WeekHoliday> holidays = weekHolidayRepository.findAllById(weekDayIdList).stream()
            .filter(w -> "Active".equals(w.getStatus()))
            .collect(Collectors.toList());

        if (holidays.isEmpty()) {
            throw new RuntimeException("No matching Week Holidays found.");
        }

        Object modifiedByObj = model.get("Modified_By");
        Integer modifiedBy = modifiedByObj != null ? Integer.parseInt(modifiedByObj.toString()) : 0;
        if (modifiedBy == 0) {
            Object loginIdObj = model.get("LoginId");
            if (loginIdObj != null) {
                modifiedBy = Integer.parseInt(loginIdObj.toString());
            }
        }

        for (WeekHoliday holiday : holidays) {
            holiday.setStatus("Inactive");
            holiday.setModifiedBy(modifiedBy);
            holiday.setModifiedDate(new Date());
            weekHolidayRepository.save(holiday);
        }

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Week Holiday(s) Deleted Successfully");
        return result;
    }

    public List<Map<String, Object>> getAllWeekHolidays(Map<String, Object> model) {
        Object loginIdObj = model.get("LoginId");
        Integer loginId = loginIdObj != null ? Integer.parseInt(loginIdObj.toString()) : 0;
        if (loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        List<WeekHoliday> holidays = weekHolidayRepository.findByStatusOrderByWeekDayIdDesc("Active");

        if (holidays == null || holidays.isEmpty()) {
            throw new RuntimeException("No Week Holidays Found");
        }

        Map<String, List<WeekHoliday>> grouped = new LinkedHashMap<>();
        for (WeekHoliday h : holidays) {
            String title = h.getTitle() != null ? h.getTitle() : "";
            String desc = h.getDescription() != null ? h.getDescription() : "";
            String status = h.getStatus() != null ? h.getStatus() : "";
            String key = title + "|" + desc + "|" + status;
            grouped.computeIfAbsent(key, k -> new ArrayList<>()).add(h);
        }

        List<Map<String, Object>> result = new ArrayList<>();
        for (Map.Entry<String, List<WeekHoliday>> entry : grouped.entrySet()) {
            List<WeekHoliday> group = entry.getValue();
            WeekHoliday first = group.get(0);

            List<Integer> weekDayIds = group.stream()
                .map(WeekHoliday::getWeekDayId)
                .filter(Objects::nonNull)
                .distinct()
                .collect(Collectors.toList());

            List<String> days = group.stream()
                .map(WeekHoliday::getDay)
                .filter(Objects::nonNull)
                .distinct()
                .collect(Collectors.toList());

            List<Integer> locationIds = group.stream()
                .map(WeekHoliday::getLocationId)
                .filter(Objects::nonNull)
                .distinct()
                .collect(Collectors.toList());

            List<String> locations = group.stream()
                .map(WeekHoliday::getLocation)
                .filter(Objects::nonNull)
                .distinct()
                .collect(Collectors.toList());

            Map<String, Object> item = new LinkedHashMap<>();
            item.put("WeekDay_ID", weekDayIds);
            item.put("Day", days);
            item.put("Title", first.getTitle());
            item.put("Description", first.getDescription());
            item.put("Year", first.getYear() != null ? first.getYear() : 2025);
            item.put("Status", first.getStatus());
            item.put("Created_By", first.getCreatedBy() != null ? first.getCreatedBy() : 0);
            item.put("Created_Date", first.getCreatedDate());
            item.put("Modified_By", first.getModifiedBy());
            item.put("Modified_Date", first.getModifiedDate());
            item.put("LocationId", locationIds);
            item.put("Location", locations);
            result.add(item);
        }

        return result;
    }

    public Map<String, Object> getWeekHolidayById(Map<String, Object> model) {
        return Map.of("WeekHolidayId", model.get("WeekHolidayId"), "WeekDay", "Sunday", "StatusCode", 200);
    }

    public Map<String, Object> getGradeWiseHierarchy(Map<String, Object> model) {
        return Map.of("Success", true, "Data", Map.of("GradeWise", new ArrayList<>(), "Summary", Map.of()), "StatusCode", 200);
    }

    public Map<String, Object> fetchAttendance() {
        return Map.of("StatusCode", 200, "Message", "Attendance fetched successfully for yesterday!");
    }

    public Map<String, Object> cfLeaveCredits() {
        return Map.of("StatusCode", 200, "Message", "CL and EL Credited and Carry forwarded successfully for Today!");
    }

    public Map<String, Object> addVendorList(Map<String, Object> model) {
        return Map.of("msg", "Added", "StatusCode", 200);
    }

    public Map<String, Object> addProjectList(Map<String, Object> model) {
        return Map.of("msg", "Added", "StatusCode", 200);
    }

    public List<Map<String, Object>> getEmpHolidays(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        result.add(Map.of("HolidayId", 1, "HolidayName", "Sample Holiday", "HolidayDate", "2025-01-01", "StatusCode", 200));
        return result;
    }

    public List<Map<String, Object>> getAllEmpAccDetails(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == 0) throw new RuntimeException("EmpId is Missing");

        List<EmployeeAccDetail> accs = employeeAccDetailRepository.findByIsActiveAndIsDeletedOrderByCreatedDateDesc(true, false);
        return accs.stream().map(a -> {
            Map<String, Object> m = new HashMap<>();
            m.put("AccId", a.getAccId());
            m.put("EmpId", a.getEmpId());
            Integer empId = a.getEmpId();
            if (empId != null) {
                EmployeeMaster emp = employeeMasterRepository.findById(empId).orElse(null);
                m.put("EmpCode", emp != null ? emp.getEmpCode() : "");
                m.put("EmpName", (emp != null ? emp.getFirstName() : "")
                    + " " + (emp != null ? emp.getMiddleName() : "")
                    + " " + (emp != null ? emp.getLastName() : ""));
            } else {
                m.put("EmpCode", "");
                m.put("EmpName", "");
            }
            m.put("BankName", a.getBankName());
            m.put("BranchName", a.getBranchName());
            m.put("IFSCCode", a.getIfscCode());
            m.put("AccHolderName", a.getAccHolderName());
            m.put("AccNo", a.getAccNo());
            m.put("PFNo", a.getPfNo());
            m.put("ESIInsuranceNo", a.getEsiInsuranceNo());
            m.put("HealthInsuranceNo", a.getHealthInsuranceNo());
            m.put("PANNo", a.getPanNo());
            m.put("UANNo", a.getUanNo());
            m.put("AadharNo", a.getAadharNo());
            m.put("MobileNo", a.getMobileNo());
            m.put("Status", a.getStatus());
            m.put("CreatedBy", a.getCreatedBy());
            m.put("CreatedDate", a.getCreatedDate());
            m.put("LastUpdatedBy", a.getLastUpdatedBy());
            m.put("LastUpdatedDate", a.getLastUpdatedDate());
            m.put("IsActive", a.getIsActive());
            m.put("IsUpdated", a.getIsUpdated());
            m.put("IsDeleted", a.getIsDeleted());
            return m;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getAllEmpCareerDetails() {
        List<EmployeeCareerDetail> careers = employeeCareerDetailRepository.findByIsActiveAndIsDeletedOrderByCareerIdDesc(true, false);
        return careers.stream().map(c -> {
            Map<String, Object> m = new HashMap<>();
            m.put("CareerId", c.getCareerId());
            m.put("EmpId", c.getEmpId());
            m.put("Company", c.getCompany());
            m.put("Designation", c.getDesignation());
            m.put("FromDate", convertToJsonDate(c.getFromDate()));
            m.put("ToDate", convertToJsonDate(c.getToDate()));
            m.put("Experience", c.getExperience());
            m.put("CTC", c.getCtc());
            m.put("Reason", c.getReason());
            return m;
        }).collect(Collectors.toList());
    }

    // =================== Calendar Year CRUD ===================

    public List<Map<String, Object>> getAllCalendarYear(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<Object[]> rows = entityManager.createNativeQuery(
            "SELECT Id, Year, Status, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate, IsActive, IsUpdated, IsDeleted FROM FinanceMaster WHERE IsActive = 1 AND IsDeleted = 0 AND Status = 1 ORDER BY CreatedDate DESC")
            .getResultList();

        if (rows.isEmpty()) throw new RuntimeException("Calendar Year Detail Not Found");

        List<Map<String, Object>> result = new ArrayList<>();
        for (Object[] row : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("LoginId", loginId);
            m.put("Id", row[0]);
            m.put("Year", row[1]);
            m.put("Status", row[2]);
            m.put("CreatedBy", row[3]);
            m.put("CreatedDate", row[4] != null ? "/Date(" + ((java.util.Date) row[4]).getTime() + ")/" : null);
            m.put("LastUpdatedBy", row[5]);
            m.put("LastUpdatedDate", row[6] != null ? "/Date(" + ((java.util.Date) row[6]).getTime() + ")/" : null);
            m.put("IsActive", row[7]);
            m.put("IsUpdated", row[8]);
            m.put("IsDeleted", row[9]);
            result.add(m);
        }
        return result;
    }

    public Map<String, Object> getCalendarYear(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer id = parseInteger(model.get("Id"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");
        if (id == 0) throw new RuntimeException("Id is Missing");

        List<Object[]> rows = entityManager.createNativeQuery(
            "SELECT Id, Year, Status, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate, IsActive, IsUpdated, IsDeleted FROM FinanceMaster WHERE Id = ?1 AND IsActive = 1 AND IsDeleted = 0 AND Status = 1")
            .setParameter(1, id)
            .getResultList();

        if (rows.isEmpty()) throw new RuntimeException("Calendar Year Details Not Found");

        Object[] row = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("LoginId", loginId);
        m.put("Id", row[0]);
        m.put("Year", row[1]);
        m.put("Status", row[2]);
        m.put("CreatedBy", row[3]);
        m.put("CreatedDate", row[4] != null ? "/Date(" + ((java.util.Date) row[4]).getTime() + ")/" : null);
        m.put("LastUpdatedBy", row[5]);
        m.put("LastUpdatedDate", row[6] != null ? "/Date(" + ((java.util.Date) row[6]).getTime() + ")/" : null);
        m.put("IsActive", row[7]);
        m.put("IsUpdated", row[8]);
        m.put("IsDeleted", row[9]);
        return m;
    }

    public Map<String, Object> addCalendarYear(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer year = parseInteger(model.get("Year"));
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (year == 0) throw new RuntimeException("Year is Missing");

        List<Object[]> existing = entityManager.createNativeQuery(
            "SELECT Id FROM FinanceMaster WHERE Year = ?1 AND IsActive = 1 AND IsDeleted = 0 AND Status = 1")
            .setParameter(1, year)
            .getResultList();

        if (!existing.isEmpty()) throw new RuntimeException("Calendar Year Details Already Exists");

        entityManager.createNativeQuery(
            "INSERT INTO FinanceMaster (Year, Status, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate, IsActive, IsUpdated, IsDeleted) VALUES (?1, 1, ?2, GETDATE(), ?3, GETDATE(), 1, 0, 0)")
            .setParameter(1, year)
            .setParameter(2, loginId)
            .setParameter(3, loginId)
            .executeUpdate();

        result.put("Status", 200);
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateCalendarYear(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer id = parseInteger(model.get("Id"));
        Integer year = parseInteger(model.get("Year"));
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Id is Missing");

        List<Object[]> existing = entityManager.createNativeQuery(
            "SELECT Id FROM FinanceMaster WHERE Id = ?1 AND IsActive = 1 AND IsDeleted = 0 AND Status = 1")
            .setParameter(1, id)
            .getResultList();

        if (existing.isEmpty()) throw new RuntimeException("Calendar Year Details Not Found");

        entityManager.createNativeQuery(
            "UPDATE FinanceMaster SET Year = ?1, LastUpdatedBy = ?2, LastUpdatedDate = GETDATE(), IsUpdated = 1 WHERE Id = ?3")
            .setParameter(1, year)
            .setParameter(2, loginId)
            .setParameter(3, id)
            .executeUpdate();

        result.put("Status", 200);
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteCalendarYear(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer id = parseInteger(model.get("Id"));
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");
        if (id == 0) throw new RuntimeException("Id is Missing");

        List<Object[]> existing = entityManager.createNativeQuery(
            "SELECT Id FROM FinanceMaster WHERE Id = ?1 AND IsActive = 1 AND IsDeleted = 0 AND Status = 1")
            .setParameter(1, id)
            .getResultList();

        if (existing.isEmpty()) throw new RuntimeException("Calendar Year Details Not Found");

        entityManager.createNativeQuery(
            "UPDATE FinanceMaster SET IsUpdated = 1, IsDeleted = 1, LastUpdatedBy = ?1, LastUpdatedDate = GETDATE() WHERE Id = ?2")
            .setParameter(1, loginId)
            .setParameter(2, id)
            .executeUpdate();

        result.put("Status", 200);
        result.put("msg", "Deleted");
        return result;
    }

//    public Map<String, Object> getCalendarYear(Map<String, Object> model) {
//        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
//        Integer id = model.get("Id") != null ? Integer.parseInt(model.get("Id").toString()) : 0;
//
//        if (loginId == null || loginId == 0) {
//            throw new RuntimeException("LoginId is Missing");
//        }
//
//        Optional<CalendarYearMaster> yOpt = calendarYearMasterRepository.findById(id);
//        if (yOpt.isEmpty() || (yOpt.get().getIsDeleted() != null && yOpt.get().getIsDeleted())
//            || (yOpt.get().getStatus() != null && !yOpt.get().getStatus())) {
//            throw new RuntimeException("Calendar Year Details Not Found");
//        }
//
//        CalendarYearMaster y = yOpt.get();
//        Map<String, Object> m = new LinkedHashMap<>();
//        m.put("LoginId", loginId);
//        m.put("Id", y.getId());
//        m.put("Year", y.getYear());
//        m.put("Status", y.getStatus());
//        m.put("CreatedBy", y.getCreatedBy());
//        m.put("CreatedDate", y.getCreatedDate() != null ? "\\/Date(" + y.getCreatedDate().getTime() + ")\\/" : null);
//        m.put("LastUpdatedBy", y.getLastUpdatedBy());
//        m.put("LastUpdatedDate", y.getLastUpdatedDate() != null ? "\\/Date(" + y.getLastUpdatedDate().getTime() + ")\\/" : null);
//        m.put("IsActive", y.getIsActive());
//        m.put("IsUpdated", y.getIsUpdated());
//        m.put("IsDeleted", y.getIsDeleted());
//        return m;
//    }

//    public Map<String, Object> addCalendarYear(Map<String, Object> model) {
//        Map<String, Object> result = new LinkedHashMap<>();
//        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
//        Integer year = model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : 0;
//
//        if (loginId == null || loginId == 0) {
//            throw new RuntimeException("LoginId is Mismatching");
//        }
//
//        boolean exists = calendarYearMasterRepository.findByIsActiveAndIsDeletedAndStatus(true, false, true).stream()
//            .anyMatch(y -> y.getYear() != null && y.getYear().equals(year));
//
//        if (exists) {
//            throw new RuntimeException("Calendar Year Details Already Exists");
//        }
//
//        CalendarYearMaster ym = new CalendarYearMaster();
//        ym.setYear(year);
//        ym.setStatus(true);
//        ym.setCreatedBy(loginId);
//        ym.setCreatedDate(new Date());
//        ym.setLastUpdatedBy(loginId);
//        ym.setLastUpdatedDate(new Date());
//        ym.setIsActive(true);
//        ym.setIsUpdated(false);
//        ym.setIsDeleted(false);
//        calendarYearMasterRepository.save(ym);
//
//        result.put("Status", 200);
//        result.put("msg", "Added");
//        return result;
//    }

//    public Map<String, Object> updateCalendarYear(Map<String, Object> model) {
//        Map<String, Object> result = new LinkedHashMap<>();
//        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
//        Integer id = model.get("Id") != null ? Integer.parseInt(model.get("Id").toString()) : 0;
//
//        if (loginId == null || loginId == 0) {
//            throw new RuntimeException("LoginId is Mismatching");
//        }
//
//        Optional<CalendarYearMaster> yOpt = calendarYearMasterRepository.findById(id);
//        if (yOpt.isEmpty() || (yOpt.get().getIsDeleted() != null && yOpt.get().getIsDeleted())
//            || (yOpt.get().getStatus() != null && !yOpt.get().getStatus())) {
//            throw new RuntimeException("Calendar Year Details Not Found");
//        }
//
//        CalendarYearMaster ym = yOpt.get();
//        ym.setYear(model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : ym.getYear());
//        ym.setLastUpdatedBy(loginId);
//        ym.setLastUpdatedDate(new Date());
//        ym.setIsUpdated(true);
//        calendarYearMasterRepository.save(ym);
//
//        result.put("Status", 200);
//        result.put("msg", "Updated");
//        return result;
//    }

//    public Map<String, Object> deleteCalendarYear(Map<String, Object> model) {
//        Map<String, Object> result = new LinkedHashMap<>();
//        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
//        Integer id = model.get("Id") != null ? Integer.parseInt(model.get("Id").toString()) : 0;
//
//        if (loginId == null || loginId == 0) {
//            throw new RuntimeException("LoginId is Missing");
//        }
//
//        Optional<CalendarYearMaster> yOpt = calendarYearMasterRepository.findById(id);
//        if (yOpt.isEmpty() || (yOpt.get().getIsDeleted() != null && yOpt.get().getIsDeleted())
//            || (yOpt.get().getStatus() != null && !yOpt.get().getStatus())) {
//            throw new RuntimeException("Calendar Year Details Not Found");
//        }
//
//        CalendarYearMaster ym = yOpt.get();
//        ym.setIsUpdated(true);
//        ym.setIsDeleted(true);
//        ym.setLastUpdatedBy(loginId);
//        ym.setLastUpdatedDate(new Date());
//        calendarYearMasterRepository.save(ym);
//
//        result.put("Status", 200);
//        result.put("msg", "Deleted");
//        return result;
//    }

    // =================== Financial Year CRUD ===================

    public List<Map<String, Object>> getAllFinancialYear(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        List<FinancialYearMaster> years = financialYearMasterRepository.findByIsActiveAndIsDeleted(true, false);

        if (years.isEmpty()) {
            throw new RuntimeException("Financial Year Detail Not Found");
        }

        return years.stream().filter(y -> y.getStatus() != null && y.getStatus()).map(y -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("LoginId", loginId);
            m.put("Id", y.getYearId());
            m.put("YearId", y.getYearId());
            m.put("FinancialYear", y.getFinancialYear());
            m.put("Status", y.getStatus());
            m.put("CreatedBy", y.getCreatedBy());
            m.put("CreatedDate", y.getCreatedDate() != null ? "\\/Date(" + y.getCreatedDate().getTime() + ")\\/" : null);
            m.put("LastUpdatedBy", y.getLastUpdatedBy());
            m.put("LastUpdatedDate", y.getLastUpdatedDate() != null ? "\\/Date(" + y.getLastUpdatedDate().getTime() + ")\\/" : null);
            m.put("IsActive", y.getIsActive());
            m.put("IsUpdated", y.getIsUpdated());
            m.put("IsDeleted", y.getIsDeleted());
            return m;
        }).collect(Collectors.toList());
    }

    public Map<String, Object> getFinancialYear(Map<String, Object> model) {
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer yearId = model.get("YearId") != null ? Integer.parseInt(model.get("YearId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Optional<FinancialYearMaster> yOpt = financialYearMasterRepository.findById(yearId);
        if (yOpt.isEmpty()
            || (yOpt.get().getIsActive() != null && !yOpt.get().getIsActive())
            || (yOpt.get().getIsDeleted() != null && yOpt.get().getIsDeleted())
            || (yOpt.get().getStatus() != null && !yOpt.get().getStatus())) {
            throw new RuntimeException("Financial Year Details Not Found");
        }

        FinancialYearMaster y = yOpt.get();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("LoginId", loginId);
        m.put("Id", y.getYearId());
        m.put("YearId", y.getYearId());
        m.put("FinancialYear", y.getFinancialYear());
        m.put("Status", y.getStatus());
        m.put("CreatedBy", y.getCreatedBy());
        m.put("CreatedDate", y.getCreatedDate() != null ? "\\/Date(" + y.getCreatedDate().getTime() + ")\\/" : null);
        m.put("LastUpdatedBy", y.getLastUpdatedBy());
        m.put("LastUpdatedDate", y.getLastUpdatedDate() != null ? "\\/Date(" + y.getLastUpdatedDate().getTime() + ")\\/" : null);
        m.put("IsActive", y.getIsActive());
        m.put("IsUpdated", y.getIsUpdated());
        m.put("IsDeleted", y.getIsDeleted());
        return m;
    }

    public Map<String, Object> addFinancialYear(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        String financialYear = model.get("FinancialYear") != null ? model.get("FinancialYear").toString() : "";

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        boolean exists = financialYearMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .anyMatch(y -> y.getFinancialYear() != null && y.getFinancialYear().equals(financialYear)
                && y.getStatus() != null && y.getStatus());

        if (exists) {
            throw new RuntimeException("Financial Year Details Already Exists");
        }

        FinancialYearMaster fym = new FinancialYearMaster();
        fym.setFinancialYear(financialYear);
        fym.setStatus(true);
        fym.setCreatedBy(loginId);
        fym.setCreatedDate(new Date());
        fym.setLastUpdatedBy(loginId);
        fym.setLastUpdatedDate(new Date());
        fym.setIsActive(true);
        fym.setIsUpdated(false);
        fym.setIsDeleted(false);
        financialYearMasterRepository.save(fym);

        result.put("Status", 200);
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateFinancialYear(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = model.get("LoginId") != null ? Integer.parseInt(model.get("LoginId").toString()) : 0;
        Integer yearId = model.get("YearId") != null ? Integer.parseInt(model.get("YearId").toString()) : 0;

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Mismatching");
        }

        Optional<FinancialYearMaster> yOpt = financialYearMasterRepository.findById(yearId);
        if (yOpt.isEmpty()
            || (yOpt.get().getIsActive() != null && !yOpt.get().getIsActive())
            || (yOpt.get().getIsDeleted() != null && yOpt.get().getIsDeleted())
            || (yOpt.get().getStatus() != null && !yOpt.get().getStatus())) {
            throw new RuntimeException("Financial Year Details Not Found");
        }

        FinancialYearMaster fym = yOpt.get();
        fym.setFinancialYear(model.get("FinancialYear") != null ? model.get("FinancialYear").toString() : fym.getFinancialYear());
        fym.setLastUpdatedBy(loginId);
        fym.setLastUpdatedDate(new Date());
        fym.setIsUpdated(true);
        financialYearMasterRepository.save(fym);

        result.put("Status", 200);
        result.put("msg", "Updated");
        return result;
    }

    public Map<String, Object> deleteFinancialYear(Map<String, Object> model) {
        Map<String, Object> result = new LinkedHashMap<>();
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer yearId = parseInteger(model.get("YearId"));

        if (loginId == null || loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }
        if (yearId == null || yearId == 0) {
            throw new RuntimeException("Financial Year Details Not Found");
        }

        Optional<FinancialYearMaster> yOpt = financialYearMasterRepository.findById(yearId);
        if (yOpt.isEmpty()
            || (yOpt.get().getIsActive() != null && !yOpt.get().getIsActive())
            || (yOpt.get().getIsDeleted() != null && yOpt.get().getIsDeleted())
            || (yOpt.get().getStatus() != null && !yOpt.get().getStatus())) {
            throw new RuntimeException("Financial Year Details Not Found");
        }

        FinancialYearMaster fym = yOpt.get();
        fym.setIsUpdated(true);
        fym.setIsDeleted(true);
        fym.setLastUpdatedBy(loginId);
        fym.setLastUpdatedDate(new Date());
        financialYearMasterRepository.save(fym);

        result.put("Status", 200);
        result.put("msg", "Deleted");
        return result;
    }

    public List<Map<String, Object>> getAllFinanceMaster(Map<String, Object> model) {
        List<Object[]> rows = entityManager.createNativeQuery(
            "SELECT Id, Year FROM FinanceMaster ORDER BY Year")
            .getResultList();

        if (rows.isEmpty()) throw new RuntimeException("No Finance Year records found.");

        List<Map<String, Object>> result = new ArrayList<>();
        for (Object[] row : rows) {
            Map<String, Object> m = new HashMap<>();
            m.put("Id", row[0]);
            m.put("Year", row[1]);
            result.add(m);
        }
        return result;
    }
    public List<Map<String, Object>> getEmpProjects(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> getEmpEducationDetails(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        if (empId == 0) return new ArrayList<>();
        return employeeEducationRepository.findByEmpIdAndIsActiveAndIsDeleted(empId, true, false).stream().map(e -> {
            Map<String, Object> m = new HashMap<>();
            m.put("Id", e.getId());
            m.put("EmpId", e.getEmpId());
            m.put("DocId", e.getDocId());
            m.put("School", e.getSchool());
            m.put("DegreeId", e.getDegreeId());
            m.put("Filed", e.getFiled());
            m.put("StartDate", e.getStartDate());
            m.put("EndDate", e.getEndDate());
            m.put("Grade", e.getGrade());
            m.put("Description", e.getDescription());
            String path = e.getPath();
            if (path != null && path.contains("Uploads")) {
                String[] parts = path.split("Uploads", 2);
                if (parts.length > 1) path = "Uploads" + parts[1];
            }
            m.put("Path", path != null ? path : "");
            return m;
        }).collect(Collectors.toList());
    }
    public List<Map<String, Object>> getEmpDocuments(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> getEmployeeTimeline(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> getEmployeeStatusHistory(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> exportEmployeeList(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> getEmpAddressDetails(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> getEmpPhoneDetails(Map<String, Object> model) { return new ArrayList<>(); }
    public List<Map<String, Object>> getEmpEmergencyContacts(Map<String, Object> model) { return new ArrayList<>(); }

    public List<Map<String, Object>> getEmpAccDetails(Map<String, Object> model) {
        Integer empId = parseInteger(model.get("EmpId"));
        if (empId == 0) return new ArrayList<>();
        return employeeAccDetailRepository.findByEmpIdAndIsActiveAndIsDeletedOrderByCreatedDateDesc(empId, true, false).stream().map(a -> {
            Map<String, Object> m = new HashMap<>();
            m.put("AccId", a.getAccId());
            m.put("EmpId", a.getEmpId());
            if (empId != null) {
                EmployeeMaster emp = employeeMasterRepository.findById(empId).orElse(null);
                m.put("EmpCode", emp != null ? emp.getEmpCode() : "");
                m.put("EmpName", (emp != null ? emp.getFirstName() : "")
                    + " " + (emp != null ? emp.getMiddleName() : "")
                    + " " + (emp != null ? emp.getLastName() : ""));
            } else {
                m.put("EmpCode", "");
                m.put("EmpName", "");
            }
            m.put("BankName", a.getBankName());
            m.put("BranchName", a.getBranchName());
            m.put("IFSCCode", a.getIfscCode());
            m.put("AccHolderName", a.getAccHolderName());
            m.put("AccNo", a.getAccNo());
            m.put("PFNo", a.getPfNo());
            m.put("ESIInsuranceNo", a.getEsiInsuranceNo());
            m.put("HealthInsuranceNo", a.getHealthInsuranceNo());
            m.put("PANNo", a.getPanNo());
            m.put("UANNo", a.getUanNo());
            m.put("AadharNo", a.getAadharNo());
            m.put("MobileNo", a.getMobileNo());
            m.put("Status", a.getStatus());
            m.put("CreatedBy", a.getCreatedBy());
            m.put("CreatedDate", a.getCreatedDate());
            m.put("LastUpdatedBy", a.getLastUpdatedBy());
            m.put("LastUpdatedDate", a.getLastUpdatedDate());
            m.put("IsActive", a.getIsActive());
            m.put("IsUpdated", a.getIsUpdated());
            m.put("IsDeleted", a.getIsDeleted());
            return m;
        }).collect(Collectors.toList());
    }

    public Map<String, Object> addEmpAccDetails(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer empId = parseInteger(model.get("EmpId"));
        if (loginId == 0) throw new RuntimeException("EmpId is Mismatching");

        EmployeeAccDetail acc = new EmployeeAccDetail();
        acc.setEmpId(empId);
        acc.setBankName(parseString(model.get("BankName")));
        acc.setBranchName(parseString(model.get("BranchName")));
        acc.setIfscCode(parseString(model.get("IFSCCode")));
        acc.setAccHolderName(parseString(model.get("AccHolderName")));
        acc.setAccNo(parseString(model.get("AccNo")));
        acc.setPfNo(parseString(model.get("PFNo")));
        acc.setEsiInsuranceNo(parseString(model.get("ESIInsuranceNo")));
        acc.setHealthInsuranceNo(parseString(model.get("HealthInsuranceNo")));
        acc.setPanNo(parseString(model.get("PANNo")));
        acc.setUanNo(parseString(model.get("UANNo")));
        acc.setAadharNo(parseString(model.get("AadharNo")));
        acc.setMobileNo(parseString(model.get("MobileNo")));
        acc.setStatus(true);
        acc.setCreatedBy(loginId);
        acc.setCreatedDate(new Date());
        acc.setLastUpdatedBy(loginId);
        acc.setLastUpdatedDate(new Date());
        acc.setIsActive(true);
        acc.setIsUpdated(false);
        acc.setIsDeleted(false);
        employeeAccDetailRepository.save(acc);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Added");
        return result;
    }

    public Map<String, Object> updateEmpAccDetails(Map<String, Object> model) {
        Integer loginId = parseInteger(model.get("LoginId"));
        Integer empId = parseInteger(model.get("EmpId"));
        Integer accId = parseInteger(model.get("AccId"));
        if (loginId == 0) throw new RuntimeException("EmpId is Mismatching");

        if (accId == 0) {
            EmployeeAccDetail acc = new EmployeeAccDetail();
            acc.setEmpId(empId);
            acc.setBankName(parseString(model.get("BankName")));
            acc.setBranchName(parseString(model.get("BranchName")));
            acc.setIfscCode(parseString(model.get("IFSCCode")));
            acc.setAccHolderName(parseString(model.get("AccHolderName")));
            acc.setAccNo(parseString(model.get("AccNo")));
            acc.setPfNo(parseString(model.get("PFNo")));
            acc.setEsiInsuranceNo(parseString(model.get("ESIInsuranceNo")));
            acc.setHealthInsuranceNo(parseString(model.get("HealthInsuranceNo")));
            acc.setPanNo(parseString(model.get("PANNo")));
            acc.setUanNo(parseString(model.get("UANNo")));
            acc.setAadharNo(parseString(model.get("AadharNo")));
            acc.setMobileNo(parseString(model.get("MobileNo")));
            acc.setStatus(true);
            acc.setIsActive(true);
            acc.setIsUpdated(false);
            acc.setIsDeleted(false);
            acc.setCreatedBy(loginId);
            acc.setCreatedDate(new Date());
            acc.setLastUpdatedBy(loginId);
            acc.setLastUpdatedDate(new Date());
            employeeAccDetailRepository.save(acc);

            Map<String, Object> result = new HashMap<>();
            result.put("msg", "Added");
            return result;
        } else {
            EmployeeAccDetail acc = employeeAccDetailRepository.findByEmpIdAndAccIdAndIsActiveAndIsDeleted(empId, accId, true, false);
            if (acc == null) throw new RuntimeException("Account Details Not Found");

            acc.setBankName(parseString(model.get("BankName")));
            acc.setBranchName(parseString(model.get("BranchName")));
            acc.setIfscCode(parseString(model.get("IFSCCode")));
            acc.setAccHolderName(parseString(model.get("AccHolderName")));
            acc.setAccNo(parseString(model.get("AccNo")));
            acc.setPfNo(parseString(model.get("PFNo")));
            acc.setEsiInsuranceNo(parseString(model.get("ESIInsuranceNo")));
            acc.setHealthInsuranceNo(parseString(model.get("HealthInsuranceNo")));
            acc.setPanNo(parseString(model.get("PANNo")));
            acc.setUanNo(parseString(model.get("UANNo")));
            acc.setAadharNo(parseString(model.get("AadharNo")));
            acc.setMobileNo(parseString(model.get("MobileNo")));
            acc.setStatus(true);
            acc.setIsActive(true);
            acc.setIsUpdated(true);
            acc.setIsDeleted(false);
            acc.setLastUpdatedBy(loginId);
            acc.setLastUpdatedDate(new Date());
            employeeAccDetailRepository.save(acc);

            Map<String, Object> result = new HashMap<>();
            result.put("msg", "Updated");
            return result;
        }
    }

    public Map<String, Object> deleteEmpAccDetails(Map<String, Object> model) {
        Integer accId = parseInteger(model.get("AccId"));
        if (accId == 0) throw new RuntimeException("AccId is Missing");

        Optional<EmployeeAccDetail> opt = employeeAccDetailRepository.findById(accId);
        if (opt.isEmpty()) throw new RuntimeException("Account detail not found");

        EmployeeAccDetail acc = opt.get();
        acc.setIsDeleted(true);
        acc.setIsUpdated(true);
        acc.setLastUpdatedDate(new Date());
        employeeAccDetailRepository.save(acc);

        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Deleted");
        return result;
    }

    private List<Integer> parseAuthorisedEntities(String str) {
        List<Integer> result = new ArrayList<>();
        if (str == null || str.isEmpty()) return result;
        String cleaned = str.trim();
        if (cleaned.startsWith("[")) {
            cleaned = cleaned.substring(1);
        }
        if (cleaned.endsWith("]")) {
            cleaned = cleaned.substring(0, cleaned.length() - 1);
        }
        if (cleaned.isEmpty()) return result;
        for (String e : cleaned.split(",")) {
            String trimmed = e.trim();
            if (!trimmed.isEmpty()) {
                result.add(Integer.parseInt(trimmed));
            }
        }
        return result;
         }
     
     /**
      * Cleans time string by removing extra decimal precision (e.g., "18:56:36.0000000" -> "18:56:36")
      * @param timeString The time string to clean
      * @return Cleaned time string
      */
    private String cleanTimeString(String timeString) {
        if (timeString == null || timeString.isEmpty()) {
            return timeString;
        }
        if (timeString.contains(".")) {
            return timeString.split("\\.")[0];
        }
        return timeString;
    }

    private Map<String, Object> parseTimeToObject(String timeStr) {
        Map<String, Object> result = new HashMap<>();
        if (timeStr == null || timeStr.isEmpty()) {
            result.put("Hours", 0);
            result.put("Minutes", 0);
            result.put("Seconds", 0);
            return result;
        }
        String clean = timeStr.contains(".") ? timeStr.split("\\.")[0] : timeStr;
        String[] parts = clean.split(":");
        int hours = 0, minutes = 0, seconds = 0;
        try {
            if (parts.length >= 1) hours = Integer.parseInt(parts[0]);
            if (parts.length >= 2) minutes = Integer.parseInt(parts[1]);
            if (parts.length >= 3) seconds = Integer.parseInt(parts[2]);
        } catch (NumberFormatException e) {
            // ignore
        }
        result.put("Hours", hours);
        result.put("Minutes", minutes);
        result.put("Seconds", seconds);
        return result;
    }
}
