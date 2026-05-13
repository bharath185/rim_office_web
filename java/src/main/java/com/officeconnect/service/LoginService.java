package com.officeconnect.service;

import com.officeconnect.config.JwtAuthenticationFilter;
import com.officeconnect.dto.CheckAuthViewModel;
import com.officeconnect.dto.EmployeeMasterViewModel;
import com.officeconnect.dto.FRViewModel;
import com.officeconnect.dto.LoginDetailsViewModel;
import com.officeconnect.dto.LoginViewModel;
import com.officeconnect.dto.WFHLoginlogViewModel;
import com.officeconnect.entity.*;
import com.officeconnect.repository.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.nio.charset.StandardCharsets;
import java.text.SimpleDateFormat;
import java.util.*;
import java.util.stream.Collectors;
import java.util.Base64;

@Service
public class LoginService {

    // Helper method to format date as /Date(timestamp)/
    private Object formatDate(Date date) {
        if (date == null) {
            return null;
        }
        return "/Date(" + date.getTime() + ")/";
    }

    // Helper method to capitalize first letter
    private String capitalizeFirst(String str) {
        if (str == null || str.isEmpty()) {
            return str;
        }
        return str.substring(0, 1).toUpperCase() + str.substring(1).toLowerCase();
    }

    @Autowired
    private EmployeeMasterRepository employeeMasterRepository;

    @Autowired
    private SessionMasterRepository sessionMasterRepository;

    @Autowired
    private CompanyMasterRepository companyMasterRepository;

    @Autowired
    private CPwdManagementRepository cpwdManagementRepository;

    @Autowired
    private FPwdManagementRepository fpwdManagementRepository;

    @Autowired
    private PassHistoryManagementRepository passHistoryManagementRepository;

    @Autowired
    private EmailSetUpRepository emailSetUpRepository;

    @Autowired
    private WFHLoginlogRepository wfhLoginlogRepository;

    public EmployeeMasterViewModel checkLogin(LoginViewModel loginUser) {
        if (loginUser == null || loginUser.getUserName() == null || loginUser.getPassword() == null ||
            loginUser.getUserName().isEmpty() || loginUser.getPassword().isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Invalid Input Parameters\"}");
        }

        String username = loginUser.getUserName();
        String password = loginUser.getPassword();

        List<EmployeeMaster> empDetails = employeeMasterRepository.findActiveUserByUserName(username);

        if (empDetails == null || empDetails.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"UserName is Mismatching\"}");
        }

        EmployeeMaster emp = empDetails.get(0);
        int empId = emp.getEmpId();
        Integer oldEmpId = emp.getOldEmp_ID();
        Integer leId = emp.getLeId();

        // Find authorized employees
        List<EmployeeMaster> authorizedEmp = null;
        if (leId != null && leId == 1) {
            authorizedEmp = employeeMasterRepository.findByReportIdAndEmpCodeStartingWith(oldEmpId, "3DCAD-");
            if (authorizedEmp == null || authorizedEmp.isEmpty()) {
                authorizedEmp = employeeMasterRepository.findByReportIdAndEmpCodeStartingWith(oldEmpId, "3DCADVS-");
            }
            if (authorizedEmp == null || authorizedEmp.isEmpty()) {
                authorizedEmp = employeeMasterRepository.findByReportIdAndEmpCodeStartingWith(oldEmpId, "3DCADPU-");
            }
            if (authorizedEmp == null || authorizedEmp.isEmpty()) {
                authorizedEmp = employeeMasterRepository.findByReportId(oldEmpId);
            }
        } else {
            authorizedEmp = employeeMasterRepository.findByReportIdAndEmpCodeStartingWith(empId, "RIM-");
        }

EmployeeMasterViewModel userDetails = new EmployeeMasterViewModel();
        userDetails.setCompId(emp.getCompId());
        if (emp.getCompId() != null) {
            Optional<CompanyMaster> company = companyMasterRepository.findById(emp.getCompId());
            company.ifPresent(c -> userDetails.setCompany(c.getCompany()));
        }
        userDetails.setOldEmp_ID(null);
        userDetails.setLeId(null);
        userDetails.setLegalEntity(null);
        userDetails.setBuId(null);
        userDetails.setBusinessUnit(null);
        userDetails.setLocationId(null);
        userDetails.setLocation(null);
        userDetails.setCategoryId(null);
        userDetails.setDeptId(emp.getCategoryId());
        userDetails.setDeptName(emp.getDeptName());
        userDetails.setDesignationId(emp.getDesignationId());
        userDetails.setDesignation(emp.getDesignationName());
        userDetails.setEmpId(emp.getEmpId());
        userDetails.setLoginId(emp.getEmpId());
        userDetails.setEmpCode(emp.getEmpCode());
        userDetails.setUserName(emp.getUserName());
        String plainPassword = emp.getPassword();
        if (plainPassword != null) {
            String encoded = Base64.getEncoder().encodeToString(plainPassword.getBytes(StandardCharsets.UTF_16LE));
            userDetails.setPassword(encoded);
        } else {
            userDetails.setPassword(null);
        }
        userDetails.setPhoto(null);
        userDetails.setSalutationId(null);
        userDetails.setSalutation(null);
        userDetails.setFirstName(emp.getFirstName());
        userDetails.setMiddleName(emp.getMiddleName());
        userDetails.setLastName(emp.getLastName());
        userDetails.setDob(formatDate(emp.getDob()));
        userDetails.setMobileNo(emp.getMobileNo());
        userDetails.setEmailId(emp.getEmailId());
        userDetails.setBloodGroup(null);
        userDetails.setMaritalStatus(null);
        userDetails.setGender(capitalizeFirst(emp.getGender()));
        userDetails.setInterviewDate(null);
        userDetails.setJoiningDate(formatDate(emp.getJoiningDate()));
        userDetails.setEndDate(null);
        userDetails.setEmpStatus(emp.getEmpStatus());
        userDetails.setReason(null);
        userDetails.setEmpType(null);
        userDetails.setEmpTypeId(null);
        userDetails.setcEndDate(null);
        userDetails.setcPwd(false);
        userDetails.setOnSiteLogInId(null);
        userDetails.setOnSiteLogInDate(null);
        userDetails.setOnSiteLogOutDate(null);
        userDetails.setOnSiteLogInTime(null);
        userDetails.setOnSiteLogOutTime(null);
        userDetails.setOnSiteStatus(null);
        userDetails.setAuthorisedEntity(null);
        userDetails.setRelievedReason(null);
        userDetails.setRelievedDate(null);
        userDetails.setRelievedEffectiveDate(null);
        userDetails.setIsRelieved(null);
        userDetails.setFromDate(null);
        userDetails.setToDate(null);
        userDetails.setStatus(null);
        userDetails.setReportId(emp.getReportId());
        userDetails.setApproverId(null);
        userDetails.setApprover(null);
        userDetails.setReportEmpCode(null);
        userDetails.setReportEmpName(null);

        if (emp.getReportId() != null) {
            Optional<EmployeeMaster> reporter = employeeMasterRepository.findById(emp.getReportId());
            reporter.ifPresent(r -> userDetails.setReportEmpCode(r.getEmpCode()));
        }

        if (authorizedEmp != null && !authorizedEmp.isEmpty()) {
            userDetails.setAuthorised(true);
        } else {
            userDetails.setAuthorised(false);
        }

        userDetails.setIsActive(emp.getIsActive());
        userDetails.setIsUpdated(emp.getIsUpdated());
        userDetails.setIsDeleted(emp.getIsDeleted());
        userDetails.setIsProbation(null);
        userDetails.setIsProbationConfirm(null);
        userDetails.setProbationConfirmationEffectiveDate(null);
        userDetails.setProbationConfirmationDate(null);
        userDetails.setProbationRemarks(null);
        userDetails.setProbationConfirmationStatus(null);
        userDetails.setMsg(null);
        userDetails.setCreatedBy(emp.getCreatedBy());
        userDetails.setCreatedDate(formatDate(emp.getCreatedDate()));
        userDetails.setLastUpdatedBy(emp.getLastUpdatedBy());
        userDetails.setLastUpdatedDate(formatDate(emp.getLastUpdatedDate()));
        userDetails.setcPwd(false);

        // Check if has compulsory password changed
        try {
            List<CPwdManagement> cpwdList = cpwdManagementRepository.findByEmpCodeIgnoreCaseAndCpwdAndExpiredAndIsActiveAndIsDeleted(username, true, false, true, false);
            if (cpwdList != null && !cpwdList.isEmpty()) {
                userDetails.setcPwd(true);
            }
        } catch (Exception e) {
            // Skip if table doesn't exist
        }

        // Generate tokens - handle null designationId
        String userName = userDetails.getUserName();
        Integer roleId = userDetails.getDesignationId();
        String roleIdStr = (roleId != null) ? String.valueOf(roleId) : "1";
        
        String tokenId = JwtAuthenticationFilter.encodeAuthToken(userName);
        String userAuth = JwtAuthenticationFilter.encodeToken(userName, roleIdStr);
        
        userDetails.setTokenId(tokenId);
        userDetails.setUserAuth(userAuth);
        
        System.out.println("=== TOKEN GENERATED ===");
        System.out.println("TokenId: " + tokenId);
        System.out.println("UserAuth: " + userAuth);

        // Verify password
        try {
            String storedPassword = emp.getPassword();
            boolean passwordMatch = false;
            
            if (storedPassword != null) {
                try {
                    byte[] decrypted = Base64.getDecoder().decode(storedPassword);
                    // Try UTF-16 (with BOM) first, then UTF-16LE (legacy .NET)
                    String decryptedPassword = new String(decrypted, StandardCharsets.UTF_16);
                    if (!decryptedPassword.trim().equals(password.trim())) {
                        decryptedPassword = new String(decrypted, StandardCharsets.UTF_16LE);
                    }
                    if (decryptedPassword.trim().equals(password.trim())) {
                        passwordMatch = true;
                    }
                } catch (Exception e) {
                    // Not base64, try plain match
                }
                
                if (!passwordMatch && storedPassword.equals(password)) {
                    passwordMatch = true;
                }
            }
            
            if (!passwordMatch) {
                throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Password is Mismatching\"}");
            }
            
            SessionMaster session = new SessionMaster();
            session.setUsername(username);
            session.setTockenId(userDetails.getTokenId());
            session.setAuthKey(userDetails.getUserAuth());
            session.setRoleId(userDetails.getDesignationId());
            session.setStatus(true);
            session.setExpired(false);
            session.setWfh(false);
            session.setIsActive(true);
            session.setIsDeleted(false);
            session.setCreatedDate(new Date());
            session.setLastUpdatedDate(new Date());
            sessionMasterRepository.save(session);

            return userDetails;
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Password is Mismatching\"}");
        }
    }

    public List<Map<String, Object>> getAllWFHDays(Map<String, Object> model) {
        List<Map<String, Object>> list = new java.util.ArrayList<>();
        Map<String, Object> day = new java.util.HashMap<>();
        day.put("day", "2026-01-01");
        day.put("status", "Planned");
        list.add(day);
        return list;
    }

    public EmployeeMasterViewModel checkLogOut(LoginViewModel loginUser) {
        if (loginUser == null || loginUser.getUserName() == null || loginUser.getUserName().isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Not Found\"}");
        }

        String username = loginUser.getUserName();
        String token = loginUser.getTokenId();
        String authKey = loginUser.getAuthKey();
        Integer roleId = loginUser.getRoleId();

        List<EmployeeMaster> empDetails = employeeMasterRepository.findActiveUserByUserName(username);
        if (empDetails == null || empDetails.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User is Not Found\"}");
        }

        EmployeeMaster emp = empDetails.get(0);
        EmployeeMasterViewModel userDetails = new EmployeeMasterViewModel();
        userDetails.setEmpId(emp.getEmpId());
        userDetails.setCompId(0);
        userDetails.setCategoryId(0);
        userDetails.setDesignationId(0);
        userDetails.setEmpCode(emp.getEmpCode());
        userDetails.setUserName(emp.getUserName());
        userDetails.setEmpStatus(emp.getEmpStatus());
        userDetails.setTokenId("Expired");

        // Find and update session
        Optional<SessionMaster> sessionOpt = sessionMasterRepository.findActiveSessionByUsernameAndToken(username, token);
        if (sessionOpt.isPresent()) {
            SessionMaster session = sessionOpt.get();
            Optional<SessionMaster> sessionWithAuth = sessionMasterRepository.findActiveSessionByUsernameTokenAuthKeyAndRole(username, token, authKey, roleId);
            if (sessionWithAuth.isPresent()) {
                SessionMaster s = sessionWithAuth.get();
                s.setExpired(true);
                s.setLastUpdatedDate(new Date());
                sessionMasterRepository.save(s);
            } else {
                throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User Authorization is Failed\"}");
            }
        } else {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User Token is Expired\"}");
        }

        return userDetails;
    }

    public CheckAuthViewModel checkAuth(LoginViewModel loginUser) {
        String username = loginUser.getUserName();

        List<EmployeeMaster> empDetails = employeeMasterRepository.findActiveUserByUserName(username);
        if (empDetails == null || empDetails.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"UserName is Mismatching\"}");
        }

        CheckAuthViewModel checkAuth = new CheckAuthViewModel();
        checkAuth.setUserName(username);
        checkAuth.setTokenId("Success");
        checkAuth.setAuthKey("Success");

        return checkAuth;
    }

    public FRViewModel forgetPassword(FRViewModel model) {
        if (model.getUserName() == null || model.getUserName().isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"UserName is Mismatching\"}");
        }

        String username = model.getUserName();
        String email = model.getEmail();

        List<EmployeeMaster> empDetails = employeeMasterRepository.findActiveUserByUserName(username);
        if (empDetails == null || empDetails.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User is not found\"}");
        }

        EmployeeMaster emp = empDetails.get(0);
        if (!email.equals(emp.getEmailId())) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"EmailId is Mismatching\"}");
        }

        String otp = generateSecureOTP();

        // Save OTP
        FPwdManagement fpm = new FPwdManagement();
        fpm.setEmpId(emp.getEmpId());
        fpm.setEmpCode(username);
        fpm.setOtp(otp);
        fpm.setExpired(false);
        fpm.setCreatedBy(emp.getEmpId());
        fpm.setCreatedDate(new Date());
        fpm.setLastUpdatedBy(emp.getEmpId());
        fpm.setLastUpdatedDate(new Date());
        fpm.setIsActive(true);
        fpm.setIsUpdated(false);
        fpm.setIsDeleted(false);
        fpwdManagementRepository.save(fpm);

        FRViewModel frvm = new FRViewModel();
        frvm.setMsg("OTP Send successfully");
        frvm.setOtp(otp);
        frvm.setUserName(username);

        return frvm;
    }

    public FRViewModel fpwdVerify(FRViewModel model) {
        String username = model.getUserName();
        String otp = model.getOtp();

        Optional<FPwdManagement> fpwdDetails = fpwdManagementRepository.findActiveByEmpCodeAndOtp(username, otp);
        if (fpwdDetails.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"OTP is Invalid\"}");
        }

        // Expire all OTPs for this user
        List<FPwdManagement> fpwdList = fpwdManagementRepository.findActiveByEmpCode(username);
        for (FPwdManagement f : fpwdList) {
            f.setExpired(true);
            f.setIsUpdated(true);
            f.setLastUpdatedDate(new Date());
            fpwdManagementRepository.save(f);
        }

        EmployeeMaster emp = employeeMasterRepository.findActiveUserByUserName(username).get(0);

        FRViewModel frvm = new FRViewModel();
        frvm.setMsg("OTP Verified");
        frvm.setUserName(username);
        frvm.setEmpId(emp.getEmpId());
        frvm.setEmpCode(emp.getEmpCode());

        return frvm;
    }

    public FRViewModel changePassword(FRViewModel model) {
        String empCode = model.getUserName();
        String newPassword = model.getOtp(); // Using OTP field for new password
        Boolean fpwd = model.getMsg() != null && model.getMsg().equals("FPwd");

        List<EmployeeMaster> empDetails = employeeMasterRepository.findActiveUserByUserName(empCode);
        if (empDetails == null || empDetails.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User details is Mismatching\"}");
        }

        EmployeeMaster emp = empDetails.get(0);

        // Update password
        String encodedPassword = Base64.getEncoder().encodeToString(newPassword.getBytes(StandardCharsets.UTF_16));
        emp.setPassword(encodedPassword);
        emp.setIsUpdated(true);
        emp.setLastUpdatedBy(emp.getEmpId());
        emp.setLastUpdatedDate(new Date());
        employeeMasterRepository.save(emp);

        // Create history
        PassHistoryManagement phm = new PassHistoryManagement();
        phm.setEmpId(emp.getEmpId());
        phm.setEmpCode(empCode);
        phm.setOldPassword(emp.getPassword());
        phm.setNewPassword(newPassword);
        phm.setFpwd(fpwd);
        phm.setCpwd(!fpwd);
        phm.setExpired(false);
        phm.setCreatedBy(emp.getEmpId());
        phm.setCreatedDate(new Date());
        phm.setIsActive(true);
        phm.setIsUpdated(false);
        phm.setIsDeleted(false);
        passHistoryManagementRepository.save(phm);

        FRViewModel frvm = new FRViewModel();
        frvm.setMsg("Password Changed");
        frvm.setEmpCode(empCode);

        return frvm;
    }

    private String generateSecureOTP() {
        int randomNumber = (int) (Math.random() * 900000) + 100000;
        return String.valueOf(randomNumber);
    }

    public LoginDetailsViewModel getLoginDetails(LoginDetailsViewModel model) {
        String username = model.getUserName();
        if (username == null || username.isEmpty()) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"UserName is Mismatching\"}");
        }

        LoginDetailsViewModel ldvm = new LoginDetailsViewModel();
        ldvm.setUserName(username);
        ldvm.setEmpCode(username);
        ldvm.setMode("");
        ldvm.setDate("");
        ldvm.setTime("");

        return ldvm;
    }

    public Map<String, Object> getAllWFHDetails(Map<String, Object> model) {
        Map<String, Object> result = new java.util.HashMap<>();
        result.put("msg", "WFH details retrieved");
        return result;
    }

    public Map<String, Object> getAllWFHAnalysis(Map<String, Object> model) {
        Map<String, Object> result = new java.util.HashMap<>();
        result.put("msg", "WFH analysis retrieved");
        return result;
    }

    public Map<String, Object> getAllWFHFilterDetails(Map<String, Object> model) {
        Map<String, Object> result = new java.util.HashMap<>();
        try {
            Integer loginId = 0;
            if (model.containsKey("LoginId") && model.get("LoginId") != null) {
                loginId = parseInteger(model.get("LoginId"));
            }
            if (loginId == 0) {
                result.put("StatusCode", 404);
                result.put("Message", "EmpId is Missing");
                return result;
            }

            Integer compId = parseInteger(model.get("CompId"));
            Integer deptId = parseInteger(model.get("DeptId"));
            Integer designationId = parseInteger(model.get("DesignationId"));
            Integer empId = parseInteger(model.get("EmpId"));
            String fromDateStr = model.get("FromDate") != null ? model.get("FromDate").toString() : "";
            String toDateStr = model.get("ToDate") != null ? model.get("ToDate").toString() : "";

            SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
            SimpleDateFormat timeFormat = new SimpleDateFormat("HH:mm:ss");
            Calendar today = Calendar.getInstance();
            today.set(Calendar.HOUR_OF_DAY, 0);
            today.set(Calendar.MINUTE, 0);
            today.set(Calendar.SECOND, 0);
            today.set(Calendar.MILLISECOND, 0);
            Date todayDate = today.getTime();

            List<WFHLoginlog> allWFH = wfhLoginlogRepository.findAll().stream()
                .filter(w -> w.getIsActive() != null && w.getIsActive() && w.getIsDeleted() != null && !w.getIsDeleted())
                .filter(w -> w.getLoginTime() != null)
                .collect(Collectors.toList());

            List<EmployeeMaster> employees = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
                .filter(e -> e.getEmpStatus() != null && "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
                .collect(Collectors.toList());
            Map<Integer, EmployeeMaster> empMap = employees.stream()
                .collect(Collectors.toMap(EmployeeMaster::getEmpId, e -> e, (a, b) -> a));

            List<CompanyMaster> companies = companyMasterRepository.findByIsActiveAndIsDeleted(true, false);
            Map<Integer, String> compMap = companies.stream()
                .collect(Collectors.toMap(CompanyMaster::getCompId, CompanyMaster::getCompany, (a, b) -> a));

            Map<String, List<WFHLoginlog>> groupedData = allWFH.stream()
                .filter(w -> empMap.containsKey(w.getEmpId()))
                .collect(Collectors.groupingBy(w -> w.getEmpId() + "_" + sdf.format(w.getDate())));

            List<WFHLoginlogViewModel> vmList = new ArrayList<>();
            Calendar defaultLogout = Calendar.getInstance();
            defaultLogout.set(Calendar.HOUR_OF_DAY, 18);
            defaultLogout.set(Calendar.MINUTE, 35);
            defaultLogout.set(Calendar.SECOND, 0);
            defaultLogout.set(Calendar.MILLISECOND, 0);
            int defaultLogoutMs = ((18 * 3600 + 35 * 60) * 1000);

            for (Map.Entry<String, List<WFHLoginlog>> entry : groupedData.entrySet()) {
                List<WFHLoginlog> records = entry.getValue().stream()
                    .sorted((a, b) -> {
                        int msA = getTimeMsFromDate(a.getLoginTime());
                        int msB = getTimeMsFromDate(b.getLoginTime());
                        return Integer.compare(msA, msB);
                    })
                    .collect(Collectors.toList());

                if (records.isEmpty()) continue;

                WFHLoginlog firstRecord = records.get(0);
                int firstLoginMs = getTimeMsFromDate(firstRecord.getLoginTime());
                int lastLogoutMs = 0;
                int totalActiveMs = 0;

                for (int i = 0; i < records.size(); i++) {
                    WFHLoginlog record = records.get(i);
                    int logInMs = getTimeMsFromDate(record.getLoginTime());
                    int logOutMs;

                    if (record.getLogOutTime() != null) {
                        logOutMs = getTimeMsFromDate(record.getLogOutTime());
                    } else if (i + 1 < records.size()) {
                        logOutMs = getTimeMsFromDate(records.get(i + 1).getLoginTime());
                    } else {
                        logOutMs = defaultLogoutMs;
                    }

                    if (logOutMs > logInMs) {
                        totalActiveMs += (logOutMs - logInMs);
                    }
                    if (logOutMs > lastLogoutMs) {
                        lastLogoutMs = logOutMs;
                    }
                }

                EmployeeMaster emp = empMap.get(firstRecord.getEmpId());
                if (emp == null) continue;

                String empName = (emp.getFirstName() != null ? emp.getFirstName() : "") + " " +
                    (emp.getMiddleName() != null ? emp.getMiddleName() : "") + " " +
                    (emp.getLastName() != null ? emp.getLastName() : "");

                WFHLoginlogViewModel vm = new WFHLoginlogViewModel();
                vm.setWfhId(firstRecord.getWfhId());
                vm.setLoginId(loginId);
                vm.setEmpId(firstRecord.getEmpId());
                vm.setEmpCode(firstRecord.getEmpCode());
                vm.setEmpName(empName.trim());
                vm.setIpAddress(firstRecord.getIpAddress());
                vm.setDate(sdf.format(firstRecord.getDate()));
                vm.setLoginTime(msToTimeString(firstLoginMs));
                vm.setLogOutTime(msToTimeString(lastLogoutMs));

                int hours = totalActiveMs / (1000 * 60 * 60);
                int mins = (totalActiveMs % (1000 * 60 * 60)) / (1000 * 60);
                int secs = (totalActiveMs % (1000 * 60)) / 1000;
                vm.setActivehrs(String.format("%02d:%02d:%02d", hours, mins, secs));

                vm.setIsLoggedIn(firstRecord.getIsLoggedIn());
                vm.setIsLoggedOut(firstRecord.getIsLoggedOut());
                vm.setIsActive(firstRecord.getIsActive());
                vm.setIsDeleted(firstRecord.getIsDeleted());
                vm.setCreatedBy(firstRecord.getCreatedBy());
                vm.setCreatedDate(firstRecord.getCreatedDate() != null ? sdf.format(firstRecord.getCreatedDate()) : null);
                vm.setLastUpdatedBy(firstRecord.getLastUpdatedBy());
                vm.setLastUpdatedDate(firstRecord.getLastUpdatedDate() != null ? sdf.format(firstRecord.getLastUpdatedDate()) : null);

                vm.setCompId(emp.getCompId());
                vm.setCompName(compMap.getOrDefault(emp.getCompId(), ""));
                vm.setDeptId(emp.getCategoryId());
                vm.setDeptName(emp.getDeptName());
                vm.setDesignationId(emp.getDesignationId());
                vm.setDesignation(emp.getDesignationName());

                vmList.add(vm);
            }

            vmList.sort((a, b) -> {
                int dateCompare = b.getDate().compareTo(a.getDate());
                if (dateCompare != 0) return dateCompare;
                return b.getEmpId().compareTo(a.getEmpId());
            });

            if (compId != null && compId != 0) {
                Integer fCompId = compId;
                vmList = vmList.stream()
                    .filter(v -> v.getIsActive() && v.getCompId() != null && v.getCompId().equals(fCompId))
                    .collect(Collectors.toList());
            }
            if (deptId != null && deptId != 0) {
                Integer fDeptId = deptId;
                vmList = vmList.stream()
                    .filter(v -> v.getIsActive() && v.getDeptId() != null && v.getDeptId().equals(fDeptId))
                    .collect(Collectors.toList());
            }
            if (designationId != null && designationId != 0) {
                Integer fDesigId = designationId;
                vmList = vmList.stream()
                    .filter(v -> v.getIsActive() && v.getDesignationId() != null && v.getDesignationId().equals(fDesigId))
                    .collect(Collectors.toList());
            }
            if (empId != null && empId != 0) {
                Integer fEmpId = empId;
                vmList = vmList.stream()
                    .filter(v -> v.getIsActive() && v.getEmpId().equals(fEmpId))
                    .collect(Collectors.toList());
            }

            if (!fromDateStr.isEmpty() && !toDateStr.isEmpty()) {
                try {
                    Date fDate = sdf.parse(fromDateStr);
                    Date tDate = sdf.parse(toDateStr);
                    vmList = vmList.stream()
                        .filter(v -> {
                            try {
                                Date itemDate = sdf.parse(v.getDate());
                                return !itemDate.before(fDate) && !itemDate.after(tDate) && v.getIsActive();
                            } catch (Exception e) { return false; }
                        })
                        .collect(Collectors.toList());
                } catch (Exception e) {
                    // Invalid date format, skip date filtering
                }
            }

            for (WFHLoginlogViewModel item : vmList) {
                try {
                    Date itemDate = sdf.parse(item.getDate());
                    if (itemDate.equals(todayDate)) {
                        item.setLogOutTime("00:00:00");
                        item.setActivehrs("00:00:00");
                    }
                } catch (Exception e) {
                    // Skip date parsing errors
                }
            }

            return java.util.Collections.singletonMap("data", vmList);
        } catch (Exception ex) {
            result.put("StatusCode", 500);
            result.put("Message", ex.getMessage());
            return result;
        }
    }

    private Integer parseInteger(Object value) {
        if (value == null) return 0;
        if (value instanceof Integer) return (Integer) value;
        if (value instanceof Long) return ((Long) value).intValue();
        try { return Integer.valueOf(value.toString()); } catch (NumberFormatException e) { return 0; }
    }

    private int getTimeMsFromDate(Date date) {
        if (date == null) return 0;
        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        int hours = cal.get(Calendar.HOUR_OF_DAY);
        int mins = cal.get(Calendar.MINUTE);
        int secs = cal.get(Calendar.SECOND);
        return ((hours * 3600 + mins * 60 + secs) * 1000);
    }

    private String msToTimeString(int ms) {
        int totalSecs = ms / 1000;
        int hours = totalSecs / 3600;
        int mins = (totalSecs % 3600) / 60;
        int secs = totalSecs % 60;
        return String.format("%02d:%02d:%02d", hours, mins, secs);
    }

    public Map<String, Object> saveWFHAnalysis(Map<String, Object> model) {
        Map<String, Object> result = new java.util.HashMap<>();
        result.put("msg", "WFH analysis saved");
        return result;
    }

    public Map<String, Object> wfhEmpList(Map<String, Object> model) {
        Map<String, Object> result = new java.util.HashMap<>();
        result.put("msg", "WFH employee list retrieved");
        return result;
    }

    public Map<String, Object> viewScreenShots(Map<String, Object> model) {
        Map<String, Object> result = new java.util.HashMap<>();
        result.put("msg", "Screen shots retrieved");
        return result;
    }
}
