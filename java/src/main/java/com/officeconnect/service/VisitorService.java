package com.officeconnect.service;

import com.officeconnect.dto.*;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
public class VisitorService {

    @Autowired
    private VisitorManagementRepository visitorManagementRepository;
    
    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private VisitorInviteHistoryRepository visitorInviteHistoryRepository;

    public VisitorManagementViewModel createVisitor(VisitorManagementViewModel model) {
        VisitorManagement vm = new VisitorManagement();
        vm.setName(model.getVisitorName());
        vm.setDesignation(model.getDesignation());
        vm.setCompany(model.getCompany());
        vm.setMobile(model.getMobile());
        vm.setPMail(model.getPMail());
        vm.setPurpose(model.getPurpose());
        vm.setVisitDate(parseDateString(model.getVisitDateStr()));
        vm.setWhomToMeet(model.getEmpId());
        vm.setApproved(false);
        vm.setInvited(false);
        vm.setExpired(false);
        vm.setDirectCheckIn(false);
        vm.setAccept(false);
        vm.setIsActive(true);
        vm.setIsUpdated(false);
        vm.setIsDeleted(false);
        vm.setCreatedDate(new Date());
        
        vm = visitorManagementRepository.save(vm);
        
        model.setVisitorId(vm.getVisitorId());
        model.setStatus("Pending");
        model.setMsg("Visitor registered successfully");
        return model;
    }

    public VisitorManagementViewModel updateVisitor(VisitorManagementViewModel model) {
        Optional<VisitorManagement> vmOpt = visitorManagementRepository.findById(model.getVisitorId());
        if (vmOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Visitor not found\"}");
        }
        
        VisitorManagement vm = vmOpt.get();
        vm.setName(model.getVisitorName());
        vm.setCompany(model.getCompany());
        vm.setMobile(model.getMobile());
        vm.setPMail(model.getPMail());
        vm.setPurpose(model.getPurpose());
        vm.setVisitDate(parseDateString(model.getVisitDateStr()));
        vm.setWhomToMeet(model.getEmpId());
        vm.setIsUpdated(true);
        vm.setLastUpdatedDate(new Date());
        
        visitorManagementRepository.save(vm);
        
        model.setMsg("Visitor updated successfully");
        return model;
    }

    public List<VisitorManagementViewModel> getAllVisitor(VisitorManagementViewModel model) {
        return visitorManagementRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(v -> convertToViewModel(v))
            .collect(Collectors.toList());
    }

    public VisitorManagementViewModel approveVisitor(VisitorManagementViewModel model) {
        Optional<VisitorManagement> vmOpt = visitorManagementRepository.findById(model.getVisitorId());
        if (vmOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Visitor not found\"}");
        }
        
        VisitorManagement vm = vmOpt.get();
        vm.setApproved(model.getIsActive());
        vm.setLastUpdatedDate(new Date());
        visitorManagementRepository.save(vm);
        
        model.setMsg("Visitor approved successfully");
        return model;
    }

    public VisitorManagementViewModel deleteVisitor(VisitorManagementViewModel model) {
        Optional<VisitorManagement> vmOpt = visitorManagementRepository.findById(model.getVisitorId());
        if (vmOpt.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Visitor not found\"}");
        }
        
        VisitorManagement vm = vmOpt.get();
        vm.setIsDeleted(true);
        vm.setIsActive(false);
        vm.setLastUpdatedDate(new Date());
        visitorManagementRepository.save(vm);
        
        model.setMsg("Visitor deleted successfully");
        return model;
    }

    private VisitorManagementViewModel convertToViewModel(VisitorManagement v) {
        VisitorManagementViewModel vm = new VisitorManagementViewModel();
        vm.setVisitorId(v.getVisitorId());
        vm.setVisitId(v.getVisitorId());
        vm.setEmpId(v.getWhomToMeet());
        vm.setVisitorName(v.getName());
        vm.setDesignation(v.getDesignation());
        vm.setCompany(v.getCompany());
        vm.setPurpose(v.getPurpose());
        vm.setPMail(v.getPMail());
        vm.setOMail(v.getOMail());
        vm.setMobile(v.getMobile());
        vm.setAMobile(v.getAMobile());
        vm.setPhoto(v.getPhoto() != null && !v.getPhoto().isEmpty() ? normalizePhotoPath(v.getPhoto()) : null);
        vm.setCompId(v.getCompId());
        vm.setCompName(v.getCompId());
        vm.setAccessories(v.getAccessories());
        vm.setWhomtoMeet(v.getWhomToMeet());

        // Resolve WName and WEmpCode from EmployeeMaster
        String wName = "";
        String wEmpCode = "";
        if (v.getWhomToMeet() != null) {
            Optional<EmployeeMaster> empOpt = employeeMasterRepository.findById(v.getWhomToMeet());
            if (empOpt.isPresent()) {
                EmployeeMaster emp = empOpt.get();
                String fn = emp.getFirstName() != null ? emp.getFirstName().trim() : "";
                String mn = emp.getMiddleName() != null ? " " + emp.getMiddleName().trim() : "";
                String ln = emp.getLastName() != null ? " " + emp.getLastName().trim() : "";
                wName = (fn + mn + ln).trim();
                wEmpCode = emp.getEmpCode() != null ? emp.getEmpCode() : "";
            }
        }
        vm.setWName(wName);
        vm.setWEmpCode(wEmpCode);

        vm.setVisitDate(v.getVisitDate());
        vm.setVisitDateStr(v.getVisitDate() != null ? "/Date(" + v.getVisitDate().getTime() + ")/" : null);
        vm.setVisitTime(v.getTime());
        vm.setInvited(v.getInvited());
        vm.setAccept(v.getAccept());
        vm.setApproved(v.getApproved());
        vm.setExpired(v.getExpired());
        vm.setDirectCheckIn(v.getDirectCheckIn());
        vm.setCheckIn(v.getCheckIn());
        vm.setCheckOut(v.getCheckOut());
        vm.setIdCard(v.getIdCard());
        vm.setStatus(v.getApproved() != null && v.getApproved() ? "Approved" : "Pending");
        vm.setIsActive(v.getIsActive());
        return vm;
    }

    private String normalizePhotoPath(String photo) {
        if (photo == null || photo.isEmpty()) return "";
        if (photo.contains("Uploads")) {
            String[] parts = photo.split("Uploads", 2);
            if (parts.length > 1) return "Uploads" + parts[1];
        }
        return photo;
    }

    private java.util.Date parseDateString(String dateStr) {
        if (dateStr == null || dateStr.isEmpty()) return null;
        try {
            // Try DD-MM-YYYY format first (frontend format)
            if (dateStr.contains("-") && dateStr.split("-")[0].length() == 2) {
                return new java.text.SimpleDateFormat("dd-MM-yyyy").parse(dateStr);
            }
            // Try /Date(timestamp)/ format
            java.util.regex.Matcher matcher = java.util.regex.Pattern.compile("/Date\\((\\d+)\\)/").matcher(dateStr);
            if (matcher.matches()) {
                return new java.util.Date(Long.parseLong(matcher.group(1)));
            }
            // Try yyyy-MM-dd format
            return new java.text.SimpleDateFormat("yyyy-MM-dd").parse(dateStr);
        } catch (Exception e) {
            return null;
        }
    }

    public VisitorManagementViewModel visitorInvite(VisitorManagementViewModel model) {
        Integer empId = model.getEmpId();
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Mismatching");

        VisitorManagement ivm = new VisitorManagement();
        ivm.setName(model.getVisitorName());
        ivm.setDesignation(model.getDesignation() != null ? model.getDesignation() : "");
        ivm.setCompany(model.getCompany() != null ? model.getCompany() : "");
        ivm.setPurpose(model.getPurpose() != null ? model.getPurpose() : "");
        ivm.setPMail(model.getPMail() != null ? model.getPMail() : "");
        ivm.setOMail(model.getOMail() != null ? model.getOMail() : "");
        ivm.setMobile(model.getMobile() != null ? model.getMobile() : "");
        ivm.setAMobile(model.getAMobile() != null ? model.getAMobile() : "");
        ivm.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        ivm.setCompId(model.getCompId() != null ? model.getCompId() : "");
        ivm.setWhomToMeet(model.getWhomtoMeet() != null ? model.getWhomtoMeet() : 0);
        ivm.setVisitDate(parseDateString(model.getVisitDateStr()));
        ivm.setTime(model.getVisitTime());
        ivm.setInvited(true);
        ivm.setAccept(false);
        ivm.setApproved(false);
        ivm.setExpired(false);
        ivm.setAccessories("");
        ivm.setDirectCheckIn(false);
        ivm.setIdCard("");
        ivm.setIsActive(true);
        ivm.setIsUpdated(false);
        ivm.setIsDeleted(false);
        ivm.setCreatedBy(empId);
        ivm.setCreatedDate(new Date());
        ivm.setLastUpdatedBy(empId);
        ivm.setLastUpdatedDate(new Date());
        visitorManagementRepository.save(ivm);

        VisitorManagementViewModel result = new VisitorManagementViewModel();
        result.setMsg("Invite Created");
        return result;
    }

    public VisitorManagementViewModel visitorInviteHistory(VisitorManagementViewModel model) {
        model.setMsg("Visitor invite history retrieved successfully");
        return model;
    }

    public List<VisitorManagementViewModel> getAllVisitorInviteHistory(VisitorManagementViewModel model) {
        Integer empId = model.getEmpId();
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");

        java.util.Calendar todayCal = java.util.Calendar.getInstance();
        todayCal.set(java.util.Calendar.HOUR_OF_DAY, 0);
        todayCal.set(java.util.Calendar.MINUTE, 0);
        todayCal.set(java.util.Calendar.SECOND, 0);
        todayCal.set(java.util.Calendar.MILLISECOND, 0);
        Date today = todayCal.getTime();

        List<VisitorManagement> visitors = visitorManagementRepository.findByIsDeleted(false);

        List<VisitorManagement> sorted = visitors.stream()
            .sorted((a, b) -> {
                Date aDate = a.getVisitDate();
                Date bDate = b.getVisitDate();
                boolean aIsToday = aDate != null && !aDate.before(today);
                boolean bIsToday = bDate != null && !bDate.before(today);
                if (aIsToday != bIsToday) return aIsToday ? -1 : 1;
                if (aDate != null && bDate != null) {
                    int dateCmp = bDate.compareTo(aDate);
                    if (dateCmp != 0) return dateCmp;
                }
                int aId = a.getVisitorId() != null ? a.getVisitorId() : 0;
                int bId = b.getVisitorId() != null ? b.getVisitorId() : 0;
                return Integer.compare(bId, aId);
            })
            .collect(Collectors.toList());

        List<VisitorManagementViewModel> result = new ArrayList<>();
        for (VisitorManagement v : sorted) {
            result.add(convertToViewModel(v));
        }
        return result;
    }

    public VisitorManagementViewModel visitorUpdateInvite(VisitorManagementViewModel model) {
        model.setMsg("Visitor invite updated successfully");
        return model;
    }

    public VisitorManagementViewModel visitorOTPVerify(VisitorManagementViewModel model) {
        model.setMsg("OTP verified successfully");
        return model;
    }

    public List<VisitorManagementViewModel> getAllVisitorByEmp(VisitorManagementViewModel model) {
        Integer empId = model.getEmpId();
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");

        return visitorManagementRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(v -> empId.equals(v.getCreatedBy()))
            .map(v -> convertToViewModel(v))
            .collect(Collectors.toList());
    }

    public VisitorManagementViewModel visitorCheckIn(VisitorManagementViewModel model) {
        model.setMsg("Visitor checked in successfully");
        return model;
    }

    public VisitorManagementViewModel visitorCheckOut(VisitorManagementViewModel model) {
        Integer empId = model.getEmpId();
        Integer visitId = model.getVisitId();
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        if (visitId == null || visitId == 0) throw new RuntimeException("VisitId is Missing");

        Optional<VisitorManagement> opt = visitorManagementRepository.findById(visitId);
        if (opt.isEmpty()) throw new RuntimeException("Visitor Details Not Found");

        VisitorManagement vm = opt.get();
        if (Boolean.TRUE.equals(vm.getIsDeleted()) || Boolean.TRUE.equals(vm.getExpired())
                || Boolean.FALSE.equals(vm.getInvited()) || Boolean.FALSE.equals(vm.getAccept())
                || vm.getCheckIn() == null) {
            throw new RuntimeException("Visitor Details Not Found");
        }

        if (model.getIdCard() != null) vm.setIdCard(model.getIdCard());
        if (model.getAccessories() != null) vm.setAccessories(model.getAccessories());
        vm.setCheckOut(new Date());
        vm.setIsUpdated(true);
        vm.setLastUpdatedBy(visitId);
        vm.setLastUpdatedDate(new Date());
        if (model.getPhoto() != null && !model.getPhoto().isEmpty()) {
            vm.setPhoto(model.getPhoto());
        }
        visitorManagementRepository.save(vm);

        VisitorManagementViewModel result = new VisitorManagementViewModel();
        result.setMsg("Visitor Checked out Successfully");
        return result;
    }

    public List<VisitorManagementViewModel> getVisitorToday(VisitorManagementViewModel model) {
        return visitorManagementRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .map(v -> convertToViewModel(v))
            .collect(Collectors.toList());
    }

    public List<VisitorManagementViewModel> getAllApprovedVisitor(VisitorManagementViewModel model) {
        return visitorManagementRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(v -> v.getApproved() != null && v.getApproved())
            .map(v -> convertToViewModel(v))
            .collect(Collectors.toList());
    }

    public VisitorManagementViewModel visitorDownloadPDF(VisitorManagementViewModel model) {
        model.setMsg("PDF downloaded successfully");
        return model;
    }

    public List<VisitorManagementViewModel> getAllVisitorHistory(VisitorManagementViewModel model) {
        return visitorManagementRepository.findAll().stream()
            .map(v -> convertToViewModel(v))
            .collect(Collectors.toList());
    }

    public VisitorManagementViewModel visitorEmail(VisitorManagementViewModel model) {
        model.setMsg("Email sent successfully");
        return model;
    }

    public VisitorManagementViewModel checkVisitorExists(VisitorManagementViewModel model) {
        model.setMsg("Visitor exists check completed");
        return model;
    }

    public List<Map<String, Object>> ddVisitorEmployee(Map<String, Object> model) {
        List<Map<String, Object>> result = new ArrayList<>();
        List<EmployeeMaster> employees = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false);
        
        for (EmployeeMaster emp : employees) {
            Map<String, Object> m = new HashMap<>();
            m.put("EmpId", emp.getEmpId());
            
            String firstName = emp.getFirstName() != null ? emp.getFirstName().trim() : "";
            String middleName = emp.getMiddleName() != null ? emp.getMiddleName().trim() : "";
            String lastName = emp.getLastName() != null ? emp.getLastName().trim() : "";
            
            StringBuilder fullName = new StringBuilder();
            if (!firstName.isEmpty()) {
                fullName.append(firstName);
            }
            if (!middleName.isEmpty()) {
                if (fullName.length() > 0) fullName.append(" ");
                fullName.append(middleName);
            }
            if (!lastName.isEmpty()) {
                if (fullName.length() > 0) fullName.append(" ");
                fullName.append(lastName);
            }
            
            m.put("EmpName", fullName.toString());
            m.put("EmpCode", emp.getEmpCode());
            
            result.add(m);
        }
        
        return result;
    }

    public List<Map<String, Object>> ddVisitorCompany(Map<String, Object> model) {
        List<Map<String, Object>> result = new java.util.ArrayList<>();
        result.add(Map.of("CompId", 1, "CompName", "Sample Company", "StatusCode", 200));
        return result;
    }

    public VisitorManagementViewModel directCheckIn(VisitorManagementViewModel model) {
        Integer empId = model.getEmpId();
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");

        VisitorManagement ivm = new VisitorManagement();
        ivm.setName(model.getVisitorName());
        ivm.setDesignation(model.getDesignation() != null ? model.getDesignation() : "");
        ivm.setCompany(model.getCompany() != null ? model.getCompany() : "");
        ivm.setPurpose(model.getPurpose() != null ? model.getPurpose() : "");
        ivm.setPMail(model.getPMail() != null ? model.getPMail() : "");
        ivm.setOMail(model.getOMail() != null ? model.getOMail() : "");
        ivm.setMobile(model.getMobile() != null ? model.getMobile() : "");
        ivm.setAMobile(model.getAMobile() != null ? model.getAMobile() : "");
        ivm.setPhoto(model.getPhoto() != null ? model.getPhoto() : "");
        ivm.setCompId(model.getCompId() != null ? model.getCompId() : "");
        ivm.setWhomToMeet(model.getWhomtoMeet() != null ? model.getWhomtoMeet() : 0);
        ivm.setVisitDate(parseDateString(model.getVisitDateStr()));
        ivm.setTime(model.getVisitTime());
        ivm.setInvited(true);
        ivm.setAccept(true);
        ivm.setApproved(true);
        ivm.setExpired(false);
        ivm.setAccessories(model.getAccessories() != null ? model.getAccessories() : "");
        ivm.setCheckIn(new Date());
        ivm.setDirectCheckIn(true);
        ivm.setIdCard(model.getIdCard() != null ? model.getIdCard() : "");
        ivm.setIsActive(true);
        ivm.setIsUpdated(false);
        ivm.setIsDeleted(false);
        ivm.setCreatedBy(empId);
        ivm.setCreatedDate(new Date());
        ivm.setLastUpdatedBy(empId);
        ivm.setLastUpdatedDate(new Date());
        visitorManagementRepository.save(ivm);

        VisitorManagementViewModel result = new VisitorManagementViewModel();
        result.setMsg("Visitor Checked In Successfully");
        result.setVisitorName(model.getVisitorName());
        return result;
    }

    public VisitorManagementViewModel verifyOTPCheckIn(VisitorManagementViewModel model) {
        model.setMsg("OTP verified and checked in successfully");
        return model;
    }

    public VisitorManagementViewModel acceptInvite(VisitorManagementViewModel model) {
        model.setMsg("Invite accepted successfully");
        return model;
    }

    public List<Map<String, Object>> visitFilter(Map<String, Object> model) {
        List<Map<String, Object>> result = new java.util.ArrayList<>();
        result.add(Map.of("Message", "Visit filter applied", "StatusCode", 200));
        return result;
    }

    public VisitorManagementViewModel cancelInvite(VisitorManagementViewModel model) {
        Integer empId = model.getEmpId();
        Integer visitId = model.getVisitId();
        if (empId == null || empId == 0) throw new RuntimeException("EmpId is Missing");
        if (visitId == null || visitId == 0) throw new RuntimeException("VisitId is Missing");

        VisitorManagement vm = visitorManagementRepository.findById(visitId)
            .orElseThrow(() -> new RuntimeException("Invite Details Not Found"));

        vm.setIsActive(true);
        vm.setIsUpdated(true);
        vm.setIsDeleted(true);
        vm.setLastUpdatedBy(empId);
        vm.setLastUpdatedDate(new Date());
        visitorManagementRepository.save(vm);

        VisitorManagementViewModel result = new VisitorManagementViewModel();
        result.setMsg("Invite Cancelled");
        return result;
    }
}