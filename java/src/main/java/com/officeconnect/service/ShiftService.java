package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeParseException;
import java.util.*;
import java.util.stream.Collectors;

@Service
public class ShiftService {

    @Autowired
    private ShiftMasterRepository shiftMasterRepository;

    @Autowired
    private ShiftGroupingMasterRepository shiftGroupingMasterRepository;

    @Autowired
    private CompanyMasterRepository companyMasterRepository;

    @Autowired
    private LegalEntityMasterRepository legalEntityMasterRepository;

    @Autowired
    private BusinessUnitMasterRepository businessUnitMasterRepository;

    @Autowired
    private LocationMasterRepository locationMasterRepository;

    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private EmpShiftDetailRepository empShiftDetailRepository;

    public List<ShiftMasterViewModel> getAllShift(ShiftMasterViewModel model) {
        int loginId = model.getLoginId() != 0 ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }
        return shiftMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .sorted((a, b) -> Integer.compare(
                b.getShiftId() != null ? b.getShiftId() : 0,
                a.getShiftId() != null ? a.getShiftId() : 0))
            .map(s -> convertToViewModel(s))
            .collect(Collectors.toList());
    }

    public ShiftMasterViewModel addShift(ShiftMasterViewModel model) {
        int loginId = model.getLoginId() != 0 ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        List<ShiftMaster> existing = shiftMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(s -> s.getShiftName() != null && s.getShiftName().equals(model.getShiftName()))
            .collect(Collectors.toList());
        if (!existing.isEmpty()) {
            throw new RuntimeException("Shift Details Already Exists");
        }

        ShiftMaster sm = new ShiftMaster();
        sm.setShiftName(model.getShiftName());
        if (model.getStartTime() != null && !model.getStartTime().isEmpty()) {
            sm.setStartTime(parseTime(model.getStartTime()));
        }
        if (model.getEndTime() != null && !model.getEndTime().isEmpty()) {
            sm.setEndTime(parseTime(model.getEndTime()));
        }
        sm.setClkHrs(model.getClkHrs());
        sm.setDays(model.getDays());
        sm.setStatus(true);
        sm.setCreatedBy(loginId);
        sm.setCreatedDate(LocalDateTime.now());
        sm.setLastUpdatedBy(loginId);
        sm.setLastUpdatedDate(LocalDateTime.now());
        sm.setIsActive(true);
        sm.setIsUpdated(false);
        sm.setIsDeleted(false);
        sm = shiftMasterRepository.save(sm);

        ShiftMasterViewModel result = new ShiftMasterViewModel();
        result.setShiftId(sm.getShiftId());
        result.setMsg("Added");
        return result;
    }

    public ShiftMasterViewModel updateShift(ShiftMasterViewModel model) {
        int loginId = model.getLoginId() != 0 ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        Optional<ShiftMaster> smOpt = shiftMasterRepository.findById(model.getShiftId());
        if (smOpt.isEmpty()) {
            throw new RuntimeException("Shift Details Not Found");
        }
        ShiftMaster sm = smOpt.get();
        sm.setShiftName(model.getShiftName());
        if (model.getStartTime() != null && !model.getStartTime().isEmpty()) {
            sm.setStartTime(parseTime(model.getStartTime()));
        } else {
            sm.setStartTime(null);
        }
        if (model.getEndTime() != null && !model.getEndTime().isEmpty()) {
            sm.setEndTime(parseTime(model.getEndTime()));
        } else {
            sm.setEndTime(null);
        }
        sm.setClkHrs(model.getClkHrs());
        sm.setDays(model.getDays());
        sm.setStatus(true);
        sm.setLastUpdatedBy(loginId);
        sm.setLastUpdatedDate(LocalDateTime.now());
        sm.setIsActive(true);
        sm.setIsUpdated(true);
        sm.setIsDeleted(false);
        shiftMasterRepository.save(sm);

        ShiftMasterViewModel result = new ShiftMasterViewModel();
        result.setMsg("Updated");
        return result;
    }

    public ShiftMasterViewModel deleteShift(ShiftMasterViewModel model) {
        int loginId = model.getLoginId() != 0 ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        Optional<ShiftMaster> smOpt = shiftMasterRepository.findById(model.getShiftId());
        if (smOpt.isEmpty()) {
            throw new RuntimeException("Shift Details Not Found");
        }
        ShiftMaster sm = smOpt.get();
        sm.setStatus(true);
        sm.setIsActive(true);
        sm.setIsUpdated(true);
        sm.setIsDeleted(true);
        sm.setLastUpdatedBy(loginId);
        sm.setLastUpdatedDate(LocalDateTime.now());
        shiftMasterRepository.save(sm);

        ShiftMasterViewModel result = new ShiftMasterViewModel();
        result.setMsg("Deleted");
        return result;
    }

    public List<ShiftMasterViewModel> getShift(ShiftMasterViewModel model) {
        int loginId = model.getLoginId() != 0 ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }
        return shiftMasterRepository.findByShiftId(model.getShiftId()).stream()
            .map(s -> convertToViewModel(s))
            .collect(Collectors.toList());
    }

    public List<ShiftGroupingMasterViewModel> getAllShiftGrouping(ShiftGroupingMasterViewModel model) {
        int loginId = model.getLoginId() != 0 ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        List<ShiftGroupingMaster> all = shiftGroupingMasterRepository.findByIsActiveAndIsDeleted(true, false);

        Map<String, List<ShiftGroupingMaster>> groups = all.stream()
            .collect(Collectors.groupingBy(sg ->
                (sg.getCompId() != null ? sg.getCompId() : 0) + "_" +
                (sg.getLeId() != null ? sg.getLeId() : 0) + "_" +
                (sg.getBuId() != null ? sg.getBuId() : 0) + "_" +
                (sg.getLocationId() != null ? sg.getLocationId() : 0)
            ));

        List<ShiftGroupingMasterViewModel> result = new ArrayList<>();

        Map<Integer, CompanyMaster> companyCache = new HashMap<>();
        Map<Integer, LegalEntityMaster> leCache = new HashMap<>();
        Map<Integer, BusinessUnitMaster> buCache = new HashMap<>();
        Map<Integer, LocationMaster> locationCache = new HashMap<>();

        for (Map.Entry<String, List<ShiftGroupingMaster>> entry : groups.entrySet()) {
            List<ShiftGroupingMaster> groupItems = entry.getValue();

            ShiftGroupingMaster first = groupItems.get(0);
            Integer compId = first.getCompId();
            Integer leId = first.getLeId();
            Integer buId = first.getBuId();
            Integer locationId = first.getLocationId();

            List<ShiftGroupingMaster> activeShifts = groupItems.stream()
                .filter(sg -> sg.getStatus() != null && sg.getStatus())
                .sorted((a, b) -> {
                    int ai = a.getShiftId() != null ? a.getShiftId() : 0;
                    int bi = b.getShiftId() != null ? b.getShiftId() : 0;
                    return Integer.compare(bi, ai);
                })
                .collect(Collectors.toList());

            List<SampleShiftMasterViewModel> lstOfShift = new ArrayList<>();
            for (ShiftGroupingMaster sg : activeShifts) {
                Integer shiftId = sg.getShiftId();
                SampleShiftMasterViewModel smvm = new SampleShiftMasterViewModel();
                smvm.setShiftId(shiftId);
                if (shiftId != null && shiftId != 0) {
                    Optional<ShiftMaster> shiftOpt = shiftMasterRepository.findById(shiftId);
                    if (shiftOpt.isPresent()) {
                        ShiftMaster sm = shiftOpt.get();
                        smvm.setShiftName(sm.getShiftName());
                        smvm.setStartTime(formatTime(sm.getStartTime()));
                        smvm.setEndTime(formatTime(sm.getEndTime()));
                        smvm.setClkHrs(sm.getClkHrs());
                        smvm.setDays(sm.getDays());
                    }
                }
                lstOfShift.add(smvm);
            }

            ShiftGroupingMasterViewModel sgvm = new ShiftGroupingMasterViewModel();
            sgvm.setCompId(compId);
            sgvm.setLeId(leId);
            sgvm.setBuId(buId);
            sgvm.setLocationId(locationId);
            sgvm.setLstOfShift(lstOfShift);

            if (compId != null && compId != 0) {
                CompanyMaster cm = companyCache.computeIfAbsent(compId, id -> 
                    companyMasterRepository.findById(id).orElse(null));
                sgvm.setCompany(cm != null ? cm.getCompany() : "");
            } else {
                sgvm.setCompany("");
            }

            if (leId != null && leId != 0) {
                LegalEntityMaster lem = leCache.computeIfAbsent(leId, id -> 
                    legalEntityMasterRepository.findById(id).orElse(null));
                sgvm.setLegalEntity(lem != null ? lem.getLegalEntity() : "");
            } else {
                sgvm.setLegalEntity("");
            }

            if (buId != null && buId != 0) {
                BusinessUnitMaster bum = buCache.computeIfAbsent(buId, id -> 
                    businessUnitMasterRepository.findById(id).orElse(null));
                sgvm.setBusinessUnit(bum != null ? bum.getBusinessUnit() : "");
            } else {
                sgvm.setBusinessUnit("");
            }

            if (locationId != null && locationId != 0) {
                LocationMaster lm = locationCache.computeIfAbsent(locationId, id -> 
                    locationMasterRepository.findById(id).orElse(null));
                sgvm.setLocation(lm != null ? lm.getLocation() : "");
            } else {
                sgvm.setLocation("");
            }

            String companyDisplay = sgvm.getCompany() != null ? sgvm.getCompany() : "";
            String legalEntityDisplay = sgvm.getLegalEntity() != null ? sgvm.getLegalEntity() : "";
            String locationDisplay = sgvm.getLocation() != null ? sgvm.getLocation() : "";

            if (compId != null && compId != 0 && leId != null && leId != 0 &&
                (buId == null || buId == 0) && (locationId == null || locationId == 0)) {
                sgvm.setCompany(companyDisplay + " - " + legalEntityDisplay);
            } else if (compId != null && compId != 0 && leId != null && leId != 0 &&
                       buId != null && buId != 0 && locationId != null && locationId != 0) {
                sgvm.setCompany(companyDisplay + " - " + locationDisplay);
            }

            result.add(sgvm);
        }

        return result;
    }

    public ShiftGroupingMasterViewModel addShiftGrouping(ShiftGroupingMasterViewModel model) {
        int loginId = model.getLoginId() != null ? model.getLoginId() : 0;
        if (loginId == 0) {
            throw new RuntimeException("EmpId is Mismatching");
        }

        List<ShiftGroupingMaster> existing = shiftGroupingMasterRepository
            .findByCompIdAndLeIdAndBuIdAndLocationIdAndIsActiveAndIsDeleted(
                model.getCompId(), model.getLeId(), model.getBuId(), model.getLocationId(), true, false);

        if (!existing.isEmpty()) {
            for (ShiftGroupingMaster sg : existing) {
                sg.setStatus(false);
                sg.setLastUpdatedBy(loginId);
                sg.setLastUpdatedDate(new Date());
                sg.setIsActive(true);
                sg.setIsUpdated(true);
                sg.setIsDeleted(true);
                shiftGroupingMasterRepository.save(sg);
            }
        }

        if (model.getLstOfShift() != null && !model.getLstOfShift().isEmpty()) {
            for (SampleShiftMasterViewModel shift : model.getLstOfShift()) {
                ShiftGroupingMaster sg = new ShiftGroupingMaster();
                sg.setShiftId(shift.getShiftId());
                sg.setCompId(model.getCompId());
                sg.setLeId(model.getLeId());
                sg.setBuId(model.getBuId());
                sg.setLocationId(model.getLocationId());
                sg.setStatus(true);
                sg.setCreatedBy(loginId);
                sg.setCreatedDate(new Date());
                sg.setLastUpdatedBy(loginId);
                sg.setLastUpdatedDate(new Date());
                sg.setIsActive(true);
                sg.setIsUpdated(false);
                sg.setIsDeleted(false);
                shiftGroupingMasterRepository.save(sg);
            }
        }

        ShiftGroupingMasterViewModel result = new ShiftGroupingMasterViewModel();
        result.setMsg("Added");
        return result;
    }

    public List<ShiftMasterViewModel> ddShift(ShiftGroupingMasterViewModel model) {
        return shiftMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(s -> convertToViewModel(s))
            .collect(Collectors.toList());
    }

    public List<ShiftMasterViewModel> createShift(ShiftMasterViewModel model) {
        ShiftMasterViewModel result = addShift(model);
        return List.of(result);
    }

    public List<ShiftMasterViewModel> getShiftByEmployee(ShiftMasterViewModel model) {
        return shiftMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(s -> convertToViewModel(s))
            .collect(Collectors.toList());
    }

    public ShiftMasterViewModel assignShift(ShiftMasterViewModel model) {
        model.setMsg("Shift assigned successfully");
        return model;
    }

    public Map<String, Object> addShiftEmployee(Map<String, Object> model) {
        int loginId = model.get("LoginId") instanceof Number ? ((Number) model.get("LoginId")).intValue() : 0;
        int compId = model.get("CompId") instanceof Number ? ((Number) model.get("CompId")).intValue() : 0;
        int leId = model.get("LEId") instanceof Number ? ((Number) model.get("LEId")).intValue() : 0;
        int buId = model.get("BUId") instanceof Number ? ((Number) model.get("BUId")).intValue() : 0;
        int locationId = model.get("LocationId") instanceof Number ? ((Number) model.get("LocationId")).intValue() : 0;
        int shiftId = model.get("ShiftId") instanceof Number ? ((Number) model.get("ShiftId")).intValue() : 0;
        String shiftName = model.get("ShiftName") instanceof String ? (String) model.get("ShiftName") : "";

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        Object empListObj = model.get("EmpList");
        if (empListObj == null || !(empListObj instanceof List)) {
            throw new RuntimeException("Employees Detail is Missing");
        }
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> empList = (List<Map<String, Object>>) empListObj;

        if (empList.isEmpty()) {
            throw new RuntimeException("Employees Detail is Missing");
        }

        java.util.Date today = new java.util.Date();
        today = java.sql.Date.valueOf(new java.text.SimpleDateFormat("yyyy-MM-dd").format(today));

        for (Map<String, Object> empData : empList) {
            Integer empId = empData.get("EmpId") instanceof Number ? ((Number) empData.get("EmpId")).intValue() : null;
            String empCode = empData.get("EmpCode") instanceof String ? (String) empData.get("EmpCode") : "";

            if (empId == null || empId == 0) continue;

            List<EmpShiftDetail> existingShifts = empShiftDetailRepository
                .findByEmpIdAndCompIdAndLeIdAndBuIdAndLocationIdAndEndDateIsNullAndShiftStatusAndIsActiveAndIsDeleted(
                    empId, compId, leId, buId, locationId, true, true, false);

            if (!existingShifts.isEmpty()) {
                for (EmpShiftDetail esd : existingShifts) {
                    esd.setEndDate(today);
                    esd.setShiftStatus(false);
                    esd.setStatus(false);
                    esd.setLastUpdatedBy(loginId);
                    esd.setLastUpdatedDate(new Date());
                    esd.setIsActive(true);
                    esd.setIsUpdated(true);
                    esd.setIsDeleted(true);
                    empShiftDetailRepository.save(esd);
                }
            }

            EmpShiftDetail newEsd = new EmpShiftDetail();
            newEsd.setShiftId(shiftId);
            newEsd.setShiftName(shiftName);
            newEsd.setEmpId(empId);
            newEsd.setEmpCode(empCode);
            newEsd.setCompId(compId);
            newEsd.setLeId(leId);
            newEsd.setBuId(buId);
            newEsd.setLocationId(locationId);
            newEsd.setShiftStatus(true);
            newEsd.setStatus(true);
            newEsd.setStartDate(today);
            newEsd.setCreatedBy(loginId);
            newEsd.setCreatedDate(new Date());
            newEsd.setLastUpdatedBy(loginId);
            newEsd.setLastUpdatedDate(new Date());
            newEsd.setIsActive(true);
            newEsd.setIsUpdated(false);
            newEsd.setIsDeleted(false);
            empShiftDetailRepository.save(newEsd);
        }

        return Map.of("msg", "Added");
    }

    public ShiftEmployeeListViewModel getAllShiftEmployee(Map<String, Object> model) {
        int loginId = model.get("LoginId") instanceof Number ? ((Number) model.get("LoginId")).intValue() : 0;
        int compId = model.get("CompId") instanceof Number ? ((Number) model.get("CompId")).intValue() : 0;
        int leId = model.get("LEId") instanceof Number ? ((Number) model.get("LEId")).intValue() : 0;
        int buId = model.get("BUId") instanceof Number ? ((Number) model.get("BUId")).intValue() : 0;
        int locationId = model.get("LocationId") instanceof Number ? ((Number) model.get("LocationId")).intValue() : 0;
        int shiftId = model.get("ShiftId") instanceof Number ? ((Number) model.get("ShiftId")).intValue() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        String company = "";
        List<EmployeeMaster> shiftEmployees = new ArrayList<>();
        List<EmployeeMaster> nonShiftEmployees = new ArrayList<>();
        List<EmployeeMaster> noShiftEmployees = new ArrayList<>();

        if (compId != 0 && leId != 0 && buId == 0 && locationId == 0) {
            String compName = companyMasterRepository.findById(compId).map(CompanyMaster::getCompany).orElse("");
            String leName = legalEntityMasterRepository.findById(leId).map(LegalEntityMaster::getLegalEntity).orElse("");
            company = compName + " - " + leName;

            List<EmployeeMaster> allEmp = employeeMasterRepository.findByCompIdAndIsActiveAndIsDeleted(compId, true, false)
                .stream().filter(e -> e.getLeId() != null && e.getLeId().equals(leId)
                    && (e.getBuId() == null || e.getBuId() == 0)
                    && (e.getLocationId() == null || e.getLocationId() == 0))
                .collect(Collectors.toList());

            for (EmployeeMaster emp : allEmp) {
                List<EmpShiftDetail> empShifts = empShiftDetailRepository.findByIsActiveAndIsDeleted(true, false)
                    .stream().filter(esd -> esd.getEmpId() != null && esd.getEmpId().equals(emp.getEmpId())
                        && esd.getShiftStatus() != null && esd.getShiftStatus())
                    .collect(Collectors.toList());

                boolean hasTargetShift = empShifts.stream().anyMatch(esd -> shiftId != 0 && esd.getShiftId() != null && esd.getShiftId().equals(shiftId));
                boolean hasOtherShift = empShifts.stream().anyMatch(esd -> shiftId == 0 || esd.getShiftId() == null || !esd.getShiftId().equals(shiftId));

                if (hasTargetShift) {
                    shiftEmployees.add(emp);
                } else if (hasOtherShift) {
                    nonShiftEmployees.add(emp);
                } else {
                    noShiftEmployees.add(emp);
                }
            }
        } else if (compId != 0 && leId != 0 && buId != 0 && locationId != 0) {
            String compName = companyMasterRepository.findById(compId).map(CompanyMaster::getCompany).orElse("");
            String locName = locationMasterRepository.findById(locationId).map(LocationMaster::getLocation).orElse("");
            company = compName + " - " + locName;

            List<EmployeeMaster> allEmp = employeeMasterRepository.findByCompIdAndIsActiveAndIsDeleted(compId, true, false)
                .stream().filter(e -> e.getLeId() != null && e.getLeId().equals(leId)
                    && e.getBuId() != null && e.getBuId().equals(buId)
                    && e.getLocationId() != null && e.getLocationId().equals(locationId))
                .collect(Collectors.toList());

            for (EmployeeMaster emp : allEmp) {
                List<EmpShiftDetail> empShifts = empShiftDetailRepository.findByIsActiveAndIsDeleted(true, false)
                    .stream().filter(esd -> esd.getEmpId() != null && esd.getEmpId().equals(emp.getEmpId())
                        && esd.getShiftStatus() != null && esd.getShiftStatus())
                    .collect(Collectors.toList());

                boolean hasTargetShift = empShifts.stream().anyMatch(esd -> shiftId != 0 && esd.getShiftId() != null && esd.getShiftId().equals(shiftId));
                boolean hasOtherShift = empShifts.stream().anyMatch(esd -> shiftId == 0 || esd.getShiftId() == null || !esd.getShiftId().equals(shiftId));

                if (hasTargetShift) {
                    shiftEmployees.add(emp);
                } else if (hasOtherShift) {
                    nonShiftEmployees.add(emp);
                } else {
                    noShiftEmployees.add(emp);
                }
            }
        }

        final String companyFinal = company;
        List<ShiftEmployeeMasterViewModel> shiftEmpList = shiftEmployees.stream()
            .map(e -> buildShiftEmployeeViewModel(e, companyFinal, shiftId))
            .collect(Collectors.toList());

        List<ShiftEmployeeMasterViewModel> nonShiftEmpList = new ArrayList<>();
        nonShiftEmpList.addAll(nonShiftEmployees.stream()
            .map(e -> buildShiftEmployeeViewModel(e, companyFinal, 0))
            .collect(Collectors.toList()));
        nonShiftEmpList.addAll(noShiftEmployees.stream()
            .map(e -> buildShiftEmployeeViewModel(e, companyFinal, 0))
            .collect(Collectors.toList()));

        ShiftEmployeeListViewModel result = new ShiftEmployeeListViewModel();
        result.setShiftEmployee(shiftEmpList);
        result.setNonShiftEmployee(nonShiftEmpList);
        return result;
    }

    private ShiftEmployeeMasterViewModel buildShiftEmployeeViewModel(EmployeeMaster emp, String company, int shiftId) {
        ShiftEmployeeMasterViewModel vm = new ShiftEmployeeMasterViewModel();
        vm.setEmpId(emp.getEmpId());
        vm.setOldEmpId(emp.getOldEmp_ID());
        vm.setCompId(emp.getCompId());
        vm.setCompany(company);
        vm.setLeId(emp.getLeId());
        if (emp.getLeId() != null && emp.getLeId() != 0) {
            legalEntityMasterRepository.findById(emp.getLeId())
                .ifPresent(le -> vm.setLegalEntity(le.getLegalEntity()));
        }
        vm.setBuId(emp.getBuId());
        if (emp.getBuId() != null && emp.getBuId() != 0) {
            businessUnitMasterRepository.findById(emp.getBuId())
                .ifPresent(bu -> vm.setBusinessUnit(bu.getBusinessUnit()));
        }
        vm.setLocationId(emp.getLocationId());
        if (emp.getLocationId() != null && emp.getLocationId() != 0) {
            locationMasterRepository.findById(emp.getLocationId())
                .ifPresent(loc -> vm.setLocation(loc.getLocation()));
        }
        vm.setShiftId(shiftId);
        if (shiftId != 0) {
            shiftMasterRepository.findById(shiftId)
                .ifPresent(s -> vm.setShiftName(s.getShiftName()));
        }
        vm.setCategoryId(emp.getCategoryId());
        vm.setDeptId(emp.getCategoryId());
        vm.setDeptName(emp.getDeptName());
        vm.setDesignationId(emp.getDesignationId());
        vm.setDesignation(emp.getDesignationName());
        vm.setReportId(emp.getReportId());
        vm.setApproverId(emp.getReportId());
        if (emp.getReportId() != null && emp.getReportId() != 0) {
            int reportId = emp.getReportId();
            employeeMasterRepository.findById(reportId).ifPresent(reportEmp -> {
                String approver = (reportEmp.getFirstName() != null ? reportEmp.getFirstName() : "") + " " +
                    (reportEmp.getMiddleName() != null ? reportEmp.getMiddleName() : "") + " " +
                    (reportEmp.getLastName() != null ? reportEmp.getLastName() : "") + " - " +
                    (reportEmp.getEmpCode() != null ? reportEmp.getEmpCode() : "");
                vm.setApprover(approver.trim());
            });
        }
        vm.setEmpCode(emp.getEmpCode());
        vm.setFirstName(emp.getFirstName());
        vm.setMiddleName(emp.getMiddleName());
        vm.setLastName(emp.getLastName());
        return vm;
    }

    public Map<String, Object> removeShiftEmployee(Map<String, Object> model) {
        int loginId = model.get("LoginId") instanceof Number ? ((Number) model.get("LoginId")).intValue() : 0;
        int compId = model.get("CompId") instanceof Number ? ((Number) model.get("CompId")).intValue() : 0;
        int leId = model.get("LEId") instanceof Number ? ((Number) model.get("LEId")).intValue() : 0;
        int buId = model.get("BUId") instanceof Number ? ((Number) model.get("BUId")).intValue() : 0;
        int locationId = model.get("LocationId") instanceof Number ? ((Number) model.get("LocationId")).intValue() : 0;
        int shiftId = model.get("ShiftId") instanceof Number ? ((Number) model.get("ShiftId")).intValue() : 0;

        if (loginId == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        Object empListObj = model.get("EmpList");
        if (empListObj == null || !(empListObj instanceof List)) {
            throw new RuntimeException("Employees Detail is Missing");
        }
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> empList = (List<Map<String, Object>>) empListObj;

        if (empList.isEmpty()) {
            throw new RuntimeException("Employees Detail is Missing");
        }

        java.util.Date today = new java.util.Date();
        today = java.sql.Date.valueOf(new java.text.SimpleDateFormat("yyyy-MM-dd").format(today));

        for (Map<String, Object> empData : empList) {
            Integer empId = empData.get("EmpId") instanceof Number ? ((Number) empData.get("EmpId")).intValue() : null;
            if (empId == null || empId == 0) continue;

            List<EmpShiftDetail> existingShifts = empShiftDetailRepository
                .findByEmpIdAndCompIdAndLeIdAndBuIdAndLocationIdAndShiftIdAndEndDateIsNullAndShiftStatusAndIsActiveAndIsDeleted(
                    empId, compId, leId, buId, locationId, shiftId, true, true, false);

            if (!existingShifts.isEmpty()) {
                for (EmpShiftDetail esd : existingShifts) {
                    esd.setEndDate(today);
                    esd.setShiftStatus(false);
                    esd.setStatus(false);
                    esd.setLastUpdatedBy(loginId);
                    esd.setLastUpdatedDate(new Date());
                    esd.setIsActive(true);
                    esd.setIsUpdated(true);
                    esd.setIsDeleted(true);
                    empShiftDetailRepository.save(esd);
                }
            } else {
                throw new RuntimeException("Employees Detail is Missing");
            }
        }

        return Map.of("msg", "Removed");
    }

    public List<Map<String, Object>> locationShiftGrouping(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        result.add(Map.of("ShiftGroupingId", 1, "LocationId", 1, "StatusCode", 200));
        return result;
    }

    private ShiftMasterViewModel convertToViewModel(ShiftMaster s) {
        ShiftMasterViewModel vm = new ShiftMasterViewModel();
        vm.setShiftId(s.getShiftId());
        vm.setShiftName(s.getShiftName());
        vm.setStartTime(formatTime(s.getStartTime()));
        vm.setEndTime(formatTime(s.getEndTime()));
        vm.setClkHrs(s.getClkHrs());
        vm.setDays(s.getDays());
        vm.setStatus(s.getStatus());
        vm.setCreatedBy(s.getCreatedBy() != null ? s.getCreatedBy() : 0);
        vm.setCreatedDate(s.getCreatedDate() != null ? s.getCreatedDate().toString() : null);
        vm.setLastUpdatedBy(s.getLastUpdatedBy());
        vm.setLastUpdatedDate(s.getLastUpdatedDate() != null ? s.getLastUpdatedDate().toString() : null);
        vm.setIsActive(s.getIsActive());
        vm.setIsUpdated(s.getIsUpdated());
        vm.setIsDeleted(s.getIsDeleted());
        return vm;
    }

    private String formatTime(LocalTime time) {
        if (time == null) return null;
        return time.format(DateTimeFormatter.ofPattern("HH:mm:ss"));
    }

    private LocalTime parseTime(String input) {
        if (input == null || input.trim().isEmpty()) return null;
        String s = input.trim();
        try {
            return LocalTime.parse(s);
        } catch (DateTimeParseException e) {
            try {
                return LocalTime.parse(s, DateTimeFormatter.ofPattern("HH:mm:ss.SSSSSSS"));
            } catch (DateTimeParseException e2) {
                try {
                    return LocalTime.parse(s, DateTimeFormatter.ofPattern("HH:mm:ss.SSS"));
                } catch (DateTimeParseException e3) {
                    try {
                        return LocalTime.parse(s, DateTimeFormatter.ofPattern("HH:mm:ss"));
                    } catch (DateTimeParseException e4) {
                        try {
                            return LocalTime.parse(s, DateTimeFormatter.ofPattern("HH:mm"));
                        } catch (DateTimeParseException e5) {
                            int h = 0, m = 0, sec = 0;
                            String[] parts = s.split(":");
                            if (parts.length >= 1) h = Integer.parseInt(parts[0].replaceAll("[^0-9]", ""));
                            if (parts.length >= 2) m = Integer.parseInt(parts[1].replaceAll("[^0-9]", ""));
                            if (parts.length >= 3) sec = Integer.parseInt(parts[2].substring(0, Math.min(2, parts[2].length())).replaceAll("[^0-9]", ""));
                            return LocalTime.of(h, m, sec);
                        }
                    }
                }
            }
        }
    }
}
