using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class DDCompViewModel
    {
        public int CompId { get; set; }
        public int LEId { get; set; }
        public int LocationId { get; set; }
        public string CompName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDEmpViewModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
    }
    public class DDDeptEmpViewModel
    {
        public int DeptId { get; set; }
        public int DesignationId { get; set; }
        public int LoginId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
    }
    public class DDDeptViewModel
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDRoleViewModel
    {
        public int? DeptId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDDesignationViewModel
    {
        public int? DeptId { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public int EmpId { get; set; }
    }

    public class DDGradeViewModel
    {
        public int? GradeId { get; set; }
        public string Grade { get; set; }
        public int EmpId { get; set; }
    }
    public class DDModuleViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDSubModuleViewModel
    {
        public int? ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDPageModuleViewModel
    {
        public int? ModuleId { get; set; }
        public int? SubModuleId { get; set; }
        public int PageModuleId { get; set; }
        public string PageName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDAccessViewModel
    {
        public int AccessId { get; set; }
        public string AccessName { get; set; }
        public int EmpId { get; set; }
    }
    public class DDAccessPolicyViewModel
    {
        public int? AccessId { get; set; }
        public string AccessName { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public int? PageModuleId { get; set; }
        public string PageName { get; set; }
        public bool? AddAccess { get; set; }
        public bool? UpdateAccess { get; set; }
        public bool? DeleteAccess { get; set; }
        public bool? ViewAccess { get; set; }
        public int EmpId { get; set; }

    }
    public class DeptViewModel
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string RoleName { get; set; }
        public int? GradeId { get; set; }
        public string Grade { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
    public class ModuleViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
    public class SubModuleViewModel
    {
        public int SubModuleId { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
    public class PageAccessViewModel
    {
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public Nullable<bool> PageAccess { get; set; }
        public int? RoleId { get; set; }
        public int PageModuleId { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public string PageName { get; set; }
        public string PageModuleName { get; set; }
        public Nullable<bool> AddAccess { get; set; }
        public Nullable<bool> UpdateAccess { get; set; }
        public Nullable<bool> DeleteAccess { get; set; }
        public Nullable<bool> ViewAccess { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
    public class PageModuleViewModel
    {
        public int PageModuleId { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public string PageName { get; set; }
        public string PageModuleName { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
    public class AccessViewModel
    {
        public int AccessId { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public int? PageModuleId { get; set; }
        public string PageModuleName { get; set; }
        public string AccessName { get; set; }
        public Nullable<bool> AddAccess { get; set; }
        public Nullable<bool> UpdateAccess { get; set; }
        public Nullable<bool> DeleteAccess { get; set; }
        public Nullable<bool> ViewAccess { get; set; }
        public string msg { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int EmpId { get; set; }
    }
 
}