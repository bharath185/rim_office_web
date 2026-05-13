using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeConnect_Web.Models;
using OfficeConnect_Web.ViewModel;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class ShiftController : Controller
    {
        ShiftModel SM = new ShiftModel();

        // POST: Shift/GetAllShift
        [Route("Shift/GetAllShift")]
        [HttpPost]
        public ActionResult GetAllShift(ShiftMasterViewModel model)
        {
            try
            {
                var emp = SM.GetAllShift(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/GetShift
        [Route("Shift/GetShift")]
        [HttpPost]
        public ActionResult GetShift(ShiftMasterViewModel model)
        {
            try
            {
                var emp = SM.GetShift(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/AddShift
        [Route("Shift/AddShift")]
        [HttpPost]
        public ActionResult AddShift(ShiftMasterViewModel model)
        {
            try
            {
                var emp = SM.AddShift(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/UpdateShift
        [Route("Shift/UpdateShift")]
        [HttpPost]
        public ActionResult UpdateShift(ShiftMasterViewModel model)
        {
            try
            {
                var emp = SM.UpdateShift(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/DeleteShift
        [Route("Shift/DeleteShift")]
        [HttpPost]
        public ActionResult DeleteShift(ShiftMasterViewModel model)
        {
            try
            {
                var emp = SM.DeleteShift(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/GetAllShiftGrouping
        [Route("Shift/GetAllShiftGrouping")]
        [HttpPost]
        public ActionResult GetAllShiftGrouping(ShiftGroupingViewModel model)
        {
            try
            {
                var emp = SM.GetAllShiftGrouping(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        ////// POST: Shift/LocationShiftGrouping
        ////[Route("Shift/LocationShiftGrouping")]
        ////[HttpPost]
        ////public ActionResult LocationShiftGrouping(ShiftGroupingViewModel model)
        ////{
        ////    try
        ////    {
        ////        var emp = SM.LocationShiftGrouping(model);
        ////        return Json(emp, JsonRequestBehavior.AllowGet);
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
        ////    }
        ////}
        
        // POST: Shift/AddShiftGrouping
        [Route("Shift/AddShiftGrouping")]
        [HttpPost]
        public ActionResult AddShiftGrouping(ShiftGroupingViewModel model)
        {
            try
            {
                var emp = SM.AddShiftGrouping(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/DDShift
        [Route("Shift/DDShift")]
        [HttpPost]
        public ActionResult DDShift(ShiftGroupingViewModel Empdd)
        {
            try
            {
                var Emp = SM.DDShift(Empdd);
                return Json(Emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/GetAllShiftEmployee
        [Route("Shift/GetAllShiftEmployee")]
        [HttpPost]
        public ActionResult GetAllShiftEmployee(ShiftEmployeeMasterViewModel model)
        {
            try
            {
                var emp = SM.GetAllShiftEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/AddShiftEmployee
        [Route("Shift/AddShiftEmployee")]
        [HttpPost]
        public ActionResult AddShiftEmployee(ShiftEmployeeMappingMasterViewModel model)
        {
            try
            {
                var emp = SM.AddShiftEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
        // POST: Shift/RemoveShiftEmployee
        [Route("Shift/RemoveShiftEmployee")]
        [HttpPost]
        public ActionResult RemoveShiftEmployee(ShiftEmployeeMappingMasterViewModel model)
        {
            try
            {
                var emp = SM.RemoveShiftEmployee(model);
                return Json(emp, JsonRequestBehavior.AllowGet);
            }
            catch (CustomApiException ex)
            {
                return Json(new { StatusCode = ex.StatusCode, Message = ex.Message });
            }
        }
    }
}