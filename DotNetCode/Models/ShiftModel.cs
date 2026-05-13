using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;

namespace OfficeConnect_Web.Models
{
    public class ShiftModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        public List<ShiftMasterViewModel> GetAllShift(ShiftMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var shiftdetails = (from sh in DB.ShiftMasters
                                    where sh.IsActive == true && sh.IsDeleted == false
                                    select sh).OrderByDescending(x => x.ShiftId).ToList();

                if (loginId != 0)
                {
                    if (shiftdetails != null)
                    {
                        List<ShiftMasterViewModel> lstofShift = new List<ShiftMasterViewModel>();

                        for (int i = 0; i < shiftdetails.Count(); i++)
                        {
                            ShiftMasterViewModel smvm = new ShiftMasterViewModel();
                            smvm.ShiftId = shiftdetails[i].ShiftId;
                            smvm.ShiftName = shiftdetails[i].ShiftName;
                            smvm.StartTime = shiftdetails[i].StartTime;
                            smvm.EndTime = shiftdetails[i].EndTime;
                            smvm.ClkHrs = shiftdetails[i].ClkHrs;
                            smvm.Days = shiftdetails[i].Days;
                            smvm.Status = shiftdetails[i].Status;
                            smvm.CreatedBy = shiftdetails[i].CreatedBy;
                            smvm.CreatedDate = shiftdetails[i].CreatedDate;
                            smvm.LastUpdatedBy = shiftdetails[i].LastUpdatedBy;
                            smvm.LastUpdatedDate = shiftdetails[i].LastUpdatedDate;
                            smvm.IsActive = shiftdetails[i].IsActive;
                            smvm.IsUpdated = shiftdetails[i].IsUpdated;
                            smvm.IsDeleted = shiftdetails[i].IsDeleted;
                            lstofShift.Add(smvm);
                        }

                        return lstofShift;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Shifts Detail Not Found");
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
        public ShiftMasterViewModel GetShift(ShiftMasterViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? shiftId = (model.ShiftId != 0) ? model.ShiftId : 0;

                var shiftdetails = (from sh in DB.ShiftMasters
                                    where sh.ShiftId == shiftId && sh.IsActive == true && sh.IsDeleted == false
                                    select sh).FirstOrDefault();

                if (loginId != 0)
                {
                    if (shiftdetails != null)
                    {
                        ShiftMasterViewModel smvm = new ShiftMasterViewModel();
                        smvm.ShiftId = shiftdetails.ShiftId;
                        smvm.ShiftName = shiftdetails.ShiftName;
                        smvm.StartTime = shiftdetails.StartTime;
                        smvm.EndTime = shiftdetails.EndTime;
                        smvm.ClkHrs = shiftdetails.ClkHrs;
                        smvm.Days = shiftdetails.Days;
                        smvm.Status = shiftdetails.Status;
                        smvm.CreatedBy = shiftdetails.CreatedBy;
                        smvm.CreatedDate = shiftdetails.CreatedDate;
                        smvm.LastUpdatedBy = shiftdetails.LastUpdatedBy;
                        smvm.LastUpdatedDate = shiftdetails.LastUpdatedDate;
                        smvm.IsActive = shiftdetails.IsActive;
                        smvm.IsUpdated = shiftdetails.IsUpdated;
                        smvm.IsDeleted = shiftdetails.IsDeleted;

                        return smvm;
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
        public ShiftMasterViewModel AddShift(ShiftMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var shiftdetails = (from sh in DB.ShiftMasters
                                    where sh.ShiftName == model.ShiftName
                                    && sh.IsActive == true && sh.IsDeleted == false
                                    select sh).ToList();

                if (loginId != 0)
                {
                    if (shiftdetails.Count() == 0)
                    {
                        ShiftMaster sm = new ShiftMaster();
                        sm.ShiftId = model.ShiftId;
                        sm.ShiftName = model.ShiftName;
                        sm.StartTime = model.StartTime;
                        sm.EndTime = model.EndTime;
                        sm.ClkHrs = model.ClkHrs;
                        sm.Days = model.Days;
                        sm.Status = true;
                        sm.CreatedBy = model.LoginId;
                        sm.CreatedDate = DateTime.Now;
                        sm.LastUpdatedBy = model.LoginId;
                        sm.LastUpdatedDate = DateTime.Now;
                        sm.IsActive = true;
                        sm.IsUpdated = false;
                        sm.IsDeleted = false;
                        DB.ShiftMasters.Add(sm);
                        DB.SaveChanges();
                        int ShiftId = sm.ShiftId;

                        ShiftMasterViewModel smvm = new ShiftMasterViewModel();
                        smvm.ShiftId = ShiftId;
                        smvm.msg = "Added";

                        return smvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Shift Details Already Exists");
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
        public ShiftMasterViewModel UpdateShift(ShiftMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? shiftId = (model.ShiftId != 0) ? model.ShiftId : 0;

                var shiftdetails = (from sh in DB.ShiftMasters
                                    where sh.ShiftId == shiftId && sh.IsActive == true && sh.IsDeleted == false
                                    select sh).FirstOrDefault();

                if (loginId != 0)
                {
                    if (shiftdetails != null)
                    {
                        shiftdetails.ShiftId = model.ShiftId;
                        shiftdetails.ShiftName = model.ShiftName;
                        shiftdetails.StartTime = model.StartTime;
                        shiftdetails.EndTime = model.EndTime;
                        shiftdetails.ClkHrs = model.ClkHrs;
                        shiftdetails.Days = model.Days;
                        shiftdetails.Status = true;
                        shiftdetails.LastUpdatedBy = model.LoginId;
                        shiftdetails.LastUpdatedDate = DateTime.Now;
                        shiftdetails.IsActive = true;
                        shiftdetails.IsUpdated = true;
                        shiftdetails.IsDeleted = false;
                        DB.SaveChanges();

                        ShiftMasterViewModel emvm = new ShiftMasterViewModel();
                        emvm.msg = "Updated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Shift Details Not Found");
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
        public ShiftMasterViewModel DeleteShift(ShiftMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? shiftId = (model.ShiftId != 0) ? model.ShiftId : 0;

                var shiftdetails = (from sh in DB.ShiftMasters
                                    where sh.ShiftId == shiftId && sh.IsActive == true && sh.IsDeleted == false
                                    select sh).FirstOrDefault();

                if (loginId != 0)
                {
                    if (shiftdetails != null)
                    {
                        shiftdetails.Status = true;
                        shiftdetails.IsActive = true;
                        shiftdetails.IsUpdated = true;
                        shiftdetails.IsDeleted = true;
                        shiftdetails.LastUpdatedBy = model.LoginId;
                        shiftdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        ShiftMasterViewModel emvm = new ShiftMasterViewModel();
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
        public List<ShiftGroupingViewModel> GetAllShiftGrouping(ShiftGroupingViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId, leId, buId, locationId, shiftId = 0;

                var groupedshiftdetails = (from sg in DB.ShiftGroupingMasters
                                           where sg.IsActive == true && sg.IsDeleted == false
                                           group sg by new { sg.CompId, sg.LEId, sg.BUId, sg.LocationId } into grouped
                                           orderby grouped.Max(x => x.ShiftId) descending
                                           select new
                                           {
                                               CompId = grouped.Key.CompId,
                                               LEId = grouped.Key.LEId,
                                               BUId = grouped.Key.BUId,
                                               LocationId = grouped.Key.LocationId // If you need the latest ShiftId for each group
                                           }).ToList();

                if (loginId != 0)
                {
                    if (groupedshiftdetails != null)
                    {
                        List<ShiftGroupingViewModel> lstofShiftGroup = new List<ShiftGroupingViewModel>();

                        for (int i = 0; i < groupedshiftdetails.Count(); i++)
                        {
                            compId = groupedshiftdetails[i].CompId;
                            leId = groupedshiftdetails[i].LEId;
                            buId = groupedshiftdetails[i].BUId;
                            locationId = groupedshiftdetails[i].LocationId;

                            var shiftdetails = (from sg in DB.ShiftGroupingMasters
                                                where sg.CompId == compId && sg.LEId == leId && sg.BUId == buId && sg.LocationId == locationId
                                                && sg.IsActive == true && sg.Status == true && sg.IsDeleted == false
                                                select sg).OrderByDescending(x => x.ShiftId).ToList();

                            List<SampleShiftMasterViewModel> lstofShift = new List<SampleShiftMasterViewModel>();

                            for (int j = 0; j < shiftdetails.Count(); j++)
                            {
                                SampleShiftMasterViewModel smvm = new SampleShiftMasterViewModel();
                                smvm.ShiftId = Convert.ToInt32(shiftdetails[j].ShiftId);
                                smvm.ShiftName = (smvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == smvm.ShiftId).Select(x => x.ShiftName).FirstOrDefault() : "";
                                smvm.StartTime = DB.ShiftMasters.Where(x => x.ShiftId == smvm.ShiftId).Select(x => x.StartTime).FirstOrDefault();
                                smvm.EndTime = DB.ShiftMasters.Where(x => x.ShiftId == smvm.ShiftId).Select(x => x.EndTime).FirstOrDefault();
                                smvm.ClkHrs = (smvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == smvm.ShiftId).Select(x => x.ClkHrs).FirstOrDefault() : "";
                                smvm.Days = (smvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == smvm.ShiftId).Select(x => x.Days).FirstOrDefault() : "";
                                lstofShift.Add(smvm);
                            }

                            ShiftGroupingViewModel sgvm = new ShiftGroupingViewModel();
                            sgvm.CompId = compId;
                            sgvm.Company = (sgvm.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == sgvm.CompId).Select(x => x.Company).FirstOrDefault() : "";
                            sgvm.LEId = leId;
                            sgvm.LegalEntity = (sgvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == sgvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                            sgvm.BUId = buId;
                            sgvm.BusinessUnit = (sgvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == sgvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                            sgvm.LocationId = locationId;
                            sgvm.Location = (sgvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == sgvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";

                            if (compId != 0 && leId != 0 && buId == 0 && locationId == 0)
                            {
                                sgvm.Company = sgvm.Company + " - " + sgvm.LegalEntity;
                            }
                            else if (compId != 0 && leId != 0 && buId != 0 && locationId != 0)
                            {
                                sgvm.Company = sgvm.Company + " - " + sgvm.Location;
                            }
                            else
                            {
                                sgvm.Company = (sgvm.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == sgvm.CompId).Select(x => x.Company).FirstOrDefault() : "";
                            }
                            sgvm.lstOfShift = lstofShift;
                            lstofShiftGroup.Add(sgvm);

                        }

                        return lstofShiftGroup;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Shifts Grouping Detail Not Found");
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
        //public List<ShiftGroupingViewModel> LocationShiftGrouping(ShiftGroupingViewModel model)
        //{
        //    try
        //    {
        //        string msg = "";
        //        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        //        int? compId, leId, buId, locationId, shiftId = 0;

        //        var groupedshiftdetails = (from sg in DB.ShiftGroupingMasters
        //                                    where sg.IsActive == true && sg.IsDeleted == false
        //                                    group sg by new { sg.CompId, sg.LEId, sg.BUId, sg.LocationId } into grouped
        //                                    orderby grouped.Max(x => x.ShiftId) descending
        //                                    select new
        //                                    {
        //                                        CompId = grouped.Key.CompId,
        //                                        LEId = grouped.Key.LEId,
        //                                        BUId = grouped.Key.BUId,
        //                                        LocationId = grouped.Key.LocationId // If you need the latest ShiftId for each group
        //                                    }).ToList();

        //        if (loginId != 0)
        //        {
        //            if (groupedshiftdetails != null)
        //            {
        //                List<ShiftGroupingViewModel> lstofShiftGroup = new List<ShiftGroupingViewModel>();

        //                for (int i = 0; i < groupedshiftdetails.Count(); i++)
        //                {
        //                    compId = groupedshiftdetails[i].CompId;
        //                    leId = groupedshiftdetails[i].LEId;
        //                    buId = groupedshiftdetails[i].BUId;
        //                    locationId = groupedshiftdetails[i].LocationId;

        //                    var shiftdetails = (from sg in DB.ShiftGroupingMasters
        //                                        where sg.CompId == compId && sg.LEId == leId && sg.BUId == buId && sg.LocationId == locationId
        //                                        && sg.IsActive == true && sg.Status == true && sg.IsDeleted == false
        //                                        select sg).OrderByDescending(x => x.ShiftId).ToList();

        //                    List<ShiftMasterViewModel> lstofShift = new List<ShiftMasterViewModel>();

        //                    for (int j = 0; j < shiftdetails.Count(); i++)
        //                    {
        //                        ShiftMasterViewModel smvm = new ShiftMasterViewModel();
        //                        smvm.ShiftId = Convert.ToInt32(shiftdetails[j].ShiftId);
        //                        smvm.ShiftName = (smvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == smvm.ShiftId).Select(x => x.ShiftName).FirstOrDefault() : "";
        //                        smvm.Status = shiftdetails[j].Status;
        //                        smvm.CreatedBy = shiftdetails[j].CreatedBy;
        //                        smvm.CreatedDate = shiftdetails[j].CreatedDate;
        //                        smvm.LastUpdatedBy = shiftdetails[j].LastUpdatedBy;
        //                        smvm.LastUpdatedDate = shiftdetails[j].LastUpdatedDate;
        //                        smvm.IsActive = shiftdetails[j].IsActive;
        //                        smvm.IsUpdated = shiftdetails[j].IsUpdated;
        //                        smvm.IsDeleted = shiftdetails[j].IsDeleted;
        //                        lstofShift.Add(smvm);
        //                    }

        //                    ShiftGroupingViewModel sgvm = new ShiftGroupingViewModel();
        //                    sgvm.CompId = compId;
        //                    sgvm.Company = (sgvm.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == sgvm.CompId).Select(x => x.Company).FirstOrDefault() : "";
        //                    sgvm.LEId = leId;
        //                    sgvm.LegalEntity = (sgvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == sgvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
        //                    sgvm.BUId = buId;
        //                    sgvm.BusinessUnit = (sgvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == sgvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
        //                    sgvm.LocationId = locationId;
        //                    sgvm.Location = (sgvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == sgvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
        //                    sgvm.lstOfShift = lstofShift;
        //                    lstofShiftGroup.Add(sgvm);

        //                }

        //                return lstofShiftGroup;
        //            }
        //            else
        //            {
        //                throw new CustomApiException(HttpStatusCode.NotFound, "Shifts Detail Not Found");
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
        public ShiftGroupingViewModel AddShiftGrouping(ShiftGroupingViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId, leId, buId, locationId, shiftId = 0;


                compId = model.CompId;
                leId = model.LEId;
                buId = model.BUId;
                locationId = model.LocationId;

                var shiftdetails = (from sh in DB.ShiftGroupingMasters
                                    where sh.CompId == compId && sh.LEId == leId && sh.BUId == buId && sh.LocationId == locationId
                                    && sh.IsActive == true && sh.IsDeleted == false
                                    select sh).ToList();

                if (loginId != 0)
                {
                    if (shiftdetails.Count() != 0)
                    {
                        for (int j = 0; j < shiftdetails.Count(); j++)
                        {
                            shiftdetails[j].Status = false;
                            shiftdetails[j].LastUpdatedBy = model.LoginId;
                            shiftdetails[j].LastUpdatedDate = DateTime.Now;
                            shiftdetails[j].IsActive = true;
                            shiftdetails[j].IsUpdated = true;
                            shiftdetails[j].IsDeleted = true;
                            DB.SaveChanges();
                        }

                        if (model.lstOfShift.Count() != 0)
                        {
                            for (int i = 0; i < model.lstOfShift.Count(); i++)
                            {
                                ShiftGroupingMaster sqm = new ShiftGroupingMaster();
                                sqm.ShiftId = model.lstOfShift[i].ShiftId;
                                sqm.CompId = model.CompId;
                                sqm.LEId = model.LEId;
                                sqm.BUId = model.BUId;
                                sqm.LocationId = model.LocationId;
                                sqm.Status = true;
                                sqm.CreatedBy = model.LoginId;
                                sqm.CreatedDate = DateTime.Now;
                                sqm.LastUpdatedBy = model.LoginId;
                                sqm.LastUpdatedDate = DateTime.Now;
                                sqm.IsActive = true;
                                sqm.IsUpdated = false;
                                sqm.IsDeleted = false;
                                DB.ShiftGroupingMasters.Add(sqm);
                                DB.SaveChanges();
                            }

                            ShiftGroupingViewModel sqvm = new ShiftGroupingViewModel();
                            sqvm.msg = "Added";

                            return sqvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Shift Grouping Details Already Exists");
                        }
                    }
                    else
                    {
                        if (model.lstOfShift.Count() != 0)
                        {
                            for (int i = 0; i < model.lstOfShift.Count(); i++)
                            {
                                ShiftGroupingMaster sqm = new ShiftGroupingMaster();
                                sqm.ShiftId = model.lstOfShift[i].ShiftId;
                                sqm.CompId = model.CompId;
                                sqm.LEId = model.LEId;
                                sqm.BUId = model.BUId;
                                sqm.LocationId = model.LocationId;
                                sqm.Status = true;
                                sqm.CreatedBy = model.LoginId;
                                sqm.CreatedDate = DateTime.Now;
                                sqm.LastUpdatedBy = model.LoginId;
                                sqm.LastUpdatedDate = DateTime.Now;
                                sqm.IsActive = true;
                                sqm.IsUpdated = false;
                                sqm.IsDeleted = false;
                                DB.ShiftGroupingMasters.Add(sqm);
                                DB.SaveChanges();
                            }

                            ShiftGroupingViewModel sqvm = new ShiftGroupingViewModel();
                            sqvm.msg = "Added";

                            return sqvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Shift Grouping Details Already Exists");
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
        public List<DDShiftViewModel> DDShift(ShiftGroupingViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locationId = (model.LocationId != 0) ? model.LocationId : 0;

                var Empdetails = (from Sh in DB.ShiftMasters
                                  join sg in DB.ShiftGroupingMasters on Sh.ShiftId equals sg.ShiftId
                                  where sg.CompId == compId && sg.LEId == leId && sg.BUId == buId && sg.LocationId == locationId &&
                                  Sh.IsActive == true && Sh.IsDeleted == false && sg.IsActive == true && sg.IsDeleted == false
                                  select new DDShiftViewModel
                                  {
                                      ShiftId = Sh.ShiftId,
                                      ShiftName = Sh.ShiftName,
                                  }).ToList();

                if (LoginId != 0)
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
        public ShiftEmployeeListViewModel GetAllShiftEmployee(ShiftEmployeeMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? ShiftId = (model.ShiftId != 0) ? model.ShiftId : 0;

                string Company = "";

                ShiftEmployeeListViewModel lstofShiftEmp = new ShiftEmployeeListViewModel();

                if (compId != 0 && leId != 0 && buId == 0 && locationId == 0)
                {
                    string company = (compId != 0) ? DB.CompanyMasters.Where(x => x.CompId == compId).Select(x => x.Company).FirstOrDefault() : "";
                    string entity = (leId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == leId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                    Company = company + " - " + entity;

                    var Shiftempdetails = (from emp in DB.EmployeeMasters
                                           join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId
                                           join Comp in DB.CompanyMasters on emp.CompId equals Comp.CompId
                                           join LE in DB.LegalEntityMasters on emp.LEId equals LE.LEId
                                           where emp.CompId == compId && emp.LEId == leId && emp.BUId == 0 && emp.LocationId == 0 &&
                                           esd.ShiftId == ShiftId && esd.ShiftStatus == true && esd.IsActive == true && esd.IsDeleted == false &&
                                           emp.IsActive == true && emp.IsDeleted == false
                                           select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();

                    var nonShiftempdetails = (from emp in DB.EmployeeMasters
                                           join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId
                                           join Comp in DB.CompanyMasters on emp.CompId equals Comp.CompId
                                           join LE in DB.LegalEntityMasters on emp.LEId equals LE.LEId
                                           where emp.CompId == compId && emp.LEId == leId && emp.BUId == 0 && emp.LocationId == 0 &&
                                           esd.ShiftId != ShiftId && esd.ShiftStatus == true && esd.IsActive == true && esd.IsDeleted == false &&
                                           emp.IsActive == true && emp.IsDeleted == false
                                           select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();

                    var empdetails = (from emp in DB.EmployeeMasters
                                      join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId into shiftGroup
                                      from esd in shiftGroup.DefaultIfEmpty() // This performs a left join
                                      where emp.CompId == compId && emp.LEId == leId && emp.BUId == 0 && emp.LocationId == 0 && 
                                      emp.IsActive == true && emp.IsDeleted == false
                                      && (esd == null) || (emp.CompId == compId && emp.LEId == leId && emp.BUId == 0 && emp.LocationId == 0 &&
                                      esd.ShiftStatus == false && esd.Status == false && esd.IsActive == true && esd.IsDeleted == true)
                                      select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();

                    //if (empdetails.Count() == 0)
                    //{
                    //    empdetails = (from emp in DB.EmployeeMasters
                    //                  join esd in DB.EmpShiftDetails on emp.LEId equals esd.LEId into shiftGroup
                    //                  from esd in shiftGroup.DefaultIfEmpty() // This performs a left join
                    //                  where emp.CompId == compId && emp.LEId == leId && emp.BUId == 0 && emp.LocationId == 0 &&
                    //                  emp.IsActive == true && emp.IsDeleted == false
                    //                  select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();
                    //}

                    if (loginId != 0)
                    {
                        if (Shiftempdetails != null)
                        {
                            List<ShiftEmployeeMasterViewModel> lstofEmp = new List<ShiftEmployeeMasterViewModel>();
                            List<ShiftEmployeeMasterViewModel> lstofNonEmp = new List<ShiftEmployeeMasterViewModel>();

                            for (int i = 0; i < Shiftempdetails.Count(); i++)
                            {
                                ShiftEmployeeMasterViewModel emvm = new ShiftEmployeeMasterViewModel();
                                emvm.EmpId = Shiftempdetails[i].EmpId;
                                emvm.OldEmp_ID = Shiftempdetails[i].OldEmp_ID;
                                emvm.CompId = Shiftempdetails[i].CompId;
                                emvm.Company = Company;
                                emvm.LEId = (Shiftempdetails[i].LEId != 0) ? Shiftempdetails[i].LEId : 0;
                                emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                                emvm.BUId = (Shiftempdetails[i].BUId != 0) ? Shiftempdetails[i].BUId : 0;
                                emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                                emvm.LocationId = (Shiftempdetails[i].LocationId != 0) ? Shiftempdetails[i].LocationId : 0;
                                emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                                emvm.ShiftId = (emvm.EmpId != 0) ? DB.EmpShiftDetails.Where(x => x.EmpId == emvm.EmpId && x.ShiftStatus == true && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftId).FirstOrDefault() : 0;
                                emvm.ShiftName = (emvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == emvm.ShiftId && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftName).FirstOrDefault() : "";
                                emvm.CategoryId = Shiftempdetails[i].CategoryId;
                                emvm.DeptId = Shiftempdetails[i].CategoryId;
                                emvm.DeptName = Shiftempdetails[i].DeptName;
                                emvm.DesignationId = Shiftempdetails[i].DesignationId;
                                emvm.Designation = Shiftempdetails[i].DesignationName;
                                emvm.ReportId = Shiftempdetails[i].ReportId;
                                emvm.ApproverId = Shiftempdetails[i].ReportId;
                                emvm.Approver = "";
                                if (emvm.ReportId != 0)
                                {
                                    emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                                }
                                emvm.EmpCode = Shiftempdetails[i].EmpCode; emvm.FirstName = Shiftempdetails[i].FirstName;
                                emvm.MiddleName = Shiftempdetails[i].MiddleName;
                                emvm.LastName = Shiftempdetails[i].LastName;
                                lstofEmp.Add(emvm);
                            }

                            for (int j = 0; j < nonShiftempdetails.Count(); j++)
                            {
                                ShiftEmployeeMasterViewModel emvm = new ShiftEmployeeMasterViewModel();
                                emvm.EmpId = nonShiftempdetails[j].EmpId;
                                emvm.OldEmp_ID = nonShiftempdetails[j].OldEmp_ID;
                                emvm.CompId = nonShiftempdetails[j].CompId;
                                emvm.Company = Company;
                                emvm.LEId = (nonShiftempdetails[j].LEId != 0) ? nonShiftempdetails[j].LEId : 0;
                                emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                                emvm.BUId = (nonShiftempdetails[j].BUId != 0) ? nonShiftempdetails[j].BUId : 0;
                                emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                                emvm.LocationId = (nonShiftempdetails[j].LocationId != 0) ? nonShiftempdetails[j].LocationId : 0;
                                emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                                emvm.ShiftId = (emvm.EmpId != 0) ? DB.EmpShiftDetails.Where(x => x.EmpId == emvm.EmpId && x.ShiftStatus == true && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftId).FirstOrDefault() : 0;
                                emvm.ShiftName = (emvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == emvm.ShiftId && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftName).FirstOrDefault() : "";
                                emvm.CategoryId = nonShiftempdetails[j].CategoryId;
                                emvm.DeptId = nonShiftempdetails[j].CategoryId;
                                emvm.DeptName = nonShiftempdetails[j].DeptName;
                                emvm.DesignationId = nonShiftempdetails[j].DesignationId;
                                emvm.Designation = nonShiftempdetails[j].DesignationName;
                                emvm.ReportId = nonShiftempdetails[j].ReportId;
                                emvm.ApproverId = nonShiftempdetails[j].ReportId;
                                emvm.Approver = "";
                                if (emvm.ReportId != 0)
                                {
                                    emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                                }
                                emvm.EmpCode = nonShiftempdetails[j].EmpCode; emvm.FirstName = nonShiftempdetails[j].FirstName;
                                emvm.MiddleName = nonShiftempdetails[j].MiddleName;
                                emvm.LastName = nonShiftempdetails[j].LastName;
                                lstofNonEmp.Add(emvm);
                            }

                            for (int k = 0; k < empdetails.Count(); k++)
                            {
                                ShiftEmployeeMasterViewModel emvm = new ShiftEmployeeMasterViewModel();
                                emvm.EmpId = empdetails[k].EmpId;
                                emvm.OldEmp_ID = empdetails[k].OldEmp_ID;
                                emvm.CompId = empdetails[k].CompId;
                                emvm.Company = Company;
                                emvm.LEId = (empdetails[k].LEId != 0) ? empdetails[k].LEId : 0;
                                emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                                emvm.BUId = (empdetails[k].BUId != 0) ? empdetails[k].BUId : 0;
                                emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                                emvm.LocationId = (empdetails[k].LocationId != 0) ? empdetails[k].LocationId : 0;
                                emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                                emvm.ShiftId = 0;
                                emvm.ShiftName = "";
                                emvm.CategoryId = empdetails[k].CategoryId;
                                emvm.DeptId = empdetails[k].CategoryId;
                                emvm.DeptName = empdetails[k].DeptName;
                                emvm.DesignationId = empdetails[k].DesignationId;
                                emvm.Designation = empdetails[k].DesignationName;
                                emvm.ReportId = empdetails[k].ReportId;
                                emvm.ApproverId = empdetails[k].ReportId;
                                emvm.Approver = "";
                                if (emvm.ReportId != 0)
                                {
                                    emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                                }
                                emvm.EmpCode = empdetails[k].EmpCode; emvm.FirstName = empdetails[k].FirstName;
                                emvm.MiddleName = empdetails[k].MiddleName;
                                emvm.LastName = empdetails[k].LastName;
                                lstofNonEmp.Add(emvm);
                            }

                            lstofShiftEmp.ShiftEmployee = lstofEmp;
                            lstofShiftEmp.NonShiftEmployee = lstofNonEmp;
                            return lstofShiftEmp;
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
                else if (compId != 0 && leId != 0 && buId != 0 && locationId != 0)
                {
                    string company = (compId != 0) ? DB.CompanyMasters.Where(x => x.CompId == compId).Select(x => x.Company).FirstOrDefault() : "";
                    string location = (locationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == locationId).Select(x => x.Location).FirstOrDefault() : "";
                    Company = company + " - " + location;


                    var Shiftempdetails = (from emp in DB.EmployeeMasters
                                           join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId
                                           join Comp in DB.CompanyMasters on emp.CompId equals Comp.CompId
                                           join LE in DB.LegalEntityMasters on emp.LEId equals LE.LEId
                                           where emp.CompId == compId && emp.LEId == leId && emp.BUId == buId && emp.LocationId == locationId &&
                                           esd.ShiftId == ShiftId && esd.ShiftStatus == true && esd.IsActive == true && esd.IsDeleted == false &&
                                           emp.IsActive == true && emp.IsDeleted == false
                                           select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();

                    var nonShiftempdetails = (from emp in DB.EmployeeMasters
                                              join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId
                                              join Comp in DB.CompanyMasters on emp.CompId equals Comp.CompId
                                              join LE in DB.LegalEntityMasters on emp.LEId equals LE.LEId
                                              where emp.CompId == compId && emp.LEId == leId && emp.BUId == buId && emp.LocationId == locationId &&
                                              esd.ShiftId != ShiftId && esd.ShiftStatus == true && esd.IsActive == true && esd.IsDeleted == false &&
                                              emp.IsActive == true && emp.IsDeleted == false
                                              select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();

                    var empdetails = (from emp in DB.EmployeeMasters
                                      join esd in DB.EmpShiftDetails on emp.EmpId equals esd.EmpId into shiftGroup
                                      from esd in shiftGroup.DefaultIfEmpty() // This performs a left join
                                      where emp.CompId == compId && emp.LEId == leId && emp.BUId == buId && emp.LocationId == locationId &&
                                      emp.IsActive == true && emp.IsDeleted == false
                                      && (esd == null) // || (esd.ShiftStatus == true && esd.IsActive == true && esd.IsDeleted == false))
                                      || (emp.CompId == compId && emp.LEId == leId && emp.BUId == buId && emp.LocationId == locationId &&
                                      esd.ShiftStatus == false && esd.Status == false && esd.IsActive == true && esd.IsDeleted == true)
                                      select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();

                    //if (empdetails.Count() == 0)
                    //{
                    //    empdetails = (from emp in DB.EmployeeMasters
                    //                  join esd in DB.EmpShiftDetails on emp.LEId equals esd.LEId into shiftGroup
                    //                  from esd in shiftGroup.DefaultIfEmpty() // This performs a left join
                    //                  where emp.CompId == compId && emp.LEId == leId && emp.BUId == buId && emp.LocationId == locationId &&
                    //                  emp.IsActive == true && emp.IsDeleted == false
                    //                  select emp).OrderByDescending(x => x.EmpId).Distinct().ToList();
                    //}

                    if (loginId != 0)
                    {
                        if (Shiftempdetails != null)
                        {
                            List<ShiftEmployeeMasterViewModel> lstofEmp = new List<ShiftEmployeeMasterViewModel>();
                            List<ShiftEmployeeMasterViewModel> lstofNonEmp = new List<ShiftEmployeeMasterViewModel>();

                            for (int i = 0; i < Shiftempdetails.Count(); i++)
                            {
                                ShiftEmployeeMasterViewModel emvm = new ShiftEmployeeMasterViewModel();
                                emvm.EmpId = Shiftempdetails[i].EmpId;
                                emvm.OldEmp_ID = Shiftempdetails[i].OldEmp_ID;
                                emvm.CompId = Shiftempdetails[i].CompId;
                                emvm.Company = Company;
                                emvm.LEId = (Shiftempdetails[i].LEId != 0) ? Shiftempdetails[i].LEId : 0;
                                emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                                emvm.BUId = (Shiftempdetails[i].BUId != 0) ? Shiftempdetails[i].BUId : 0;
                                emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                                emvm.LocationId = (Shiftempdetails[i].LocationId != 0) ? Shiftempdetails[i].LocationId : 0;
                                emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                                emvm.ShiftId = (emvm.EmpId != 0) ? DB.EmpShiftDetails.Where(x => x.EmpId == emvm.EmpId && x.ShiftStatus == true && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftId).FirstOrDefault() : 0;
                                emvm.ShiftName = (emvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == emvm.ShiftId && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftName).FirstOrDefault() : "";
                                emvm.CategoryId = Shiftempdetails[i].CategoryId;
                                emvm.DeptId = Shiftempdetails[i].CategoryId;
                                emvm.DeptName = Shiftempdetails[i].DeptName;
                                emvm.DesignationId = Shiftempdetails[i].DesignationId;
                                emvm.Designation = Shiftempdetails[i].DesignationName;
                                emvm.ReportId = Shiftempdetails[i].ReportId;
                                emvm.ApproverId = Shiftempdetails[i].ReportId;
                                emvm.Approver = "";
                                if (emvm.ReportId != 0)
                                {
                                    emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                                }
                                emvm.EmpCode = Shiftempdetails[i].EmpCode; emvm.FirstName = Shiftempdetails[i].FirstName;
                                emvm.MiddleName = Shiftempdetails[i].MiddleName;
                                emvm.LastName = Shiftempdetails[i].LastName;
                                lstofEmp.Add(emvm);
                            }

                            for (int j = 0; j < nonShiftempdetails.Count(); j++)
                            {
                                ShiftEmployeeMasterViewModel emvm = new ShiftEmployeeMasterViewModel();
                                emvm.EmpId = nonShiftempdetails[j].EmpId;
                                emvm.OldEmp_ID = nonShiftempdetails[j].OldEmp_ID;
                                emvm.CompId = nonShiftempdetails[j].CompId;
                                emvm.Company = Company;
                                emvm.LEId = (nonShiftempdetails[j].LEId != 0) ? nonShiftempdetails[j].LEId : 0;
                                emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                                emvm.BUId = (nonShiftempdetails[j].BUId != 0) ? nonShiftempdetails[j].BUId : 0;
                                emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                                emvm.LocationId = (nonShiftempdetails[j].LocationId != 0) ? nonShiftempdetails[j].LocationId : 0;
                                emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                                emvm.ShiftId = (emvm.EmpId != 0) ? DB.EmpShiftDetails.Where(x => x.EmpId == emvm.EmpId && x.ShiftStatus == true && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftId).FirstOrDefault() : 0;
                                emvm.ShiftName = (emvm.ShiftId != 0) ? DB.ShiftMasters.Where(x => x.ShiftId == emvm.ShiftId && x.IsActive == true && x.IsDeleted == false).Select(x => x.ShiftName).FirstOrDefault() : "";
                                emvm.CategoryId = nonShiftempdetails[j].CategoryId;
                                emvm.DeptId = nonShiftempdetails[j].CategoryId;
                                emvm.DeptName = nonShiftempdetails[j].DeptName;
                                emvm.DesignationId = nonShiftempdetails[j].DesignationId;
                                emvm.Designation = nonShiftempdetails[j].DesignationName;
                                emvm.ReportId = nonShiftempdetails[j].ReportId;
                                emvm.ApproverId = nonShiftempdetails[j].ReportId;
                                emvm.Approver = "";
                                if (emvm.ReportId != 0)
                                {
                                    emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                                }
                                emvm.EmpCode = nonShiftempdetails[j].EmpCode; emvm.FirstName = nonShiftempdetails[j].FirstName;
                                emvm.MiddleName = nonShiftempdetails[j].MiddleName;
                                emvm.LastName = nonShiftempdetails[j].LastName;
                                lstofNonEmp.Add(emvm);
                            }

                            for (int k = 0; k < empdetails.Count(); k++)
                            {
                                ShiftEmployeeMasterViewModel emvm = new ShiftEmployeeMasterViewModel();
                                emvm.EmpId = empdetails[k].EmpId;
                                emvm.OldEmp_ID = empdetails[k].OldEmp_ID;
                                emvm.CompId = empdetails[k].CompId;
                                emvm.Company = Company;
                                emvm.LEId = (empdetails[k].LEId != 0) ? empdetails[k].LEId : 0;
                                emvm.LegalEntity = (emvm.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == emvm.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                                emvm.BUId = (empdetails[k].BUId != 0) ? empdetails[k].BUId : 0;
                                emvm.BusinessUnit = (emvm.BUId != 0) ? DB.BusinessUnitMasters.Where(x => x.BUId == emvm.BUId).Select(x => x.BusinessUnit).FirstOrDefault() : "";
                                emvm.LocationId = (empdetails[k].LocationId != 0) ? empdetails[k].LocationId : 0;
                                emvm.Location = (emvm.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == emvm.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                                emvm.ShiftId = 0;
                                emvm.ShiftName = "";
                                emvm.CategoryId = empdetails[k].CategoryId;
                                emvm.DeptId = empdetails[k].CategoryId;
                                emvm.DeptName = empdetails[k].DeptName;
                                emvm.DesignationId = empdetails[k].DesignationId;
                                emvm.Designation = empdetails[k].DesignationName;
                                emvm.ReportId = empdetails[k].ReportId;
                                emvm.ApproverId = empdetails[k].ReportId;
                                emvm.Approver = "";
                                if (emvm.ReportId != 0)
                                {
                                    emvm.Approver = (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.FirstName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.MiddleName).FirstOrDefault()) + " " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.LastName).FirstOrDefault()) + " - " +
                                    (DB.EmployeeMasters.Where(x => x.EmpId == emvm.ReportId).Select(x => x.EmpCode).FirstOrDefault());
                                }
                                emvm.EmpCode = empdetails[k].EmpCode; emvm.FirstName = empdetails[k].FirstName;
                                emvm.MiddleName = empdetails[k].MiddleName;
                                emvm.LastName = empdetails[k].LastName;
                                lstofNonEmp.Add(emvm);
                            }

                            lstofShiftEmp.ShiftEmployee = lstofEmp;
                            lstofShiftEmp.NonShiftEmployee = lstofNonEmp;
                            return lstofShiftEmp;
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
                else
                {
                    Company = "";
                    throw new CustomApiException(HttpStatusCode.NotFound, "Company Details is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public NewResponseViewModel AddShiftEmployee(ShiftEmployeeMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? ShiftId = (model.ShiftId != 0) ? model.ShiftId : 0;


                compId = model.CompId;
                leId = model.LEId;
                buId = model.BUId;
                locationId = model.LocationId;

                DateTime today = DateTime.Now.Date;

                if (loginId != 0)
                {
                    if (model.EmpList.Count() != 0)
                    {
                        for (int k = 0; k < model.EmpList.Count(); k++)
                        {
                            int? empid = model.EmpList[k].EmpId;

                            var shiftempdetails = (from esd in DB.EmpShiftDetails
                                                   where esd.CompId == compId && esd.LEId == leId && esd.BUId == buId && esd.LocationId == locationId && esd.LocationId == locationId
                                                   && esd.EmpId == empid && esd.EndDate == null && esd.ShiftStatus == true && esd.IsActive == true && esd.IsDeleted == false
                                                   select esd).ToList();

                            if (shiftempdetails.Count() != 0)
                            {
                                for (int j = 0; j < shiftempdetails.Count(); j++)
                                {
                                    shiftempdetails[j].EndDate = today;
                                    shiftempdetails[j].ShiftStatus = false;
                                    shiftempdetails[j].Status = false;
                                    shiftempdetails[j].LastUpdatedBy = model.LoginId;
                                    shiftempdetails[j].LastUpdatedDate = DateTime.Now;
                                    shiftempdetails[j].IsActive = true;
                                    shiftempdetails[j].IsUpdated = true;
                                    shiftempdetails[j].IsDeleted = true;
                                    DB.SaveChanges();
                                }

                                EmpShiftDetail esdm = new EmpShiftDetail();
                                esdm.ShiftId = model.ShiftId;
                                esdm.ShiftName = model.ShiftName;
                                esdm.EmpId = model.EmpList[k].EmpId;
                                esdm.EmpCode = model.EmpList[k].EmpCode;
                                esdm.CompId = model.CompId;
                                esdm.LEId = model.LEId;
                                esdm.BUId = model.BUId;
                                esdm.LocationId = model.LocationId;
                                esdm.ShiftStatus = true;
                                esdm.Status = true;
                                esdm.StartDate = today;
                                esdm.CreatedBy = Convert.ToInt32(model.LoginId);
                                esdm.CreatedDate = DateTime.Now;
                                esdm.LastUpdatedBy = model.LoginId;
                                esdm.LastUpdatedDate = DateTime.Now;
                                esdm.IsActive = true;
                                esdm.IsUpdated = false;
                                esdm.IsDeleted = false;
                                DB.EmpShiftDetails.Add(esdm);
                                DB.SaveChanges();
                            }
                            else
                            {
                                EmpShiftDetail esdm = new EmpShiftDetail();
                                esdm.ShiftId = model.ShiftId;
                                esdm.ShiftName = model.ShiftName;
                                esdm.EmpId = model.EmpList[k].EmpId;
                                esdm.EmpCode = model.EmpList[k].EmpCode;
                                esdm.CompId = model.CompId;
                                esdm.LEId = model.LEId;
                                esdm.BUId = model.BUId;
                                esdm.LocationId = model.LocationId;
                                esdm.ShiftStatus = true;
                                esdm.Status = true;
                                esdm.StartDate = today;
                                esdm.CreatedBy = Convert.ToInt32(model.LoginId);
                                esdm.CreatedDate = DateTime.Now;
                                esdm.LastUpdatedBy = model.LoginId;
                                esdm.LastUpdatedDate = DateTime.Now;
                                esdm.IsActive = true;
                                esdm.IsUpdated = false;
                                esdm.IsDeleted = false;
                                DB.EmpShiftDetails.Add(esdm);
                                DB.SaveChanges();
                            }
                        }

                        NewResponseViewModel nrvm = new NewResponseViewModel();
                        nrvm.LoginId = loginId;
                        nrvm.msg = "Added";

                        return nrvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail is Missing");
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
        public NewResponseViewModel RemoveShiftEmployee(ShiftEmployeeMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? ShiftId = (model.ShiftId != 0) ? model.ShiftId : 0;


                compId = model.CompId;
                leId = model.LEId;
                buId = model.BUId;
                locationId = model.LocationId;

                DateTime today = DateTime.Now.Date;

                if (loginId != 0)
                {
                    if (model.EmpList.Count() != 0)
                    {
                        for (int k = 0; k < model.EmpList.Count(); k++)
                        {
                            int? empid = model.EmpList[k].EmpId;

                            var shiftempdetails = (from esd in DB.EmpShiftDetails
                                                   where esd.CompId == compId && esd.LEId == leId && esd.BUId == buId && esd.LocationId == locationId && esd.LocationId == locationId
                                                   && esd.EmpId == empid && esd.ShiftId == ShiftId && esd.EndDate == null && esd.ShiftStatus == true 
                                                   && esd.IsActive == true && esd.IsDeleted == false
                                                   select esd).ToList();

                            if (shiftempdetails.Count() != 0)
                            {
                                for (int j = 0; j < shiftempdetails.Count(); j++)
                                {
                                    shiftempdetails[j].EndDate = today;
                                    shiftempdetails[j].ShiftStatus = false;
                                    shiftempdetails[j].Status = false;
                                    shiftempdetails[j].LastUpdatedBy = model.LoginId;
                                    shiftempdetails[j].LastUpdatedDate = DateTime.Now;
                                    shiftempdetails[j].IsActive = true;
                                    shiftempdetails[j].IsUpdated = true;
                                    shiftempdetails[j].IsDeleted = true;
                                    DB.SaveChanges();
                                }
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail is Missing");
                            }
                        }

                        NewResponseViewModel nrvm = new NewResponseViewModel();
                        nrvm.LoginId = loginId;
                        nrvm.msg = "Removed";

                        return nrvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employees Detail is Missing");
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