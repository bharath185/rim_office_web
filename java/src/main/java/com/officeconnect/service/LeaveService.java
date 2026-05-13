package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.stream.Collectors;
import java.text.SimpleDateFormat;

@Service
public class LeaveService {

    @Autowired
    private LeaveTypeMasterRepository leaveTypeMasterRepository;

    @Autowired
    private EmpLeaveApplicationRepository empLeaveApplicationRepository;

    @Autowired
    private CompOffRequestRepository compOffRequestRepository;

    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private LeaveCarryForwardMasterRepository leaveCarryForwardMasterRepository;

    @Autowired
    private AttendanceRepository attendanceRepository;

    @Autowired
    private HolidayRepository holidayRepository;

    public List<LeaveTypeViewModel> getAllLeaveType(LeaveTypeViewModel model) {
        return leaveTypeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(lt -> {
                LeaveTypeViewModel vm = new LeaveTypeViewModel();
                vm.setLeaveTypeId(lt.getLeaveTypeId());
                vm.setLeaveType(lt.getLeaveName());
                vm.setLeaveName(lt.getLeaveName());
                vm.setShortName(lt.getShortName());
                vm.setDescription(lt.getDescription());
                vm.setCredit(lt.getCredit() != null ? lt.getCredit() : 0);
                return vm;
            })
            .collect(Collectors.toList());
    }

    public List<LeaveTypeViewModel> getDDLeaveType(DDLeaveTypePayloadViewModel model) {
        if (model.getLoginId() == null || model.getLoginId() == 0) {
            throw new RuntimeException("EmpId is Missing");
        }

        Integer loginId = model.getLoginId();
        
        // Get employee details to get locationId if not provided
        Integer locationId = model.getLocationId() != null ? model.getLocationId() : 0;
        if (locationId == 0) {
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(loginId);
            if (empOpt.isPresent()) {
                locationId = empOpt.get().getLocationId() != null ? empOpt.get().getLocationId() : 0;
            }
        }

        // Get employee details
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(loginId);
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        EmployeeMaster emp = empOpt.get();

        Date joiningDate = emp.getJoiningDate();
        String gender = emp.getGender() != null ? emp.getGender() : "";
        String maritalStatus = emp.getMaritalStatus() != null ? emp.getMaritalStatus() : "";

        // Check if completed one year
        boolean hasCompletedOneYear = false;
        boolean isEligibleForCLThisMonth = true;
        Calendar cal = Calendar.getInstance();
        int currentYear = cal.get(Calendar.YEAR);
        int currentMonth = cal.get(Calendar.MONTH) + 1;

        if (joiningDate != null) {
            Calendar joinCal = Calendar.getInstance();
            joinCal.setTime(joiningDate);
            long diff = new Date().getTime() - joiningDate.getTime();
            hasCompletedOneYear = diff >= 365L * 24 * 60 * 60 * 1000;

            // Check if joined in current month/year and after 15th
            if (joinCal.get(Calendar.YEAR) == currentYear && 
                joinCal.get(Calendar.MONTH) + 1 == currentMonth) {
                if (joinCal.get(Calendar.DAY_OF_MONTH) > 15) {
                    isEligibleForCLThisMonth = false;
                }
            }
        }

        // Get RH holiday count for date range
        Integer rhCount = 0;
        if (model.getStartDate() != null && model.getEndDate() != null) {
            // This would need HolidayRepository - for now skip
            rhCount = 0;
        }

        // Get leave types applicable to location
        List<LeaveTypeMaster> allLeaveTypes = leaveTypeMasterRepository
            .findByIsActiveAndIsDeleted(true, false);

        List<LeaveTypeViewModel> result = new ArrayList<>();

        for (LeaveTypeMaster lt : allLeaveTypes) {
            if (lt.getLocationId() == null || lt.getApplicableTo() == null) continue;

            String locIdsStr = lt.getLocationId();
            List<Integer> locIds = Arrays.stream(locIdsStr.split(","))
                .map(String::trim)
                .filter(s -> !s.isEmpty())
                .map(Integer::parseInt)
                .collect(Collectors.toList());

            if (!locIds.contains(locationId)) continue;

            String applicableTo = lt.getApplicableTo().toUpperCase();
            if (!applicableTo.equals("ALL") && !applicableTo.equals(gender.toUpperCase())) {
                continue;
            }

            // Format: "LeaveName - (ShortName)"
            LeaveTypeViewModel vm = new LeaveTypeViewModel();
            vm.setLeaveTypeId(lt.getLeaveTypeId());
            vm.setLeaveType(lt.getLeaveName() + " - (" + lt.getShortName() + ")");
            vm.setLeaveName(lt.getLeaveName());
            vm.setShortName(lt.getShortName());
            result.add(vm);
        }

        // Filter CL if not eligible
        if (!isEligibleForCLThisMonth) {
            result = result.stream()
                .filter(vm -> !"CL".equals(vm.getShortName()))
                .collect(Collectors.toList());
        }

        // Filter EL if not completed one year
        if (!hasCompletedOneYear) {
            result = result.stream()
                .filter(vm -> !"EL".equals(vm.getShortName()))
                .collect(Collectors.toList());
        }

        // Filter RH if no RH holidays
        if (rhCount == 0) {
            result = result.stream()
                .filter(vm -> !"RH".equals(vm.getShortName()))
                .collect(Collectors.toList());
        }

        return result;
    }

    public LeaveTypeViewModel addLeaveType(LeaveTypeViewModel model) {
        // Check for duplicate leave type name
        List<LeaveTypeMaster> existingTypes = leaveTypeMasterRepository.findByLeaveNameAndIsActiveTrueAndIsDeletedFalse(
            model.getLeaveName() != null ? model.getLeaveName() : model.getLeaveType());
        if (!existingTypes.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Leave Type Details Already Exists\"}");
        }

        LeaveTypeMaster lt = new LeaveTypeMaster();
        lt.setLocationId(model.getLocationId());
        lt.setYearType(model.getYearType());
        lt.setLeaveName(model.getLeaveName() != null ? model.getLeaveName() : model.getLeaveType());
        lt.setShortName(model.getShortName());
        lt.setDescription(model.getDescription());
        lt.setDurationType(model.getDurationType());
        lt.setApplicableTo(model.getApplicableTo());
        lt.setEmpTypeId(model.getEmpTypeId());
        lt.setEmpLevel(model.getEmpLevel());
        lt.setCarryForward(model.getCarryForward());
        lt.setCredit(model.getCredit());
        lt.setIsMonth(model.getIsMonth());
        lt.setIsYear(model.getIsYear());
        lt.setMaxCarryForward(model.getMaxCarryForward());
        lt.setEncashable(model.getEncashable());
        lt.setMaxPerMonth(model.getMaxPerMonth());
        lt.setMaxPerYear(model.getMaxPerYear());
        lt.setMaxApply(model.getMaxApply());
        lt.setIsPaid(model.getIsPaid());
        lt.setApplicableDuration(model.getApplicableDuration());
        lt.setIsSingleApplication(model.getIsSingleApplication());
        lt.setMaxAllowedEvents(model.getMaxAllowedEvents());
        lt.setWeekEndInclusive(model.getWeekEndInclusive());
        lt.setResetYear(model.getResetYear());
        lt.setIsActive(true);
        lt.setIsUpdated(false);
        lt.setIsDeleted(false);
        lt.setCreatedBy(model.getLoginId());
        lt.setCreatedDate(new Date());
        lt.setLastUpdatedBy(model.getLoginId());
        lt.setLastUpdatedDate(new Date());

        lt = leaveTypeMasterRepository.save(lt);

        model.setLeaveTypeId(lt.getLeaveTypeId());
        model.setMsg("Added");
        return model;
    }

    public LeaveTypeViewModel updateLeaveType(LeaveTypeViewModel model) {
        Optional<LeaveTypeMaster> ltOpt = leaveTypeMasterRepository.findById(model.getLeaveTypeId());
        if (ltOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Leave type not found\"}");
        }
        
        LeaveTypeMaster lt = ltOpt.get();
        lt.setLocationId(model.getLocationId());
        lt.setYearType(model.getYearType());
        lt.setLeaveName(model.getLeaveName() != null ? model.getLeaveName() : model.getLeaveType());
        lt.setShortName(model.getShortName());
        lt.setDescription(model.getDescription());
        lt.setDurationType(model.getDurationType());
        lt.setApplicableTo(model.getApplicableTo());
        lt.setEmpTypeId(model.getEmpTypeId());
        lt.setEmpLevel(model.getEmpLevel());
        lt.setCarryForward(model.getCarryForward());
        lt.setCredit(model.getCredit());
        lt.setIsMonth(model.getIsMonth());
        lt.setIsYear(model.getIsYear());
        lt.setMaxCarryForward(model.getMaxCarryForward());
        lt.setEncashable(model.getEncashable());
        lt.setMaxPerMonth(model.getMaxPerMonth());
        lt.setMaxPerYear(model.getMaxPerYear());
        lt.setMaxApply(model.getMaxApply());
        lt.setIsPaid(model.getIsPaid());
        lt.setApplicableDuration(model.getApplicableDuration());
        lt.setIsSingleApplication(model.getIsSingleApplication());
        lt.setMaxAllowedEvents(model.getMaxAllowedEvents());
        lt.setWeekEndInclusive(model.getWeekEndInclusive());
        lt.setResetYear(model.getResetYear());
        lt.setIsUpdated(true);
        lt.setLastUpdatedBy(model.getLoginId());
        lt.setLastUpdatedDate(new Date());
        
        leaveTypeMasterRepository.save(lt);
        
        model.setMsg("Updated");
        return model;
    }

    public LeaveTypeViewModel deleteLeaveType(LeaveTypeViewModel model) {
        Optional<LeaveTypeMaster> ltOpt = leaveTypeMasterRepository.findById(model.getLeaveTypeId());
        if (ltOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Leave type not found\"}");
        }
        
        LeaveTypeMaster lt = ltOpt.get();
        lt.setIsDeleted(true);
        lt.setIsActive(false);
        lt.setLastUpdatedDate(new Date());
        leaveTypeMasterRepository.save(lt);
        
        model.setMsg("Leave type deleted successfully");
        return model;
    }

    public EmpLeaveApplicationViewModel applyLeave(EmpLeaveApplicationViewModel model) {
        Integer loginId = model.getLoginId();
        Integer empId = model.getEmpId();
        Integer leaveTypeId = model.getLeaveTypeId();
        if (loginId == null || loginId == 0) throw new RuntimeException("EmpId is Mismatching");
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        if (leaveTypeId == null || leaveTypeId == 0) throw new RuntimeException("Select the Leave Type");

        Date startDate = model.getStartDate();
        Date endDate = model.getEndDate();

        // Get leave type
        LeaveTypeMaster lt = leaveTypeMasterRepository.findById(leaveTypeId)
            .orElseThrow(() -> new RuntimeException("Leave type not found"));
        String shortName = lt.getShortName() != null ? lt.getShortName().toUpperCase() : "";

        // Check exact duplicate (same dates, same emp, not cancelled/withdrawn/deleted/rejected)
        List<EmpLeaveApplication> exactDuplicates = empLeaveApplicationRepository
            .findByEmpIdAndIsDeleted(empId, false).stream()
            .filter(e -> e.getFromDate() != null && e.getToDate() != null
                && e.getFromDate().equals(startDate) && e.getToDate().equals(endDate)
                && !"CANCELLED".equalsIgnoreCase(e.getStatus())
                && !"WITHDRAWN".equalsIgnoreCase(e.getStatus())
                && !"DELETE".equalsIgnoreCase(e.getStatus())
                && !e.getStatus().toUpperCase().contains("REJECT")
                && Boolean.TRUE.equals(e.getIsActive()))
            .collect(Collectors.toList());

        if (!exactDuplicates.isEmpty()) throw new RuntimeException("Leave Already Exists");

        // Overlap check
        List<EmpLeaveApplication> overlapping = empLeaveApplicationRepository
            .findByEmpIdAndIsDeleted(empId, false).stream()
            .filter(e -> Boolean.TRUE.equals(e.getIsActive())
                && !"CANCELLED".equalsIgnoreCase(e.getStatus())
                && !"WITHDRAWN".equalsIgnoreCase(e.getStatus())
                && !"DELETE".equalsIgnoreCase(e.getStatus())
                && !e.getStatus().toUpperCase().contains("REJECT"))
            .filter(e -> e.getFromDate() != null && e.getToDate() != null
                && startDate != null && endDate != null
                && !e.getToDate().before(startDate) && !endDate.before(e.getFromDate()))
            .collect(Collectors.toList());

        if (!overlapping.isEmpty()) throw new RuntimeException("Leave already applied for the selected date range.");

        // Check draft with same dates
        List<EmpLeaveApplication> draftCheck = empLeaveApplicationRepository
            .findByEmpIdAndIsDeleted(empId, false).stream()
            .filter(e -> e.getFromDate() != null && e.getToDate() != null
                && e.getFromDate().equals(startDate) && e.getToDate().equals(endDate)
                && "DRAFT".equalsIgnoreCase(e.getStatus())
                && !Boolean.TRUE.equals(e.getIsActive()))
            .collect(Collectors.toList());

        if (!draftCheck.isEmpty()) throw new RuntimeException("Leave request could not be submitted. A draft leave for the same date already exists. Please review your draft requests.");

        // Get carry forward balance
        Calendar cal = Calendar.getInstance();
        int year = cal.get(Calendar.YEAR);
        int month = cal.get(Calendar.MONTH) + 1;
        LeaveCarryForwardMaster carryForward = leaveCarryForwardMasterRepository
            .findByEmpIdAndLeaveTypeIdAndLeaveYearAndLeaveMonth(empId, leaveTypeId, year, month);

        if (carryForward == null) throw new RuntimeException("Your Leave Balance Not Available");

        double availCount = (carryForward.getOpeningBalance() != null ? carryForward.getOpeningBalance() : 0.0)
            - (carryForward.getAvailed() != null ? carryForward.getAvailed() : 0.0);

        // Balance validation (skip if IsLOP)
        if (!Boolean.TRUE.equals(model.getIsLOP())) {
            if (leaveTypeId == 1) {
                Integer leaveMonth = startDate != null ? getMonth(startDate) : null;
                Integer leaveYear = startDate != null ? getYear(startDate) : null;
                LeaveCarryForwardMaster clBalance = leaveCarryForwardMasterRepository
                    .findByEmpIdAndLeaveTypeIdAndLeaveYearAndLeaveMonth(empId, leaveTypeId, leaveYear, leaveMonth);
                if (clBalance == null)
                    throw new RuntimeException("Insufficient CL leave balance for the last month. This leave will be marked as LOP. Confirmation is required to proceed.");
                if (clBalance.getClosingBalance() == null || clBalance.getClosingBalance() == 0)
                    throw new RuntimeException("Insufficient CL leave balance for the last month. This leave will be marked as LOP. Confirmation is required to proceed.");
                Double totalDays = model.getDuration();
                if (totalDays != null && totalDays > clBalance.getClosingBalance())
                    throw new RuntimeException("Insufficient CL leave balance for the last month. This leave will be marked as LOP. Confirmation is required to proceed.");
            }
            if (availCount < (model.getDuration() != null ? model.getDuration() : 0.0))
                throw new RuntimeException("Your " + shortName + " balance - " + (long)availCount + ". Applied leave will be Consider as LOP");
        }

        // Get reporting manager
        EmployeeMaster emp = employeeMasterRepository.findById(empId)
            .orElseThrow(() -> new RuntimeException("Employee not found"));
        Integer reportId = emp.getReportId() != null && emp.getReportId() != 0 ? emp.getReportId() : 149;
        Integer hrId = 149;

        // Max days check
        if (lt.getMaxApply() != null && startDate != null && endDate != null) {
            int dateDiff = (int)((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24)) + 1;
            if (dateDiff > lt.getMaxApply())
                throw new RuntimeException("For this LeaveType, user can apply maximum " + lt.getMaxApply() + " days only..");
        }

        // Holiday check
        if (startDate != null && endDate != null) {
            Integer locationId = emp.getLocationId();
            if (locationId != null) {
                List<Holiday> holidays = holidayRepository.findByLocationIdAndDateBetween(locationId, startDate, endDate);
                List<Date> holidayDates = holidays.stream()
                    .filter(h -> h.getHolidayType() == null || !"RH HOLIDAYS".equalsIgnoreCase(h.getHolidayType().trim()))
                    .map(Holiday::getDate)
                    .filter(d -> d != null)
                    .collect(Collectors.toList());
                if (!holidayDates.isEmpty()) {
                    SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
                    String dates = holidayDates.stream().map(sdf::format).collect(Collectors.joining(", "));
                    throw new RuntimeException("Leave cannot be applied on holiday(s): " + dates);
                }
            }
        }

        // Build entity
        EmpLeaveApplication ela = new EmpLeaveApplication();
        ela.setEmpId(empId);
        ela.setEmpCode(model.getEmpCode());
        boolean isLOP = Boolean.TRUE.equals(model.getIsLOP());
        if (isLOP) {
            ela.setLeaveTypeId(0);
        } else {
            ela.setLeaveTypeId(model.getLeaveTypeId());
        }
        Integer finalLeaveTypeId = isLOP ? 0 : leaveTypeId;
        ela.setFromDate(startDate);
        ela.setToDate(endDate);

        // Duration logic including EL Friday/weekend handling
        Double modelDuration = model.getDuration() != null ? model.getDuration() : 1.0;
        if (startDate != null) {
            Calendar startCal = Calendar.getInstance();
            startCal.setTime(startDate);
            if (startCal.get(Calendar.DAY_OF_WEEK) == Calendar.MONDAY && "EL".equalsIgnoreCase(shortName)) {
                Calendar lastFriday = Calendar.getInstance();
                lastFriday.setTime(startDate);
                lastFriday.add(Calendar.DAY_OF_MONTH, -3);
                Date fridayDate = lastFriday.getTime();

                boolean hasFridayLeave = empLeaveApplicationRepository.findByEmpIdAndIsDeleted(empId, false).stream()
                    .anyMatch(e -> e.getFromDate() != null && e.getToDate() != null
                        && !fridayDate.before(e.getFromDate()) && !fridayDate.after(e.getToDate())
                        && !"CANCELLED".equalsIgnoreCase(e.getStatus())
                        && Boolean.TRUE.equals(e.getIsActive())
                        && (finalLeaveTypeId != 0 && finalLeaveTypeId.equals(e.getLeaveTypeId())));

                if (hasFridayLeave) {
                    ela.setNoOfDays(2);
                    modelDuration = modelDuration + 2;
                } else {
                    ela.setNoOfDays(modelDuration.intValue());
                }
            } else {
                ela.setNoOfDays(modelDuration.intValue());
            }
        } else {
            ela.setNoOfDays(modelDuration.intValue());
        }

        ela.setReason(model.getReason());
        ela.setStatus("APPLIED");

        if ("COMP OFF".equals(shortName)) {
            ela.setCompOffDate(model.getCompOffDate());
            ela.setCompOffReason(model.getCompOffReason());
        }

        if (model.getDocName() != null && !model.getDocName().isEmpty()) {
            ela.setDocName(model.getDocName());
        } else {
            ela.setDocName("");
        }

        ela.setAppliedDate(new Date());
        ela.setApprovedBy(reportId);
        ela.setHrApproved(hrId);
        ela.setRemarks(model.getRemarks());
        ela.setIsActive(true);
        ela.setIsUpdated(false);
        ela.setIsDeleted(false);
        ela.setCreatedBy(loginId);
        ela.setCreatedDate(new Date());
        ela.setLastUpdatedBy(loginId);
        ela.setLastUpdatedDate(new Date());

        ela = empLeaveApplicationRepository.save(ela);

        // Update carry forward balance
        if (carryForward != null) {
            double open = carryForward.getOpeningBalance() != null ? carryForward.getOpeningBalance() : 0.0;
            double avail = carryForward.getAvailed() != null ? carryForward.getAvailed() : 0.0;
            double close = carryForward.getClosingBalance() != null ? carryForward.getClosingBalance() : 0.0;
            double daysCount = modelDuration;

            Boolean isSingleApp = lt.getIsSingleApplication();
            carryForward.setOpeningBalance(open);
            carryForward.setAvailed(avail + daysCount);
            if (close == 0) {
                carryForward.setClosingBalance(open - daysCount);
            } else {
                carryForward.setClosingBalance(close - daysCount);
            }
            if (Boolean.TRUE.equals(isSingleApp)) {
                carryForward.setOpeningBalance(0.0);
                carryForward.setAvailed(avail + daysCount);
                carryForward.setClosingBalance(0.0);
            }
            carryForward.setLastUpdatedBy(loginId);
            carryForward.setLastUpdatedDate(new Date());
            carryForward.setIsActive(true);
            carryForward.setIsUpdated(true);
            leaveCarryForwardMasterRepository.save(carryForward);
        }

        model.setEmpLeaveId(ela.getEmpLeaveId());
        model.setStatus("APPLIED");
        model.setMsg("Applied");
        return model;
    }

    public EmpLeaveApplicationViewModel draftLeave(EmpLeaveApplicationViewModel model) {
        // Get employee to get EmpCode
        Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(model.getEmpId());
        if (empOpt.isEmpty()) {
            throw new RuntimeException("Employee not found");
        }
        EmployeeMaster emp = empOpt.get();
        
        EmpLeaveApplication ela = new EmpLeaveApplication();
        ela.setEmpId(model.getEmpId());
        ela.setEmpCode(emp.getEmpCode());
        ela.setLeaveTypeId(model.getLeaveTypeId());
        ela.setFromDate(model.getFromDate());
        ela.setToDate(model.getToDate());
        // Handle leaveDay (Full Day/Half Day) and convert to numeric duration
        String leaveDay = model.getLeaveDay();
        Double duration = 1.0;
        if ("Half Day".equals(leaveDay)) {
            duration = 0.5;
        }
        ela.setNoOfDays(duration.intValue());
        ela.setReason(model.getReason());
        ela.setStatus("Draft");
        ela.setIsActive(true);
        ela.setIsUpdated(false);
        ela.setIsDeleted(false);
        ela.setAppliedDate(new Date());
        ela.setCreatedDate(new Date());
        
        ela = empLeaveApplicationRepository.save(ela);
        
        model.setEmpLeaveId(ela.getEmpLeaveId());
        model.setStatus("Draft");
        model.setMsg("Leave saved as draft");
        return model;
    }

    public List<EmpLeaveApplicationViewModel> getAllLeave(EmpLeaveApplicationViewModel model) {
        if (model.getLoginId() == null || model.getLoginId() == 0) {
            throw new RuntimeException("LoginId is Missing");
        }
        
        Integer empId = model.getEmpId();
        List<EmpLeaveApplication> leaveList;
        
        if (empId != null && empId != 0) {
            leaveList = empLeaveApplicationRepository.findByEmpIdAndIsDeletedFalse(empId);
        } else {
            leaveList = empLeaveApplicationRepository.findByIsDeletedFalse();
        }
        
        return leaveList.stream()
            .sorted((a, b) -> {
                Date dateA = a.getCreatedDate() != null ? a.getCreatedDate() : new Date(0);
                Date dateB = b.getCreatedDate() != null ? b.getCreatedDate() : new Date(0);
                return dateB.compareTo(dateA);
            })
            .map(ela -> convertToViewModel(ela))
            .collect(Collectors.toList());
    }

    public List<EmpLeaveApplicationViewModel> getAllEmpLeave(EmpLeaveApplicationViewModel model) {
        Integer empId = model.getEmpId();
        if (empId == null) {
            return empLeaveApplicationRepository.findByIsDeleted(false).stream()
                .map(ela -> convertToViewModel(ela))
                .collect(Collectors.toList());
        }
        return empLeaveApplicationRepository.findByEmpIdAndIsDeleted(empId, false).stream()
            .map(ela -> convertToViewModel(ela))
            .collect(Collectors.toList());
    }

    public List<EmpLeaveApplicationViewModel> getAllApplyManagerLeave(EmpLeaveApplicationViewModel model) {
        return getAllManagerLeave(model);
    }

    public List<EmpLeaveApplicationViewModel> getAllManagerLeave(EmpLeaveApplicationViewModel model) {
        // Returns leave applications that need manager approval
        return empLeaveApplicationRepository.findByIsDeleted(false).stream()
            .filter(ela -> !"Rejected".equals(ela.getStatus()) && !"Cancelled".equals(ela.getStatus()))
            .filter(ela -> ela.getApprovedBy() == null || ela.getApprovedBy() == 0)
            .map(ela -> convertToViewModel(ela))
            .collect(Collectors.toList());
    }

    public List<EmpLeaveApplicationViewModel> getAllApplyHRLeave(EmpLeaveApplicationViewModel model) {
        // Returns leave applications that need HR approval
        // .NET logic: Status = "APPROVED BY MANAGER" AND (HRApproved = 0 OR HRApproved IS NULL)
        return empLeaveApplicationRepository.findByIsDeleted(false).stream()
            .filter(ela -> "APPROVED BY MANAGER".equals(ela.getStatus()))
            .filter(ela -> ela.getHrApproved() == null || ela.getHrApproved() == 0)
            .map(ela -> convertToViewModel(ela))
            .collect(Collectors.toList());
    }

    public List<EmpLeaveApplicationViewModel> getAllHRLeave(EmpLeaveApplicationViewModel model) {
        // Same as GetAllApplyHRLeave
        return getAllApplyHRLeave(model);
    }

    public ApproveLeaveViewModel approveLeaveByManager(ApproveLeaveViewModel model) {
        List<Integer> approvedIds = new ArrayList<>();
        List<Integer> failedIds = new ArrayList<>();
        List<String> errors = new ArrayList<>();

        if (model.getLstofLevAppId() != null) {
            for (ApproveLeaveViewModel.LeaveAppIdItem item : model.getLstofLevAppId()) {
                Optional<EmpLeaveApplication> elaOpt = empLeaveApplicationRepository.findById(item.getLeaveAppId());
                if (elaOpt.isEmpty()) {
                    failedIds.add(item.getLeaveAppId());
                    errors.add("Leave application " + item.getLeaveAppId() + " not found");
                    continue;
                }

                EmpLeaveApplication ela = elaOpt.get();
                ela.setStatus("APPROVED BY MANAGER");
                ela.setApprovedBy(model.getApprovedBy());
                ela.setApprovedDate(new Date());
                ela.setRemarks(item.getRemarks());
                ela.setLastUpdatedDate(new Date());
                empLeaveApplicationRepository.save(ela);

                approvedIds.add(item.getLeaveAppId());
            }
        }

        ApproveLeaveViewModel result = new ApproveLeaveViewModel();
        result.setApprovedIds(approvedIds);
        result.setFailedIds(failedIds);
        result.setErrors(errors);
        result.setStatus(approvedIds.size() >0 ? "200" : "206");
        result.setMsg("Leave approved by manager");
        return result;
    }

    public ApproveLeaveViewModel rejectLeaveByManager(ApproveLeaveViewModel model) {
        List<Integer> rejectedIds = new ArrayList<>();
        List<Integer> failedIds = new ArrayList<>();
        List<String> errors = new ArrayList<>();

        if (model.getLstofLevAppId() != null) {
            for (ApproveLeaveViewModel.LeaveAppIdItem item : model.getLstofLevAppId()) {
                Optional<EmpLeaveApplication> elaOpt = empLeaveApplicationRepository.findById(item.getLeaveAppId());
                if (elaOpt.isEmpty()) {
                    failedIds.add(item.getLeaveAppId());
                    errors.add("Leave application " + item.getLeaveAppId() + " not found");
                    continue;
                }

                EmpLeaveApplication ela = elaOpt.get();
                ela.setStatus("REJECTED BY MANAGER");
                ela.setApprovedBy(model.getApprovedBy());
                ela.setRemarks(item.getRemarks());
                ela.setLastUpdatedDate(new Date());
                empLeaveApplicationRepository.save(ela);

                rejectedIds.add(item.getLeaveAppId());
            }
        }

        ApproveLeaveViewModel result = new ApproveLeaveViewModel();
        result.setApprovedIds(rejectedIds);
        result.setFailedIds(failedIds);
        result.setErrors(errors);
        result.setStatus(rejectedIds.size() >0 ? "200" : "206");
        result.setMsg("Leave rejected by manager");
        return result;
    }

    public CompOffRequestViewModel compOffLeave(CompOffRequestViewModel model) {
        CompOffRequest cor = new CompOffRequest();
        cor.setEmpId(model.getEmpId());
        cor.setEmpCode(model.getEmpCode());
        cor.setDate(model.getDate());
        cor.setProject(model.getProject());
        cor.setTask(model.getTask());
        cor.setHrs(model.getHrs());
        cor.setActualHrs(model.getActualHrs());
        cor.setWorkMode(model.getWorkMode());
        cor.setReason(model.getReason());
        cor.setIsRequested(true);
        cor.setIsApproved(false);
        cor.setIsRejected(false);
        cor.setIsActive(true);
        cor.setIsUpdated(false);
        cor.setIsDeleted(false);
        cor.setCreatedDate(new Date());
        
        cor = compOffRequestRepository.save(cor);
        
        model.setCompOffReqId(cor.getCompOffReqId());
        model.setMsg("Comp off leave applied successfully");
        return model;
    }

    public ApproveLeaveViewModel approveCompOff(ApproveLeaveViewModel model) {
        ApproveLeaveViewModel result = new ApproveLeaveViewModel();
        result.setApprovedIds(new java.util.ArrayList<>());
        result.setFailedIds(new java.util.ArrayList<>());
        result.setErrors(new java.util.ArrayList<>());

        List<ApproveLeaveViewModel.CompOffReqIdItem> compOffList = model.getLstofCompOffReqId();
        if (compOffList == null || compOffList.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":400,\"Message\":\"No comp off request IDs provided\"}");
        }

        for (ApproveLeaveViewModel.CompOffReqIdItem item : compOffList) {
            try {
                Optional<CompOffRequest> corOpt = compOffRequestRepository.findById(item.getCompOffReqId());
                if (corOpt.isEmpty()) {
                    result.getFailedIds().add(item.getCompOffReqId());
                    result.getErrors().add("Comp off request " + item.getCompOffReqId() + " not found");
                    continue;
                }

                CompOffRequest cor = corOpt.get();
                cor.setIsApproved(true);
                cor.setIsRejected(false);
                cor.setLastUpdatedDate(new Date());
                compOffRequestRepository.save(cor);

                result.getApprovedIds().add(item.getCompOffReqId());
            } catch (Exception e) {
                result.getFailedIds().add(item.getCompOffReqId());
                result.getErrors().add("Error processing comp off request " + item.getCompOffReqId() + ": " + e.getMessage());
            }
        }

        result.setMsg("Comp off approval processed. Approved: " + result.getApprovedIds().size() + ", Failed: " + result.getFailedIds().size());
        return result;
    }

    private EmpLeaveApplicationViewModel convertToViewModel(EmpLeaveApplication ela) {
        EmpLeaveApplicationViewModel vm = new EmpLeaveApplicationViewModel();
        vm.setLoginId(0);
        vm.setLeaveAppId(ela.getEmpLeaveId());
        vm.setEmpId(ela.getEmpId());
        vm.setEmpCode(ela.getEmpCode() != null ? ela.getEmpCode() : "");
        
        // Get employee name
        if (ela.getEmpId() != null) {
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(ela.getEmpId());
            if (empOpt.isPresent()) {
                EmployeeMaster emp = empOpt.get();
                String firstName = emp.getFirstName() != null ? emp.getFirstName().trim() : "";
                String lastName = emp.getLastName() != null ? emp.getLastName().trim() : "";
                vm.setEmpName((firstName + " " + lastName).trim());
            }
        }
        
        vm.setLeaveTypeId(ela.getLeaveTypeId());
        vm.setLeaveType(ela.getLeaveTypeId() != null ? "Leave Type " + ela.getLeaveTypeId() : "LOP");
        
        vm.setStartDate(ela.getFromDate());
        vm.setEndDate(ela.getToDate());
        vm.setDuration(ela.getNoOfDays() != null ? ela.getNoOfDays().doubleValue() : 0.0);
        vm.setReason(ela.getReason());
        vm.setStatus(ela.getStatus());
        vm.setAppliedDate(ela.getAppliedDate());
        
        vm.setApprovedBy(ela.getApprovedBy());
        vm.setApprovedDate(ela.getApprovedDate());
        
        // Get approver name
        // If approved, get from ApprovedBy; otherwise get from employee's manager (ReportId)
        Integer approverId = ela.getApprovedBy();
        if (approverId == null && ela.getEmpId() != null) {
            // Get the employee's manager from ReportId
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(ela.getEmpId());
            if (empOpt.isPresent()) {
                approverId = empOpt.get().getReportId();
            }
        }
        
        if (approverId != null && approverId > 0) {
            Optional<EmployeeMaster> approverOpt = employeeMasterRepository.findById(approverId);
            if (approverOpt.isPresent()) {
                EmployeeMaster approver = approverOpt.get();
                String firstName = approver.getFirstName() != null ? approver.getFirstName().trim() : "";
                String lastName = approver.getLastName() != null ? approver.getLastName().trim() : "";
                vm.setApprover((firstName + " " + lastName).trim());
            }
        } else {
            vm.setApprover("");
        }
        
        vm.setHrApproved(ela.getHrApproved());
        vm.setHrApprovedDate(ela.getHrApprovedDate());
        vm.setRemarks(ela.getRemarks());
        
        vm.setCreatedby(ela.getCreatedBy());
        vm.setCreatedDate(ela.getCreatedDate());
        
        return vm;
    }

    public LeaveTypeViewModel activateLeaveType(LeaveTypeViewModel model) {
        Optional<LeaveTypeMaster> ltOpt = leaveTypeMasterRepository.findById(model.getLeaveTypeId());
        if (ltOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Leave type not found\"}");
        }
        LeaveTypeMaster lt = ltOpt.get();
        lt.setIsActive(true);
        lt.setLastUpdatedDate(new Date());
        leaveTypeMasterRepository.save(lt);
        model.setMsg("Leave type activated successfully");
        return model;
    }

    public LeaveTypeViewModel deactivateLeaveType(LeaveTypeViewModel model) {
        Optional<LeaveTypeMaster> ltOpt = leaveTypeMasterRepository.findById(model.getLeaveTypeId());
        if (ltOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Leave type not found\"}");
        }
        LeaveTypeMaster lt = ltOpt.get();
        lt.setIsActive(false);
        lt.setLastUpdatedDate(new Date());
        leaveTypeMasterRepository.save(lt);
        model.setMsg("Leave type deactivated successfully");
        return model;
    }

    public ApproveLeaveViewModel rejectCompOff(ApproveLeaveViewModel model) {
        ApproveLeaveViewModel result = new ApproveLeaveViewModel();
        result.setApprovedIds(new java.util.ArrayList<>());
        result.setFailedIds(new java.util.ArrayList<>());
        result.setErrors(new java.util.ArrayList<>());

        List<ApproveLeaveViewModel.CompOffReqIdItem> compOffList = model.getLstofCompOffReqId();
        if (compOffList == null || compOffList.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":400,\"Message\":\"No comp off request IDs provided\"}");
        }

        for (ApproveLeaveViewModel.CompOffReqIdItem item : compOffList) {
            try {
                Optional<CompOffRequest> corOpt = compOffRequestRepository.findById(item.getCompOffReqId());
                if (corOpt.isEmpty()) {
                    result.getFailedIds().add(item.getCompOffReqId());
                    result.getErrors().add("Comp off request " + item.getCompOffReqId() + " not found");
                    continue;
                }

                CompOffRequest cor = corOpt.get();
                cor.setIsApproved(false);
                cor.setIsRejected(true);
                cor.setLastUpdatedDate(new Date());
                compOffRequestRepository.save(cor);

                result.getApprovedIds().add(item.getCompOffReqId());
            } catch (Exception e) {
                result.getFailedIds().add(item.getCompOffReqId());
                result.getErrors().add("Error processing comp off request " + item.getCompOffReqId() + ": " + e.getMessage());
            }
        }

        result.setMsg("Comp off rejection processed. Processed: " + result.getApprovedIds().size() + ", Failed: " + result.getFailedIds().size());
        return result;
    }

    public EmpLeaveApplicationViewModel draftApplyLeave(EmpLeaveApplicationViewModel model) {
        Integer loginId = model.getLoginId();
        Integer empId = model.getEmpId();
        Integer leaveTypeId = model.getLeaveTypeId();
        Integer leaveAppId = model.getLeaveAppId();
        if (loginId == null || loginId == 0) throw new RuntimeException("EmpId is Mismatching");
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        if (leaveTypeId == null || leaveTypeId == 0) throw new RuntimeException("Select the Leave Type");

        Date startDate = model.getStartDate();
        Date endDate = model.getEndDate();

        // Get leave type
        LeaveTypeMaster lt = leaveTypeMasterRepository.findById(leaveTypeId)
            .orElseThrow(() -> new RuntimeException("Leave type not found"));
        String shortName = lt.getShortName() != null ? lt.getShortName().toUpperCase() : "";

        // Fetch existing draft record
        if (leaveAppId == null || leaveAppId == 0) throw new RuntimeException("LeaveAppId is Missing");
        EmpLeaveApplication existingDraft = empLeaveApplicationRepository.findById(leaveAppId)
            .orElseThrow(() -> new RuntimeException("Draft Leave details Not Found"));
        if (!"DRAFT".equals(existingDraft.getStatus())) throw new RuntimeException("Draft Leave details Not Found");

        // Overlap check
        List<EmpLeaveApplication> overlapping = empLeaveApplicationRepository
            .findByEmpIdAndIsDeleted(empId, false).stream()
            .filter(e -> Boolean.TRUE.equals(e.getIsActive())
                && !"CANCELLED".equalsIgnoreCase(e.getStatus())
                && !"WITHDRAWN".equalsIgnoreCase(e.getStatus())
                && !"DELETE".equalsIgnoreCase(e.getStatus())
                && !e.getStatus().toUpperCase().contains("REJECT"))
            .filter(e -> e.getFromDate() != null && e.getToDate() != null
                && startDate != null && endDate != null
                && !e.getToDate().before(startDate) && !endDate.before(e.getFromDate()))
            .collect(Collectors.toList());
        if (!overlapping.isEmpty()) throw new RuntimeException("Leave already applied for the selected date range.");

        // Check exact duplicate
        List<EmpLeaveApplication> exactDuplicates = empLeaveApplicationRepository
            .findByEmpIdAndIsDeleted(empId, false).stream()
            .filter(e -> e.getFromDate() != null && e.getToDate() != null
                && e.getFromDate().equals(startDate) && e.getToDate().equals(endDate)
                && !"CANCELLED".equalsIgnoreCase(e.getStatus())
                && !"WITHDRAWN".equalsIgnoreCase(e.getStatus())
                && !"DELETE".equalsIgnoreCase(e.getStatus())
                && !e.getStatus().toUpperCase().contains("REJECT")
                && Boolean.TRUE.equals(e.getIsActive()))
            .collect(Collectors.toList());
        if (!exactDuplicates.isEmpty()) throw new RuntimeException("Leave Already Exists");

        // Get carry forward balance
        Calendar cal = Calendar.getInstance();
        int year = cal.get(Calendar.YEAR);
        int month = cal.get(Calendar.MONTH) + 1;
        LeaveCarryForwardMaster carryForward = leaveCarryForwardMasterRepository
            .findByEmpIdAndLeaveTypeIdAndLeaveYearAndLeaveMonth(empId, leaveTypeId, year, month);
        if (carryForward == null) throw new RuntimeException("Your Leave Balance Not Available");
        double availCount = (carryForward.getOpeningBalance() != null ? carryForward.getOpeningBalance() : 0.0)
            - (carryForward.getAvailed() != null ? carryForward.getAvailed() : 0.0);

        if (!Boolean.TRUE.equals(model.getIsLOP())) {
            if (availCount < (model.getDuration() != null ? model.getDuration() : 0.0))
                throw new RuntimeException("Your " + shortName + " balance - " + (long)availCount + ". Applied leave will be Consider as LOP");
        }

        // Get reporting manager
        EmployeeMaster emp = employeeMasterRepository.findById(empId)
            .orElseThrow(() -> new RuntimeException("Employee not found"));
        Integer reportId = emp.getReportId() != null && emp.getReportId() != 0 ? emp.getReportId() : 149;
        Integer hrId = 149;

        // Max days check
        if (lt.getMaxApply() != null && startDate != null && endDate != null) {
            int dateDiff = (int)((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24)) + 1;
            if (dateDiff > lt.getMaxApply())
                throw new RuntimeException("For this LeaveType, user can apply maximum " + lt.getMaxApply() + " days only..");
        }

        // Update existing draft
        existingDraft.setEmpId(empId);
        existingDraft.setEmpCode(model.getEmpCode());
        if (Boolean.TRUE.equals(model.getIsLOP())) {
            existingDraft.setLeaveTypeId(0);
            leaveTypeId = 0;
        } else {
            existingDraft.setLeaveTypeId(model.getLeaveTypeId());
        }
        existingDraft.setFromDate(startDate);
        existingDraft.setToDate(endDate);
        existingDraft.setNoOfDays(model.getDuration() != null ? model.getDuration().intValue() : 1);
        existingDraft.setReason(model.getReason());
        existingDraft.setStatus("APPLIED");

        if ("COMP OFF".equals(shortName)) {
            existingDraft.setCompOffDate(model.getCompOffDate());
            existingDraft.setCompOffReason(model.getCompOffReason());
        }

        if (model.getDocName() != null && !model.getDocName().isEmpty()) {
            existingDraft.setDocName(model.getDocName());
        } else {
            existingDraft.setDocName("");
        }

        existingDraft.setAppliedDate(new Date());
        existingDraft.setApprovedBy(reportId);
        existingDraft.setHrApproved(hrId);
        existingDraft.setRemarks(model.getRemarks());
        existingDraft.setIsActive(true);
        existingDraft.setIsUpdated(true);
        existingDraft.setIsDeleted(false);
        existingDraft.setLastUpdatedBy(loginId);
        existingDraft.setLastUpdatedDate(new Date());
        empLeaveApplicationRepository.save(existingDraft);

        // Update carry forward balance
        if (carryForward != null) {
            double open = carryForward.getOpeningBalance() != null ? carryForward.getOpeningBalance() : 0.0;
            double avail = carryForward.getAvailed() != null ? carryForward.getAvailed() : 0.0;
            double close = carryForward.getClosingBalance() != null ? carryForward.getClosingBalance() : 0.0;
            double daysCount = model.getDuration() != null ? model.getDuration() : 1.0;

            Boolean isSingleApp = lt.getIsSingleApplication();
            carryForward.setOpeningBalance(open);
            carryForward.setAvailed(avail + daysCount);
            if (close == 0) {
                carryForward.setClosingBalance(open - daysCount);
            } else {
                carryForward.setClosingBalance(close - daysCount);
            }
            if (Boolean.TRUE.equals(isSingleApp)) {
                carryForward.setOpeningBalance(0.0);
                carryForward.setAvailed(avail + daysCount);
                carryForward.setClosingBalance(0.0);
            }
            carryForward.setLastUpdatedBy(loginId);
            carryForward.setLastUpdatedDate(new Date());
            carryForward.setIsActive(true);
            carryForward.setIsUpdated(true);
            leaveCarryForwardMasterRepository.save(carryForward);
        }

        model.setEmpLeaveId(existingDraft.getEmpLeaveId());
        model.setStatus("APPLIED");
        model.setMsg("Applied");
        return model;
    }

    public List<Map<String, Object>> getDDApproveManager() {
        List<Map<String, Object>> result = new ArrayList<>();
        for (EmployeeMaster emp : employeeMasterRepository.findByIsActiveAndIsDeleted(true, false)) {
            if (emp.getReportId() == null || emp.getReportId() == 0) {
                Map<String, Object> m = new HashMap<>();
                m.put("managerListId", emp.getEmpId());
                String firstName = emp.getFirstName() != null ? emp.getFirstName().trim() : "";
                String lastName = emp.getLastName() != null ? emp.getLastName().trim() : "";
                m.put("ManagerName", firstName + " " + lastName);
                result.add(m);
            }
        }
        return result;
    }

    public CompOffRequestViewModel getCompOffHours(CompOffRequestViewModel model) {
        // Get employee details
        if (model.getEmpId() == null || model.getEmpId() == 0) {
            throw new RuntimeException("EmpId is Missing");
        }
        
        // Find employee by empId to get empCode
        EmployeeMaster employee = employeeMasterRepository.findById(model.getEmpId())
            .orElseThrow(() -> new RuntimeException("Employee not found"));
        
        String empCode = employee.getEmpCode();
        Date date = model.getDate();
        
        if (empCode == null || empCode.isEmpty() || date == null) {
            model.setActualHrs("0");
            model.setWorkMode("-");
            model.setMsg("Comp off hours retrieved");
            return model;
        }
        
        // Find attendance records for the employee on the given date
        // Get all attendance logs for the date
        List<Attendance> attendanceLogs = attendanceRepository.findByLogDateBetween(date, date);
        attendanceLogs = attendanceLogs.stream()
            .filter(a -> empCode.equals(a.getEmpCode()))
            .collect(Collectors.toList());
        
        if (attendanceLogs.isEmpty()) {
            model.setActualHrs("0");
            model.setWorkMode("-");
        } else {
            // Calculate actual hours from attendance logs
            // For simplicity, count unique log entries as hours worked
            // In real implementation, calculate time difference between IN and OUT
            long hourCount = attendanceLogs.stream()
                .map(Attendance::getLogTime)
                .filter(Objects::nonNull)
                .count();
            
            model.setActualHrs(String.valueOf(hourCount));
            model.setWorkMode("Present"); // Default work mode
        }
        
        model.setMsg("Comp off hours retrieved");
        return model;
    }

    public List<CompOffRequestViewModel> getAllEmpCompOffLeave(CompOffRequestViewModel model) {
        List<CompOffRequest> compOffRequests;
        
        if (model.getEmpId() != null && model.getEmpId() > 0) {
            compOffRequests = compOffRequestRepository.findByEmpIdAndIsDeletedFalse(model.getEmpId());
        } else {
            compOffRequests = compOffRequestRepository.findByIsDeletedFalse();
        }
        
        return compOffRequests.stream()
            .map(this::convertCompOffToViewModel)
            .collect(Collectors.toList());
    }

    public List<CompOffRequestViewModel> getAllCompOffLeave(CompOffRequestViewModel model) {
        return getAllEmpCompOffLeave(model);
    }

    private CompOffRequestViewModel convertCompOffToViewModel(CompOffRequest cor) {
        CompOffRequestViewModel vm = new CompOffRequestViewModel();
        vm.setCompOffReqId(cor.getCompOffReqId());
        vm.setEmpId(cor.getEmpId());
        vm.setEmpCode(cor.getEmpCode());
        vm.setManagerId(cor.getManagerId());
        vm.setManagerCode(cor.getManagerCode());
        vm.setDate(cor.getDate());
        vm.setProjectId(cor.getProjectId());
        vm.setProject(cor.getProject());
        vm.setTaskId(cor.getTaskId());
        vm.setTask(cor.getTask());
        vm.setActualHrs(cor.getActualHrs());
        vm.setHrs(cor.getHrs());
        vm.setWorkMode(cor.getWorkMode());
        vm.setIsRequested(cor.getIsRequested());
        vm.setIsApproved(cor.getIsApproved());
        vm.setIsRejected(cor.getIsRejected());
        vm.setReason(cor.getReason());
        vm.setAppliedDate(cor.getCreatedDate());
        
        // Get employee name
        if (cor.getEmpId() != null) {
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(cor.getEmpId());
            if (empOpt.isPresent()) {
                EmployeeMaster emp = empOpt.get();
                vm.setEmployeeName(emp.getFirstName() + " " + emp.getLastName());
            }
        }
        
        vm.setIsActive(cor.getIsActive());
        return vm;
    }

    public LeaveTypeViewModel getLeaveType(LeaveTypeViewModel model) {
        Optional<LeaveTypeMaster> ltOpt = leaveTypeMasterRepository.findById(model.getLeaveTypeId());
        if (ltOpt.isEmpty()) {
            throw new RuntimeException("Leave type not found");
        }
        LeaveTypeMaster lt = ltOpt.get();
        LeaveTypeViewModel vm = new LeaveTypeViewModel();
        vm.setLeaveTypeId(lt.getLeaveTypeId());
        vm.setLeaveName(lt.getLeaveName());
        vm.setShortName(lt.getShortName());
        vm.setDescription(lt.getDescription());
        vm.setCredit(lt.getCredit() != null ? lt.getCredit() : 0);
        return vm;
    }

    public EmpLeaveApplicationViewModel deleteDraftLeave(EmpLeaveApplicationViewModel model) {
        Optional<EmpLeaveApplication> elaOpt = empLeaveApplicationRepository.findById(model.getEmpLeaveId());
        if (elaOpt.isEmpty()) {
            model.setMsg("Draft leave not found");
            return model;
        }
        EmpLeaveApplication ela = elaOpt.get();
        ela.setIsDeleted(true);
        ela.setIsActive(false);
        empLeaveApplicationRepository.save(ela);
        model.setStatus("Deleted");
        model.setMsg("Draft leave deleted");
        return model;
    }

    public Map<String, Object> getIndividualLeaveCount(EmpLeaveApplicationViewModel model) {
        Map<String, Object> leaveCount = new java.util.LinkedHashMap<>();
        
        Integer empId = model.getEmpId();
        String empCode = "";
        
        if (empId != null) {
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(empId);
            if (empOpt.isPresent()) {
                empCode = empOpt.get().getEmpCode() != null ? empOpt.get().getEmpCode() : "";
            }
        }
        
        leaveCount.put("EmpId", empId);
        
        Calendar cal = Calendar.getInstance();
        int currentYear = cal.get(Calendar.YEAR);
        int currentMonth = cal.get(Calendar.MONTH) + 1;
        
        List<Map<String, Object>> casualCounts = getLeaveCountsByType(empId, empCode, "CL", currentYear, currentMonth, 1);
        leaveCount.put("CasualCounts", casualCounts);
        
        List<Map<String, Object>> reservedHolidayCounts = getLeaveCountsByType(empId, empCode, "RH", currentYear, 0, 7);
        leaveCount.put("ReservedHolidayCounts", reservedHolidayCounts);
        
        List<Map<String, Object>> earnedLeaveCounts = getLeaveCountsByType(empId, empCode, "EL", currentYear, 0, 2);
        leaveCount.put("EarnedLeaveCounts", earnedLeaveCounts);
        
        List<Map<String, Object>> compOffCounts = getLeaveCountsByType(empId, empCode, "COMP OFF", currentYear, currentMonth, 3);
        leaveCount.put("CompOffCounts", compOffCounts);
        
        leaveCount.put("MLCounts", new ArrayList<>());
        leaveCount.put("PLCounts", null);
        
        return leaveCount;
    }
    
    private List<Map<String, Object>> getLeaveCountsByType(Integer empId, String empCode, String shortName, Integer year, Integer month, Integer defaultLeaveTypeId) {
        List<Map<String, Object>> counts = new ArrayList<>();
        
        List<LeaveTypeMaster> ltList = leaveTypeMasterRepository.findByShortNameAndIsActiveAndIsDeleted(shortName, true, false);
        Integer leaveTypeId = defaultLeaveTypeId;
        if (ltList != null && !ltList.isEmpty()) {
            leaveTypeId = ltList.get(0).getLeaveTypeId();
        }
        
        LeaveCarryForwardMaster cf = null;
        if (empId != null && leaveTypeId != null) {
            cf = leaveCarryForwardMasterRepository.findByEmpIdAndLeaveTypeIdAndLeaveYearAndLeaveMonth(empId, leaveTypeId, year, month);
        }
        
        Map<String, Object> data = new java.util.LinkedHashMap<>();
        data.put("CFId", cf != null ? cf.getCfId() : 0);
        data.put("EmpId", empId != null ? empId : 0);
        data.put("EmpCode", empCode != null ? empCode : "");
        data.put("LeaveTypeId", leaveTypeId);
        data.put("LeaveType", shortName);
        data.put("LeaveYear", year);
        data.put("LeaveMonth", month);
        
        if (cf != null) {
            data.put("OpeningBalance", cf.getOpeningBalance() != null ? cf.getOpeningBalance().intValue() : 0);
            data.put("Availed", cf.getAvailed() != null ? cf.getAvailed().intValue() : 0);
            data.put("ClosingBalance", cf.getClosingBalance() != null ? cf.getClosingBalance().intValue() : 0);
        } else {
            data.put("OpeningBalance", 0);
            data.put("Availed", 0);
            data.put("ClosingBalance", 0);
        }
        
        counts.add(data);
        return counts;
    }

    public List<Map<String, Object>> leaveBalReport(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        
        Integer buId = model.get("BUId") != null ? Integer.parseInt(model.get("BUId").toString()) : 0;
        Integer locationId = model.get("LocationId") != null ? Integer.parseInt(model.get("LocationId").toString()) : 0;
        Integer deptId = model.get("DeptId") != null ? Integer.parseInt(model.get("DeptId").toString()) : 0;
        Integer designationId = model.get("DesignationId") != null ? Integer.parseInt(model.get("DesignationId").toString()) : 0;
        Integer empId = model.get("EmpId") != null ? Integer.parseInt(model.get("EmpId").toString()) : 0;
        Integer year = model.get("Year") != null ? Integer.parseInt(model.get("Year").toString()) : Calendar.getInstance().get(Calendar.YEAR);
        Integer month = model.get("Month") != null ? Integer.parseInt(model.get("Month").toString()) : 0;
        
        List<EmployeeMaster> employees = employeeMasterRepository.findByIsActiveAndIsDeleted(Boolean.TRUE, Boolean.FALSE);
        
        // Filter employees based on criteria (only if value > 0)
        employees = employees.stream()
            .filter(e -> buId == 0 || (e.getBuId() != null && e.getBuId().equals(buId)))
            .filter(e -> locationId == 0 || (e.getLocationId() != null && e.getLocationId().equals(locationId)))
            .filter(e -> deptId == 0 || (e.getCategoryId() != null && e.getCategoryId().equals(deptId)))
            .filter(e -> designationId == 0 || (e.getDesignationId() != null && e.getDesignationId().equals(designationId)))
            .filter(e -> empId == 0 || e.getEmpId().equals(empId))
            .collect(Collectors.toList());
        
        if (employees.isEmpty()) {
            throw new RuntimeException("No employees found for the given criteria");
        }
        
        if (employees.isEmpty()) {
            throw new RuntimeException("No employees found for the given criteria");
        }
        
        for (EmployeeMaster emp : employees) {
            Map<String, Object> m = new HashMap<>();
            
            // Basic employee info
            m.put("EmpId", emp.getEmpId());
            m.put("EmpCode", emp.getEmpCode());
            
            String fullName = String.join(" ", 
                emp.getFirstName() != null ? emp.getFirstName().trim() : "",
                emp.getMiddleName() != null ? emp.getMiddleName().trim() : "",
                emp.getLastName() != null ? emp.getLastName().trim() : "").trim();
            m.put("EmpName", fullName);
            m.put("DeptId", emp.getCategoryId() != null ? emp.getCategoryId() : 0);
            m.put("LocationId", emp.getLocationId() != null ? emp.getLocationId() : 0);
            m.put("Year", year);
            m.put("Month", month != null ? month : 0);
            
        List<LeaveTypeMaster> leaveTypes = leaveTypeMasterRepository.findAll();
        System.out.println("Total leave types: " + leaveTypes.size());
        for (LeaveTypeMaster lt : leaveTypes) {
            System.out.println("Leave Type: " + lt.getLeaveName() + " (ID: " + lt.getLeaveTypeId() + ")");
        }
        
        Integer clTypeId = leaveTypes.stream()
                .filter(lt -> "CL".equalsIgnoreCase(lt.getLeaveName()))
                .findFirst()
                .map(LeaveTypeMaster::getLeaveTypeId)
                .orElse(null);
        System.out.println("CL Type ID: " + clTypeId);
            Integer elTypeId = leaveTypes.stream()
                .filter(lt -> "EL".equalsIgnoreCase(lt.getLeaveName()) || "PL".equalsIgnoreCase(lt.getLeaveName()))
                .findFirst()
                .map(LeaveTypeMaster::getLeaveTypeId)
                .orElse(null);
            Integer rhTypeId = leaveTypes.stream()
                .filter(lt -> "RH".equalsIgnoreCase(lt.getLeaveName()))
                .findFirst()
                .map(LeaveTypeMaster::getLeaveTypeId)
                .orElse(null);
            Integer compoffTypeId = leaveTypes.stream()
                .filter(lt -> "COMPOFF".equalsIgnoreCase(lt.getLeaveName()))
                .findFirst()
                .map(LeaveTypeMaster::getLeaveTypeId)
                .orElse(null);
            
            // Calculate CL balance
            if (clTypeId != null) {
                List<EmpLeaveApplication> clApproved = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndStatusAndIsDeletedFalse(emp.getEmpId(), clTypeId, "APPROVED BY HR");
                Double clOpening = clApproved.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                List<EmpLeaveApplication> clAvailedList;
                if (month != null && month > 0) {
                    clAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearMonthAndStatusAndIsDeletedFalse(emp.getEmpId(), clTypeId, year, month);
                } else {
                    clAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearAndStatusAndIsDeletedFalse(emp.getEmpId(), clTypeId, year);
                }
                Double clAvailed = clAvailedList.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                m.put("CLOpeningBalance", clOpening);
                m.put("CLAvailed", clAvailed);
                m.put("CLColsingBalance", clOpening - clAvailed);
                m.put("CLCarryFroward", 0.0);
            } else {
                m.put("CLOpeningBalance", 0.0);
                m.put("CLAvailed", 0.0);
                m.put("CLColsingBalance", 0.0);
                m.put("CLCarryFroward", 0.0);
            }
            
            // Calculate EL balance
            if (elTypeId != null) {
                List<EmpLeaveApplication> elApproved = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndStatusAndIsDeletedFalse(emp.getEmpId(), elTypeId, "APPROVED BY HR");
                Double elOpening = elApproved.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                List<EmpLeaveApplication> elAvailedList;
                if (month != null && month > 0) {
                    elAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearMonthAndStatusAndIsDeletedFalse(emp.getEmpId(), elTypeId, year, month);
                } else {
                    elAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearAndStatusAndIsDeletedFalse(emp.getEmpId(), elTypeId, year);
                }
                Double elAvailed = elAvailedList.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                m.put("ELOpeningBalance", elOpening);
                m.put("ELAvailed", elAvailed);
                m.put("ELColsingBalance", elOpening - elAvailed);
            } else {
                m.put("ELOpeningBalance", 0.0);
                m.put("ELAvailed", 0.0);
                m.put("ELColsingBalance", 0.0);
            }
            
            // Calculate RH balance
            if (rhTypeId != null) {
                List<EmpLeaveApplication> rhApproved = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndStatusAndIsDeletedFalse(emp.getEmpId(), rhTypeId, "APPROVED BY HR");
                Double rhOpening = rhApproved.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                List<EmpLeaveApplication> rhAvailedList;
                if (month != null && month > 0) {
                    rhAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearMonthAndStatusAndIsDeletedFalse(emp.getEmpId(), rhTypeId, year, month);
                } else {
                    rhAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearAndStatusAndIsDeletedFalse(emp.getEmpId(), rhTypeId, year);
                }
                Double rhAvailed = rhAvailedList.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                m.put("RHOpeningBalance", rhOpening);
                m.put("RHAvailed", rhAvailed);
                m.put("RHColsingBalance", rhOpening - rhAvailed);
            } else {
                m.put("RHOpeningBalance", 0.0);
                m.put("RHAvailed", 0.0);
                m.put("RHColsingBalance", 0.0);
            }
            
            // Calculate COMPOFF balance
            if (compoffTypeId != null) {
                List<EmpLeaveApplication> compoffApproved = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndStatusAndIsDeletedFalse(emp.getEmpId(), compoffTypeId, "APPROVED BY HR");
                Double compoffOpening = compoffApproved.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                List<EmpLeaveApplication> compoffAvailedList;
                if (month != null && month > 0) {
                    compoffAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearMonthAndStatusAndIsDeletedFalse(emp.getEmpId(), compoffTypeId, year, month);
                } else {
                    compoffAvailedList = empLeaveApplicationRepository.findByEmpIdAndLeaveTypeIdAndYearAndStatusAndIsDeletedFalse(emp.getEmpId(), compoffTypeId, year);
                }
                Double compoffAvailed = compoffAvailedList.stream().mapToDouble(e -> e.getNoOfDays() != null ? e.getNoOfDays().doubleValue() : 0.0).sum();
                
                m.put("COMPOFFOpeningBalance", compoffOpening);
                m.put("COMPOFFAvailed", compoffAvailed);
                m.put("COMPOFFColsingBalance", compoffOpening - compoffAvailed);
            } else {
                m.put("COMPOFFOpeningBalance", 0.0);
                m.put("COMPOFFAvailed", 0.0);
                m.put("COMPOFFColsingBalance", 0.0);
            }
            
            result.add(m);
        }
        
        return result;
    }

    public ApproveLeaveViewModel approveLeaveByHR(ApproveLeaveViewModel model) {
        List<Integer> approvedIds = new ArrayList<>();
        List<Integer> failedIds = new ArrayList<>();
        List<String> errors = new ArrayList<>();

        if (model.getLstofLevAppId() != null) {
            for (ApproveLeaveViewModel.LeaveAppIdItem item : model.getLstofLevAppId()) {
                Optional<EmpLeaveApplication> elaOpt = empLeaveApplicationRepository.findById(item.getLeaveAppId());
                if (elaOpt.isEmpty()) {
                    failedIds.add(item.getLeaveAppId());
                    errors.add("Leave application " + item.getLeaveAppId() + " not found");
                    continue;
                }

                EmpLeaveApplication ela = elaOpt.get();
                ela.setStatus("APPROVED BY HR");
                ela.setHrApproved(model.getApprovedBy());
                ela.setHrApprovedDate(new Date());
                ela.setRemarks(item.getRemarks());
                ela.setLastUpdatedDate(new Date());
                empLeaveApplicationRepository.save(ela);

                approvedIds.add(item.getLeaveAppId());
            }
        }

        ApproveLeaveViewModel result = new ApproveLeaveViewModel();
        result.setApprovedIds(approvedIds);
        result.setFailedIds(failedIds);
        result.setErrors(errors);
        result.setStatus(approvedIds.size() >0 ? "200" : "206");
        result.setMsg("Leave approved by HR");
        return result;
    }

    public ApproveLeaveViewModel rejectLeaveByHR(ApproveLeaveViewModel model) {
        List<Integer> rejectedIds = new ArrayList<>();
        List<Integer> failedIds = new ArrayList<>();
        List<String> errors = new ArrayList<>();

        if (model.getLstofLevAppId() != null) {
            for (ApproveLeaveViewModel.LeaveAppIdItem item : model.getLstofLevAppId()) {
                Optional<EmpLeaveApplication> elaOpt = empLeaveApplicationRepository.findById(item.getLeaveAppId());
                if (elaOpt.isEmpty()) {
                    failedIds.add(item.getLeaveAppId());
                    errors.add("Leave application " + item.getLeaveAppId() + " not found");
                    continue;
                }

                EmpLeaveApplication ela = elaOpt.get();
                ela.setStatus("REJECTED BY HR");
                ela.setHrApproved(model.getApprovedBy());
                ela.setRemarks(item.getRemarks());
                ela.setLastUpdatedDate(new Date());
                empLeaveApplicationRepository.save(ela);

                rejectedIds.add(item.getLeaveAppId());
            }
        }

        ApproveLeaveViewModel result = new ApproveLeaveViewModel();
        result.setApprovedIds(rejectedIds);
        result.setFailedIds(failedIds);
        result.setErrors(errors);
        result.setStatus(rejectedIds.size() >0 ? "200" : "206");
        result.setMsg("Leave rejected by HR");
        return result;
    }

    private int getMonth(Date date) {
        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        return cal.get(Calendar.MONTH) + 1;
    }

    private int getYear(Date date) {
        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        return cal.get(Calendar.YEAR);
    }
}
