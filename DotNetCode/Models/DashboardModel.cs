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
    public class DashboardModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();

        public DashboardViewModel GetEmployeeEvents(DashboardViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;

                int? year = DateTime.Now.Year;
                DateTime Today = DateTime.Today;

                DashboardViewModel dbvm = new DashboardViewModel();
                dbvm.LoginId = loginId;

                if (loginId != 0)
                {
                    var empdetails = (from emp in DB.EmployeeMasters
                                      where emp.EmpId == loginId && emp.IsActive == true && emp.IsDeleted == false && emp.EmpStatus.ToUpper() == "ACTIVE"
                                      select emp).FirstOrDefault();

                    var tdybirthdaydetails = (from bd in DB.EmployeeMasters
                                              where (bd.ReportId == loginId || bd.ReportId == empdetails.OldEmp_ID || bd.EmpCode == empdetails.EmpCode)
                                                    && bd.IsActive == true && bd.IsDeleted == false && bd.EmpStatus.ToUpper() == "ACTIVE"
                                                    && bd.DOB.HasValue && bd.DOB.Value.Month == Today.Month && bd.DOB.Value.Day == Today.Day
                                              select bd).ToList();

                    ////// Find the Monday of this week
                    ////DateTime startOfWeek = Today.AddDays(-(int)Today.DayOfWeek + (int)DayOfWeek.Monday);

                    ////// Find the Sunday of this week
                    ////DateTime endOfWeek = startOfWeek.AddDays(6);

                    DayOfWeek startOfWeek = DayOfWeek.Monday; // or your week start
                    DateTime weekStart = Today.AddDays(-(int)Today.DayOfWeek + (int)startOfWeek);
                    DateTime weekEnd = weekStart.AddDays(6);

                    var weekBirthdayDetails = DB.EmployeeMasters
                        .Where(bd => (bd.ReportId == loginId || bd.ReportId == empdetails.OldEmp_ID || bd.EmpCode == empdetails.EmpCode)
                        && bd.IsActive == true && bd.IsDeleted == false && bd.EmpStatus.ToUpper() == "ACTIVE" && bd.DOB.HasValue)
                        .AsEnumerable()  // <-- switch to LINQ to Objects
                        .Where(bd => {
                            DateTime dob = bd.DOB.Value;
                            DateTime birthdayThisYear = new DateTime(Today.Year, dob.Month, dob.Day);
                            return birthdayThisYear >= weekStart && birthdayThisYear <= weekEnd;
                        })
                        .OrderBy(bd => bd.DOB.Value.Day)
                        .ToList();
                    int currentMonth = Today.Month;

                    var monthBirthdayDetails = (from bd in DB.EmployeeMasters
                                                where (bd.ReportId == loginId || bd.ReportId == empdetails.OldEmp_ID || bd.EmpCode == empdetails.EmpCode)
                                                       && bd.IsActive == true && bd.IsDeleted == false && bd.EmpStatus.ToUpper() == "ACTIVE"
                                                      && bd.DOB.Value.Month == currentMonth
                                                select bd).OrderBy(x => x.DOB.Value.Day).ToList();

                    if (leId > 0)
                    {
                        tdybirthdaydetails = tdybirthdaydetails.Where(x => x.LEId == leId).ToList();
                        weekBirthdayDetails = weekBirthdayDetails.Where(x => x.LEId == leId).ToList();
                        monthBirthdayDetails = monthBirthdayDetails.Where(x => x.LEId == leId).ToList();
                    }

                    ListBirthdayViewModel lbvm = new ListBirthdayViewModel();
                    lbvm.daycount = tdybirthdaydetails.Count();
                    lbvm.weekcount = weekBirthdayDetails.Count();
                    lbvm.monthcount = monthBirthdayDetails.Count();

                    if (tdybirthdaydetails.Count() > 0)
                    {
                        List<BirthdayViewModel> lstofDaybd = new List<BirthdayViewModel>();
                        for (int i = 0; i < tdybirthdaydetails.Count(); i++)
                        {
                            BirthdayViewModel bvm = new BirthdayViewModel();
                            bvm.EmpId = tdybirthdaydetails[i].EmpId;
                            bvm.UserName = tdybirthdaydetails[i].UserName;
                            bvm.EmpCode = tdybirthdaydetails[i].EmpCode;
                            bvm.FirstName = tdybirthdaydetails[i].FirstName;
                            bvm.MiddleName = tdybirthdaydetails[i].MiddleName;
                            bvm.LastName = tdybirthdaydetails[i].LastName;
                            bvm.DOB = tdybirthdaydetails[i].DOB?.ToString("dd-MM-yyyy");
                            bvm.Day = "Birth Day";
                            bvm.Gender = tdybirthdaydetails[i].Gender;
                            lstofDaybd.Add(bvm);
                        }
                        lbvm.lstofdaybirthday = lstofDaybd;
                    }
                    if (weekBirthdayDetails.Count() > 0)
                    {
                        List<BirthdayViewModel> lstofWeekbd = new List<BirthdayViewModel>();
                        for (int i = 0; i < weekBirthdayDetails.Count(); i++)
                        {
                            BirthdayViewModel bvm = new BirthdayViewModel();
                            bvm.EmpId = weekBirthdayDetails[i].EmpId;
                            bvm.UserName = weekBirthdayDetails[i].UserName;
                            bvm.EmpCode = weekBirthdayDetails[i].EmpCode;
                            bvm.FirstName = weekBirthdayDetails[i].FirstName;
                            bvm.MiddleName = weekBirthdayDetails[i].MiddleName;
                            bvm.LastName = weekBirthdayDetails[i].LastName;
                            bvm.DOB = weekBirthdayDetails[i].DOB?.ToString("dd-MM-yyyy");
                            bvm.Day = "Birth Day";
                            bvm.Gender = weekBirthdayDetails[i].Gender;
                            lstofWeekbd.Add(bvm);
                        }
                        lbvm.lstofweekbirthday = lstofWeekbd;
                    }
                    if (monthBirthdayDetails.Count() > 0)
                    {
                        List<BirthdayViewModel> lstofMonthbd = new List<BirthdayViewModel>();
                        for (int i = 0; i < monthBirthdayDetails.Count(); i++)
                        {
                            BirthdayViewModel bvm = new BirthdayViewModel();
                            bvm.EmpId = monthBirthdayDetails[i].EmpId;
                            bvm.UserName = monthBirthdayDetails[i].UserName;
                            bvm.EmpCode = monthBirthdayDetails[i].EmpCode;
                            bvm.FirstName = monthBirthdayDetails[i].FirstName;
                            bvm.MiddleName = monthBirthdayDetails[i].MiddleName;
                            bvm.LastName = monthBirthdayDetails[i].LastName;
                            bvm.DOB = monthBirthdayDetails[i].DOB?.ToString("dd-MM-yyyy");
                            bvm.Day = "Birth Day";
                            bvm.Gender = monthBirthdayDetails[i].Gender;
                            lstofMonthbd.Add(bvm);
                        }
                        lbvm.lstofmonthbirthday = lstofMonthbd;
                    }
                    dbvm.lstofbirthday = lbvm;

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
                        List<HolidayListViewModel> lstofHolid = new List<HolidayListViewModel>();
                        for (int i = 0; i < holidaydetails.Count(); i++)
                        {
                            HolidayListViewModel hlvm = new HolidayListViewModel();
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
                        dbvm.lstofholiday = lstofHolid;
                    }

                    var emplist = (from emp in DB.EmployeeMasters
                                   where (emp.ReportId == loginId || emp.ReportId == empdetails.OldEmp_ID || emp.EmpCode == empdetails.EmpCode)
                                   && emp.IsActive == true && emp.IsDeleted == false
                                   && emp.EmpStatus.ToUpper() == "ACTIVE"
                                   select emp).ToList();

                    if (leId > 0)
                    {
                        emplist = emplist.Where(x => x.LEId == leId).ToList();
                    }

                    if (emplist.Count() > 0)
                    {
                        List<EmployeeListViewModel> lstofEmplist = new List<EmployeeListViewModel>();
                        for (int i = 0; i < emplist.Count(); i++)
                        {
                            EmployeeListViewModel elvm = new EmployeeListViewModel();
                            elvm.EmpId = emplist[i].EmpId;
                            elvm.UserName = emplist[i].UserName;
                            elvm.EmpCode = emplist[i].EmpCode;
                            elvm.FirstName = emplist[i].FirstName;
                            elvm.MiddleName = emplist[i].MiddleName;
                            elvm.LastName = emplist[i].LastName;
                            elvm.DeptId = emplist[i].CategoryId;
                            elvm.Department = emplist[i].DeptName;
                            elvm.DesigId = emplist[i].DesignationId;
                            elvm.Designation = emplist[i].DesignationName;
                            elvm.EmailId = emplist[i].EmailId;
                            elvm.Gender = emplist[i].Gender;
                            lstofEmplist.Add(elvm);
                        }
                        dbvm.lstofemp = lstofEmplist;
                    }
                    return dbvm;
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


        public List<HRCountViewModel> GetAllHRCount(VisitorManagementViewModel model)
        {
            try
            {
                int loginId = model.LoginId;
                //  int loginId = model?.LoginId ?? 0;

                if (loginId == 0)
                {
                    throw new Exception("LoginId is missing");
                }

                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                DateTime firstDay = new DateTime(today.Year, today.Month, 1);
                DateTime nextMonth = firstDay.AddMonths(1);

                var visitorToday = DB.VisitorManagements
                    .Where(vm => vm.IsDeleted == false
&& vm.Date >= today
&& vm.Date < tomorrow)
                    .OrderByDescending(vm => vm.VisitId)
                    .Select(vm => new VisitorManagementViewModel
                    {
                        //  VisitId = vm.VisitId,
                        Name = vm.Name,
                        //   Designation = vm.Designation,
                        OMail = vm.OMail,
                        //   Mobile = vm.Mobile,
                        Date = vm.Date,
                        Company = vm.Company,
                        Accept = vm.Accept,
                        Approved = vm.Approved,
                        Time = vm.Time
                    }).ToList();

                var monthEmployees = DB.EmployeeMasters
            .Where(emp => emp.IsDeleted == false &&
                (
                    (emp.JoiningDate.HasValue &&
                     emp.JoiningDate.Value >= firstDay &&
                     emp.JoiningDate.Value < nextMonth)

                    ||

                    (emp.RelievedDate.HasValue &&
                     emp.RelievedDate.Value >= firstDay &&
                     emp.RelievedDate.Value < nextMonth)
                )
            )
            .Select(emp => new EmployeeMasterViewModel
            {

                EmpCode = emp.EmpCode,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                JoiningDate = emp.JoiningDate,
                RelievedDate = emp.RelievedDate,
                EmpStatus = emp.EmpStatus,
                //   ApproverId = emp.ReportId,
                Approver = emp.ReportName ?? "No Approve"
            }).ToList();


                // STEP 1: LoginId

                //   int? loginId = model != null ? model.LoginId : 0;
                // STEP 2: Employee details
                var empData = DB.EmployeeMasters
                    .Where(x => x.EmpId == loginId && x.IsActive == true && x.IsDeleted == false)
                    .Select(x => new
                    {
                        x.LocationId,
                        x.DesignationId,
                        x.CategoryId
                    }).FirstOrDefault();

                // null check
                int locationid = empData?.LocationId ?? 0;
                int desigId = empData?.DesignationId ?? 0;
                int deptId = empData?.CategoryId ?? 0;

                // STEP 3: Pending Leaves Query
                IQueryable<EmpLeaveApplication> pendingLeaveQuery = DB.EmpLeaveApplications
                    .Where(l => l.IsActive == true && l.IsDeleted == false && l.Status == "APPLIED");

                // STEP 3.1: All Leaves Query (NEW)
                IQueryable<EmpLeaveApplication> allLeaveQuery = DB.EmpLeaveApplications
                    .Where(l => l.IsActive == true
&& l.IsDeleted == false
&& l.Status != "APPLIED");

                // STEP 4: Role logic

                // 🔹 Pending
                if (desigId == 186)
                {
                }
                else if (deptId > 1)
                {
                    pendingLeaveQuery = pendingLeaveQuery.Where(l => l.ApprovedBy == loginId);
                }
                else
                {
                    pendingLeaveQuery = pendingLeaveQuery.Where(l =>
                        DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && e.LocationId == locationid));
                }

                // 🔹 All
                if (desigId == 186)
                {
                }
                else if (deptId > 1)
                {
                    allLeaveQuery = allLeaveQuery.Where(l => l.ApprovedBy == loginId);
                }
                else
                {
                    allLeaveQuery = allLeaveQuery.Where(l =>
                        DB.EmployeeMasters.Any(e => e.EmpId == l.EmpId && e.LocationId == locationid));
                }

                // STEP 5: Count
                //   int pendingLeaveCount = pendingLeaveQuery.Count();

                // STEP 6: Pending List

                DateTime startDate = string.IsNullOrEmpty(model.StartDate)
                     ? new DateTime(DateTime.Now.Year, 1, 1)
                    : DateTime.Parse(model.StartDate);

                DateTime endDate = string.IsNullOrEmpty(model.EndDate)
                    ? DateTime.Now
                    : DateTime.Parse(model.EndDate);

                var pendingLeaves = (from l in pendingLeaveQuery
                                     where l.Status == "APPLIED"
&& l.StartDate.HasValue
&& l.EndDate.HasValue
                                     join e in DB.EmployeeMasters on l.EmpId equals e.EmpId
                                     join a in DB.EmployeeMasters on l.ApprovedBy equals a.EmpId into appr
                                     from ap in appr.DefaultIfEmpty()
                                     join lt in DB.LeaveTypeMasters on l.LeaveTypeId equals lt.LeaveTypeId into ltjoin
                                     from ltdata in ltjoin.DefaultIfEmpty()
                                     select new EmpLeaveApplicationViewModel
                                     {
                                         EmpName = e.FirstName,
                                         Approver = ap != null ? ap.FirstName : null,
                                         Status = l.Status,
                                         LeaveType = l.LeaveTypeId == 0 ? "LOP" : ltdata.LeaveName,
                                         StartDate = l.StartDate,
                                         EndDate = l.EndDate
                                     }).ToList();

                // STEP 7: ALL Leaves List (NEW)
                var allLeaves = (from l in allLeaveQuery
                                 where l.StartDate.HasValue
&& l.EndDate.HasValue

                                 join e in DB.EmployeeMasters on l.EmpId equals e.EmpId
                                 join a in DB.EmployeeMasters on l.ApprovedBy equals a.EmpId into appr
                                 from ap in appr.DefaultIfEmpty()
                                 join lt in DB.LeaveTypeMasters on l.LeaveTypeId equals lt.LeaveTypeId into ltjoin
                                 from ltdata in ltjoin.DefaultIfEmpty()

                                 select new EmpLeaveApplicationViewModel
                                 {
                                     EmpName = e.FirstName,
                                     Approver = ap != null ? ap.FirstName : null,
                                     Status = l.Status,
                                     LeaveType = l.LeaveTypeId == 0 ? "LOP" : ltdata.LeaveName,
                                     StartDate = l.StartDate,
                                     EndDate = l.EndDate
                                 }).ToList();


                var compOffList = (from comp in DB.CompOffRequests
                                   where comp.IsRequested == true
&& comp.IsActive == true
&& comp.IsDeleted == false
&& comp.ManagerId == loginId
&& comp.Date.HasValue

                                   select new CompOffRequestViewModel
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
                return new List<HRCountViewModel>
        {
            new HRCountViewModel
            {
                GetvisitorToday = visitorToday,
                CurrentmonthemployeeList = monthEmployees,
               //   PendingLeaveCount = pendingLeaveCount,
                   PendingLeaves = pendingLeaves,
                     AllLeaves = allLeaves,
                        CompOffList = compOffList

            }
        };
            }
            catch (Exception ex)
            {
                // logging kavali ante ikkada rayachu
                throw new Exception("Error in GetAllHRCount: " + ex.Message);
            }
        }
        }
}