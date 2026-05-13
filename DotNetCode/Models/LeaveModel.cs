using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.Services;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
 
namespace OfficeConnect_Web.Models
{
    public class LeaveModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();

        // ADD THIS: Notification Service
        private readonly INotificationService _notificationService;

        public List<LeaveTypeViewModel> GetAllLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.IsDeleted == false
                                  select lev).OrderByDescending(x => x.LeaveTypeId).ToList();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        List<LeaveTypeViewModel> lstoflvetype = new List<LeaveTypeViewModel>();

                        for (int i = 0; i < levdetails.Count(); i++)
                        {
                            LeaveTypeViewModel ltvm = new LeaveTypeViewModel();
                            ltvm.LeaveTypeId = levdetails[i].LeaveTypeId;
                            ltvm.LocationId = levdetails[i].LocationId;
                            string locationids = levdetails[i].LocationId ?? "";
                            var locIds = locationids.Split(',')
                                                    .Select(x => x.Trim())
                                                    .Where(x => !string.IsNullOrEmpty(x))
                                                    .Select(int.Parse)
                                                    .ToList();

                            var locationNames = DB.LocationMasters
                                                  .Where(l => locIds.Contains(l.LocationId) && l.IsActive == true && l.IsDeleted == false)
                                                  .Select(l => l.Location)
                                                  .ToList();

                            string LocationName = string.Join(", ", locationNames);
                            ltvm.Location = LocationName;
                            ltvm.YearType = levdetails[i].YearType;
                            ltvm.LeaveName = levdetails[i].LeaveName;
                            ltvm.ShortName = levdetails[i].ShortName;
                            ltvm.Description = levdetails[i].Description;
                            ltvm.DurationType = levdetails[i].DurationType;
                            ltvm.ApplicableTo = levdetails[i].ApplicableTo;
                            ltvm.EmpTypeId = levdetails[i].EmpTypeId;
                            string emptypeids = levdetails[i].EmpTypeId ?? "";
                            var emptyIds = emptypeids.Split(',')
                                                    .Select(x => x.Trim())
                                                    .Where(x => !string.IsNullOrEmpty(x))
                                                    .Select(int.Parse)
                                                    .ToList();

                            var emptypes = DB.EmpTypeMasters
                                                  .Where(l => emptyIds.Contains(l.EmpTypId) && l.IsActive == true && l.IsDeleted == false)
                                                  .Select(l => l.EmpType)
                                                  .ToList();

                            string EmpType = string.Join(", ", emptypes);
                            ltvm.EmpType = EmpType;
                            ltvm.EmpLevel = levdetails[i].EmpLevel;
                            ltvm.CarryForward = levdetails[i].CarryForward;
                            ltvm.Credit = levdetails[i].Credit;
                            ltvm.IsMonth = levdetails[i].IsMonth;
                            ltvm.IsYear = levdetails[i].IsYear;
                            ltvm.MaxCarryForward = levdetails[i].MaxCarryForward;
                            ltvm.ResetYear = levdetails[i].ResetYear;
                            ltvm.Encashable = levdetails[i].Encashable;
                            ltvm.MaxPerMonth = levdetails[i].MaxPerMonth;
                            ltvm.MaxPerYear = levdetails[i].MaxPerYear;
                            ltvm.MaxApply = levdetails[i].MaxApply;
                            ltvm.IsPaid = levdetails[i].IsPaid;
                            ltvm.ApplicableDuration = levdetails[i].ApplicableDuration;
                            ltvm.IsSingleApplication = levdetails[i].IsSingleApplication;
                            ltvm.MaxAllowedEvents = levdetails[i].MaxAllowedEvents;
                            ltvm.WeekEndInclusive = levdetails[i].WeekEndInclusive;
                            ltvm.CreatedBy = levdetails[i].CreatedBy;
                            ltvm.CreatedDate = levdetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = levdetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = levdetails[i].LastUpdatedDate;
                            ltvm.IsActive = levdetails[i].IsActive;
                            ltvm.IsUpdated = levdetails[i].IsUpdated;
                            ltvm.IsDeleted = levdetails[i].IsDeleted;
                            lstoflvetype.Add(ltvm);

                        }
                        return lstoflvetype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Not Found");
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
        public LeaveTypeViewModel GetLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveTypeId == model.LeaveTypeId && lev.IsDeleted == false
                                  select lev).OrderByDescending(x => x.LeaveTypeId).FirstOrDefault();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        LeaveTypeViewModel ltvm = new LeaveTypeViewModel();
                        ltvm.LeaveTypeId = levdetails.LeaveTypeId;
                        ltvm.LocationId = levdetails.LocationId;
                        string locationids = levdetails.LocationId ?? "";
                        var locIds = locationids.Split(',')
                                                .Select(x => x.Trim())
                                                .Where(x => !string.IsNullOrEmpty(x))
                                                .Select(int.Parse)
                                                .ToList();

                        var locationNames = DB.LocationMasters
                                              .Where(l => locIds.Contains(l.LocationId) && l.IsActive == true && l.IsDeleted == false)
                                              .Select(l => l.Location)
                                              .ToList();

                        string LocationName = string.Join(", ", locationNames);
                        ltvm.Location = LocationName;
                        ltvm.YearType = levdetails.YearType;
                        ltvm.LeaveName = levdetails.LeaveName;
                        ltvm.ShortName = levdetails.ShortName;
                        ltvm.Description = levdetails.Description;
                        ltvm.DurationType = levdetails.DurationType;
                        ltvm.ApplicableTo = levdetails.ApplicableTo;
                        ltvm.EmpTypeId = levdetails.EmpTypeId;
                        string emptypeids = levdetails.EmpTypeId ?? "";
                        var emptyIds = emptypeids.Split(',')
                                                .Select(x => x.Trim())
                                                .Where(x => !string.IsNullOrEmpty(x))
                                                .Select(int.Parse)
                                                .ToList();

                        var emptypes = DB.EmpTypeMasters
                                              .Where(l => emptyIds.Contains(l.EmpTypId) && l.IsActive == true && l.IsDeleted == false)
                                              .Select(l => l.EmpType)
                                              .ToList();

                        string EmpType = string.Join(", ", emptypes);
                        ltvm.EmpType = EmpType;
                        ltvm.EmpLevel = levdetails.EmpLevel;
                        ltvm.CarryForward = levdetails.CarryForward;
                        ltvm.Credit = levdetails.Credit;
                        ltvm.IsMonth = levdetails.IsMonth;
                        ltvm.IsYear = levdetails.IsYear;
                        ltvm.MaxCarryForward = levdetails.MaxCarryForward;
                        ltvm.ResetYear = levdetails.ResetYear;
                        ltvm.Encashable = levdetails.Encashable;
                        ltvm.MaxPerMonth = levdetails.MaxPerMonth;
                        ltvm.MaxPerYear = levdetails.MaxPerYear;
                        ltvm.MaxApply = levdetails.MaxApply;
                        ltvm.IsPaid = levdetails.IsPaid;
                        ltvm.ApplicableDuration = levdetails.ApplicableDuration;
                        ltvm.IsSingleApplication = levdetails.IsSingleApplication;
                        ltvm.MaxAllowedEvents = levdetails.MaxAllowedEvents;
                        ltvm.WeekEndInclusive = levdetails.WeekEndInclusive;
                        ltvm.CreatedBy = levdetails.CreatedBy;
                        ltvm.CreatedDate = levdetails.CreatedDate;
                        ltvm.LastUpdatedBy = levdetails.LastUpdatedBy;
                        ltvm.LastUpdatedDate = levdetails.LastUpdatedDate;
                        ltvm.IsActive = levdetails.IsActive;
                        ltvm.IsUpdated = levdetails.IsUpdated;
                        ltvm.IsDeleted = levdetails.IsDeleted;
                        return ltvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Not Found");
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
        public LeaveResponseViewModel AddLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveName == model.LeaveName
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).ToList();

                if (loginId != 0)
                {
                    if (levdetails.Count() == 0)
                    {
                        LeaveTypeMaster ltm = new LeaveTypeMaster();
                        //em.EmpId = model.modelId;
                        ltm.LocationId = model.LocationId;
                        ltm.YearType = model.YearType;
                        ltm.LeaveName = model.LeaveName;
                        ltm.ShortName = model.ShortName;
                        ltm.Description = model.Description;
                        ltm.DurationType = model.DurationType;
                        ltm.ApplicableTo = model.ApplicableTo;
                        ltm.EmpTypeId = model.EmpTypeId;
                        ltm.EmpLevel = model.EmpLevel;
                        ltm.CarryForward = model.CarryForward;
                        ltm.Credit = model.Credit;
                        ltm.IsMonth = model.IsMonth;
                        ltm.IsYear = model.IsYear;
                        ltm.MaxCarryForward = model.MaxCarryForward;
                        ltm.Encashable = model.Encashable;
                        ltm.MaxPerMonth = model.MaxPerMonth;
                        ltm.MaxPerYear = model.MaxPerYear;
                        ltm.MaxApply = model.MaxApply;
                        ltm.IsPaid = model.IsPaid;
                        ltm.ApplicableDuration = model.ApplicableDuration;
                        ltm.IsSingleApplication = model.IsSingleApplication;
                        ltm.MaxAllowedEvents = model.MaxAllowedEvents;
                        ltm.WeekEndInclusive = model.WeekEndInclusive;
                        ltm.ResetYear = model.ResetYear;
                        ltm.IsActive = true;
                        ltm.IsUpdated = false;
                        ltm.IsDeleted = false;
                        ltm.CreatedBy = model.LoginId;
                        ltm.CreatedDate = DateTime.Now;
                        ltm.LastUpdatedBy = model.LoginId;
                        ltm.LastUpdatedDate = DateTime.Now;
                        DB.LeaveTypeMasters.Add(ltm);
                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Already Exists");
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
        public LeaveResponseViewModel UpdateLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.LeaveTypeId != 0) ? model.LeaveTypeId : 0;

                var levdetails = (from acc in DB.LeaveTypeMasters
                                  where acc.LeaveTypeId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id != 0)
                    {
                        if (levdetails != null)
                        {
                            levdetails.LocationId = model.LocationId;
                            levdetails.YearType = model.YearType;
                            levdetails.LeaveName = model.LeaveName;
                            levdetails.ShortName = model.ShortName;
                            levdetails.Description = model.Description;
                            levdetails.DurationType = model.DurationType;
                            levdetails.ApplicableTo = model.ApplicableTo;
                            levdetails.EmpTypeId = model.EmpTypeId;
                            levdetails.EmpLevel = model.EmpLevel;
                            levdetails.CarryForward = model.CarryForward;
                            levdetails.Credit = model.Credit;
                            levdetails.IsMonth = model.IsMonth;
                            levdetails.IsYear = model.IsYear;
                            levdetails.MaxCarryForward = model.MaxCarryForward;
                            levdetails.Encashable = model.Encashable;
                            levdetails.MaxPerMonth = model.MaxPerMonth;
                            levdetails.MaxPerYear = model.MaxPerYear;
                            levdetails.MaxApply = model.MaxApply;
                            levdetails.IsPaid = model.IsPaid;
                            levdetails.ApplicableDuration = model.ApplicableDuration;
                            levdetails.IsSingleApplication = model.IsSingleApplication;
                            levdetails.MaxAllowedEvents = model.MaxAllowedEvents;
                            levdetails.WeekEndInclusive = model.WeekEndInclusive;
                            levdetails.ResetYear = model.ResetYear;
                            levdetails.IsActive = true;
                            levdetails.IsUpdated = true;
                            levdetails.IsDeleted = false;
                            levdetails.LastUpdatedBy = model.LoginId;
                            levdetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Id is Mismatching");
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
        public LeaveResponseViewModel DeleteLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.LeaveTypeId != 0) ? model.LeaveTypeId : 0;

                var levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveTypeId == id && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        levdetails.IsActive = true;
                        levdetails.IsUpdated = true;
                        levdetails.IsDeleted = true;
                        levdetails.LastUpdatedBy = model.LoginId;
                        levdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Not Found");
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
        public LeaveResponseViewModel ActivateLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.LeaveTypeId != 0) ? model.LeaveTypeId : 0;

                var levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveTypeId == id && lev.IsActive == false && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        levdetails.IsActive = true;
                        levdetails.IsUpdated = true;
                        levdetails.IsDeleted = false;
                        levdetails.LastUpdatedBy = model.LoginId;
                        levdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Activated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Not Found");
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
        public LeaveResponseViewModel DeactivateLeaveType(LeaveTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.LeaveTypeId != 0) ? model.LeaveTypeId : 0;

                var levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveTypeId == id && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        levdetails.IsActive = false;
                        levdetails.IsUpdated = true;
                        levdetails.IsDeleted = false;
                        levdetails.LastUpdatedBy = model.LoginId;
                        levdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deactivated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type Details Not Found");
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
        public List<DDLeaveTypeViewModel> GetDDLeaveType(DDLeaveTypePayloadViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                int? year = DateTime.Now.Year;
                DateTime Today = DateTime.Now;

                int compid = DB.EmployeeMasters.Where(x => x.EmpId == LoginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.CompId).FirstOrDefault() ?? 0;

                int leid = DB.EmployeeMasters.Where(x => x.EmpId == LoginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LEId).FirstOrDefault() ?? 0;

                int locationid = DB.EmployeeMasters.Where(x => x.EmpId == LoginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LocationId).FirstOrDefault() ?? 0;

                if (locationid == 0)
                {
                    locationid = 4;
                }

                var JoiningDate = (from emp in DB.EmployeeMasters
                                   where emp.EmpId == LoginId && emp.IsActive == true && emp.IsDeleted == false
                                   select emp.JoiningDate).FirstOrDefault();

                string gender = DB.EmployeeMasters.Where(x => x.EmpId == LoginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.Gender).FirstOrDefault() ?? "";

                // Get marital status from employee master
                var maritalStatus = (from emp in DB.EmployeeMasters
                                     where emp.EmpId == LoginId && emp.IsActive == true && emp.IsDeleted == false
                                     select emp.MaritalStatus).FirstOrDefault();

                bool hasCompletedOneYear = false;
                bool isEligibleForCLThisMonth = true;   // default true


                if (JoiningDate != null)
                {
                    var difference = DateTime.Now - JoiningDate.Value;
                    hasCompletedOneYear = difference.TotalDays >= 365;

                    // Check if current month == joining month & current year == joining year
                    if (JoiningDate.Value.Year == DateTime.Now.Year &&
                        JoiningDate.Value.Month == DateTime.Now.Month)
                    {
                        // If joined after 15th → not eligible for CL for this month
                        if (JoiningDate.Value.Day > 15)
                        {
                            isEligibleForCLThisMonth = false;
                        }
                    }
                }

                var RHdetails = (from hol in DB.Holidays
                                 where hol.Year == year && hol.LocationId == locationid
                                 && hol.Date >= model.StartDate && hol.Date <= model.EndDate && hol.Status == "Active"
                                 && hol.HolidayType == "RH Holidays"
                                 select hol).Count();

                if (leid == 2)
                {
                    RHdetails = (from hol in DB.Holidays
                                 where hol.Year == year && hol.LocationId == 4
                                 && hol.Date >= model.StartDate && hol.Date <= model.EndDate && hol.Status == "Active"
                                 && hol.HolidayType == "RH Holidays"
                                 select hol).Count();
                }

                var Levdetails = (from lev in DB.LeaveTypeMasters
                                   where lev.LocationId.Contains(locationid.ToString()) && 
                                   lev.ApplicableTo.ToUpper() == "ALL" &&
                                   lev.IsActive == true && lev.IsDeleted == false 
                                  select new DDLeaveTypeViewModel
                                   {
                                       LeaveTypeId = lev.LeaveTypeId,
                                       LeaveType = lev.LeaveName + " - (" + lev.ShortName + ")",
                                       ShortName = lev.ShortName,
                                   }).ToList();

                var Levdetails1 = (from lev in DB.LeaveTypeMasters
                                   where lev.LocationId.Contains(locationid.ToString()) &&
                                   lev.ApplicableTo.ToUpper() == gender.ToUpper() &&
                                   lev.IsActive == true && lev.IsDeleted == false
                                   select new DDLeaveTypeViewModel
                                   {
                                       LeaveTypeId = lev.LeaveTypeId,
                                       LeaveType = lev.LeaveName + " - (" + lev.ShortName + ")",
                                       ShortName = lev.ShortName,
                                   }).ToList();

                if (locationid == 0)
                {
                    int? loctn = DB.LocationMasters.Where(x => x.Location.ToUpper() == "BANGALORE" && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LocationId).FirstOrDefault();

                    Levdetails = (from lev in DB.LeaveTypeMasters
                                  where lev.LocationId.Contains(loctn.ToString()) &&
                                  lev.ApplicableTo.ToUpper() == "ALL" &&
                                  lev.IsActive == true && lev.IsDeleted == false
                                  select new DDLeaveTypeViewModel
                                  {
                                      LeaveTypeId = lev.LeaveTypeId,
                                      LeaveType = lev.LeaveName + " - (" + lev.ShortName + ")",
                                      ShortName = lev.ShortName,
                                  }).ToList();

                    Levdetails1 = (from lev in DB.LeaveTypeMasters
                                   where lev.LocationId.Contains(loctn.ToString()) &&
                                   lev.ApplicableTo.ToUpper() == gender.ToUpper() &&
                                   lev.IsActive == true && lev.IsDeleted == false
                                   select new DDLeaveTypeViewModel
                                   {
                                       LeaveTypeId = lev.LeaveTypeId,
                                       LeaveType = lev.LeaveName + " - (" + lev.ShortName + ")",
                                       ShortName = lev.ShortName,
                                   }).ToList();
                }

                if (Levdetails1.Count() > 0)
                {
                    // Merge lists
                    Levdetails.AddRange(Levdetails1);
                }

                // Filter CL if not eligible
                if (isEligibleForCLThisMonth != true)
                {
                    Levdetails = Levdetails.Where(x => x.ShortName != "CL").ToList(); // exclude CL

                }

                // ✅ Apply your conditions
                if (hasCompletedOneYear != true)
                {
                    // Remove EL if employee has not completed 1 year
                    Levdetails = Levdetails.Where(x => x.ShortName != "EL").ToList();
                }

                if (RHdetails == 0)
                {
                    // Remove RH if no RH holidays available
                    Levdetails = Levdetails.Where(x => x.ShortName != "RH").ToList();
                }

                ////// ✅ Remove ML if not married
                ////if (maritalStatus == null || !maritalStatus.Equals("Married", StringComparison.OrdinalIgnoreCase))
                ////{
                ////    if (gender.ToUpper() == "MALE")
                ////    {
                ////        Levdetails = Levdetails.Where(x => x.ShortName != "ML").ToList();
                ////    }
                ////    else if (gender.ToUpper() == "FEMALE")
                ////    {
                ////        Levdetails = Levdetails.Where(x => x.ShortName != "PL").ToList();
                ////    }
                ////}

                if (EmpId != 0)
                {
                    if (Levdetails != null)
                    {
                        return Levdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "DD Leave Type Details Not Found");
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
        public List<DDComOffManager> DDApproveManager(DDComOffManager model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? HRId = 149;

                int? reportid = (from comp in DB.EmployeeMasters
                                where comp.EmpId == LoginId 
                                && comp.IsActive == true && comp.IsDeleted == false
                                select comp.ReportId).FirstOrDefault();

                if (reportid != null)
                {
                    var DDComOffdetails = (from user in DB.EmployeeMasters
                                           where user.IsActive == true && user.IsDeleted == false && user.EmpId == reportid
                                           select new DDComOffManager
                                           {
                                               LoginId = LoginId,
                                               ManagerId = user.EmpId,
                                               ManagerName = user.FirstName,
                                               ManagerCode = user.EmpCode,
                                           }).ToList();

                    if (LoginId != 0)
                    {
                        if (DDComOffdetails != null)
                        {
                            return DDComOffdetails;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Approver Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                    }
                }
                else
                {
                    var DDComOffdetails = (from user in DB.EmployeeMasters
                                           where user.IsActive == true && user.IsDeleted == false && user.EmpId == HRId
                                           select new DDComOffManager
                                           {
                                               LoginId = LoginId,
                                               ManagerId = user.EmpId,
                                               ManagerName = user.FirstName,
                                               ManagerCode = user.EmpCode,
                                           }).ToList();

                    if (LoginId != 0)
                    {
                        if (DDComOffdetails != null)
                        {
                            return DDComOffdetails;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Approver Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public LeaveResponseViewModel CompOffLeave(CompOffRequestViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                DateTime Today = DateTime.Now; //more than 10 days
                int? Year = Today.Year;
                int? Month = Today.Month;

                string empcode = (from emp in DB.EmployeeMasters 
                                  where emp.EmpId == model.EmpId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                  && emp.IsActive == true && emp.IsDeleted == false
                                  select emp.EmpCode).FirstOrDefault();

                var CompOffdetails = (from comp in DB.CompOffRequests
                                      where comp.EmpId == model.EmpId && comp.Date == model.Date 
                                      && comp.IsActive == true && comp.IsDeleted == false
                                      select comp).ToList();

                int? ReportId = (from emp in DB.EmployeeMasters
                                 where emp.EmpId == model.EmpId
                                    && emp.IsActive == true
                                    && emp.IsDeleted == false
                                 select emp.ReportId).FirstOrDefault() ?? 0;

                ////var esslatt = (from essl in DB.Emp_AttendanceTime
                ////               where essl.EmpCode.ToUpper() == empcode && essl.Date == model.Date
                ////               && essl.IsActive == true && essl.IsDeleted == false
                ////               select essl).ToList();

                ////var wfhatt = (from wfh in DB.CompOffRequests
                ////              where wfh.EmpId == model.EmpId && wfh.Date == model.Date
                ////              && wfh.IsActive == true && wfh.IsDeleted == false
                ////              select wfh).ToList();

                ////var onsiteatt = (from onsite in DB.CompOffRequests
                ////                 where onsite.EmpId == model.EmpId && onsite.Date == model.Date
                ////                 && onsite.IsActive == true && onsite.IsDeleted == false
                ////                 select onsite).ToList();

                int? HRId = 149;

                if (loginId != 0)
                {
                    if (CompOffdetails.Count() == 0)
                    {
                        CompOffRequest cor = new CompOffRequest();
                        //em.EmpId = model.modelId;
                        cor.EmpId = model.EmpId;
                        cor.EmpCode = model.EmpCode;
                        cor.ManagerId = model.ManagerId;
                        cor.ManagerCode = model.ManagerCode;
                        cor.Date = model.Date;
                        cor.ProjectId = 0;
                        cor.Project = model.Project;
                        cor.TaskId = 0;
                        cor.Task = model.Task;
                        cor.Hrs = model.Hrs;
                        cor.ActualHrs = model.ActualHrs;
                        cor.WorkMode = model.WorkMode;
                        cor.IsRequested = true;
                        cor.IsApproved = false;
                        cor.IsRejected = false;
                        cor.IsUsed = false;
                        cor.IsActive = true;
                        cor.IsUpdated = false;
                        cor.IsDeleted = false;
                        cor.CreatedBy = model.LoginId;
                        cor.CreatedDate = DateTime.Now;
                        cor.LastUpdatedBy = model.LoginId;
                        cor.LastUpdatedDate = DateTime.Now;
                        DB.CompOffRequests.Add(cor);
                        DB.SaveChanges();

                        // ========== NEW: ADD NOTIFICATION ==========
                        Task.Run(async () =>
                        {
                            await _notificationService.CreateCompOffAppliedNotification(cor.CompOffReqId, model.EmpId ?? 0);
                        });

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "CompOff Requested";

                        return emvm;
                    }
                    else
                    {
                        var CompOffdetails1  = (from comp in DB.CompOffRequests
                                                where comp.EmpId == model.EmpId && comp.Date == model.Date && comp.IsRequested == true
                                                && comp.IsActive == true && comp.IsDeleted == false
                                                select comp).FirstOrDefault();

                        if (CompOffdetails != null)
                        {
                            if (CompOffdetails1.IsApproved != true && CompOffdetails1.IsRejected != true)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "A CompOff request for this date is already pending approval.");
                            }
                            else if (CompOffdetails1.IsApproved == true && CompOffdetails1.IsRejected != true)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "A CompOff request for this date has already been approved.");
                            }
                            else if (CompOffdetails1.IsApproved != true && CompOffdetails1.IsRejected == true)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "A CompOff request for this date has already been rejected.");
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "A CompOff record already exists for this date.");
                            }
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "A CompOff record already exists for this date.");
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
        public CompOffHoursRequestViewModel CompOffHours(CompOffRequestViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                DateTime Today = DateTime.Now; //more than 10 days
                int? Year = Today.Year;
                int? Month = Today.Month;
                string activeHours = "00:00:00";
                string workmode = "";

                string empcode = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == model.EmpId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                  && emp.IsActive == true && emp.IsDeleted == false
                                  select emp.EmpCode).FirstOrDefault();

                var ESSL = (from essl in DB.Emp_AttendanceTime
                            where essl.EmpCode.ToUpper() == model.EmpCode.ToUpper() && essl.LogDate == model.Date
                            select essl).FirstOrDefault();

                var WFH = (from wfh in DB.WFHLoginlogs
                           where wfh.EmpCode.ToUpper() == model.EmpCode.ToUpper() && wfh.Date == model.Date
                           select wfh).OrderBy(x => x.LoginTime).ToList();

                var Onsite = (from onsite in DB.OnSiteLoginlogs
                               where onsite.EmpCode.ToUpper() == model.EmpCode.ToUpper() && onsite.ActiveHrs != null && onsite.LoginDate == model.Date
                               select onsite).ToList();

                if (ESSL != null)
                {
                    if (ESSL.Duration.HasValue)
                    {
                        activeHours = ((DateTime)ESSL.Duration).ToString("HH:mm:ss");
                        workmode = "ESSL";
                    }
                }
                if (WFH.Count() > 0)
                {

                    TimeSpan totalWfhActiveHours = TimeSpan.Zero;
                    TimeSpan defaultLogout = new TimeSpan(18, 35, 0);

                    TimeSpan? firstLogin = null;
                    TimeSpan? lastLogout = null;

                    if (WFH.Any())
                    {
                        for (int i = 0; i < WFH.Count; i++)
                        {
                            var entry = WFH[i];

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
                            else if (i + 1 < WFH.Count && WFH[i + 1].LoginTime.HasValue)
                            {
                                logOut = WFH[i + 1].LoginTime.Value;
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
                        activeHours = totalWfhActiveHours.ToString(@"hh\:mm\:ss");
                        workmode = "WFH";
                    }
                }
                if (Onsite.Count() > 0)
                {

                    TimeSpan totalOnsiteActiveHours = TimeSpan.Zero;
                    TimeSpan OnsitedefaultLogout = new TimeSpan(18, 35, 0);

                    TimeSpan? OnsitefirstLogin = null;
                    TimeSpan? OnsitelastLogout = null;

                    if (Onsite.Any())
                    {
                        for (int i = 0; i < Onsite.Count; i++)
                        {
                            var entry = Onsite[i];

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
                            else if (i + 1 < Onsite.Count && Onsite[i + 1].LogInTime.HasValue)
                            {
                                logOut = Onsite[i + 1].LogInTime.Value;
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
                        activeHours = totalOnsiteActiveHours.ToString(@"hh\:mm\:ss");
                        workmode = "ONSITE";
                    }
                }
                CompOffHoursRequestViewModel cohrvm = new CompOffHoursRequestViewModel();
                cohrvm.LoginId = model.LoginId;
                cohrvm.EmpId = model.LoginId;
                cohrvm.EmpCode = model.EmpCode;
                cohrvm.ActualHrs = activeHours;
                cohrvm.WorkMode = workmode;

                return cohrvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<CompOffRequestViewModel> GetAllEmpCompOffLeave(CompOffRequestViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                ////var allcompoffdetails = (from comp in DB.CompOffRequests
                ////                         where comp.EmpId == loginId && comp.IsActive == true && comp.IsDeleted == false
                ////                         select comp).OrderByDescending(x => x.CreatedDate).ToList();

                var allcompoffdetails = DB.CompOffRequests
                                        .Where(comp => comp.EmpId == loginId && comp.IsActive == true && comp.IsDeleted == false)
                                        .OrderByDescending(x =>
                                            x.IsRequested == true &&
                                            x.IsApproved == false &&
                                            x.IsRejected == false) // bring requested-but-not-approved/rejected first
                                        .ThenByDescending(x => x.CreatedDate)
                                        .ToList();


                if (loginId != 0)
                {
                    if (allcompoffdetails != null)
                    {
                        List<CompOffRequestViewModel> lstofcompall = new List<CompOffRequestViewModel>();

                        for (int i = 0; i < allcompoffdetails.Count(); i++)
                        {
                            CompOffRequestViewModel corvm = new CompOffRequestViewModel();
                            corvm.CompOffReqId = allcompoffdetails[i].CompOffReqId;
                            corvm.EmpId = allcompoffdetails[i].EmpId;
                            corvm.EmpCode = allcompoffdetails[i].EmpCode;
                            corvm.ManagerId = allcompoffdetails[i].ManagerId;
                            corvm.ManagerCode = allcompoffdetails[i].ManagerCode;
                            corvm.Date = allcompoffdetails[i].Date;
                            corvm.ProjectId = allcompoffdetails[i].ProjectId;
                            corvm.Project = allcompoffdetails[i].Project;
                            corvm.TaskId = allcompoffdetails[i].TaskId;
                            corvm.Task = allcompoffdetails[i].Task;
                            corvm.Hrs = allcompoffdetails[i].Hrs;
                            corvm.ActualHrs = allcompoffdetails[i].ActualHrs;
                            corvm.WorkMode = allcompoffdetails[i].WorkMode;
                            corvm.IsRequested = allcompoffdetails[i].IsRequested;
                            corvm.IsApproved = allcompoffdetails[i].IsApproved;
                            corvm.IsRejected = allcompoffdetails[i].IsRejected;
                            corvm.IsUsed = allcompoffdetails[i].IsUsed;
                            corvm.IsActive = allcompoffdetails[i].IsActive;
                            corvm.IsUpdated = allcompoffdetails[i].IsUpdated;
                            corvm.IsDeleted = allcompoffdetails[i].IsDeleted;
                            corvm.CreatedBy = allcompoffdetails[i].CreatedBy;
                            corvm.CreatedDate = allcompoffdetails[i].CreatedDate;
                            corvm.LastUpdatedBy = allcompoffdetails[i].LastUpdatedBy;
                            corvm.LastUpdatedDate = allcompoffdetails[i].LastUpdatedDate;
                            lstofcompall.Add(corvm);
                        }
                        return lstofcompall;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        public List<CompOffRequestViewModel> GetAllCompOffLeave(CompOffRequestViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                ////var allcompoffdetails = (from comp in DB.CompOffRequests
                ////                         where comp.ManagerId == loginId && comp.IsRequested == true && comp.IsActive == true && comp.IsDeleted == false
                ////                         select comp).OrderByDescending(x => x.CreatedDate).ToList();

                var allcompoffdetails = DB.CompOffRequests
                                        .Where(comp => comp.ManagerId == loginId && comp.IsRequested == true && comp.IsActive == true && comp.IsDeleted == false)
                                        .OrderByDescending(x =>
                                            x.IsRequested == true &&
                                            x.IsApproved == false &&
                                            x.IsRejected == false) // bring requested-but-not-approved/rejected first
                                        .ThenByDescending(x => x.CreatedDate)
                                        .ToList();

                string dept = (from user in DB.EmployeeMasters 
                               join dep in DB.DeptMasters on user.CategoryId equals dep.DeptId
                               where user.EmpId == loginId && user.IsActive == true && user.IsDeleted == false //&& user.EmpStatus.ToUpper() == 'ACTIVE'
                               && dep.IsActive == true && dep.IsDeleted == false
                               select dep.DeptShortName).FirstOrDefault();

                if (dept.ToUpper() == "HR")
                {
                    ////allcompoffdetails = (from comp in DB.CompOffRequests
                    ////                     where comp.IsRequested == true && comp.IsActive == true && comp.IsDeleted == false
                    ////                     select comp).OrderByDescending(x => x.CreatedDate).ToList();

                    allcompoffdetails = DB.CompOffRequests
                                        .Where(comp => comp.IsRequested == true && comp.IsActive == true && comp.IsDeleted == false)
                                        .OrderByDescending(x =>
                                            x.IsRequested == true &&
                                            x.IsApproved == false &&
                                            x.IsRejected == false) // bring requested-but-not-approved/rejected first
                                        .ThenByDescending(x => x.CreatedDate)
                                        .ToList();
                }

                if (loginId != 0)
                {
                    if (allcompoffdetails != null)
                    {
                        List<CompOffRequestViewModel> lstofcompall = new List<CompOffRequestViewModel>();

                        for (int i = 0; i < allcompoffdetails.Count(); i++)
                        {
                            CompOffRequestViewModel corvm = new CompOffRequestViewModel();
                            corvm.CompOffReqId = allcompoffdetails[i].CompOffReqId;
                            corvm.EmpId = allcompoffdetails[i].EmpId;
                            int? empid = allcompoffdetails[i].EmpId;
                            corvm.EmpCode = allcompoffdetails[i].EmpCode;
                            corvm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.FirstName).FirstOrDefault() + " " +
                                DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.MiddleName).FirstOrDefault() + " " +
                                DB.EmployeeMasters.Where(x => x.EmpId == empid).Select(x => x.LastName).FirstOrDefault();
                            corvm.ManagerId = allcompoffdetails[i].ManagerId;
                            corvm.ManagerCode = allcompoffdetails[i].ManagerCode;
                            corvm.Date = allcompoffdetails[i].Date;
                            corvm.ProjectId = allcompoffdetails[i].ProjectId;
                            corvm.Project = allcompoffdetails[i].Project;
                            corvm.TaskId = allcompoffdetails[i].TaskId;
                            corvm.Task = allcompoffdetails[i].Task;
                            corvm.Hrs = allcompoffdetails[i].Hrs;
                            corvm.ActualHrs = allcompoffdetails[i].ActualHrs;
                            corvm.WorkMode = allcompoffdetails[i].WorkMode;
                            corvm.IsRequested = allcompoffdetails[i].IsRequested;
                            corvm.IsApproved = allcompoffdetails[i].IsApproved;
                            corvm.IsRejected = allcompoffdetails[i].IsRejected;
                            corvm.IsUsed = allcompoffdetails[i].IsUsed;
                            corvm.IsActive = allcompoffdetails[i].IsActive;
                            corvm.IsUpdated = allcompoffdetails[i].IsUpdated;
                            corvm.IsDeleted = allcompoffdetails[i].IsDeleted;
                            corvm.CreatedBy = allcompoffdetails[i].CreatedBy;
                            corvm.CreatedDate = allcompoffdetails[i].CreatedDate;
                            corvm.LastUpdatedBy = allcompoffdetails[i].LastUpdatedBy;
                            corvm.LastUpdatedDate = allcompoffdetails[i].LastUpdatedDate;
                            lstofcompall.Add(corvm);
                        }
                        return lstofcompall;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        public LeaveResponseViewModel ApproveCompOff(ApproveCompOffViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var levtypedetails1 = (from lev in DB.LeaveTypeMasters
                                      where lev.ShortName.ToUpper() == "COMP OFF" && lev.IsActive == true && lev.IsDeleted == false
                                      select lev).FirstOrDefault();

                if (levtypedetails1 == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave Type - COMP OFF is Not Found");
                }

                if (loginId != 0)
                {
                    if (model.lstofCompOffReqId.Count() > 0)
                    {
                        for (int i = 0; i < model.lstofCompOffReqId.Count; i++)
                        {
                            var compoffreqid = model.lstofCompOffReqId[i].CompOffReqId;

                            var compoffdetails = (from lev in DB.CompOffRequests
                                                  where lev.CompOffReqId == compoffreqid && lev.IsRequested == true && lev.IsApproved == false && lev.IsRejected == false
                                                  && lev.IsActive == true && lev.IsDeleted == false
                                                  select lev).FirstOrDefault();

                            if (compoffdetails != null)
                            {
                                compoffdetails.Reason = model.lstofCompOffReqId[i].Remarks;
                                compoffdetails.IsApproved = true;
                                compoffdetails.IsRejected = false;
                                compoffdetails.IsActive = true;
                                compoffdetails.IsUpdated = true;
                                compoffdetails.IsDeleted = false;
                                compoffdetails.LastUpdatedBy = model.LoginId;
                                compoffdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                DateTime Today = DateTime.Now; //more than 10 days
                                int? Year = Today.Year;
                                int? Month = Today.Month;

                                var levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.ShortName.ToUpper() == "COMP OFF" && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                int? leavetypeid = levtypedetails.LeaveTypeId;

                                int? userid = compoffdetails.EmpId;

                                var Carrydetails = (from lev in DB.LeaveCarryForwardMasters
                                                      where lev.LeaveTypeId == leavetypeid && lev.EmpId == userid && lev.LeaveYear == Year
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                if (Carrydetails != null)
                                {
                                    decimal? openbal = (Carrydetails.OpeningBalance + 1);
                                    decimal? closebal = (Carrydetails.ClosingBalance + 1);
                                    decimal? avail = (Carrydetails.Availed);
                                    Carrydetails.OpeningBalance = openbal;
                                    Carrydetails.Availed = avail;
                                    Carrydetails.CarryForward = Convert.ToDecimal(0.00);
                                    Carrydetails.Encashment = Convert.ToDecimal(0.00);
                                    Carrydetails.ClosingBalance = closebal;
                                    Carrydetails.LastUpdatedBy = model.LoginId; ;
                                    Carrydetails.LastUpdatedDate = DateTime.Now;
                                    Carrydetails.IsActive = true;
                                    Carrydetails.IsUpdated = true;
                                    Carrydetails.IsDeleted = false;
                                    DB.SaveChanges();
                                }
                                else
                                {
                                    LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster();
                                    cf.EmpId = userid;
                                    cf.EmpCode = DB.EmployeeMasters.Where(x => x.EmpId == userid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.UserName).FirstOrDefault();
                                    cf.LeaveTypeId = leavetypeid;
                                    cf.LeaveMonth = Month;
                                    cf.LeaveYear = Year;
                                    cf.OpeningBalance = 1;
                                    cf.Availed = Convert.ToDecimal(0.00);
                                    cf.CarryForward = Convert.ToDecimal(0.00);
                                    cf.Encashment = Convert.ToDecimal(0.00);
                                    cf.ClosingBalance = 1;
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

                                var managerdetails = (from lev in DB.EmployeeMasters
                                                      where lev.EmpId == model.EmpId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();


                                var userdetails = (from lev in DB.EmployeeMasters
                                                   where lev.EmpId == userid
                                                   && lev.IsActive == true && lev.IsDeleted == false
                                                   select lev).FirstOrDefault();

                                var actualHRdetails = (from lev in DB.EmailConfigMasters
                                                       where lev.Name.ToUpper() == "LEAVE"
                                                       && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                                       && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                                       && lev.IsActive == true && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                var HRuserdetails = (from lev in DB.EmployeeMasters
                                                     where lev.EmpId == 149
                                                     && lev.IsActive == true && lev.IsDeleted == false
                                                     select lev).FirstOrDefault();

                                string HRmailid = "";

                                ////if (actualHRdetails != null)
                                ////{
                                ////    HRmailid = actualHRdetails.EmailId;
                                ////}
                                ////else
                                ////{
                                ////    HRmailid = HRuserdetails.EmailId;
                                ////}

                                string date = compoffdetails.Date?.ToString("yyyy-MM-dd");

                                string to = userdetails.EmailId;
                                string cc = HRmailid;
                                string subject = "Office Connect - CompOff Update";
                                string body = $@"
                                <p>Dear {userdetails.FirstName},</p>
                                <p>Your Comp Off request submitted on <strong>{date}</strong> for <strong>{compoffdetails.Hrs}</strong> has been Approved by your manager (<strong>{managerdetails.FirstName}</strong>).</p>
                                <p></p>
                                <p>You can view the updated status of your application in the Office Connect portal.</p>
                                <p></p>
                                <p>Best regards,</p>
                                <p>Office Connect.</p>";

                                Task.Run(() => SendLeaveMail(to, cc, subject, body));

                                // ========== NEW: ADD NOTIFICATION ==========
                                Task.Run(async () =>
                                {
                                    await _notificationService.CreateCompOffApprovedNotification(compoffreqid, model.LoginId);
                                });
                            }
                            ////else
                            ////{
                            ////    throw new CustomApiException(HttpStatusCode.NotFound, "Applied CompOff Details Not Found");
                            ////}
                        }
                    }

                    LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                    emvm.Status = 200;
                    emvm.msg = "Approved";

                    return emvm;
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
        public LeaveResponseViewModel RejectCompOff(ApproveCompOffViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                if (loginId != 0)
                {
                    if (model.lstofCompOffReqId.Count() > 0)
                    {
                        for (int i = 0; i < model.lstofCompOffReqId.Count; i++)
                        {
                            var compoffreqid = model.lstofCompOffReqId[i].CompOffReqId;

                            var compoffdetails = (from lev in DB.CompOffRequests
                                                  where lev.CompOffReqId == compoffreqid && lev.IsRequested == true && lev.IsApproved == false && lev.IsRejected == false
                                                  && lev.IsActive == true && lev.IsDeleted == false
                                                  select lev).FirstOrDefault();

                            if (compoffdetails != null)
                            {
                                compoffdetails.Reason = model.lstofCompOffReqId[i].Remarks;
                                compoffdetails.IsApproved = false;
                                compoffdetails.IsRejected = true;
                                compoffdetails.IsActive = true;
                                compoffdetails.IsUpdated = true;
                                compoffdetails.IsDeleted = false;
                                compoffdetails.LastUpdatedBy = model.LoginId;
                                compoffdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                var managerdetails = (from lev in DB.EmployeeMasters
                                                      where lev.EmpId == model.EmpId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                var userdetails = (from lev in DB.EmployeeMasters
                                                   where lev.EmpId == loginId
                                                   && lev.IsActive == true && lev.IsDeleted == false
                                                   select lev).FirstOrDefault();

                                var actualHRdetails = (from lev in DB.EmailConfigMasters
                                                       where lev.Name.ToUpper() == "LEAVE"
                                                       && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                                       && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                                       && lev.IsActive == true && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                var HRuserdetails = (from lev in DB.EmployeeMasters
                                                     where lev.EmpId == 149
                                                     && lev.IsActive == true && lev.IsDeleted == false
                                                     select lev).FirstOrDefault();

                                string HRmailid = "";

                                ////if (actualHRdetails != null)
                                ////{
                                ////    HRmailid = actualHRdetails.EmailId;
                                ////}
                                ////else
                                ////{
                                ////    HRmailid = HRuserdetails.EmailId;
                                ////}

                                string date = compoffdetails.Date?.ToString("yyyy-MM-dd");

                                string to = userdetails.EmailId;
                                string cc = HRmailid;
                                string subject = "Office Connect - CompOff Update";
                                string body = $@"
                                <p>Dear {userdetails.FirstName},</p>
                                <p>Your Comp Off request submitted on <strong>{date}</strong> for <strong>{compoffdetails.Hrs}</strong> has been Rejected by your manager (<strong>{managerdetails.FirstName}</strong>).</p>
                                <p></p>
                                <p>You can view the updated status of your application in the Office Connect portal.</p>
                                <p></p>
                                <p>Best regards,</p>
                                <p>Office Connect.</p>";

                                Task.Run(() => SendLeaveMail(to, cc, subject, body));

                                // ========== NEW: ADD NOTIFICATION ==========
                                Task.Run(async () =>
                                {
                                    await _notificationService.CreateCompOffRejectedNotification(compoffreqid, model.LoginId);
                                });
                            }
                            else
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Applied CompOff Details Not Found");
                            }
                        }
                    }
                    LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                    emvm.Status = 200;
                    emvm.msg = "Rejected";

                    return emvm;
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
        public LeaveResponseViewModel DraftLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LeaveTypeId = (model.EmpId != 0) ? model.EmpId : 0;

                DateTime Today = DateTime.Now; //more than 10 days
                int? Year = Today.Year;
                int? Month = Today.Month;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.EmpId == model.EmpId && lev.StartDate == model.StartDate && lev.EndDate == model.EndDate && lev.Status.ToUpper() != "CANCELLED"
                                  && lev.Status.ToUpper() != "WITHDRAWN" && lev.Status.ToUpper() != "DELETE" && !lev.Status.ToUpper().Contains("REJECT")
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).ToList();

                var levdetails2 = DB.EmpLeaveApplications.Where(lev => lev.EmpId == model.EmpId && lev.IsActive == true && lev.IsDeleted == false
                                    && lev.Status.ToUpper() != "CANCELLED" && lev.Status.ToUpper() != "WITHDRAWN" && lev.Status.ToUpper() != "DELETE"
                                    && !lev.Status.ToUpper().Contains("REJECT")
                                    // 🔥 OVERLAP CHECK
                                    && lev.StartDate <= model.EndDate && lev.EndDate >= model.StartDate).ToList();

                if (levdetails2.Any())
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave already applied for the selected date range.");
                }

                int? ReportId = (from emp in DB.EmployeeMasters
                                 where emp.EmpId == model.EmpId
                                    && emp.IsActive == true
                                    && emp.IsDeleted == false
                                 select emp.ReportId).FirstOrDefault() ?? 0;

                if (ReportId == 0)
                {
                    ReportId = 149;
                }

                int? HRId = 149;

                if (loginId != 0)
                {
                    if (LeaveTypeId != 0)
                    {
                        if (levdetails.Count() == 0)
                        {
                            int? leavetypeId = model.LeaveTypeId;

                            string levtype = (from lev in DB.LeaveTypeMasters
                                              where lev.LeaveTypeId == leavetypeId
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev.ShortName).FirstOrDefault();


                            // Calculate difference in days (inclusive)
                            int datediffer = (int)(model.EndDate.Value.Date - model.StartDate.Value.Date).TotalDays + 1;

                            // Get maximum allowed days for leave type
                            int? maxdays = (from lev in DB.LeaveTypeMasters
                                            where lev.LeaveTypeId == leavetypeId
                                            && lev.IsActive == true && lev.IsDeleted == false
                                            select lev.MaxApply).FirstOrDefault();

                            // Check if exceeding maxdays
                            if (datediffer > maxdays)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "For this LeaveType, user can apply maximum " + maxdays + " days only..");
                            }


                            EmpLeaveApplication ela = new EmpLeaveApplication();
                            //em.EmpId = model.modelId;
                            ela.EmpId = model.EmpId;
                            ela.EmpCode = model.EmpCode;
                            ela.LeaveTypeId = model.LeaveTypeId;
                            ela.StartDate = model.StartDate;
                            ela.EndDate = model.EndDate;

                            //02.01.2026
                            DateTime appliedDate = Convert.ToDateTime(model.StartDate);

                            if (appliedDate.DayOfWeek == DayOfWeek.Monday)
                            {
                                DateTime lastFriday = appliedDate.AddDays(-3);

                                ela.Duration = model.Duration;
                                //ela.LeaveTypeId = model.LeaveTypeId;

                                var applyleavetype = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == model.LeaveTypeId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                                if (applyleavetype.ShortName.ToUpper() == "EL")
                                {
                                    var fridayLeave = (from lev in DB.EmpLeaveApplications
                                                       where lev.EmpId == model.EmpId
                                                       && lastFriday >= lev.StartDate
                                                       && lastFriday <= lev.EndDate
                                                       && lev.Status.ToUpper() != "CANCELLED"
                                                       && lev.IsActive == true
                                                       && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                    if (fridayLeave != null &&
                                        fridayLeave.LeaveTypeId == ela.LeaveTypeId)
                                    {
                                        // Friday + Weekend + Monday
                                        ela.Duration = 2;
                                        model.Duration = model.Duration + ela.Duration;
                                    }
                                    else
                                    {
                                        ela.Duration = model.Duration;
                                    }
                                }
                                else
                                {
                                    ela.Duration = model.Duration;
                                }
                            }
                            else
                            {
                                ela.Duration = model.Duration;
                            }
                            //ela.Duration = model.Duration;
                            ela.Reason = model.Reason;
                            ela.Status = "DRAFT";
                            if (levtype.ToUpper() == "COMP OFF")
                            {
                                ela.CompOffDate = model.CompOffDate;
                                ela.CompOffReason = model.CompOffReason;
                            }
                            if (model.DocName != "")
                            {
                                ela.DocName = model.DocName;
                            }
                            else if (model.DocName != null)
                            {
                                ela.DocName = model.DocName;
                            }
                            else
                            {
                                ela.DocName = "";
                            }
                            ela.AppliedDate = model.AppliedDate;
                            ela.ApprovedBy = ReportId;
                            //ela.ApprovedDate = model.ApprovedDate;
                            ela.HRApproved = HRId;
                            //ela.ApprovedDate = model.ApprovedDate;
                            ela.Remarks = model.Remarks;
                            ela.IsActive = false;
                            ela.IsUpdated = false;
                            ela.IsDeleted = false;
                            ela.Createdby = model.LoginId;
                            ela.CreatedDate = DateTime.Now;
                            ela.LastUpdatedBy = model.LoginId;
                            ela.LastUpdatedDate = DateTime.Now;
                            DB.EmpLeaveApplications.Add(ela);
                            DB.SaveChanges();

                            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Drafted";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Leave Already Exists");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Select the Leave Type");
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
        public LeaveResponseViewModel ApplyLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LeaveTypeId = (model.LeaveTypeId != 0) ? model.LeaveTypeId : 0;

                DateTime Today = DateTime.Now; //more than 10 days
                int? Year = Today.Year;
                int? Month = Today.Month;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.EmpId == model.EmpId && lev.StartDate == model.StartDate && lev.EndDate == model.EndDate && lev.Status.ToUpper() != "CANCELLED"
                                  && lev.Status.ToUpper() != "WITHDRAWN" && lev.Status.ToUpper() != "DELETE" && !lev.Status.ToUpper().Contains("REJECT")
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).ToList();

                var levdetails2 = DB.EmpLeaveApplications.Where(lev => lev.EmpId == model.EmpId && lev.IsActive == true && lev.IsDeleted == false
                                    && lev.Status.ToUpper() != "CANCELLED" && lev.Status.ToUpper() != "WITHDRAWN" && lev.Status.ToUpper() != "DELETE"
                                    && !lev.Status.ToUpper().Contains("REJECT")
                                    // 🔥 OVERLAP CHECK
                                    && lev.StartDate <= model.EndDate && lev.EndDate >= model.StartDate).ToList();

                if (levdetails2.Any())
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave already applied for the selected date range.");
                }

                var levdetails1 = (from lev in DB.EmpLeaveApplications
                                  where lev.EmpId == model.EmpId && lev.StartDate == model.StartDate && lev.EndDate == model.EndDate && lev.Status.ToUpper() == "DRAFT"
                                  && lev.IsActive == false && lev.IsDeleted == false
                                  select lev).ToList();

                if (levdetails1.Count() > 0)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave request could not be submitted. A draft leave for the same date already exists. " +
                        "Please review your draft requests.");
                }

                string levtype = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveTypeId == LeaveTypeId
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev.ShortName).FirstOrDefault();

                var levdays = (from lev in DB.EmpLeaveApplications
                                  where lev.EmpId == model.EmpId && lev.Status != "Cancelled"
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).ToList();

                var carryforwords = (from lev in DB.LeaveCarryForwardMasters
                                     where lev.EmpId == model.EmpId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                     && lev.IsActive == true && lev.IsDeleted == false
                                     select lev).FirstOrDefault();

                if (carryforwords == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Your Leave Balance Not Available");
                }

                decimal? availcount = (carryforwords.OpeningBalance - carryforwords.Availed);

                if (model.IsLOP == false)
                {
                    if (LeaveTypeId == 1)
                    {
                        int? leavemonth = model.StartDate?.Month;
                        int? leaveyear = model.StartDate?.Year;

                        var leavebalcount = (from lev in DB.LeaveCarryForwardMasters
                                             where lev.EmpId == model.EmpId && lev.LeaveMonth == leavemonth && lev.LeaveYear == leaveyear && lev.LeaveTypeId == LeaveTypeId
                                             && lev.IsDeleted == false
                                             select lev).FirstOrDefault();

                        if (leavebalcount == null)
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Insufficient CL leave balance for the last month. This leave will be marked as LOP. " +
                                    "Confirmation is required to proceed.");
                        }
                        else
                        {
                            //int? totalDays = model.StartDate.HasValue && model.EndDate.HasValue ? (model.EndDate.Value - model.StartDate.Value).Days + 1 : (int?)null;

                            decimal? totalDays = model.Duration;

                            if (leavebalcount.ClosingBalance == 0)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "Insufficient CL leave balance for the last month. This leave will be marked as LOP. " +
                                    "Confirmation is required to proceed.");
                            }
                            else if (leavebalcount.ClosingBalance > 0)
                            {
                                if (totalDays > leavebalcount.ClosingBalance)
                                {
                                    throw new CustomApiException(HttpStatusCode.NotFound, "Insufficient CL leave balance for the last month. This leave will be marked as LOP. " +
                                    "Confirmation is required to proceed.");
                                }
                            }
                        }
                    }
                    if (availcount < model.Duration)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Your " + levtype + " balance - " + availcount + ". Applied leave will be Consider as LOP");
                    }
                }

                int? ReportId = (from emp in DB.EmployeeMasters
                                 where emp.EmpId == model.EmpId
                                    && emp.IsActive == true
                                    && emp.IsDeleted == false
                                 select emp.ReportId).FirstOrDefault() ?? 0;

                if (ReportId == 0)
                {
                    ReportId = 149;
                }

                int? HRId = 149;

                if (loginId != 0)
                {
                    if (LeaveTypeId != 0)
                    {
                        if (levdetails.Count() == 0)
                        {
                            ////int? leavetypeId = model.LeaveTypeId;

                            ////string levtype = (from lev in DB.LeaveTypeMasters
                            ////                  where lev.LeaveTypeId == leavetypeId 
                            ////                  && lev.IsActive == true && lev.IsDeleted == false
                            ////                  select lev.ShortName).FirstOrDefault();

                            // Calculate difference in days (inclusive)
                            int datediffer = (int)(model.EndDate.Value.Date - model.StartDate.Value.Date).TotalDays + 1;

                            // Get maximum allowed days for leave type
                            int? maxdays = (from lev in DB.LeaveTypeMasters
                                           where lev.LeaveTypeId == LeaveTypeId
                                           && lev.IsActive == true && lev.IsDeleted == false
                                           select lev.MaxApply).FirstOrDefault();

                            // Check if exceeding maxdays
                            if (datediffer > maxdays)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "For this LeaveType, user can apply maximum " + maxdays + " days only..");
                            }

                            int locationid = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LocationId).FirstOrDefault() ?? 0;

                            // Get holidays in the given range
                            var holidaysInRange = (from hol in DB.Holidays
                                                   where hol.Year == Year
                                                   && hol.LocationId == locationid
                                                   && hol.Status == "Active"
                                                   && hol.HolidayType.ToUpper() != "RH HOLIDAYS"
                                                         && hol.Date >= model.StartDate
                                                         && hol.Date <= model.EndDate
                                                   select hol.Date).ToList();

                            if (holidaysInRange != null && holidaysInRange.Any())
                            {
                                // Prepare holiday list to show in message (optional)
                                string holidayDates = string.Join(", ", holidaysInRange.Select(d => d.ToString("yyyy-MM-dd")));
                                throw new CustomApiException(HttpStatusCode.NotFound, "Leave cannot be applied on holiday(s): " + holidayDates );
                            }

                            EmpLeaveApplication ela = new EmpLeaveApplication();
                            //em.EmpId = model.modelId;
                            ela.EmpId = model.EmpId;
                            ela.EmpCode = model.EmpCode;
                            if (model.IsLOP == true)
                            {
                                ela.LeaveTypeId = 0;
                                LeaveTypeId = 0;
                            }
                            else
                            {
                                ela.LeaveTypeId = model.LeaveTypeId;
                            }
                            ela.StartDate = model.StartDate;
                            ela.EndDate = model.EndDate;

                            //02.01.2026
                            DateTime appliedDate = Convert.ToDateTime(model.StartDate);

                            if (appliedDate.DayOfWeek == DayOfWeek.Monday)
                            {
                                DateTime lastFriday = appliedDate.AddDays(-3);

                                ela.Duration = model.Duration;
                                //ela.LeaveTypeId = model.LeaveTypeId;

                                var applyleavetype = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == model.LeaveTypeId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                                if (applyleavetype.ShortName.ToUpper() == "EL")
                                {
                                    var fridayLeave = (from lev in DB.EmpLeaveApplications
                                                       where lev.EmpId == model.EmpId
                                                       && lastFriday >= lev.StartDate
                                                       && lastFriday <= lev.EndDate
                                                       && lev.Status.ToUpper() != "CANCELLED"
                                                       && lev.IsActive == true
                                                       && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                    if (fridayLeave != null &&
                                        fridayLeave.LeaveTypeId == ela.LeaveTypeId)
                                    {
                                        // Friday + Weekend + Monday
                                        ela.Duration = 2;
                                        model.Duration = model.Duration + ela.Duration;
                                    }
                                    else
                                    {
                                        ela.Duration = model.Duration;
                                    }
                                }
                                else
                                {
                                    ela.Duration = model.Duration;
                                }
                            }
                            else
                            {
                                ela.Duration = model.Duration;
                            }
                            ela.Reason = model.Reason;
                            ela.Status = "APPLIED";
                            if (levtype.ToUpper() == "COMP OFF")
                            {
                                ela.CompOffDate = model.CompOffDate;
                                ela.CompOffReason = model.CompOffReason;
                            }
                            if (model.DocName != "")
                            {
                                ela.DocName = model.DocName;
                            }
                            else if (model.DocName != null)
                            {
                                ela.DocName = model.DocName;
                            }
                            else
                            {
                                ela.DocName = "";
                            }
                            ela.AppliedDate = model.AppliedDate;
                            ela.ApprovedBy = ReportId;
                            //ela.ApprovedDate = model.ApprovedDate;
                            ela.HRApproved = HRId;
                            //ela.ApprovedDate = model.ApprovedDate;
                            ela.Remarks = model.Remarks;
                            ela.IsActive = true;
                            ela.IsUpdated = false;
                            ela.IsDeleted = false;
                            ela.Createdby = model.LoginId;
                            ela.CreatedDate = DateTime.Now;
                            ela.LastUpdatedBy = model.LoginId;
                            ela.LastUpdatedDate = DateTime.Now;
                            DB.EmpLeaveApplications.Add(ela);
                            DB.SaveChanges();

                            ////DateTime Today = DateTime.Now;
                            ////int? Year = Today.Year;
                            ////int? Month = Today.Month;
                            ///

                            var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                        where lev.EmpId == model.EmpId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                        && lev.IsActive == true && lev.IsDeleted == false
                                                        select lev).FirstOrDefault();

                            if (levcarryFrowddetails != null)
                            {
                                decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                                decimal? avail = levcarryFrowddetails.Availed ?? 0;
                                decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                                decimal? dayscount = model.Duration;

                                bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

                                levcarryFrowddetails.OpeningBalance = (open);
                                levcarryFrowddetails.Availed = (avail + dayscount);
                                if (close == 0)
                                {
                                    levcarryFrowddetails.ClosingBalance = (open - dayscount);
                                }
                                else 
                                {
                                    levcarryFrowddetails.ClosingBalance = (close - dayscount);
                                }
                                if (SingleApp == true)
                                {
                                    levcarryFrowddetails.OpeningBalance = 0;
                                    levcarryFrowddetails.Availed = (avail + dayscount);
                                    levcarryFrowddetails.ClosingBalance = 0;
                                }
                                levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                                levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                                levcarryFrowddetails.IsActive = true;
                                levcarryFrowddetails.IsUpdated = true;
                                levcarryFrowddetails.IsDeleted = false;
                                DB.SaveChanges();
                            }
                            else
                            {

                            }

                            var userdetails = (from lev in DB.EmployeeMasters
                                              where lev.EmpId == model.EmpId 
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();


                            var reportuserdetails = (from lev in DB.EmployeeMasters
                                                       where lev.EmpId == ReportId
                                                       && lev.IsActive == true && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                            var actualHRdetails = (from lev in DB.EmailConfigMasters
                                                   where lev.Name.ToUpper() == "LEAVE" 
                                                   && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                                   && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                                   && lev.IsActive == true && lev.IsDeleted == false
                                                   select lev).FirstOrDefault();

                            var HRuserdetails = (from lev in DB.EmployeeMasters
                                                     where lev.EmpId == 149
                                                     && lev.IsActive == true && lev.IsDeleted == false
                                                     select lev).FirstOrDefault();

                            string HRmailid = "";

                            ////if (actualHRdetails != null)
                            ////{
                            ////    HRmailid = actualHRdetails.EmailId;
                            ////}
                            ////else
                            ////{
                            ////    HRmailid = HRuserdetails.EmailId;
                            ////}

                            var levtypedetails = (from lev in DB.LeaveTypeMasters
                                                  where lev.IsActive == true && lev.IsDeleted == false
                                                  select lev).FirstOrDefault();

                            string shortname = "";

                            if (LeaveTypeId == 0)
                            {
                                levtypedetails = (from lev in DB.LeaveTypeMasters
                                                  where lev.LeaveTypeId == LeaveTypeId
                                                  && lev.IsActive == true && lev.IsDeleted == false
                                                  select lev).FirstOrDefault();
                                shortname = "LOP";
                            }
                            else
                            {
                                levtypedetails = (from lev in DB.LeaveTypeMasters
                                                  where lev.LeaveTypeId == LeaveTypeId
                                                  && lev.IsActive == true && lev.IsDeleted == false
                                                  select lev).FirstOrDefault();
                                shortname = levtypedetails.ShortName;
                            }

                            string startDateOnly = model.StartDate?.ToString("yyyy-MM-dd");
                            string endDateOnly = model.EndDate?.ToString("yyyy-MM-dd");

                            string to = reportuserdetails.EmailId;
                            string cc = HRmailid;
                            string subject = "Office Connect - Leave Approval";
                            string body = $@"
                                <p>Dear {reportuserdetails.FirstName},</p>
                                <p>This is to inform you that the following employee has submitted a leave request and is awaiting your response:</p>
                                <p><strong>Employee Name: </strong>{userdetails.FirstName}</p>
                                <p><strong>Employee Code: </strong>{userdetails.EmpCode}</p>
                                <p><strong>Date of Application: </strong>{startDateOnly} - {endDateOnly}</p>
                                <p><strong>Number of Leave Days: </strong>{model.Duration}</p>
                                <p><strong>Type of Leave: </strong>{shortname}</p>
                                <p></p>
                                <p>Please review and take the necessary action at your earliest convenience. </p>
                                <p></p>
                                <p>Best regards,</p>
                                <p>Office Connect.</p>";

                            Task.Run(() => SendLeaveMail(to, cc, subject, body));

                            // ========== NEW: ADD NOTIFICATION ==========
                            // Call notification service asynchronously without waiting
                            Task.Run(async () =>
                            {
                                await _notificationService.CreateLeaveAppliedNotification(ela.LeaveAppId, model.EmpId ?? 0);
                            });

                            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Applied";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Leave Already Exists");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Select the Leave Type");
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
        public LeaveResponseViewModel DraftApplyLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LeaveTypeId = (model.LeaveTypeId != 0) ? model.LeaveTypeId : 0;
                int? LeaveAppId = (model.LeaveAppId != 0) ? model.LeaveAppId : 0;

                DateTime Today = DateTime.Now; //more than 10 days
                int? Year = Today.Year;
                int? Month = Today.Month;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.LeaveAppId == LeaveAppId && lev.Status == "DRAFT"
                                  && lev.IsActive == false && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                var levdetails2 = DB.EmpLeaveApplications.Where(lev => lev.EmpId == model.EmpId && lev.IsActive == true && lev.IsDeleted == false
                                    && lev.Status.ToUpper() != "CANCELLED" && lev.Status.ToUpper() != "WITHDRAWN" && lev.Status.ToUpper() != "DELETE"
                                    && !lev.Status.ToUpper().Contains("REJECT")
                                    // 🔥 OVERLAP CHECK
                                    && lev.StartDate <= model.EndDate && lev.EndDate >= model.StartDate).ToList();

                if (levdetails2.Any())
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave already applied for the selected date range.");
                }

                string levtype = (from lev in DB.LeaveTypeMasters
                                  where lev.LeaveTypeId == LeaveTypeId
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev.ShortName).FirstOrDefault();

                int levcount = (from lev in DB.EmpLeaveApplications
                                  where lev.EmpId == model.EmpId && lev.StartDate == model.StartDate && lev.EndDate == model.EndDate && lev.Status.ToUpper() != "CANCELLED"
                                  && lev.Status.ToUpper() != "WITHDRAWN" && lev.Status.ToUpper() != "DELETE" && !lev.Status.ToUpper().Contains("REJECT")
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev.LeaveAppId).Count();

                if (levcount > 0)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave Already Exists");
                }

                var carryforwords = (from lev in DB.LeaveCarryForwardMasters
                                     where lev.EmpId == model.EmpId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                     && lev.IsActive == true && lev.IsDeleted == false
                                     select lev).FirstOrDefault();

                decimal? availcount = (carryforwords.OpeningBalance - carryforwords.Availed);

                if (model.IsLOP == false)
                {
                    if (availcount < model.Duration)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Your " + levtype + " balance - " + availcount + ". Applied leave will be Consider as LOP");
                    }

                }

                int? ReportId = (from emp in DB.EmployeeMasters
                                 where emp.EmpId == model.EmpId
                                    && emp.IsActive == true
                                    && emp.IsDeleted == false
                                 select emp.ReportId).FirstOrDefault() ?? 0;

                if (ReportId == 0)
                {
                    ReportId = 149;
                }

                int? HRId = 149;

                if (loginId != 0)
                {
                    if (LeaveTypeId != 0)
                    {
                        if (levdetails != null)
                        {
                            int? leavetypeId = model.LeaveTypeId;

                            //string levtype = (from lev in DB.LeaveTypeMasters
                            //                  where lev.LeaveTypeId == leavetypeId
                            //                  && lev.IsActive == true && lev.IsDeleted == false
                            //                  select lev.ShortName).FirstOrDefault();

                            // Calculate difference in days (inclusive)
                            int datediffer = (int)(model.EndDate.Value.Date - model.StartDate.Value.Date).TotalDays + 1;

                            // Get maximum allowed days for leave type
                            int? maxdays = (from lev in DB.LeaveTypeMasters
                                            where lev.LeaveTypeId == leavetypeId
                                            && lev.IsActive == true && lev.IsDeleted == false
                                            select lev.MaxApply).FirstOrDefault();

                            // Check if exceeding maxdays
                            if (datediffer > maxdays)
                            {
                                throw new CustomApiException(HttpStatusCode.NotFound, "For this LeaveType, user can apply maximum " + maxdays + " days only..");
                            }

                            EmpLeaveApplication ela = new EmpLeaveApplication();
                            //em.EmpId = model.modelId;
                            levdetails.EmpId = model.EmpId;
                            levdetails.EmpCode = model.EmpCode;
                            if (model.IsLOP == true)
                            {
                                levdetails.LeaveTypeId = 0;
                                LeaveTypeId = 0;
                            }
                            else
                            {
                                levdetails.LeaveTypeId = model.LeaveTypeId;
                            }
                            levdetails.StartDate = model.StartDate;
                            levdetails.EndDate = model.EndDate;
                            levdetails.Duration = model.Duration;
                            levdetails.Reason = model.Reason;
                            levdetails.Status = "APPLIED";
                            if (levtype.ToUpper() == "COMP OFF")
                            {
                                levdetails.CompOffDate = model.CompOffDate;
                                levdetails.CompOffReason = model.CompOffReason;
                            }
                            if (model.DocName != "")
                            {
                                levdetails.DocName = model.DocName;
                            }
                            else if (model.DocName != null)
                            {
                                levdetails.DocName = model.DocName;
                            }
                            else
                            {
                                levdetails.DocName = "";
                            }
                            levdetails.AppliedDate = model.AppliedDate;
                            levdetails.ApprovedBy = ReportId;
                            //ela.ApprovedDate = model.ApprovedDate;
                            levdetails.HRApproved = HRId;
                            //ela.ApprovedDate = model.ApprovedDate;
                            levdetails.Remarks = model.Remarks;
                            levdetails.IsActive = true;
                            levdetails.IsUpdated = true;
                            levdetails.IsDeleted = false;
                            levdetails.LastUpdatedBy = model.LoginId;
                            levdetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                        where lev.EmpId == model.EmpId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                        && lev.IsActive == true && lev.IsDeleted == false
                                                        select lev).FirstOrDefault();

                            if (levcarryFrowddetails != null)
                            {
                                decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                                decimal? avail = levcarryFrowddetails.Availed ?? 0;
                                decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                                decimal? dayscount = model.Duration;

                                bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

                                levcarryFrowddetails.OpeningBalance = (open);
                                levcarryFrowddetails.Availed = (avail + dayscount);
                                if (close == 0)
                                {
                                    levcarryFrowddetails.ClosingBalance = (open - dayscount);
                                }
                                else
                                {
                                    levcarryFrowddetails.ClosingBalance = (close - dayscount);
                                }
                                if (SingleApp == true)
                                {
                                    levcarryFrowddetails.OpeningBalance = 0;
                                    levcarryFrowddetails.Availed = (avail + dayscount);
                                    levcarryFrowddetails.ClosingBalance = 0;
                                }

                                levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                                levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                                levcarryFrowddetails.IsActive = true;
                                levcarryFrowddetails.IsUpdated = true;
                                levcarryFrowddetails.IsDeleted = false;
                                DB.SaveChanges();
                            }
                            else
                            {

                            }

                            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Applied";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Draft Leave details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Select the Leave Type");
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
        public LeaveResponseViewModel WithDrawLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LeaveAppId = (model.LeaveAppId != 0) ? model.LeaveAppId : 0;

                DateTime today = DateTime.Now.Date;
                DateTime lastmonthday = DateTime.Now.AddDays(-30);

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.LeaveAppId == LeaveAppId && lev.EmpId == EmpId && lev.StartDate >= lastmonthday 
                                  //&& lev.Status == "APPLIED" //&& lev.StartDate == model.StartDate && lev.EndDate == model.EndDate
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        int? LeaveTypeId = levdetails.LeaveTypeId;
                        string rejectStatus = levdetails.Status;
                        int? UserId = levdetails.EmpId;

                        levdetails.Status = "WITHDRAWN";
                        levdetails.IsActive = true;
                        levdetails.IsUpdated = true;
                        levdetails.IsDeleted = false;
                        levdetails.LastUpdatedBy = model.LoginId;
                        levdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        if (rejectStatus.ToUpper() == "APPROVED BY HR")
                        {
                            DateTime Today = DateTime.Now;
                            ////int? Year = Today.Year;
                            ////int? Month = Today.Month;
                            int? Year = levdetails.StartDate?.Year;
                            int? Month = levdetails.StartDate?.Month;

                            var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                        where lev.EmpId == UserId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                        && lev.IsActive == true && lev.IsDeleted == false
                                                        select lev).FirstOrDefault();

                            if (levcarryFrowddetails != null)
                            {
                                decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                                decimal? avail = levcarryFrowddetails.Availed ?? 0;
                                decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                                decimal? dayscount = levdetails.Duration;

                                ////levcarryFrowddetails.OpeningBalance = (open);
                                ////levcarryFrowddetails.Availed = (avail - dayscount);
                                ////levcarryFrowddetails.ClosingBalance = (close + dayscount);
                                levcarryFrowddetails.OpeningBalance = (close + dayscount);
                                if (avail == 0)
                                {
                                    levcarryFrowddetails.Availed = 0;
                                }
                                else
                                {
                                    if (avail >= dayscount)
                                    {
                                        levcarryFrowddetails.Availed = (avail - dayscount);
                                    }
                                    else
                                    {
                                        levcarryFrowddetails.Availed = 0;
                                    }

                                }
                                levcarryFrowddetails.ClosingBalance = (close + dayscount);
                                levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                                levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                                levcarryFrowddetails.IsActive = true;
                                levcarryFrowddetails.IsUpdated = true;
                                levcarryFrowddetails.IsDeleted = false;
                                DB.SaveChanges();
                            }
                            else
                            {

                            }
                        }
                        else if (rejectStatus.ToUpper() == "APPROVED BY MANAGER")
                        {
                            DateTime Today = DateTime.Now;
                            ////int? Year = Today.Year;
                            ////int? Month = Today.Month;
                            int? Year = levdetails.StartDate?.Year;
                            int? Month = levdetails.StartDate?.Month;

                            var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                        where lev.EmpId == model.EmpId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                        && lev.IsActive == true && lev.IsDeleted == false
                                                        select lev).FirstOrDefault();

                            if (levcarryFrowddetails != null)
                            {
                                decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                                decimal? avail = levcarryFrowddetails.Availed ?? 0;
                                decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                                decimal? dayscount = levdetails.Duration;

                                bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

                                int? maxmdays = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.MaxPerYear).FirstOrDefault();

                                levcarryFrowddetails.OpeningBalance = (close + dayscount);
                                if (avail == 0)
                                {
                                    levcarryFrowddetails.Availed = 0;
                                }
                                else
                                {
                                    if (avail >= dayscount)
                                    {
                                        levcarryFrowddetails.Availed = (avail - dayscount);
                                    }
                                    else
                                    {
                                        levcarryFrowddetails.Availed = 0;
                                    }
                                    
                                }
                                levcarryFrowddetails.ClosingBalance = (close + dayscount);
                                if (SingleApp == true)
                                {

                                    levcarryFrowddetails.OpeningBalance = maxmdays;
                                    levcarryFrowddetails.Availed = 0;
                                    levcarryFrowddetails.ClosingBalance = maxmdays;
                                }

                                levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                                levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                                levcarryFrowddetails.IsActive = true;
                                levcarryFrowddetails.IsUpdated = true;
                                levcarryFrowddetails.IsDeleted = false;
                                DB.SaveChanges();
                            }
                            else
                            {

                            }
                        }

                        var userdetails = (from lev in DB.EmployeeMasters
                                           where lev.EmpId == model.EmpId
                                           && lev.IsActive == true && lev.IsDeleted == false
                                           select lev).FirstOrDefault();

                        var reportuserdetails = (from lev in DB.EmployeeMasters
                                                 where lev.EmpId == userdetails.ReportId
                                                 && lev.IsActive == true && lev.IsDeleted == false
                                                 select lev).FirstOrDefault();

                        var actualHRdetails = (from lev in DB.EmailConfigMasters
                                               where lev.Name.ToUpper() == "LEAVE"
                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                               && lev.IsActive == true && lev.IsDeleted == false
                                               select lev).FirstOrDefault();

                        var HRuserdetails = (from lev in DB.EmployeeMasters
                                             where lev.EmpId == 149
                                             && lev.IsActive == true && lev.IsDeleted == false
                                             select lev).FirstOrDefault();

                        string HRmailid = "";

                        ////if (actualHRdetails != null)
                        ////{
                        ////    HRmailid = actualHRdetails.EmailId;
                        ////}
                        ////else
                        ////{
                        ////    HRmailid = HRuserdetails.EmailId;
                        ////}

                        var levtypedetails = (from lev in DB.LeaveTypeMasters
                                              where lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();

                        string shortname = "";

                        if (LeaveTypeId == 0)
                        {
                            levtypedetails = (from lev in DB.LeaveTypeMasters
                                              where lev.LeaveTypeId == LeaveTypeId
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();
                            shortname = "LOP";
                        }
                        else
                        {
                            levtypedetails = (from lev in DB.LeaveTypeMasters
                                              where lev.LeaveTypeId == LeaveTypeId
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();
                            shortname = levtypedetails.ShortName;
                        }

                        string startDateOnly = model.StartDate?.ToString("yyyy-MM-dd");
                        string endDateOnly = model.EndDate?.ToString("yyyy-MM-dd");

                        string to = reportuserdetails.EmailId;
                        string cc = HRmailid;
                        string subject = "Office Connect - Leave Withdraw";
                        string body = $@"
                                <p>Dear {reportuserdetails.FirstName},</p>
                                <p>This is to notify you that the following employee has withdrawn their previously approved leave request through the Office Connect application:</p>
                                <p><strong>Employee Name: </strong>{userdetails.FirstName}</p>
                                <p><strong>Employee Code: </strong>{userdetails.EmpCode}</p>
                                <p><strong>Date of Application: </strong>{startDateOnly} - {endDateOnly}</p>
                                <p><strong>Number of Leave Days: </strong>{model.Duration}</p>
                                <p><strong>Type of Leave: </strong>{shortname}</p>
                                <p></p>
                                <p>The leave status has been updated accordingly in Office Connect. </p>
                                <p></p>
                                <p>Best regards,</p>
                                <p>Office Connect.</p>";

                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

                        // ========== NEW: ADD NOTIFICATION ==========
                        Task.Run(async () =>
                        {
                            await _notificationService.CreateLeaveWithdrawnNotification(model.LeaveAppId, model.EmpId ?? 0);
                        });

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "WithDrawn";

                        return emvm;
                    }
                    else
                    {
                        var levdetails1 = (from lev in DB.EmpLeaveApplications
                                          where lev.LeaveAppId == LeaveAppId && lev.EmpId == EmpId && lev.StartDate >= lastmonthday
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails1 == null)
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Leave can be withdrawn only within 30 days prior to the leave date.");
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
                        }

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
        public LeaveResponseViewModel CancelLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LeaveAppId = (model.LeaveAppId != 0) ? model.LeaveAppId : 0;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.LeaveAppId == LeaveAppId && lev.EmpId == EmpId && lev.Status == "APPLIED" //&& lev.StartDate == model.StartDate && lev.EndDate == model.EndDate
                                  && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                int? LeaveTypeId = levdetails.LeaveTypeId;

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        levdetails.Status = "CANCELLED";
                        levdetails.IsActive = true;
                        levdetails.IsUpdated = true;
                        levdetails.IsDeleted = false;
                        levdetails.LastUpdatedBy = model.LoginId;
                        levdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        DateTime Today = DateTime.Now;
                        ////int? Year = Today.Year;
                        ////int? Month = Today.Month;
                        int? Year = levdetails.StartDate?.Year;
                        int? Month = levdetails.StartDate?.Month;

                        var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                    where lev.EmpId == model.EmpId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                    && lev.IsActive == true && lev.IsDeleted == false
                                                    select lev).FirstOrDefault();

                        if (levcarryFrowddetails != null)
                        {
                            decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                            decimal? avail = levcarryFrowddetails.Availed ?? 0;
                            decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                            decimal? dayscount = levdetails.Duration;

                            bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

                            int? maxmdays = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                            && x.IsDeleted == false).Select(x => x.MaxPerYear).FirstOrDefault();

                            levcarryFrowddetails.OpeningBalance = (open);
                            levcarryFrowddetails.Availed = (avail - dayscount);
                            levcarryFrowddetails.ClosingBalance = (close + dayscount);
                            if (SingleApp == true)
                            {

                                levcarryFrowddetails.OpeningBalance = maxmdays;
                                levcarryFrowddetails.Availed = 0;
                                levcarryFrowddetails.ClosingBalance = maxmdays;
                            }

                            levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                            levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                            levcarryFrowddetails.IsActive = true;
                            levcarryFrowddetails.IsUpdated = true;
                            levcarryFrowddetails.IsDeleted = false;
                            DB.SaveChanges();
                        }
                        else
                        {

                        }

                        var userdetails = (from lev in DB.EmployeeMasters
                                           where lev.EmpId == model.EmpId
                                           && lev.IsActive == true && lev.IsDeleted == false
                                           select lev).FirstOrDefault();

                        var reportuserdetails = (from lev in DB.EmployeeMasters
                                                 where lev.EmpId == userdetails.ReportId
                                                 && lev.IsActive == true && lev.IsDeleted == false
                                                 select lev).FirstOrDefault();

                        var actualHRdetails = (from lev in DB.EmailConfigMasters
                                               where lev.Name.ToUpper() == "LEAVE"
                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                               && lev.IsActive == true && lev.IsDeleted == false
                                               select lev).FirstOrDefault();

                        var HRuserdetails = (from lev in DB.EmployeeMasters
                                             where lev.EmpId == 149
                                             && lev.IsActive == true && lev.IsDeleted == false
                                             select lev).FirstOrDefault();

                        string HRmailid = "";

                        ////if (actualHRdetails != null)
                        ////{
                        ////    HRmailid = actualHRdetails.EmailId;
                        ////}
                        ////else
                        ////{
                        ////    HRmailid = HRuserdetails.EmailId;
                        ////}

                        var levtypedetails = (from lev in DB.LeaveTypeMasters
                                              where lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();

                        string shortname = "";

                        if (LeaveTypeId == 0)
                        {
                            levtypedetails = (from lev in DB.LeaveTypeMasters
                                                  where lev.LeaveTypeId == LeaveTypeId
                                                  && lev.IsActive == true && lev.IsDeleted == false
                                                  select lev).FirstOrDefault();
                            shortname = "LOP";
                        }
                        else
                        {
                            levtypedetails = (from lev in DB.LeaveTypeMasters
                                              where lev.LeaveTypeId == LeaveTypeId
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();
                            shortname = levtypedetails.ShortName;
                        }

                        string startDateOnly = model.StartDate?.ToString("yyyy-MM-dd");
                        string endDateOnly = model.EndDate?.ToString("yyyy-MM-dd");

                        string to = reportuserdetails.EmailId;
                        string cc = HRmailid;
                        string subject = "Office Connect - Leave Cancel";
                        string body = $@"
                                <p>Dear {reportuserdetails.FirstName},</p>
                                <p>This is to notify you that the following employee has Cancelled their previously applied leave request through the Office Connect application:</p>
                                <p><strong>Employee Name: </strong>{userdetails.FirstName}</p>
                                <p><strong>Employee Code: </strong>{userdetails.EmpCode}</p>
                                <p><strong>Date of Application: </strong>{startDateOnly} - {endDateOnly}</p>
                                <p><strong>Number of Leave Days: </strong>{model.Duration}</p>
                                <p><strong>Type of Leave: </strong>{shortname}</p>
                                <p></p>
                                <p>The leave status has been updated accordingly in Office Connect. </p>
                                <p></p>
                                <p>Best regards,</p>
                                <p>Office Connect.</p>";

                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

                        // ========== NEW: ADD NOTIFICATION ==========
                        Task.Run(async () =>
                        {
                            await _notificationService.CreateLeaveCancelledNotification(model.LeaveAppId, model.EmpId ?? 0);
                        });

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Cancelled";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        public LeaveResponseViewModel DeleteDraftLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? LeaveAppId = (model.LeaveAppId != 0) ? model.LeaveAppId : 0;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.LeaveAppId == LeaveAppId && lev.EmpId == EmpId && lev.Status == "DRAFT" //&& lev.StartDate == model.StartDate && lev.EndDate == model.EndDate
                                  && lev.IsActive == false && lev.IsDeleted == false
                                  select lev).FirstOrDefault();

                int? LeaveTypeId = levdetails.LeaveTypeId ?? 0;

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        levdetails.Status = "DELETE";
                        levdetails.IsActive = false;
                        levdetails.IsUpdated = true;
                        levdetails.IsDeleted = true;
                        levdetails.LastUpdatedBy = model.LoginId;
                        levdetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted - Draft";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        public List<EmpLeaveApplicationViewModel> GetAllLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where (lev.EmpId == empId && lev.IsActive == true && lev.IsDeleted == false) ||
                                  (lev.EmpId == empId && lev.IsActive == false && lev.IsDeleted == false)
                                  select lev).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        List<EmpLeaveApplicationViewModel> lstoflve = new List<EmpLeaveApplicationViewModel>();

                        for (int i = 0; i < levdetails.Count(); i++)
                        {
                            EmpLeaveApplicationViewModel elavm = new EmpLeaveApplicationViewModel();
                            elavm.LeaveAppId = levdetails[i].LeaveAppId;
                            elavm.EmpId = levdetails[i].EmpId;
                            int? EmpId = levdetails[i].EmpId;
                            elavm.EmpCode = levdetails[i].EmpCode;
                            elavm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == EmpId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.LeaveTypeId = levdetails[i].LeaveTypeId;
                            int? leavetypeid = levdetails[i].LeaveTypeId;
                            if (leavetypeid == 0)
                            {
                                elavm.LeaveType = "LOP";
                            }
                            else
                            {
                                string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                                string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                    && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                                elavm.LeaveType = leavename + " - (" + shortname + ")";
                            }
                            elavm.StartDate = levdetails[i].StartDate;
                            elavm.EndDate = levdetails[i].EndDate;
                            elavm.Duration = levdetails[i].Duration;
                            elavm.Reason = levdetails[i].Reason;
                            elavm.Status = levdetails[i].Status;
                            string status = levdetails[i].Status;
                            if (levdetails[i].CompOffDate != null)
                            {
                                elavm.CompOffDate = levdetails[i].CompOffDate;
                            }
                            if (levdetails[i].CompOffReason != null)
                            {
                                elavm.CompOffReason = levdetails[i].CompOffReason;
                            }
                            if (levdetails[i].DocName != "")
                            {
                                elavm.DocName = levdetails[i].DocName;
                            }
                            elavm.AppliedDate = levdetails[i].AppliedDate;
                            //if (status == "APPLIED" || status == "DRAFT" || status == "CANCELLED")
                            //{
                            //    elavm.ApprovedBy = 0;
                            //    int? ApproverId = 0;
                            //    elavm.Approver = "";
                            //}
                            //else
                            //{
                            //    elavm.ApprovedBy = levdetails[i].ApprovedBy;
                            //    int? ApproverId = levdetails[i].ApprovedBy;
                            //    elavm.Approver = DB.EmployeeMasters.Where(x => x.EmpId == ApproverId && x.IsActive == true
                            //                        && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            //}
                            elavm.ApprovedBy = levdetails[i].ApprovedBy;
                            int? ApproverId = levdetails[i].ApprovedBy;
                            elavm.Approver = DB.EmployeeMasters.Where(x => x.EmpId == ApproverId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.ApprovedDate = levdetails[i].ApprovedDate;
                            elavm.Remarks = levdetails[i].Remarks;
                            elavm.Createdby = levdetails[i].Createdby;
                            elavm.CreatedDate = levdetails[i].CreatedDate;
                            elavm.LastUpdatedBy = levdetails[i].LastUpdatedBy;
                            elavm.LastUpdatedDate = levdetails[i].LastUpdatedDate;
                            elavm.IsActive = levdetails[i].IsActive;
                            elavm.IsUpdated = levdetails[i].IsUpdated;
                            elavm.IsDeleted = levdetails[i].IsDeleted;
                            lstoflve.Add(elavm);

                        }
                        return lstoflve;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        public List<EmpLeaveApplicationViewModel> GetAllApplyManagerLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                var levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.ApprovedBy == loginId && lev.Status == "APPLIED" && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        List<EmpLeaveApplicationViewModel> lstoflve = new List<EmpLeaveApplicationViewModel>();

                        for (int i = 0; i < levdetails.Count(); i++)
                        {
                            EmpLeaveApplicationViewModel elavm = new EmpLeaveApplicationViewModel();
                            elavm.LeaveAppId = levdetails[i].LeaveAppId;
                            elavm.EmpId = levdetails[i].EmpId;
                            int? EmpId = levdetails[i].EmpId;
                            elavm.EmpCode = levdetails[i].EmpCode;
                            elavm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == EmpId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.LeaveTypeId = levdetails[i].LeaveTypeId;
                            int? leavetypeid = levdetails[i].LeaveTypeId;
                            if (leavetypeid == 0)
                            {
                                elavm.LeaveType = "LOP";
                            }
                            else
                            {
                                string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                                string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                    && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                                elavm.LeaveType = leavename + " - (" + shortname + ")";
                            }
                            elavm.StartDate = levdetails[i].StartDate;
                            elavm.EndDate = levdetails[i].EndDate;
                            elavm.Duration = levdetails[i].Duration;
                            elavm.Reason = levdetails[i].Reason;
                            elavm.Status = levdetails[i].Status;
                            if (levdetails[i].CompOffDate != null)
                            {
                                elavm.CompOffDate = levdetails[i].CompOffDate;
                            }
                            if (levdetails[i].CompOffReason != null)
                            {
                                elavm.CompOffReason = levdetails[i].CompOffReason;
                            }
                            if (levdetails[i].DocName != "")
                            {
                                elavm.DocName = levdetails[i].DocName;
                            }
                            elavm.AppliedDate = levdetails[i].AppliedDate;
                            elavm.ApprovedBy = levdetails[i].ApprovedBy;
                            int? ApproverId = levdetails[i].ApprovedBy;
                            elavm.Approver = DB.EmployeeMasters.Where(x => x.EmpId == ApproverId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.ApprovedDate = levdetails[i].ApprovedDate;
                            elavm.Remarks = levdetails[i].Remarks;
                            elavm.Createdby = levdetails[i].Createdby;
                            elavm.CreatedDate = levdetails[i].CreatedDate;
                            elavm.LastUpdatedBy = levdetails[i].LastUpdatedBy;
                            elavm.LastUpdatedDate = levdetails[i].LastUpdatedDate;
                            elavm.IsActive = levdetails[i].IsActive;
                            elavm.IsUpdated = levdetails[i].IsUpdated;
                            elavm.IsDeleted = levdetails[i].IsDeleted;
                            lstoflve.Add(elavm);
                        }
                        return lstoflve;
                        //return listofall;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Apply Details Not Found");
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
        public List<EmpLeaveApplicationViewModel> GetAllManagerLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                DateTime? startDate = model.StartDate;
                DateTime? endDate = model.EndDate;
                string status = model.Status;

                var alllevdetails = (from lev in DB.EmpLeaveApplications
                                      where lev.ApprovedBy == loginId && lev.Status != "APPLIED" && lev.IsActive == true && lev.IsDeleted == false
                                      select lev).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (alllevdetails != null)
                    {
                        List<EmpLeaveApplicationViewModel> lstoflveall = new List<EmpLeaveApplicationViewModel>();

                        for (int i = 0; i < alllevdetails.Count(); i++)
                        {
                            EmpLeaveApplicationViewModel elavm = new EmpLeaveApplicationViewModel();
                            elavm.LeaveAppId = alllevdetails[i].LeaveAppId;
                            elavm.EmpId = alllevdetails[i].EmpId;
                            int? EmpId = alllevdetails[i].EmpId;
                            elavm.EmpCode = alllevdetails[i].EmpCode;
                            elavm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == EmpId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.LeaveTypeId = alllevdetails[i].LeaveTypeId;
                            int? leavetypeid = alllevdetails[i].LeaveTypeId;
                            if (leavetypeid == 0)
                            {
                                elavm.LeaveType = "LOP";
                            }
                            else
                            {
                                string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                                string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                    && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                                elavm.LeaveType = leavename + " - (" + shortname + ")";
                            }
                            elavm.StartDate = alllevdetails[i].StartDate;
                            elavm.EndDate = alllevdetails[i].EndDate;
                            elavm.Duration = alllevdetails[i].Duration;
                            elavm.Reason = alllevdetails[i].Reason;
                            elavm.Status = alllevdetails[i].Status;
                            string appstatus = alllevdetails[i].Status;

                            if (appstatus.ToUpper() == "APPROVED BY HR")
                            {
                                elavm.Status = "APPROVED";
                            }
                            else if (appstatus.ToUpper() == "APPROVED BY MANAGER")
                            {
                                elavm.Status = "APPROVED";
                            }
                            
                            else if (appstatus.ToUpper() == "REJECTED BY HR")
                            {
                                elavm.Status = "REJECTED";
                            }
                            else if (appstatus.ToUpper() == "REJECTED BY MANAGER")
                            {
                                elavm.Status = "REJECTED";
                            }
                            else
                            {
                                elavm.Status = alllevdetails[i].Status;
                            }

                            if (alllevdetails[i].CompOffDate != null)
                            {
                                elavm.CompOffDate = alllevdetails[i].CompOffDate;
                            }
                            if (alllevdetails[i].CompOffReason != null)
                            {
                                elavm.CompOffReason = alllevdetails[i].CompOffReason;
                            }
                            if (alllevdetails[i].DocName != null)
                            {
                                if (alllevdetails[i].DocName != "")
                                {
                                    elavm.DocName = alllevdetails[i].DocName;
                                }
                            }
                            elavm.AppliedDate = alllevdetails[i].AppliedDate;
                            elavm.ApprovedBy = alllevdetails[i].ApprovedBy;
                            int? ApproverId = alllevdetails[i].ApprovedBy;
                            elavm.Approver = DB.EmployeeMasters.Where(x => x.EmpId == ApproverId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.ApprovedDate = alllevdetails[i].ApprovedDate;
                            elavm.Remarks = alllevdetails[i].Remarks ?? "";
                            elavm.Createdby = alllevdetails[i].Createdby;
                            elavm.CreatedDate = alllevdetails[i].CreatedDate;
                            elavm.LastUpdatedBy = alllevdetails[i].LastUpdatedBy;
                            elavm.LastUpdatedDate = alllevdetails[i].LastUpdatedDate;
                            elavm.IsActive = alllevdetails[i].IsActive;
                            elavm.IsUpdated = alllevdetails[i].IsUpdated;
                            elavm.IsDeleted = alllevdetails[i].IsDeleted;
                            lstoflveall.Add(elavm);

                        }
                        if (startDate != null && endDate != null)
                        {
                            var list = lstoflveall.Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();
                            lstoflveall = list.ToList();
                        }
                        if (status != "")
                        {
                            if (status == "Approved")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "APPROVED BY MANAGER" || x.Status.ToUpper() == "APPROVED BY HR").ToList();
                                lstoflveall = list.ToList();
                            }
                            else if (status == "Rejected")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "REJECTED BY MANAGER" || x.Status.ToUpper() == "REJECTED BY HR").ToList();
                                lstoflveall = list.ToList();
                            }
                            else if (status == "Cancelled")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "CANCELLED").ToList();
                                lstoflveall = list.ToList();
                            }
                            else if (status == "WithDrawn")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "WITHDRAWN").ToList();
                                lstoflveall = list.ToList();
                            }
                        }
                        return lstoflveall;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        ////public LeaveResponseViewModel ApproveLeaveByManager(ApproveLeaveViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        if (loginId != 0)
        ////        {
        ////            if (model.lstofLevAppId.Count() > 0)
        ////            {
        ////                for (int i = 0; i < model.lstofLevAppId.Count; i++)
        ////                {
        ////                    var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

        ////                    var levdetails = (from lev in DB.EmpLeaveApplications
        ////                                      where lev.LeaveAppId == leaveAppId && lev.Status == "APPLIED"
        ////                                      && lev.IsActive == true && lev.IsDeleted == false
        ////                                      select lev).FirstOrDefault();

        ////                    if (levdetails != null)
        ////                    {
        ////                        levdetails.Status = "APPROVED BY MANAGER";
        ////                        levdetails.Remarks = model.lstofLevAppId[i].Remarks;
        ////                        levdetails.ApprovedBy = model.LoginId;
        ////                        levdetails.ApprovedDate = DateTime.Now;
        ////                        levdetails.IsActive = true;
        ////                        levdetails.IsUpdated = true;
        ////                        levdetails.IsDeleted = false;
        ////                        levdetails.LastUpdatedBy = model.LoginId;
        ////                        levdetails.LastUpdatedDate = DateTime.Now;
        ////                        DB.SaveChanges();

        ////                        var managerdetails = (from lev in DB.EmployeeMasters
        ////                                           where lev.EmpId == model.EmpId
        ////                                           && lev.IsActive == true && lev.IsDeleted == false
        ////                                           select lev).FirstOrDefault();


        ////                        var userdetails = (from lev in DB.EmployeeMasters
        ////                                                 where lev.EmpId == levdetails.EmpId
        ////                                                 && lev.IsActive == true && lev.IsDeleted == false
        ////                                                 select lev).FirstOrDefault();

        ////                        var actualHRdetails = (from lev in DB.EmailConfigMasters
        ////                                               where lev.Name.ToUpper() == "LEAVE"
        ////                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
        ////                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
        ////                                               && lev.IsActive == true && lev.IsDeleted == false
        ////                                               select lev).FirstOrDefault();

        ////                        var HRuserdetails = (from lev in DB.EmployeeMasters
        ////                                             where lev.EmpId == 149
        ////                                             && lev.IsActive == true && lev.IsDeleted == false
        ////                                             select lev).FirstOrDefault();

        ////                        string HRmailid = "";

        ////                        ////if (actualHRdetails != null)
        ////                        ////{
        ////                        ////    HRmailid = actualHRdetails.EmailId;
        ////                        ////}
        ////                        ////else
        ////                        ////{
        ////                        ////    HRmailid = HRuserdetails.EmailId;
        ////                        ////}

        ////                        var levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        string shortname = "";

        ////                        if (levdetails.LeaveTypeId == 0)
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = "LOP";
        ////                        }
        ////                        else
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = levtypedetails.ShortName;
        ////                        }

        ////                        string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
        ////                        string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

        ////                        string to = userdetails.EmailId;
        ////                        string cc = HRmailid;
        ////                        string subject = "Office Connect - Leave Request Update";
        ////                        string body = $@"
        ////                        <p>Dear {userdetails.FirstName},</p>
        ////                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Approved by your manager (<strong>{managerdetails.FirstName}</strong>).</p>
        ////                        <p></p>
        ////                        <p>You can view the updated status of your application in the Office Connect portal.</p>
        ////                        <p></p>
        ////                        <p>Best regards,</p>
        ////                        <p>Office Connect.</p>";

        ////                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

        ////                        // ========== NEW: ADD NOTIFICATION ==========
        ////                        Task.Run(async () =>
        ////                        {
        ////                            await _notificationService.CreateLeaveApprovedByManagerNotification(leaveAppId, model.LoginId);
        ////                        });
        ////                    }
        ////                    else
        ////                    {
        ////                        throw new CustomApiException(HttpStatusCode.NotFound, "Applied Leave Details Not Found");
        ////                    }
        ////                }
        ////            }

        ////            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
        ////            emvm.Status = 200;
        ////            emvm.msg = "Approved By Manager";

        ////            return emvm;
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        ////public LeaveResponseViewModel RejectLeaveByManager(ApproveLeaveViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        if (loginId != 0)
        ////        {
        ////            if (model.lstofLevAppId.Count() > 0)
        ////            {
        ////                for (int i = 0; i < model.lstofLevAppId.Count; i++) 
        ////                {
        ////                    var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

        ////                    var levdetails = (from lev in DB.EmpLeaveApplications
        ////                                      where lev.LeaveAppId == leaveAppId && lev.Status == "APPLIED" 
        ////                                      && lev.IsActive == true && lev.IsDeleted == false
        ////                                      select lev).FirstOrDefault();

        ////                    int? LeaveTypeId = levdetails.LeaveTypeId;
        ////                    int? UserId = levdetails.EmpId;

        ////                    if (levdetails != null)
        ////                    {
        ////                        levdetails.Status = "REJECTED BY MANAGER";
        ////                        levdetails.Remarks = model.lstofLevAppId[i].Remarks;
        ////                        levdetails.ApprovedBy = model.LoginId;
        ////                        levdetails.ApprovedDate = DateTime.Now;
        ////                        levdetails.IsActive = true;
        ////                        levdetails.IsUpdated = true;
        ////                        levdetails.IsDeleted = false;
        ////                        levdetails.LastUpdatedBy = model.LoginId;
        ////                        levdetails.LastUpdatedDate = DateTime.Now;
        ////                        DB.SaveChanges();

        ////                        DateTime Today = DateTime.Now;
        ////                        int? Year = Today.Year;
        ////                        int? Month = Today.Month;

        ////                        var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
        ////                                                    where lev.EmpId == UserId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
        ////                                                    && lev.IsActive == true && lev.IsDeleted == false
        ////                                                    select lev).FirstOrDefault();

        ////                        if (levcarryFrowddetails != null)
        ////                        {
        ////                            decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
        ////                            decimal? avail = levcarryFrowddetails.Availed ?? 0;
        ////                            decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
        ////                            decimal? dayscount = levdetails.Duration;

        ////                            bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
        ////                                        && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

        ////                            int? maxmdays = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
        ////                                            && x.IsDeleted == false).Select(x => x.MaxPerYear).FirstOrDefault();

        ////                            levcarryFrowddetails.OpeningBalance = (open);
        ////                            levcarryFrowddetails.Availed = (avail - dayscount);
        ////                            levcarryFrowddetails.ClosingBalance = (close + dayscount);
        ////                            if (SingleApp == true)
        ////                            {

        ////                                levcarryFrowddetails.OpeningBalance = maxmdays;
        ////                                levcarryFrowddetails.Availed = 0;
        ////                                levcarryFrowddetails.ClosingBalance = maxmdays;
        ////                            }

        ////                            levcarryFrowddetails.LastUpdatedBy = model.LoginId;
        ////                            levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
        ////                            levcarryFrowddetails.IsActive = true;
        ////                            levcarryFrowddetails.IsUpdated = true;
        ////                            levcarryFrowddetails.IsDeleted = false;
        ////                            DB.SaveChanges();
        ////                        }
        ////                        else
        ////                        {

        ////                        }

        ////                        var managerdetails = (from lev in DB.EmployeeMasters
        ////                                              where lev.EmpId == model.EmpId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        var userdetails = (from lev in DB.EmployeeMasters
        ////                                           where lev.EmpId == levdetails.EmpId
        ////                                           && lev.IsActive == true && lev.IsDeleted == false
        ////                                           select lev).FirstOrDefault();

        ////                        var actualHRdetails = (from lev in DB.EmailConfigMasters
        ////                                               where lev.Name.ToUpper() == "LEAVE"
        ////                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
        ////                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
        ////                                               && lev.IsActive == true && lev.IsDeleted == false
        ////                                               select lev).FirstOrDefault();

        ////                        var HRuserdetails = (from lev in DB.EmployeeMasters
        ////                                             where lev.EmpId == 149
        ////                                             && lev.IsActive == true && lev.IsDeleted == false
        ////                                             select lev).FirstOrDefault();

        ////                        string HRmailid = "";

        ////                        ////if (actualHRdetails != null)
        ////                        ////{
        ////                        ////    HRmailid = actualHRdetails.EmailId;
        ////                        ////}
        ////                        ////else
        ////                        ////{
        ////                        ////    HRmailid = HRuserdetails.EmailId;
        ////                        ////}

        ////                        var levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        string shortname = "";

        ////                        if (levdetails.LeaveTypeId == 0)
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = "LOP";
        ////                        }
        ////                        else
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = levtypedetails.ShortName;
        ////                        }

        ////                        string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
        ////                        string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

        ////                        string to = userdetails.EmailId;
        ////                        string cc = HRmailid;
        ////                        string subject = "Office Connect - Leave Request Update";
        ////                        string body = $@"
        ////                        <p>Dear {userdetails.FirstName},</p>
        ////                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Rejected by your manager (<strong>{managerdetails.FirstName}</strong>).</p>
        ////                        <p></p>
        ////                        <p>You can view the updated status of your application in the Office Connect portal.</p>
        ////                        <p></p>
        ////                        <p>Best regards,</p>
        ////                        <p>Office Connect.</p>";

        ////                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

        ////                        // ========== NEW: ADD NOTIFICATION ==========
        ////                        Task.Run(async () =>
        ////                        {
        ////                            await _notificationService.CreateLeaveRejectedByManagerNotification(leaveAppId, model.LoginId);
        ////                        });
        ////                    }
        ////                    else
        ////                    {
        ////                        throw new CustomApiException(HttpStatusCode.NotFound, "Rejected Leave Details Not Found");
        ////                    }
        ////                }
        ////            }
        ////            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
        ////            emvm.Status = 200;
        ////            emvm.msg = "Rejected By Manager";

        ////            return emvm;
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        public List<EmpLeaveApplicationViewModel> GetAllApplyHRLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;


                DateTime? startDate = model.StartDate;
                DateTime? endDate = model.EndDate;
                string status = model.Status;

                int locationid = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LocationId).FirstOrDefault() ?? 0;

                int desigId = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.DesignationId).FirstOrDefault() ?? 0;

                int deptId = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.CategoryId).FirstOrDefault() ?? 0;

                var levdetails = (from emp in DB.EmployeeMasters
                                  join lev in DB.EmpLeaveApplications on emp.EmpId equals lev.EmpId
                                  where emp.LocationId == locationid && (lev.Status == "APPLIED") && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).OrderBy(x => x.Status).OrderByDescending(x => x.LeaveTypeId).ToList();

                if (desigId == 186)
                {
                    levdetails = (from lev in DB.EmpLeaveApplications
                                  where (lev.Status == "APPLIED") && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).OrderBy(x => x.Status).OrderByDescending(x => x.LeaveTypeId).ToList();
                }
                else if (deptId > 1)
                {
                    levdetails = (from lev in DB.EmpLeaveApplications
                                  where lev.ApprovedBy == model.LoginId && (lev.Status == "APPLIED") && lev.IsActive == true && lev.IsDeleted == false
                                  select lev).OrderBy(x => x.Status).OrderByDescending(x => x.LeaveTypeId).ToList();
                }

                if (loginId != 0)
                {
                    if (levdetails != null)
                    {
                        List<EmpLeaveApplicationViewModel> lstoflve = new List<EmpLeaveApplicationViewModel>();

                        for (int i = 0; i < levdetails.Count(); i++)
                        {
                            EmpLeaveApplicationViewModel elavm = new EmpLeaveApplicationViewModel();
                            elavm.LeaveAppId = levdetails[i].LeaveAppId;
                            elavm.EmpId = levdetails[i].EmpId;
                            int? EmpId = levdetails[i].EmpId;
                            elavm.EmpCode = levdetails[i].EmpCode;
                            elavm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == EmpId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.LeaveTypeId = levdetails[i].LeaveTypeId;
                            int? leavetypeid = levdetails[i].LeaveTypeId;
                            if (leavetypeid == 0)
                            {
                                elavm.LeaveType = "LOP";
                            }
                            else
                            {
                                string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                                string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                    && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                                elavm.LeaveType = leavename + " - (" + shortname + ")";
                            }
                            elavm.StartDate = levdetails[i].StartDate;
                            elavm.EndDate = levdetails[i].EndDate;
                            elavm.Duration = levdetails[i].Duration;
                            elavm.Reason = levdetails[i].Reason;
                            elavm.Status = levdetails[i].Status;
                            string appstatus = levdetails[i].Status;

                            if (appstatus.ToUpper() == "APPROVED BY HR")
                            {
                                elavm.Status = "APPROVED";
                            }
                            else if (appstatus.ToUpper() == "APPROVED BY MANAGER")
                            {
                                elavm.Status = "APPROVED";
                            }

                            else if (appstatus.ToUpper() == "REJECTED BY HR")
                            {
                                elavm.Status = "REJECTED";
                            }
                            else if (appstatus.ToUpper() == "REJECTED BY MANAGER")
                            {
                                elavm.Status = "REJECTED";
                            }
                            else
                            {
                                elavm.Status = levdetails[i].Status;
                            }
                            if (levdetails[i].CompOffDate != null)
                            {
                                elavm.CompOffDate = levdetails[i].CompOffDate;
                            }
                            if (levdetails[i].CompOffReason != null)
                            {
                                elavm.CompOffReason = levdetails[i].CompOffReason;
                            }
                            if (levdetails[i].DocName != "")
                            {
                                elavm.DocName = levdetails[i].DocName;
                            }
                            elavm.AppliedDate = levdetails[i].AppliedDate;
                            elavm.ApprovedBy = levdetails[i].ApprovedBy;
                            int? ApproverId = levdetails[i].ApprovedBy;
                            elavm.Approver = DB.EmployeeMasters.Where(x => x.EmpId == ApproverId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.ApprovedDate = levdetails[i].ApprovedDate;
                            elavm.Remarks = levdetails[i].Remarks;
                            elavm.Createdby = levdetails[i].Createdby;
                            elavm.CreatedDate = levdetails[i].CreatedDate;
                            elavm.LastUpdatedBy = levdetails[i].LastUpdatedBy;
                            elavm.LastUpdatedDate = levdetails[i].LastUpdatedDate;
                            elavm.IsActive = levdetails[i].IsActive;
                            elavm.IsUpdated = levdetails[i].IsUpdated;
                            elavm.IsDeleted = levdetails[i].IsDeleted;
                            lstoflve.Add(elavm);

                        }
                        if (startDate != null && endDate != null)
                        {
                            var list = lstoflve.Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();
                            lstoflve = list.ToList();
                        }
                        if (status != "")
                        {
                            if (status == "Approved")
                            {
                                var list = lstoflve.Where(x => x.Status.ToUpper() == "APPROVED BY MANAGER" || x.Status.ToUpper() == "APPROVED BY HR").ToList();
                                lstoflve = list.ToList();
                            }
                            else if (status == "Rejected")
                            {
                                var list = lstoflve.Where(x => x.Status.ToUpper() == "REJECTED BY MANAGER" || x.Status.ToUpper() == "REJECTED BY HR").ToList();
                                lstoflve = list.ToList();
                            }
                            else if (status == "Cancelled")
                            {
                                var list = lstoflve.Where(x => x.Status.ToUpper() == "CANCELLED").ToList();
                                lstoflve = list.ToList();
                            }
                            else if (status == "WithDrawn")
                            {
                                var list = lstoflve.Where(x => x.Status.ToUpper() == "WITHDRAWN").ToList();
                                lstoflve = list.ToList();
                            }
                        }

                        return lstoflve;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Apply Details Not Found");
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
        public List<EmpLeaveApplicationViewModel> GetAllHRLeave(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                DateTime? startDate = model.StartDate;
                DateTime? endDate = model.EndDate;
                string status = model.Status;

                int locationid = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LocationId).FirstOrDefault() ?? 0;

                int desigId = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.DesignationId).FirstOrDefault() ?? 0;

                int deptId = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.CategoryId).FirstOrDefault() ?? 0;

                var alllevdetails = (from emp in DB.EmployeeMasters
                                     join lev in DB.EmpLeaveApplications on emp.EmpId equals lev.EmpId
                                     where emp.LocationId == locationid && (lev.Status != "APPLIED") && lev.IsActive == true && lev.IsDeleted == false
                                     select lev).OrderByDescending(x => x.LeaveTypeId).ToList();

                if (desigId == 186)
                {

                    alllevdetails = (from lev in DB.EmpLeaveApplications
                                     where (lev.Status != "APPLIED")  && lev.IsActive == true && lev.IsDeleted == false
                                     select lev).OrderByDescending(x => x.LeaveTypeId).ToList();
                }
                else if (deptId > 1)
                {
                    alllevdetails = (from lev in DB.EmpLeaveApplications
                                     where lev.ApprovedBy == loginId && (lev.Status != "APPLIED") && lev.IsActive == true && lev.IsDeleted == false
                                     select lev).OrderByDescending(x => x.LeaveTypeId).ToList();
                }

                if (loginId != 0)
                {
                    if (alllevdetails != null)
                    {
                        List<EmpLeaveApplicationViewModel> lstoflveall = new List<EmpLeaveApplicationViewModel>();

                        for (int i = 0; i < alllevdetails.Count(); i++)
                        {
                            EmpLeaveApplicationViewModel elavm = new EmpLeaveApplicationViewModel();
                            elavm.LeaveAppId = alllevdetails[i].LeaveAppId;
                            elavm.EmpId = alllevdetails[i].EmpId;
                            int? EmpId = alllevdetails[i].EmpId;
                            elavm.EmpCode = alllevdetails[i].EmpCode;
                            elavm.EmpName = DB.EmployeeMasters.Where(x => x.EmpId == EmpId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.LeaveTypeId = alllevdetails[i].LeaveTypeId;
                            int? leavetypeid = alllevdetails[i].LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            elavm.LeaveType = leavename + " - (" + shortname + ")";
                            elavm.StartDate = alllevdetails[i].StartDate;
                            elavm.EndDate = alllevdetails[i].EndDate;
                            elavm.Duration = alllevdetails[i].Duration;
                            elavm.Reason = alllevdetails[i].Reason;
                            elavm.Status = alllevdetails[i].Status;
                            elavm.Status = alllevdetails[i].Status;
                            string appstatus = alllevdetails[i].Status;

                            if (appstatus.ToUpper() == "APPROVED BY HR")
                            {
                                elavm.Status = "APPROVED";
                            }
                            else if (appstatus.ToUpper() == "APPROVED BY MANAGER")
                            {
                                elavm.Status = "APPROVED";
                            }

                            else if (appstatus.ToUpper() == "REJECTED BY HR")
                            {
                                elavm.Status = "REJECTED";
                            }
                            else if (appstatus.ToUpper() == "REJECTED BY MANAGER")
                            {
                                elavm.Status = "REJECTED";
                            }
                            else
                            {
                                elavm.Status = alllevdetails[i].Status;
                            }
                            if (alllevdetails[i].CompOffDate != null)
                            {
                                elavm.CompOffDate = alllevdetails[i].CompOffDate;
                            }
                            if (alllevdetails[i].CompOffReason != null)
                            {
                                elavm.CompOffReason = alllevdetails[i].CompOffReason;
                            }
                            if (alllevdetails[i].DocName != null)
                            {
                                if (alllevdetails[i].DocName != "")
                                {
                                    elavm.DocName = alllevdetails[i].DocName;
                                }
                            }
                            elavm.AppliedDate = alllevdetails[i].AppliedDate;
                            elavm.ApprovedBy = alllevdetails[i].ApprovedBy;
                            int? ApproverId = alllevdetails[i].ApprovedBy;
                            elavm.Approver = DB.EmployeeMasters.Where(x => x.EmpId == ApproverId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault() ?? "";
                            elavm.ApprovedDate = alllevdetails[i].ApprovedDate;
                            elavm.Remarks = alllevdetails[i].Remarks;
                            elavm.Createdby = alllevdetails[i].Createdby;
                            elavm.CreatedDate = alllevdetails[i].CreatedDate;
                            elavm.LastUpdatedBy = alllevdetails[i].LastUpdatedBy;
                            elavm.LastUpdatedDate = alllevdetails[i].LastUpdatedDate;
                            elavm.IsActive = alllevdetails[i].IsActive;
                            elavm.IsUpdated = alllevdetails[i].IsUpdated;
                            elavm.IsDeleted = alllevdetails[i].IsDeleted;
                            lstoflveall.Add(elavm);

                        }
                        if (startDate != null && endDate != null)
                        {
                            var list = lstoflveall.Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();
                            lstoflveall = list.ToList();
                        }
                        if (status != "")
                        {
                            if (status == "Approved")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "APPROVED BY MANAGER" || x.Status.ToUpper() == "APPROVED BY HR").ToList();
                                lstoflveall = list.ToList();
                            }
                            else if (status == "Rejected")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "REJECTED BY MANAGER" || x.Status.ToUpper() == "REJECTED BY HR").ToList();
                                lstoflveall = list.ToList();
                            }
                            else if (status == "Cancelled")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "CANCELLED").ToList();
                                lstoflveall = list.ToList();
                            }
                            else if (status == "WithDrawn")
                            {
                                var list = lstoflveall.Where(x => x.Status.ToUpper() == "WITHDRAWN").ToList();
                                lstoflveall = list.ToList();
                            }
                        }
                        return lstoflveall;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Leave Details Not Found");
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
        ////public LeaveResponseViewModel ApproveLeaveByHR(ApproveLeaveViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        if (loginId != 0)
        ////        {
        ////            if (model.lstofLevAppId.Count() > 0)
        ////            {
        ////                for (int i = 0; i < model.lstofLevAppId.Count; i++)
        ////                {
        ////                    var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

        ////                    var levdetails = (from lev in DB.EmpLeaveApplications
        ////                                      where lev.LeaveAppId == leaveAppId && (lev.Status == "APPLIED" || lev.Status == "APPROVED BY MANAGER")
        ////                                      && lev.IsActive == true && lev.IsDeleted == false
        ////                                      select lev).FirstOrDefault();

        ////                    if (levdetails != null)
        ////                    {
        ////                        levdetails.Status = "APPROVED BY HR";
        ////                        levdetails.Remarks = model.lstofLevAppId[i].Remarks;
        ////                        levdetails.ApprovedBy = model.LoginId;
        ////                        levdetails.ApprovedDate = DateTime.Now;
        ////                        levdetails.IsActive = true;
        ////                        levdetails.IsUpdated = true;
        ////                        levdetails.IsDeleted = false;
        ////                        levdetails.LastUpdatedBy = model.LoginId;
        ////                        levdetails.LastUpdatedDate = DateTime.Now;
        ////                        DB.SaveChanges();

        ////                        var managerdetails = (from lev in DB.EmployeeMasters
        ////                                              where lev.EmpId == model.EmpId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();


        ////                        var userdetails = (from lev in DB.EmployeeMasters
        ////                                           where lev.EmpId == levdetails.EmpId
        ////                                           && lev.IsActive == true && lev.IsDeleted == false
        ////                                           select lev).FirstOrDefault();

        ////                        var actualHRdetails = (from lev in DB.EmailConfigMasters
        ////                                               where lev.Name.ToUpper() == "LEAVE"
        ////                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
        ////                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
        ////                                               && lev.IsActive == true && lev.IsDeleted == false
        ////                                               select lev).FirstOrDefault();

        ////                        var HRuserdetails = (from lev in DB.EmployeeMasters
        ////                                             where lev.EmpId == 149
        ////                                             && lev.IsActive == true && lev.IsDeleted == false
        ////                                             select lev).FirstOrDefault();

        ////                        string HRmailid = "";

        ////                        ////if (actualHRdetails != null)
        ////                        ////{
        ////                        ////    HRmailid = actualHRdetails.EmailId;
        ////                        ////}
        ////                        ////else
        ////                        ////{
        ////                        ////    HRmailid = HRuserdetails.EmailId;
        ////                        ////}

        ////                        var levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        string shortname = "";

        ////                        if (levdetails.LeaveTypeId == 0)
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = "LOP";
        ////                        }
        ////                        else
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = levtypedetails.ShortName;
        ////                        }

        ////                        string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
        ////                        string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

        ////                        string to = userdetails.EmailId;
        ////                        string cc = HRmailid;
        ////                        string subject = "Office Connect - Leave Request Update";
        ////                        string body = $@"
        ////                        <p>Dear {userdetails.FirstName},</p>
        ////                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Approved by your HR (<strong>{managerdetails.FirstName}</strong>).</p>
        ////                        <p></p>
        ////                        <p>You can view the updated status of your application in the Office Connect portal.</p>
        ////                        <p></p>
        ////                        <p>Best regards,</p>
        ////                        <p>Office Connect.</p>";

        ////                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

        ////                        // ========== NEW: ADD NOTIFICATION ==========
        ////                        Task.Run(async () =>
        ////                        {
        ////                            await _notificationService.CreateLeaveApprovedByHRNotification(leaveAppId, model.LoginId);
        ////                        });
        ////                    }
        ////                    else
        ////                    {
        ////                        throw new CustomApiException(HttpStatusCode.NotFound, "Applied Leave Details Not Found");
        ////                    }
        ////                }
        ////            }
        ////            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
        ////            emvm.Status = 200;
        ////            emvm.msg = "Approved By HR";

        ////            return emvm;
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        /// -- 05.03.2026
        /// 
        ////public LeaveResponseViewModel ApproveLeaveByHR(ApproveLeaveViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        if (loginId != 0)
        ////        {
        ////            if (model.lstofLevAppId.Count() > 0)
        ////            {
        ////                List<int> approvedIds = new List<int>();
        ////                List<int> notFoundIds = new List<int>();
        ////                List<string> errorMessages = new List<string>();

        ////                for (int i = 0; i < model.lstofLevAppId.Count; i++)
        ////                {
        ////                    var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

        ////                    var levdetails = (from lev in DB.EmpLeaveApplications
        ////                                      where lev.LeaveAppId == leaveAppId && (lev.Status == "APPLIED" || lev.Status == "APPROVED BY MANAGER")
        ////                                      && lev.IsActive == true && lev.IsDeleted == false
        ////                                      select lev).FirstOrDefault();

        ////                    if (levdetails != null)
        ////                    {
        ////                        // Process the approval
        ////                        levdetails.Status = "APPROVED BY HR";
        ////                        levdetails.Remarks = model.lstofLevAppId[i].Remarks;
        ////                        levdetails.ApprovedBy = model.LoginId;
        ////                        levdetails.ApprovedDate = DateTime.Now;
        ////                        levdetails.IsActive = true;
        ////                        levdetails.IsUpdated = true;
        ////                        levdetails.IsDeleted = false;
        ////                        levdetails.LastUpdatedBy = model.LoginId;
        ////                        levdetails.LastUpdatedDate = DateTime.Now;
        ////                        DB.SaveChanges();

        ////                        // Add to approved list
        ////                        approvedIds.Add(leaveAppId);

        ////                        // Rest of your existing code for notifications and emails
        ////                        var managerdetails = (from lev in DB.EmployeeMasters
        ////                                              where lev.EmpId == model.EmpId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        var userdetails = (from lev in DB.EmployeeMasters
        ////                                           where lev.EmpId == levdetails.EmpId
        ////                                           && lev.IsActive == true && lev.IsDeleted == false
        ////                                           select lev).FirstOrDefault();

        ////                        var actualHRdetails = (from lev in DB.EmailConfigMasters
        ////                                               where lev.Name.ToUpper() == "LEAVE"
        ////                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
        ////                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
        ////                                               && lev.IsActive == true && lev.IsDeleted == false
        ////                                               select lev).FirstOrDefault();

        ////                        var HRuserdetails = (from lev in DB.EmployeeMasters
        ////                                             where lev.EmpId == 149
        ////                                             && lev.IsActive == true && lev.IsDeleted == false
        ////                                             select lev).FirstOrDefault();

        ////                        string HRmailid = "";

        ////                        var levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        string shortname = "";

        ////                        if (levdetails.LeaveTypeId == 0)
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = "LOP";
        ////                        }
        ////                        else
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = levtypedetails.ShortName;
        ////                        }

        ////                        string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
        ////                        string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

        ////                        string to = userdetails.EmailId;
        ////                        string cc = HRmailid;
        ////                        string subject = "Office Connect - Leave Request Update";
        ////                        string body = $@"
        ////                <p>Dear {userdetails.FirstName},</p>
        ////                <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Approved by your HR (<strong>{managerdetails.FirstName}</strong>).</p>
        ////                <p></p>
        ////                <p>You can view the updated status of your application in the Office Connect portal.</p>
        ////                <p></p>
        ////                <p>Best regards,</p>
        ////                <p>Office Connect.</p>";

        ////                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

        ////                        // ========== NEW: ADD NOTIFICATION ==========
        ////                        Task.Run(async () =>
        ////                        {
        ////                            await _notificationService.CreateLeaveApprovedByHRNotification(leaveAppId, model.LoginId);
        ////                        });
        ////                    }
        ////                    else
        ////                    {
        ////                        // Add to not found list
        ////                        notFoundIds.Add(leaveAppId);
        ////                        errorMessages.Add($"Leave application ID {leaveAppId} not found or not in approvable status");
        ////                    }
        ////                }

        ////                // Prepare response based on results
        ////                LeaveResponseViewModel emvm = new LeaveResponseViewModel();

        ////                if (approvedIds.Count > 0 && notFoundIds.Count == 0)
        ////                {
        ////                    // All approved
        ////                    emvm.Status = 200;
        ////                    emvm.msg = $"Successfully approved {approvedIds.Count} leave applications";
        ////                }
        ////                else if (approvedIds.Count > 0 && notFoundIds.Count > 0)
        ////                {
        ////                    // Partial success
        ////                    emvm.Status = 206; // Partial Content status code
        ////                    emvm.msg = $"Approved {approvedIds.Count} leave applications. Failed to approve {notFoundIds.Count} applications.";
        ////                    emvm.FailedIds = notFoundIds;
        ////                    emvm.Errors = errorMessages;
        ////                }
        ////                else if (approvedIds.Count == 0 && notFoundIds.Count > 0)
        ////                {
        ////                    // Complete failure
        ////                    emvm.Status = 404;
        ////                    emvm.msg = "No leave applications could be approved";
        ////                    emvm.FailedIds = notFoundIds;
        ////                    emvm.Errors = errorMessages;
        ////                }

        ////                return emvm;
        ////            }
        ////            else
        ////            {
        ////                LeaveResponseViewModel emvm = new LeaveResponseViewModel();
        ////                emvm.Status = 400;
        ////                emvm.msg = "No leave applications selected";
        ////                return emvm;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
        ////            emvm.Status = 400;
        ////            emvm.msg = "LoginId is Invalid";
        ////            return emvm;
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new CustomApiException(HttpStatusCode.InternalServerError, ex.Message);
        ////    }
        ////}
        public LeaveResponseViewModel ApproveLeaveByHR(ApproveLeaveViewModel model)
        {
            // Use a transaction to ensure all or nothing
            using (var transaction = DB.Database.BeginTransaction())
            {
                try
                {
                    string msg = "";
                    int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                    int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                    if (loginId != 0)
                    {
                        if (model.lstofLevAppId != null && model.lstofLevAppId.Any())
                        {
                            List<int> approvedIds = new List<int>();
                            List<int> notFoundIds = new List<int>();
                            List<string> errorMessages = new List<string>();

                            foreach (var item in model.lstofLevAppId)
                            {
                                var leaveAppId = item.LeaveAppId;

                                var levdetails = DB.EmpLeaveApplications
                                    .FirstOrDefault(lev => lev.LeaveAppId == leaveAppId
                                        && (lev.Status == "APPLIED" || lev.Status == "APPROVED BY MANAGER")
                                        && lev.IsActive == true
                                        && lev.IsDeleted == false);

                                if (levdetails != null)
                                {
                                    try
                                    {
                                        // Process the approval
                                        levdetails.Status = "APPROVED BY HR";
                                        levdetails.Remarks = item.Remarks;
                                        levdetails.ApprovedBy = model.LoginId;
                                        levdetails.ApprovedDate = DateTime.Now;
                                        levdetails.IsActive = true;
                                        levdetails.IsUpdated = true;
                                        levdetails.IsDeleted = false;
                                        levdetails.LastUpdatedBy = model.LoginId;
                                        levdetails.LastUpdatedDate = DateTime.Now;

                                        // Don't save here - save after all updates
                                        approvedIds.Add(leaveAppId);
                                    }
                                    catch (Exception ex)
                                    {
                                        errorMessages.Add($"Error processing LeaveAppId {leaveAppId}: {ex.Message}");
                                        notFoundIds.Add(leaveAppId);
                                    }
                                }
                                else
                                {
                                    notFoundIds.Add(leaveAppId);
                                    errorMessages.Add($"Leave application ID {leaveAppId} not found or not in approvable status");
                                }
                            }

                            // Save all changes at once if there are any approved items
                            if (approvedIds.Any())
                            {
                                DB.SaveChanges();
                                transaction.Commit();

                                // Send emails and notifications after successful save
                                foreach (var leaveAppId in approvedIds)
                                {
                                    try
                                    {
                                        // Fetch the updated leave details
                                        var levdetails = DB.EmpLeaveApplications
                                            .FirstOrDefault(lev => lev.LeaveAppId == leaveAppId);

                                        if (levdetails != null)
                                        {
                                            // Get manager details
                                            var managerdetails = DB.EmployeeMasters
                                                .FirstOrDefault(lev => lev.EmpId == model.EmpId
                                                    && lev.IsActive == true
                                                    && lev.IsDeleted == false);

                                            // Get user details
                                            var userdetails = DB.EmployeeMasters
                                                .FirstOrDefault(lev => lev.EmpId == levdetails.EmpId
                                                    && lev.IsActive == true
                                                    && lev.IsDeleted == false);

                                            if (userdetails != null)
                                            {
                                                // Get leave type details
                                                string shortname = "LOP";
                                                if (levdetails.LeaveTypeId > 0)
                                                {
                                                    var levtypedetails = DB.LeaveTypeMasters
                                                        .FirstOrDefault(lev => lev.LeaveTypeId == levdetails.LeaveTypeId
                                                            && lev.IsActive == true
                                                            && lev.IsDeleted == false);
                                                    shortname = levtypedetails?.ShortName ?? "LOP";
                                                }

                                                string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd") ?? "";
                                                string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd") ?? "";

                                                // Get HR email (commented out as in original)
                                                string HRmailid = "";

                                                string to = userdetails.EmailId;
                                                string subject = "Office Connect - Leave Request Update";
                                                string body = $@"
                                            <p>Dear {userdetails.FirstName},</p>
                                            <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Approved by your HR (<strong>{managerdetails?.FirstName ?? "HR"}</strong>).</p>
                                            <p></p>
                                            <p>You can view the updated status of your application in the Office Connect portal.</p>
                                            <p></p>
                                            <p>Best regards,</p>
                                            <p>Office Connect.</p>";

                                                // Fire and forget email and notification
                                                Task.Run(() => SendLeaveMail(to, HRmailid, subject, body));
                                                Task.Run(async () =>
                                                {
                                                    await _notificationService.CreateLeaveApprovedByHRNotification(leaveAppId, Convert.ToInt32(model.LoginId));
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log but don't fail the main operation
                                        Console.WriteLine($"Error sending notification for LeaveAppId {leaveAppId}: {ex.Message}");
                                    }
                                }
                            }

                            // Prepare response based on results
                            LeaveResponseViewModel emvm = new LeaveResponseViewModel();

                            if (approvedIds.Count > 0 && notFoundIds.Count == 0)
                            {
                                emvm.Status = 200;
                                emvm.msg = $"Successfully approved {approvedIds.Count} leave applications";
                                emvm.ApprovedIds = approvedIds;
                            }
                            else if (approvedIds.Count > 0 && notFoundIds.Count > 0)
                            {
                                emvm.Status = 206;
                                emvm.msg = $"Approved {approvedIds.Count} leave applications. Failed to approve {notFoundIds.Count} applications.";
                                emvm.ApprovedIds = approvedIds;
                                emvm.FailedIds = notFoundIds;
                                emvm.Errors = errorMessages;
                            }
                            else if (approvedIds.Count == 0 && notFoundIds.Count > 0)
                            {
                                emvm.Status = 404;
                                emvm.msg = "No leave applications could be approved";
                                emvm.FailedIds = notFoundIds;
                                emvm.Errors = errorMessages;
                            }

                            return emvm;
                        }
                        else
                        {
                            return new LeaveResponseViewModel
                            {
                                Status = 400,
                                msg = "No leave applications selected"
                            };
                        }
                    }
                    else
                    {
                        return new LeaveResponseViewModel
                        {
                            Status = 400,
                            msg = "LoginId is Invalid"
                        };
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomApiException(HttpStatusCode.InternalServerError,
                        $"Error processing leave approvals: {ex.Message}");
                }
            }
        }
        public LeaveResponseViewModel RejectLeaveByHR(ApproveLeaveViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                if (loginId != 0)
                {
                    if (model.lstofLevAppId.Count() > 0)
                    {
                        List<int> approvedIds = new List<int>();
                        List<int> notFoundIds = new List<int>();
                        List<string> errorMessages = new List<string>();

                        for (int i = 0; i < model.lstofLevAppId.Count; i++)
                        {
                            var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

                            var levdetails = (from lev in DB.EmpLeaveApplications
                                              where lev.LeaveAppId == leaveAppId && (lev.Status == "APPLIED" || lev.Status == "APPROVED BY MANAGER")
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();

                            if (levdetails != null)
                            {
                                int? LeaveTypeId = levdetails.LeaveTypeId;
                                int? UserId = levdetails.EmpId;

                                levdetails.Status = "REJECTED BY HR";
                                levdetails.Remarks = model.lstofLevAppId[i].Remarks;
                                levdetails.ApprovedBy = model.LoginId;
                                levdetails.ApprovedDate = DateTime.Now;
                                levdetails.IsActive = true;
                                levdetails.IsUpdated = true;
                                levdetails.IsDeleted = false;
                                levdetails.LastUpdatedBy = model.LoginId;
                                levdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                DateTime Today = DateTime.Now;
                                int? Year = Today.Year;
                                int? Month = Today.Month;

                                var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                            where lev.EmpId == UserId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                            && lev.IsActive == true && lev.IsDeleted == false
                                                            select lev).FirstOrDefault();

                                if (levcarryFrowddetails != null)
                                {
                                    decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                                    decimal? avail = levcarryFrowddetails.Availed ?? 0;
                                    decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                                    decimal? dayscount = levdetails.Duration;

                                    bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

                                    int? maxmdays = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                    && x.IsDeleted == false).Select(x => x.MaxPerYear).FirstOrDefault();

                                    levcarryFrowddetails.OpeningBalance = (open);
                                    levcarryFrowddetails.Availed = (avail - dayscount);
                                    levcarryFrowddetails.ClosingBalance = (close + dayscount);
                                    if (SingleApp == true)
                                    {
                                        levcarryFrowddetails.OpeningBalance = maxmdays;
                                        levcarryFrowddetails.Availed = 0;
                                        levcarryFrowddetails.ClosingBalance = maxmdays;
                                    }

                                    levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                                    levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                                    levcarryFrowddetails.IsActive = true;
                                    levcarryFrowddetails.IsUpdated = true;
                                    levcarryFrowddetails.IsDeleted = false;
                                    DB.SaveChanges();
                                }

                                var managerdetails = (from lev in DB.EmployeeMasters
                                                      where lev.EmpId == model.EmpId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                var userdetails = (from lev in DB.EmployeeMasters
                                                   where lev.EmpId == levdetails.EmpId
                                                   && lev.IsActive == true && lev.IsDeleted == false
                                                   select lev).FirstOrDefault();

                                var actualHRdetails = (from lev in DB.EmailConfigMasters
                                                       where lev.Name.ToUpper() == "LEAVE"
                                                       && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                                       && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                                       && lev.IsActive == true && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                var HRuserdetails = (from lev in DB.EmployeeMasters
                                                     where lev.EmpId == 149
                                                     && lev.IsActive == true && lev.IsDeleted == false
                                                     select lev).FirstOrDefault();

                                string HRmailid = "";

                                var levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                string shortname = "";

                                if (levdetails.LeaveTypeId == 0)
                                {
                                    levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.LeaveTypeId == levdetails.LeaveTypeId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();
                                    shortname = "LOP";
                                }
                                else
                                {
                                    levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.LeaveTypeId == levdetails.LeaveTypeId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();
                                    shortname = levtypedetails.ShortName;
                                }

                                string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
                                string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

                                string to = userdetails.EmailId;
                                string cc = HRmailid;
                                string subject = "Office Connect - Leave Request Update";
                                string body = $@"
                        <p>Dear {userdetails.FirstName},</p>
                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Rejected by your HR (<strong>{managerdetails.FirstName}</strong>).</p>
                        <p></p>
                        <p>You can view the updated status of your application in the Office Connect portal.</p>
                        <p></p>
                        <p>Best regards,</p>
                        <p>Office Connect.</p>";

                                Task.Run(() => SendLeaveMail(to, cc, subject, body));

                                // ========== NEW: ADD NOTIFICATION ==========
                                Task.Run(async () =>
                                {
                                    await _notificationService.CreateLeaveRejectedByHRNotification(leaveAppId, model.LoginId);
                                });

                                approvedIds.Add(leaveAppId);
                            }
                            else
                            {
                                notFoundIds.Add(leaveAppId);
                                errorMessages.Add($"Leave application ID {leaveAppId} not found or not in approvable status");
                            }
                        }

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();

                        if (approvedIds.Count > 0 && notFoundIds.Count == 0)
                        {
                            emvm.Status = 200;
                            emvm.msg = $"Successfully rejected {approvedIds.Count} leave applications";
                        }
                        else if (approvedIds.Count > 0 && notFoundIds.Count > 0)
                        {
                            emvm.Status = 206;
                            emvm.msg = $"Rejected {approvedIds.Count} leave applications. Failed to reject {notFoundIds.Count} applications.";
                            emvm.FailedIds = notFoundIds;
                            emvm.Errors = errorMessages;
                        }
                        else if (approvedIds.Count == 0 && notFoundIds.Count > 0)
                        {
                            emvm.Status = 404;
                            emvm.msg = "No leave applications could be rejected";
                            emvm.FailedIds = notFoundIds;
                            emvm.Errors = errorMessages;
                        }

                        return emvm;
                    }
                    else
                    {
                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 400;
                        emvm.msg = "No leave applications selected";
                        return emvm;
                    }
                }
                else
                {
                    LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                    emvm.Status = 400;
                    emvm.msg = "LoginId is Invalid";
                    return emvm;
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
        public LeaveResponseViewModel ApproveLeaveByManager(ApproveLeaveViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                if (loginId != 0)
                {
                    if (model.lstofLevAppId.Count() > 0)
                    {
                        List<int> approvedIds = new List<int>();
                        List<int> notFoundIds = new List<int>();
                        List<string> errorMessages = new List<string>();

                        for (int i = 0; i < model.lstofLevAppId.Count; i++)
                        {
                            var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

                            var levdetails = (from lev in DB.EmpLeaveApplications
                                              where lev.LeaveAppId == leaveAppId && lev.Status == "APPLIED"
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();

                            if (levdetails != null)
                            {
                                levdetails.Status = "APPROVED BY MANAGER";
                                levdetails.Remarks = model.lstofLevAppId[i].Remarks;
                                levdetails.ApprovedBy = model.LoginId;
                                levdetails.ApprovedDate = DateTime.Now;
                                levdetails.IsActive = true;
                                levdetails.IsUpdated = true;
                                levdetails.IsDeleted = false;
                                levdetails.LastUpdatedBy = model.LoginId;
                                levdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                var managerdetails = (from lev in DB.EmployeeMasters
                                                      where lev.EmpId == model.EmpId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                var userdetails = (from lev in DB.EmployeeMasters
                                                   where lev.EmpId == levdetails.EmpId
                                                   && lev.IsActive == true && lev.IsDeleted == false
                                                   select lev).FirstOrDefault();

                                var actualHRdetails = (from lev in DB.EmailConfigMasters
                                                       where lev.Name.ToUpper() == "LEAVE"
                                                       && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                                       && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                                       && lev.IsActive == true && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                var HRuserdetails = (from lev in DB.EmployeeMasters
                                                     where lev.EmpId == 149
                                                     && lev.IsActive == true && lev.IsDeleted == false
                                                     select lev).FirstOrDefault();

                                string HRmailid = "";

                                var levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                string shortname = "";

                                if (levdetails.LeaveTypeId == 0)
                                {
                                    levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.LeaveTypeId == levdetails.LeaveTypeId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();
                                    shortname = "LOP";
                                }
                                else
                                {
                                    levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.LeaveTypeId == levdetails.LeaveTypeId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();
                                    shortname = levtypedetails.ShortName;
                                }

                                string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
                                string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

                                string to = userdetails.EmailId;
                                string cc = HRmailid;
                                string subject = "Office Connect - Leave Request Update";
                                string body = $@"
                        <p>Dear {userdetails.FirstName},</p>
                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Approved by your manager (<strong>{managerdetails.FirstName}</strong>).</p>
                        <p></p>
                        <p>You can view the updated status of your application in the Office Connect portal.</p>
                        <p></p>
                        <p>Best regards,</p>
                        <p>Office Connect.</p>";

                                Task.Run(() => SendLeaveMail(to, cc, subject, body));

                                // ========== NEW: ADD NOTIFICATION ==========
                                Task.Run(async () =>
                                {
                                    await _notificationService.CreateLeaveApprovedByManagerNotification(leaveAppId, model.LoginId);
                                });

                                approvedIds.Add(leaveAppId);
                            }
                            else
                            {
                                notFoundIds.Add(leaveAppId);
                                errorMessages.Add($"Leave application ID {leaveAppId} not found or not in APPLIED status");
                            }
                        }

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();

                        if (approvedIds.Count > 0 && notFoundIds.Count == 0)
                        {
                            emvm.Status = 200;
                            emvm.msg = $"Successfully approved {approvedIds.Count} leave applications";
                        }
                        else if (approvedIds.Count > 0 && notFoundIds.Count > 0)
                        {
                            emvm.Status = 206;
                            emvm.msg = $"Approved {approvedIds.Count} leave applications. Failed to approve {notFoundIds.Count} applications.";
                            emvm.FailedIds = notFoundIds;
                            emvm.Errors = errorMessages;
                        }
                        else if (approvedIds.Count == 0 && notFoundIds.Count > 0)
                        {
                            emvm.Status = 404;
                            emvm.msg = "No leave applications could be approved";
                            emvm.FailedIds = notFoundIds;
                            emvm.Errors = errorMessages;
                        }

                        return emvm;
                    }
                    else
                    {
                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 400;
                        emvm.msg = "No leave applications selected";
                        return emvm;
                    }
                }
                else
                {
                    LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                    emvm.Status = 400;
                    emvm.msg = "LoginId is Invalid";
                    return emvm;
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
        public LeaveResponseViewModel RejectLeaveByManager(ApproveLeaveViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                if (loginId != 0)
                {
                    if (model.lstofLevAppId.Count() > 0)
                    {
                        List<int> approvedIds = new List<int>();
                        List<int> notFoundIds = new List<int>();
                        List<string> errorMessages = new List<string>();

                        for (int i = 0; i < model.lstofLevAppId.Count; i++)
                        {
                            var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

                            var levdetails = (from lev in DB.EmpLeaveApplications
                                              where lev.LeaveAppId == leaveAppId && lev.Status == "APPLIED"
                                              && lev.IsActive == true && lev.IsDeleted == false
                                              select lev).FirstOrDefault();

                            if (levdetails != null)
                            {
                                int? LeaveTypeId = levdetails.LeaveTypeId;
                                int? UserId = levdetails.EmpId;

                                levdetails.Status = "REJECTED BY MANAGER";
                                levdetails.Remarks = model.lstofLevAppId[i].Remarks;
                                levdetails.ApprovedBy = model.LoginId;
                                levdetails.ApprovedDate = DateTime.Now;
                                levdetails.IsActive = true;
                                levdetails.IsUpdated = true;
                                levdetails.IsDeleted = false;
                                levdetails.LastUpdatedBy = model.LoginId;
                                levdetails.LastUpdatedDate = DateTime.Now;
                                DB.SaveChanges();

                                DateTime Today = DateTime.Now;
                                int? Year = Today.Year;
                                int? Month = Today.Month;

                                var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
                                                            where lev.EmpId == UserId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
                                                            && lev.IsActive == true && lev.IsDeleted == false
                                                            select lev).FirstOrDefault();

                                if (levcarryFrowddetails != null)
                                {
                                    decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
                                    decimal? avail = levcarryFrowddetails.Availed ?? 0;
                                    decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
                                    decimal? dayscount = levdetails.Duration;

                                    bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

                                    int? maxmdays = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
                                                    && x.IsDeleted == false).Select(x => x.MaxPerYear).FirstOrDefault();

                                    levcarryFrowddetails.OpeningBalance = (open);
                                    levcarryFrowddetails.Availed = (avail - dayscount);
                                    levcarryFrowddetails.ClosingBalance = (close + dayscount);
                                    if (SingleApp == true)
                                    {
                                        levcarryFrowddetails.OpeningBalance = maxmdays;
                                        levcarryFrowddetails.Availed = 0;
                                        levcarryFrowddetails.ClosingBalance = maxmdays;
                                    }

                                    levcarryFrowddetails.LastUpdatedBy = model.LoginId;
                                    levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
                                    levcarryFrowddetails.IsActive = true;
                                    levcarryFrowddetails.IsUpdated = true;
                                    levcarryFrowddetails.IsDeleted = false;
                                    DB.SaveChanges();
                                }

                                var managerdetails = (from lev in DB.EmployeeMasters
                                                      where lev.EmpId == model.EmpId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                var userdetails = (from lev in DB.EmployeeMasters
                                                   where lev.EmpId == levdetails.EmpId
                                                   && lev.IsActive == true && lev.IsDeleted == false
                                                   select lev).FirstOrDefault();

                                var actualHRdetails = (from lev in DB.EmailConfigMasters
                                                       where lev.Name.ToUpper() == "LEAVE"
                                                       && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
                                                       && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
                                                       && lev.IsActive == true && lev.IsDeleted == false
                                                       select lev).FirstOrDefault();

                                var HRuserdetails = (from lev in DB.EmployeeMasters
                                                     where lev.EmpId == 149
                                                     && lev.IsActive == true && lev.IsDeleted == false
                                                     select lev).FirstOrDefault();

                                string HRmailid = "";

                                var levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();

                                string shortname = "";

                                if (levdetails.LeaveTypeId == 0)
                                {
                                    levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.LeaveTypeId == levdetails.LeaveTypeId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();
                                    shortname = "LOP";
                                }
                                else
                                {
                                    levtypedetails = (from lev in DB.LeaveTypeMasters
                                                      where lev.LeaveTypeId == levdetails.LeaveTypeId
                                                      && lev.IsActive == true && lev.IsDeleted == false
                                                      select lev).FirstOrDefault();
                                    shortname = levtypedetails.ShortName;
                                }

                                string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
                                string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

                                string to = userdetails.EmailId;
                                string cc = HRmailid;
                                string subject = "Office Connect - Leave Request Update";
                                string body = $@"
                        <p>Dear {userdetails.FirstName},</p>
                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Rejected by your manager (<strong>{managerdetails.FirstName}</strong>).</p>
                        <p></p>
                        <p>You can view the updated status of your application in the Office Connect portal.</p>
                        <p></p>
                        <p>Best regards,</p>
                        <p>Office Connect.</p>";

                                Task.Run(() => SendLeaveMail(to, cc, subject, body));

                                // ========== NEW: ADD NOTIFICATION ==========
                                Task.Run(async () =>
                                {
                                    await _notificationService.CreateLeaveRejectedByManagerNotification(leaveAppId, model.LoginId);
                                });

                                approvedIds.Add(leaveAppId);
                            }
                            else
                            {
                                notFoundIds.Add(leaveAppId);
                                errorMessages.Add($"Leave application ID {leaveAppId} not found or not in APPLIED status");
                            }
                        }

                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();

                        if (approvedIds.Count > 0 && notFoundIds.Count == 0)
                        {
                            emvm.Status = 200;
                            emvm.msg = $"Successfully rejected {approvedIds.Count} leave applications";
                        }
                        else if (approvedIds.Count > 0 && notFoundIds.Count > 0)
                        {
                            emvm.Status = 206;
                            emvm.msg = $"Rejected {approvedIds.Count} leave applications. Failed to reject {notFoundIds.Count} applications.";
                            emvm.FailedIds = notFoundIds;
                            emvm.Errors = errorMessages;
                        }
                        else if (approvedIds.Count == 0 && notFoundIds.Count > 0)
                        {
                            emvm.Status = 404;
                            emvm.msg = "No leave applications could be rejected";
                            emvm.FailedIds = notFoundIds;
                            emvm.Errors = errorMessages;
                        }

                        return emvm;
                    }
                    else
                    {
                        LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                        emvm.Status = 400;
                        emvm.msg = "No leave applications selected";
                        return emvm;
                    }
                }
                else
                {
                    LeaveResponseViewModel emvm = new LeaveResponseViewModel();
                    emvm.Status = 400;
                    emvm.msg = "LoginId is Invalid";
                    return emvm;
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

        ////public LeaveResponseViewModel RejectLeaveByHR(ApproveLeaveViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        if (loginId != 0)
        ////        {
        ////            if (model.lstofLevAppId.Count() > 0)
        ////            {
        ////                for (int i = 0; i < model.lstofLevAppId.Count; i++)
        ////                {
        ////                    var leaveAppId = model.lstofLevAppId[i].LeaveAppId;

        ////                    var levdetails = (from lev in DB.EmpLeaveApplications
        ////                                      where lev.LeaveAppId == leaveAppId && (lev.Status == "APPLIED" || lev.Status == "APPROVED BY MANAGER")
        ////                                      && lev.IsActive == true && lev.IsDeleted == false
        ////                                      select lev).FirstOrDefault();

        ////                    int? LeaveTypeId = levdetails.LeaveTypeId;
        ////                    int? UserId = levdetails.EmpId;

        ////                    if (levdetails != null)
        ////                    {
        ////                        levdetails.Status = "REJECTED BY HR";
        ////                        levdetails.Remarks = model.lstofLevAppId[i].Remarks;
        ////                        levdetails.ApprovedBy = model.LoginId;
        ////                        levdetails.ApprovedDate = DateTime.Now;
        ////                        levdetails.IsActive = true;
        ////                        levdetails.IsUpdated = true;
        ////                        levdetails.IsDeleted = false;
        ////                        levdetails.LastUpdatedBy = model.LoginId;
        ////                        levdetails.LastUpdatedDate = DateTime.Now;
        ////                        DB.SaveChanges();

        ////                        DateTime Today = DateTime.Now;
        ////                        int? Year = Today.Year;
        ////                        int? Month = Today.Month;

        ////                        var levcarryFrowddetails = (from lev in DB.LeaveCarryForwardMasters
        ////                                                    where lev.EmpId == UserId && (lev.LeaveMonth == Month || lev.LeaveYear == Year) && lev.LeaveTypeId == LeaveTypeId
        ////                                                    && lev.IsActive == true && lev.IsDeleted == false
        ////                                                    select lev).FirstOrDefault();

        ////                        if (levcarryFrowddetails != null)
        ////                        {
        ////                            decimal? open = levcarryFrowddetails.OpeningBalance ?? 0;
        ////                            decimal? avail = levcarryFrowddetails.Availed ?? 0;
        ////                            decimal? close = levcarryFrowddetails.ClosingBalance ?? 0;
        ////                            decimal? dayscount = levdetails.Duration;

        ////                            bool? SingleApp = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
        ////                                        && x.IsDeleted == false).Select(x => x.IsSingleApplication).FirstOrDefault();

        ////                            int? maxmdays = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == LeaveTypeId && x.IsActive == true
        ////                                            && x.IsDeleted == false).Select(x => x.MaxPerYear).FirstOrDefault();

        ////                            levcarryFrowddetails.OpeningBalance = (open);
        ////                            levcarryFrowddetails.Availed = (avail - dayscount);
        ////                            levcarryFrowddetails.ClosingBalance = (close + dayscount);
        ////                            if (SingleApp == true)
        ////                            {

        ////                                levcarryFrowddetails.OpeningBalance = maxmdays;
        ////                                levcarryFrowddetails.Availed = 0;
        ////                                levcarryFrowddetails.ClosingBalance = maxmdays;
        ////                            }

        ////                            levcarryFrowddetails.LastUpdatedBy = model.LoginId;
        ////                            levcarryFrowddetails.LastUpdatedDate = DateTime.Now;
        ////                            levcarryFrowddetails.IsActive = true;
        ////                            levcarryFrowddetails.IsUpdated = true;
        ////                            levcarryFrowddetails.IsDeleted = false;
        ////                            DB.SaveChanges();
        ////                        }
        ////                        else
        ////                        {

        ////                        }

        ////                        var managerdetails = (from lev in DB.EmployeeMasters
        ////                                              where lev.EmpId == model.EmpId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();


        ////                        var userdetails = (from lev in DB.EmployeeMasters
        ////                                           where lev.EmpId == levdetails.EmpId
        ////                                           && lev.IsActive == true && lev.IsDeleted == false
        ////                                           select lev).FirstOrDefault();

        ////                        var actualHRdetails = (from lev in DB.EmailConfigMasters
        ////                                               where lev.Name.ToUpper() == "LEAVE"
        ////                                               && lev.CompId == userdetails.CompId && lev.LEId == userdetails.LEId
        ////                                               && lev.BUId == userdetails.BUId && lev.LocId == userdetails.LocationId
        ////                                               && lev.IsActive == true && lev.IsDeleted == false
        ////                                               select lev).FirstOrDefault();

        ////                        var HRuserdetails = (from lev in DB.EmployeeMasters
        ////                                             where lev.EmpId == 149
        ////                                             && lev.IsActive == true && lev.IsDeleted == false
        ////                                             select lev).FirstOrDefault();

        ////                        string HRmailid = "";

        ////                        ////if (actualHRdetails != null)
        ////                        ////{
        ////                        ////    HRmailid = actualHRdetails.EmailId;
        ////                        ////}
        ////                        ////else
        ////                        ////{
        ////                        ////    HRmailid = HRuserdetails.EmailId;
        ////                        ////}

        ////                        var levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();

        ////                        string shortname = "";

        ////                        if (levdetails.LeaveTypeId == 0)
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = "LOP";
        ////                        }
        ////                        else
        ////                        {
        ////                            levtypedetails = (from lev in DB.LeaveTypeMasters
        ////                                              where lev.LeaveTypeId == levdetails.LeaveTypeId
        ////                                              && lev.IsActive == true && lev.IsDeleted == false
        ////                                              select lev).FirstOrDefault();
        ////                            shortname = levtypedetails.ShortName;
        ////                        }

        ////                        string startDateOnly = levdetails.StartDate?.ToString("yyyy-MM-dd");
        ////                        string endDateOnly = levdetails.EndDate?.ToString("yyyy-MM-dd");

        ////                        string to = userdetails.EmailId;
        ////                        string cc = HRmailid;
        ////                        string subject = "Office Connect - Leave Request Update";
        ////                        string body = $@"
        ////                        <p>Dear {userdetails.FirstName},</p>
        ////                        <p>Your leave request submitted on <strong>{startDateOnly} - {endDateOnly}</strong> for <strong>{levdetails.Duration}</strong> (<strong>{shortname}</strong>) has been Rejected by your HR (<strong>{managerdetails.FirstName}</strong>).</p>
        ////                        <p></p>
        ////                        <p>You can view the updated status of your application in the Office Connect portal.</p>
        ////                        <p></p>
        ////                        <p>Best regards,</p>
        ////                        <p>Office Connect.</p>";

        ////                        Task.Run(() => SendLeaveMail(to, cc, subject, body));

        ////                        // ========== NEW: ADD NOTIFICATION ==========
        ////                        Task.Run(async () =>
        ////                        {
        ////                            await _notificationService.CreateLeaveRejectedByHRNotification(leaveAppId, model.LoginId);
        ////                        });
        ////                    }
        ////                    else
        ////                    {
        ////                        throw new CustomApiException(HttpStatusCode.NotFound, "Rejected Leave Details Not Found");
        ////                    }
        ////                }
        ////            }
        ////            LeaveResponseViewModel emvm = new LeaveResponseViewModel();
        ////            emvm.Status = 200;
        ////            emvm.msg = "Rejected By HR";

        ////            return emvm;
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        public void SendLeaveMail(string to, string cc, string subject, string body)
        {
            var GetSMTPData = (from ES in DB.EmailSetUps
                               select ES).FirstOrDefault();

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
                    message.To.Add(to);
                    if (!string.IsNullOrEmpty(cc))
                        message.CC.Add(cc);


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
        //public void ProcessLeaveCarryForward(int year)
        //{
        //    var employees = DB.EmployeeMasters.Where(e => e.IsActive && !e.IsDeleted).ToList();

        //    foreach (var emp in employees)
        //    {
        //        var leaveTypes = DB.LeaveTypeMasters.Where(x => x.IsActive && !x.IsDeleted).ToList();

        //        foreach (var leaveType in leaveTypes)
        //        {
        //            var leaveBalance = GetEmployeeLeaveBalance(emp.EmpId, leaveType.LeaveTypeId, year);
        //            decimal opening = leaveBalance.OpeningBalance;
        //            decimal availed = leaveBalance.Availed;
        //            decimal unused = opening - availed;

        //            decimal carryForward = 0;
        //            decimal encashment = 0;
        //            decimal closingBalance = 0;

        //            if (leaveType.ShortName == "CL")
        //            {
        //                carryForward = 0; // CL lapses
        //                closingBalance = unused;
        //            }
        //            else if (leaveType.ShortName == "EL")
        //            {
        //                decimal carryForwardLimit = 30; // example
        //                carryForward = Math.Min(unused, carryForwardLimit);

        //                // optional: encashment policy
        //                if (unused > carryForwardLimit)
        //                {
        //                    encashment = unused - carryForwardLimit;
        //                }

        //                closingBalance = carryForward + encashment;
        //            }

        //            LeaveCarryForwardMaster cf = new LeaveCarryForwardMaster
        //            {
        //                EmpId = emp.EmpId,
        //                LeaveTypeId = leaveType.LeaveTypeId,
        //                LeaveYear = year,
        //                OpeningBalance = opening,
        //                Availed = availed,
        //                CarryForward = carryForward,
        //                Encashment = encashment,
        //                ClosingBalance = closingBalance,
        //                CreatedDate = DateTime.Now,
        //                IsActive = true,
        //                IsDeleted = false
        //            };

        //            DB.LeaveCarryForwardMasters.InsertOnSubmit(cf);
        //        }
        //    }
        //    DB.SubmitChanges();
        //}
        public FileUploadAPIViewModel UploadFileLeave(FileUploadAPIViewModel model)
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
                var uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/File/Leave/Doc");

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
        ////public void ProcessLeaveCarryForward()
        ////{
        ////    DateTime today = DateTime.Now;
        ////    int currentYear = today.Year;
        ////    int currentMonth = today.Month;

        ////    using (var db = new DB_Offc_ConEntities())
        ////    {
        ////        // Get active employees
        ////        var employees = db.EmployeeMasters
        ////                          .Where(e => e.IsActive == true && e.IsDeleted == false)
        ////                          .ToList();

        ////        foreach (var emp in employees)
        ////        {
        ////            int? clid = db.LeaveTypeMasters.Where(x => x.ShortName == "CL" && x.IsActive == true && x.IsDeleted == false)
        ////                            .Select(x => x.LeaveTypeId).FirstOrDefault();
        ////            int? elid = db.LeaveTypeMasters.Where(x => x.ShortName == "EL" && x.IsActive == true && x.IsDeleted == false)
        ////                            .Select(x => x.LeaveTypeId).FirstOrDefault();

        ////            // ------------------ CL Processing ------------------
        ////            var clRecord = db.LeaveCarryForwardMasters
        ////                .FirstOrDefault(x => x.EmpId == emp.EmpId
        ////                                     && x.LeaveYear == currentYear
        ////                                     && x.LeaveMonth == currentMonth
        ////                                     && x.LeaveTypeId == clid);

        ////            if (clRecord == null)
        ////            {
        ////                // Get last month closing balance
        ////                var prevCL = db.LeaveCarryForwardMasters
        ////                    .Where(x => x.EmpId == emp.EmpId
        ////                                && x.LeaveTypeId == clid)
        ////                    .OrderByDescending(x => x.LeaveYear)
        ////                    .ThenByDescending(x => x.LeaveMonth)
        ////                    .FirstOrDefault();

        ////                decimal openingBalance = prevCL?.ClosingBalance ?? 0;
        ////                decimal credit = 1; // 1 CL per month
        ////                decimal availed = 0; // will be updated separately
        ////                decimal closingBalance = openingBalance + credit - availed;

        ////                LeaveCarryForwardMaster newCL = new LeaveCarryForwardMaster
        ////                {
        ////                    EmpId = emp.EmpId,
        ////                    LeaveTypeId = clid,
        ////                    LeaveYear = currentYear,
        ////                    LeaveMonth = currentMonth,
        ////                    OpeningBalance = openingBalance,
        ////                    Availed = availed,
        ////                    CarryForward = 0,
        ////                    Encashment = 0,
        ////                    ClosingBalance = closingBalance,
        ////                    CreatedBy = 1, // system
        ////                    CreatedDate = today,
        ////                    IsActive = true,
        ////                    IsUpdated = false,
        ////                    IsDeleted = false
        ////                };
        ////                db.LeaveCarryForwardMasters.Add(newCL);
        ////            }

        ////            // ------------------ EL Processing ------------------
        ////            // Employee must have completed 1 year
        ////            if (emp.JoiningDate.HasValue && emp.JoiningDate.Value.AddYears(1) <= today)
        ////            {
        ////                if (currentMonth == 1) // Run yearly in Jan (or Apr for FY)
        ////                {
        ////                    var prevEL = db.LeaveCarryForwardMasters
        ////                        .Where(x => x.EmpId == emp.EmpId
        ////                                    && x.LeaveTypeId == elid
        ////                                    && x.LeaveYear == currentYear - 1)
        ////                        .OrderByDescending(x => x.LeaveMonth)
        ////                        .FirstOrDefault();

        ////                    decimal openingBalance = prevEL?.ClosingBalance ?? 0;
        ////                    decimal carryForward = Math.Min(openingBalance, 12); // max 12
        ////                    decimal credit = 18; // yearly credit
        ////                    decimal availed = 0;
        ////                    decimal closingBalance = carryForward + credit - availed;

        ////                    LeaveCarryForwardMaster newEL = new LeaveCarryForwardMaster
        ////                    {
        ////                        EmpId = emp.EmpId,
        ////                        LeaveTypeId = elid,
        ////                        LeaveYear = currentYear,
        ////                        LeaveMonth = 1,
        ////                        OpeningBalance = carryForward,
        ////                        Availed = availed,
        ////                        CarryForward = carryForward,
        ////                        Encashment = 0,
        ////                        ClosingBalance = closingBalance,
        ////                        CreatedBy = 1,
        ////                        CreatedDate = today,
        ////                        IsActive = true,
        ////                        IsUpdated = false,
        ////                        IsDeleted = false
        ////                    };
        ////                    db.LeaveCarryForwardMasters.Add(newEL);
        ////                }
        ////            }
        ////        }

        ////        db.SaveChanges();
        ////    }
        ////} //03.03.2026  checking Carryforward stoppped this
        public LeaveCountsViewModel IndividualLeaveCount(EmpLeaveApplicationViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                int? CLId = (from cl in DB.LeaveTypeMasters
                                  where cl.ShortName == "CL" && cl.IsActive == true && cl.IsDeleted == false
                                  select cl.LeaveTypeId).FirstOrDefault();

                int? RHId = (from rh in DB.LeaveTypeMasters
                             where rh.ShortName == "RH" && rh.IsActive == true && rh.IsDeleted == false
                             select rh.LeaveTypeId).FirstOrDefault();

                int? ELId = (from el in DB.LeaveTypeMasters
                             where el.ShortName == "EL" && el.IsActive == true && el.IsDeleted == false
                             select el.LeaveTypeId).FirstOrDefault();

                int? MLId = (from el in DB.LeaveTypeMasters
                             where el.ShortName == "ML" && el.IsActive == true && el.IsDeleted == false
                             select el.LeaveTypeId).FirstOrDefault();

                int? PLId = (from el in DB.LeaveTypeMasters
                             where el.ShortName == "PL" && el.IsActive == true && el.IsDeleted == false
                             select el.LeaveTypeId).FirstOrDefault();

                int? CompoffId = (from el in DB.LeaveTypeMasters
                             where el.ShortName.ToUpper() == "COMP OFF" && el.IsActive == true && el.IsDeleted == false
                             select el.LeaveTypeId).FirstOrDefault();

                LeaveCountsViewModel listoflevCounts = new LeaveCountsViewModel();
                listoflevCounts.EmpId = loginId;

                DateTime Today = DateTime.Now;
                int? Year = Today.Year;
                int? Month = Today.Month;

                if (loginId != 0)
                {
                    List<CarryForwardMasterViewModel> CLlist = new List<CarryForwardMasterViewModel>();
                    List<CarryForwardMasterViewModel> RHlist = new List<CarryForwardMasterViewModel>();
                    List<CarryForwardMasterViewModel> ELlist = new List<CarryForwardMasterViewModel>();
                    List<CarryForwardMasterViewModel> MLlist = new List<CarryForwardMasterViewModel>();
                    List<CarryForwardMasterViewModel> PLlist = new List<CarryForwardMasterViewModel>();
                    List<CarryForwardMasterViewModel> COMPOFFlist = new List<CarryForwardMasterViewModel>();

                    if (CLId != 0)
                    {
                        var levdetails = (from lev in DB.LeaveCarryForwardMasters
                                          where lev.EmpId == loginId && lev.LeaveTypeId == CLId && lev.LeaveYear == Year && lev.LeaveMonth == Month
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails != null)
                        {
                            CarryForwardMasterViewModel cl = new CarryForwardMasterViewModel();
                            cl.EmpId = levdetails.EmpId;
                            cl.EmpCode = levdetails.EmpCode;
                            cl.LeaveTypeId = levdetails.LeaveTypeId;
                            int? leavetypeid = levdetails.LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            cl.LeaveType = shortname;
                            cl.LeaveYear = levdetails.LeaveYear;
                            cl.LeaveMonth = levdetails.LeaveMonth;
                            cl.OpeningBalance = levdetails.OpeningBalance;
                            cl.Availed = levdetails.Availed;
                            cl.ClosingBalance = levdetails.ClosingBalance;
                            CLlist.Add(cl);
                        }
                        else 
                        {
                            CarryForwardMasterViewModel cl = new CarryForwardMasterViewModel();
                            cl.EmpId = 0;
                            cl.EmpCode = "";
                            cl.LeaveTypeId = 0;
                            cl.LeaveType = "CL";
                            cl.LeaveYear = 0;
                            cl.LeaveMonth = 0;
                            cl.OpeningBalance = 0;
                            cl.Availed = 0;
                            cl.ClosingBalance = 0;
                            CLlist.Add(cl);
                        }
                    }
                    if (RHId != 0)
                    {
                        var levdetails = (from lev in DB.LeaveCarryForwardMasters
                                          where lev.EmpId == loginId && lev.LeaveTypeId == RHId && lev.LeaveYear == Year && lev.LeaveMonth == 0
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails != null)
                        {
                            CarryForwardMasterViewModel rh = new CarryForwardMasterViewModel();
                            rh.EmpId = levdetails.EmpId;
                            rh.EmpCode = levdetails.EmpCode;
                            rh.LeaveTypeId = levdetails.LeaveTypeId;
                            int? leavetypeid = levdetails.LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            rh.LeaveType = shortname;
                            rh.LeaveYear = levdetails.LeaveYear;
                            rh.LeaveMonth = levdetails.LeaveMonth;
                            rh.OpeningBalance = levdetails.OpeningBalance;
                            rh.Availed = levdetails.Availed;
                            rh.ClosingBalance = levdetails.ClosingBalance;
                            RHlist.Add(rh);
                        }
                        else
                        {
                            CarryForwardMasterViewModel rh = new CarryForwardMasterViewModel();
                            rh.EmpId = 0;
                            rh.EmpCode = "";
                            rh.LeaveTypeId = 0;
                            rh.LeaveType = "RH";
                            rh.LeaveYear = 0;
                            rh.LeaveMonth = 0;
                            rh.OpeningBalance = 0;
                            rh.Availed = 0;
                            rh.ClosingBalance = 0;
                            RHlist.Add(rh);
                        }
                    }
                    if (ELId != 0)
                    {
                        var levdetails = (from lev in DB.LeaveCarryForwardMasters
                                          where lev.EmpId == loginId && lev.LeaveTypeId == ELId && lev.LeaveYear == Year && lev.LeaveMonth == 0
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails != null)
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = levdetails.EmpId;
                            el.EmpCode = levdetails.EmpCode;
                            el.LeaveTypeId = levdetails.LeaveTypeId;
                            int? leavetypeid = levdetails.LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            el.LeaveType = shortname;
                            el.LeaveYear = levdetails.LeaveYear;
                            el.LeaveMonth = levdetails.LeaveMonth;
                            el.OpeningBalance = levdetails.OpeningBalance;
                            el.Availed = levdetails.Availed;
                            el.ClosingBalance = levdetails.ClosingBalance;
                            ELlist.Add(el);
                        }
                        else
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = 0;
                            el.EmpCode = "";
                            el.LeaveTypeId = 0;
                            el.LeaveType = "EL";
                            el.LeaveYear = 0;
                            el.LeaveMonth = 0;
                            el.OpeningBalance = 0;
                            el.Availed = 0;
                            el.ClosingBalance = 0;
                            ELlist.Add(el);
                        }
                    }
                    if (MLId != 0)
                    {
                        var levdetails = (from lev in DB.LeaveCarryForwardMasters
                                          where lev.EmpId == loginId && lev.LeaveTypeId == MLId && lev.LeaveYear == Year && lev.LeaveMonth == 0
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails != null)
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = levdetails.EmpId;
                            el.EmpCode = levdetails.EmpCode;
                            el.LeaveTypeId = levdetails.LeaveTypeId;
                            int? leavetypeid = levdetails.LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            el.LeaveType = shortname;
                            el.LeaveYear = levdetails.LeaveYear;
                            el.LeaveMonth = levdetails.LeaveMonth;
                            el.OpeningBalance = levdetails.OpeningBalance;
                            el.Availed = levdetails.Availed;
                            el.ClosingBalance = levdetails.ClosingBalance;
                            MLlist.Add(el);
                        }
                        else
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = 0;
                            el.EmpCode = "";
                            el.LeaveTypeId = 0;
                            el.LeaveType = "ML";
                            el.LeaveYear = 0;
                            el.LeaveMonth = 0;
                            el.OpeningBalance = 0;
                            el.Availed = 0;
                            el.ClosingBalance = 0;
                            MLlist.Add(el);
                        }
                    }
                    if (PLId != 0)
                    {
                        var levdetails = (from lev in DB.LeaveCarryForwardMasters
                                          where lev.EmpId == loginId && lev.LeaveTypeId == PLId && lev.LeaveYear == Year && lev.LeaveMonth == 0
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails != null)
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = levdetails.EmpId;
                            el.EmpCode = levdetails.EmpCode;
                            el.LeaveTypeId = levdetails.LeaveTypeId;
                            int? leavetypeid = levdetails.LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            el.LeaveType = shortname;
                            el.LeaveYear = levdetails.LeaveYear;
                            el.LeaveMonth = levdetails.LeaveMonth;
                            el.OpeningBalance = levdetails.OpeningBalance;
                            el.Availed = levdetails.Availed;
                            el.ClosingBalance = levdetails.ClosingBalance;
                            PLlist.Add(el);
                        }
                        else
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = 0;
                            el.EmpCode = "";
                            el.LeaveTypeId = 0;
                            el.LeaveType = "PL";
                            el.LeaveYear = 0;
                            el.LeaveMonth = 0;
                            el.OpeningBalance = 0;
                            el.Availed = 0;
                            el.ClosingBalance = 0;
                            PLlist.Add(el);
                        }
                    }
                    if (CompoffId != 0)
                    {
                        var levdetails = (from lev in DB.LeaveCarryForwardMasters
                                          where lev.EmpId == loginId && lev.LeaveTypeId == CompoffId && lev.LeaveYear == Year //&& lev.LeaveMonth == 0
                                          && lev.IsActive == true && lev.IsDeleted == false
                                          select lev).FirstOrDefault();

                        if (levdetails != null)
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = levdetails.EmpId;
                            el.EmpCode = levdetails.EmpCode;
                            el.LeaveTypeId = levdetails.LeaveTypeId;
                            int? leavetypeid = levdetails.LeaveTypeId;
                            string leavename = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.LeaveName).FirstOrDefault() ?? "";
                            string shortname = DB.LeaveTypeMasters.Where(x => x.LeaveTypeId == leavetypeid && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.ShortName).FirstOrDefault() ?? "";
                            el.LeaveType = shortname;
                            el.LeaveYear = levdetails.LeaveYear;
                            el.LeaveMonth = levdetails.LeaveMonth;
                            el.OpeningBalance = levdetails.OpeningBalance;
                            el.Availed = levdetails.Availed;
                            el.ClosingBalance = levdetails.ClosingBalance;
                            COMPOFFlist.Add(el);
                        }
                        else
                        {
                            CarryForwardMasterViewModel el = new CarryForwardMasterViewModel();
                            el.EmpId = 0;
                            el.EmpCode = "";
                            el.LeaveTypeId = 0;
                            el.LeaveType = "COMP OFF";
                            el.LeaveYear = 0;
                            el.LeaveMonth = 0;
                            el.OpeningBalance = 0;
                            el.Availed = 0;
                            el.ClosingBalance = 0;
                            COMPOFFlist.Add(el);
                        }
                    }

                    string gender = DB.EmployeeMasters.Where(x => x.EmpId == loginId && x.IsActive == true
                                                && x.IsDeleted == false).Select(x => x.Gender).FirstOrDefault() ?? "";

                    listoflevCounts.CasualCounts = CLlist;
                    listoflevCounts.ReservedHolidayCounts = RHlist;
                    listoflevCounts.EarnedLeaveCounts = ELlist;
                    if (gender.ToUpper() == "FEMALE")
                    {
                        listoflevCounts.MLCounts = MLlist;
                    }
                    if (gender.ToUpper() == "MALE")
                    {
                        listoflevCounts.PLCounts = PLlist;
                    }
                    listoflevCounts.CompOffCounts = COMPOFFlist;

                    return listoflevCounts;
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
        public List<LeaveBalReportViewModel> LeaveBalReport(LeaveBalReportViewModel model)
        {
            try
            {
                if (model.LoginId <= 0)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

                int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locId = (model.LocationId != 0) ? model.LocationId : 0;
                int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? designationId = (model.DesignationId != 0) ? model.DesignationId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                DateTime Today = DateTime.Now; //more than 10 days
                int? Year = Today.Year;
                int? Month = Today.Month;

                if (model.Month != 0 && model.Year != 0)
                {
                    Year = model.Year;
                    Month = model.Month;
                }
                else if (model.Month == 0 && model.Year != 0)
                {
                    Year = model.Year;
                    Month = model.Month;
                }

                var empdetails = (from emp in DB.EmployeeMasters
                                  where //emp.EmpStatus.ToUpper() == "ACTIVE" &&
                                        emp.IsActive == true && emp.IsDeleted == false
                                  select emp).ToList();

                if (compId != 0)
                {
                    empdetails = empdetails.Where(x => x.CompId == compId).ToList();
                }
                if (leId != 0)
                {
                    empdetails = empdetails.Where(x => x.LEId == leId).ToList();
                }
                if (buId != 0)
                {
                    empdetails = empdetails.Where(x => x.BUId == buId).ToList();
                }
                if (locId != 0)
                {
                    empdetails = empdetails.Where(x => x.LocationId == locId).ToList();
                }
                if (deptId != 0)
                {
                    empdetails = empdetails.Where(x => x.CategoryId == deptId).ToList();
                }
                if (designationId != 0)
                {
                    empdetails = empdetails.Where(x => x.DesignationId == designationId).ToList();
                }
                if (empId != 0)
                {
                    empdetails = empdetails.Where(x => x.EmpId == empId).ToList();
                }

                var lcfdetails = (from lcf in DB.LeaveCarryForwardMasters
                                  join lty in DB.LeaveTypeMasters on lcf.LeaveTypeId equals lty.LeaveTypeId
                                  where (lcf.LeaveMonth == Month && lcf.LeaveYear == Year || lcf.LeaveMonth == 0 && lcf.LeaveYear == Year) && lcf.IsDeleted == false
                                  && lty.IsActive == true && lty.IsDeleted == false
                                  select new
                                  {
                                      lty.LeaveTypeId,
                                      lty.LeaveName,
                                      lty.ShortName,
                                      lcf.EmpId,
                                      lcf.EmpCode,
                                      lcf.LeaveMonth,
                                      lcf.LeaveYear,
                                      lcf.OpeningBalance,
                                      lcf.Availed,
                                      lcf.CarryForward,
                                      lcf.ClosingBalance,
                                      lcf.IsActive
                                  }).ToList();

                List<LeaveBalReportViewModel> lstlevreport = new List<LeaveBalReportViewModel>();

                for (int i = 0; i < empdetails.Count(); i++)
                {
                    LeaveBalReportViewModel LBRVM = new LeaveBalReportViewModel();
                    LBRVM.LoginId = model.LoginId;
                    LBRVM.CompId = empdetails[i].CompId;
                    LBRVM.LEId = empdetails[i].LEId;
                    LBRVM.BUId = empdetails[i].BUId;
                    LBRVM.LocationId = empdetails[i].LocationId;
                    LBRVM.DeptId = empdetails[i].CategoryId;
                    LBRVM.DesignationId = empdetails[i].DesignationId;
                    LBRVM.EmpId = empdetails[i].EmpId;
                    int empid = empdetails[i].EmpId;
                    LBRVM.EmpName = empdetails[i].FirstName;
                    LBRVM.EmpCode = empdetails[i].EmpCode;
                    LBRVM.Year = model.Year;
                    LBRVM.Month = model.Month;
                    var CLLeavecountdetails = lcfdetails.Where(x => x.ShortName.ToUpper() == "CL" && x.EmpId == empid).FirstOrDefault();
                    var ELLeavecountdetails = lcfdetails.Where(x => x.ShortName.ToUpper() == "EL" && x.EmpId == empid && x.IsActive == true).FirstOrDefault();
                    var RHLeavecountdetails = lcfdetails.Where(x => x.ShortName.ToUpper() == "RH" && x.EmpId == empid && x.IsActive == true).FirstOrDefault();
                    var COMPOFFLeavecountdetails = lcfdetails.Where(x => x.ShortName.ToUpper() == "COMP OFF" && x.EmpId == empid && x.IsActive == true).FirstOrDefault();
                    if (CLLeavecountdetails != null)
                    {
                        LBRVM.CLLeaveTypeId = CLLeavecountdetails.LeaveTypeId;
                        LBRVM.CLLeaveType = CLLeavecountdetails.LeaveName + " (" + CLLeavecountdetails.ShortName + ")";
                        LBRVM.CLOpeningBalance = CLLeavecountdetails.OpeningBalance;
                        LBRVM.CLAvailed = CLLeavecountdetails.Availed;
                        LBRVM.CLCarryFroward = CLLeavecountdetails.CarryForward;
                        LBRVM.CLColsingBalance = CLLeavecountdetails.ClosingBalance;
                    }
                    else
                    {
                        LBRVM.CLLeaveTypeId = 0;
                        LBRVM.CLLeaveType = "CL";
                        LBRVM.CLOpeningBalance = 0;
                        LBRVM.CLAvailed = 0;
                        LBRVM.CLCarryFroward = 0;
                        LBRVM.CLColsingBalance = 0;
                    }
                    if (ELLeavecountdetails != null)
                    {
                        LBRVM.ELLeaveTypeId = ELLeavecountdetails.LeaveTypeId;
                        LBRVM.ELLeaveType = ELLeavecountdetails.LeaveName + " (" + ELLeavecountdetails.ShortName + ")";
                        LBRVM.ELOpeningBalance = ELLeavecountdetails.OpeningBalance;
                        LBRVM.ELAvailed = ELLeavecountdetails.Availed;
                        LBRVM.ELCarryFroward = ELLeavecountdetails.CarryForward;
                        LBRVM.ELColsingBalance = ELLeavecountdetails.ClosingBalance;
                    }
                    else
                    {
                        LBRVM.ELLeaveTypeId = 0;
                        LBRVM.ELLeaveType = "EL";
                        LBRVM.ELOpeningBalance = 0;
                        LBRVM.ELAvailed = 0;
                        LBRVM.ELCarryFroward = 0;
                        LBRVM.ELColsingBalance = 0;
                    }
                    if (RHLeavecountdetails != null)
                    {
                        LBRVM.RHLeaveTypeId = RHLeavecountdetails.LeaveTypeId;
                        LBRVM.RHLeaveType = RHLeavecountdetails.LeaveName + " (" + RHLeavecountdetails.ShortName + ")";
                        LBRVM.RHOpeningBalance = RHLeavecountdetails.OpeningBalance;
                        LBRVM.RHAvailed = RHLeavecountdetails.Availed;
                        LBRVM.RHCarryFroward = RHLeavecountdetails.CarryForward;
                        LBRVM.RHColsingBalance = RHLeavecountdetails.ClosingBalance;
                    }
                    else
                    {
                        LBRVM.RHLeaveTypeId = 0;
                        LBRVM.RHLeaveType = "RH";
                        LBRVM.RHOpeningBalance = 0;
                        LBRVM.RHAvailed = 0;
                        LBRVM.RHCarryFroward = 0;
                        LBRVM.RHColsingBalance = 0;
                    }
                    if (COMPOFFLeavecountdetails != null)
                    {
                        LBRVM.COMPOFFLeaveTypeId = COMPOFFLeavecountdetails.LeaveTypeId;
                        LBRVM.COMPOFFLeaveType = COMPOFFLeavecountdetails.LeaveName + " (" + COMPOFFLeavecountdetails.ShortName + ")";
                        LBRVM.COMPOFFOpeningBalance = COMPOFFLeavecountdetails.OpeningBalance;
                        LBRVM.COMPOFFAvailed = COMPOFFLeavecountdetails.Availed;
                        LBRVM.COMPOFFCarryFroward = COMPOFFLeavecountdetails.CarryForward;
                        LBRVM.COMPOFFColsingBalance = COMPOFFLeavecountdetails.ClosingBalance;
                    }
                    else
                    {
                        LBRVM.COMPOFFLeaveTypeId = 0;
                        LBRVM.COMPOFFLeaveType = "COMP OFF";
                        LBRVM.COMPOFFOpeningBalance = 0;
                        LBRVM.COMPOFFAvailed = 0;
                        LBRVM.COMPOFFCarryFroward = 0;
                        LBRVM.COMPOFFColsingBalance = 0;
                    }
                    lstlevreport.Add(LBRVM);
                }


                if (!lstlevreport.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Leave Count Details Not Found");

                return lstlevreport;
            }
            catch (CustomApiException)
            {
                throw;
            }
        }
        ////public List<LeaveBalReportViewModel> LeaveBalReport(LeaveBalReportViewModel model)
        ////{
        ////    try
        ////    {
        ////        if (model.LoginId <= 0)
        ////            throw new CustomApiException(HttpStatusCode.BadRequest, "LoginId is Missing");

        ////        int? compId = (model.CompId != 0) ? model.CompId : 0;
        ////        int? leId = (model.LEId != 0) ? model.LEId : 0;
        ////        int? buId = (model.BUId != 0) ? model.BUId : 0;
        ////        int? locId = (model.LocationId != 0) ? model.LocationId : 0;
        ////        int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
        ////        int? designationId = (model.DesignationId != 0) ? model.DesignationId : 0;
        ////        int? empId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        DateTime today = DateTime.Now;
        ////        int year = model.Year != 0 ? model.Year : today.Year;
        ////        int month = model.Month != 0 ? model.Month : today.Month;

        ////        // ---------------- EMPLOYEE QUERY (FILTERED IN DB) ----------------
        ////        var empQuery = DB.EmployeeMasters.Where(e =>
        ////            e.EmpStatus == "ACTIVE" &&
        ////            e.IsActive == true &&
        ////            e.IsDeleted == false);

        ////        if (compId > 0) empQuery = empQuery.Where(x => x.CompId == compId);
        ////        if (leId > 0) empQuery = empQuery.Where(x => x.LEId == leId);
        ////        if (buId > 0) empQuery = empQuery.Where(x => x.BUId == buId);
        ////        if (locId > 0) empQuery = empQuery.Where(x => x.LocationId == locId);
        ////        if (deptId > 0) empQuery = empQuery.Where(x => x.CategoryId == deptId);
        ////        if (designationId > 0) empQuery = empQuery.Where(x => x.DesignationId == designationId);
        ////        if (empId > 0) empQuery = empQuery.Where(x => x.EmpId == empId);

        ////        var empDetails = empQuery.Select(e => new
        ////        {
        ////            e.EmpId,
        ////            e.EmpCode,
        ////            e.FirstName,
        ////            e.CompId,
        ////            e.LEId,
        ////            e.BUId,
        ////            e.LocationId,
        ////            e.CategoryId,
        ////            e.DesignationId
        ////        }).ToList();

        ////        // ---------------- LEAVE DATA (GROUPED & INDEXED) ----------------
        ////        var leaveDict = (
        ////            from lcf in DB.LeaveCarryForwardMasters
        ////            join lty in DB.LeaveTypeMasters
        ////                on lcf.LeaveTypeId equals lty.LeaveTypeId
        ////            where (lcf.LeaveMonth == month || lcf.LeaveYear == year)
        ////                  && lcf.IsActive == true&& lcf.IsDeleted == false
        ////                  && lty.IsActive == true && lty.IsDeleted == false
        ////            select new
        ////            {
        ////                lcf.EmpId,
        ////                lty.LeaveTypeId,
        ////                lty.LeaveName,
        ////                ShortName = lty.ShortName.ToUpper(),
        ////                lcf.OpeningBalance,
        ////                lcf.Availed,
        ////                lcf.CarryForward,
        ////                lcf.ClosingBalance
        ////            })
        ////            .ToList()
        ////            .GroupBy(x => new { x.EmpId, x.ShortName })
        ////            .ToDictionary(g => g.Key, g => g.First());

        ////        // ---------------- BUILD RESPONSE ----------------
        ////        List<LeaveBalReportViewModel> result = new List<LeaveBalReportViewModel>();

        ////        foreach (var emp in empDetails)
        ////        {
        ////            var vm = new LeaveBalReportViewModel
        ////            {
        ////                LoginId = model.LoginId,
        ////                CompId = emp.CompId,
        ////                LEId = emp.LEId,
        ////                BUId = emp.BUId,
        ////                LocationId = emp.LocationId,
        ////                DeptId = emp.CategoryId,
        ////                DesignationId = emp.DesignationId,
        ////                EmpId = emp.EmpId,
        ////                EmpName = emp.FirstName,
        ////                EmpCode = emp.EmpCode,
        ////                Year = year,
        ////                Month = month
        ////            };

        ////            FillLeave(vm, leaveDict, emp.EmpId, "CL");
        ////            FillLeave(vm, leaveDict, emp.EmpId, "EL");
        ////            FillLeave(vm, leaveDict, emp.EmpId, "RH");
        ////            FillLeave(vm, leaveDict, emp.EmpId, "COMP OFF");

        ////            result.Add(vm);
        ////        }

        ////        if (!result.Any())
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "Leave Count Details Not Found");

        ////        return result;
        ////    }
        ////    catch (CustomApiException)
        ////    {
        ////        throw;
        ////    }
        ////}

    }
}