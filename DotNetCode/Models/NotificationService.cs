// Create a new file: Services/NotificationService.cs

using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OfficeConnect_Web.Services
{
    public interface INotificationService
    {
        // Leave Notifications
        Task<NotificationResponseViewModel> CreateLeaveAppliedNotification(int leaveAppId, int employeeId);
        Task<NotificationResponseViewModel> CreateLeaveApprovedByManagerNotification(int leaveAppId, int managerId);
        Task<NotificationResponseViewModel> CreateLeaveApprovedByHRNotification(int leaveAppId, int hrId);
        Task<NotificationResponseViewModel> CreateLeaveRejectedByManagerNotification(int leaveAppId, int managerId);
        Task<NotificationResponseViewModel> CreateLeaveRejectedByHRNotification(int leaveAppId, int hrId);
        Task<NotificationResponseViewModel> CreateLeaveCancelledNotification(int leaveAppId, int employeeId);
        Task<NotificationResponseViewModel> CreateLeaveWithdrawnNotification(int leaveAppId, int employeeId);

        // CompOff Notifications
        Task<NotificationResponseViewModel> CreateCompOffAppliedNotification(int compOffReqId, int employeeId);
        Task<NotificationResponseViewModel> CreateCompOffApprovedNotification(int compOffReqId, int approverId);
        Task<NotificationResponseViewModel> CreateCompOffRejectedNotification(int compOffReqId, int approverId);
    }

    public class NotificationService : INotificationService
    {
        private readonly DB_Offc_ConEntities DB;

        public NotificationService()
        {
            DB = new DB_Offc_ConEntities();
        }

        // Helper method to get employee name
        private string GetEmployeeName(int? empId)
        {
            if (empId == null || empId == 0) return "System";
            var emp = DB.EmployeeMasters.FirstOrDefault(x => x.EmpId == empId);
            return emp != null ? $"{emp.FirstName} {emp.LastName}".Trim() : "Unknown";
        }

        // Helper method to get manager/HR details
        private (int? managerId, string managerName) GetManagerDetails(int employeeId)
        {
            var emp = DB.EmployeeMasters.FirstOrDefault(x => x.EmpId == employeeId);
            if (emp == null || emp.ReportId == null) return (null, null);

            var manager = DB.EmployeeMasters.FirstOrDefault(x => x.EmpId == emp.ReportId);
            return (emp.ReportId, manager != null ? $"{manager.FirstName} {manager.LastName}".Trim() : null);
        }

        // Helper method to get HR users
        private List<int> GetHRUserIds(int? locationId = null)
        {
            var hrQuery = from emp in DB.EmployeeMasters
                          join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
                          where des.Designation.ToUpper().Contains("HR")
                                && emp.IsActive == true
                                && emp.IsDeleted == false
                          select emp.EmpId;

            if (locationId.HasValue)
            {
                hrQuery = hrQuery.Where(x => DB.EmployeeMasters.Any(e => e.EmpId == x && e.LocationId == locationId));
            }

            return hrQuery.ToList();
        }

        // Helper method to save notification
        private async Task<NotificationResponseViewModel> SaveNotification(
            string title,
            string message,
            string shortDesc,
            string notificationType,
            string notificationSubType,
            int? referenceId,
            string actionUrl,
            string icon,
            string color,
            int createdBy,
            string createdByName,
            List<int> recipientUserIds,
            List<string> recipientRoles = null)
        {
            using (var transaction = DB.Database.BeginTransaction())
            {
                try
                {
                    // Create notification
                    var notification = new Notification
                    {
                        Title = title,
                        Message = message,
                        ShortDescription = shortDesc,
                        NotificationType = notificationType,
                        NotificationSubType = notificationSubType,
                        ReferenceId = referenceId,
                        ActionUrl = actionUrl,
                        NotificationIcon = icon,
                        NotificationColor = color,
                        CreatedDate = DateTime.Now,
                        ExpiryDate = DateTime.Now.AddMonths(3), // Expire after 3 months
                        CreatedBy = createdBy,
                        CreatedByName = createdByName,
                        IsActive = true,
                        IsDeleted = false
                    };

                    DB.Notifications.Add(notification);
                    await DB.SaveChangesAsync();

                    // Add user notifications
                    var userNotifications = new List<UserNotification>();

                    // Add by user IDs
                    if (recipientUserIds != null && recipientUserIds.Any())
                    {
                        foreach (var userId in recipientUserIds.Distinct())
                        {
                            userNotifications.Add(new UserNotification
                            {
                                NotificationId = notification.NotificationId,
                                UserId = userId,
                                UserRole = "Employee", // Will be updated if needed
                                IsRead = false,
                                IsDelivered = false,
                                IsStarred = false
                            });
                        }
                    }

                    // Add by roles (Admin, HR, etc.) - You can expand this based on your role table
                    if (recipientRoles != null && recipientRoles.Any())
                    {
                        foreach (var role in recipientRoles)
                        {
                            var roleUsers = DB.EmployeeMasters
                                .Where(x => x.IsActive == true && x.IsDeleted == false)
                                .ToList(); // You'll need proper role mapping

                            foreach (var user in roleUsers)
                            {
                                if (!userNotifications.Any(x => x.UserId == user.EmpId))
                                {
                                    userNotifications.Add(new UserNotification
                                    {
                                        NotificationId = notification.NotificationId,
                                        UserId = user.EmpId,
                                        UserRole = role,
                                        IsRead = false,
                                        IsDelivered = false,
                                        IsStarred = false
                                    });
                                }
                            }
                        }
                    }

                    if (userNotifications.Any())
                    {
                        DB.UserNotifications.AddRange(userNotifications);
                        await DB.SaveChangesAsync();
                    }

                    transaction.Commit();

                    return new NotificationResponseViewModel
                    {
                        Success = true,
                        Message = "Notification created successfully",
                        NotificationId = notification.NotificationId,
                        RecipientCount = userNotifications.Count
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new NotificationResponseViewModel
                    {
                        Success = false,
                        Message = $"Error creating notification: {ex.Message}"
                    };
                }
            }
        }

        // ============= LEAVE NOTIFICATIONS =============

        // NEW: When employee applies for leave
        public async Task<NotificationResponseViewModel> CreateLeaveAppliedNotification(int leaveAppId, int employeeId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var empName = GetEmployeeName(employeeId);
                var (managerId, managerName) = GetManagerDetails(employeeId);
                var hrUserIds = GetHRUserIds();

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = $"New Leave Application - {empName}";
                string message = $"{empName} has applied for {leaveType} leave from {startDate} to {endDate} ({leave.Duration} days)";
                string shortDesc = $"{leaveType} - {leave.Duration} days";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                List<int> recipients = new List<int>();
                if (managerId.HasValue) recipients.Add(managerId.Value);
                recipients.AddRange(hrUserIds);

                // Also notify admins (you can add admin IDs here)
                // recipients.AddRange(adminUserIds);

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "Applied",
                    leaveAppId, actionUrl,
                    "calendar-plus", "info",
                    employeeId, empName,
                    recipients
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When manager approves leave
        public async Task<NotificationResponseViewModel> CreateLeaveApprovedByManagerNotification(int leaveAppId, int managerId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var managerName = GetEmployeeName(managerId);
                var hrUserIds = GetHRUserIds();

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = "Leave Approved by Manager";
                string message = $"Your {leaveType} leave from {startDate} to {endDate} has been approved by {managerName}";
                string shortDesc = $"Approved - {leave.Duration} days";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "ApprovedByManager",
                    leaveAppId, actionUrl,
                    "check-circle", "success",
                    managerId, managerName,
                    new List<int> { leave.EmpId ?? 0 }, // Notify employee
                    new List<string> { "HR" } // Also notify HR
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When HR approves leave
        public async Task<NotificationResponseViewModel> CreateLeaveApprovedByHRNotification(int leaveAppId, int hrId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var hrName = GetEmployeeName(hrId);

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = "Leave Approved by HR";
                string message = $"Your {leaveType} leave from {startDate} to {endDate} has been approved by HR ({hrName})";
                string shortDesc = $"Approved - {leave.Duration} days";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "ApprovedByHR",
                    leaveAppId, actionUrl,
                    "check-circle", "success",
                    hrId, hrName,
                    new List<int> { leave.EmpId ?? 0 } // Notify employee
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When manager rejects leave
        public async Task<NotificationResponseViewModel> CreateLeaveRejectedByManagerNotification(int leaveAppId, int managerId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var managerName = GetEmployeeName(managerId);

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = "Leave Rejected by Manager";
                string message = $"Your {leaveType} leave from {startDate} to {endDate} has been rejected by {managerName}";
                string shortDesc = "Rejected";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "RejectedByManager",
                    leaveAppId, actionUrl,
                    "x-circle", "danger",
                    managerId, managerName,
                    new List<int> { leave.EmpId ?? 0 }
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When HR rejects leave
        public async Task<NotificationResponseViewModel> CreateLeaveRejectedByHRNotification(int leaveAppId, int hrId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var hrName = GetEmployeeName(hrId);

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = "Leave Rejected by HR";
                string message = $"Your {leaveType} leave from {startDate} to {endDate} has been rejected by HR ({hrName})";
                string shortDesc = "Rejected";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "RejectedByHR",
                    leaveAppId, actionUrl,
                    "x-circle", "danger",
                    hrId, hrName,
                    new List<int> { leave.EmpId ?? 0 }
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When employee cancels leave
        public async Task<NotificationResponseViewModel> CreateLeaveCancelledNotification(int leaveAppId, int employeeId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var empName = GetEmployeeName(employeeId);
                var (managerId, _) = GetManagerDetails(employeeId);

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = "Leave Cancelled";
                string message = $"{empName} has cancelled their {leaveType} leave from {startDate} to {endDate}";
                string shortDesc = "Cancelled";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                List<int> recipients = new List<int>();
                if (managerId.HasValue) recipients.Add(managerId.Value);
                recipients.AddRange(GetHRUserIds());

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "Cancelled",
                    leaveAppId, actionUrl,
                    "slash-circle", "warning",
                    employeeId, empName,
                    recipients
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When employee withdraws leave
        public async Task<NotificationResponseViewModel> CreateLeaveWithdrawnNotification(int leaveAppId, int employeeId)
        {
            try
            {
                var leave = await DB.EmpLeaveApplications
                    .FirstOrDefaultAsync(x => x.LeaveAppId == leaveAppId);

                if (leave == null)
                    return new NotificationResponseViewModel { Success = false, Message = "Leave not found" };

                var empName = GetEmployeeName(employeeId);
                var (managerId, _) = GetManagerDetails(employeeId);

                string startDate = leave.StartDate?.ToString("dd MMM yyyy");
                string endDate = leave.EndDate?.ToString("dd MMM yyyy");
                string leaveType = GetLeaveTypeName(leave.LeaveTypeId);

                string title = "Leave Withdrawn";
                string message = $"{empName} has withdrawn their {leaveType} leave from {startDate} to {endDate}";
                string shortDesc = "Withdrawn";
                string actionUrl = $"/Leave/Details/{leaveAppId}";

                List<int> recipients = new List<int>();
                if (managerId.HasValue) recipients.Add(managerId.Value);
                recipients.AddRange(GetHRUserIds());

                return await SaveNotification(
                    title, message, shortDesc,
                    "Leave", "Withdrawn",
                    leaveAppId, actionUrl,
                    "arrow-left-circle", "info",
                    employeeId, empName,
                    recipients
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // ============= COMPOFF NOTIFICATIONS =============

        // NEW: When employee applies for CompOff
        public async Task<NotificationResponseViewModel> CreateCompOffAppliedNotification(int compOffReqId, int employeeId)
        {
            try
            {
                var compOff = await DB.CompOffRequests
                    .FirstOrDefaultAsync(x => x.CompOffReqId == compOffReqId);

                if (compOff == null)
                    return new NotificationResponseViewModel { Success = false, Message = "CompOff not found" };

                var empName = GetEmployeeName(employeeId);
                var (managerId, managerName) = GetManagerDetails(employeeId);

                string date = compOff.Date?.ToString("dd MMM yyyy");

                string title = $"New CompOff Request - {empName}";
                string message = $"{empName} has requested CompOff for {date} ({compOff.Hrs} hours)";
                string shortDesc = $"CompOff - {compOff.Hrs} hrs";
                string actionUrl = $"/CompOff/Details/{compOffReqId}";

                List<int> recipients = new List<int>();
                if (managerId.HasValue) recipients.Add(managerId.Value);
                recipients.AddRange(GetHRUserIds());

                return await SaveNotification(
                    title, message, shortDesc,
                    "CompOff", "Applied",
                    compOffReqId, actionUrl,
                    "clock", "info",
                    employeeId, empName,
                    recipients
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When CompOff is approved
        public async Task<NotificationResponseViewModel> CreateCompOffApprovedNotification(int compOffReqId, int approverId)
        {
            try
            {
                var compOff = await DB.CompOffRequests
                    .FirstOrDefaultAsync(x => x.CompOffReqId == compOffReqId);

                if (compOff == null)
                    return new NotificationResponseViewModel { Success = false, Message = "CompOff not found" };

                var approverName = GetEmployeeName(approverId);

                string date = compOff.Date?.ToString("dd MMM yyyy");

                string title = "CompOff Approved";
                string message = $"Your CompOff request for {date} has been approved";
                string shortDesc = "Approved";
                string actionUrl = $"/CompOff/Details/{compOffReqId}";

                return await SaveNotification(
                    title, message, shortDesc,
                    "CompOff", "Approved",
                    compOffReqId, actionUrl,
                    "check-circle", "success",
                    approverId, approverName,
                    new List<int> { compOff.EmpId ?? 0 }
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        // NEW: When CompOff is rejected
        public async Task<NotificationResponseViewModel> CreateCompOffRejectedNotification(int compOffReqId, int approverId)
        {
            try
            {
                var compOff = await DB.CompOffRequests
                    .FirstOrDefaultAsync(x => x.CompOffReqId == compOffReqId);

                if (compOff == null)
                    return new NotificationResponseViewModel { Success = false, Message = "CompOff not found" };

                var approverName = GetEmployeeName(approverId);

                string date = compOff.Date?.ToString("dd MMM yyyy");

                string title = "CompOff Rejected";
                string message = $"Your CompOff request for {date} has been rejected";
                string shortDesc = "Rejected";
                string actionUrl = $"/CompOff/Details/{compOffReqId}";

                return await SaveNotification(
                    title, message, shortDesc,
                    "CompOff", "Rejected",
                    compOffReqId, actionUrl,
                    "x-circle", "danger",
                    approverId, approverName,
                    new List<int> { compOff.EmpId ?? 0 }
                );
            }
            catch (Exception ex)
            {
                return new NotificationResponseViewModel { Success = false, Message = ex.Message };
            }
        }

        private string GetLeaveTypeName(int? leaveTypeId)
        {
            if (leaveTypeId == null || leaveTypeId == 0) return "LOP";
            var leaveType = DB.LeaveTypeMasters.FirstOrDefault(x => x.LeaveTypeId == leaveTypeId);
            return leaveType?.ShortName ?? "Leave";
        }
    }
}