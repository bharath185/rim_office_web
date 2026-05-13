using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Security.Principal;
using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using OfficeConnect_Web.Controllers;
using System.Security.Cryptography;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OfficeConnect_Web.Models
{
    public class LoginModel
    {
        //DbContext DB = new DbContext(ClsDatabase.connecttodb());
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();
        ClsAuthorization Auth = new ClsAuthorization();


        byte[] _decryted;
        string pwd;

        public EmployeeMasterViewModel CheckLogin(LoginViewModel LoginUser)
        {
            try
            {
                string username = LoginUser.UserName;
                string password = LoginUser.Password;
                string msg = "";

                var Empdetails = (from user in DB.EmployeeMasters
                                  where (user.UserName).ToUpper() == (username).ToUpper() && user.IsActive == true && user.IsDeleted == false
                                  select user).ToList();

                if (Empdetails.Count() != 0)
                {
                    int emp = Empdetails[0].EmpId;
                    int? OldEmp_ID = Empdetails[0].OldEmp_ID;
                    int? leId = Empdetails[0].LEId;

                    var AuthorisedEmp = (from user in DB.EmployeeMasters
                                         where user.ReportId == OldEmp_ID && user.EmpCode.Contains("3DCAD-") && user.IsActive == true && user.IsDeleted == false
                                         select user).ToList();
                    if (leId == 1)
                    {
                        if (AuthorisedEmp.Count() == 0)
                        {
                            AuthorisedEmp = (from user in DB.EmployeeMasters
                                             where user.ReportId == OldEmp_ID && user.EmpCode.Contains("3DCADVS-") && user.IsActive == true && user.IsDeleted == false
                                             select user).ToList();

                            if (AuthorisedEmp.Count() == 0)
                            {
                                AuthorisedEmp = (from user in DB.EmployeeMasters
                                                 where user.ReportId == OldEmp_ID && user.EmpCode.Contains("3DCADPU-") && user.IsActive == true && user.IsDeleted == false
                                                 select user).ToList();

                                if (AuthorisedEmp.Count() == 0)
                                {
                                    AuthorisedEmp = (from user in DB.EmployeeMasters
                                                     where user.ReportId == emp && user.IsActive == true && user.IsDeleted == false
                                                     select user).ToList();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (AuthorisedEmp.Count() == 0)
                        {
                            AuthorisedEmp = (from user in DB.EmployeeMasters
                                             where user.ReportId == emp && user.EmpCode.Contains("RIM-") && user.IsActive == true && user.IsDeleted == false
                                             select user).ToList();
                        }
                    }

                    EmployeeMasterViewModel userdetails = new EmployeeMasterViewModel();
                    userdetails.CompId = Empdetails[0].CompId;
                    userdetails.Company = DB.CompanyMasters.Where(x => x.CompId == userdetails.CompId).Select(x => x.Company).FirstOrDefault();
                    userdetails.DeptId = Empdetails[0].CategoryId;
                    //userdetails.DeptName = DB.DeptMasters.Where(x => x.DeptId == userdetails.DeptId).Select(x => x.DeptName).FirstOrDefault();
                    userdetails.DeptName = Empdetails[0].DeptName;
                    userdetails.DesignationId = Empdetails[0].DesignationId;
                    //userdetails.Designation = DB.DesignationMasters.Where(x => x.DesignationId == userdetails.DesignationId).Select(x => x.Designation).FirstOrDefault();
                    userdetails.Designation = Empdetails[0].DesignationName;
                    userdetails.EmpId = Empdetails[0].EmpId;
                    userdetails.LoginId = Empdetails[0].EmpId;
                    userdetails.EmpCode = Empdetails[0].EmpCode;
                    userdetails.UserName = Empdetails[0].UserName;
                    userdetails.Password = Empdetails[0].Password;
                    userdetails.FirstName = Empdetails[0].FirstName;
                    userdetails.MiddleName = Empdetails[0].MiddleName;
                    userdetails.LastName = Empdetails[0].LastName;
                    userdetails.MobileNo = Empdetails[0].MobileNo;
                    userdetails.EmailId = Empdetails[0].EmailId;
                    userdetails.Gender = Empdetails[0].Gender;
                    userdetails.JoiningDate = Empdetails[0].JoiningDate;
                    userdetails.EmpStatus = Empdetails[0].EmpStatus;
                    userdetails.ReportId = Empdetails[0].ReportId;
                    userdetails.ReportEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == userdetails.ReportId).Select(x => x.EmpCode).FirstOrDefault();
                    if (AuthorisedEmp.Count > 0) { userdetails.Authorised = true; }
                    else { userdetails.Authorised = false; }
                    userdetails.IsActive = Empdetails[0].IsActive;
                    userdetails.IsUpdated = Empdetails[0].IsUpdated;
                    userdetails.IsDeleted = Empdetails[0].IsDeleted;
                    userdetails.CreatedBy = Empdetails[0].CreatedBy;
                    userdetails.CreatedDate = Empdetails[0].CreatedDate;
                    userdetails.LastUpdatedBy = Empdetails[0].LastUpdatedBy;
                    userdetails.LastUpdatedDate = Empdetails[0].LastUpdatedDate;
                    userdetails.CPwd = false;

                    // ✅ Save to session
                    System.Web.HttpContext.Current.Session["EmpId"] = Empdetails[0].EmpId;

                    var pass = (from cp in DB.CPwdManagements
                                where cp.EmpCode.ToUpper() == (username).ToUpper() && cp.CPwd == true
                                && cp.Expired == false && cp.IsActive == true && cp.IsDeleted == false
                                select cp).ToList();

                    if (pass != null)
                    {
                        if (pass.Count() != 0)
                        {
                            userdetails.CPwd = true;
                        }
                    }

                    userdetails.TokenId = ObjAuth.GetJwt(userdetails.UserName);
                    userdetails.UserAuth = Auth.GetAuthorization(userdetails.UserName, Convert.ToString(userdetails.DesignationId));

                    if (userdetails == null)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User is Not Found");
                    }
                    else
                    {
                        _decryted = Convert.FromBase64String(userdetails.Password);
                        pwd = System.Text.Encoding.Unicode.GetString(_decryted, 0, _decryted.ToArray().Length);
                        if (pwd.Trim() == password.Trim())
                        {
                            SessionMaster sm = new SessionMaster();
                            sm.Username = username;
                            sm.TockenId = userdetails.TokenId;
                            sm.AuthKey = userdetails.UserAuth;
                            sm.RoleId = userdetails.DesignationId;
                            sm.Status = true;
                            sm.Expired = false;
                            sm.WFH = false;
                            sm.IsActive = true;
                            sm.IsDeleted = false;
                            sm.CreatedDate = DateTime.Now;
                            sm.LastUpdatedDate = DateTime.Now;
                            DB.SessionMasters.Add(sm);
                            DB.SaveChanges();
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Password is Mismatching");
                        }
                        return userdetails;
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public EmployeeMasterViewModel CheckLogOut(LoginViewModel LoginUser)
        {
            try
            {
                string username = LoginUser.UserName;
                string token = LoginUser.TokenId;
                string authKey = LoginUser.AuthKey;
                int roleid = LoginUser.RoleId;
                string msg = "";

                var userdetails = (from user in DB.EmployeeMasters
                                   where (user.UserName).ToUpper() == (username).ToUpper() && user.IsActive == true
                                   select new EmployeeMasterViewModel
                                   {
                                       EmpId = user.EmpId,
                                       CompId = 0,
                                       CategoryId = 0,
                                       DesignationId = 0,
                                       EmpCode = user.EmpCode,
                                       UserName = user.UserName,
                                       EmpStatus = user.EmpStatus,
                                       TokenId = "Expired",
                                   }).FirstOrDefault();

                if (userdetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "User is Not Found");
                }
                else
                {
                    var updateSessionUser = (from suser in DB.SessionMasters
                                             where suser.Username.ToUpper() == username.ToUpper() && suser.TockenId == token
                                            && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false
                                             orderby suser.CreatedDate descending
                                             select suser).FirstOrDefault();

                    if (updateSessionUser != null)
                    {
                        var updateSessionUser1 = (from suser in DB.SessionMasters
                                                  where suser.Username.ToUpper() == username.ToUpper() && suser.TockenId == token && suser.AuthKey == authKey && suser.RoleId == roleid
                                                  && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false
                                                  orderby suser.CreatedDate descending
                                                  select suser).FirstOrDefault();

                        if (updateSessionUser1 != null)
                        {
                            updateSessionUser.Expired = true;
                            updateSessionUser.IsActive = true;
                            updateSessionUser.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            updateSessionUser1.Expired = true;
                            updateSessionUser1.IsActive = true;
                            updateSessionUser1.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "User Authorization is Failed");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User Token is Expired");
                    }

                    return userdetails;
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeMasterViewModel WFHCheckLogin(WFHLoginViewModel LoginUser)
        {
            try
            {
                string username = LoginUser.UserName;
                string password = LoginUser.Password;
                string msg = "";

                var Empdetails = (from user in DB.EmployeeMasters
                                  where (user.UserName).ToUpper() == (username).ToUpper() && user.IsActive == true && user.IsDeleted == false
                                  select user).ToList();

                if (Empdetails.Count() != 0)
                {
                    int emp = Empdetails[0].EmpId;
                    int? OldEmp_ID = Empdetails[0].OldEmp_ID;

                    var AuthorisedEmp = (from user in DB.EmployeeMasters
                                         where user.ReportId == OldEmp_ID && user.IsActive == true && user.IsDeleted == false
                                         select user).ToList();

                    EmployeeMasterViewModel userdetails = new EmployeeMasterViewModel();
                    userdetails.CompId = Empdetails[0].CompId;
                    userdetails.Company = DB.CompanyMasters.Where(x => x.CompId == userdetails.CompId).Select(x => x.Company).FirstOrDefault();
                    userdetails.DeptId = Empdetails[0].CategoryId;
                    //userdetails.DeptName = DB.DeptMasters.Where(x => x.DeptId == userdetails.DeptId).Select(x => x.DeptName).FirstOrDefault();
                    userdetails.DeptName = Empdetails[0].DeptName;
                    userdetails.DesignationId = Empdetails[0].DesignationId;
                    //userdetails.Designation = DB.DesignationMasters.Where(x => x.DesignationId == userdetails.DesignationId).Select(x => x.Designation).FirstOrDefault();
                    userdetails.Designation = Empdetails[0].DesignationName;
                    userdetails.EmpId = Empdetails[0].EmpId;
                    userdetails.LoginId = Empdetails[0].EmpId;
                    userdetails.EmpCode = Empdetails[0].EmpCode;
                    userdetails.UserName = Empdetails[0].UserName;
                    userdetails.Password = Empdetails[0].Password;
                    userdetails.FirstName = Empdetails[0].FirstName;
                    userdetails.MiddleName = Empdetails[0].MiddleName;
                    userdetails.LastName = Empdetails[0].LastName;
                    userdetails.MobileNo = Empdetails[0].MobileNo;
                    userdetails.EmailId = Empdetails[0].EmailId;
                    userdetails.Gender = Empdetails[0].Gender;
                    userdetails.JoiningDate = Empdetails[0].JoiningDate;
                    userdetails.EmpStatus = Empdetails[0].EmpStatus;
                    userdetails.ReportId = Empdetails[0].ReportId;
                    userdetails.ReportEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == userdetails.ReportId).Select(x => x.EmpCode).FirstOrDefault();
                    if (AuthorisedEmp.Count > 0) { userdetails.Authorised = true; }
                    else { userdetails.Authorised = false; }
                    userdetails.IsActive = Empdetails[0].IsActive;
                    userdetails.IsUpdated = Empdetails[0].IsUpdated;
                    userdetails.IsDeleted = Empdetails[0].IsDeleted;
                    userdetails.CreatedBy = Empdetails[0].CreatedBy;
                    userdetails.CreatedDate = Empdetails[0].CreatedDate;
                    userdetails.LastUpdatedBy = Empdetails[0].LastUpdatedBy;
                    userdetails.LastUpdatedDate = Empdetails[0].LastUpdatedDate;


                    userdetails.TokenId = ObjAuth.GetJwt(userdetails.UserName);
                    userdetails.UserAuth = Auth.GetAuthorization(userdetails.UserName, Convert.ToString(userdetails.DesignationId));

                    if (userdetails == null)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User is Not Found");
                    }
                    else
                    {
                        _decryted = Convert.FromBase64String(userdetails.Password);
                        pwd = System.Text.Encoding.Unicode.GetString(_decryted, 0, _decryted.ToArray().Length);
                        if (pwd.Trim() == password.Trim())
                        {
                            SessionMaster sm = new SessionMaster();
                            sm.Username = username;
                            sm.TockenId = userdetails.TokenId;
                            sm.AuthKey = userdetails.UserAuth;
                            sm.RoleId = userdetails.DesignationId;
                            sm.Status = true;
                            sm.Expired = false;
                            sm.WFH = true;
                            sm.IsActive = true;
                            sm.IsDeleted = false;
                            sm.CreatedDate = DateTime.Now;
                            sm.LastUpdatedDate = DateTime.Now;
                            DB.SessionMasters.Add(sm);
                            DB.SaveChanges();


                            WFHLoginlog wfh = new WFHLoginlog();
                            wfh.EmpId = Empdetails[0].EmpId;
                            wfh.EmpCode = username;
                            wfh.IPAddress = LoginUser.IPAddress;
                            DateTime today = DateTime.Now; 
                            wfh.Date = today.Date;
                            wfh.LoginTime = today.TimeOfDay;
                            //wfh.LogOutTime = today.TimeOfDay;
                            //wfh.Activehrs = today.TimeOfDay;
                            wfh.IsLoggedIn = true;
                            wfh.IsLoggedOut = false;
                            wfh.IsActive = true;
                            wfh.IsUpdated = false;
                            wfh.IsDeleted = false;
                            wfh.CreatedBy = Empdetails[0].EmpId;
                            wfh.CreatedDate = DateTime.Now;
                            wfh.LastUpdatedBy = Empdetails[0].EmpId;
                            wfh.LastUpdatedDate = DateTime.Now;
                            DB.WFHLoginlogs.Add(wfh);
                            DB.SaveChanges();
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Password is Mismatching");
                        }
                        return userdetails;
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public EmployeeMasterViewModel WFHCheckLogOut(WFHLoginViewModel LoginUser)
        {
            try
            {
                string username = LoginUser.UserName;
                string token = LoginUser.TokenId;
                string authKey = LoginUser.AuthKey;
                int roleid = LoginUser.RoleId;
                string msg = "";

                var userdetails = (from user in DB.EmployeeMasters
                                   where (user.UserName).ToUpper() == (username).ToUpper() && user.IsActive == true
                                   select new EmployeeMasterViewModel
                                   {
                                       EmpId = user.EmpId,
                                       CompId = 0,
                                       CategoryId = 0,
                                       DesignationId = 0,
                                       EmpCode = user.EmpCode,
                                       UserName = user.UserName,
                                       EmpStatus = user.EmpStatus,
                                       TokenId = "Expired",
                                   }).FirstOrDefault();

                if (userdetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "User is Not Found");
                }
                else
                {
                    var updateSessionUser = (from suser in DB.SessionMasters
                                             where suser.Username.ToUpper() == username.ToUpper() && suser.TockenId == token
                                            && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false && suser.WFH == true
                                             orderby suser.CreatedDate descending
                                             select suser).FirstOrDefault();

                    if (updateSessionUser != null)
                    {
                        var updateSessionUser1 = (from suser in DB.SessionMasters
                                                  where suser.Username.ToUpper() == username.ToUpper() && suser.TockenId == token && suser.AuthKey == authKey && suser.RoleId == roleid
                                                  && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false && suser.WFH == true
                                                  orderby suser.CreatedDate descending
                                                  select suser).FirstOrDefault();

                        DateTime today = DateTime.Now;


                        var wfhdetails = (from wfhd in DB.WFHLoginlogs
                                          where wfhd.EmpCode.ToUpper() == username.ToUpper() && wfhd.EmpId == userdetails.EmpId && wfhd.IsLoggedIn == true && wfhd.IsLoggedOut == false
                                          && wfhd.Date == today.Date && wfhd.IsActive == true && wfhd.IsDeleted == false
                                          orderby wfhd.CreatedDate descending
                                          select wfhd).FirstOrDefault();

                        if (updateSessionUser1 != null)
                        {
                            updateSessionUser.Expired = true;
                            updateSessionUser.IsActive = true;
                            updateSessionUser.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            updateSessionUser1.Expired = true;
                            updateSessionUser1.IsActive = true;
                            updateSessionUser1.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            wfhdetails.LogOutTime = today.TimeOfDay;
                            wfhdetails.IsLoggedOut = true;
                            wfhdetails.Activehrs = (wfhdetails.LogOutTime - wfhdetails.LoginTime);
                            wfhdetails.IsUpdated = true;
                            wfhdetails.LastUpdatedBy = userdetails.EmpId;
                            wfhdetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "User Authorization is Failed");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User Token is Expired");
                    }

                    return userdetails;
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public CheckAuthViewModel CheckAuth(LoginViewModel LoginUser)
        {
            try
            {
                string username = LoginUser.UserName;
                string msg = "";

                var Empdetails = (from user in DB.EmployeeMasters
                                  where (user.UserName).ToUpper() == (username).ToUpper() && user.IsActive == true && user.IsDeleted == false
                                  select user).ToList();

                if (Empdetails.Count() != 0)
                {
                    CheckAuthViewModel chvm = new CheckAuthViewModel();
                    chvm.UserName = username;
                    chvm.TokenId = "Success";
                    chvm.AuthKey = "Success";

                    return chvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        //public EmployeeMasterViewModel ForgetPassword(FRViewModel model)
        //{
        //    try
        //    {
        //        // Find the user based on the provided username or email
        //        var user = (from emp in DB.EmployeeMasters
        //                    where (emp.UserName.ToUpper() == model.UserName.ToUpper() || emp.EmailId.ToUpper() == model.Email.ToUpper())
        //                          && emp.IsActive == true && emp.IsDeleted == false
        //                    select emp).FirstOrDefault();

        //        if (user == null)
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "User not found");
        //        }


        //        return userdetails;
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}
        public FRViewModel ForgetPassword(FRViewModel model)
        {
            try
            {
                string msg = "";
                string username = (model.UserName != "") ? model.UserName : "";
                string emailid = (model.Email != "") ? model.Email : "";

                if (username != "")
                {
                    var Empdetails = (from user in DB.EmployeeMasters
                                      where (user.UserName).ToUpper() == (username).ToUpper()
                                      && user.IsActive == true && user.IsDeleted == false
                                      select user).FirstOrDefault();

                    var Empdetails1 = (from user in DB.EmployeeMasters
                                      where (user.UserName).ToUpper() == (username).ToUpper() && user.EmailId == model.Email
                                      && user.IsActive == true && user.IsDeleted == false
                                      select user).FirstOrDefault();

                    if (Empdetails != null)
                    {
                        if (Empdetails1 != null)
                        {
                            string InviteCode = GenerateSecureOTP();

                            string subject = "Password Reset Request";

                            string body = $@"
                                <p>Dear {username},</p>
                                <p>We received a request to reset your password. Please use the OTP (One-Time Pa/'>C'ssword) below to proceed with resetting your password:</p>
                                <p><strong>OTP: </strong> {InviteCode}</p>
                                <p>This OTP is valid for [Time Duration, e.g., 10 minutes] and can only be used once. If you did not request a password reset, please ignore this email or contact our support team.</p>
                                <p>Thank you,</p>
                                <p>Best regards,</p>
                                <p>3DCAD-Support Team</p>";


                            //SendEmail(model.OMail, subject, body);

                            FPwdManagement fpm = new FPwdManagement();
                            fpm.EmpId = Empdetails.EmpId;
                            fpm.EmpCode = username;
                            fpm.Otp = InviteCode;
                            fpm.Expired = false;
                            fpm.CreatedBy = Empdetails.EmpId;
                            fpm.CreatedDate = DateTime.Now;
                            fpm.LastUpdatedBy = Empdetails.EmpId;
                            fpm.LastUpdatedDate = DateTime.Now;
                            fpm.IsActive = true;
                            fpm.IsUpdated = false;
                            fpm.IsDeleted = false;
                            DB.FPwdManagements.Add(fpm);
                            DB.SaveChanges();

                            FRViewModel frvm = new FRViewModel();
                            frvm.msg = "OTP Send successfully";
                            frvm.Otp = InviteCode;
                            frvm.UserName = username;

                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmail(emailid, subject, body));
                            //SendEmailWithQRCode(model.OMail, subject, body, QRCode);

                            return frvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "EmailId is Mismatching");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User is not found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        private string GenerateSecureOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[4];
                rng.GetBytes(byteArray);
                int randomNumber = BitConverter.ToInt32(byteArray, 0);
                randomNumber = Math.Abs(randomNumber % 1000000);
                return randomNumber.ToString("D6");
            }
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public FRViewModel FPwdVerify(FRViewModel model)
        {
            try
            {
                string msg = "";
                string username = (model.UserName != "") ? model.UserName : "";
                string emailid = (model.Email != "") ? model.Email : "";
                string otp = (model.Otp != "") ? model.Otp : "";

                if (username != "")
                {
                    var FPwddetails = (from user in DB.FPwdManagements
                                       where (user.EmpCode).ToUpper() == (username).ToUpper() && user.Otp == otp && user.Expired == false
                                       && user.IsActive == true && user.IsDeleted == false
                                       select user).FirstOrDefault();

                    if (FPwddetails != null)
                    {
                        var FPwddetails1 = (from user in DB.FPwdManagements
                                           where (user.EmpCode).ToUpper() == (username).ToUpper() && user.Otp == otp && user.Expired == false
                                           && user.IsActive == true && user.IsDeleted == false
                                           select user).ToList();

                        for (int i = 0; i < FPwddetails1.Count(); i++)
                        {
                            FPwddetails1[i].Expired = true;
                            FPwddetails1[i].IsUpdated = true;
                            FPwddetails1[i].LastUpdatedBy = FPwddetails.EmpId;
                            FPwddetails1[i].LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        var Empdetails = (from user in DB.EmployeeMasters
                                          where (user.UserName).ToUpper() == (username).ToUpper() && user.IsActive == true && user.IsDeleted == false
                                          select user).FirstOrDefault();

                        FRViewModel frvm = new FRViewModel();
                        frvm.msg = "OTP Verified";
                        frvm.UserName = username;
                        frvm.EmpId = Empdetails.EmpId;
                        frvm.EmpCode = Empdetails.EmpCode;

                        return frvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "OTP is Invalid");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PassHistoryManagementViewModel ChangePassword(PassHistoryManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string EmpCode = (model.EmpCode != "") ? model.EmpCode : "";
                string OldPassword = (model.OldPassword != "") ? model.OldPassword : "";
                bool CNPwd = false;



                if (EmpCode != "")
                {
                    var PassHistory = (from user in DB.PassHistoryManagements
                                       where (user.EmpCode).ToUpper() == (EmpCode).ToUpper() && user.Expired == false
                                       && user.IsActive == true && user.IsDeleted == false
                                       select user).ToList();

                    for (int i = 0; i < PassHistory.Count(); i++)
                    {
                        PassHistory[i].Expired = true;
                        DB.SaveChanges();
                    }

                    if (model.FPwd == true)
                    {
                        var Empdetails = (from user in DB.EmployeeMasters
                                          where (user.UserName).ToUpper() == (EmpCode).ToUpper() && user.IsActive == true && user.IsDeleted == false
                                          select user).FirstOrDefault();

                        if (Empdetails != null)
                        {
                            var PassHistory1 = (from user in DB.PassHistoryManagements
                                               where (user.EmpCode).ToUpper() == (EmpCode).ToUpper()
                                               && user.IsActive == true && user.IsDeleted == false
                                               select user).ToList();

                            for (int j = 0; j < PassHistory1.Count(); j++)
                            {
                                if (PassHistory1[j].NewPassword == model.NewPassword)
                                {
                                    CNPwd = true;
                                }
                            }

                            if (CNPwd == true)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "New Password and Old Password is Same");
                            }
                            else
                            {
                                string pwd = model.NewPassword;
                                byte[] _encryted;
                                _encryted = System.Text.Encoding.Unicode.GetBytes(pwd);
                                string NewPassword = Convert.ToBase64String(_encryted);
                                Empdetails.Password = NewPassword;
                                Empdetails.IsUpdated = true;
                                Empdetails.LastUpdatedBy = EmpId;
                                Empdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                PassHistoryManagement phm = new PassHistoryManagement();
                                phm.EmpId = EmpId;
                                phm.EmpCode = EmpCode;
                                phm.OldPassword = model.OldPassword;
                                phm.NewPassword = model.NewPassword;
                                phm.FPwd = true;
                                phm.CPwd = false;
                                phm.Expired = false;
                                phm.CreatedBy = EmpId;
                                phm.CreatedDate = DateTime.Now;
                                phm.IsActive = true;
                                phm.IsUpdated = false;
                                phm.IsDeleted = false;
                                DB.PassHistoryManagements.Add(phm);
                                DB.SaveChanges();

                                PassHistoryManagementViewModel phmvm = new PassHistoryManagementViewModel();
                                phmvm.msg = "Password Changed";
                                phmvm.EmpCode = EmpCode;

                                return phmvm;
                            }
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "User details is Mismatching");
                        }
                    }
                    else if (model.CPwd == true)
                    {
                        var Empdetails = (from user in DB.EmployeeMasters
                                          where (user.UserName).ToUpper() == (EmpCode).ToUpper() && user.IsActive == true && user.IsDeleted == false
                                          select user).FirstOrDefault();

                        if (Empdetails != null)
                        {
                            string dcyptOldPwd = Empdetails.Password;
                            byte[] _encryted1;
                            _encryted1 = System.Text.Encoding.Unicode.GetBytes(model.OldPassword);
                            string oldPassword = Convert.ToBase64String(_encryted1);

                            if (dcyptOldPwd == oldPassword)
                            {
                                var CPwdDetails = (from user in DB.CPwdManagements
                                                   where (user.EmpCode).ToUpper() == (EmpCode).ToUpper() && user.CPwd == true && user.Expired == false
                                                   && user.IsActive == true && user.IsDeleted == false
                                                   select user).ToList();

                                //if (CPwdDetails.Count() > 0)
                                //{
                                    
                                //}
                                //else
                                //{
                                //    throw new CustomApiException(HttpStatusCode.NotFound, "Change password req is not there");
                                //}
                                var PassHistory1 = (from user in DB.PassHistoryManagements
                                                    where (user.EmpCode).ToUpper() == (EmpCode).ToUpper()
                                                    && user.IsActive == true && user.IsDeleted == false
                                                    select user).ToList();

                                for (int j = 0; j < PassHistory1.Count(); j++)
                                {
                                    if (PassHistory1[j].NewPassword == model.NewPassword)
                                    {
                                        CNPwd = true;
                                    }
                                }

                                if (CNPwd == true)
                                {
                                    throw new CustomApiException(HttpStatusCode.NotFound, "New Password and Old Password is Same");
                                }
                                else
                                {
                                    string pwd = model.NewPassword;
                                    byte[] _encryted;
                                    _encryted = System.Text.Encoding.Unicode.GetBytes(pwd);
                                    string NewPassword = Convert.ToBase64String(_encryted);
                                    Empdetails.Password = NewPassword;
                                    Empdetails.IsUpdated = true;
                                    Empdetails.LastUpdatedBy = EmpId;
                                    Empdetails.LastUpdatedDate = DateTime.Now;
                                    DB.SaveChanges();

                                    PassHistoryManagement phm = new PassHistoryManagement();
                                    phm.EmpId = EmpId;
                                    phm.EmpCode = EmpCode;
                                    phm.OldPassword = model.OldPassword;
                                    phm.NewPassword = model.NewPassword;
                                    phm.CPwd = true;
                                    phm.FPwd = false;
                                    phm.Expired = false;
                                    phm.CreatedBy = EmpId;
                                    phm.CreatedDate = DateTime.Now;
                                    phm.IsActive = true;
                                    phm.IsUpdated = false;
                                    phm.IsDeleted = false;
                                    DB.PassHistoryManagements.Add(phm);
                                    DB.SaveChanges();

                                    for (int j = 0; j < CPwdDetails.Count(); j++)
                                    {
                                        CPwdDetails[j].Expired = true;
                                        DB.SaveChanges();
                                    }

                                    PassHistoryManagementViewModel phmvm = new PassHistoryManagementViewModel();
                                    phmvm.msg = "Password Changed";
                                    phmvm.EmpCode = EmpCode;

                                    return phmvm;
                                }
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Old password is Mismatching");
                            }
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "User details is Mismatching");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Password Type is Mismatching");
                    }
                    
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public LoginDetailsViewModel LoginDetails(LoginDetailsViewModel model)
        {
            try
            {
                string msg = "";
                string username = (model.UserName != "") ? model.UserName : "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                DateTime today = DateTime.Today;

                LoginDetailsViewModel ldvm = new LoginDetailsViewModel();

                if (username != "")
                {
                    var EsslDetails = (from essl in DB.Emp_AttendanceTime
                                       where (essl.EmpCode).ToUpper() == (model.EmpCode).ToUpper() && essl.LogDate == today
                                       select essl).ToList();

                    var WFHDetails = (from wfh in DB.WFHLoginlogs
                                       where (wfh.EmpCode).ToUpper() == (model.EmpCode).ToUpper() && wfh.Date == today 
                                       select wfh).ToList();

                    var OnSiteDetails = (from onsite in DB.Loginlogs
                                         where (onsite.EmpCode).ToUpper() == (model.EmpCode).ToUpper() && onsite.LoginDate == today
                                       select onsite).ToList();

                    if (EsslDetails.Count() > 0)
                    {
                        ldvm.EmpCode = EsslDetails[0].EmpCode;
                        ldvm.EmpId = LoginId;
                        ldvm.Mode = "ESSL";
                        ldvm.Date = EsslDetails[0].LogDate?.ToString("yyyy-MM-dd") ?? "";
                        ldvm.Time = EsslDetails[0].Duration?.ToString(@"hh\:mm") ?? "";
                        return ldvm;
                    }
                    else if (WFHDetails.Count() > 0)
                    {
                        ldvm.EmpCode = WFHDetails[0].EmpCode;
                        ldvm.EmpId = LoginId;
                        ldvm.Mode = "WFH";
                        ldvm.Date = WFHDetails[0].Date?.ToString("yyyy-MM-dd") ?? "";
                        ldvm.Time = WFHDetails[0].LoginTime?.ToString(@"hh\:mm") ?? "";
                        return ldvm;
                    }
                    else if (OnSiteDetails.Count() > 0)
                    {
                        ldvm.EmpCode = OnSiteDetails[0].EmpCode;
                        ldvm.EmpId = LoginId;
                        ldvm.Mode = "OnSite";
                        ldvm.Date = OnSiteDetails[0].LoginDate?.ToString("yyyy-MM-dd") ?? "";
                        ldvm.Time = OnSiteDetails[0].LogInTime?.ToString(@"hh\:mm") ?? "";
                        return ldvm;
                    }
                    else 
                    {
                        ldvm.EmpCode = username;
                        ldvm.EmpId = LoginId;
                        ldvm.Mode = "";
                        ldvm.Date = "";
                        ldvm.Time = "";
                        return ldvm;
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "UserName is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
    }
}