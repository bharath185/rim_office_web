using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;

namespace OfficeConnect_Web.Models
{
    public class PerformanceModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();

        public List<DDFinancialYear> DDFYear(DDFinancialYear model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Yeardetails = (from Fyear in DB.FinancialYearMasters
                                   where Fyear.IsActive == true && Fyear.IsDeleted == false
                                   select new DDFinancialYear
                                   {
                                       YearId = Fyear.YearId,
                                       FinancialYear = Fyear.FinancialYear,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (Yeardetails != null)
                    {
                        return Yeardetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDQuater> DDQuater(DDQuater model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Quaterdetails = (from Quater in DB.QuaterMasters
                                     where Quater.IsActive == true && Quater.IsDeleted == false
                                     select new DDQuater
                                     {
                                         QId = Quater.QId,
                                         Name = Quater.Name + " (" + Quater.StartDate + " - " + Quater.EndDate + ") ",
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Quaterdetails != null)
                    {
                        return Quaterdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Quater Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDReviewStatus> DDReviewStatus(DDReviewStatus model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                //var Reviewdetails = DB.Per_Goal
                //                        .Where(rs => rs.IsActive == true
                //                                  && rs.IsDeleted == false
                //                                  && !string.IsNullOrEmpty(rs.Status)) // skip empty or null
                //                        .Select(rs => new DDReviewStatus
                //                        {
                //                            Id = rs.GoalId,
                //                            OverAllStatus = rs.Status
                //                        })
                //                        .Distinct() // apply distinct
                //                        .ToList();
                var Reviewdetails = DB.Per_Goal
                                        .Where(rs => rs.IsActive == true
                                                  && rs.IsDeleted == false
                                                  && !string.IsNullOrEmpty(rs.Status)) // skip null or empty
                                        .GroupBy(rs => rs.Status)                     // group by Status (distinct)
                                        .Select(g => new DDReviewStatus
                                        {
                                            Id = g.FirstOrDefault().GoalId,              // pick any QId from the group
                                            OverAllStatus = g.Key                     // the distinct Status value
                                        })
                                        .ToList();
                if (EmpId != 0)
                {
                    if (Reviewdetails != null)
                    {
                        return Reviewdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Review Status Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<QuaterMasterViewModel> GetQuaterDetails(QuaterMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string Type = (model.Type != null) ? model.Type : "";

                //var Yeardetails = (from Quater in DB.QuaterMasters
                //                   where Quater.IsActive == true && Quater.IsDeleted == false && Quater.Type == Type
                //                   select new QuaterMasterViewModel
                //                   {
                //                       QId = Quater.QId,
                //                       Type = Quater.Type,
                //                       Name = Quater.Name,
                //                       StartDate = Quater.StartDate,
                //                       EndDate = Quater.EndDate,
                //                       Status = Quater.Status,
                //                       CreatedBy = Quater.CreatedBy,
                //                       CreatedDate = Quater.CreatedDate,
                //                       LastUpdatedDate = Quater.LastUpdatedDate,
                //                       IsActive = Quater.IsActive,
                //                       IsUpdated = Quater.IsUpdated,
                //                       IsDeleted = Quater.IsDeleted,
                //                   }).ToList();

                var Quaterdetails = (from Quater in DB.QuaterMasters
                                   where Quater.IsActive == true && Quater.IsDeleted == false && Quater.Type == Type
                                   select Quater).ToList();

                List<QuaterMasterViewModel> listquater = new List<QuaterMasterViewModel>();

                for (int i = 0; i < Quaterdetails.Count(); i++)
                {
                    QuaterMasterViewModel quater = new QuaterMasterViewModel();
                    quater.QId = Quaterdetails[i].QId;
                    quater.Type = Quaterdetails[i].Type;
                    quater.Name = Quaterdetails[i].Name;
                    quater.StartDate = Quaterdetails[i].StartDate;
                    quater.EndDate = Quaterdetails[i].EndDate;
                    quater.Status = Quaterdetails[i].Status;
                    quater.CreatedBy = Quaterdetails[i].CreatedBy;
                    quater.CreatedDate = Convert.ToDateTime(Quaterdetails[i].CreatedDate);
                    quater.LastUpdatedBy = Quaterdetails[i].LastUpdatedBy;
                    quater.LastUpdatedDate = Convert.ToDateTime(Quaterdetails[i].LastUpdatedDate);
                    quater.IsActive = Quaterdetails[i].IsActive;
                    quater.IsUpdated = Quaterdetails[i].IsUpdated;
                    quater.IsDeleted = Quaterdetails[i].IsDeleted;
                    listquater.Add(quater);
                }

                if (EmpId != 0)
                {
                    if (listquater != null)
                    {
                        return listquater;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<FyearDetailsViewModel> GetFYearDetails(FyearDetailsViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Yeardetails = (from Fyear in DB.ConfigSetups
                                   join F in DB.FinancialYearMasters on Fyear.FYearId equals F.YearId 
                                   where Fyear.Status == "ACTIVE" && F.Status == true &&
                                   Fyear.IsActive == true && Fyear.IsDeleted == false &&
                                   F.IsActive == true && F.IsDeleted == false 
                                   select new FyearDetailsViewModel
                                   {
                                       FYearId = Fyear.FYearId,
                                       FinancialYear = F.FinancialYear,
                                       QName = Fyear.Type,
                                       StartDate = Fyear.StartDate,
                                       EndDate = Fyear.EndDate,
                                       FinancialDetails = F.FinancialYear + ", " + Fyear.StartDate + " - " + Fyear.EndDate
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (Yeardetails != null)
                    {
                        return Yeardetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public ConfigSetupViewmodel SubmitConfigSetup(ConfigSetupViewmodel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? FYearId = (model.FYearId != 0) ? model.FYearId : 0;
                string FYear = (model.FYear != "") ? model.FYear : "";
                string Type = (model.Type != "" || model.Type != null) ? model.Type : "";

                var Configdetails = (from config in DB.ConfigSetups
                                     where config.FYearId == FYearId
                                     && config.IsActive == true && config.IsDeleted == false && (config.Status == "ACTIVE" || config.Status == "PENDING")
                                     select config).ToList();

                if (Configdetails.Count() == 0)
                {
                    var Quaterdetails = (from config in DB.QuaterMasters
                                         where config.Type == Type && config.IsActive == true && config.IsDeleted == false
                                         select config).ToList();

                    if (EmpId != 0)
                    {
                        for (int i = 0; i < Quaterdetails.Count(); i++)
                        {
                            ConfigSetup cs = new ConfigSetup();
                            cs.FYearId = FYearId;
                            cs.QId = Quaterdetails[i].QId;
                            cs.Type = Quaterdetails[i].Type;
                            cs.StartDate = Quaterdetails[i].StartDate;
                            cs.EndDate = Quaterdetails[i].EndDate;
                            cs.CreationDate = Quaterdetails[i].StartDate;
                            cs.ExtendCreationDate = "";
                            cs.SubmitDate = Quaterdetails[i].EndDate;
                            cs.ExtendSubmitDate = "";
                            if (i == 0)
                            {
                                cs.Status = "ACTIVE";
                            }
                            else
                            {
                                cs.Status = "PENDING";
                            }
                            cs.CreatedBy = EmpId;
                            cs.CreatedDate = DateTime.Now;
                            cs.LastUpdateBy = EmpId;
                            cs.LastUpdatedDate = DateTime.Now;
                            cs.IsActive = true;
                            cs.IsUpdated = false;
                            cs.IsDeleted = false;
                            DB.ConfigSetups.Add(cs);
                            DB.SaveChanges();
                        }
                        ConfigSetupViewmodel csvm = new ConfigSetupViewmodel();
                        csvm.FYear = FYear;
                        csvm.msg = "Config Setup Done";

                        return csvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "This Financial Year configuration setup is already exist");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public ConfigSetupViewmodel UpdateConfigSetup(ConfigSetupViewmodel model)
        {
            try
            {
                string msg = "";
                int? ConfigSetupId = (model.ConfigSetupId != 0) ? model.ConfigSetupId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? FYearId = (model.FYearId != 0) ? model.FYearId : 0;
                string FYear = (model.FYear != "") ? model.FYear : "";
                int? QId = (model.QId != 0) ? model.QId : 0;
                string Type = (model.Type != "" || model.Type != null) ? model.Type : "";
                string ExtendCreationDate = (model.ExtendCreationDate != "" || model.ExtendCreationDate != null) ? model.ExtendCreationDate : "";
                string ExtendSubmitDate = (model.ExtendSubmitDate != "" || model.ExtendSubmitDate != null) ? model.ExtendSubmitDate : "";

                var Configdetails = (from config in DB.ConfigSetups
                                     where config.ConfigSetupId == ConfigSetupId && config.FYearId == FYearId && config.QId == QId
                                     && config.Status == "ACTIVE" && config.IsActive == true && config.IsDeleted == false
                                     select config).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Configdetails != null)
                    {
                        if (ExtendCreationDate != "")
                        {
                            Configdetails.ExtendCreationDate = model.ExtendCreationDate;
                            Configdetails.Status = "ACTIVE";
                            Configdetails.IsActive = true;
                            Configdetails.IsUpdated = true;
                            Configdetails.IsDeleted = false;
                            Configdetails.LastUpdateBy = EmpId;
                            Configdetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            if (ExtendSubmitDate != "")
                            {
                                Configdetails.ExtendSubmitDate = model.ExtendSubmitDate;
                                Configdetails.Status = "ACTIVE";
                                Configdetails.IsActive = true;
                                Configdetails.IsUpdated = true;
                                Configdetails.IsDeleted = false;
                                Configdetails.LastUpdateBy = EmpId;
                                Configdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                ConfigSetupViewmodel csvm1 = new ConfigSetupViewmodel();
                                csvm1.msg = "Extended Submit Date";

                                return csvm1;
                            }

                            ConfigSetupViewmodel csvm = new ConfigSetupViewmodel();
                            csvm.msg = "Extended Creation Date";

                            return csvm;
                        }
                        else if (ExtendSubmitDate != "")
                        {
                            Configdetails.ExtendSubmitDate = model.ExtendSubmitDate;
                            Configdetails.Status = "ACTIVE";
                            Configdetails.IsActive = true;
                            Configdetails.IsUpdated = true;
                            Configdetails.IsDeleted = false;
                            Configdetails.LastUpdateBy = EmpId;
                            Configdetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            ConfigSetupViewmodel csvm = new ConfigSetupViewmodel();
                            csvm.msg = "Extended Submit Date";

                            return csvm;
                        }
                        else
                        {
                            if (ExtendCreationDate != "")
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Config Setup Details Not Found");
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Config Setup Details Not Found");
                            }
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Config Setup Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<ConfigSetupViewmodel> GetAllConfigSetup(ConfigSetupViewmodel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Configdetails = (from config in DB.ConfigSetups
                                     where config.IsActive == true && config.IsDeleted == false
                                     select config).ToList();

                List<ConfigSetupViewmodel> listofconfigsetup = new List<ConfigSetupViewmodel>();

                for (int i = 0; i < Configdetails.Count(); i++)
                {
                    ConfigSetupViewmodel configsetup = new ConfigSetupViewmodel();
                    configsetup.ConfigSetupId = Configdetails[i].ConfigSetupId;
                    configsetup.FYearId = Configdetails[i].FYearId;
                    configsetup.FYear = Convert.ToString(DB.FinancialYearMasters.Where(x => x.YearId == configsetup.FYearId).Select(x => x.FinancialYear).FirstOrDefault());
                    configsetup.Type = Configdetails[i].Type;
                    configsetup.QId = Configdetails[i].QId;
                    configsetup.QName = Convert.ToString(DB.QuaterMasters.Where(x => x.QId == configsetup.QId).Select(x => x.Name).FirstOrDefault());
                    configsetup.StartDate = Configdetails[i].StartDate;
                    configsetup.EndDate = Configdetails[i].EndDate;
                    configsetup.CreationDate = Configdetails[i].CreationDate;
                    configsetup.ExtendCreationDate = Configdetails[i].ExtendCreationDate;
                    configsetup.SubmitDate = Configdetails[i].SubmitDate;
                    configsetup.ExtendSubmitDate = Configdetails[i].ExtendSubmitDate;
                    configsetup.Status = Configdetails[i].Status;
                    configsetup.CreatedBy = Configdetails[i].CreatedBy;
                    configsetup.CreatedDate = Convert.ToDateTime(Configdetails[i].CreatedDate);
                    configsetup.LastUpdateBy = Configdetails[i].LastUpdateBy;
                    configsetup.LastUpdatedDate = Convert.ToDateTime(Configdetails[i].LastUpdatedDate);
                    configsetup.IsActive = Configdetails[i].IsActive;
                    configsetup.IsUpdated = Configdetails[i].IsUpdated;
                    configsetup.IsDeleted = Configdetails[i].IsDeleted;
                    listofconfigsetup.Add(configsetup);
                }

                if (EmpId != 0)
                {
                    if (listofconfigsetup != null)
                    {
                        return listofconfigsetup;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Config Setup Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<EmployeeMasterViewModel> GetEmployeeDetails(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string username = (model.UserName != "") ? model.UserName : "";

                var Empdetails = (from user in DB.EmployeeMasters
                                   where user.EmpId == model.EmpId && user.IsActive == true && user.IsDeleted == false
                                   select user).ToList();

                ////int? reportid = Empdetails[0].ReportId;
                ////int? OldEmp_ID = Empdetails[0].OldEmp_ID;
                ////int? leId = Empdetails[0].LEId;

                ////var AuthorisedEmp = (from user in DB.EmployeeMasters
                ////                     where user.ReportId == OldEmp_ID && user.EmpCode.Contains("3DCAD-") && user.IsActive == true && user.IsDeleted == false
                ////                     select user).ToList();
                ////if (leId == 1)
                ////{
                ////    if (AuthorisedEmp.Count() == 0)
                ////    {
                ////        AuthorisedEmp = (from user in DB.EmployeeMasters
                ////                         where user.ReportId == OldEmp_ID && user.EmpCode.Contains("3DCADVS-") && user.IsActive == true && user.IsDeleted == false
                ////                         select user).ToList();

                ////        if (AuthorisedEmp.Count() == 0)
                ////        {
                ////            AuthorisedEmp = (from user in DB.EmployeeMasters
                ////                             where user.ReportId == OldEmp_ID && user.EmpCode.Contains("3DCADPU-") && user.IsActive == true && user.IsDeleted == false
                ////                             select user).ToList();

                ////            if (AuthorisedEmp.Count() == 0)
                ////            {
                ////                AuthorisedEmp = (from user in DB.EmployeeMasters
                ////                                 where user.ReportId == EmpId && user.IsActive == true && user.IsDeleted == false
                ////                                 select user).ToList();
                ////            }
                ////        }
                ////    }
                ////}
                ////else
                ////{
                ////    if (AuthorisedEmp.Count() == 0)
                ////    {
                ////        AuthorisedEmp = (from user in DB.EmployeeMasters
                ////                         where user.ReportId == EmpId && user.EmpCode.Contains("RIM-") && user.IsActive == true && user.IsDeleted == false
                ////                         select user).ToList();
                ////    }
                ////}
                ///

                int? reportid = Empdetails[0].ReportId != null ? Empdetails[0].ReportId : 0;
                int? OldEmp_ID = Empdetails[0].OldEmp_ID != null ? Empdetails[0].OldEmp_ID : 0;

                var AuthorisedEmp = (from user in DB.EmployeeMasters
                                     where user.ReportId == OldEmp_ID && user.IsActive == true && user.IsDeleted == false
                                     select user).ToList();

                if (OldEmp_ID == 0)
                {
                    AuthorisedEmp = (from user in DB.EmployeeMasters
                                     where user.ReportId == model.EmpId && user.IsActive == true && user.IsDeleted == false
                                     select user).ToList();
                }
                else
                {
                    AuthorisedEmp = (from user in DB.EmployeeMasters
                                     where user.ReportId == OldEmp_ID && user.IsActive == true && user.IsDeleted == false
                                     select user).ToList();

                    if (AuthorisedEmp.Count() == 0)
                    {
                        AuthorisedEmp = (from user in DB.EmployeeMasters
                                         where user.ReportId == model.EmpId && user.IsActive == true && user.IsDeleted == false
                                         select user).ToList();
                    }
                }

                DateTime tdy = DateTime.Now.Date; // Ensuring we compare only the date part
                int? loginlogid = 0;

                var loginlogsemp = (from ll in DB.Loginlogs
                                    where ll.LoginDate == tdy
                                          && ll.EmpId == model.EmpId && ll.LogoutDate == null
                                          && ll.IsActive == true
                                          && ll.IsDeleted == false
                                    orderby ll.CreatedDate descending // Specify the column for ordering
                                    select ll).ToList();

                if (loginlogsemp.Count > 0)
                {
                    loginlogid = loginlogsemp[0].Id;
                }
                else
                {
                    loginlogid = 0;
                }

                var loginlogsemp1 = (from ll in DB.OnSiteLoginlogs
                                    where ll.EmpId == model.EmpId
                                            && ll.LogoutDate == null
                                          && ll.IsActive == true
                                          && ll.IsDeleted == false
                                    orderby ll.CreatedDate descending // Specify the column for ordering
                                    select ll).ToList();


                List<EmployeeMasterViewModel> listofuserdetails = new List<EmployeeMasterViewModel>();

                for (int i = 0; i < Empdetails.Count(); i++)
                {
                    EmployeeMasterViewModel userdetails = new EmployeeMasterViewModel();
                    userdetails.CompId = Empdetails[i].CompId;
                    userdetails.OldEmp_ID = OldEmp_ID;
                    userdetails.Company = DB.CompanyMasters.Where(x => x.CompId == userdetails.CompId).Select(x => x.Company).FirstOrDefault();
                    userdetails.DeptId = Empdetails[i].CategoryId;
                    //userdetails.DeptName = DB.DeptMasters.Where(x => x.DeptId == userdetails.DeptId).Select(x => x.DeptName).FirstOrDefault();
                    userdetails.DeptName = Empdetails[i].DeptName;
                    userdetails.DesignationId = Empdetails[i].DesignationId;
                    //userdetails.Designation = DB.DesignationMasters.Where(x => x.DesignationId == userdetails.DesignationId).Select(x => x.Designation).FirstOrDefault();
                    userdetails.Designation = Empdetails[i].DesignationName;
                    userdetails.EmpId = Empdetails[i].EmpId;
                    userdetails.LoginId = Empdetails[i].EmpId;
                    userdetails.EmpCode = Empdetails[i].EmpCode;
                    userdetails.UserName = Empdetails[i].UserName;
                    userdetails.FirstName = Empdetails[i].FirstName;
                    userdetails.MiddleName = Empdetails[i].MiddleName;
                    userdetails.LastName = Empdetails[i].LastName;
                    userdetails.MobileNo = Empdetails[i].MobileNo;
                    userdetails.EmailId = Empdetails[i].EmailId;
                    userdetails.Gender = Empdetails[i].Gender;
                    userdetails.JoiningDate = Empdetails[i].JoiningDate;
                    userdetails.EmpStatus = Empdetails[i].EmpStatus;
                    userdetails.AuthorisedEntity = Empdetails[i].AuthorisedEntity;
                    userdetails.ReportId = Empdetails[i].ReportId;
                    userdetails.ReportEmpCode = DB.EmployeeMasters.Where(x => x.OldEmp_ID == userdetails.ReportId || x.EmpId == userdetails.ReportId).Select(x => x.EmpCode).FirstOrDefault();
                    if (AuthorisedEmp.Count > 0) { userdetails.Authorised = true; }
                    else { userdetails.Authorised = false; }
                    if (loginlogid == 0) {
                        userdetails.OnSiteLogInId = 0;
                        if (loginlogsemp1.Count == 0)
                        {
                            userdetails.OnSiteLogInDate = null;
                            userdetails.OnSiteLogInTime = null;
                            userdetails.OnSiteLogOutDate = null;
                            userdetails.OnSiteLogOutTime = null;
                            userdetails.OnSiteStatus = "LOGOUT";
                        }
                        else
                        {
                            userdetails.OnSiteLogInId = loginlogsemp1[0].Id;
                            userdetails.OnSiteLogInDate = loginlogsemp1[0].LoginDate;
                            userdetails.OnSiteLogInTime = loginlogsemp1[0].LogInTime;
                            userdetails.OnSiteLogOutDate = null;
                            userdetails.OnSiteLogOutTime = null;
                            userdetails.OnSiteStatus = "LOGIN";
                        }
                    }
                    else
                    {
                        userdetails.OnSiteLogInId = loginlogsemp[0].Id;
                        userdetails.OnSiteLogInDate = loginlogsemp[0].LoginDate;
                        userdetails.OnSiteLogInTime = loginlogsemp[0].LogInTime;
                        userdetails.OnSiteLogOutDate = null;
                        userdetails.OnSiteLogOutTime = null;
                        userdetails.OnSiteStatus = "LOGIN";
                    }
                    userdetails.IsActive = Empdetails[i].IsActive;
                    userdetails.IsUpdated = Empdetails[i].IsUpdated;
                    userdetails.IsDeleted = Empdetails[i].IsDeleted;
                    userdetails.CreatedBy = Empdetails[i].CreatedBy;
                    userdetails.CreatedDate = Empdetails[i].CreatedDate;
                    userdetails.LastUpdatedBy = Empdetails[i].LastUpdatedBy;
                    userdetails.LastUpdatedDate = Empdetails[i].LastUpdatedDate;
                    userdetails.CPwd = false;

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

                    listofuserdetails.Add(userdetails);
                }

                

                if (EmpId != 0)
                {
                    if (listofuserdetails != null)
                    {
                        return listofuserdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<Per_GoalViewModel> GetAllGoal(Per_GoalViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Goaldetails = (from Goal in DB.Per_Goal
                                   where Goal.EmpId == EmpId && Goal.IsActive == true && Goal.IsDeleted == false
                                   select Goal).ToList();

                List<Per_GoalViewModel> listofgoals = new List<Per_GoalViewModel>();

                for (int i = 0; i < Goaldetails.Count(); i++)
                {
                    Per_GoalViewModel goals = new Per_GoalViewModel();
                    goals.GoalId = Goaldetails[i].GoalId;
                    goals.Goal = Goaldetails[i].Goal;
                    goals.EmpId = Goaldetails[i].EmpId;
                    goals.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == goals.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                    goals.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == goals.EmpId).Select(x => x.FirstName).FirstOrDefault();
                    goals.QId = Goaldetails[i].QId;
                    goals.Type = DB.QuaterMasters.Where(x => x.QId == goals.QId).Select(x => x.Type).FirstOrDefault();
                    goals.QName = DB.QuaterMasters.Where(x => x.QId == goals.QId).Select(x => x.Name).FirstOrDefault();
                    goals.PeriodId = Goaldetails[i].PeriodId;
                    goals.FYear = DB.FinancialYearMasters.Where(x => x.YearId == goals.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                    goals.Description = Goaldetails[i].Description;
                    goals.FinalSubmit = Goaldetails[i].FinalSubmit;
                    goals.Weightage = Goaldetails[i].Weightage;
                    goals.EmpReview = Goaldetails[i].EmpReview;
                    goals.EDescription = Goaldetails[i].EDescription;
                    goals.ManagerReview = Goaldetails[i].ManagerReview;
                    goals.MDescription = Goaldetails[i].MDescription;
                    goals.Status = Goaldetails[i].Status;
                    goals.IsActive = Goaldetails[i].IsActive;
                    goals.IsUpdated = Goaldetails[i].IsUpdated;
                    goals.IsDeleted = Goaldetails[i].IsDeleted;
                    goals.CreatedBy = Goaldetails[i].CreatedBy;
                    goals.CreatedDate = Goaldetails[i].CreatedDate;
                    goals.LastUpdatedBy = Goaldetails[i].LastUpdatedBy;
                    goals.LastUpdatedDate = Goaldetails[i].LastUpdatedDate;
                    listofgoals.Add(goals);
                }

                if (EmpId != 0)
                {
                    if (listofgoals != null)
                    {
                        return listofgoals;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        ////public List<PerreportViewModel> PerformanceReport(PerreportViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
        ////        int? FYearId = (model.FYearId != 0) ? model.FYearId : 0;
        ////        int? QId = (model.QId != 0) ? model.QId : 0;

        ////        var grouped = DB.Per_Goal
        ////                        .Where(g => g.IsActive == true && g.IsDeleted == false)
        ////                        .GroupBy(g => g.EmpId)
        ////                        .Select(g => new
        ////                        {
        ////                            EmpId = g.Key,
        ////                            GoalCount = g.Count(),
        ////                            LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                        }).ToList();

        ////        if (FYearId > 0 && QId == 0)
        ////        {
        ////            grouped = DB.Per_Goal
        ////                        .Where(g => g.IsActive == true && g.IsDeleted == false && g.PeriodId == FYearId)
        ////                        .GroupBy(g => g.EmpId)
        ////                        .Select(g => new
        ////                        {
        ////                            EmpId = g.Key,
        ////                            GoalCount = g.Count(),
        ////                            LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                        }).ToList();

        ////            if (grouped.Count() > 0)
        ////            {
        ////                if (model.OverAllStatus == "Pending")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Pending")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Approved")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Approved")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Emp Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Emp Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Manager Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Manager Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////            }
        ////        }
        ////        else if (FYearId == 0 && QId > 0)
        ////        {
        ////            grouped = DB.Per_Goal
        ////                        .Where(g => g.IsActive == true && g.IsDeleted == false && g.QId == QId)
        ////                        .GroupBy(g => g.EmpId)
        ////                        .Select(g => new
        ////                        {
        ////                            EmpId = g.Key,
        ////                            GoalCount = g.Count(),
        ////                            LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                        }).ToList();

        ////            if (grouped.Count() > 0)
        ////            {
        ////                if (model.OverAllStatus == "Pending")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Pending")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Approved")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Approved")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Emp Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Emp Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Manager Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Manager Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////            }
        ////        }
        ////        else if (FYearId == 0 && QId == 0)
        ////        {
        ////            grouped = DB.Per_Goal
        ////                        .Where(g => g.IsActive == true && g.IsDeleted == false)
        ////                        .GroupBy(g => g.EmpId)
        ////                        .Select(g => new
        ////                        {
        ////                            EmpId = g.Key,
        ////                            GoalCount = g.Count(),
        ////                            LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                        }).ToList();

        ////            if (grouped.Count() > 0)
        ////            {
        ////                if (model.OverAllStatus == "Pending")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Pending")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Approved")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Approved")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Emp Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Emp Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Manager Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Manager Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////            }
        ////        }
        ////        else if (FYearId > 0 && QId > 0)
        ////        {
        ////            grouped = DB.Per_Goal
        ////                        .Where(g => g.IsActive == true && g.IsDeleted == false && g.PeriodId == FYearId && g.QId == QId)
        ////                        .GroupBy(g => g.EmpId)
        ////                        .Select(g => new
        ////                        {
        ////                            EmpId = g.Key,
        ////                            GoalCount = g.Count(),
        ////                            LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                        }).ToList();

        ////            if (grouped.Count() > 0)
        ////            {
        ////                if (model.OverAllStatus == "Pending")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Pending")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Approved")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Approved")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Emp Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Emp Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else if (model.OverAllStatus == "Manager Review Completed")
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "Manager Review Completed")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////                else
        ////                {
        ////                    grouped = DB.Per_Goal
        ////                                .Where(g => g.IsActive == true && g.IsDeleted == false && g.Status == "")
        ////                                .GroupBy(g => g.EmpId)
        ////                                .Select(g => new
        ////                                {
        ////                                    EmpId = g.Key,
        ////                                    GoalCount = g.Count(),
        ////                                    LatestGoalDate = g.Max(x => x.CreatedDate)
        ////                                }).ToList();
        ////                }
        ////            }
        ////        }

        ////        List<PerreportViewModel> listofPerReport = new List<PerreportViewModel>();

        ////        for (int i = 0; i < grouped.Count(); i++)
        ////        {
        ////            var PRdetails = (from Goal in DB.Per_Goal
        ////                             where Goal.EmpId == grouped[i].EmpId && Goal.IsActive == true && Goal.IsDeleted == false
        ////                             select Goal).FirstOrDefault();

        ////            PerreportViewModel prvm = new PerreportViewModel();
        ////            prvm.GoalId = PRdetails.GoalId;
        ////            prvm.Goal = PRdetails.Goal;
        ////            prvm.EmpId = PRdetails.EmpId;
        ////            prvm.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == prvm.EmpId).Select(x => x.EmpCode).FirstOrDefault();
        ////            prvm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == prvm.EmpId).Select(x => x.FirstName).FirstOrDefault();
        ////            prvm.QId = PRdetails.QId;
        ////            prvm.Type = DB.QuaterMasters.Where(x => x.QId == prvm.QId).Select(x => x.Type).FirstOrDefault();
        ////            prvm.QName = DB.QuaterMasters.Where(x => x.QId == prvm.QId).Select(x => x.Name).FirstOrDefault();
        ////            prvm.PeriodId = PRdetails.PeriodId;
        ////            prvm.FYear = DB.FinancialYearMasters.Where(x => x.YearId == prvm.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
        ////            prvm.Description = PRdetails.Description;
        ////            prvm.FinalSubmit = PRdetails.FinalSubmit;
        ////            prvm.Weightage = PRdetails.Weightage;
        ////            prvm.EmpReview = PRdetails.EmpReview;
        ////            prvm.EDescription = PRdetails.EDescription;
        ////            prvm.ManagerReview = PRdetails.ManagerReview;
        ////            prvm.MDescription = PRdetails.MDescription;
        ////            prvm.Status = PRdetails.Status;
        ////            prvm.IsActive = PRdetails.IsActive;
        ////            prvm.IsUpdated = PRdetails.IsUpdated;
        ////            prvm.IsDeleted = PRdetails.IsDeleted;
        ////            prvm.CreatedBy = PRdetails.CreatedBy;
        ////            prvm.CreatedDate = PRdetails.CreatedDate;
        ////            prvm.LastUpdatedBy = PRdetails.LastUpdatedBy;
        ////            prvm.LastUpdatedDate = PRdetails.LastUpdatedDate;
        ////            listofPerReport.Add(prvm);
        ////        }

        ////        if (EmpId != 0)
        ////        {
        ////            if (listofPerReport != null)
        ////            {
        ////                return listofPerReport;
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Performance Report Details Not Found");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}


        public List<PerreportViewModel> PerformanceReport(PerreportViewModel model)
        {
            try
            {
                if (model.EmpId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");

                var query = DB.Per_Goal.Where(g => g.IsActive == true && g.IsDeleted == false);

                if (model.FYearId > 0)
                    query = query.Where(g => g.PeriodId == model.FYearId);

                if (model.QId > 0)
                    query = query.Where(g => g.QId == model.QId);

                ////if (!string.IsNullOrEmpty(model.OverAllStatus))
                ////    query = query.Where(g => g.Status.ToUpper() == model.OverAllStatus.ToUpper());
                ///
                if (!string.IsNullOrEmpty(model.OverAllStatus))
                {
                    if (model.OverAllStatus.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    {
                        // no filter — query stays as is
                    }
                    else
                    {
                        query = query.Where(g => g.Status != null &&
                                                 g.Status.Equals(model.OverAllStatus, StringComparison.OrdinalIgnoreCase));
                    }
                }


                var grouped = query
                    .GroupBy(g => g.EmpId)
                    .Select(g => new
                    {
                        EmpId = g.Key,
                        GoalCount = g.Count(),
                        LatestGoalDate = g.Max(x => x.CreatedDate)
                    }).ToList();

                if (!grouped.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Performance Report Details Not Found");

                var empIds = grouped.Select(g => g.EmpId).ToList();
                var goals = DB.Per_Goal
                    .Where(g => empIds.Contains(g.EmpId) && g.IsActive == true && g.IsDeleted == false)
                    .GroupBy(g => g.EmpId)
                    .Select(g => g.OrderByDescending(x => x.CreatedDate).FirstOrDefault())
                    .ToList();

                var employees = DB.EmployeeMasters
                    .Where(e => empIds.Contains(e.EmpId))
                    .ToList();

                if (model.EmpId == 149)
                {
                    employees = DB.EmployeeMasters
                    .Where(e => empIds.Contains(e.EmpId))
                    .ToList();
                }
                else if (model.EmpId == 550)
                {
                    employees = DB.EmployeeMasters
                    .Where(e => empIds.Contains(e.EmpId) && e.EmpCode.Contains("3DCADVS"))
                    .ToList();
                }
                else if (model.EmpId == 488)
                {
                    employees = DB.EmployeeMasters
                    .Where(e => empIds.Contains(e.EmpId) && e.EmpCode.Contains("3DCADPU"))
                    .ToList();
                }

                var quaters = DB.QuaterMasters.ToList();
                var years = DB.FinancialYearMasters.ToList();

                var list = goals.Select(goal =>
                {
                    var emp = employees.FirstOrDefault(e => e.EmpId == goal.EmpId && e.IsActive == true);
                    var quarter = quaters.FirstOrDefault(q => q.QId == goal.QId && q.Status == true);
                    var year = years.FirstOrDefault(y => y.YearId == goal.PeriodId);

                    return new PerreportViewModel
                    {
                        GoalId = goal.GoalId,
                        Goal = goal.Goal,
                        EmpId = goal.EmpId,
                        EmpCode = emp?.EmpCode,
                        EmpName = emp?.FirstName,
                        QId = goal.QId,
                        Type = quarter?.Type,
                        QName = quarter?.Name,
                        PeriodId = goal.PeriodId,
                        FYear = year?.FinancialYear,
                        Description = goal.Description,
                        FinalSubmit = goal.FinalSubmit,
                        Weightage = goal.Weightage,
                        EmpReview = goal.EmpReview,
                        EDescription = goal.EDescription,
                        ManagerReview = goal.ManagerReview,
                        MDescription = goal.MDescription,
                        Status = goal.Status,
                        IsActive = goal.IsActive,
                        IsUpdated = goal.IsUpdated,
                        IsDeleted = goal.IsDeleted,
                        CreatedBy = goal.CreatedBy,
                        CreatedDate = goal.CreatedDate,
                        LastUpdatedBy = goal.LastUpdatedBy,
                        LastUpdatedDate = goal.LastUpdatedDate
                    };
                }).Where(x => x.EmpCode != null && x.EmpName != null).OrderByDescending(x => x.LastUpdatedDate).ToList();

                return list;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }


        public List<Per_GoalViewModel> GetAllGoalEmployee(Per_GoalViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var oldEmpId = (from Emp in DB.EmployeeMasters
                               where Emp.EmpId == EmpId && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).FirstOrDefault();

                int? OldEmp_ID = oldEmpId.OldEmp_ID;
                int? Desig_Id = oldEmpId.DesignationId;

                var Emplist = (from Emp in DB.EmployeeMasters
                               where (Emp.ReportId == OldEmp_ID || Emp.ReportId == EmpId) && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();

                if (Desig_Id == 310)
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where Emp.EmpCode.StartsWith("3DCADPU") && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                else if (Desig_Id == 1073)
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where Emp.EmpCode.StartsWith("3DCADVS") && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                else if (Desig_Id == 22 || Desig_Id == 186 || Desig_Id == 59 || Desig_Id == 191 || Desig_Id == 94)
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                else
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where (Emp.ReportId == OldEmp_ID || Emp.ReportId == EmpId) && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                //var Emplist = (from Emp in DB.EmployeeMasters
                //               where (Emp.ReportId == OldEmp_ID || Emp.ReportId == EmpId) && Emp.IsActive == true && Emp.IsDeleted == false
                //               select Emp).ToList();

                List<Per_GoalViewModel> listofempgoals = new List<Per_GoalViewModel>();

                for (int i = 0; i < Emplist.Count(); i++)
                {
                    int? gempid = Emplist[i].EmpId;

                    var Goaldetails = (from Goal in DB.Per_Goal
                                       where Goal.IsActive == true && Goal.IsDeleted == false && Goal.EmpId == gempid
                                       && Goal.FinalSubmit == true && (Goal.Status).ToUpper() == "PENDING" 
                                       && Goal.ReviewedByEmp == false && Goal.ReviewedByManager == false
                                       select Goal).ToList();

                    if (Goaldetails.Count() != 0)
                    {
                        for (int j = 0; j < Goaldetails.Count(); j++)
                        {
                            Per_GoalViewModel empgoals = new Per_GoalViewModel();
                            empgoals.GoalId = Goaldetails[j].GoalId;
                            empgoals.Goal = Goaldetails[j].Goal;
                            empgoals.EmpId = Goaldetails[j].EmpId;
                            empgoals.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == empgoals.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                            empgoals.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == empgoals.EmpId).Select(x => x.FirstName).FirstOrDefault();
                            empgoals.QId = Goaldetails[j].QId;
                            empgoals.Type = DB.QuaterMasters.Where(x => x.QId == empgoals.QId).Select(x => x.Type).FirstOrDefault();
                            empgoals.QName = DB.QuaterMasters.Where(x => x.QId == empgoals.QId).Select(x => x.Name).FirstOrDefault();
                            empgoals.PeriodId = Goaldetails[j].PeriodId;
                            empgoals.FYear = DB.FinancialYearMasters.Where(x => x.YearId == empgoals.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                            empgoals.Description = Goaldetails[j].Description;
                            empgoals.FinalSubmit = Goaldetails[j].FinalSubmit;
                            empgoals.Weightage = Goaldetails[j].Weightage;
                            empgoals.EmpReview = Goaldetails[j].EmpReview;
                            empgoals.ManagerReview = Goaldetails[j].ManagerReview;
                            empgoals.Status = Goaldetails[j].Status;
                            empgoals.IsActive = Goaldetails[j].IsActive;
                            empgoals.IsUpdated = Goaldetails[j].IsUpdated;
                            empgoals.IsDeleted = Goaldetails[j].IsDeleted;
                            empgoals.CreatedBy = Goaldetails[j].CreatedBy;
                            empgoals.CreatedDate = Goaldetails[j].CreatedDate;
                            empgoals.LastUpdatedBy = Goaldetails[j].LastUpdatedBy;
                            empgoals.LastUpdatedDate = Goaldetails[j].LastUpdatedDate;
                            listofempgoals.Add(empgoals);
                        }
                    }
                }


                if (EmpId != 0)
                {
                    if (listofempgoals != null)
                    {
                        return listofempgoals;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_GoalViewModel GetGoal(Per_GoalViewModel model)
        {
            try
            {
                int id = (model.GoalId != 0) ? model.GoalId : 0;
                string Goalname = model.Goal;
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Goaldetails = (from Goal in DB.Per_Goal
                                   where Goal.EmpId == EmpId && Goal.GoalId == id && Goal.IsActive == true && Goal.IsDeleted == false
                                   select Goal).FirstOrDefault();

                Per_GoalViewModel goals = new Per_GoalViewModel();
                goals.GoalId = Goaldetails.GoalId;
                goals.Goal = Goaldetails.Goal;
                goals.EmpId = Goaldetails.EmpId;
                goals.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == goals.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                goals.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == goals.EmpId).Select(x => x.FirstName).FirstOrDefault();
                goals.QId = Goaldetails.QId;
                goals.Type = DB.QuaterMasters.Where(x => x.QId == goals.QId).Select(x => x.Type).FirstOrDefault();
                goals.QName = DB.QuaterMasters.Where(x => x.QId == goals.QId).Select(x => x.Name).FirstOrDefault();
                goals.PeriodId = Goaldetails.PeriodId;
                goals.FYear = DB.FinancialYearMasters.Where(x => x.YearId == goals.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                goals.Description = Goaldetails.Description;
                goals.FinalSubmit = Goaldetails.FinalSubmit;
                goals.Weightage = Goaldetails.Weightage;
                goals.EmpReview = Goaldetails.EmpReview;
                goals.ManagerReview = Goaldetails.ManagerReview;
                goals.Status = Goaldetails.Status;
                goals.IsActive = Goaldetails.IsActive;
                goals.IsUpdated = Goaldetails.IsUpdated;
                goals.IsDeleted = Goaldetails.IsDeleted;
                goals.CreatedBy = Goaldetails.CreatedBy;
                goals.CreatedDate = Goaldetails.CreatedDate;
                goals.LastUpdatedBy = Goaldetails.LastUpdatedBy;
                goals.LastUpdatedDate = Goaldetails.LastUpdatedDate;

                if (EmpId != 0)
                {
                    if (goals != null)
                    {
                        return goals;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public Per_GoalListViewModel AddAllGoal(Per_GoalListViewModel model)
        {
            try
            {
                if (model.EmpId != 0)
                {
                    for (int i = 0; i < model.listofGoal.Count; i++)
                    {
                        string msg = "";
                        int? EmpId = (model.listofGoal[i].EmpId != 0) ? model.listofGoal[i].EmpId : 0;
                        int id = (model.listofGoal[i].GoalId != 0) ? model.listofGoal[i].GoalId : 0;
                        string Goalname = (model.listofGoal[i].Goal != "" || model.listofGoal[i].Goal != null) ? model.listofGoal[i].Goal : "";
                        string Weightage = (model.listofGoal[i].Weightage != "0" || model.listofGoal[i].Weightage != null) ? model.listofGoal[i].Weightage : "0";

                        var Goaldetails = (from Goal in DB.Per_Goal
                                           where Goal.GoalId == id && Goal.IsActive == true && Goal.IsDeleted == false
                                           && Goal.FinalSubmit == false
                                           select Goal).FirstOrDefault();

                        if (Goaldetails != null)
                        {
                            Goaldetails.Goal = model.listofGoal[i].Goal;
                            Goaldetails.Weightage = model.listofGoal[i].Weightage;
                            Goaldetails.Status = "Pending";
                            Goaldetails.FinalSubmit = true;
                            Goaldetails.IsActive = true;
                            Goaldetails.IsUpdated = true;
                            Goaldetails.IsDeleted = false;
                            Goaldetails.LastUpdatedBy = EmpId;
                            Goaldetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "This " + model.listofGoal[i].Goal + " Detail is Not Found");
                        }

                    }
                    Per_GoalListViewModel glvm = new Per_GoalListViewModel();
                    glvm.msg = "Final Subimission Done";
                    return glvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public Per_GoalListViewModel ApproveAllGoal(Per_GoalListViewModel model)
        {
            try
            {
                if (model.EmpId != 0)
                {
                    for (int i = 0; i < model.listofGoal.Count; i++)
                    {
                        string msg = "";
                        int? EmpId = (model.listofGoal[i].EmpId != 0) ? model.listofGoal[i].EmpId : 0;
                        int id = (model.listofGoal[i].GoalId != 0) ? model.listofGoal[i].GoalId : 0;
                        string Goalname = (model.listofGoal[i].Goal != "" || model.listofGoal[i].Goal != null) ? model.listofGoal[i].Goal : "";
                        string Weightage = (model.listofGoal[i].Weightage != "0" || model.listofGoal[i].Weightage != null) ? model.listofGoal[i].Weightage : "0";

                        var Goaldetails = (from Goal in DB.Per_Goal
                                           where Goal.GoalId == id && Goal.IsActive == true && Goal.IsDeleted == false
                                           select Goal).FirstOrDefault();

                        if (Goaldetails != null)
                        {
                            Goaldetails.Goal = model.listofGoal[i].Goal;
                            Goaldetails.Weightage = model.listofGoal[i].Weightage;
                            Goaldetails.Status = "Approved";
                            Goaldetails.FinalSubmit = true;
                            Goaldetails.IsActive = true;
                            Goaldetails.IsUpdated = true;
                            Goaldetails.IsDeleted = false;
                            Goaldetails.LastUpdatedBy = EmpId;
                            Goaldetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "This " + model.listofGoal[i].Goal + " Detail is Not Found");
                        }

                    }
                    Per_GoalListViewModel glvm = new Per_GoalListViewModel();
                    glvm.msg = "Approved by Manager";
                    return glvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public Per_GoalViewModel AddGoal(Per_GoalViewModel model)
        {
            try
            {
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = DB.FinancialYearMasters.Where(x => x.IsActive == true && x.Status == true).Select(x => x.YearId).FirstOrDefault();

                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string Goalname = (model.Goal != "" || model.Goal != null) ? model.Goal : "";

                var Goaldetails = (from Goal in DB.Per_Goal
                                   where Goal.Goal == Goalname && Goal.IsActive == true && Goal.IsDeleted == false
                                   select Goal).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Goaldetails == null)
                    {
                        Per_Goal dm = new Per_Goal();
                        dm.Goal = model.Goal;
                        dm.QId = (QId != 0) ? QId : 0;
                        dm.PeriodId = (FYearId != 0) ? FYearId : 0;
                        dm.EmpId = model.EmpId;
                        dm.Description = (model.Description != "") ? model.Description : "";
                        dm.Weightage = model.Weightage;
                        dm.EmpReview = (model.EmpReview != "") ? model.EmpReview : "";
                        dm.ManagerReview = (model.ManagerReview != "") ? model.ManagerReview : "";
                        dm.Status = "";
                        dm.ReviewedByEmp = false;
                        dm.ReviewedByManager = false;
                        dm.FinalSubmit = false;
                        dm.IsActive = true;
                        dm.IsDeleted = false;
                        dm.CreatedBy = EmpId;
                        dm.CreatedDate = DateTime.Now;
                        dm.LastUpdatedBy = EmpId;
                        dm.LastUpdatedDate = DateTime.Now;
                        DB.Per_Goal.Add(dm);
                        DB.SaveChanges();

                        Per_GoalViewModel gvm = new Per_GoalViewModel();
                        gvm.msg = "Added";
                        gvm.Goal = model.Goal;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_GoalViewModel UpdateGoal(Per_GoalViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.GoalId != 0) ? model.GoalId : 0;
                string Goalname = (model.Goal != "" || model.Goal != null) ? model.Goal : "";

                var Goaldetails = (from Goal in DB.Per_Goal
                                   where Goal.GoalId == id && Goal.IsActive == true && Goal.IsDeleted == false
                                   select Goal).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Goaldetails != null)
                    {
                        Goaldetails.Goal = model.Goal;
                        Goaldetails.QId = (model.QId != 0) ? model.QId : 0;
                        Goaldetails.PeriodId = (model.PeriodId != 0) ? model.PeriodId : 0;
                        Goaldetails.EmpId = model.EmpId;
                        Goaldetails.Description = (model.Description != "") ? model.Description : "";
                        Goaldetails.Weightage = model.Weightage;
                        Goaldetails.EmpReview = (model.EmpReview != "") ? model.EmpReview : "";
                        Goaldetails.ManagerReview = (model.ManagerReview != "") ? model.ManagerReview : "";
                        Goaldetails.Status = "";
                        Goaldetails.FinalSubmit = false;
                        Goaldetails.IsActive = true;
                        Goaldetails.IsUpdated = true;
                        Goaldetails.IsDeleted = false;
                        Goaldetails.LastUpdatedBy = EmpId;
                        Goaldetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_GoalViewModel gvm = new Per_GoalViewModel();
                        gvm.msg = "Updated";
                        gvm.Goal = model.Goal;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_GoalViewModel DeleteGoal(Per_GoalViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.GoalId != 0) ? model.GoalId : 0;
                string Goalname = (model.Goal != "" || model.Goal != null) ? model.Goal : "";

                var Goaldetails = (from Goal in DB.Per_Goal
                                   where Goal.GoalId == id && Goal.IsActive == true && Goal.IsDeleted == false
                                   select Goal).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Goaldetails != null)
                    {
                        Goaldetails.IsDeleted = true;
                        Goaldetails.LastUpdatedBy = EmpId;
                        Goaldetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_GoalViewModel gvm = new Per_GoalViewModel();
                        gvm.msg = "Deleted";
                        gvm.Goal = model.Goal;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<Per_TaskViewModel> GetAllTask(Per_TaskViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Taskdetails = (from Task in DB.Per_Task
                                   join Goal in DB.Per_Goal on Task.GoalId equals Goal.GoalId
                                   where Task.EmpId == EmpId && Task.GoalId == model.GoalId && Task.Status == true && Task.IsActive == true && Task.IsDeleted == false
                                   select Task).ToList();

                List<Per_TaskViewModel> listoftasks = new List<Per_TaskViewModel>();

                for (int i = 0; i < Taskdetails.Count(); i++)
                {
                    Per_TaskViewModel tasks = new Per_TaskViewModel();
                    tasks.TaskId = Taskdetails[i].TaskId;
                    tasks.Task = Taskdetails[i].Task;
                    tasks.QId = Taskdetails[i].QId;
                    tasks.Type = DB.QuaterMasters.Where(x => x.QId == tasks.QId).Select(x => x.Type).FirstOrDefault();
                    tasks.QName = DB.QuaterMasters.Where(x => x.QId == tasks.QId).Select(x => x.Name).FirstOrDefault();
                    tasks.PeriodId = Taskdetails[i].PeriodId;
                    tasks.FYear = DB.FinancialYearMasters.Where(x => x.YearId == tasks.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                    tasks.EmpId = Taskdetails[i].EmpId;
                    tasks.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == tasks.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                    tasks.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == tasks.EmpId).Select(x => x.FirstName).FirstOrDefault();
                    tasks.GoalId = Taskdetails[i].GoalId;
                    tasks.Goal = DB.Per_Goal.Where(x => x.EmpId == tasks.EmpId && x.GoalId == tasks.GoalId).Select(x => x.Goal).FirstOrDefault();
                    tasks.Description = Taskdetails[i].Description;
                    tasks.Status = Taskdetails[i].Status;
                    tasks.IsActive = Taskdetails[i].IsActive;
                    tasks.IsUpdated = Taskdetails[i].IsUpdated;
                    tasks.IsDeleted = Taskdetails[i].IsDeleted;
                    tasks.CreatedBy = Taskdetails[i].CreatedBy;
                    tasks.CreatedDate = Taskdetails[i].CreatedDate;
                    tasks.LastUpdatedBy = Taskdetails[i].LastUpdatedBy;
                    tasks.LastUpdatedDate = Taskdetails[i].LastUpdatedDate;
                    listoftasks.Add(tasks);
                }

                if (EmpId != 0)
                {
                    if (listoftasks != null)
                    {
                        return listoftasks;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Task Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_TaskViewModel GetTask(Per_TaskViewModel model)
        {
            try
            {
                int id = (model.TaskId != 0) ? model.TaskId : 0;
                string Taskname = model.Task;
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Taskdetails = (from Task in DB.Per_Task
                                   join Goal in DB.Per_Goal on Task.GoalId equals Goal.GoalId
                                   where Task.EmpId == EmpId && Task.GoalId == model.GoalId && Task.TaskId == id && Task.Status == true
                                   && Task.IsActive == true && Task.IsDeleted == false
                                   select Task).FirstOrDefault();


                Per_TaskViewModel tasks = new Per_TaskViewModel();
                tasks.TaskId = Taskdetails.TaskId;
                tasks.Task = Taskdetails.Task;
                tasks.QId = Taskdetails.QId;
                tasks.Type = DB.QuaterMasters.Where(x => x.QId == tasks.QId).Select(x => x.Type).FirstOrDefault();
                tasks.QName = DB.QuaterMasters.Where(x => x.QId == tasks.QId).Select(x => x.Name).FirstOrDefault();
                tasks.PeriodId = Taskdetails.PeriodId;
                tasks.FYear = DB.FinancialYearMasters.Where(x => x.YearId == tasks.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                tasks.EmpId = Taskdetails.EmpId;
                tasks.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == tasks.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                tasks.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == tasks.EmpId).Select(x => x.FirstName).FirstOrDefault();
                tasks.GoalId = Taskdetails.GoalId;
                tasks.Goal = DB.Per_Goal.Where(x => x.EmpId == tasks.EmpId && x.GoalId == tasks.GoalId).Select(x => x.Goal).FirstOrDefault();
                tasks.Description = Taskdetails.Description;
                tasks.Status = Taskdetails.Status;
                tasks.IsActive = Taskdetails.IsActive;
                tasks.IsUpdated = Taskdetails.IsUpdated;
                tasks.IsDeleted = Taskdetails.IsDeleted;
                tasks.CreatedBy = Taskdetails.CreatedBy;
                tasks.CreatedDate = Taskdetails.CreatedDate;
                tasks.LastUpdatedBy = Taskdetails.LastUpdatedBy;
                tasks.LastUpdatedDate = Taskdetails.LastUpdatedDate;


                if (EmpId != 0)
                {
                    if (tasks != null)
                    {
                        return tasks;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Task Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_TaskViewModel AddTask(Per_TaskViewModel model)
        {
            try
            {
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = DB.FinancialYearMasters.Where(x => x.IsActive == true && x.Status == true).Select(x => x.YearId).FirstOrDefault();

                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string Taskname = (model.Task != "" || model.Task != null) ? model.Task : "";

                var Taskdetails = (from Task in DB.Per_Task
                                   where Task.Task == Taskname && Task.Status == true && Task.IsActive == true && Task.IsDeleted == false
                                   select Task).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Taskdetails == null)
                    {
                        Per_Task dm = new Per_Task();
                        dm.Task = model.Task;
                        dm.QId = (QId != 0) ? QId : 0;
                        dm.PeriodId = (FYearId != 0) ? FYearId : 0;
                        dm.GoalId = (model.GoalId != 0) ? model.GoalId : 0;
                        dm.EmpId = model.EmpId;
                        dm.Description = (model.Description != "") ? model.Description : "";
                        dm.Status = true;
                        dm.IsActive = true;
                        dm.IsDeleted = false;
                        dm.CreatedBy = EmpId;
                        dm.CreatedDate = DateTime.Now;
                        dm.LastUpdatedBy = EmpId;
                        dm.LastUpdatedDate = DateTime.Now;
                        DB.Per_Task.Add(dm);
                        DB.SaveChanges();

                        Per_TaskViewModel gvm = new Per_TaskViewModel();
                        gvm.msg = "Added";
                        gvm.Task = model.Task;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Task Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_TaskViewModel UpdateTask(Per_TaskViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.TaskId != 0) ? model.TaskId : 0;
                string Taskname = (model.Task != "" || model.Task != null) ? model.Task : "";

                var Taskdetails = (from Task in DB.Per_Task
                                   where Task.TaskId == id && Task.Status == true && Task.IsActive == true && Task.IsDeleted == false
                                   select Task).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Taskdetails != null)
                    {
                        Taskdetails.Task = model.Task;
                        Taskdetails.QId = (model.QId != 0) ? model.QId : 0;
                        Taskdetails.PeriodId = (model.PeriodId != 0) ? model.PeriodId : 0;
                        Taskdetails.GoalId = (model.GoalId != 0) ? model.GoalId : 0;
                        Taskdetails.EmpId = model.EmpId;
                        Taskdetails.Description = (model.Description != "") ? model.Description : "";
                        Taskdetails.Status = true;
                        Taskdetails.IsActive = true;
                        Taskdetails.IsUpdated = true;
                        Taskdetails.IsDeleted = false;
                        Taskdetails.LastUpdatedBy = EmpId;
                        Taskdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_TaskViewModel gvm = new Per_TaskViewModel();
                        gvm.msg = "Updated";
                        gvm.Task = model.Task;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Task Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_TaskViewModel DeleteTask(Per_TaskViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.TaskId != 0) ? model.TaskId : 0;
                string Taskname = (model.Task != "" || model.Task != null) ? model.Task : "";

                var Taskdetails = (from Task in DB.Per_Task
                                   where Task.TaskId == id && Task.Status == true && Task.IsActive == true && Task.IsDeleted == false
                                   select Task).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Taskdetails != null)
                    {
                        Taskdetails.IsDeleted = true;
                        Taskdetails.LastUpdatedBy = EmpId;
                        Taskdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_TaskViewModel gvm = new Per_TaskViewModel();
                        gvm.msg = "Deleted";
                        gvm.Task = model.Task;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Task Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<Per_BehaviourViewModel> GetAllBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Behaviourdetails = (from Behaviour in DB.Per_BehaviourMaster
                                   where Behaviour.IsActive == true && Behaviour.IsDeleted == false
                                   select Behaviour).ToList();

                List<Per_BehaviourViewModel> listofbehaviors = new List<Per_BehaviourViewModel>();

                for (int i = 0; i < Behaviourdetails.Count(); i++)
                {
                    Per_BehaviourViewModel behaviors = new Per_BehaviourViewModel();
                    behaviors.Id = Behaviourdetails[i].Id;
                    behaviors.Behaviour = Behaviourdetails[i].Behaviour;
                    behaviors.QId = Behaviourdetails[i].QId;
                    behaviors.Type = DB.QuaterMasters.Where(x => x.QId == behaviors.QId).Select(x => x.Type).FirstOrDefault();
                    behaviors.QName = DB.QuaterMasters.Where(x => x.QId == behaviors.QId).Select(x => x.Name).FirstOrDefault();
                    behaviors.PeriodId = Behaviourdetails[i].PeriodId;
                    behaviors.FYear = DB.FinancialYearMasters.Where(x => x.YearId == behaviors.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                    behaviors.EmpId = EmpId;
                    behaviors.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == behaviors.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                    behaviors.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == behaviors.EmpId).Select(x => x.FirstName).FirstOrDefault();
                    behaviors.Description = Behaviourdetails[i].Description;
                    behaviors.Weightage = Behaviourdetails[i].Weightage;
                    behaviors.EmpReview = DB.Per_BehaviourDetail.Where(x => x.EmpId == behaviors.EmpId && x.BehaviourId == behaviors.Id).Select(x => x.EmpReview).FirstOrDefault();
                    behaviors.EDescription = DB.Per_BehaviourDetail.Where(x => x.EmpId == behaviors.EmpId && x.BehaviourId == behaviors.Id).Select(x => x.EDescription).FirstOrDefault();
                    behaviors.ManagerReview = DB.Per_BehaviourDetail.Where(x => x.EmpId == behaviors.EmpId && x.BehaviourId == behaviors.Id).Select(x => x.ManagerReview).FirstOrDefault();
                    behaviors.MDescription = DB.Per_BehaviourDetail.Where(x => x.EmpId == behaviors.EmpId && x.BehaviourId == behaviors.Id).Select(x => x.MDescription).FirstOrDefault();
                    behaviors.Status = Behaviourdetails[i].Status;
                    behaviors.IsActive = Behaviourdetails[i].IsActive;
                    behaviors.IsUpdated = Behaviourdetails[i].IsActive;
                    behaviors.IsDeleted = Behaviourdetails[i].IsActive;
                    behaviors.CreatedBy = Behaviourdetails[i].CreatedBy;
                    behaviors.CreatedDate = Behaviourdetails[i].CreatedDate;
                    behaviors.LastUpdatedBy = Behaviourdetails[i].LastUpdatedBy;
                    behaviors.LastUpdatedDate = Behaviourdetails[i].LastUpdatedDate;
                    listofbehaviors.Add(behaviors);
                }

                if (EmpId != 0)
                {
                    if (listofbehaviors != null)
                    {
                        return listofbehaviors;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Behaviour Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourViewModel GetBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                int id = (model.Id != 0) ? model.Id : 0;
                string Behaviourname = model.Behaviour;
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Behaviourdetails = (from Behaviour in DB.Per_BehaviourMaster
                                        where Behaviour.Id == id && Behaviour.IsActive == true && Behaviour.IsDeleted == false
                                   select Behaviour).FirstOrDefault();

                Per_BehaviourViewModel behaviors = new Per_BehaviourViewModel();
                behaviors.Id = Behaviourdetails.Id;
                behaviors.Behaviour = Behaviourdetails.Behaviour;
                behaviors.QId = Behaviourdetails.QId;
                behaviors.Type = DB.QuaterMasters.Where(x => x.QId == behaviors.QId).Select(x => x.Type).FirstOrDefault();
                behaviors.QName = DB.QuaterMasters.Where(x => x.QId == behaviors.QId).Select(x => x.Name).FirstOrDefault();
                behaviors.PeriodId = Behaviourdetails.PeriodId;
                behaviors.FYear = DB.FinancialYearMasters.Where(x => x.YearId == behaviors.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                behaviors.EmpId = EmpId;
                behaviors.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == behaviors.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                behaviors.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == behaviors.EmpId).Select(x => x.FirstName).FirstOrDefault();
                behaviors.Description = Behaviourdetails.Description;
                behaviors.Weightage = Behaviourdetails.Weightage;
                behaviors.Status = Behaviourdetails.Status;
                behaviors.IsActive = Behaviourdetails.IsActive;
                behaviors.IsUpdated = Behaviourdetails.IsUpdated;
                behaviors.IsDeleted = Behaviourdetails.IsDeleted;
                behaviors.CreatedBy = Behaviourdetails.CreatedBy;
                behaviors.CreatedDate = Behaviourdetails.CreatedDate;
                behaviors.LastUpdatedBy = Behaviourdetails.LastUpdatedBy;
                behaviors.LastUpdatedDate = Behaviourdetails.LastUpdatedDate;

                if (EmpId != 0)
                {
                    if (behaviors != null)
                    {
                        return behaviors;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Behaviour Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourViewModel AddBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = DB.FinancialYearMasters.Where(x => x.IsActive == true && x.Status == true).Select(x => x.YearId).FirstOrDefault();

                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string Behaviourname = (model.Behaviour != "" || model.Behaviour != null) ? model.Behaviour : "";

                var Behaviourdetails = (from Behaviour in DB.Per_BehaviourMaster
                                        where Behaviour.Behaviour == Behaviourname && Behaviour.IsActive == true && Behaviour.IsDeleted == false
                                   select Behaviour).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Behaviourdetails == null)
                    {
                        Per_BehaviourMaster dm = new Per_BehaviourMaster();
                        dm.Behaviour = model.Behaviour;
                        dm.QId = (QId != 0) ? QId : 0;
                        dm.PeriodId = (FYearId != 0) ? FYearId : 0;
                        dm.Description = (model.Description != "") ? model.Description : "";
                        dm.Weightage = model.Weightage;
                        dm.IsActive = true;
                        dm.IsDeleted = false;
                        dm.CreatedBy = EmpId;
                        dm.CreatedDate = DateTime.Now;
                        dm.LastUpdatedBy = EmpId;
                        dm.LastUpdatedDate = DateTime.Now;
                        DB.Per_BehaviourMaster.Add(dm);
                        DB.SaveChanges();

                        Per_BehaviourViewModel gvm = new Per_BehaviourViewModel();
                        gvm.msg = "Added";
                        gvm.Behaviour = model.Behaviour;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Behaviour Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourViewModel UpdateBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.Id != 0) ? model.Id : 0;
                string Behaviourname = (model.Behaviour != "" || model.Behaviour != null) ? model.Behaviour : "";

                var Behaviourdetails = (from Behaviour in DB.Per_BehaviourMaster
                                        where Behaviour.Id == id && Behaviour.IsActive == true && Behaviour.IsDeleted == false
                                   select Behaviour).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Behaviourdetails != null)
                    {
                        Behaviourdetails.Behaviour = model.Behaviour;
                        Behaviourdetails.QId = (model.QId != 0) ? model.QId : 0;
                        Behaviourdetails.PeriodId = (model.PeriodId != 0) ? model.PeriodId : 0;
                        Behaviourdetails.Description = (model.Description != "") ? model.Description : "";
                        Behaviourdetails.Weightage = model.Weightage;
                        Behaviourdetails.IsActive = true;
                        Behaviourdetails.IsUpdated = true;
                        Behaviourdetails.IsDeleted = false;
                        Behaviourdetails.LastUpdatedBy = EmpId;
                        Behaviourdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_BehaviourViewModel gvm = new Per_BehaviourViewModel();
                        gvm.msg = "Updated";
                        gvm.Behaviour = model.Behaviour;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Behaviour Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourViewModel DeleteBehaviour(Per_BehaviourViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.Id != 0) ? model.Id : 0;
                string Behaviourname = (model.Behaviour != "" || model.Behaviour != null) ? model.Behaviour : "";

                var Behaviourdetails = (from Behaviour in DB.Per_BehaviourMaster
                                        where Behaviour.Id == id && Behaviour.IsActive == true && Behaviour.IsDeleted == false
                                   select Behaviour).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Behaviourdetails != null)
                    {
                        Behaviourdetails.IsDeleted = true;
                        Behaviourdetails.LastUpdatedBy = EmpId;
                        Behaviourdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_BehaviourViewModel gvm = new Per_BehaviourViewModel();
                        gvm.msg = "Deleted";
                        gvm.Behaviour = model.Behaviour;

                        return gvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Behaviour Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<Per_BehaviourDetailViewModel> GetAllBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var BehaviourDetaildetails = (from BehaviourDetail in DB.Per_BehaviourDetail
                                               join Behaviour in DB.Per_BehaviourMaster on BehaviourDetail.BehaviourId equals Behaviour.Id
                                               where BehaviourDetail.EmpId == EmpId && BehaviourDetail.BehaviourId == model.Id && Behaviour.Status == true 
                                               && BehaviourDetail.IsActive == true && BehaviourDetail.IsDeleted == false
                                               select BehaviourDetail).ToList();

                List<Per_BehaviourDetailViewModel> listofbehaviourdetails = new List<Per_BehaviourDetailViewModel>();

                for (int i = 0; i < BehaviourDetaildetails.Count(); i++)
                {
                    Per_BehaviourDetailViewModel behaviourdetails = new Per_BehaviourDetailViewModel();
                    behaviourdetails.Id = BehaviourDetaildetails[i].Id;
                    behaviourdetails.BehaviourId = BehaviourDetaildetails[i].BehaviourId;
                    behaviourdetails.Behaviour = BehaviourDetaildetails[i].Behaviour;
                    behaviourdetails.QId = BehaviourDetaildetails[i].QId;
                    behaviourdetails.Type = DB.QuaterMasters.Where(x => x.QId == behaviourdetails.QId).Select(x => x.Type).FirstOrDefault();
                    behaviourdetails.QName = DB.QuaterMasters.Where(x => x.QId == behaviourdetails.QId).Select(x => x.Name).FirstOrDefault();
                    behaviourdetails.PeriodId = BehaviourDetaildetails[i].PeriodId;
                    behaviourdetails.FYear = DB.FinancialYearMasters.Where(x => x.YearId == behaviourdetails.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                    behaviourdetails.EmpId = EmpId;
                    behaviourdetails.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == behaviourdetails.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                    behaviourdetails.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == behaviourdetails.EmpId).Select(x => x.FirstName).FirstOrDefault();
                    behaviourdetails.Description = BehaviourDetaildetails[i].Description;
                    behaviourdetails.Weightage = BehaviourDetaildetails[i].Weightage;
                    behaviourdetails.EmpReview = BehaviourDetaildetails[i].EmpReview;
                    behaviourdetails.ManagerReview = BehaviourDetaildetails[i].ManagerReview;
                    behaviourdetails.IsActive = BehaviourDetaildetails[i].IsActive;
                    behaviourdetails.IsUpdated = BehaviourDetaildetails[i].IsUpdated;
                    behaviourdetails.IsDeleted = BehaviourDetaildetails[i].IsDeleted;
                    behaviourdetails.CreatedBy = BehaviourDetaildetails[i].CreatedBy;
                    behaviourdetails.CreatedDate = BehaviourDetaildetails[i].CreatedDate;
                    behaviourdetails.LastUpdatedBy = BehaviourDetaildetails[i].LastUpdatedBy;
                    behaviourdetails.LastUpdatedDate = BehaviourDetaildetails[i].LastUpdatedDate;
                    listofbehaviourdetails.Add(behaviourdetails);
                }

                if (EmpId != 0)
                {
                    if (listofbehaviourdetails != null)
                    {
                        return listofbehaviourdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "BehaviourDetail Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourDetailViewModel GetBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                int? id = (model.BehaviourId != 0) ? model.BehaviourId : 0;
                string Behaviour = model.Behaviour;
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var BehaviourDetaildetails = (from BehaviourDetail in DB.Per_BehaviourDetail
                                                join Behaviours in DB.Per_BehaviourMaster on BehaviourDetail.BehaviourId equals Behaviours.Id
                                                where BehaviourDetail.EmpId == EmpId && BehaviourDetail.BehaviourId == model.Id && BehaviourDetail.Id == model.Id
                                               && BehaviourDetail.IsActive == true && BehaviourDetail.IsDeleted == false
                                               select BehaviourDetail).FirstOrDefault();

                Per_BehaviourDetailViewModel behaviourdetails = new Per_BehaviourDetailViewModel();
                behaviourdetails.Id = BehaviourDetaildetails.Id;
                behaviourdetails.BehaviourId = BehaviourDetaildetails.BehaviourId;
                behaviourdetails.Behaviour = BehaviourDetaildetails.Behaviour;
                behaviourdetails.QId = BehaviourDetaildetails.QId;
                behaviourdetails.Type = DB.QuaterMasters.Where(x => x.QId == behaviourdetails.QId).Select(x => x.Type).FirstOrDefault();
                behaviourdetails.QName = DB.QuaterMasters.Where(x => x.QId == behaviourdetails.QId).Select(x => x.Name).FirstOrDefault();
                behaviourdetails.PeriodId = BehaviourDetaildetails.PeriodId;
                behaviourdetails.FYear = DB.FinancialYearMasters.Where(x => x.YearId == behaviourdetails.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                behaviourdetails.EmpId = EmpId;
                behaviourdetails.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == behaviourdetails.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                behaviourdetails.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == behaviourdetails.EmpId).Select(x => x.FirstName).FirstOrDefault();
                behaviourdetails.Description = BehaviourDetaildetails.Description;
                behaviourdetails.Weightage = BehaviourDetaildetails.Weightage;
                behaviourdetails.EmpReview = BehaviourDetaildetails.EmpReview;
                behaviourdetails.ManagerReview = BehaviourDetaildetails.ManagerReview;
                behaviourdetails.IsActive = BehaviourDetaildetails.IsActive;
                behaviourdetails.IsUpdated = BehaviourDetaildetails.IsUpdated;
                behaviourdetails.IsDeleted = BehaviourDetaildetails.IsDeleted;
                behaviourdetails.CreatedBy = BehaviourDetaildetails.CreatedBy;
                behaviourdetails.CreatedDate = BehaviourDetaildetails.CreatedDate;
                behaviourdetails.LastUpdatedBy = BehaviourDetaildetails.LastUpdatedBy;
                behaviourdetails.LastUpdatedDate = BehaviourDetaildetails.LastUpdatedDate;

                if (EmpId != 0)
                {
                    if (behaviourdetails != null)
                    {
                        return behaviourdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "BehaviourDetail Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourDetailViewModel AddBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = DB.FinancialYearMasters.Where(x => x.IsActive == true && x.Status == true).Select(x => x.YearId).FirstOrDefault();

                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string Behaviour = (model.Behaviour != "" || model.Behaviour != null) ? model.Behaviour : "";

                var BehaviourDetaildetails = (from BehaviourDetail in DB.Per_BehaviourDetail
                                   where BehaviourDetail.Behaviour == Behaviour && BehaviourDetail.IsActive == true && BehaviourDetail.IsDeleted == false
                                   select BehaviourDetail).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (BehaviourDetaildetails == null)
                    {
                        Per_BehaviourDetail dm = new Per_BehaviourDetail();
                        dm.Behaviour = model.Behaviour;
                        dm.QId = (QId != 0) ? QId : 0;
                        dm.PeriodId = (FYearId != 0) ? FYearId : 0;
                        dm.BehaviourId = (model.BehaviourId != 0) ? model.BehaviourId : 0;
                        dm.EmpId = model.EmpId;
                        dm.Description = (model.Description != "") ? model.Description : "";
                        dm.Weightage = model.Weightage;
                        dm.EmpReview = (model.EmpReview != "") ? model.EmpReview : "";
                        dm.ManagerReview = (model.ManagerReview != "") ? model.ManagerReview : "";
                        dm.IsActive = true;
                        dm.IsDeleted = false;
                        dm.CreatedBy = EmpId;
                        dm.CreatedDate = DateTime.Now;
                        dm.LastUpdatedBy = EmpId;
                        dm.LastUpdatedDate = DateTime.Now;
                        DB.Per_BehaviourDetail.Add(dm);
                        DB.SaveChanges();

                        Per_BehaviourDetailViewModel bdvm = new Per_BehaviourDetailViewModel();
                        bdvm.msg = "Added";
                        bdvm.Behaviour = model.Behaviour;

                        return bdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "BehaviourDetail Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourDetailViewModel UpdateBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.BehaviourId != 0) ? model.BehaviourId : 0;
                string Behaviour = (model.Behaviour != "" || model.Behaviour != null) ? model.Behaviour : "";

                var BehaviourDetaildetails = (from BehaviourDetail in DB.Per_BehaviourDetail
                                   where BehaviourDetail.BehaviourId == id && BehaviourDetail.IsActive == true && BehaviourDetail.IsDeleted == false
                                   select BehaviourDetail).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (BehaviourDetaildetails != null)
                    {
                        BehaviourDetaildetails.Behaviour = model.Behaviour;
                        BehaviourDetaildetails.QId = (model.QId != 0) ? model.QId : 0;
                        BehaviourDetaildetails.PeriodId = (model.PeriodId != 0) ? model.PeriodId : 0;
                        BehaviourDetaildetails.BehaviourId = (model.BehaviourId != 0) ? model.BehaviourId : 0;
                        BehaviourDetaildetails.EmpId = model.EmpId;
                        BehaviourDetaildetails.Description = (model.Description != "") ? model.Description : "";
                        BehaviourDetaildetails.Weightage = model.Weightage;
                        BehaviourDetaildetails.EmpReview = (model.EmpReview != "") ? model.EmpReview : "";
                        BehaviourDetaildetails.ManagerReview = (model.ManagerReview != "") ? model.ManagerReview : "";
                        BehaviourDetaildetails.IsActive = true;
                        BehaviourDetaildetails.IsUpdated = true;
                        BehaviourDetaildetails.IsDeleted = false;
                        BehaviourDetaildetails.LastUpdatedBy = EmpId;
                        BehaviourDetaildetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_BehaviourDetailViewModel bdvm = new Per_BehaviourDetailViewModel();
                        bdvm.msg = "Updated";
                        bdvm.Behaviour = model.Behaviour;

                        return bdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "BehaviourDetail Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_BehaviourDetailViewModel DeleteBehaviourDetail(Per_BehaviourDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.BehaviourId != 0) ? model.BehaviourId : 0;
                string Behaviour = (model.Behaviour != "" || model.Behaviour != null) ? model.Behaviour : "";

                var BehaviourDetaildetails = (from BehaviourDetail in DB.Per_BehaviourDetail
                                   where BehaviourDetail.BehaviourId == id && BehaviourDetail.IsActive == true && BehaviourDetail.IsDeleted == false
                                   select BehaviourDetail).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (BehaviourDetaildetails != null)
                    {
                        BehaviourDetaildetails.IsDeleted = true;
                        BehaviourDetaildetails.LastUpdatedBy = EmpId;
                        BehaviourDetaildetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_BehaviourDetailViewModel bdvm = new Per_BehaviourDetailViewModel();
                        bdvm.msg = "Deleted";
                        bdvm.Behaviour = model.Behaviour;

                        return bdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "BehaviourDetail Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<Per_SelfDevelopmentViewModel> GetAllSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var SelfDevelopmentdetails = (from SelfDevelopment in DB.Per_SelfDevelopment
                                            where SelfDevelopment.CreatedBy == EmpId && SelfDevelopment.IsActive == true && SelfDevelopment.IsDeleted == false
                                            select SelfDevelopment).ToList();

                List<Per_SelfDevelopmentViewModel> listofSelfdevelops = new List<Per_SelfDevelopmentViewModel>();

                for (int i = 0; i < SelfDevelopmentdetails.Count(); i++)
                {
                    Per_SelfDevelopmentViewModel Selfdevelops = new Per_SelfDevelopmentViewModel();
                    Selfdevelops.Id = SelfDevelopmentdetails[i].Id;
                    Selfdevelops.Activity = SelfDevelopmentdetails[i].Activity;
                    Selfdevelops.QId = SelfDevelopmentdetails[i].QId;
                    Selfdevelops.Type = DB.QuaterMasters.Where(x => x.QId == Selfdevelops.QId).Select(x => x.Type).FirstOrDefault();
                    Selfdevelops.QName = DB.QuaterMasters.Where(x => x.QId == Selfdevelops.QId).Select(x => x.Name).FirstOrDefault();
                    Selfdevelops.PeriodId = SelfDevelopmentdetails[i].PeriodId;
                    Selfdevelops.FYear = DB.FinancialYearMasters.Where(x => x.YearId == Selfdevelops.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                    Selfdevelops.EmpId = EmpId;
                    Selfdevelops.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == Selfdevelops.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                    Selfdevelops.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == Selfdevelops.EmpId).Select(x => x.FirstName).FirstOrDefault();
                    Selfdevelops.ActionDescription = SelfDevelopmentdetails[i].ActionDescription;
                    Selfdevelops.ActionType = SelfDevelopmentdetails[i].ActionType;
                    Selfdevelops.StartDate = SelfDevelopmentdetails[i].StartDate;
                    Selfdevelops.DueDate = SelfDevelopmentdetails[i].DueDate;
                    Selfdevelops.CompletedDate = SelfDevelopmentdetails[i].CompletedDate;
                    Selfdevelops.Status = SelfDevelopmentdetails[i].Status;
                    Selfdevelops.IsActive = SelfDevelopmentdetails[i].IsActive;
                    Selfdevelops.IsUpdated = SelfDevelopmentdetails[i].IsUpdated;
                    Selfdevelops.IsDeleted = SelfDevelopmentdetails[i].IsDeleted;
                    Selfdevelops.CreatedBy = SelfDevelopmentdetails[i].CreatedBy;
                    Selfdevelops.CreatedDate = SelfDevelopmentdetails[i].CreatedDate;
                    Selfdevelops.LastUpdatedBy = SelfDevelopmentdetails[i].LastUpdatedBy;
                    Selfdevelops.LastUpdatedDate = SelfDevelopmentdetails[i].LastUpdatedDate;
                    listofSelfdevelops.Add(Selfdevelops);
                }

                if (EmpId != 0)
                {
                    if (listofSelfdevelops != null)
                    {
                        return listofSelfdevelops;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SelfDevelopment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_SelfDevelopmentViewModel GetSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                int id = (model.Id != 0) ? model.Id : 0;
                string Activity = model.Activity;
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var SelfDevelopmentdetails = (from SelfDevelopment in DB.Per_SelfDevelopment
                                             where SelfDevelopment.Id == id && SelfDevelopment.IsActive == true && SelfDevelopment.IsDeleted == false
                                             select SelfDevelopment).FirstOrDefault();

                Per_SelfDevelopmentViewModel Selfdevelops = new Per_SelfDevelopmentViewModel();
                Selfdevelops.Id = SelfDevelopmentdetails.Id;
                Selfdevelops.Activity = SelfDevelopmentdetails.Activity;
                Selfdevelops.QId = SelfDevelopmentdetails.QId;
                Selfdevelops.Type = DB.QuaterMasters.Where(x => x.QId == Selfdevelops.QId).Select(x => x.Type).FirstOrDefault();
                Selfdevelops.QName = DB.QuaterMasters.Where(x => x.QId == Selfdevelops.QId).Select(x => x.Name).FirstOrDefault();
                Selfdevelops.PeriodId = SelfDevelopmentdetails.PeriodId;
                Selfdevelops.FYear = DB.FinancialYearMasters.Where(x => x.YearId == Selfdevelops.PeriodId).Select(x => x.FinancialYear).FirstOrDefault();
                Selfdevelops.EmpId = EmpId;
                Selfdevelops.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == Selfdevelops.EmpId).Select(x => x.EmpCode).FirstOrDefault();
                Selfdevelops.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == Selfdevelops.EmpId).Select(x => x.FirstName).FirstOrDefault();
                Selfdevelops.ActionDescription = SelfDevelopmentdetails.ActionDescription;
                Selfdevelops.ActionType = SelfDevelopmentdetails.ActionType;
                Selfdevelops.StartDate = SelfDevelopmentdetails.StartDate;
                Selfdevelops.DueDate = SelfDevelopmentdetails.DueDate;
                Selfdevelops.CompletedDate = SelfDevelopmentdetails.CompletedDate;
                Selfdevelops.Status = SelfDevelopmentdetails.Status;
                Selfdevelops.IsActive = SelfDevelopmentdetails.IsActive;
                Selfdevelops.IsUpdated = SelfDevelopmentdetails.IsUpdated;
                Selfdevelops.IsDeleted = SelfDevelopmentdetails.IsDeleted;
                Selfdevelops.CreatedBy = SelfDevelopmentdetails.CreatedBy;
                Selfdevelops.CreatedDate = SelfDevelopmentdetails.CreatedDate;
                Selfdevelops.LastUpdatedBy = SelfDevelopmentdetails.LastUpdatedBy;
                Selfdevelops.LastUpdatedDate = SelfDevelopmentdetails.LastUpdatedDate;

                if (EmpId != 0)
                {
                    if (Selfdevelops != null)
                    {
                        return Selfdevelops;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SelfDevelopment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_SelfDevelopmentViewModel AddSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = DB.FinancialYearMasters.Where(x => x.IsActive == true && x.Status == true).Select(x => x.YearId).FirstOrDefault();

                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string Activity = (model.Activity != "" || model.Activity != null) ? model.Activity : "";

                var SelfDevelopmentdetails = (from SelfDevelopment in DB.Per_SelfDevelopment
                                        where SelfDevelopment.Activity == Activity && SelfDevelopment.IsActive == true && SelfDevelopment.IsDeleted == false
                                        select SelfDevelopment).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (SelfDevelopmentdetails == null)
                    {
                        Per_SelfDevelopment dm = new Per_SelfDevelopment();
                        dm.Activity = model.Activity;
                        dm.QId = (QId != 0) ? QId : 0;
                        dm.PeriodId = (FYearId != 0) ? FYearId : 0;
                        dm.ActionDescription = (model.ActionDescription != "") ? model.ActionDescription : "";
                        dm.ActionType = (model.ActionType != "") ? model.ActionType : "";
                        dm.StartDate = (model.StartDate != null) ? model.StartDate : DateTime.Now;
                        dm.DueDate = (model.DueDate != null) ? model.DueDate : DateTime.Now;
                        dm.CompletedDate = (model.CompletedDate != null) ? model.CompletedDate : DateTime.Now;
                        dm.Status = true;
                        dm.IsActive = true;
                        dm.IsDeleted = false;
                        dm.CreatedBy = EmpId;
                        dm.CreatedDate = DateTime.Now;
                        dm.LastUpdatedBy = EmpId;
                        dm.LastUpdatedDate = DateTime.Now;
                        DB.Per_SelfDevelopment.Add(dm);
                        DB.SaveChanges();

                        Per_SelfDevelopmentViewModel sdvm = new Per_SelfDevelopmentViewModel();
                        sdvm.msg = "Added";
                        sdvm.Activity = model.Activity;

                        return sdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SelfDevelopment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_SelfDevelopmentViewModel UpdateSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.Id != 0) ? model.Id : 0;
                string Activity = (model.Activity != "" || model.Activity != null) ? model.Activity : "";

                var SelfDevelopmentdetails = (from SelfDevelopment in DB.Per_SelfDevelopment
                                        where SelfDevelopment.Id == id && SelfDevelopment.IsActive == true && SelfDevelopment.IsDeleted == false
                                        select SelfDevelopment).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (SelfDevelopmentdetails != null)
                    {
                        SelfDevelopmentdetails.Activity = model.Activity;
                        SelfDevelopmentdetails.QId = (model.QId != 0) ? model.QId : 0;
                        SelfDevelopmentdetails.PeriodId = (model.PeriodId != 0) ? model.PeriodId : 0;
                        SelfDevelopmentdetails.ActionDescription = (model.ActionDescription != "") ? model.ActionDescription : "";
                        SelfDevelopmentdetails.ActionType = (model.ActionType != "") ? model.ActionType : "";
                        SelfDevelopmentdetails.StartDate = (model.StartDate != null) ? model.StartDate : DateTime.Now;
                        SelfDevelopmentdetails.DueDate = (model.DueDate != null) ? model.DueDate : DateTime.Now;
                        SelfDevelopmentdetails.CompletedDate = (model.CompletedDate != null) ? model.CompletedDate : DateTime.Now;
                        SelfDevelopmentdetails.Status = true;
                        SelfDevelopmentdetails.IsActive = true;
                        SelfDevelopmentdetails.IsUpdated = true;
                        SelfDevelopmentdetails.IsDeleted = false;
                        SelfDevelopmentdetails.LastUpdatedBy = EmpId;
                        SelfDevelopmentdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_SelfDevelopmentViewModel sdvm = new Per_SelfDevelopmentViewModel();
                        sdvm.msg = "Updated";
                        sdvm.Activity = model.Activity;

                        return sdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SelfDevelopment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_SelfDevelopmentViewModel DeleteSelfDevelopment(Per_SelfDevelopmentViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int id = (model.Id != 0) ? model.Id : 0;
                string Activity = (model.Activity != "" || model.Activity != null) ? model.Activity : "";

                var SelfDevelopmentdetails = (from SelfDevelopment in DB.Per_SelfDevelopment
                                        where SelfDevelopment.Id == id && SelfDevelopment.IsActive == true && SelfDevelopment.IsDeleted == false
                                        select SelfDevelopment).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (SelfDevelopmentdetails != null)
                    {
                        SelfDevelopmentdetails.IsDeleted = true;
                        SelfDevelopmentdetails.LastUpdatedBy = EmpId;
                        SelfDevelopmentdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        Per_SelfDevelopmentViewModel sdvm = new Per_SelfDevelopmentViewModel();
                        sdvm.msg = "Deleted";
                        sdvm.Activity = model.Activity;

                        return sdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SelfDevelopment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_EmployeeReviewViewModel SaveEmployeeReview(Per_EmployeeReviewViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = (from fan in DB.FinancialYearMasters
                                where fan.Status == true && fan.IsActive == true && fan.IsDeleted == false
                                select fan.YearId).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (model.listofGoal != null)
                    {
                        for (int i = 0; i < model.listofGoal.Count(); i++)
                        {
                            int? gid = model.listofGoal[i].GoalId;

                            var Goaldetails = (from Goal in DB.Per_Goal
                                               where Goal.GoalId == gid && Goal.EmpId == EmpId && Goal.ReviewedByEmp == false 
                                               && Goal.ReviewedByManager == false && Goal.IsActive == true && Goal.IsDeleted == false
                                               select Goal).FirstOrDefault();

                            if (Goaldetails != null)
                            {
                                Goaldetails.EmpReview = model.listofGoal[i].EmpReview;
                                Goaldetails.EDescription = model.listofGoal[i].EDescription;
                                Goaldetails.ReviewedByEmp = true;
                                Goaldetails.Status = "Emp Review Completed";
                                Goaldetails.FinalSubmit = true;
                                Goaldetails.IsActive = true;
                                Goaldetails.IsUpdated = true;
                                Goaldetails.IsDeleted = false;
                                Goaldetails.LastUpdatedBy = EmpId;
                                Goaldetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "This " + model.listofGoal[i].Goal + " Detail is Not Found");
                            }
                        }
                        
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goal Details Not Found");
                    }
                    if (model.listofBehavior != null)
                    {
                        for (int i = 0; i < model.listofBehavior.Count(); i++)
                        {
                            int? bid = model.listofBehavior[i].Id;

                            var Behaviourdetails = (from Behav in DB.Per_BehaviourDetail
                                                    where Behav.BehaviourId == bid && Behav.EmpId == EmpId && Behav.IsActive == true && Behav.IsDeleted == false
                                                    select Behav).FirstOrDefault();

                            if (Behaviourdetails != null)
                            {
                                Behaviourdetails.EmpReview = model.listofBehavior[i].EmpReview;
                                Behaviourdetails.EDescription = model.listofBehavior[i].EDescription;
                                Behaviourdetails.ReviewedByEmp = true;
                                Behaviourdetails.IsActive = true;
                                Behaviourdetails.IsUpdated = true;
                                Behaviourdetails.IsDeleted = false;
                                Behaviourdetails.LastUpdatedBy = EmpId;
                                Behaviourdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();
                            }
                            else
                            {
                                var Behaviour = (from Behav in DB.Per_BehaviourMaster
                                                 where Behav.Id == bid && Behav.IsActive == true && Behav.IsDeleted == false
                                                 select Behav).FirstOrDefault();

                                if (Behaviour != null)
                                {
                                    Per_BehaviourDetail pbd = new Per_BehaviourDetail();

                                    pbd.QId = (Behaviour.QId != 0) ? Behaviour.QId : 0;
                                    pbd.PeriodId = (Behaviour.PeriodId != 0) ? Behaviour.PeriodId : 0;
                                    pbd.EmpId = EmpId;
                                    pbd.BehaviourId = (Behaviour.Id != 0) ? Behaviour.Id : 0;
                                    pbd.Behaviour = Behaviour.Behaviour;
                                    pbd.Description = (Behaviour.Description != "") ? Behaviour.Description : "";
                                    pbd.Weightage = Behaviour.Weightage;
                                    pbd.EmpReview = model.listofBehavior[i].EmpReview;
                                    pbd.ManagerReview = "";
                                    pbd.EDescription = model.listofBehavior[i].EDescription;
                                    pbd.MDescription = "";
                                    pbd.ReviewedByEmp = true;
                                    pbd.ReviewedByManager = false;
                                    pbd.IsActive = true;
                                    pbd.IsUpdated = false;
                                    pbd.IsDeleted = false;
                                    pbd.CreatedBy = EmpId;
                                    pbd.CreatedDate = DateTime.Now;
                                    pbd.LastUpdatedBy = EmpId;
                                    pbd.LastUpdatedDate = DateTime.Now;
                                    DB.Per_BehaviourDetail.Add(pbd);
                                    DB.SaveChanges();
                                }
                            }
                        }

                        var EmpReviewdetails = (from Revw in DB.ReviewLists
                                                where Revw.QId == QId && Revw.FYearId == FYearId && Revw.EmpId == EmpId && Revw.IsActive == true && Revw.IsDeleted == false
                                                select Revw).FirstOrDefault();

                        if (EmpReviewdetails == null)
                        {
                            ReviewList rl = new ReviewList();

                            rl.FYearId = FYearId;
                            rl.QId = QId;
                            rl.EmpId = EmpId;
                            rl.QType = DB.QuaterMasters.Where(x => x.QId == QId).Select(x => x.Type).FirstOrDefault();
                            rl.Status = "Emp Review Completed";
                            rl.ReviewedByEmp = true;
                            rl.ReviewedByManager = false;
                            rl.Completed = false;
                            rl.CreatedBy = EmpId;
                            rl.CreatedDate = DateTime.Now;
                            rl.LastUpdatedBy = EmpId;
                            rl.LastUpdatedDate = DateTime.Now;
                            rl.IsActive = true;
                            rl.IsUpdated = false;
                            rl.IsDeleted = false;
                            DB.ReviewLists.Add(rl);
                            DB.SaveChanges();
                            int reviewid = rl.ReviewId;
                        }

                        Per_EmployeeReviewViewModel glvm = new Per_EmployeeReviewViewModel();
                        glvm.msg = "Employee Reviw Completed";
                        return glvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Review Details Not Found");
                    }

                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<ReviewListViewModel> GetAllEmployeeReviewList(ReviewListViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var oldEmpId = (from Emp in DB.EmployeeMasters
                                where Emp.EmpId == EmpId && Emp.IsActive == true && Emp.IsDeleted == false
                                select Emp).FirstOrDefault();

                int? OldEmp_ID = oldEmpId.OldEmp_ID;
                int? Desig_Id = oldEmpId.DesignationId;

                var Emplist = (from Emp in DB.EmployeeMasters
                               where (Emp.ReportId == OldEmp_ID || Emp.ReportId == EmpId) && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();

                if (Desig_Id == 310)
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where Emp.EmpCode.StartsWith("3DCADPU") && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                else if (Desig_Id == 1073)
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where Emp.EmpCode.StartsWith("3DCADVS") && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                else if (Desig_Id == 22 || Desig_Id == 186 || Desig_Id == 59 || Desig_Id == 191 || Desig_Id == 94)
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }
                else
                {
                    Emplist = (from Emp in DB.EmployeeMasters
                               where (Emp.ReportId == OldEmp_ID || Emp.ReportId == EmpId) && Emp.IsActive == true && Emp.IsDeleted == false
                               select Emp).ToList();
                }

                //var Emplist = (from Emp in DB.EmployeeMasters
                //               where (Emp.ReportId == OldEmp_ID || Emp.ReportId == EmpId) && Emp.IsActive == true && Emp.IsDeleted == false
                //               select Emp).ToList();

                //var Emplist = (from Emp in DB.EmployeeMasters
                //               where Emp.ReportId == EmpId && Emp.IsActive == true && Emp.IsDeleted == false
                //               select Emp).ToList();

                List<ReviewListViewModel> listReview = new List<ReviewListViewModel>();

                for (int j = 0; j < Emplist.Count(); j++)
                {
                    int? rempid = Emplist[j].EmpId;

                    var EmpReviewdetails = (from Revw in DB.ReviewLists
                                            where Revw.Completed == false && Revw.ReviewedByEmp == true
                                            && Revw.ReviewedByManager == false && Revw.EmpId == rempid && Revw.IsActive == true && Revw.IsDeleted == false
                                            select Revw).ToList();

                    if (EmpReviewdetails.Count() != 0)
                    {
                        for (int i = 0; i < EmpReviewdetails.Count(); i++)
                        {
                            ReviewListViewModel review = new ReviewListViewModel();
                            review.ReviewId = EmpReviewdetails[i].ReviewId;
                            review.FYearId = EmpReviewdetails[i].FYearId;
                            review.FYear = DB.FinancialYearMasters.Where(x => x.YearId == review.FYearId).Select(x => x.FinancialYear).FirstOrDefault();
                            review.QId = EmpReviewdetails[i].QId;
                            review.EmpId = EmpReviewdetails[i].EmpId;
                            review.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == review.EmpId).Select(x => x.FirstName).FirstOrDefault();
                            string speriod = DB.QuaterMasters.Where(x => x.QId == review.QId).Select(x => x.StartDate).FirstOrDefault();
                            string eperiod = DB.QuaterMasters.Where(x => x.QId == review.QId).Select(x => x.EndDate).FirstOrDefault();
                            review.Period = speriod + " - " + eperiod;
                            review.Status = EmpReviewdetails[i].Status;
                            review.ReviewedByEmp = EmpReviewdetails[i].ReviewedByEmp;
                            review.ReviewedByManager = EmpReviewdetails[i].ReviewedByManager;
                            review.Completed = EmpReviewdetails[i].Completed;
                            review.CreatedBy = EmpReviewdetails[i].CreatedBy;
                            review.CreatedDate = Convert.ToDateTime(EmpReviewdetails[i].CreatedDate);
                            review.LastUpdatedBy = EmpReviewdetails[i].LastUpdatedBy;
                            review.LastUpdatedDate = Convert.ToDateTime(EmpReviewdetails[i].LastUpdatedDate);
                            review.IsActive = EmpReviewdetails[i].IsActive;
                            review.IsUpdated = EmpReviewdetails[i].IsUpdated;
                            review.IsDeleted = EmpReviewdetails[i].IsDeleted;
                            listReview.Add(review);
                        }
                    }
                }


                if (EmpId != 0)
                {
                    if (listReview != null)
                    {
                        return listReview;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Review Details Not Found");
                    }
                    //if (EmpId == 3)
                    //{

                    //}
                    //else
                    //{
                    //    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is not Authorized");
                    //}
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<ReviewListViewModel> GetEmployeeReviewList(ReviewListViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var EmpReviewdetails = (from Revw in DB.ReviewLists
                                        where Revw.EmpId == EmpId && Revw.IsActive == true && Revw.IsDeleted == false
                                        select Revw).ToList();

                List<ReviewListViewModel> listReview = new List<ReviewListViewModel>();

                for (int i = 0; i < EmpReviewdetails.Count(); i++)
                {
                    ReviewListViewModel review = new ReviewListViewModel();
                    review.ReviewId = EmpReviewdetails[i].ReviewId;
                    review.FYearId = EmpReviewdetails[i].FYearId;
                    review.FYear = DB.FinancialYearMasters.Where(x => x.YearId == review.FYearId).Select(x => x.FinancialYear).FirstOrDefault();
                    review.QId = EmpReviewdetails[i].QId;
                    review.EmpId = EmpReviewdetails[i].EmpId;
                    review.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == review.EmpId).Select(x => x.FirstName).FirstOrDefault();
                    string speriod = DB.QuaterMasters.Where(x => x.QId == review.QId).Select(x => x.StartDate).FirstOrDefault();
                    string eperiod = DB.QuaterMasters.Where(x => x.QId == review.QId).Select(x => x.EndDate).FirstOrDefault();
                    review.Period = speriod + " - " + eperiod;
                    review.Status = EmpReviewdetails[i].Status;
                    review.ReviewedByEmp = EmpReviewdetails[i].ReviewedByEmp;
                    review.ReviewedByManager = EmpReviewdetails[i].ReviewedByManager;
                    review.Completed = EmpReviewdetails[i].Completed;
                    review.CreatedBy = EmpReviewdetails[i].CreatedBy;
                    review.CreatedDate = Convert.ToDateTime(EmpReviewdetails[i].CreatedDate);
                    review.LastUpdatedBy = EmpReviewdetails[i].LastUpdatedBy;
                    review.LastUpdatedDate = Convert.ToDateTime(EmpReviewdetails[i].LastUpdatedDate);
                    review.IsActive = EmpReviewdetails[i].IsActive;
                    review.IsUpdated = EmpReviewdetails[i].IsUpdated;
                    review.IsDeleted = EmpReviewdetails[i].IsDeleted;
                    listReview.Add(review);
                }

                if (EmpId != 0)
                {
                    if (listReview != null)
                    {
                        return listReview;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public Per_EmployeeReviewViewModel SaveManagerReview(Per_EmployeeReviewViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? ManagerId = (model.ManagerId != 0) ? model.ManagerId : 0;
                int? QId = DB.QuaterMasters.Where(x => x.IsActive == true).Select(x => x.QId).FirstOrDefault();
                int? FYearId = 2;

                if (EmpId != 0)
                {
                    if (model.listofGoal != null)
                    {
                        for (int i = 0; i < model.listofGoal.Count(); i++)
                        {
                            int? gid = model.listofGoal[i].GoalId;

                            var Goaldetails = (from Goal in DB.Per_Goal
                                               where Goal.GoalId == gid && Goal.ReviewedByEmp == true
                                               && Goal.ReviewedByManager == false && Goal.IsActive == true && Goal.IsDeleted == false
                                               select Goal).FirstOrDefault();

                            if (Goaldetails != null)
                            {
                                Goaldetails.ManagerReview = model.listofGoal[i].ManagerReview;
                                Goaldetails.MDescription = model.listofGoal[i].MDescription;
                                Goaldetails.ReviewedByManager = true;
                                Goaldetails.Status = "Manager Review Completed";
                                Goaldetails.FinalSubmit = true;
                                Goaldetails.IsActive = true;
                                Goaldetails.IsUpdated = true;
                                Goaldetails.IsDeleted = false;
                                Goaldetails.LastUpdatedBy = EmpId;
                                Goaldetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "This " + model.listofGoal[i].Goal + " Detail is Not Found");
                            }
                        }

                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Goals Detail Details Not Found");
                    }
                    if (model.listofBehavior != null)
                    {
                        for (int i = 0; i < model.listofBehavior.Count(); i++)
                        {
                            int? bid = model.listofBehavior[i].Id;

                            var Behaviourdetails = (from Behav in DB.Per_BehaviourDetail
                                                    where Behav.BehaviourId == bid && Behav.ReviewedByEmp == true
                                                    && Behav.ReviewedByManager == false && Behav.IsActive == true && Behav.IsDeleted == false
                                                    select Behav).FirstOrDefault();

                            var Behaviour = (from Behav in DB.Per_BehaviourMaster
                                             where Behav.Id == bid && Behav.IsActive == true && Behav.IsDeleted == false
                                             select Behav).FirstOrDefault();

                            if (Behaviourdetails != null)
                            {
                                Behaviourdetails.ManagerReview = model.listofGoal[i].ManagerReview;
                                Behaviourdetails.MDescription = model.listofGoal[i].MDescription;
                                Behaviourdetails.ReviewedByManager = true;
                                Behaviourdetails.IsActive = true;
                                Behaviourdetails.IsUpdated = true;
                                Behaviourdetails.IsDeleted = false;
                                Behaviourdetails.LastUpdatedBy = EmpId;
                                Behaviourdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();
                            }
                        }

                        var EmpReviewdetails = (from Revw in DB.ReviewLists
                                                where Revw.QId == QId && Revw.Completed == false && Revw.FYearId == FYearId &&
                                                Revw.EmpId == EmpId && Revw.IsActive == true && Revw.IsDeleted == false //&& Revw.LastUpdatedBy == ManagerId
                                                select Revw).FirstOrDefault();

                        //var EmpReviewdetails = (from Revw in DB.ReviewLists
                        //                        where Revw.QId == QId && Revw.FYearId == FYearId && Revw.EmpId == EmpId && Revw.IsActive == true && Revw.IsDeleted == false
                        //                        select Revw).FirstOrDefault();

                        if (EmpReviewdetails == null)
                        {
                            ReviewList rl = new ReviewList();

                            rl.FYearId = FYearId;
                            rl.QId = QId;
                            rl.EmpId = EmpId;
                            rl.Status = "Manager Review Completed";
                            rl.ReviewedByEmp = true;
                            rl.ReviewedByManager = true;
                            rl.Completed = true;
                            rl.CreatedBy = ManagerId;
                            rl.CreatedDate = DateTime.Now;
                            rl.LastUpdatedBy = ManagerId;
                            rl.LastUpdatedDate = DateTime.Now;
                            rl.IsActive = true;
                            rl.IsUpdated = false;
                            rl.IsDeleted = false;
                            DB.ReviewLists.Add(rl);
                            DB.SaveChanges();
                        }
                        else
                        {
                            EmpReviewdetails.Status = "Manager Review Completed";
                            EmpReviewdetails.ReviewedByManager = true;
                            EmpReviewdetails.Completed = true;
                            EmpReviewdetails.LastUpdatedBy = ManagerId;
                            EmpReviewdetails.LastUpdatedDate = DateTime.Now;
                            EmpReviewdetails.IsActive = true;
                            EmpReviewdetails.IsUpdated = true;
                            EmpReviewdetails.IsDeleted = false;
                            DB.SaveChanges();
                        }

                        Per_EmployeeReviewViewModel glvm = new Per_EmployeeReviewViewModel();
                        glvm.msg = "Manager Reviw Completed";
                        return glvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Manager Review Detail Details Not Found");
                    }

                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
    }
}