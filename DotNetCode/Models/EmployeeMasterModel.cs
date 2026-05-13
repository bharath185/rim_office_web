using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace OfficeConnect_Web.Models
{
    public class EmployeeMasterModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();
        //LogsModel logsmodel = new LogsModel();

        //public List<EmployeeMasterViewModel> GetEmployee(EmployeeMasterViewModel Emp)
        //{

        //    var userdetails = (from user in DB.EmployeeMasters
        //                       where user.IsActive == true
        //                       select new EmployeeMasterViewModel
        //                       {
        //                           EmpId = user.EmpId,
        //                           CompId = user.CompId,
        //                           CategoryId = user.CategoryId,
        //                           DesignationId = user.DesignationId,
        //                           EmpCode = user.EmpCode,
        //                           UserName = user.UserName,
        //                           EmpStatus = user.EmpStatus,
        //                           TokenId = ObjAuth.GetJwt(user.UserName),
        //                       }).ToList();

        //    if (userdetails == null)
        //    {
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            Content = new StringContent(string.Format("No Employee found with ID = {0}")),
        //            ReasonPhrase = "Employee Not Found"
        //        };

        //        throw new HttpResponseException(response);
        //    }
        //    else
        //    {
        //        return userdetails;
        //    }
        //}
        public List<DDCompanyViewModel> GetDDCompany(DDCompanyViewModel compdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (compdd.EmpId != 0) ? compdd.EmpId : 0;

                var Compdetails = (from comp in DB.CompanyMasters
                                   where comp.IsActive == true && comp.IsDeleted == false
                                   select new DDCompanyViewModel
                                   {
                                       CompId = comp.CompId,
                                       Company = comp.Company,
                                       CompanyCode = comp.CompanyCode,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (Compdetails != null)
                    {
                        return Compdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
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
        public List<DDLegalEntityViewModel> GetDDLegalEntity(DDLegalEntityViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;
                string authorisedEntity = (model.AuthorisedEntity != null) ? model.AuthorisedEntity : null;

                var authorisedEntities = model.AuthorisedEntity?
                                            .Split(',')
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                var Legaldetails = (from le in DB.LegalEntityMasters
                                    where authorisedEntities.Contains(le.LEId)
                                          && le.IsActive == true && le.IsDeleted == false
                                    select new DDLegalEntityViewModel
                                    {
                                        CompId = le.CompId,
                                        LEId = le.LEId,
                                        LegalEntity = le.LegalEntity,
                                    }).ToList();

                if (CompId != 0)
                {
                    Legaldetails = (from le in DB.LegalEntityMasters
                                    where authorisedEntities.Contains(le.LEId)
                                          && le.CompId == CompId && le.IsActive == true && le.IsDeleted == false
                                    select new DDLegalEntityViewModel
                                    {
                                        CompId = le.CompId,
                                        LEId = le.LEId,
                                        LegalEntity = le.LegalEntity,
                                    }).ToList();
                }

                if (EmpId != 0)
                {
                    if (Legaldetails != null)
                    {
                        return Legaldetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Legal Entity Details Not Found");
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
        public List<DDAuthorisedEntityViewModel> DDAuthorisedEntity(DDAuthorisedEntityViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string authorisedEntity = (model.AuthorisedEntity != null) ? model.AuthorisedEntity : null;

                var authorisedEntities = model.AuthorisedEntity?
                                            .Split(',')
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                var legalDetails = (from le in DB.LegalEntityMasters
                                    where ////authorisedEntities.Contains(le.LEId) && 
                                    le.IsActive == true && le.IsDeleted == false
                                    select new DDAuthorisedEntityViewModel
                                    {
                                        LEId = le.LEId,
                                        LegalEntity = le.LegalEntity
                                    }).ToList();


                if (EmpId != 0)
                {
                    if (legalDetails != null)
                    {
                        return legalDetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Authorised Legal Entity Details Not Found");
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
        public List<DDBusinessUnitViewModel> GetDDBusinessUnit(DDBusinessUnitViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;
                string authorisedEntity = (model.AuthorisedEntity != null) ? model.AuthorisedEntity : null;

                var authorisedEntities = model.AuthorisedEntity?
                                            .Split(',')
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                var BUnitdetails = (from bu in DB.BusinessUnitMasters
                                    where bu.LEId.HasValue
                                             && authorisedEntities.Contains(bu.LEId.Value)
                                          && bu.IsActive == true && bu.IsDeleted == false
                                    select new DDBusinessUnitViewModel
                                    {
                                        CompId = bu.CompId,
                                        LEId = bu.LEId,
                                        BUId = bu.BUId,
                                        BusinessUnit = bu.BusinessUnit,
                                    }).ToList();

                if (CompId != 0)
                {
                    BUnitdetails = (from bu in DB.BusinessUnitMasters
                                    where bu.LEId.HasValue
                                            && authorisedEntities.Contains(bu.LEId.Value)
                                          && bu.CompId == CompId && bu.LEId == LEId && bu.IsActive == true && bu.IsDeleted == false
                                    select new DDBusinessUnitViewModel
                                    {
                                        CompId = bu.CompId,
                                        LEId = bu.LEId,
                                        BUId = bu.BUId,
                                        BusinessUnit = bu.BusinessUnit,
                                    }).ToList();
                }

                if (EmpId != 0)
                {
                    if (BUnitdetails != null)
                    {
                        return BUnitdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
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
        public List<DDLocationViewModel> GetDDLocation(DDLocationViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;
                int? BUId = (model.BUId != 0) ? model.BUId : 0;
                string authorisedEntity = (model.AuthorisedEntity != null) ? model.AuthorisedEntity : null;

                var authorisedEntities = model.AuthorisedEntity?
                                            .Split(',')
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                var Locationdetails = (from lm in DB.LocationMasters
                                       where lm.LEId.HasValue
                                            && authorisedEntities.Contains(lm.LEId.Value)
                                          && lm.IsActive == true && lm.IsDeleted == false
                                       select new DDLocationViewModel
                                       {
                                           CompId = lm.CompId,
                                           LEId = lm.LEId,
                                           BUId = lm.BUId,
                                           LocationId = lm.LocationId,
                                           Location = lm.Location,
                                       }).ToList();

                if (CompId != 0)
                {
                    ////Locationdetails = (from lm in DB.LocationMasters
                    ////                   where lm.LEId.HasValue
                    ////                        && authorisedEntities.Contains(lm.LEId.Value)
                    ////                      && lm.CompId == CompId && lm.LEId == LEId && lm.IsActive == true && lm.IsDeleted == false
                    ////                   select new DDLocationViewModel
                    ////                   {
                    ////                       CompId = lm.CompId,
                    ////                       LEId = lm.LEId,
                    ////                       BUId = lm.BUId,
                    ////                       LocationId = lm.LocationId,
                    ////                       Location = lm.Location,
                    ////                   }).ToList();
                    ///

                    Locationdetails = Locationdetails.Where(x => x.CompId == CompId).ToList();
                }
                if (LEId != 0)
                {
                    Locationdetails = Locationdetails.Where(x => x.LEId == LEId).ToList();
                }
                if (BUId != 0)
                {
                    Locationdetails = Locationdetails.Where(x => x.BUId == BUId).ToList();
                }

                if (EmpId != 0)
                {
                    if (Locationdetails != null)
                    {
                        return Locationdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
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
        public List<NewDDCompanyViewModel> GetNewDDCompany(NewDDCompanyViewModel compdd)
        {
            try
            {
                string msg = "";
                int? LoginId = (compdd.LoginId != 0) ? compdd.LoginId : 0;

                var Compdetails = (from comp in DB.CompanyMasters
                                   where comp.IsActive == true && comp.IsDeleted == false
                                   select new NewDDCompanyViewModel
                                   {
                                       CompId = comp.CompId,
                                       Company = comp.Company,
                                       CompanyCode = comp.CompanyCode,
                                   }).ToList();

                if (LoginId != 0)
                {
                    if (Compdetails != null)
                    {
                        return Compdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
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
        public List<NewDDLegalEntityViewModel> GetNewDDLegalEntity(NewDDLegalEntityViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;

                var Legaldetails = (from le in DB.LegalEntityMasters
                                    where le.IsActive == true && le.IsDeleted == false
                                    select new NewDDLegalEntityViewModel
                                    {
                                        CompId = le.CompId,
                                        LEId = le.LEId,
                                        LegalEntity = le.LegalEntity,
                                    }).ToList();

                if (CompId != 0)
                {
                    Legaldetails = Legaldetails.Where(x => x.CompId == CompId).ToList();
                }


                if (LoginId != 0)
                {
                    if (Legaldetails != null)
                    {
                        return Legaldetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Legal Entity Details Not Found");
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
        
        public List<NewDDBusinessUnitViewModel> GetNewDDBusinessUnit(NewDDBusinessUnitViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;

                var BUnitdetails = (from bu in DB.BusinessUnitMasters
                                    where bu.IsActive == true && bu.IsDeleted == false
                                    select new NewDDBusinessUnitViewModel
                                    {
                                        CompId = bu.CompId,
                                        LEId = bu.LEId,
                                        BUId = bu.BUId,
                                        BusinessUnit = bu.BusinessUnit,
                                    }).ToList();

                if (CompId != 0)
                {
                    BUnitdetails = BUnitdetails.Where(x => x.CompId == CompId).ToList();
                }
                if (LEId != 0)
                {
                    BUnitdetails = BUnitdetails.Where(x => x.LEId == LEId).ToList();
                }

                if (LoginId != 0)
                {
                    if (BUnitdetails != null)
                    {
                        return BUnitdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Business Unit Details Not Found");
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
        public List<NewDDLocationViewModel> GetNewDDLocation(NewDDLocationViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;

                var Locationdetails = (from lm in DB.LocationMasters
                                       where lm.IsActive == true && lm.IsDeleted == false
                                       select new NewDDLocationViewModel
                                       {
                                           CompId = lm.CompId,
                                           LEId = lm.LEId,
                                           LocationId = lm.LocationId,
                                           Location = lm.Location,
                                       }).ToList();

                if (CompId != 0)
                {
                    Locationdetails = Locationdetails.Where(x => x.CompId == CompId).ToList();
                }
                if (LEId != 0)
                {
                    Locationdetails = Locationdetails.Where(x => x.LEId == LEId).ToList();
                }

                if (LoginId != 0)
                {
                    if (Locationdetails != null)
                    {
                        return Locationdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
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
        public List<DDSaluationViewModel> DDSalutation(DDSaluationViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Saluationdetails = (from sm in DB.SalutationMasters
                                        where sm.IsActive == true && sm.IsDeleted == false
                                        select new DDSaluationViewModel
                                        {
                                            SalutationId = sm.SalutationId,
                                            Salutation = sm.Salutation,
                                        }).ToList();
                if (EmpId != 0)
                {
                    if (Saluationdetails != null)
                    {
                        return Saluationdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Saluation Details Not Found");
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
        public List<DDGenderViewModel> DDGender(DDGenderViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Genderdetails = (from gm in DB.GenderMasters
                                     where gm.IsActive == true && gm.IsDeleted == false
                                     select new DDGenderViewModel
                                     {
                                         GenderId = gm.GenderId,
                                         Gender = gm.Gender,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Genderdetails != null)
                    {
                        return Genderdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Gender Details Not Found");
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
        public List<DDEmpTypeViewModel> DDEmpType(DDEmpTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var EmpTypedetails = (from etm in DB.EmpTypeMasters
                                      where etm.IsActive == true && etm.IsDeleted == false
                                      select new DDEmpTypeViewModel
                                      {
                                          EmpTypeId = etm.EmpTypId,
                                          EmpType = etm.EmpType,
                                      }).ToList();

                if (EmpId != 0)
                {
                    if (EmpTypedetails != null)
                    {
                        return EmpTypedetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
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
        public List<DDApproverViewModel> DDApprover(DDApproverViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? CompId = (model.CompId != 0) ? model.CompId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;
                int? BUId = (model.BUId != 0) ? model.BUId : 0;
                int? LocationId = (model.LocationId != 0) ? model.LocationId : 0;


                var Approverdetails = (from em in DB.EmployeeMasters
                                       where em.CompId == CompId //&& em.LEId == LEId && em.BUId == BUId && em.LocationId == LocationId 
                                       && em.IsActive == true && em.IsDeleted == false && em.EmpStatus.ToUpper() == "ACTIVE"
                                       select new DDApproverViewModel
                                       {
                                           ApproverId = em.EmpId,
                                           Approver = em.FirstName + " " + em.MiddleName + " " + em.LastName + " - " + em.EmpCode,
                                       }).ToList();

                if (EmpId != 0)
                {
                    if (Approverdetails != null)
                    {
                        return Approverdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
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
        public FetchEmployeeViewModel FetchEmployee(FetchEmployeeViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                var empdetails1 = (from emp in DB.EmployeeDetails
                                   where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                   select emp).FirstOrDefault();

                var accdetails = (from acc in DB.EmployeeAccDetails
                                  where acc.EmpId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).OrderByDescending(x => x.AccId).FirstOrDefault();

                var docdetails = (from govt in DB.EmployeeGovtDocs
                                  join doc in DB.DocumentMasters on govt.DocId equals doc.DocId
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 2 && govt.EmpId == id && govt.IsActive == true && govt.IsDeleted == false
                                  select govt).OrderByDescending(x => x.GovId).ToList();

                var docdetails1 = (from edu in DB.EmployeeEducations
                                   join doc in DB.DocumentMasters on edu.DocId equals doc.DocId
                                   where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 1 && edu.EmpId == id && edu.IsActive == true && edu.IsDeleted == false
                                   select edu).OrderByDescending(x => x.Id).ToList();

                var Careerdetails = (from car in DB.EmployeeCareerDetails
                                     where car.EmpId == id && car.IsActive == true && car.IsDeleted == false
                                     select car).OrderByDescending(x => x.CareerId).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        FetchEmployeeViewModel emvm = new FetchEmployeeViewModel();
                        emvm.EmpId = empdetails.EmpId;
                        emvm.OldEmp_ID = empdetails.OldEmp_ID;
                        emvm.CompId = empdetails.CompId;
                        emvm.Company = DB.CompanyMasters.Where(x => x.CompId == emvm.CompId).Select(x => x.Company).FirstOrDefault();
                        emvm.LEId = (empdetails.LEId != 0) ? empdetails.LEId : 0;
                        emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                        emvm.BUId = (empdetails.BUId != 0) ? empdetails.BUId : 0;
                        emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                        emvm.LocationId = (empdetails.LocationId != 0) ? empdetails.LocationId : 0;
                        emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                        emvm.CategoryId = empdetails.CategoryId;
                        emvm.DeptId = empdetails.CategoryId;
                        emvm.DeptName = empdetails.DeptName;
                        emvm.DesignationId = empdetails.DesignationId;
                        emvm.Designation = empdetails.DesignationName;
                        emvm.ReportId = empdetails.ReportId;
                        emvm.ApproverId = empdetails.ReportId;
                        emvm.Approver = "";
                        if (emvm.ReportId != 0)
                        {
                            emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                            (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                            (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                            (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                        }
                        emvm.EmpCode = empdetails.EmpCode;
                        emvm.UserName = empdetails.UserName;
                        emvm.Photo = empdetails.Photo;
                        emvm.Salutation = empdetails.Salutation;
                        emvm.FirstName = empdetails.FirstName;
                        emvm.MiddleName = empdetails.MiddleName;
                        emvm.LastName = empdetails.LastName;
                        emvm.DOB = empdetails.DOB;
                        emvm.MobileNo = empdetails.MobileNo;
                        emvm.EmailId = empdetails.EmailId;
                        emvm.BloodGroup = empdetails.BloodGroup;
                        emvm.MaritalStatus = empdetails.MaritalStatus;
                        emvm.Gender = empdetails.Gender;
                        emvm.JoiningDate = empdetails.JoiningDate;
                        emvm.EndDate = empdetails.EndDate;
                        emvm.EmpStatus = empdetails.EmpStatus;
                        emvm.Reason = empdetails.Reason;
                        emvm.EmpTypeId = (empdetails.EmpType != null) ? empdetails.EmpType : 0;
                        emvm.EmpType = (emvm.EmpTypeId != 0) ? DB.EmpTypeMasters.Where(x => x.EmpTypId == emvm.EmpTypeId).Select(x => x.EmpType).FirstOrDefault() : "";
                        emvm.CEndDate = empdetails.CEndDate;
                        emvm.IsActive = empdetails.IsActive;
                        emvm.IsUpdated = empdetails.IsUpdated;
                        emvm.IsDeleted = empdetails.IsDeleted;
                        emvm.CreatedBy = empdetails.CreatedBy;
                        emvm.CreatedDate = empdetails.CreatedDate;
                        emvm.LastUpdatedBy = empdetails.LastUpdatedBy;
                        emvm.LastUpdatedDate = empdetails.LastUpdatedDate;

                        emvm.Id = empdetails1.Id;
                        emvm.AMobileNo = empdetails1.AMobileNo;
                        emvm.PMailId = empdetails1.PMailId;
                        emvm.FatherName = empdetails1.FatherName;
                        emvm.MotherName = empdetails1.MotherName;
                        emvm.HusbandName = empdetails1.HusbandName;
                        emvm.FContactNo = empdetails1.FContactNo;
                        emvm.MContactNo = empdetails1.MContactNo;
                        emvm.HContactNo = empdetails1.HContactNo;
                        emvm.EContactName = empdetails1.FContactNo;
                        emvm.EContactNo = empdetails1.MContactNo;
                        emvm.EContactRelationship = empdetails1.HContactNo;
                        emvm.Height = empdetails1.Height;
                        emvm.Weight = empdetails1.Weight;
                        emvm.DateOfAnniversary = empdetails1.DateOfAnniversary;
                        emvm.Disability = empdetails1.Disability;
                        emvm.TotalExperience = empdetails1.TotalExperience;
                        emvm.RelevantExperience = empdetails1.RelevantExperience;
                        emvm.ECActivities = empdetails1.ECActivities;
                        emvm.Sports = empdetails1.Sports;
                        emvm.CurrentBuildingName = empdetails1.CurrentBuildingName;
                        emvm.CurrentCity = empdetails1.CurrentCity;
                        emvm.CurrentCountry = empdetails1.CurrentCountry;
                        emvm.CurrentDoorNumber = empdetails1.CurrentDoorNumber;
                        emvm.CurrentLocation = empdetails1.CurrentLocation;
                        emvm.CurrentPinCode = empdetails1.CurrentPinCode;
                        emvm.CurrentState = empdetails1.CurrentState;
                        emvm.CurrentStreet = empdetails1.CurrentStreet;
                        emvm.PermanentBuildingName = empdetails1.PermanentBuildingName;
                        emvm.PermanentCity = empdetails1.PermanentCity;
                        emvm.PermanentCountry = empdetails1.PermanentCountry;
                        emvm.PermanentDoorNumber = empdetails1.PermanentDoorNumber;
                        emvm.PermanentLocation = empdetails1.PermanentLocation;
                        emvm.PermanentPinCode = empdetails1.PermanentPinCode;
                        emvm.PermanentState = empdetails1.PermanentState;
                        emvm.PermanentStreet = empdetails1.PermanentStreet;
                        emvm.Caste = empdetails1.Caste;
                        emvm.Region = empdetails1.Region;
                        emvm.Country = empdetails1.Country;
                        emvm.Nationality = empdetails1.Nationality;

                        emvm.AccId = accdetails.AccId;
                        emvm.BankName = accdetails.BankName;
                        emvm.IFSCCode = accdetails.IFSCCode;
                        emvm.BranchName = accdetails.BranchName;
                        emvm.AccHolderName = accdetails.AccHolderName;
                        emvm.AccNo = accdetails.AccNo;
                        emvm.PFNo = accdetails.PFNo;
                        emvm.MobileNo = accdetails.MobileNo;
                        emvm.AccStatus = accdetails.Status;

                        if (docdetails != null)
                        {
                            for (int i = 0; i < docdetails.Count(); i++)
                            {
                                EmployeeGovtDocViewModel egdvm = new EmployeeGovtDocViewModel();
                                egdvm.GovId = docdetails[i].GovId;
                                egdvm.EmpId = docdetails[i].EmpId;
                                egdvm.DocId = docdetails[i].DocId;
                                egdvm.Others = docdetails[i].Others;
                                egdvm.DocName = docdetails[i].DocName;
                                egdvm.Name = docdetails[i].Name;
                                egdvm.DocNo = docdetails[i].DocNo;
                                egdvm.IssuedDate = docdetails[i].IssuedDate;
                                egdvm.ExpiredDate = docdetails[i].ExpiredDate;
                                egdvm.Description = docdetails[i].Description;
                                egdvm.Path = docdetails[i].Path;

                                if (egdvm.Path != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = egdvm.Path.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    egdvm.Path = "Uploads" + lnkval;
                                }

                                egdvm.CreatedBy = docdetails[i].CreatedBy;
                                egdvm.CreatedDate = docdetails[i].CreatedDate;
                                egdvm.LastUpdatedBy = docdetails[i].LastUpdatedBy;
                                egdvm.LastUpdatedDate = docdetails[i].LastUpdatedDate;
                                egdvm.IsActive = docdetails[i].IsActive;
                                egdvm.IsUpdated = docdetails[i].IsUpdated;
                                egdvm.IsDeleted = docdetails[i].IsDeleted;
                                emvm.lstEmpGovtDoc.Add(egdvm);
                            }
                        }

                        if (docdetails1 != null)
                        {
                            for (int i = 0; i < docdetails.Count(); i++)
                            {
                                EmployeeEducationViewModel eevm = new EmployeeEducationViewModel();
                                eevm.Id = docdetails1[i].Id;
                                eevm.EmpId = docdetails1[i].EmpId;
                                eevm.DocId = docdetails1[i].DocId;
                                eevm.Others = docdetails1[i].Others;
                                eevm.School = docdetails1[i].School;
                                eevm.DegreeId = docdetails1[i].DegreeId;
                                eevm.Filed = docdetails1[i].Filed;
                                eevm.StartDate = docdetails1[i].StartDate;
                                eevm.EndDate = docdetails1[i].EndDate;
                                eevm.Grade = docdetails1[i].Grade;
                                eevm.Description = docdetails1[i].Description;
                                eevm.Path = docdetails1[i].Path;

                                if (eevm.Path != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = eevm.Path.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    eevm.Path = "Uploads" + lnkval;
                                }

                                eevm.CreatedBy = docdetails1[i].CreatedBy;
                                eevm.CreatedDate = docdetails1[i].CreatedDate;
                                eevm.LastUpdatedBy = docdetails1[i].LastUpdatedBy;
                                eevm.LastUpdatedDate = docdetails1[i].LastUpdatedDate;
                                eevm.IsActive = docdetails1[i].IsActive;
                                eevm.IsUpdated = docdetails1[i].IsUpdated;
                                eevm.IsDeleted = docdetails1[i].IsDeleted;
                                emvm.lstEmpEduDoc.Add(eevm);

                            }
                        }
                        if (Careerdetails != null)
                        {
                            for (int i = 0; i < Careerdetails.Count(); i++)
                            {
                                EmployeeCareerDetailViewModel ecdvm = new EmployeeCareerDetailViewModel();
                                ecdvm.CareerId = Careerdetails[i].CareerId;
                                ecdvm.EmpId = Careerdetails[i].EmpId;
                                ecdvm.Company = Careerdetails[i].Company;
                                ecdvm.Designation = Careerdetails[i].Designation;
                                ecdvm.FromDate = Careerdetails[i].FromDate;
                                ecdvm.ToDate = Careerdetails[i].ToDate;
                                ecdvm.Experience = Careerdetails[i].Experience;
                                ecdvm.PMonth1 = Careerdetails[i].PMonth1;
                                ecdvm.PaySlip1 = Careerdetails[i].PaySlip1;
                                ecdvm.PMonth2 = Careerdetails[i].PMonth2;
                                ecdvm.PaySlip2 = Careerdetails[i].PaySlip2;
                                ecdvm.PMonth3 = Careerdetails[i].PMonth3;
                                ecdvm.PaySlip3 = Careerdetails[i].PaySlip3;
                                ecdvm.OfferLetter = Careerdetails[i].OfferLetter;
                                ecdvm.SalaryLetter = Careerdetails[i].SalaryLetter;
                                ecdvm.ExperienceLetter = Careerdetails[i].ExperienceLetter;
                                ecdvm.RelievingLetter = Careerdetails[i].RelievingLetter;
                                ecdvm.ContactName = Careerdetails[i].ContactName;
                                ecdvm.ContactDesignation = Careerdetails[i].ContactDesignation;
                                ecdvm.ContactEmail = Careerdetails[i].ContactEmail;
                                ecdvm.ContactMobile = Careerdetails[i].ContactMobile;
                                ecdvm.CTC = Careerdetails[i].CTC;
                                ecdvm.Reason = Careerdetails[i].Reason;

                                if (ecdvm.PaySlip1 != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.PaySlip1.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.PaySlip1 = "Uploads" + lnkval;
                                }
                                if (ecdvm.PaySlip2 != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.PaySlip2.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.PaySlip2 = "Uploads" + lnkval;
                                }
                                if (ecdvm.PaySlip3 != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.PaySlip3.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.PaySlip3 = "Uploads" + lnkval;
                                }
                                if (ecdvm.OfferLetter != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.OfferLetter.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.OfferLetter = "Uploads" + lnkval;
                                }
                                if (ecdvm.SalaryLetter != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.SalaryLetter.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.SalaryLetter = "Uploads" + lnkval;
                                }
                                if (ecdvm.ExperienceLetter != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.ExperienceLetter.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.ExperienceLetter = "Uploads" + lnkval;
                                }
                                if (ecdvm.RelievingLetter != "")
                                {
                                    string[] stringSeparators = new string[] { "Uploads" };
                                    string[] firstNames = ecdvm.RelievingLetter.Split(stringSeparators, StringSplitOptions.None);
                                    string lnkval = firstNames[1];
                                    ecdvm.RelievingLetter = "Uploads" + lnkval;
                                }
                                ecdvm.CreatedBy = Careerdetails[i].CreatedBy;
                                ecdvm.CreatedDate = Careerdetails[i].CreatedDate;
                                ecdvm.LastUpdatedBy = Careerdetails[i].LastUpdatedBy;
                                ecdvm.LastUpdatedDate = Careerdetails[i].LastUpdatedDate;
                                ecdvm.IsActive = Careerdetails[i].IsActive;
                                ecdvm.IsUpdated = Careerdetails[i].IsUpdated;
                                ecdvm.IsDeleted = Careerdetails[i].IsDeleted;
                                emvm.lstEmpCareerDoc.Add(ecdvm);

                            }
                        }
                        return emvm;
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
        public responseViewModel PCAddAllEmployee(List<ProjectConnectUserViewModel> model)
        {
            try
            {
                string msg = "";
                int? loginId = (model[0].LoginId != 0) ? model[0].LoginId : 0;

                if (loginId == 11321)
                {
                    for (int i = 0; i < model.Count(); i++)
                    {
                        int? empid = model[i].EmpId;
                        string username = model[i].UserName;

                        var empdetails = (from pcu in DB.ProjectConnectUsers
                                          where pcu.EmpId == empid && pcu.UserName.ToUpper() == username.ToUpper() &&
                                          pcu.IsActive == true && pcu.IsDeleted == false && pcu.IsTransffered == true
                                          select pcu).OrderByDescending(x => x.EmpId).ToList();

                        if (empdetails.Count() == 0)
                        {
                            ProjectConnectUser pc = new ProjectConnectUser();
                            pc.EmpId = empid;
                            pc.UserName = username;
                            pc.CreatedBy = loginId;
                            pc.CreatedDate = DateTime.Now;
                            pc.IsTransffered = true;
                            pc.IsActive = true;
                            pc.IsUpdated = false;
                            pc.IsDeleted = false;
                            DB.ProjectConnectUsers.Add(pc);
                            DB.SaveChanges();
                        }
                    }

                    responseViewModel rvm = new responseViewModel();
                    rvm.status = 200;
                    rvm.msg = "UPDATED";

                    return rvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Login Id is mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<EmployeeMasterViewModel> PCGetAllEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
                                  join pcu in DB.ProjectConnectUsers
                                      on emp.EmpId equals pcu.EmpId into empPcuJoin
                                  from pcu in empPcuJoin.DefaultIfEmpty() // left join
                                  where emp.IsActive == true && emp.IsDeleted == false && emp.EmpStatus.ToUpper() == "ACTIVE"
                                        && (pcu == null || (pcu.EmpId != emp.EmpId && pcu.UserName != emp.UserName && pcu.IsTransffered == true)) // filter not in ProjectConnectUsers
                                  select emp)
                                 .OrderBy(x => x.FirstName).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<EmployeeMasterViewModel> lstofEmp = new List<EmployeeMasterViewModel>();

                        for (int i = 0; i < empdetails.Count(); i++)
                        {
                            EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                            emvm.EmpId = empdetails[i].EmpId;
                            emvm.OldEmp_ID = empdetails[i].OldEmp_ID;
                            emvm.CompId = empdetails[i].CompId;
                            int? compId = empdetails[i].CompId;
                            emvm.Company = (compId != 0) ? DB.CompanyMasters.Where(x => x.CompId == compId).Select(x => x.Company).FirstOrDefault() : "";
                            emvm.LEId = (empdetails[i].LEId != 0) ? empdetails[i].LEId : 0;
                            int? leid = (empdetails[i].LEId != 0) ? empdetails[i].LEId : 0;
                            emvm.LegalEntity = (leid != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == leid).Select(x => x.LegalEntity).FirstOrDefault() : "";
                            emvm.BUId = (empdetails[i].BUId != 0) ? empdetails[i].BUId : 0;
                            int? buid = (empdetails[i].BUId != 0) ? empdetails[i].BUId : 0;
                            emvm.BusinessUnit = (buid != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == buid).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                            emvm.LocationId = (empdetails[i].LocationId != 0) ? empdetails[i].LocationId : 0;
                            int? locationid = (empdetails[i].LocationId != 0) ? empdetails[i].LocationId : 0;
                            emvm.Location = (locationid != 0) ? DB.LocationMasters.Where(x => x.LocationId == locationid).Select(x => x.Location).FirstOrDefault() : "";
                            emvm.CategoryId = empdetails[i].CategoryId;
                            emvm.DeptId = empdetails[i].CategoryId;
                            emvm.DeptName = empdetails[i].DeptName;
                            emvm.DesignationId = empdetails[i].DesignationId;
                            emvm.Designation = empdetails[i].DesignationName;
                            emvm.ReportId = empdetails[i].ReportId;
                            int? reportid = empdetails[i].ReportId;
                            emvm.ApproverId = empdetails[i].ReportId;
                            emvm.Approver = "";
                            if (emvm.ReportId != 0)
                            {
                                emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault());
                            }
                            emvm.EmpCode = empdetails[i].EmpCode;
                            emvm.UserName = empdetails[i].UserName;
                            emvm.Photo = (empdetails[i].Photo != null) ? empdetails[i].Photo : "";
                            string photo = (empdetails[i].Photo != null) ? empdetails[i].Photo : "";
                            if (photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                emvm.Photo = "Uploads" + lnkval;
                            }
                            emvm.SalutationId = (empdetails[i].Salutation != null) ? empdetails[i].Salutation : 0;
                            int? salutationId = (empdetails[i].Salutation != null) ? empdetails[i].Salutation : 0;
                            emvm.Salutation = (salutationId != 0) ? DB.SalutationMasters.Where(x => x.SalutationId == salutationId).Select(x => x.Salutation).FirstOrDefault() : "";
                            emvm.FirstName = empdetails[i].FirstName;
                            emvm.MiddleName = empdetails[i].MiddleName;
                            emvm.LastName = empdetails[i].LastName;
                            emvm.DOB = empdetails[i].DOB;
                            emvm.MobileNo = empdetails[i].MobileNo;
                            emvm.EmailId = empdetails[i].EmailId;
                            emvm.BloodGroup = empdetails[i].BloodGroup;
                            emvm.MaritalStatus = empdetails[i].MaritalStatus;
                            emvm.Gender = empdetails[i].Gender;
                            emvm.JoiningDate = empdetails[i].JoiningDate;
                            emvm.EndDate = empdetails[i].EndDate;
                            emvm.EmpStatus = empdetails[i].EmpStatus;
                            emvm.Reason = empdetails[i].Reason;
                            emvm.EmpTypeId = (empdetails[i].EmpType != null) ? empdetails[i].EmpType : 0;
                            int? emptypeid = (empdetails[i].EmpType != null) ? empdetails[i].EmpType : 0;
                            emvm.EmpType = (emptypeid != 0) ? DB.EmpTypeMasters.Where(x => x.EmpTypId == emptypeid).Select(x => x.EmpType).FirstOrDefault() : "";
                            emvm.CEndDate = empdetails[i].CEndDate;
                            emvm.IsActive = empdetails[i].IsActive;
                            emvm.IsUpdated = empdetails[i].IsUpdated;
                            emvm.IsDeleted = empdetails[i].IsDeleted;
                            emvm.CreatedBy = empdetails[i].CreatedBy;
                            emvm.CreatedDate = empdetails[i].CreatedDate;
                            emvm.LastUpdatedBy = empdetails[i].LastUpdatedBy;
                            emvm.LastUpdatedDate = empdetails[i].LastUpdatedDate;
                            lstofEmp.Add(emvm);
                        }

                        return lstofEmp;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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
        public List<EmployeeMasterViewModel> GetAllEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locId = (model.LocationId != 0) ? model.LocationId : 0;
                int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? designationId = (model.DesignationId != 0) ? model.DesignationId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;
                ////int? empTypeId = (model.EmpTypeId != 0) ? model.EmpTypeId : 0;
                int? empTypeId = model?.EmpTypeId != null && model.EmpTypeId != 0 ? model.EmpTypeId : 0;
                DateTime? fromdate = model.FromDate;
                DateTime? todate = model.ToDate;
                string status = (model.Status != null) ? model.Status : "";

                var Loginempdetails = (from emp in DB.EmployeeMasters
                                       where emp.EmpId == loginId && emp.IsActive == true && emp.IsDeleted == false //&& emp.EmpStatus.ToUpper() == "ACTIVE"
                                       select emp).FirstOrDefault();

                int? LcompId = Loginempdetails.CompId != 0 ? Loginempdetails.CompId : 0;
                //int? Lleid = Loginempdetails.LEId ?? 0;
                //int? Lbuid = Loginempdetails.BUId ?? 0;
                int? Llocid = Loginempdetails.LocationId != 0 ? Loginempdetails.LocationId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  join Comp in DB.CompanyMasters on emp.CompId equals Comp.CompId
                                  where emp.IsActive == true && emp.IsDeleted == false //&& emp.EmpStatus.ToUpper() == "ACTIVE"
                                  select emp).OrderBy(x => x.FirstName).ToList();

                if (compId != 0)
                {
                    var compfilter = empdetails.Where(x => x.CompId == compId).ToList();
                    empdetails = compfilter.ToList();
                }
                if (leId != 0)
                {
                    var lefilter = empdetails.Where(x => x.LEId == leId).ToList();
                    empdetails = lefilter.ToList();
                }
                if (buId != 0)
                {
                    var bufilter = empdetails.Where(x => x.BUId == buId).ToList();
                    empdetails = bufilter.ToList();
                }
                if (locId != 0)
                {
                    var locfilter = empdetails.Where(x => x.LocationId == locId).ToList();
                    empdetails = locfilter.ToList();
                }
                if (deptId != 0)
                {
                    var deptfilter = empdetails.Where(x => x.CategoryId == deptId).ToList();
                    empdetails = deptfilter.ToList();


                }
                if (designationId != 0)
                {
                    var desgfilter = empdetails.Where(x => x.DesignationId == designationId).ToList();
                    empdetails = desgfilter.ToList();
                }
                if (empTypeId != 0)
                {
                    var empTypefilter = empdetails.Where(x => x.EmpType == empTypeId).ToList();
                    empdetails = empTypefilter.ToList();
                }
                if (empId != 0)
                {
                    var empfilter = empdetails.Where(x => x.EmpId == empId).ToList();
                    empdetails = empfilter.ToList();
                }
                if (fromdate != null && todate != null)
                {
                    if (status.ToUpper() == "JOINED")
                    {
                        var empfilter = empdetails.Where(x => x.JoiningDate >= fromdate && x.JoiningDate <= todate).ToList();
                        empdetails = empfilter.ToList();
                    }
                    else if (status.ToUpper() == "RELIEVED")
                    {
                        var empfilter = empdetails.Where(x => x.RelievedDate >= fromdate && x.RelievedDate <= todate).ToList();
                        empdetails = empfilter.ToList();
                    }
                    ////else if (status.ToUpper() == "BOTH")
                    ////{

                    ////}
                }
                if (status.ToUpper() == "JOINED")
                {
                    var empfilter = empdetails.Where(x => x.JoiningDate != null).ToList();
                    empdetails = empfilter.ToList();
                }
                else if (status.ToUpper() == "RELIEVED")
                {
                    var empfilter = empdetails.Where(x => x.RelievedDate != null).ToList();
                    empdetails = empfilter.ToList();
                }

                ////if (loginId != 149)
                ////{
                ////    if (LcompId != 0)
                ////    {
                ////        var compfilter = empdetails.Where(x => x.CompId == LcompId).ToList();
                ////        empdetails = compfilter.ToList();
                ////    }
                ////    ////if (Lleid != 0)
                ////    ////{
                ////    ////    empdetails = empdetails.Where(x => x.LEId == Lleid).ToList();
                ////    ////}
                ////    ////if (Lbuid != 0)
                ////    ////{
                ////    ////    empdetails = empdetails.Where(x => x.BUId == Lbuid).ToList();
                ////    ////}
                ////    if (Llocid != 0)
                ////    {
                ////        var locfilter = empdetails.Where(x => x.LocationId == Llocid).ToList();
                ////        empdetails = locfilter.ToList();
                ////    }
                ////}

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<EmployeeMasterViewModel> lstofEmp = new List<EmployeeMasterViewModel>();

                        for (int i = 0; i < empdetails.Count(); i++)
                        {
                            EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                            emvm.EmpId = empdetails[i].EmpId;
                            emvm.OldEmp_ID = empdetails[i].OldEmp_ID;
                            emvm.CompId = empdetails[i].CompId;
                            int? CompId = empdetails[i].CompId;
                            emvm.Company = (CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == CompId).Select(x => x.Company).FirstOrDefault() : "";
                            emvm.LEId = (empdetails[i].LEId != 0) ? empdetails[i].LEId : 0;
                            int? leid = (empdetails[i].LEId != 0) ? empdetails[i].LEId : 0;
                            emvm.LegalEntity = (leid != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == leid).Select(x => x.LegalEntity).FirstOrDefault() : "";
                            emvm.BUId = (empdetails[i].BUId != 0) ? empdetails[i].BUId : 0;
                            int? buid = (empdetails[i].BUId != 0) ? empdetails[i].BUId : 0;
                            emvm.BusinessUnit = (buid != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == buid).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                            emvm.LocationId = (empdetails[i].LocationId != 0) ? empdetails[i].LocationId : 0;
                            int? locationid = (empdetails[i].LocationId != 0) ? empdetails[i].LocationId : 0;
                            emvm.Location = (locationid != 0) ? DB.LocationMasters.Where(x => x.LocationId == locationid).Select(x => x.Location).FirstOrDefault() : "";
                            emvm.CategoryId = empdetails[i].CategoryId;
                            emvm.DeptId = empdetails[i].CategoryId;
                            emvm.DeptName = empdetails[i].DeptName;
                            emvm.DesignationId = empdetails[i].DesignationId;
                            emvm.Designation = empdetails[i].DesignationName;
                            emvm.ReportId = empdetails[i].ReportId;
                            emvm.ReportEmpCode = "";
                            emvm.ReportEmpName = "";
                            int? reportid = empdetails[i].ReportId;
                            emvm.ApproverId = empdetails[i].ReportId;
                            emvm.Approver = "";
                            if (emvm.ReportId != 0)
                            {
                                emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault());
                                emvm.ReportEmpCode = (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.LastName).FirstOrDefault());
                                emvm.ReportEmpName = (DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault());
                            }
                            emvm.EmpCode = empdetails[i].EmpCode;
                            emvm.UserName = empdetails[i].UserName;
                            emvm.Photo = (empdetails[i].Photo != null) ? empdetails[i].Photo : "";
                            string photo = (empdetails[i].Photo != null) ? empdetails[i].Photo : "";
                            if (photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                emvm.Photo = "Uploads" + lnkval;
                            }
                            emvm.SalutationId = (empdetails[i].Salutation != null) ? empdetails[i].Salutation : 0;
                            int? salutationId = (empdetails[i].Salutation != null) ? empdetails[i].Salutation : 0;
                            emvm.Salutation = (salutationId != 0) ? DB.SalutationMasters.Where(x => x.SalutationId == salutationId).Select(x => x.Salutation).FirstOrDefault() : "";
                            emvm.FirstName = empdetails[i].FirstName;
                            emvm.MiddleName = empdetails[i].MiddleName;
                            emvm.LastName = empdetails[i].LastName;
                            emvm.DOB = empdetails[i].DOB;
                            emvm.MobileNo = empdetails[i].MobileNo;
                            emvm.EmailId = empdetails[i].EmailId;
                            emvm.BloodGroup = empdetails[i].BloodGroup;
                            emvm.MaritalStatus = empdetails[i].MaritalStatus;
                            emvm.Gender = empdetails[i].Gender;
                            emvm.InterviewDate = empdetails[i].InterviewDate;
                            emvm.JoiningDate = empdetails[i].JoiningDate;
                            emvm.RelievedDate = empdetails[i].RelievedDate;
                            emvm.RelievedReason = empdetails[i].RelievedReason;
                            emvm.RelievedEffectiveDate = empdetails[i].RelievedEffectiveDate;
                            emvm.IsRelieved = empdetails[i].IsRelieved;
                            emvm.EndDate = empdetails[i].EndDate;
                            emvm.EmpStatus = empdetails[i].EmpStatus;
                            emvm.AuthorisedEntity = empdetails[i].AuthorisedEntity;
                            emvm.Reason = empdetails[i].Reason;
                            emvm.EmpTypeId = (empdetails[i].EmpType != null) ? empdetails[i].EmpType : 0;
                            int? emptypeid = (empdetails[i].EmpType != null) ? empdetails[i].EmpType : 0;
                            emvm.EmpType = (emptypeid != 0) ? DB.EmpTypeMasters.Where(x => x.EmpTypId == emptypeid).Select(x => x.EmpType).FirstOrDefault() : "";
                            emvm.CEndDate = empdetails[i].CEndDate;
                            emvm.IsActive = empdetails[i].IsActive;
                            emvm.IsUpdated = empdetails[i].IsUpdated;
                            emvm.IsDeleted = empdetails[i].IsDeleted;
                            emvm.CreatedBy = empdetails[i].CreatedBy;
                            emvm.CreatedDate = empdetails[i].CreatedDate;
                            emvm.LastUpdatedBy = empdetails[i].LastUpdatedBy;
                            emvm.LastUpdatedDate = empdetails[i].LastUpdatedDate;
                            lstofEmp.Add(emvm);
                        }

                        return lstofEmp.OrderBy(x => x.EmpStatus).ToList();
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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
        public EmployeeMasterViewModel GetEmployee(EmployeeMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                      //join Comp in DB.CompanyMasters on emp.CompId equals Comp.CompId
                                      //join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
                                      //join role in DB.DesignationMasters on emp.DesignationId equals role.DesignationId
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.EmpId = empdetails.EmpId;
                        emvm.OldEmp_ID = empdetails.OldEmp_ID;
                        emvm.CompId = empdetails.CompId;
                        emvm.Company = DB.CompanyMasters.Where(x => x.CompId == emvm.CompId).Select(x => x.Company).FirstOrDefault();
                        emvm.LEId = (empdetails.LEId != 0) ? empdetails.LEId : 0;
                        emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                        emvm.BUId = (empdetails.BUId != 0) ? empdetails.BUId : 0;
                        emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                        emvm.LocationId = (empdetails.LocationId != 0) ? empdetails.LocationId : 0;
                        emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                        emvm.CategoryId = empdetails.CategoryId;
                        emvm.DeptId = empdetails.CategoryId;
                        emvm.DeptName = empdetails.DeptName;
                        emvm.DesignationId = empdetails.DesignationId;
                        emvm.Designation = empdetails.DesignationName;
                        emvm.ReportId = empdetails.ReportId;
                        emvm.ApproverId = empdetails.ReportId;
                        emvm.AuthorisedEntity = empdetails.AuthorisedEntity;
                        emvm.Approver = "";
                        if (emvm.ReportId != 0)
                        {
                            emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                            (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                            (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                            (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                        }
                        emvm.EmpCode = empdetails.EmpCode;
                        emvm.UserName = empdetails.UserName;
                        emvm.Photo = empdetails.Photo;
                        if (emvm.Photo != "")
                        {
                            string[] stringSeparators = new string[] { "Uploads" };
                            string[] firstNames = emvm.Photo.Split(stringSeparators, StringSplitOptions.None);
                            string lnkval = firstNames[1];
                            emvm.Photo = "Uploads" + lnkval;
                        }
                        emvm.SalutationId = (empdetails.Salutation != null) ? empdetails.Salutation : 0;
                        emvm.Salutation = (emvm.SalutationId != 0) ? DB.SalutationMasters.Where(x => x.SalutationId == emvm.SalutationId).Select(x => x.Salutation).FirstOrDefault() : "";
                        emvm.FirstName = empdetails.FirstName;
                        emvm.MiddleName = empdetails.MiddleName;
                        emvm.LastName = empdetails.LastName;
                        emvm.DOB = empdetails.DOB;
                        emvm.MobileNo = empdetails.MobileNo;
                        emvm.EmailId = empdetails.EmailId;
                        emvm.BloodGroup = empdetails.BloodGroup;
                        emvm.MaritalStatus = empdetails.MaritalStatus;
                        emvm.Gender = empdetails.Gender;
                        emvm.JoiningDate = empdetails.JoiningDate;
                        emvm.InterviewDate = empdetails.InterviewDate;
                        emvm.EndDate = empdetails.EndDate;
                        emvm.EmpStatus = empdetails.EmpStatus.ToUpper();
                        emvm.Reason = empdetails.Reason;
                        emvm.EmpTypeId = (empdetails.EmpType != null) ? empdetails.EmpType : 0;
                        emvm.EmpType = (emvm.EmpTypeId != 0) ? DB.EmpTypeMasters.Where(x => x.EmpTypId == emvm.EmpTypeId).Select(x => x.EmpType).FirstOrDefault() : "";
                        emvm.CEndDate = empdetails.CEndDate;

                        var probation = DB.EmpProbationTrackingHistories
                                            .Where(x => x.EmpId == emvm.EmpId)
                                            .OrderByDescending(x => x.CreatedDate)
                                            .FirstOrDefault();

                        if (probation != null)
                        {
                            emvm.IsProbation = probation.IsProbation;

                            if (probation.IsProbation == true)
                            {
                                emvm.ProbationConfirmationStatus =
                                probation.IsProbation == true ? "Probation" : "";

                                emvm.ProbationConfirmationEffectiveDate =
                                    probation.IsProbation == true && probation.ProbationEndDate.HasValue
                                        ? probation.ProbationEndDate.Value.ToString("dd/MM/yyyy")
                                        : "";

                                emvm.ProbationConfirmationDate =
                                    probation.IsProbation == true && probation.ConfirmDate.HasValue
                                        ? probation.ConfirmDate.Value.ToString("dd/MM/yyyy")
                                        : "";
                            }
                            else
                            {
                                emvm.ProbationConfirmationStatus =
                                probation.IsProbation == false ? "Permanent" : "";

                                emvm.ProbationConfirmationEffectiveDate =
                                    probation.IsProbation == false && probation.ProbationEndDate.HasValue
                                        ? probation.ProbationEndDate.Value.ToString("dd/MM/yyyy")
                                        : "";

                                emvm.ProbationConfirmationDate =
                                    probation.IsProbation == false && probation.ConfirmDate.HasValue
                                        ? probation.ConfirmDate.Value.ToString("dd/MM/yyyy")
                                        : "";
                            }
                        }
                        else
                        {
                            emvm.IsProbation = false;
                            emvm.ProbationConfirmationStatus = "No Status";
                            emvm.ProbationConfirmationEffectiveDate = "";
                            emvm.ProbationConfirmationDate = "";
                        }
                        emvm.IsActive = empdetails.IsActive;
                        emvm.IsUpdated = empdetails.IsUpdated;
                        emvm.IsDeleted = empdetails.IsDeleted;
                        emvm.CreatedBy = empdetails.CreatedBy;
                        emvm.CreatedDate = empdetails.CreatedDate;
                        emvm.LastUpdatedBy = empdetails.LastUpdatedBy;
                        emvm.LastUpdatedDate = empdetails.LastUpdatedDate;

                        return emvm;
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
        public EmployeeMasterViewModel AddEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empid = 0;
                int? LocationId = (model.LocationId != null) ? model.LocationId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpCode == model.EmpCode
                                  //emp.FirstName == model.FirstName && emp.MiddleName == model.MiddleName && emp.LastName == model.LastName
                                  && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).ToList();

                if (loginId != 0)
                {
                    if (empdetails.Count() == 0)
                    {
                        EmployeeMaster em = new EmployeeMaster();
                        em.OldEmp_ID = 0;
                        em.CompId = model.CompId;
                        em.LEId = (model.LEId != null) ? model.LEId : 0;
                        em.BUId = (model.BUId != null) ? model.BUId : 0;
                        em.LocationId = (model.LocationId != null) ? model.LocationId : 0;
                        em.CategoryId = model.DeptId;
                        em.DeptName = model.DeptName;
                        em.DesignationId = model.DesignationId;
                        em.DesignationName = model.Designation;
                        em.ReportId = model.ReportId;
                        int? reportid = model.ReportId;
                        em.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        em.EmpCode = model.EmpCode;
                        em.UserName = model.EmpCode;
                        em.Password = "password";
                        byte[] _encryted;
                        _encryted = System.Text.Encoding.Unicode.GetBytes(em.Password);
                        string NewPassword = Convert.ToBase64String(_encryted);
                        em.Password = NewPassword;
                        em.Photo = (model.Photo != null) ? model.Photo : "";
                        em.Salutation = model.SalutationId;
                        em.FirstName = model.FirstName;
                        em.MiddleName = (model.MiddleName != null) ? model.MiddleName : "";
                        em.LastName = model.LastName;
                        em.DOB = model.DOB;
                        em.MobileNo = model.MobileNo;
                        em.EmailId = model.EmailId;
                        em.BloodGroup = model.BloodGroup;
                        em.MaritalStatus = model.MaritalStatus;
                        em.Gender = model.Gender;
                        em.InterviewDate = model.InterviewDate;
                        em.JoiningDate = model.JoiningDate;
                        em.EmpType = (model.EmpTypeId != null) ? model.EmpTypeId : 0;
                        //em.EndDate = model.EndDate;
                        em.EmpStatus = "Active";
                        em.AuthorisedEntity = model.AuthorisedEntity;
                        em.IsRelieved = false;
                        //em.Reason = model.Reason;
                        em.CEndDate = model.CEndDate;
                        em.IsActive = true;
                        em.IsUpdated = false;
                        em.IsDeleted = false;
                        em.CreatedBy = model.LoginId;
                        em.CreatedDate = DateTime.Now;
                        em.LastUpdatedBy = model.LoginId;
                        em.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasters.Add(em);
                        DB.SaveChanges();
                        empid = em.EmpId;

                        EmployeeMasterLog eml = new EmployeeMasterLog();
                        eml.EmpId = Convert.ToInt32(empid);
                        eml.OldEmp_ID = 0;
                        eml.CompId = model.CompId;
                        eml.LEId = (model.LEId != null) ? model.LEId : 0;
                        eml.BUId = (model.BUId != null) ? model.BUId : 0;
                        eml.LocationId = (model.LocationId != null) ? model.LocationId : 0;
                        eml.CategoryId = model.DeptId;
                        eml.DeptName = model.DeptName;
                        eml.DesignationId = model.DesignationId;
                        eml.DesignationName = model.Designation;
                        eml.ReportId = model.ReportId;
                        eml.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        eml.EmpCode = model.EmpCode;
                        eml.UserName = model.EmpCode;
                        eml.Password = "password";
                        eml.Password = NewPassword;
                        eml.Photo = (model.Photo != null) ? model.Photo : "";
                        eml.Salutation = model.SalutationId;
                        eml.FirstName = model.FirstName;
                        eml.MiddleName = (model.MiddleName != null) ? model.MiddleName : "";
                        eml.LastName = model.LastName;
                        eml.DOB = model.DOB;
                        eml.MobileNo = model.MobileNo;
                        eml.EmailId = model.EmailId;
                        eml.BloodGroup = model.BloodGroup;
                        eml.MaritalStatus = model.MaritalStatus;
                        eml.Gender = model.Gender;
                        eml.JoiningDate = model.JoiningDate;
                        eml.EmpType = (model.EmpTypeId != null) ? model.EmpTypeId : 0;
                        //eml.EndDate = model.EndDate;
                        eml.EmpStatus = "Active";
                        eml.AuthorisedEntity = model.AuthorisedEntity;
                        //eml.Reason = model.Reason;
                        eml.CEndDate = model.CEndDate;
                        eml.IsActive = true;
                        eml.IsUpdated = false;
                        eml.IsDeleted = false;
                        eml.CreatedBy = model.LoginId;
                        eml.CreatedDate = DateTime.Now;
                        eml.LastUpdatedBy = model.LoginId;
                        eml.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasterLogs.Add(eml);
                        DB.SaveChanges();


                        CPwdManagement cpm = new CPwdManagement();
                        cpm.EmpId = empid;
                        cpm.EmpCode = model.EmpCode;
                        cpm.CPwd = true;
                        cpm.Expired = false;
                        cpm.CreatedBy = model.LoginId;
                        cpm.CreatedDate = DateTime.Now;
                        cpm.LastUpdatedBy = model.LoginId;
                        cpm.LastUpdatedDate = DateTime.Now;
                        cpm.IsActive = true;
                        cpm.IsUpdated = false;
                        cpm.IsDeleted = false;
                        DB.CPwdManagements.Add(cpm);
                        DB.SaveChanges();

                        if (model.EmpTypeId > 0)
                        {
                            int? emptypeId = model.EmpTypeId;
                            bool? IsProbation = model.IsProbation;

                            ////var emptypedetails = (from etm in DB.EmpTypeMasters
                            ////                      where etm.EmpTypId == emptypeId
                            ////                      && etm.IsActive == true && etm.IsDeleted == false
                            ////                      select etm).FirstOrDefault();

                            ////string emptype = emptypedetails.EmpType;

                            if (IsProbation == true)
                            {
                                EmpProbationTrackingHistory epth = new EmpProbationTrackingHistory();
                                epth.EmpId = empid;
                                epth.JoiningDate = model.JoiningDate;

                                var locdetails = (from emp in DB.LocationMasters
                                                  where emp.LocationId == LocationId
                                                  //emp.FirstName == model.FirstName && emp.MiddleName == model.MiddleName && emp.LastName == model.LastName
                                                  && emp.IsActive == true && emp.IsDeleted == false
                                                  select emp).FirstOrDefault();

                                // ✅ Safe Probation Days
                                int probationDays = locdetails.ProbationPeriod ?? 90;

                                DateTime joiningDate = model.JoiningDate ?? DateTime.Now;
                                DateTime probationdate = joiningDate.AddDays(probationDays);

                                epth.ProbationDays = probationDays;
                                epth.ProbationEndDate = probationdate;
                                epth.ReportId = reportid;
                                epth.ReportCode = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                                epth.IsProbation = true;
                                epth.IsPermanent = false;
                                epth.IsContract = false;
                                epth.IsConsultant = false;
                                //epth.ConfirmDate = ConfirmDate;
                                //epth.ConfirmBy = empid;
                                epth.Remarks = "";
                                epth.CreatedBy = model.LoginId;
                                epth.CreatedDate = DateTime.Now;
                                epth.LastupdatedBy = model.LoginId;
                                epth.LastUpdatedDate = DateTime.Now;
                                epth.IsActive = true;
                                epth.IsUpdated = false;
                                epth.IsDeleted = false;
                                DB.EmpProbationTrackingHistories.Add(epth);
                                DB.SaveChanges();
                            }
                        }

                        var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                    where lev.EmpId == empid
                                                    && lev.IsActive == true && lev.IsDeleted == false
                                                    select lev).FirstOrDefault();

                        if (levcarryFrowddetails == null)
                        {
                            var Leavetypedetails = (from lev in DB.LeaveTypeMasters
                                                    where lev.IsActive == true && lev.IsDeleted == false
                                                    select lev).ToList();

                            bool hasCompletedOneYear = false;
                            bool isEligibleForCLThisMonth = true;   // default true

                            DateTime Today = DateTime.Now; //more than 10 days
                            int? Year = Today.Year;
                            int? Month = Today.Month;

                            if (model.JoiningDate != null)
                            {
                                var difference = DateTime.Now - model.JoiningDate.Value;
                                hasCompletedOneYear = difference.TotalDays >= 365;

                                // Check if current month == joining month & current year == joining year
                                if (model.JoiningDate.Value.Year == DateTime.Now.Year &&
                                    model.JoiningDate.Value.Month == DateTime.Now.Month)
                                {
                                    // If joined after 15th → not eligible for CL for this month
                                    if (model.JoiningDate.Value.Day > 15)
                                    {
                                        isEligibleForCLThisMonth = false;
                                    }
                                }
                            }

                            for (int i = 0; i < Leavetypedetails.Count(); i++)
                            {
                                if (Leavetypedetails[i].ShortName == "CL")
                                {
                                    // Filter CL if not eligible
                                    if (isEligibleForCLThisMonth != true)
                                    {
                                        LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                        cf.EmpId = empid;
                                        cf.EmpCode = model.EmpCode;
                                        cf.LeaveTypeId = Leavetypedetails[i].LeaveTypeId;
                                        cf.LeaveMonth = Month;
                                        cf.LeaveYear = Year;
                                        cf.OpeningBalance = Convert.ToDecimal(0.00);
                                        cf.Availed = Convert.ToDecimal(0.00);
                                        cf.CarryForward = Convert.ToDecimal(0.00);
                                        cf.Encashment = Convert.ToDecimal(0.00);
                                        cf.ClosingBalance = Convert.ToDecimal(0.00);
                                        cf.CreatedBy = model.LoginId; ;
                                        cf.CreatedDate = DateTime.Now;
                                        cf.LastUpdatedBy = model.LoginId; ;
                                        cf.LastUpdatedDate = DateTime.Now;
                                        cf.IsActive = true;
                                        cf.IsUpdated = false;
                                        cf.IsDeleted = false;
                                        DB.LeaveCarryForwardMasters.Add(cf);
                                        DB.SaveChanges();
                                    }
                                    else
                                    {
                                        LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                        cf.EmpId = empid;
                                        cf.EmpCode = model.EmpCode;
                                        cf.LeaveTypeId = Leavetypedetails[i].LeaveTypeId;
                                        cf.LeaveMonth = Month;
                                        cf.LeaveYear = Year;
                                        cf.OpeningBalance = Leavetypedetails[i].Credit;
                                        cf.Availed = Convert.ToDecimal(0.00);
                                        cf.CarryForward = Convert.ToDecimal(0.00);
                                        cf.Encashment = Convert.ToDecimal(0.00);
                                        cf.ClosingBalance = Leavetypedetails[i].Credit;
                                        cf.CreatedBy = model.LoginId; ;
                                        cf.CreatedDate = DateTime.Now;
                                        cf.LastUpdatedBy = model.LoginId; ;
                                        cf.LastUpdatedDate = DateTime.Now;
                                        cf.IsActive = true;
                                        cf.IsUpdated = false;
                                        cf.IsDeleted = false;
                                        DB.LeaveCarryForwardMasters.Add(cf);
                                        DB.SaveChanges();
                                    }
                                }
                                else if (Leavetypedetails[i].ShortName == "EL")
                                {
                                    // ✅ Apply your conditions
                                    if (hasCompletedOneYear != true)
                                    {
                                        LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                        cf.EmpId = empid;
                                        cf.EmpCode = model.EmpCode;
                                        cf.LeaveTypeId = Leavetypedetails[i].LeaveTypeId;
                                        cf.LeaveMonth = 0;
                                        cf.LeaveYear = Year;
                                        cf.OpeningBalance = Convert.ToDecimal(0.00);
                                        cf.Availed = Convert.ToDecimal(0.00);
                                        cf.CarryForward = Convert.ToDecimal(0.00);
                                        cf.Encashment = Convert.ToDecimal(0.00);
                                        cf.ClosingBalance = Convert.ToDecimal(0.00);
                                        cf.CreatedBy = model.LoginId; ;
                                        cf.CreatedDate = DateTime.Now;
                                        cf.LastUpdatedBy = model.LoginId; ;
                                        cf.LastUpdatedDate = DateTime.Now;
                                        cf.IsActive = true;
                                        cf.IsUpdated = false;
                                        cf.IsDeleted = false;
                                        DB.LeaveCarryForwardMasters.Add(cf);
                                        DB.SaveChanges();
                                    }
                                    else
                                    {
                                        LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                        cf.EmpId = empid;
                                        cf.EmpCode = model.EmpCode;
                                        cf.LeaveTypeId = Leavetypedetails[i].LeaveTypeId;
                                        cf.LeaveMonth = 0;
                                        cf.LeaveYear = Year;
                                        cf.OpeningBalance = Leavetypedetails[i].Credit;
                                        cf.Availed = Convert.ToDecimal(0.00);
                                        cf.CarryForward = Convert.ToDecimal(0.00);
                                        cf.Encashment = Convert.ToDecimal(0.00);
                                        cf.ClosingBalance = Leavetypedetails[i].Credit;
                                        cf.CreatedBy = model.LoginId; ;
                                        cf.CreatedDate = DateTime.Now;
                                        cf.LastUpdatedBy = model.LoginId; ;
                                        cf.LastUpdatedDate = DateTime.Now;
                                        cf.IsActive = true;
                                        cf.IsUpdated = false;
                                        cf.IsDeleted = false;
                                        DB.LeaveCarryForwardMasters.Add(cf);
                                        DB.SaveChanges();
                                    }
                                }
                                else if (Leavetypedetails[i].ShortName == "RH")
                                {
                                    LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                    cf.EmpId = empid;
                                    cf.EmpCode = model.EmpCode;
                                    cf.LeaveTypeId = Leavetypedetails[i].LeaveTypeId;
                                    cf.LeaveMonth = 0;
                                    cf.LeaveYear = Year;
                                    cf.OpeningBalance = Leavetypedetails[i].MaxPerYear;
                                    cf.Availed = Convert.ToDecimal(0.00);
                                    cf.CarryForward = Convert.ToDecimal(0.00);
                                    cf.Encashment = Convert.ToDecimal(0.00);
                                    cf.ClosingBalance = Leavetypedetails[i].MaxPerYear;
                                    cf.CreatedBy = model.LoginId; ;
                                    cf.CreatedDate = DateTime.Now;
                                    cf.LastUpdatedBy = model.LoginId; ;
                                    cf.LastUpdatedDate = DateTime.Now;
                                    cf.IsActive = true;
                                    cf.IsUpdated = false;
                                    cf.IsDeleted = false;
                                    DB.LeaveCarryForwardMasters.Add(cf);
                                    DB.SaveChanges();
                                }
                                else
                                {
                                    LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                    cf.EmpId = empid;
                                    cf.EmpCode = model.EmpCode;
                                    cf.LeaveTypeId = Leavetypedetails[i].LeaveTypeId;
                                    cf.LeaveMonth = 0;
                                    cf.LeaveYear = Year;
                                    cf.OpeningBalance = Leavetypedetails[i].Credit;
                                    cf.ClosingBalance = Leavetypedetails[i].Credit;
                                    if (Leavetypedetails[i].Credit == 0)
                                    {
                                        cf.OpeningBalance = Leavetypedetails[i].MaxPerYear;
                                        cf.ClosingBalance = Leavetypedetails[i].MaxPerYear;
                                    }
                                    
                                    cf.Availed = Convert.ToDecimal(0.00);
                                    cf.CarryForward = Convert.ToDecimal(0.00);
                                    cf.Encashment = Convert.ToDecimal(0.00);
                                    cf.CreatedBy = model.LoginId; ;
                                    cf.CreatedDate = DateTime.Now;
                                    cf.LastUpdatedBy = model.LoginId; ;
                                    cf.LastUpdatedDate = DateTime.Now;
                                    cf.IsActive = true;
                                    cf.IsUpdated = false;
                                    cf.IsDeleted = false;
                                    DB.LeaveCarryForwardMasters.Add(cf);
                                    DB.SaveChanges();
                                }
                            }
                        }

                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.EmpId = empid;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeMasterViewModel UpdateEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        //empdetails.OldEmp_ID = model.OldEmp_ID;
                        empdetails.CompId = model.CompId;
                        empdetails.LEId = (model.LEId != null) ? model.LEId : 0;
                        empdetails.BUId = (model.BUId != null) ? model.BUId : 0;
                        empdetails.LocationId = (model.LocationId != null) ? model.LocationId : 0;
                        empdetails.CategoryId = model.DeptId;
                        empdetails.DeptName = model.DeptName;
                        empdetails.DesignationId = model.DesignationId;
                        empdetails.DesignationName = model.Designation;
                        empdetails.ReportId = model.ReportId;
                        int? reportid = model.ReportId;
                        empdetails.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        empdetails.EmpCode = model.EmpCode;
                        empdetails.UserName = model.EmpCode;
                        //empdetails.Password = model.Password;
                        empdetails.Photo = (model.Photo != null) ? model.Photo : "";
                        empdetails.Salutation = model.SalutationId;
                        empdetails.FirstName = model.FirstName;
                        empdetails.MiddleName = (model.MiddleName != null) ? model.MiddleName : "";
                        empdetails.LastName = model.LastName;
                        empdetails.DOB = model.DOB;
                        empdetails.MobileNo = model.MobileNo;
                        empdetails.EmailId = model.EmailId;
                        empdetails.BloodGroup = model.BloodGroup;
                        empdetails.MaritalStatus = model.MaritalStatus;
                        empdetails.Gender = model.Gender;
                        empdetails.InterviewDate = model.InterviewDate;
                        empdetails.JoiningDate = model.JoiningDate;
                        empdetails.EndDate = model.EndDate;
                        empdetails.EmpStatus = "Active";
                        empdetails.Reason = model.Reason;
                        empdetails.EmpType = (model.EmpTypeId != null) ? model.EmpTypeId : 0;
                        empdetails.CEndDate = model.CEndDate;
                        empdetails.AuthorisedEntity = model.AuthorisedEntity;
                        empdetails.IsActive = true;
                        empdetails.IsUpdated = true;
                        empdetails.IsDeleted = false;
                        empdetails.LastUpdatedBy = model.LoginId;
                        empdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeMasterLog eml = new EmployeeMasterLog();
                        eml.EmpId = Convert.ToInt32(id);
                        eml.OldEmp_ID = 0;
                        eml.CompId = model.CompId;
                        eml.LEId = (model.LEId != null) ? model.LEId : 0;
                        eml.BUId = (model.BUId != null) ? model.BUId : 0;
                        eml.LocationId = (model.LocationId != null) ? model.LocationId : 0;
                        eml.CategoryId = model.DeptId;
                        eml.DeptName = model.DeptName;
                        eml.DesignationId = model.DesignationId;
                        eml.DesignationName = model.Designation;
                        eml.ReportId = model.ReportId;
                        eml.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        eml.EmpCode = model.EmpCode;
                        eml.UserName = model.EmpCode;
                        ////eml.Password = "password";
                        ////eml.Password = NewPassword;
                        eml.Photo = (model.Photo != null) ? model.Photo : "";
                        eml.Salutation = model.SalutationId;
                        eml.FirstName = model.FirstName;
                        eml.MiddleName = (model.MiddleName != null) ? model.MiddleName : "";
                        eml.LastName = model.LastName;
                        eml.DOB = model.DOB;
                        eml.MobileNo = model.MobileNo;
                        eml.EmailId = model.EmailId;
                        eml.BloodGroup = model.BloodGroup;
                        eml.MaritalStatus = model.MaritalStatus;
                        eml.Gender = model.Gender;
                        eml.JoiningDate = model.JoiningDate;
                        eml.EmpType = (model.EmpTypeId != null) ? model.EmpTypeId : 0;
                        eml.EndDate = model.EndDate;
                        eml.EmpStatus = "Active";
                        eml.AuthorisedEntity = model.AuthorisedEntity;
                        eml.Reason = model.Reason;
                        eml.CEndDate = model.CEndDate;
                        eml.IsActive = true;
                        eml.IsUpdated = false;
                        eml.IsDeleted = false;
                        eml.CreatedBy = model.LoginId;
                        eml.CreatedDate = DateTime.Now;
                        eml.LastUpdatedBy = model.LoginId;
                        eml.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasterLogs.Add(eml);
                        DB.SaveChanges();

                        if (model.EmpTypeId > 0)
                        {
                            int? emptypeId = model.EmpTypeId;
                            bool? IsProbation = model.IsProbation;
                            bool? IsProbationConfirm = model.IsProbationConfirm;

                            var pthdetails = (from etm in DB.EmpProbationTrackingHistories
                                                  where etm.EmpId == id && etm.IsProbation == true
                                                  && etm.IsActive == true && etm.IsDeleted == false
                                                  select etm).FirstOrDefault();

                            if (IsProbationConfirm == true)
                            {
                                if (pthdetails != null)
                                {
                                    pthdetails.IsProbation = false;
                                    pthdetails.ConfirmBy = loginId;
                                    pthdetails.ConfirmDate = Convert.ToDateTime(model.ProbationConfirmationDate);
                                    pthdetails.IsPermanent = true;
                                    pthdetails.Remarks = model.ProbationRemarks;
                                    pthdetails.LastupdatedBy = model.LoginId;
                                    pthdetails.LastUpdatedDate = DateTime.Now;
                                    pthdetails.IsUpdated = true;
                                    DB.SaveChanges();
                                }
                            }
                        }

                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.msg = "Updated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeMasterViewModel DeleteEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        empdetails.EmpStatus = "Deleted";
                        empdetails.Reason = model.Reason;
                        empdetails.IsActive = true;
                        empdetails.IsUpdated = true;
                        empdetails.IsDeleted = true;
                        empdetails.LastUpdatedBy = model.LoginId;
                        empdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeMasterLog eml = new EmployeeMasterLog();
                        eml.EmpId = Convert.ToInt32(id);
                        eml.OldEmp_ID = 0;
                        eml.CompId = empdetails.CompId;
                        eml.LEId = (empdetails.LEId != null) ? empdetails.LEId : 0;
                        eml.BUId = (empdetails.BUId != null) ? empdetails.BUId : 0;
                        eml.LocationId = (empdetails.LocationId != null) ? empdetails.LocationId : 0;
                        eml.CategoryId = empdetails.CategoryId;
                        eml.DeptName = empdetails.DeptName;
                        eml.DesignationId = empdetails.DesignationId;
                        eml.DesignationName = empdetails.DesignationName;
                        eml.ReportId = empdetails.ReportId;
                        int? reportid = model.ReportId;
                        eml.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        eml.EmpCode = empdetails.EmpCode;
                        eml.UserName = empdetails.EmpCode;
                        ////eml.Password = "password";
                        ////eml.Password = NewPassword;
                        eml.Photo = (empdetails.Photo != null) ? empdetails.Photo : "";
                        eml.Salutation = empdetails.Salutation;
                        eml.FirstName = empdetails.FirstName;
                        eml.MiddleName = (empdetails.MiddleName != null) ? empdetails.MiddleName : "";
                        eml.LastName = empdetails.LastName;
                        eml.DOB = empdetails.DOB;
                        eml.MobileNo = empdetails.MobileNo;
                        eml.EmailId = empdetails.EmailId;
                        eml.BloodGroup = empdetails.BloodGroup;
                        eml.MaritalStatus = empdetails.MaritalStatus;
                        eml.Gender = empdetails.Gender;
                        eml.JoiningDate = empdetails.JoiningDate;
                        eml.EmpType = (empdetails.EmpType != null) ? empdetails.EmpType : 0;
                        eml.EndDate = empdetails.EndDate;
                        eml.EmpStatus = "Deleted";
                        eml.Reason = model.Reason;
                        eml.AuthorisedEntity = empdetails.AuthorisedEntity;
                        eml.CEndDate = empdetails.CEndDate;
                        eml.IsActive = true;
                        eml.IsUpdated = false;
                        eml.IsDeleted = false;
                        eml.CreatedBy = model.LoginId;
                        eml.CreatedDate = DateTime.Now;
                        eml.LastUpdatedBy = model.LoginId;
                        eml.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasterLogs.Add(eml);
                        DB.SaveChanges();

                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Shift Details Not Found");
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
        public EmployeeMasterViewModel ActiveEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();


                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        empdetails.EmpStatus = "Active";
                        empdetails.Reason = model.Reason;
                        empdetails.IsActive = true;
                        empdetails.IsUpdated = true;
                        empdetails.IsDeleted = false;
                        empdetails.LastUpdatedBy = model.LoginId;
                        empdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeMasterLog eml = new EmployeeMasterLog();
                        eml.EmpId = Convert.ToInt32(id);
                        eml.OldEmp_ID = 0;
                        eml.CompId = empdetails.CompId;
                        eml.LEId = (empdetails.LEId != null) ? empdetails.LEId : 0;
                        eml.BUId = (empdetails.BUId != null) ? empdetails.BUId : 0;
                        eml.LocationId = (empdetails.LocationId != null) ? empdetails.LocationId : 0;
                        eml.CategoryId = empdetails.CategoryId;
                        eml.DeptName = empdetails.DeptName;
                        eml.DesignationId = empdetails.DesignationId;
                        eml.DesignationName = empdetails.DesignationName;
                        eml.ReportId = empdetails.ReportId;
                        int? reportid = model.ReportId;
                        eml.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        eml.EmpCode = empdetails.EmpCode;
                        eml.UserName = empdetails.EmpCode;
                        ////eml.Password = "password";
                        ////eml.Password = NewPassword;
                        eml.Photo = (empdetails.Photo != null) ? empdetails.Photo : "";
                        eml.Salutation = empdetails.Salutation;
                        eml.FirstName = empdetails.FirstName;
                        eml.MiddleName = (empdetails.MiddleName != null) ? empdetails.MiddleName : "";
                        eml.LastName = empdetails.LastName;
                        eml.DOB = empdetails.DOB;
                        eml.MobileNo = empdetails.MobileNo;
                        eml.EmailId = empdetails.EmailId;
                        eml.BloodGroup = empdetails.BloodGroup;
                        eml.MaritalStatus = empdetails.MaritalStatus;
                        eml.Gender = empdetails.Gender;
                        eml.JoiningDate = empdetails.JoiningDate;
                        eml.EmpType = (empdetails.EmpType != null) ? empdetails.EmpType : 0;
                        eml.EndDate = empdetails.EndDate;
                        eml.EmpStatus = "Active";
                        eml.Reason = model.Reason;
                        eml.AuthorisedEntity = empdetails.AuthorisedEntity;
                        eml.CEndDate = empdetails.CEndDate;
                        eml.IsActive = true;
                        eml.IsUpdated = false;
                        eml.IsDeleted = false;
                        eml.CreatedBy = model.LoginId;
                        eml.CreatedDate = DateTime.Now;
                        eml.LastUpdatedBy = model.LoginId;
                        eml.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasterLogs.Add(eml);
                        DB.SaveChanges();

                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.msg = "Actived";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeMasterViewModel DeActiveEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        empdetails.EndDate = DateTime.Now;
                        empdetails.EmpStatus = "Deactive";
                        empdetails.Reason = model.Reason;
                        empdetails.IsActive = true;
                        empdetails.IsUpdated = true;
                        empdetails.IsDeleted = false;
                        empdetails.LastUpdatedBy = model.LoginId;
                        empdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeMasterLog eml = new EmployeeMasterLog();
                        eml.EmpId = Convert.ToInt32(id);
                        eml.OldEmp_ID = 0;
                        eml.CompId = empdetails.CompId;
                        eml.LEId = (empdetails.LEId != null) ? empdetails.LEId : 0;
                        eml.BUId = (empdetails.BUId != null) ? empdetails.BUId : 0;
                        eml.LocationId = (empdetails.LocationId != null) ? empdetails.LocationId : 0;
                        eml.CategoryId = empdetails.CategoryId;
                        eml.DeptName = empdetails.DeptName;
                        eml.DesignationId = empdetails.DesignationId;
                        eml.DesignationName = empdetails.DesignationName;
                        eml.ReportId = empdetails.ReportId;
                        int? reportid = model.ReportId;
                        eml.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        eml.EmpCode = empdetails.EmpCode;
                        eml.UserName = empdetails.EmpCode;
                        ////eml.Password = "password";
                        ////eml.Password = NewPassword;
                        eml.Photo = (empdetails.Photo != null) ? empdetails.Photo : "";
                        eml.Salutation = empdetails.Salutation;
                        eml.FirstName = empdetails.FirstName;
                        eml.MiddleName = (empdetails.MiddleName != null) ? empdetails.MiddleName : "";
                        eml.LastName = empdetails.LastName;
                        eml.DOB = empdetails.DOB;
                        eml.MobileNo = empdetails.MobileNo;
                        eml.EmailId = empdetails.EmailId;
                        eml.BloodGroup = empdetails.BloodGroup;
                        eml.MaritalStatus = empdetails.MaritalStatus;
                        eml.Gender = empdetails.Gender;
                        eml.JoiningDate = empdetails.JoiningDate;
                        eml.EmpType = (empdetails.EmpType != null) ? empdetails.EmpType : 0;
                        eml.EndDate = empdetails.EndDate;
                        eml.EmpStatus = "Deactive";
                        eml.Reason = model.Reason;
                        eml.AuthorisedEntity = empdetails.AuthorisedEntity;
                        eml.CEndDate = empdetails.CEndDate;
                        eml.IsActive = true;
                        eml.IsUpdated = false;
                        eml.IsDeleted = false;
                        eml.CreatedBy = model.LoginId;
                        eml.CreatedDate = DateTime.Now;
                        eml.LastUpdatedBy = model.LoginId;
                        eml.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasterLogs.Add(eml);
                        DB.SaveChanges();

                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.msg = "Deactived";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeMasterViewModel RelievedEmployee(EmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        empdetails.EmpStatus = "Relieved";
                        empdetails.RelievedReason = model.RelievedReason;
                        empdetails.RelievedDate = model.RelievedDate;
                        empdetails.RelievedEffectiveDate = model.RelievedEffectiveDate;
                        empdetails.IsRelieved = model.IsRelieved;
                        empdetails.IsActive = true;
                        empdetails.IsUpdated = true;
                        empdetails.IsDeleted = false;
                        empdetails.LastUpdatedBy = model.LoginId;
                        empdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeMasterLog eml = new EmployeeMasterLog();
                        eml.EmpId = Convert.ToInt32(id);
                        eml.OldEmp_ID = 0;
                        eml.CompId = empdetails.CompId;
                        eml.LEId = (empdetails.LEId != null) ? empdetails.LEId : 0;
                        eml.BUId = (empdetails.BUId != null) ? empdetails.BUId : 0;
                        eml.LocationId = (empdetails.LocationId != null) ? empdetails.LocationId : 0;
                        eml.CategoryId = empdetails.CategoryId;
                        eml.DeptName = empdetails.DeptName;
                        eml.DesignationId = empdetails.DesignationId;
                        eml.DesignationName = empdetails.DesignationName;
                        eml.ReportId = empdetails.ReportId;
                        int? reportid = model.ReportId;
                        eml.ReportName = DB.EmployeeMasters.Where(x => x.EmpId == reportid).Select(x => x.EmpCode).FirstOrDefault();
                        eml.EmpCode = empdetails.EmpCode;
                        eml.UserName = empdetails.EmpCode;
                        ////eml.Password = "password";
                        ////eml.Password = NewPassword;
                        eml.Photo = (empdetails.Photo != null) ? empdetails.Photo : "";
                        eml.Salutation = empdetails.Salutation;
                        eml.FirstName = empdetails.FirstName;
                        eml.MiddleName = (empdetails.MiddleName != null) ? empdetails.MiddleName : "";
                        eml.LastName = empdetails.LastName;
                        eml.DOB = empdetails.DOB;
                        eml.MobileNo = empdetails.MobileNo;
                        eml.EmailId = empdetails.EmailId;
                        eml.BloodGroup = empdetails.BloodGroup;
                        eml.MaritalStatus = empdetails.MaritalStatus;
                        eml.Gender = empdetails.Gender;
                        eml.JoiningDate = empdetails.JoiningDate;
                        eml.EmpType = (empdetails.EmpType != null) ? empdetails.EmpType : 0;
                        eml.EndDate = empdetails.EndDate;
                        eml.EmpStatus = "Relieved";
                        eml.RelievedReason = model.RelievedReason;
                        eml.RelievedDate = model.RelievedDate;
                        eml.RelievedEffectiveDate = model.RelievedEffectiveDate;
                        eml.IsRelieved = model.IsRelieved;
                        eml.Reason = "";
                        eml.AuthorisedEntity = empdetails.AuthorisedEntity;
                        eml.CEndDate = empdetails.CEndDate;
                        eml.IsActive = true;
                        eml.IsUpdated = false;
                        eml.IsDeleted = false;
                        eml.CreatedBy = model.LoginId;
                        eml.CreatedDate = DateTime.Now;
                        eml.LastUpdatedBy = model.LoginId;
                        eml.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeMasterLogs.Add(eml);
                        DB.SaveChanges();

                        EmployeeMasterViewModel emvm = new EmployeeMasterViewModel();
                        emvm.msg = "Relieved";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<EmployeeDetailViewModel> GetAllEmployeeDetails(EmployeeDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var empdetails = (from emp in DB.EmployeeDetails
                                  where emp.IsActive == true && emp.IsDeleted == false
                                  select emp).OrderByDescending(x => x.Id).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<EmployeeDetailViewModel> lstogempdetails = new List<EmployeeDetailViewModel>();
                        for (int i = 0; i < empdetails.Count(); i++)
                        {
                            EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
                            edvm.Id = empdetails[i].Id;
                            edvm.EmpId = empdetails[i].EmpId;
                            edvm.AMobileNo = empdetails[i].AMobileNo;
                            edvm.PMailId = empdetails[i].PMailId;
                            edvm.FatherName = empdetails[i].FatherName;
                            edvm.MotherName = empdetails[i].MotherName;
                            edvm.HusbandName = empdetails[i].HusbandName;
                            edvm.FContactNo = empdetails[i].FContactNo;
                            edvm.MContactNo = empdetails[i].MContactNo;
                            edvm.HContactNo = empdetails[i].HContactNo;
                            edvm.EContactName = empdetails[i].FContactNo;
                            edvm.EContactNo = empdetails[i].MContactNo;
                            edvm.EContactRelationship = empdetails[i].HContactNo;
                            edvm.Height = empdetails[i].Height;
                            edvm.Weight = empdetails[i].Weight;
                            edvm.DateOfAnniversary = empdetails[i].DateOfAnniversary;
                            edvm.Disability = empdetails[i].Disability;
                            edvm.TotalExperience = empdetails[i].TotalExperience;
                            edvm.RelevantExperience = empdetails[i].RelevantExperience;
                            edvm.ECActivities = empdetails[i].ECActivities;
                            edvm.Sports = empdetails[i].Sports;
                            edvm.Caste = empdetails[i].Caste;
                            edvm.Region = empdetails[i].Region;
                            edvm.Country = empdetails[i].Country;
                            edvm.Nationality = empdetails[i].Nationality;
                            edvm.CurrentBuildingName = empdetails[i].CurrentBuildingName;
                            edvm.CurrentCity = empdetails[i].CurrentCity;
                            edvm.CurrentCountry = empdetails[i].CurrentCountry;
                            edvm.CurrentDoorNumber = empdetails[i].CurrentDoorNumber;
                            edvm.CurrentLocation = empdetails[i].CurrentLocation;
                            edvm.CurrentPinCode = empdetails[i].CurrentPinCode;
                            edvm.CurrentState = empdetails[i].CurrentState;
                            edvm.CurrentStreet = empdetails[i].CurrentBuildingName;
                            edvm.PermanentBuildingName = empdetails[i].CurrentBuildingName;
                            edvm.PermanentCity = empdetails[i].CurrentBuildingName;
                            edvm.PermanentCountry = empdetails[i].CurrentBuildingName;
                            edvm.PermanentDoorNumber = empdetails[i].CurrentBuildingName;
                            edvm.PermanentLocation = empdetails[i].CurrentBuildingName;
                            edvm.PermanentPinCode = empdetails[i].CurrentBuildingName;
                            edvm.PermanentState = empdetails[i].CurrentBuildingName;
                            edvm.PermanentStreet = empdetails[i].CurrentBuildingName;
                            edvm.CreatedBy = empdetails[i].CreatedBy;
                            edvm.CreatedDate = empdetails[i].CreatedDate;
                            edvm.LastUpdatedBy = empdetails[i].LastUpdatedBy;
                            edvm.LastUpdatedDate = empdetails[i].LastUpdatedDate;
                            edvm.IsActive = empdetails[i].IsActive;
                            edvm.IsUpdated = empdetails[i].IsUpdated;
                            edvm.IsDeleted = empdetails[i].IsDeleted;
                            lstogempdetails.Add(edvm);
                        }

                        return lstogempdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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
        public EmployeeDetailViewModel GetEmployeeDetails(EmployeeDetailViewModel model)
        {
            //try
            //{

            //    string msg = "";
            //    int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
            //    int? id = (model.EmpId != 0) ? model.EmpId : 0;

            //    var empdetails = (from emp in DB.EmployeeDetails
            //                      where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
            //                      select emp).FirstOrDefault();

            //    if (loginId != 0)
            //    {
            //        if (empdetails != null)
            //        {
            //            EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
            //            edvm.Id = empdetails.Id;
            //            edvm.EmpId = empdetails.EmpId;
            //            edvm.AMobileNo = empdetails.AMobileNo;
            //            edvm.PMailId = empdetails.PMailId;
            //            edvm.FatherName = empdetails.FatherName;
            //            edvm.MotherName = empdetails.MotherName;
            //            edvm.HusbandName = empdetails.HusbandName;
            //            edvm.FContactNo = empdetails.FContactNo;
            //            edvm.MContactNo = empdetails.MContactNo;
            //            edvm.HContactNo = empdetails.HContactNo;
            //            edvm.EContactName = empdetails.EContactName;
            //            edvm.EContactNo = empdetails.MContactNo;
            //            edvm.EContactRelationship = empdetails.EContactRelationship;
            //            edvm.Height = empdetails.Height;
            //            edvm.Weight = empdetails.Weight;
            //            edvm.DateOfAnniversary = empdetails.DateOfAnniversary;
            //            edvm.Disability = empdetails.Disability;
            //            edvm.TotalExperience = empdetails.TotalExperience;
            //            edvm.RelevantExperience = empdetails.RelevantExperience;
            //            edvm.ECActivities = empdetails.ECActivities;

            //            edvm.EContactNo1 = empdetails.EContactNo1;
            //            edvm.EContactName1 = empdetails.EContactName1;
            //            edvm.EContactRelationship1 = empdetails.EContactRelationship1;

            //            edvm.EContactNo2 = empdetails.EContactNo2;
            //            edvm.EContactName2 = empdetails.EContactName2;
            //            edvm.EContactRelationship2 = empdetails.EContactRelationship2;
            //            edvm.Sports = empdetails.Sports;
            //            edvm.CurrentBuildingName = empdetails.CurrentBuildingName;
            //            edvm.CurrentCity = empdetails.CurrentCity;
            //            edvm.CurrentCountry = empdetails.CurrentCountry;
            //            edvm.CurrentDoorNumber = empdetails.CurrentDoorNumber;
            //            edvm.CurrentLocation = empdetails.CurrentLocation;
            //            edvm.CurrentPinCode = empdetails.CurrentPinCode;
            //            edvm.CurrentState = empdetails.CurrentState;
            //            edvm.CurrentStreet = empdetails.CurrentStreet;
            //            edvm.PermanentBuildingName = empdetails.PermanentBuildingName;
            //            edvm.PermanentCity = empdetails.PermanentCity;
            //            edvm.PermanentCountry = empdetails.PermanentCountry;
            //            edvm.PermanentDoorNumber = empdetails.PermanentDoorNumber;
            //            edvm.PermanentLocation = empdetails.PermanentLocation;
            //            edvm.PermanentPinCode = empdetails.PermanentPinCode;
            //            edvm.PermanentState = empdetails.PermanentState;
            //            edvm.PermanentStreet = empdetails.PermanentStreet;
            //            edvm.Caste = empdetails.Caste;
            //            edvm.Region = empdetails.Region;
            //            edvm.Country = empdetails.Country;
            //            edvm.Nationality = empdetails.Nationality;
            //            edvm.CreatedBy = empdetails.CreatedBy;
            //            edvm.CreatedDate = empdetails.CreatedDate;
            //            edvm.LastUpdatedBy = empdetails.LastUpdatedBy;
            //            edvm.LastUpdatedDate = empdetails.LastUpdatedDate;
            //            edvm.IsActive = empdetails.IsActive;
            //            edvm.IsUpdated = empdetails.IsUpdated;
            //            edvm.IsDeleted = empdetails.IsDeleted;

            //            return edvm;
            //        }
            //        else
            //        {
            //            throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
            //        }
            //    }
            //    else
            //    {
            //        throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
            //    }
            //}
            //catch (CustomApiException ex)
            //{
            //    throw new CustomApiException(ex.StatusCode, ex.Message);
            //}
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeDetails
                                  where emp.EmpId == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
                        edvm.Id = empdetails.Id;
                        edvm.EmpId = empdetails.EmpId;
                        edvm.AMobileNo = empdetails.AMobileNo;
                        edvm.PMailId = empdetails.PMailId;
                        edvm.FatherName = empdetails.FatherName;
                        edvm.MotherName = empdetails.MotherName;
                        edvm.HusbandName = empdetails.HusbandName;
                        edvm.FContactNo = empdetails.FContactNo;
                        edvm.MContactNo = empdetails.MContactNo;
                        edvm.HContactNo = empdetails.HContactNo;

                        edvm.EContactNo = empdetails.EContactNo;
                        edvm.EContactName = empdetails.EContactName;

                        edvm.EContactRelationship = empdetails.EContactRelationship;


                        // Added Contact1 and Contact2 columns to EmployeeDetail table and updated EmployeeDetailViewModel and APIs

                        edvm.EContactNo1 = empdetails.EContactNo1;
                        edvm.EContactName1 = empdetails.EContactName1;
                        edvm.EContactRelationship1 = empdetails.EContactRelationship1;

                        edvm.EContactNo2 = empdetails.EContactNo2;
                        edvm.EContactName2 = empdetails.EContactName2;
                        edvm.EContactRelationship2 = empdetails.EContactRelationship2;





                        edvm.Height = empdetails.Height;
                        edvm.Weight = empdetails.Weight;
                        edvm.DateOfAnniversary = empdetails.DateOfAnniversary;
                        edvm.Disability = empdetails.Disability;
                        edvm.TotalExperience = empdetails.TotalExperience;
                        edvm.RelevantExperience = empdetails.RelevantExperience;
                        edvm.ECActivities = empdetails.ECActivities;
                        edvm.Sports = empdetails.Sports;
                        edvm.CurrentBuildingName = empdetails.CurrentBuildingName;
                        edvm.CurrentCity = empdetails.CurrentCity;
                        edvm.CurrentCountry = empdetails.CurrentCountry;
                        edvm.CurrentDoorNumber = empdetails.CurrentDoorNumber;
                        edvm.CurrentLocation = empdetails.CurrentLocation;
                        edvm.CurrentPinCode = empdetails.CurrentPinCode;
                        edvm.CurrentState = empdetails.CurrentState;
                        edvm.CurrentStreet = empdetails.CurrentStreet;
                        edvm.PermanentBuildingName = empdetails.PermanentBuildingName;
                        edvm.PermanentCity = empdetails.PermanentCity;
                        edvm.PermanentCountry = empdetails.PermanentCountry;
                        edvm.PermanentDoorNumber = empdetails.PermanentDoorNumber;
                        edvm.PermanentLocation = empdetails.PermanentLocation;
                        edvm.PermanentPinCode = empdetails.PermanentPinCode;
                        edvm.PermanentState = empdetails.PermanentState;
                        edvm.PermanentStreet = empdetails.PermanentStreet;
                        edvm.Caste = empdetails.Caste;
                        edvm.Region = empdetails.Region;
                        edvm.Country = empdetails.Country;
                        edvm.Nationality = empdetails.Nationality;
                        edvm.CreatedBy = empdetails.CreatedBy;
                        edvm.CreatedDate = empdetails.CreatedDate;
                        edvm.LastUpdatedBy = empdetails.LastUpdatedBy;
                        edvm.LastUpdatedDate = empdetails.LastUpdatedDate;
                        edvm.IsActive = empdetails.IsActive;
                        edvm.IsUpdated = empdetails.IsUpdated;
                        edvm.IsDeleted = empdetails.IsDeleted;

                        return edvm;
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
        public EmployeeDetailViewModel AddEmployeeDetails(EmployeeDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empid = (model.EmpId != 0) ? model.EmpId : 0;

                var empdetails = (from emp in DB.EmployeeDetails
                                  where emp.EmpId == empid && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).ToList();

                if (loginId != 0)
                {
                    if (empdetails.Count() == 0)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed.EmpId = empid;
                        ed.AMobileNo = model.AMobileNo;
                        ed.PMailId = model.PMailId;
                        ed.FatherName = model.FatherName;
                        ed.MotherName = model.MotherName;
                        ed.HusbandName = model.HusbandName;
                        ed.FContactNo = model.FContactNo;
                        ed.MContactNo = model.MContactNo;
                        ed.HContactNo = model.HContactNo;
                        ed.EContactName = model.EContactName;
                        ed.EContactNo = model.EContactNo;
                        ed.EContactRelationship = model.EContactRelationship;
                        ed.Height = model.Height;
                        ed.Weight = model.Weight;
                        ed.DateOfAnniversary = model.DateOfAnniversary;
                        ed.Disability = model.Disability;
                        ed.TotalExperience = model.TotalExperience;
                        ed.RelevantExperience = model.RelevantExperience;
                        ed.ECActivities = model.ECActivities;
                        ed.Sports = model.Sports;
                        ed.CurrentBuildingName = model.CurrentBuildingName;
                        ed.CurrentCity = model.CurrentCity;
                        ed.CurrentCountry = model.CurrentCountry;
                        ed.CurrentDoorNumber = model.CurrentDoorNumber;
                        ed.EContactNo1 = model.EContactNo1;
                        ed.EContactName1 = model.EContactName;
                        ed.EContactRelationship1 = model.EContactRelationship1;
                        ed.EContactNo2 = model.EContactNo2;
                        ed.EContactName2 = model.EContactName2;
                        ed.EContactRelationship2 = model.EContactRelationship2;
                        ed.CurrentLocation = model.CurrentLocation;
                        ed.CurrentPinCode = model.CurrentPinCode;
                        ed.CurrentState = model.CurrentState;
                        ed.CurrentStreet = model.CurrentStreet;
                        ed.PermanentBuildingName = model.PermanentBuildingName;
                        ed.PermanentCity = model.PermanentCity;
                        ed.PermanentCountry = model.PermanentCountry;
                        ed.PermanentDoorNumber = model.PermanentDoorNumber;
                        ed.PermanentLocation = model.PermanentLocation;
                        ed.PermanentPinCode = model.PermanentPinCode;
                        ed.PermanentState = model.PermanentState;
                        ed.PermanentStreet = model.PermanentStreet;
                        ed.Caste = model.Caste;
                        ed.Region = model.Region;
                        ed.Country = model.Country;
                        ed.Nationality = model.Nationality;
                        ed.CreatedBy = model.LoginId;
                        ed.CreatedDate = DateTime.Now;
                        ed.LastUpdatedBy = model.LoginId;
                        ed.LastUpdatedDate = DateTime.Now;
                        ed.IsActive = true;
                        ed.IsUpdated = false;
                        ed.IsDeleted = false;
                        //ed.LastUpdatedBy = model.EmpId;
                        //ed.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeDetails.Add(ed);
                        DB.SaveChanges();

                        EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
                        edvm.msg = "Added";

                        return edvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeDetailViewModel UpdateEmployeeDetails(EmployeeDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empid = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.Id != 0) ? model.Id : 0;

                var empdetails = (from emp in DB.EmployeeDetails
                                  where emp.Id == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id == 0)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed.EmpId = empid;
                        ed.AMobileNo = model.AMobileNo;
                        ed.PMailId = model.PMailId;
                        ed.FatherName = model.FatherName;
                        ed.MotherName = model.MotherName;
                        ed.HusbandName = model.HusbandName;
                        ed.FContactNo = model.FContactNo;
                        ed.MContactNo = model.MContactNo;
                        ed.HContactNo = model.HContactNo;
                        ed.EContactName = model.EContactName;
                        ed.EContactNo = model.EContactNo;
                        ed.EContactRelationship = model.EContactRelationship;
                        ed.Height = model.Height;
                        ed.Weight = model.Weight;
                        ed.DateOfAnniversary = model.DateOfAnniversary;
                        ed.Disability = model.Disability;
                        ed.TotalExperience = model.TotalExperience;
                        ed.RelevantExperience = model.RelevantExperience;
                        ed.ECActivities = model.ECActivities;

                        ed.EContactNo1 = model.EContactNo1;
                        ed.EContactName1 = model.EContactName1;
                        ed.EContactRelationship1 = model.EContactRelationship1;

                        ed.EContactNo2 = model.EContactNo2;
                        ed.EContactName2 = model.EContactName2;
                        ed.EContactRelationship2 = model.EContactRelationship2;
                        ed.Sports = model.Sports;
                        ed.CurrentBuildingName = model.CurrentBuildingName;
                        ed.CurrentCity = model.CurrentCity;
                        ed.CurrentCountry = model.CurrentCountry;
                        ed.CurrentDoorNumber = model.CurrentDoorNumber;
                        ed.CurrentLocation = model.CurrentLocation;
                        ed.CurrentPinCode = model.CurrentPinCode;
                        ed.CurrentState = model.CurrentState;
                        ed.CurrentStreet = model.CurrentStreet;
                        ed.PermanentBuildingName = model.PermanentBuildingName;
                        ed.PermanentCity = model.PermanentCity;
                        ed.PermanentCountry = model.PermanentCountry;
                        ed.PermanentDoorNumber = model.PermanentDoorNumber;
                        ed.PermanentLocation = model.PermanentLocation;
                        ed.PermanentPinCode = model.PermanentPinCode;
                        ed.PermanentState = model.PermanentState;
                        ed.PermanentStreet = model.PermanentStreet;
                        ed.Caste = model.Caste;
                        ed.Region = model.Region;
                        ed.Country = model.Country;
                        ed.Nationality = model.Nationality;
                        ed.CreatedBy = model.LoginId;
                        ed.CreatedDate = DateTime.Now;
                        ed.LastUpdatedBy = model.LoginId;
                        ed.LastUpdatedDate = DateTime.Now;
                        ed.IsActive = true;
                        ed.IsUpdated = false;
                        ed.IsDeleted = false;
                        //ed.LastUpdatedBy = model.EmpId;
                        //ed.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeDetails.Add(ed);
                        DB.SaveChanges();

                        EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
                        edvm.msg = "Added";

                        return edvm;
                    }
                    else
                    {
                        if (empdetails != null)
                        {
                            empdetails.Id = model.Id;
                            empdetails.EmpId = empid;
                            empdetails.AMobileNo = model.AMobileNo;
                            empdetails.PMailId = model.PMailId;
                            empdetails.FatherName = model.FatherName;
                            empdetails.MotherName = model.MotherName;
                            empdetails.HusbandName = model.HusbandName;
                            empdetails.FContactNo = model.FContactNo;
                            empdetails.MContactNo = model.MContactNo;
                            empdetails.HContactNo = model.HContactNo;
                            empdetails.EContactName = model.EContactName;
                            empdetails.EContactNo = model.EContactNo;
                            empdetails.EContactRelationship = model.EContactRelationship;
                            empdetails.Height = model.Height;
                            empdetails.Weight = model.Weight;
                            empdetails.DateOfAnniversary = model.DateOfAnniversary;
                            empdetails.Disability = model.Disability;
                            empdetails.TotalExperience = model.TotalExperience;
                            empdetails.RelevantExperience = model.RelevantExperience;
                            empdetails.ECActivities = model.ECActivities;
                            empdetails.EContactNo1 = model.EContactNo1;
                            empdetails.EContactName1 = model.EContactName1;
                            empdetails.EContactRelationship1 = model.EContactRelationship1;
                            empdetails.EContactNo2 = model.EContactNo2;
                            empdetails.EContactName2 = model.EContactName2;
                            empdetails.EContactRelationship2 = model.EContactRelationship2;
                            empdetails.Sports = model.Sports;
                            empdetails.CurrentBuildingName = model.CurrentBuildingName;
                            empdetails.CurrentCity = model.CurrentCity;
                            empdetails.CurrentCountry = model.CurrentCountry;
                            empdetails.CurrentDoorNumber = model.CurrentDoorNumber;
                            empdetails.CurrentLocation = model.CurrentLocation;
                            empdetails.CurrentPinCode = model.CurrentPinCode;
                            empdetails.CurrentState = model.CurrentState;
                            empdetails.CurrentStreet = model.CurrentStreet;
                            empdetails.PermanentBuildingName = model.PermanentBuildingName;
                            empdetails.PermanentCity = model.PermanentCity;
                            empdetails.PermanentCountry = model.PermanentCountry;
                            empdetails.PermanentDoorNumber = model.PermanentDoorNumber;
                            empdetails.PermanentLocation = model.PermanentLocation;
                            empdetails.PermanentPinCode = model.PermanentPinCode;
                            empdetails.PermanentState = model.PermanentState;
                            empdetails.PermanentStreet = model.PermanentStreet;
                            empdetails.Caste = model.Caste;
                            empdetails.Region = model.Region;
                            empdetails.Country = model.Country;
                            empdetails.Nationality = model.Nationality;
                            empdetails.LastUpdatedBy = model.LoginId;
                            empdetails.LastUpdatedDate = DateTime.Now;
                            empdetails.IsUpdated = true;
                            empdetails.IsDeleted = false;
                            DB.SaveChanges();

                            EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
                            edvm.msg = "Updated";

                            return edvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                        }
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeDetailViewModel DeleteEmployeeDetails(EmployeeDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empid = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.Id != 0) ? model.Id : 0;

                var empdetails = (from emp in DB.EmployeeDetails
                                  where emp.Id == id && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).FirstOrDefault();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        empdetails.IsActive = true;
                        empdetails.IsDeleted = true;
                        empdetails.LastUpdatedBy = loginId;
                        empdetails.LastUpdatedDate = DateTime.Now;

                        DB.SaveChanges();

                        EmployeeDetailViewModel edvm = new EmployeeDetailViewModel();
                        edvm.msg = "deleted";

                        return edvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Department Details Not Found");
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
        public FileUploadAPIViewModel UploadImage(FileUploadAPIViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.ImageType))
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid request");
                }

                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count == 0)
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
                }

                string folderName = "";

                var imageType = model.ImageType.ToUpper();

                if (imageType == "PROFILEPIC")
                {
                    folderName = "ProfilePic";
                }
                else if (imageType == "LOGO")
                {
                    folderName = "Logo";
                }
                else if (imageType == "LOGOWITHADDRESS")
                {
                    folderName = "LogoWithAddress";
                }
                else if (imageType == "WEBAPPLOGO")
                {
                    folderName = "WebAppLogo";
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid Image Type");
                }

                string baseVirtualPath = $"~/Uploads/Images/{folderName}";
                string uploadDir = HttpContext.Current.Server.MapPath(baseVirtualPath);

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string savedFileName = "";
                string physicalPath = "";

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var extension = Path.GetExtension(postedFile.FileName).ToLower();

                        // ✅ Validate extension
                        if (extension != ".jpg")
                        {
                            throw new CustomApiException(HttpStatusCode.BadRequest, "Only JPG files are allowed");
                        }

                        // ✅ Unique file name
                        savedFileName = $"{folderName}_{model.EmpId}_{DateTime.Now:yyyyMMddHHmmss}.jpg";

                        physicalPath = Path.Combine(uploadDir, savedFileName);

                        postedFile.SaveAs(physicalPath);
                    }
                }

                // ✅ Return virtual path (important)
                var result = new FileUploadAPIViewModel
                {
                    msg = $"{folderName} Uploaded",
                    path = $"{baseVirtualPath}/{savedFileName}"
                };

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    $"File upload failed: {ex.Message}");
            }
        }
        //Screenshot upload function for WFH people
        public WFHFileUploadAPIViewModel WFHUploadImage(WFHFileUploadAPIViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
                }
                var path = "~/Uploads/Images/WorkFromHome/ScreenShot";
                var httpRequest = HttpContext.Current.Request;
                var docfiles = new List<string>();
                DateTime today = DateTime.Now;
                string monthName = today.ToString("MMMM"); // E.g., "September"
                string shortMonthName = today.ToString("MMM"); // E.g., "Sep"
                string formattedDate = today.ToString("dd-MM-yyyy");
                var uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/Images/WorkFromHome/ScreenShot/" + monthName);
                var uploadDir1 = HttpContext.Current.Server.MapPath("~/Uploads/Images/WorkFromHome/ScreenShot/" + monthName + "/" + model.EmpCode);
                var uploadDir2 = HttpContext.Current.Server.MapPath("~/Uploads/Images/WorkFromHome/ScreenShot/" + monthName + "/" + model.EmpCode + "/" + formattedDate);

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                if (!Directory.Exists(uploadDir1))
                {
                    Directory.CreateDirectory(uploadDir1);
                }
                if (!Directory.Exists(uploadDir2))
                {
                    Directory.CreateDirectory(uploadDir2);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    //var postedFile = model.file;

                    if (postedFile != null)
                    {
                        var docName = Path.GetFileName(postedFile.FileName);
                        var extension = Path.GetExtension(docName).ToLower();

                        var ImgName = "ScreenShot_" + model.EmpCode + "_" + model.EmpId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        path = Path.Combine(uploadDir2, ImgName);

                        postedFile.SaveAs(path);
                        docfiles.Add(path);
                    }
                }

                ////if (model.file != null || model.file.Length != 0)
                ////{
                ////    var fileName = "ScreenShot_" + model.EmpCode + "_" + model.EmpId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(model.file.FileName);
                ////    var filePath = Path.Combine(uploadDir2, fileName);

                ////    // Save the file to the server
                ////    using (var stream = new FileStream(filePath, FileMode.Create))
                ////    {
                ////        model.file.CopyToAsync(stream);
                ////    }
                ////}

                WFHFileUploadAPIViewModel dmvm = new WFHFileUploadAPIViewModel();
                dmvm.msg = "ScreenShot is Uploaded";
                dmvm.path = path;

                return dmvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }



        //public async Task<WFHFileUploadAPIViewModel> WFHUploadImageAsync(WFHFileUploadAPIViewModel model)
        //{
        //    try
        //    {
        //        if (model == null || model.files == null || model.files.Length == 0)
        //        {
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
        //        }

        //        // Define paths for upload directories
        //        DateTime today = DateTime.Now;
        //        string monthName = today.ToString("MMMM");
        //        string formattedDate = today.ToString("dd-MM-yyyy");

        //        // Create necessary directory paths
        //        var basePath = "~/Uploads/Images/WorkFromHome/ScreenShot";
        //        var uploadDir = HttpContext.Current.Server.MapPath($"{basePath}/{monthName}");
        //        var empDir = HttpContext.Current.Server.MapPath($"{basePath}/{monthName}/{model.EmpCode}");
        //        var dateDir = HttpContext.Current.Server.MapPath($"{basePath}/{monthName}/{model.EmpCode}/{formattedDate}");

        //        // Create directories if they do not exist
        //        if (!Directory.Exists(uploadDir))
        //        {
        //            Directory.CreateDirectory(uploadDir);
        //        }
        //        if (!Directory.Exists(empDir))
        //        {
        //            Directory.CreateDirectory(empDir);
        //        }
        //        if (!Directory.Exists(dateDir))
        //        {
        //            Directory.CreateDirectory(dateDir);
        //        }

        //        // Define the file name and path
        //        var fileName = $"ScreenShot_{model.EmpCode}_{model.EmpId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(model.files.FileName)}";
        //        var filePath = Path.Combine(dateDir, fileName);

        //        // Save the file asynchronously
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await model.files.CopyToAsync(stream);
        //        }

        //        // Create response view model with the file path
        //        WFHFileUploadAPIViewModel response = new WFHFileUploadAPIViewModel
        //        {
        //            msg = "Screenshot is uploaded successfully.",
        //            path = filePath // Return the full file path for confirmation
        //        };

        //        return response;
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return a more generic error message for unexpected exceptions
        //        throw new CustomApiException(HttpStatusCode.InternalServerError, $"An error occurred while uploading the screenshot: {ex.Message}");
        //    }
        //}

        //public WFHFileUploadAPIViewModel WFHUploadImage([FromForm] WFHFileUploadAPIViewModel model)
        //{
        //    try
        //    {
        //        if (model == null || model.files == null)
        //        {
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
        //            //return BadRequest("No file uploaded or invalid model");
        //        }

        //        // Define the directory path for storing uploaded images
        //        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "WorkFromHome", "ScreenShot");
        //        var formattedDate = DateTime.Now.ToString("dd-MM-yyyy");
        //        var monthName = DateTime.Now.ToString("MMMM");

        //        // Path: ~/Uploads/Images/WorkFromHome/ScreenShot/{Month}/{EmpCode}/{Date}
        //        var uploadDir2 = Path.Combine(uploadDir, monthName, model.EmpCode, formattedDate);

        //        // Create directory if it doesn't exist
        //        if (!Directory.Exists(uploadDir2))
        //        {
        //            Directory.CreateDirectory(uploadDir2);
        //        }

        //        // Construct the file name using the given format
        //        var fileName = $"ScreenShot_{model.EmpCode}_{model.EmpId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(model.files.FileName)}";
        //        var filePath = Path.Combine(uploadDir2, fileName);

        //        // Save the file to the server
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.files.CopyToAsync(stream);
        //        }

        //        // Return a success message with the file path
        //        //return Ok(new { message = "Screenshot uploaded successfully", path = filePath });

        //        WFHFileUploadAPIViewModel dmvm = new WFHFileUploadAPIViewModel();
        //        dmvm.msg = "ScreenShot is Uploaded";
        //        dmvm.path = filePath;

        //        return dmvm;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new CustomApiException(HttpStatusCode.InternalServerError, $"An error occurred while uploading the screenshot: {ex.Message}");
        //    }
        //}
        public FileUploadAPIViewModel UploadFileEducation(FileUploadAPIViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
                }
                var path = "~/Uploads/Images/ProfilePic";
                var httpRequest = HttpContext.Current.Request;
                var docfiles = new List<string>();
                var uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/Images/ProfilePic");

                if (model.DocName.ToUpper() == "GRADUATE")
                {
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Education/Graduate");
                }
                else if (model.DocName.ToUpper() == "POST GRADUATE")
                {
                    model.DocName = "POSTGRADUATE";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Education/PostGraduate");
                }
                else if (model.DocName.ToUpper() == "HSC")
                {
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Education/HSC");
                }
                else if (model.DocName.ToUpper() == "SSLC")
                {
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Education/SSLC");
                }
                else if (model.DocName.ToUpper() == "OTHERS")
                {
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Education/Others");
                }


                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var docName = Path.GetFileName(postedFile.FileName);
                        var extension = Path.GetExtension(docName).ToLower();
                        var ImgName = model.DocName.ToUpper() + "_" + +model.EmpId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        path = Path.Combine(uploadDir, ImgName);

                        postedFile.SaveAs(path);
                        docfiles.Add(path);
                    }
                }

                FileUploadAPIViewModel dmvm = new FileUploadAPIViewModel();
                dmvm.msg = "Education Document Uploaded";
                dmvm.path = path;

                return dmvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            //catch (Exception ex)
            //{
            //    throw new CustomApiException(HttpStatusCode.InternalServerError, "An error occurred while uploading the document(s)");
            //}
        }
        public FileUploadAPIViewModel UploadFileGovt(FileUploadAPIViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
                }
                var path = "~/Uploads/Images/ProfilePic";
                var httpRequest = HttpContext.Current.Request;
                var docfiles = new List<string>();
                var uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/Images/ProfilePic");

                if (model.DocName.ToUpper() == "AADHAR CARD")
                {
                    model.DocName = "AADHARCARD";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Govt/Aadharcard");
                }
                else if (model.DocName.ToUpper() == "PAN CARD")
                {
                    model.DocName = "PANCARD";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Govt/Pancard");
                }
                else if (model.DocName.ToUpper() == "VOTER ID")
                {
                    model.DocName = "VOTERID";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Govt/VoterId");
                }
                else if (model.DocName.ToUpper() == "DRIVING LISENCE")
                {
                    model.DocName = "DRIVINGLISENCE";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Govt/Drivinglisence");
                }
                else if (model.DocName.ToUpper() == "OTHERS")
                {
                    model.DocName = "OTHERS";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Govt/Others");
                }


                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var docName = Path.GetFileName(postedFile.FileName);
                        var extension = Path.GetExtension(docName).ToLower();
                        var ImgName = model.DocName.ToUpper() + "_" + model.EmpId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        path = Path.Combine(uploadDir, ImgName);

                        postedFile.SaveAs(path);
                        docfiles.Add(path);
                    }
                }

                FileUploadAPIViewModel dmvm = new FileUploadAPIViewModel();
                dmvm.msg = "Govt Document Uploaded";
                dmvm.path = path;

                return dmvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            //catch (Exception ex)
            //{
            //    throw new CustomApiException(HttpStatusCode.InternalServerError, "An error occurred while uploading the document(s)");
            //}
        }
        public List<DDDocViewModel> GetDDEducationDoc(DDDocViewModel compdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (compdd.EmpId != 0) ? compdd.EmpId : 0;

                var Docdetails = (from doc in DB.DocumentMasters
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 1
                                  select new DDDocViewModel
                                  {
                                      DocId = doc.DocId,
                                      EduId = doc.EduId,
                                      DocName = doc.DocName,
                                  }).ToList();

                if (EmpId != 0)
                {
                    if (Docdetails != null)
                    {
                        return Docdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
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
        public List<DDDocViewModel> GetDDGovtDoc(DDDocViewModel compdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (compdd.EmpId != 0) ? compdd.EmpId : 0;

                var Docdetails = (from doc in DB.DocumentMasters
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 2
                                  select new DDDocViewModel
                                  {
                                      DocId = doc.DocId,
                                      EduId = doc.EduId,
                                      DocName = doc.DocName,
                                  }).ToList();

                if (EmpId != 0)
                {
                    if (Docdetails != null)
                    {
                        return Docdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Govt Document Details Not Found");
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
        public List<EmployeeEducationViewModel> GetAllEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var docdetails = (from edu in DB.EmployeeEducations
                                  join doc in DB.DocumentMasters on edu.DocId equals doc.DocId
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 1 && edu.IsActive == true && edu.IsDeleted == false
                                  select edu).OrderByDescending(x => x.Id).ToList();

                if (loginId != 0)
                {
                    if (docdetails != null)
                    {
                        List<EmployeeEducationViewModel> lstofeducationDoc = new List<EmployeeEducationViewModel>();

                        for (int i = 0; i < docdetails.Count(); i++)
                        {
                            EmployeeEducationViewModel eevm = new EmployeeEducationViewModel();
                            eevm.Id = docdetails[i].Id;
                            eevm.EmpId = docdetails[i].EmpId;
                            eevm.DocId = docdetails[i].DocId;
                            eevm.Others = docdetails[i].Others;
                            eevm.School = docdetails[i].School;
                            eevm.DegreeId = docdetails[i].DegreeId;
                            eevm.Filed = docdetails[i].Filed;
                            eevm.StartDate = docdetails[i].StartDate;
                            eevm.EndDate = docdetails[i].EndDate;
                            eevm.Grade = docdetails[i].Grade;
                            eevm.Description = docdetails[i].Description;
                            eevm.Path = docdetails[i].Path;

                            if (eevm.Path != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = eevm.Path.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                eevm.Path = "Uploads" + lnkval;
                            }

                            eevm.CreatedBy = docdetails[i].CreatedBy;
                            eevm.CreatedDate = docdetails[i].CreatedDate;
                            eevm.LastUpdatedBy = docdetails[i].LastUpdatedBy;
                            eevm.LastUpdatedDate = docdetails[i].LastUpdatedDate;
                            eevm.IsActive = docdetails[i].IsActive;
                            eevm.IsUpdated = docdetails[i].IsUpdated;
                            eevm.IsDeleted = docdetails[i].IsDeleted;
                            lstofeducationDoc.Add(eevm);

                        }
                        return lstofeducationDoc;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
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
        public List<EmployeeEducationViewModel> GetEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var docdetails = (from edu in DB.EmployeeEducations
                                  join doc in DB.DocumentMasters on edu.DocId equals doc.DocId
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 1 && edu.EmpId == EmpId && edu.IsActive == true && edu.IsDeleted == false
                                  select edu).OrderByDescending(x => x.Id).ToList();

                if (loginId != 0)
                {
                    if (docdetails != null)
                    {
                        List<EmployeeEducationViewModel> lstofeducationDoc = new List<EmployeeEducationViewModel>();

                        for (int i = 0; i < docdetails.Count(); i++)
                        {
                            EmployeeEducationViewModel eevm = new EmployeeEducationViewModel();
                            eevm.Id = docdetails[i].Id;
                            eevm.EmpId = docdetails[i].EmpId;
                            eevm.DocId = docdetails[i].DocId;
                            eevm.Others = docdetails[i].Others;
                            eevm.School = docdetails[i].School;
                            eevm.DegreeId = docdetails[i].DegreeId;
                            eevm.Filed = docdetails[i].Filed;
                            eevm.StartDate = docdetails[i].StartDate;
                            eevm.EndDate = docdetails[i].EndDate;
                            eevm.Grade = docdetails[i].Grade;
                            eevm.Description = docdetails[i].Description;
                            eevm.Path = docdetails[i].Path;

                            if (eevm.Path != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = eevm.Path.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                eevm.Path = "Uploads" + lnkval;
                            }

                            eevm.CreatedBy = docdetails[i].CreatedBy;
                            eevm.CreatedDate = docdetails[i].CreatedDate;
                            eevm.LastUpdatedBy = docdetails[i].LastUpdatedBy;
                            eevm.LastUpdatedDate = docdetails[i].LastUpdatedDate;
                            eevm.IsActive = docdetails[i].IsActive;
                            eevm.IsUpdated = docdetails[i].IsUpdated;
                            eevm.IsDeleted = docdetails[i].IsDeleted;
                            lstofeducationDoc.Add(eevm);

                        }
                        return lstofeducationDoc;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
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
        public EmployeeEducationViewModel AddEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var edudetails = (from edu in DB.EmployeeEducations
                                  where edu.EmpId == EmpId && edu.DocId == model.DocId
                                  && edu.IsActive == true && edu.IsDeleted == false
                                  select edu).ToList();

                if (loginId != 0)
                {
                    if (edudetails.Count() == 0)
                    {
                        EmployeeEducation ee = new EmployeeEducation();
                        //em.EmpId = model.modelId;
                        ee.Id = model.Id;
                        ee.EmpId = model.EmpId;
                        ee.DocId = model.DocId;
                        ee.Others = "";
                        ee.School = model.School;
                        ee.DegreeId = model.DocName;
                        if (model.DocName.ToUpper() == "OTHERS")
                        {
                            ee.Others = model.Others;
                        }
                        ee.Filed = model.Filed;
                        ee.StartDate = model.StartDate;
                        ee.EndDate = model.EndDate;
                        ee.Grade = model.Grade;
                        ee.Description = model.Description;
                        ee.Path = (model.Path != null) ? model.Path : "";
                        ee.IsActive = true;
                        ee.IsUpdated = false;
                        ee.IsDeleted = false;
                        ee.CreatedBy = model.LoginId;
                        ee.CreatedDate = DateTime.Now;
                        ee.LastUpdatedBy = model.LoginId;
                        ee.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeEducations.Add(ee);
                        DB.SaveChanges();

                        EmployeeEducationViewModel emvm = new EmployeeEducationViewModel();
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeEducationViewModel UpdateEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.Id != 0) ? model.Id : 0;

                var edudetails = (from edu in DB.EmployeeEducations
                                  where edu.EmpId == EmpId && edu.Id == id && edu.IsActive == true && edu.IsDeleted == false
                                  select edu).FirstOrDefault();

                if (loginId != 0)
                {
                    if (edudetails != null)
                    {
                        edudetails.EmpId = model.EmpId;
                        edudetails.DocId = model.DocId;
                        edudetails.Others = "";
                        edudetails.School = model.School;
                        edudetails.DegreeId = model.DocName;
                        if (model.DocName.ToUpper() == "OTHERS")
                        {
                            edudetails.Others = model.Others;
                        }
                        edudetails.Filed = model.Filed;
                        edudetails.StartDate = model.StartDate;
                        edudetails.EndDate = model.EndDate;
                        edudetails.Grade = model.Grade;
                        edudetails.Description = model.Description;
                        edudetails.Path = (model.Path != null) ? model.Path : "";
                        edudetails.IsActive = true;
                        edudetails.IsUpdated = true;
                        edudetails.IsDeleted = false;
                        edudetails.LastUpdatedBy = model.LoginId;
                        edudetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeEducationViewModel emvm = new EmployeeEducationViewModel();
                        emvm.msg = "Updated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeEducationViewModel DeleteEducationDoc(EmployeeEducationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.Id != 0) ? model.Id : 0;

                var edudetails = (from edu in DB.EmployeeEducations
                                  where edu.EmpId == EmpId && edu.Id == id && edu.IsActive == true && edu.IsDeleted == false
                                  select edu).FirstOrDefault();

                if (loginId != 0)
                {
                    if (edudetails != null)
                    {
                        edudetails.IsActive = true;
                        edudetails.IsUpdated = true;
                        edudetails.IsDeleted = true;
                        edudetails.LastUpdatedBy = model.LoginId;
                        edudetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeEducationViewModel emvm = new EmployeeEducationViewModel();
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
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
        public List<EmployeeGovtDocViewModel> GetAllGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var docdetails = (from govt in DB.EmployeeGovtDocs
                                  join doc in DB.DocumentMasters on govt.DocId equals doc.DocId
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 2 && govt.IsActive == true && govt.IsDeleted == false
                                  select govt).OrderByDescending(x => x.GovId).ToList();

                if (loginId != 0)
                {
                    if (docdetails != null)
                    {
                        List<EmployeeGovtDocViewModel> lstofgovtDoc = new List<EmployeeGovtDocViewModel>();

                        for (int i = 0; i < docdetails.Count(); i++)
                        {
                            EmployeeGovtDocViewModel egdvm = new EmployeeGovtDocViewModel();
                            egdvm.GovId = docdetails[i].GovId;
                            egdvm.EmpId = docdetails[i].EmpId;
                            egdvm.DocId = docdetails[i].DocId;
                            egdvm.Others = docdetails[i].Others;
                            egdvm.DocName = docdetails[i].DocName;
                            egdvm.Name = docdetails[i].Name;
                            egdvm.DocNo = docdetails[i].DocNo;
                            egdvm.IssuedDate = docdetails[i].IssuedDate;
                            egdvm.ExpiredDate = docdetails[i].ExpiredDate;
                            egdvm.Description = docdetails[i].Description;
                            egdvm.Path = docdetails[i].Path;

                            if (egdvm.Path != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = egdvm.Path.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                egdvm.Path = "Uploads" + lnkval;
                            }

                            egdvm.CreatedBy = docdetails[i].CreatedBy;
                            egdvm.CreatedDate = docdetails[i].CreatedDate;
                            egdvm.LastUpdatedBy = docdetails[i].LastUpdatedBy;
                            egdvm.LastUpdatedDate = docdetails[i].LastUpdatedDate;
                            egdvm.IsActive = docdetails[i].IsActive;
                            egdvm.IsUpdated = docdetails[i].IsUpdated;
                            egdvm.IsDeleted = docdetails[i].IsDeleted;
                            lstofgovtDoc.Add(egdvm);

                        }
                        return lstofgovtDoc;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
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
        public List<EmployeeGovtDocViewModel> GetGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var docdetails = (from govt in DB.EmployeeGovtDocs
                                  join doc in DB.DocumentMasters on govt.DocId equals doc.DocId
                                  where doc.IsActive == true && doc.IsDeleted == false && doc.EduId == 2 && govt.EmpId == EmpId && govt.IsActive == true && govt.IsDeleted == false
                                  select govt).OrderByDescending(x => x.GovId).ToList();

                if (loginId != 0)
                {
                    if (docdetails != null)
                    {
                        List<EmployeeGovtDocViewModel> lstofgovtDoc = new List<EmployeeGovtDocViewModel>();

                        for (int i = 0; i < docdetails.Count(); i++)
                        {
                            EmployeeGovtDocViewModel egdvm = new EmployeeGovtDocViewModel();
                            egdvm.GovId = docdetails[i].GovId;
                            egdvm.EmpId = docdetails[i].EmpId;
                            egdvm.DocId = docdetails[i].DocId;
                            egdvm.Others = docdetails[i].Others;
                            egdvm.DocName = docdetails[i].DocName;
                            egdvm.Name = docdetails[i].Name;
                            egdvm.DocNo = docdetails[i].DocNo;
                            egdvm.IssuedDate = docdetails[i].IssuedDate;
                            egdvm.ExpiredDate = docdetails[i].ExpiredDate;
                            egdvm.Description = docdetails[i].Description;
                            egdvm.Path = docdetails[i].Path;

                            if (egdvm.Path != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = egdvm.Path.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                egdvm.Path = "Uploads" + lnkval;
                            }

                            egdvm.CreatedBy = docdetails[i].CreatedBy;
                            egdvm.CreatedDate = docdetails[i].CreatedDate;
                            egdvm.LastUpdatedBy = docdetails[i].LastUpdatedBy;
                            egdvm.LastUpdatedDate = docdetails[i].LastUpdatedDate;
                            egdvm.IsActive = docdetails[i].IsActive;
                            egdvm.IsUpdated = docdetails[i].IsUpdated;
                            egdvm.IsDeleted = docdetails[i].IsDeleted;
                            lstofgovtDoc.Add(egdvm);

                        }
                        return lstofgovtDoc;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
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
        public EmployeeGovtDocViewModel AddGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var govtdetails = (from govt in DB.EmployeeGovtDocs
                                   where govt.EmpId == EmpId && govt.DocId == model.DocId
                                   && govt.IsActive == true && govt.IsDeleted == false
                                   select govt).ToList();

                if (loginId != 0)
                {
                    if (govtdetails.Count() == 0)
                    {
                        EmployeeGovtDoc egd = new EmployeeGovtDoc();
                        //em.EmpId = model.modelId;
                        egd.EmpId = model.EmpId;
                        egd.DocId = model.DocId;
                        egd.Others = "";
                        egd.DocName = model.DocName;
                        egd.Name = model.Name;
                        if (model.DocName.ToUpper() == "OTHERS")
                        {
                            egd.Others = model.Others;
                        }
                        egd.DocNo = model.DocNo;
                        egd.IssuedDate = model.IssuedDate;
                        egd.ExpiredDate = model.ExpiredDate;
                        egd.Description = model.Description;
                        egd.Path = (model.Path != null) ? model.Path : "";
                        egd.IsActive = true;
                        egd.IsUpdated = false;
                        egd.IsDeleted = false;
                        egd.CreatedBy = model.LoginId;
                        egd.CreatedDate = DateTime.Now;
                        egd.LastUpdatedBy = model.LoginId;
                        egd.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeGovtDocs.Add(egd);
                        DB.SaveChanges();

                        EmployeeGovtDocViewModel emvm = new EmployeeGovtDocViewModel();
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeGovtDocViewModel UpdateGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.GovId != 0) ? model.GovId : 0;

                var govtdetails = (from govt in DB.EmployeeGovtDocs
                                   where govt.EmpId == EmpId && govt.GovId == id && govt.IsActive == true && govt.IsDeleted == false
                                   select govt).FirstOrDefault();

                if (loginId != 0)
                {
                    if (govtdetails != null)
                    {
                        govtdetails.EmpId = model.EmpId;
                        govtdetails.DocId = model.DocId;
                        govtdetails.Others = "";
                        govtdetails.DocName = model.DocName;
                        govtdetails.Name = model.Name;
                        if (model.DocName.ToUpper() == "OTHERS")
                        {
                            govtdetails.Others = model.Others;
                        }
                        govtdetails.DocNo = model.DocNo;
                        govtdetails.IssuedDate = model.IssuedDate;
                        govtdetails.ExpiredDate = model.ExpiredDate;
                        govtdetails.Description = model.Description;
                        govtdetails.Path = (model.Path != null) ? model.Path : "";
                        govtdetails.IsActive = true;
                        govtdetails.IsUpdated = true;
                        govtdetails.IsDeleted = false;
                        govtdetails.LastUpdatedBy = model.LoginId;
                        govtdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeGovtDocViewModel emvm = new EmployeeGovtDocViewModel();
                        emvm.msg = "Updated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Education Document Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeGovtDocViewModel DeleteGovtDoc(EmployeeGovtDocViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.GovId != 0) ? model.GovId : 0;

                var govtdetails = (from govt in DB.EmployeeGovtDocs
                                   where govt.EmpId == EmpId && govt.GovId == id && govt.IsActive == true && govt.IsDeleted == false
                                   select govt).FirstOrDefault();

                if (loginId != 0)
                {
                    if (govtdetails != null)
                    {
                        govtdetails.IsActive = true;
                        govtdetails.IsUpdated = true;
                        govtdetails.IsDeleted = true;
                        govtdetails.LastUpdatedBy = model.LoginId;
                        govtdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeGovtDocViewModel emvm = new EmployeeGovtDocViewModel();
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Govt Document Details Not Found");
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
        public List<EmployeeAccDetailViewModel> GetAllEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var accdetails = (from acc in DB.EmployeeAccDetails
                                  where acc.IsActive == true && acc.IsDeleted == false
                                  select acc).OrderByDescending(x => x.AccId).ToList();

                if (loginId != 0)
                {
                    if (accdetails != null)
                    {
                        List<EmployeeAccDetailViewModel> lstofgovtDoc = new List<EmployeeAccDetailViewModel>();

                        for (int i = 0; i < accdetails.Count(); i++)
                        {
                            EmployeeAccDetailViewModel eadvm = new EmployeeAccDetailViewModel();
                            eadvm.AccId = accdetails[i].AccId;
                            eadvm.EmpId = accdetails[i].EmpId;
                            int? empid = accdetails[i].EmpId;
                            eadvm.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.EmpCode).FirstOrDefault();
                            eadvm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.FirstName).FirstOrDefault() + " " + DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.MiddleName).FirstOrDefault() + " " + DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.LastName).FirstOrDefault();
                            eadvm.BankName = accdetails[i].BankName;
                            eadvm.BranchName = accdetails[i].BranchName;
                            eadvm.IFSCCode = accdetails[i].IFSCCode;
                            eadvm.AccHolderName = accdetails[i].AccHolderName;
                            eadvm.AccNo = accdetails[i].AccNo;
                            eadvm.PFNo = accdetails[i].PFNo;
                            eadvm.ESIInsuranceNo = accdetails[i].ESIInsuranceNo;
                            eadvm.HealthInsuranceNo = accdetails[i].HealthInsuranceNo;
                            eadvm.PANNo = accdetails[i].PANNo;
                            eadvm.UANNo = accdetails[i].UANNo;
                            eadvm.AadharNo = accdetails[i].AadharNo;
                            eadvm.MobileNo = accdetails[i].MobileNo;
                            eadvm.Status = accdetails[i].Status;
                            eadvm.CreatedBy = accdetails[i].CreatedBy;
                            eadvm.CreatedDate = accdetails[i].CreatedDate;
                            eadvm.LastUpdatedBy = accdetails[i].LastUpdatedBy;
                            eadvm.LastUpdatedDate = accdetails[i].LastUpdatedDate;
                            eadvm.IsActive = accdetails[i].IsActive;
                            eadvm.IsUpdated = accdetails[i].IsUpdated;
                            eadvm.IsDeleted = accdetails[i].IsDeleted;
                            lstofgovtDoc.Add(eadvm);

                        }
                        return lstofgovtDoc;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Account Details Not Found");
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
        public EmployeeAccDetailViewModel GetEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var accdetails = (from acc in DB.EmployeeAccDetails
                                  where acc.EmpId == EmpId && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).OrderByDescending(x => x.AccId).FirstOrDefault();

                if (loginId != 0)
                {
                    if (accdetails != null)
                    {
                        EmployeeAccDetailViewModel eadvm = new EmployeeAccDetailViewModel();
                        eadvm.AccId = accdetails.AccId;
                        eadvm.EmpId = accdetails.EmpId;
                        int? empid = accdetails.EmpId;
                        eadvm.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.EmpCode).FirstOrDefault();
                        eadvm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.FirstName).FirstOrDefault() + " " + DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.MiddleName).FirstOrDefault() + " " + DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.LastName).FirstOrDefault();
                        eadvm.BankName = accdetails.BankName;
                        eadvm.BranchName = accdetails.BranchName;
                        eadvm.IFSCCode = accdetails.IFSCCode;
                        eadvm.AccHolderName = accdetails.AccHolderName;
                        eadvm.AccNo = accdetails.AccNo;
                        eadvm.PFNo = accdetails.PFNo;
                        eadvm.ESIInsuranceNo = accdetails.ESIInsuranceNo;
                        eadvm.HealthInsuranceNo = accdetails.HealthInsuranceNo;
                        eadvm.PANNo = accdetails.PANNo;
                        eadvm.UANNo = accdetails.UANNo;
                        eadvm.AadharNo = accdetails.AadharNo;
                        eadvm.MobileNo = accdetails.MobileNo;
                        eadvm.Status = accdetails.Status;
                        eadvm.CreatedBy = accdetails.CreatedBy;
                        eadvm.CreatedDate = accdetails.CreatedDate;
                        eadvm.LastUpdatedBy = accdetails.LastUpdatedBy;
                        eadvm.LastUpdatedDate = accdetails.LastUpdatedDate;
                        eadvm.IsActive = accdetails.IsActive;
                        eadvm.IsUpdated = accdetails.IsUpdated;
                        eadvm.IsDeleted = accdetails.IsDeleted;
                        return eadvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Account Details Not Found");
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
        public EmployeeAccDetailViewModel AddEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var accdetails = (from acc in DB.EmployeeAccDetails
                                  where acc.EmpId == EmpId && acc.AccId == model.AccId
                                  && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).ToList();

                if (loginId != 0)
                {
                    if (accdetails.Count() == 0)
                    {
                        EmployeeAccDetail ead = new EmployeeAccDetail();
                        //em.EmpId = model.modelId;
                        ead.EmpId = model.EmpId;
                        ead.BankName = model.BankName;
                        ead.BranchName = model.BranchName;
                        ead.IFSCCode = model.IFSCCode;
                        ead.AccHolderName = model.AccHolderName;
                        ead.AccNo = model.AccNo;
                        ead.PFNo = model.PFNo;
                        ead.ESIInsuranceNo = model.ESIInsuranceNo;
                        ead.HealthInsuranceNo = model.HealthInsuranceNo;
                        ead.PANNo = model.PANNo;
                        ead.UANNo = model.UANNo;
                        ead.AadharNo = model.AadharNo;
                        ead.MobileNo = model.MobileNo;
                        ead.Status = true;
                        ead.IsActive = true;
                        ead.IsUpdated = false;
                        ead.IsDeleted = false;
                        ead.CreatedBy = model.LoginId;
                        ead.CreatedDate = DateTime.Now;
                        ead.LastUpdatedBy = model.LoginId;
                        ead.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeAccDetails.Add(ead);
                        DB.SaveChanges();

                        EmployeeAccDetailViewModel emvm = new EmployeeAccDetailViewModel();
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Account Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeAccDetailViewModel UpdateEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.AccId != 0) ? model.AccId : 0;

                var accdetails = (from acc in DB.EmployeeAccDetails
                                  where acc.EmpId == EmpId && acc.AccId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id == 0)
                    {
                        EmployeeAccDetail ead = new EmployeeAccDetail();
                        //em.EmpId = model.modelId;
                        ead.EmpId = model.EmpId;
                        ead.BankName = model.BankName;
                        ead.IFSCCode = model.IFSCCode;
                        ead.BranchName = model.BranchName;
                        ead.AccHolderName = model.AccHolderName;
                        ead.AccNo = model.AccNo;
                        ead.PFNo = model.PFNo;
                        ead.ESIInsuranceNo = model.ESIInsuranceNo;
                        ead.HealthInsuranceNo = model.HealthInsuranceNo;
                        ead.PANNo = model.PANNo;
                        ead.UANNo = model.UANNo;
                        ead.AadharNo = model.AadharNo;
                        ead.MobileNo = model.MobileNo;
                        ead.Status = true;
                        ead.IsActive = true;
                        ead.IsUpdated = false;
                        ead.IsDeleted = false;
                        ead.CreatedBy = model.LoginId;
                        ead.CreatedDate = DateTime.Now;
                        ead.LastUpdatedBy = model.LoginId;
                        ead.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeAccDetails.Add(ead);
                        DB.SaveChanges();

                        EmployeeAccDetailViewModel emvm = new EmployeeAccDetailViewModel();
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        if (accdetails != null)
                        {
                            accdetails.EmpId = model.EmpId;
                            accdetails.BankName = model.BankName;
                            accdetails.IFSCCode = model.IFSCCode;
                            accdetails.BranchName = model.BranchName;
                            accdetails.AccHolderName = model.AccHolderName;
                            accdetails.AccNo = model.AccNo;
                            accdetails.PFNo = model.PFNo;
                            accdetails.ESIInsuranceNo = model.ESIInsuranceNo;
                            accdetails.HealthInsuranceNo = model.HealthInsuranceNo;
                            accdetails.PANNo = model.PANNo;
                            accdetails.UANNo = model.UANNo;
                            accdetails.AadharNo = model.AadharNo;
                            accdetails.MobileNo = model.MobileNo;
                            accdetails.Status = true;
                            accdetails.IsActive = true;
                            accdetails.IsUpdated = true;
                            accdetails.IsDeleted = false;
                            accdetails.LastUpdatedBy = model.LoginId;
                            accdetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            EmployeeAccDetailViewModel emvm = new EmployeeAccDetailViewModel();
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Account Details Not Found");
                        }
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeAccDetailViewModel DeleteEmpAccDetails(EmployeeAccDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.AccId != 0) ? model.AccId : 0;

                var accdetails = (from acc in DB.EmployeeAccDetails
                                  where acc.EmpId == EmpId && acc.AccId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (accdetails != null)
                    {
                        accdetails.IsActive = true;
                        accdetails.IsUpdated = true;
                        accdetails.IsDeleted = true;
                        accdetails.LastUpdatedBy = model.LoginId;
                        accdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeAccDetailViewModel emvm = new EmployeeAccDetailViewModel();
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Account Details Not Found");
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
        public FileUploadAPIViewModel UploadFileCareer(FileUploadAPIViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
                }
                var path = "~/Uploads/File/Career";
                var httpRequest = HttpContext.Current.Request;
                var docfiles = new List<string>();
                var uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Career");

                if (model.DocName.ToUpper() == "EXPERIENCE LETTER")
                {
                    model.DocName = "EXPERIENCELETTER";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Career/ExperienceLetter");
                }
                else if (model.DocName.ToUpper() == "OFFER LETTER")
                {
                    model.DocName = "OFFERLETTER";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Career/OfferLetter");
                }
                else if (model.DocName.ToUpper() == "PAY SLIP")
                {
                    model.DocName = "PAYSLIP";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Career/PaySlip");
                }
                else if (model.DocName.ToUpper() == "RELIEVING LETTER")
                {
                    model.DocName = "RELIEVINGLETTER";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Career/RelievingLetter");
                }
                else if (model.DocName.ToUpper() == "SALARY INCREMENT LETTER")
                {
                    model.DocName = "SALARYINCREMENTLETTER";
                    uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Career/SalaryIncrementLetter");
                }


                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var docName = Path.GetFileName(postedFile.FileName);
                        var extension = Path.GetExtension(docName).ToLower();
                        var ImgName = model.DocName.ToUpper() + "_" + model.EmpId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        path = Path.Combine(uploadDir, ImgName);

                        postedFile.SaveAs(path);
                        docfiles.Add(path);
                    }
                }

                FileUploadAPIViewModel dmvm = new FileUploadAPIViewModel();
                dmvm.msg = "Career Document Uploaded";
                dmvm.path = path;

                return dmvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<EmployeeCareerDetailViewModel> GetAllEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Careerdetails = (from car in DB.EmployeeCareerDetails
                                     where car.IsActive == true && car.IsDeleted == false
                                     select car).OrderByDescending(x => x.CareerId).ToList();

                if (loginId != 0)
                {
                    if (Careerdetails != null)
                    {
                        List<EmployeeCareerDetailViewModel> lstofcareerdetails = new List<EmployeeCareerDetailViewModel>();

                        for (int i = 0; i < Careerdetails.Count(); i++)
                        {
                            EmployeeCareerDetailViewModel ecdvm = new EmployeeCareerDetailViewModel();
                            ecdvm.CareerId = Careerdetails[i].CareerId;
                            ecdvm.EmpId = Careerdetails[i].EmpId;
                            ecdvm.Company = Careerdetails[i].Company;
                            ecdvm.Designation = Careerdetails[i].Designation;
                            ecdvm.FromDate = Careerdetails[i].FromDate;
                            ecdvm.ToDate = Careerdetails[i].ToDate;
                            ecdvm.Experience = Careerdetails[i].Experience;
                            ecdvm.PMonth1 = Careerdetails[i].PMonth1;
                            ecdvm.PaySlip1 = Careerdetails[i].PaySlip1;
                            ecdvm.PMonth2 = Careerdetails[i].PMonth2;
                            ecdvm.PaySlip2 = Careerdetails[i].PaySlip2;
                            ecdvm.PMonth3 = Careerdetails[i].PMonth3;
                            ecdvm.PaySlip3 = Careerdetails[i].PaySlip3;
                            ecdvm.OfferLetter = Careerdetails[i].OfferLetter;
                            ecdvm.SalaryLetter = Careerdetails[i].SalaryLetter;
                            ecdvm.ExperienceLetter = Careerdetails[i].ExperienceLetter;
                            ecdvm.RelievingLetter = Careerdetails[i].RelievingLetter;
                            ecdvm.ContactName = Careerdetails[i].ContactName;
                            ecdvm.ContactDesignation = Careerdetails[i].ContactDesignation;
                            ecdvm.ContactEmail = Careerdetails[i].ContactEmail;
                            ecdvm.ContactMobile = Careerdetails[i].ContactMobile;
                            ecdvm.CTC = Careerdetails[i].CTC;
                            ecdvm.Reason = Careerdetails[i].Reason;

                            if (ecdvm.PaySlip1 != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.PaySlip1.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.PaySlip1 = "Uploads" + lnkval;
                            }
                            if (ecdvm.PaySlip2 != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.PaySlip2.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.PaySlip2 = "Uploads" + lnkval;
                            }
                            if (ecdvm.PaySlip3 != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.PaySlip3.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.PaySlip3 = "Uploads" + lnkval;
                            }
                            if (ecdvm.OfferLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.OfferLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.OfferLetter = "Uploads" + lnkval;
                            }
                            if (ecdvm.SalaryLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.SalaryLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.SalaryLetter = "Uploads" + lnkval;
                            }
                            if (ecdvm.ExperienceLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.ExperienceLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.ExperienceLetter = "Uploads" + lnkval;
                            }
                            if (ecdvm.RelievingLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.RelievingLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.RelievingLetter = "Uploads" + lnkval;
                            }
                            ecdvm.CreatedBy = Careerdetails[i].CreatedBy;
                            ecdvm.CreatedDate = Careerdetails[i].CreatedDate;
                            ecdvm.LastUpdatedBy = Careerdetails[i].LastUpdatedBy;
                            ecdvm.LastUpdatedDate = Careerdetails[i].LastUpdatedDate;
                            ecdvm.IsActive = Careerdetails[i].IsActive;
                            ecdvm.IsUpdated = Careerdetails[i].IsUpdated;
                            ecdvm.IsDeleted = Careerdetails[i].IsDeleted;
                            lstofcareerdetails.Add(ecdvm);

                        }
                        return lstofcareerdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Career Details Not Found");
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
        public List<EmployeeCareerDetailViewModel> GetEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Careerdetails = (from car in DB.EmployeeCareerDetails
                                     where car.EmpId == EmpId && car.IsActive == true && car.IsDeleted == false
                                     select car).OrderByDescending(x => x.CareerId).ToList();

                if (loginId != 0)
                {
                    if (Careerdetails != null)
                    {
                        List<EmployeeCareerDetailViewModel> lstofcareerdetails = new List<EmployeeCareerDetailViewModel>();

                        for (int i = 0; i < Careerdetails.Count(); i++)
                        {
                            EmployeeCareerDetailViewModel ecdvm = new EmployeeCareerDetailViewModel();
                            ecdvm.CareerId = Careerdetails[i].CareerId;
                            ecdvm.EmpId = Careerdetails[i].EmpId;
                            ecdvm.Company = Careerdetails[i].Company;
                            ecdvm.Designation = Careerdetails[i].Designation;
                            ecdvm.FromDate = Careerdetails[i].FromDate;
                            ecdvm.ToDate = Careerdetails[i].ToDate;
                            ecdvm.Experience = Careerdetails[i].Experience;
                            ecdvm.PMonth1 = Careerdetails[i].PMonth1;
                            ecdvm.PaySlip1 = Careerdetails[i].PaySlip1;
                            ecdvm.PMonth2 = Careerdetails[i].PMonth2;
                            ecdvm.PaySlip2 = Careerdetails[i].PaySlip2;
                            ecdvm.PMonth3 = Careerdetails[i].PMonth3;
                            ecdvm.PaySlip3 = Careerdetails[i].PaySlip3;
                            ecdvm.OfferLetter = Careerdetails[i].OfferLetter;
                            ecdvm.SalaryLetter = Careerdetails[i].SalaryLetter;
                            ecdvm.ExperienceLetter = Careerdetails[i].ExperienceLetter;
                            ecdvm.RelievingLetter = Careerdetails[i].RelievingLetter;
                            ecdvm.ContactName = Careerdetails[i].ContactName;
                            ecdvm.ContactDesignation = Careerdetails[i].ContactDesignation;
                            ecdvm.ContactEmail = Careerdetails[i].ContactEmail;
                            ecdvm.ContactMobile = Careerdetails[i].ContactMobile;
                            ecdvm.CTC = Careerdetails[i].CTC;
                            ecdvm.Reason = Careerdetails[i].Reason;

                            if (ecdvm.PaySlip1 != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.PaySlip1.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.PaySlip1 = "Uploads" + lnkval;
                            }
                            if (ecdvm.PaySlip2 != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.PaySlip2.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.PaySlip2 = "Uploads" + lnkval;
                            }
                            if (ecdvm.PaySlip3 != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.PaySlip3.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.PaySlip3 = "Uploads" + lnkval;
                            }
                            if (ecdvm.OfferLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.OfferLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.OfferLetter = "Uploads" + lnkval;
                            }
                            if (ecdvm.SalaryLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.SalaryLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.SalaryLetter = "Uploads" + lnkval;
                            }
                            if (ecdvm.ExperienceLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.ExperienceLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.ExperienceLetter = "Uploads" + lnkval;
                            }
                            if (ecdvm.RelievingLetter != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = ecdvm.RelievingLetter.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                ecdvm.RelievingLetter = "Uploads" + lnkval;
                            }
                            ecdvm.CreatedBy = Careerdetails[i].CreatedBy;
                            ecdvm.CreatedDate = Careerdetails[i].CreatedDate;
                            ecdvm.LastUpdatedBy = Careerdetails[i].LastUpdatedBy;
                            ecdvm.LastUpdatedDate = Careerdetails[i].LastUpdatedDate;
                            ecdvm.IsActive = Careerdetails[i].IsActive;
                            ecdvm.IsUpdated = Careerdetails[i].IsUpdated;
                            ecdvm.IsDeleted = Careerdetails[i].IsDeleted;
                            lstofcareerdetails.Add(ecdvm);

                        }
                        return lstofcareerdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Career Details Not Found");
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
        public EmployeeCareerDetailViewModel AddEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var careerdetails = (from car in DB.EmployeeCareerDetails
                                     where car.EmpId == EmpId && car.CareerId == model.CareerId
                                     && car.IsActive == true && car.IsDeleted == false
                                     select car).ToList();

                if (loginId != 0)
                {
                    if (careerdetails.Count() == 0)
                    {
                        EmployeeCareerDetail ecd = new EmployeeCareerDetail();
                        //em.EmpId = model.modelId;
                        ecd.EmpId = model.EmpId;
                        ecd.Company = model.Company;
                        ecd.Designation = model.Designation;
                        ecd.FromDate = model.FromDate;
                        ecd.ToDate = model.ToDate;
                        ecd.Experience = model.Experience;
                        ecd.PMonth1 = model.PMonth1;
                        ecd.PaySlip1 = (model.PaySlip1 != null) ? model.PaySlip1 : "";
                        ecd.PMonth2 = model.PMonth2;
                        ecd.PaySlip2 = (model.PaySlip2 != null) ? model.PaySlip2 : "";
                        ecd.PMonth3 = model.PMonth3;
                        ecd.PaySlip3 = (model.PaySlip3 != null) ? model.PaySlip3 : "";
                        ecd.OfferLetter = (model.OfferLetter != null) ? model.OfferLetter : "";
                        ecd.SalaryLetter = (model.SalaryLetter != null) ? model.SalaryLetter : "";
                        ecd.ExperienceLetter = (model.ExperienceLetter != null) ? model.ExperienceLetter : "";
                        ecd.RelievingLetter = (model.RelievingLetter != null) ? model.RelievingLetter : "";
                        ecd.ContactName = model.ContactName;
                        ecd.ContactDesignation = model.ContactDesignation;
                        ecd.ContactMobile = model.ContactMobile;
                        ecd.ContactEmail = model.ContactEmail;
                        ecd.CTC = model.CTC;
                        ecd.Reason = model.Reason;
                        ecd.IsActive = true;
                        ecd.IsUpdated = false;
                        ecd.IsDeleted = false;
                        ecd.CreatedBy = model.LoginId;
                        ecd.CreatedDate = DateTime.Now;
                        ecd.LastUpdatedBy = model.LoginId;
                        ecd.LastUpdatedDate = DateTime.Now;
                        DB.EmployeeCareerDetails.Add(ecd);
                        DB.SaveChanges();

                        EmployeeCareerDetailViewModel emvm = new EmployeeCareerDetailViewModel();
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Career Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeCareerDetailViewModel UpdateEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.CareerId != 0) ? model.CareerId : 0;

                var careerdetails = (from car in DB.EmployeeCareerDetails
                                     where car.EmpId == EmpId && car.CareerId == id && car.IsActive == true && car.IsDeleted == false
                                     select car).FirstOrDefault();

                if (loginId != 0)
                {
                    if (careerdetails != null)
                    {
                        careerdetails.EmpId = model.EmpId;
                        careerdetails.Company = model.Company;
                        careerdetails.Designation = model.Designation;
                        careerdetails.FromDate = model.FromDate;
                        careerdetails.ToDate = model.ToDate;
                        careerdetails.Experience = model.Experience;
                        careerdetails.PMonth1 = model.PMonth1;
                        careerdetails.PaySlip1 = (model.PaySlip1 != null) ? model.PaySlip1 : "";
                        careerdetails.PMonth2 = model.PMonth2;
                        careerdetails.PaySlip2 = (model.PaySlip2 != null) ? model.PaySlip2 : "";
                        careerdetails.PMonth3 = model.PMonth3;
                        careerdetails.PaySlip3 = (model.PaySlip3 != null) ? model.PaySlip3 : "";
                        careerdetails.OfferLetter = (model.OfferLetter != null) ? model.OfferLetter : "";
                        careerdetails.SalaryLetter = (model.SalaryLetter != null) ? model.SalaryLetter : "";
                        careerdetails.ExperienceLetter = (model.ExperienceLetter != null) ? model.ExperienceLetter : "";
                        careerdetails.RelievingLetter = (model.RelievingLetter != null) ? model.RelievingLetter : "";
                        careerdetails.ContactName = model.ContactName;
                        careerdetails.ContactDesignation = model.ContactDesignation;
                        careerdetails.ContactMobile = model.ContactMobile;
                        careerdetails.ContactEmail = model.ContactEmail;
                        careerdetails.CTC = model.CTC;
                        careerdetails.Reason = model.Reason;
                        careerdetails.IsActive = true;
                        careerdetails.IsUpdated = true;
                        careerdetails.IsDeleted = false;
                        careerdetails.LastUpdatedBy = model.LoginId;
                        careerdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeCareerDetailViewModel emvm = new EmployeeCareerDetailViewModel();
                        emvm.msg = "Updated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Career Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public EmployeeCareerDetailViewModel DeleteEmpCareerDetails(EmployeeCareerDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.CareerId != 0) ? model.CareerId : 0;

                var careerdetails = (from car in DB.EmployeeCareerDetails
                                     where car.EmpId == EmpId && car.CareerId == id && car.IsActive == true && car.IsDeleted == false
                                     select car).FirstOrDefault();

                if (loginId != 0)
                {
                    if (careerdetails != null)
                    {
                        careerdetails.IsActive = true;
                        careerdetails.IsUpdated = true;
                        careerdetails.IsDeleted = true;
                        careerdetails.LastUpdatedBy = model.LoginId;
                        careerdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        EmployeeCareerDetailViewModel emvm = new EmployeeCareerDetailViewModel();
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Career Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);//WorkTypeMasterViewModel GetAllWorkType
            }
        }
        //public List<WorkTypeMasterViewModel> GetAllWorkType(WorkTypeMasterViewModel model)
        //{
        //    try
        //    {
        //        string msg = "";
        //        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

        //        var workdetails = (from work in DB.WorkTypeMasters
        //                          where work.EmpId == loginId && work.IsActive == true && work.IsDeleted == false
        //                          select work).OrderByDescending(x => x.CreatedDate).ToList();

        //        if (loginId != 0)
        //        {
        //            if (workdetails != null)
        //            {
        //                List<WorkTypeMasterViewModel> lstofWork = new List<WorkTypeMasterViewModel>();

        //                for (int i = 0; i < workdetails.Count(); i++)
        //                {
        //                    WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
        //                    wtvm.WorkTypeId = workdetails[i].WorkTypeId;
        //                    wtvm.WorkType = workdetails[i].WorkType;
        //                    wtvm.EmpId = workdetails[i].EmpId;
        //                    wtvm.EmpCode = workdetails[i].EmpCode;
        //                    wtvm.EmpName = (wtvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) + " " +
        //                    (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
        //                    (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.LastName).FirstOrDefault()) : "";
        //                    wtvm.StartDate = workdetails[i].StartDate;
        //                    wtvm.EndDate = workdetails[i].EndDate;
        //                    wtvm.Reason = workdetails[i].Reason;
        //                    wtvm.ApproverDescription = workdetails[i].ApproverDescription;
        //                    wtvm.IsApproved = workdetails[i].IsApproved;
        //                    wtvm.IsApprovedBy = workdetails[i].IsApprovedBy;
        //                    wtvm.Approver = (wtvm.IsApprovedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
        //                    wtvm.IsRejected = workdetails[i].IsRejected;
        //                    wtvm.IsRejectedBy = workdetails[i].IsRejectedBy;
        //                    wtvm.RApprover = (wtvm.IsRejectedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
        //                    wtvm.IsEnd = workdetails[i].IsEnd;
        //                    wtvm.IsActive = workdetails[i].IsActive;
        //                    wtvm.IsUpdated = workdetails[i].IsUpdated;
        //                    wtvm.IsDeleted = workdetails[i].IsDeleted;
        //                    wtvm.CreatedBy = workdetails[i].CreatedBy;
        //                    wtvm.CreatedDate = workdetails[i].CreatedDate;
        //                    wtvm.LastUpdatedBy = workdetails[i].LastUpdatedBy;
        //                    wtvm.LastupdatedDate = workdetails[i].LastupdatedDate;
        //                    lstofWork.Add(wtvm);
        //                }

        //                return lstofWork;
        //            }
        //            else
        //            {
        //                throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
        //            }
        //        }
        //        else
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        //        }
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}

        public List<WorkTypeMasterViewModel> GetAllWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.EmpId == loginId && work.IsActive == true && work.IsDeleted == false
                                   orderby work.CreatedDate descending
                                   select work).ToList();

                if (loginId == 0)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }

                if (workdetails == null || workdetails.Count == 0)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
                }

                List<WorkTypeMasterViewModel> lstofWork = new List<WorkTypeMasterViewModel>();

                foreach (var work in workdetails)
                {
                    WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
                    wtvm.WorkTypeId = work.WorkTypeId;
                    wtvm.WorkType = work.WorkType;
                    wtvm.EmpId = work.EmpId;
                    wtvm.EmpCode = work.EmpCode;

                    // Fetch Employee Name
                    var emp = DB.EmployeeMasters.FirstOrDefault(x => x.EmpId == work.EmpId);
                    wtvm.EmpName = emp != null ? (emp.FirstName + " " + emp.MiddleName + " " + emp.LastName).Trim() : "N/A";

                    wtvm.StartDate = work.StartDate;
                    wtvm.EndDate = work.EndDate;
                    wtvm.Reason = work.Reason;
                    wtvm.ApproverDescription = work.ApproverDescription;
                    wtvm.IsApproved = work.IsApproved;
                    wtvm.IsApprovedBy = work.IsApprovedBy;
                    wtvm.IsRejected = work.IsRejected;
                    wtvm.IsRejectedBy = work.IsRejectedBy;
                    wtvm.IsEnd = work.IsEnd;
                    wtvm.IsActive = work.IsActive;
                    wtvm.IsUpdated = work.IsUpdated;
                    wtvm.IsDeleted = work.IsDeleted;
                    wtvm.CreatedBy = work.CreatedBy;
                    wtvm.CreatedDate = work.CreatedDate;
                    wtvm.LastUpdatedBy = work.LastUpdatedBy;
                    wtvm.LastupdatedDate = work.LastupdatedDate;

                    // Set Status Field
                    wtvm.Status = (bool)work.IsApproved ? "Approved" : (bool)work.IsRejected ? "Rejected" : "Applied";

                    // Fetch Approver Name (Based on IsApprovedBy field)
                    var approver = DB.EmployeeMasters.FirstOrDefault(x => x.EmpId == work.IsApprovedBy);
                    wtvm.Approver = approver != null ? (approver.FirstName + " " + approver.MiddleName + " " + approver.LastName).Trim() : "No Approver";



                    lstofWork.Add(wtvm);
                }

                return lstofWork;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public WorkTypeMasterViewModel GetWorkType(WorkTypeMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;
                int? wid = (model.WorkTypeId != 0) ? model.WorkTypeId : 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.EmpId == id && work.WorkTypeId == wid && work.IsActive == true && work.IsDeleted == false
                                   select work).FirstOrDefault();

                if (loginId != 0)
                {
                    if (workdetails != null)
                    {
                        WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
                        wtvm.WorkTypeId = workdetails.WorkTypeId;
                        wtvm.WorkType = workdetails.WorkType;
                        wtvm.EmpId = workdetails.EmpId;
                        wtvm.EmpCode = workdetails.EmpCode;
                        wtvm.EmpName = (wtvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                        (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                        (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.LastName).FirstOrDefault()) : "";
                        wtvm.StartDate = workdetails.StartDate;
                        wtvm.EndDate = workdetails.EndDate;
                        wtvm.Reason = workdetails.Reason;
                        wtvm.ApproverDescription = workdetails.ApproverDescription;
                        wtvm.IsApproved = workdetails.IsApproved;
                        wtvm.IsApprovedBy = workdetails.IsApprovedBy;
                        wtvm.Approver = (wtvm.IsApprovedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
                        wtvm.IsRejected = workdetails.IsRejected;
                        wtvm.IsRejectedBy = workdetails.IsRejectedBy;
                        wtvm.RApprover = (wtvm.IsRejectedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
                        wtvm.IsEnd = workdetails.IsEnd;
                        wtvm.IsActive = workdetails.IsActive;
                        wtvm.IsUpdated = workdetails.IsUpdated;
                        wtvm.IsDeleted = workdetails.IsDeleted;
                        wtvm.CreatedBy = workdetails.CreatedBy;
                        wtvm.CreatedDate = workdetails.CreatedDate;
                        wtvm.LastUpdatedBy = workdetails.LastUpdatedBy;
                        wtvm.LastupdatedDate = workdetails.LastupdatedDate;

                        return wtvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
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
        public WorkTypeMasterViewModel AddWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? wid = 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.EmpId == loginId && work.IsApproved == true && work.IsEnd == false && work.IsActive == true && work.IsDeleted == false
                                   select work).ToList();

                if (loginId != 0)
                {
                    if (workdetails.Count() == 0)
                    {
                        WorkTypeMaster wt = new WorkTypeMaster();
                        wt.WorkType = model.WorkType;
                        wt.EmpId = model.EmpId;
                        wt.EmpCode = model.EmpCode;
                        wt.StartDate = model.StartDate;
                        wt.EndDate = model.EndDate;
                        wt.Reason = model.Reason;
                        wt.ApproverDescription = "";
                        wt.IsApproved = false;
                        wt.IsApprovedBy = 0;
                        wt.IsRejected = false;
                        wt.IsRejectedBy = 0;
                        wt.IsEnd = model.IsEnd;
                        wt.IsActive = true;
                        wt.IsUpdated = false;
                        wt.IsDeleted = false;
                        wt.CreatedBy = loginId;
                        wt.CreatedDate = DateTime.Now;
                        wt.LastUpdatedBy = loginId;
                        wt.LastupdatedDate = DateTime.Now;
                        DB.WorkTypeMasters.Add(wt);
                        DB.SaveChanges();
                        wid = wt.WorkTypeId;

                        WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
                        wtmvm.EmpId = loginId;
                        wtmvm.msg = "Added";

                        return wtmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public WorkTypeMasterViewModel UpdateWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;
                int? wid = (model.WorkTypeId != 0) ? model.WorkTypeId : 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.WorkTypeId == wid && work.IsActive == true && work.IsDeleted == false
                                   select work).FirstOrDefault();

                if (loginId != 0)
                {
                    if (workdetails != null)
                    {
                        workdetails.WorkType = model.WorkType;
                        workdetails.EmpId = model.EmpId;
                        workdetails.EmpCode = model.EmpCode;
                        workdetails.StartDate = model.StartDate;
                        workdetails.EndDate = model.EndDate;
                        workdetails.Reason = model.Reason;
                        workdetails.ApproverDescription = "";
                        workdetails.IsApproved = false;
                        workdetails.IsApprovedBy = 0;
                        workdetails.IsRejected = false;
                        workdetails.IsRejectedBy = 0;
                        workdetails.IsEnd = model.IsEnd;
                        workdetails.IsUpdated = true;
                        workdetails.LastUpdatedBy = loginId;
                        workdetails.LastupdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
                        wtmvm.msg = "Updated";

                        return wtmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public WorkTypeMasterViewModel DeleteWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;
                int? wid = (model.WorkTypeId != 0) ? model.WorkTypeId : 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.WorkTypeId == wid && work.IsActive == true && work.IsDeleted == false
                                   select work).FirstOrDefault();

                if (loginId != 0)
                {
                    if (workdetails != null)
                    {
                        workdetails.Reason = model.Reason;
                        workdetails.IsActive = true;
                        workdetails.IsUpdated = true;
                        workdetails.IsDeleted = true;
                        workdetails.LastUpdatedBy = model.LoginId;
                        workdetails.LastupdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
                        wtmvm.msg = "Deleted";

                        return wtmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
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
        public List<WorkTypeMasterViewModel> GetAllApproverWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                int? oldempid = (from emp in DB.EmployeeMasters
                                 where emp.IsActive == true && emp.IsDeleted == false && emp.EmpId == loginId
                                 select emp.OldEmp_ID).FirstOrDefault();

                var empdetails = (from emp in DB.EmployeeMasters
                                  where (emp.ReportId == oldempid || emp.ReportId == loginId) && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).OrderByDescending(x => x.EmpId).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<WorkTypeMasterViewModel> lstofWork = new List<WorkTypeMasterViewModel>();

                        for (int j = 0; j < empdetails.Count(); j++)
                        {
                            int EmpId = empdetails[j].EmpId;

                            var workdetails = (from work in DB.WorkTypeMasters
                                               where /*work.EmpId == EmpId && */work.IsActive == true && work.IsDeleted == false
                                               select work).OrderByDescending(x => x.CreatedDate).ToList();

                            if (workdetails != null)
                            {
                                for (int i = 0; i < workdetails.Count(); i++)
                                {
                                    WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
                                    wtvm.WorkTypeId = workdetails[i].WorkTypeId;
                                    wtvm.WorkType = workdetails[i].WorkType;
                                    wtvm.EmpId = workdetails[i].EmpId;
                                    wtvm.EmpCode = workdetails[i].EmpCode;
                                    wtvm.EmpName = (wtvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.LastName).FirstOrDefault()) : "";
                                    wtvm.StartDate = workdetails[i].StartDate;
                                    wtvm.EndDate = workdetails[i].EndDate;
                                    wtvm.Reason = workdetails[i].Reason;
                                    wtvm.ApproverDescription = workdetails[i].ApproverDescription;
                                    wtvm.IsApproved = workdetails[i].IsApproved;
                                    wtvm.IsApprovedBy = workdetails[i].IsApprovedBy;
                                    wtvm.Approver = (wtvm.IsApprovedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
                                    wtvm.IsRejected = workdetails[i].IsRejected;
                                    wtvm.IsRejectedBy = workdetails[i].IsRejectedBy;
                                    wtvm.RApprover = (wtvm.IsRejectedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
                                    wtvm.IsEnd = workdetails[i].IsEnd;
                                    wtvm.IsActive = workdetails[i].IsActive;
                                    wtvm.IsUpdated = workdetails[i].IsUpdated;
                                    wtvm.IsDeleted = workdetails[i].IsDeleted;
                                    wtvm.CreatedBy = workdetails[i].CreatedBy;
                                    wtvm.CreatedDate = workdetails[i].CreatedDate;
                                    wtvm.LastUpdatedBy = workdetails[i].LastUpdatedBy;
                                    wtvm.LastupdatedDate = workdetails[i].LastupdatedDate;
                                    lstofWork.Add(wtvm);
                                }
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
                            }
                        }
                        //return lstofWork.OrderByDescending(x => x.CreatedDate).ToList();
                        return lstofWork;
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


        public List<DDEmployeeViewModel> DDEmployeeApprover(DDEmployeeViewModel empdd)
        {
            try
            {
                string msg = "";
                int? loginId = (empdd.LoginId != 0) ? empdd.LoginId : 0;

                int? oldempid = (from emp in DB.EmployeeMasters
                                 where emp.IsActive == true && emp.IsDeleted == false && emp.EmpId == loginId
                                 select emp.OldEmp_ID).FirstOrDefault();

                var empdetails = (from emp in DB.EmployeeMasters
                                  where (emp.ReportId == loginId || emp.ReportId == oldempid) && emp.IsActive == true && emp.IsDeleted == false
                                  select emp).OrderByDescending(x => x.EmpId).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<DDEmployeeViewModel> lstofDDEmp = new List<DDEmployeeViewModel>();

                        for (int j = 0; j < empdetails.Count(); j++)
                        {
                            DDEmployeeViewModel devm = new DDEmployeeViewModel();
                            devm.EmpId = empdetails[j].EmpId;
                            devm.EmpName = empdetails[j].FirstName + " " + empdetails[j].MiddleName + " " + empdetails[j].LastName;
                            devm.EmpCode = empdetails[j].UserName;
                            lstofDDEmp.Add(devm);
                        }
                        return lstofDDEmp.ToList();
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
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
        public List<WorkTypeMasterViewModel> GetAllWorkTypeFilter(WorkTypeFilterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                bool approved = false, rejected = false, active = false, end = false;

                if (model.FromDate == null && model.ToDate == null)
                {
                    model.FromDate = "";
                    model.ToDate = "";
                }

                if (model.Status != null)
                {
                    if (model.Status.ToUpper() == "APPROVED")
                    {
                        active = true;
                        approved = true;
                    }
                    else if (model.Status.ToUpper() == "REJECTED")
                    {
                        active = true;
                        rejected = true;
                    }
                    else if (model.Status.ToUpper() == "COMPLETED")
                    {
                        active = true;
                        approved = true;
                        end = true;
                    }
                    else if (model.Status.ToUpper() == "APPLIED")
                    {
                        active = true;
                        approved = false;
                        end = false;
                    }
                    else
                    {
                        model.Status = "";
                    }
                }
                else
                {
                    model.Status = "";
                }

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.IsActive == true && work.IsDeleted == false
                                   select work).OrderByDescending(x => x.CreatedDate).ToList();

                if (EmpId != 0)
                {
                    var list = workdetails.Where(x => x.EmpId == EmpId).ToList();

                    if (model.FromDate != "" && model.ToDate != "")
                    {
                        DateTime? fdate = Convert.ToDateTime(model.FromDate);
                        DateTime? tdate = Convert.ToDateTime(model.ToDate);

                        if (model.Status != "")
                        {
                            list = workdetails.Where(x => x.EmpId == EmpId && x.StartDate >= fdate && x.EndDate <= tdate && x.IsApproved == approved
                            && x.IsRejected == rejected && x.IsEnd == end).ToList();
                            workdetails = list;
                        }
                        else
                        {
                            list = workdetails.Where(x => x.EmpId == EmpId && x.StartDate >= fdate && x.EndDate <= tdate).ToList();
                            workdetails = list;
                        }
                    }
                    else
                    {
                        if (model.Status != "")
                        {
                            list = workdetails.Where(x => x.EmpId == EmpId && x.IsApproved == approved
                            && x.IsRejected == rejected && x.IsEnd == end).ToList();
                            workdetails = list;
                        }
                        else
                        {
                            list = workdetails.Where(x => x.EmpId == EmpId).ToList();
                            workdetails = list;
                        }
                    }
                }
                else
                {
                    var list = workdetails.ToList();

                    if (model.FromDate != "" && model.ToDate != "")
                    {
                        DateTime? fdate = Convert.ToDateTime(model.FromDate);
                        DateTime? tdate = Convert.ToDateTime(model.ToDate);
                        if (model.Status != "")
                        {
                            list = workdetails.Where(x => x.StartDate >= fdate && x.EndDate <= tdate && x.IsApproved == approved
                            && x.IsRejected == rejected && x.IsEnd == end).ToList();
                            workdetails = list;
                        }
                        else
                        {
                            list = workdetails.Where(x => x.StartDate >= fdate && x.EndDate <= tdate).ToList();
                            workdetails = list;
                        }
                    }
                    else
                    {
                        if (model.Status != "")
                        {
                            list = workdetails.Where(x => x.IsApproved == approved
                            && x.IsRejected == rejected && x.IsEnd == end).ToList();
                            workdetails = list;
                        }
                        else
                        {
                            list = workdetails.ToList();
                            workdetails = list;
                        }
                    }
                }

                if (workdetails != null)
                {
                    List<WorkTypeMasterViewModel> lstofWork = new List<WorkTypeMasterViewModel>();

                    for (int i = 0; i < workdetails.Count(); i++)
                    {
                        WorkTypeMasterViewModel wtvm = new WorkTypeMasterViewModel();
                        wtvm.WorkTypeId = workdetails[i].WorkTypeId;
                        wtvm.WorkType = workdetails[i].WorkType;
                        wtvm.EmpId = workdetails[i].EmpId;
                        wtvm.EmpCode = workdetails[i].EmpCode;
                        wtvm.EmpName = (wtvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                        (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                        (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.LastName).FirstOrDefault()) : "";
                        wtvm.StartDate = workdetails[i].StartDate;
                        wtvm.EndDate = workdetails[i].EndDate;
                        wtvm.Reason = workdetails[i].Reason;
                        wtvm.ApproverDescription = workdetails[i].ApproverDescription;
                        wtvm.IsApproved = workdetails[i].IsApproved;
                        wtvm.IsApprovedBy = workdetails[i].IsApprovedBy;
                        wtvm.Approver = (wtvm.IsApprovedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
                        wtvm.IsRejected = workdetails[i].IsRejected;
                        wtvm.IsRejectedBy = workdetails[i].IsRejectedBy;
                        wtvm.RApprover = (wtvm.IsRejectedBy != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wtvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) : "";
                        wtvm.IsEnd = workdetails[i].IsEnd;
                        wtvm.IsActive = workdetails[i].IsActive;
                        wtvm.IsUpdated = workdetails[i].IsUpdated;
                        wtvm.IsDeleted = workdetails[i].IsDeleted;
                        wtvm.CreatedBy = workdetails[i].CreatedBy;
                        wtvm.CreatedDate = workdetails[i].CreatedDate;
                        wtvm.LastUpdatedBy = workdetails[i].LastUpdatedBy;
                        wtvm.LastupdatedDate = workdetails[i].LastupdatedDate;
                        lstofWork.Add(wtvm);
                    }

                    return lstofWork;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public WorkTypeMasterViewModel ApproveWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;
                int? wid = (model.WorkTypeId != 0) ? model.WorkTypeId : 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.WorkTypeId == wid && work.IsApproved == false && work.IsRejected == false
                                   && work.IsActive == true && work.IsDeleted == false
                                   select work).FirstOrDefault();

                if (loginId != 0)
                {
                    if (workdetails != null)
                    {
                        workdetails.ApproverDescription = model.ApproverDescription;
                        workdetails.IsApproved = true;
                        workdetails.IsApprovedBy = loginId;
                        workdetails.IsRejected = false;
                        workdetails.IsRejectedBy = 0;
                        workdetails.IsEnd = model.IsEnd;
                        workdetails.IsUpdated = true;
                        workdetails.LastUpdatedBy = loginId;
                        workdetails.LastupdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
                        wtmvm.msg = "Approved";

                        return wtmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public WorkTypeMasterViewModel RejectWorkType(WorkTypeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? id = (model.EmpId != 0) ? model.EmpId : 0;
                int? wid = (model.WorkTypeId != 0) ? model.WorkTypeId : 0;

                var workdetails = (from work in DB.WorkTypeMasters
                                   where work.WorkTypeId == wid && work.IsApproved == false && work.IsRejected == false
                                   && work.IsActive == true && work.IsDeleted == false
                                   select work).FirstOrDefault();

                if (loginId != 0)
                {
                    if (workdetails != null)
                    {
                        workdetails.ApproverDescription = model.ApproverDescription;
                        workdetails.IsApproved = false;
                        workdetails.IsApprovedBy = 0;
                        workdetails.IsRejected = true;
                        workdetails.IsRejectedBy = loginId;
                        workdetails.IsEnd = model.IsEnd;
                        workdetails.IsUpdated = true;
                        workdetails.LastUpdatedBy = loginId;
                        workdetails.LastupdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        WorkTypeMasterViewModel wtmvm = new WorkTypeMasterViewModel();
                        wtmvm.msg = "Rejected";

                        return wtmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Work Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        //public List<WFHLoginlogViewModel> GetAllWFHDetails(WFHLoginlogViewModel model)
        //{
        //    try
        //    {
        //        string msg = "";
        //        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

        //        var wfhdetails = (from wfh in DB.WFHLoginlogs
        //                          where wfh.IsActive == true && wfh.IsDeleted == false
        //                          select wfh).OrderByDescending(x => x.EmpId).ToList();

        //        if (loginId != 0)
        //        {
        //            if (wfhdetails != null)
        //            {
        //                List<WFHLoginlogViewModel> lstofEmpWFH = new List<WFHLoginlogViewModel>();

        //                for (int i = 0; i < wfhdetails.Count(); i++)
        //                {
        //                    WFHLoginlogViewModel wfhvm = new WFHLoginlogViewModel();
        //                    wfhvm.EmpId = wfhdetails[i].EmpId;
        //                    wfhvm.EmpCode = wfhdetails[i].EmpCode;
        //                    wfhvm.EmpName = (wfhvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.FirstName).FirstOrDefault()) + " " +
        //                    (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
        //                    (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.LastName).FirstOrDefault()) : "";
        //                    wfhvm.Activehrs = wfhdetails[i].Activehrs;
        //                    wfhvm.CreatedBy = model.EmpId;
        //                    wfhvm.CreatedDate = wfhdetails[i].CreatedDate;
        //                    wfhvm.Date = wfhdetails[i].Date;
        //                    wfhvm.IPAddress = wfhdetails[i].IPAddress;
        //                    wfhvm.IsActive = wfhdetails[i].IsActive;
        //                    wfhvm.IsDeleted = wfhdetails[i].IsDeleted;
        //                    wfhvm.IsLoggedIn = wfhdetails[i].IsLoggedIn;
        //                    wfhvm.IsLoggedOut = wfhdetails[i].IsLoggedOut;
        //                    wfhvm.IsUpdated = wfhdetails[i].IsUpdated;
        //                    wfhvm.LastUpdatedBy = wfhdetails[i].LastUpdatedBy;
        //                    wfhvm.LastUpdatedDate = wfhdetails[i].LastUpdatedDate;
        //                    wfhvm.LoginTime = wfhdetails[i].LoginTime;
        //                    wfhvm.LogOutTime = wfhdetails[i].LogOutTime;
        //                    wfhvm.CompId = (wfhvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.CompId).FirstOrDefault()) : 0;
        //                    wfhvm.CompName = (wfhvm.CompId != 0) ? (DB.CompanyMasters.Where(x => x.CompId == wfhvm.CompId).Select(x => x.Company).FirstOrDefault()) : "";
        //                    wfhvm.DeptId = (wfhvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.CategoryId).FirstOrDefault()) : 0;
        //                    wfhvm.DeptName = (wfhvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.DeptName).FirstOrDefault()) : "";
        //                    wfhvm.DesignationId = (wfhvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.DesignationId).FirstOrDefault()) : 0;
        //                    wfhvm.Designation = (wfhvm.EmpId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == wfhvm.EmpId).Select(x => x.DesignationName).FirstOrDefault()) : "";

        //                    lstofEmpWFH.Add(wfhvm);
        //                }

        //                return lstofEmpWFH;
        //            }
        //            else
        //            {
        //                throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
        //            }
        //        }
        //        else
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        //        }
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}

        ////public List<WFHLoginlogViewModel> GetAllWFHDetails(WFHLoginlogViewModel model)
        ////{
        ////    try
        ////    {
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

        ////        ////var wfhDetails = (from wfh in DB.WFHLoginlogs
        ////        ////                  join em in DB.EmployeeMasters on wfh.EmpId equals em.EmpId
        ////        ////                  join comp in DB.CompanyMasters on em.CompId equals comp.CompId
        ////        ////                  //join dept in DB.DeptMasters on em.CategoryId equals dept.DeptId
        ////        ////                  //join desig in DB.DesignationMasters on em.DesignationId equals desig.DesignationId
        ////        ////                  where wfh.IsActive == true && wfh.IsDeleted == false
        ////        ////                  select new
        ////        ////                  {
        ////        ////                      wfh,
        ////        ////                      FullName = em.FirstName + " " + em.MiddleName + " " + em.LastName,
        ////        ////                      comp.Company,
        ////        ////                      em.DeptName,
        ////        ////                      em.DesignationName,
        ////        ////                      em.CategoryId,
        ////        ////                      em.CompId,
        ////        ////                      em.DesignationId
        ////        ////                  })
        ////        ////                  .Distinct()
        ////        ////                  .OrderByDescending(x => x.wfh.EmpId)
        ////        ////                  .ToList();
        ////        ///

        ////        DateTime today = DateTime.Today;

        ////        DateTime startDate = new DateTime(today.Year, today.Month, 1);
        ////        DateTime endDate = today;


        ////        var wfhDetailsQuery = from wfh in DB.WFHLoginlogs
        ////                              join em in DB.EmployeeMasters on wfh.EmpId equals em.EmpId
        ////                              join comp in DB.CompanyMasters on em.CompId equals comp.CompId
        ////                              where wfh.IsActive == true && wfh.IsDeleted == false
        ////                              && wfh.Date >= startDate && wfh.Date <= endDate
        ////                              && em.IsActive == true && em.IsDeleted == false && em.EmpStatus.ToUpper() == "ACTIVE"
        ////                              && comp.IsActive == true && comp.IsDeleted == false
        ////                              select new
        ////                              {
        ////                                  WFH = wfh,
        ////                                  FullName =
        ////                                        (em.FirstName + " "
        ////                                        + (string.IsNullOrEmpty(em.MiddleName) ? "" : em.MiddleName + " ")
        ////                                        + em.LastName).Trim(),
        ////                                  Company = comp.Company,
        ////                                  DeptName = em.DeptName,
        ////                                  DesignationName = em.DesignationName,
        ////                                  CategoryId = em.CategoryId,
        ////                                  CompId = em.CompId,
        ////                                  DesignationId = em.DesignationId
        ////                              };

        ////        if (loginId != 0)
        ////        {
        ////            if (wfhDetailsQuery.Count() > 0)
        ////            {
        ////                // Remove duplicates by WFHId
        ////                var groupedWFH = wfhDetailsQuery
        ////                                    .GroupBy(x => x.WFH.WFHId)
        ////                                    .Select(g => g.FirstOrDefault())
        ////                                    .OrderByDescending(x => x.WFH.Date)
        ////                                    .ThenByDescending(x => x.WFH.EmpId)
        ////                                    .ToList();

        ////                List<WFHLoginlogViewModel> lstofEmpWFH = groupedWFH.Select(detail =>
        ////                {
        ////                    var login = detail.WFH.LoginTime;
        ////                    var logout = detail.WFH.LogOutTime;
        ////                    if (detail.WFH.Date != today)
        ////                    {
        ////                        logout = detail.WFH.LogOutTime ?? new TimeSpan(18, 33, 0); // 18:33
        ////                    }
        ////                    else
        ////                    {
        ////                        logout = detail.WFH.LogOutTime ?? new TimeSpan(00, 00, 0); //00:00
        ////                        login = new TimeSpan(00, 00, 0); //00:00
        ////                    }

        ////                    return new WFHLoginlogViewModel
        ////                    {
        ////                        WFHId = detail.WFH.WFHId,
        ////                        LoginId = loginId,
        ////                        EmpId = detail.WFH.EmpId,
        ////                        EmpName = detail.FullName,
        ////                        EmpCode = detail.WFH.EmpCode,
        ////                        IPAddress = detail.WFH.IPAddress,
        ////                        Date = detail.WFH.Date,
        ////                        LoginTime = detail.WFH.LoginTime,
        ////                        LogOutTime = logout,
        ////                        Activehrs = detail.WFH.Activehrs ?? (logout - login),
        ////                        IsLoggedIn = detail.WFH.IsLoggedIn,
        ////                        IsLoggedOut = detail.WFH.IsLoggedOut,
        ////                        CreatedBy = detail.WFH.CreatedBy,
        ////                        CreatedDate = detail.WFH.CreatedDate,
        ////                        LastUpdatedBy = detail.WFH.LastUpdatedBy,
        ////                        LastUpdatedDate = detail.WFH.LastUpdatedDate,
        ////                        IsActive = detail.WFH.IsActive,
        ////                        IsUpdated = detail.WFH.IsUpdated,
        ////                        IsDeleted = detail.WFH.IsDeleted,
        ////                        CompId = detail.CompId,
        ////                        CompName = detail.Company,
        ////                        DeptId = detail.CategoryId,
        ////                        DeptName = detail.DeptName,
        ////                        DesignationId = detail.DesignationId,
        ////                        Designation = detail.DesignationName
        ////                    };

        ////                }).ToList();

        ////                ////// Convert to ViewModel list
        ////                ////List<WFHLoginlogViewModel> lstofEmpWFH = groupedWFH.Select(detail => new WFHLoginlogViewModel
        ////                ////{
        ////                ////    WFHId = detail.WFH.WFHId,
        ////                ////    LoginId = loginId,
        ////                ////    EmpId = detail.WFH.EmpId,
        ////                ////    EmpName = detail.FullName,
        ////                ////    EmpCode = detail.WFH.EmpCode,
        ////                ////    IPAddress = detail.WFH.IPAddress,
        ////                ////    Date = detail.WFH.Date,
        ////                ////    LoginTime = detail.WFH.LoginTime,
        ////                ////    LogOutTime = detail.WFH.LogOutTime == null ? 18:33,
        ////                ////    Activehrs = detail.WFH.Activehrs == null ? (LoginTime - LogOutTime),
        ////                ////    IsLoggedIn = detail.WFH.IsLoggedIn,
        ////                ////    IsLoggedOut = detail.WFH.IsLoggedOut,
        ////                ////    CreatedBy = detail.WFH.CreatedBy,
        ////                ////    CreatedDate = detail.WFH.CreatedDate,
        ////                ////    LastUpdatedBy = detail.WFH.LastUpdatedBy,
        ////                ////    LastUpdatedDate = detail.WFH.LastUpdatedDate,
        ////                ////    IsActive = detail.WFH.IsActive,
        ////                ////    IsUpdated = detail.WFH.IsUpdated,
        ////                ////    IsDeleted = detail.WFH.IsDeleted,
        ////                ////    CompId = detail.CompId,
        ////                ////    CompName = detail.Company,
        ////                ////    DeptId = detail.CategoryId,
        ////                ////    DeptName = detail.DeptName,
        ////                ////    DesignationId = detail.DesignationId,
        ////                ////    Designation = detail.DesignationName
        ////                ////}).ToList();

        ////                return lstofEmpWFH;
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}  //18.12.2025

        public List<WFHLoginlogViewModel> GetAllWFHDetails(WFHLoginlogViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                DateTime today = DateTime.Today;

                DateTime startDate = new DateTime(today.Year, today.Month, 1);
                DateTime endDate = today;


                var wfhDetailsQuery = from wfh in DB.WFHLoginlogs
                                      join em in DB.EmployeeMasters on wfh.EmpId equals em.EmpId
                                      join comp in DB.CompanyMasters on em.CompId equals comp.CompId
                                      where wfh.IsActive == true && wfh.IsDeleted == false
                                      && wfh.Date >= startDate && wfh.Date <= endDate
                                      && em.IsActive == true && em.IsDeleted == false && em.EmpStatus.ToUpper() == "ACTIVE"
                                      && comp.IsActive == true && comp.IsDeleted == false
                                      select new
                                      {
                                          WFH = wfh,
                                          FullName =
                                                (em.FirstName + " "
                                                + (string.IsNullOrEmpty(em.MiddleName) ? "" : em.MiddleName + " ")
                                                + em.LastName).Trim(),
                                          Company = comp.Company,
                                          DeptName = em.DeptName,
                                          DesignationName = em.DesignationName,
                                          CategoryId = em.CategoryId,
                                          CompId = em.CompId,
                                          DesignationId = em.DesignationId
                                      };

                if (loginId != 0)
                {
                    if (wfhDetailsQuery.Count() > 0)
                    {
                        var groupedWFH = wfhDetailsQuery
                                            .AsEnumerable()
                                            .GroupBy(x => new { x.WFH.EmpId, x.WFH.Date })
                                            .OrderByDescending(g => g.Key.Date)
                                            .ThenByDescending(g => g.Key.EmpId)
                                            .ToList();

                        List<WFHLoginlogViewModel> lstofEmpWFH = new List<WFHLoginlogViewModel>();

                        TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

                        foreach (var group in groupedWFH)
                        {
                            var entries = group
                                .Select(x => x.WFH)
                                .Where(x => x.LoginTime.HasValue)
                                .OrderBy(x => x.LoginTime)
                                .ToList();

                            if (!entries.Any())
                                continue;

                            TimeSpan totalActiveHours = TimeSpan.Zero;
                            TimeSpan firstLogin = entries.First().LoginTime.Value;
                            TimeSpan lastLogout = TimeSpan.Zero;

                            for (int i = 0; i < entries.Count; i++)
                            {
                                var current = entries[i];
                                TimeSpan logIn = current.LoginTime.Value;
                                TimeSpan logOut;

                                if (current.LogOutTime.HasValue)
                                {
                                    logOut = current.LogOutTime.Value;
                                }
                                else if (i + 1 < entries.Count)
                                {
                                    logOut = entries[i + 1].LoginTime.Value;
                                }
                                else
                                {
                                    logOut = defaultLogout;
                                }

                                if (logOut > logIn)
                                    totalActiveHours += (logOut - logIn);

                                if (logOut > lastLogout)
                                    lastLogout = logOut;
                            }

                            var detail = group.First();

                            lstofEmpWFH.Add(new WFHLoginlogViewModel
                            {
                                WFHId = detail.WFH.WFHId,
                                LoginId = loginId,
                                EmpId = detail.WFH.EmpId,
                                EmpName = detail.FullName,
                                EmpCode = detail.WFH.EmpCode,
                                IPAddress = detail.WFH.IPAddress,
                                Date = detail.WFH.Date,

                                // ✅ Final consolidated values
                                LoginTime = firstLogin,
                                LogOutTime = lastLogout,
                                Activehrs = totalActiveHours,

                                IsLoggedIn = detail.WFH.IsLoggedIn,
                                IsLoggedOut = detail.WFH.IsLoggedOut,
                                CreatedBy = detail.WFH.CreatedBy,
                                CreatedDate = detail.WFH.CreatedDate,
                                LastUpdatedBy = detail.WFH.LastUpdatedBy,
                                LastUpdatedDate = detail.WFH.LastUpdatedDate,
                                IsActive = detail.WFH.IsActive,
                                IsUpdated = detail.WFH.IsUpdated,
                                IsDeleted = detail.WFH.IsDeleted,

                                CompId = detail.CompId,
                                CompName = detail.Company,
                                DeptId = detail.CategoryId,
                                DeptName = detail.DeptName,
                                DesignationId = detail.DesignationId,
                                Designation = detail.DesignationName
                            });
                        }
                        if (lstofEmpWFH != null)
                        {
                            foreach (var item in lstofEmpWFH)
                            {
                                if (item.Date == today)
                                {
                                    item.LogOutTime = new TimeSpan(0, 0, 0);
                                    item.Activehrs = new TimeSpan(0, 0, 0);
                                }
                            }
                        }

                        return lstofEmpWFH;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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

        public List<WFHLoginlogViewModel> GetAllWFHFilterDetails(WFHLoginlogFilterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? designationId = (model.DesignationId != 0) ? model.DesignationId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;
                DateTime today = DateTime.Today;

                if (loginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");

                TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

                var query = from wfh in DB.WFHLoginlogs
                            join em in DB.EmployeeMasters on wfh.EmpId equals em.EmpId
                            join comp in DB.CompanyMasters on em.CompId equals comp.CompId
                            where wfh.IsActive == true && wfh.IsDeleted == false
                                  && em.IsActive == true && em.IsDeleted == false && em.EmpStatus.ToUpper() == "ACTIVE"
                                  && comp.IsActive == true && comp.IsDeleted == false
                            select new
                            {
                                WFH = wfh,
                                EmpName = (em.FirstName + " "
                                    + (string.IsNullOrEmpty(em.MiddleName) ? "" : em.MiddleName + " ")
                                    + em.LastName).Trim(),
                                em.CategoryId,
                                em.DesignationId,
                                em.DeptName,
                                em.DesignationName,
                                em.CompId,
                                Company = comp.Company
                            };

                var groupedData = query
                    .AsEnumerable()
                    .GroupBy(x => new { x.WFH.EmpId, x.WFH.Date })
                    .OrderByDescending(g => g.Key.Date)
                    .ThenByDescending(g => g.Key.EmpId)
                    .ToList();

                List<WFHLoginlogViewModel> result = new List<WFHLoginlogViewModel>();

                foreach (var group in groupedData)
                {
                    var records = group
                        .Select(x => x.WFH)
                        .Where(x => x.LoginTime.HasValue)
                        .OrderBy(x => x.LoginTime)
                        .ToList();

                    if (!records.Any())
                        continue;

                    TimeSpan totalActiveHours = TimeSpan.Zero;
                    TimeSpan firstLogin = records.First().LoginTime.Value;
                    TimeSpan lastLogout = TimeSpan.Zero;

                    for (int i = 0; i < records.Count; i++)
                    {
                        TimeSpan logIn = records[i].LoginTime.Value;
                        TimeSpan logOut;

                        if (records[i].LogOutTime.HasValue)
                            logOut = records[i].LogOutTime.Value;
                        else if (i + 1 < records.Count)
                            logOut = records[i + 1].LoginTime.Value;
                        else
                            logOut = defaultLogout;

                        if (logOut > logIn)
                            totalActiveHours += (logOut - logIn);

                        if (logOut > lastLogout)
                            lastLogout = logOut;
                    }

                    var meta = group.First();

                    result.Add(new WFHLoginlogViewModel
                    {
                        LoginId = loginId,
                        EmpId = meta.WFH.EmpId,
                        EmpCode = meta.WFH.EmpCode,
                        EmpName = meta.EmpName,
                        IPAddress = meta.WFH.IPAddress,
                        Date = meta.WFH.Date,

                        LoginTime = firstLogin,
                        LogOutTime = lastLogout,
                        Activehrs = totalActiveHours,

                        IsLoggedIn = meta.WFH.IsLoggedIn,
                        IsLoggedOut = meta.WFH.IsLoggedOut,
                        IsActive = meta.WFH.IsActive,
                        IsDeleted = meta.WFH.IsDeleted,
                        CreatedBy = meta.WFH.CreatedBy,
                        CreatedDate = meta.WFH.CreatedDate,
                        LastUpdatedBy = meta.WFH.LastUpdatedBy,
                        LastUpdatedDate = meta.WFH.LastUpdatedDate,

                        CompId = meta.CompId,
                        CompName = meta.Company,
                        DeptId = meta.CategoryId,
                        DeptName = meta.DeptName,
                        DesignationId = meta.DesignationId,
                        Designation = meta.DesignationName
                    });
                }

                //Filters
                if (compId != 0)
                {
                    var result1 = result.Where(x => x.CompId == compId
                                    && x.IsActive == true).ToList();
                    result = result1;
                }
                if (deptId != 0)
                {
                    var result1 = result.Where(x => x.DeptId == deptId
                                    && x.IsActive == true).ToList();
                    result = result1;
                }
                if (designationId != 0)
                {
                    var result1 = result.Where(x => x.DesignationId == designationId
                                    && x.IsActive == true).ToList();
                    result = result1;
                }

                // 🔹 Filters
                if (empId != 0)
                {
                    var result1 = result.Where(x => x.EmpId == empId
                                    && x.IsActive == true).ToList();
                    result = result1;
                }
                if (model.FromDate == null && model.ToDate == null)
                {
                    model.FromDate = "";
                    model.ToDate = "";
                }
                if (model.FromDate != "" && model.ToDate != "")
                {
                    DateTime? fdate = Convert.ToDateTime(model.FromDate);
                    DateTime? tdate = Convert.ToDateTime(model.ToDate);

                    var result1 = result.Where(x => x.Date >= fdate && x.Date <= tdate
                                        && x.IsActive == true).ToList();
                    result = result1;
                }

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item.Date == today)
                        {
                            item.LogOutTime = new TimeSpan(0, 0, 0);
                            item.Activehrs = new TimeSpan(0, 0, 0);
                        }
                    }
                }

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }

        ////public List<WFHLoginlogViewModel> GetAllWFHFilterDetails(WFHLoginlogFilterViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? compId = (model.CompId != 0) ? model.CompId : 0;
        ////        int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
        ////        int? designationId = (model.DesignationId != 0) ? model.DesignationId : 0;
        ////        int? empId = (model.EmpId != 0)  ? model.EmpId : 0;
        ////        DateTime today = DateTime.Today;

        ////        ////var wfhdetails = (from wfh in DB.WFHLoginlogs
        ////        ////                  join emp in DB.EmployeeMasters on wfh.EmpId equals emp.EmpId
        ////        ////                  where wfh.IsActive == true && wfh.IsDeleted == false && emp.IsActive == true && emp.IsDeleted == false && emp.EmpStatus.ToUpper() == "ACTIVE"
        ////        ////                  select new WFHLoginlogViewModel
        ////        ////                  {

        ////        ////                      EmpId = wfh.EmpId,
        ////        ////                      EmpCode = wfh.EmpCode,
        ////        ////                      EmpName = (empId != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == empId).Select(x => x.FirstName).FirstOrDefault()) + " " +
        ////        ////                      (DB.EmployeeMasters.Where(x => x.EmpId == empId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
        ////        ////                      (DB.EmployeeMasters.Where(x => x.EmpId == empId).Select(x => x.LastName).FirstOrDefault()) : "",
        ////        ////                      Activehrs = wfh.Activehrs,
        ////        ////                      CreatedBy = empId,
        ////        ////                      CreatedDate = wfh.CreatedDate,
        ////        ////                      Date = wfh.Date,
        ////        ////                      IPAddress = wfh.IPAddress,
        ////        ////                      IsActive = wfh.IsActive,
        ////        ////                      IsDeleted = wfh.IsDeleted,
        ////        ////                      IsLoggedIn = wfh.IsLoggedIn,
        ////        ////                      IsLoggedOut = wfh.IsLoggedOut,
        ////        ////                      IsUpdated = wfh.IsUpdated,
        ////        ////                      LastUpdatedBy = wfh.LastUpdatedBy,
        ////        ////                      LastUpdatedDate = wfh.LastUpdatedDate,
        ////        ////                      LoginTime = wfh.LoginTime,
        ////        ////                      LogOutTime = wfh.LogOutTime,
        ////        ////                      CompId = emp.CompId,
        ////        ////                      CompName = (compId != 0) ? (DB.CompanyMasters.Where(x => x.CompId == compId).Select(x => x.Company).FirstOrDefault()) : "",
        ////        ////                      DeptId = emp.CategoryId,
        ////        ////                      DeptName = emp.DeptName,
        ////        ////                      DesignationId = emp.DesignationId,
        ////        ////                      Designation = emp.DesignationName,
        ////        ////                  }
        ////        ////                  ).OrderByDescending(x => x.Date)
        ////        ////                            .ThenByDescending(x => x.EmpId).ToList();
        ////        ///
        ////        var wfhDetailsQuery = from wfh in DB.WFHLoginlogs
        ////                              join em in DB.EmployeeMasters on wfh.EmpId equals em.EmpId
        ////                              join comp in DB.CompanyMasters on em.CompId equals comp.CompId
        ////                              where wfh.IsActive == true && wfh.IsDeleted == false
        ////                              && em.IsActive == true && em.IsDeleted == false && em.EmpStatus.ToUpper() == "ACTIVE"
        ////                              && comp.IsActive == true && comp.IsDeleted == false
        ////                              select new
        ////                              {
        ////                                  WFH = wfh,
        ////                                  FullName =
        ////                                        (em.FirstName + " "
        ////                                        + (string.IsNullOrEmpty(em.MiddleName) ? "" : em.MiddleName + " ")
        ////                                        + em.LastName).Trim(),
        ////                                  Company = comp.Company,
        ////                                  DeptName = em.DeptName,
        ////                                  DesignationName = em.DesignationName,
        ////                                  CategoryId = em.CategoryId,
        ////                                  CompId = em.CompId,
        ////                                  DesignationId = em.DesignationId
        ////                              };

        ////        if (loginId != 0)
        ////        {
        ////            if (wfhDetailsQuery.Count() > 0)
        ////            {
        ////                // Remove duplicates by WFHId
        ////                var groupedWFH = wfhDetailsQuery
        ////                                    .GroupBy(x => x.WFH.WFHId)
        ////                                    .Select(g => g.FirstOrDefault())
        ////                                    .OrderByDescending(x => x.WFH.Date)
        ////                                    .ThenByDescending(x => x.WFH.EmpId)
        ////                                    .ToList();

        ////                List<WFHLoginlogViewModel> lstofEmpWFH = groupedWFH.Select(detail =>
        ////                {
        ////                    var login = detail.WFH.LoginTime;
        ////                    var logout = detail.WFH.LogOutTime;
        ////                    if (detail.WFH.Date != today)
        ////                    {
        ////                        logout = detail.WFH.LogOutTime ?? new TimeSpan(18, 33, 0); // 18:33
        ////                    }
        ////                    else
        ////                    {
        ////                        logout = detail.WFH.LogOutTime ?? new TimeSpan(00, 00, 0); //00:00
        ////                        login = new TimeSpan(00, 00, 0); //00:00
        ////                    }

        ////                    return new WFHLoginlogViewModel
        ////                    {
        ////                        WFHId = detail.WFH.WFHId,
        ////                        LoginId = loginId,
        ////                        EmpId = detail.WFH.EmpId,
        ////                        EmpName = detail.FullName,
        ////                        EmpCode = detail.WFH.EmpCode,
        ////                        IPAddress = detail.WFH.IPAddress,
        ////                        Date = detail.WFH.Date,
        ////                        LoginTime = detail.WFH.LoginTime,
        ////                        LogOutTime = logout,
        ////                        Activehrs = detail.WFH.Activehrs ?? (logout - login),
        ////                        IsLoggedIn = detail.WFH.IsLoggedIn,
        ////                        IsLoggedOut = detail.WFH.IsLoggedOut,
        ////                        CreatedBy = detail.WFH.CreatedBy,
        ////                        CreatedDate = detail.WFH.CreatedDate,
        ////                        LastUpdatedBy = detail.WFH.LastUpdatedBy,
        ////                        LastUpdatedDate = detail.WFH.LastUpdatedDate,
        ////                        IsActive = detail.WFH.IsActive,
        ////                        IsUpdated = detail.WFH.IsUpdated,
        ////                        IsDeleted = detail.WFH.IsDeleted,
        ////                        CompId = detail.CompId,
        ////                        CompName = detail.Company,
        ////                        DeptId = detail.CategoryId,
        ////                        DeptName = detail.DeptName,
        ////                        DesignationId = detail.DesignationId,
        ////                        Designation = detail.DesignationName
        ////                    };

        ////                }).ToList();

        ////                var list = lstofEmpWFH.ToList();

        ////                if (compId != 0)
        ////                {
        ////                    var list1 = list.Where(x => x.CompId == compId
        ////                                        && x.IsActive == true).ToList();
        ////                    list = list1;
        ////                }
        ////                if (deptId != 0)
        ////                {
        ////                    var list1 = list.Where(x => x.DeptId == deptId
        ////                                    && x.IsActive == true).ToList();
        ////                    list = list1;

        ////                }
        ////                if (designationId != 0)
        ////                {
        ////                    var list1 = list.Where(x => x.DesignationId == designationId
        ////                                && x.IsActive == true).ToList();
        ////                    list = list1;

        ////                }
        ////                if (empId != 0)
        ////                {
        ////                    var list1 = list.Where(x => x.EmpId == empId
        ////                            && x.IsActive == true).ToList();
        ////                    list = list1;
        ////                }
        ////                if (model.FromDate == null && model.ToDate == null)
        ////                {
        ////                    model.FromDate = "";
        ////                    model.ToDate = "";
        ////                }
        ////                if (model.FromDate != "" && model.ToDate != "")
        ////                {
        ////                    DateTime? fdate = Convert.ToDateTime(model.FromDate);
        ////                    DateTime? tdate = Convert.ToDateTime(model.ToDate);

        ////                    var list1 = list.Where(x => x.Date >= fdate && x.Date <= tdate
        ////                                        && x.IsActive == true).ToList();
        ////                    list = list1;
        ////                }
        ////                if (loginId != 0)
        ////                {
        ////                    if (list != null)
        ////                    {
        ////                        List<WFHLoginlogViewModel> lstofEmpWFH1 = new List<WFHLoginlogViewModel>();

        ////                        for (int i = 0; i < list.Count(); i++)
        ////                        {
        ////                            WFHLoginlogViewModel wfhvm = new WFHLoginlogViewModel();
        ////                            wfhvm.EmpId = list[i].EmpId;
        ////                            int? emp = list[i].EmpId;
        ////                            wfhvm.EmpCode = list[i].EmpCode;
        ////                            wfhvm.CompId = list[i].CompId;
        ////                            wfhvm.CompName = list[i].CompName;
        ////                            wfhvm.DeptId = list[i].DeptId;
        ////                            wfhvm.DeptName = list[i].DeptName;
        ////                            wfhvm.DesignationId = list[i].DesignationId;
        ////                            wfhvm.Designation = list[i].Designation;
        ////                            wfhvm.EmpName = (emp != 0) ? (DB.EmployeeMasters.Where(x => x.EmpId == emp).Select(x => x.FirstName).FirstOrDefault()) + " " +
        ////                            (DB.EmployeeMasters.Where(x => x.EmpId == emp).Select(x => x.MiddleName).FirstOrDefault()) + " " +
        ////                            (DB.EmployeeMasters.Where(x => x.EmpId == emp).Select(x => x.LastName).FirstOrDefault()) : "";
        ////                            wfhvm.Activehrs = list[i].Activehrs;
        ////                            wfhvm.Date = list[i].Date;
        ////                            wfhvm.IPAddress = list[i].IPAddress;
        ////                            wfhvm.LoginTime = list[i].LoginTime;
        ////                            wfhvm.LogOutTime = list[i].LogOutTime;
        ////                            wfhvm.IsActive = list[i].IsActive;
        ////                            wfhvm.IsDeleted = list[i].IsDeleted;
        ////                            wfhvm.IsLoggedIn = list[i].IsLoggedIn;
        ////                            wfhvm.IsLoggedOut = list[i].IsLoggedOut;
        ////                            wfhvm.IsUpdated = list[i].IsUpdated;
        ////                            wfhvm.CreatedBy = list[i].CreatedBy;
        ////                            wfhvm.CreatedDate = list[i].CreatedDate;
        ////                            wfhvm.LastUpdatedBy = list[i].LastUpdatedBy;
        ////                            wfhvm.LastUpdatedDate = list[i].LastUpdatedDate;

        ////                            lstofEmpWFH1.Add(wfhvm);
        ////                        }

        ////                        return lstofEmpWFH1;
        ////                    }
        ////                    else
        ////                    {
        ////                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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
        ////}  //18.12.2025

        public WFHLoginlogViewModel SaveWFHAnalysis(WFHLoginlogViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var wfhDetails = (from wfh in DB.WFHLoginlogs
                                  where wfh.EmpCode.ToUpper() == model.EmpCode.ToUpper() && wfh.Date == model.Date && 
                                  wfh.IsActive == true && wfh.IsDeleted == false
                                  select wfh).ToList();

                if (loginId != 0)
                {
                    if (wfhDetails != null)
                    {
                        if (wfhDetails.Count() > 0)
                        {
                            for (int i = 0; i < wfhDetails.Count(); i++)
                            {
                                wfhDetails[i].AnalysisHr = model.AnalysisHr;
                                DB.SaveChanges();
                            }

                            WFHLoginlogViewModel wlvm = new WFHLoginlogViewModel();
                            wlvm.EmpCode = model.EmpCode;
                            wlvm.msg = "Analysis Hr Added";
                            return wlvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Employee WFH Login Details not found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee WFH Login Details not found");
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

        public List<WFHLoginlogViewModel> GetAllWFHAnalysis(WFHLoginlogViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var wfhDetails = (from wfh in DB.WFHLoginlogs
                                  join em in DB.EmployeeMasters on wfh.EmpId equals em.EmpId
                                  join comp in DB.CompanyMasters on em.CompId equals comp.CompId
                                  join dept in DB.DeptMasters on em.CategoryId equals dept.DeptId
                                  join desig in DB.DesignationMasters on em.DesignationId equals desig.DesignationId
                                  where wfh.AnalysisHr != null && wfh.IsActive == true && wfh.IsDeleted == false && em.EmpStatus.ToUpper() == "ACTIVE"
                                  select new
                                  {
                                      wfh,
                                      FullName = em.FirstName + " " + em.MiddleName + " " + em.LastName,
                                      comp.Company,
                                      dept.DeptName,
                                      desig.Designation,
                                      em.CategoryId,
                                      em.CompId,
                                      em.DesignationId
                                  })
                                  .Distinct()
                                  .OrderByDescending(x => x.wfh.EmpId)
                                  .ToList();

                if (loginId != 0)
                {
                    if (wfhDetails != null)
                    {
                        List<WFHLoginlogViewModel> lstofEmpWFH = new List<WFHLoginlogViewModel>();

                        foreach (var detail in wfhDetails)
                        {
                            WFHLoginlogViewModel wfhvm = new WFHLoginlogViewModel
                            {
                                WFHId = detail.wfh.WFHId,
                                LoginId = model.LoginId,
                                EmpId = detail.wfh.EmpId,
                                EmpName = detail.FullName,
                                EmpCode = detail.wfh.EmpCode,
                                IPAddress = detail.wfh.IPAddress,
                                Date = detail.wfh.Date,
                                LoginTime = detail.wfh.LoginTime,
                                LogOutTime = detail.wfh.LogOutTime,
                                Activehrs = detail.wfh.Activehrs,
                                AnalysisHr = detail.wfh.AnalysisHr,
                                IsLoggedIn = detail.wfh.IsLoggedIn,
                                IsLoggedOut = detail.wfh.IsLoggedOut,
                                CreatedBy = detail.wfh.CreatedBy,
                                CreatedDate = detail.wfh.CreatedDate,
                                LastUpdatedBy = detail.wfh.LastUpdatedBy,
                                LastUpdatedDate = detail.wfh.LastUpdatedDate,
                                IsActive = detail.wfh.IsActive,
                                IsUpdated = detail.wfh.IsUpdated,
                                IsDeleted = detail.wfh.IsDeleted,
                                CompId = detail.CompId,
                                CompName = detail.Company,
                                DeptId = detail.CategoryId,
                                DeptName = detail.DeptName,
                                DesignationId = detail.DesignationId,
                                Designation = detail.Designation
                            };

                            lstofEmpWFH.Add(wfhvm);
                        }

                        return lstofEmpWFH;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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

        public class LargeJsonResult : JsonResult
        {
            public LargeJsonResult()
            {
                MaxJsonLength = int.MaxValue;
            }

            public override void ExecuteResult(System.Web.Mvc.ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }

                if (Data != null)
                {
                    var json = new System.Web.Script.Serialization.JavaScriptSerializer
                    {
                        MaxJsonLength = (int)MaxJsonLength
                    };
                    response.Write(json.Serialize(Data));
                }
            }
        }

        //public List<AttendaceDateViewModel> EmployeeAttendance(AttendanceFilterViewModel model)
        //{
        //    try
        //    {
        //        int? empId = (model.EmpId != 0) ? model.EmpId : (int?)null;
        //        DateTime startDate = (model.StartDate != null) ? model.StartDate : DateTime.Today.AddMonths(-1);
        //        DateTime endDate = (model.EndDate != null) ? model.EndDate : DateTime.Today;

        //        startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
        //        endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

        //        var empDetails = (from emp in DB.EmployeeMasters
        //                          join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
        //                          join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
        //                          join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
        //                          where emp.EmpCode.Contains("3DCAD-")
        //                          select new
        //                          {
        //                              emp.EmpId,
        //                              emp.EmpCode,
        //                              EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
        //                              emp.OldEmp_ID,
        //                              emp.CompId,
        //                              CompName = comp.Company,
        //                              DesignationName = des.Designation,
        //                              DeptName = dept.DeptName,
        //                              emp.CategoryId,
        //                              emp.DesignationId
        //                          }).ToList();

        //        var logInData = DB.Attendances
        //            .Where(a => a.Type.ToUpper() == "IN" && a.LogDate >= startDate && a.LogDate <= endDate)
        //            .Select(a => new { a.LogID, a.LogDate, a.LogTime })
        //            .ToList();

        //        var logOutData = DB.Attendances
        //            .Where(a => a.Type.ToUpper() == "OUT" && a.LogDate >= startDate && a.LogDate <= endDate)
        //            .Select(a => new { a.LogID, a.LogDate, a.LogTime })
        //            .ToList();

        //        var attendanceTimes = DB.Emp_AttendanceTime
        //            .Where(at => at.LogDate >= startDate && at.LogDate <= endDate)
        //            .Select(at => new { at.LogId, at.LogDate, at.AttendHours, at.AttendMins, at.AttendSec })
        //            .ToList();

        //        List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();

        //        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //        {
        //            AttendaceDateViewModel advm = new AttendaceDateViewModel
        //            {
        //                AttendaceDate = date.ToString("yyyy-MM-dd")
        //            };

        //            List<AttendanceViewModel> lstOfAtt = new List<AttendanceViewModel>();

        //            foreach (var emp in empDetails)
        //            {
        //                var logInEntry = logInData.FirstOrDefault(x => x.LogID == emp.OldEmp_ID && x.LogDate == date);
        //                var logOutEntry = logOutData.FirstOrDefault(x => x.LogID == emp.OldEmp_ID && x.LogDate == date);
        //                var attendanceTimeEntry = attendanceTimes.FirstOrDefault(x => x.LogId == emp.OldEmp_ID && x.LogDate == date);

        //                AttendanceViewModel avm = new AttendanceViewModel
        //                {
        //                    EmpId = emp.EmpId,
        //                    EmpCode = emp.EmpCode,
        //                    EmpName = emp.EmpName.Trim(),
        //                    LogDate = date,
        //                    LogInTime = logInEntry?.LogTime != null ? logInEntry.LogTime.Value.ToString("HH:mm:ss") : "00:00:00",
        //                    LogOutTime = logOutEntry?.LogTime != null ? logOutEntry.LogTime.Value.ToString("HH:mm:ss") : "00:00:00",
        //                    WorkingHours = string.Format("{0:D2}:{1:D2}:{2:D2}",
        //                        attendanceTimeEntry?.AttendHours ?? 0,
        //                        attendanceTimeEntry?.AttendMins ?? 0,
        //                        attendanceTimeEntry?.AttendSec ?? 0),
        //                    CompId = emp.CompId,
        //                    CompName = emp.CompName,
        //                    Designation = emp.DesignationName,
        //                    DeptName = emp.DeptName,
        //                    DeptId = emp.CategoryId,
        //                    DesignationId = emp.DesignationId
        //                };

        //                lstOfAtt.Add(avm);
        //            }
        //            // Order the list of attendance by WorkingHours and EmpId
        //            advm.lstofAttendance = lstOfAtt
        //                .OrderBy(att => att.WorkingHours)
        //                .ThenBy(att => att.EmpId)
        //                .ToList();

        //            // advm.lstofAttendance = lstOfAtt;
        //            lstOfDate.Add(advm);
        //        }

        //        return lstOfDate;
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}

        public PaginatedResponse<AttendaceDateViewModel> EmployeeAttendance(AttendanceFilterViewModel model)
        {
            try
            {
                int? loginid = (model.LoginId != 0) ? model.LoginId : (int?)null;
                DateTime startDate = (model.StartDate != null) ? Convert.ToDateTime(model.StartDate) : DateTime.Today.AddMonths(-1);
                DateTime endDate = (model.EndDate != null) ? Convert.ToDateTime(model.EndDate) : DateTime.Today;

                int? compid = (model.CompId != 0) ? model.CompId : 0;
                int? deptid = (model.DeptId != 0) ? model.DeptId : 0;
                int? desigid = (model.DesignationId != 0) ? model.DesignationId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
                endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

                int pageNumber = model.PageNumber > 0 ? model.PageNumber : 1;
                int pageSize = model.PageSize > 0 ? model.PageSize : 10;

                string empcode = DB.EmployeeMasters.Where(x => x.EmpId == loginid && x.EmpStatus == "Active" && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmpCode).FirstOrDefault();

                var empDetailsQuery = from emp in DB.EmployeeMasters
                                      join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
                                      join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
                                      join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
                                      where (emp.ReportId == loginid || emp.ReportName.ToUpper() == empcode.ToUpper()) &&
                                      emp.EmpStatus.ToUpper() == "ACTIVE" && emp.IsActive == true && emp.IsDeleted == false
                                      select new
                                      {
                                          emp.EmpId,
                                          emp.EmpCode,
                                          EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                                          emp.OldEmp_ID,
                                          emp.CompId,
                                          CompName = comp.Company,
                                          DesignationName = des.Designation,
                                          DeptName = dept.DeptName,
                                          emp.CategoryId,
                                          emp.DesignationId
                                      };

                int totalRecords = empDetailsQuery.Count();

                var empDetails = empDetailsQuery    //.Where(x => x.EmpCode == "3DCAD-898")
                                  .OrderBy(emp => emp.EmpId)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

                var holidays = DB.Holidays
                            .Where(h =>
                                model.LocationId.Contains((int)h.LocationId) &&
                                h.Date >= startDate &&
                                h.Date <= endDate &&
                                h.Status == "Active")
                            .Select(h => new { h.Date, h.Title })
                            .ToList();

                var holidayDict = holidays
                    .GroupBy(h => h.Date.Date)
                    .ToDictionary(g => g.Key, g => string.Join(", ", g.Select(x => x.Title)));


                var weeklyHolidays = DB.WeekHolidays
                    .Where(w => w.Year == startDate.Year &&
                                w.Status == "Active" &&
                                model.LocationId.Contains((int)w.LocationId))
                    .Select(w => new { w.Day, w.LocationId })
                    .ToList();

                var weeklyHolidayDict = weeklyHolidays
                   .GroupBy(w => w.Day)
                   .ToDictionary(g => g.Key, g => g.Select(x => x.LocationId).Distinct().ToList());


                var logInData = DB.Attendances
                                .Where(a => a.Type.ToUpper() == "IN" && a.LogDate >= startDate && a.LogDate <= endDate)
                                .Select(a => new { a.LogID, a.LogDate, a.LogTime, a.EmpCode })
                                .ToList();

                var logOutData = DB.Attendances
                                .Where(a => a.Type.ToUpper() == "OUT" && a.LogDate >= startDate && a.LogDate <= endDate)
                                .Select(a => new { a.LogID, a.LogDate, a.LogTime, a.EmpCode })
                                .ToList();

                ////var shiftDetails = DB.CompanySettingMasters
                ////       .Where(shift => shift.IsActive == true && shift.IsDeleted == false)
                ////       .Select(shift => new
                ////       {
                ////           ShiftId = shift.ShiftId,
                ////           ShiftName = shift.Shift,
                ////           ShiftStart = shift.ShiftStart,
                ////           ShiftEnd = shift.ShiftEnd
                ////       })
                ////       .ToList();

                var attendanceTimes = DB.Emp_AttendanceTime
                        .Where(at => at.LogDate >= startDate && at.LogDate <= endDate)
                        .Select(at => new { at.LogId, at.LogDate, at.Duration, at.AttendHours, at.AttendMins, at.AttendSec, at.EmpCode })
                        .ToList();


                var wfhData = DB.WFHLoginlogs
                               .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
                               .Select(wfh => new
                               {
                                   wfh.EmpId,
                                   wfh.EmpCode,
                                   wfh.Date,
                                   wfh.IPAddress,
                                   wfh.LoginTime,
                                   wfh.LogOutTime,
                                   wfh.Activehrs,
                                   wfh.IsLoggedIn,
                                   wfh.IsLoggedOut
                               })
                               .ToList();

                var onsitedata = DB.OnSiteLoginlogs
                                 .Where(at => at.LoginDate >= startDate && at.LoginDate <= endDate)
                                 .Select(at => new { at.EmpId, at.LoginDate, at.LogInTime, at.LogOutTime, at.ActiveHrs, at.EmpCode })
                                 .ToList();

                var shiftDetails = DB.EmpShiftDetails
                           .Where(shift => shift.IsActive == true && shift.IsDeleted == false)
                           .Select(shift => new
                           {
                               shift.EmpId,
                               shift.EmpCode,
                               shift.ShiftId,
                               shift.ShiftName,
                               shift.StartDate,
                               shift.EndDate,
                               shift.IsActive
                           })
                           .ToList();

                List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    AttendaceDateViewModel advm = new AttendaceDateViewModel
                    {
                        AttendaceDate = date.ToString("yyyy-MM-dd")
                    };

                    List<AttendanceViewModel> lstOfAtt = new List<AttendanceViewModel>();

                    foreach (var emp in empDetails)
                    {
                        ////var logInEntry = logInData.FirstOrDefault(x => x.LogID == emp.OldEmp_ID && x.LogDate == date);
                        ////var logOutEntry = logOutData.FirstOrDefault(x => x.LogID == emp.OldEmp_ID && x.LogDate == date);

                        ////string logInTime = logInEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
                        ////string logOutTime = logOutEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";

                        ////var empShift = shiftDetails.FirstOrDefault(shift => shift.ShiftId == emp.CategoryId);
                        ////string shiftName = empShift?.ShiftName ?? "General Shift";

                        ////bool isHoliday = false;
                        ////string holidayReason = null;
                        ////var locationIds = model.LocationId;

                        ////if (holidayDict.ContainsKey(date.Date))
                        ////{
                        ////    isHoliday = true;
                        ////    holidayReason = holidayDict[date.Date];
                        ////}
                        ////else if (weeklyHolidayDict.ContainsKey(date.DayOfWeek.ToString()) &&
                        ////weeklyHolidayDict[date.DayOfWeek.ToString()].Where(id => id.HasValue).Select(id => id.Value).Intersect(locationIds).Any())
                        ////{
                        ////    isHoliday = true;
                        ////    holidayReason = date.DayOfWeek.ToString();
                        ////}


                        ////AttendanceViewModel avm = new AttendanceViewModel
                        ////{
                        ////    EmpId = emp.EmpId,
                        ////    EmpCode = emp.EmpCode,
                        ////    EmpName = emp.EmpName.Trim(),
                        ////    LogDate = date,
                        ////    LogInTime = isHoliday ? holidayReason : logInTime,
                        ////    LogOutTime = isHoliday ? holidayReason : logOutTime,
                        ////    CompId = emp.CompId,
                        ////    CompName = emp.CompName,
                        ////    Designation = emp.DesignationName,
                        ////    DeptName = emp.DeptName,
                        ////    DeptId = emp.CategoryId,
                        ////    DesignationId = emp.DesignationId,
                        ////    ShiftName = shiftName,
                        ////    IsHoliday = isHoliday,
                        ////    HolidayName = isHoliday ? holidayReason : null
                        ////};  //16.10.2025
                        ///

                        var logInEntry = logInData.FirstOrDefault(x => x.EmpCode == emp.EmpCode && x.LogDate == date);
                        var logOutEntry = logOutData.FirstOrDefault(x => x.EmpCode == emp.EmpCode && x.LogDate == date);
                        ////var attendanceTimeEntry = attendanceTimes.FirstOrDefault(x => x.LogId == emp.OldEmp_ID && x.LogDate == date).OrderByDescending(x => x.AttendHours);
                        //var attendanceTimeEntry = attendanceTimes.Where(x => x.LogId == emp.OldEmp_ID && x.LogDate == date).OrderByDescending(x => x.AttendHours).FirstOrDefault();
                        var attendanceTimeEntry = attendanceTimes.Where(x => x.EmpCode == emp.EmpCode && x.LogDate == date).OrderByDescending(x => x.AttendHours).FirstOrDefault();
                        var empShift = shiftDetails.FirstOrDefault(shift => shift.EmpCode == emp.EmpCode && date >= shift.StartDate && date <= shift.EndDate);
                        string logInTime = logInEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
                        string logOutTime = logOutEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
                        string activeHours = "00:00:00";
                        int wfhDetails = 0;
                        int onsite = 0;

                        if (logInEntry?.LogTime != null)
                        {
                            logInTime = ((DateTime)logInEntry.LogTime).ToString("HH:mm:ss");
                        }

                        if (logOutEntry?.LogTime != null)
                        {
                            logOutTime = ((DateTime)logOutEntry.LogTime).ToString("HH:mm:ss");
                        }


                        if (logInTime == "00:00:00" && logOutTime == "00:00:00")
                        {
                            var wfhEntries = wfhData
                                                .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.Date == date.Date)
                                                .OrderBy(x => x.LoginTime)
                                                .ToList();

                            TimeSpan totalWfhActiveHours = TimeSpan.Zero;
                            TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

                            TimeSpan? firstLogin = null;
                            TimeSpan? lastLogout = null;

                            if (wfhEntries.Any())
                            {
                                for (int i = 0; i < wfhEntries.Count; i++)
                                {
                                    var entry = wfhEntries[i];

                                    if (!entry.LoginTime.HasValue)
                                        continue;

                                    TimeSpan logIn = entry.LoginTime.Value;
                                    TimeSpan logOut;

                                    // Earliest login
                                    if (!firstLogin.HasValue || logIn < firstLogin)
                                        firstLogin = logIn;

                                    // Determine logout
                                    if (entry.LogOutTime.HasValue)
                                    {
                                        logOut = entry.LogOutTime.Value;
                                    }
                                    else if (i + 1 < wfhEntries.Count && wfhEntries[i + 1].LoginTime.HasValue)
                                    {
                                        logOut = wfhEntries[i + 1].LoginTime.Value;
                                    }
                                    else
                                    {
                                        logOut = defaultLogout;
                                    }

                                    // Latest logout
                                    if (!lastLogout.HasValue || logOut > lastLogout)
                                        lastLogout = logOut;

                                    if (logOut > logIn)
                                        totalWfhActiveHours += (logOut - logIn);
                                }

                                // Final output
                                logInTime = firstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                                logOutTime = lastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                                activeHours = totalWfhActiveHours.ToString(@"hh\:mm\:ss");
                                wfhDetails = 1; 
                            }
                            ////if (wfhEntry != null)
                            ////{
                            ////    if (wfhEntry.LoginTime.HasValue && wfhEntry.LogOutTime.HasValue)
                            ////    {
                            ////        logInTime = wfhEntry.LoginTime.Value.ToString(@"hh\:mm\:ss");
                            ////        logOutTime = wfhEntry.LogOutTime.Value.ToString(@"hh\:mm\:ss");

                            ////        TimeSpan logIn = (TimeSpan)wfhEntry.LoginTime;
                            ////        TimeSpan logOut = (TimeSpan)wfhEntry.LogOutTime;
                            ////        activeHours = (logOut - logIn > TimeSpan.Zero) ? (logOut - logIn).ToString(@"hh\:mm\:ss") : "00:00:00";
                            ////    }
                            ////    else
                            ////    {

                            ////        activeHours = wfhEntry.Activehrs?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                            ////    }
                            ////    wfhDetails = 1;
                            ////}
                            else
                            {
                                ////var onsiteEntry = onsitedata.FirstOrDefault(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LoginDate == date);
                                ////if (onsiteEntry != null)
                                ////{
                                ////    if (onsiteEntry.LogInTime.HasValue && onsiteEntry.LogOutTime.HasValue)
                                ////    {
                                ////        logInTime = onsiteEntry.LogInTime.Value.ToString(@"hh\:mm\:ss");
                                ////        logOutTime = onsiteEntry.LogOutTime.Value.ToString(@"hh\:mm\:ss");

                                ////        TimeSpan logIn = (TimeSpan)onsiteEntry.LogInTime.Value;
                                ////        TimeSpan logOut = (TimeSpan)onsiteEntry.LogOutTime.Value;
                                ////        activeHours = (logOut - logIn > TimeSpan.Zero) ? (logOut - logIn).ToString(@"hh\:mm\:ss") : "00:00:00";
                                ////    }
                                ////    onsite = 2;
                                ////}
                                ///

                                var onsiteEntry = onsitedata
                                                    .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LoginDate == date.Date)
                                                    .OrderBy(x => x.LogInTime)
                                                    .ToList();

                                TimeSpan totalOnsiteActiveHours = TimeSpan.Zero;
                                TimeSpan OnsitedefaultLogout = new TimeSpan(18, 36, 0);

                                TimeSpan? OnsitefirstLogin = null;
                                TimeSpan? OnsitelastLogout = null;

                                if (onsiteEntry.Any())
                                {
                                    for (int i = 0; i < onsiteEntry.Count; i++)
                                    {
                                        var entry = onsiteEntry[i];

                                        if (!entry.LogInTime.HasValue)
                                            continue;

                                        TimeSpan logIn = entry.LogInTime.Value;
                                        TimeSpan logOut;

                                        // Earliest login
                                        if (!OnsitefirstLogin.HasValue || logIn < OnsitefirstLogin)
                                            OnsitefirstLogin = logIn;

                                        // Determine logout
                                        if (entry.LogOutTime.HasValue)
                                        {
                                            logOut = entry.LogOutTime.Value;
                                        }
                                        else if (i + 1 < onsiteEntry.Count && onsiteEntry[i + 1].LogInTime.HasValue)
                                        {
                                            logOut = onsiteEntry[i + 1].LogInTime.Value;
                                        }
                                        else
                                        {
                                            logOut = OnsitedefaultLogout;
                                        }

                                        // Latest logout
                                        if (!OnsitelastLogout.HasValue || logOut > OnsitelastLogout)
                                            OnsitelastLogout = logOut;

                                        if (logOut > logIn)
                                            totalOnsiteActiveHours += (logOut - logIn);
                                    }

                                    // Final output
                                    logInTime = OnsitefirstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                                    logOutTime = OnsitelastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                                    activeHours = totalOnsiteActiveHours.ToString(@"hh\:mm\:ss");
                                    //workmode = "ONSITE";
                                    onsite = 2;
                                }
                            }
                        }
                        else
                        {
                            if (attendanceTimeEntry != null)
                            {
                                if (attendanceTimeEntry.Duration.HasValue)
                                {
                                    activeHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
                                }
                            }
                        }

                        //if (!string.IsNullOrWhiteSpace(logInTime) && !string.IsNullOrWhiteSpace(logOutTime) &&
                        //    logInTime != "00:00:00" && logOutTime != "00:00:00")
                        //{
                        //    if (TimeSpan.TryParse(logInTime, out TimeSpan logIn) && TimeSpan.TryParse(logOutTime, out TimeSpan logOut))
                        //    {
                        //        TimeSpan activeDuration = logOut - logIn;
                        //        activeHours = activeDuration > TimeSpan.Zero ? activeDuration.ToString(@"hh\:mm\:ss") : "00:00:00";
                        //        //if (wfhDetails == 0)
                        //        //{
                        //        //    if (onsite == 0)
                        //        //    {
                        //        //        if (attendanceTimeEntry != null)
                        //        //        {
                        //        //            if (attendanceTimeEntry.Duration.HasValue)
                        //        //            {
                        //        //                activeHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
                        //        //            }
                        //        //        }
                        //        //    }
                        //        //}
                        //    }
                        //}
                        //else {
                        //    if (attendanceTimeEntry != null)
                        //    {
                        //        if (attendanceTimeEntry.Duration.HasValue)
                        //        {
                        //            activeHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
                        //        }
                        //    }
                        //}


                        TimeSpan workingHours = TimeSpan.Zero;
                        if (DateTime.TryParse(logInTime, out DateTime logInDateTime) && DateTime.TryParse(logOutTime, out DateTime logOutDateTime))
                        {
                            TimeSpan logIn = logInDateTime.TimeOfDay;
                            TimeSpan logOut = logOutDateTime.TimeOfDay;

                            workingHours = logOut - logIn;

                            if (workingHours < TimeSpan.Zero)
                            {
                                workingHours = TimeSpan.Zero;
                            }
                        }

                        var onsiteLogs = DB.Loginlogs
                            .Where(log => log.EmpId == emp.EmpId && log.LoginDate == date)
                            .Select(log => new { log.LogInTime, log.LogOutTime })
                            .ToList();

                        TimeSpan totalActiveHours = TimeSpan.Zero;
                        TimeSpan breakTime = TimeSpan.Zero;

                        if (onsiteLogs.Any())
                        {
                            var minLogIn = onsiteLogs.Min(log => log.LogInTime);
                            var maxLogOut = onsiteLogs.Max(log => log.LogOutTime);

                            if (minLogIn.HasValue && maxLogOut.HasValue)
                            {
                                TimeSpan totalTime = maxLogOut.Value - minLogIn.Value;

                                foreach (var log in onsiteLogs)
                                {
                                    if (log.LogInTime.HasValue && log.LogOutTime.HasValue)
                                    {
                                        TimeSpan sessionDuration = log.LogOutTime.Value - log.LogInTime.Value;
                                        if (sessionDuration > TimeSpan.Zero)
                                        {
                                            totalActiveHours += sessionDuration;
                                        }
                                    }
                                }

                                breakTime = totalTime - totalActiveHours;
                                if (breakTime < TimeSpan.Zero)
                                {
                                    breakTime = TimeSpan.Zero;
                                }
                            }
                        }

                        ////int year = Convert.ToInt32(model.Year);
                        ////int month = model.MonthNo;

                        ////decimal? totalDays = DateTime.DaysInMonth(year, month);

                        ////// start & end dates
                        ////DateTime startDate = new DateTime(year, month, 1);
                        ////DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                        ///

                        DateTime startDate1 = startDate; // assign existing startDate to a new variable
                        DateTime endDate1 = endDate;     // assign existing endDate to a new variable

                        // Calculate the difference using the new variables
                        TimeSpan difference = endDate1 - startDate1;

                        // Get total days as decimal
                        decimal totalDays = (decimal)difference.TotalDays;

                        var lop = (from lev in DB.EmpLeaveApplications
                                   where lev.EmpId == emp.EmpId
                                      && lev.LeaveTypeId == 0
                                      && lev.StartDate >= startDate
                                      && lev.EndDate <= endDate
                                      && lev.IsActive == true
                                      && lev.IsDeleted == false
                                   orderby lev.StartDate descending
                                   select lev).ToList();

                        ////var cl = (from lev in DB.EmpLeaveApplications
                        ////          join 6

                        ////           where lev.EmpId == emp.EmpId
                        ////              && lev.LeaveTypeId == 0
                        ////              && lev.StartDate >= startDate
                        ////              && lev.EndDate <= endDate
                        ////              && lev.IsActive == true
                        ////              && lev.IsDeleted == false
                        ////           orderby lev.StartDate descending
                        ////           select lev).ToList();

                        ////var el = (from lev in DB.EmpLeaveApplications
                        ////           where lev.EmpId == emp.EmpId
                        ////              && lev.LeaveTypeId == 0
                        ////              && lev.StartDate >= startDate
                        ////              && lev.EndDate <= endDate
                        ////              && lev.IsActive == true
                        ////              && lev.IsDeleted == false
                        ////           orderby lev.StartDate descending
                        ////           select lev).ToList();

                        ////var rh = (from lev in DB.EmpLeaveApplications
                        ////           where lev.EmpId == emp.EmpId
                        ////              && lev.LeaveTypeId == 0
                        ////              && lev.StartDate >= startDate
                        ////              && lev.EndDate <= endDate
                        ////              && lev.IsActive == true
                        ////              && lev.IsDeleted == false
                        ////           orderby lev.StartDate descending
                        ////           select lev).ToList();

                        decimal? lopDuration = (from lev in DB.EmpLeaveApplications
                                                where lev.EmpId == emp.EmpId
                                                   && lev.LeaveTypeId == 0
                                                   && lev.StartDate >= startDate
                                                   && lev.EndDate <= endDate
                                                   && lev.IsActive == true
                                                   && lev.IsDeleted == false
                                                select lev.Duration)
                                               .DefaultIfEmpty(0)           // avoid null result
                                               .Sum();

                        decimal? workingdays = totalDays - lopDuration;

                        AttendanceViewModel avm = new AttendanceViewModel
                        {
                            EmpId = emp.EmpId,
                            EmpCode = emp.EmpCode,
                            EmpName = emp.EmpName.Trim(),
                            LogDate = date,
                            LogInTime = logInTime,
                            LogOutTime = logOutTime,
                            WorkingHours = activeHours, /*workingHours.ToString(@"hh\:mm\:ss"),*/
                            CompId = emp.CompId,
                            CompName = emp.CompName,
                            Designation = emp.DesignationName,
                            DeptName = emp.DeptName,
                            DeptId = emp.CategoryId,
                            DesignationId = emp.DesignationId,
                            PayDays = workingdays,
                            LeaveType = "",
                            //PayDays = workingdays,
                            ActiveHours = activeHours,
                            ShiftName = empShift?.ShiftName ?? "No Shift",
                            WorkType = wfhDetails == 1 ? "WFH" : (onsite == 2 ? "OnSite" : string.Empty),
                            BreakTime = breakTime.ToString(@"hh\:mm\:ss"),
                        };

                        lstOfAtt.Add(avm);
                    }

                    if (compid != 0)
                    {
                        lstOfAtt.Where(x => x.CompId == compid).OrderBy(att => att.EmpId).ToList();
                    }
                    if (deptid != 0)
                    {
                        lstOfAtt.Where(x => x.DeptId == deptid).OrderBy(att => att.EmpId).ToList();
                    }
                    if (desigid != 0)
                    {
                        lstOfAtt.Where(x => x.DesignationId == desigid).OrderBy(att => att.EmpId).ToList();
                    }
                    if (empId != 0)
                    {
                        lstOfAtt.Where(x => x.EmpId == empId).OrderBy(att => att.EmpId).ToList();
                    }
                    advm.lstofAttendance = lstOfAtt.OrderBy(att => att.EmpName).ToList();
                    lstOfDate.Add(advm);
                }

                return new PaginatedResponse<AttendaceDateViewModel>
                {
                    Data = lstOfDate,
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        ////// ----------------------- 03.02.2026 -- Response time -- Working Properly -- Start ------------------------------------------------------------------- //////
        ////public List<AttendaceDateViewModel> AttendanceFilter(AttendanceFilterViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? compId = (model.CompId != 0) ? model.CompId : 0;
        ////        int? leId = (model.LEId != 0) ? model.LEId : 0;
        ////        int? buId = (model.BUId != 0) ? model.BUId : 0;
        ////        int? locId = (model.LocId != 0) ? model.LocId : 0;
        ////        int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
        ////        int? designationId = (model.DesignationId != 0) ? model.DesignationId : 0;
        ////        int? empId = (model.EmpId != 0) ? model.EmpId : 0;
        ////        int? clid = 0, elid = 0, rhid = 0;

        ////        DateTime today = DateTime.Today;

        ////        DateTime startDate = (model.StartDate != null) ? Convert.ToDateTime(model.StartDate) : new DateTime(today.Year, today.Month, 1);  //? new DateTime(today.Year, today.Month, 1)  DateTime.Today.AddMonths(-1)
        ////        DateTime endDate = (model.EndDate != null) ? Convert.ToDateTime(model.EndDate) : today.AddDays(-1);

        ////        startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
        ////        endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

        ////        int weekdenddayscount = 0;
        ////        int weekdenddayscount1 = 0;

        ////        for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        ////        {
        ////            if (date.DayOfWeek == DayOfWeek.Saturday ||
        ////                date.DayOfWeek == DayOfWeek.Sunday)
        ////            {
        ////                weekdenddayscount++;
        ////            }
        ////        }

        ////        for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        ////        {
        ////            if (date.DayOfWeek == DayOfWeek.Sunday)
        ////            {
        ////                weekdenddayscount1++;
        ////            }
        ////        }

        ////        clid = (from lev in DB.LeaveTypeMasters
        ////                where lev.ShortName == "CL"
        ////                   && lev.IsActive == true
        ////                   && lev.IsDeleted == false
        ////                select lev.LeaveTypeId).FirstOrDefault();

        ////        elid = (from lev in DB.LeaveTypeMasters
        ////                where lev.ShortName == "EL"
        ////                   && lev.IsActive == true
        ////                   && lev.IsDeleted == false
        ////                select lev.LeaveTypeId).FirstOrDefault();

        ////        rhid = (from lev in DB.LeaveTypeMasters
        ////                where lev.ShortName == "RH"
        ////                   && lev.IsActive == true
        ////                   && lev.IsDeleted == false
        ////                select lev.LeaveTypeId).FirstOrDefault();

        ////        var empDetails = (from emp in DB.EmployeeMasters
        ////                          join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
        ////                          join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
        ////                          join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
        ////                          //where emp.EmpCode.Contains("3DCAD-")
        ////                          where emp.IsActive == true && emp.EmpStatus.ToUpper() == "ACTIVE"
        ////                          select new
        ////                          {
        ////                              emp.EmpId,
        ////                              emp.EmpCode,
        ////                              EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
        ////                              emp.OldEmp_ID,
        ////                              emp.CompId,
        ////                              emp.LEId,
        ////                              emp.BUId,
        ////                              emp.LocationId,
        ////                              CompName = comp.Company,
        ////                              DesignationName = des.Designation,
        ////                              DeptName = dept.DeptName,
        ////                              emp.CategoryId,
        ////                              emp.DesignationId
        ////                          }).ToList();

        ////        if (compId != 0)
        ////        {
        ////            var compfilter = empDetails.Where(x => x.CompId == compId).ToList();
        ////            empDetails = compfilter.ToList();
        ////        }
        ////        if (leId != 0)
        ////        {
        ////            var lefilter = empDetails.Where(x => x.LEId == leId).ToList();
        ////            empDetails = lefilter.ToList();
        ////        }
        ////        if (buId != 0)
        ////        {
        ////            var bufilter = empDetails.Where(x => x.BUId == buId).ToList();
        ////            empDetails = bufilter.ToList();
        ////        }
        ////        if (locId != 0)
        ////        {
        ////            var locfilter = empDetails.Where(x => x.LocationId == locId).ToList();
        ////            empDetails = locfilter.ToList();
        ////        }
        ////        if (deptId != 0)
        ////        {
        ////            var deptfilter = empDetails.Where(x => x.CategoryId == deptId).ToList();
        ////            empDetails = deptfilter.ToList();


        ////        }
        ////        if (designationId != 0)
        ////        {
        ////            var desgfilter = empDetails.Where(x => x.DesignationId == designationId).ToList();
        ////            empDetails = desgfilter.ToList();
        ////        }
        ////        if (designationId != 0)
        ////        {
        ////            var desgfilter = empDetails.Where(x => x.DesignationId == designationId).ToList();
        ////            empDetails = desgfilter.ToList();
        ////        }
        ////        if (empId != 0)
        ////        {
        ////            var empfilter = empDetails.Where(x => x.EmpId == empId).ToList();
        ////            empDetails = empfilter.ToList();
        ////        }

        ////        if (empDetails.Count() != 0)
        ////        {
        ////            var logInData = DB.Attendances
        ////            .Where(a => a.Type.ToUpper() == "IN" && a.LogDate >= startDate && a.LogDate <= endDate)
        ////            .Select(a => new { a.LogID, a.LogDate, a.LogTime, a.EmpCode })
        ////            .ToList();

        ////            var logOutData = DB.Attendances
        ////                .Where(a => a.Type.ToUpper() == "OUT" && a.LogDate >= startDate && a.LogDate <= endDate)
        ////                .Select(a => new { a.LogID, a.LogDate, a.LogTime, a.EmpCode })
        ////                .ToList();

        ////            var attendanceTimes = DB.Emp_AttendanceTime
        ////                .Where(at => at.LogDate >= startDate && at.LogDate <= endDate)
        ////                .Select(at => new { at.LogId, at.LogDate, at.Duration, at.AttendHours, at.AttendMins, at.AttendSec, at.EmpCode })
        ////                .ToList();


        ////            var wfhData = DB.WFHLoginlogs
        ////                           .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
        ////                           .Select(wfh => new
        ////                           {
        ////                               wfh.EmpId,
        ////                               wfh.EmpCode,
        ////                               wfh.Date,
        ////                               wfh.IPAddress,
        ////                               wfh.LoginTime,
        ////                               wfh.LogOutTime,
        ////                               wfh.Activehrs,
        ////                               wfh.IsLoggedIn,
        ////                               wfh.IsLoggedOut
        ////                           })
        ////                           .ToList();

        ////            var onsitedata = DB.OnSiteLoginlogs
        ////                             .Where(at => at.LoginDate >= startDate && at.LoginDate <= endDate)
        ////                             .Select(at => new { at.EmpId, at.LoginDate, at.LogInTime, at.LogOutTime, at.ActiveHrs, at.EmpCode })
        ////                             .ToList();

        ////            var manualdata = DB.ManualAttendances
        ////                             .Where(at => at.Date >= startDate && at.Date <= endDate)
        ////                             .Select(at => new { at.EmpCode, at.Date, at.Time, at.Id })
        ////                             .ToList();

        ////            var shiftDetails = DB.EmpShiftDetails
        ////                       .Where(shift => shift.IsActive == true && shift.IsDeleted == false)
        ////                       .Select(shift => new
        ////                       {
        ////                           shift.EmpId,
        ////                           shift.EmpCode,
        ////                           shift.ShiftId,
        ////                           shift.ShiftName,
        ////                           shift.StartDate,
        ////                           shift.EndDate,
        ////                           shift.IsActive
        ////                       })
        ////                       .ToList();

        ////            List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();

        ////            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        ////            {
        ////                AttendaceDateViewModel advm = new AttendaceDateViewModel
        ////                {
        ////                    AttendaceDate = date.ToString("yyyy-MM-dd")
        ////                };

        ////                List<AttendanceViewModel> lstOfAtt = new List<AttendanceViewModel>();

        ////                foreach (var emp in empDetails)
        ////                {
        ////                    var logInEntry = logInData.FirstOrDefault(x => x.EmpCode == emp.EmpCode && x.LogDate == date);
        ////                    var logOutEntry = logOutData.FirstOrDefault(x => x.EmpCode == emp.EmpCode && x.LogDate == date);
        ////                    //var attendanceTimeEntry = attendanceTimes.FirstOrDefault(x => x.LogId == emp.OldEmp_ID && x.LogDate == date);
        ////                    var attendanceTimeEntry = attendanceTimes.Where(x => x.EmpCode == emp.EmpCode && x.LogDate == date).OrderByDescending(x => x.AttendHours).FirstOrDefault();
        ////                    var empShift = shiftDetails.FirstOrDefault(shift => shift.EmpCode == emp.EmpCode && date >= shift.StartDate && date <= shift.EndDate);
        ////                    int? locationId = DB.EmployeeMasters.Where(x => x.EmpId == emp.EmpId && x.EmpStatus.ToUpper() == "ACTIVE" && x.IsActive == true && x.IsDeleted == false)
        ////                                        .Select(x => x.LocationId).FirstOrDefault() ?? 0;
        ////                    string ESSLlogInTime = logInEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
        ////                    string ESSLlogOutTime = logOutEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
        ////                    string logInTime = "00:00:00";
        ////                    string logOutTime = "00:00:00";
        ////                    string WFHlogInTime = "00:00:00";
        ////                    string WFHlogOutTime = "00:00:00";
        ////                    string ONSITElogInTime = "00:00:00";
        ////                    string ONSITElogOutTime = "00:00:00";
        ////                    string MANUALlogInTime = "00:00:00";
        ////                    string MANUALlogOutTime = "00:00:00";
        ////                    string activeHours = "00:00:00";
        ////                    string ESSLactiveHours = "00:00:00";
        ////                    string WFHactiveHours = "00:00:00";
        ////                    string ONSITEactiveHours = "00:00:00";
        ////                    string MANUALactiveHours = "00:00:00";
        ////                    string wfhDetails = "";
        ////                    string onsiteDetails = "";
        ////                    string esslDetails = "";
        ////                    string manualDetails = "";

        ////                    if (logInEntry?.LogTime != null)
        ////                    {
        ////                        ESSLlogInTime = ((DateTime)logInEntry.LogTime).ToString("HH:mm:ss");
        ////                    }

        ////                    if (logOutEntry?.LogTime != null)
        ////                    {
        ////                        ESSLlogOutTime = ((DateTime)logOutEntry.LogTime).ToString("HH:mm:ss");
        ////                    }

        ////                    var wfhEntries = wfhData
        ////                                            .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.Date == date.Date)
        ////                                            .OrderBy(x => x.LoginTime)
        ////                                            .ToList();

        ////                    TimeSpan totalWfhActiveHours = TimeSpan.Zero;
        ////                    TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

        ////                    TimeSpan? firstLogin = null;
        ////                    TimeSpan? lastLogout = null;

        ////                    if (wfhEntries.Any())
        ////                    {
        ////                        for (int i = 0; i < wfhEntries.Count; i++)
        ////                        {
        ////                            var entry = wfhEntries[i];

        ////                            if (!entry.LoginTime.HasValue)
        ////                                continue;

        ////                            TimeSpan logIn = entry.LoginTime.Value;
        ////                            TimeSpan logOut;

        ////                            // Earliest login
        ////                            if (!firstLogin.HasValue || logIn < firstLogin)
        ////                                firstLogin = logIn;

        ////                            // Determine logout
        ////                            if (entry.LogOutTime.HasValue)
        ////                            {
        ////                                logOut = entry.LogOutTime.Value;
        ////                            }
        ////                            else if (i + 1 < wfhEntries.Count && wfhEntries[i + 1].LoginTime.HasValue)
        ////                            {
        ////                                logOut = wfhEntries[i + 1].LoginTime.Value;
        ////                            }
        ////                            else
        ////                            {
        ////                                logOut = defaultLogout;
        ////                            }

        ////                            // Latest logout
        ////                            if (!lastLogout.HasValue || logOut > lastLogout)
        ////                                lastLogout = logOut;

        ////                            if (logOut > logIn)
        ////                                totalWfhActiveHours += (logOut - logIn);
        ////                        }

        ////                        // Final output
        ////                        WFHlogInTime = firstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                        WFHlogOutTime = lastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                        WFHactiveHours = totalWfhActiveHours.ToString(@"hh\:mm\:ss");
        ////                        wfhDetails = "WFH";
        ////                    }

        ////                    var onsiteEntry = onsitedata
        ////                                           .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LoginDate == date.Date)
        ////                                           .OrderBy(x => x.LogInTime)
        ////                                           .ToList();

        ////                    TimeSpan totalOnsiteActiveHours = TimeSpan.Zero;
        ////                    TimeSpan OnsitedefaultLogout = new TimeSpan(18, 36, 0);

        ////                    TimeSpan? OnsitefirstLogin = null;
        ////                    TimeSpan? OnsitelastLogout = null;

        ////                    if (onsiteEntry.Any())
        ////                    {
        ////                        for (int i = 0; i < onsiteEntry.Count; i++)
        ////                        {
        ////                            var entry = onsiteEntry[i];

        ////                            if (!entry.LogInTime.HasValue)
        ////                                continue;

        ////                            TimeSpan logIn = entry.LogInTime.Value;
        ////                            TimeSpan logOut;

        ////                            // Earliest login
        ////                            if (!OnsitefirstLogin.HasValue || logIn < OnsitefirstLogin)
        ////                                OnsitefirstLogin = logIn;

        ////                            // Determine logout
        ////                            if (entry.LogOutTime.HasValue)
        ////                            {
        ////                                logOut = entry.LogOutTime.Value;
        ////                            }
        ////                            else if (i + 1 < onsiteEntry.Count && onsiteEntry[i + 1].LogInTime.HasValue)
        ////                            {
        ////                                logOut = onsiteEntry[i + 1].LogInTime.Value;
        ////                            }
        ////                            else
        ////                            {
        ////                                logOut = OnsitedefaultLogout;
        ////                            }

        ////                            // Latest logout
        ////                            if (!OnsitelastLogout.HasValue || logOut > OnsitelastLogout)
        ////                                OnsitelastLogout = logOut;

        ////                            if (logOut > logIn)
        ////                                totalOnsiteActiveHours += (logOut - logIn);
        ////                        }

        ////                        // Final output
        ////                        ONSITElogInTime = OnsitefirstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                        ONSITElogOutTime = OnsitelastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                        ONSITEactiveHours = totalOnsiteActiveHours.ToString(@"hh\:mm\:ss");
        ////                        //workmode = "ONSITE";
        ////                        onsiteDetails = "ONSITE";
        ////                    }

        ////                    if (attendanceTimeEntry != null)
        ////                    {
        ////                        if (attendanceTimeEntry.Duration.HasValue)
        ////                        {
        ////                            ESSLactiveHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
        ////                            if (ESSLactiveHours != "00:00:00")
        ////                            {
        ////                                esslDetails = "ESSL";
        ////                            }

        ////                        }
        ////                    }

        ////                    var manualEntry = manualdata
        ////                                           .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.Date == date.Date)
        ////                                           .OrderByDescending(x => x.Id)
        ////                                           .FirstOrDefault();

        ////                    if (manualEntry != null)
        ////                    {
        ////                        if (manualEntry.Time.HasValue)
        ////                        {
        ////                            MANUALactiveHours = manualEntry.Time.Value.ToString(@"hh\:mm\:ss");

        ////                            if (MANUALactiveHours != "00:00:00")
        ////                            {
        ////                                manualDetails = "MANUAL";
        ////                            }
        ////                        }
        ////                    }

        ////                    ////TimeSpan essl = TimeSpan.Parse(ESSLactiveHours);
        ////                    ////TimeSpan wfh = TimeSpan.Parse(WFHactiveHours);
        ////                    ////TimeSpan onsite = TimeSpan.Parse(ONSITEactiveHours);

        ////                    ////TimeSpan total = essl + wfh + onsite;

        ////                    ////activeHours = total.ToString(@"hh\:mm\:ss");
        ////                    ///

        ////                    //Active Hrs calculation ESSL, WFH & ONSITE without conflicts 12.01.2026
        ////                    //Start
        ////                    TimeSpan esslLogin = TimeSpan.Parse(ESSLlogInTime);
        ////                    TimeSpan esslLogout = TimeSpan.Parse(ESSLlogOutTime);

        ////                    TimeSpan wfhLogin = TimeSpan.Parse(WFHlogInTime);
        ////                    TimeSpan wfhLogout = TimeSpan.Parse(WFHlogOutTime);

        ////                    TimeSpan onsiteLogin = TimeSpan.Parse(ONSITElogInTime);
        ////                    TimeSpan onsiteLogout = TimeSpan.Parse(ONSITElogOutTime);

        ////                    List<TimeInterval> intervals = new List<TimeInterval>();

        ////                    // 1️⃣ ESSL (PRIMARY)
        ////                    TimeSpan total = TimeSpan.Zero;

        ////                    if (ESSLIsValid(esslLogin, esslLogout))
        ////                    {
        ////                        intervals.Add(new TimeInterval { Start = esslLogin, End = esslLogout });
        ////                        total += TimeSpan.Parse(ESSLactiveHours);
        ////                    }

        ////                    // 2️⃣ WFH (Add only NON-overlapping time)
        ////                    if (IsValid(wfhLogin, wfhLogout))
        ////                    {
        ////                        TimeInterval wfh = new TimeInterval { Start = wfhLogin, End = wfhLogout };

        ////                        TimeSpan overlap = TimeSpan.Zero;
        ////                        foreach (var i in intervals)
        ////                            overlap += GetOverlap(i, wfh);

        ////                        total += (wfh.End - wfh.Start - overlap);
        ////                        intervals.Add(wfh);
        ////                    }

        ////                    // 3️⃣ ONSITE (Add only NON-overlapping time)
        ////                    if (ESSLIsValid(onsiteLogin, onsiteLogout))
        ////                    {
        ////                        TimeInterval onsite = new TimeInterval { Start = onsiteLogin, End = onsiteLogout };

        ////                        // Total duration (handles night shift)
        ////                        TimeSpan onsiteDuration = CalculateDuration(onsite.Start, onsite.End);

        ////                        ////TimeSpan overlap = TimeSpan.Zero;
        ////                        ////foreach (var i in intervals)
        ////                        ////    overlap += GetOverlap(i, onsite);

        ////                        ////total += (onsite.End - onsite.Start - overlap);
        ////                        ///
        ////                        // Calculate overlaps
        ////                        TimeSpan overlap = TimeSpan.Zero;
        ////                        foreach (var i in intervals)
        ////                        {
        ////                            overlap += GetOverlap(i, onsite);
        ////                        }

        ////                        // Add NON-overlapping worked time
        ////                        total += onsiteDuration - overlap;
        ////                    }

        ////                    // Final Active Hours
        ////                    activeHours = total.ToString(@"hh\:mm\:ss");

        ////                    //End

        ////                    string worktype = "";

        ////                    if (!string.IsNullOrEmpty(esslDetails))
        ////                        worktype += esslDetails;

        ////                    if (!string.IsNullOrEmpty(wfhDetails))
        ////                        worktype += (worktype == "" ? "" : " + ") + wfhDetails;

        ////                    if (!string.IsNullOrEmpty(onsiteDetails))
        ////                        worktype += (worktype == "" ? "" : " + ") + onsiteDetails;

        ////                    //activeHours = ESSLactiveHours + WFHactiveHours + ONSITEactiveHours;

        ////                    TimeSpan workingHours = TimeSpan.Zero;
        ////                    if (DateTime.TryParse(logInTime, out DateTime logInDateTime) && DateTime.TryParse(logOutTime, out DateTime logOutDateTime))
        ////                    {
        ////                        TimeSpan logIn = logInDateTime.TimeOfDay;
        ////                        TimeSpan logOut = logOutDateTime.TimeOfDay;

        ////                        workingHours = logOut - logIn;

        ////                        if (workingHours < TimeSpan.Zero)
        ////                        {
        ////                            workingHours = TimeSpan.Zero;
        ////                        }
        ////                    }

        ////                    var onsiteLogs = DB.Loginlogs
        ////                        .Where(log => log.EmpId == emp.EmpId && log.LoginDate == date)
        ////                        .Select(log => new { log.LogInTime, log.LogOutTime })
        ////                        .ToList();

        ////                    TimeSpan totalActiveHours = TimeSpan.Zero;
        ////                    TimeSpan breakTime = TimeSpan.Zero;

        ////                    if (onsiteLogs.Any())
        ////                    {
        ////                        var minLogIn = onsiteLogs.Min(log => log.LogInTime);
        ////                        var maxLogOut = onsiteLogs.Max(log => log.LogOutTime);

        ////                        if (minLogIn.HasValue && maxLogOut.HasValue)
        ////                        {
        ////                            TimeSpan totalTime = maxLogOut.Value - minLogIn.Value;

        ////                            foreach (var log in onsiteLogs)
        ////                            {
        ////                                if (log.LogInTime.HasValue && log.LogOutTime.HasValue)
        ////                                {
        ////                                    TimeSpan sessionDuration = log.LogOutTime.Value - log.LogInTime.Value;
        ////                                    if (sessionDuration > TimeSpan.Zero)
        ////                                    {
        ////                                        totalActiveHours += sessionDuration;
        ////                                    }
        ////                                }
        ////                            }

        ////                            breakTime = totalTime - totalActiveHours;
        ////                            if (breakTime < TimeSpan.Zero)
        ////                            {
        ////                                breakTime = TimeSpan.Zero;
        ////                            }
        ////                        }
        ////                    }

        ////                    DateTime startDate1 = startDate; // assign existing startDate to a new variable
        ////                    DateTime endDate1 = endDate;     // assign existing endDate to a new variable

        ////                    // Calculate the difference using the new variables
        ////                    TimeSpan difference = endDate1 - startDate1;

        ////                    // Get total days as decimal
        ////                    decimal totalDays = (decimal)difference.TotalDays;

        ////                    var lopLeaves1 = (from lev in DB.EmpLeaveApplications
        ////                                      where lev.EmpId == emp.EmpId
        ////                                         && lev.LeaveTypeId == 0
        ////                                         && lev.StartDate <= date
        ////                                         && lev.EndDate >= date
        ////                                         && lev.IsActive == true
        ////                                         && lev.IsDeleted == false
        ////                                         && lev.Status.Contains("APPROVED")
        ////                                      orderby lev.StartDate descending
        ////                                      select lev).ToList();

        ////                    var clLeaves1 = (from lev in DB.EmpLeaveApplications
        ////                                     where lev.EmpId == emp.EmpId
        ////                                        && lev.LeaveTypeId == clid
        ////                                        && lev.StartDate <= date
        ////                                         && lev.EndDate >= date
        ////                                        && lev.IsActive == true
        ////                                        && lev.IsDeleted == false
        ////                                        && lev.Status.Contains("APPROVED")
        ////                                     orderby lev.StartDate descending
        ////                                     select lev).ToList();

        ////                    var elLeaves1 = (from lev in DB.EmpLeaveApplications
        ////                                     where lev.EmpId == emp.EmpId
        ////                                        && lev.LeaveTypeId == elid
        ////                                        && lev.StartDate <= date
        ////                                         && lev.EndDate >= date
        ////                                        && lev.IsActive == true
        ////                                        && lev.IsDeleted == false
        ////                                        && lev.Status.Contains("APPROVED")
        ////                                     orderby lev.StartDate descending
        ////                                     select lev).ToList();

        ////                    var rhLeaves1 = (from lev in DB.EmpLeaveApplications
        ////                                     where lev.EmpId == loginId
        ////                                        && lev.LeaveTypeId == rhid
        ////                                        && lev.StartDate <= date
        ////                                         && lev.EndDate >= date
        ////                                        && lev.IsActive == true
        ////                                        && lev.IsDeleted == false
        ////                                        && lev.Status.Contains("APPROVED")
        ////                                     orderby lev.StartDate descending
        ////                                     select lev).ToList();


        ////                    var holiLeaves1 = (from lev in DB.Holidays
        ////                                         where lev.Date <= date
        ////                                            && lev.Date >= date
        ////                                            && lev.Status == "ACTIVE"
        ////                                            && lev.HolidayType.ToUpper() != "RH HOLIDAYS"
        ////                                            && lev.LocationId == locationId
        ////                                       orderby lev.Created_Date descending
        ////                                         select lev).ToList();

        ////                    bool isLopDay = lopLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
        ////                    bool isCLDay = clLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
        ////                    bool isELDay = elLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
        ////                    bool isRHDay = rhLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
        ////                    bool isHoliDay = holiLeaves1.Any(lev => lev.Date <= date && lev.Date >= date);

        ////                    ////decimal? lopDuration = (from lev in DB.EmpLeaveApplications
        ////                    ////                        where lev.EmpId == emp.EmpId
        ////                    ////                           && lev.LeaveTypeId == 0
        ////                    ////                           && lev.StartDate >= startDate
        ////                    ////                           && lev.EndDate <= endDate
        ////                    ////                           && lev.IsActive == true
        ////                    ////                           && lev.IsDeleted == false
        ////                    ////                           && lev.Status.Contains("APPROVED")
        ////                    ////                        select lev.Duration)
        ////                    ////                       .DefaultIfEmpty(0)           // avoid null result
        ////                    ////                       .Sum();

        ////                    decimal? workingdays = 0;

        ////                    if (worktype.Contains("ESSL"))
        ////                    {
        ////                        logInTime = ESSLlogInTime;
        ////                        logOutTime = ESSLlogOutTime;
        ////                    }
        ////                    else if (worktype.ToUpper() == "WFH")
        ////                    {
        ////                        logInTime = WFHlogInTime;
        ////                        logOutTime = WFHlogOutTime;
        ////                    }
        ////                    else if (worktype.ToUpper() == "ONSITE")
        ////                    {
        ////                        logInTime = ONSITElogInTime;
        ////                        logOutTime = ONSITElogOutTime;
        ////                    }//

        ////                    if (manualDetails.ToUpper() == "MANUAL")
        ////                    {
        ////                        logInTime = ESSLlogInTime = WFHlogInTime = ONSITElogInTime = "00:00:00";
        ////                        logOutTime = ESSLlogOutTime = WFHlogOutTime = ONSITElogOutTime = "00:00:00";
        ////                        ESSLactiveHours = WFHactiveHours = ONSITEactiveHours = "00:00:00";
        ////                        activeHours = MANUALactiveHours;
        ////                        worktype = "MANUAL";
        ////                    }

        ////                    AttendanceViewModel avm = new AttendanceViewModel
        ////                    {
        ////                        EmpId = emp.EmpId,
        ////                        EmpCode = emp.EmpCode,
        ////                        EmpName = emp.EmpName.Trim(),
        ////                        LogDate = date,
        ////                        LogInTime = logInTime,
        ////                        LogOutTime = logOutTime,
        ////                        ESSLLogInTime = ESSLlogInTime,
        ////                        ESSLLogOutTime = ESSLlogOutTime,
        ////                        WFHLogInTime = WFHlogInTime,
        ////                        WFHLogOutTime = WFHlogOutTime,
        ////                        ONSITELogInTime = ONSITElogInTime,
        ////                        ONSITELogOutTime = ONSITElogOutTime,
        ////                        WorkingHours = activeHours, //workingHours.ToString(@"hh\:mm\:ss"),
        ////                        CompId = emp.CompId,
        ////                        CompName = emp.CompName,
        ////                        Designation = emp.DesignationName,
        ////                        DeptName = emp.DeptName,
        ////                        DeptId = emp.CategoryId,
        ////                        DesignationId = emp.DesignationId,
        ////                        PayDays = workingdays,
        ////                        LeaveType = isLopDay ? "LOP" : isCLDay ? "CL" : isELDay ? "EL" : isRHDay ? "RH" : isHoliDay ? "Holiday" : "",
        ////                        DaysPresent = 0,
        ////                        ActiveHours = activeHours,
        ////                        ESSLActiveHours = ESSLactiveHours,
        ////                        WFHActiveHours = WFHactiveHours,
        ////                        ONSITEActiveHours = ONSITEactiveHours,
        ////                        ShiftName = empShift?.ShiftName ?? "No Shift",
        ////                        WorkType = worktype,
        ////                        BreakTime = breakTime.ToString(@"hh\:mm\:ss"),
        ////                    };
        ////                    lstOfAtt.Add(avm);
        ////                }

        ////                advm.lstofAttendance = lstOfAtt
        ////                   .OrderBy(att => att.EmpName)
        ////                   //.ThenBy(att => att.EmpId)
        ////                   .ToList();

        ////                lstOfDate.Add(advm);

        ////                var lopLeaves = (from lev in DB.EmpLeaveApplications
        ////                                 where //lev.EmpId == emp.EmpId && 
        ////                                 lev.LeaveTypeId == 0
        ////                                    && lev.StartDate <= endDate
        ////                                    && lev.EndDate >= startDate
        ////                                    && lev.IsActive == true
        ////                                    && lev.IsDeleted == false
        ////                                    && lev.Status.Contains("APPROVED")
        ////                                 orderby lev.StartDate descending
        ////                                 select lev).ToList();

        ////                var clLeaves = (from lev in DB.EmpLeaveApplications
        ////                                where //lev.EmpId == emp.EmpId && 
        ////                                lev.LeaveTypeId == clid
        ////                                   && lev.StartDate <= endDate
        ////                                    && lev.EndDate >= startDate
        ////                                   && lev.IsActive == true
        ////                                   && lev.IsDeleted == false
        ////                                   && lev.Status.Contains("APPROVED")
        ////                                orderby lev.StartDate descending
        ////                                select lev).ToList();

        ////                var elLeaves = (from lev in DB.EmpLeaveApplications
        ////                                where //lev.EmpId == emp.EmpId && 
        ////                                lev.LeaveTypeId == elid
        ////                                   && lev.StartDate <= endDate
        ////                                    && lev.EndDate >= startDate
        ////                                   && lev.IsActive == true
        ////                                   && lev.IsDeleted == false
        ////                                orderby lev.StartDate descending
        ////                                select lev).ToList();

        ////                var rhLeaves = (from lev in DB.EmpLeaveApplications
        ////                                where //lev.EmpId == emp.EmpId && 
        ////                                lev.LeaveTypeId == rhid
        ////                                   && lev.StartDate <= endDate
        ////                                    && lev.EndDate >= startDate
        ////                                   && lev.IsActive == true
        ////                                   && lev.IsDeleted == false
        ////                                   && lev.Status.Contains("APPROVED")
        ////                                orderby lev.StartDate descending
        ////                                select lev).ToList();

        ////                var holiLeaves = (from lev in DB.Holidays
        ////                                    where lev.Date >= startDate
        ////                                       && lev.Date <= endDate
        ////                                       && lev.Status == "ACTIVE"
        ////                                       && lev.HolidayType.ToUpper() != "RH HOLIDAYS"
        ////                                       //&& lev.LocationId == locationId
        ////                                  orderby lev.Created_Date descending
        ////                                  select lev).ToList();


        ////                ////foreach (var day in lstOfDate)
        ////                ////{
        ////                ////    foreach (var empAtt in day.lstofAttendance)
        ////                ////    {
        ////                ////        DateTime attendanceDate = Convert.ToDateTime(day.AttendaceDate);

        ////                ////        bool isLOP = lopLeaves.Any(l =>
        ////                ////            l.EmpId == empAtt.EmpId &&                // ✅ match employee here
        ////                ////            attendanceDate >= Convert.ToDateTime(l.StartDate) &&
        ////                ////            attendanceDate <= Convert.ToDateTime(l.EndDate)
        ////                ////        );

        ////                ////        if (isLOP)
        ////                ////        {
        ////                ////            empAtt.LeaveType = "LOP";
        ////                ////            //empAtt.PayDays = 0;
        ////                ////        }
        ////                ////    }
        ////                ////}

        ////                ////foreach (var day in lstOfDate)
        ////                ////{
        ////                ////    foreach (var emp in day.lstofAttendance)
        ////                ////    {
        ////                ////        TimeSpan workingHours = TimeSpan.Zero;

        ////                ////        if (!string.IsNullOrWhiteSpace(emp.WorkingHours))
        ////                ////            TimeSpan.TryParse(emp.WorkingHours, out workingHours);

        ////                ////        emp.PayDays = CalculatePayDay(workingHours);
        ////                ////        if (emp.PayDays == Convert.ToDecimal(0.5))
        ////                ////        {
        ////                ////            emp.DaysPresent = 1;
        ////                ////        }
        ////                ////    }
        ////                ////}
        ////                ///



        ////                foreach (var day in lstOfDate)
        ////                {


        ////                    foreach (var emp in day.lstofAttendance)
        ////                    {
        ////                        int? locationId = DB.EmployeeMasters.Where(x => x.EmpId == emp.EmpId && x.EmpStatus.ToUpper() == "ACTIVE" && x.IsActive == true && x.IsDeleted == false)
        ////                                        .Select(x => x.LocationId).FirstOrDefault() ?? 0;

        ////                        if (locationId == 0)
        ////                        {
        ////                            locationId = 4;
        ////                        }

        ////                        var holidayDates = holiLeaves.Where(x => x.LocationId == locationId)
        ////                                    .Select(h => h.Date.Date)
        ////                                    .ToHashSet();

        ////                        bool isHoliday = holidayDates.Contains(Convert.ToDateTime(day.AttendaceDate).Date);

        ////                        // ❌ Holiday → No payday
        ////                        if (isHoliday)
        ////                        {
        ////                            emp.PayDays = 0.0m;
        ////                            emp.DaysPresent = 0;
        ////                            continue;
        ////                        }

        ////                        // Normal calculation
        ////                        TimeSpan workingHours = TimeSpan.Zero;

        ////                        if (!string.IsNullOrWhiteSpace(emp.ActiveHours))
        ////                            TimeSpan.TryParse(emp.ActiveHours, out workingHours);

        ////                        emp.PayDays = CalculatePayDay(workingHours);

        ////                        if (emp.PayDays == 0.5m)
        ////                        {
        ////                            emp.DaysPresent = 1;
        ////                        }
        ////                    }
        ////                }

        ////                var empGroups = lstOfDate
        ////                                    .SelectMany(x => x.lstofAttendance)
        ////                                    .GroupBy(x => x.EmpId);

        ////                foreach (var group in empGroups)
        ////                {
        ////                    int? employeeId = group.Key;   // ✅ renamed

        ////                    var empInfo = DB.EmployeeMasters
        ////                                    .Where(e => e.EmpStatus.ToUpper() == "ACTIVE" && e.IsActive == true && e.IsDeleted == false)
        ////                                    .Select(e => new {
        ////                                        e.EmpId,
        ////                                        e.LocationId,
        ////                                        e.JoiningDate
        ////                                    })
        ////                                    .ToDictionary(e => e.EmpId);

        ////                    var emp = empInfo[employeeId.Value];
        ////                    int? locationId = (emp.LocationId == null || emp.LocationId == 0) ? 4 : emp.LocationId.Value;

        ////                    DateTime doj = emp.JoiningDate ?? new DateTime(2000, 1, 1);


        ////                    bool isDojInRange = doj.Date >= startDate.Date && doj.Date <= endDate.Date;

        ////                    //DateTime calculationStartDate = isDojInRange ? doj.Date : startDate.Date;

        ////                    int? dojweekendDaysCount = 0;
        ////                    int? dojsundayCount = 0;

        ////                    if (isDojInRange && doj.Date <= endDate.Date)
        ////                    {
        ////                        for (DateTime d = doj.Date; d <= endDate.Date; d = d.AddDays(1))
        ////                        {
        ////                            if (d.DayOfWeek == DayOfWeek.Sunday)
        ////                            {
        ////                                dojsundayCount++;
        ////                                dojweekendDaysCount++;
        ////                            }
        ////                            else if (d.DayOfWeek == DayOfWeek.Saturday)
        ////                            {
        ////                                dojweekendDaysCount++;
        ////                            }
        ////                        }
        ////                    }


        ////                    // LOP days ONLY for this employee
        ////                    decimal? lopDaysCount = lopLeaves
        ////                        .Where(l => l.EmpId == employeeId)
        ////                        .Sum(l => l.Duration);

        ////                    // CL days ONLY for this employee
        ////                    decimal? clDaysCount = clLeaves
        ////                        .Where(l => l.EmpId == employeeId)
        ////                        .Sum(l => l.Duration);

        ////                    // EL days ONLY for this employee
        ////                    decimal? elDaysCount = elLeaves
        ////                        .Where(l => l.EmpId == employeeId)
        ////                        .Sum(l => l.Duration);

        ////                    // RH days ONLY for this employee
        ////                    decimal? rhDaysCount = rhLeaves
        ////                        .Where(l => l.EmpId == employeeId)
        ////                        .Sum(l => l.Duration);

        ////                    // Holidays days ONLY for this employee
        ////                    decimal? holiDaysCount = holiLeaves.Where(l => l.LocationId == locationId).Count();

        ////                    decimal? clelDaysCount = clDaysCount + elDaysCount; 
        ////                    decimal? holirhDaysCount = rhDaysCount + holiDaysCount;


        ////                    int ? shiftid = DB.EmpShiftDetails
        ////                                        .Where(x => x.EmpId == employeeId && x.IsActive == true && x.IsDeleted == false)
        ////                                        .Select(x => x.ShiftId)
        ////                                        .FirstOrDefault();

        ////                    string days = "";

        ////                    if (shiftid.HasValue && shiftid.Value > 0)
        ////                    {
        ////                        days = DB.ShiftMasters
        ////                                 .Where(x => x.ShiftId == shiftid.Value && x.IsActive == true && x.IsDeleted == false)
        ////                                 .Select(x => x.Days)
        ////                                 .FirstOrDefault() ?? "";
        ////                    }

        ////                    decimal totalPayDays = 0;
        ////                    decimal finalPayDays = 0;

        ////                    if (days == "6")
        ////                    {
        ////                        if (isDojInRange == true)
        ////                        {
        ////                            totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                            if (totalPayDays == 0 && clelDaysCount == 0)
        ////                            {
        ////                                finalPayDays = 0;
        ////                            }
        ////                            else
        ////                            {
        ////                                finalPayDays = totalPayDays + Convert.ToDecimal(dojsundayCount) - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(holirhDaysCount);
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                            if (totalPayDays == 0 && clelDaysCount == 0)
        ////                            {
        ////                                finalPayDays = 0;
        ////                            }
        ////                            else
        ////                            {
        ////                                finalPayDays = totalPayDays + weekdenddayscount1 - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(holirhDaysCount);
        ////                            }

        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        if (isDojInRange == true)
        ////                        {
        ////                            totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                            if (totalPayDays == 0 && clelDaysCount == 0)
        ////                            {
        ////                                finalPayDays = 0;
        ////                            }
        ////                            else
        ////                            {
        ////                                finalPayDays = totalPayDays + Convert.ToDecimal(dojweekendDaysCount) - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(holirhDaysCount);
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                            if (totalPayDays == 0 && clelDaysCount == 0)
        ////                            {
        ////                                finalPayDays = 0;
        ////                            }
        ////                            else
        ////                            {
        ////                                finalPayDays = totalPayDays + weekdenddayscount - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(holirhDaysCount);
        ////                            }
        ////                        }
        ////                    }

        ////                    ////if (days == "6")
        ////                    ////{
        ////                    ////    totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                    ////    finalPayDays = totalPayDays + weekdenddayscount1 - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount);
        ////                    ////}
        ////                    ////else
        ////                    ////{
        ////                    ////    totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                    ////    finalPayDays = totalPayDays + weekdenddayscount - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount);
        ////                    ////}

        ////                    // Assign TOTAL only (do NOT overwrite PayDays)
        ////                    foreach (var record in group)
        ////                    {
        ////                        record.PayDays = finalPayDays;
        ////                        record.clelcount = clelDaysCount;
        ////                        record.holirhcount = holirhDaysCount;
        ////                        record.weekendcount = weekdenddayscount;
        ////                        record.weekendcount1 = weekdenddayscount1;
        ////                        record.dojweekendDaysCount = dojweekendDaysCount;
        ////                        record.dojsundayCount = dojsundayCount;
        ////                        record.totalpaydaycount = totalPayDays;
        ////                        record.lopcount = lopDaysCount;
        ////                    }
        ////                }

        ////            }

        ////            return lstOfDate;
        ////        }
        ////        else
        ////        {
        ////            List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();
        ////            return lstOfDate;
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        ////// ----------------------- 03.02.2026 -- Response time -- Working Properly -- End ------------------------------------------------------------------- //////

        ////// ----------------------- 03.02.2026 -- Reducing Response time -- Changed to SP -- Start ------------------------------------------------------------------- //////
        public List<AttendaceDateViewModel> AttendanceFilter(AttendanceFilterViewModel model)
        {
            try
            {
                // Call stored procedure
                var result = DB.Database.SqlQuery<AttendanceViewModel>(
                                        @"EXEC sp_GetAttendanceReport 
                                    @LoginId, @CompId, @LEId, @BUId, 
                                    @LocId, @DeptId, @DesignationId, @EmpId, 
                                    @StartDate, @EndDate",
                    new SqlParameter("@LoginId", (object)model.LoginId ?? DBNull.Value),
                    new SqlParameter("@CompId", (object)model.CompId ?? DBNull.Value),
                    new SqlParameter("@LEId", (object)model.LEId ?? DBNull.Value),
                    new SqlParameter("@BUId", (object)model.BUId ?? DBNull.Value),
                    new SqlParameter("@LocId", (object)model.LocId ?? DBNull.Value),
                    new SqlParameter("@DeptId", (object)model.DeptId ?? DBNull.Value),
                    new SqlParameter("@DesignationId", (object)model.DesignationId ?? DBNull.Value),
                    new SqlParameter("@EmpId", (object)model.EmpId ?? DBNull.Value),
                    new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                    new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value)
                ).ToList();

                // Group by date (to match your original structure)
                var groupedResult = result
                    .GroupBy(r => r.LogDate.ToString("yyyy-MM-dd"))
                    .Select(g => new AttendaceDateViewModel
                    {
                        AttendaceDate = g.Key,
                        lstofAttendance = g.OrderBy(x => x.EmpName).ToList()
                    })
                    .ToList();

                return groupedResult;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(System.Net.HttpStatusCode.InternalServerError,
                    $"SP Execution Error: {ex.Message}");
            }
        }
        ////// ----------------------- 03.02.2026 -- Reducing Response time -- Changed to SP -- Start ------------------------------------------------------------------- //////
        ////// ----------------------- 23.04.2026 -- Attendance for manager -- Parimala's requirement ------------------------------------------------------------------- //////
        public List<AttendaceDateViewModel> ReportingEmployeeAttendance(AttendanceFilterViewModel model)
        {
            try
            {
                // Call stored procedure
                var result = DB.Database.SqlQuery<AttendanceViewModel>(
                                        @"EXEC sp_GetReporteesAttendanceReport 
                                    @LoginId, @CompId, @LEId, @BUId, 
                                    @LocId, @DeptId, @DesignationId, @EmpId, 
                                    @StartDate, @EndDate",
                    new SqlParameter("@LoginId", (object)model.LoginId ?? DBNull.Value),
                    new SqlParameter("@CompId", (object)model.CompId ?? DBNull.Value),
                    new SqlParameter("@LEId", (object)model.LEId ?? DBNull.Value),
                    new SqlParameter("@BUId", (object)model.BUId ?? DBNull.Value),
                    new SqlParameter("@LocId", (object)model.LocId ?? DBNull.Value),
                    new SqlParameter("@DeptId", (object)model.DeptId ?? DBNull.Value),
                    new SqlParameter("@DesignationId", (object)model.DesignationId ?? DBNull.Value),
                    new SqlParameter("@EmpId", (object)model.EmpId ?? DBNull.Value),
                    new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                    new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value)
                ).ToList();

                // Group by date (to match your original structure)
                var groupedResult = result
                    .GroupBy(r => r.LogDate.ToString("yyyy-MM-dd"))
                    .Select(g => new AttendaceDateViewModel
                    {
                        AttendaceDate = g.Key,
                        lstofAttendance = g.OrderBy(x => x.EmpName).ToList()
                    })
                    .ToList();

                return groupedResult;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(System.Net.HttpStatusCode.InternalServerError,
                    $"SP Execution Error: {ex.Message}");
            }
        }
        ////// ----------------------- 23.04.2026 -- Attendance for manager -- Parimala's requirement ------------------------------------------------------------------- //////
        private static decimal CalculatePayDay(TimeSpan workingHours)
        {
            TimeSpan halfDay = new TimeSpan(4, 30, 0);
            TimeSpan fullDay = new TimeSpan(8, 30, 0);

            if (workingHours >= fullDay)
                return 1.0m;
            else if (workingHours >= halfDay)
                return 0.5m;
            else
                return 0.0m;
        }
        bool IsValid(TimeSpan start, TimeSpan end)
        {
            return start != TimeSpan.Zero && end != TimeSpan.Zero && end > start;
        }
        bool ESSLIsValid(TimeSpan start, TimeSpan end)
        {
            return start != TimeSpan.Zero && end != TimeSpan.Zero;
        }
        // Handles day + night shift
        TimeSpan CalculateDuration(TimeSpan start, TimeSpan end)
        {
            if (end < start) // Night shift
                end += TimeSpan.FromDays(1);

            return end - start;
        }
        TimeSpan GetOverlap(TimeInterval a, TimeInterval b)
        {
            TimeSpan aStart = a.Start;
            TimeSpan aEnd = a.End < a.Start ? a.End + TimeSpan.FromDays(1) : a.End;

            TimeSpan bStart = b.Start;
            TimeSpan bEnd = b.End < b.Start ? b.End + TimeSpan.FromDays(1) : b.End;

            var start = aStart > bStart ? aStart : bStart;
            var end = aEnd < bEnd ? aEnd : bEnd;

            return end > start ? end - start : TimeSpan.Zero;
        }
        //TimeSpan GetOverlap(TimeInterval a, TimeInterval b)
        //{
        //    var start = a.Start > b.Start ? a.Start : b.Start;
        //    var end = a.End < b.End ? a.End : b.End;
        //    return end > start ? end - start : TimeSpan.Zero;
        //}
        //public List<AttendaceDateViewModel> EachEmployeeAttendance(AttendanceFilterViewModel model)
        //{
        //    try
        //    {
        //        int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
        //        DateTime startDate = (model.StartDate != null) ? model.StartDate : DateTime.Today.AddMonths(-1);
        //        DateTime endDate = (model.EndDate != null) ? model.EndDate : DateTime.Today;

        //        startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
        //        endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

        //        var employee = (from emp in DB.EmployeeMasters
        //                        join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
        //                        join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
        //                        join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
        //                        where emp.EmpId == LoginId && emp.IsActive == true && emp.EmpCode.Contains("3DCAD-")
        //                        select new
        //                        {
        //                            emp.EmpId,
        //                            emp.EmpCode,
        //                            EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
        //                            emp.OldEmp_ID,
        //                            emp.CompId,
        //                            CompName = comp.Company,
        //                            DesignationName = des.Designation,
        //                            DeptName = dept.DeptName,
        //                            emp.CategoryId,
        //                            emp.DesignationId
        //                        }).ToList();

        //        if (LoginId != 0)
        //        {

        //            var empDetail = employee.First();

        //            var logInData = DB.Attendances
        //                               .Where(a => a.Type.ToUpper() == "IN" && a.LogID == empDetail.OldEmp_ID
        //                               && a.LogDate >= startDate && a.LogDate <= endDate)
        //                               .Select(a => new { a.LogDate, a.LogTime, a.LogID })
        //                               .ToList();

        //            var logOutData = DB.Attendances
        //                .Where(a => a.Type.ToUpper() == "OUT" && a.LogID == empDetail.OldEmp_ID
        //                && a.LogDate >= startDate && a.LogDate <= endDate)
        //                .Select(a => new { a.LogDate, a.LogTime, a.LogID })
        //                .ToList();

        //            var attendanceTimes = DB.Emp_AttendanceTime
        //                .Where(at => at.LogId == empDetail.OldEmp_ID &&
        //                at.LogDate >= startDate && at.LogDate <= endDate)
        //                .Select(at => new { at.LogDate, at.AttendHours, at.AttendMins, at.AttendSec, at.LogId })
        //                .ToList();

        //            var wfhData = DB.WFHLoginlogs
        //                          .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
        //                          .Select(wfh => new
        //                          {
        //                              wfh.EmpId,
        //                              wfh.Date,
        //                              wfh.IPAddress,
        //                              wfh.LoginTime,
        //                              wfh.LogOutTime,
        //                              wfh.Activehrs,
        //                              wfh.IsLoggedIn,
        //                              wfh.IsLoggedOut
        //                          })
        //                          .ToList();

        //            var onsitedata = DB.OnSiteLoginlogs
        //                            .Where(at => at.LoginDate >= startDate && at.LoginDate <= endDate)
        //                            .Select(at => new { at.EmpId, at.LoginDate, at.LogInTime, at.LogOutTime })
        //                            .ToList();

        //            var shiftDetails = DB.EmpShiftDetails
        //                       .Where(shift => shift.IsActive == true && shift.IsDeleted == false)
        //                       .Select(shift => new
        //                       {
        //                           shift.EmpId,
        //                           shift.ShiftId,
        //                           shift.ShiftName,
        //                           shift.StartDate,
        //                           shift.EndDate,
        //                           shift.IsActive
        //                       })
        //                       .ToList();


        //            List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();

        //            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //            {
        //                AttendaceDateViewModel advm = new AttendaceDateViewModel
        //                {
        //                    AttendaceDate = date.ToString("yyyy-MM-dd")
        //                };

        //                List<AttendanceViewModel> lstOfAtt = new List<AttendanceViewModel>();

        //                foreach (var emp in employee)
        //                {

        //                    var logInEntry = logInData.FirstOrDefault(x => x.LogID == emp.EmpId && x.LogDate == date);
        //                    var logOutEntry = logOutData.FirstOrDefault(x => x.LogID == emp.EmpId && x.LogDate == date);
        //                    var attendanceTimeEntry = attendanceTimes.FirstOrDefault(x => x.LogId == emp.EmpId && x.LogDate == date);
        //                    var wfhEntry = wfhData.FirstOrDefault(x => x.EmpId == emp.EmpId && x.Date == date);
        //                    var empShift = shiftDetails.FirstOrDefault(shift => shift.EmpId == emp.EmpId && date >= shift.StartDate && date <= shift.EndDate);
        //                    string logInTime = logInEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
        //                    string logOutTime = logOutEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
        //                    string activeHours = "00:00:00";
        //                    int wfhDetails = 0;
        //                    int onsite = 0;

        //                    if (logInEntry?.LogTime != null)
        //                    {
        //                        logInTime = ((DateTime)logInEntry.LogTime).ToString("HH:mm:ss");
        //                    }

        //                    if (logOutEntry?.LogTime != null)
        //                    {
        //                        logOutTime = ((DateTime)logOutEntry.LogTime).ToString("HH:mm:ss");
        //                    }


        //                    if (logInTime == "00:00:00" && logOutTime == "00:00:00")
        //                    {
        //                        var WfhEntry = wfhData.FirstOrDefault(x => x.EmpId == emp.OldEmp_ID && x.Date == date.Date);

        //                        if (WfhEntry != null)
        //                        {
        //                            if (wfhEntry.LoginTime.HasValue && wfhEntry.LogOutTime.HasValue)
        //                            {
        //                                logInTime = wfhEntry.LoginTime.Value.ToString(@"hh\:mm\:ss");
        //                                logOutTime = wfhEntry.LogOutTime.Value.ToString(@"hh\:mm\:ss");

        //                                TimeSpan logIn = (TimeSpan)wfhEntry.LoginTime;
        //                                TimeSpan logOut = (TimeSpan)wfhEntry.LogOutTime;
        //                                activeHours = (logOut - logIn > TimeSpan.Zero) ? (logOut - logIn).ToString(@"hh\:mm\:ss") : "00:00:00";
        //                            }
        //                            else
        //                            {
        //                                activeHours = wfhEntry.Activehrs?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        //                            }
        //                            wfhDetails = 1;
        //                        }
        //                        else
        //                        {
        //                            var onsiteEntry = onsitedata.FirstOrDefault(x => x.EmpId == emp.OldEmp_ID && x.LoginDate == date);
        //                            if (onsiteEntry != null)
        //                            {
        //                                if (onsiteEntry.LogInTime.HasValue && onsiteEntry.LogOutTime.HasValue)
        //                                {
        //                                    logInTime = onsiteEntry.LogInTime.Value.ToString(@"hh\:mm\:ss");
        //                                    logOutTime = onsiteEntry.LogOutTime.Value.ToString(@"hh\:mm\:ss");

        //                                    TimeSpan logIn = (TimeSpan)onsiteEntry.LogInTime.Value;
        //                                    TimeSpan logOut = (TimeSpan)onsiteEntry.LogOutTime.Value;
        //                                    activeHours = (logOut - logIn > TimeSpan.Zero) ? (logOut - logIn).ToString(@"hh\:mm\:ss") : "00:00:00";
        //                                }
        //                                onsite = 2;
        //                            }
        //                        }
        //                    }

        //                    if (!string.IsNullOrWhiteSpace(logInTime) && !string.IsNullOrWhiteSpace(logOutTime) &&
        //                        logInTime != "00:00:00" && logOutTime != "00:00:00")
        //                    {
        //                        if (TimeSpan.TryParse(logInTime, out TimeSpan logIn) && TimeSpan.TryParse(logOutTime, out TimeSpan logOut))
        //                        {
        //                            TimeSpan activeDuration = logOut - logIn;
        //                            activeHours = activeDuration > TimeSpan.Zero ? activeDuration.ToString(@"hh\:mm\:ss") : "00:00:00";
        //                        }
        //                    }

        //                    TimeSpan workingHours = TimeSpan.Zero;
        //                    if (DateTime.TryParse(logInTime, out DateTime logInDateTime) && DateTime.TryParse(logOutTime, out DateTime logOutDateTime))
        //                    {
        //                        TimeSpan logIn = logInDateTime.TimeOfDay;
        //                        TimeSpan logOut = logOutDateTime.TimeOfDay;

        //                        workingHours = logOut - logIn;

        //                        if (workingHours < TimeSpan.Zero)
        //                        {
        //                            workingHours = TimeSpan.Zero;
        //                        }
        //                    }

        //                    var onsiteLogs = DB.OnSiteLoginlogs
        //                      .Where(log => log.EmpId == emp.EmpId && log.LoginDate == date)
        //                      .Select(log => new { log.LogInTime, log.LogOutTime })
        //                      .ToList();

        //                    TimeSpan totalActiveHours = TimeSpan.Zero;
        //                    TimeSpan breakTime = TimeSpan.Zero;

        //                    if (onsiteLogs.Any())
        //                    {
        //                        var minLogIn = onsiteLogs.Min(log => log.LogInTime);
        //                        var maxLogOut = onsiteLogs.Max(log => log.LogOutTime);

        //                        if (minLogIn.HasValue && maxLogOut.HasValue)
        //                        {
        //                            TimeSpan totalTime = maxLogOut.Value - minLogIn.Value;

        //                            foreach (var log in onsiteLogs)
        //                            {
        //                                if (log.LogInTime.HasValue && log.LogOutTime.HasValue)
        //                                {
        //                                    TimeSpan sessionDuration = log.LogOutTime.Value - log.LogInTime.Value;
        //                                    if (sessionDuration > TimeSpan.Zero)
        //                                    {
        //                                        totalActiveHours += sessionDuration;
        //                                    }
        //                                }
        //                            }

        //                            breakTime = totalTime - totalActiveHours;
        //                            if (breakTime < TimeSpan.Zero)
        //                            {
        //                                breakTime = TimeSpan.Zero;
        //                            }
        //                        }
        //                    }

        //                    AttendanceViewModel avm = new AttendanceViewModel
        //                    {
        //                        EmpId = emp.EmpId,
        //                        EmpCode = emp.EmpCode,
        //                        EmpName = emp.EmpName.Trim(),
        //                        LogDate = date,
        //                        LogInTime = logInTime,
        //                        LogOutTime = logOutTime,
        //                        WorkingHours = workingHours.ToString(@"hh\:mm\:ss"),
        //                        CompId = emp.CompId,
        //                        CompName = emp.CompName,
        //                        Designation = emp.DesignationName,
        //                        DeptName = emp.DeptName,
        //                        DeptId = emp.CategoryId,
        //                        DesignationId = emp.DesignationId,
        //                        ActiveHours = activeHours,
        //                        ShiftName = empShift?.ShiftName ?? "No Shift",
        //                        WorkType = wfhDetails == 1 ? "WFH" : (onsite == 2 ? "OnSite" : string.Empty),
        //                        BreakTime = breakTime.ToString(@"hh\:mm\:ss"),


        //                    };

        //                    lstOfAtt.Add(avm);
        //                }
        //                advm.lstofAttendance = lstOfAtt
        //                   .OrderBy(att => att.WorkingHours)
        //                   .ThenBy(att => att.EmpId)
        //                   .ToList();

        //                lstOfDate.Add(advm);


        //            }

        //            return lstOfDate;
        //        }
        //        else
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is missing");
        //        }

        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}
        public List<AttendaceDateViewModel> EachEmployeeAttendance(AttendanceFilterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? clid = 0, elid = 0, rhid = 0;


                DateTime today = DateTime.Today;

                DateTime startDate = (model.StartDate != null) ? Convert.ToDateTime(model.StartDate) : new DateTime(today.Year, today.Month, 1);
                DateTime endDate = (model.EndDate != null) ? Convert.ToDateTime(model.EndDate) : today.AddDays(-1);

                startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
                endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

                int weekdenddayscount = 0;
                int weekdenddayscount1 = 0;

                for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    if (date.DayOfWeek == DayOfWeek.Saturday ||
                        date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekdenddayscount++;
                    }
                }

                for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekdenddayscount1++;
                    }
                }

                clid = (from lev in DB.LeaveTypeMasters
                        where lev.ShortName == "CL"
                           && lev.IsActive == true
                           && lev.IsDeleted == false
                        select lev.LeaveTypeId).FirstOrDefault();

                elid = (from lev in DB.LeaveTypeMasters
                        where lev.ShortName == "EL"
                           && lev.IsActive == true
                           && lev.IsDeleted == false
                        select lev.LeaveTypeId).FirstOrDefault();

                rhid = (from lev in DB.LeaveTypeMasters
                        where lev.ShortName == "RH"
                           && lev.IsActive == true
                           && lev.IsDeleted == false
                        select lev.LeaveTypeId).FirstOrDefault();

                var empDetails = (from emp in DB.EmployeeMasters
                                  join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
                                  join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
                                  join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
                                  where emp.EmpId == loginId && emp.IsActive == true && emp.EmpStatus.ToUpper() == "ACTIVE"
                                  select new
                                  {
                                      emp.EmpId,
                                      emp.EmpCode,
                                      EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                                      emp.OldEmp_ID,
                                      emp.CompId,
                                      emp.LEId,
                                      emp.BUId,
                                      emp.LocationId,
                                      CompName = comp.Company,
                                      DesignationName = des.Designation,
                                      DeptName = dept.DeptName,
                                      emp.CategoryId,
                                      emp.DesignationId
                                  }).FirstOrDefault();

                // FIX: Check if empDetails is null immediately and return empty list
                if (empDetails == null)
                {
                    return new List<AttendaceDateViewModel>(); // This will return empty array in response
                }

                string empcode = empDetails.EmpCode.ToUpper();

                var logInData = DB.Attendances
                    .Where(a => a.Type.ToUpper() == "IN" && a.LogDate >= startDate && a.LogDate <= endDate && a.EmpCode.ToUpper() == empcode)
                    .Select(a => new { a.LogID, a.LogDate, a.LogTime, a.EmpCode })
                    .ToList();

                var logOutData = DB.Attendances
                    .Where(a => a.Type.ToUpper() == "OUT" && a.LogDate >= startDate && a.LogDate <= endDate && a.EmpCode.ToUpper() == empcode)
                    .Select(a => new { a.LogID, a.LogDate, a.LogTime, a.EmpCode })
                    .ToList();

                var attendanceTimes = DB.Emp_AttendanceTime
                    .Where(at => at.LogDate >= startDate && at.LogDate <= endDate && at.EmpCode.ToUpper() == empcode)
                    .Select(at => new { at.LogId, at.LogDate, at.Duration, at.AttendHours, at.AttendMins, at.AttendSec, at.EmpCode })
                    .ToList();

                var wfhData = DB.WFHLoginlogs
                           .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate && wfh.EmpCode.ToUpper() == empcode)
                           .Select(wfh => new
                           {
                               wfh.EmpId,
                               wfh.EmpCode,
                               wfh.Date,
                               wfh.IPAddress,
                               wfh.LoginTime,
                               wfh.LogOutTime,
                               wfh.Activehrs,
                               wfh.IsLoggedIn,
                               wfh.IsLoggedOut
                           })
                           .ToList();

                var onsitedata = DB.OnSiteLoginlogs
                                 .Where(at => at.LoginDate >= startDate && at.LoginDate <= endDate && at.EmpCode.ToUpper() == empcode)
                                 .Select(at => new { at.EmpId, at.LoginDate, at.LogInTime, at.LogOutTime, at.ActiveHrs, at.EmpCode })
                                 .ToList();

                var shiftDetails = DB.EmpShiftDetails
                           .Where(shift => shift.IsActive == true && shift.IsDeleted == false && shift.EmpCode.ToUpper() == empcode)
                           .Select(shift => new
                           {
                               shift.EmpId,
                               shift.EmpCode,
                               shift.ShiftId,
                               shift.ShiftName,
                               shift.StartDate,
                               shift.EndDate,
                               shift.IsActive
                           })
                           .ToList();

                List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    AttendaceDateViewModel advm = new AttendaceDateViewModel
                    {
                        AttendaceDate = date.ToString("yyyy-MM-dd")
                    };

                    List<AttendanceViewModel> lstOfAtt = new List<AttendanceViewModel>();

                    var logInEntry = logInData.FirstOrDefault(x => x.EmpCode.ToUpper() == empcode && x.LogDate == date);
                    var logOutEntry = logOutData.FirstOrDefault(x => x.EmpCode.ToUpper() == empcode && x.LogDate == date);
                    var attendanceTimeEntry = attendanceTimes.Where(x => x.EmpCode.ToUpper() == empcode && x.LogDate == date).OrderByDescending(x => x.AttendHours).FirstOrDefault();
                    var empShift = shiftDetails.FirstOrDefault(shift => shift.EmpCode.ToUpper() == empcode && date >= shift.StartDate && date <= shift.EndDate);

                    int? locationId = DB.EmployeeMasters.Where(x => x.EmpId == empDetails.EmpId && x.EmpStatus.ToUpper() == "ACTIVE" && x.IsActive == true && x.IsDeleted == false)
                                        .Select(x => x.LocationId).FirstOrDefault() ?? 0;

                    string ESSLlogInTime = logInEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
                    string ESSLlogOutTime = logOutEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
                    string logInTime = "00:00:00";
                    string logOutTime = "00:00:00";
                    string WFHlogInTime = "00:00:00";
                    string WFHlogOutTime = "00:00:00";
                    string ONSITElogInTime = "00:00:00";
                    string ONSITElogOutTime = "00:00:00";
                    string activeHours = "00:00:00";
                    string ESSLactiveHours = "00:00:00";
                    string WFHactiveHours = "00:00:00";
                    string ONSITEactiveHours = "00:00:00";
                    string wfhDetails = "";
                    string onsiteDetails = "";
                    string esslDetails = "";

                    if (logInEntry?.LogTime != null)
                    {
                        ESSLlogInTime = ((DateTime)logInEntry.LogTime).ToString("HH:mm:ss");
                    }

                    if (logOutEntry?.LogTime != null)
                    {
                        ESSLlogOutTime = ((DateTime)logOutEntry.LogTime).ToString("HH:mm:ss");
                    }

                    var wfhEntries = wfhData
                                            .Where(x => x.EmpCode.ToUpper() == empcode && x.Date == date.Date)
                                            .OrderBy(x => x.LoginTime)
                                            .ToList();

                    TimeSpan totalWfhActiveHours = TimeSpan.Zero;
                    TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

                    TimeSpan? firstLogin = null;
                    TimeSpan? lastLogout = null;

                    if (wfhEntries.Any())
                    {
                        for (int i = 0; i < wfhEntries.Count; i++)
                        {
                            var entry = wfhEntries[i];

                            if (!entry.LoginTime.HasValue)
                                continue;

                            TimeSpan logIn = entry.LoginTime.Value;
                            TimeSpan logOut;

                            if (!firstLogin.HasValue || logIn < firstLogin)
                                firstLogin = logIn;

                            if (entry.LogOutTime.HasValue)
                            {
                                logOut = entry.LogOutTime.Value;
                            }
                            else if (i + 1 < wfhEntries.Count && wfhEntries[i + 1].LoginTime.HasValue)
                            {
                                logOut = wfhEntries[i + 1].LoginTime.Value;
                            }
                            else
                            {
                                logOut = defaultLogout;
                            }

                            if (!lastLogout.HasValue || logOut > lastLogout)
                                lastLogout = logOut;

                            if (logOut > logIn)
                                totalWfhActiveHours += (logOut - logIn);
                        }

                        WFHlogInTime = firstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                        WFHlogOutTime = lastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                        WFHactiveHours = totalWfhActiveHours.ToString(@"hh\:mm\:ss");
                        wfhDetails = "WFH";
                    }

                    var onsiteEntry = onsitedata
                                           .Where(x => x.EmpCode.ToUpper() == empcode && x.LoginDate == date.Date)
                                           .OrderBy(x => x.LogInTime)
                                           .ToList();

                    TimeSpan totalOnsiteActiveHours = TimeSpan.Zero;
                    TimeSpan OnsitedefaultLogout = new TimeSpan(18, 36, 0);

                    TimeSpan? OnsitefirstLogin = null;
                    TimeSpan? OnsitelastLogout = null;

                    if (onsiteEntry.Any())
                    {
                        for (int i = 0; i < onsiteEntry.Count; i++)
                        {
                            var entry = onsiteEntry[i];

                            if (!entry.LogInTime.HasValue)
                                continue;

                            TimeSpan logIn = entry.LogInTime.Value;
                            TimeSpan logOut;

                            if (!OnsitefirstLogin.HasValue || logIn < OnsitefirstLogin)
                                OnsitefirstLogin = logIn;

                            if (entry.LogOutTime.HasValue)
                            {
                                logOut = entry.LogOutTime.Value;
                            }
                            else if (i + 1 < onsiteEntry.Count && onsiteEntry[i + 1].LogInTime.HasValue)
                            {
                                logOut = onsiteEntry[i + 1].LogInTime.Value;
                            }
                            else
                            {
                                logOut = OnsitedefaultLogout;
                            }

                            if (!OnsitelastLogout.HasValue || logOut > OnsitelastLogout)
                                OnsitelastLogout = logOut;

                            if (logOut > logIn)
                                totalOnsiteActiveHours += (logOut - logIn);
                        }

                        ONSITElogInTime = OnsitefirstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                        ONSITElogOutTime = OnsitelastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
                        ONSITEactiveHours = totalOnsiteActiveHours.ToString(@"hh\:mm\:ss");
                        onsiteDetails = "ONSITE";
                    }

                    if (attendanceTimeEntry != null)
                    {
                        if (attendanceTimeEntry.Duration.HasValue)
                        {
                            ESSLactiveHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
                            if (ESSLactiveHours != "00:00:00")
                            {
                                esslDetails = "ESSL";
                            }
                        }
                    }

                    // Active Hrs calculation
                    TimeSpan esslLogin = TimeSpan.Parse(ESSLlogInTime);
                    TimeSpan esslLogout = TimeSpan.Parse(ESSLlogOutTime);
                    TimeSpan wfhLogin = TimeSpan.Parse(WFHlogInTime);
                    TimeSpan wfhLogout = TimeSpan.Parse(WFHlogOutTime);
                    TimeSpan onsiteLogin = TimeSpan.Parse(ONSITElogInTime);
                    TimeSpan onsiteLogout = TimeSpan.Parse(ONSITElogOutTime);

                    List<TimeInterval> intervals = new List<TimeInterval>();

                    TimeSpan total = TimeSpan.Zero;

                    if (ESSLIsValid(esslLogin, esslLogout))
                    {
                        intervals.Add(new TimeInterval { Start = esslLogin, End = esslLogout });
                        total += TimeSpan.Parse(ESSLactiveHours);
                    }

                    if (IsValid(wfhLogin, wfhLogout))
                    {
                        TimeInterval wfh = new TimeInterval { Start = wfhLogin, End = wfhLogout };

                        TimeSpan overlap = TimeSpan.Zero;
                        foreach (var i in intervals)
                            overlap += GetOverlap(i, wfh);

                        total += (wfh.End - wfh.Start - overlap);
                        intervals.Add(wfh);
                    }

                    if (ESSLIsValid(onsiteLogin, onsiteLogout))
                    {
                        TimeInterval onsite = new TimeInterval { Start = onsiteLogin, End = onsiteLogout };
                        TimeSpan onsiteDuration = CalculateDuration(onsite.Start, onsite.End);

                        TimeSpan overlap = TimeSpan.Zero;
                        foreach (var i in intervals)
                        {
                            overlap += GetOverlap(i, onsite);
                        }

                        total += onsiteDuration - overlap;
                    }

                    activeHours = total.ToString(@"hh\:mm\:ss");

                    string worktype = "";

                    if (!string.IsNullOrEmpty(esslDetails))
                        worktype += esslDetails;

                    if (!string.IsNullOrEmpty(wfhDetails))
                        worktype += (worktype == "" ? "" : " + ") + wfhDetails;

                    if (!string.IsNullOrEmpty(onsiteDetails))
                        worktype += (worktype == "" ? "" : " + ") + onsiteDetails;

                    TimeSpan workingHours = TimeSpan.Zero;
                    if (DateTime.TryParse(logInTime, out DateTime logInDateTime) && DateTime.TryParse(logOutTime, out DateTime logOutDateTime))
                    {
                        TimeSpan logIn = logInDateTime.TimeOfDay;
                        TimeSpan logOut = logOutDateTime.TimeOfDay;

                        workingHours = logOut - logIn;

                        if (workingHours < TimeSpan.Zero)
                        {
                            workingHours = TimeSpan.Zero;
                        }
                    }

                    var onsiteLogs = DB.Loginlogs
                        .Where(log => log.EmpId == loginId && log.LoginDate == date)
                        .Select(log => new { log.LogInTime, log.LogOutTime })
                        .ToList();

                    TimeSpan totalActiveHours = TimeSpan.Zero;
                    TimeSpan breakTime = TimeSpan.Zero;

                    if (onsiteLogs.Any())
                    {
                        var minLogIn = onsiteLogs.Min(log => log.LogInTime);
                        var maxLogOut = onsiteLogs.Max(log => log.LogOutTime);

                        if (minLogIn.HasValue && maxLogOut.HasValue)
                        {
                            TimeSpan totalTime = maxLogOut.Value - minLogIn.Value;

                            foreach (var log in onsiteLogs)
                            {
                                if (log.LogInTime.HasValue && log.LogOutTime.HasValue)
                                {
                                    TimeSpan sessionDuration = log.LogOutTime.Value - log.LogInTime.Value;
                                    if (sessionDuration > TimeSpan.Zero)
                                    {
                                        totalActiveHours += sessionDuration;
                                    }
                                }
                            }

                            breakTime = totalTime - totalActiveHours;
                            if (breakTime < TimeSpan.Zero)
                            {
                                breakTime = TimeSpan.Zero;
                            }
                        }
                    }

                    DateTime startDate1 = startDate;
                    DateTime endDate1 = endDate;

                    TimeSpan difference = endDate1 - startDate1;
                    decimal totalDays = (decimal)difference.TotalDays;

                    var lopLeaves1 = (from lev in DB.EmpLeaveApplications
                                      where lev.EmpId == loginId
                                         && lev.LeaveTypeId == 0
                                         && lev.StartDate <= date
                                         && lev.EndDate >= date
                                         && lev.IsActive == true
                                         && lev.IsDeleted == false
                                         && lev.Status.Contains("APPROVED")
                                      orderby lev.StartDate descending
                                      select lev).ToList();

                    var clLeaves1 = (from lev in DB.EmpLeaveApplications
                                     where lev.EmpId == loginId
                                        && lev.LeaveTypeId == clid
                                        && lev.StartDate <= date
                                         && lev.EndDate >= date
                                        && lev.IsActive == true
                                        && lev.IsDeleted == false
                                        && lev.Status.Contains("APPROVED")
                                     orderby lev.StartDate descending
                                     select lev).ToList();

                    var elLeaves1 = (from lev in DB.EmpLeaveApplications
                                     where lev.EmpId == loginId
                                        && lev.LeaveTypeId == elid
                                        && lev.StartDate <= date
                                         && lev.EndDate >= date
                                        && lev.IsActive == true
                                        && lev.IsDeleted == false
                                        && lev.Status.Contains("APPROVED")
                                     orderby lev.StartDate descending
                                     select lev).ToList();

                    var rhLeaves1 = (from lev in DB.EmpLeaveApplications
                                     where lev.EmpId == loginId
                                        && lev.LeaveTypeId == rhid
                                        && lev.StartDate <= date
                                         && lev.EndDate >= date
                                        && lev.IsActive == true
                                        && lev.IsDeleted == false
                                        && lev.Status.Contains("APPROVED")
                                     orderby lev.StartDate descending
                                     select lev).ToList();

                    var holiLeaves1 = (from lev in DB.Holidays
                                       where lev.Date <= date
                                          && lev.Date >= date
                                          && lev.Status == "ACTIVE"
                                          && lev.HolidayType.ToUpper() != "RH HOLIDAYS"
                                          && lev.LocationId == locationId
                                       orderby lev.Created_Date descending
                                       select lev).ToList();

                    bool isLopDay = lopLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
                    bool isCLDay = clLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
                    bool isELDay = elLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
                    bool isRHDay = rhLeaves1.Any(lev => lev.StartDate <= date && lev.EndDate >= date);
                    bool isHoliDay = holiLeaves1.Any(lev => lev.Date <= date && lev.Date >= date);

                    decimal? workingdays = 0;

                    if (worktype.Contains("ESSL"))
                    {
                        logInTime = ESSLlogInTime;
                        logOutTime = ESSLlogOutTime;
                    }
                    //else if (worktype.ToUpper() == "WFH")
                    //{
                    //    logInTime = WFHlogInTime;
                    //    logOutTime = WFHlogOutTime;
                    //}
                    //else if (worktype.ToUpper() == "ONSITE")
                    //{
                    //    logInTime = ONSITElogInTime;
                    //    logOutTime = ONSITElogOutTime;
                    //}
                    else if (worktype.Contains("WFH"))
                    {
                        logInTime = WFHlogInTime;
                        logOutTime = WFHlogOutTime;
                    }
                    else if (worktype.Contains("ONSITE"))
                    {
                        logInTime = ONSITElogInTime;
                        logOutTime = ONSITElogOutTime;
                    }
                    AttendanceViewModel avm = new AttendanceViewModel
                    {
                        EmpId = loginId,
                        EmpCode = empcode,
                        EmpName = empDetails.EmpName.Trim(),
                        LogDate = date,
                        LogInTime = logInTime,
                        LogOutTime = logOutTime,
                        ESSLLogInTime = ESSLlogInTime,
                        ESSLLogOutTime = ESSLlogOutTime,
                        WFHLogInTime = WFHlogInTime,
                        WFHLogOutTime = WFHlogOutTime,
                        ONSITELogInTime = ONSITElogInTime,
                        ONSITELogOutTime = ONSITElogOutTime,
                        WorkingHours = activeHours,
                        CompId = empDetails.CompId,
                        CompName = empDetails.CompName,
                        Designation = empDetails.DesignationName,
                        DeptName = empDetails.DeptName,
                        DeptId = empDetails.CategoryId,
                        DesignationId = empDetails.DesignationId,
                        PayDays = workingdays,
                        LeaveType = isLopDay ? "LOP" : isCLDay ? "CL" : isELDay ? "EL" : isRHDay ? "RH" : isHoliDay ? "Holiday" : "",
                        DaysPresent = 0,
                        ActiveHours = activeHours,
                        ESSLActiveHours = ESSLactiveHours,
                        WFHActiveHours = WFHactiveHours,
                        ONSITEActiveHours = ONSITEactiveHours,
                        ShiftName = empShift?.ShiftName ?? "No Shift",
                        WorkType = worktype,
                        BreakTime = breakTime.ToString(@"hh\:mm\:ss"),
                    };
                    lstOfAtt.Add(avm);

                    advm.lstofAttendance = lstOfAtt
                       .OrderBy(att => att.EmpName)
                       .ToList();

                    lstOfDate.Add(advm);

                    var lopLeaves = (from lev in DB.EmpLeaveApplications
                                     where lev.LeaveTypeId == 0
                                        && lev.StartDate <= endDate
                                        && lev.EndDate >= startDate
                                        && lev.IsActive == true
                                        && lev.IsDeleted == false
                                        && lev.Status.Contains("APPROVED")
                                     orderby lev.StartDate descending
                                     select lev).ToList();

                    var clLeaves = (from lev in DB.EmpLeaveApplications
                                    where lev.LeaveTypeId == clid
                                       && lev.StartDate <= endDate
                                        && lev.EndDate >= startDate
                                       && lev.IsActive == true
                                       && lev.IsDeleted == false
                                       && lev.Status.Contains("APPROVED")
                                    orderby lev.StartDate descending
                                    select lev).ToList();

                    var elLeaves = (from lev in DB.EmpLeaveApplications
                                    where lev.LeaveTypeId == elid
                                       && lev.StartDate <= endDate
                                        && lev.EndDate >= startDate
                                       && lev.IsActive == true
                                       && lev.IsDeleted == false
                                       && lev.Status.Contains("APPROVED")
                                    orderby lev.StartDate descending
                                    select lev).ToList();

                    var rhLeaves = (from lev in DB.EmpLeaveApplications
                                    where lev.LeaveTypeId == rhid
                                       && lev.StartDate <= endDate
                                        && lev.EndDate >= startDate
                                       && lev.IsActive == true
                                       && lev.IsDeleted == false
                                       && lev.Status.Contains("APPROVED")
                                    orderby lev.StartDate descending
                                    select lev).ToList();

                    var holiLeaves = (from lev in DB.Holidays
                                      where lev.Date >= startDate
                                         && lev.Date <= endDate
                                         && lev.Status == "ACTIVE"
                                         && lev.HolidayType.ToUpper() != "RH HOLIDAYS"
                                      orderby lev.Created_Date descending
                                      select lev).ToList();

                    var holidayDates = holiLeaves
                                        .Select(h => h.Date.Date)
                                        .ToHashSet();

                    foreach (var day in lstOfDate)
                    {
                        bool isHoliday = holidayDates.Contains(Convert.ToDateTime(day.AttendaceDate).Date);

                        foreach (var emp in day.lstofAttendance)
                        {
                            if (isHoliday)
                            {
                                emp.PayDays = 0.0m;
                                emp.DaysPresent = 0;
                                continue;
                            }

                            TimeSpan workingHours1 = TimeSpan.Zero;

                            if (!string.IsNullOrWhiteSpace(emp.WorkingHours))
                                TimeSpan.TryParse(emp.WorkingHours, out workingHours1);

                            emp.PayDays = CalculatePayDay(workingHours1);

                            if (emp.PayDays == 0.5m)
                            {
                                emp.DaysPresent = 1;
                            }
                        }
                    }

                    var empGroups = lstOfDate
                                        .SelectMany(x => x.lstofAttendance)
                                        .GroupBy(x => x.EmpId);

                    foreach (var group in empGroups)
                    {
                        int? employeeId = group.Key;

                        locationId = DB.EmployeeMasters.Where(x => x.EmpId == employeeId && x.EmpStatus.ToUpper() == "ACTIVE" && x.IsActive == true && x.IsDeleted == false)
                                            .Select(x => x.LocationId).FirstOrDefault() ?? 0;

                        if (locationId == 0)
                        { locationId = 4; }

                        DateTime doj = DB.EmployeeMasters.Where(x => x.EmpId == employeeId && x.EmpStatus.ToUpper() == "ACTIVE" && x.IsActive == true && x.IsDeleted == false)
                                                    .Select(x => x.JoiningDate).FirstOrDefault() ?? new DateTime(2000, 1, 1);

                        bool isDojInRange = doj.Date >= startDate.Date && doj.Date <= endDate.Date;

                        int? dojweekendDaysCount = 0;
                        int? dojsundayCount = 0;

                        if (isDojInRange == true)
                        {
                            for (DateTime dojdate = doj.Date; dojdate <= endDate.Date; dojdate = dojdate.AddDays(1))
                            {
                                if (dojdate.DayOfWeek == DayOfWeek.Saturday ||
                                    dojdate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dojweekendDaysCount++;
                                }

                                if (dojdate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dojsundayCount++;
                                }
                            }
                        }

                        decimal? lopDaysCount = lopLeaves
                            .Where(l => l.EmpId == employeeId)
                            .Sum(l => l.Duration);

                        decimal? clDaysCount = clLeaves
                            .Where(l => l.EmpId == employeeId)
                            .Sum(l => l.Duration);

                        decimal? elDaysCount = elLeaves
                            .Where(l => l.EmpId == employeeId)
                            .Sum(l => l.Duration);

                        decimal? rhDaysCount = rhLeaves
                            .Where(l => l.EmpId == employeeId)
                            .Sum(l => l.Duration);

                        decimal? holiDaysCount = holiLeaves.Where(l => l.LocationId == locationId).Count();

                        decimal? clelDaysCount = clDaysCount + elDaysCount;
                        decimal? HoliDaysCount = rhDaysCount + holiDaysCount;

                        int? shiftid = DB.EmpShiftDetails
                                            .Where(x => x.EmpId == employeeId && x.IsActive == true && x.IsDeleted == false)
                                            .Select(x => x.ShiftId)
                                            .FirstOrDefault();

                        string days = "";

                        if (shiftid.HasValue && shiftid.Value > 0)
                        {
                            days = DB.ShiftMasters
                                     .Where(x => x.ShiftId == shiftid.Value && x.IsActive == true && x.IsDeleted == false)
                                     .Select(x => x.Days)
                                     .FirstOrDefault() ?? "";
                        }

                        decimal totalPayDays = 0;
                        decimal finalPayDays = 0;

                        if (days == "6")
                        {
                            if (isDojInRange == true)
                            {
                                totalPayDays = group.Sum(x => x.PayDays ?? 0);
                                if (totalPayDays == 0 && clelDaysCount == 0)
                                {
                                    finalPayDays = 0;
                                }
                                else
                                {
                                    finalPayDays = totalPayDays + Convert.ToDecimal(dojsundayCount) - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(HoliDaysCount);
                                }
                            }
                            else
                            {
                                totalPayDays = group.Sum(x => x.PayDays ?? 0);
                                if (totalPayDays == 0 && clelDaysCount == 0)
                                {
                                    finalPayDays = 0;
                                }
                                else
                                {
                                    finalPayDays = totalPayDays + weekdenddayscount1 - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(HoliDaysCount);
                                }
                            }
                        }
                        else
                        {
                            if (isDojInRange == true)
                            {
                                totalPayDays = group.Sum(x => x.PayDays ?? 0);
                                if (totalPayDays == 0 && clelDaysCount == 0)
                                {
                                    finalPayDays = 0;
                                }
                                else
                                {
                                    finalPayDays = totalPayDays + Convert.ToDecimal(dojweekendDaysCount) - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(HoliDaysCount);
                                }
                            }
                            else
                            {
                                totalPayDays = group.Sum(x => x.PayDays ?? 0);
                                if (totalPayDays == 0 && clelDaysCount == 0)
                                {
                                    finalPayDays = 0;
                                }
                                else
                                {
                                    finalPayDays = totalPayDays + weekdenddayscount - Convert.ToDecimal(lopDaysCount) + Convert.ToDecimal(clelDaysCount) + Convert.ToDecimal(HoliDaysCount);
                                }
                            }
                        }

                        foreach (var record in group)
                        {
                            record.PayDays = finalPayDays;
                        }
                    }
                }

                // Check if we have any data to return
                if (lstOfDate == null || !lstOfDate.Any() || !lstOfDate.Any(x => x.lstofAttendance != null && x.lstofAttendance.Any()))
                {
                    return new List<AttendaceDateViewModel>(); // Return empty list if no attendance data
                }

                return lstOfDate.OrderByDescending(x => x.AttendaceDate).ToList();
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging
                // Return empty list instead of throwing 500 error
                return new List<AttendaceDateViewModel>();
            }
        }



        /////------------------ 23.04.2026 Parimala's req - ----- Fro manager -----------------------------------
        ////public List<AttendaceDateViewModel> ReportingEmployeeAttendance(AttendanceFilterViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;

        ////        var Empdetails = (from user in DB.EmployeeMasters
        ////                          where user.EmpId == LoginId && user.EmpStatus.ToUpper() == "ACTIVE" && user.IsActive == true && user.IsDeleted == false
        ////                          select user).ToList();

        ////        if (Empdetails.Count() != 0)
        ////        {
        ////            int? reportid = Empdetails[0].ReportId;
        ////            int? OldEmp_ID = Empdetails[0].OldEmp_ID;

        ////            var AuthorisedEmp = (from user in DB.EmployeeMasters
        ////                                 join comp in DB.CompanyMasters on user.CompId equals comp.CompId
        ////                                 where (user.ReportId == OldEmp_ID || user.ReportId == LoginId) && user.EmpStatus.ToUpper() == "ACTIVE" && user.IsActive == true && user.IsDeleted == false
        ////                                 select new
        ////                                 {
        ////                                     user.EmpId,
        ////                                     user.EmpCode,
        ////                                     user.FirstName,
        ////                                     user.MiddleName,
        ////                                     user.LastName,
        ////                                     user.CompId,
        ////                                     comp.Company,
        ////                                     user.DesignationName,
        ////                                     user.DeptName,
        ////                                     user.CategoryId,
        ////                                     user.DesignationId,
        ////                                     user.OldEmp_ID
        ////                                 }).ToList().OrderBy(x => x.FirstName).ThenBy(x => x.EmpCode).ToList();

        ////            if (LoginId == 603)
        ////            {
        ////                AuthorisedEmp = (from user in DB.EmployeeMasters
        ////                                 join comp in DB.CompanyMasters on user.CompId equals comp.CompId
        ////                                 where user.CompId == 1 && user.LEId == 2 && user.EmpStatus.ToUpper() == "ACTIVE" && user.IsActive == true && user.IsDeleted == false
        ////                                 select new
        ////                                 {
        ////                                     user.EmpId,
        ////                                     user.EmpCode,
        ////                                     user.FirstName,
        ////                                     user.MiddleName,
        ////                                     user.LastName,
        ////                                     user.CompId,
        ////                                     comp.Company,
        ////                                     user.DesignationName,
        ////                                     user.DeptName,
        ////                                     user.CategoryId,
        ////                                     user.DesignationId,
        ////                                     user.OldEmp_ID
        ////                                 }).ToList().OrderBy(x => x.FirstName).ThenBy(x => x.EmpCode).ToList();
        ////            }
        ////            else if (LoginId == 149)
        ////            {
        ////                AuthorisedEmp = (from user in DB.EmployeeMasters
        ////                                 join comp in DB.CompanyMasters on user.CompId equals comp.CompId
        ////                                 where user.CompId != 0 && user.EmpStatus.ToUpper() == "ACTIVE" && user.IsActive == true && user.IsDeleted == false
        ////                                 select new
        ////                                 {
        ////                                     user.EmpId,
        ////                                     user.EmpCode,
        ////                                     user.FirstName,
        ////                                     user.MiddleName,
        ////                                     user.LastName,
        ////                                     user.CompId,
        ////                                     comp.Company,
        ////                                     user.DesignationName,
        ////                                     user.DeptName,
        ////                                     user.CategoryId,
        ////                                     user.DesignationId,
        ////                                     user.OldEmp_ID
        ////                                 }).ToList().OrderBy(x => x.FirstName).ThenBy(x => x.EmpCode).ToList();
        ////            }
        ////            DateTime startDate = (model.StartDate != null) ? Convert.ToDateTime(model.StartDate) : DateTime.Today.AddMonths(-1);
        ////            DateTime endDate = (model.EndDate != null) ? Convert.ToDateTime(model.EndDate) : DateTime.Today;

        ////            startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
        ////            endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

        ////            int weekdenddayscount = 0;

        ////            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        ////            {
        ////                if (date.DayOfWeek == DayOfWeek.Saturday ||
        ////                    date.DayOfWeek == DayOfWeek.Sunday)
        ////                {
        ////                    weekdenddayscount++;
        ////                }
        ////            }

        ////            List<AttendaceDateViewModel> lstOfDate = new List<AttendaceDateViewModel>();

        ////            if (LoginId != 0)
        ////            {
        ////                var logInData = DB.Attendances
        ////                   .Where(a => a.Type.ToUpper() == "IN" && a.LogDate >= startDate && a.LogDate <= endDate)
        ////                   .Select(a => new { a.LogID, a.EmpCode, a.LogDate, a.LogTime })
        ////                   .ToList();

        ////                var logOutData = DB.Attendances
        ////                    .Where(a => a.Type.ToUpper() == "OUT" && a.LogDate >= startDate && a.LogDate <= endDate)
        ////                    .Select(a => new { a.LogID, a.EmpCode, a.LogDate, a.LogTime })
        ////                    .ToList();

        ////                var attendanceTimes = DB.Emp_AttendanceTime
        ////                    .Where(at => at.LogDate >= startDate && at.LogDate <= endDate)
        ////                    .Select(at => new { at.LogId, at.LogDate, at.EmpCode, at.Duration, at.AttendHours, at.AttendMins, at.AttendSec })
        ////                    .ToList();



        ////                ////var wfhData = DB.WFHLoginlogs
        ////                ////   .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
        ////                ////   .Select(wfh => new AttWFHLoginlogViewModel
        ////                ////   { 
        ////                ////       wfh.EmpId,
        ////                ////       wfh.EmpCode,
        ////                ////       wfh.Date,
        ////                ////       wfh.IPAddress,
        ////                ////       wfh.LoginTime,
        ////                ////       wfh.LogOutTime,
        ////                ////       wfh.Activehrs,
        ////                ////       wfh.IsLoggedIn,
        ////                ////       wfh.IsLoggedOut
        ////                ////   })
        ////                ////   .ToList();

        ////                var wfhData = DB.WFHLoginlogs
        ////                                .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
        ////                                .Select(wfh => new AttWFHLoginlogViewModel
        ////                                {
        ////                                    EmpId = wfh.EmpId,
        ////                                    EmpCode = wfh.EmpCode,
        ////                                    Date = wfh.Date,
        ////                                    IPAddress = wfh.IPAddress,
        ////                                    LoginTime = wfh.LoginTime,
        ////                                    //LogOutTime = wfh.LogOutTime ?? new TimeSpan(6, 32, 32),
        ////                                    LogOutTime = wfh.LogOutTime,
        ////                                    Activehrs = wfh.Activehrs,
        ////                                    IsLoggedIn = wfh.IsLoggedIn,
        ////                                    IsLoggedOut = wfh.IsLoggedOut
        ////                                })
        ////                                .ToList();

        ////                ////var onsitedata = DB.Loginlogs
        ////                ////        .Where(at => at.LoginDate >= startDate && at.LoginDate <= endDate)
        ////                ////        .Select(at => new { at.EmpId, at.EmpCode, at.LoginDate, at.LogInTime, at.LogOutTime })
        ////                ////        .ToList();

        ////                var onsitedata = DB.OnSiteLoginlogs
        ////                        .Where(at => at.LoginDate >= startDate && at.LoginDate <= endDate)
        ////                        .Select(at => new AttLoginlogViewModel 
        ////                        { 
        ////                            EmpId = at.EmpId,
        ////                            EmpCode = at.EmpCode,
        ////                            LoginDate = at.LoginDate,
        ////                            LogInTime = at.LogInTime,
        ////                            LogOutTime = at.LogOutTime,
        ////                            ActiveHrs = at.ActiveHrs
        ////                        })
        ////                        .ToList();


        ////                var shiftDetails = DB.EmpShiftDetails
        ////                           .Where(shift => shift.IsActive == true && shift.IsDeleted == false)
        ////                           .Select(shift => new
        ////                           {
        ////                               shift.EmpId,
        ////                               shift.ShiftId,
        ////                               shift.ShiftName,
        ////                               shift.StartDate,
        ////                               shift.EndDate,
        ////                               shift.IsActive
        ////                           })
        ////                           .ToList();

        ////                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        ////                {
        ////                    AttendaceDateViewModel advm = new AttendaceDateViewModel
        ////                    {
        ////                        AttendaceDate = date.ToString("yyyy-MM-dd")
        ////                    };
        ////                    List<AttendanceViewModel> lstOfAtt = new List<AttendanceViewModel>();

        ////                    foreach (var emp in AuthorisedEmp)
        ////                    {
        ////                        var logInEntry = logInData.FirstOrDefault(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LogDate == date);
        ////                        var logOutEntry = logOutData.FirstOrDefault(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LogDate == date);
        ////                        var attendanceTimeEntry = attendanceTimes.FirstOrDefault(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LogDate == date);
        ////                        var wfhEntry = wfhData.FirstOrDefault(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.Date == date);

        ////                        var empShift = shiftDetails.FirstOrDefault(shift => shift.EmpId == emp.EmpId && date >= shift.StartDate && date <= shift.EndDate);

        ////                        string logInTime = logInEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
        ////                        string logOutTime = logOutEntry?.LogTime?.ToString("HH:mm:ss") ?? "00:00:00";
        ////                        string activeHours = "00:00:00";
        ////                        int wfhDetails = 0;
        ////                        int onsite = 0;

        ////                        if (logInEntry?.LogTime != null)
        ////                        {
        ////                            logInTime = ((DateTime)logInEntry.LogTime).ToString("HH:mm:ss");
        ////                        }

        ////                        if (logOutEntry?.LogTime != null)
        ////                        {
        ////                            logOutTime = ((DateTime)logOutEntry.LogTime).ToString("HH:mm:ss");
        ////                        }


        ////                        if (logInTime == "00:00:00" && logOutTime == "00:00:00")
        ////                        {
        ////                            var wfhEntries = wfhData
        ////                                            .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.Date == date.Date)
        ////                                            .OrderBy(x => x.LoginTime)
        ////                                            .ToList();

        ////                            TimeSpan totalWfhActiveHours = TimeSpan.Zero;
        ////                            TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

        ////                            TimeSpan? firstLogin = null;
        ////                            TimeSpan? lastLogout = null;

        ////                            if (wfhEntries.Any())
        ////                            {
        ////                                for (int i = 0; i < wfhEntries.Count; i++)
        ////                                {
        ////                                    var entry = wfhEntries[i];

        ////                                    if (!entry.LoginTime.HasValue)
        ////                                        continue;

        ////                                    TimeSpan logIn = entry.LoginTime.Value;
        ////                                    TimeSpan logOut;

        ////                                    // Earliest login
        ////                                    if (!firstLogin.HasValue || logIn < firstLogin)
        ////                                        firstLogin = logIn;

        ////                                    // Determine logout
        ////                                    if (entry.LogOutTime.HasValue)
        ////                                    {
        ////                                        logOut = entry.LogOutTime.Value;
        ////                                    }
        ////                                    else if (i + 1 < wfhEntries.Count && wfhEntries[i + 1].LoginTime.HasValue)
        ////                                    {
        ////                                        logOut = wfhEntries[i + 1].LoginTime.Value;
        ////                                    }
        ////                                    else
        ////                                    {
        ////                                        logOut = defaultLogout;
        ////                                    }

        ////                                    // Latest logout
        ////                                    if (!lastLogout.HasValue || logOut > lastLogout)
        ////                                        lastLogout = logOut;

        ////                                    if (logOut > logIn)
        ////                                        totalWfhActiveHours += (logOut - logIn);
        ////                                }

        ////                                // Final output
        ////                                logInTime = firstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                                logOutTime = lastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                                activeHours = totalWfhActiveHours.ToString(@"hh\:mm\:ss");
        ////                                wfhDetails = 1;
        ////                            }
        ////                            else
        ////                            {
        ////                                ////var onsiteEntry = onsitedata.FirstOrDefault(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LoginDate == date);
        ////                                ////if (onsiteEntry != null)
        ////                                ////{
        ////                                ////    if (onsiteEntry.LogInTime.HasValue)
        ////                                ////    {
        ////                                ////        if (!onsiteEntry.LogOutTime.HasValue)
        ////                                ////        {
        ////                                ////            onsiteEntry.LogOutTime = new TimeSpan(18, 33, 33);
        ////                                ////        }
        ////                                ////        logInTime = onsiteEntry.LogInTime.Value.ToString(@"hh\:mm\:ss");
        ////                                ////        logOutTime = onsiteEntry.LogOutTime.Value.ToString(@"hh\:mm\:ss");

        ////                                ////        TimeSpan logIn = (TimeSpan)onsiteEntry.LogInTime.Value;
        ////                                ////        TimeSpan logOut = (TimeSpan)onsiteEntry.LogOutTime.Value;
        ////                                ////        activeHours = (logOut - logIn > TimeSpan.Zero) ? (logOut - logIn).ToString(@"hh\:mm\:ss") : "00:00:00";
        ////                                ////    }
        ////                                ////    onsite = 2;
        ////                                ////}
        ////                                ///

        ////                                var onsiteEntry = onsitedata
        ////                                            .Where(x => x.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && x.LoginDate == date.Date)
        ////                                            .OrderBy(x => x.LogInTime)
        ////                                            .ToList();

        ////                                TimeSpan totalOnsiteActiveHours = TimeSpan.Zero;
        ////                                TimeSpan OnsitedefaultLogout = new TimeSpan(18, 36, 0);

        ////                                TimeSpan? OnsitefirstLogin = null;
        ////                                TimeSpan? OnsitelastLogout = null;

        ////                                if (onsiteEntry.Any())
        ////                                {
        ////                                    for (int i = 0; i < onsiteEntry.Count; i++)
        ////                                    {
        ////                                        var entry = onsiteEntry[i];

        ////                                        if (!entry.LogInTime.HasValue)
        ////                                            continue;

        ////                                        TimeSpan logIn = entry.LogInTime.Value;
        ////                                        TimeSpan logOut;

        ////                                        // Earliest login
        ////                                        if (!OnsitefirstLogin.HasValue || logIn < OnsitefirstLogin)
        ////                                            OnsitefirstLogin = logIn;

        ////                                        // Determine logout
        ////                                        if (entry.LogOutTime.HasValue)
        ////                                        {
        ////                                            logOut = entry.LogOutTime.Value;
        ////                                        }
        ////                                        else if (i + 1 < onsiteEntry.Count && onsiteEntry[i + 1].LogInTime.HasValue)
        ////                                        {
        ////                                            logOut = onsiteEntry[i + 1].LogInTime.Value;
        ////                                        }
        ////                                        else
        ////                                        {
        ////                                            logOut = OnsitedefaultLogout;
        ////                                        }

        ////                                        // Latest logout
        ////                                        if (!OnsitelastLogout.HasValue || logOut > OnsitelastLogout)
        ////                                            OnsitelastLogout = logOut;

        ////                                        if (logOut > logIn)
        ////                                            totalOnsiteActiveHours += (logOut - logIn);
        ////                                    }

        ////                                    // Final output
        ////                                    logInTime = OnsitefirstLogin?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                                    logOutTime = OnsitelastLogout?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        ////                                    activeHours = totalOnsiteActiveHours.ToString(@"hh\:mm\:ss");
        ////                                    //workmode = "ONSITE";
        ////                                    onsite = 2;
        ////                                }
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            if (attendanceTimeEntry != null)
        ////                            {
        ////                                if (attendanceTimeEntry.Duration.HasValue)
        ////                                {
        ////                                    activeHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
        ////                                }
        ////                            }
        ////                        }

        ////                        //if (!string.IsNullOrWhiteSpace(logInTime) && !string.IsNullOrWhiteSpace(logOutTime) &&
        ////                        //    logInTime != "00:00:00" && logOutTime != "00:00:00")
        ////                        //{
        ////                        //    if (TimeSpan.TryParse(logInTime, out TimeSpan logIn) && TimeSpan.TryParse(logOutTime, out TimeSpan logOut))
        ////                        //    {
        ////                        //        TimeSpan activeDuration = logOut - logIn;
        ////                        //        //activeHours = activeDuration > TimeSpan.Zero ? activeDuration.ToString(@"hh\:mm\:ss") : "00:00:00";
        ////                        //        activeHours = ((DateTime)attendanceTimeEntry.Duration).ToString("HH:mm:ss");
        ////                        //    }
        ////                        //}

        ////                        TimeSpan workingHours = TimeSpan.Zero;
        ////                        if (DateTime.TryParse(logInTime, out DateTime logInDateTime) && DateTime.TryParse(logOutTime, out DateTime logOutDateTime))
        ////                        {
        ////                            TimeSpan logIn = logInDateTime.TimeOfDay;
        ////                            TimeSpan logOut = logOutDateTime.TimeOfDay;

        ////                            workingHours = logOut - logIn;

        ////                            if (workingHours < TimeSpan.Zero)
        ////                            {
        ////                                workingHours = TimeSpan.Zero;
        ////                            }
        ////                        }

        ////                        var onsiteLogs = DB.Loginlogs
        ////                     .Where(log => log.EmpCode.ToUpper() == emp.EmpCode.ToUpper() && log.LoginDate == date)
        ////                     .Select(log => new { log.LogInTime, log.LogOutTime })
        ////                     .ToList();

        ////                        TimeSpan totalActiveHours = TimeSpan.Zero;
        ////                        TimeSpan breakTime = TimeSpan.Zero;

        ////                        if (onsiteLogs.Any())
        ////                        {
        ////                            var minLogIn = onsiteLogs.Min(log => log.LogInTime);
        ////                            var maxLogOut = onsiteLogs.Max(log => log.LogOutTime);

        ////                            if (minLogIn.HasValue && maxLogOut.HasValue)
        ////                            {
        ////                                TimeSpan totalTime = maxLogOut.Value - minLogIn.Value;

        ////                                foreach (var log in onsiteLogs)
        ////                                {
        ////                                    if (log.LogInTime.HasValue && log.LogOutTime.HasValue)
        ////                                    {
        ////                                        TimeSpan sessionDuration = log.LogOutTime.Value - log.LogInTime.Value;
        ////                                        if (sessionDuration > TimeSpan.Zero)
        ////                                        {
        ////                                            totalActiveHours += sessionDuration;
        ////                                        }
        ////                                    }
        ////                                }

        ////                                breakTime = totalTime - totalActiveHours;
        ////                                if (breakTime < TimeSpan.Zero)
        ////                                {
        ////                                    breakTime = TimeSpan.Zero;
        ////                                }
        ////                            }
        ////                        }

        ////                        DateTime startDate1 = startDate; // assign existing startDate to a new variable
        ////                        DateTime endDate1 = endDate;     // assign existing endDate to a new variable

        ////                        // Calculate the difference using the new variables
        ////                        TimeSpan difference = endDate1 - startDate1;

        ////                        // Get total days as decimal
        ////                        decimal totalDays = (decimal)difference.TotalDays;

        ////                        var lop = (from lev in DB.EmpLeaveApplications
        ////                                   where lev.EmpId == emp.EmpId
        ////                                      && lev.LeaveTypeId == 0
        ////                                      && lev.StartDate >= startDate
        ////                                      && lev.EndDate <= endDate
        ////                                      && lev.IsActive == true
        ////                                      && lev.IsDeleted == false
        ////                                   orderby lev.StartDate descending
        ////                                   select lev).ToList();

        ////                        decimal? lopDuration = (from lev in DB.EmpLeaveApplications
        ////                                                where lev.EmpId == emp.EmpId
        ////                                                   && lev.LeaveTypeId == 0
        ////                                                   && lev.StartDate >= startDate
        ////                                                   && lev.EndDate <= endDate
        ////                                                   && lev.IsActive == true
        ////                                                   && lev.IsDeleted == false
        ////                                                select lev.Duration)
        ////                                               .DefaultIfEmpty(0)           // avoid null result
        ////                                               .Sum();

        ////                        decimal? workingdays = totalDays - lopDuration;

        ////                        AttendanceViewModel avm = new AttendanceViewModel
        ////                        {
        ////                            EmpId = emp.EmpId,
        ////                            EmpCode = emp.EmpCode,
        ////                            EmpName = emp.FirstName + emp.MiddleName + emp.LastName,
        ////                            LogDate = date,
        ////                            LogInTime = logInTime,
        ////                            LogOutTime = logOutTime,
        ////                            WorkingHours = workingHours.ToString(@"hh\:mm\:ss"),
        ////                            CompId = emp.CompId,
        ////                            CompName = emp.Company,
        ////                            Designation = emp.DesignationName,
        ////                            DeptName = emp.DeptName,
        ////                            DeptId = emp.CategoryId,
        ////                            DesignationId = emp.DesignationId,
        ////                            PayDays = workingdays,
        ////                            LeaveType = "",
        ////                            ActiveHours = activeHours,
        ////                            WorkType = wfhDetails == 1 ? "WFH" : (onsite == 2 ? "OnSite" : string.Empty),
        ////                            ShiftName = empShift?.ShiftName ?? "No Shift",
        ////                            BreakTime = breakTime.ToString(@"hh\:mm\:ss"),
        ////                        };

        ////                        lstOfAtt.Add(avm);

        ////                    }

        ////                    advm.lstofAttendance = lstOfAtt
        ////                        .OrderBy(att => att.WorkingHours)
        ////                        .ThenBy(att => att.EmpId)
        ////                        .ToList();

        ////                    lstOfDate.Add(advm);
        ////                    ////foreach (var day in lstOfDate)
        ////                    ////{
        ////                    ////    foreach (var emp in day.lstofAttendance)
        ////                    ////    {
        ////                    ////        TimeSpan workingHours = TimeSpan.Zero;

        ////                    ////        if (!string.IsNullOrWhiteSpace(emp.WorkingHours))
        ////                    ////            TimeSpan.TryParse(emp.WorkingHours, out workingHours);

        ////                    ////        emp.PayDays = CalculatePayDay(workingHours);
        ////                    ////        if (emp.PayDays == Convert.ToDecimal(0.5))
        ////                    ////        {
        ////                    ////            emp.DaysPresent = 1;
        ////                    ////        }
        ////                    ////    }
        ////                    ////}

        ////                    ////var empGroups = lstOfDate
        ////                    ////                    .SelectMany(x => x.lstofAttendance)
        ////                    ////                    .GroupBy(x => x.EmpId);

        ////                    ////foreach (var group in empGroups)
        ////                    ////{
        ////                    ////    decimal totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                    ////    decimal finalPayDays = totalPayDays + weekdenddayscount;

        ////                    ////    // Assign TOTAL only (do NOT overwrite PayDays)
        ////                    ////    foreach (var record in group)
        ////                    ////    {
        ////                    ////        record.PayDays = finalPayDays;
        ////                    ////    }
        ////                    ////}
        ////                    var lopLeaves = (from lev in DB.EmpLeaveApplications
        ////                                     where //lev.EmpId == emp.EmpId && 
        ////                                     lev.LeaveTypeId == 0
        ////                                        && lev.StartDate >= startDate
        ////                                        && lev.EndDate <= endDate
        ////                                        && lev.IsActive == true
        ////                                        && lev.IsDeleted == false
        ////                                     orderby lev.StartDate descending
        ////                                     select lev).ToList();

        ////                    ////foreach (var day in lstOfDate)
        ////                    ////{
        ////                    ////    foreach (var empAtt in day.lstofAttendance)
        ////                    ////    {
        ////                    ////        DateTime attendanceDate = Convert.ToDateTime(day.AttendaceDate);

        ////                    ////        bool isLOP = lopLeaves.Any(l =>
        ////                    ////            l.EmpId == empAtt.EmpId &&                // ✅ match employee here
        ////                    ////            attendanceDate >= Convert.ToDateTime(l.StartDate) &&
        ////                    ////            attendanceDate <= Convert.ToDateTime(l.EndDate)
        ////                    ////        );

        ////                    ////        if (isLOP)
        ////                    ////        {
        ////                    ////            empAtt.LeaveType = "LOP";
        ////                    ////            //empAtt.PayDays = 0;
        ////                    ////        }
        ////                    ////    }
        ////                    ////}

        ////                    foreach (var day in lstOfDate)
        ////                    {
        ////                        foreach (var emp in day.lstofAttendance)
        ////                        {
        ////                            TimeSpan workingHours = TimeSpan.Zero;

        ////                            if (!string.IsNullOrWhiteSpace(emp.WorkingHours))
        ////                                TimeSpan.TryParse(emp.WorkingHours, out workingHours);

        ////                            emp.PayDays = CalculatePayDay(workingHours);
        ////                            if (emp.PayDays == Convert.ToDecimal(0.5))
        ////                            {
        ////                                emp.DaysPresent = 1;
        ////                            }
        ////                        }
        ////                    }

        ////                    var empGroups = lstOfDate
        ////                                        .SelectMany(x => x.lstofAttendance)
        ////                                        .GroupBy(x => x.EmpId);

        ////                    foreach (var group in empGroups)
        ////                    {
        ////                        int? employeeId = group.Key;   // ✅ renamed

        ////                        // LOP days ONLY for this employee
        ////                        decimal? lopDaysCount = lopLeaves
        ////                            .Where(l => l.EmpId == employeeId)
        ////                            .Sum(l => l.Duration);

        ////                        decimal totalPayDays = group.Sum(x => x.PayDays ?? 0);
        ////                        decimal finalPayDays = totalPayDays + weekdenddayscount - Convert.ToDecimal(lopDaysCount);

        ////                        // Assign TOTAL only (do NOT overwrite PayDays)
        ////                        foreach (var record in group)
        ////                        {
        ////                            record.PayDays = finalPayDays;
        ////                        }
        ////                    }
        ////                }
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        ////            }

        ////            return lstOfDate;
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "Employee's count is Missing");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}

        public List<Loginlog> GetAllLoginLogs(LoginlogViewModel model)
        {
            try
            {

                string msg = "";
                //string empname = model.empname;
                int? id = (model.LoginId != 0) ? model.LoginId : 0;

                int? oldempid = (from emp in DB.EmployeeMasters
                                 where emp.IsActive == true && emp.IsDeleted == false && emp.EmpId == id
                                 select emp.OldEmp_ID).FirstOrDefault();

                if (id != 0)
                {
                    var empdetails = (from emp in DB.EmployeeMasters
                                      where emp.IsActive == true && emp.IsDeleted == false && (emp.ReportId == id || emp.ReportId == oldempid)
                                      select emp.EmpId).ToList();

                    var Loginlogsdata = (from osd in DB.Loginlogs
                                         join emp in DB.EmployeeMasters on osd.EmpId equals emp.EmpId
                                         where emp.IsActive == true && emp.IsDeleted == false && empdetails.Any(e => e == osd.EmpId)
                                         select osd).OrderByDescending(x => x.CreatedDate).ToList();

                    if (Loginlogsdata != null)
                    {
                        return Loginlogsdata;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Login details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Login Id is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<Loginlog> GetLoginLogs(LoginlogViewModel model)
        {
            try
            {

                string msg = "";
                //string empname = model.empname;
                int? id = (model.LoginId != 0) ? model.LoginId : 0;

                if (id != 0)
                {
                    var Loginlogsdata = (from osd in DB.Loginlogs
                                         join emp in DB.EmployeeMasters on osd.EmpId equals emp.EmpId
                                         where emp.IsActive == true && emp.IsDeleted == false && osd.EmpId == id
                                         select osd).OrderByDescending(x => x.CreatedDate).ToList();

                    if (Loginlogsdata != null)
                    {
                        return Loginlogsdata;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Login details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Login Id is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<OnSiteLoginlog> GetOnSiteData(OnSiteDataViewModel model)
        {
            try
            {
                string msg = "";
                //string empname = model.empname;
                int? id = (model.LoginId != 0) ? model.LoginId : 0;

                if (id != 0)
                {
                    var onsitedata = (from osd in DB.OnSiteLoginlogs
                                      join emp in DB.EmployeeMasters on osd.EmpId equals emp.EmpId
                                      where emp.IsActive == true && emp.IsDeleted == false && osd.EmpId == id
                                      select osd).OrderByDescending(x => x.Id).ToList();


                    //List<OnSiteDataViewModel> lstofOnsitelog = new List<OnSiteDataViewModel>();
                    if (onsitedata != null)
                    {
                        //    for (int i = 0; i < onsitedata.Count(); i++)
                        //    {
                        //        OnSiteDataViewModel ondvm = new OnSiteDataViewModel();
                        //        ondvm.Id = onsitedata[i].Id;
                        //        ondvm.EmpId = onsitedata[i].EmpId;
                        //        ondvm.EmpCode = onsitedata[i].EmpCode;
                        //        ondvm.Company = onsitedata[i].Company;
                        //        ondvm.LoginAddress = onsitedata[i].LoginAddress;
                        //        ondvm.LoginCity = onsitedata[i].LoginCity;
                        //        ondvm.LoginDate = onsitedata[i].LoginDate;
                        //        ondvm.LoginLongitude = onsitedata[i].LoginLongitude;
                        //        ondvm.LoginLatitude = onsitedata[i].LoginLatitude;
                        //        ondvm.LogoutAddress = onsitedata[i].LogoutAddress;
                        //        ondvm.LogoutCity = onsitedata[i].LogoutCity;
                        //        ondvm.LogoutDate = onsitedata[i].LogoutDate;
                        //        ondvm.LogoutLongitude = onsitedata[i].LogoutLongitude;
                        //        ondvm.LogoutLatitude = onsitedata[i].LogoutLatitude;
                        //        ondvm.LogInTime = onsitedata[i].LogInTime;
                        //        ondvm.LogOutTime = onsitedata[i].LogOutTime;
                        //        ondvm.CreatedBy = onsitedata[i].CreatedBy;
                        //        ondvm.CreatedDate = onsitedata[i].CreatedDate;
                        //        ondvm.LastUpdatedBy = onsitedata[i].LastUpdatedBy;
                        //        ondvm.LastUpdatedDate = onsitedata[i].LastUpdatedDate;
                        //        ondvm.IsActive = onsitedata[i].IsActive;
                        //        ondvm.IsUpdated = onsitedata[i].IsUpdated;
                        //        ondvm.IsDeleted = onsitedata[i].IsDeleted;
                        //        lstofOnsitelog.Add(ondvm);
                        //    }

                        return onsitedata;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee onsite details Not Found");
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
        public LoginLogViewModel OnSiteLogin(LoginLogViewModel model)
        {
            try
            {
                string msg = "";
                int? empid = (model.LoginId != 0) ? model.LoginId : 0;
                DateTime tdy = Convert.ToDateTime(model.LoginDate);

                if (empid != 0)
                {
                    var loginlogdetails = (from l in DB.Loginlogs
                                           where l.EmpId == empid && l.ActionType == "LOGIN"
                                           select l).OrderByDescending(x => x.EmpId).ToList();

                    if (loginlogdetails.Count() == 0)
                    {
                        Loginlog osd = new Loginlog();
                        osd.EmpId = Convert.ToInt32(empid);
                        osd.EmpCode = model.EmpCode;
                        osd.LoginAddress = model.LoginAddress;
                        osd.LoginCity = model.LoginCity;
                        osd.LoginDate = DateTime.Now;
                        osd.LoginLongitude = model.LoginLongitude;
                        osd.LoginLatitude = model.LoginLatitude;
                        osd.ActionType = "LOGIN";
                        osd.LogoutAddress = "";
                        osd.LogoutCity = "";
                        osd.LogoutLongitude = "";
                        osd.LogoutLatitude = "";
                        osd.LogoutDate = null;
                        osd.LogInTime = DateTime.Now.TimeOfDay;
                        osd.CreatedBy = model.LoginId;
                        osd.CreatedDate = DateTime.Now;
                        osd.IsActive = true;
                        osd.IsUpdated = false;
                        osd.IsDeleted = false;

                        DB.Loginlogs.Add(osd);
                        DB.SaveChanges();

                        LoginLogViewModel osdvm = new LoginLogViewModel();
                        osdvm.msg = "Login";

                        return osdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User Already Logged In");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public LoginLogViewModel OnSiteLogout(LoginLogViewModel model)
        {
            try
            {
                string msg = "";
                int? empid = (model.LoginId != 0) ? model.LoginId : 0;
                DateTime tdy = Convert.ToDateTime(model.LoginDate);
                int? id = (model.Id != 0) ? model.Id : 0;

                if (empid != 0)
                {
                    var loginlogdetails = (from osd in DB.Loginlogs
                                           where osd.EmpId == empid && osd.ActionType == "LOGIN" && osd.Id == id
                                           select osd).OrderByDescending(x => x.EmpId).FirstOrDefault();

                    if (loginlogdetails != null)
                    {
                        loginlogdetails.LogoutAddress = model.LogoutAddress;
                        loginlogdetails.LogoutCity = model.LogoutCity;
                        loginlogdetails.LogoutDate = DateTime.Now;
                        loginlogdetails.LogoutLongitude = model.LogoutLongitude;
                        loginlogdetails.LogoutLatitude = model.LogoutLatitude;
                        loginlogdetails.LogOutTime = DateTime.Now.TimeOfDay;
                        loginlogdetails.ActionType = "LOGOUT";
                        loginlogdetails.IsUpdated = true;
                        loginlogdetails.LastUpdatedBy = model.LoginId;
                        loginlogdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        LoginLogViewModel osdvm = new LoginLogViewModel();
                        osdvm.msg = "Logout";

                        return osdvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Login details not found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public OnSiteDataViewModel AddOnSiteData(OnSiteDataViewModel model)
        {
            try
            {
                string msg = "";
                int? empid = (model.LoginId != 0) ? model.LoginId : 0;
                DateTime tdy = Convert.ToDateTime(model.LoginDate);

                if (empid != 0)
                {
                    var workTypeDetails = (from wtm in DB.WorkTypeMasters
                                           where wtm.EmpId == empid && wtm.IsApproved == true && wtm.IsRejected == false
                                           && wtm.StartDate <= tdy && wtm.EndDate >= tdy
                                           select wtm).FirstOrDefault();


                    if (workTypeDetails != null)
                    {
                        if (model.WorkStatus == "Login")
                        {
                            OnSiteLoginlog osd = new OnSiteLoginlog();
                            osd.EmpId = Convert.ToInt32(empid);
                            osd.EmpCode = model.EmpCode;
                            osd.Company = model.Company;
                            osd.LoginAddress = model.LoginAddress;
                            osd.LoginCity = model.LoginCity;
                            osd.LoginDate = model.LoginDate;
                            osd.LogInTime = DateTime.Now.TimeOfDay;
                            osd.LoginLongitude = model.LoginLongitude;
                            osd.LoginLatitude = model.LoginLatitude;
                            osd.Purpose = model.Purpose;
                            osd.Description = model.Description;
                            osd.LogoutAddress = model.LogoutAddress;
                            osd.LogoutCity = model.LogoutCity;
                            osd.LogoutDate = model.LogoutDate;
                            osd.LogoutLongitude = model.LogoutLongitude;
                            osd.LogoutLatitude = model.LogoutLatitude;
                            osd.CreatedBy = model.LoginId;
                            osd.CreatedDate = DateTime.Now;
                            osd.IsActive = true;
                            osd.IsUpdated = false;
                            osd.IsDeleted = false;

                            DB.OnSiteLoginlogs.Add(osd);
                            DB.SaveChanges();

                            OnSiteDataViewModel osdvm = new OnSiteDataViewModel();
                            osdvm.msg = "Added";

                            return osdvm;
                        }
                        else
                        {
                            int? id = (model.Id != 0) ? model.Id : 0;
                            if (id != 0)
                            {
                                var onsitedetails = (from osd in DB.OnSiteLoginlogs
                                                     where osd.EmpCode == model.EmpCode && osd.LogInTime != null && osd.Id == id
                                                     select osd).OrderByDescending(x => x.EmpId).FirstOrDefault();

                                if (onsitedetails != null)
                                {
                                    onsitedetails.Description = model.Description;
                                    onsitedetails.LogoutAddress = model.LogoutAddress;
                                    onsitedetails.LogoutCity = model.LogoutCity;
                                    onsitedetails.LogoutDate = model.LogoutDate;
                                    onsitedetails.LogoutLongitude = model.LogoutLongitude;
                                    onsitedetails.LogoutLatitude = model.LogoutLatitude;
                                    onsitedetails.LogOutTime = DateTime.Now.TimeOfDay;
                                    onsitedetails.ActiveHrs = (onsitedetails.LogOutTime - onsitedetails.LogInTime);
                                    onsitedetails.IsUpdated = true;
                                    onsitedetails.LastUpdatedBy = model.LoginId;
                                    onsitedetails.LastUpdatedDate = DateTime.Now;
                                    DB.SaveChanges();

                                    OnSiteDataViewModel osdvm = new OnSiteDataViewModel();
                                    osdvm.msg = "Updated";

                                    return osdvm;
                                }
                                else
                                {
                                    throw new CustomApiException(HttpStatusCode.NotFound, "Login details not found");
                                }
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Onsite Id is missing");
                            }
                        }
                    }
                    else
                    {
                        //var workTypeDetails1 = (from wtm in DB.WorkTypeMasters
                        //                        where wtm.EmpId == empid && wtm.IsApproved == false && wtm.IsRejected == true
                        //                        && wtm.StartDate <= tdy && wtm.EndDate >= tdy
                        //                        select wtm).FirstOrDefault();

                        //var workTypeDetails2 = (from wtm in DB.WorkTypeMasters
                        //                        where wtm.EmpId == empid && wtm.IsApproved == false && wtm.IsRejected == false
                        //                        && wtm.StartDate <= tdy && wtm.EndDate >= tdy
                        //                        select wtm).FirstOrDefault();

                        //if (workTypeDetails1 != null)
                        //{
                        //    throw new CustomApiException(HttpStatusCode.NotFound, "Onsite Request is Rejected");
                        //}
                        //else if (workTypeDetails2 != null)
                        //{
                        //    throw new CustomApiException(HttpStatusCode.NotFound, "Onsite Request is not Approved");
                        //}
                        //else
                        //{
                        //    throw new CustomApiException(HttpStatusCode.NotFound, "Onsite Approved details not found");
                        //}

                        if (model.WorkStatus == "Login")
                        {
                            OnSiteLoginlog osd = new OnSiteLoginlog();
                            osd.EmpId = Convert.ToInt32(empid);
                            osd.EmpCode = model.EmpCode;
                            osd.Company = model.Company;
                            osd.LoginAddress = model.LoginAddress;
                            osd.LoginCity = model.LoginCity;
                            osd.LoginDate = model.LoginDate;
                            osd.LogInTime = DateTime.Now.TimeOfDay;
                            osd.LoginLongitude = model.LoginLongitude;
                            osd.LoginLatitude = model.LoginLatitude;
                            osd.Purpose = model.Purpose;
                            osd.Description = model.Description;
                            osd.LogoutAddress = model.LogoutAddress;
                            osd.LogoutCity = model.LogoutCity;
                            osd.LogoutDate = model.LogoutDate;
                            osd.LogoutLongitude = model.LogoutLongitude;
                            osd.LogoutLatitude = model.LogoutLatitude;
                            osd.CreatedBy = model.LoginId;
                            osd.CreatedDate = DateTime.Now;
                            osd.IsActive = true;
                            osd.IsUpdated = false;
                            osd.IsDeleted = false;

                            DB.OnSiteLoginlogs.Add(osd);
                            DB.SaveChanges();

                            OnSiteDataViewModel osdvm = new OnSiteDataViewModel();
                            osdvm.msg = "Added";

                            return osdvm;
                        }
                        else
                        {
                            int? id = (model.Id != 0) ? model.Id : 0;
                            if (id != 0)
                            {
                                var onsitedetails = (from osd in DB.OnSiteLoginlogs
                                                     where osd.EmpCode == model.EmpCode && osd.Id == id
                                                     select osd).OrderByDescending(x => x.EmpId).FirstOrDefault();

                                if (onsitedetails != null)
                                {
                                    onsitedetails.Description = model.Description;
                                    onsitedetails.LogoutAddress = model.LogoutAddress;
                                    onsitedetails.LogoutCity = model.LogoutCity;
                                    onsitedetails.LogoutDate = model.LogoutDate;
                                    onsitedetails.LogoutLongitude = model.LogoutLongitude;
                                    onsitedetails.LogoutLatitude = model.LogoutLatitude;
                                    onsitedetails.LogOutTime = DateTime.Now.TimeOfDay;
                                    onsitedetails.IsUpdated = true;
                                    onsitedetails.LastUpdatedBy = model.LoginId;
                                    onsitedetails.LastUpdatedDate = DateTime.Now;
                                    DB.SaveChanges();

                                    OnSiteDataViewModel osdvm = new OnSiteDataViewModel();
                                    osdvm.msg = "Updated";

                                    return osdvm;
                                }
                                else
                                {
                                    throw new CustomApiException(HttpStatusCode.NotFound, "Login details not found");
                                }
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Onsite Id is missing");
                            }
                        }
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public List<SelectEmployeeViewModel> SelectEmployee(SelectEmployeeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  join loc in DB.LocationMasters on emp.LocationId equals loc.LocationId
                                  where emp.IsActive == true && emp.IsDeleted == false
                                  select emp).OrderByDescending(x => x.EmpId).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<SelectEmployeeViewModel> lstofEmp = new List<SelectEmployeeViewModel>();

                        for (int i = 0; i < empdetails.Count(); i++)
                        {
                            SelectEmployeeViewModel emvm = new SelectEmployeeViewModel();
                            emvm.EmpId = empdetails[i].EmpId;
                            emvm.CompId = empdetails[i].CompId;
                            emvm.Company = DB.CompanyMasters.Where(x => x.CompId == emvm.CompId).Select(x => x.Company).FirstOrDefault();
                            emvm.DeptName = empdetails[i].DeptName;
                            emvm.ReportId = empdetails[i].ReportId;
                            emvm.EmpCode = empdetails[i].EmpCode;
                            emvm.FirstName = empdetails[i].FirstName;
                            emvm.MiddleName = empdetails[i].MiddleName;
                            emvm.LastName = empdetails[i].LastName;
                            emvm.IsActive = empdetails[i].IsActive;
                            emvm.IsUpdated = empdetails[i].IsUpdated;
                            emvm.IsDeleted = empdetails[i].IsDeleted;

                            lstofEmp.Add(emvm);
                        }

                        return lstofEmp;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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

        public TotalEmployeeViewModel GetTotalEmployeeCount(TotalEmployeeViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                int totalEmployeeCount = DB.EmployeeMasters
                   .Where(emp => emp.IsActive == true && emp.IsDeleted == false)
                   .Count();

                model.TotalEmployeeCount = totalEmployeeCount;

                return model;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public AttendanceSourceViewModel GetAttendanceSource(AttendanceSourceViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                DateTime startDate = (model.StartDate != null) ? model.StartDate : DateTime.Today.AddMonths(-1);
                DateTime endDate = (model.EndDate != null) ? model.EndDate : DateTime.Today;

                startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
                endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

                int deviceCheckIns = DB.Attendances
                   .Where(log =>
                       log.Type == "IN" &&
                       DbFunctions.TruncateTime(log.LogDate) >= DbFunctions.TruncateTime(startDate) &&
                       DbFunctions.TruncateTime(log.LogDate) <= DbFunctions.TruncateTime(endDate))
                   .GroupBy(log => new { log.LogID, Date = DbFunctions.TruncateTime(log.LogDate) })
                   .Select(group => group.OrderBy(log => log.LogTime).FirstOrDefault())
                   .Count();

                int appCheckIns = DB.WFHLoginlogs
                   .Where(log =>
                       log.IsLoggedIn == true &&
                       log.IsActive == true &&
                       log.IsDeleted == false &&
                       DbFunctions.TruncateTime(log.Date) >= DbFunctions.TruncateTime(startDate) &&
                       DbFunctions.TruncateTime(log.Date) <= DbFunctions.TruncateTime(endDate))
                   .Count();

                int onSiteCheckIns = DB.Loginlogs
                   .Where(log =>
                       log.IsActive == true &&
                       log.IsDeleted == false &&
                       DbFunctions.TruncateTime(log.LoginDate) >= DbFunctions.TruncateTime(startDate) &&
                       DbFunctions.TruncateTime(log.LoginDate) <= DbFunctions.TruncateTime(endDate))
                   .Count();

                AttendanceSourceViewModel result = new AttendanceSourceViewModel
                {
                    DeviceCheckIns = deviceCheckIns,
                    AppCheckIns = appCheckIns,
                    OnSiteCheckIns = onSiteCheckIns
                };

                return result;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<ddLocationViewModel> DDGetLocation(ddLocationViewModel model)
        {
            try
            {
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var locationDetails = (from lm in DB.LocationMasters
                                       where lm.IsActive == true && lm.IsDeleted == false
                                       select new ddLocationViewModel
                                       {
                                           LocationId = lm.LocationId,
                                           Location = lm.Location,
                                       }).ToList();

                if (EmpId != 0)
                {
                    if (EmpId == 149)
                    {
                        return locationDetails;
                    }
                }
                return locationDetails;

            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        //public List<DDSelectEmpViewModel> DDselectEmployee(DDSelectEmpViewModel model)
        //{
        //    try
        //    {
        //        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
        //        int? LocationId = model.LocationId;

        //        var Empdetails = (from Emp in DB.EmployeeMasters
        //                          join loc in DB.LocationMasters on Emp.LocationId equals loc.LocationId
        //                          where Emp.IsActive == true
        //                                && Emp.IsDeleted == false
        //                                && loc.IsActive == true
        //                                && loc.IsDeleted == false
        //                                && (LocationId == null || Emp.LocationId == LocationId)
        //                          select new DDSelectEmpViewModel
        //                          {
        //                              EmpId = Emp.EmpId,
        //                              EmpName = Emp.FirstName + " " + Emp.MiddleName + " " + Emp.LastName,
        //                              EmpCode = Emp.UserName,
        //                          }).ToList();

        //        if (EmpId != 0)
        //        {
        //            if (Empdetails != null && Empdetails.Any())
        //            {
        //                return Empdetails;
        //            }
        //            else
        //            {
        //                throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found for the given Location or EmpId");
        //            }
        //        }
        //        else
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        //        }
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}

        public List<DDSelectEmpViewModel> DDselectEmployee(DDSelectEmpViewModel model)
        {
            try
            {
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LocationId = model.LocationId;

                var Empdetails = (from Emp in DB.EmployeeMasters
                                  join loc in DB.LocationMasters on Emp.LocationId equals loc.LocationId
                                  where Emp.IsActive == true && Emp.IsDeleted == false && loc.IsActive == true
                                  && loc.IsDeleted == false && (LocationId == null || Emp.LocationId == LocationId)
                                  select new DDSelectEmpViewModel
                                  {
                                      EmpId = Emp.EmpId,
                                      FirstName = Emp.FirstName,
                                      MiddleName = Emp.MiddleName,
                                      LastName = Emp.LastName,
                                      EmpCode = Emp.UserName,
                                  }).ToList();

                if (EmpId != 0)
                {
                    if (Empdetails != null && Empdetails.Any())
                    {
                        return Empdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found for the given Location or EmpId");
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

        //public ConsolidatedAttendanceSummaryViewModel GetConsolidatedAttendanceSummary(AttendanceFilterViewModel model)
        //{
        //    try
        //    {
        //        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        //        DateTime startDate = (model.StartDate != default(DateTime)) ? model.StartDate : DateTime.Today.AddMonths(-1);
        //        DateTime endDate = (model.EndDate != default(DateTime)) ? model.EndDate : DateTime.Today;

        //        startDate = startDate < DateTime.MinValue.AddDays(1) ? DateTime.MinValue.AddDays(1) : startDate;
        //        endDate = endDate > DateTime.MaxValue.AddDays(-1) ? DateTime.MaxValue.AddDays(-1) : endDate;

        //        var summary = new ConsolidatedAttendanceSummaryViewModel
        //        {
        //            TotalWorkedHours = 0,
        //            MaxWorkingHours = 0,
        //            OfficeCount = 0,
        //            WorkFromHomeCount = 0,
        //            OnSiteCount = 0
        //        };

        //        var employees = DB.EmployeeMasters
        //           .Where(employee => employee.IsActive == true)
        //           .ToList();

        //        var attendanceRecords = DB.Attendances
        //           .Where(a => a.LogDate >= startDate && a.LogDate <= endDate)
        //           .Select(a => new { a.LogDate, a.LogTime, a.LogID, a.Type })
        //           .ToList();

        //        var wfhData = DB.WFHLoginlogs
        //           .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
        //           .Select(wfh => new { wfh.EmpId, wfh.Date, wfh.LoginTime, wfh.LogOutTime, wfh.Activehrs })
        //           .ToList();

        //        var onsiteData = DB.OnSiteLoginlogs
        //           .Where(onsite => onsite.Date >= startDate && onsite.Date <= endDate)
        //           .Select(onsite => new { onsite.EmpId, onsite.Date, onsite.LogInTime, onsite.LogOutTime })
        //           .ToList();

        //        foreach (var emp in employees)
        //        {
        //            double empWorkedHours = 0;

        //            var officeLogs = attendanceRecords.Where(a => a.LogID == emp.EmpId).ToList();
        //            foreach (var log in officeLogs)
        //            {
        //                if (log.Type == "IN" && log.LogTime != null)
        //                {
        //                    var logInTime = log.LogTime.Value;

        //                    var logOutTime = officeLogs
        //                        .FirstOrDefault(l => l.Type == "OUT" && l.LogDate == log.LogDate)?.LogTime;

        //                    if (logOutTime != null)
        //                    {
        //                        TimeSpan workingDuration = logOutTime.Value - logInTime;
        //                        empWorkedHours += workingDuration.TotalHours;
        //                    }
        //                }
        //            }

        //            var wfhLogs = wfhData.Where(wfh => wfh.EmpId == emp.EmpId).ToList();
        //            foreach (var wfhLog in wfhLogs)
        //            {
        //                if (wfhLog.LoginTime.HasValue && wfhLog.LogOutTime.HasValue)
        //                {
        //                    TimeSpan wfhDuration = wfhLog.LogOutTime.Value - wfhLog.LoginTime.Value;
        //                    empWorkedHours += wfhDuration.TotalHours;
        //                }
        //                else if (wfhLog.Activehrs.HasValue)
        //                {
        //                    empWorkedHours += wfhLog.Activehrs.Value.TotalHours;
        //                }
        //            }

        //            var onsiteLogs = onsiteData.Where(onsite => onsite.EmpId == emp.EmpId).ToList();
        //            foreach (var onsiteLog in onsiteLogs)
        //            {
        //                if (onsiteLog.LogInTime.HasValue && onsiteLog.LogOutTime.HasValue)
        //                {
        //                    TimeSpan onsiteDuration = onsiteLog.LogOutTime.Value - onsiteLog.LogInTime.Value;
        //                    empWorkedHours += onsiteDuration.TotalHours;
        //                }
        //            }

        //            summary.TotalWorkedHours += empWorkedHours;

        //            var employeeShiftDetails = DB.EmpShiftDetails
        //               .Where(shift => shift.EmpId == emp.EmpId && shift.IsActive == true && shift.IsDeleted == false)
        //               .ToList();

        //            foreach (var shift in employeeShiftDetails)
        //            {
        //                if (shift.StartDate.HasValue && shift.EndDate.HasValue)
        //                {
        //                    DateTime shiftStart = shift.StartDate.Value >= startDate ? startDate : shift.StartDate.Value;
        //                    DateTime shiftEnd = shift.EndDate.Value >= endDate ? endDate : shift.EndDate.Value;

        //                    if (shiftStart <= shiftEnd)
        //                    {
        //                        TimeSpan shiftDuration = shiftEnd - shiftStart;
        //                        int workingDays = shiftDuration.Days + 1;
        //                        double dailyWorkingHours = 9.0;
        //                        summary.MaxWorkingHours += workingDays * dailyWorkingHours;
        //                    }
        //                }
        //            }
        //        }

        //        summary.OfficeCount = DB.Attendances
        //           .Where(log => log.Type == "IN" && log.LogDate >= startDate && log.LogDate <= endDate)
        //           .Select(log => log.LogID)
        //           .Distinct()
        //           .Count();

        //        summary.WorkFromHomeCount = DB.WFHLoginlogs
        //            .Where(log => log.IsActive == true && log.IsDeleted == false && log.Date >= startDate && log.Date <= endDate)
        //            .Select(log => log.EmpId)
        //            .Distinct()
        //            .Count();

        //        summary.OnSiteCount = DB.OnSiteLoginlogs
        //            .Where(log => log.IsActive == true && log.IsDeleted == false && log.Date >= startDate && log.Date <= endDate)
        //            .Select(log => log.EmpId)
        //            .Distinct()
        //            .Count();

        //        return summary;
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}
        //   public List<DailyAttendanceSummaryViewModel> GetOnTimeCheckInForAllEmployees(OnTimeCheckInViewModel model)
        //{
        //    try
        //    {
        //        DateTime startDate = model.StartDate ?? DateTime.Today.AddMonths(-1);
        //        DateTime endDate = model.EndDate ?? DateTime.Today;

        //        string empCode = model.EmpCode;
        //        int? locationId = model.LocationId;

        //        bool isSingleEmployeeSearch = !string.IsNullOrEmpty(empCode);

        //        var employeeQuery = DB.EmployeeMasters.Where(emp => emp.IsActive == true);

        //        if (isSingleEmployeeSearch)
        //        {
        //            employeeQuery = employeeQuery.Where(emp => emp.EmpCode == empCode);
        //        }

        //        if (!isSingleEmployeeSearch && locationId.HasValue)
        //        {
        //            employeeQuery = employeeQuery.Where(emp => emp.LocationId == locationId);
        //        }

        //        var employees = employeeQuery
        //           .Select(emp => new
        //           {
        //               emp.EmpId,
        //               emp.OldEmp_ID,
        //               emp.EmpCode,
        //               EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName
        //           })
        //           .ToList();

        //        if (!employees.Any())
        //        {
        //            return new List<DailyAttendanceSummaryViewModel>();
        //        }

        //        var employeeIds = employees.Select(emp => emp.OldEmp_ID).ToList();

        //        var attendanceRecords = DB.Attendances
        //           .Where(att => att.Type == "IN" &&
        //                         att.LogDate >= startDate &&
        //                         att.LogDate <= endDate &&
        //                         employeeIds.Contains((int)att.LogID))
        //           .Select(att => new
        //           {
        //               att.LogDate,
        //               att.LogTime,
        //               att.LogID
        //           })
        //           .ToList();

        //        var groupedAttendance = attendanceRecords
        //           .GroupBy(record => new { record.LogID, Date = record.LogDate.Value.Date })
        //           .Select(group => new
        //           {
        //               EmpId = group.Key.LogID,
        //               Date = group.Key.Date,
        //               OnTimeCheckInCount = group.Count(record => record.LogTime.HasValue && record.LogTime.Value.TimeOfDay <= new TimeSpan(9, 30, 0)),
        //               LateCheckInCount = group.Count(record => record.LogTime.HasValue && record.LogTime.Value.TimeOfDay > new TimeSpan(9, 30, 0))
        //           })
        //           .ToList();

        //        var response = groupedAttendance.GroupBy(att => att.Date).Select(group =>
        //        {
        //            var baseResponse = new DailyAttendanceSummaryViewModel
        //            {
        //                Date = group.Key.ToString("yyyy-MM-dd"),
        //                OnTimeCheckInCount = group.Sum(att => att.OnTimeCheckInCount),
        //                LateCheckInCount = group.Sum(att => att.LateCheckInCount),
        //                TotalEmployeeCount = isSingleEmployeeSearch ? 1 : employees.Count,
        //                EmpCode = isSingleEmployeeSearch ? employees.First().EmpCode : null
        //            };
        //            return baseResponse;
        //        }).ToList();

        //        return response;
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}

        //////----------------------------------------- 31.03.2026 changed the Dashboard API----------------------------------------------------------------
        private string SumTimeSpans(IEnumerable<string> timeStrings)
        {
            int totalSeconds = timeStrings
                .Where(t => !string.IsNullOrEmpty(t) && t != "00:00:00")
                .Sum(t =>
                {
                    var parts = t.Split(':');
                    return (int.Parse(parts[0]) * 3600) + (int.Parse(parts[1]) * 60) + int.Parse(parts[2]);
                });

            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
        }
        public List<object> GetConsolidatedAttendanceData(AttendanceFilterViewModel model)
        {
            try
            {
                DateTime startDate = (Convert.ToDateTime(model.StartDate) != default(DateTime))
                    ? Convert.ToDateTime(model.StartDate)
                    : DateTime.Today.AddMonths(-1);

                DateTime endDate = (Convert.ToDateTime(model.EndDate) != default(DateTime))
                    ? Convert.ToDateTime(model.EndDate)
                    : DateTime.Today;

                List<int> locationIds = model.LocationId ?? new List<int>();
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;

                // Get employee code for the logged-in user
                var loggedInEmployee = DB.EmployeeMasters
                    .Where(emp => emp.EmpId == LoginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                           && emp.IsActive == true && emp.IsDeleted == false)
                    .Select(x => new { x.EmpCode, x.LocationId, x.DesignationId, x.CategoryId })
                    .FirstOrDefault();

                string empcode = loggedInEmployee?.EmpCode;
                int locationid = loggedInEmployee?.LocationId ?? 0;
                int desigId = loggedInEmployee?.DesignationId ?? 0;
                int deptId = loggedInEmployee?.CategoryId ?? 0;

                if (model.IsOverall)
                {
                    // Get total employee count based on filters
                    var employeeQuery = DB.EmployeeMasters
                        .Where(emp => emp.EmpStatus.ToUpper() == "ACTIVE" && emp.IsActive == true && emp.IsDeleted == false);

                    // Apply Legal Entity filter if provided
                    if (LEId > 0)
                    {
                        employeeQuery = employeeQuery.Where(emp => emp.LEId == LEId);
                    }

                    // Apply ReportName filter if provided
                    if (!string.IsNullOrEmpty(empcode))
                    {
                        if (deptId == 1)
                        {
                        }
                        else
                        {
                            employeeQuery = employeeQuery.Where(emp => emp.ReportName.ToUpper() == empcode.ToUpper());
                        }
                    }

                    // Apply location filter
                    if (locationIds != null && locationIds.Any() && locationIds.Any(id => id > 0))
                    {
                        employeeQuery = employeeQuery.Where(emp => locationIds.Contains(emp.LocationId ?? 0));
                    }

                    // Apply specific employee filter
                    if (model.EmpId != null && model.EmpId != 0)
                    {
                        employeeQuery = employeeQuery.Where(emp => emp.EmpId == model.EmpId);
                    }

                    var employeeList = employeeQuery.Select(x => x.EmpCode).ToList();
                    var totalEmployeeCount = employeeList.Count();

                    // Get all attendance records from the cached table
                    var attendanceRecords = DB.DailyAttendanceRecords
                        .Where(a => a.LogDate >= startDate.Date
                                 && a.LogDate <= endDate.Date
                                 && employeeList.Contains(a.EmpCode))
                        .ToList();

                    // Calculate metrics from the cached table
                    var officeCheckInCount = attendanceRecords
                        .Where(a => a.WorkType == "ESSL" || a.WorkType.Contains("ESSL"))
                        //.Select(a => a.EmpCode)
                        //.Distinct()
                        .Count();

                    var wfhData = attendanceRecords
                        .Where(a => a.WorkType == "WFH" || a.WorkType.Contains("WFH"))
                        //.Select(a => a.EmpCode)
                        //.Distinct()
                        .Count();

                    var onsiteData = attendanceRecords
                        .Where(a => a.WorkType == "ONSITE" || a.WorkType.Contains("ONSITE"))
                        //.Select(a => a.EmpCode)
                        //.Distinct()
                        .Count();

                    string workedHoursFormatted = SumTimeSpans(attendanceRecords
                        .Where(a => a.WorkingHoursDecimal > 0)
                        .Select(a => a.ActiveHours));

                    double daysInRange = (endDate - startDate).TotalDays;
                    int totalMaxSeconds = (int)(totalEmployeeCount * 8.5 * 3600 * daysInRange);
                    TimeSpan maxWorkTimeSpan = TimeSpan.FromSeconds(totalMaxSeconds);
                    string maxWorkingHoursFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        (int)maxWorkTimeSpan.TotalHours,
                        maxWorkTimeSpan.Minutes,
                        maxWorkTimeSpan.Seconds);

                    // On-time check-in data
                    var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                        .Select(offset => startDate.AddDays(offset))
                        .ToList();

                    var onTimeCheckInData = allDates
                        .Select(date => new
                        {
                            Date = date.ToString("yyyy-MM-dd"),
                            OnTimeCheckInCount = attendanceRecords
                                .Where(a => a.LogDate == date &&
                                           a.LogInTime != "00:00:00" &&
                                           TimeSpan.Parse(a.LogInTime) <= new TimeSpan(9, 30, 0))
                                .Count(),
                            LateCheckInCount = attendanceRecords
                                .Where(a => a.LogDate == date &&
                                           a.LogInTime != "00:00:00" &&
                                           TimeSpan.Parse(a.LogInTime) > new TimeSpan(9, 30, 0))
                                .Count()
                        })
                        .ToList();

                    // Get HR Data for Overall View
                    var hrData = GetHRDataForOverall(employeeList, locationIds, model.EmpId, LoginId ?? 0, desigId, deptId, locationid, startDate, endDate);

                    // Get Shift Management Data
                    var shiftManagementData = GetShiftManagementData(employeeList, locationIds, startDate, endDate);

                    DateTime yesterday = DateTime.Now.Date.AddDays(-1);

                    
                    var attendancedetails = DB.DailyAttendanceRecords
                        .Where(a => a.LogDate >= yesterday.Date && (a.WorkType == "ESSL" || a.WorkType.Contains("ESSL"))
                                 && employeeList.Contains(a.EmpCode))
                        .ToList();

                    var leavedetails = DB.EmpLeaveApplications
                        .Where(a => a.AppliedDate >= yesterday.Date
                                 && employeeList.Contains(a.EmpCode))
                        .ToList();

                    var attendancewfhdetails = DB.DailyAttendanceRecords
                        .Where(a => a.LogDate >= yesterday.Date && (a.WorkType == "WFH" || a.WorkType.Contains("WFH"))
                                 && employeeList.Contains(a.EmpCode))
                        .ToList();

                    var attendanceonsitedetails = DB.DailyAttendanceRecords
                        .Where(a => a.LogDate >= yesterday.Date && (a.WorkType == "ONSITE" || a.WorkType.Contains("ONSITE"))
                                 && employeeList.Contains(a.EmpCode))
                        .ToList();

                    int? totalyesterdaycount = employeeList.Count();
                    int? presentyesterday = attendancedetails.Count();
                    int? onleaveyesterday = leavedetails.Count();
                    int? WFHyesterday = attendancewfhdetails.Count();
                    int? ONSITEyesterday = attendanceonsitedetails.Count();
                    int? Absentyesterday = (totalyesterdaycount - (presentyesterday + onleaveyesterday + WFHyesterday + ONSITEyesterday));


                    var result = new List<object>
                    {
                        new { AttendanceSource = new
                            {
                                TotalEmployeeCount = totalEmployeeCount,
                                DeviceCheckInCount = officeCheckInCount,
                                OnSiteCount = onsiteData,
                                WFHCount = wfhData
                            }
                        },
                        new { YesterdayAttendanceDetails = new
                            {
                                PresentYesterday = presentyesterday,
                                AbsentYesterday = Absentyesterday,
                                OnLeaveYesterday = onleaveyesterday,
                                //NotMarkedYesterday = 0,
                                WFHYesterday = WFHyesterday,
                                ONSITEYesterday = ONSITEyesterday
                            }
                        },
                        new { CurrentMonthWorkedHours = new
                            {
                                TotalWH = workedHoursFormatted,
                                MaxWH = maxWorkingHoursFormatted
                            }
                        },
                        new { OnTimeCheckIn = onTimeCheckInData },
                        new { GetvisitorToday = hrData.VisitorToday },
                        new { CurrentmonthemployeeList = hrData.MonthEmployees },
                        new { PendingLeaves = hrData.PendingLeaves },
                        new { AllLeaves = hrData.AllLeaves },
                        new { CompOffList = hrData.CompOffList },
                        new { ShiftManagement = shiftManagementData } // Added Shift Management
                    };

                    return result;
                }
                else
                {
                    // Single employee view
                    var employee = DB.EmployeeMasters
                        .FirstOrDefault(emp => emp.EmpId == LoginId &&
                                               emp.EmpStatus.ToUpper() == "ACTIVE" &&
                                               emp.IsActive == true &&
                                               emp.IsDeleted == false);

                    if (employee == null)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found");
                    }

                    // Get employee's attendance records from cached table
                    var attendanceRecords = DB.DailyAttendanceRecords
                        .Where(a => a.EmpCode.ToUpper() == empcode.ToUpper()
                                 && a.LogDate >= startDate.Date
                                 && a.LogDate <= endDate.Date)
                        .ToList();

                    // Calculate metrics
                    var officeCheckInCount = attendanceRecords
                        .Where(a => a.WorkType == "ESSL" || a.WorkType.Contains("ESSL"))
                        .Count();

                    var wfhCount = attendanceRecords
                        .Where(a => a.WorkType == "WFH" || a.WorkType.Contains("WFH"))
                        .Count();

                    var onsiteCount = attendanceRecords
                        .Where(a => a.WorkType == "ONSITE" || a.WorkType.Contains("ONSITE"))
                        .Count();

                    string workedHoursFormatted = SumTimeSpans(attendanceRecords
                        .Where(a => a.WorkingHoursDecimal > 0)
                        .Select(a => a.ActiveHours));

                    double daysInRange = (endDate - startDate).TotalDays;
                    int totalMaxSeconds = (int)(8.5 * 3600 * daysInRange);
                    TimeSpan maxWorkTimeSpan = TimeSpan.FromSeconds(totalMaxSeconds);
                    string maxWorkingHoursFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        (int)maxWorkTimeSpan.TotalHours,
                        maxWorkTimeSpan.Minutes,
                        maxWorkTimeSpan.Seconds);

                    // On-time check-in data
                    var allDates = Enumerable.Range(0, (endDate.Date - startDate.Date).Days + 1)
                        .Select(offset => startDate.AddDays(offset))
                        .ToList();

                    var onTimeCheckInData = allDates
                        .Select(date => new
                        {
                            Date = date.ToString("yyyy-MM-dd"),
                            OnTimeCheckInCount = attendanceRecords
                                .Where(a => a.LogDate == date &&
                                           a.LogInTime != "00:00:00" &&
                                           TimeSpan.Parse(a.LogInTime) <= new TimeSpan(9, 30, 0))
                                .Count(),
                            LateCheckInCount = attendanceRecords
                                .Where(a => a.LogDate == date &&
                                           a.LogInTime != "00:00:00" &&
                                           TimeSpan.Parse(a.LogInTime) > new TimeSpan(9, 30, 0))
                                .Count()
                        })
                        .ToList();

                    // Get HR Data for Individual View
                    var hrData = GetHRDataForIndividual(LoginId ?? 0, desigId, deptId, locationid, startDate, endDate);

                    var result = new List<object>
                    {
                        new { AttendanceSource = new
                            {
                                DeviceCheckInCount = officeCheckInCount,
                                OnSiteCount = onsiteCount,
                                WFHCount = wfhCount
                            }
                        },
                        new { CurrentMonthWorkedHours = new
                            {
                                TotalWH = workedHoursFormatted,
                                MaxWH = maxWorkingHoursFormatted
                            }
                        },
                        new { OnTimeCheckIn = onTimeCheckInData },
                        new { GetvisitorToday = hrData.VisitorToday },
                        new { CurrentmonthemployeeList = hrData.MonthEmployees },
                        new { PendingLeaves = hrData.PendingLeaves },
                        new { AllLeaves = hrData.AllLeaves },
                        new { CompOffList = hrData.CompOffList }
                    };

                    return result;
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        private List<object> GetShiftManagementData(List<string> employeeList, List<int> locationIds, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Build the location filter condition
                var hasLocationFilter = locationIds != null && locationIds.Any() && locationIds.Any(id => id > 0);

                // Get the data with proper joins
                var shiftData = (from emp in DB.EmployeeMasters
                                 join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId
                                 join sm in DB.ShiftMasters on esd.ShiftId equals sm.ShiftId
                                 join sgm in DB.ShiftGroupingMasters on sm.ShiftId equals sgm.ShiftId
                                 where employeeList.Contains(emp.EmpCode) &&
                                       (!hasLocationFilter || (emp.LocationId.HasValue && locationIds.Contains(emp.LocationId.Value))) &&
                                       esd.StartDate <= endDate &&
                                       (esd.EndDate >= startDate || esd.EndDate == null) &&
                                       esd.Status == true &&
                                       esd.IsActive == true &&
                                       esd.IsDeleted == false &&
                                       sm.Status == true &&
                                       sm.IsActive == true &&
                                       sm.IsDeleted == false &&
                                       sgm.Status == true &&
                                       sgm.IsActive == true &&
                                       sgm.IsDeleted == false
                                 select new
                                 {
                                     emp.EmpId,
                                     sm.ShiftId,
                                     sm.ShiftName,
                                     sm.ClkHrs,
                                     sm.Days,
                                     sm.StartTime,
                                     sm.EndTime
                                 })
                                 .ToList(); // Execute to get the data

                // Group and format the data
                var result = shiftData
                    .GroupBy(x => new
                    {
                        x.ShiftId,
                        x.ShiftName,
                        x.ClkHrs,
                        x.Days,
                        x.StartTime,
                        x.EndTime
                    })
                    .Select(g => new
                    {
                        shiftId = g.Key.ShiftId,
                        Shift = g.Key.ShiftName,
                        ShiftClkHrs = FormatClockHours(g.Key.ClkHrs),
                        Shiftdays = g.Key.Days ?? "",
                        ShiftTime = FormatShiftTime(g.Key.StartTime, g.Key.EndTime),
                        ShiftEmpCount = g.Select(x => x.EmpId).Distinct().Count(),
                        ShiftStartTime = FormatTimeOnly(g.Key.StartTime),
                        ShiftEndTime = FormatTimeOnly(g.Key.EndTime)
                    })
                    .OrderBy(x => x.shiftId)
                    .ToList<object>();

                return result;
            }
            catch (Exception ex)
            {
                // Log exception
                return new List<object>();
            }
        }

        // Helper methods
        private string FormatClockHours(object clkHrs)
        {
            if (clkHrs == null) return "00:00";

            try
            {
                decimal hours = Convert.ToDecimal(clkHrs);
                int wholeHours = (int)hours;
                int minutes = (int)((hours - wholeHours) * 60);
                return $"{wholeHours:D2}:{minutes:D2}";
            }
            catch
            {
                return "00:00";
            }
        }

        private string FormatShiftTime(object startTime, object endTime)
        {
            if (startTime == null || endTime == null) return "Not Configured";

            string start = FormatTimeOnly(startTime);
            string end = FormatTimeOnly(endTime);

            return $"{start} - {end}";
        }

        private string FormatTimeOnly(object timeValue)
        {
            if (timeValue == null) return "00:00";

            string timeString = timeValue.ToString();

            // Handle TimeSpan format
            if (timeString.Contains("."))
            {
                timeString = timeString.Substring(0, timeString.IndexOf('.'));
            }

            // Return only HH:mm format
            if (timeString.Length >= 5)
            {
                return timeString.Substring(0, 5);
            }

            return "00:00";
        }
        private dynamic GetHRDataForOverall(List<string> employeeList, List<int> locationIds, int? specificEmpId, int loginId, int desigId, int deptId, int locationid, DateTime startDate, DateTime endDate)
        {
            DateTime today = DateTime.Today;
            DateTime firstDay = new DateTime(today.Year, today.Month, 1);
            DateTime nextMonth = firstDay.AddMonths(1);

            // Get employee IDs from the employee codes
            var employeeIds = DB.EmployeeMasters
                .Where(e => employeeList.Contains(e.EmpCode) && e.IsActive == true && e.IsDeleted == false)
                .Select(e => e.EmpId)
                .ToList();

            // Visitor Today - Filter by employees in the list
            var visitorToday = DB.VisitorManagements
                .Where(vm => vm.IsDeleted == false
                        && vm.Date >= startDate.Date
                        && vm.Date <= endDate.Date
                        && vm.WhomtoMeet.HasValue // Check if it has value
                        && employeeIds.Contains(vm.WhomtoMeet.Value)) // Use .Value to get int
                .OrderByDescending(vm => vm.VisitId)
                .Select(vm => new VisitorManagementViewModel
                {
                    Name = vm.Name,
                    OMail = vm.OMail,
                    Date = vm.Date,
                    Company = vm.Company,
                    Accept = vm.Accept,
                    Approved = vm.Approved,
                    Time = vm.Time
                }).ToList();

            // Month Employees - Filter by location and employee list
            var monthEmployeesQuery = DB.EmployeeMasters
                .Where(emp => emp.IsDeleted == false && employeeList.Contains(emp.EmpCode) &&
                    (
                        (emp.JoiningDate.HasValue &&
                         emp.JoiningDate.Value >= firstDay &&
                         emp.JoiningDate.Value < nextMonth)
                        ||
                        (emp.RelievedDate.HasValue &&
                         emp.RelievedDate.Value >= firstDay &&
                         emp.RelievedDate.Value < nextMonth)
                    ));

            if (locationIds != null && locationIds.Any() && locationIds.Any(id => id > 0))
            {
                monthEmployeesQuery = monthEmployeesQuery.Where(emp => locationIds.Contains(emp.LocationId ?? 0));
            }

            var monthEmployees = monthEmployeesQuery
                .Select(emp => new EmployeeMasterViewModel
                {
                    EmpCode = emp.EmpCode,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    JoiningDate = emp.JoiningDate,
                    RelievedDate = emp.RelievedDate,
                    EmpStatus = emp.EmpStatus,
                    Approver = emp.ReportName ?? "No Approve"
                }).ToList();

            // Leaves Queries
            IQueryable<EmpLeaveApplication> pendingLeaveQuery = DB.EmpLeaveApplications
                .Where(l => l.IsActive == true && l.IsDeleted == false && l.Status == "APPLIED");

            IQueryable<EmpLeaveApplication> allLeaveQuery = DB.EmpLeaveApplications
                .Where(l => l.IsActive == true && l.IsDeleted == false && l.Status != "APPLIED");

            // Apply employee list filter
            pendingLeaveQuery = pendingLeaveQuery.Where(l => employeeList.Contains(l.EmpCode));
            allLeaveQuery = allLeaveQuery.Where(l => employeeList.Contains(l.EmpCode));

            // Apply role-based filters
            if (desigId != 186)
            {
                if (deptId > 1)
                {
                    pendingLeaveQuery = pendingLeaveQuery.Where(l => l.ApprovedBy == loginId);
                    allLeaveQuery = allLeaveQuery.Where(l => l.ApprovedBy == loginId);
                }
                else
                {
                    pendingLeaveQuery = pendingLeaveQuery.Where(l =>
                        DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && e.LocationId == locationid));
                    allLeaveQuery = allLeaveQuery.Where(l =>
                        DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && e.LocationId == locationid));
                }
            }

            // Apply location filter
            if (locationIds != null && locationIds.Any() && locationIds.Any(id => id > 0))
            {
                pendingLeaveQuery = pendingLeaveQuery.Where(l =>
                    DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && locationIds.Contains(e.LocationId ?? 0)));
                allLeaveQuery = allLeaveQuery.Where(l =>
                    DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && locationIds.Contains(e.LocationId ?? 0)));
            }

            // Get pending leaves
            var pendingLeaves = (from l in pendingLeaveQuery
                                 where l.Status == "APPLIED" && l.StartDate.HasValue && l.EndDate.HasValue
                                 join e in DB.EmployeeMasters on l.EmpId equals e.EmpId
                                 join a in DB.EmployeeMasters on l.ApprovedBy equals a.EmpId into appr
                                 from ap in appr.DefaultIfEmpty()
                                 join lt in DB.LeaveTypeMasters on l.LeaveTypeId equals lt.LeaveTypeId into ltjoin
                                 from ltdata in ltjoin.DefaultIfEmpty()
                                 where l.AppliedDate >= startDate && l.AppliedDate <= endDate
                                 select new EmpLeaveApplicationViewModel
                                 {
                                     EmpName = e.FirstName,
                                     Approver = ap != null ? ap.FirstName : null,
                                     Status = l.Status,
                                     LeaveType = l.LeaveTypeId == 0 ? "LOP" : ltdata.LeaveName,
                                     StartDate = l.StartDate,
                                     EndDate = l.EndDate
                                 }).ToList();

            // Get all leaves
            var allLeaves = (from l in allLeaveQuery
                             where l.StartDate.HasValue && l.EndDate.HasValue
                             join e in DB.EmployeeMasters on l.EmpId equals e.EmpId
                             join a in DB.EmployeeMasters on l.ApprovedBy equals a.EmpId into appr
                             from ap in appr.DefaultIfEmpty()
                             join lt in DB.LeaveTypeMasters on l.LeaveTypeId equals lt.LeaveTypeId into ltjoin
                             from ltdata in ltjoin.DefaultIfEmpty()
                             where l.AppliedDate >= startDate && l.AppliedDate <= endDate
                             select new EmpLeaveApplicationViewModel
                             {
                                 EmpName = e.FirstName,
                                 Approver = ap != null ? ap.FirstName : null,
                                 Status = l.Status,
                                 LeaveType = l.LeaveTypeId == 0 ? "LOP" : ltdata.LeaveName,
                                 StartDate = l.StartDate,
                                 EndDate = l.EndDate
                             }).ToList();

            // CompOff List
            var compOffQuery = DB.CompOffRequests
                .Where(comp => comp.IsRequested == true && comp.IsActive == true && comp.IsDeleted == false && comp.Date.HasValue && comp.Date >= startDate && comp.Date <= endDate)
                .Where(comp => employeeList.Contains(comp.EmpCode));

            if (desigId != 186)
            {
                if (deptId > 1)
                {
                    compOffQuery = compOffQuery.Where(comp => comp.ManagerId == loginId);
                }
                else
                {
                    compOffQuery = compOffQuery.Where(comp =>
                        DB.EmployeeMasters.Any(e => e.EmpId == comp.EmpId && e.LocationId == locationid));
                }
            }

            if (locationIds != null && locationIds.Any() && locationIds.Any(id => id > 0))
            {
                compOffQuery = compOffQuery.Where(comp =>
                    DB.EmployeeMasters.Any(e => e.EmpId == comp.EmpId && locationIds.Contains(e.LocationId ?? 0)));
            }

            var compOffList = compOffQuery
                .Select(comp => new CompOffRequestViewModel
                {
                    CompOffReqId = comp.CompOffReqId,
                    EmpId = comp.EmpId,
                    EmpCode = comp.EmpCode,
                    EmpName = DB.EmployeeMasters
                                .Where(x => x.EmpId == comp.EmpId)
                                .Select(x => x.FirstName + " " + x.LastName)
                                .FirstOrDefault(),
                    ManagerId = comp.ManagerId,
                    ManagerCode = comp.ManagerCode,
                    Date = comp.Date,
                    Project = comp.Project,
                    Task = comp.Task,
                    Hrs = comp.Hrs,
                    ActualHrs = comp.ActualHrs,
                    WorkMode = comp.WorkMode,
                    IsRequested = comp.IsRequested,
                    IsApproved = comp.IsApproved,
                    IsRejected = comp.IsRejected
                }).ToList();

            return new
            {
                VisitorToday = visitorToday,
                MonthEmployees = monthEmployees,
                PendingLeaves = pendingLeaves,
                AllLeaves = allLeaves,
                CompOffList = compOffList
            };
        }

        private dynamic GetHRDataForIndividual(int loginId, int desigId, int deptId, int locationid, DateTime startDate, DateTime endDate)
        {
            DateTime today = DateTime.Today;
            DateTime firstDay = new DateTime(today.Year, today.Month, 1);
            DateTime nextMonth = firstDay.AddMonths(1);

            // Visitor Today - For specific employee
            var visitorToday = DB.VisitorManagements
                .Where(vm => vm.IsDeleted == false
                        && vm.Date >= startDate.Date
                        && vm.Date <= endDate.Date
                        && vm.WhomtoMeet == loginId)
                .OrderByDescending(vm => vm.VisitId)
                .Select(vm => new VisitorManagementViewModel
                {
                    Name = vm.Name,
                    OMail = vm.OMail,
                    Date = vm.Date,
                    Company = vm.Company,
                    Accept = vm.Accept,
                    Approved = vm.Approved,
                    Time = vm.Time
                }).ToList();

            // Month Employees - For specific employee
            var monthEmployees = DB.EmployeeMasters
                .Where(emp => emp.ReportId == loginId && emp.IsDeleted == false && emp.EmpStatus.ToUpper() == "ACTIVE" &&
                    (
                        (emp.JoiningDate.HasValue &&
                         emp.JoiningDate.Value >= firstDay &&
                         emp.JoiningDate.Value < nextMonth)
                        ||
                        (emp.RelievedDate.HasValue &&
                         emp.RelievedDate.Value >= firstDay &&
                         emp.RelievedDate.Value < nextMonth)
                    ))
                .Select(emp => new EmployeeMasterViewModel
                {
                    EmpCode = emp.EmpCode,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    JoiningDate = emp.JoiningDate,
                    RelievedDate = emp.RelievedDate,
                    EmpStatus = emp.EmpStatus,
                    Approver = emp.ReportName ?? "No Approve"
                }).ToList();

            // Leaves Queries for specific employee
            IQueryable<EmpLeaveApplication> pendingLeaveQuery = DB.EmpLeaveApplications
                .Where(l => l.EmpId == loginId && l.IsActive == true && l.IsDeleted == false && l.Status == "APPLIED");

            IQueryable<EmpLeaveApplication> allLeaveQuery = DB.EmpLeaveApplications
                .Where(l => l.EmpId == loginId && l.IsActive == true && l.IsDeleted == false && l.Status != "APPLIED");

            // Role-based filters for leaves
            if (desigId != 186)
            {
                if (deptId > 1)
                {
                    pendingLeaveQuery = pendingLeaveQuery.Where(l => l.EmpId == loginId);
                    allLeaveQuery = allLeaveQuery.Where(l => l.EmpId == loginId);
                }
                else
                {
                    pendingLeaveQuery = pendingLeaveQuery.Where(l =>
                        DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && e.LocationId == locationid));
                    allLeaveQuery = allLeaveQuery.Where(l =>
                        DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && e.LocationId == locationid));

                    monthEmployees = DB.EmployeeMasters
                    .Where(emp => emp.IsDeleted == false && emp.EmpStatus.ToUpper() == "ACTIVE" &&
                        (
                            (emp.JoiningDate.HasValue &&
                             emp.JoiningDate.Value >= firstDay &&
                             emp.JoiningDate.Value < nextMonth)
                            ||
                            (emp.RelievedDate.HasValue &&
                             emp.RelievedDate.Value >= firstDay &&
                             emp.RelievedDate.Value < nextMonth)
                        ))
                    .Select(emp => new EmployeeMasterViewModel
                    {
                        EmpCode = emp.EmpCode,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        JoiningDate = emp.JoiningDate,
                        RelievedDate = emp.RelievedDate,
                        EmpStatus = emp.EmpStatus,
                        Approver = emp.ReportName ?? "No Approve"
                    }).ToList();
                }
            }

            // Get pending leaves
            var pendingLeaves = (from l in pendingLeaveQuery
                                 where l.Status == "APPLIED" && l.StartDate.HasValue && l.EndDate.HasValue
                                 join e in DB.EmployeeMasters on l.EmpId equals e.EmpId
                                 join a in DB.EmployeeMasters on l.ApprovedBy equals a.EmpId into appr
                                 from ap in appr.DefaultIfEmpty()
                                 join lt in DB.LeaveTypeMasters on l.LeaveTypeId equals lt.LeaveTypeId into ltjoin
                                 from ltdata in ltjoin.DefaultIfEmpty()
                                 where l.AppliedDate >= startDate && l.AppliedDate <= endDate
                                 select new EmpLeaveApplicationViewModel
                                 {
                                     EmpName = e.FirstName,
                                     Approver = ap != null ? ap.FirstName : null,
                                     Status = l.Status,
                                     LeaveType = l.LeaveTypeId == 0 ? "LOP" : ltdata.LeaveName,
                                     StartDate = l.StartDate,
                                     EndDate = l.EndDate
                                 }).ToList();

            // Get all leaves
            var allLeaves = (from l in allLeaveQuery
                             where l.StartDate.HasValue && l.EndDate.HasValue
                             join e in DB.EmployeeMasters on l.EmpId equals e.EmpId
                             join a in DB.EmployeeMasters on l.ApprovedBy equals a.EmpId into appr
                             from ap in appr.DefaultIfEmpty()
                             join lt in DB.LeaveTypeMasters on l.LeaveTypeId equals lt.LeaveTypeId into ltjoin
                             from ltdata in ltjoin.DefaultIfEmpty()
                             where l.AppliedDate >= startDate && l.AppliedDate <= endDate
                             select new EmpLeaveApplicationViewModel
                             {
                                 EmpName = e.FirstName,
                                 Approver = ap != null ? ap.FirstName : null,
                                 Status = l.Status,
                                 LeaveType = l.LeaveTypeId == 0 ? "LOP" : ltdata.LeaveName,
                                 StartDate = l.StartDate,
                                 EndDate = l.EndDate
                             }).ToList();

            // CompOff List for specific employee
            var compOffList = DB.CompOffRequests
                .Where(comp => comp.IsRequested == true && comp.IsActive == true && comp.IsDeleted == false
                        && comp.Date.HasValue && comp.EmpId == loginId && comp.Date >= startDate && comp.Date <= endDate)
                .Select(comp => new CompOffRequestViewModel
                {
                    CompOffReqId = comp.CompOffReqId,
                    EmpId = comp.EmpId,
                    EmpCode = comp.EmpCode,
                    EmpName = DB.EmployeeMasters
                                .Where(x => x.EmpId == comp.EmpId)
                                .Select(x => x.FirstName + " " + x.LastName)
                                .FirstOrDefault(),
                    ManagerId = comp.ManagerId,
                    ManagerCode = comp.ManagerCode,
                    Date = comp.Date,
                    Project = comp.Project,
                    Task = comp.Task,
                    Hrs = comp.Hrs,
                    ActualHrs = comp.ActualHrs,
                    WorkMode = comp.WorkMode,
                    IsRequested = comp.IsRequested,
                    IsApproved = comp.IsApproved,
                    IsRejected = comp.IsRejected
                }).ToList();

            return new
            {
                VisitorToday = visitorToday,
                MonthEmployees = monthEmployees,
                PendingLeaves = pendingLeaves,
                AllLeaves = allLeaves,
                CompOffList = compOffList
            };
        }

        ////public List<object> GetConsolidatedAttendanceData(AttendanceFilterViewModel model)
        ////{
        ////    try
        ////    {
        ////        DateTime startDate = (Convert.ToDateTime(model.StartDate) != default(DateTime)) ? Convert.ToDateTime(model.StartDate) : DateTime.Today.AddMonths(-1);
        ////        DateTime endDate = (Convert.ToDateTime(model.EndDate) != default(DateTime)) ? Convert.ToDateTime(model.EndDate) : DateTime.Today;
        ////        string empCode = model.EmpCode;
        ////        List<int> locationIds = model.LocationId ?? new List<int>(); 
        ////        //int? locationId = (model.LocationId != 0) ? model.LocationId : 0;
        ////        int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        bool isSingleEmployeeSearch = !string.IsNullOrEmpty(empCode);
        ////        int? OldEmpId = DB.EmployeeMasters.Where(emp => emp.EmpId == LoginId && emp.EmpStatus.ToUpper() == "ACTIVE" && emp.IsActive == true && emp.IsDeleted == false).Select(x => x.OldEmp_ID ?? x.EmpId).FirstOrDefault();
        ////        string empcode = DB.EmployeeMasters.Where(emp => emp.EmpId == LoginId && emp.EmpStatus.ToUpper() == "ACTIVE" && emp.IsActive == true && emp.IsDeleted == false).Select(x => x.EmpCode).FirstOrDefault();

        ////        if (model.IsOverall)
        ////        {
        ////            var totalEmployeeCount = DB.EmployeeMasters.Count(emp => (emp.ReportName.ToUpper() == empCode.ToUpper()) && emp.IsActive == true && emp.IsDeleted == false);

        ////            var EmployeeList = DB.EmployeeMasters
        ////                                .Where(emp => (emp.ReportName.ToUpper() == empCode.ToUpper())
        ////                                           && emp.IsActive == true
        ////                                           && emp.IsDeleted == false)
        ////                                .Select(x => x.EmpCode)
        ////                                .ToList();

        ////            if (model.EmpId != 0)
        ////            {
        ////                EmployeeList = DB.EmployeeMasters
        ////                                .Where(emp => (emp.ReportName.ToUpper() == empCode.ToUpper())
        ////                                           && emp.EmpId == model.EmpId
        ////                                           && emp.IsActive == true
        ////                                           && emp.IsDeleted == false)
        ////                                .Select(x => x.EmpCode)
        ////                                .ToList();
        ////            }

        ////            if (locationIds != null && locationIds.Any())
        ////            {
        ////                totalEmployeeCount = DB.EmployeeMasters.Count(emp => (emp.ReportName.ToUpper() == empCode) && locationIds.Contains(emp.LocationId ?? 0) && emp.IsActive == true && emp.IsDeleted == false);

        ////                EmployeeList = DB.EmployeeMasters
        ////                                .Where(emp => (emp.ReportName.ToUpper() == empCode.ToUpper())
        ////                                        && locationIds.Contains(emp.LocationId ?? 0)
        ////                                           && emp.EmpId == model.EmpId
        ////                                           && emp.IsActive == true
        ////                                           && emp.IsDeleted == false)
        ////                                .Select(x => x.EmpCode)
        ////                                .ToList();
        ////            }

        ////            var attendanceRecords = DB.Attendances
        ////                   .Where(a => a.LogDate >= startDate && a.LogDate <= endDate && a.Type == "IN" && EmployeeList.Contains(a.EmpCode))
        ////                   .Select(a => new { a.LogDate, a.LogTime, a.LogID, a.Type, a.EmpCode })
        ////                   .Distinct()
        ////                   .ToList();

        ////            var attendanceTimeRecords = DB.Emp_AttendanceTime
        ////                .Where(a => a.LogDate >= startDate.Date && a.LogDate <= endDate.Date && EmployeeList.Contains(a.EmpCode))
        ////                .Select(a => new { a.LogDate, a.AttendHours, a.AttendMins, a.AttendSec, a.EmpCode, a.Duration})
        ////                .ToList();

        ////            var wfhData = (from wfh in DB.WFHLoginlogs
        ////                           join emp in EmployeeList on wfh.EmpCode equals emp
        ////                           where wfh.Date >= startDate && wfh.Date <= endDate
        ////                           select wfh.EmpCode)
        ////                          .Distinct()
        ////                          .Count();

        ////            var onsiteData = (from onsite in DB.OnSiteLoginlogs
        ////                              join empId in EmployeeList on onsite.EmpCode equals empId
        ////                              where onsite.LoginDate >= startDate && onsite.LoginDate <= endDate
        ////                              select onsite.EmpCode)
        ////                             .Distinct()
        ////                             .Count();

        ////            var officeCheckInCount = attendanceRecords.Where(log => log.Type == "IN").Select(log => log.EmpCode).Distinct().Count();

        ////            var totalWorkedMinutes = attendanceTimeRecords
        ////                    .Where(a => a.LogDate >= startDate && a.LogDate <= endDate)
        ////                    .Sum(a => (a.AttendHours * 60) + a.AttendMins); // Convert to total minutes

        ////            // Convert total minutes to TimeSpan
        ////            var workedTime = TimeSpan.FromMinutes(Convert.ToDouble(totalWorkedMinutes));

        ////            // Format as decimal hours (e.g., 12.50 hrs)
        ////            double workedHours = Math.Round(workedTime.TotalHours, 2);

        ////            double days1 = (endDate - startDate).TotalDays;

        ////            double maxWorkingHours = Convert.ToDouble(totalEmployeeCount) * 8.30 * (days1);

        ////            var employeeQuery = DB.EmployeeMasters.Where(emp => emp.IsActive == true);

        ////            if (isSingleEmployeeSearch)
        ////            {
        ////                employeeQuery = employeeQuery.Where(emp => emp.EmpCode.Equals(empCode, StringComparison.OrdinalIgnoreCase));
        ////            }

        ////            if (locationIds != null && locationIds.Any())
        ////            {
        ////                employeeQuery = employeeQuery.Where(emp => locationIds.Contains(emp.LocationId ?? 0));
        ////            }

        ////            var employees = employeeQuery.Select(emp => new { emp.EmpId, emp.EmpCode }).ToList();

        ////            var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
        ////                .Select(offset => startDate.AddDays(offset))
        ////                .ToList();

        ////            var attendanceSummary = attendanceRecords
        ////                .Where(record => record.LogTime.HasValue)
        ////                .GroupBy(record => record.LogDate.Value.Date)
        ////                .ToDictionary(
        ////                    group => group.Key,
        ////                    group => new
        ////                    {
        ////                        OnTimeCheckInCount = group.Count(record => record.LogTime.Value.TimeOfDay <= new TimeSpan(9, 30, 0)),
        ////                        LateCheckInCount = group.Count(record => record.LogTime.Value.TimeOfDay > new TimeSpan(9, 30, 0))
        ////                    }
        ////                );

        ////            var onTimeCheckInData = allDates
        ////                .Select(date => new
        ////                {
        ////                    Date = date.ToString("yyyy-MM-dd"),
        ////                    OnTimeCheckInCount = attendanceSummary.ContainsKey(date) ? attendanceSummary[date].OnTimeCheckInCount : 0,
        ////                    LateCheckInCount = attendanceSummary.ContainsKey(date) ? attendanceSummary[date].LateCheckInCount : 0
        ////                })
        ////                .ToList();

        ////            var result = new List<object>
        ////                            {
        ////                                new
        ////                                {
        ////                                    AttendanceSource = new
        ////                                    {
        ////                                        TotalEmployeeCount = totalEmployeeCount,
        ////                                        DeviceCheckInCount = officeCheckInCount,
        ////                                        OnSiteCount = onsiteData,
        ////                                        WFHCount = wfhData
        ////                                    }
        ////                                },
        ////                                new
        ////                                {
        ////                                    CurrentMonthWorkedHours = new
        ////                                    {
        ////                                        TotalWH = workedHours,
        ////                                        MaxWH = maxWorkingHours
        ////                                    }
        ////                                },
        ////                                new
        ////                                {
        ////                                    OnTimeCheckIn = onTimeCheckInData
        ////                                }
        ////                            };

        ////            return result;
        ////        }
        ////        else
        ////        {
        ////            var employee = DB.EmployeeMasters
        ////                .FirstOrDefault(emp => emp.EmpId == LoginId && emp.EmpStatus.ToUpper() == "ACTIVE" && emp.IsActive == true && emp.IsDeleted == false);

        ////            if (employee == null)
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found");
        ////            }

        ////            var attendanceRecords = DB.Attendances
        ////                .Where(a => a.EmpCode.ToUpper() == empcode.ToUpper() && a.LogDate >= startDate.Date && a.LogDate <= endDate.Date)
        ////                .Select(a => new { a.LogDate, a.LogTime, a.Type, a.EmpCode })
        ////                .ToList();

        ////            var attendanceTimeRecords = DB.Emp_AttendanceTime
        ////                .Where(a => a.EmpCode.ToUpper() == empcode.ToUpper() && a.LogDate >= startDate.Date && a.LogDate <= endDate.Date)
        ////                .Select(a => new { a.LogDate, a.AttendHours, a.AttendMins, a.AttendSec, a.EmpCode, a.Duration })
        ////                .ToList();

        ////            var wfhCount = DB.WFHLoginlogs
        ////                .Where(wfh => wfh.EmpCode.ToUpper() == empcode.ToUpper() && wfh.Date >= startDate.Date && wfh.Date <= endDate.Date)
        ////                .Count();

        ////            //var onsiteCount = DB.Loginlogs
        ////            //    .Where(onsite => onsite.EmpId == employee.EmpId && onsite.LoginDate >= startDate.Date && onsite.LoginDate <= endDate.Date)
        ////            //    .Count();

        ////            var onsiteCount = DB.OnSiteLoginlogs
        ////                                .Where(onsite => onsite.EmpCode.ToUpper() == empcode.ToUpper() && onsite.LoginDate >= startDate.Date && onsite.LoginDate <= endDate.Date)
        ////                                .GroupBy(onsite => onsite.LoginDate) // Group by Date to ensure only one per day
        ////                                                                     //.Select(g => g.First()) // Take only one record per group
        ////                                .Count();

        ////            //var officeCheckInCount = DB.Attendances
        ////            //    .Where(a => a.LogID == OldEmpId && a.LogDate >= startDate.Date && a.LogDate <= endDate.Date && a.Type == "IN")
        ////            //    .Count(); //04.06.2025

        ////            var officeCheckInCount = DB.Attendances
        ////                                    .Where(a => a.EmpCode.ToUpper() == empcode.ToUpper()
        ////                                             && a.Type == "IN"
        ////                                             && a.LogDate >= startDate.Date
        ////                                             && a.LogDate <= endDate.Date
        ////                                             && a.LogDate != a.LogTime)
        ////                                    .Select(a => a.LogDate)
        ////                                    .Distinct()
        ////                                    .Count();

        ////            //var officeCheckInCount = DB.Attendances
        ////            //                        .Any(a => a.LogID == OldEmpId
        ////            //                               && a.LogDate >= startDate.Date
        ////            //                               && a.LogDate <= endDate.Date
        ////            //                               && a.Type == "IN") ? 1 : 0;

        ////            //var workedHours = attendanceRecords
        ////            //    .Where(a => a.Type == "IN" || a.Type == "OUT")
        ////            //    .GroupBy(a => new { a.LogDate })
        ////            //    .Select(g =>
        ////            //    .Select(g =>
        ////            //    {
        ////            //        var inLog = g.Where(x => x.Type == "IN").OrderBy(x => x.LogTime).FirstOrDefault();
        ////            //        var outLog = g.Where(x => x.Type == "OUT").OrderByDescending(x => x.LogTime).FirstOrDefault();

        ////            //        if (inLog != null && outLog != null && outLog.LogTime > inLog.LogTime)
        ////            //        {
        ////            //            return (outLog.LogTime - inLog.LogTime)?.TotalHours ?? 0;
        ////            //        }

        ////            //        return 0;
        ////            //    })
        ////            //    .Sum();


        ////            var totalWorkedMinutes = attendanceTimeRecords
        ////                    .Where(a => a.LogDate >= startDate && a.LogDate <= endDate)
        ////                    .Sum(a => (a.AttendHours * 60) + a.AttendMins); // Convert to total minutes

        ////            string totalWorkedHoursFormatted = "";

        ////            //if (totalWorkedMinutes == 0)
        ////            //{
        ////            //    // Step 1: Get total minutes from Activehrs
        ////            //    var totalMinutes = DB.WFHLoginlogs
        ////            //        .Where(w => w.EmpId == EmpId && w.Date >= startDate && w.Date <= endDate && w.Activehrs.HasValue)
        ////            //        .Select(w => w.Activehrs.Value.TotalMinutes)
        ////            //        .Sum();

        ////            //    // Step 2: Convert total minutes to TimeSpan
        ////            //    var totalTimeSpan = TimeSpan.FromMinutes(totalMinutes);

        ////            //    // Step 3: Format as "HH:mm"
        ////            //    totalWorkedHoursFormatted = string.Format("{0:D2}:{1:D2}", (int)totalTimeSpan.TotalHours, totalTimeSpan.Minutes);

        ////            //    totalWorkedMinutes = (int)totalMinutes;
        ////            //}
        ////            ////else if (totalWorkedMinutes == 0)
        ////            ////{
        ////            ////    totalWorkedMinutes = (int?)Math.Round(DB.OnSiteLoginlogs
        ////            ////                            .Where(a => a.EmpId == OldEmpId && a.Date >= startDate.Date && a.Date <= endDate.Date)
        ////            ////                            .Select(a => a.Activehrs.HasValue ? a.Activehrs.Value.TotalMinutes : 0)
        ////            ////                            .Sum());
        ////            ////}

        ////            // Convert total minutes to TimeSpan
        ////            var workedTime = TimeSpan.FromMinutes(Convert.ToDouble(totalWorkedMinutes));

        ////            // Format as decimal hours (e.g., 12.50 hrs)
        ////            double workedHours = Math.Round(workedTime.TotalHours, 2);


        ////            double days1 = (endDate - startDate).TotalDays;

        ////            double maxWorkingHours = (8.30 * days1);

        ////            var allDates = Enumerable.Range(0, (endDate.Date - startDate.Date).Days + 1)
        ////                .Select(offset => startDate.AddDays(offset))
        ////                .ToList();

        ////            var attendanceSummary = attendanceRecords
        ////                .Where(record => record.LogTime.HasValue)
        ////                .GroupBy(record => record.LogDate.Value.Date)
        ////                .ToDictionary(
        ////                    group => group.Key,
        ////                    group => new
        ////                    {
        ////                        OnTimeCheckInCount = group.Count(record => record.LogTime.Value.TimeOfDay <= new TimeSpan(9, 30, 0)),
        ////                        LateCheckInCount = group.Count(record => record.LogTime.Value.TimeOfDay > new TimeSpan(9, 30, 0))
        ////                    }
        ////                );

        ////            var onTimeCheckInData = allDates
        ////                .Select(date => new
        ////                {
        ////                    Date = date.ToString("yyyy-MM-dd"),
        ////                    OnTimeCheckInCount = attendanceSummary.ContainsKey(date) ? attendanceSummary[date].OnTimeCheckInCount : 0,
        ////                    LateCheckInCount = attendanceSummary.ContainsKey(date) ? attendanceSummary[date].LateCheckInCount : 0
        ////                })
        ////                .ToList();

        ////            var result = new List<object>
        ////                            {
        ////                                new
        ////                                {
        ////                                    AttendanceSource = new
        ////                                    {
        ////                                        DeviceCheckInCount = officeCheckInCount,
        ////                                        OnSiteCount = onsiteCount,
        ////                                        WFHCount = wfhCount
        ////                                    }
        ////                                },
        ////                                new
        ////                                {
        ////                                    CurrentMonthWorkedHours = new
        ////                                    {
        ////                                        TotalWH = workedHours,
        ////                                        MaxWH = maxWorkingHours
        ////                                    }
        ////                                },
        ////                                new
        ////                                {
        ////                                    OnTimeCheckIn = onTimeCheckInData
        ////                                }
        ////                            };

        ////            return result;
        ////        }

        ////    }

        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}

        //public LogActivityResponseViewModel LogActivity(LogActivityViewModel activity)
        //{
        //    try
        //    {
        //        if (activity == null || string.IsNullOrEmpty(activity.EmpCode) || string.IsNullOrEmpty(activity.Action))
        //        {
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid request data.");
        //        }

        //        var employee = DB.EmployeeMasters.FirstOrDefault(e => e.EmpCode.ToUpper() == activity.EmpCode.ToUpper() && e.IsActive == true && e.IsDeleted == false);

        //        if (employee == null)
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found.");
        //        }

        //        DateTime currentTime = DateTime.Now;

        //        string action = activity.Action.ToUpper();
        //        if (action != "LOGIN" && action != "LOGOUT")
        //        {
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid action. Use 'LOGIN' or 'LOGOUT'.");
        //        }

        //        var logRecord = new LoginLogoutRecord
        //        {
        //            EmpId = employee.EmpId,
        //            EmpCode = employee.EmpCode,
        //            Address = activity.Address,
        //            City = activity.City,
        //            Date = currentTime.Date,
        //            LogInTime = action == "LOGIN" ? (DateTime?)currentTime : null,
        //            LogOutTime = action == "LOGOUT" ? (DateTime?)currentTime : null,
        //            ActionType = action,
        //            CreatedBy = activity.CreatedBy,
        //            CreatedDate = currentTime,
        //            LastUpdatedBy = activity.CreatedBy,
        //            LastUpdatedDate = currentTime,
        //            IsActive = true,
        //            IsUpdated = false,
        //            IsDeleted = false
        //        };

        //        DB.LoginLogoutRecords.Add(logRecord);
        //        DB.SaveChanges();

        //        return new LogActivityResponseViewModel
        //        {
        //            Message = $"{action} action recorded successfully.",
        //            EmpCode = logRecord.EmpCode,
        //            Action = logRecord.ActionType,
        //            LogInTime = logRecord.LogInTime.HasValue ? logRecord.LogInTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        //            LogOutTime = logRecord.LogOutTime.HasValue ? logRecord.LogOutTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        //            Address = logRecord.Address,
        //            City = logRecord.City
        //        };
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}

        public List<SelectEmployeeViewModel> DashboardEmployee(SelectEmployeeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  join loc in DB.LocationMasters on emp.LocationId equals loc.LocationId
                                  where emp.IsActive == true && emp.IsDeleted == false
                                  select emp).OrderByDescending(x => x.EmpId).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<SelectEmployeeViewModel> lstofEmp = new List<SelectEmployeeViewModel>();

                        for (int i = 0; i < empdetails.Count(); i++)
                        {
                            SelectEmployeeViewModel emvm = new SelectEmployeeViewModel();
                            emvm.EmpId = empdetails[i].EmpId;
                            emvm.CompId = empdetails[i].CompId;
                            emvm.Company = DB.CompanyMasters.Where(x => x.CompId == emvm.CompId).Select(x => x.Company).FirstOrDefault();
                            emvm.DeptName = empdetails[i].DeptName;
                            emvm.ReportId = empdetails[i].ReportId;
                            emvm.EmpCode = empdetails[i].EmpCode;
                            emvm.EmpName = empdetails[i].FirstName + " " + empdetails[i].MiddleName + " " + empdetails[i].LastName;
                            emvm.IsActive = empdetails[i].IsActive;
                            emvm.IsUpdated = empdetails[i].IsUpdated;
                            emvm.IsDeleted = empdetails[i].IsDeleted;

                            lstofEmp.Add(emvm);
                        }

                        return lstofEmp;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail Not Found");
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
        public List<AttendanceFilterViewModel> DashboardDetails(AttendanceFilterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = model.LoginId != 0 ? model.LoginId : 0;
                DateTime startDate = Convert.ToDateTime(model.StartDate) != default(DateTime) ? Convert.ToDateTime(model.StartDate) : DateTime.Today.AddMonths(-1);
                DateTime endDate = Convert.ToDateTime(model.EndDate) != default(DateTime) ? Convert.ToDateTime(model.EndDate) : DateTime.Today;
                int? EmpId = model.LoginId != 0 ? model.LoginId : (int?)null;

                int? OldEmpId = DB.EmployeeMasters
                   .Where(emp => emp.EmpId == EmpId && emp.IsActive == true && emp.IsDeleted == false)
                   .Select(emp => emp.OldEmp_ID)
                   .FirstOrDefault();

                var deviceCheckInEmployees = DB.Attendances
                   .Where(a => a.LogDate >= startDate && a.LogDate <= endDate && a.Type == "IN")
                   .Select(a => a.LogID)
                   .Distinct()
                   .ToList();

                var onSiteEmployees = DB.Loginlogs
                   .Where(l => l.LoginDate >= startDate && l.LoginDate <= endDate)
                   .Select(l => l.EmpId)
                   .Distinct()
                   .ToList();

                var wfhEmployees = DB.WFHLoginlogs
                   .Where(wfh => wfh.Date >= startDate && wfh.Date <= endDate)
                   .Select(wfh => wfh.EmpId)
                   .Distinct()
                   .ToList();

                var attendanceTimes = DB.Attendances
               .Where(a => a.LogDate >= startDate && a.LogDate <= endDate)
               .GroupBy(a => a.LogID)
               .ToList()
               .Select(g => new
               {
                   LogID = g.Key,
                   Date = g.Select(a => a.LogDate).FirstOrDefault(),
                   LoginTime = g.Where(a => a.Type == "IN")
                                .OrderBy(a => a.LogDate).ThenBy(a => a.LogTime)
                                .Select(a => a.LogTime).FirstOrDefault(),
                   LogoutTime = g.Where(a => a.Type == "OUT")
                                 .OrderByDescending(a => a.LogDate).ThenByDescending(a => a.LogTime)
                                 .Select(a => a.LogTime).FirstOrDefault()

               })
               .ToList();

                var deviceCheckInDetails = DB.EmployeeMasters
                 .Where(emp => deviceCheckInEmployees.Contains(emp.OldEmp_ID))
                 .ToList()
                 .Select(emp => new EmployeeDetailsViewModel
                 {
                     EmpId = emp.EmpId,
                     EmpCode = emp.EmpCode,
                     FullName = $"{emp.FirstName} {emp.MiddleName} {emp.LastName}".Trim(),
                     DesignationName = emp.DesignationName,
                     DeptName = emp.DeptName,
                     Date = attendanceTimes.FirstOrDefault(a => a.LogID == emp.OldEmp_ID)?.Date.ToString() ?? "0000-00-00",  // Add Date
                     LoginTime = attendanceTimes.FirstOrDefault(a => a.LogID == emp.OldEmp_ID)?.LoginTime?.ToString(@"hh\:mm\:ss") ?? "00:00:00",
                     LogoutTime = attendanceTimes.FirstOrDefault(a => a.LogID == emp.OldEmp_ID)?.LogoutTime?.ToString(@"hh\:mm\:ss") ?? "00:00:00"
                 })
                 .ToList();

                var onSiteDetails = DB.EmployeeMasters
                  .Where(emp => onSiteEmployees.Contains(emp.EmpId))
                  .ToList()
                  .Select(emp => new EmployeeDetailsViewModel
                  {
                      EmpId = emp.EmpId,
                      EmpCode = emp.EmpCode,
                      FullName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                      DesignationName = emp.DesignationName,
                      DeptName = emp.DeptName,
                      Date = DB.Loginlogs
                                 .Where(l => l.EmpId == emp.EmpId && l.LoginDate >= startDate && l.LoginDate <= endDate)
                                 .OrderBy(l => l.LoginDate).ThenBy(l => l.LogInTime)
                                 .Select(l => l.LoginDate).FirstOrDefault()?.ToString("yyyy-MM-dd") ?? "0000-00-00",  // Add Date
                      LoginTime = DB.Loginlogs
                                  .Where(l => l.EmpId == emp.EmpId && l.LoginDate >= startDate && l.LoginDate <= endDate)
                                  .OrderBy(l => l.LoginDate).ThenBy(l => l.LogInTime)
                                  .Select(l => l.LogInTime).FirstOrDefault()?.ToString(@"hh\:mm\:ss") ?? "00:00:00",
                      LogoutTime = DB.Loginlogs
                                   .Where(l => l.EmpId == emp.EmpId && l.LoginDate >= startDate && l.LoginDate <= endDate)
                                   .OrderByDescending(l => l.LoginDate).ThenByDescending(l => l.LogInTime)
                                   .Select(l => l.LogOutTime).FirstOrDefault()?.ToString(@"hh\:mm\:ss") ?? "00:00:00"
                  })
                  .ToList();

                var wfhDetails = DB.EmployeeMasters
                  .Where(emp => wfhEmployees.Contains(emp.EmpId))
                  .ToList()
                  .Select(emp => new EmployeeDetailsViewModel
                  {
                      EmpId = emp.EmpId,
                      EmpCode = emp.EmpCode,
                      FullName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                      DesignationName = emp.DesignationName,
                      DeptName = emp.DeptName,
                      Date = DB.WFHLoginlogs
                                 .Where(wfh => wfh.EmpId == emp.EmpId && wfh.Date >= startDate && wfh.Date <= endDate)
                                 .OrderBy(wfh => wfh.Date).ThenBy(wfh => wfh.LoginTime)
                                 .Select(wfh => wfh.Date).FirstOrDefault()?.ToString("yyyy-MM-dd") ?? "0000-00-00",  // Add Date
                      LoginTime = DB.WFHLoginlogs
                                  .Where(wfh => wfh.EmpId == emp.EmpId && wfh.Date >= startDate && wfh.Date <= endDate)
                                  .OrderBy(wfh => wfh.Date).ThenBy(wfh => wfh.LoginTime)
                                  .Select(wfh => wfh.LoginTime).FirstOrDefault()?.ToString(@"hh\:mm\:ss") ?? "00:00:00",
                      LogoutTime = DB.WFHLoginlogs
                                   .Where(wfh => wfh.EmpId == emp.EmpId && wfh.Date >= startDate && wfh.Date <= endDate)
                                   .OrderByDescending(wfh => wfh.Date).ThenByDescending(wfh => wfh.LogOutTime)
                                   .Select(wfh => wfh.LogOutTime).FirstOrDefault()?.ToString(@"hh\:mm\:ss") ?? "00:00:00"
                  })
                  .ToList();

                return new List<AttendanceFilterViewModel>
        {
            new AttendanceFilterViewModel
            {
                Device = deviceCheckInDetails,
                Site = onSiteDetails,
                WorkFromHome = wfhDetails
            }
        };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public string CreateCompanySetting(CompanySettingViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.CompanyName) || model.CreatedBy == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Company Name and CreatedBy are required.");

                var company = DB.CompanyMasters.FirstOrDefault(c => c.Company == model.CompanyName && c.IsDeleted == false);
                if (company == null)
                {
                    company = new CompanyMaster
                    {
                        Company = model.CompanyName,
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };
                    DB.CompanyMasters.Add(company);
                    DB.SaveChanges();
                }

                var businessUnit = DB.BusinessUnitMasters.FirstOrDefault(b => b.BusinessUnit == model.BusinessUnitName && b.IsDeleted == false);
                if (businessUnit == null)
                {
                    businessUnit = new BusinessUnitMaster
                    {
                        BusinessUnit = model.BusinessUnitName,
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };
                    DB.BusinessUnitMasters.Add(businessUnit);
                    DB.SaveChanges();
                }

                var legalEntity = DB.LegalEntityMasters.FirstOrDefault(l => l.LegalEntity == model.LEName && l.IsDeleted == false);
                if (legalEntity == null)
                {
                    legalEntity = new LegalEntityMaster
                    {
                        LegalEntity = model.LEName,
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };
                    DB.LegalEntityMasters.Add(legalEntity);
                    DB.SaveChanges();
                }

                var location = DB.LocationMasters.FirstOrDefault(l => l.Location == model.LocationName && l.IsDeleted == false);
                if (location == null)
                {
                    location = new LocationMaster
                    {
                        Location = model.LocationName,
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };
                    DB.LocationMasters.Add(location);
                    DB.SaveChanges();
                }

                CompanySettingMaster newSetting = new CompanySettingMaster
                {
                    CompanyId = company.CompId,
                    BusinessUnitId = businessUnit.BUId,
                    LEId = legalEntity.LEId,
                    LocationId = location.LocationId,
                    Shift = model.Shift,
                    ShiftStart = model.ShiftStart ?? TimeSpan.Zero,
                    ShiftEnd = model.ShiftEnd ?? TimeSpan.Zero,
                    MinWorkHoursDay = model.MinWorkHoursDay ?? 0,
                    MinWorkHoursWeek = model.MinWorkHoursWeek ?? 0,
                    PayDay = model.PayDay ?? 0,
                    HalfDayLossHours = model.HalfDayLossHours ?? 0,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = model.UpdatedBy ?? 0,
                    UpdatedDate = DateTime.Now,
                    CompWrkHrs = model.CompWrkHrs ?? 0
                };

                DB.CompanySettingMasters.Add(newSetting);
                DB.SaveChanges();

                return "Company setting created successfully.";
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public ShiftViewModel CreateShift(ShiftViewModel model)
        {
            try
            {
                if (model == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid shift data.");

                int? companyId = (model.CompanyId != 0) ? model.CompanyId : 0;
                int? businessUnitId = (model.BusinessUnitId != 0) ? model.BusinessUnitId : 0;
                int? legalEntityId = (model.LegalEntityId != 0) ? model.LegalEntityId : 0;
                int? locationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? createdBy = (model.CreatedBy != 0) ? model.CreatedBy : 0;

                if (createdBy == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid CreatedBy ID.");

                if (model.ShiftStart == null || model.ShiftEnd == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Shift start and end times are required.");

                TimeSpan shiftDuration = model.ShiftEnd - model.ShiftStart;

                if (shiftDuration.TotalHours > 9 || shiftDuration.TotalHours < 9)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Shift duration cannot exceed/less 9 hours.");

                var existingShift = (from shift in DB.CompanySettingMasters
                                     where shift.CompanyId == companyId &&
                                           shift.BusinessUnitId == businessUnitId &&
                                           shift.LEId == legalEntityId &&
                                           shift.LocationId == locationId &&
                                           shift.Shift == model.Shift
                                     select shift).ToList();

                if (!existingShift.Any())
                {
                    CompanySettingMaster shift = new CompanySettingMaster
                    {
                        CompanyId = (int)companyId,
                        BusinessUnitId = (int)businessUnitId,
                        LEId = (int)legalEntityId,
                        LocationId = (int)locationId,
                        Shift = model.Shift,
                        ShiftStart = model.ShiftStart,
                        ShiftEnd = model.ShiftEnd,
                        MinWorkHoursDay = model.MinWorkHoursDay,
                        MinWorkHoursWeek = model.MinWorkHoursWeek,
                        PayDay = model.PayDay,
                        HalfDayLossHours = model.HalfDayLossHours,
                        CreatedBy = (int)createdBy,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = (int)createdBy,
                        UpdatedDate = DateTime.Now,
                        CompWrkHrs = model.CompWrkHrs
                    };

                    DB.CompanySettingMasters.Add(shift);
                    DB.SaveChanges();

                    return new ShiftViewModel { msg = "Shift Created Successfully" };
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.Conflict, "Shift already exists for the specified location.");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }


        public WorkHoursViewModel GetWorkHours(WorkHoursViewModel model)
        {
            try
            {
                int? Empid = (model.EmpId != 0) ? model.EmpId : 0;

                if (model.EmpId == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid Employee ID.");

                DateTime startDate = !string.IsNullOrEmpty(model.StartDate)
                   ? DateTime.Parse(model.StartDate)
                   : DateTime.Today.AddMonths(-1);

                DateTime endDate = !string.IsNullOrEmpty(model.EndDate)
                    ? DateTime.Parse(model.EndDate)
                    : DateTime.Today;

                var employee = DB.EmployeeMasters
                              .Where(e => e.EmpId == model.EmpId)
                              .Select(e => new { e.CompId, e.DeptName, e.CategoryId })
                              .FirstOrDefault();

                if (employee == null)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found.");

                var companySettings = DB.CompanySettingMasters
                                       .Where(s => s.CompanyId == employee.CompId)
                                       .Select(s => new { s.MinWorkHoursDay, s.MinWorkHoursWeek })
                                       .FirstOrDefault();

                int requiredDailyHours = companySettings?.MinWorkHoursDay ?? 9;
                int requiredWeeklyHours = companySettings?.MinWorkHoursWeek ?? 45;

                var attendanceList = DB.Emp_AttendanceTime
                                     .Where(a => a.LogDate >= startDate && a.LogDate <= endDate)
                                     .Select(a => new
                                     {
                                         a.LogDate,
                                         a.AttendHours,
                                         a.AttendMins,
                                         a.AttendSec
                                     })
                                     .ToList();

                if (!attendanceList.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "No work hours found for the specified date range.");

                var dailyWorkHours = attendanceList
                   .GroupBy(a => a.LogDate)
                   .Select(g => new WorkHoursDetail
                   {
                       Date = g.Key.HasValue ? g.Key.Value.ToString("yyyy-MM-dd") : null,
                       WorkHours = (g.Sum(a => a.AttendHours.GetValueOrDefault()) / 60.0)
                       //+
                       //            (g.Sum(a => a.AttendMins.GetValueOrDefault()) / 60.0) +
                       //            (g.Sum(a => a.AttendSec.GetValueOrDefault()) / 3600.0)
                   })
                   .ToList();

                //var dailyWorkHours = attendanceList
                //    .GroupBy(a => a.LogDate)
                //    .Select(g => new WorkHoursDetail
                //    {
                //        Date = g.Key.HasValue ? g.Key.Value.ToString("yyyy-MM-dd") : null,
                //        WorkHours = (g.Sum(a => a.AttendHours.GetValueOrDefault()) / 60.0) +  // Convert minutes to hours
                //                    (g.Sum(a => a.AttendMins.GetValueOrDefault()) / 60.0) +
                //                    (g.Sum(a => a.AttendSec.GetValueOrDefault()) / 3600.0)
                //    })
                //    .ToList();


                double totalWorkHours = dailyWorkHours.Sum(d => d.WorkHours);
                bool isCompliant = totalWorkHours >= requiredWeeklyHours;

                return new WorkHoursViewModel
                {
                    EmpId = model.EmpId,
                    msg = "Work hours retrieved successfully.",
                    StartDate = startDate.ToString("yyyy-MM-dd"),
                    EndDate = endDate.ToString("yyyy-MM-dd"),
                    WorkHoursData = dailyWorkHours,
                    TotalWorkHours = TimeSpan.FromHours(totalWorkHours),
                    RequiredDailyHours = TimeSpan.FromHours(requiredDailyHours),
                    RequiredWeeklyHours = TimeSpan.FromHours(requiredWeeklyHours)

                };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }


        public WorkHoursViewModel CheckHalfDayLoss(WorkHoursViewModel model, DateTime date)
        {
            try
            {
                int? empId = model.EmpId != 0 ? model.EmpId : (int?)null;

                if (empId == 0 || empId == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid Employee ID.");

                var employee = DB.EmployeeMasters
                                .Where(e => e.EmpId == empId)
                                .Select(e => new { e.CompId })
                                .FirstOrDefault();

                if (employee == null)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found.");

                var companySettings = DB.CompanySettingMasters
                                       .Where(s => s.CompanyId == employee.CompId)
                                       .Select(s => new { s.MinWorkHoursDay, s.MinWorkHoursWeek })
                                       .FirstOrDefault();

                TimeSpan requiredDailyHours = TimeSpan.FromHours(companySettings?.MinWorkHoursDay ?? 9);
                TimeSpan requiredWeeklyHours = TimeSpan.FromHours(companySettings?.MinWorkHoursWeek ?? 45);

                var attendance = DB.Emp_AttendanceTime
                                  .Where(a => a.LogDate == date)
                                  .Select(a => new
                                  {
                                      a.AttendHours,
                                      a.AttendMins,
                                      a.AttendSec
                                  })
                                  .FirstOrDefault();

                if (attendance == null)
                {
                    return new WorkHoursViewModel
                    {
                        EmpId = empId.Value,
                        Date = date.ToString("yyyy-MM-dd"),
                        IsHalfDay = false,
                        Reason = "No attendance record found",
                        TotalWorkHours = TimeSpan.Zero,
                        RequiredDailyHours = requiredDailyHours,
                        RequiredWeeklyHours = requiredWeeklyHours
                    };
                }

                TimeSpan totalWorkHours = new TimeSpan(
                   attendance.AttendHours.GetValueOrDefault(),
                   attendance.AttendMins.GetValueOrDefault(),
                   attendance.AttendSec.GetValueOrDefault());

                bool isHalfDay = totalWorkHours < requiredDailyHours;

                return new WorkHoursViewModel
                {
                    EmpId = empId.Value,
                    Date = date.ToString("yyyy-MM-dd"),
                    IsHalfDay = isHalfDay,
                    Reason = isHalfDay ? "Worked less than required daily hours" : "Met required work hours",
                    TotalWorkHours = totalWorkHours,
                    RequiredDailyHours = requiredDailyHours,
                    RequiredWeeklyHours = requiredWeeklyHours
                };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<PageAccessViewModel> GetAllPages(PageAccessViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? roleId = (model.RoleId != 0) ? model.RoleId : 0;

                var pageList = (from pg in DB.PageModuleMasters
                                join mm in DB.ModuleMasters on pg.ModuleId equals mm.ModuleId
                                join sm in DB.SubModuleMasters on pg.SubModuleId equals sm.SubModuleId
                                where pg.IsDeleted == false
                                      && mm.IsDeleted == false
                                      && sm.IsDeleted == false
                                orderby pg.PageModuleId descending
                                select new PageAccessViewModel
                                {
                                    PageModuleId = pg.PageModuleId,
                                    PageAccess = true,
                                    ModuleId = pg.ModuleId,
                                    ModuleName = mm.ModuleName,
                                    SubModuleId = pg.SubModuleId,
                                    SubModuleName = sm.SubModuleName,
                                    PageName = pg.PageName,
                                    AddAccess = false,
                                    UpdateAccess = false,
                                    DeleteAccess = false, 
                                    ViewAccess = false,
                                    CreatedBy = pg.CreatedBy,
                                    CreatedDate = pg.CreatedDate,
                                    LastUpdatedBy = pg.LastUpdatedBy,
                                    LastUpdatedDate = pg.LastUpdatedDate,
                                    IsActive = pg.IsActive
                                }).ToList();

                if (pageList == null || pageList.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "No pages found.");

                if (deptId != 0 && roleId != 0)
                {
                    var pageList1 = (from ap in DB.AccessPolicies 
                                     join pg in DB.PageModuleMasters on ap.PageModuleId equals pg.PageModuleId
                                    join mm in DB.ModuleMasters on pg.ModuleId equals mm.ModuleId
                                    join sm in DB.SubModuleMasters on pg.SubModuleId equals sm.SubModuleId
                                    where ap.DeptId == deptId && ap.RoleId == roleId &&
                                    ap.IsDeleted == false && pg.IsDeleted == false
                                          && mm.IsDeleted == false
                                          && sm.IsDeleted == false
                                    orderby pg.PageModuleId descending
                                    select new PageAccessViewModel
                                    {
                                        PageModuleId = pg.PageModuleId,
                                        PageAccess = true,
                                        ModuleId = pg.ModuleId,
                                        ModuleName = mm.ModuleName,
                                        SubModuleId = pg.SubModuleId,
                                        SubModuleName = sm.SubModuleName,
                                        PageName = pg.PageName,
                                        AddAccess = ap.AddAccess,
                                        UpdateAccess = ap.UpdateAccess,
                                        DeleteAccess = ap.DeleteAccess,
                                        ViewAccess = ap.ViewAccess,
                                        CreatedBy = pg.CreatedBy,
                                        CreatedDate = pg.CreatedDate,
                                        LastUpdatedBy = pg.LastUpdatedBy,
                                        LastUpdatedDate = pg.LastUpdatedDate,
                                        IsActive = pg.IsActive
                                    }).ToList();

                    // 🔹 Merge access rights from pageList1 into pageList
                    foreach (var item in pageList)
                    {
                        var access = pageList1.FirstOrDefault(x => x.PageModuleId == item.PageModuleId);
                        if (access != null)
                        {
                            item.PageAccess = access.PageAccess;
                            item.AddAccess = access.AddAccess;
                            item.UpdateAccess = access.UpdateAccess;
                            item.DeleteAccess = access.DeleteAccess;
                            item.ViewAccess = access.ViewAccess;
                        }
                        else
                        {
                            item.PageAccess = false;
                            item.AddAccess = false;
                            item.UpdateAccess = false;
                            item.DeleteAccess = false;
                            item.ViewAccess = false;
                        }
                    }
                }
                return pageList;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        // public List<AccessViewModel> SubmitAccessControls(List<AccessViewModel> accessList)
        //{
        //    try
        //    {
        //        if (accessList == null || accessList.Count == 0)
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "Access list cannot be empty.");

        //        List<AccessViewModel> resultList = new List<AccessViewModel>();

        //        for (int i = 0; i < accessList.Count; i++)
        //        {
        //            var Accessdd = accessList[i];
        //            int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;
        //            string accessName = (!string.IsNullOrWhiteSpace(Accessdd.AccessName)) ? Accessdd.AccessName.Trim() : "";

        //            if (EmpId == 0)
        //            {
        //                resultList.Add(new AccessViewModel
        //                {
        //                    AccessName = accessName,
        //                    msg = $"EmpId is missing for record at index {i + 1}"
        //                });
        //                continue;
        //            }

        //            if (string.IsNullOrEmpty(accessName))
        //            {
        //                resultList.Add(new AccessViewModel
        //                {
        //                    AccessName = "N/A",
        //                    msg = $"AccessName is missing for record at index {i + 1}"
        //                });
        //                continue;
        //            }

        //            var existingAccess = DB.AccessPolicies
        //                .FirstOrDefault(a => a.AccessName == accessName && a.IsActive == true && a.IsDeleted == false);

        //            if (existingAccess == null)
        //            {
        //                AccessPolicy am = new AccessPolicy();
        //                am.AccessName = Accessdd.AccessName;
        //                am.DeptId = Accessdd.DeptId;
        //                am.RoleId = Accessdd.RoleId;
        //                am.ModuleId = Accessdd.ModuleId;
        //                am.SubModuleId = Accessdd.SubModuleId;
        //                am.PageModuleId = Accessdd.PageModuleId;
        //                am.AddAccess = Accessdd.AddAccess;
        //                am.UpdateAccess = Accessdd.UpdateAccess;
        //                am.DeleteAccess = Accessdd.DeleteAccess;
        //                am.ViewAccess = Accessdd.ViewAccess;
        //                am.IsActive = true;
        //                am.IsUpdated = false;
        //                am.IsDeleted = false;
        //                am.CreatedBy = (int)EmpId;
        //                am.CreatedDate = DateTime.Now;
        //                am.LastUpdatedBy = EmpId;
        //                am.LastUpdatedDate = DateTime.Now;

        //                DB.AccessPolicies.Add(am);
        //                DB.SaveChanges();

        //                resultList.Add(new AccessViewModel
        //                {
        //                    AccessName = accessName,
        //                    msg = "Added"
        //                });
        //            }
        //            else
        //            {
        //                resultList.Add(new AccessViewModel
        //                {
        //                    AccessName = accessName,
        //                    msg = "Already Exists"
        //                });
        //            }
        //        }

        //        return resultList;
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}
        public LeaveResponseViewModel SubmitAccessControls(List<AccessViewModel> accessList)
        {
            try
            {
                int? EmpId = (accessList[0].EmpId != 0) ? accessList[0].EmpId : 0;

                if (EmpId != 0)
                {
                    if (accessList == null || accessList.Count == 0)
                        throw new CustomApiException(HttpStatusCode.BadRequest, "Access list cannot be empty.");

                    List<AccessViewModel> resultList = new List<AccessViewModel>();

                    int? deptid = (accessList[0].DeptId != 0) ? accessList[0].DeptId : 0;
                    int? roleid = (accessList[0].RoleId != 0) ? accessList[0].RoleId : 0;

                    var preAccesscontroll = (from acc in DB.AccessPolicies
                                             where acc.DeptId == deptid &&
                                                    acc.RoleId == roleid &&
                                                   acc.IsActive == true && acc.IsDeleted == false
                                             select acc).ToList();

                    for (int j = 0; j < preAccesscontroll.Count(); j++)
                    {
                        preAccesscontroll[j].IsDeleted = true;
                        preAccesscontroll[j].LastUpdatedBy = (int)EmpId;
                        preAccesscontroll[j].LastUpdatedDate = DateTime.Now;
                        //DB.SaveChanges();
                    }
                    DB.SaveChanges();

                    for (int i = 0; i < accessList.Count(); i++)
                    {
                        var Accessdd = accessList[i];
                        //int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;
                        string accessName = (!string.IsNullOrWhiteSpace(Accessdd.AccessName)) ? Accessdd.AccessName.Trim() : "";

                        var existingAccess = DB.AccessPolicies.FirstOrDefault(a =>
                            a.AccessName == accessName &&
                            a.DeptId == Accessdd.DeptId &&
                            a.RoleId == Accessdd.RoleId &&
                            a.ModuleId == Accessdd.ModuleId &&
                            a.SubModuleId == Accessdd.SubModuleId &&
                            a.PageModuleId == Accessdd.PageModuleId &&
                            a.IsActive == true &&
                            a.IsDeleted == false);

                        if (existingAccess == null)
                        {
                            AccessPolicy am = new AccessPolicy();
                            am.AccessName = Accessdd.AccessName;
                            am.DeptId = Accessdd.DeptId;
                            am.RoleId = Accessdd.RoleId;
                            am.ModuleId = Accessdd.ModuleId;
                            am.SubModuleId = Accessdd.SubModuleId;
                            am.PageModuleId = Accessdd.PageModuleId;
                            am.AddAccess = Accessdd.AddAccess;
                            am.UpdateAccess = Accessdd.UpdateAccess;
                            am.DeleteAccess = Accessdd.DeleteAccess;
                            am.ViewAccess = Accessdd.ViewAccess;
                            am.IsActive = true;
                            am.IsUpdated = false;
                            am.IsDeleted = false;
                            am.CreatedBy = (int)EmpId;
                            am.CreatedDate = DateTime.Now;
                            am.LastUpdatedBy = EmpId;
                            am.LastUpdatedDate = DateTime.Now;

                            DB.AccessPolicies.Add(am);
                            //DB.SaveChanges();
                        }
                    }
                    DB.SaveChanges();

                    LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                    emvm.Status = 200;
                    emvm.msg = "Access Policy Updated";

                    return emvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public PageModuleViewModel GetPageById(int pageModuleId)
        {
            try
            {
                var page = (from pg in DB.PageModuleMasters
                            join mm in DB.ModuleMasters on pg.ModuleId equals mm.ModuleId
                            join sm in DB.SubModuleMasters on pg.SubModuleId equals sm.SubModuleId
                            where pg.PageModuleId == pageModuleId && pg.IsDeleted == false
                                  && mm.IsDeleted == false && sm.IsDeleted == false
                            select new PageModuleViewModel
                            {
                                PageModuleId = pg.PageModuleId,
                                ModuleId = pg.ModuleId,
                                ModuleName = mm.ModuleName,
                                SubModuleId = pg.SubModuleId,
                                SubModuleName = sm.SubModuleName,
                                PageName = pg.PageName,
                                CreatedBy = pg.CreatedBy,
                                CreatedDate = pg.CreatedDate,
                                LastUpdatedBy = pg.LastUpdatedBy,
                                LastUpdatedDate = pg.LastUpdatedDate,
                                IsActive = pg.IsActive
                            }).FirstOrDefault();

                if (page == null)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Page module not found.");

                return page;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<AccessViewModel> UpdatePageModules(List<AccessViewModel> accessList)
        {
            try
            {
                if (accessList == null || accessList.Count == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No access records provided for update.");

                int? empId = accessList.FirstOrDefault()?.EmpId ?? 0;
                if (empId == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "EmpId is missing.");

                List<AccessViewModel> resultList = new List<AccessViewModel>();

                for (int i = 0; i < accessList.Count; i++)
                {
                    var accessItem = accessList[i];
                    string accessName = !string.IsNullOrWhiteSpace(accessItem.AccessName) ? accessItem.AccessName.Trim() : "N/A";

                    var existing = DB.AccessPolicies.FirstOrDefault(a =>
                        a.DeptId == accessItem.DeptId &&
                        a.RoleId == accessItem.RoleId &&
                        a.ModuleId == accessItem.ModuleId &&
                        a.SubModuleId == accessItem.SubModuleId &&
                        a.PageModuleId == accessItem.PageModuleId &&
                        a.IsActive == true &&
                        a.IsDeleted == false
                    );

                    if (existing != null)
                    {
                        existing.AccessName = accessItem.AccessName;
                        existing.AddAccess = accessItem.AddAccess;
                        existing.UpdateAccess = accessItem.UpdateAccess;
                        existing.DeleteAccess = accessItem.DeleteAccess;
                        existing.ViewAccess = accessItem.ViewAccess;
                        existing.IsUpdated = true;
                        existing.LastUpdatedBy = empId;
                        existing.LastUpdatedDate = DateTime.Now;

                        resultList.Add(new AccessViewModel
                        {
                            AccessName = accessName,
                            msg = "Updated"
                        });
                    }
                    else
                    {
                        resultList.Add(new AccessViewModel
                        {
                            AccessName = accessName,
                            msg = $"No matching record found at index {i + 1}"
                        });
                    }
                }

                DB.SaveChanges();

                return resultList;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }


        public List<AccessViewModel> DeletePageModules(List<AccessViewModel> accessList)
        {
            try
            {
                if (accessList == null || accessList.Count == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No access records provided for deletion.");

                int? empId = accessList.FirstOrDefault()?.EmpId ?? 0;
                if (empId == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "EmpId is missing.");

                List<AccessViewModel> resultList = new List<AccessViewModel>();

                for (int i = 0; i < accessList.Count; i++)
                {
                    var accessItem = accessList[i];
                    string accessName = (!string.IsNullOrWhiteSpace(accessItem.AccessName)) ? accessItem.AccessName.Trim() : "N/A";

                    var existing = DB.AccessPolicies.FirstOrDefault(a =>
                        a.DeptId == accessItem.DeptId &&
                        a.RoleId == accessItem.RoleId &&
                        a.ModuleId == accessItem.ModuleId &&
                        a.SubModuleId == accessItem.SubModuleId &&
                        a.PageModuleId == accessItem.PageModuleId &&
                        a.IsDeleted == false &&
                        a.IsActive == true
                    );

                    if (existing != null)
                    {
                        existing.IsDeleted = true;
                        existing.LastUpdatedBy = empId;
                        existing.LastUpdatedDate = DateTime.Now;

                        resultList.Add(new AccessViewModel
                        {
                            AccessName = accessName,
                            msg = "Deleted"
                        });
                    }
                    else
                    {
                        resultList.Add(new AccessViewModel
                        {
                            AccessName = accessName,
                            msg = $"No matching record found at index {i + 1}"
                        });
                    }
                }

                DB.SaveChanges();

                return resultList;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public HolidayViewModel UpdateHoliday(HolidayViewModel model)
        {
            try
            {
                if (model.Modify_By == null || model.Modify_By == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Invalid ModifiedBy ID.");

                if (model.Holiday_Id == null || model.Holiday_Id.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Invalid Holiday ID list.");

                if (model.HolidayLocationId == null || model.HolidayLocationId.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "HolidayLocationId list is required.");

                var updatedHolidays = new List<HolidayViewModel>();

                var existingHolidays = DB.Holidays
                   .Where(h => model.Holiday_Id.Contains(h.Holiday_Id))
                   .ToList();

                var selectedLocationIds = model.HolidayLocationId;

                var holidaysToRemove = existingHolidays
                   .Where(h => h.LocationId.HasValue && !selectedLocationIds.Contains(h.LocationId.Value))
                   .ToList();

                foreach (var h in holidaysToRemove)
                {
                    DB.Holidays.Remove(h);
                }

                DB.SaveChanges();

                for (int i = 0; i < selectedLocationIds.Count; i++)
                {
                    int locationId = selectedLocationIds[i];
                    string locationName = (model.HolidayLocation != null && i < model.HolidayLocation.Count)
                        ? model.HolidayLocation[i]
                        : "";

                    var existingHoliday = existingHolidays
                       .FirstOrDefault(h => h.LocationId == locationId);

                    if (existingHoliday != null)
                    {

                        existingHoliday.Title = model.Title;
                        existingHoliday.Date = model.Date;
                        existingHoliday.Year = model.Year;
                        existingHoliday.Description = model.Description;
                        existingHoliday.Location = locationName;
                        existingHoliday.Modify_By = model.Modify_By;
                        existingHoliday.Modify_Date = DateTime.UtcNow;
                        existingHoliday.Status = model.Status;
                        existingHoliday.HolidayType = model.HolidayType;
                    }
                    else
                    {

                        var newHoliday = new Holiday
                        {
                            Title = model.Title,
                            Date = model.Date,
                            Year = model.Year,
                            Description = model.Description,
                            LocationId = locationId,
                            Location = locationName,
                            Created_By = model.Created_By,
                            Created_Date = DateTime.UtcNow,
                            Modify_By = model.Modify_By,
                            Modify_Date = DateTime.UtcNow,
                            Status = model.Status,
                            HolidayType = model.HolidayType
                        };
                        DB.Holidays.Add(newHoliday);
                    }

                    DB.SaveChanges();

                    updatedHolidays.Add(new HolidayViewModel
                    {
                        Holiday_Id = model.Holiday_Id,
                        Title = model.Title,
                        Date = model.Date,
                        Year = model.Year,
                        Description = model.Description,
                        HolidayLocationId = new List<int> { locationId },
                        HolidayLocation = new List<string> { locationName },
                        Created_By = model.Created_By,
                        Created_Date = DateTime.UtcNow,
                        Modify_By = model.Modify_By,
                        Modify_Date = DateTime.UtcNow,
                        Status = model.Status,
                        HolidayType = model.HolidayType
                    });
                }

                var grouped = new HolidayViewModel
                {
                    msg = "Holiday(s) updated successfully.",
                    UpdatedHolidays = new List<HolidayViewModel>
            {
                new HolidayViewModel
                {
                    Title = model.Title,
                    Description = model.Description,
                    Date = model.Date,
                    Year = model.Year,
                    Status = model.Status,
                    HolidayType = model.HolidayType,
                    Modify_By = model.Modify_By,
                    Modify_Date = DateTime.UtcNow,
                    Created_By = model.Created_By,
                    Created_Date = DateTime.UtcNow,
                    Holiday_Id = model.Holiday_Id,
                    HolidayLocationId = selectedLocationIds.Distinct().ToList(),
                    HolidayLocation = model.HolidayLocation?.Distinct().ToList() ?? new List<string>()
                }
            }
                };

                return grouped;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public HolidayViewModel DeleteHoliday(HolidayViewModel model)
        {
            try
            {
                if (model.Holiday_Id == null || model.Holiday_Id.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Invalid Holiday_Id list.");

                var holidays = DB.Holidays
                   .Where(w => model.Holiday_Id.Contains(w.Holiday_Id) && w.Status == "Active")
                   .ToList();

                if (!holidays.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "No matching Week Holidays found.");

                foreach (var holiday in holidays)
                {
                    holiday.Status = "Inactive";
                    holiday.Modify_By = model.Modify_By;
                    holiday.Modify_Date = DateTime.UtcNow;
                }

                DB.SaveChanges();

                return new HolidayViewModel { msg = "Holiday(s) Deleted Successfully" };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public List<HolidayViewModel> GetAllHolidays(HolidayViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? locid = 4;

                if (loginId == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                locid = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.EmpStatus.ToUpper() == "ACTIVE" && x.IsActive == true && x.IsDeleted == false).Select(x => x.LocationId).FirstOrDefault();

                if (locid == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Employee location is not mapped!!");

                if (locid == 0)
                {
                    locid = 4;
                }

                var holidays = DB.Holidays
                    .Where(h => h.Status == "Active" && h.LocationId == locid)
                    .OrderByDescending(x => x.Holiday_Id)
                    .ToList();

                if (loginId == 149)
                {
                    holidays = DB.Holidays
                    .Where(h => h.Status == "Active")
                    .OrderByDescending(x => x.Holiday_Id)
                    .ToList();
                }

                if (holidays == null || holidays.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "No Holidays Found");

                var groupedHolidays = holidays
                   .GroupBy(h => new { h.Title, h.Date, h.HolidayType })
                   .Select(group => new HolidayViewModel
                   {
                       Title = group.Key.Title,
                       Date = group.Key.Date,
                       HolidayType = group.Key.HolidayType,
                       Description = group.First().Description,
                       Year = group.First().Year ?? 0,
                       Status = group.First().Status,
                       Created_By = group.First().Created_By,
                       Created_Date = group.First().Created_Date,
                       Holiday_Id = group.Select(g => g.Holiday_Id).Distinct().ToList(),
                       HolidayLocationId = group.Select(g => g.LocationId ?? 0).Distinct().ToList(),
                       HolidayLocation = group.Select(g => g.Location ?? "").Distinct().ToList(),
                       LocationId = group.Select(g => g.LocationId ?? 0).Distinct().ToList(),
                       Location = group.Select(g => g.Location ?? "").Distinct().ToList()
                   })
                   .ToList();

                return groupedHolidays;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        //public WeekHolidayViewModel CreateWeekHoliday(WeekHolidayViewModel model)
        //{
        //    try
        //    {
        //        if (model == null)
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid week holiday data.");

        //        if (model.Created_By == 0)
        //            throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid CreatedBy ID.");

        //        var existingWeekHoliday = DB.WeekHolidays
        //            .Where(w => w.Day == model.Day && w.LocationId == model.LocationId)
        //            .FirstOrDefault();

        //        if (existingWeekHoliday == null)
        //        {
        //            WeekHoliday holiday = new WeekHoliday
        //            {
        //                Day = model.Day,
        //                Created_By = model.Created_By,
        //                Created_Date = DateTime.UtcNow,
        //                Status = model.Status,
        //                LocationId = model.LocationId,
        //                Location = model.Location,
        //                Title = model.Title,
        //                Description = model.Description,
        //            };

        //            DB.WeekHolidays.Add(holiday);
        //            DB.SaveChanges();

        //            return new WeekHolidayViewModel { msg = "Week Holiday Created Successfully" };
        //        }
        //        else
        //        {
        //            throw new CustomApiException(HttpStatusCode.NotFound, "Week holiday already exists for this day.");
        //        }
        //    }
        //    catch (CustomApiException ex)
        //    {
        //        throw new CustomApiException(ex.StatusCode, ex.Message);
        //    }
        //}


        public List<EmpHolidayListViewModel> GetEmpHolidays(EmpHolidayListViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                int? year = DateTime.Now.Year;
                DateTime Today = DateTime.Today;

                if (loginId > 0)
                {
                    int? locid = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == loginId && emp.IsActive == true && emp.IsDeleted == false
                                  && emp.EmpStatus.ToUpper() == "ACTIVE"
                                  select emp.LocationId).FirstOrDefault() ?? 0;

                    if (locid == 0)
                    {
                        locid = 4;
                    }

                    var holidaydetails = (from hd in DB.Holidays
                                          where hd.Status.ToUpper() == "ACTIVE" && hd.Year == year && hd.LocationId == locid
                                          select hd).ToList();

                    if (holidaydetails.Count() > 0)
                    {
                        List<EmpHolidayListViewModel> lstofHolid = new List<EmpHolidayListViewModel>();
                        for (int i = 0; i < holidaydetails.Count(); i++)
                        {
                            EmpHolidayListViewModel hlvm = new EmpHolidayListViewModel();
                            ////hlvm.CompanyId = holidaydetails[i].CompanyId;
                            ////int compid = holidaydetails[i].CompanyId;
                            ////if (compid != 0)
                            ////{
                            ////    hlvm.Company = DB.CompanyMasters.Where(x => x.CompId == compid && x.IsActive == true && x.IsDeleted == false).Select(x => x.Company).FirstOrDefault();
                            ////}
                            ////else
                            ////{
                            ////    hlvm.Company = "";
                            ////}
                            ////hlvm.LEId = holidaydetails[i].LEId;
                            ////int leid = holidaydetails[i].LEId;
                            ////if (leid != 0)
                            ////{
                            ////    hlvm.LegalEntity = DB.LegalEntityMasters.Where(x => x.LEId == leid && x.IsActive == true && x.IsDeleted == false).Select(x => x.LegalEntity).FirstOrDefault();
                            ////}
                            ////else
                            ////{
                            ////    hlvm.LegalEntity = "";
                            ////}
                            ////hlvm.BUId = holidaydetails[i].BUId;
                            ////int buid = holidaydetails[i].BUId;
                            ////if (buid != 0)
                            ////{
                            ////    hlvm.BusinessUnit = DB.BusinessUnitMasters.Where(x => x.BUId == buid && x.IsActive == true && x.IsDeleted == false).Select(x => x.BusinessUnit).FirstOrDefault();
                            ////}
                            ////else
                            ////{
                            ////    hlvm.BusinessUnit = "";
                            ////}
                            hlvm.LocationId = holidaydetails[i].LocationId;
                            int? locationid = holidaydetails[i].LocationId;
                            if (locationid != 0)
                            {
                                hlvm.Location = DB.LocationMasters.Where(x => x.LocationId == locationid && x.IsActive == true && x.IsDeleted == false).Select(x => x.Location).FirstOrDefault();
                            }
                            else
                            {
                                hlvm.Location = "";
                            }
                            hlvm.HolidayId = holidaydetails[i].Holiday_Id;
                            hlvm.Year = holidaydetails[i].Year;
                            hlvm.Title = holidaydetails[i].Title;
                            hlvm.Date = holidaydetails[i].Date.ToString("dd-MM-yyyy");
                            hlvm.HolidayType = holidaydetails[i].HolidayType;
                            lstofHolid.Add(hlvm);
                        }
                        return lstofHolid;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Holiday details not found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        public WeekHolidayViewModel UpdateWeekHoliday(WeekHolidayViewModel model)
        {
            try
            {
                if (model.Modified_By == null || model.Modified_By == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid Modified_By ID.");

                if (model.LocationId == null || model.LocationId.Count == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LocationId list is required.");

                if (model.Day == null || model.Day.Count == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Day list is required.");

                var updatedHolidays = new List<WeekHolidayViewModel>();

                string combinedDays = string.Join(", ", model.Day.Distinct());

                var existingWeekHolidays = DB.WeekHolidays
                   .Where(w => w.Year == model.Year && w.Title == model.Title)
                   .ToList();

                var selectedLocationIds = model.LocationId;

                var holidaysToRemove = existingWeekHolidays
                   .Where(w => w.LocationId.HasValue && !selectedLocationIds.Contains(w.LocationId.Value))
                   .ToList();

                foreach (var h in holidaysToRemove)
                {
                    DB.WeekHolidays.Remove(h);
                }

                DB.SaveChanges();

                for (int i = 0; i < selectedLocationIds.Count; i++)
                {
                    int locationId = selectedLocationIds[i];
                    string locationName = (model.Location != null && i < model.Location.Count) ? model.Location[i] : "";

                    var existing = existingWeekHolidays
                        .FirstOrDefault(w => w.LocationId == locationId);

                    if (existing != null)
                    {

                        existing.Day = combinedDays;
                        existing.Title = model.Title;
                        existing.Description = model.Description;
                        existing.Year = model.Year;
                        existing.Status = model.Status;
                        existing.Modified_By = model.Modified_By;
                        existing.Modified_Date = DateTime.UtcNow;
                        existing.Location = locationName;
                    }
                    else
                    {

                        var newWeekHoliday = new WeekHoliday
                        {
                            Day = combinedDays,
                            LocationId = locationId,
                            Location = locationName,
                            Title = model.Title,
                            Description = model.Description,
                            Year = model.Year,
                            Status = model.Status,
                            Created_By = model.Created_By,
                            Created_Date = DateTime.UtcNow,
                            Modified_By = model.Modified_By,
                            Modified_Date = DateTime.UtcNow
                        };
                        DB.WeekHolidays.Add(newWeekHoliday);
                    }

                    DB.SaveChanges();

                    var saved = DB.WeekHolidays
                        .FirstOrDefault(w => w.LocationId == locationId && w.Day == combinedDays && w.Year == model.Year);

                    updatedHolidays.Add(new WeekHolidayViewModel
                    {
                        WeekDay_ID = new List<int> { saved?.WeekDay_ID ?? 0 },
                        Day = model.Day.Distinct().ToList(),
                        LocationId = new List<int> { locationId },
                        Location = new List<string> { locationName },
                        Title = model.Title,
                        Description = model.Description,
                        Status = model.Status,
                        Year = model.Year,
                        Modified_By = model.Modified_By,
                        Modified_Date = DateTime.UtcNow
                    });
                }


                var grouped = new WeekHolidayViewModel
                {
                    msg = "Week Holidays updated successfully",
                    UpdatedWeekHolidays = new List<WeekHolidayViewModel>
            {
                new WeekHolidayViewModel
                {
                    Title = model.Title,
                    Description = model.Description,
                    Status = model.Status,
                    Year = model.Year,
                    Modified_By = model.Modified_By,
                    Modified_Date = DateTime.UtcNow,
                    WeekDay_ID = updatedHolidays.SelectMany(x => x.WeekDay_ID).ToList(),
                    Day = updatedHolidays.SelectMany(x => x.Day).Distinct().ToList(),
                    Location = updatedHolidays.SelectMany(x => x.Location).Distinct().ToList(),
                    LocationId = updatedHolidays.SelectMany(x => x.LocationId).ToList()
                }
            }
                };

                return grouped;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public WeekHolidayViewModel DeleteWeekHoliday(WeekHolidayViewModel model)
        {
            try
            {
                if (model.WeekDay_ID == null || model.WeekDay_ID.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Invalid WeekDay_ID list.");

                var holidays = DB.WeekHolidays
                   .Where(w => model.WeekDay_ID.Contains(w.WeekDay_ID) && w.Status == "Active")
                   .ToList();

                if (!holidays.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "No matching Week Holidays found.");

                foreach (var holiday in holidays)
                {
                    holiday.Status = "Inactive";
                    holiday.Modified_By = model.Modified_By;
                    holiday.Modified_Date = DateTime.UtcNow;
                }

                DB.SaveChanges();

                return new WeekHolidayViewModel { msg = "Week Holiday(s) Deleted Successfully" };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }



        public List<WeekHolidayViewModel> GetAllWeekHolidays(WeekHolidayViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                if (loginId == 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                var holidays = DB.WeekHolidays
                    .Where(h => h.Status == "Active")
                    .OrderByDescending(x => x.WeekDay_ID)
                    .ToList();

                if (holidays == null || holidays.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "No Week Holidays Found");

                var groupedHolidays = holidays
                    .GroupBy(h => new { h.Title, h.Description, h.Status })
                    .Select(group => new WeekHolidayViewModel
                    {
                        Title = group.Key.Title,
                        Description = group.Key.Description,
                        Year = group.First().Year ?? 2025,
                        Status = group.Key.Status,
                        Created_By = group.First().Created_By ?? 0,
                        Created_Date = group.First().Created_Date,
                        Modified_By = group.First().Modified_By,
                        Modified_Date = group.First().Modified_Date,
                        WeekDay_ID = group.Select(g => g.WeekDay_ID).ToList(),
                        Day = group.Select(g => g.Day).Distinct().ToList(),
                        LocationId = group.Select(g => g.LocationId ?? 0).Distinct().ToList(),
                        Location = group.Select(g => g.Location ?? "").Distinct().ToList()
                    })
                    //.OrderBy(x => x.Title)
                    .ToList();

                return groupedHolidays;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<FinanceMasterViewModel> GetAllFinanceMaster(FinanceMasterViewModel model)
        {
            try
            {
                var financeList = DB.FinanceMasters
                                    .OrderBy(f => f.Year)
                                    .Select(f => new FinanceMasterViewModel
                                    {
                                        Id = f.Id,
                                        Year = f.Year
                                    })
                                    .ToList();

                if (financeList == null || financeList.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "No Finance Year records found.");

                return financeList;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }

        }

        public HolidayViewModel AddHoliday(HolidayViewModel model)
        {
            try
            {
                if (model.HolidayType == "Weekly Holidays")
                {
                    if (model.Day == null || model.LocationId == null)
                        throw new CustomApiException(HttpStatusCode.NotFound, "Day and LocationId are required for weekly holidays.");

                    string combinedDays = string.Join(", ", model.Day);

                    for (int i = 0; i < model.LocationId.Count; i++)
                    {
                        int locId = model.LocationId[i];
                        string locName = model.Location != null && model.Location.Count > i ? model.Location[i] : "";

                        var exists = DB.WeekHolidays
                            .FirstOrDefault(w => w.Day == combinedDays && w.LocationId == locId && w.Status == "Active");

                        if (exists != null)
                            throw new CustomApiException(HttpStatusCode.NotFound, $"Weekly holiday for '{combinedDays}' already exists at location '{locName}'.");

                        var weekHoliday = new WeekHoliday
                        {
                            Day = combinedDays,
                            Created_By = model.Created_By,
                            Created_Date = DateTime.UtcNow,
                            Status = model.Status ?? "Active",
                            LocationId = locId,
                            Title = model.Title,
                            Description = model.Description,
                            Location = locName,
                            HolidayType = model.HolidayType
                        };

                        DB.WeekHolidays.Add(weekHoliday);
                    }

                    DB.SaveChanges();
                    model.msg = "Weekly Holidays Created Successfully";
                    return model;
                }
                else
                {
                    for (int i = 0; i < model.HolidayLocationId.Count; i++)
                    {
                        int locId = model.HolidayLocationId[i];
                        string locName = model.HolidayLocation != null && model.HolidayLocation.Count > i ? model.HolidayLocation[i] : "";

                        var existingHoliday = DB.Holidays
                           .FirstOrDefault(h => h.Date == model.Date && h.LocationId == locId
                                             && h.HolidayType == model.HolidayType && h.Status == "Active");

                        if (existingHoliday != null)
                            throw new CustomApiException(HttpStatusCode.NotFound, "Records already exists");

                        var newHoliday = new Holiday
                        {
                            Title = model.Title,
                            Date = model.Date,
                            Description = model.Description,
                            LocationId = locId,
                            Created_By = model.Created_By,
                            Created_Date = DateTime.UtcNow,
                            Status = model.Status ?? "Active",
                            Year = model.Year,
                            HolidayType = model.HolidayType,
                            Location = locName
                        };

                        DB.Holidays.Add(newHoliday);
                    }

                    DB.SaveChanges();

                    model.msg = " Holidays Created Successfully";
                    return model;
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        ////public static void FetchAttendance()
        ////{
        ////    DateTime todayDate = DateTime.Today;

        ////    RunAttendanceStoredProcedure(todayDate);
        ////} //2026-03-10 issue

        public static void FetchAttendance()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            string formattedDate = yesterday.ToString("yyyy-MM-dd");

            RunAttendanceStoredProcedure(yesterday); // Or pass the formatted string if needed
        }

        private static void RunAttendanceStoredProcedure(DateTime attendanceDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_Offc_Con"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_InsertAttendanceData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = attendanceDate;
                cmd.CommandTimeout = 300;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // Log success message
                    LogsModel.LogSuccess($"Attendance fetched successfully for {attendanceDate:yyyy-MM-dd}", "DailyAttendance");
                    // OR use separate file:
                    // LogsModel.LogSuccessToSeparateFile($"Attendance fetched successfully for {attendanceDate:yyyy-MM-dd}", "DailyAttendance");
                }
                catch (SqlException ex)
                {
                    LogsModel.LogErrorToFile(ex, "DailyAttendance");
                    throw;
                }
            }
        }
        public static void EmployeeConfirmationMail()
        {
            DateTime today = DateTime.Today;
            RunEmployeeConfirmationMail(today);
        }

        private static void RunEmployeeConfirmationMail(DateTime today)
        {
            // Create an instance of your DataContext - Use your actual DB context class name
            // Based on your error, it seems you have a 'DB' object, so let's find the type
            using (var db = new DB_Offc_ConEntities()) // Replace with your actual DataContext class name
            {
                // Get all active employees from EmployeeMaster
                var empdetails = (from lm in db.EmployeeMasters
                                  where lm.EmpStatus.ToUpper() == "ACTIVE" && lm.IsActive == true && lm.IsDeleted == false
                                  select lm).ToList();

                // Get probation tracking details where IsProbation is true
                var probationDetails = (from pt in db.EmpProbationTrackingHistories
                                        where pt.IsProbation == true && pt.IsActive == true && pt.IsDeleted == false
                                        select pt).ToList();

                foreach (var probation in probationDetails)
                {
                    // Find the employee details
                    var employee = empdetails.FirstOrDefault(e => e.EmpId == probation.EmpId);
                    if (employee == null) continue;

                    DateTime probationEndDate = probation.ProbationEndDate.Value;
                    DateTime joiningDate = probation.JoiningDate.Value;

                    // Calculate days remaining for probation end
                    int daysRemaining = (probationEndDate - today).Days;

                    // Get manager email (ReportCode from EmployeeMaster)
                    var manager = empdetails.FirstOrDefault(e => e.ReportName.ToUpper() == employee.ReportName.ToUpper());
                    string managerEmail = manager?.EmailId;

                    if (string.IsNullOrEmpty(managerEmail)) continue;

                    // Case 1: Send email before 15 days of probation end date
                    if (daysRemaining == 15)
                    {
                        SendProbationEndingSoonEmail(employee, managerEmail, probationEndDate);
                    }

                    // Case 2: After probation date, employee not confirmed - send weekly email
                    if (today > probationEndDate && probation.IsPermanent == false)
                    {
                        // Check if already sent this week
                        if (!IsWeeklyEmailSentThisWeek((int)probation.EmpId, probationEndDate))
                        {
                            SendProbationOverdueEmail(employee, managerEmail, probationEndDate);
                            UpdateWeeklyEmailSentStatus((int)probation.EmpId, probationEndDate);
                        }
                    }

                    // Optional: Send exact probation end date email
                    if (daysRemaining == 0)
                    {
                        SendProbationEndDateEmail(employee, managerEmail, probationEndDate);
                    }
                }
            }
        }

        // Method to send email before 15 days of probation end
        private static void SendProbationEndingSoonEmail(EmployeeMaster employee, string managerEmail, DateTime probationEndDate)
        {
            string subject = $"Probation Ending Soon - {employee.FirstName} {employee.LastName}";
            string body = $@"
        <html>
        <body>
            <h3>Probation Period Ending Soon</h3>
            <p>Dear Manager,</p>
            <p>This is to inform you that employee <strong>{employee.FirstName} {employee.LastName}</strong> (EmpCode: {employee.EmpCode}) will complete their probation period on <strong>{probationEndDate:dd-MM-yyyy}</strong>.</p>
            <p>Please review their performance and take necessary action for confirmation.</p>
            <p><strong>Employee Details:</strong></p>
            <ul>
                <li>Name: {employee.FirstName} {employee.LastName}</li>
                <li>Employee Code: {employee.EmpCode}</li>
                <li>Joining Date: {employee.JoiningDate:dd-MM-yyyy}</li>
                <li>Probation End Date: {probationEndDate:dd-MM-yyyy}</li>
                <li>Days Remaining: 15 days</li>
            </ul>
            <p>Please confirm the employee status before the probation end date.</p>
            <br/>
            <p>Regards,<br/>HR Team</p>
        </body>
        </html>";

            SendEmail(managerEmail, subject, body);
        }

        // Method to send email after probation date (overdue)
        private static void SendProbationOverdueEmail(EmployeeMaster employee, string managerEmail, DateTime probationEndDate)
        {
            int overdueDays = (DateTime.Today - probationEndDate).Days;

            string subject = $"URGENT: Probation Period Overdue - {employee.FirstName} {employee.LastName}";
            string body = $@"
        <html>
        <body>
            <h3 style='color:red'>Probation Period Overdue - Action Required</h3>
            <p>Dear Manager,</p>
            <p>This is a weekly reminder that employee <strong>{employee.FirstName} {employee.LastName}</strong> (EmpCode: {employee.EmpCode}) has completed their probation period on <strong>{probationEndDate:dd-MM-yyyy}</strong>.</p>
            <p><strong style='color:red'>Overdue by: {overdueDays} days</strong></p>
            <p>The employee has not been confirmed yet. Please take immediate action:</p>
            <ul>
                <li>Confirm the employee as permanent</li>
                <li>Extend the probation period if required</li>
                <li>Or take appropriate action as per company policy</li>
            </ul>
            <p><strong>Employee Information:</strong></p>
            <ul>
                <li>Name: {employee.FirstName} {employee.LastName}</li>
                <li>Employee Code: {employee.EmpCode}</li>
                <li>Designation: {employee.DesignationName}</li>
                <li>Department: {employee.DeptName}</li>
                <li>Joining Date: {employee.JoiningDate:dd-MM-yyyy}</li>
                <li>Probation End Date: {probationEndDate:dd-MM-yyyy}</li>
                <li>Current Status: Not Confirmed</li>
            </ul>
            <p>Please update the employee status at the earliest.</p>
            <br/>
            <p>Regards,<br/>HR Team</p>
        </body>
        </html>";

            SendEmail(managerEmail, subject, body);
        }

        // Method to send email on exact probation end date
        private static void SendProbationEndDateEmail(EmployeeMaster employee, string managerEmail, DateTime probationEndDate)
        {
            string subject = $"Probation Period Completed - {employee.FirstName} {employee.LastName}";
            string body = $@"
        <html>
        <body>
            <h3>Probation Period Completed</h3>
            <p>Dear Manager,</p>
            <p>Employee <strong>{employee.FirstName} {employee.LastName}</strong> (EmpCode: {employee.EmpCode}) has completed their probation period today (<strong>{probationEndDate:dd-MM-yyyy}</strong>).</p>
            <p>Please take necessary action for confirmation or extension of probation period.</p>
            <p><strong>Employee Details:</strong></p>
            <ul>
                <li>Name: {employee.FirstName} {employee.LastName}</li>
                <li>Employee Code: {employee.EmpCode}</li>
                <li>Designation: {employee.DesignationName}</li>
                <li>Email: {employee.EmailId}</li>
            </ul>
            <br/>
            <p>Regards,<br/>HR Team</p>
        </body>
        </html>";

            SendEmail(managerEmail, subject, body);
        }

        // Helper method to check if weekly email was already sent this week
        private static bool IsWeeklyEmailSentThisWeek(int empId, DateTime probationEndDate)
        {
            // Use your existing DB instance - Note: This needs to be instance, so we need to create a new DB context
            using (var db = new DB_Offc_ConEntities())
            {
                var result = from log in db.EmpEmailTrackingLogs
                             where log.EmpId == empId
                             && log.EmailType == "ProbationOverdue"
                             && log.SentDate >= DateTime.Today.AddDays(-7)
                             select log;

                return result.Any();
            }
        }

        // Helper method to update weekly email sent status
        private static void UpdateWeeklyEmailSentStatus(int empId, DateTime probationEndDate)
        {
            // Use your existing DB instance
            using (var db = new DB_Offc_ConEntities())
            {
                // Create a new record in EmailTrackingLogs table
                // If EmailTrackingLog class doesn't exist, create it or use your actual table name
                var log = new EmpEmailTrackingLog() // Use your actual table class name
                {
                    EmpId = empId,
                    EmailType = "ProbationOverdue",
                    SentDate = DateTime.Today,
                    ProbationEndDate = probationEndDate
                };

                db.EmpEmailTrackingLogs.Add(log);
                db.SaveChanges();
            }
        }

        // Generic email sending method
        private static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Implement your email sending logic here
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("hr@yourcompany.com");
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("your-smtp-server", 25))
                    {
                        smtp.Credentials = new NetworkCredential("username", "password");
                        smtp.EnableSsl = false;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Email failed to send to {toEmail}: {ex.Message}");
            }
        }

        public static void CFLeaveCredits()
        {
            DateTime today = DateTime.Today;
            RunCFLeaveCreditsStoredProcedure(today);
        }

        private static void RunCFLeaveCreditsStoredProcedure(DateTime attendanceDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_Offc_Con"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_ProcessLeaveCredits", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // FIX: Parameter name should match the stored procedure parameter (@CurrentDate)
                cmd.Parameters.Add("@CurrentDate", SqlDbType.Date).Value = attendanceDate;
                cmd.CommandTimeout = 300;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // Log success message
                    LogsModel.LogSuccess($"CL and EL Credited and Carry forwarded successfully for {attendanceDate:yyyy-MM-dd}", "CFLeaveCredits");
                }
                catch (SqlException ex)
                {
                    LogsModel.LogErrorToFile(ex, "CFLeaveCredits");
                    throw;
                }
            }
        }

        ////private static void RunAttendanceStoredProcedure(DateTime attendanceDate)
        ////{
        ////    string connectionString =
        ////    ConfigurationManager.ConnectionStrings["DB_Offc_Con"].ConnectionString;

        ////    ////using (SqlConnection conn = new SqlConnection(connectionString))
        ////    ////using (SqlCommand cmd = new SqlCommand("sp_InsertAttendanceData", conn))
        ////    ////{
        ////    ////    cmd.CommandType = CommandType.StoredProcedure;

        ////    ////    cmd.Parameters.Add("@StartDate", SqlDbType.Date)
        ////    ////                   .Value = attendanceDate;

        ////    ////    conn.Open();
        ////    ////    cmd.ExecuteNonQuery();
        ////    ////}

        ////    using (SqlConnection conn = new SqlConnection(connectionString))
        ////    using (SqlCommand cmd = new SqlCommand("dbo.sp_InsertAttendanceData", conn))
        ////    {
        ////        cmd.CommandType = CommandType.StoredProcedure;
        ////        cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = attendanceDate;

        ////        try
        ////        {
        ////            conn.Open();
        ////            cmd.ExecuteNonQuery();
        ////        }
        ////        catch (SqlException ex)
        ////        {
        ////            // Call the logging method
        ////            //logsmodel.LogErrorToFile(ex, ErroName);
        ////            LogsModel.LogErrorToFile(ex, "DailyAttendance");

        ////            throw; // optionally rethrow
        ////        }
        ////    }
        ////}


        /// //////// 25.03.2026
        ////public SPAttendanceViewModel SPAttendance(SPAttendanceViewModel model)
        ////{
        ////    try
        ////    {
        ////        if (model.LoginId <= 0)
        ////            throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

        ////        if (string.IsNullOrWhiteSpace(model.Date))
        ////            throw new CustomApiException(HttpStatusCode.BadRequest, "Please select the Date!!");

        ////        DateTime spdate;
        ////        // Conversion
        ////        DateTime.TryParseExact(
        ////            model.Date,
        ////            "yyyy-MM-dd",
        ////            System.Globalization.CultureInfo.InvariantCulture,
        ////            System.Globalization.DateTimeStyles.None,
        ////            out spdate
        ////        );

        ////        // spdate = 2026-01-27 00:00:00

        ////        string connectionString = ConfigurationManager.ConnectionStrings["DB_Offc_Con"].ConnectionString;

        ////        using (SqlConnection conn = new SqlConnection(connectionString))
        ////        using (SqlCommand cmd = new SqlCommand("dbo.sp_InsertAttendanceData", conn))
        ////        {
        ////            cmd.CommandType = CommandType.StoredProcedure;
        ////            cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = spdate;

        ////            try
        ////            {
        ////                conn.Open();
        ////                cmd.ExecuteNonQuery();
        ////            }
        ////            catch (SqlException ex)
        ////            {
        ////                // Call the logging method
        ////                //logsmodel.LogErrorToFile(ex, ErroName);
        ////                LogsModel.LogErrorToFile(ex, "ManualAttendance");

        ////                throw; // optionally rethrow
        ////            }
        ////        }

        ////        SPAttendanceViewModel spvm = new SPAttendanceViewModel();
        ////        spvm.msg = "Attendance for " + spdate + " loaded successfully!";

        ////        return spvm;
        ////    }
        ////    catch (CustomApiException)
        ////    {
        ////        throw;
        ////    }
        ////}
        ///
        public SPAttendanceViewModel SPAttendance(SPAttendanceViewModel model)
        {
            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                if (string.IsNullOrWhiteSpace(model.Date))
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Please select the Date!!");

                DateTime spdate;
                // Conversion
                DateTime.TryParseExact(
                    model.Date,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out spdate
                );

                // spdate = 2026-01-27 00:00:00

                string connectionString = ConfigurationManager.ConnectionStrings["DB_Offc_Con"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_InsertAttendanceDataManual", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = spdate;

                    // Set timeout to 5 minutes (300 seconds) or as needed
                    cmd.CommandTimeout = 300;

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Log success message
                        LogsModel.LogSuccess($"Load Attendance fetched successfully for {spdate:yyyy-MM-dd}", "LoadAttendance - Manual");
                    }
                    catch (SqlException ex)
                    {
                        // Call the logging method
                        //logsmodel.LogErrorToFile(ex, ErroName);
                        LogsModel.LogErrorToFile(ex, "LoadAttendance - Manual");

                        throw; // optionally rethrow
                    }
                }

                SPAttendanceViewModel spvm = new SPAttendanceViewModel();
                spvm.msg = "Attendance for " + spdate + " loaded successfully!";

                return spvm;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }

        public UploadResult UploadAttendance(int? loginId)
        {
            UploadResult result = new UploadResult();

            result.TotalRecords = DB.TempManualAttendances.Count();

            // Insert valid records
            // Using ExecuteSqlCommand with parameter
            result.InsertedRecords = DB.Database.ExecuteSqlCommand(
                "EXEC InsertValidAttendance @LoginId",
                new SqlParameter("@LoginId", loginId)
            );

            // Fetch exceptions
            result.Exceptions = DB.Database
                                .SqlQuery<AttendanceException>(
                                    "EXEC GetAttendanceExceptions @LoginId",
                                    new SqlParameter("@LoginId", loginId)
                                ).ToList();

            result.FailedRecords = result.Exceptions.Count;

            // Clean temp table
            DB.Database.ExecuteSqlCommand("TRUNCATE TABLE TempManualAttendance");

            return result;
        }
        public UploadResult UploadSingleAttendance(UploadAttendanceSingleViewModel model)
        {
            UploadResult result = new UploadResult();

            int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

            DB.TempManualAttendances.Add(new TempManualAttendance
            {
                EmpCode = model.EmpCode,
                Date = model.Date,
                Time = model.Time,
                Status = model.Status
            });

            result.TotalRecords = DB.TempManualAttendances.Count();

            // Insert valid records
            // Using ExecuteSqlCommand with parameter
            result.InsertedRecords = DB.Database.ExecuteSqlCommand(
                "EXEC InsertValidAttendance @LoginId",
                new SqlParameter("@LoginId", loginId)
            );

            // Fetch exceptions
            result.Exceptions = DB.Database
                                .SqlQuery<AttendanceException>(
                                    "EXEC GetAttendanceExceptions @LoginId",
                                    new SqlParameter("@LoginId", loginId)
                                ).ToList();

            result.FailedRecords = result.Exceptions.Count;

            // Clean temp table
            DB.Database.ExecuteSqlCommand("TRUNCATE TABLE TempManualAttendance");

            return result;
        }
        public UploadResult UploadMultiAttendance(List<UploadAttendanceSingleViewModel> model)
        {
            UploadResult result = new UploadResult();

            int? loginId = (model[0].LoginId != 0) ? model[0].LoginId : 0;

            for (int i = 0; i < model.Count(); i++) // skip header
            {
                var empCode = model[i].EmpCode;
                var date = model[i].Date;
                var workedHrs = model[i].Time;
                var status = "Active";

                if (string.IsNullOrEmpty(empCode)) continue;

                ////// Add to TempManualAttendance
                DB.TempManualAttendances.Add(new TempManualAttendance
                {
                    EmpCode = empCode,
                    Date = date,
                    Time = workedHrs,
                    Status = status
                });
            }
            DB.SaveChanges();

            result.TotalRecords = DB.TempManualAttendances.Count();

            // Insert valid records
            // Using ExecuteSqlCommand with parameter
            result.InsertedRecords = DB.Database.ExecuteSqlCommand(
                "EXEC InsertValidAttendance @LoginId",
                new SqlParameter("@LoginId", loginId)
            );

            // Fetch exceptions
            result.Exceptions = DB.Database
                                .SqlQuery<AttendanceException>(
                                    "EXEC GetAttendanceExceptions @LoginId",
                                    new SqlParameter("@LoginId", loginId)
                                ).ToList();

            result.FailedRecords = result.Exceptions.Count;

            // Clean temp table
            DB.Database.ExecuteSqlCommand("TRUNCATE TABLE TempManualAttendance");

            return result;
        }
        public List<ManualAttendanceViewModel> GetAllManualAttendance(ManualAttendanceViewModel model)
        {
            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                var result = (
                    from ma in DB.ManualAttendances
                    join emp in DB.EmployeeMasters on ma.EmpCode equals emp.EmpCode
                    join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
                    join le in DB.LegalEntityMasters on emp.LEId equals le.LEId
                    join bu in DB.BusinessUnitMasters on emp.BUId equals bu.BUId
                    join loc in DB.LocationMasters on emp.LocationId equals loc.LocationId
                    where ma.Status == "Active"
                       && emp.EmpStatus.ToUpper() == "ACTIVE"
                       && emp.IsActive == true
                       && emp.IsDeleted == false
                       && comp.IsActive == true && comp.IsDeleted == false
                       && le.IsActive == true && le.IsDeleted == false
                       && bu.IsActive == true && bu.IsDeleted == false
                       && loc.IsActive == true && loc.IsDeleted == false
                    orderby ma.Id descending
                    select new ManualAttendanceViewModel
                    {
                        EmpCode = ma.EmpCode,
                        FullName = emp.FirstName
                                     + (string.IsNullOrEmpty(emp.MiddleName) ? " " : " " + emp.MiddleName + " ")
                                     + emp.LastName,
                        CompId = emp.CompId,
                        Company = comp.Company,
                        LEId = emp.LEId,
                        LegalEntity = le.LegalEntity,
                        BUId = emp.BUId,
                        BusinessUnit = bu.BusinessUnit,
                        LocationId = emp.LocationId,
                        Location = loc.Location,
                        Date = ma.Date.ToString(),
                        WorkedHrs = ma.Time.ToString(),
                        Status = ma.Status
                    }
                ).ToList();

                if (!result.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Manual Attendance is not Found");

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }
        public List<DDEmployeeViewModel> DDEmpList(DDEmpListViewModel empdd)
        {
            try
            {
                if (empdd.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                var query = DB.EmployeeMasters
                    .Where(emp =>
                        emp.EmpStatus == "ACTIVE" &&
                        emp.IsActive == true &&
                        emp.IsDeleted == false
                    );

                // Apply filters ONLY if value > 0
                if (empdd.CompId > 0)
                    query = query.Where(emp => emp.CompId == empdd.CompId);

                if (empdd.LEId > 0)
                    query = query.Where(emp => emp.LEId == empdd.LEId);

                if (empdd.BUId > 0)
                    query = query.Where(emp => emp.BUId == empdd.BUId);

                if (empdd.LocationId > 0)
                    query = query.Where(emp => emp.LocationId == empdd.LocationId);

                var result = query
                    .OrderByDescending(emp => emp.EmpId)
                    .Select(emp => new DDEmployeeViewModel
                    {
                        EmpId = emp.EmpId,
                        EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                        EmpCode = emp.UserName
                    })
                    .ToList();

                if (!result.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }
        public List<AttendaceDeptReportViewModel> AttendanceDeptReport(AttendanceFilterViewModel model)
        {
            try
            {
                // Call stored procedure with the correct model
                // Call stored procedure
                var result = DB.Database.SqlQuery<DepartmentAttendanceRawResult>(
                                        @"EXEC sp_GetDepartmentDailyAttendanceReport 
                                    @StartDate, @EndDate, @CompId, @LEId, @BUId, @LocId, @DeptId",
                    new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                    new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                    new SqlParameter("@CompId", (object)model.CompId ?? DBNull.Value),
                    new SqlParameter("@LEId", (object)model.LEId ?? DBNull.Value),
                    new SqlParameter("@BUId", (object)model.BUId ?? DBNull.Value),
                    new SqlParameter("@LocId", (object)model.LocId ?? DBNull.Value),
                    new SqlParameter("@DeptId", (object)model.DeptId ?? DBNull.Value)
                    
                ).ToList();

                // Group by date and map to final view model
                var groupedResult = result
                    .GroupBy(r => new { r.Date, r.Day })
                    .Select(g => new AttendaceDeptReportViewModel
                    {
                        Date = g.Key.Date,
                        Day = g.Key.Day,
                        lstofDept = g.Select(x => new DepartmentAttendanceViewModel
                        {
                            DeptName = x.DeptName,
                            DeptShortName = x.DeptShortName,
                            Total = Convert.ToInt32(x.Total),
                            OverAllAbsentPercentage = x.OverAllAbsentPercentage,
                            Present = Convert.ToInt32(x.Present),
                            Absent = Convert.ToInt32(x.Absent),
                            Leave = Convert.ToInt32(x.Leave),
                            AbsentPesent = x.AbsentPesent,
                            IsHoliday = x.IsHoliday
                        }).ToList()
                    })
                    .ToList();

                return groupedResult;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(System.Net.HttpStatusCode.InternalServerError,
                    $"SP Execution Error: {ex.Message}");
            }
        }
        public ContractAttendanceViewModel ContractAttendanceChecking(ContractViewModel model)
        {
            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                DateTime Today = DateTime.Now.Date;  // This gives you 2026-02-25 00:00:00

                var attendance = DB.ContractAttendances
                    .Where(emp =>
                        emp.Date == Today &&  // Now both are dates without time
                        emp.Mobile == model.MobileNo &&
                        emp.IsLogin == true &&
                        emp.IsLogout == false &&
                        emp.IsActive == true &&
                        emp.IsDeleted == false
                    )
                    .FirstOrDefault();

                ContractAttendanceViewModel result;

                if (attendance != null)
                {
                    result = new ContractAttendanceViewModel
                    {
                        LoginStatus = "LOGIN",
                        CId = attendance.CId,
                        Date = attendance.Date,
                        Mobile = attendance.Mobile,
                        Mail = attendance.Mail,
                        EmpCode = attendance.EmpCode,
                        EmpName = attendance.EmpName,
                        Skill = attendance.Skill,
                        VendorId = attendance.VendorId,
                        ERPVendorId = attendance.ERPVendorId,
                        VendorCode = attendance.VendorCode,
                        Vendor = attendance.Vendor,
                        ProjectId = attendance.ProjectId,
                        ERPProjectId = attendance.ERPProjectId,
                        ProjectCode = attendance.ProjectCode,
                        Project = attendance.Project,
                        SiteId = attendance.SiteId,
                        Site = attendance.Site,
                        SiteDetails = attendance.SiteDetails,
                        ManagerId = attendance.ManagerId,
                        ManagerEmpCode = attendance.ManagerEmpCode,
                        ManagerName = attendance.ManagerName,
                        Status = attendance.Status,
                        IsLogin = attendance.IsLogin,
                        IsLogout = attendance.IsLogout,
                        LoginTime = attendance.LoginTime,
                        LogoutTime = attendance.LogoutTime,
                        Activehrs = attendance.Activehrs,
                        Approvedhrs = attendance.Approvedhrs,
                        LoginAddress = attendance.LoginAddress,
                        LoginLonqitude = attendance.LoginLonqitude,
                        LoginLatitude = attendance.LoginLatitude,
                        LogoutAddress = attendance.LogoutAddress,
                        LogoutLonqitude = attendance.LogoutLonqitude,
                        LogoutLatitude = attendance.LogoutLatitude,
                        Description = attendance.Description,
                        ManPowerApproval = attendance.ManPowerApproval,
                        IsApproved = attendance.IsApproved,
                        IsLogoutManager = attendance.IsLogoutManager,
                        CreatedBy = attendance.CreatedBy,
                        CreatedDate = attendance.CreatedDate,
                        LastUpdatedBy = attendance.LastUpdatedBy,
                        LastUpdatedDate = attendance.LastUpdatedDate,
                        IsActive = attendance.IsActive,
                        IsUpdated = attendance.IsUpdated,
                        IsDeleted = attendance.IsDeleted
                    };
                }
                else
                {
                    // Create a minimal object with status "No Data"
                    result = new ContractAttendanceViewModel
                    {
                        LoginStatus = "No Data"
                    };

                    // You might want to throw an exception instead of returning a "No Data" object
                    // throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
                }

                ////// Check if we have data (if you want to throw exception for no data)
                ////if (attendance == null)
                ////    throw new CustomApiException(HttpStatusCode.NotFound, "Contract Attendance Details Not Found");

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new CustomApiException(HttpStatusCode.InternalServerError, "An error occurred while processing your request");
            }
        }
        ////public List<ContractAttendanceViewModel> ContractAttendanceManager(ContractViewModel model)
        ////{
        ////    //    try
        ////    //    {
        ////    //        if (model.LoginId <= 0)
        ////    //            throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

        ////    //        // Get employee details once
        ////    //        var currentEmployee = DB.EmployeeMasters
        ////    //            .Where(emp => emp.EmpId == model.LoginId && emp.IsDeleted == false)
        ////    //            .Select(emp => new {
        ////    //                emp.CategoryId,
        ////    //                emp.CompId,
        ////    //                emp.LEId,
        ////    //                emp.BUId,
        ////    //                emp.LocationId
        ////    //            })
        ////    //            .FirstOrDefault();

        ////    //        if (currentEmployee == null)
        ////    //            throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found");

        ////    //        // Base query
        ////    //        IQueryable<ContractAttendance> query;

        ////    //        // Check if user is in HR department
        ////    //        var isHR = DB.DeptMasters
        ////    //            .Any(dept => dept.DeptId == currentEmployee.CategoryId &&
        ////    //                        dept.IsDeleted == false &&
        ////    //                        dept.DeptName.ToUpper() == "HUMAN RESOURCE");

        ////    //        if (isHR)
        ////    //        {
        ////    //            if (model.LoginId == 149)
        ////    //            {
        ////    //                // HR sees all employees in their company/LE/BU/Location
        ////    //                query = from ca in DB.ContractAttendances
        ////    //                        join emp in DB.EmployeeMasters on ca.EmpCode.ToUpper() equals emp.EmpCode.ToUpper()
        ////    //                        where ca.IsDeleted == false 
        ////    //                        select ca;
        ////    //            }
        ////    //            else
        ////    //            {
        ////    //                // HR sees all employees in their company/LE/BU/Location
        ////    //                query = from ca in DB.ContractAttendances
        ////    //                        join emp in DB.EmployeeMasters on ca.EmpCode.ToUpper() equals emp.EmpCode.ToUpper()
        ////    //                        where ca.IsDeleted == false &&
        ////    //                              emp.LocationId == currentEmployee.LocationId
        ////    //                        select ca;
        ////    //            }
        ////    //        }
        ////    //        else
        ////    //        {
        ////    //            // Regular manager sees only their team
        ////    //            query = DB.ContractAttendances
        ////    //                .Where(ca => ca.ManagerId == model.LoginId && ca.IsDeleted == false);
        ////    //        }

        ////    //        // Apply date filter
        ////    //        if (model.FromDate.HasValue && model.ToDate.HasValue)
        ////    //        {
        ////    //            var fromDate = model.FromDate.Value.Date;
        ////    //            var toDate = model.ToDate.Value.Date.AddDays(1); // Include the entire end date
        ////    //            query = query.Where(ca => ca.Date >= fromDate && ca.Date < toDate);
        ////    //        }
        ////    //        else
        ////    //        {
        ////    //            var yesterday = DateTime.Now.Date.AddDays(-1);
        ////    //            var tomorrow = DateTime.Now.Date.AddDays(1);
        ////    //            query = query.Where(ca => ca.Date >= yesterday && ca.Date < tomorrow);
        ////    //        }

        ////    //        // Apply status filter - FIXED the incorrect condition
        ////    //        if (!string.IsNullOrEmpty(model.Status))
        ////    //        {
        ////    //            var statusUpper = model.Status.ToUpper();
        ////    //            if (statusUpper == "PENDING")
        ////    //            {
        ////    //                query = query.Where(ca => ca.IsApproved == false);
        ////    //            }
        ////    //            else if (statusUpper == "APPROVED")
        ////    //            {
        ////    //                query = query.Where(ca => ca.IsApproved == true);
        ////    //            }
        ////    //            // "ALL" - no filter needed
        ////    //        }
        ////    //        else
        ////    //        {
        ////    //            // Default to pending if no status specified
        ////    //            query = query.Where(ca => ca.IsApproved == false);
        ////    //        }

        ////    //        // Apply project filter
        ////    //        if (model.ProjectId > 0)
        ////    //        {
        ////    //            query = query.Where(ca => ca.ProjectId == model.ProjectId);
        ////    //        }

        ////    //        // Apply vendor filter
        ////    //        if (model.VendorId > 0)
        ////    //        {
        ////    //            query = query.Where(ca => ca.VendorId == model.VendorId);
        ////    //        }

        ////    //        // Get results with optimized mapping
        ////    //        var attendanceList = query
        ////    //            .OrderByDescending(ca => ca.CreatedDate)
        ////    //            .Select(ca => new ContractAttendanceViewModel
        ////    //            {
        ////    //                CId = ca.CId,
        ////    //                Date = ca.Date,
        ////    //                Mobile = ca.Mobile,
        ////    //                Mail = ca.Mail,
        ////    //                EmpCode = ca.EmpCode,
        ////    //                EmpName = ca.EmpName,
        ////    //                Skill = ca.Skill,
        ////    //                VendorId = ca.VendorId,
        ////    //                ERPVendorId = ca.ERPVendorId,
        ////    //                Vendor = ca.Vendor,
        ////    //                VendorCode = ca.VendorCode,
        ////    //                ProjectId = ca.ProjectId,
        ////    //                ERPProjectId = ca.ERPProjectId,
        ////    //                ProjectCode = ca.ProjectCode,
        ////    //                Project = ca.Project,
        ////    //                SiteId = ca.SiteId,
        ////    //                Site = ca.Site,
        ////    //                SiteDetails = ca.SiteDetails,
        ////    //                ManagerId = ca.ManagerId,
        ////    //                ManagerEmpCode = ca.ManagerEmpCode,
        ////    //                ManagerName = ca.ManagerName,
        ////    //                Status = ca.Status,
        ////    //                LoginStatus = ca.IsLogin == true && ca.IsLogout == false ? "LOGIN" :
        ////    //                             ca.IsLogin == true && ca.IsLogout == true ? "LOGOUT" : "UNKNOWN",
        ////    //                IsLogin = ca.IsLogin,
        ////    //                IsLogout = ca.IsLogout,
        ////    //                LoginTime = ca.LoginTime,
        ////    //                LogoutTime = ca.LogoutTime,
        ////    //                Activehrs = ca.Activehrs,
        ////    //                Approvedhrs = ca.Approvedhrs,
        ////    //                LoginAddress = ca.LoginAddress,
        ////    //                LoginLonqitude = ca.LoginLonqitude,
        ////    //                LoginLatitude = ca.LoginLatitude,
        ////    //                LogoutAddress = ca.LogoutAddress,
        ////    //                LogoutLonqitude = ca.LogoutLonqitude,
        ////    //                LogoutLatitude = ca.LogoutLatitude,
        ////    //                Description = ca.Description,
        ////    //                ManPowerApproval = ca.ManPowerApproval,
        ////    //                IsApproved = ca.IsApproved,
        ////    //                IsLogoutManager = ca.IsLogoutManager,
        ////    //                CreatedBy = ca.CreatedBy,
        ////    //                CreatedDate = ca.CreatedDate,
        ////    //                LastUpdatedBy = ca.LastUpdatedBy,
        ////    //                LastUpdatedDate = ca.LastUpdatedDate,
        ////    //                IsActive = ca.IsActive,
        ////    //                IsUpdated = ca.IsUpdated,
        ////    //                IsDeleted = ca.IsDeleted
        ////    //            })
        ////    //            .ToList();

        ////    //        if (!attendanceList.Any())
        ////    //            throw new CustomApiException(HttpStatusCode.NotFound, "Contract Attendance Details Not Found");

        ////    //        return attendanceList;
        ////    //    }
        ////    //    catch (CustomApiException)
        ////    //    {
        ////    //        throw;
        ////    //    }
        ////    //    catch (Exception ex)
        ////    //    {
        ////    //        // Log the exception here
        ////    //        throw new CustomApiException(HttpStatusCode.InternalServerError,
        ////    //            "An error occurred while processing your request");
        ////    //    //}
        ////    try
        ////    {
        ////        if (model.LoginId <= 0)
        ////            throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

        ////        // Get employee details once
        ////        var currentEmployee = DB.EmployeeMasters
        ////            .Where(emp => emp.EmpId == model.LoginId && emp.IsDeleted == false)
        ////            .Select(emp => new
        ////            {
        ////                emp.CategoryId,
        ////                emp.CompId,
        ////                emp.LEId,
        ////                emp.BUId,
        ////                emp.EmpCode,
        ////                emp.DeptName,
        ////                emp.LocationId
        ////            })
        ////            .FirstOrDefault();

        ////        if (currentEmployee == null)
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found");

        ////        // Base query
        ////        IQueryable<ContractAttendance> query;

        ////        // Check if user is in HR department
        ////        //var isHR = DB.DeptMasters
        ////        //    .Any(dept => dept.DeptId == currentEmployee.CategoryId &&
        ////        //                dept.IsDeleted == false &&
        ////        //                dept.DeptName.ToUpper() == "HUMAN RESOURCE");

        ////        //var isHR = DB.EmployeeMasters.Where(e => e.EmpId == model.LoginId && e.IsDeleted == false).Select(e => e.DeptName == "HUMAN RESOURCE").FirstOrDefault();

        ////        var isHR = DB.EmployeeMasters.Any(e => e.EmpId == model.LoginId && e.IsDeleted == false && e.DeptName.ToUpper().Trim() == "HUMAN RESOURCE");

        ////        if (isHR)
        ////        {
        ////            //if (model.LoginId == 149)
        ////            //{
        ////            // HR sees all employees in their company/LE/BU/Location
        ////            //query = from ca in DB.ContractAttendances
        ////            //        join emp in DB.EmployeeMasters on ca.EmpCode.ToUpper() equals emp.EmpCode.ToUpper()
        ////            //        where ca.IsDeleted == false
        ////            //        select ca;

        ////            query = DB.ContractAttendances
        ////                    .Where(ca => ca.IsDeleted == false);
        ////            //}
        ////            //else
        ////            //{
        ////            //    // HR sees all employees in their company/LE/BU/Location
        ////            //    query = from ca in DB.ContractAttendances
        ////            //            join emp in DB.EmployeeMasters on ca.EmpCode.ToUpper() equals emp.EmpCode.ToUpper()
        ////            //            where ca.IsDeleted == false &&
        ////            //                  emp.LocationId == currentEmployee.LocationId
        ////            //            select ca;
        ////            //}
        ////        }
        ////        else
        ////        {
        ////            // Regular manager sees only their team
        ////            query = DB.ContractAttendances
        ////              .Where(ca => ca.IsDeleted == false &&
        ////                ca.ManagerId == model.LoginId);
        ////        }
        ////        //else
        ////        //{
        ////        //    var loggedInEmpCode = DB.EmployeeMasters.Where(e => e.EmpId == model.LoginId && e.IsDeleted == false).Select(e => e.EmpCode.ToUpper()).FirstOrDefault();

        ////        //    query = DB.ContractAttendances
        ////        //        .Where(ca => ca.IsDeleted == false &&
        ////        //                     ca.ManagerEmpCode != null &&
        ////        //                     ca.ManagerEmpCode.ToUpper() == loggedInEmpCode);
        ////        //}
        ////        // Apply date filter
        ////        if (model.FromDate.HasValue && model.ToDate.HasValue)
        ////        {
        ////            var fromDate = model.FromDate.Value.Date;
        ////            var toDate = model.ToDate.Value.Date.AddDays(1);
        ////            query = query.Where(ca => ca.Date >= fromDate && ca.Date < toDate);
        ////        }
        ////        //else
        ////        //{
        ////        //    //var yesterday = DateTime.Now.Date.AddDays(-1);
        ////        //    //var tomorrow = DateTime.Now.Date.AddDays(1);
        ////        //    //query = query.Where(ca => ca.Date >= yesterday && ca.Date < tomorrow);
        ////        //}

        ////        // Apply status filter - FIXED the incorrect condition
        ////        //if (!string.IsNullOrEmpty(model.Status))
        ////        //{
        ////        //    var statusUpper = model.Status.ToUpper();
        ////        //    if (statusUpper == "PENDING")
        ////        //    {
        ////        //        query = query.Where(ca => ca.IsApproved == false);
        ////        //    }
        ////        //    else if (statusUpper == "APPROVED")
        ////        //    {
        ////        //        query = query.Where(ca => ca.IsApproved == true);
        ////        //    }
        ////        // "ALL" - no filter needed
        ////        //}
        ////        else
        ////        {
        ////            // Default to pending if no status specified
        ////            query = query.Where(ca => ca.IsApproved == false);
        ////        }

        ////        // Apply project filter
        ////        if (model.ProjectId > 0)
        ////        {
        ////            query = query.Where(ca => ca.ProjectId == model.ProjectId);
        ////        }

        ////        // Apply vendor filter
        ////        if (model.VendorId > 0)
        ////        {
        ////            query = query.Where(ca => ca.VendorId == model.VendorId);
        ////        }

        ////        // Get results with optimized mapping
        ////        var attendanceList = query
        ////            .OrderByDescending(ca => ca.CreatedDate)
        ////            .Select(ca => new ContractAttendanceViewModel
        ////            {
        ////                CId = ca.CId,
        ////                Date = ca.Date,
        ////                Mobile = ca.Mobile,
        ////                Mail = ca.Mail,
        ////                EmpCode = ca.EmpCode,
        ////                EmpName = ca.EmpName,
        ////                Skill = ca.Skill,
        ////                VendorId = ca.VendorId,
        ////                Vendor = ca.Vendor,
        ////                VendorCode = ca.VendorCode,
        ////                ProjectId = ca.ProjectId,
        ////                ProjectCode = ca.ProjectCode,
        ////                Project = ca.Project,
        ////                SiteId = ca.SiteId,
        ////                Site = ca.Site,
        ////                SiteDetails = ca.SiteDetails,
        ////                ManagerId = ca.ManagerId,
        ////                ManagerEmpCode = ca.ManagerEmpCode,
        ////                ManagerName = ca.ManagerName,
        ////                Status = ca.Status,
        ////                LoginStatus = ca.IsLogin == true && ca.IsLogout == false ? "LOGIN" :
        ////                             ca.IsLogin == true && ca.IsLogout == true ? "LOGOUT" : "UNKNOWN",
        ////                IsLogin = ca.IsLogin,
        ////                IsLogout = ca.IsLogout,
        ////                LoginTime = ca.LoginTime,
        ////                LogoutTime = ca.LogoutTime,
        ////                Activehrs = ca.Activehrs,
        ////                Approvedhrs = ca.Approvedhrs,
        ////                LoginAddress = ca.LoginAddress,
        ////                LoginLonqitude = ca.LoginLonqitude,
        ////                LoginLatitude = ca.LoginLatitude,
        ////                LogoutAddress = ca.LogoutAddress,
        ////                LogoutLonqitude = ca.LogoutLonqitude,
        ////                LogoutLatitude = ca.LogoutLatitude,
        ////                Description = ca.Description,
        ////                ManPowerApproval = ca.ManPowerApproval,
        ////                IsApproved = ca.IsApproved,
        ////                IsLogoutManager = ca.IsLogoutManager,
        ////                CreatedBy = ca.CreatedBy,
        ////                CreatedDate = ca.CreatedDate,
        ////                LastUpdatedBy = ca.LastUpdatedBy,
        ////                LastUpdatedDate = ca.LastUpdatedDate,
        ////                IsActive = ca.IsActive,
        ////                IsUpdated = ca.IsUpdated,
        ////                IsDeleted = ca.IsDeleted
        ////            })
        ////            .ToList();

        ////        if (!attendanceList.Any())
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "Contract Attendance Details Not Found");

        ////        return attendanceList;
        ////    }
        ////    catch (CustomApiException)
        ////    {
        ////        throw;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        // Log the exception here
        ////        throw new CustomApiException(HttpStatusCode.InternalServerError,
        ////            "An error occurred while processing your request");
        ////    }
        ////}
        public List<ContractAttendanceViewModel> ContractAttendanceManager(ContractViewModel model)

        {
            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                // Get employee details once
                var currentEmployee = DB.EmployeeMasters
                    .Where(emp => emp.EmpId == model.LoginId && emp.IsDeleted == false)
                    .Select(emp => new
                    {
                        emp.CategoryId,
                        emp.CompId,
                        emp.LEId,
                        emp.BUId,
                        emp.EmpCode,
                        emp.DeptName,
                        emp.LocationId
                    })
                    .FirstOrDefault();

                if (currentEmployee == null)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee not found");

                // Base query
                IQueryable<ContractAttendance> query;


                var isHR = DB.EmployeeMasters.Any(e => e.EmpId == model.LoginId && e.IsDeleted == false && e.DeptName.ToUpper().Trim() == "HUMAN RESOURCE");

                if (isHR)
                {

                    query = DB.ContractAttendances
                            .Where(ca => ca.IsDeleted == false);
                }
                else
                {
                    // Regular manager sees only their team
                    query = DB.ContractAttendances
                      .Where(ca => ca.IsDeleted == false &&
                        ca.ManagerId == model.LoginId);
                }
                if (model.FromDate != null && model.ToDate != null)
                {
                    var fromDate = model.FromDate.Value.Date;
                    var toDate = model.ToDate.Value.Date.AddDays(1);

                    query = query.Where(ca => ca.Date >= fromDate &&
                                              ca.Date < toDate);
                }
                else
                {
                    // ✅ Default → Last 2 Days (Yesterday + Today)
                    var twoDaysAgo = DateTime.Today.AddDays(-2);
                    var tomorrow = DateTime.Today.AddDays(1);

                    query = query.Where(ca => ca.Date >= twoDaysAgo &&
                                              ca.Date < tomorrow);
                }
                // Apply project filter
                if (model.ProjectId > 0)
                {
                    query = query.Where(ca => ca.ProjectId == model.ProjectId);
                }

                // Apply vendor filter
                if (model.VendorId > 0)
                {
                    query = query.Where(ca => ca.VendorId == model.VendorId);
                }

                // Get results with optimized mapping
                var attendanceList = query
                    .OrderByDescending(ca => ca.CreatedDate)
                    .Select(ca => new ContractAttendanceViewModel
                    {
                        CId = ca.CId,
                        Date = ca.Date,
                        Mobile = ca.Mobile,
                        Mail = ca.Mail,
                        EmpCode = ca.EmpCode,
                        EmpName = ca.EmpName,
                        Skill = ca.Skill,
                        VendorId = ca.VendorId,
                        Vendor = ca.Vendor,
                        VendorCode = ca.VendorCode,
                        ProjectId = ca.ProjectId,
                        ProjectCode = ca.ProjectCode,
                        Project = ca.Project,
                        SiteId = ca.SiteId,
                        Site = ca.Site,
                        SiteDetails = ca.SiteDetails,
                        ManagerId = ca.ManagerId,
                        ManagerEmpCode = ca.ManagerEmpCode,
                        ManagerName = ca.ManagerName,
                        Status = ca.Status,
                        LoginStatus = ca.IsLogin == true && ca.IsLogout == false ? "LOGIN" :
                                     ca.IsLogin == true && ca.IsLogout == true ? "LOGOUT" : "UNKNOWN",
                        IsLogin = ca.IsLogin,
                        IsLogout = ca.IsLogout,
                        LoginTime = ca.LoginTime,
                        LogoutTime = ca.LogoutTime,
                        Activehrs = ca.Activehrs,
                        Approvedhrs = ca.Approvedhrs,
                        LoginAddress = ca.LoginAddress,
                        LoginLonqitude = ca.LoginLonqitude,
                        LoginLatitude = ca.LoginLatitude,
                        LogoutAddress = ca.LogoutAddress,
                        LogoutLonqitude = ca.LogoutLonqitude,
                        LogoutLatitude = ca.LogoutLatitude,
                        Description = ca.Description,
                        ManPowerApproval = ca.ManPowerApproval,
                        IsApproved = ca.IsApproved,
                        IsLogoutManager = ca.IsLogoutManager,
                        CreatedBy = ca.CreatedBy,
                        CreatedDate = ca.CreatedDate,
                        LastUpdatedBy = ca.LastUpdatedBy,
                        LastUpdatedDate = ca.LastUpdatedDate,
                        IsActive = ca.IsActive,
                        IsUpdated = ca.IsUpdated,
                        IsDeleted = ca.IsDeleted
                    })
                    .ToList();

                if (!attendanceList.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Contract Attendance Details Not Found");

                return attendanceList;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    "An error occurred while processing your request");
            }
        }
        public List<ContractAttendanceViewModel> ERPContractAttendanceManager(ContractViewModel model)
        {
            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                // Base query
                IQueryable<ContractAttendance> query;

                // HR sees all employees in their company/LE/BU/Location
                query = from ca in DB.ContractAttendances
                        where ca.IsDeleted == false && ca.IsApproved == true && ca.IsActive == true
                        select ca;

                // Apply date filter
                if (model.FromDate.HasValue && model.ToDate.HasValue)
                {
                    var fromDate = model.FromDate.Value.Date;
                    var toDate = model.ToDate.Value.Date.AddDays(1); // Include the entire end date
                    query = query.Where(ca => ca.Date >= fromDate && ca.Date < toDate);
                }
                else
                {
                    var yesterday = DateTime.Now.Date.AddDays(-1);
                    var today = DateTime.Now.Date;
                    var tomorrow = DateTime.Now.Date.AddDays(1);
                    query = query.Where(ca => ca.Date == today);
                }

                // Get results with optimized mapping
                var attendanceList = query
                    .OrderByDescending(ca => ca.CreatedDate)
                    .Select(ca => new ContractAttendanceViewModel
                    {
                        CId = ca.CId,
                        Date = ca.Date,
                        Mobile = ca.Mobile,
                        Mail = ca.Mail,
                        EmpCode = ca.EmpCode,
                        EmpName = ca.EmpName,
                        Skill = ca.Skill,
                        VendorId = ca.VendorId,
                        ERPVendorId = ca.ERPVendorId,
                        Vendor = ca.Vendor,
                        VendorCode = ca.VendorCode,
                        ProjectId = ca.ProjectId,
                        ERPProjectId = ca.ERPProjectId,
                        ProjectCode = ca.ProjectCode,
                        Project = ca.Project,
                        SiteId = ca.SiteId,
                        Site = ca.Site,
                        SiteDetails = ca.SiteDetails,
                        ManagerId = ca.ManagerId,
                        ManagerEmpCode = ca.ManagerEmpCode,
                        ManagerName = ca.ManagerName,
                        Status = ca.Status,
                        LoginStatus = ca.IsLogin == true && ca.IsLogout == false ? "LOGIN" :
                                     ca.IsLogin == true && ca.IsLogout == true ? "LOGOUT" : "UNKNOWN",
                        IsLogin = ca.IsLogin,
                        IsLogout = ca.IsLogout,
                        LoginTime = ca.LoginTime,
                        LogoutTime = ca.LogoutTime,
                        Activehrs = ca.Activehrs,
                        Approvedhrs = ca.Approvedhrs,
                        LoginAddress = ca.LoginAddress,
                        LoginLonqitude = ca.LoginLonqitude,
                        LoginLatitude = ca.LoginLatitude,
                        LogoutAddress = ca.LogoutAddress,
                        LogoutLonqitude = ca.LogoutLonqitude,
                        LogoutLatitude = ca.LogoutLatitude,
                        Description = ca.Description,
                        ManPowerApproval = ca.ManPowerApproval,
                        IsApproved = ca.IsApproved,
                        IsLogoutManager = ca.IsLogoutManager,
                        CreatedBy = ca.CreatedBy,
                        CreatedDate = ca.CreatedDate,
                        LastUpdatedBy = ca.LastUpdatedBy,
                        LastUpdatedDate = ca.LastUpdatedDate,
                        IsActive = ca.IsActive,
                        IsUpdated = ca.IsUpdated,
                        IsDeleted = ca.IsDeleted
                    })
                    .ToList();

                if (!attendanceList.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Contract Attendance Details Not Found");

                return attendanceList;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    "An error occurred while processing your request");
            }
        }
        public List<ProjectMasterViewModel> ERPProjectmappingDetails(ContractViewModel model)
        {
            try
            {
                // Validate LoginId
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                // Base query - Get all active projects that are not deleted
                var query = from pm in DB.ProjectMasters
                            where pm.IsDeleted == false
                            // Optional: Add IsActive filter if needed
                            // && pm.IsActive == true
                            select pm;

                // Apply additional filters if needed (example - uncomment if required)
                /*
                if (model.CompId > 0)
                {
                    query = query.Where(pm => pm.CompId == model.CompId);
                }

                if (model.ProjectManagerId > 0)
                {
                    query = query.Where(pm => pm.ProjectManagerId == model.ProjectManagerId);
                }
                */

                // Get results with optimized mapping
                var projectList = query
                    .OrderByDescending(pm => pm.CreatedDate)
                    .Select(pm => new ProjectMasterViewModel
                    {
                        ProjectId = pm.ProjectId,
                        ERPProjectId = pm.ERPProjectId,
                        ProjectCode = pm.ProjectCode,
                        Project = pm.Project,
                        Description = pm.Description,
                        SiteId = pm.SiteId,
                        Site = pm.Site,
                        ProjectManagerId = pm.ProjectManagerId,
                        ManagerCode = pm.ManagerCode,
                        ManagerName = pm.ManagerName,
                        CreatedBy = pm.CreatedBy,
                        CreatedDate = pm.CreatedDate,
                        LastUpdatedBy = pm.LastUpdatedBy,
                        LastUpdatedDate = pm.LastUpdatedDate,
                        IsActive = pm.IsActive,
                        IsUpdated = pm.IsUpdated,
                        IsDeleted = pm.IsDeleted
                    })
                    .ToList();

                // Check if any projects found
                if (!projectList.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Project Details Not Found");

                return projectList;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here with details
                // _logger.LogError(ex, "Error in ERPProjectmappingDetails for LoginId: {LoginId}", model?.LoginId);

                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    "An error occurred while processing your request. Please try again later.");
            }
        }
        public LeaveResponseViewModel AddContractAttendance(ContractAttendanceViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                DateTime Today = DateTime.Now;
                string Date = Today.ToString("yyyy-MM-dd"); // Fixed date format
                string Time = DateTime.Now.ToString("HH:mm:ss"); // Fixed time format and added semicolon

                var attendance = DB.ContractAttendances
                    .Where(emp =>
                        emp.Date == Today &&
                        emp.Mobile == model.Mobile &&
                        emp.IsLogin == true &&
                        emp.IsLogout == false &&
                        emp.IsActive == true &&
                        emp.IsDeleted == false
                    )
                    .FirstOrDefault();

                if (loginId != 0)
                {
                    if (attendance == null) // Fixed: was using Count() which doesn't exist on single object
                    {
                        if (model.LoginStatus.ToUpper() == "NO DATA") // Fixed: was using = instead of ==
                        {
                            ContractAttendance ca = new ContractAttendance();
                            // Fixed: Date property type mismatch - need to parse or use DateTime
                            ca.Date = DateTime.Parse(Date); // Assuming Date property is DateTime
                            ca.Mobile = model.Mobile;
                            ca.Mail = model.Mail;
                            ca.EmpCode = model.EmpCode;
                            ca.EmpName = model.EmpName;
                            ca.Skill = model.Skill;
                            ca.VendorId = model.VendorId;
                            ca.ERPVendorId = model.ERPVendorId;
                            ca.VendorCode = model.VendorCode;
                            ca.Vendor = model.Vendor;
                            ca.ProjectId = model.ProjectId;
                            ca.ERPProjectId = model.ERPProjectId;
                            ca.ProjectCode = model.ProjectCode;
                            ca.Project = model.Project;
                            ca.SiteId = model.SiteId;
                            ca.Site = model.Site;
                            ca.SiteDetails = model.SiteDetails;
                            ca.ManagerId = model.ManagerId;
                            ca.ManagerEmpCode = model.ManagerEmpCode;
                            ca.ManagerName = model.ManagerName;
                            ca.Status = true;
                            ca.IsLogin = true;
                            ca.IsLogout = false;
                            //ca.Activehrs = model.Activehrs;
                            ca.Approvedhrs = model.Approvedhrs;
                            ca.LoginTime = TimeSpan.Parse(Time); // Fixed: converting string to TimeSpan
                            ca.LoginAddress = model.LoginAddress;
                            ca.LoginLonqitude = model.LoginLonqitude;
                            ca.LoginLatitude = model.LoginLatitude;
                            //ca.LogoutAddress = model.LogoutAddress;
                            //ca.LogoutLonqitude = model.LogoutLonqitude;
                            //ca.LogoutLatitude = model.LogoutLatitude;
                            ca.IsLogoutManager = false;
                            ca.IsApproved = false;
                            ca.Description = "";
                            ca.ManPowerApproval = "";
                            ca.CreatedBy = 149;
                            ca.CreatedDate = DateTime.Now;
                            ca.LastUpdatedBy = 149;
                            ca.LastUpdatedDate = DateTime.Now;
                            ca.IsActive = true;
                            ca.IsUpdated = false;
                            ca.IsDeleted = false;
                            DB.ContractAttendances.Add(ca);
                            DB.SaveChanges();

                            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Login Successfully";

                            return emvm;
                        }
                        else // Logout case
                        {
                            var Contattendance = DB.ContractAttendances
                                                .Where(emp =>
                                                    emp.CId == model.CId
                                                )
                                                .FirstOrDefault();

                            if (Contattendance != null)
                            {
                                // Fixed: TimeSpan calculation
                                TimeSpan loginTime = Contattendance.LoginTime ?? TimeSpan.Zero;
                                TimeSpan logoutTime = TimeSpan.Parse(Time);
                                TimeSpan activehrs = logoutTime - loginTime;

                                Contattendance.LogoutTime = logoutTime; // Assuming there's a LogoutTime property
                                Contattendance.LogoutAddress = model.LogoutAddress;
                                Contattendance.LogoutLonqitude = model.LogoutLonqitude;
                                Contattendance.LogoutLatitude = model.LogoutLatitude;
                                Contattendance.IsLogout = true;
                                Contattendance.Activehrs = activehrs; // Set the active hours
                                Contattendance.Description = model.Description; // Fixed: was using attendance.Description
                                Contattendance.LastUpdatedBy = 149;
                                Contattendance.LastUpdatedDate = DateTime.Now;
                                Contattendance.IsActive = true;
                                Contattendance.IsUpdated = true;
                                Contattendance.IsDeleted = false;

                                DB.SaveChanges();

                                LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                                emvm.Status = 200;
                                emvm.msg = "Logout Successfully"; // Fixed: message should be Logout

                                return emvm;
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Attendance record not found for logout");
                            }
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.BadRequest, "Already logged in for today"); // Fixed: message and status code
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is required"); // Fixed: message and status code
                }
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new CustomApiException(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        public LeaveResponseViewModel LogoutbyManager(ContractAttendanceViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? cId = (model.CId != 0) ? model.CId : 0;

                DateTime Today = DateTime.Now;
                string Date = Today.ToString("yyyy-MM-dd"); // Fixed date format
                string Time = DateTime.Now.ToString("HH:mm:ss"); // Fixed time format and added semicolon

                if (loginId != 0)
                {
                    var Contattendance = DB.ContractAttendances
                                                .Where(emp =>
                                                    emp.CId == cId
                                                )
                                                .FirstOrDefault();

                    if (Contattendance != null)
                    {
                        // Fixed: TimeSpan calculation
                        TimeSpan loginTime = Contattendance.LoginTime ?? TimeSpan.Zero;
                        TimeSpan logoutTime = TimeSpan.Parse(Time);
                        TimeSpan activehrs = logoutTime - loginTime;

                        Contattendance.LogoutTime = logoutTime; // Assuming there's a LogoutTime property
                        Contattendance.LogoutAddress = model.LogoutAddress;
                        Contattendance.LogoutLonqitude = model.LogoutLonqitude;
                        Contattendance.LogoutLatitude = model.LogoutLatitude;
                        Contattendance.IsLogout = true;
                        Contattendance.Activehrs = activehrs; // Set the active hours
                        Contattendance.Description = model.Description; // Fixed: was using attendance.Description
                        Contattendance.IsLogoutManager = true;
                        Contattendance.LastUpdatedBy = 149;
                        Contattendance.LastUpdatedDate = DateTime.Now;
                        Contattendance.IsActive = true;
                        Contattendance.IsUpdated = true;
                        Contattendance.IsDeleted = false;

                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Logout Successfully"; // Fixed: message should be Logout

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Attendance record not found for logout");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is required"); // Fixed: message and status code
                }
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new CustomApiException(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        public LeaveResponseViewModel ApprovedHrbyManager(ContractAttendanceViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? cId = (model.CId != 0) ? model.CId : 0;

                DateTime Today = DateTime.Now;
                string Date = Today.ToString("yyyy-MM-dd"); // Fixed date format
                string Time = DateTime.Now.ToString("HH:mm:ss"); // Fixed time format and added semicolon

                if (loginId != 0)
                {
                    var Contattendance = DB.ContractAttendances
                                                .Where(emp =>
                                                    emp.CId == cId
                                                )
                                                .FirstOrDefault();

                    if (Contattendance != null)
                    {
                        Contattendance.Approvedhrs = model.Approvedhrs;
                        Contattendance.LastUpdatedBy = 149;
                        Contattendance.LastUpdatedDate = DateTime.Now;
                        Contattendance.IsActive = true;
                        Contattendance.IsUpdated = true;
                        Contattendance.IsDeleted = false;

                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Approved hours added Successfully"; // Fixed: message should be Logout

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Attendance record not found for logout");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is required"); // Fixed: message and status code
                }
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new CustomApiException(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        public LeaveResponseViewModel ApprovedbyManager(ContractApprovedViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? cId = (model.CId != 0) ? model.CId : 0;

                DateTime Today = DateTime.Now;
                string Date = Today.ToString("yyyy-MM-dd"); // Fixed date format
                string Time = DateTime.Now.ToString("HH:mm:ss"); // Fixed time format and added semicolon

                if (loginId != 0)
                {
                    if(model.lstofCantractIId.Count() > 0)
                    {
                        for (int i = 0; i < model.lstofCantractIId.Count(); i++)
                        {
                            int? cId = (model.lstofCantractIId[i].CId != 0) ? model.lstofCantractIId[i].CId : 0;

                            var Contattendance = DB.ContractAttendances
                                                .Where(emp =>
                                                    emp.CId == cId
                                                )
                                                .FirstOrDefault();

                            if (Contattendance != null)
                            {
                                Contattendance.IsApproved = true;
                                Contattendance.LastUpdatedBy = 149;
                                Contattendance.LastUpdatedDate = DateTime.Now;
                                Contattendance.IsActive = true;
                                Contattendance.IsUpdated = true;
                                Contattendance.IsDeleted = false;

                                DB.SaveChanges();
                            }
                        }

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Approved Successfully"; // Fixed: message should be Logout

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "CFIds missing");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is required"); // Fixed: message and status code
                }
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new CustomApiException(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        public List<DDVendorListViewModel> DDVendorList(DDVendorListViewModel empdd)
        {
            try
            {
                if (empdd.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                var query = DB.VendorMasters
                    .Where(emp =>
                        emp.IsActive == true &&
                        emp.IsDeleted == false
                    );

                var result = query
                    .OrderByDescending(emp => emp.VendorId)
                    .Select(emp => new DDVendorListViewModel
                    {
                        VendorId = emp.VendorId,
                        ERPVendorId = emp.ERPVendorId,
                        VendorCode = emp.VendorCode,
                        Vendor = emp.Vendor
                    })
                    .ToList();

                if (!result.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Vendor Details Not Found");

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }
        public List<DDSiteListViewModel> DDSiteList(DDSiteListViewModel empdd)
        {
            try
            {
                if (empdd.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                var query = DB.SiteMasters
                    .Where(emp =>
                        emp.IsActive == true &&
                        emp.IsDeleted == false
                    );

                var result = query
                    .OrderByDescending(emp => emp.SiteId)
                    .Select(emp => new DDSiteListViewModel
                    {
                        SiteId = emp.SiteId,
                        Site = emp.Site
                    })
                    .ToList();

                if (!result.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Site Details Not Found");

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }
        public List<DDProjectListViewModel> DDProjectList(DDProjectListViewModel empdd)
        {
            try
            {
                if (empdd.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                var query = DB.ProjectMasters
                    .Where(emp =>
                        emp.IsActive == true &&
                        emp.IsDeleted == false
                    );

                var result = query
                    .OrderByDescending(emp => emp.ProjectId)
                    .Select(emp => new DDProjectListViewModel
                    {
                        ProjectId = emp.ProjectId,
                        ERPProjectId = emp.ERPProjectId,
                        Project = emp.Project,
                        ProjectCode = emp.ProjectCode,
                        ManagerId = emp.ProjectManagerId,
                        ManagerCode = emp.ManagerCode,
                        ManagerName = emp.ManagerName,
                        SiteId = emp.SiteId,
                        Site = emp.Site,
                    })
                    .ToList();

                if (!result.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Project Details Not Found");

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }
        public responseViewModel AddVendorList(VendorListViewModel model)
        {
            responseViewModel rvm = new responseViewModel();

            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                if (model.ERPVendorId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "ERP VendorId is Missing");

                var query = DB.VendorMasters
                    .FirstOrDefault(v => v.ERPVendorId == model.ERPVendorId && v.IsActive == model.IsActive);

                if (query == null)
                {
                    VendorMaster vm = new VendorMaster
                    {
                        ERPVendorId = model.ERPVendorId,
                        VendorCode = model.VendorCode,
                        Vendor = model.Vendor,
                        Description = model.Description,
                        CreatedBy = model.LoginId,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedBy = model.LoginId,
                        LastUpdatedDate = DateTime.UtcNow,
                        IsActive = model.IsActive,
                        IsUpdated = false,
                        IsDeleted = false
                    };

                    DB.VendorMasters.Add(vm);
                    DB.SaveChanges();

                    rvm.status = 200;
                    rvm.msg = "Vendor Added Successfully!";
                }
                else
                {
                    query.VendorCode = model.VendorCode;
                    query.Vendor = model.Vendor;
                    query.Description = model.Description;
                    query.LastUpdatedBy = model.LoginId;
                    query.LastUpdatedDate = DateTime.UtcNow;
                    query.IsActive = model.IsActive;
                    query.IsUpdated = true;
                    query.IsDeleted = false;

                    DB.SaveChanges();

                    rvm.status = 200;
                    rvm.msg = "Vendor Updated Successfully!";
                }

                return rvm;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, "Something went wrong");
            }
        }
        public responseViewModel AddProjectList(ProjectListViewModel model)
        {
            responseViewModel rvm = new responseViewModel();

            try
            {
                // 🔹 Validations
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                if (model.ERPProjectId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "ERP ProjectId is Missing");

                if (string.IsNullOrWhiteSpace(model.ManagerCode))
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Manager Code is Missing");

                // 🔹 Get Manager Details
                var manager = DB.EmployeeMasters
                    .FirstOrDefault(e => e.EmpCode.ToUpper() == model.ManagerCode.ToUpper());

                if (manager == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Manager not found in Office Connect system");

                // 🔹 Check Existing Project (Removed Active filter to avoid duplicates)
                var project = DB.ProjectMasters
                    .FirstOrDefault(p => p.ERPProjectId == model.ERPProjectId && p.IsActive == model.IsActive);

                if (project == null)
                {
                    // 🔹 Insert
                    ProjectMaster pm = new ProjectMaster
                    {
                        ERPProjectId = model.ERPProjectId,
                        ProjectCode = model.ProjectCode,
                        Project = model.Project,
                        Description = model.Description,
                        SiteId = model.SiteId,
                        Site = model.Site,
                        ManagerCode = model.ManagerCode,
                        ProjectManagerId = manager.EmpId,
                        ManagerName = model.ManagerName,
                        CreatedBy = model.LoginId,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedBy = model.LoginId,
                        LastUpdatedDate = DateTime.UtcNow,
                        IsActive = model.IsActive,
                        IsUpdated = false,
                        IsDeleted = false
                    };

                    DB.ProjectMasters.Add(pm);
                    DB.SaveChanges();

                    rvm.status = 200;
                    rvm.msg = "Project Added Successfully!";
                }
                else
                {
                    // 🔹 Update
                    project.ProjectCode = model.ProjectCode;
                    project.Project = model.Project;
                    project.Description = model.Description;
                    project.SiteId = model.SiteId;
                    project.Site = model.Site;
                    project.ManagerCode = model.ManagerCode;
                    project.ProjectManagerId = manager.EmpId;
                    project.ManagerName = model.ManagerName;
                    project.LastUpdatedBy = model.LoginId;
                    project.LastUpdatedDate = DateTime.UtcNow;
                    project.IsActive = model.IsActive;
                    project.IsUpdated = true;
                    project.IsDeleted = false;

                    DB.SaveChanges();

                    rvm.status = 200;
                    rvm.msg = "Project Updated Successfully!";
                }

                // 🔹 Success Log
                //LogsModel.LogSuccess($"Project Sync Success - ERPProjectId: {model.ERPProjectId}", "ProjectAPI");

                return rvm;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, "Something went wrong");
            }
        }
        public EmpProbationTrackingHistoryListViewModel GetAllEmpProbationTrackingHistory(EmpProbationTrackingHistoryViewModel model)
        {
            try
            {
                // ✅ Validate input
                if (model?.LoginId == null || model.LoginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");

                if (model?.LEId == null || model.LEId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Legal Entity is Missing");

                int loginId = model.LoginId.Value;
                int leId = model.LEId.Value;

                // ✅ Get logged-in employee
                var empdetails = DB.EmployeeMasters
                    .FirstOrDefault(emp => emp.EmpId == loginId &&
                                           emp.IsActive == true&&
                                           emp.IsDeleted == false &&
                                           emp.EmpStatus == "ACTIVE");

                if (empdetails == null)
                    throw new CustomApiException(HttpStatusCode.NotFound,
                        $"Employee with ID {loginId} not found or is not active");

                int? deptId = empdetails.CategoryId;

                // ✅ Base Query (JOIN only once)
                var query = from pt in DB.EmpProbationTrackingHistories
                            join em in DB.EmployeeMasters on pt.EmpId equals em.EmpId
                            where pt.IsActive == true && pt.IsDeleted == false
                               && em.IsActive == true && em.IsDeleted == false
                               && em.LEId == leId
                               && em.EmpStatus == "ACTIVE"
                            select new { pt, em };

                // ✅ Dynamic Filters
                if (model.BuId.HasValue && model.BuId > 0)
                    query = query.Where(x => x.em.BUId == model.BuId);

                if (model.LocId.HasValue && model.LocId > 0)
                    query = query.Where(x => x.em.LocationId == model.LocId);

                if (model.DeptId.HasValue && model.DeptId > 0)
                    query = query.Where(x => x.em.CategoryId == model.DeptId);

                if (model.DesignationId.HasValue && model.DesignationId > 0)
                    query = query.Where(x => x.em.DesignationId == model.DesignationId);

                if (model.ReporterId.HasValue && model.ReporterId > 0)
                    query = query.Where(x => x.em.ReportId == model.ReporterId);

                if (model.EmpId.HasValue && model.EmpId > 0)
                    query = query.Where(x => x.em.EmpId == model.EmpId);

                // ✅ Role-based filter
                const int HR_DEPT_ID = 1;
                if (deptId != HR_DEPT_ID)
                    query = query.Where(x => x.pt.ReportId == loginId);

                // ✅ Execute query (IMPORTANT: keep both pt & em)
                var allRecords = query
                    .OrderByDescending(x => x.pt.CreatedDate)
                    .ToList();

                // ✅ Mapping (NO DB CALLS)
                var result = new EmpProbationTrackingHistoryListViewModel
                {
                    PendingProbationList = allRecords
                        .Where(x => x.pt.IsProbation == true)
                        .Select(x => MapToViewModel(x.pt, x.em))
                        .ToList(),

                    ProbationHistoryList = allRecords
                        .Where(x => x.pt.IsProbation == false)
                        .Select(x => MapToViewModel(x.pt, x.em))
                        .ToList()
                };

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }
        private EmpProbationTrackingHistoryViewModel MapToViewModel(EmpProbationTrackingHistory pt, EmployeeMaster em)
        {
            return new EmpProbationTrackingHistoryViewModel
            {
                EmpProbationId = pt.EmpProbationId,
                LoginId = pt.EmpId,
                EmpId = pt.EmpId,

                EmpName = (em.FirstName ?? "") + " " +
                          (em.MiddleName ?? "") + " " +
                          (em.LastName ?? ""),

                EmpCode = em.EmpCode,

                JoiningDate = pt.JoiningDate,
                ProbationDays = pt.ProbationDays,
                ProbationEndDate = pt.ProbationEndDate,
                ReportId = pt.ReportId,
                ReportCode = pt.ReportCode,
                IsProbation = pt.IsProbation,
                IsPermanent = pt.IsPermanent,
                IsContract = pt.IsContract,
                IsConsultant = pt.IsConsultant,
                ConfirmDate = pt.ConfirmDate,
                ConfirmBy = pt.ConfirmBy,
                Remarks = pt.Remarks,
                CreatedBy = pt.CreatedBy,
                CreatedDate = pt.CreatedDate,
                LastupdatedBy = pt.LastupdatedBy,
                LastUpdatedDate = pt.LastUpdatedDate,
                IsActive = pt.IsActive,
                IsUpdated = pt.IsUpdated,
                IsDeleted = pt.IsDeleted
            };
        }
        public responseViewModel ConfirmProbation(EmpProbationTrackingHistoryViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                if (loginId != 0)
                {
                        int? empid = model.EmpId;

                    var pthdetails = (from pth in DB.EmpProbationTrackingHistories
                                      where pth.EmpId == empid && pth.IsProbation == true &&
                                      pth.IsActive == true && pth.IsDeleted == false
                                      select pth).FirstOrDefault();


                    if (pthdetails != null)
                        {
                        var empdetails = (from emp in DB.EmployeeMasters
                                          where emp.EmpId == empid && emp.EmpStatus.ToUpper() == "Active" &&
                                          emp.IsActive == true && emp.IsDeleted == false
                                          select emp).FirstOrDefault();

                        if (empdetails != null)
                        {
                            pthdetails.IsProbation = false;
                            pthdetails.ConfirmBy = loginId;
                            pthdetails.ConfirmDate = DateTime.Now;
                            pthdetails.IsPermanent = true;
                            pthdetails.Remarks = model.Remarks;
                            pthdetails.LastupdatedBy = model.LoginId;
                            pthdetails.LastUpdatedDate = DateTime.Now;
                            pthdetails.IsUpdated = true;
                            DB.SaveChanges();

                            responseViewModel rvm = new responseViewModel();
                            rvm.status = 200;
                            rvm.msg = "Employee Permanented";

                            return rvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Employee is not active");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Probation details is not found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Login Id is mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDReporterListViewModel> GetDDReporterList(DDReporterListViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;
                int? BUId = (model.BUId != 0) ? model.BUId : 0;
                int? LocationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? DeptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? DesignationId = (model.DesignationId != 0) ? model.DesignationId : 0;
                int? ReporterId = (model.ReporterId != 0) ? model.ReporterId : 0;

                var Reporterdetails = DB.EmployeeMasters
                                        .Where(lml => lml.EmpStatus.ToUpper() == "ACTIVE"
                                                   && lml.IsActive == true
                                                   && lml.IsDeleted == false
                                                   && lml.ReportId != null)
                                        .GroupBy(lml => lml.ReportId)
                                        .Select(g => g.FirstOrDefault()) // take only one employee per ReportId
                                        .Join(DB.EmployeeMasters,
                                              emp => emp.ReportId,
                                              lm => lm.EmpId,
                                              (emp, lm) => new DDReporterListViewModel
                                              {
                                                  CompId = lm.CompId,
                                                  LEId = lm.LEId,
                                                  BUId = lm.BUId,
                                                  LocationId = lm.LocationId,
                                                  DeptId = lm.CategoryId,
                                                  DesignationId = lm.DesignationId,
                                                  ReporterId = lm.EmpId,
                                                  ReporterName = lm.FirstName + " " + lm.MiddleName + " " + lm.LastName,
                                                  ReporterCode = lm.EmpCode
                                              })
                                        .Where(lm => lm != null)
                                        .ToList();


                if (LEId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.LEId == LEId).ToList();
                }
                if (BUId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.BUId == BUId).ToList();
                }
                if (LocationId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.LocationId == LocationId).ToList();
                }
                if (DeptId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.DeptId == DeptId).ToList();
                }
                if (DesignationId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.DesignationId == DesignationId).ToList();
                }
                ////if (ReporterId != 0)
                ////{
                ////    Reporterdetails = Reporterdetails.Where(x => x.ReporterId == ReporterId).ToList();
                ////}

                if (LoginId != 0)
                {
                    if (Reporterdetails != null)
                    {
                        return Reporterdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Reporter Details Not Found");
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
        public List<DDEmployeeListViewModel> GetDDEmployeeList(DDEmployeeListViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? LEId = (model.LEId != 0) ? model.LEId : 0;
                int? BUId = (model.BUId != 0) ? model.BUId : 0;
                int? LocationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? DeptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? DesignationId = (model.DesignationId != 0) ? model.DesignationId : 0;
                int? ReporterId = (model.ReporterId != 0) ? model.ReporterId : 0;

                var Reporterdetails = (from lm in DB.EmployeeMasters 
                                       where lm.EmpStatus.ToUpper() == "ACTIVE" && lm.IsActive == true && lm.IsDeleted == false 
                                       select new DDEmployeeListViewModel
                                       { 
                                           CompId = lm.CompId, 
                                           LEId = lm.LEId, 
                                           BUId = lm.BUId, 
                                           LocationId = lm.LocationId, 
                                           DeptId = lm.CategoryId, 
                                           DesignationId = lm.DesignationId,
                                           EmpId = lm.EmpId,
                                           EmpName = lm.FirstName + lm.MiddleName + lm.LastName,
                                           EmpCode = lm.EmpCode,
                                           ReporterId = lm.ReportId
                                       }).ToList();

                if (LEId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.LEId == LEId).ToList();
                }
                if (BUId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.BUId == BUId).ToList();
                }
                if (LocationId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.LocationId == LocationId).ToList();
                }
                if (ReporterId != 0)
                {
                    Reporterdetails = Reporterdetails.Where(x => x.ReporterId == ReporterId).ToList();
                }
                else
                {
                    if (DeptId != 0)
                    {
                        Reporterdetails = Reporterdetails.Where(x => x.DeptId == DeptId).ToList();
                    }
                    if (DesignationId != 0)
                    {
                        Reporterdetails = Reporterdetails.Where(x => x.DesignationId == DesignationId).ToList();
                    }
                }
                if (LoginId != 0)
                {
                    if (Reporterdetails != null)
                    {
                        return Reporterdetails;
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
        public List<EmployeeMasterLogViewModel> GetAllEmployeeLogHistory(EmployeeMasterLogViewModel model)
        {
            try
            {
                // ✅ Validate input
                if (model?.LoginId == null || model.LoginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");

                if (model?.LEId == null || model.LEId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Legal Entity is Missing");

                int loginId = model.LoginId.Value;
                int leId = model.LEId.Value;

                // ✅ Get logged-in employee
                var empdetails = DB.EmployeeMasters
                    .FirstOrDefault(emp => emp.EmpId == loginId &&
                                           emp.IsActive == true &&
                                           emp.IsDeleted == false &&
                                           emp.EmpStatus == "ACTIVE");

                if (empdetails == null)
                    throw new CustomApiException(HttpStatusCode.NotFound,
                        $"Employee with ID {loginId} not found or is not active");

                int? deptId = empdetails.CategoryId;
                const int HR_DEPT_ID = 1;

                // ✅ Build query with proper IQueryable
                var query = DB.EmployeeMasterLogs
                    .Where(pt => pt.IsActive == true && pt.IsDeleted == false);

                // ✅ Apply filters BEFORE executing query
                if (model.BUId.HasValue && model.BUId > 0)
                    query = query.Where(x => x.BUId == model.BUId);

                if (model.LocationId.HasValue && model.LocationId > 0)
                    query = query.Where(x => x.LocationId == model.LocationId);

                if (model.DeptId.HasValue && model.DeptId > 0)
                    query = query.Where(x => x.CategoryId == model.DeptId);

                if (model.DesignationId.HasValue && model.DesignationId > 0)
                    query = query.Where(x => x.DesignationId == model.DesignationId);

                if (model.ReportId.HasValue && model.ReportId > 0)
                    query = query.Where(x => x.ReportId == model.ReportId);

                if (model.EmpId.HasValue && model.EmpId > 0)
                    query = query.Where(x => x.EmpId == model.EmpId);

                // ✅ Role-based filter
                if (deptId != HR_DEPT_ID)
                    query = query.Where(x => x.ReportId == loginId);

                // ✅ Execute query ONCE
                var allRecords = query
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                if (!allRecords.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee Log Details Not Found");

                // ✅ Get all required IDs for batch loading (handle nulls properly)
                var empIds = allRecords.Select(x => x.EmpId).Distinct().ToList();
                var compIds = allRecords.Select(x => x.CompId).Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();
                var leIds = allRecords.Select(x => x.LEId).Where(x => x.HasValue && x != 0).Select(x => x.Value).Distinct().ToList();
                var buIds = allRecords.Select(x => x.BUId).Where(x => x.HasValue && x != 0).Select(x => x.Value).Distinct().ToList();
                var locationIds = allRecords.Select(x => x.LocationId).Where(x => x.HasValue && x != 0).Select(x => x.Value).Distinct().ToList();
                var reportIds = allRecords.Select(x => x.ReportId).Where(x => x.HasValue && x != 0).Select(x => x.Value).Distinct().ToList();
                var salutationIds = allRecords.Select(x => x.Salutation).Where(x => x.HasValue && x != 0).Select(x => x.Value).Distinct().ToList();
                var empTypeIds = allRecords.Select(x => x.EmpType).Where(x => x.HasValue && x != 0).Select(x => x.Value).Distinct().ToList();

                // ✅ Batch load reference data with proper int keys (not int?)
                var companies = DB.CompanyMasters
                    .Where(x => compIds.Contains(x.CompId))
                    .ToDictionary(x => x.CompId, x => x.Company);

                var legalEntities = DB.LegalEntityMasters
                    .Where(x => leIds.Contains(x.LEId))
                    .ToDictionary(x => x.LEId, x => x.LegalEntity);

                var businessUnits = DB.BusinessUnitMasters
                    .Where(x => buIds.Contains(x.BUId))
                    .ToDictionary(x => x.BUId, x => x.BusinessUnit);

                var locations = DB.LocationMasters
                    .Where(x => locationIds.Contains(x.LocationId))
                    .ToDictionary(x => x.LocationId, x => x.Location);

                var employees = DB.EmployeeMasters
                    .Where(x => reportIds.Contains(x.EmpId))
                    .ToDictionary(x => x.EmpId, x => new { x.FirstName, x.MiddleName, x.LastName, x.EmpCode });

                var salutations = DB.SalutationMasters
                    .Where(x => salutationIds.Contains(x.SalutationId))
                    .ToDictionary(x => x.SalutationId, x => x.Salutation);

                var empTypes = DB.EmpTypeMasters
                    .Where(x => empTypeIds.Contains(x.EmpTypId))
                    .ToDictionary(x => x.EmpTypId, x => x.EmpType);

                // ✅ Map results efficiently with proper null checking
                var result = new List<EmployeeMasterLogViewModel>();

                foreach (var record in allRecords)
                {
                    var vm = new EmployeeMasterLogViewModel
                    {
                        EmpId = record.EmpId,
                        OldEmp_ID = record.OldEmp_ID,
                        CompId = record.CompId ?? 0,
                        Company = (record.CompId.HasValue && companies.ContainsKey(record.CompId.Value))
                            ? companies[record.CompId.Value] : "",

                        LEId = (record.LEId.HasValue && record.LEId != 0) ? record.LEId.Value : 0,
                        LegalEntity = (record.LEId.HasValue && record.LEId != 0 && legalEntities.ContainsKey(record.LEId.Value))
                            ? legalEntities[record.LEId.Value] : "",

                        BUId = (record.BUId.HasValue && record.BUId != 0) ? record.BUId.Value : 0,
                        BusinessUnit = (record.BUId.HasValue && record.BUId != 0 && businessUnits.ContainsKey(record.BUId.Value))
                            ? businessUnits[record.BUId.Value] : "",

                        LocationId = (record.LocationId.HasValue && record.LocationId != 0) ? record.LocationId.Value : 0,
                        Location = (record.LocationId.HasValue && record.LocationId != 0 && locations.ContainsKey(record.LocationId.Value))
                            ? locations[record.LocationId.Value] : "",

                        CategoryId = record.CategoryId ?? 0,
                        DeptId = record.CategoryId ?? 0,
                        DeptName = record.DeptName ?? "",
                        DesignationId = record.DesignationId ?? 0,
                        Designation = record.DesignationName ?? "",
                        ReportId = record.ReportId ?? 0,
                        ApproverId = record.ReportId ?? 0,
                        AuthorisedEntity = record.AuthorisedEntity ?? "",

                        EmpCode = record.EmpCode ?? "",
                        UserName = record.UserName ?? "",
                        Photo = !string.IsNullOrEmpty(record.Photo) ?
                            (record.Photo.Contains("Uploads") ?
                                "Uploads" + record.Photo.Split(new[] { "Uploads" }, StringSplitOptions.None)[1] :
                                record.Photo) : "",

                        SalutationId = record.Salutation ?? 0,
                        Salutation = (record.Salutation.HasValue && record.Salutation != 0 && salutations.ContainsKey(record.Salutation.Value))
                            ? salutations[record.Salutation.Value] : "",

                        FirstName = record.FirstName ?? "",
                        MiddleName = record.MiddleName ?? "",
                        LastName = record.LastName ?? "",
                        DOB = record.DOB,
                        MobileNo = record.MobileNo ?? "",
                        EmailId = record.EmailId ?? "",
                        BloodGroup = record.BloodGroup ?? "",
                        MaritalStatus = record.MaritalStatus ?? "",
                        Gender = record.Gender ?? "",
                        JoiningDate = record.JoiningDate,
                        EndDate = record.EndDate,
                        EmpStatus = record.EmpStatus?.ToUpper() ?? "",
                        Reason = record.Reason ?? "",

                        EmpTypeId = record.EmpType ?? 0,
                        EmpType = (record.EmpType.HasValue && record.EmpType != 0 && empTypes.ContainsKey(record.EmpType.Value))
                            ? empTypes[record.EmpType.Value] : "",

                        CEndDate = record.CEndDate,
                        IsActive = record.IsActive ?? false,
                        IsUpdated = record.IsUpdated ?? false,
                        IsDeleted = record.IsDeleted ?? false,
                        CreatedBy = record.CreatedBy,
                        CreatedDate = record.CreatedDate,
                        LastUpdatedBy = record.LastUpdatedBy,
                        LastUpdatedDate = record.LastUpdatedDate
                    };

                    // ✅ Set Approver name with proper null handling
                    if (record.ReportId.HasValue && record.ReportId != 0 && employees.ContainsKey(record.ReportId.Value))
                    {
                        var approver = employees[record.ReportId.Value];
                        vm.Approver = $"{approver.FirstName ?? ""} {approver.MiddleName ?? ""} {approver.LastName ?? ""} - {approver.EmpCode ?? ""}".Trim();
                        vm.Approver = System.Text.RegularExpressions.Regex.Replace(vm.Approver, @"\s+", " "); // Remove extra spaces
                    }
                    else
                    {
                        vm.Approver = "";
                    }

                    result.Add(vm);
                }

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }
        public HierarchyFinalResponse GetDesignationHierarchy(HierarchyRequestViewModel model)
        {
            try
            {
                // Step 1: Get raw data from database (no ViewModel initialization in query)
                var employeesQuery = from em in DB.EmployeeMasters
                                     join dm in DB.DesignationMasters on em.DesignationId equals dm.DesignationId
                                     join lc in DB.LocationMasters on em.LocationId equals lc.LocationId
                                     join gm in DB.GradeMasters on dm.GradeId equals gm.GradeId into gradeJoin
                                     from gm in gradeJoin.DefaultIfEmpty()
                                     join dept in DB.DeptMasters on dm.DeptId equals dept.DeptId into deptJoin
                                     from dept in deptJoin.DefaultIfEmpty()
                                     where em.IsDeleted == false
                                           && em.IsActive == true
                                           && em.EmpStatus.ToUpper() == "ACTIVE"
                                     select new
                                     {
                                         em.EmpId,
                                         em.EmpCode,
                                         em.CompId,
                                         em.LEId,
                                         em.BUId,
                                         em.LocationId,
                                         
                                         em.FirstName,
                                         em.LastName,
                                         em.MiddleName,
                                         em.ReportId,
                                         em.EmpType,
                                         em.EmpStatus,
                                         DesignationName = dm.Designation,
                                         DesignationId = em.DesignationId,
                                         Location = lc != null ? lc.Location : "",
                                         GradeId = gm != null ? gm.GradeId : (int?)null,
                                         GradeName = gm != null ? gm.Grade : null,
                                         DeptId = dept != null ? dept.DeptId : (int?)null,
                                         DeptName = dept != null ? dept.DeptName : null,
                                         DeptShortName = dept != null ? dept.DeptShortName : null
                                     };

                // Apply filters
                if (model.CompId.HasValue && model.CompId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.CompId == model.CompId.Value);
                if (model.LEId.HasValue && model.LEId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.LEId == model.LEId.Value);
                if (model.BUId.HasValue && model.BUId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.BUId == model.BUId.Value);
                if (model.LocationId.HasValue && model.LocationId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.LocationId == model.LocationId.Value);
                if (model.DeptId.HasValue && model.DeptId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.DeptId == model.DeptId.Value);
                if (model.DesignationId.HasValue && model.DesignationId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.DesignationId == model.DesignationId.Value);
                if (model.ReporterId.HasValue && model.ReporterId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.ReportId == model.ReporterId.Value);
                if (model.GradeId.HasValue && model.GradeId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.GradeId == model.GradeId.Value);
                if (model.EmpId.HasValue && model.EmpId.Value != 0)
                    employeesQuery = employeesQuery.Where(e => e.EmpId == model.EmpId.Value);

                // Execute query and get data
                var rawData = employeesQuery.ToList();

                if (rawData == null || !rawData.Any())
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "No employees found with the specified filters");
                }

                // Step 2: Convert to ViewModel after data is retrieved
                var allEmployees = rawData.Select(e => new HierarchyResponseViewModel
                {
                    EmpId = e.EmpId,
                    EmpCode = e.EmpCode,
                    CompId = e.CompId,
                    LEId = e.LEId,
                    BUId = e.BUId,
                    LocationId = e.LocationId,
                    Location = e.Location,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    EmployeeName = (e.FirstName + " " + (e.MiddleName ?? "") + " " + e.LastName).Trim(),
                    DesignationId = e.DesignationId,
                    DesignationName = e.DesignationName,
                    GradeId = e.GradeId,
                    GradeName = e.GradeName,
                    DeptId = e.DeptId,
                    DeptName = e.DeptName,
                    DeptShortName = e.DeptShortName,
                    ReporterId = e.ReportId,
                    EmpType = e.EmpType,
                    EmpStatus = e.EmpStatus,
                    HierarchyLevel = 0,
                    ReporteesCount = 0,
                    Reportees = new List<HierarchyResponseViewModel>()
                }).ToList();

                // Fill Reporter Names (only for reporters that exist in the current filtered list)
                var empDict = allEmployees.ToDictionary(e => e.EmpId, e => e);
                foreach (var emp in allEmployees)
                {
                    if (emp.ReporterId.HasValue && empDict.ContainsKey(emp.ReporterId.Value))
                    {
                        emp.ReporterName = empDict[emp.ReporterId.Value].EmployeeName;
                    }
                    else if (emp.ReporterId.HasValue && !empDict.ContainsKey(emp.ReporterId.Value))
                    {
                        // Reporter exists but not in filtered data - set as "External/Unknown"
                        emp.ReporterName = "External Manager (Not in current view)";
                    }
                }

                // FIX 1: Get root employees (those with no reporter OR reporter not in current filtered list)
                var rootEmployees = allEmployees.Where(e => !e.ReporterId.HasValue
                                                            || e.ReporterId == 0
                                                            || e.ReporterId == e.EmpId
                                                            || !empDict.ContainsKey(e.ReporterId.Value))  // KEY FIX: Include orphans as roots
                                                .ToList();

                // Sort roots by GradeId (lower grade number = higher rank, show first)
                rootEmployees = rootEmployees.OrderBy(e => e.GradeId).ToList();

                // Build hierarchy
                var hierarchyResult = new List<HierarchyResponseViewModel>();
                var visitedIds = new HashSet<int>(); // FIX 2: Track visited to prevent circular references

                foreach (var root in rootEmployees)
                {
                    var node = BuildHierarchyNode(root, allEmployees, 1, visitedIds);
                    hierarchyResult.Add(node);
                }

                // Build summary
                var summary = BuildSummary(allEmployees, hierarchyResult);

                return new HierarchyFinalResponse
                {
                    Hierarchy = hierarchyResult,
                    Summary = summary,
                    GeneratedOn = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError, "Error building hierarchy: " + ex.Message);
            }
        }

        // Recursive method to build hierarchy tree with circular reference prevention
        private HierarchyResponseViewModel BuildHierarchyNode(HierarchyResponseViewModel currentEmployee,
                                                       List<HierarchyResponseViewModel> allEmployees,
                                                       int level,
                                                       HashSet<int> visitedIds)
        {
            // FIX 3: Prevent circular references
            if (visitedIds.Contains(currentEmployee.EmpId))
            {
                // Circular reference detected - stop recursion
                currentEmployee.HierarchyLevel = level;
                currentEmployee.ReporteesCount = 0;
                currentEmployee.Reportees = new List<HierarchyResponseViewModel>();
                currentEmployee.ReporterName = "CIRCULAR REFERENCE DETECTED";
                return currentEmployee;
            }

            // Add current employee to visited set
            visitedIds.Add(currentEmployee.EmpId);

            currentEmployee.HierarchyLevel = level;

            // Find direct reports (where ReporterId equals current employee's EmpId)
            // AND the reporter exists in the current filtered list
            var reportees = allEmployees.Where(e => e.ReporterId.HasValue
                                                    && e.ReporterId.Value == currentEmployee.EmpId
                                                    && e.EmpId != currentEmployee.EmpId)  // Don't report to self
                                        .ToList();

            // Sort by GradeId (lower grade number first - assuming lower = higher rank)
            reportees = reportees.OrderBy(e => e.GradeId).ToList();

            currentEmployee.Reportees = new List<HierarchyResponseViewModel>();
            currentEmployee.ReporteesCount = reportees.Count;

            foreach (var reportee in reportees)
            {
                // Pass a new HashSet to avoid modifying the parent's visited set
                var childNode = BuildHierarchyNode(reportee, allEmployees, level + 1, new HashSet<int>(visitedIds));
                currentEmployee.Reportees.Add(childNode);
            }

            return currentEmployee;
        }

        // Optional: Overload for backward compatibility
        private HierarchyResponseViewModel BuildHierarchyNode(HierarchyResponseViewModel currentEmployee,
                                                       List<HierarchyResponseViewModel> allEmployees,
                                                       int level)
        {
            return BuildHierarchyNode(currentEmployee, allEmployees, level, new HashSet<int>());
        }

        private HierarchySummary BuildSummary(List<HierarchyResponseViewModel> allEmployees,
                                                        List<HierarchyResponseViewModel> hierarchy)
        {
            var summary = new HierarchySummary();

            // Total Employees
            summary.TotalEmployees = allEmployees.Count;

            // Total Departments
            summary.TotalDepartments = allEmployees.Select(e => e.DeptId).Distinct().Count();

            // Total Designations
            summary.TotalDesignations = allEmployees.Select(e => e.DesignationId).Distinct().Count();

            // Total Grades
            summary.TotalGrades = allEmployees.Where(e => e.GradeId.HasValue)
                                              .Select(e => e.GradeId.Value)
                                              .Distinct()
                                              .Count();

            // Employees By Grade
            summary.EmployeesByGrade = allEmployees.Where(e => e.GradeName != null)
                                                   .GroupBy(e => e.GradeName)
                                                   .ToDictionary(g => g.Key, g => g.Count());

            // Employees By Department
            summary.EmployeesByDepartment = allEmployees.Where(e => e.DeptName != null)
                                                        .GroupBy(e => e.DeptName)
                                                        .ToDictionary(g => g.Key, g => g.Count());

            // Employees By Designation
            summary.EmployeesByDesignation = allEmployees.Where(e => e.DesignationName != null)
                                                         .GroupBy(e => e.DesignationName)
                                                         .ToDictionary(g => g.Key, g => g.Count());

            // Employees By Hierarchy Level (calculate from built hierarchy)
            var levelCounts = new Dictionary<string, int>();
            CountHierarchyLevels(hierarchy, levelCounts);
            summary.EmployeesByHierarchyLevel = levelCounts;

            return summary;
        }

        private void CountHierarchyLevels(List<HierarchyResponseViewModel> nodes, Dictionary<string, int> levelCounts)
        {
            foreach (var node in nodes)
            {
                string level = node.HierarchyLevel.ToString();
                if (levelCounts.ContainsKey(level))
                    levelCounts[level]++;
                else
                    levelCounts[level] = 1;

                if (node.Reportees != null && node.Reportees.Any())
                {
                    CountHierarchyLevels(node.Reportees, levelCounts);
                }
            }
        }

        ////// Build summary with counts
        ////private HierarchySummary BuildSummary(List<HierarchyResponseViewModel> allEmployees, List<HierarchyResponseViewModel> hierarchy)
        ////{
        ////    var summary = new HierarchySummary
        ////    {
        ////        EmployeesByGrade = new Dictionary<string, int>(),
        ////        EmployeesByDepartment = new Dictionary<string, int>(),
        ////        EmployeesByDesignation = new Dictionary<string, int>(),
        ////        EmployeesByHierarchyLevel = new Dictionary<string, int>()  // Changed to string key
        ////    };

        ////    summary.TotalEmployees = allEmployees.Count;
        ////    summary.TotalDepartments = allEmployees.Where(e => !string.IsNullOrEmpty(e.DeptName))
        ////                                           .Select(e => e.DeptName).Distinct().Count();
        ////    summary.TotalDesignations = allEmployees.Where(e => e.DesignationId.HasValue)
        ////                                            .Select(e => e.DesignationId).Distinct().Count();
        ////    summary.TotalGrades = allEmployees.Where(e => e.GradeId.HasValue)
        ////                                      .Select(e => e.GradeId).Distinct().Count();

        ////    // Employees by Grade
        ////    foreach (var group in allEmployees.Where(e => !string.IsNullOrEmpty(e.GradeName))
        ////                                      .GroupBy(e => e.GradeName)
        ////                                      .OrderBy(g => g.Min(x => x.GradeId)))
        ////    {
        ////        summary.EmployeesByGrade.Add(group.Key, group.Count());
        ////    }

        ////    // Employees by Department
        ////    foreach (var group in allEmployees.Where(e => !string.IsNullOrEmpty(e.DeptName))
        ////                                      .GroupBy(e => e.DeptName)
        ////                                      .OrderBy(g => g.Key))
        ////    {
        ////        summary.EmployeesByDepartment.Add(group.Key, group.Count());
        ////    }

        ////    // Employees by Designation
        ////    foreach (var group in allEmployees.Where(e => !string.IsNullOrEmpty(e.DesignationName))
        ////                                      .GroupBy(e => e.DesignationName)
        ////                                      .OrderBy(g => g.Key))
        ////    {
        ////        summary.EmployeesByDesignation.Add(group.Key, group.Count());
        ////    }

        ////    // Employees by Hierarchy Level - Convert int key to string
        ////    var flattenedList = new List<HierarchyResponseViewModel>();
        ////    FlattenHierarchy(hierarchy, flattenedList);

        ////    foreach (var group in flattenedList.GroupBy(e => e.HierarchyLevel).OrderBy(g => g.Key))
        ////    {
        ////        summary.EmployeesByHierarchyLevel.Add(group.Key.ToString(), group.Count());
        ////    }

        ////    return summary;
        ////}

        // Flatten hierarchy for summary calculations
        public void FlattenHierarchy(List<HierarchyResponseViewModel> nodes, List<HierarchyResponseViewModel> result)
        {
            foreach (var node in nodes)
            {
                result.Add(node);
                if (node.Reportees != null && node.Reportees.Any())
                {
                    FlattenHierarchy(node.Reportees, result);
                }
            }
        }
    }
}