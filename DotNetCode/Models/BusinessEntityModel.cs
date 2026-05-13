using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OfficeConnect_Web.Models
{
    public class BusinessEntityModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();

        public List<CompanyMasterViewModel> GetAllCompany(CompanyMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var comdetails = (from Comp in DB.CompanyMasters 
                                  where Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        List<CompanyMasterViewModel> lstofCom = new List<CompanyMasterViewModel>();

                        for (int i = 0; i < comdetails.Count(); i++)
                        {
                            CompanyMasterViewModel cmvm = new CompanyMasterViewModel();
                            cmvm.LoginId = loginId;
                            cmvm.CompId = comdetails[i].CompId;
                            cmvm.Company = comdetails[i].Company;
                            cmvm.CompanyCode = comdetails[i].CompanyCode;
                            cmvm.LocationMap = comdetails[i].LocationMap;
                            cmvm.Address = comdetails[i].Address;
                            cmvm.CreatedBy = comdetails[i].CreatedBy;
                            cmvm.CreatedDate = comdetails[i].CreatedDate;
                            cmvm.LastUpdatedBy = comdetails[i].LastUpdatedBy;
                            cmvm.LastUpdatedDate = comdetails[i].LastUpdatedDate;
                            cmvm.IsActive = comdetails[i].IsActive;
                            cmvm.IsUpdated = comdetails[i].IsUpdated;
                            cmvm.IsDeleted = comdetails[i].IsDeleted;
                            lstofCom.Add(cmvm);
                        }

                        return lstofCom;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public CompanyMasterViewModel GetCompany(CompanyMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;

                var comdetails = (from Comp in DB.CompanyMasters
                                  where Comp.CompId == compId && Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        CompanyMasterViewModel cmvm = new CompanyMasterViewModel();
                        cmvm.LoginId = loginId;
                        cmvm.CompId = comdetails.CompId;
                        cmvm.Company = comdetails.Company;
                        cmvm.CompanyCode = comdetails.CompanyCode;
                        cmvm.LocationMap = comdetails.LocationMap;
                        cmvm.Address = comdetails.Address;
                        cmvm.CreatedBy = comdetails.CreatedBy;
                        cmvm.CreatedDate = comdetails.CreatedDate;
                        cmvm.LastUpdatedBy = comdetails.LastUpdatedBy;
                        cmvm.LastUpdatedDate = comdetails.LastUpdatedDate;
                        cmvm.IsActive = comdetails.IsActive;
                        cmvm.IsUpdated = comdetails.IsUpdated;
                        cmvm.IsDeleted = comdetails.IsDeleted;
                        return cmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddCompany(CompanyMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                string company = (model.Company != "") ? model.Company : "";
                string companycode = (model.CompanyCode != "") ? model.CompanyCode : "";

                var comdetails = (from Comp in DB.CompanyMasters
                                  where Comp.Company == company && Comp.CompanyCode == companycode && Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (comdetails.Count() == 0)
                    {
                        CompanyMaster cm = new CompanyMaster();
                        cm.Company = model.Company;
                        cm.CompanyCode = model.CompanyCode;
                        cm.LocationMap = model.LocationMap;
                        cm.Address = model.Address;
                        cm.CreatedBy = model.LoginId;
                        cm.CreatedDate = DateTime.Now;
                        cm.LastUpdatedBy = model.LoginId;
                        cm.LastUpdatedDate = DateTime.Now;
                        cm.IsActive = true;
                        cm.IsUpdated = false;
                        cm.IsDeleted = false;
                        DB.CompanyMasters.Add(cm);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdateCompany(CompanyMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;

                var comdetails = (from Comp in DB.CompanyMasters
                                  where Comp.CompId == compId && Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        comdetails.Company = model.Company;
                        comdetails.CompanyCode = model.CompanyCode;
                        comdetails.LocationMap = model.LocationMap;
                        comdetails.Address = model.Address;
                        comdetails.LastUpdatedBy = model.LoginId;
                        comdetails.LastUpdatedDate = DateTime.Now;
                        comdetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeleteCompany(CompanyMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;

                var comdetails = (from Comp in DB.CompanyMasters
                                  where Comp.CompId == compId && Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        comdetails.IsUpdated = true;
                        comdetails.IsDeleted = true;
                        comdetails.LastUpdatedBy = model.LoginId;
                        comdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel ActivateCompany(CompanyMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;

                var comdetails = (from Comp in DB.CompanyMasters
                                  where Comp.CompId == compId && Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        comdetails.IsActive = true;
                        comdetails.IsUpdated = true;
                        comdetails.LastUpdatedBy = model.LoginId;
                        comdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Activated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeActivateCompany(CompanyMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;

                var comdetails = (from Comp in DB.CompanyMasters
                                  where Comp.CompId == compId && Comp.IsDeleted == false
                                  select Comp).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        comdetails.IsActive = false;
                        comdetails.IsUpdated = true;
                        comdetails.LastUpdatedBy = model.LoginId;
                        comdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deactivated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<LegalEntityMasterViewModel> GetAllLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var ledetails = (from Comp in DB.CompanyMasters
                                  join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                  where Comp.IsActive == true && Comp.IsDeleted == false && le.IsDeleted == false
                                  select le).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (ledetails != null)
                    {
                        List<LegalEntityMasterViewModel> lstofLE = new List<LegalEntityMasterViewModel>();

                        for (int i = 0; i < ledetails.Count(); i++)
                        {
                            LegalEntityMasterViewModel levm = new LegalEntityMasterViewModel();
                            levm.LoginId = loginId;
                            levm.CompId = ledetails[i].CompId;
                            int? compid = ledetails[i].CompId;
                            string company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault() ?? "";
                            string companycode = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.CompanyCode).FirstOrDefault() ?? "";
                            levm.Company = company;
                            levm.CompanyCode = companycode;
                            levm.LEId = ledetails[i].LEId;
                            levm.LegalEntity = ledetails[i].LegalEntity;
                            levm.Description = ledetails[i].Description;
                            levm.CompanyType = ledetails[i].CompanyType;
                            levm.Logo = ledetails[i].Logo;
                            if (levm.Logo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = levm.Logo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                levm.Logo = "Uploads" + lnkval;
                            }
                            levm.LogoWithAddress = ledetails[i].LogoWithAddress;
                            if (levm.LogoWithAddress != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = levm.LogoWithAddress.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                levm.LogoWithAddress = "Uploads" + lnkval;
                            }
                            levm.WebAppLogo = ledetails[i].WebAppLogo;
                            if (levm.WebAppLogo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = levm.WebAppLogo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                levm.WebAppLogo = "Uploads" + lnkval;
                            }
                            levm.Website = ledetails[i].Website;
                            levm.CreatedBy = ledetails[i].CreatedBy;
                            levm.CreatedDate = ledetails[i].CreatedDate;
                            levm.LastUpdatedBy = ledetails[i].LastUpdatedBy;
                            levm.LastUpdatedDate = ledetails[i].LastUpdatedDate;
                            levm.IsActive = ledetails[i].IsActive;
                            levm.IsUpdated = ledetails[i].IsUpdated;
                            levm.IsDeleted = ledetails[i].IsDeleted;
                            lstofLE.Add(levm);
                        }

                        return lstofLE;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public LegalEntityMasterViewModel GetLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                var ledetails = (from Comp in DB.CompanyMasters
                                 join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && le.LEId == leId && le.IsDeleted == false
                                 select le).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (ledetails != null)
                    {
                        LegalEntityMasterViewModel levm = new LegalEntityMasterViewModel();
                        levm.LoginId = loginId;
                        levm.CompId = ledetails.CompId;
                        int? compid = ledetails.CompId;
                        string company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault() ?? "";
                        string companycode = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.CompanyCode).FirstOrDefault() ?? "";
                        levm.Company = company;
                        levm.CompanyCode = companycode;
                        levm.LEId = ledetails.LEId;
                        levm.LegalEntity = ledetails.LegalEntity;
                        levm.Description = ledetails.Description;
                        levm.CompanyType = ledetails.CompanyType;
                        levm.Logo = ledetails.Logo;
                        if (levm.Logo != "")
                        {
                            string[] stringSeparators = new string[] { "Uploads" };
                            string[] firstNames = levm.Logo.Split(stringSeparators, StringSplitOptions.None);
                            string lnkval = firstNames[1];
                            levm.Logo = "Uploads" + lnkval;
                        }
                        levm.LogoWithAddress = ledetails.LogoWithAddress;
                        if (levm.LogoWithAddress != "")
                        {
                            string[] stringSeparators = new string[] { "Uploads" };
                            string[] firstNames = levm.LogoWithAddress.Split(stringSeparators, StringSplitOptions.None);
                            string lnkval = firstNames[1];
                            levm.LogoWithAddress = "Uploads" + lnkval;
                        }
                        levm.WebAppLogo = ledetails.WebAppLogo;
                        if (levm.WebAppLogo != "")
                        {
                            string[] stringSeparators = new string[] { "Uploads" };
                            string[] firstNames = levm.WebAppLogo.Split(stringSeparators, StringSplitOptions.None);
                            string lnkval = firstNames[1];
                            levm.WebAppLogo = "Uploads" + lnkval;
                        }
                        levm.Website = ledetails.Website;
                        levm.CreatedBy = ledetails.CreatedBy;
                        levm.CreatedDate = ledetails.CreatedDate;
                        levm.LastUpdatedBy = ledetails.LastUpdatedBy;
                        levm.LastUpdatedDate = ledetails.LastUpdatedDate;
                        levm.IsActive = ledetails.IsActive;
                        levm.IsUpdated = ledetails.IsUpdated;
                        levm.IsDeleted = ledetails.IsDeleted;
                        return levm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                string legalentity = (model.LegalEntity != "") ? model.LegalEntity : "";

                var ledetails = (from Comp in DB.CompanyMasters
                                 join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && le.LegalEntity.ToUpper() == legalentity.ToUpper() && le.IsDeleted == false
                                 select le).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (ledetails.Count() == 0)
                    {
                        LegalEntityMaster lem = new LegalEntityMaster();
                        lem.CompId = model.CompId;
                        lem.LegalEntity = model.LegalEntity;
                        lem.Description = model.Description;
                        lem.CompanyType = model.CompanyType;
                        lem.Logo = (model.Logo != null) ? model.Logo : "";
                        lem.LogoWithAddress = (model.LogoWithAddress != null) ? model.LogoWithAddress : "";
                        lem.WebAppLogo = (model.WebAppLogo != null) ? model.WebAppLogo : "";
                        lem.Website = model.Website;
                        lem.CreatedBy = model.LoginId;
                        lem.CreatedDate = DateTime.Now;
                        lem.LastUpdatedBy = model.LoginId;
                        lem.LastUpdatedDate = DateTime.Now;
                        lem.IsActive = true;
                        lem.IsUpdated = false;
                        lem.IsDeleted = false;
                        DB.LegalEntityMasters.Add(lem);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdateLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                var ledetails = (from Comp in DB.CompanyMasters
                                 join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && le.LEId == leId && le.IsDeleted == false
                                 select le).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (ledetails != null)
                    {
                        ledetails.CompId = model.CompId;
                        ledetails.LegalEntity = model.LegalEntity;
                        ledetails.Description = model.Description;
                        ledetails.CompanyType = model.CompanyType;
                        ledetails.Logo = (model.Logo != null) ? model.Logo : "";
                        ledetails.LogoWithAddress = (model.LogoWithAddress != null) ? model.LogoWithAddress : "";
                        ledetails.WebAppLogo = (model.WebAppLogo != null) ? model.WebAppLogo : "";
                        ledetails.Website = model.Website;
                        ledetails.LastUpdatedBy = model.LoginId;
                        ledetails.LastUpdatedDate = DateTime.Now;
                        ledetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeleteLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                var ledetails = (from Comp in DB.CompanyMasters
                                 join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && le.LEId == leId && le.IsDeleted == false
                                 select le).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (ledetails != null)
                    {
                        ledetails.IsUpdated = true;
                        ledetails.IsDeleted = true;
                        ledetails.LastUpdatedBy = model.LoginId;
                        ledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel ActivateLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                var ledetails = (from Comp in DB.CompanyMasters
                                 join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && le.LEId == leId && le.IsDeleted == false
                                 select le).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (ledetails != null)
                    {
                        ledetails.IsActive = true;
                        ledetails.IsUpdated = true;
                        ledetails.LastUpdatedBy = model.LoginId;
                        ledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Activated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeActivateLegalEntity(LegalEntityMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                var ledetails = (from Comp in DB.CompanyMasters
                                 join le in DB.LegalEntityMasters on Comp.CompId equals le.CompId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && le.LEId == leId && le.IsDeleted == false
                                 select le).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (ledetails != null)
                    {
                        ledetails.IsActive = false;
                        ledetails.IsUpdated = true;
                        ledetails.LastUpdatedBy = model.LoginId;
                        ledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deactivated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leagal Entity Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<BusinessUnitMasterViewModel> GetAllBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var bumdetails = (from Comp in DB.CompanyMasters
                                 join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                 join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                 where Comp.IsActive == true && Comp.IsDeleted == false && 
                                 Le.IsActive == true && Le.IsDeleted == false &&
                                 Bum.IsDeleted == false
                                 select Bum).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (bumdetails != null)
                    {
                        List<BusinessUnitMasterViewModel> lstofBUM = new List<BusinessUnitMasterViewModel>();

                        for (int i = 0; i < bumdetails.Count(); i++)
                        {
                            BusinessUnitMasterViewModel bumvm = new BusinessUnitMasterViewModel();
                            bumvm.LoginId = loginId;
                            bumvm.CompId = bumdetails[i].CompId;
                            int? compid = bumdetails[i].CompId;
                            string company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault() ?? "";
                            string companycode = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.CompanyCode).FirstOrDefault() ?? "";
                            bumvm.Company = company;
                            bumvm.CompanyCode = companycode;
                            bumvm.LEId = bumdetails[i].LEId;
                            int? leid = bumdetails[i].LEId;
                            string le = DB.LegalEntityMasters.Where(x => x.LEId == leid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LegalEntity).FirstOrDefault() ?? "";
                            bumvm.LegalEntity = le;
                            bumvm.BUId = bumdetails[i].BUId;
                            bumvm.BusinessUnit = bumdetails[i].BusinessUnit;
                            bumvm.Description = bumdetails[i].Description;
                            bumvm.CreatedBy = bumdetails[i].CreatedBy;
                            bumvm.CreatedDate = bumdetails[i].CreatedDate;
                            bumvm.LastUpdatedBy = bumdetails[i].LastUpdatedBy;
                            bumvm.LastUpdatedDate = bumdetails[i].LastUpdatedDate;
                            bumvm.IsActive = bumdetails[i].IsActive;
                            bumvm.IsUpdated = bumdetails[i].IsUpdated;
                            bumvm.IsDeleted = bumdetails[i].IsDeleted;
                            lstofBUM.Add(bumvm);
                        }

                        return lstofBUM;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessUnitMasterViewModel GetBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? buid = (model.BUId != 0) ? model.BUId : 0;

                var bumdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  Bum.BUId == buid && Bum.IsDeleted == false
                                  select Bum).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (bumdetails != null)
                    {
                        BusinessUnitMasterViewModel bumvm = new BusinessUnitMasterViewModel();
                        bumvm.LoginId = loginId;
                        bumvm.CompId = bumdetails.CompId;
                        int? compid = bumdetails.CompId;
                        string company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault() ?? "";
                        string companycode = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.CompanyCode).FirstOrDefault() ?? "";
                        bumvm.Company = company;
                        bumvm.CompanyCode = companycode;
                        bumvm.LEId = bumdetails.LEId;
                        int? leid = bumdetails.LEId;
                        string le = DB.LegalEntityMasters.Where(x => x.LEId == leid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.LegalEntity).FirstOrDefault() ?? "";
                        bumvm.LegalEntity = le;
                        bumvm.BUId = bumdetails.BUId;
                        bumvm.BusinessUnit = bumdetails.BusinessUnit;
                        bumvm.Description = bumdetails.Description;
                        bumvm.CreatedBy = bumdetails.CreatedBy;
                        bumvm.CreatedDate = bumdetails.CreatedDate;
                        bumvm.LastUpdatedBy = bumdetails.LastUpdatedBy;
                        bumvm.LastUpdatedDate = bumdetails.LastUpdatedDate;
                        bumvm.IsActive = bumdetails.IsActive;
                        bumvm.IsUpdated = bumdetails.IsUpdated;
                        bumvm.IsDeleted = bumdetails.IsDeleted;
                        return bumvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                string businessunit = (model.BusinessUnit != "") ? model.BusinessUnit : "";

                var bumdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  Bum.BusinessUnit.ToUpper() == businessunit.ToUpper() && Bum.IsDeleted == false
                                  select Bum).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (bumdetails.Count() == 0)
                    {
                        BusinessUnitMaster lem = new BusinessUnitMaster();
                        lem.CompId = model.CompId;
                        lem.LEId = model.LEId;
                        lem.BusinessUnit = model.BusinessUnit;
                        lem.Description = model.Description;
                        lem.CreatedBy = model.LoginId;
                        lem.CreatedDate = DateTime.Now;
                        lem.LastUpdatedBy = model.LoginId;
                        lem.LastUpdatedDate = DateTime.Now;
                        lem.IsActive = true;
                        lem.IsUpdated = false;
                        lem.IsDeleted = false;
                        DB.BusinessUnitMasters.Add(lem);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdateBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? buid = (model.BUId != 0) ? model.BUId : 0;

                var bumdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  Bum.BUId == buid && Bum.IsDeleted == false
                                  select Bum).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (bumdetails != null)
                    {
                        bumdetails.CompId = model.CompId;
                        bumdetails.LEId = model.LEId;
                        bumdetails.BusinessUnit = model.BusinessUnit;
                        bumdetails.Description = model.Description;
                        bumdetails.LastUpdatedBy = model.LoginId;
                        bumdetails.LastUpdatedDate = DateTime.Now;
                        bumdetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeleteBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? buid = (model.BUId != 0) ? model.BUId : 0;

                var bumdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  Bum.BUId == buid && Bum.IsDeleted == false
                                  select Bum).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (bumdetails != null)
                    {
                        bumdetails.IsUpdated = true;
                        bumdetails.IsDeleted = true;
                        bumdetails.LastUpdatedBy = model.LoginId;
                        bumdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel ActivateBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? buid = (model.BUId != 0) ? model.BUId : 0;

                var bumdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  Bum.BUId == buid && Bum.IsDeleted == false
                                  select Bum).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (bumdetails != null)
                    {
                        bumdetails.IsActive = true;
                        bumdetails.IsUpdated = true;
                        bumdetails.LastUpdatedBy = model.LoginId;
                        bumdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Activated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeActivateBusinessUnit(BusinessUnitMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? buid = (model.BUId != 0) ? model.BUId : 0;

                var bumdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  Bum.BUId == buid && Bum.IsDeleted == false
                                  select Bum).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (bumdetails != null)
                    {
                        bumdetails.IsActive = false;
                        bumdetails.IsUpdated = true;
                        bumdetails.LastUpdatedBy = model.LoginId;
                        bumdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deactivated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<LocationMasterViewModel> GetAllLocation(LocationMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (locdetails != null)
                    {
                        List<LocationMasterViewModel> lstofLOC = new List<LocationMasterViewModel>();

                        for (int i = 0; i < locdetails.Count(); i++)
                        {
                            LocationMasterViewModel lmvm = new LocationMasterViewModel();
                            lmvm.LoginId = loginId;
                            lmvm.CompId = locdetails[i].CompId;
                            int? compid = locdetails[i].CompId;
                            string company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault() ?? "";
                            string companycode = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.CompanyCode).FirstOrDefault() ?? "";
                            lmvm.Company = company;
                            lmvm.CompanyCode = companycode;
                            lmvm.LEId = locdetails[i].LEId;
                            int? leid = locdetails[i].LEId;
                            string le = DB.LegalEntityMasters.Where(x => x.LEId == leid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LegalEntity).FirstOrDefault() ?? "";
                            lmvm.LegalEntity = le;
                            lmvm.BUId = locdetails[i].BUId;
                            int? buid = locdetails[i].BUId;
                            string bu = DB.BusinessUnitMasters.Where(x => x.BUId == buid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.BusinessUnit).FirstOrDefault() ?? "";
                            lmvm.BusinessUnit = bu;
                            lmvm.LocationId = locdetails[i].LocationId;
                            lmvm.Location = locdetails[i].Location;
                            lmvm.Description = locdetails[i].Description;
                            lmvm.LocationMap = locdetails[i].LocationMap;
                            lmvm.Address = locdetails[i].Address;
                            lmvm.City = locdetails[i].City;
                            lmvm.State = locdetails[i].State;
                            lmvm.Country = locdetails[i].Country;
                            lmvm.PostalCode = locdetails[i].PostalCode;
                            lmvm.TimeZone = locdetails[i].TimeZone;
                            lmvm.ProbationPeriod = locdetails[i].ProbationPeriod;
                            lmvm.WeeklyHoliday = locdetails[i].WeeklyHoliday;
                            lmvm.CompanyRegNo = locdetails[i].CompanyRegNo;
                            lmvm.DateofReg = locdetails[i].DateofReg;
                            lmvm.PFNo = locdetails[i].PFNo;
                            lmvm.ESINo = locdetails[i].ESINo;
                            lmvm.TANNo = locdetails[i].TANNo;
                            lmvm.VATNo = locdetails[i].VATNo;
                            lmvm.PANNo = locdetails[i].PANNo;
                            lmvm.ServiceTaxNo = locdetails[i].ServiceTaxNo;
                            lmvm.GSTNo = locdetails[i].GSTNo;
                            lmvm.CreatedBy = locdetails[i].CreatedBy;
                            lmvm.CreatedDate = locdetails[i].CreatedDate;
                            lmvm.LastUpdatedBy = locdetails[i].LastUpdatedBy;
                            lmvm.LastUpdatedDate = locdetails[i].LastUpdatedDate;
                            lmvm.IsActive = locdetails[i].IsActive;
                            lmvm.IsUpdated = locdetails[i].IsUpdated;
                            lmvm.IsDeleted = locdetails[i].IsDeleted;
                            lstofLOC.Add(lmvm);
                        }

                        return lstofLOC;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public LocationMasterViewModel GetLocation(LocationMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? locid = (model.LocationId != 0) ? model.LocationId : 0;

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.LocationId == locid && Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (locdetails != null)
                    {
                        LocationMasterViewModel lmvm = new LocationMasterViewModel();
                        lmvm.LoginId = loginId;
                        lmvm.CompId = locdetails.CompId;
                        int? compid = locdetails.CompId;
                        string company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault() ?? "";
                        string companycode = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.CompanyCode).FirstOrDefault() ?? "";
                        lmvm.Company = company;
                        lmvm.CompanyCode = companycode;
                        lmvm.LEId = locdetails.LEId;
                        int? leid = locdetails.LEId;
                        string le = DB.LegalEntityMasters.Where(x => x.LEId == leid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.LegalEntity).FirstOrDefault() ?? "";
                        lmvm.LegalEntity = le;
                        lmvm.BUId = locdetails.BUId;
                        int? buid = locdetails.BUId;
                        string bu = DB.BusinessUnitMasters.Where(x => x.BUId == buid && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.BusinessUnit).FirstOrDefault() ?? "";
                        lmvm.BusinessUnit = bu;
                        lmvm.LocationId = locdetails.LocationId;
                        lmvm.Location = locdetails.Location;
                        lmvm.Description = locdetails.Description;
                        lmvm.LocationMap = locdetails.LocationMap;
                        lmvm.Address = locdetails.Address;
                        lmvm.City = locdetails.City;
                        lmvm.State = locdetails.State;
                        lmvm.Country = locdetails.Country;
                        lmvm.PostalCode = locdetails.PostalCode;
                        lmvm.TimeZone = locdetails.TimeZone;
                        lmvm.ProbationPeriod = locdetails.ProbationPeriod;
                        lmvm.WeeklyHoliday = locdetails.WeeklyHoliday;
                        lmvm.CompanyRegNo = locdetails.CompanyRegNo;
                        lmvm.DateofReg = locdetails.DateofReg;
                        lmvm.PFNo = locdetails.PFNo;
                        lmvm.ESINo = locdetails.ESINo;
                        lmvm.TANNo = locdetails.TANNo;
                        lmvm.VATNo = locdetails.VATNo;
                        lmvm.PANNo = locdetails.PANNo;
                        lmvm.ServiceTaxNo = locdetails.ServiceTaxNo;
                        lmvm.GSTNo = locdetails.GSTNo;
                        lmvm.CreatedBy = locdetails.CreatedBy;
                        lmvm.CreatedDate = locdetails.CreatedDate;
                        lmvm.LastUpdatedBy = locdetails.LastUpdatedBy;
                        lmvm.LastUpdatedDate = locdetails.LastUpdatedDate;
                        lmvm.IsActive = locdetails.IsActive;
                        lmvm.IsUpdated = locdetails.IsUpdated;
                        lmvm.IsDeleted = locdetails.IsDeleted;
                        return lmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddLocation(LocationMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                string location = (model.Location != "") ? model.Location : "";

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.Location == location && Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (locdetails.Count() == 0)
                    {
                        LocationMaster lem = new LocationMaster();
                        lem.CompId = model.CompId;
                        lem.LEId = model.LEId;
                        lem.BUId = model.BUId;
                        lem.Location = model.Location;
                        lem.Description = model.Description;
                        lem.LocationMap = model.LocationMap;
                        lem.Address = model.Address;
                        lem.City = model.City;
                        lem.State = model.State;
                        lem.Country = model.Country;
                        lem.PostalCode = model.PostalCode;
                        lem.TimeZone = model.TimeZone;
                        lem.ProbationPeriod = model.ProbationPeriod;
                        lem.WeeklyHoliday = model.WeeklyHoliday;
                        lem.CompanyRegNo = model.CompanyRegNo;
                        lem.DateofReg = model.DateofReg;
                        lem.PFNo = model.PFNo;
                        lem.ESINo = model.ESINo;
                        lem.TANNo = model.TANNo;
                        lem.VATNo = model.VATNo;
                        lem.PANNo = model.PANNo;
                        lem.ServiceTaxNo = model.ServiceTaxNo;
                        lem.GSTNo = model.GSTNo;
                        lem.CreatedBy = model.LoginId;
                        lem.CreatedDate = DateTime.Now;
                        lem.LastUpdatedBy = model.LoginId;
                        lem.LastUpdatedDate = DateTime.Now;
                        lem.IsActive = true;
                        lem.IsUpdated = false;
                        lem.IsDeleted = false;
                        DB.LocationMasters.Add(lem);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdateLocation(LocationMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? locid = (model.LocationId != 0) ? model.LocationId : 0;

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.LocationId == locid && Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (locdetails != null)
                    {
                        locdetails.CompId = model.CompId;
                        locdetails.LEId = model.LEId;
                        locdetails.BUId = model.BUId;
                        locdetails.Location = model.Location;
                        locdetails.Description = model.Description;
                        locdetails.LocationMap = model.LocationMap;
                        locdetails.Address = model.Address;
                        locdetails.City = model.City;
                        locdetails.State = model.State;
                        locdetails.Country = model.Country;
                        locdetails.PostalCode = model.PostalCode;
                        locdetails.TimeZone = model.TimeZone;
                        locdetails.ProbationPeriod = model.ProbationPeriod;
                        locdetails.WeeklyHoliday = model.WeeklyHoliday;
                        locdetails.CompanyRegNo = model.CompanyRegNo;
                        locdetails.DateofReg = model.DateofReg;
                        locdetails.PFNo = model.PFNo;
                        locdetails.ESINo = model.ESINo;
                        locdetails.TANNo = model.TANNo;
                        locdetails.VATNo = model.VATNo;
                        locdetails.PANNo = model.PANNo;
                        locdetails.ServiceTaxNo = model.ServiceTaxNo;
                        locdetails.GSTNo = model.GSTNo;
                        locdetails.LastUpdatedBy = model.LoginId;
                        locdetails.LastUpdatedDate = DateTime.Now;
                        locdetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeleteLocation(LocationMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? locid = (model.LocationId != 0) ? model.LocationId : 0;

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.LocationId == locid && Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (locdetails != null)
                    {
                        locdetails.IsUpdated = true;
                        locdetails.IsDeleted = true;
                        locdetails.LastUpdatedBy = model.LoginId;
                        locdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel ActivateLocation(LocationMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? locid = (model.LocationId != 0) ? model.LocationId : 0;

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.LocationId == locid && Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (locdetails != null)
                    {
                        locdetails.IsActive = true;
                        locdetails.IsUpdated = true;
                        locdetails.LastUpdatedBy = model.LoginId;
                        locdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Activated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeActivateLocation(LocationMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? locid = (model.LocationId != 0) ? model.LocationId : 0;

                var locdetails = (from Comp in DB.CompanyMasters
                                  join Le in DB.LegalEntityMasters on Comp.CompId equals Le.CompId
                                  //join Bum in DB.BusinessUnitMasters on Le.LEId equals Bum.LEId
                                  join Loc in DB.LocationMasters on Le.LEId equals Loc.LEId
                                  where Comp.IsActive == true && Comp.IsDeleted == false &&
                                  Le.IsActive == true && Le.IsDeleted == false &&
                                  //Bum.IsActive == true && Bum.IsDeleted == false &&
                                  Loc.LocationId == locid && Loc.IsDeleted == false
                                  select Loc).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (locdetails != null)
                    {
                        locdetails.IsActive = false;
                        locdetails.IsUpdated = true;
                        locdetails.LastUpdatedBy = model.LoginId;
                        locdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deactivated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<CalendarYearMasterViewModel> GetAllCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var CYdetails = (from fin in DB.FinanceMasters
                                  where fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                  select fin).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (CYdetails != null)
                    {
                        List<CalendarYearMasterViewModel> lstofCY = new List<CalendarYearMasterViewModel>();

                        for (int i = 0; i < CYdetails.Count(); i++)
                        {
                            CalendarYearMasterViewModel cymvm = new CalendarYearMasterViewModel();
                            cymvm.LoginId = loginId;
                            cymvm.Id = CYdetails[i].Id;
                            cymvm.Year = CYdetails[i].Year;
                            cymvm.Status = CYdetails[i].Status;
                            cymvm.CreatedBy = CYdetails[i].CreatedBy;
                            cymvm.CreatedDate = CYdetails[i].CreatedDate;
                            cymvm.LastUpdatedBy = CYdetails[i].LastUpdatedBy;
                            cymvm.LastUpdatedDate = CYdetails[i].LastUpdatedDate;
                            cymvm.IsActive = CYdetails[i].IsActive;
                            cymvm.IsUpdated = CYdetails[i].IsUpdated;
                            cymvm.IsDeleted = CYdetails[i].IsDeleted;
                            lstofCY.Add(cymvm);
                        }

                        return lstofCY;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Calendar Year Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public CalendarYearMasterViewModel GetCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.Id != 0) ? model.Id : 0;

                var CYdetails = (from fin in DB.FinanceMasters
                                 where fin.Id == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (CYdetails != null)
                    {
                        CalendarYearMasterViewModel cymvm = new CalendarYearMasterViewModel();
                        cymvm.LoginId = loginId;
                        cymvm.Id = CYdetails.Id;
                        cymvm.Year = CYdetails.Year;
                        cymvm.Status = CYdetails.Status;
                        cymvm.CreatedBy = CYdetails.CreatedBy;
                        cymvm.CreatedDate = CYdetails.CreatedDate;
                        cymvm.LastUpdatedBy = CYdetails.LastUpdatedBy;
                        cymvm.LastUpdatedDate = CYdetails.LastUpdatedDate;
                        cymvm.IsActive = CYdetails.IsActive;
                        cymvm.IsUpdated = CYdetails.IsUpdated;
                        cymvm.IsDeleted = CYdetails.IsDeleted;
                        return cymvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Calendar Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? CalendarYear = (model.Year != 0) ? model.Year : 0;

                var CYdetails = (from fin in DB.FinanceMasters
                                 where fin.Year == CalendarYear && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (CYdetails.Count() == 0)
                    {
                        FinanceMaster fm = new FinanceMaster();
                        //fm.LoginId = loginId;
                        fm.Year = model.Year;
                        fm.Status = true;
                        fm.CreatedBy = model.LoginId;
                        fm.CreatedDate = DateTime.Now;
                        fm.LastUpdatedBy = model.LoginId;
                        fm.LastUpdatedDate = DateTime.Now;
                        fm.IsActive = true;
                        fm.IsUpdated = false;
                        fm.IsDeleted = false;
                        DB.FinanceMasters.Add(fm);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Calendar Year Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdateCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.Id != 0) ? model.Id : 0;

                var CYdetails = (from fin in DB.FinanceMasters
                                 where fin.Id == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (CYdetails != null)
                    {
                        CYdetails.Year = model.Year;
                        CYdetails.Status = true;
                        CYdetails.LastUpdatedBy = model.LoginId;
                        CYdetails.LastUpdatedDate = DateTime.Now;
                        CYdetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Calendar Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeleteCalendarYear(CalendarYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.Id != 0) ? model.Id : 0;

                var CYdetails = (from fin in DB.FinanceMasters
                                 where fin.Id == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (CYdetails != null)
                    {
                        CYdetails.IsUpdated = true;
                        CYdetails.IsDeleted = true;
                        CYdetails.LastUpdatedBy = model.LoginId;
                        CYdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Calendar Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<FinancialYearMasterViewModel> GetAllFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var FYdetails = (from fin in DB.FinancialYearMasters
                                 where fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (FYdetails != null)
                    {
                        List<FinancialYearMasterViewModel> lstofCY = new List<FinancialYearMasterViewModel>();

                        for (int i = 0; i < FYdetails.Count(); i++)
                        {
                            FinancialYearMasterViewModel cymvm = new FinancialYearMasterViewModel();
                            cymvm.LoginId = loginId;
                            cymvm.YearId = FYdetails[i].YearId;
                            cymvm.FinancialYear = FYdetails[i].FinancialYear;
                            cymvm.Status = FYdetails[i].Status;
                            cymvm.CreatedBy = FYdetails[i].CreatedBy;
                            cymvm.CreatedDate = FYdetails[i].CreatedDate;
                            cymvm.LastUpdatedBy = FYdetails[i].LastUpdatedBy;
                            cymvm.LastUpdatedDate = FYdetails[i].LastUpdatedDate;
                            cymvm.IsActive = FYdetails[i].IsActive;
                            cymvm.IsUpdated = FYdetails[i].IsUpdated;
                            cymvm.IsDeleted = FYdetails[i].IsDeleted;
                            lstofCY.Add(cymvm);
                        }

                        return lstofCY;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public FinancialYearMasterViewModel GetFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.YearId != 0) ? model.YearId : 0;

                var FyDetails = (from fin in DB.FinancialYearMasters
                                 where fin.YearId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FinancialYearMasterViewModel cymvm = new FinancialYearMasterViewModel();
                        cymvm.LoginId = loginId;
                        cymvm.YearId = FyDetails.YearId;
                        cymvm.FinancialYear = FyDetails.FinancialYear;
                        cymvm.Status = FyDetails.Status;
                        cymvm.CreatedBy = FyDetails.CreatedBy;
                        cymvm.CreatedDate = FyDetails.CreatedDate;
                        cymvm.LastUpdatedBy = FyDetails.LastUpdatedBy;
                        cymvm.LastUpdatedDate = FyDetails.LastUpdatedDate;
                        cymvm.IsActive = FyDetails.IsActive;
                        cymvm.IsUpdated = FyDetails.IsUpdated;
                        cymvm.IsDeleted = FyDetails.IsDeleted;
                        return cymvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                string FinancialYear = (model.FinancialYear != "") ? model.FinancialYear : "";

                var FyDetails = (from fin in DB.FinancialYearMasters
                                 where fin.FinancialYear == FinancialYear && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (FyDetails.Count() == 0)
                    {
                        FinancialYearMaster fym = new FinancialYearMaster();
                        //fm.LoginId = loginId;
                        fym.FinancialYear = model.FinancialYear;
                        fym.Status = true;
                        fym.CreatedBy = model.LoginId;
                        fym.CreatedDate = DateTime.Now;
                        fym.LastUpdatedBy = model.LoginId;
                        fym.LastUpdatedDate = DateTime.Now;
                        fym.IsActive = true;
                        fym.IsUpdated = false;
                        fym.IsDeleted = false;
                        DB.FinancialYearMasters.Add(fym);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdateFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.YearId != 0) ? model.YearId : 0;

                var FyDetails = (from fin in DB.FinancialYearMasters
                                 where fin.YearId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FyDetails.FinancialYear = model.FinancialYear;
                        FyDetails.Status = true;
                        FyDetails.LastUpdatedBy = model.LoginId;
                        FyDetails.LastUpdatedDate = DateTime.Now;
                        FyDetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeleteFinancialYear(FinancialYearMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.YearId != 0) ? model.YearId : 0;

                var FyDetails = (from fin in DB.FinancialYearMasters
                                 where fin.YearId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FyDetails.IsUpdated = true;
                        FyDetails.IsDeleted = true;
                        FyDetails.LastUpdatedBy = model.LoginId;
                        FyDetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Financial Year Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
    }
}