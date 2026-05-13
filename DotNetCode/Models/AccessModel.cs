using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace OfficeConnect_Web.Models
{
    public class AccessModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        public List<DDCompViewModel> DDCompany(DDCompViewModel compdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (compdd.EmpId != 0) ? compdd.EmpId : 0;

                var CompLocdetails = (from Loc in DB.LocationMasters 
                                      join Comp in DB.CompanyMasters on Loc.CompId equals Comp.CompId
                                      where Comp.IsActive == true && Comp.IsDeleted == false && Loc.IsActive == true && Loc.IsDeleted == false
                                      select Loc).ToList();

                //var CompLEdetails = (from LE in DB.LegalEntityMasters
                //                     join Loc in DB.LocationMasters on LE.LEId equals Loc.LEId
                //                     where LE.IsActive == true && LE.IsDeleted == false && Loc.IsActive == true && Loc.IsDeleted == false
                //                     select LE).ToList();

                var CompLEdetails = (from LE in DB.LegalEntityMasters
                                     join Loc in DB.LocationMasters on LE.LEId equals Loc.LEId into locationGroup
                                     from Loc in locationGroup.DefaultIfEmpty() // This performs a left join
                                     where LE.IsActive == true
                                        && LE.IsDeleted == false
                                        && Loc == null // This ensures only LegalEntityMasters without a matching LocationMasters record
                                     select LE).Distinct().ToList();

                List<DDCompViewModel> lstofComp = new List<DDCompViewModel>();

                for (int i = 0; i < CompLocdetails.Count(); i++)
                {
                    DDCompViewModel ddc = new DDCompViewModel();
                    ddc.CompId = Convert.ToInt32(CompLocdetails[i].CompId);
                    ddc.LEId = 0;
                    ddc.LocationId = CompLocdetails[i].LocationId;
                    string company = (ddc.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == ddc.CompId).Select(x => x.Company).FirstOrDefault() : "";
                    string location = (ddc.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == ddc.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                    ddc.CompName = company + " - " + location;
                    lstofComp.Add(ddc);
                }

                for (int j = 0; j < CompLEdetails.Count(); j++)
                {
                    DDCompViewModel ddc = new DDCompViewModel();
                    ddc.CompId = Convert.ToInt32(CompLEdetails[j].CompId);
                    ddc.LocationId = 0;
                    ddc.LEId = Convert.ToInt32(CompLEdetails[j].LEId);
                    string company = (ddc.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == ddc.CompId).Select(x => x.Company).FirstOrDefault() : "";
                    string entity = (ddc.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == ddc.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                    ddc.CompName = company + " - " + entity;
                    lstofComp.Add(ddc);
                }

                if (EmpId != 0)
                {
                    if (lstofComp != null)
                    {
                        return lstofComp;
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
        public List<DDEmpViewModel> DDEmployee(DDEmpViewModel empdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (empdd.EmpId != 0) ? empdd.EmpId : 0;

                var Empdetails = (from Emp in DB.EmployeeMasters
                                   where Emp.IsActive == true && Emp.IsDeleted == false
                                   select new DDEmpViewModel
                                   {
                                       EmpId = Emp.EmpId,
                                       EmpName = Emp.FirstName + " " + Emp.MiddleName + " " + Emp.LastName,
                                       EmpCode = Emp.UserName,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (Empdetails != null)
                    {
                        return Empdetails;
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
        public List<DDDeptEmpViewModel> DDDeptEmployee(DDDeptEmpViewModel empdd)
        {
            try
            {
                string msg = "";
                int? loginId = empdd.LoginId != 0 ? empdd.LoginId : (int?)null;
                int? deptId = empdd.DeptId != 0 ? empdd.DeptId : (int?)null;
                int? designationId = empdd.DesignationId != 0 ? empdd.DesignationId : (int?)null;

                var Empdetails = (from Emp in DB.EmployeeMasters
                                  where Emp.IsActive == true && Emp.IsDeleted == false
                                  && (!deptId.HasValue || Emp.CategoryId == deptId)
                                  && (!designationId.HasValue || Emp.DesignationId == designationId)
                                  select new DDDeptEmpViewModel
                                  {
                                      EmpId = Emp.EmpId,
                                      EmpName = Emp.FirstName + " " + Emp.MiddleName + " " + Emp.LastName,
                                      EmpCode = Emp.UserName,
                                  }).ToList();

                if (loginId != 0)
                {
                    if (Empdetails != null)
                    {
                        return Empdetails;
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
        public List<DDDeptViewModel> GetDDDept(DDDeptViewModel deptdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (deptdd.EmpId != 0) ? deptdd.EmpId : 0;

                var deptdetails = (from dept in DB.DeptMasters
                                   where dept.IsActive == true && dept.IsDeleted == false
                                   select new DDDeptViewModel
                                   {
                                       DeptId = dept.DeptId,
                                       DeptName = dept.DeptName,
                                       DeptShortName = dept.DeptShortName,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (deptdetails != null)
                    {
                        return deptdetails;
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
        public List<DDRoleViewModel> GetDDRole(DDRoleViewModel roledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (roledd.EmpId != 0) ? roledd.EmpId : 0;
                int? deptid = (roledd.DeptId != 0) ? roledd.DeptId : 0;

                var Roledetails = (from Role in DB.DesignationMasters
                                   where Role.IsActive == true && Role.IsDeleted == false
                                   select new DDRoleViewModel
                                   {
                                       DeptId = Role.DeptId,
                                       RoleId = Role.DesignationId,
                                       RoleName = Role.Designation,
                                   }).ToList();

                if (deptid != 0)
                {
                    Roledetails = (from Role in DB.DesignationMasters
                                       where Role.DeptId == deptid && Role.IsActive == true && Role.IsDeleted == false
                                       select new DDRoleViewModel
                                       {
                                           DeptId = Role.DeptId,
                                           RoleId = Role.DesignationId,
                                           RoleName = Role.Designation,
                                       }).ToList();
                }

                if (EmpId != 0)
                {
                    if (Roledetails != null)
                    {
                        return Roledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Not Found");
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
        public List<DDDesignationViewModel> GetDDDesignation(DDDesignationViewModel roledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (roledd.EmpId != 0) ? roledd.EmpId : 0;
                int? deptid = (roledd.DeptId != 0) ? roledd.DeptId : 0;

                var Roledetails = (from Role in DB.DesignationMasters
                                   where Role.IsActive == true && Role.IsDeleted == false
                                   select new DDDesignationViewModel
                                   {
                                       DeptId = Role.DeptId,
                                       DesignationId = Role.DesignationId,
                                       Designation = Role.Designation,
                                   }).ToList();

                if (deptid != 0)
                {
                    Roledetails = (from Role in DB.DesignationMasters
                                   where Role.DeptId == deptid && Role.IsActive == true && Role.IsDeleted == false
                                   select new DDDesignationViewModel
                                   {
                                       DeptId = Role.DeptId,
                                       DesignationId = Role.DesignationId,
                                       Designation = Role.Designation,
                                   }).ToList();
                }

                if (EmpId != 0)
                {
                    if (Roledetails != null)
                    {
                        return Roledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Not Found");
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
        public List<DDGradeViewModel> GetDDGrade(DDGradeViewModel gradedd)
        {
            try
            {
                string msg = "";
                int? EmpId = (gradedd.EmpId != 0) ? gradedd.EmpId : 0;
                int? gradeid = (gradedd.GradeId != 0) ? gradedd.GradeId : 0;

                var Gradedetails = (from grad in DB.GradeMasters
                                   where grad.IsActive == true 
                                   select new DDGradeViewModel
                                   {
                                       GradeId = grad.GradeId,
                                       Grade = grad.Grade,
                                   }).ToList();

                if (gradeid != 0)
                {
                    Gradedetails = (from grad in DB.GradeMasters
                                   where grad.GradeId == gradeid && grad.IsActive == true
                                   select new DDGradeViewModel
                                   {
                                       GradeId = grad.GradeId,
                                       Grade = grad.Grade,
                                   }).ToList();
                }

                if (EmpId != 0)
                {
                    if (Gradedetails != null)
                    {
                        return Gradedetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Grade Details Not Found");
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
        public List<DDModuleViewModel> GetDDModule(DDModuleViewModel moddd)
        {
            try
            {
                string msg = "";
                int? EmpId = (moddd.EmpId != 0) ? moddd.EmpId : 0;

                var Moduledetails = (from mod in DB.ModuleMasters
                                     where mod.IsActive == true && mod.IsDeleted == false
                                     select new DDModuleViewModel
                                     {
                                         ModuleId = mod.ModuleId,
                                         ModuleName = mod.ModuleName,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Moduledetails != null)
                    {
                        return Moduledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Module Details Not Found");
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
        public List<DDSubModuleViewModel> GetDDSubModule(DDSubModuleViewModel submoddd)
        {
            try
            {
                string msg = "";
                int? EmpId = (submoddd.EmpId != 0) ? submoddd.EmpId : 0;
                int? moduleid = (submoddd.ModuleId != 0) ? submoddd.ModuleId : 0;

                var SubModuledetails = (from submod in DB.SubModuleMasters
                                        where submod.IsActive == true && submod.IsDeleted == false
                                        select new DDSubModuleViewModel
                                        {
                                            ModuleId = submod.ModuleId,
                                            SubModuleId = submod.SubModuleId,
                                            SubModuleName = submod.SubModuleName,
                                        }).ToList();

                if (moduleid != 0)
                {
                    SubModuledetails = (from submod in DB.SubModuleMasters
                                        where submod.ModuleId == moduleid && submod.IsActive == true && submod.IsDeleted == false
                                        select new DDSubModuleViewModel
                                        {
                                            ModuleId = submod.ModuleId,
                                            SubModuleId = submod.SubModuleId,
                                            SubModuleName = submod.SubModuleName,
                                        }).ToList();
                }

                if (EmpId != 0)
                {
                    if (SubModuledetails != null)
                    {
                        return SubModuledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SubModule Details Not Found");
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
        public List<DDPageModuleViewModel> GetDDPageModule(DDPageModuleViewModel pagedd)
        {
            try
            {
                string msg = "";
                int? EmpId = (pagedd.EmpId != 0) ? pagedd.EmpId : 0;
                int? moduleid = (pagedd.ModuleId != 0) ? pagedd.ModuleId : 0;
                int? submoduleid = (pagedd.SubModuleId != 0) ? pagedd.SubModuleId : 0;

                var PageModuledetails = (from pmm in DB.PageModuleMasters
                                         where pmm.IsActive == true && pmm.IsDeleted == false
                                         select new DDPageModuleViewModel
                                         {
                                             ModuleId = pmm.ModuleId,
                                             SubModuleId = pmm.SubModuleId,
                                             PageModuleId = pmm.PageModuleId,
                                             PageName = pmm.PageName,
                                         }).ToList();

                if (moduleid != 0 && submoduleid != 0)
                {
                    PageModuledetails = (from pmm in DB.PageModuleMasters
                                         where pmm.ModuleId == moduleid && pmm.SubModuleId == submoduleid && pmm.IsActive == true && pmm.IsDeleted == false
                                         select new DDPageModuleViewModel
                                         {
                                             ModuleId = pmm.ModuleId,
                                             SubModuleId = pmm.SubModuleId,
                                             PageModuleId = pmm.PageModuleId,
                                             PageName = pmm.PageName,
                                         }).ToList();
                }

                if (EmpId != 0)
                {
                    if (PageModuledetails != null)
                    {
                        return PageModuledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Page Module Details Not Found");
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
        public List<DDAccessViewModel> GetDDAccess(DDAccessViewModel accessdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (accessdd.EmpId != 0) ? accessdd.EmpId : 0;

                var Accessdetails = (from ap in DB.AccessPolicies
                                     where ap.IsActive == true && ap.IsDeleted == false
                                     select new DDAccessViewModel
                                     {
                                         AccessId = ap.AccessId,
                                         AccessName = ap.AccessName,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Accessdetails != null)
                    {
                        return Accessdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Policies Details Not Found");
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
        public List<DDAccessPolicyViewModel> GetAccesspolicy(DDAccessPolicyViewModel accessdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (accessdd.EmpId != 0) ? accessdd.EmpId : 0;

                var Empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == EmpId
                                  select emp).FirstOrDefault();

                var Accessdetails = (from ap in DB.AccessPolicies
                                     join d in DB.DeptMasters on ap.DeptId equals d.DeptId
                                     join r in DB.DesignationMasters on ap.RoleId equals r.DesignationId
                                     join m in DB.ModuleMasters on ap.ModuleId equals m.ModuleId
                                     join sm in DB.SubModuleMasters on ap.SubModuleId equals sm.SubModuleId
                                     join p in DB.PageModuleMasters on ap.PageModuleId equals p.PageModuleId
                                     where ap.IsActive == true && ap.IsDeleted == false && ap.DeptId == Empdetails.CategoryId && ap.RoleId == Empdetails.DesignationId
                                     select new DDAccessPolicyViewModel
                                     {
                                         AccessId = ap.AccessId,
                                         AccessName = ap.AccessName,
                                         DeptId = ap.DeptId,
                                         DeptName = d.DeptName,
                                         DeptShortName = d.DeptShortName,
                                         RoleId = ap.RoleId,
                                         RoleName = r.Designation,
                                         ModuleId = ap.ModuleId,
                                         ModuleName = m.ModuleName,
                                         SubModuleId = ap.SubModuleId,
                                         SubModuleName = sm.SubModuleName,
                                         PageModuleId = ap.PageModuleId,
                                         PageName = p.PageName,
                                         AddAccess = ap.AddAccess,
                                         UpdateAccess = ap.UpdateAccess,
                                         DeleteAccess = ap.DeleteAccess,
                                         ViewAccess = ap.ViewAccess,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Accessdetails != null)
                    {
                        return Accessdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Policies Details Not Found");
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
        public List<DeptViewModel> GetAllDept(DeptViewModel deptdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (deptdd.EmpId != 0) ? deptdd.EmpId : 0;

                var deptdetails = (from dept in DB.DeptMasters
                                   where dept.IsActive == true && dept.IsDeleted == false
                                   select new DeptViewModel
                                   {
                                       DeptId = dept.DeptId,
                                       DeptName = dept.DeptName,
                                       DeptShortName = dept.DeptShortName,
                                       IsActive = dept.IsActive,
                                       IsUpdated = dept.IsUpdated,
                                       IsDeleted = dept.IsDeleted,
                                       CreatedBy = dept.CreatedBy,
                                       CreatedDate = dept.CreatedDate,
                                       LastUpdatedBy = dept.LastUpdatedBy,
                                       LastUpdatedDate = dept.LastUpdatedDate,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (deptdetails != null)
                    {
                        return deptdetails;
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
        public DeptViewModel GetDept(DeptViewModel deptdd)
        {
            try
            {
                int id = (deptdd.DeptId != 0) ? deptdd.DeptId : 0;
                string deptname = deptdd.DeptName;
                string msg = "";
                int? EmpId = (deptdd.EmpId != 0) ? deptdd.EmpId : 0;

                var deptdetails = (from dept in DB.DeptMasters
                                   where dept.DeptId == id && dept.IsActive == true && dept.IsDeleted == false
                                   select new DeptViewModel
                                   {
                                       DeptId = dept.DeptId,
                                       DeptName = dept.DeptName,
                                       DeptShortName = dept.DeptShortName,
                                       IsActive = dept.IsActive,
                                       IsUpdated = dept.IsUpdated,
                                       IsDeleted = dept.IsDeleted,
                                       CreatedBy = dept.CreatedBy,
                                       CreatedDate = dept.CreatedDate,
                                       LastUpdatedBy = dept.LastUpdatedBy,
                                       LastUpdatedDate = dept.LastUpdatedDate,
                                   }).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (deptdetails != null)
                    {
                        return deptdetails;
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
        public DeptViewModel AddDept(DeptViewModel deptdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (deptdd.EmpId != 0) ? deptdd.EmpId : 0;
                string deptname = (deptdd.DeptName != "" || deptdd.DeptName != null ) ? deptdd.DeptName : "";

                var deptdetails = (from dept in DB.DeptMasters
                                   where dept.DeptName == deptname &&  dept.IsActive == true && dept.IsDeleted == false
                                   select new DDDeptViewModel
                                   {
                                       DeptId = dept.DeptId,
                                       DeptName = dept.DeptName,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (deptdetails.Count() == 0)
                    {
                        DeptMaster dm = new DeptMaster();
                        dm.DeptName = deptdd.DeptName;
                        dm.DeptShortName = deptdd.DeptShortName;
                        dm.IsActive = true;
                        dm.IsUpdated = false;
                        dm.IsDeleted = false;
                        dm.CreatedBy = EmpId;
                        dm.CreatedDate = DateTime.Now;
                        dm.LastUpdatedBy = EmpId;
                        dm.LastUpdatedDate = DateTime.Now;
                        DB.DeptMasters.Add(dm);
                        DB.SaveChanges();

                        DeptViewModel dvm = new DeptViewModel();
                        dvm.msg = "Added";
                        dvm.DeptName = deptdd.DeptName;

                        return dvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Department Details Already Exists");
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
        public DeptViewModel UpdateDept(DeptViewModel deptdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (deptdd.EmpId != 0) ? deptdd.EmpId : 0;
                int id = (deptdd.DeptId != 0) ? deptdd.DeptId : 0;
                string deptname = (deptdd.DeptName != "" || deptdd.DeptName != null) ? deptdd.DeptName : "";

                var deptdetails = (from dept in DB.DeptMasters
                                   where dept.DeptId == id && dept.IsActive == true && dept.IsDeleted == false
                                   select dept).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (deptdetails != null)
                    {
                        deptdetails.DeptName = deptdd.DeptName;
                        deptdetails.DeptShortName = deptdd.DeptShortName;
                        deptdetails.IsActive = true;
                        deptdetails.IsUpdated = true;
                        deptdetails.IsDeleted = false;
                        deptdetails.LastUpdatedBy = EmpId;
                        deptdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        DeptViewModel dvm = new DeptViewModel();
                        dvm.msg = "Updated";
                        dvm.DeptName = deptdd.DeptName;

                        return dvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Department Details Not Found");
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
        public DeptViewModel DeleteDept(DeptViewModel deptdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (deptdd.EmpId != 0) ? deptdd.EmpId : 0;
                int id = (deptdd.DeptId != 0) ? deptdd.DeptId : 0;
                string deptname = (deptdd.DeptName != "" || deptdd.DeptName != null) ? deptdd.DeptName : "";

                var deptdetails = (from dept in DB.DeptMasters
                                   where dept.DeptId == id && dept.IsActive == true && dept.IsDeleted == false
                                   select dept).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (deptdetails != null)
                    {
                        deptdetails.IsDeleted = true;
                        deptdetails.LastUpdatedBy = EmpId;
                        deptdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        DeptViewModel dvm = new DeptViewModel();
                        dvm.msg = "Deleted";
                        dvm.DeptName = deptdd.DeptName;

                        return dvm;
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
        public List<RoleViewModel> GetAllRole(RoleViewModel Roledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Roledd.EmpId != 0) ? Roledd.EmpId : 0;

                var Roledetails = (from Role in DB.DesignationMasters
                                   join Dept in DB.DeptMasters on Role.DeptId equals Dept.DeptId
                                   where Role.IsActive == true && Role.IsDeleted == false && Dept.IsActive == true && Dept.IsDeleted == false
                                   select new RoleViewModel
                                   {
                                       RoleId = Role.DesignationId,
                                       DeptId = Role.DeptId,
                                       DeptName = Dept.DeptName,
                                       RoleName = Role.Designation,
                                       GradeId = Role.GradeId,
                                       Grade = Role.Grade,
                                       IsActive = Role.IsActive,
                                       IsUpdated = Role.IsUpdated,
                                       IsDeleted = Role.IsDeleted,
                                       CreatedBy = Role.CreatedBy,
                                       CreatedDate = Role.CreatedDate,
                                       LastUpdatedBy = Role.LastUpdatedBy,
                                       LastUpdatedDate = Role.LastUpdatedDate,
                                   }).ToList();

                if (EmpId != 0)
                {
                    if (Roledetails.Count() != 0)
                    {
                        return Roledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Not Found");
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
        public RoleViewModel GetRole(RoleViewModel Roledd)
        {
            try
            {
                int id = (Roledd.RoleId != 0) ? Roledd.RoleId : 0;
                string deptname = Roledd.DeptName;
                string msg = "";
                int? EmpId = (Roledd.EmpId != 0) ? Roledd.EmpId : 0;

                var Roledetails = (from Role in DB.DesignationMasters
                                   join Dept in DB.DeptMasters on Role.DeptId equals Dept.DeptId
                                   where Role.DesignationId == id && Role.IsActive == true && Role.IsDeleted == false && Dept.IsActive == true && Dept.IsDeleted == false
                                   select new RoleViewModel
                                   {
                                       RoleId = Role.DesignationId,
                                       DeptId = Role.DeptId,
                                       DeptName = Dept.DeptName,
                                       RoleName = Role.Designation,
                                       GradeId = Role.GradeId,
                                       Grade = Role.Grade,
                                       IsActive = Role.IsActive,
                                       IsUpdated = Role.IsUpdated,
                                       IsDeleted = Role.IsDeleted,
                                       CreatedBy = Role.CreatedBy,
                                       CreatedDate = Role.CreatedDate,
                                       LastUpdatedBy = Role.LastUpdatedBy,
                                       LastUpdatedDate = Role.LastUpdatedDate,
                                   }).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Roledetails != null)
                    {
                        return Roledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Not Found");
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
        public RoleViewModel AddRole(RoleViewModel Roledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Roledd.EmpId != 0) ? Roledd.EmpId : 0;
                string rolename = (Roledd.RoleName != "" || Roledd.RoleName != null) ? Roledd.RoleName : "";

                var Roledetails = (from Role in DB.DesignationMasters
                                   where Role.DeptId == Roledd.DeptId && Role.Designation == rolename && Role.IsActive == true && Role.IsDeleted == false
                                   select Role).ToList();

                if (EmpId != 0)
                {
                    if (Roledetails.Count() == 0)
                    {
                        DesignationMaster rm = new DesignationMaster();
                        rm.DeptId = Roledd.DeptId;
                        rm.Designation = Roledd.RoleName;
                        rm.GradeId = Roledd.GradeId;
                        rm.Grade = Roledd.Grade;
                        rm.IsActive = true;
                        rm.IsUpdated = false;
                        rm.IsDeleted = false;
                        rm.CreatedBy = EmpId;
                        rm.CreatedDate = DateTime.Now;
                        rm.LastUpdatedBy = EmpId;
                        rm.LastUpdatedDate = DateTime.Now;
                        DB.DesignationMasters.Add(rm);
                        DB.SaveChanges();

                        RoleViewModel rvm = new RoleViewModel();
                        rvm.msg = "Added";
                        rvm.RoleName = Roledd.RoleName;

                        return rvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Already Exists");
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
        public RoleViewModel UpdateRole(RoleViewModel Roledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Roledd.EmpId != 0) ? Roledd.EmpId : 0;
                int id = (Roledd.RoleId != 0) ? Roledd.RoleId : 0;
                string rolename = (Roledd.RoleName != "" || Roledd.RoleName != null) ? Roledd.RoleName : "";

                var Roledetails = (from Role in DB.DesignationMasters
                                   where Role.DesignationId == id && Role.IsActive == true && Role.IsDeleted == false
                                   select Role).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Roledetails != null)
                    {
                        Roledetails.Designation = Roledd.RoleName;
                        Roledetails.DeptId = Roledd.DeptId;
                        Roledetails.GradeId = Roledd.GradeId;
                        Roledetails.Grade = Roledd.Grade;
                        Roledetails.IsActive = true;
                        Roledetails.IsUpdated = true;
                        Roledetails.IsDeleted = false;
                        Roledetails.LastUpdatedBy = EmpId;
                        Roledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        RoleViewModel rvm = new RoleViewModel();
                        rvm.msg = "Updated";
                        rvm.RoleName = Roledd.RoleName;

                        return rvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Not Found");
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
        public RoleViewModel DeleteRole(RoleViewModel Roledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Roledd.EmpId != 0) ? Roledd.EmpId : 0;
                int id = (Roledd.RoleId != 0) ? Roledd.RoleId : 0;
                string rolename = (Roledd.RoleName != "" || Roledd.RoleName != null) ? Roledd.RoleName : "";

                var Roledetails = (from Role in DB.DesignationMasters
                                   where Role.DesignationId == id && Role.IsActive == true && Role.IsDeleted == false
                                   select Role).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Roledetails != null)
                    {
                        Roledetails.IsDeleted = true;
                        Roledetails.LastUpdatedBy = EmpId;
                        Roledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        RoleViewModel rvm = new RoleViewModel();
                        rvm.msg = "Deleted";
                        rvm.RoleName = Roledd.RoleName;

                        return rvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Role Details Not Found");
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
        public List<ModuleViewModel> GetAllModule(ModuleViewModel Moduledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Moduledd.EmpId != 0) ? Moduledd.EmpId : 0;

                var Moduledetails = (from Module in DB.ModuleMasters
                                     where Module.IsActive == true && Module.IsDeleted == false
                                     select new ModuleViewModel
                                     {
                                         ModuleId = Module.ModuleId,
                                         ModuleName = Module.ModuleName,
                                         IsActive = Module.IsActive,
                                         IsUpdated = Module.IsUpdated,
                                         IsDeleted = Module.IsDeleted,
                                         CreatedBy = Module.CreatedBy,
                                         CreatedDate = Module.CreatedDate,
                                         LastUpdatedBy = Module.LastUpdatedBy,
                                         LastUpdatedDate = Module.LastUpdatedDate,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Moduledetails.Count() != 0)
                    {
                        return Moduledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Module Details Not Found");
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
        public ModuleViewModel GetModule(ModuleViewModel Moduledd)
        {
            try
            {
                int id = (Moduledd.ModuleId != 0) ? Moduledd.ModuleId : 0;
                string modname = Moduledd.ModuleName;
                string msg = "";
                int? EmpId = (Moduledd.EmpId != 0) ? Moduledd.EmpId : 0;

                var Moduledetails = (from Module in DB.ModuleMasters
                                     where Module.ModuleId == id && Module.IsActive == true && Module.IsDeleted == false
                                     select new ModuleViewModel
                                     {
                                         ModuleId = Module.ModuleId,
                                         ModuleName = Module.ModuleName,
                                         IsActive = Module.IsActive,
                                         IsUpdated = Module.IsUpdated,
                                         IsDeleted = Module.IsDeleted,
                                         CreatedBy = Module.CreatedBy,
                                         CreatedDate = Module.CreatedDate,
                                         LastUpdatedBy = Module.LastUpdatedBy,
                                         LastUpdatedDate = Module.LastUpdatedDate,
                                     }).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Moduledetails != null)
                    {
                        return Moduledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Module Details Not Found");
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
        public ModuleViewModel AddModule(ModuleViewModel Moduledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Moduledd.EmpId != 0) ? Moduledd.EmpId : 0;
                string modname = (Moduledd.ModuleName != "" || Moduledd.ModuleName != null) ? Moduledd.ModuleName : "";

                var Moduledetails = (from Module in DB.ModuleMasters
                                     where Module.ModuleName == modname && Module.IsActive == true && Module.IsDeleted == false
                                     select new DDModuleViewModel
                                     {
                                         ModuleId = Module.ModuleId,
                                         ModuleName = Module.ModuleName,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Moduledetails.Count() == 0)
                    {
                        ModuleMaster mm = new ModuleMaster();
                        mm.ModuleName = Moduledd.ModuleName;
                        mm.IsActive = true;
                        mm.IsUpdated = false;
                        mm.IsDeleted = false;
                        mm.CreatedBy = 1;
                        mm.CreatedDate = DateTime.Now;
                        mm.LastUpdatedBy = EmpId;
                        mm.LastUpdatedDate = DateTime.Now;
                        DB.ModuleMasters.Add(mm);
                        DB.SaveChanges();

                        ModuleViewModel mvm = new ModuleViewModel();
                        mvm.msg = "Added";
                        mvm.ModuleName = Moduledd.ModuleName;

                        return mvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Module Details Already Exists");
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
        public ModuleViewModel UpdateModule(ModuleViewModel Moduledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Moduledd.EmpId != 0) ? Moduledd.EmpId : 0;
                int id = (Moduledd.ModuleId != 0) ? Moduledd.ModuleId : 0;
                string modname = (Moduledd.ModuleName != "" || Moduledd.ModuleName != null) ? Moduledd.ModuleName : "";

                var Moduledetails = (from Module in DB.ModuleMasters
                                     where Module.ModuleId == id && Module.IsActive == true && Module.IsDeleted == false
                                     select Module).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Moduledetails != null)
                    {
                        Moduledetails.ModuleName = Moduledd.ModuleName;
                        Moduledetails.IsActive = true;
                        Moduledetails.IsUpdated = true;
                        Moduledetails.IsDeleted = false;
                        Moduledetails.LastUpdatedBy = EmpId;
                        Moduledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        ModuleViewModel mvm = new ModuleViewModel();
                        mvm.msg = "Updated";
                        mvm.ModuleName = Moduledd.ModuleName;

                        return mvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Module Details Not Found");
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
        public ModuleViewModel DeleteModule(ModuleViewModel Moduledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Moduledd.EmpId != 0) ? Moduledd.EmpId : 0;
                int id = (Moduledd.ModuleId != 0) ? Moduledd.ModuleId : 0;
                string modname = (Moduledd.ModuleName != "" || Moduledd.ModuleName != null) ? Moduledd.ModuleName : "";

                var Moduledetails = (from Module in DB.ModuleMasters
                                     where Module.ModuleId == id && Module.IsActive == true && Module.IsDeleted == false
                                     select Module).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Moduledetails != null)
                    {
                        Moduledetails.IsDeleted = true;
                        Moduledetails.LastUpdatedBy = EmpId;
                        Moduledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        ModuleViewModel mvm = new ModuleViewModel();
                        mvm.msg = "Deleted";
                        mvm.ModuleName = Moduledd.ModuleName;

                        return mvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Module Details Not Found");
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
        public List<SubModuleViewModel> GetAllSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (SubModuledd.EmpId != 0) ? SubModuledd.EmpId : 0;

                var SubModuledetails = (from SubModule in DB.SubModuleMasters
                                        join Mod in DB.ModuleMasters on SubModule.ModuleId equals Mod.ModuleId
                                        where SubModule.IsActive == true && SubModule.IsDeleted == false && Mod.IsActive == true && Mod.IsDeleted == false
                                        select new SubModuleViewModel
                                        {
                                            SubModuleId = SubModule.SubModuleId,
                                            ModuleId = SubModule.ModuleId,
                                            ModuleName = Mod.ModuleName,
                                            SubModuleName = SubModule.SubModuleName,
                                            IsActive = SubModule.IsActive,
                                            IsUpdated = SubModule.IsUpdated,
                                            IsDeleted = SubModule.IsDeleted,
                                            CreatedBy = SubModule.CreatedBy,
                                            CreatedDate = SubModule.CreatedDate,
                                            LastUpdatedBy = SubModule.LastUpdatedBy,
                                            LastUpdatedDate = SubModule.LastUpdatedDate,
                                        }).ToList();

                if (EmpId != 0)
                {
                    if (SubModuledetails.Count() != 0)
                    {
                        return SubModuledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SubModule Details Not Found");
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
        public SubModuleViewModel GetSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                int id = (SubModuledd.SubModuleId != 0) ? SubModuledd.SubModuleId : 0;
                string Submodname = SubModuledd.SubModuleName;
                string msg = "";
                int? EmpId = (SubModuledd.EmpId != 0) ? SubModuledd.EmpId : 0;

                var SubModuledetails = (from SubModule in DB.SubModuleMasters
                                        join Mod in DB.ModuleMasters on SubModule.ModuleId equals Mod.ModuleId
                                        where SubModule.SubModuleId == id && SubModule.IsActive == true && SubModule.IsDeleted == false && Mod.IsActive == true && Mod.IsDeleted == false
                                        select new SubModuleViewModel
                                        {
                                            SubModuleId = SubModule.SubModuleId,
                                            ModuleId = SubModule.ModuleId,
                                            ModuleName = Mod.ModuleName,
                                            SubModuleName = SubModule.SubModuleName,
                                            IsActive = SubModule.IsActive,
                                            IsUpdated = SubModule.IsUpdated,
                                            IsDeleted = SubModule.IsDeleted,
                                            CreatedBy = SubModule.CreatedBy,
                                            CreatedDate = SubModule.CreatedDate,
                                            LastUpdatedBy = SubModule.LastUpdatedBy,
                                            LastUpdatedDate = SubModule.LastUpdatedDate,
                                        }).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (SubModuledetails != null)
                    {
                        return SubModuledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SubModule Details Not Found");
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
        public SubModuleViewModel AddSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (SubModuledd.EmpId != 0) ? SubModuledd.EmpId : 0;
                string submodname = (SubModuledd.SubModuleName != "" || SubModuledd.SubModuleName != null) ? SubModuledd.SubModuleName : "";

                var SubModuledetails = (from SubModule in DB.SubModuleMasters
                                        where SubModule.SubModuleName == submodname && SubModule.IsActive == true && SubModule.IsDeleted == false
                                        select SubModule).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (SubModuledetails == null)
                    {
                        SubModuleMaster smm = new SubModuleMaster();
                        smm.ModuleId = SubModuledd.ModuleId;
                        smm.SubModuleName = SubModuledd.SubModuleName;
                        smm.IsActive = true;
                        smm.IsUpdated = false;
                        smm.IsDeleted = false;
                        smm.CreatedBy = EmpId;
                        smm.CreatedDate = DateTime.Now;
                        smm.LastUpdatedBy = EmpId;
                        smm.LastUpdatedDate = DateTime.Now;
                        DB.SubModuleMasters.Add(smm);
                        DB.SaveChanges();

                        SubModuleViewModel smvm = new SubModuleViewModel();
                        smvm.msg = "Added";
                        smvm.SubModuleName = SubModuledd.SubModuleName;

                        return smvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SubModule Details Already Exists");
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
        public SubModuleViewModel UpdateSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (SubModuledd.EmpId != 0) ? SubModuledd.EmpId : 0;
                int id = (SubModuledd.SubModuleId != 0) ? SubModuledd.SubModuleId : 0;
                string submodname = (SubModuledd.SubModuleName != "" || SubModuledd.SubModuleName != null) ? SubModuledd.SubModuleName : "";

                var SubModuledetails = (from SubModule in DB.SubModuleMasters
                                        where SubModule.SubModuleId == id && SubModule.IsActive == true && SubModule.IsDeleted == false
                                        select SubModule).FirstOrDefault();


                if (EmpId != 0)
                {
                    if (SubModuledetails != null)
                    {
                        SubModuledetails.SubModuleName = SubModuledd.SubModuleName;
                        SubModuledetails.ModuleId = SubModuledd.ModuleId;
                        SubModuledetails.IsActive = true;
                        SubModuledetails.IsUpdated = true;
                        SubModuledetails.IsDeleted = false;
                        SubModuledetails.LastUpdatedBy = EmpId;
                        SubModuledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        SubModuleViewModel smvm = new SubModuleViewModel();
                        smvm.msg = "Updated";
                        smvm.SubModuleName = SubModuledd.SubModuleName;

                        return smvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SubModule Details Not Found");
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
        public SubModuleViewModel DeleteSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (SubModuledd.EmpId != 0) ? SubModuledd.EmpId : 0;
                int id = (SubModuledd.SubModuleId != 0) ? SubModuledd.SubModuleId : 0;
                string submodname = (SubModuledd.SubModuleName != "" || SubModuledd.SubModuleName != null) ? SubModuledd.SubModuleName : "";

                var SubModuledetails = (from SubModule in DB.SubModuleMasters
                                        where SubModule.SubModuleId == id && SubModule.IsActive == true && SubModule.IsDeleted == false
                                        select SubModule).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (SubModuledetails != null)
                    {
                        SubModuledetails.IsDeleted = true;
                        SubModuledetails.LastUpdatedBy = EmpId;
                        SubModuledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        SubModuleViewModel smvm = new SubModuleViewModel();
                        smvm.msg = "Deleted";
                        smvm.SubModuleName = SubModuledd.SubModuleName;

                        return smvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "SubModule Details Not Found");
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
        public List<PageModuleViewModel> GetAllPageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (PageModuledd.EmpId != 0) ? PageModuledd.EmpId : 0;

                var PageModuledetails = (from PageModule in DB.PageModuleMasters
                                         join Mod in DB.ModuleMasters on PageModule.ModuleId equals Mod.ModuleId
                                         join SMod in DB.SubModuleMasters on PageModule.SubModuleId equals SMod.SubModuleId
                                         where PageModule.IsActive == true && PageModule.IsDeleted == false && Mod.IsActive == true && Mod.IsDeleted == false
                                          && SMod.IsActive == true && SMod.IsDeleted == false
                                         select new PageModuleViewModel
                                         {
                                             PageModuleId = PageModule.PageModuleId,
                                             ModuleId = PageModule.ModuleId,
                                             ModuleName = Mod.ModuleName,
                                             SubModuleId = PageModule.SubModuleId,
                                             SubModuleName = SMod.SubModuleName,
                                             PageModuleName = PageModule.PageName,
                                             IsActive = PageModule.IsActive,
                                             IsUpdated = PageModule.IsUpdated,
                                             IsDeleted = PageModule.IsDeleted,
                                             CreatedBy = PageModule.CreatedBy,
                                             CreatedDate = PageModule.CreatedDate,
                                             LastUpdatedBy = PageModule.LastUpdatedBy,
                                             LastUpdatedDate = PageModule.LastUpdatedDate,
                                         }).ToList();

                if (EmpId != 0)
                {
                    if (PageModuledetails.Count() != 0)
                    {
                        return PageModuledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Page Module Details Not Found");
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
        public PageModuleViewModel GetPageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                int id = (PageModuledd.PageModuleId != 0) ? PageModuledd.PageModuleId : 0;
                string pagename = PageModuledd.PageModuleName;
                string msg = "";
                int? EmpId = (PageModuledd.EmpId != 0) ? PageModuledd.EmpId : 0;

                var PageModuledetails = (from PageModule in DB.PageModuleMasters
                                         join Mod in DB.ModuleMasters on PageModule.ModuleId equals Mod.ModuleId
                                         join SMod in DB.SubModuleMasters on PageModule.SubModuleId equals SMod.SubModuleId
                                         where PageModule.PageModuleId == id && PageModule.IsActive == true && PageModule.IsDeleted == false && Mod.IsActive == true && Mod.IsDeleted == false
                                          && SMod.IsActive == true && SMod.IsDeleted == false
                                         select new PageModuleViewModel
                                         {
                                             PageModuleId = PageModule.PageModuleId,
                                             ModuleId = PageModule.ModuleId,
                                             ModuleName = Mod.ModuleName,
                                             SubModuleId = PageModule.SubModuleId,
                                             SubModuleName = SMod.SubModuleName,
                                             PageModuleName = PageModule.PageName,
                                             IsActive = PageModule.IsActive,
                                             IsUpdated = PageModule.IsUpdated,
                                             IsDeleted = PageModule.IsDeleted,
                                             CreatedBy = PageModule.CreatedBy,
                                             CreatedDate = PageModule.CreatedDate,
                                             LastUpdatedBy = PageModule.LastUpdatedBy,
                                             LastUpdatedDate = PageModule.LastUpdatedDate,
                                         }).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (PageModuledetails != null)
                    {
                        return PageModuledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Page Module Details Not Found");
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
        public PageModuleViewModel AddPageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (PageModuledd.EmpId != 0) ? PageModuledd.EmpId : 0;
                string pagename = (PageModuledd.SubModuleName != "" || PageModuledd.SubModuleName != null) ? PageModuledd.SubModuleName : "";

                var PageModuledetails = (from PageModule in DB.PageModuleMasters
                                         where PageModule.PageName == pagename &&  PageModule.IsActive == true && PageModule.IsDeleted == false
                                         select PageModule).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (PageModuledetails == null)
                    {
                        PageModuleMaster smm = new PageModuleMaster();
                        smm.ModuleId = PageModuledd.ModuleId;
                        smm.SubModuleId = PageModuledd.SubModuleId;
                        smm.PageName = PageModuledd.PageModuleName;
                        smm.IsActive = true;
                        smm.IsUpdated = false;
                        smm.IsDeleted = false;
                        smm.CreatedBy = EmpId;
                        smm.CreatedDate = DateTime.Now;
                        smm.LastUpdatedBy = EmpId;
                        smm.LastUpdatedDate = DateTime.Now;
                        DB.PageModuleMasters.Add(smm);
                        DB.SaveChanges();

                        PageModuleViewModel pvm = new PageModuleViewModel();
                        pvm.msg = "Added";
                        pvm.PageModuleName = PageModuledd.PageModuleName;

                        return pvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Page Module Details Already Exists");
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
        public PageModuleViewModel UpdatePageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (PageModuledd.EmpId != 0) ? PageModuledd.EmpId : 0;
                int id = (PageModuledd.PageModuleId != 0) ? PageModuledd.PageModuleId : 0;
                string pagename = (PageModuledd.PageModuleName != "" || PageModuledd.PageModuleName != null) ? PageModuledd.PageModuleName : "";

                var PageModuledetails = (from PageModule in DB.PageModuleMasters
                                         where PageModule.PageModuleId == id && PageModule.IsActive == true && PageModule.IsDeleted == false
                                         select PageModule).FirstOrDefault();


                if (EmpId != 0)
                {
                    if (PageModuledetails != null)
                    {
                        PageModuledetails.PageName = PageModuledd.PageModuleName;
                        PageModuledetails.ModuleId = PageModuledd.ModuleId;
                        PageModuledetails.SubModuleId = PageModuledd.SubModuleId;
                        PageModuledetails.IsActive = true;
                        PageModuledetails.IsUpdated = true;
                        PageModuledetails.IsDeleted = false;
                        PageModuledetails.LastUpdatedBy = EmpId;
                        PageModuledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PageModuleViewModel pvm = new PageModuleViewModel();
                        pvm.msg = "Updated";
                        pvm.PageModuleName = PageModuledd.PageModuleName;

                        return pvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Page Module Details Not Found");
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
        public PageModuleViewModel DeletePageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                string msg = "";
                int? EmpId = (PageModuledd.EmpId != 0) ? PageModuledd.EmpId : 0;
                int id = (PageModuledd.PageModuleId != 0) ? PageModuledd.PageModuleId : 0;
                string pagename = (PageModuledd.PageModuleName != "" || PageModuledd.PageModuleName != null) ? PageModuledd.PageModuleName : "";

                var PageModuledetails = (from PageModule in DB.PageModuleMasters
                                         where PageModule.PageModuleId == id && PageModule.IsActive == true && PageModule.IsDeleted == false
                                         select PageModule).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (PageModuledetails != null)
                    {
                        PageModuledetails.IsDeleted = true;
                        PageModuledetails.LastUpdatedBy = EmpId;
                        PageModuledetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PageModuleViewModel pvm = new PageModuleViewModel();
                        pvm.msg = "Deleted";
                        pvm.PageModuleName = PageModuledd.PageModuleName;

                        return pvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Page Module Details Not Found");
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
        public List<AccessViewModel> GetAllAccess(AccessViewModel Accessdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;

                var Accessdetails = (from ap in DB.AccessPolicies
                                     join d in DB.DeptMasters on ap.DeptId equals d.DeptId
                                     join r in DB.DesignationMasters on ap.RoleId equals r.DesignationId
                                     join m in DB.ModuleMasters on ap.ModuleId equals m.ModuleId
                                     join sm in DB.SubModuleMasters on ap.SubModuleId equals sm.SubModuleId
                                     join p in DB.PageModuleMasters on ap.PageModuleId equals p.PageModuleId
                                     where ap.IsActive == true && ap.IsDeleted == false && d.IsActive == true && d.IsDeleted == false && r.IsActive == true && r.IsDeleted == false &&
                                     m.IsActive == true && m.IsDeleted == false && sm.IsActive == true && sm.IsDeleted == false && p.IsActive == true && p.IsDeleted == false
                                     select new AccessViewModel
                                     {
                                         AccessId = ap.AccessId,
                                         AccessName = ap.AccessName,
                                         DeptId = ap.DeptId,
                                         DeptName = d.DeptName,
                                         DeptShortName = d.DeptShortName,
                                         RoleId = ap.RoleId,
                                         RoleName = r.Designation,
                                         ModuleId = ap.ModuleId,
                                         ModuleName = m.ModuleName,
                                         SubModuleId = ap.SubModuleId,
                                         SubModuleName = sm.SubModuleName,
                                         PageModuleId = ap.PageModuleId,
                                         PageModuleName = p.PageName,
                                         AddAccess = ap.AddAccess,
                                         UpdateAccess = ap.UpdateAccess,
                                         DeleteAccess = ap.DeleteAccess,
                                         ViewAccess = ap.ViewAccess,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Accessdetails.Count() != 0)
                    {
                        return Accessdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Details Not Found");
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
        public List<AccessViewModel> GetAccess(AccessViewModel Accessdd)
        {
            try
            {
                int id = (Accessdd.AccessId != 0) ? Accessdd.AccessId : 0;
                string accessname = Accessdd.AccessName;
                string msg = "";
                int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;

                var Accessdetails = (from ap in DB.AccessPolicies
                                     join d in DB.DeptMasters on ap.DeptId equals d.DeptId
                                     join r in DB.DesignationMasters on ap.RoleId equals r.DesignationId
                                     join m in DB.ModuleMasters on ap.ModuleId equals m.ModuleId
                                     join sm in DB.SubModuleMasters on ap.SubModuleId equals sm.SubModuleId
                                     join p in DB.PageModuleMasters on ap.PageModuleId equals p.PageModuleId
                                     where ap.AccessId == id && ap.IsActive == true && ap.IsDeleted == false && d.IsActive == true && d.IsDeleted == false && r.IsActive == true && r.IsDeleted == false &&
                                     m.IsActive == true && m.IsDeleted == false && sm.IsActive == true && sm.IsDeleted == false && p.IsActive == true && p.IsDeleted == false
                                     select new AccessViewModel
                                     {
                                         AccessId = ap.AccessId,
                                         AccessName = ap.AccessName,
                                         DeptId = ap.DeptId,
                                         DeptName = d.DeptName,
                                         RoleId = ap.RoleId,
                                         RoleName = r.Designation,
                                         ModuleId = ap.ModuleId,
                                         ModuleName = m.ModuleName,
                                         SubModuleId = ap.SubModuleId,
                                         SubModuleName = sm.SubModuleName,
                                         PageModuleId = ap.PageModuleId,
                                         PageModuleName = p.PageName,
                                         AddAccess = ap.AddAccess,
                                         UpdateAccess = ap.UpdateAccess,
                                         DeleteAccess = ap.DeleteAccess,
                                         ViewAccess = ap.ViewAccess,
                                     }).ToList();

                if (EmpId != 0)
                {
                    if (Accessdetails.Count() != 0)
                    {
                        return Accessdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Details Not Found");
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
        public AccessViewModel AddAccess(AccessViewModel Accessdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;
                string accessname = (Accessdd.AccessName != "" || Accessdd.AccessName != null) ? Accessdd.AccessName : "";

                var Accessdetails = (from Access in DB.AccessPolicies
                                     where Access.AccessName == accessname && Access.IsActive == true && Access.IsDeleted == false
                                     select Access).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Accessdetails == null)
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
                        am.CreatedBy = Convert.ToInt32(EmpId);
                        am.CreatedDate = DateTime.Now;
                        am.LastUpdatedBy = EmpId;
                        am.LastUpdatedDate = DateTime.Now;
                        DB.AccessPolicies.Add(am);
                        DB.SaveChanges();

                        AccessViewModel avm = new AccessViewModel();
                        avm.msg = "Added";
                        avm.AccessName = Accessdd.AccessName;

                        return avm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Details Already Exists");
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
        public AccessViewModel UpdateAccess(AccessViewModel Accessdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;
                int id = (Accessdd.AccessId != 0) ? Accessdd.AccessId : 0;
                string accessname = (Accessdd.AccessName != "" || Accessdd.AccessName != null) ? Accessdd.AccessName : "";

                var Accessdetails = (from Access in DB.AccessPolicies
                                     where Access.AccessId == id && Access.IsActive == true && Access.IsDeleted == false
                                     select Access).FirstOrDefault();


                if (EmpId != 0)
                {
                    if (Accessdetails != null)
                    {
                        Accessdetails.AccessName = Accessdd.AccessName;
                        Accessdetails.DeptId = Accessdd.DeptId;
                        Accessdetails.RoleId = Accessdd.RoleId;
                        Accessdetails.ModuleId = Accessdd.ModuleId;
                        Accessdetails.SubModuleId = Accessdd.SubModuleId;
                        Accessdetails.PageModuleId = Accessdd.PageModuleId;
                        Accessdetails.AddAccess = Accessdd.AddAccess;
                        Accessdetails.UpdateAccess = Accessdd.UpdateAccess;
                        Accessdetails.DeleteAccess = Accessdd.DeleteAccess;
                        Accessdetails.ViewAccess = Accessdd.ViewAccess;
                        Accessdetails.IsActive = true;
                        Accessdetails.IsUpdated = true;
                        Accessdetails.IsDeleted = false;
                        Accessdetails.LastUpdatedBy = EmpId;
                        Accessdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        AccessViewModel avm = new AccessViewModel();
                        avm.msg = "Updated";
                        avm.AccessName = Accessdd.AccessName;

                        return avm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Details Not Found");
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
        public AccessViewModel DeleteAccess(AccessViewModel Accessdd)
        {
            try
            {
                string msg = "";
                int? EmpId = (Accessdd.EmpId != 0) ? Accessdd.EmpId : 0;
                int id = (Accessdd.AccessId != 0) ? Accessdd.AccessId : 0;
                string accessname = (Accessdd.AccessName != "" || Accessdd.AccessName != null) ? Accessdd.AccessName : "";

                var Accessdetails = (from Access in DB.AccessPolicies
                                     where Access.AccessId == id && Access.IsActive == true && Access.IsDeleted == false
                                     select Access).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Accessdetails != null)
                    {
                        Accessdetails.IsDeleted = true;
                        Accessdetails.LastUpdatedBy = EmpId;
                        Accessdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        AccessViewModel avm = new AccessViewModel();
                        avm.msg = "Deleted";
                        avm.AccessName = Accessdd.AccessName;

                        return avm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Access Details Not Found");
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