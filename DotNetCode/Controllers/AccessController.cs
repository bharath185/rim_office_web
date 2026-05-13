using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class AccessController : Controller
    {
        AccessModel AM = new AccessModel();

        // GET: Access
        public ActionResult Index()
        {
            return View();
        }
        // POST: Access/DDCompany
        [Route("Access/DDCompany")]
        [HttpPost]
        public ActionResult DDCompany(DDCompViewModel Deptdd)
        {
            try
            {
                var Comp = AM.DDCompany(Deptdd);
                return Json(Comp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDEmployee
        [Route("Access/DDEmployee")]
        [HttpPost]
        public ActionResult DDEmployee(DDEmpViewModel Empdd)
        {
            try
            {
                var Emp = AM.DDEmployee(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDDeptEmployee
        [Route("Access/DDDeptEmployee")]
        [HttpPost]
        public ActionResult DDDeptEmployee(DDDeptEmpViewModel Empdd)
        {
            try
            {
                var Emp = AM.DDDeptEmployee(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDDept
        [Route("Access/DDDept")]
        [HttpPost]
        public ActionResult DDDept(DDDeptViewModel Deptdd)
        {
            try
            {
                var Dept = AM.GetDDDept(Deptdd);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAllDept
        [Route("Access/GetAllDept")]
        [HttpPost]
        public ActionResult GetAllDept(DeptViewModel Deptdd)
        {
            try
            {
                var Dept = AM.GetAllDept(Deptdd);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetDept
        [Route("Access/GetDept")]
        [HttpPost]
        public ActionResult GetDept(DeptViewModel Deptdd)
        {
            try
            {
                var Dept = AM.GetDept(Deptdd);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/AddDept
        [Route("Access/AddDept")]
        [HttpPost]
        public ActionResult AddDept(DeptViewModel Deptdd)
        {
            try
            {
                var Dept = AM.AddDept(Deptdd);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/UpdateDept
        [Route("Access/UpdateDept")]
        [HttpPost]
        public ActionResult UpdateDept(DeptViewModel Deptdd)
        {
            try
            {
                var Dept = AM.UpdateDept(Deptdd);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DeleteDept
        [Route("Access/DeleteDept")]
        [HttpPost]
        public ActionResult DeleteDept(DeptViewModel Deptdd)
        {
            try
            {
                var Dept = AM.DeleteDept(Deptdd);
                return Json(Dept, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDRole
        [Route("Access/DDRole")]
        [HttpPost]
        public ActionResult DDRole(DDRoleViewModel roledd)
        {
            try
            {
                var Roles = AM.GetDDRole(roledd);
                return Json(Roles, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDDesignation
        [Route("Access/DDDesignation")]
        [HttpPost]
        public ActionResult DDDesignation(DDDesignationViewModel roledd)
        {
            try
            {
                var Roles = AM.GetDDDesignation(roledd);
                return Json(Roles, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDGrade
        [Route("Access/DDGrade")]
        [HttpPost]
        public ActionResult DDGrade(DDGradeViewModel gradedd)
        {
            try
            {
                var Roles = AM.GetDDGrade(gradedd);
                return Json(Roles, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAllRole
        [Route("Access/GetAllRole")]
        [HttpPost]
        public ActionResult GetAllRole(RoleViewModel Roledd)
        {
            try
            {
                var Role = AM.GetAllRole(Roledd);
                return Json(Role, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetRole
        [Route("Access/GetRole")]
        [HttpPost]
        public ActionResult GetRole(RoleViewModel Roledd)
        {
            try
            {
                var Role = AM.GetRole(Roledd);
                return Json(Role, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/AddRole
        [Route("Access/AddRole")]
        [HttpPost]
        public ActionResult AddRole(RoleViewModel Roledd)
        {
            try
            {
                var Role = AM.AddRole(Roledd);
                return Json(Role, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/UpdateRole
        [Route("Access/UpdateRole")]
        [HttpPost]
        public ActionResult UpdateRole(RoleViewModel Roledd)
        {
            try
            {
                var Role = AM.UpdateRole(Roledd);
                return Json(Role, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DeleteRole
        [Route("Access/DeleteRole")]
        [HttpPost]
        public ActionResult DeleteRole(RoleViewModel Roledd)
        {
            try
            {
                var Role = AM.DeleteRole(Roledd);
                return Json(Role, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDModule
        [Route("Access/DDModule")]
        [HttpPost]
        public ActionResult DDModule(DDModuleViewModel moddd)
        {
            try
            {
                var module = AM.GetDDModule(moddd);
                return Json(module, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAllModule
        [Route("Access/GetAllModule")]
        [HttpPost]
        public ActionResult GetAllModule(ModuleViewModel Moduledd)
        {
            try
            {
                var Module = AM.GetAllModule(Moduledd);
                return Json(Module, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetModule
        [Route("Access/GetModule")]
        [HttpPost]
        public ActionResult GetModule(ModuleViewModel Moduledd)
        {
            try
            {
                var Module = AM.GetModule(Moduledd);
                return Json(Module, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/AddModule
        [Route("Access/AddModule")]
        [HttpPost]
        public ActionResult AddModule(ModuleViewModel Moduledd)
        {
            try
            {
                var Module = AM.AddModule(Moduledd);
                return Json(Module, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/UpdateModule
        [Route("Access/UpdateModule")]
        [HttpPost]
        public ActionResult UpdateModule(ModuleViewModel Moduledd)
        {
            try
            {
                var Module = AM.UpdateModule(Moduledd);
                return Json(Module, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DeleteModule
        [Route("Access/DeleteModule")]
        [HttpPost]
        public ActionResult DeleteModule(ModuleViewModel Moduledd)
        {
            try
            {
                var Module = AM.DeleteModule(Moduledd);
                return Json(Module, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDSubModule
        [Route("Access/DDSubModule")]
        [HttpPost]
        public ActionResult DDSubModule(DDSubModuleViewModel submoddd)
        {
            try
            {
                var submodule = AM.GetDDSubModule(submoddd);
                return Json(submodule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAllSubModule
        [Route("Access/GetAllSubModule")]
        [HttpPost]
        public ActionResult GetAllSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                var SubModule = AM.GetAllSubModule(SubModuledd);
                return Json(SubModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetSubModule
        [Route("Access/GetSubModule")]
        [HttpPost]
        public ActionResult GetSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                var SubModule = AM.GetSubModule(SubModuledd);
                return Json(SubModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/AddSubModule
        [Route("Access/AddSubModule")]
        [HttpPost]
        public ActionResult AddSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                var SubModule = AM.AddSubModule(SubModuledd);
                return Json(SubModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/UpdateSubModule
        [Route("Access/UpdateSubModule")]
        [HttpPost]
        public ActionResult UpdateSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                var SubModule = AM.UpdateSubModule(SubModuledd);
                return Json(SubModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DeleteSubModule
        [Route("Access/DeleteSubModule")]
        [HttpPost]
        public ActionResult DeleteSubModule(SubModuleViewModel SubModuledd)
        {
            try
            {
                var SubModule = AM.DeleteSubModule(SubModuledd);
                return Json(SubModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDPageModule
        [Route("Access/DDPageModule")]
        [HttpPost]
        public ActionResult DDPageModule(DDPageModuleViewModel pagedd)
        {
            try
            {
                var page = AM.GetDDPageModule(pagedd);
                return Json(page, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAllPageModule
        [Route("Access/GetAllPageModule")]
        [HttpPost]
        public ActionResult GetAllPageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                var PageModule = AM.GetAllPageModule(PageModuledd);
                return Json(PageModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetPageModule
        [Route("Access/GetPageModule")]
        [HttpPost]
        public ActionResult GetPageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                var PageModule = AM.GetPageModule(PageModuledd);
                return Json(PageModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/AddPageModule
        [Route("Access/AddPageModule")]
        [HttpPost]
        public ActionResult AddPageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                var PageModule = AM.AddPageModule(PageModuledd);
                return Json(PageModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/UpdatePageModule
        [Route("Access/UpdatePageModule")]
        [HttpPost]
        public ActionResult UpdatePageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                var PageModule = AM.UpdatePageModule(PageModuledd);
                return Json(PageModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DeletePageModule
        [Route("Access/DeletePageModule")]
        [HttpPost]
        public ActionResult DeletePageModule(PageModuleViewModel PageModuledd)
        {
            try
            {
                var PageModule = AM.DeletePageModule(PageModuledd);
                return Json(PageModule, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DDAccess
        [Route("Access/DDAccess")]
        [HttpPost]
        public ActionResult DDAccess(DDAccessViewModel accessdd)
        {
            try
            {
                var access = AM.GetDDAccess(accessdd);
                return Json(access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAllAccess
        [Route("Access/GetAllAccess")]
        [HttpPost]
        public ActionResult GetAllAccess(AccessViewModel Accessdd)
        {
            try
            {
                var Access = AM.GetAllAccess(Accessdd);
                return Json(Access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAccess
        [Route("Access/GetAccess")]
        [HttpPost]
        public ActionResult GetAccess(AccessViewModel Accessdd)
        {
            try
            {
                var Access = AM.GetAccess(Accessdd);
                return Json(Access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/AddAccess
        [Route("Access/AddAccess")]
        [HttpPost]
        public ActionResult AddAccess(AccessViewModel Accessdd)
        {
            try
            {
                var Access = AM.AddAccess(Accessdd);
                return Json(Access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/UpdateAccess
        [Route("Access/UpdateAccess")]
        [HttpPost]
        public ActionResult UpdateAccess(AccessViewModel Accessdd)
        {
            try
            {
                var Access = AM.UpdateAccess(Accessdd);
                return Json(Access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/DeleteAccess
        [Route("Access/DeleteAccess")]
        [HttpPost]
        public ActionResult DeleteAccess(AccessViewModel Accessdd)
        {
            try
            {
                var Access = AM.DeleteAccess(Accessdd);
                return Json(Access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Access/GetAccesspolicy
        [Route("Access/GetAccesspolicy")]
        [HttpPost]
        public ActionResult GetAccesspolicy(DDAccessPolicyViewModel accessdd)
        {
            try
            {
                var access = AM.GetAccesspolicy(accessdd);
                return Json(access, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
       
    }
}