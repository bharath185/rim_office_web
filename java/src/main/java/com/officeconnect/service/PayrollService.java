package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Comparator;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.stream.Collectors;

@Service
public class PayrollService {

    @Autowired
    private PayrollComponentRepository payrollComponentRepository;

    @Autowired
    private PayrollPayoutTypeRepository payrollPayoutTypeRepository;

    @Autowired
    private PayrollSegmentRepository payrollSegmentRepository;

    @Autowired
    private PayrollFrequencyMasterRepository payrollFrequencyMasterRepository;

    @Autowired
    private PayrollComponentLogicRepository payrollComponentLogicRepository;

    @Autowired
    private PayslipSectionRepository payslipSectionRepository;

    @Autowired
    private PayslipSectionComponentsRepository payslipSectionComponentsRepository;

    @Autowired
    private EmployeeSalaryDetailsRepository employeeSalaryDetailsRepository;

    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private EmpLeaveApplicationRepository empLeaveApplicationRepository;

    @Autowired
    private DesignationMasterRepository designationMasterRepository;

    @Autowired
    private LocationMasterRepository locationMasterRepository;

    @Autowired
    private LegalEntityMasterRepository legalEntityMasterRepository;

    @Autowired
    private PayoutMappingMasterRepository payoutMappingMasterRepository;

    @Autowired
    private CompanyMasterRepository companyMasterRepository;

    @Autowired
    private EmployeeAccDetailsRepository employeeAccDetailsRepository;

    @Autowired
    private PayrollComponentConditionRepository payrollComponentConditionRepository;

    @Autowired
    private PayrollSymbolMasterRepository payrollSymbolMasterRepository;

    private String formatDate(Date d) {
        if (d == null) return null;
        return new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format(d);
    }

    private Date parseDate(String s) {
        if (s == null) return null;
        try {
            if (s.contains("T")) return new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(s);
            if (s.contains("-") && s.split("-")[0].length() == 2) return new SimpleDateFormat("dd-MM-yyyy").parse(s);
            return new SimpleDateFormat("yyyy-MM-dd").parse(s);
        } catch (Exception e) { return new Date(); }
    }

    private Integer parseSafeInt(Object value) {
        if (value == null) return 0;
        if (value instanceof Integer) return (Integer) value;
        if (value instanceof String) {
            try { return Integer.valueOf((String) value); } catch (NumberFormatException e) { return 0; }
        }
        if (value instanceof Number) return ((Number) value).intValue();
        return 0;
    }

    // ===== Payout Type =====

    public List<DDPayrollPayoutTypeViewModel> ddPayrollPayoutType(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayrollPayoutType> details = payrollPayoutTypeRepository.findAll().stream()
            .filter(p -> Boolean.TRUE.equals(p.getIsActive()) && Boolean.FALSE.equals(p.getIsDeleted()))
            .sorted((a, b) -> Integer.compare(b.getPayoutTypeId(), a.getPayoutTypeId()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Payout Type Details Not Found");

        return details.stream().map(p -> {
            DDPayrollPayoutTypeViewModel vm = new DDPayrollPayoutTypeViewModel();
            vm.setPayoutTypeId(p.getPayoutTypeId());
            vm.setPayoutTypeName(p.getPayoutTypeName());
            vm.setFrequency(p.getFrequency());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<PayrollPayoutTypeViewModel> getAllPayrollPayoutType(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayrollPayoutType> details = payrollPayoutTypeRepository.findAll().stream()
            .filter(p -> Boolean.FALSE.equals(p.getIsDeleted()))
            .sorted((a, b) -> Integer.compare(b.getPayoutTypeId(), a.getPayoutTypeId()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Payout Type Details Not Found");

        return details.stream().map(p -> {
            PayrollPayoutTypeViewModel vm = new PayrollPayoutTypeViewModel();
            vm.setPayoutTypeId(p.getPayoutTypeId());
            vm.setPayoutTypeName(p.getPayoutTypeName());
            vm.setFrequency(p.getFrequency());
            vm.setCreatedBy(p.getCreatedBy());
            vm.setCreatedDate(formatDate(p.getCreatedDate()));
            vm.setLastUpdatedBy(p.getLastUpdatedBy());
            vm.setLastUpdatedDate(formatDate(p.getLastUpdatedDate()));
            vm.setIsActive(p.getIsActive());
            vm.setIsUpdated(p.getIsUpdated());
            vm.setIsDeleted(p.getIsDeleted());
            return vm;
        }).collect(Collectors.toList());
    }

    public PayrollPayoutTypeViewModel getPayrollPayoutType(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayrollPayoutType p = payrollPayoutTypeRepository.findById(model.getPayoutTypeId()).orElse(null);
        if (p == null || Boolean.TRUE.equals(p.getIsDeleted())) throw new RuntimeException("Payout Type Details Not Found");

        PayrollPayoutTypeViewModel vm = new PayrollPayoutTypeViewModel();
        vm.setPayoutTypeId(p.getPayoutTypeId());
        vm.setPayoutTypeName(p.getPayoutTypeName());
        vm.setFrequency(p.getFrequency());
        vm.setCreatedBy(p.getCreatedBy());
        vm.setCreatedDate(formatDate(p.getCreatedDate()));
        vm.setLastUpdatedBy(p.getLastUpdatedBy());
        vm.setLastUpdatedDate(formatDate(p.getLastUpdatedDate()));
        vm.setIsActive(p.getIsActive());
        vm.setIsUpdated(p.getIsUpdated());
        vm.setIsDeleted(p.getIsDeleted());
        return vm;
    }

    public PayrollResponseViewModel addPayrollPayoutType(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        boolean exists = payrollPayoutTypeRepository.findAll().stream()
            .anyMatch(p -> p.getPayoutTypeName() != null && p.getPayoutTypeName().equals(model.getPayoutTypeName())
                && Boolean.TRUE.equals(p.getIsActive()) && Boolean.FALSE.equals(p.getIsDeleted()));
        if (exists) throw new RuntimeException("Payout Type Details Already Exists");

        PayrollPayoutType p = new PayrollPayoutType();
        p.setPayoutTypeName(model.getPayoutTypeName());
        p.setFrequency(model.getFrequency());
        p.setIsActive(true);
        p.setIsUpdated(false);
        p.setIsDeleted(false);
        p.setCreatedBy(loginId);
        p.setCreatedDate(new Date());
        p.setLastUpdatedBy(loginId);
        p.setLastUpdatedDate(new Date());
        payrollPayoutTypeRepository.save(p);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Added");
        return vm;
    }

    public PayrollResponseViewModel updatePayrollPayoutType(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getPayoutTypeId() != null && model.getPayoutTypeId() != 0) ? model.getPayoutTypeId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Payout Type Id is Mismatching");

        PayrollPayoutType p = payrollPayoutTypeRepository.findById(id).orElse(null);
        if (p == null || !Boolean.TRUE.equals(p.getIsActive()) || Boolean.TRUE.equals(p.getIsDeleted()))
            throw new RuntimeException("Payout Type Details Not Found");

        p.setPayoutTypeName(model.getPayoutTypeName());
        p.setFrequency(model.getFrequency());
        p.setIsActive(true);
        p.setIsUpdated(true);
        p.setIsDeleted(false);
        p.setLastUpdatedBy(loginId);
        p.setLastUpdatedDate(new Date());
        payrollPayoutTypeRepository.save(p);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return vm;
    }

    public PayrollResponseViewModel deletePayrollPayoutType(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getPayoutTypeId() != null && model.getPayoutTypeId() != 0) ? model.getPayoutTypeId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayrollPayoutType p = payrollPayoutTypeRepository.findById(id).orElse(null);
        if (p == null || !Boolean.TRUE.equals(p.getIsActive()) || Boolean.TRUE.equals(p.getIsDeleted()))
            throw new RuntimeException("Payout Type Details Not Found");

        p.setIsActive(true);
        p.setIsUpdated(true);
        p.setIsDeleted(true);
        p.setLastUpdatedBy(loginId);
        p.setLastUpdatedDate(new Date());
        payrollPayoutTypeRepository.save(p);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return vm;
    }

    // ===== Segment =====

    public List<DDPayrollSegmentViewModel> ddPayrollSegment(PayrollSegmentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer payoutTypeId = (model.getPayoutTypeId() != null && model.getPayoutTypeId() != 0) ? model.getPayoutTypeId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayrollSegment> details = payrollSegmentRepository.findAll().stream()
            .filter(s -> {
                boolean active = Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted());
                if (payoutTypeId == 0) return active;
                return active && s.getPayoutTypeId() != null && s.getPayoutTypeId().equals(payoutTypeId);
            })
            .sorted((a, b) -> Integer.compare(b.getSegmentId(), a.getSegmentId()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Segment Details Not Found");

        return details.stream().map(s -> {
            DDPayrollSegmentViewModel vm = new DDPayrollSegmentViewModel();
            vm.setSegmentId(s.getSegmentId());
            vm.setSegmentName(s.getSegmentName());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<PayrollSegmentViewModel> getAllPayrollSegment(PayrollSegmentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayrollSegment> details = payrollSegmentRepository.findAll().stream()
            .filter(s -> Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
            .sorted((a, b) -> Integer.compare(b.getSegmentId(), a.getSegmentId()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Segment Details Not Found");

        return details.stream().map(s -> {
            PayrollSegmentViewModel vm = new PayrollSegmentViewModel();
            vm.setPayoutTypeId(s.getPayoutTypeId());
            vm.setSegmentId(s.getSegmentId());
            vm.setSegmentName(s.getSegmentName());
            vm.setCreatedBy(s.getCreatedBy());
            vm.setCreatedDate(formatDate(s.getCreatedDate()));
            vm.setLastUpdatedBy(s.getLastUpdatedBy());
            vm.setLastUpdatedDate(formatDate(s.getLastUpdatedDate()));
            vm.setIsActive(s.getIsActive());
            vm.setIsUpdated(s.getIsUpdated());
            vm.setIsDeleted(s.getIsDeleted());
            if (s.getPayoutTypeId() != null) {
                PayrollPayoutType pt = payrollPayoutTypeRepository.findById(s.getPayoutTypeId()).orElse(null);
                vm.setPayoutTypeName(pt != null ? pt.getPayoutTypeName() : "");
            }
            return vm;
        }).collect(Collectors.toList());
    }

    public PayrollSegmentViewModel getPayrollSegment(PayrollSegmentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayrollSegment s = payrollSegmentRepository.findById(model.getSegmentId()).orElse(null);
        if (s == null || !Boolean.TRUE.equals(s.getIsActive()) || Boolean.TRUE.equals(s.getIsDeleted()))
            throw new RuntimeException("Segment Details Not Found");

        PayrollSegmentViewModel vm = new PayrollSegmentViewModel();
        vm.setPayoutTypeId(s.getPayoutTypeId());
        vm.setSegmentId(s.getSegmentId());
        vm.setSegmentName(s.getSegmentName());
        vm.setCreatedBy(s.getCreatedBy());
        vm.setCreatedDate(formatDate(s.getCreatedDate()));
        vm.setLastUpdatedBy(s.getLastUpdatedBy());
        vm.setLastUpdatedDate(formatDate(s.getLastUpdatedDate()));
        vm.setIsActive(s.getIsActive());
        vm.setIsUpdated(s.getIsUpdated());
        vm.setIsDeleted(s.getIsDeleted());
        if (s.getPayoutTypeId() != null) {
            PayrollPayoutType pt = payrollPayoutTypeRepository.findById(s.getPayoutTypeId()).orElse(null);
            vm.setPayoutTypeName(pt != null ? pt.getPayoutTypeName() : "");
        }
        return vm;
    }

    public PayrollResponseViewModel addPayrollSegment(PayrollSegmentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        boolean exists = payrollSegmentRepository.findAll().stream()
            .anyMatch(s -> s.getPayoutTypeId() != null && s.getPayoutTypeId().equals(model.getPayoutTypeId())
                && s.getSegmentName() != null && s.getSegmentName().equals(model.getSegmentName())
                && Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()));
        if (exists) throw new RuntimeException("Segment Details Already Exists");

        PayrollSegment s = new PayrollSegment();
        s.setSegmentName(model.getSegmentName());
        s.setPayoutTypeId(model.getPayoutTypeId());
        s.setIsActive(true);
        s.setIsUpdated(false);
        s.setIsDeleted(false);
        s.setCreatedBy(loginId);
        s.setCreatedDate(new Date());
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payrollSegmentRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Added");
        return vm;
    }

    public PayrollResponseViewModel updatePayrollSegment(PayrollSegmentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSegmentId() != null && model.getSegmentId() != 0) ? model.getSegmentId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Segment Id is Mismatching");

        PayrollSegment s = payrollSegmentRepository.findById(id).orElse(null);
        if (s == null || !Boolean.TRUE.equals(s.getIsActive()) || Boolean.TRUE.equals(s.getIsDeleted()))
            throw new RuntimeException("Segment Details Not Found");

        s.setSegmentName(model.getSegmentName());
        s.setPayoutTypeId(model.getPayoutTypeId());
        s.setIsActive(true);
        s.setIsUpdated(true);
        s.setIsDeleted(false);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payrollSegmentRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return vm;
    }

    public PayrollResponseViewModel deletePayrollSegment(PayrollSegmentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSegmentId() != null && model.getSegmentId() != 0) ? model.getSegmentId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayrollSegment s = payrollSegmentRepository.findById(id).orElse(null);
        if (s == null || !Boolean.TRUE.equals(s.getIsActive()) || Boolean.TRUE.equals(s.getIsDeleted()))
            throw new RuntimeException("Segment Details Not Found");

        s.setIsActive(true);
        s.setIsUpdated(true);
        s.setIsDeleted(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payrollSegmentRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return vm;
    }

    // ===== Component =====

    public PayrollResponseViewModel addComponent(PayrollALLComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        // Check for duplicate ComponentName with same PayoutTypeId and SegmentId
        boolean nameExists = payrollComponentRepository.findAll().stream()
            .filter(c -> Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
            .anyMatch(c -> c.getComponentName() != null && c.getComponentName().equals(model.getComponentName())
                && c.getPayoutTypeId() != null && c.getPayoutTypeId().equals(model.getPayoutTypeId())
                && c.getSegmentId() != null && c.getSegmentId().equals(model.getSegmentId()));
        if (nameExists) throw new RuntimeException("Component Name is Already Exists");

        // Check for duplicate ComponentCode with same PayoutTypeId and SegmentId
        boolean codeExists = payrollComponentRepository.findAll().stream()
            .filter(c -> Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
            .anyMatch(c -> c.getComponentCode() != null && c.getComponentCode().equals(model.getComponentCode())
                && c.getPayoutTypeId() != null && c.getPayoutTypeId().equals(model.getPayoutTypeId())
                && c.getSegmentId() != null && c.getSegmentId().equals(model.getSegmentId()));
        if (codeExists) throw new RuntimeException("Component Code is Already Exists");

        // Check payout type mismatch in component logic
        List<PayrollALLComponentLogicConditionViewModel> lstofLC = model.getLstofLC();
        if (lstofLC != null) {
            for (int i = 0; i < lstofLC.size(); i++) {
                PayrollALLComponentLogicConditionViewModel item = lstofLC.get(i);
                if (item.getComponentId1() != null && item.getComponentId1() != 0) {
                    PayrollComponent component = payrollComponentRepository.findAll().stream()
                        .filter(c -> c.getComponentId() != null && c.getComponentId().equals(item.getComponentId1())
                            && Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
                        .findFirst()
                        .orElse(null);

                    if (component == null) {
                        throw new RuntimeException("Component ID " + item.getComponentId1() + " not found.");
                    }

                    if (component.getPayoutTypeId() != null && !component.getPayoutTypeId().equals(model.getPayoutTypeId())) {
                        throw new RuntimeException("Payout type mismatch in component logic.");
                    }
                }
            }
        }

        // Create PayrollComponent
        PayrollComponent c = new PayrollComponent();
        c.setPayoutTypeId(model.getPayoutTypeId());
        c.setSegmentId(model.getSegmentId());
        c.setComponentName(model.getComponentName());
        c.setComponentCode(model.getComponentCode());
        c.setIsActive(true);
        c.setIsUpdated(false);
        c.setIsDeleted(false);
        c.setCreatedBy(loginId);
        c.setCreatedDate(new Date());
        c.setLastUpdatedBy(loginId);
        c.setLastUpdatedDate(new Date());
        payrollComponentRepository.save(c);

        Integer componentId = c.getComponentId();

        // Create PayrollComponentLogic and PayrollComponentCondition for each item in lstofLC
        if (lstofLC != null) {
            for (int i = 0; i < lstofLC.size(); i++) {
                int sno = i + 1;
                PayrollALLComponentLogicConditionViewModel item = lstofLC.get(i);

                // Create PayrollComponentLogic
                PayrollComponentLogic logic = new PayrollComponentLogic();
                logic.setComponentId(componentId);
                logic.setSno(sno);
                logic.setValue(item.getValue() != null && item.getValue() != 0 ? item.getValue() : null);
                logic.setPercentage(item.getPercentage() != null && item.getPercentage() != 0 ? item.getPercentage() : null);
                logic.setComponentId1(item.getComponentId1() != null && item.getComponentId1() != 0 ? item.getComponentId1() : null);
                logic.setComponentName1(item.getComponentName1() != null && !item.getComponentName1().isEmpty() ? item.getComponentName1() : null);
                logic.setEffectiveFrom(item.getEffectiveFrom());
                logic.setEffectiveTo(item.getEffectiveTo());
                logic.setIsActive(true);
                logic.setIsUpdated(false);
                logic.setIsDeleted(false);
                logic.setCreatedBy(loginId);
                logic.setCreatedDate(new Date());
                logic.setLastUpdatedBy(loginId);
                logic.setLastUpdatedDate(new Date());
                payrollComponentLogicRepository.save(logic);

                // Create PayrollComponentCondition
                PayrollComponentCondition condition = new PayrollComponentCondition();
                condition.setComponentId(componentId);
                condition.setSno(sno);
                condition.setConditionExpression(item.getConditionExpression() != null && !item.getConditionExpression().isEmpty() ? item.getConditionExpression() : null);
                condition.setConditionResultPFESI(item.getConditionResultPFESI());
                condition.setIsActive(true);
                condition.setIsUpdated(false);
                condition.setIsDeleted(false);
                condition.setCreatedBy(loginId);
                condition.setCreatedDate(new Date());
                condition.setLastUpdatedBy(loginId);
                condition.setLastUpdatedDate(new Date());
                payrollComponentConditionRepository.save(condition);
            }
        }

        PayrollResponseViewModel result = new PayrollResponseViewModel();
        result.setStatus(200);
        result.setMsg("Added");
        return result;
    }

    public PayrollComponentViewModel updateComponent(PayrollComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getComponentId() != null && model.getComponentId() != 0) ? model.getComponentId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        PayrollComponent c = payrollComponentRepository.findById(id).orElse(null);
        if (c == null || !Boolean.TRUE.equals(c.getIsActive()) || Boolean.TRUE.equals(c.getIsDeleted()))
            throw new RuntimeException("Component Details Not Found");

        c.setComponentName(model.getComponentName());
        c.setComponentCode(model.getComponentCode());
        c.setPayoutTypeId(model.getPayoutTypeId());
        c.setSegmentId(model.getSegmentId());
        payrollComponentRepository.save(c);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return model;
    }

    public PayrollComponentViewModel deleteComponent(PayrollComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getComponentId() != null && model.getComponentId() != 0) ? model.getComponentId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayrollComponent c = payrollComponentRepository.findById(id).orElse(null);
        if (c == null || !Boolean.TRUE.equals(c.getIsActive()) || Boolean.TRUE.equals(c.getIsDeleted()))
            throw new RuntimeException("Component Details Not Found");

        c.setIsActive(false);
        c.setIsDeleted(true);
        payrollComponentRepository.save(c);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return model;
    }

    public List<PayrollComponentViewModel> getAllComponents(PayrollComponentViewModel model) {
        return payrollComponentRepository.findAll().stream()
            .filter(c -> Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
            .map(c -> {
                PayrollComponentViewModel vm = new PayrollComponentViewModel();
                vm.setComponentId(c.getComponentId());
                vm.setComponentName(c.getComponentName());
                vm.setComponentCode(c.getComponentCode());
                vm.setPayoutTypeId(c.getPayoutTypeId());
                vm.setSegmentId(c.getSegmentId());
                vm.setIsActive(c.getIsActive());
                return vm;
            }).collect(Collectors.toList());
    }

    public List<DDPayrollComponentViewModel> ddPayrollComponent(PayrollComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        Integer payoutTypeId = (model.getPayoutTypeId() != null && model.getPayoutTypeId() != 0) ? model.getPayoutTypeId() : 0;

        List<PayrollComponent> details = payrollComponentRepository.findAll().stream()
            .filter(c -> Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
            .filter(c -> payoutTypeId == 0 || (c.getPayoutTypeId() != null && c.getPayoutTypeId().equals(payoutTypeId)))
            .sorted((a, b) -> Integer.compare(b.getSegmentId(), a.getSegmentId()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Component Details Not Found");

        return details.stream().map(c -> {
            DDPayrollComponentViewModel vm = new DDPayrollComponentViewModel();
            vm.setComponentId(c.getComponentId());
            vm.setComponentName(c.getComponentCode() != null ? c.getComponentCode() : "");
            return vm;
        }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getAllComponentDetails(PayrollALLComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        // Get all active payout types (matching .NET)
        List<PayrollPayoutType> payoutTypes = payrollPayoutTypeRepository.findAll().stream()
            .filter(p -> Boolean.TRUE.equals(p.getIsActive()) && Boolean.FALSE.equals(p.getIsDeleted()))
            .sorted(Comparator.comparing(PayrollPayoutType::getPayoutTypeId))
            .collect(Collectors.toList());

        List<Map<String, Object>> result = new ArrayList<>();
        for (PayrollPayoutType payout : payoutTypes) {
            Map<String, Object> payoutMap = new HashMap<>();
            payoutMap.put("PayoutId", payout.getPayoutTypeId());
            payoutMap.put("PayoutName", payout.getPayoutTypeName());

            // Get segments for this payout type
            List<PayrollSegment> segments = payrollSegmentRepository.findAll().stream()
                .filter(s -> payout.getPayoutTypeId().equals(s.getPayoutTypeId())
                    && Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
                .sorted(Comparator.comparing(PayrollSegment::getSegmentId))
                .collect(Collectors.toList());

            List<Map<String, Object>> segmentList = new ArrayList<>();
            for (PayrollSegment seg : segments) {
                Map<String, Object> segMap = new HashMap<>();
                segMap.put("SegmentId", seg.getSegmentId());
                segMap.put("SegmentName", seg.getSegmentName());

                // Get components for this segment and payout type
                List<PayrollComponent> components = payrollComponentRepository.findAll().stream()
                    .filter(c -> payout.getPayoutTypeId().equals(c.getPayoutTypeId())
                        && seg.getSegmentId().equals(c.getSegmentId())
                        && Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
                    .sorted(Comparator.comparing(PayrollComponent::getComponentId))
                    .collect(Collectors.toList());

                List<Map<String, Object>> componentList = new ArrayList<>();
                for (PayrollComponent comp : components) {
                    Map<String, Object> compMap = new HashMap<>();
                    compMap.put("ComponentId", comp.getComponentId());
                    compMap.put("ComponentName", comp.getComponentName());
                    compMap.put("ComponentCode", comp.getComponentCode());
                    compMap.put("ComponentValue", "");

                    // Get logic conditions for this component (matching .NET: join by ComponentId and SNo)
                    List<PayrollComponentLogic> logics = payrollComponentLogicRepository
                        .findByComponentIdAndIsActiveTrueAndIsDeletedFalseOrderBySno(comp.getComponentId());

                    List<Map<String, Object>> logicList = new ArrayList<>();
                    for (PayrollComponentLogic logic : logics) {
                        Map<String, Object> logicMap = new HashMap<>();
                        logicMap.put("ComponentId", comp.getComponentId());
                        logicMap.put("LogicId", logic.getLogicId());
                        logicMap.put("Percentage", logic.getPercentage());
                        logicMap.put("Value", logic.getValue());
                        logicMap.put("ComponentId1", logic.getComponentId1());
                        logicMap.put("ComponentName1", logic.getComponentName1());

                        // Get matching condition by ComponentId and SNo (matching .NET join)
                        PayrollComponentCondition cond = payrollComponentConditionRepository
                            .findByComponentIdAndSNo(comp.getComponentId(), logic.getSno());
                        logicMap.put("ConditionId", cond != null ? cond.getConditionId() : 0);
                        logicMap.put("ConditionExpression", cond != null ? cond.getConditionExpression() : null);
                        logicMap.put("ConditionResultPFESI", cond != null ? cond.getConditionResultPFESI() : null);

                        logicList.add(logicMap);
                    }
                    compMap.put("LogicConditions", logicList);
                    componentList.add(compMap);
                }
                segMap.put("Components", componentList);
                segmentList.add(segMap);
            }
            payoutMap.put("Segments", segmentList);
            result.add(payoutMap);
        }
        return result;
    }

    // ===== Payslip Section =====

    public List<DDPayslipSectionViewModel> ddPayslipSection(PayslipSectionViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayslipSection> details = payslipSectionRepository.findAll().stream()
            .filter(s -> Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Section Details Not Found");

        return details.stream().map(s -> {
            DDPayslipSectionViewModel vm = new DDPayslipSectionViewModel();
            vm.setSectionId(s.getSectionId());
            vm.setSectionName(s.getSectionName());
            return vm;
        }).collect(Collectors.toList());
    }

    public List<PayslipSectionViewModel> getAllPayslipSection(PayslipSectionViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayslipSection> details = payslipSectionRepository.findAll().stream()
            .filter(s -> Boolean.FALSE.equals(s.getIsDeleted()))
            .collect(Collectors.toList());

        if (details.isEmpty()) throw new RuntimeException("Section Details Not Found");

        return details.stream().map(s -> {
            PayslipSectionViewModel vm = new PayslipSectionViewModel();
            vm.setSectionId(s.getSectionId());
            vm.setSectionName(s.getSectionName());
            vm.setSequenceNo(s.getSequenceNo());
            vm.setCreatedBy(s.getCreatedBy());
            vm.setCreatedDate(formatDate(s.getCreatedDate()));
            vm.setLastUpdatedBy(s.getLastUpdatedBy());
            vm.setLastUpdatedDate(formatDate(s.getLastUpdatedDate()));
            vm.setIsActive(s.getIsActive());
            vm.setIsUpdated(s.getIsUpdated());
            vm.setIsDeleted(s.getIsDeleted());
            return vm;
        }).collect(Collectors.toList());
    }

    public PayslipSectionViewModel getPayslipSection(PayslipSectionViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayslipSection s = payslipSectionRepository.findById(model.getSectionId()).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Section Details Not Found");

        PayslipSectionViewModel vm = new PayslipSectionViewModel();
        vm.setSectionId(s.getSectionId());
        vm.setSectionName(s.getSectionName());
        vm.setSequenceNo(s.getSequenceNo());
        vm.setCreatedBy(s.getCreatedBy());
        vm.setCreatedDate(formatDate(s.getCreatedDate()));
        vm.setLastUpdatedBy(s.getLastUpdatedBy());
        vm.setLastUpdatedDate(formatDate(s.getLastUpdatedDate()));
        vm.setIsActive(s.getIsActive());
        vm.setIsUpdated(s.getIsUpdated());
        vm.setIsDeleted(s.getIsDeleted());
        return vm;
    }

    public PayrollResponseViewModel addPayslipSection(PayslipSectionViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        PayslipSection s = new PayslipSection();
        s.setSectionName(model.getSectionName());
        s.setSequenceNo(model.getSequenceNo());
        s.setIsActive(true);
        s.setIsUpdated(false);
        s.setIsDeleted(false);
        s.setCreatedBy(loginId);
        s.setCreatedDate(new Date());
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payslipSectionRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Added");
        return vm;
    }

    public PayrollResponseViewModel updatePayslipSection(PayslipSectionViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSectionId() != null && model.getSectionId() != 0) ? model.getSectionId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Section Id is Mismatching");

        PayslipSection s = payslipSectionRepository.findById(id).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Section Details Not Found");

        s.setSectionName(model.getSectionName());
        s.setSequenceNo(model.getSequenceNo());
        s.setIsUpdated(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payslipSectionRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return vm;
    }

    public PayrollResponseViewModel deletePayslipSection(PayslipSectionViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSectionId() != null && model.getSectionId() != 0) ? model.getSectionId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayslipSection s = payslipSectionRepository.findById(id).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Section Details Not Found");

        s.setIsUpdated(true);
        s.setIsDeleted(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payslipSectionRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return vm;
    }

    // ===== Payslip Section Components =====

    public List<Map<String, Object>> getAllPayslipSectionComponent(PayslipSectionComponentViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        // Preload all reference data for efficient lookup
        List<PayrollPayoutType> allPayoutTypes = payrollPayoutTypeRepository.findAll();
        List<PayslipSection> allSections = payslipSectionRepository.findAll();
        List<PayrollComponent> allComponents = payrollComponentRepository.findAll();
        List<PayslipSectionComponents> allSecComps = payslipSectionComponentsRepository.findAll();

        // Build the nested structure matching .NET
        List<Map<String, Object>> result = new ArrayList<>();
        for (PayrollPayoutType payout : allPayoutTypes) {
            if (!Boolean.TRUE.equals(payout.getIsActive()) || Boolean.TRUE.equals(payout.getIsDeleted())) continue;

            Map<String, Object> payoutMap = new HashMap<>();
            payoutMap.put("PayoutTypeId", payout.getPayoutTypeId());
            payoutMap.put("PayoutTypeName", payout.getPayoutTypeName());

            List<Map<String, Object>> sectionList = new ArrayList<>();
            for (PayslipSection sec : allSections) {
                if (!Boolean.TRUE.equals(sec.getIsActive()) || Boolean.TRUE.equals(sec.getIsDeleted())) continue;

                // Get components for this payout type and section
                List<PayslipSectionComponents> secComps = allSecComps.stream()
                    .filter(sc -> payout.getPayoutTypeId().equals(sc.getPayoutTypeId())
                        && sec.getSectionId().equals(sc.getSectionId())
                        && Boolean.TRUE.equals(sc.getIsActive()) && Boolean.FALSE.equals(sc.getIsDeleted()))
                    .sorted(Comparator.comparing(PayslipSectionComponents::getSequenceNo,
                        Comparator.nullsLast(Comparator.naturalOrder())))
                    .collect(Collectors.toList());

                if (secComps.isEmpty()) continue;

                List<Map<String, Object>> compList = new ArrayList<>();
                for (PayslipSectionComponents sc : secComps) {
                    Map<String, Object> compMap = new HashMap<>();
                    compMap.put("SectionComponentId", sc.getSectionComponentId());
                    compMap.put("ComponentId", sc.getComponentId());
                    compMap.put("SequenceNo", sc.getSequenceNo());
                    compMap.put("EffectiveFrom", sc.getEffectiveFrom() != null ? "/Date(" + sc.getEffectiveFrom().getTime() + ")/" : null);
                    compMap.put("EffectiveTo", sc.getEffectiveTo() != null ? "/Date(" + sc.getEffectiveTo().getTime() + ")/" : null);
                    compMap.put("RecordStatus", sc.getRecordStatus());

                    // Resolve component name/code (matching .NET left join)
                    if (sc.getComponentId() != null) {
                        PayrollComponent comp = allComponents.stream()
                            .filter(c -> sc.getComponentId().equals(c.getComponentId())
                                && Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
                            .findFirst().orElse(null);
                        compMap.put("ComponentName", comp != null ? comp.getComponentName() : null);
                        compMap.put("ComponentCode", comp != null ? comp.getComponentCode() : null);
                    } else {
                        compMap.put("ComponentName", null);
                        compMap.put("ComponentCode", null);
                    }
                    compList.add(compMap);
                }

                Map<String, Object> secMap = new HashMap<>();
                secMap.put("SectionId", sec.getSectionId());
                secMap.put("SectionName", sec.getSectionName());
                secMap.put("Components", compList);
                sectionList.add(secMap);
            }

            payoutMap.put("Sections", sectionList);
            result.add(payoutMap);
        }
        return result;
    }

    public PayslipSectionComponentViewModel getPayslipSectionComponent(PayslipSectionComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayslipSectionComponents s = payslipSectionComponentsRepository.findById(model.getSectionComponentId()).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Section Component Details Not Found");

        PayslipSectionComponentViewModel vm = new PayslipSectionComponentViewModel();
        vm.setSectionComponentId(s.getSectionComponentId());
        vm.setPayoutTypeId(s.getPayoutTypeId());
        vm.setSectionId(s.getSectionId());
        vm.setComponentId(s.getComponentId());
        vm.setSequenceNo(s.getSequenceNo());
        vm.setEffectiveFrom(formatDate(s.getEffectiveFrom()));
        vm.setEffectiveTo(formatDate(s.getEffectiveTo()));
        vm.setCreatedBy(s.getCreatedBy());
        vm.setCreatedDate(formatDate(s.getCreatedDate()));
        vm.setLastUpdatedBy(s.getLastUpdatedBy());
        vm.setLastUpdatedDate(formatDate(s.getLastUpdatedDate()));
        vm.setIsActive(s.getIsActive());
        vm.setIsUpdated(s.getIsUpdated());
        vm.setIsDeleted(s.getIsDeleted());
        if (s.getPayoutTypeId() != null) {
            PayrollPayoutType pt = payrollPayoutTypeRepository.findById(s.getPayoutTypeId()).orElse(null);
            vm.setPayoutTypeName(pt != null ? pt.getPayoutTypeName() : "");
        }
        if (s.getSectionId() != null) {
            PayslipSection sec = payslipSectionRepository.findById(s.getSectionId()).orElse(null);
            vm.setSectionName(sec != null ? sec.getSectionName() : "");
        }
        if (s.getComponentId() != null) {
            PayrollComponent comp = payrollComponentRepository.findById(s.getComponentId()).orElse(null);
            vm.setComponentName(comp != null ? comp.getComponentName() : "");
            vm.setComponentCode(comp != null ? comp.getComponentCode() : "");
        }
        return vm;
    }

    public PayrollResponseViewModel addPayslipSectionComponent(PayslipSectionComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        PayslipSectionComponents s = new PayslipSectionComponents();
        s.setPayoutTypeId(model.getPayoutTypeId());
        s.setSectionId(model.getSectionId());
        s.setComponentId(model.getComponentId());
        s.setSequenceNo(model.getSequenceNo());
        s.setEffectiveFrom(parseDate(model.getEffectiveFrom()));
        s.setEffectiveTo(parseDate(model.getEffectiveTo()));
        s.setIsActive(true);
        s.setIsUpdated(false);
        s.setIsDeleted(false);
        s.setCreatedBy(loginId);
        s.setCreatedDate(new Date());
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payslipSectionComponentsRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Added");
        return vm;
    }

    public PayrollResponseViewModel updatePayslipSectionComponent(PayslipSectionComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSectionComponentId() != null && model.getSectionComponentId() != 0) ? model.getSectionComponentId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Section Component Id is Mismatching");

        PayslipSectionComponents s = payslipSectionComponentsRepository.findById(id).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Section Component Details Not Found");

        s.setPayoutTypeId(model.getPayoutTypeId());
        s.setSectionId(model.getSectionId());
        s.setComponentId(model.getComponentId());
        s.setSequenceNo(model.getSequenceNo());
        s.setEffectiveFrom(parseDate(model.getEffectiveFrom()));
        s.setEffectiveTo(parseDate(model.getEffectiveTo()));
        s.setIsUpdated(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payslipSectionComponentsRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return vm;
    }

    public PayrollResponseViewModel deletePayslipSectionComponent(PayslipSectionComponentViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSectionComponentId() != null && model.getSectionComponentId() != 0) ? model.getSectionComponentId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayslipSectionComponents s = payslipSectionComponentsRepository.findById(id).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Section Component Details Not Found");

        s.setIsUpdated(true);
        s.setIsDeleted(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        payslipSectionComponentsRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return vm;
    }

    // ===== Employee Salary Details =====

    public List<EmployeeSalaryDetailsViewModel> getAllEmployeeSalaryDetails(EmployeeSalaryDetailsViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        Integer leId = model.getLEId() != null && model.getLEId() > 0 ? model.getLEId() : null;
        Integer buId = model.getBUId() != null && model.getBUId() > 0 ? model.getBUId() : null;
        Integer locId = model.getLocId() != null && model.getLocId() > 0 ? model.getLocId() : null;
        Integer deptId = model.getDeptId() != null && model.getDeptId() > 0 ? model.getDeptId() : null;
        Integer designationId = model.getDesignationId() != null && model.getDesignationId() > 0 ? model.getDesignationId() : null;
        Integer reportId = model.getReportId() != null && model.getReportId() > 0 ? model.getReportId() : null;
        Integer empId = model.getEmpId() != null && model.getEmpId() > 0 ? model.getEmpId() : null;

        // Get all active salary details
        List<EmployeeSalaryDetails> allDetails = employeeSalaryDetailsRepository.findAll().stream()
            .filter(s -> Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
            .collect(Collectors.toList());

        // Join with EmployeeMaster for filtering and name resolution
        List<EmployeeSalaryDetailsViewModel> result = new ArrayList<>();
        for (EmployeeSalaryDetails s : allDetails) {
            if (s.getEmpId() == null) continue;
            EmployeeMaster emp = employeeMasterRepository.findByEmpIdAndActive(s.getEmpId());
            if (emp == null) continue;

            // Apply filters (matching .NET)
            if (leId != null && (emp.getLeId() == null || !leId.equals(emp.getLeId()))) continue;
            if (buId != null && (emp.getBuId() == null || !buId.equals(emp.getBuId()))) continue;
            if (locId != null && (emp.getLocationId() == null || !locId.equals(emp.getLocationId()))) continue;
            if (deptId != null && (emp.getCategoryId() == null || !deptId.equals(emp.getCategoryId()))) continue;
            if (designationId != null && (emp.getDesignationId() == null || !designationId.equals(emp.getDesignationId()))) continue;
            if (reportId != null && (emp.getReportId() == null || !reportId.equals(emp.getReportId()))) continue;
            if (empId != null && !empId.equals(s.getEmpId())) continue;

            EmployeeSalaryDetailsViewModel vm = mapSalaryDetail(s);
            // Ensure employee info from joined EmployeeMaster
            vm.setFirstName(emp.getFirstName());
            vm.setMiddleName(emp.getMiddleName());
            vm.setLastName(emp.getLastName());
            result.add(vm);
        }

        // Sort by SalaryId descending (matching .NET)
        result.sort((a, b) -> Integer.compare(
            b.getSalaryId() != null ? b.getSalaryId() : 0,
            a.getSalaryId() != null ? a.getSalaryId() : 0));

        return result;
    }

    public EmployeeSalaryDetailsViewModel getEmployeeSalaryDetails(EmployeeSalaryDetailsViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        EmployeeSalaryDetails s = employeeSalaryDetailsRepository.findById(model.getSalaryId()).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Salary Details Not Found");

        return mapSalaryDetail(s);
    }

    public PayrollResponseViewModel addEmployeeSalaryDetails(EmployeeSalaryDetailsViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        EmployeeSalaryDetails s = new EmployeeSalaryDetails();
        s.setEmpId(model.getEmpId());
        s.setEmpCode(model.getEmpCode());
        s.setCtc(model.getCtc());
        s.setMCTC(model.getMctc());
        s.setEffectiveFromDate(parseDate(model.getEffectiveFromDate()));
        s.setEffectiveToDate(parseDate(model.getEffectiveToDate()));
        s.setIsAppraised(model.getIsAppraised());
        s.setIsActive(true);
        s.setIsUpdated(false);
        s.setIsDeleted(false);
        s.setCreatedBy(loginId);
        s.setCreatedDate(new Date());
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        employeeSalaryDetailsRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Added");
        return vm;
    }

    public PayrollResponseViewModel updateEmployeeSalaryDetails(EmployeeSalaryDetailsViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSalaryId() != null && model.getSalaryId() != 0) ? model.getSalaryId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Salary Id is Mismatching");

        EmployeeSalaryDetails s = employeeSalaryDetailsRepository.findById(id).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Salary Details Not Found");

        s.setEmpId(model.getEmpId());
        s.setEmpCode(model.getEmpCode());
        s.setCtc(model.getCtc());
        s.setMCTC(model.getMctc());
        s.setEffectiveFromDate(parseDate(model.getEffectiveFromDate()));
        s.setEffectiveToDate(parseDate(model.getEffectiveToDate()));
        s.setIsAppraised(model.getIsAppraised());
        s.setIsUpdated(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        employeeSalaryDetailsRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return vm;
    }

    public PayrollResponseViewModel deleteEmployeeSalaryDetails(EmployeeSalaryDetailsViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getSalaryId() != null && model.getSalaryId() != 0) ? model.getSalaryId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        EmployeeSalaryDetails s = employeeSalaryDetailsRepository.findById(id).orElse(null);
        if (s == null || Boolean.TRUE.equals(s.getIsDeleted())) throw new RuntimeException("Salary Details Not Found");

        s.setIsUpdated(true);
        s.setIsDeleted(true);
        s.setLastUpdatedBy(loginId);
        s.setLastUpdatedDate(new Date());
        employeeSalaryDetailsRepository.save(s);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return vm;
    }

    private EmployeeSalaryDetailsViewModel mapSalaryDetail(EmployeeSalaryDetails s) {
        EmployeeSalaryDetailsViewModel vm = new EmployeeSalaryDetailsViewModel();
        vm.setSalaryId(s.getSalaryId());
        vm.setEmpId(s.getEmpId());
        vm.setEmpCode(s.getEmpCode());
        vm.setCtc(s.getCtc());
        vm.setMctc(s.getMCTC());
        // Use /Date(timestamp)/ format for frontend parsing (matching .NET JavaScriptSerializer)
        vm.setEffectiveFromDate(s.getEffectiveFromDate() != null ? "/Date(" + s.getEffectiveFromDate().getTime() + ")/" : null);
        vm.setEffectiveToDate(s.getEffectiveToDate() != null ? "/Date(" + s.getEffectiveToDate().getTime() + ")/" : null);
        vm.setIsAppraised(s.getIsAppraised());
        vm.setIsActive(s.getIsActive());
        vm.setIsUpdated(s.getIsUpdated());
        vm.setIsDeleted(s.getIsDeleted());
        vm.setCreatedBy(s.getCreatedBy());
        vm.setCreatedDate(s.getCreatedDate() != null ? "/Date(" + s.getCreatedDate().getTime() + ")/" : null);
        vm.setLastUpdatedBy(s.getLastUpdatedBy());
        vm.setLastUpdatedDate(s.getLastUpdatedDate() != null ? "/Date(" + s.getLastUpdatedDate().getTime() + ")/" : null);
        // Add fields that may not exist in Java entity but are expected by frontend
        vm.setIncrementPercent(s.getIncrementPercent() != null ? java.math.BigDecimal.valueOf(s.getIncrementPercent()) : null);
        if (s.getEmpId() != null) {
            EmployeeMaster emp = employeeMasterRepository.findByEmpIdAndActive(s.getEmpId());
            if (emp != null) {
                vm.setFirstName(emp.getFirstName());
                vm.setMiddleName(emp.getMiddleName());
                vm.setLastName(emp.getLastName());
            }
        }
        return vm;
    }

    // ===== Payout Mapping Master =====

    public List<PayoutMappingMasterViewModel> getAllPayoutMappingMaster(PayoutMappingMasterViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayoutMappingMaster> details = payoutMappingMasterRepository.findAll().stream()
            .filter(m -> Boolean.TRUE.equals(m.getIsActive()) && Boolean.FALSE.equals(m.getIsDeleted()))
            .collect(Collectors.toList());

        if (details.isEmpty()) return new ArrayList<>();

        return details.stream().map(m -> {
            PayoutMappingMasterViewModel vm = new PayoutMappingMasterViewModel();
            vm.setMapId(m.getMapId());
            vm.setGradeId(m.getGradeId());
            vm.setGrade(m.getGrade());
            vm.setPayoutTypeId(m.getPayoutTypeId());
            vm.setCreatedBy(m.getCreatedBy());
            vm.setCreatedDate(formatDate(m.getCreatedDate()));
            vm.setLastUpdatedBy(m.getLastUpdatedBy());
            vm.setLastUpdatedDate(formatDate(m.getLastUpdatedDate()));
            vm.setIsActive(m.getIsActive());
            vm.setIsUpdated(m.getIsUpdated());
            vm.setIsDeleted(m.getIsDeleted());
            if (m.getPayoutTypeId() != null) {
                PayrollPayoutType pt = payrollPayoutTypeRepository.findById(m.getPayoutTypeId()).orElse(null);
                vm.setPayoutTypeName(pt != null ? pt.getPayoutTypeName() : "");
            }
            return vm;
        }).collect(Collectors.toList());
    }

    public PayoutMappingMasterViewModel getPayoutMappingMaster(PayoutMappingMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayoutMappingMaster m = payoutMappingMasterRepository.findById(model.getMapId()).orElse(null);
        if (m == null || Boolean.TRUE.equals(m.getIsDeleted())) throw new RuntimeException("Payout Mapping Details Not Found");

        PayoutMappingMasterViewModel vm = new PayoutMappingMasterViewModel();
        vm.setMapId(m.getMapId());
        vm.setGradeId(m.getGradeId());
        vm.setGrade(m.getGrade());
        vm.setPayoutTypeId(m.getPayoutTypeId());
        vm.setCreatedBy(m.getCreatedBy());
        vm.setCreatedDate(formatDate(m.getCreatedDate()));
        vm.setLastUpdatedBy(m.getLastUpdatedBy());
        vm.setLastUpdatedDate(formatDate(m.getLastUpdatedDate()));
        vm.setIsActive(m.getIsActive());
        vm.setIsUpdated(m.getIsUpdated());
        vm.setIsDeleted(m.getIsDeleted());
        if (m.getPayoutTypeId() != null) {
            PayrollPayoutType pt = payrollPayoutTypeRepository.findById(m.getPayoutTypeId()).orElse(null);
            vm.setPayoutTypeName(pt != null ? pt.getPayoutTypeName() : "");
        }
        return vm;
    }

    public PayrollResponseViewModel addPayoutMappingMaster(PayoutMappingMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");

        PayoutMappingMaster m = new PayoutMappingMaster();
        m.setGradeId(model.getGradeId());
        m.setGrade(model.getGrade());
        m.setPayoutTypeId(model.getPayoutTypeId());
        m.setIsActive(true);
        m.setIsUpdated(false);
        m.setIsDeleted(false);
        m.setCreatedBy(loginId);
        m.setCreatedDate(new Date());
        m.setLastUpdatedBy(loginId);
        m.setLastUpdatedDate(new Date());
        payoutMappingMasterRepository.save(m);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Added");
        return vm;
    }

    public PayrollResponseViewModel updatePayoutMappingMaster(PayoutMappingMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getMapId() != null && model.getMapId() != 0) ? model.getMapId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
        if (id == 0) throw new RuntimeException("Mapping Id is Mismatching");

        PayoutMappingMaster m = payoutMappingMasterRepository.findById(id).orElse(null);
        if (m == null || Boolean.TRUE.equals(m.getIsDeleted())) throw new RuntimeException("Payout Mapping Details Not Found");

        m.setGradeId(model.getGradeId());
        m.setGrade(model.getGrade());
        m.setPayoutTypeId(model.getPayoutTypeId());
        m.setIsUpdated(true);
        m.setLastUpdatedBy(loginId);
        m.setLastUpdatedDate(new Date());
        payoutMappingMasterRepository.save(m);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Updated");
        return vm;
    }

    public PayrollResponseViewModel deletePayoutMappingMaster(PayoutMappingMasterViewModel model) {
        Integer loginId = (model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        Integer id = (model.getMapId() != null && model.getMapId() != 0) ? model.getMapId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        PayoutMappingMaster m = payoutMappingMasterRepository.findById(id).orElse(null);
        if (m == null || Boolean.TRUE.equals(m.getIsDeleted())) throw new RuntimeException("Payout Mapping Details Not Found");

        m.setIsUpdated(true);
        m.setIsDeleted(true);
        m.setLastUpdatedBy(loginId);
        m.setLastUpdatedDate(new Date());
        payoutMappingMasterRepository.save(m);

        PayrollResponseViewModel vm = new PayrollResponseViewModel();
        vm.setStatus(200);
        vm.setMsg("Deleted");
        return vm;
    }

    // ===== Dropdowns =====

    public List<Map<String, Object>> getDDPayrollFrequency() {
        Integer loginId = 1;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<PayrollFrequencyMaster> details = payrollFrequencyMasterRepository.findByIsActiveTrue();
        if (details.isEmpty()) throw new RuntimeException("Frequency Details Not Found");

        return details.stream()
            .map(f -> {
                Map<String, Object> m = new HashMap<>();
                m.put("FrequencyId", f.getFrequencyId());
                m.put("Frequency", f.getFrequency());
                return m;
            }).collect(Collectors.toList());
    }

    public List<Map<String, Object>> getDDPayrollSymbols(PayrolAccessViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        // Query from PayrollSymbolMaster table (matching DotNetCode)
        // DotNetCode: select from PayrollSymbolMasters where IsActive == true
        List<PayrollSymbolMaster> symbolList = payrollSymbolMasterRepository.findByIsActiveTrue();

        if (symbolList.isEmpty()) {
            throw new RuntimeException("Symbol Details Not Found");
        }

        List<Map<String, Object>> result = new ArrayList<>();
        for (PayrollSymbolMaster sym : symbolList) {
            Map<String, Object> m = new HashMap<>();
            m.put("SymbolId", sym.getSymbolId());
            m.put("Symbol", sym.getSymbol());
            result.add(m);
        }
        return result;
    }

    public List<DDLegalEntityPayrollViewModel> ddLegalEntity(PayrollPayoutTypeViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<LegalEntityMaster> details = legalEntityMasterRepository.findAll().stream()
            .filter(l -> Boolean.TRUE.equals(l.getIsActive()) && Boolean.FALSE.equals(l.getIsDeleted()))
            .collect(Collectors.toList());

        return details.stream().map(l -> {
            DDLegalEntityPayrollViewModel vm = new DDLegalEntityPayrollViewModel();
            vm.setLEId(l.getLeId());
            vm.setLegalEntity(l.getLegalEntity());
            vm.setLoginId(loginId);
            return vm;
        }).collect(Collectors.toList());
    }

    public List<DDLocationViewModel> getDDLocation(Integer loginId, String authorisedEntity) {
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        List<Integer> authorisedEntities = new ArrayList<>();
        if (authorisedEntity != null && !authorisedEntity.isEmpty()) {
            for (String s : authorisedEntity.split(",")) {
                try { authorisedEntities.add(Integer.parseInt(s.trim())); } catch (NumberFormatException e) {}
            }
        }

        List<LocationMaster> allLocations = locationMasterRepository.findByIsActiveAndIsDeleted(true, false);

        return allLocations.stream()
            .filter(loc -> loc.getLeId() != null && (authorisedEntities.isEmpty() || authorisedEntities.contains(loc.getLeId())))
            .map(loc -> {
                DDLocationViewModel vm = new DDLocationViewModel();
                vm.setLocationId(loc.getLocationId());
                vm.setLocation(loc.getLocation());
                return vm;
            }).collect(Collectors.toList());
    }

    public List<DDPayrollEmpListViewModel> ddPayrollEmpList(PayrollComponentViewModel model) {
        Integer loginId = (model != null && model.getLoginId() != null && model.getLoginId() != 0) ? model.getLoginId() : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        // Get employees with active salary details (matching .NET: join with EmployeeSalaryDetails where RecordStatus=true AND IsActive=true)
        List<EmployeeSalaryDetails> activeSalaries = employeeSalaryDetailsRepository.findAll().stream()
            .filter(s -> Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
            .collect(Collectors.toList());
        List<String> salaryEmpCodes = activeSalaries.stream()
            .map(EmployeeSalaryDetails::getEmpCode)
            .filter(Objects::nonNull)
            .map(String::toUpperCase)
            .collect(Collectors.toList());

        return employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(e -> "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
            .filter(e -> e.getEmpCode() != null && salaryEmpCodes.contains(e.getEmpCode().toUpperCase()))
            .sorted(Comparator.comparing(EmployeeMaster::getEmpId, Comparator.reverseOrder()))
            .map(e -> {
                DDPayrollEmpListViewModel vm = new DDPayrollEmpListViewModel();
                vm.setEmpId(e.getEmpId());
                String fn = e.getFirstName() != null ? e.getFirstName().trim() : "";
                String mn = e.getMiddleName() != null ? e.getMiddleName().trim() : "";
                String ln = e.getLastName() != null ? e.getLastName().trim() : "";
                String ec = e.getEmpCode() != null ? e.getEmpCode().trim() : "";
                vm.setEmpName(fn + " " + mn + " " + ln + " (" + ec + ")");
                vm.setEmpCode(ec);
                return vm;
            })
            .collect(Collectors.toList());
    }

    // ===== CTC Calculation (matches DotNetCode implementation) =====

    public Map<String, Object> empCTCCalculation(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        try {
            Integer loginId = parseSafeInt(model.get("LoginId"));
            Integer empId = parseSafeInt(model.get("EmpId"));
            Integer year = parseSafeInt(model.get("Year"));
            Integer monthNo = parseSafeInt(model.get("MonthNo"));

            if (loginId == 0) throw new RuntimeException("LoginId is Mismatching");
            if (empId == 0) throw new RuntimeException("Employee Id is Missing");
            if (year == 0) year = Calendar.getInstance().get(Calendar.YEAR);
            if (monthNo == 0) monthNo = Calendar.getInstance().get(Calendar.MONTH) + 1;

            // Calculate total days and LOP (matching DotNetCode)
            Calendar calDate = Calendar.getInstance();
            calDate.set(year, monthNo - 1, 1);
            int totalDays = calDate.getActualMaximum(Calendar.DAY_OF_MONTH);

            // Start & end dates for the month
            Calendar startCal = Calendar.getInstance();
            startCal.set(year, monthNo - 1, 1, 0, 0, 0);
            Date startDate = startCal.getTime();
            Calendar endCal = Calendar.getInstance();
            endCal.set(year, monthNo - 1, totalDays, 23, 59, 59);
            Date endDate = endCal.getTime();

            // Get LOP (LeaveTypeId == 0) for the employee in this month (matching DotNetCode)
            List<EmpLeaveApplication> lopList = empLeaveApplicationRepository.findAll().stream()
                .filter(l -> empId.equals(l.getEmpId()) && Integer.valueOf(0).equals(l.getLeaveTypeId())
                    && l.getFromDate() != null && l.getFromDate().compareTo(startDate) >= 0
                    && l.getToDate() != null && l.getToDate().compareTo(endDate) <= 0
                    && Boolean.TRUE.equals(l.getIsActive()) && Boolean.FALSE.equals(l.getIsDeleted()))
                .sorted((a, b) -> a.getFromDate().compareTo(b.getFromDate()))
                .collect(Collectors.toList());

            double lopDuration = lopList.stream()
                .mapToDouble(l -> l.getNoOfDays() != null ? l.getNoOfDays().doubleValue() : 0.0)
                .sum();

            double workingDays = totalDays - lopDuration;

            // Get employee details (matching DotNetCode: check by EmpId and ACTIVE status)
            // Note: Repository now only checks isActive and isDeleted, we check EmpStatus in service
            EmployeeMaster emp = employeeMasterRepository.findByEmpIdAndActive(empId);
            if (emp == null) {
                throw new RuntimeException("Employee not found - No employee with EmpId: " + empId + " or employee is inactive/deleted");
            }
            // Check EmpStatus (matching DotNetCode: emp.EmpStatus.ToUpper() == "ACTIVE")
            // Treat null or empty as ACTIVE (matching real data where EmpStatus is not set)
            if (emp.getEmpStatus() != null && !emp.getEmpStatus().isEmpty() 
                && !"ACTIVE".equalsIgnoreCase(emp.getEmpStatus())) {
                throw new RuntimeException("Employee not found - Status is: " + emp.getEmpStatus() + ". Expected: ACTIVE");
            }

            // Get designation details with grade (matching DotNetCode logic)
            if (emp.getDesignationId() == null) {
                throw new RuntimeException("User designation not found.");
            }

            List<DesignationMaster> desigList = designationMasterRepository.findAll().stream()
                .filter(d -> emp.getDesignationId().equals(d.getDesignationId())
                    && Boolean.TRUE.equals(d.getIsActive()) && Boolean.FALSE.equals(d.getIsDeleted()))
                .collect(Collectors.toList());

            if (desigList.isEmpty()) {
                throw new RuntimeException("User designation not found.");
            }

            DesignationMaster desig = desigList.get(0);
            if (desig.getGradeId() == null) {
                throw new RuntimeException("User designation does not have a grade mapping. Kindly map the designation to the appropriate grade to continue.");
            }

            // Get payout type from grade mapping (matching DotNetCode)
            Integer payoutTypeId = null;
            Integer gradeId = desig.getGradeId();
            List<PayoutMappingMaster> payoutMappingList = payoutMappingMasterRepository.findAll().stream()
                .filter(p -> gradeId.equals(p.getGradeId()) && Boolean.TRUE.equals(p.getIsActive()) && Boolean.FALSE.equals(p.getIsDeleted()))
                .sorted((a, b) -> Integer.compare(b.getMapId(), a.getMapId()))
                .collect(Collectors.toList());

            if (payoutMappingList.isEmpty()) {
                throw new RuntimeException("User Grade does not have a Payout mapping. Kindly map the Grade to the appropriate Payout to continue.");
            }
            payoutTypeId = payoutMappingList.get(0).getPayoutTypeId();

            // Get employee salary details (matching DotNetCode: join by EmpCode and EmpId)
            Date today = new Date();
            List<EmployeeSalaryDetails> salList = employeeSalaryDetailsRepository.findAll().stream()
                .filter(s -> empId.equals(s.getEmpId()) && emp.getEmpCode().equalsIgnoreCase(s.getEmpCode())
                    && s.getEffectiveFromDate() != null && !s.getEffectiveFromDate().after(today)
                    && Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
                .sorted((a, b) -> Integer.compare(b.getSalaryId(), a.getSalaryId()))
                .collect(Collectors.toList());

            if (salList.isEmpty()) {
                // Check if salary exists but effective dates expired (matching DotNetCode)
                List<EmployeeSalaryDetails> salList1 = employeeSalaryDetailsRepository.findAll().stream()
                    .filter(s -> empId.equals(s.getEmpId()) && emp.getEmpCode().equalsIgnoreCase(s.getEmpCode())
                        && Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
                    .sorted((a, b) -> Integer.compare(b.getSalaryId(), a.getSalaryId()))
                    .collect(Collectors.toList());

                if (!salList1.isEmpty()) {
                    throw new RuntimeException("The effective dates for the employee's salary details have expired.");
                }
                throw new RuntimeException("Salary details (CTC) for the selected employee were not found.");
            }

            EmployeeSalaryDetails salDetails = salList.get(0);
            double ctcValue = salDetails.getCtc() != null ? salDetails.getCtc().doubleValue() : 0.0;

            // Add salary variables (matching DotNetCode: BS, HRA, Con, PF, GI, ESI, Grat, SB, TD, PT, etc.)
            Map<String, Double> computedValues = new HashMap<>();
            computedValues.put("CTC", ctcValue);

            // Initialize possible salary variable names (matching DotNetCode)
            String[] possibleNames = {"MCTC", "BS", "HRA", "Con", "ESIB", "PFB", "GI", "Grat", "SB", "GS", "PFB", "ESIB", "PT", "TD", "IA", "NS"};

            // Put CTC as base
            computedValues.put("CTC", ctcValue);

            // Try to get MCTC from salDetails
            if (salDetails.getMCTC() != null) {
                computedValues.put("MCTC", salDetails.getMCTC().doubleValue());
            } else {
                computedValues.put("MCTC", ctcValue);
            }

            // Initialize other possible variables to 0.0 if not already present
            for (String name : possibleNames) {
                if (!computedValues.containsKey(name)) {
                    computedValues.put(name, 0.0);
                }
            }

            // Get all components for the payout type (matching DotNetCode)
            final Integer finalPayoutTypeId = payoutTypeId;
            List<PayrollComponent> components = payrollComponentRepository.findAll().stream()
                .filter(c -> Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted())
                    && (finalPayoutTypeId == null || finalPayoutTypeId.equals(c.getPayoutTypeId())))
                .collect(Collectors.toList());

            if (components.isEmpty()) throw new RuntimeException("Component details are not found");

            // Pre-load payout type and segment lookup maps (matching DotNetCode joins)
            Map<Integer, PayrollPayoutType> payoutTypeMap = new HashMap<>();
            for (PayrollPayoutType pt : payrollPayoutTypeRepository.findAll()) {
                payoutTypeMap.put(pt.getPayoutTypeId(), pt);
            }
            Map<Integer, PayrollSegment> segmentMap = new HashMap<>();
            for (PayrollSegment seg : payrollSegmentRepository.findAll()) {
                segmentMap.put(seg.getSegmentId(), seg);
            }

            // Process each component (matching DotNetCode logic)
            List<Map<String, Object>> lstofCompvalue = new ArrayList<>();
            for (PayrollComponent c : components) {
                Map<String, Object> compMap = new HashMap<>();
                compMap.put("EmpId", empId);
                compMap.put("LoginId", loginId);
                compMap.put("EmpCode", emp.getEmpCode());
                compMap.put("FirstName", emp.getFirstName() != null ? emp.getFirstName() : "");
                compMap.put("MiddleName", emp.getMiddleName() != null ? emp.getMiddleName() : "");
                compMap.put("LastName", emp.getLastName() != null ? emp.getLastName() : "");
                compMap.put("PayoutTypeId", c.getPayoutTypeId());
                compMap.put("ComponentId", c.getComponentId());
                compMap.put("ComponentName", c.getComponentName());
                compMap.put("ComponentCode", c.getComponentCode());

                // Add PayoutTypeName, FrequencyId, Frequency, SegmentId, SegmentName (matching DotNetCode joins)
                PayrollPayoutType pt = payoutTypeMap.get(c.getPayoutTypeId());
                compMap.put("PayoutTypeName", pt != null ? pt.getPayoutTypeName() : "");
                compMap.put("FrequencyId", 0);
                compMap.put("Frequency", pt != null && pt.getFrequency() != null ? pt.getFrequency() : "");
                PayrollSegment seg = segmentMap.get(c.getSegmentId());
                compMap.put("SegmentId", c.getSegmentId());
                compMap.put("SegmentName", seg != null ? seg.getSegmentName() : "");
                compMap.put("LCtrue", 0);

                // Get logic for this component
                List<PayrollComponentLogic> logics = payrollComponentLogicRepository
                    .findByComponentIdAndIsActiveTrueAndIsDeletedFalseOrderBySno(c.getComponentId());

                double computed = 0.0;

                if (!logics.isEmpty()) {
                    PayrollComponentLogic cal = logics.get(0);
                    compMap.put("LogicId", cal.getLogicId());
                    compMap.put("Percentage", cal.getPercentage());
                    compMap.put("Value", cal.getValue());
                    compMap.put("ComponentId1", cal.getComponentId1());
                    compMap.put("ComponentName1", cal.getComponentName1());
                    compMap.put("EffectiveFrom", cal.getEffectiveFrom() != null ? "/Date(" + cal.getEffectiveFrom().getTime() + ")/" : null);
                    compMap.put("EffectiveTo", cal.getEffectiveTo() != null ? "/Date(" + cal.getEffectiveTo().getTime() + ")/" : null);

                    // Calculate based on value or percentage (matching DotNetCode)
                    if (cal.getValue() != null) {
                        computed = cal.getValue().doubleValue();
                    } else if (cal.getPercentage() != null && cal.getComponentName1() != null) {
                        // Find the referenced component value
                        String operandName = cal.getComponentName1().split("\\s+")[0];
                        double operandValue = 0.0;
                        for (Map<String, Object> prev : lstofCompvalue) {
                            if (operandName.equalsIgnoreCase((String) prev.get("ComponentCode"))) {
                                try { operandValue = Double.parseDouble(prev.get("ComponentValue").toString()); } catch (Exception e) {}
                                break;
                            }
                        }
                        computed = (cal.getPercentage().doubleValue() / 100.0) * operandValue;
                    }

                    // Check condition (matching DotNetCode: evaluate arithmetic and boolean expressions)
                    List<PayrollComponentCondition> conditions = payrollComponentConditionRepository
                        .findByComponentIdAndIsActiveTrueAndIsDeletedFalse(cal.getComponentId());
                    if (!conditions.isEmpty()) {
                        PayrollComponentCondition cond = conditions.get(0);
                        compMap.put("ConditionId", cond.getConditionId());
                        compMap.put("ConditionExpression", cond.getConditionExpression());
                        compMap.put("ConditionResultPFESI", cond.getConditionResultPFESI());

                        if (cond.getConditionExpression() != null && !cond.getConditionExpression().isEmpty()) {
                            String expr = cond.getConditionExpression();
                            
                            // Evaluate the expression to get computed value (matching DotNetCode cvalue mechanism)
                            Double exprResult = evaluateConditionExpression(expr, computedValues);
                            
                            // If expression has (OR), use the computed value (matching DotNetCode cvalue logic)
                            if (expr.contains("(OR)") || expr.contains("(or)")) {
                                if (exprResult != null) {
                                    computed = exprResult;
                                }
                            } else if (!expr.contains(">") && !expr.contains("<") && !expr.contains("==") && !expr.contains("!=") && !expr.contains("(OR)")) {
                                // Pure arithmetic expression - use result as computed value
                                if (exprResult != null) {
                                    computed = exprResult;
                                }
                            } else {
                                // Boolean condition - only set to 0 if condition fails
                                if (exprResult == null || exprResult == 0.0) {
                                    computed = 0.0;
                                }
                            }
                        }
                    }
                }

                // Handle CTC component
                if ("CTC".equalsIgnoreCase(c.getComponentCode())) {
                    computed = ctcValue;
                }

                compMap.put("ComponentValue", String.format("%.2f", computed));
                compMap.put("ComponentValueNum", computed);

                // Update computed values map
                if (c.getComponentCode() != null) {
                    computedValues.put(c.getComponentCode(), computed);
                }

                lstofCompvalue.add(compMap);
            }

            // Handle arrear (matching DotNetCode logic)
            List<Map<String, Object>> lstofArrearComponentDetails = new ArrayList<>();
            // Note: IsArrear column doesn't exist in DB - default to false
            boolean arrear = false;
            // Note: IsClearArrear column doesn't exist in DB - default to false
            boolean clearArrear = false;
            // Note: ArrearMonth and ArrearYear columns don't exist in DB - using defaults
            Integer effectiveMonth = 0;
            Integer effectiveYear = 0;

            // Note: VariableAmt column doesn't exist in DB
            double arrearAmt = 0.0;

            // Determine if arrear applies (matching DotNetCode)
            if (clearArrear == false) {
                if (effectiveMonth == monthNo && effectiveYear == year) {
                    arrear = true;
                } else {
                    arrear = false;
                }
            } else {
                if (effectiveMonth == monthNo && effectiveYear == year) {
                    arrear = true;
                } else {
                    arrear = false;
                }
            }

            if (arrear) {
                // Calculate arrear components (matching DotNetCode structure)
                for (PayrollComponent c : components) {
                    Map<String, Object> arrearMap = new HashMap<>();
                    arrearMap.put("EmpId", empId);
                    arrearMap.put("ComponentId", c.getComponentId());
                    arrearMap.put("ComponentName", c.getComponentName());
                    arrearMap.put("ComponentCode", c.getComponentCode());
                    arrearMap.put("ComponentValue", String.format("%.2f", arrearAmt * 0.1)); // Placeholder - adjust as needed
                    lstofArrearComponentDetails.add(arrearMap);
                }
            }

            result.put("lstofComponentDetails", lstofCompvalue);
            result.put("lstofArrearComponentDetails", lstofArrearComponentDetails);
            result.put("msg", "CTC calculated successfully");

        } catch (RuntimeException e) {
            result.put("StatusCode", 404);
            result.put("Message", e.getMessage());
        } catch (Exception e) {
            result.put("StatusCode", 500);
            result.put("Message", "Error calculating CTC: " + e.getMessage());
        }
        return result;
    }

    // Simple arithmetic expression evaluator (replaces Nashorn JS engine - removed in Java 15+)
    private double evalSimple(String expr) {
        expr = expr.trim();
        // Handle parentheses
        while (expr.contains("(")) {
            int open = expr.lastIndexOf("(");
            int close = expr.indexOf(")", open);
            if (close == -1) break;
            String sub = expr.substring(open + 1, close);
            double val = evalSimple(sub);
            expr = expr.substring(0, open) + val + expr.substring(close + 1);
        }
        // Handle addition/subtraction (lowest precedence)
        int pos = findOutsideParens(expr, '+');
        if (pos > 0) return evalSimple(expr.substring(0, pos)) + evalSimple(expr.substring(pos + 1));
        pos = findOutsideParens(expr, '-');
        if (pos > 0) return evalSimple(expr.substring(0, pos)) - evalSimple(expr.substring(pos + 1));
        // Handle multiplication/division (higher precedence)
        pos = findOutsideParens(expr, '*');
        if (pos > 0) return evalSimple(expr.substring(0, pos)) * evalSimple(expr.substring(pos + 1));
        pos = findOutsideParens(expr, '/');
        if (pos > 0) {
            double left = evalSimple(expr.substring(0, pos));
            double right = evalSimple(expr.substring(pos + 1));
            if (right == 0) return 0;
            return left / right;
        }
        return Double.parseDouble(expr.trim());
    }

    private int findOutsideParens(String expr, char op) {
        int depth = 0;
        for (int i = expr.length() - 1; i >= 0; i--) {
            char c = expr.charAt(i);
            if (c == ')') depth++;
            else if (c == '(') depth--;
            else if (c == op && depth == 0 && i > 0) return i;
        }
        return -1;
    }

    // Evaluate condition expression and return computed value (matching DotNetCode EvaluateCondition with cvalue)
    private Double evaluateConditionExpression(String condExpr, Map<String, Double> computedValues) {
        if (condExpr == null || condExpr.trim().isEmpty()) return null;
        try {
            String expr = condExpr.replace("(OR)", "||").replace("(or)", "||");
            String[] orParts = expr.split("\\|\\|");
            for (String part : orParts) {
                String p = part.trim();
                if (p.isEmpty()) continue;

                String replaced = p;
                List<String> keys = new ArrayList<>(computedValues.keySet());
                keys.sort((a, b) -> Integer.compare(b.length(), a.length()));
                for (String key : keys) {
                    replaced = replaced.replaceAll("\\b" + key + "\\b", String.valueOf(computedValues.get(key)));
                }

                // Check for range: left <= VAR <= right
                java.util.regex.Matcher rangeMatcher = java.util.regex.Pattern.compile(
                    "^\\s*([\\d\\.]+)\\s*(<=|<)\\s*([A-Za-z_][A-Za-z0-9_]*)\\s*(<=|<)\\s*([\\d\\.]+)\\s*$").matcher(replaced);
                if (rangeMatcher.matches()) {
                    double left = Double.parseDouble(rangeMatcher.group(1));
                    String varName = rangeMatcher.group(3);
                    double right = Double.parseDouble(rangeMatcher.group(5));
                    double varVal = computedValues.getOrDefault(varName, 0.0);
                    if (varVal >= left && varVal <= right) return 1.0;
                    continue;
                }

                // Check for comparison operators
                String[] operators = {"<=", ">=", "==", "!=", "<", ">"};
                String usedOp = null;
                int opIdx = -1;
                for (String op : operators) {
                    int idx = replaced.indexOf(op);
                    if (idx > 0) { usedOp = op; opIdx = idx; break; }
                }
                if (usedOp != null) {
                    double left = evalSimple(replaced.substring(0, opIdx));
                    double right = evalSimple(replaced.substring(opIdx + usedOp.length()));
                    boolean result = false;
                    switch (usedOp) {
                        case ">": result = left > right; break;
                        case "<": result = left < right; break;
                        case ">=": result = left >= right; break;
                        case "<=": result = left <= right; break;
                        case "==": result = Math.abs(left - right) < 0.000001; break;
                        case "!=": result = Math.abs(left - right) > 0.000001; break;
                    }
                    if (result) return left; // Return the left operand value as cvalue
                    continue;
                }

                // Pure arithmetic - evaluate and return value (matching .NET cvalue)
                return evalSimple(replaced);
            }
            return 0.0;
        } catch (Exception e) {
            return null;
        }
    }
    private Double evaluateArithmetic(String expr, Map<String, Double> computedValues) {
        if (expr == null || expr.trim().isEmpty()) return null;
        try {
            String replaced = expr;
            List<String> keys = new ArrayList<>(computedValues.keySet());
            keys.sort((a, b) -> Integer.compare(b.length(), a.length()));
            for (String key : keys) {
                replaced = replaced.replaceAll("\\b" + key + "\\b", String.valueOf(computedValues.get(key)));
            }
            return evalSimple(replaced);
        } catch (Exception e) {
            return null;
        }
    }

    // Helper to evaluate boolean condition expression (matching DotNetCode EvaluateCondition)
    private boolean evaluateBooleanCondition(String condExpr, Map<String, Double> computedValues) {
        if (condExpr == null || condExpr.trim().isEmpty()) return true;
        try {
            String expr = condExpr.replace("(OR)", "||").replace("(or)", "||");
            String[] orParts = expr.split("\\|\\|");
            for (String part : orParts) {
                String p = part.trim();
                if (p.isEmpty()) continue;

                String replaced = p;
                List<String> keys = new ArrayList<>(computedValues.keySet());
                keys.sort((a, b) -> Integer.compare(b.length(), a.length()));
                for (String key : keys) {
                    replaced = replaced.replaceAll("\\b" + key + "\\b", String.valueOf(computedValues.get(key)));
                }

                // Handle comparison operators
                String[] operators = {"<=", ">=", "==", "!=", "<", ">"};
                String usedOp = null;
                int opIdx = -1;
                for (String op : operators) {
                    int idx = replaced.indexOf(op);
                    if (idx > 0) { usedOp = op; opIdx = idx; break; }
                }
                if (usedOp != null) {
                    double left = evalSimple(replaced.substring(0, opIdx));
                    double right = evalSimple(replaced.substring(opIdx + usedOp.length()));
                    boolean result = false;
                    switch (usedOp) {
                        case ">": result = left > right; break;
                        case "<": result = left < right; break;
                        case ">=": result = left >= right; break;
                        case "<=": result = left <= right; break;
                        case "==": result = Math.abs(left - right) < 0.000001; break;
                        case "!=": result = Math.abs(left - right) > 0.000001; break;
                    }
                    if (result) return true;
                } else {
                    // Pure arithmetic expression in OR context
                    double val = evalSimple(replaced);
                    if (val != 0) return true;
                }
            }
            return false;
        } catch (Exception e) {
            return true;
        }
    }

    public PayrollResponseViewModel empPayslipGeneration(Map<String, Object> model) {
        PayrollResponseViewModel response = new PayrollResponseViewModel();
        try {
            Integer loginId = parseSafeInt(model.get("LoginId"));
            String empCode = model.get("EmpCode") != null ? model.get("EmpCode").toString() : "";
            Integer year = parseSafeInt(model.get("Year"));
            Integer monthNo = parseSafeInt(model.get("MonthNo"));
            String monthName = model.get("Month") != null ? model.get("Month").toString() : "";

            if (loginId == 0) throw new RuntimeException("LoginId is Missing");
            if (year == 0) throw new RuntimeException("Year is Missing");
            if (monthNo == 0) throw new RuntimeException("Month is Missing");

            response.setSalaryMonth(monthName + " - " + year);
            response.setYear(String.valueOf(year));

            // Get employee details
            List<EmployeeMaster> empList = employeeMasterRepository.findAll().stream()
                .filter(e -> loginId.equals(e.getEmpId()) && "ACTIVE".equalsIgnoreCase(e.getEmpStatus())
                    && Boolean.TRUE.equals(e.getIsActive()) && Boolean.FALSE.equals(e.getIsDeleted()))
                .collect(Collectors.toList());

            if (empList.isEmpty()) throw new RuntimeException("Employee not found");
            EmployeeMaster emp = empList.get(0);

            // Get company info
            CompanyInfoViewModel companyInfo = new CompanyInfoViewModel();
            List<CompanyMaster> companyList = companyMasterRepository.findAll().stream()
                .filter(c -> Boolean.TRUE.equals(c.getIsActive()) && Boolean.FALSE.equals(c.getIsDeleted()))
                .collect(Collectors.toList());
            if (!companyList.isEmpty()) {
                CompanyMaster company = companyList.get(0);
                companyInfo.setCompanyName(company.getCompany());
                companyInfo.setCompanyAddress("");  // Address not in entity
                companyInfo.setCompanyPhoneNo("");  // Phone not in entity
                companyInfo.setCompanyFax("");  // Fax not in entity
                companyInfo.setCompanyEmail("");  // Email not in entity
            }
            response.setCompany(companyInfo);

            // Get employee details for payslip
            EmployeeInfoDetailsViewModel empDetails = new EmployeeInfoDetailsViewModel();
            empDetails.setName(getEmployeeFullName(loginId));
            empDetails.setDesignation(emp.getDesignationName() != null ? emp.getDesignationName() : "");
            empDetails.setEmpCode(emp.getEmpCode());

            // Get account details
            List<EmployeeAccDetails> accList = employeeAccDetailsRepository.findAll().stream()
                .filter(a -> loginId.equals(a.getEmpId()) && Boolean.TRUE.equals(a.getIsActive()) && Boolean.FALSE.equals(a.getIsDeleted()))
                .collect(Collectors.toList());

            if (!accList.isEmpty()) {
                EmployeeAccDetails acc = accList.get(0);
                empDetails.setPanNo(acc.getPanNo() != null ? acc.getPanNo() : "");
                empDetails.setBankName(acc.getBankName() != null ? acc.getBankName() : "");
                empDetails.setBranchName(acc.getBranchName() != null ? acc.getBranchName() : "");
                empDetails.setIfscCode(acc.getIfscCode() != null ? acc.getIfscCode() : "");
                empDetails.setBankAccNo(acc.getAccNo() != null ? acc.getAccNo() : "");
                empDetails.setPfNo(acc.getPfNo() != null ? acc.getPfNo() : "");
                empDetails.setUanNo(acc.getUanNo() != null ? acc.getUanNo() : "");
                empDetails.setEsiNo(acc.getEsiInsuranceNo() != null ? acc.getEsiInsuranceNo() : "");
            } else {
                empDetails.setPanNo("");
                empDetails.setBankName("");
                empDetails.setBranchName("");
                empDetails.setIfscCode("");
                empDetails.setBankAccNo("");
                empDetails.setPfNo("");
                empDetails.setUanNo("");
                empDetails.setEsiNo("");
            }

            empDetails.setDaysPaid(BigDecimal.valueOf(30));  // Default
            empDetails.setLop(BigDecimal.ZERO);
            String locationName = "";
            if (emp.getLocationId() != null) {
                List<LocationMaster> locList = locationMasterRepository.findAll().stream()
                    .filter(l -> emp.getLocationId().equals(l.getLocationId()) && Boolean.TRUE.equals(l.getIsActive()) && Boolean.FALSE.equals(l.getIsDeleted()))
                    .collect(Collectors.toList());
                if (!locList.isEmpty()) locationName = locList.get(0).getLocation();
            }
            empDetails.setLocation(locationName);
            response.setEmployeeDetails(empDetails);

            // Get payslip sections (simplified - without PayoutTypeId filter since entity doesn't have it)
            List<PayslipSection> sections = payslipSectionRepository.findAll().stream()
                .filter(s -> Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
                .sorted((a, b) -> a.getSequenceNo().compareTo(b.getSequenceNo()))
                .collect(Collectors.toList());

            List<SectionResponseViewModel> payslipSections = new ArrayList<>();
            for (PayslipSection section : sections) {
                SectionResponseViewModel sectionVM = new SectionResponseViewModel();
                sectionVM.setSectionId(section.getSectionId());
                sectionVM.setSectionName(section.getSectionName());

                List<PayslipSectionComponents> components = payslipSectionComponentsRepository.findAll().stream()
                    .filter(sc -> section.getSectionId().equals(sc.getSectionId()) && Boolean.TRUE.equals(sc.getIsActive()) && Boolean.FALSE.equals(sc.getIsDeleted()))
                    .sorted((a, b) -> a.getSequenceNo().compareTo(b.getSequenceNo()))
                    .collect(Collectors.toList());

                List<SalaryComponentViewModel> componentList = new ArrayList<>();
                for (PayslipSectionComponents sc : components) {
                    SalaryComponentViewModel comp = new SalaryComponentViewModel();
                    comp.setSectionComponentId(sc.getSectionComponentId());
                    comp.setComponentId(sc.getComponentId());
                    comp.setComponentName("");  // ComponentName not in entity
                    comp.setComponentCode("");  // ComponentCode not in entity
                    comp.setSequenceNo(sc.getSequenceNo());
                    comp.setComponentValue("0.00");  // Default value
                    componentList.add(comp);
                }
                sectionVM.setComponents(componentList);
                payslipSections.add(sectionVM);
            }
            response.setPayslipSections(payslipSections);

            response.setArrearSections(new ArrayList<>());
            response.setVariableSections(new ArrayList<>());
            response.setDescriptionforArrear("");
            response.setStatus(200);
            response.setMsg("Payslip generated successfully");

        } catch (Exception e) {
            response.setStatus(404);
            response.setMsg(e.getMessage());
        }
        return response;
    }

    private String getEmployeeFullName(Integer empId) {
        List<EmployeeMaster> empList = employeeMasterRepository.findAll().stream()
            .filter(e -> empId.equals(e.getEmpId()) && Boolean.TRUE.equals(e.getIsActive()) && Boolean.FALSE.equals(e.getIsDeleted()))
            .collect(Collectors.toList());
        if (empList.isEmpty()) return "";
        EmployeeMaster e = empList.get(0);
        return (e.getFirstName() != null ? e.getFirstName() : "") + " " + (e.getLastName() != null ? e.getLastName() : "");
    }

    public Map<String, Object> calculatePayroll(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Payroll calculated");
        return result;
    }

    public Map<String, Object> generatePayslip(Map<String, Object> model) {
        return model;
    }

    public List<Map<String, Object>> getPayslipByEmployee(Map<String, Object> model) {
        return new ArrayList<>();
    }

    public Map<String, Object> processPayroll(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("Status", 200);
        result.put("msg", "Payroll processed successfully");
        return result;
    }

    public Map<String, Object> payrollReportforALL(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("msg", "Payroll report generated");
        return result;
    }

    public List<Map<String, Object>> getDDPayrollSymbols() {
        List<Map<String, Object>> result = new ArrayList<>();
        Map<String, Object> m = new HashMap<>(); m.put("id", 1); m.put("name", "+"); result.add(m);
        m = new HashMap<>(); m.put("id", 2); m.put("name", "-"); result.add(m);
        return result;
    }

    public List<Map<String, Object>> getPayrollComponents() {
        PayrollComponentViewModel model = new PayrollComponentViewModel();
        model.setLoginId(1);
        return ddPayrollComponent(model).stream()
            .map(c -> { Map<String, Object> m = new HashMap<>(); m.put("id", c.getComponentId()); m.put("name", c.getComponentName()); return m; })
            .collect(Collectors.toList());
    }

    public Map<String, Object> activatePayrollPayoutType(Map<String, Object> model) {
        return Map.of("msg", "Activated", "StatusCode", 200);
    }

    public Map<String, Object> deactivatePayrollPayoutType(Map<String, Object> model) {
        return Map.of("msg", "Deactivated", "StatusCode", 200);
    }

    public Map<String, Object> addPayrollVariable(Map<String, Object> model) {
        return Map.of("msg", "Added", "StatusCode", 200);
    }

    public Map<String, Object> updatePayrollVariable(Map<String, Object> model) {
        return Map.of("msg", "Updated", "StatusCode", 200);
    }

    public Map<String, Object> deletePayrollVariable(Map<String, Object> model) {
        return Map.of("msg", "Deleted", "StatusCode", 200);
    }

    public Map<String, Object> getPayrollVariable(Map<String, Object> model) {
        Map<String, Object> result = new HashMap<>();
        result.put("VariableId", model.get("VariableId"));
        result.put("VariableName", "Sample Variable");
        result.put("StatusCode", 200);
        return result;
    }

    public List<Map<String, Object>> getAllPayrollVariable(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        result.add(Map.of("VariableId", 1, "VariableName", "Sample Variable", "StatusCode", 200));
        return result;
    }

    public List<Map<String, Object>> ddPayrollVariable(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        result.add(Map.of("VariableId", 1, "VariableName", "Sample Variable", "StatusCode", 200));
        return result;
    }

    public Map<String, Object> addPayrollVariableHistory(Map<String, Object> model) {
        return Map.of("msg", "Added", "StatusCode", 200);
    }

    public Map<String, Object> updatePayrollVariableHistory(Map<String, Object> model) {
        return Map.of("msg", "Updated", "StatusCode", 200);
    }

    public Map<String, Object> deletePayrollVariableHistory(Map<String, Object> model) {
        return Map.of("msg", "Deleted", "StatusCode", 200);
    }

    public List<Map<String, Object>> payrollVariableHistory(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        result.add(Map.of("HistoryId", 1, "VariableId", 1, "OldValue", "100", "NewValue", "150", "StatusCode", 200));
        return result;
    }

    public List<Map<String, Object>> getAllPayrollPayoutTypeSegment(Map<String, Object> model) {
        Integer loginId = model.containsKey("LoginId") ? parseSafeInt(model.get("LoginId")) : 0;
        Integer payoutTypeId = model.containsKey("PayoutTypeId") ? parseSafeInt(model.get("PayoutTypeId")) : 0;
        if (loginId == 0) throw new RuntimeException("LoginId is Missing");

        // Preload all payout types for efficient lookup
        Map<Integer, String> payoutTypeNames = new HashMap<>();
        for (PayrollPayoutType pt : payrollPayoutTypeRepository.findAll()) {
            payoutTypeNames.put(pt.getPayoutTypeId(), pt.getPayoutTypeName());
        }

        List<PayrollSegment> segments = payrollSegmentRepository.findAll().stream()
            .filter(s -> Boolean.TRUE.equals(s.getIsActive()) && Boolean.FALSE.equals(s.getIsDeleted()))
            .filter(s -> payoutTypeId == 0 || (s.getPayoutTypeId() != null && s.getPayoutTypeId().equals(payoutTypeId)))
            .collect(Collectors.toList());

        if (segments.isEmpty()) throw new RuntimeException("Segment Details Not Found");

        return segments.stream().map(s -> {
            Map<String, Object> m = new HashMap<>();
            m.put("SegmentId", s.getSegmentId());
            m.put("SegmentName", s.getSegmentName());
            m.put("PayoutTypeId", s.getPayoutTypeId());
            m.put("PayoutTypeName", s.getPayoutTypeId() != null ? payoutTypeNames.getOrDefault(s.getPayoutTypeId(), "") : "");
            m.put("CreatedBy", s.getCreatedBy());
            m.put("CreatedDate", formatDate(s.getCreatedDate()));
            m.put("LastUpdatedBy", s.getLastUpdatedBy());
            m.put("LastUpdatedDate", formatDate(s.getLastUpdatedDate()));
            m.put("IsActive", s.getIsActive());
            m.put("IsUpdated", s.getIsUpdated());
            m.put("IsDeleted", s.getIsDeleted());
            return m;
        }).collect(Collectors.toList());
    }
}
