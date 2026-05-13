using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class LoginController : Controller
    {
        LoginModel LM = new LoginModel();

        // POST: Login/Create
        [Route("Login/Login")]
        [HttpPost]
        public ActionResult Login(LoginViewModel LoginUser)
        {
            try
            {
                // TODO: Add insert logic here
                if (LoginUser == null || string.IsNullOrEmpty(LoginUser.UserName) || string.IsNullOrEmpty(LoginUser.Password))
                {
                    //var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("No Employee found with ID = {0}", LoginUser)),
                    //    ReasonPhrase = "Invalid input parameters"
                    //};

                    //throw new System.Web.Http.HttpResponseException(response);
                    throw new CustomApiException(HttpStatusCode.NotFound, "Invalid Input Parameters");
                }
                else
                {
                    EmployeeMasterViewModel EM = new EmployeeMasterViewModel();
                    var Emp = LM.CheckLogin(LoginUser);
                    return Json(Emp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }

        // POST: Login/Create
        [Route("Login/LogOut")]
        [HttpPost]
        public ActionResult LogOut(LoginViewModel LoginUser)
        {
            try
            {
                // TODO: Add insert logic here
                if (LoginUser == null || string.IsNullOrEmpty(LoginUser.UserName))
                {
                    //var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("No Employee found with ID = {0}", LoginUser)),
                    //    ReasonPhrase = "Invalid input parameters"
                    //};

                    //throw new System.Web.Http.HttpResponseException(response);

                    throw new CustomApiException(HttpStatusCode.NotFound, "Not Found");
                }
                else
                {
                    EmployeeMasterViewModel EM = new EmployeeMasterViewModel();
                    var Emp = LM.CheckLogOut(LoginUser);
                    return Json(Emp, JsonRequestBehavior.AllowGet);
                }
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Login/CheckAuth
        [Route("Login/CheckAuth")]
        [HttpPost]
        public ActionResult CheckAuth(LoginViewModel model)
        {
            try
            {
                var CheckAuth = LM.CheckAuth(model);
                return Json(CheckAuth, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Login/ForgetPassword
        [Route("Login/ForgetPassword")]
        [HttpPost]
        public ActionResult ForgetPassword(FRViewModel FPM)
        {
            try
            {
                var Emp = LM.ForgetPassword(FPM);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Login/FPwdVerify
        [Route("Login/FPwdVerify")]
        [HttpPost]
        public ActionResult FPwdVerify(FRViewModel FPM)
        {
            try
            {
                var Emp = LM.FPwdVerify(FPM);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Login/ChangePassword
        [Route("Login/ChangePassword")]
        [HttpPost]
        public ActionResult ChangePassword(PassHistoryManagementViewModel FPM)
        {
            try
            {
                var Emp = LM.ChangePassword(FPM);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Login/LoginDetails
        [Route("Login/LoginDetails")]
        [HttpPost]
        public ActionResult LoginDetails(LoginDetailsViewModel LDVM)
        {
            try
            {
                var Emp = LM.LoginDetails(LDVM);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}
