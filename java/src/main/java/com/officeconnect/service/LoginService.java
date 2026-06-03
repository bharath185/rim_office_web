package com.officeconnect.service;

import com.officeconnect.config.JwtAuthenticationFilter;
import com.officeconnect.dto.CheckAuthViewModel;
import com.officeconnect.dto.EmployeeMasterViewModel;
import com.officeconnect.dto.FRViewModel;
import com.officeconnect.dto.LoginDetailsViewModel;
import com.officeconnect.dto.LoginViewModel;
import com.officeconnect.dto.ScreenshotsViewModel;
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

    @Autowired
    private DeptMasterRepository deptMasterRepository;

    @Autowired
    private DesignationMasterRepository designationMasterRepository;

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

    public List<WFHLoginlogViewModel> getAllWFHDetails(Map<String, Object> model) {
        Integer loginId = 0;
        if (model.containsKey("LoginId") && model.get("LoginId") != null) {
            loginId = parseInteger(model.get("LoginId"));
        }
        if (loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.HOUR_OF_DAY, 0);
        cal.set(Calendar.MINUTE, 0);
        cal.set(Calendar.SECOND, 0);
        cal.set(Calendar.MILLISECOND, 0);
        Date today = cal.getTime();

        cal.set(Calendar.DAY_OF_MONTH, 1);
        Date startDate = cal.getTime();

        List<WFHLoginlog> allWfh = wfhLoginlogRepository.findByDateBetween(startDate, today).stream()
            .filter(w -> w.getIsActive() != null && w.getIsActive())
            .filter(w -> w.getIsDeleted() == null || !w.getIsDeleted())
            .filter(w -> w.getLoginTime() != null)
            .collect(Collectors.toList());

        if (allWfh.isEmpty()) {
            throw new RuntimeException("Employees Detail Not Found");
        }

        List<EmployeeMaster> activeEmployees = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(e -> e.getEmpStatus() != null && "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
            .collect(Collectors.toList());
        Map<Integer, EmployeeMaster> empMap = activeEmployees.stream()
            .collect(Collectors.toMap(EmployeeMaster::getEmpId, e -> e, (a, b) -> a));

        List<CompanyMaster> companies = companyMasterRepository.findByIsActiveAndIsDeleted(true, false);
        Map<Integer, String> compMap = companies.stream()
            .collect(Collectors.toMap(CompanyMaster::getCompId, CompanyMaster::getCompany, (a, b) -> a));

        Map<String, List<WFHLoginlog>> groupedData = allWfh.stream()
            .filter(w -> empMap.containsKey(w.getEmpId()))
            .collect(Collectors.groupingBy(w -> w.getEmpId() + "_" + new SimpleDateFormat("yyyy-MM-dd").format(w.getDate())));

        List<WFHLoginlogViewModel> resultList = new ArrayList<>();
        int defaultLogoutMs = ((18 * 3600 + 35 * 60) * 1000);

        for (Map.Entry<String, List<WFHLoginlog>> entry : groupedData.entrySet()) {
            List<WFHLoginlog> records = entry.getValue().stream()
                .sorted((a, b) -> Integer.compare(getTimeMsFromDate(a.getLoginTime()), getTimeMsFromDate(b.getLoginTime())))
                .collect(Collectors.toList());

            if (records.isEmpty()) continue;

            WFHLoginlog firstRecord = records.get(0);
            int firstLoginMs = getTimeMsFromDate(firstRecord.getLoginTime());
            int lastLogoutMs = 0;
            int totalActiveMs = 0;

            for (int i = 0; i < records.size(); i++) {
                int logInMs = getTimeMsFromDate(records.get(i).getLoginTime());
                int logOutMs;

                if (records.get(i).getLogOutTime() != null) {
                    logOutMs = getTimeMsFromDate(records.get(i).getLogOutTime());
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
            vm.setDate(firstRecord.getDate() != null ? "/Date(" + firstRecord.getDate().getTime() + ")/" : null);
            vm.setLoginTime(msToTimeSpanObject(firstLoginMs));
            vm.setLogOutTime(msToTimeSpanObject(lastLogoutMs));
            vm.setActivehrs(msToTimeSpanObject(totalActiveMs));
            vm.setIsLoggedIn(firstRecord.getIsLoggedIn());
            vm.setIsLoggedOut(firstRecord.getIsLoggedOut());
            vm.setIsActive(firstRecord.getIsActive());
            vm.setIsDeleted(firstRecord.getIsDeleted());
            vm.setCreatedBy(firstRecord.getCreatedBy());
            vm.setCreatedDate(firstRecord.getCreatedDate() != null ? "/Date(" + firstRecord.getCreatedDate().getTime() + ")/" : null);
            vm.setLastUpdatedBy(firstRecord.getLastUpdatedBy());
            vm.setLastUpdatedDate(firstRecord.getLastUpdatedDate() != null ? "/Date(" + firstRecord.getLastUpdatedDate().getTime() + ")/" : null);
            vm.setCompId(emp.getCompId());
            vm.setCompName(compMap.getOrDefault(emp.getCompId(), ""));
            vm.setDeptId(emp.getCategoryId());
            vm.setDeptName(emp.getDeptName());
            vm.setDesignationId(emp.getDesignationId());
            vm.setDesignation(emp.getDesignationName());

            if (firstRecord.getDate() != null) {
                Calendar recCal = Calendar.getInstance();
                recCal.setTime(firstRecord.getDate());
                Calendar todayCal = Calendar.getInstance();
                if (recCal.get(Calendar.YEAR) == todayCal.get(Calendar.YEAR) &&
                    recCal.get(Calendar.DAY_OF_YEAR) == todayCal.get(Calendar.DAY_OF_YEAR)) {
                    vm.setLogOutTime(msToTimeSpanObject(0));
                    vm.setActivehrs(msToTimeSpanObject(0));
                }
            }

            resultList.add(vm);
        }

        resultList.sort((a, b) -> {
            String dateA = (String) a.getDate();
            String dateB = (String) b.getDate();
            int dateCompare = dateB.compareTo(dateA);
            if (dateCompare != 0) return dateCompare;
            Integer empIdA = a.getEmpId() != null ? a.getEmpId() : 0;
            Integer empIdB = b.getEmpId() != null ? b.getEmpId() : 0;
            return empIdB.compareTo(empIdA);
        });

        return resultList;
    }

    public List<WFHLoginlogViewModel> getAllWFHAnalysis(Map<String, Object> model) {
        Integer loginId = 0;
        if (model.containsKey("LoginId") && model.get("LoginId") != null) {
            loginId = parseInteger(model.get("LoginId"));
        } else if (model.containsKey("loginId") && model.get("loginId") != null) {
            loginId = parseInteger(model.get("loginId"));
        }
        if (loginId == 0) {
            throw new RuntimeException("LoginId is Missing");
        }

        List<WFHLoginlog> wfhLogs = wfhLoginlogRepository.findByAnalysisHrIsNotNullAndIsActiveAndIsDeleted();

        if (wfhLogs == null || wfhLogs.isEmpty()) {
            throw new RuntimeException("Employees Detail Not Found");
        }

        List<EmployeeMaster> activeEmployees = employeeMasterRepository.findByIsActiveAndIsDeleted(true, false).stream()
            .filter(e -> e.getEmpStatus() != null && "ACTIVE".equalsIgnoreCase(e.getEmpStatus()))
            .collect(Collectors.toList());
        Map<Integer, EmployeeMaster> empMap = activeEmployees.stream()
            .collect(Collectors.toMap(EmployeeMaster::getEmpId, e -> e, (a, b) -> a));

        List<CompanyMaster> companies = companyMasterRepository.findByIsActiveAndIsDeleted(true, false);
        Map<Integer, String> compMap = companies.stream()
            .collect(Collectors.toMap(CompanyMaster::getCompId, CompanyMaster::getCompany, (a, b) -> a));

        List<DeptMaster> depts = deptMasterRepository.findByIsDeleted(false);
        Set<Integer> validDeptIds = depts.stream()
            .map(DeptMaster::getDeptId)
            .collect(Collectors.toSet());

        List<DesignationMaster> designations = designationMasterRepository.findByIsDeleted(false);
        Set<Integer> validDesignationIds = designations.stream()
            .map(DesignationMaster::getDesignationId)
            .collect(Collectors.toSet());

        List<WFHLoginlogViewModel> resultList = new ArrayList<>();

        for (WFHLoginlog wfh : wfhLogs) {
            EmployeeMaster emp = empMap.get(wfh.getEmpId());
            if (emp == null) continue;

            if (!validDeptIds.contains(emp.getCategoryId())) continue;
            if (!validDesignationIds.contains(emp.getDesignationId())) continue;

            String empName = (emp.getFirstName() != null ? emp.getFirstName() : "") + " " +
                (emp.getMiddleName() != null ? emp.getMiddleName() : "") + " " +
                (emp.getLastName() != null ? emp.getLastName() : "");

            WFHLoginlogViewModel vm = new WFHLoginlogViewModel();
            vm.setWfhId(wfh.getWfhId());
            vm.setLoginId(loginId);
            vm.setEmpId(wfh.getEmpId());
            vm.setEmpCode(wfh.getEmpCode());
            vm.setEmpName(empName.trim());
            vm.setIpAddress(wfh.getIpAddress());

            if (wfh.getDate() != null) {
                vm.setDate("/Date(" + wfh.getDate().getTime() + ")/");
            } else {
                vm.setDate(null);
            }

            vm.setLoginTime(toTimeSpanObject(wfh.getLoginTime()));
            vm.setLogOutTime(toTimeSpanObject(wfh.getLogOutTime()));
            vm.setActivehrs(toTimeSpanObject(wfh.getActivehrs()));
            vm.setAnalysisHr(toTimeSpanObject(wfh.getAnalysisHr()));

            vm.setIsLoggedIn(wfh.getIsLoggedIn());
            vm.setIsLoggedOut(wfh.getIsLoggedOut());
            vm.setCreatedBy(wfh.getCreatedBy());
            vm.setCreatedDate(wfh.getCreatedDate() != null ? "/Date(" + wfh.getCreatedDate().getTime() + ")/" : null);
            vm.setLastUpdatedBy(wfh.getLastUpdatedBy());
            vm.setLastUpdatedDate(wfh.getLastUpdatedDate() != null ? "/Date(" + wfh.getLastUpdatedDate().getTime() + ")/" : null);
            vm.setIsActive(wfh.getIsActive());
            vm.setIsUpdated(wfh.getIsUpdated());
            vm.setIsDeleted(wfh.getIsDeleted());

            vm.setCompId(emp.getCompId());
            vm.setCompName(compMap.getOrDefault(emp.getCompId(), ""));
            vm.setDeptId(emp.getCategoryId());
            vm.setDeptName(emp.getDeptName());
            vm.setDesignationId(emp.getDesignationId());
            vm.setDesignation(emp.getDesignationName());

            resultList.add(vm);
        }

        resultList.sort((a, b) -> {
            Integer empIdA = a.getEmpId() != null ? a.getEmpId() : 0;
            Integer empIdB = b.getEmpId() != null ? b.getEmpId() : 0;
            return empIdB.compareTo(empIdA);
        });

        return resultList;
    }

    private Map<String, Object> toTimeSpanObject(Date date) {
        if (date == null) return null;
        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        int hours = cal.get(Calendar.HOUR_OF_DAY);
        int minutes = cal.get(Calendar.MINUTE);
        int seconds = cal.get(Calendar.SECOND);
        int millis = cal.get(Calendar.MILLISECOND);

        long totalMs = (long) hours * 3600000L + (long) minutes * 60000L + (long) seconds * 1000L + millis;
        long ticks = totalMs * 10000L;
        double totalDays = totalMs / 86400000.0;
        double totalHoursDouble = totalMs / 3600000.0;
        double totalMinutes = totalMs / 60000.0;
        double totalSecondsDouble = totalMs / 1000.0;

        Map<String, Object> timeSpan = new LinkedHashMap<>();
        timeSpan.put("Hours", hours);
        timeSpan.put("Minutes", minutes);
        timeSpan.put("Seconds", seconds);
        timeSpan.put("Milliseconds", millis);
        timeSpan.put("Ticks", ticks);
        timeSpan.put("Days", (int) totalDays);
        timeSpan.put("TotalDays", totalDays);
        timeSpan.put("TotalHours", totalHoursDouble);
        timeSpan.put("TotalMilliseconds", (double) totalMs);
        timeSpan.put("TotalMinutes", totalMinutes);
        timeSpan.put("TotalSeconds", totalSecondsDouble);
        return timeSpan;
    }

    private Map<String, Object> msToTimeSpanObject(int ms) {
        int totalSecs = ms / 1000;
        int hours = totalSecs / 3600;
        int minutes = (totalSecs % 3600) / 60;
        int seconds = totalSecs % 60;
        int millis = ms % 1000;

        long ticks = (long) ms * 10000L;
        double totalDays = ms / 86400000.0;
        double totalHoursDouble = ms / 3600000.0;
        double totalMinutes = ms / 60000.0;
        double totalSecondsDouble = ms / 1000.0;

        Map<String, Object> timeSpan = new LinkedHashMap<>();
        timeSpan.put("Hours", hours);
        timeSpan.put("Minutes", minutes);
        timeSpan.put("Seconds", seconds);
        timeSpan.put("Milliseconds", millis);
        timeSpan.put("Ticks", ticks);
        timeSpan.put("Days", (int) totalDays);
        timeSpan.put("TotalDays", totalDays);
        timeSpan.put("TotalHours", totalHoursDouble);
        timeSpan.put("TotalMilliseconds", (double) ms);
        timeSpan.put("TotalMinutes", totalMinutes);
        timeSpan.put("TotalSeconds", totalSecondsDouble);
        return timeSpan;
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

            List<WFHLoginlog> allWFH = wfhLoginlogRepository.findByIsActiveAndIsDeleted(true, false).stream()
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
            int defaultLogoutMs = ((18 * 3600 + 35 * 60) * 1000);

            for (Map.Entry<String, List<WFHLoginlog>> entry : groupedData.entrySet()) {
                List<WFHLoginlog> records = entry.getValue().stream()
                    .sorted((a, b) -> Integer.compare(getTimeMsFromDate(a.getLoginTime()), getTimeMsFromDate(b.getLoginTime())))
                    .collect(Collectors.toList());

                if (records.isEmpty()) continue;

                WFHLoginlog firstRecord = records.get(0);
                int firstLoginMs = getTimeMsFromDate(firstRecord.getLoginTime());
                int lastLogoutMs = 0;
                int totalActiveMs = 0;

                for (int i = 0; i < records.size(); i++) {
                    int logInMs = getTimeMsFromDate(records.get(i).getLoginTime());
                    int logOutMs;

                    if (records.get(i).getLogOutTime() != null) {
                        logOutMs = getTimeMsFromDate(records.get(i).getLogOutTime());
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
                vm.setDate(firstRecord.getDate() != null ? "/Date(" + firstRecord.getDate().getTime() + ")/" : null);
                vm.setLoginTime(msToTimeSpanObject(firstLoginMs));
                vm.setLogOutTime(msToTimeSpanObject(lastLogoutMs));
                vm.setActivehrs(msToTimeSpanObject(totalActiveMs));
                vm.setIsLoggedIn(firstRecord.getIsLoggedIn());
                vm.setIsLoggedOut(firstRecord.getIsLoggedOut());
                vm.setIsActive(firstRecord.getIsActive());
                vm.setIsDeleted(firstRecord.getIsDeleted());
                vm.setCreatedBy(firstRecord.getCreatedBy());
                vm.setCreatedDate(firstRecord.getCreatedDate() != null ? "/Date(" + firstRecord.getCreatedDate().getTime() + ")/" : null);
                vm.setLastUpdatedBy(firstRecord.getLastUpdatedBy());
                vm.setLastUpdatedDate(firstRecord.getLastUpdatedDate() != null ? "/Date(" + firstRecord.getLastUpdatedDate().getTime() + ")/" : null);
                vm.setCompId(emp.getCompId());
                vm.setCompName(compMap.getOrDefault(emp.getCompId(), ""));
                vm.setDeptId(emp.getCategoryId());
                vm.setDeptName(emp.getDeptName());
                vm.setDesignationId(emp.getDesignationId());
                vm.setDesignation(emp.getDesignationName());

                vmList.add(vm);
            }

            vmList.sort((a, b) -> {
                String dateA = (String) a.getDate();
                String dateB = (String) b.getDate();
                int dateCompare = dateB.compareTo(dateA);
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
                    SimpleDateFormat dateOnlySdf = new SimpleDateFormat("yyyy-MM-dd");
                    vmList = vmList.stream()
                        .filter(v -> {
                            try {
                                String d = (String) v.getDate();
                                long ts = Long.parseLong(d.replaceAll("\\D", ""));
                                Date itemDate = new Date(ts);
                                String itemDateStr = dateOnlySdf.format(itemDate);
                                Date parsedItemDate = dateOnlySdf.parse(itemDateStr);
                                return !parsedItemDate.before(fDate) && !parsedItemDate.after(tDate) && v.getIsActive();
                            } catch (Exception e) { return false; }
                        })
                        .collect(Collectors.toList());
                } catch (Exception e) {
                    // skip
                }
            }

            Calendar todayCal = Calendar.getInstance();
            for (WFHLoginlogViewModel item : vmList) {
                try {
                    String d = (String) item.getDate();
                    long ts = Long.parseLong(d.replaceAll("\\D", ""));
                    Date itemDate = new Date(ts);
                    Calendar itemCal = Calendar.getInstance();
                    itemCal.setTime(itemDate);
                    if (itemCal.get(Calendar.YEAR) == todayCal.get(Calendar.YEAR) &&
                        itemCal.get(Calendar.DAY_OF_YEAR) == todayCal.get(Calendar.DAY_OF_YEAR)) {
                        item.setLogOutTime(msToTimeSpanObject(0));
                        item.setActivehrs(msToTimeSpanObject(0));
                    }
                } catch (Exception e) {
                    // skip
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
        try {
            String empCode = model.get("EmpCode") != null ? model.get("EmpCode").toString() : "";
            String dateStr = model.get("Date") != null ? model.get("Date").toString() : "";
            String analysisHrStr = model.get("AnalysisHr") != null ? model.get("AnalysisHr").toString() : "";

            if (empCode.isEmpty() || dateStr.isEmpty() || analysisHrStr.isEmpty()) {
                Map<String, Object> err = new java.util.HashMap<>();
                err.put("Message", "Employee WFH Login Details not found");
                return err;
            }

            SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");
            Date parsedDate = dateFormat.parse(dateStr);

            List<WFHLoginlog> wfhRecords = wfhLoginlogRepository
                .findByEmpCodeIgnoreCaseAndDateAndIsActiveAndIsDeleted(empCode, parsedDate, true, false);

            if (wfhRecords == null || wfhRecords.isEmpty()) {
                Map<String, Object> err = new java.util.HashMap<>();
                err.put("Message", "Employee WFH Login Details not found");
                return err;
            }

            String[] timeParts = analysisHrStr.split(":");
            int hours = Integer.parseInt(timeParts[0]);
            int minutes = Integer.parseInt(timeParts[1]);
            String[] secParts = timeParts[2].split("\\.");
            int seconds = Integer.parseInt(secParts[0]);
            int millis = secParts.length > 1 ? Integer.parseInt(secParts[1]) : 0;

            Calendar cal = Calendar.getInstance();
            cal.set(Calendar.HOUR_OF_DAY, hours);
            cal.set(Calendar.MINUTE, minutes);
            cal.set(Calendar.SECOND, seconds);
            cal.set(Calendar.MILLISECOND, millis);
            Date analysisTime = cal.getTime();

            for (WFHLoginlog record : wfhRecords) {
                record.setAnalysisHr(analysisTime);
                record.setLastUpdatedDate(new Date());
                wfhLoginlogRepository.save(record);
            }

            Map<String, Object> success = new java.util.HashMap<>();
            success.put("EmpCode", empCode);
            success.put("msg", "Analysis Hr Added");
            return success;
        } catch (Exception ex) {
            Map<String, Object> err = new java.util.HashMap<>();
            err.put("Message", "Employee WFH Login Details not found");
            return err;
        }
    }

    public EmployeeMasterViewModel wfhLogin(LoginViewModel loginUser) {
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

        List<EmployeeMaster> authorizedEmp = employeeMasterRepository.findByReportId(oldEmpId);
        if (authorizedEmp == null || authorizedEmp.isEmpty()) {
            authorizedEmp = employeeMasterRepository.findByReportId(empId);
        }

        EmployeeMasterViewModel userDetails = new EmployeeMasterViewModel();
        userDetails.setCompId(emp.getCompId());
        if (emp.getCompId() != null) {
            Optional<CompanyMaster> company = companyMasterRepository.findById(emp.getCompId());
            company.ifPresent(c -> userDetails.setCompany(c.getCompany()));
        }
        userDetails.setEmpId(emp.getEmpId());
        userDetails.setLoginId(emp.getEmpId());
        userDetails.setEmpCode(emp.getEmpCode());
        userDetails.setUserName(emp.getUserName());
        userDetails.setPassword(emp.getPassword());
        userDetails.setFirstName(emp.getFirstName());
        userDetails.setMiddleName(emp.getMiddleName());
        userDetails.setLastName(emp.getLastName());
        userDetails.setMobileNo(emp.getMobileNo());
        userDetails.setEmailId(emp.getEmailId());
        userDetails.setGender(capitalizeFirst(emp.getGender()));
        userDetails.setJoiningDate(formatDate(emp.getJoiningDate()));
        userDetails.setEmpStatus(emp.getEmpStatus());
        userDetails.setReportId(emp.getReportId());
        userDetails.setDeptId(emp.getCategoryId());
        userDetails.setDeptName(emp.getDeptName());
        userDetails.setDesignationId(emp.getDesignationId());
        userDetails.setDesignation(emp.getDesignationName());
        userDetails.setIsActive(emp.getIsActive());
        userDetails.setIsUpdated(emp.getIsUpdated());
        userDetails.setIsDeleted(emp.getIsDeleted());
        userDetails.setCreatedBy(emp.getCreatedBy());
        userDetails.setCreatedDate(formatDate(emp.getCreatedDate()));
        userDetails.setLastUpdatedBy(emp.getLastUpdatedBy());
        userDetails.setLastUpdatedDate(formatDate(emp.getLastUpdatedDate()));

        if (emp.getReportId() != null) {
            Optional<EmployeeMaster> reporter = employeeMasterRepository.findById(emp.getReportId());
            reporter.ifPresent(r -> userDetails.setReportEmpCode(r.getEmpCode()));
        }
        if (authorizedEmp != null && !authorizedEmp.isEmpty()) {
            userDetails.setAuthorised(true);
        } else {
            userDetails.setAuthorised(false);
        }

        String userName = userDetails.getUserName();
        Integer roleId = userDetails.getDesignationId();
        String roleIdStr = (roleId != null) ? String.valueOf(roleId) : "1";
        String tokenId = JwtAuthenticationFilter.encodeAuthToken(userName);
        String userAuth = JwtAuthenticationFilter.encodeToken(userName, roleIdStr);
        userDetails.setTokenId(tokenId);
        userDetails.setUserAuth(userAuth);

        // Verify password
        try {
            String storedPassword = emp.getPassword();
            boolean passwordMatch = false;
            if (storedPassword != null) {
                try {
                    byte[] decrypted = Base64.getDecoder().decode(storedPassword);
                    String decryptedPassword = new String(decrypted, StandardCharsets.UTF_16);
                    if (!decryptedPassword.trim().equals(password.trim())) {
                        decryptedPassword = new String(decrypted, StandardCharsets.UTF_16LE);
                    }
                    if (decryptedPassword.trim().equals(password.trim())) {
                        passwordMatch = true;
                    }
                } catch (Exception e) {
                    // not base64
                }
                if (!passwordMatch && storedPassword.equals(password)) {
                    passwordMatch = true;
                }
            }
            if (!passwordMatch) {
                throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Password is Mismatching\"}");
            }

            // Create session with WFH=true
            SessionMaster session = new SessionMaster();
            session.setUsername(username);
            session.setTockenId(userDetails.getTokenId());
            session.setAuthKey(userDetails.getUserAuth());
            session.setRoleId(userDetails.getDesignationId());
            session.setStatus(true);
            session.setExpired(false);
            session.setWfh(true);
            session.setIsActive(true);
            session.setIsDeleted(false);
            session.setCreatedDate(new Date());
            session.setLastUpdatedDate(new Date());
            sessionMasterRepository.save(session);

            // Create WFHLoginlog entry
            WFHLoginlog wfhLog = new WFHLoginlog();
            wfhLog.setEmpId(emp.getEmpId());
            wfhLog.setEmpCode(username);
            wfhLog.setIpAddress(loginUser.getIpAddress());
            Date now = new Date();
            Calendar nowCal = Calendar.getInstance();
            nowCal.setTime(now);
            nowCal.set(Calendar.HOUR_OF_DAY, 0);
            nowCal.set(Calendar.MINUTE, 0);
            nowCal.set(Calendar.SECOND, 0);
            nowCal.set(Calendar.MILLISECOND, 0);
            wfhLog.setDate(nowCal.getTime());
            wfhLog.setLoginTime(now);
            wfhLog.setIsLoggedIn(true);
            wfhLog.setIsLoggedOut(false);
            wfhLog.setIsActive(true);
            wfhLog.setIsUpdated(false);
            wfhLog.setIsDeleted(false);
            wfhLog.setCreatedBy(emp.getEmpId());
            wfhLog.setCreatedDate(now);
            wfhLog.setLastUpdatedBy(emp.getEmpId());
            wfhLog.setLastUpdatedDate(now);
            wfhLoginlogRepository.save(wfhLog);

            return userDetails;
        } catch (RuntimeException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"Password is Mismatching\"}");
        }
    }

    public EmployeeMasterViewModel wfhLogout(LoginViewModel loginUser) {
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

        Optional<SessionMaster> sessionOpt = sessionMasterRepository.findActiveSessionByUsernameAndToken(username, token);
        if (sessionOpt.isPresent()) {
            SessionMaster session = sessionOpt.get();
            if (!Boolean.TRUE.equals(session.getWfh())) {
                throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User Authorization is Failed\"}");
            }
            Optional<SessionMaster> sessionWithAuth = sessionMasterRepository.findActiveSessionByUsernameTokenAuthKeyAndRole(username, token, authKey, roleId);
            if (sessionWithAuth.isPresent()) {
                SessionMaster s = sessionWithAuth.get();
                s.setExpired(true);
                s.setLastUpdatedDate(new Date());
                sessionMasterRepository.save(s);

                // Update WFHLoginlog for today
                Date now = new Date();
                Calendar cal = Calendar.getInstance();
                cal.setTime(now);
                cal.set(Calendar.HOUR_OF_DAY, 0);
                cal.set(Calendar.MINUTE, 0);
                cal.set(Calendar.SECOND, 0);
                cal.set(Calendar.MILLISECOND, 0);
                Date today = cal.getTime();

                List<WFHLoginlog> wfhLogs = wfhLoginlogRepository.findTodayActiveLogin(username, emp.getEmpId(), today);
                if (wfhLogs != null && !wfhLogs.isEmpty()) {
                    WFHLoginlog wfhLog = wfhLogs.get(0);
                    wfhLog.setLogOutTime(now);
                    wfhLog.setIsLoggedOut(true);

                    if (wfhLog.getLoginTime() != null) {
                        long diffMs = now.getTime() - wfhLog.getLoginTime().getTime();
                        Calendar timeCal = Calendar.getInstance();
                        timeCal.setTimeInMillis(diffMs);
                        // Create a time-only Date from the diff
                        Calendar activeCal = Calendar.getInstance();
                        activeCal.set(Calendar.HOUR_OF_DAY, timeCal.get(Calendar.HOUR_OF_DAY));
                        activeCal.set(Calendar.MINUTE, timeCal.get(Calendar.MINUTE));
                        activeCal.set(Calendar.SECOND, timeCal.get(Calendar.SECOND));
                        activeCal.set(Calendar.MILLISECOND, timeCal.get(Calendar.MILLISECOND));
                        wfhLog.setActivehrs(activeCal.getTime());
                    }

                    wfhLog.setIsUpdated(true);
                    wfhLog.setLastUpdatedBy(emp.getEmpId());
                    wfhLog.setLastUpdatedDate(now);
                    wfhLoginlogRepository.save(wfhLog);
                }
            } else {
                throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User Authorization is Failed\"}");
            }
        } else {
            throw new RuntimeException("{\"StatusCode\":404,\"Message\":\"User Token is Expired\"}");
        }

        return userDetails;
    }

    public List<ScreenshotsViewModel> wfhEmpList(Map<String, Object> model) {
        String basePath = "C:\\Users\\rim0972\\Documents\\office_web\\java\\Uploads\\Images\\Screenshot\\";
        String currentMonth = new SimpleDateFormat("MMMM", Locale.ENGLISH).format(new Date());
        String empPath = basePath + currentMonth;

        java.io.File dir = new java.io.File(empPath);
        if (!dir.exists() || !dir.isDirectory()) {
            return new ArrayList<>();
        }

        java.io.File[] empDirs = dir.listFiles(java.io.File::isDirectory);
        if (empDirs == null) return new ArrayList<>();

        List<ScreenshotsViewModel> result = new ArrayList<>();
        for (java.io.File f : empDirs) {
            ScreenshotsViewModel vm = new ScreenshotsViewModel();
            vm.setEmpCode(f.getName());
            result.add(vm);
        }
        return result;
    }

    public Object viewScreenShots(Map<String, Object> model) {
        String empCode = model.get("EmpCode") != null ? model.get("EmpCode").toString() : "";
        String dateStr = model.get("Date") != null ? model.get("Date").toString() : "";

        if (empCode.isEmpty()) {
            throw new RuntimeException("Employee code is required.");
        }

        String basePath = "C:\\Users\\rim0972\\Documents\\office_web\\java\\Uploads\\Images\\Screenshot\\";
        String currentMonth = new SimpleDateFormat("MMMM", Locale.ENGLISH).format(new Date());
        String empPath = basePath + currentMonth + "\\" + empCode;

        java.io.File empDir = new java.io.File(empPath);
        if (!empDir.exists() || !empDir.isDirectory()) {
            throw new RuntimeException("Path not found: " + empPath);
        }

        // If date not provided, return list of date folders
        if (dateStr.isEmpty()) {
            java.io.File[] dateDirs = empDir.listFiles(java.io.File::isDirectory);
            List<ScreenshotsViewModel> folders = new ArrayList<>();
            if (dateDirs != null) {
                for (java.io.File f : dateDirs) {
                    ScreenshotsViewModel vm = new ScreenshotsViewModel();
                    vm.setEmpCode(empCode);
                    vm.setDate(f.getName());
                    folders.add(vm);
                }
            }
            return folders;
        }

        // If date provided, return ZIP
        String dateFolderPath = empPath + "\\" + dateStr;
        java.io.File dateDir = new java.io.File(dateFolderPath);
        if (!dateDir.exists() || !dateDir.isDirectory()) {
            throw new RuntimeException("Date folder not found.");
        }

        try {
            java.io.ByteArrayOutputStream baos = new java.io.ByteArrayOutputStream();
            try (java.util.zip.ZipOutputStream zos = new java.util.zip.ZipOutputStream(baos)) {
                java.io.File[] files = dateDir.listFiles();
                if (files != null) {
                    byte[] buffer = new byte[4096];
                    for (java.io.File file : files) {
                        if (file.isFile()) {
                            java.util.zip.ZipEntry entry = new java.util.zip.ZipEntry(file.getName());
                            zos.putNextEntry(entry);
                            try (java.io.FileInputStream fis = new java.io.FileInputStream(file)) {
                                int len;
                                while ((len = fis.read(buffer)) > 0) {
                                    zos.write(buffer, 0, len);
                                }
                            }
                            zos.closeEntry();
                        }
                    }
                }
            }

            String zipFileName = empCode + "_" + dateStr + ".zip";
            byte[] zipBytes = baos.toByteArray();

            Map<String, Object> fileResponse = new LinkedHashMap<>();
            fileResponse.put("fileName", zipFileName);
            fileResponse.put("fileBytes", zipBytes);
            fileResponse.put("contentType", "application/zip");
            return fileResponse;
        } catch (Exception ex) {
            throw new RuntimeException("Failed to create zip file: " + ex.getMessage());
        }
    }
}
