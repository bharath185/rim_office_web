using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeConnect_Web.Controllers
{
    [AuthAttribute]
    public class NotificationController : Controller
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();

        // UPDATED: Get user's notifications with CreatedDate and IsDeleted
        [HttpPost]
        [Route("Notification/GetMyNotifications")]
        public ActionResult GetMyNotifications(NotificationFilterViewModel filter)
        {
            try
            {
                int userId = GetCurrentUserId();

                var query = from un in DB.UserNotifications
                            join n in DB.Notifications on un.NotificationId equals n.NotificationId
                            where un.UserId == userId
                                  && (un.IsDeleted == null || un.IsDeleted == false)  // 👈 Added IsDeleted check
                                  && n.IsActive == true
                                  && n.IsDeleted == false
                                  && (n.ExpiryDate == null || n.ExpiryDate > DateTime.Now)
                            orderby un.CreatedDate descending  // 👈 Use un.CreatedDate instead of n.CreatedDate
                            select new NotificationViewModel
                            {
                                NotificationId = n.NotificationId,
                                UserNotificationId = un.UserNotificationId,
                                Title = n.Title,
                                Message = n.Message,
                                ShortDescription = n.ShortDescription,
                                NotificationType = n.NotificationType,
                                NotificationSubType = n.NotificationSubType,
                                ReferenceId = n.ReferenceId,
                                ActionUrl = n.ActionUrl,
                                NotificationIcon = n.NotificationIcon,
                                NotificationColor = n.NotificationColor,
                                CreatedDate = un.CreatedDate ?? DateTime.Now,  // 👈 Use un.CreatedDate
                                CreatedByName = n.CreatedByName,
                                IsRead = un.IsRead ?? false,
                                ReadDate = un.ReadDate,
                                IsStarred = un.IsStarred ?? false
                            };

                // Apply filters
                if (!string.IsNullOrEmpty(filter.Type))
                {
                    query = query.Where(x => x.NotificationType == filter.Type);
                }

                if (filter.IsRead.HasValue)
                {
                    query = query.Where(x => x.IsRead == filter.IsRead.Value);
                }

                // 👈 Filter by CreatedDate on UserNotifications
                if (filter.FromDate.HasValue)
                {
                    query = query.Where(x => x.CreatedDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(x => x.CreatedDate <= filter.ToDate.Value);
                }

                int totalCount = query.Count();

                var notifications = query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                int unreadCount = query.Count(x => !x.IsRead);

                return Json(new
                {
                    Success = true,
                    Data = notifications,
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize,
                    UnreadCount = unreadCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // UPDATED: Get unread count with IsDeleted
        [HttpPost]
        [Route("Notification/GetUnreadCount")]
        public ActionResult GetUnreadCount()
        {
            try
            {
                int userId = GetCurrentUserId();

                var counts = (from un in DB.UserNotifications
                              join n in DB.Notifications on un.NotificationId equals n.NotificationId
                              where un.UserId == userId
                                    && (un.IsDeleted == null || un.IsDeleted == false)  // 👈 Added IsDeleted
                                    && (un.IsRead == null || un.IsRead == false)
                                    && n.IsActive == true
                                    && n.IsDeleted == false
                                    && (n.ExpiryDate == null || n.ExpiryDate > DateTime.Now)
                              group n by n.NotificationType into g
                              select new
                              {
                                  Type = g.Key,
                                  Count = g.Count()
                              }).ToDictionary(x => x.Type, x => x.Count);

                int total = counts.Values.Sum();

                return Json(new
                {
                    Success = true,
                    TotalUnread = total,
                    ModuleWiseCount = counts
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // UPDATED: Mark notification as read (no change needed)
        [HttpPost]
        [Route("Notification/MarkAsRead")]
        public ActionResult MarkAsRead(int notificationId)
        {
            try
            {
                int userId = GetCurrentUserId();

                var userNotification = DB.UserNotifications
                    .FirstOrDefault(x => x.NotificationId == notificationId
                                        && x.UserId == userId
                                        && (x.IsDeleted == null || x.IsDeleted == false));  // 👈 Added IsDeleted

                if (userNotification == null)
                {
                    return Json(new { Success = false, Message = "Notification not found" });
                }

                userNotification.IsRead = true;
                userNotification.ReadDate = DateTime.Now;

                DB.SaveChanges();

                return Json(new { Success = true, Message = "Marked as read" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // UPDATED: Mark multiple notifications as read with IsDeleted
        [HttpPost]
        [Route("Notification/MarkMultipleAsRead")]
        public ActionResult MarkMultipleAsRead(MarkReadViewModel model)
        {
            try
            {
                int userId = GetCurrentUserId();

                var query = DB.UserNotifications
                    .Where(x => x.UserId == userId
                        && (x.IsDeleted == null || x.IsDeleted == false)  // 👈 Added IsDeleted
                        && (x.IsRead == null || x.IsRead == false));

                if (!model.MarkAll && model.NotificationIds != null && model.NotificationIds.Any())
                {
                    query = query.Where(x => model.NotificationIds.Contains(x.NotificationId));
                }

                var notifications = query.ToList();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    notification.ReadDate = DateTime.Now;
                }

                DB.SaveChanges();

                return Json(new
                {
                    Success = true,
                    Message = $"Marked {notifications.Count} notifications as read"
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // UPDATED: Mark all as read with IsDeleted
        [HttpPost]
        [Route("Notification/MarkAllAsRead")]
        public ActionResult MarkAllAsRead()
        {
            try
            {
                int userId = GetCurrentUserId();

                var unreadNotifications = DB.UserNotifications
                    .Where(x => x.UserId == userId
                        && (x.IsDeleted == null || x.IsDeleted == false)  // 👈 Added IsDeleted
                        && (x.IsRead == null || x.IsRead == false))
                    .ToList();

                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = true;
                    notification.ReadDate = DateTime.Now;
                }

                DB.SaveChanges();

                return Json(new
                {
                    Success = true,
                    Message = $"Marked {unreadNotifications.Count} notifications as read"
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // UPDATED: Toggle star with IsDeleted
        [HttpPost]
        [Route("Notification/ToggleStar")]
        public ActionResult ToggleStar(int notificationId)
        {
            try
            {
                int userId = GetCurrentUserId();

                var userNotification = DB.UserNotifications
                    .FirstOrDefault(x => x.NotificationId == notificationId
                                        && x.UserId == userId
                                        && (x.IsDeleted == null || x.IsDeleted == false));  // 👈 Added IsDeleted

                if (userNotification == null)
                {
                    return Json(new { Success = false, Message = "Notification not found" });
                }

                userNotification.IsStarred = !(userNotification.IsStarred ?? false);

                DB.SaveChanges();

                return Json(new
                {
                    Success = true,
                    IsStarred = userNotification.IsStarred
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // UPDATED: Delete notification (soft delete using IsDeleted)
        [HttpPost]
        [Route("Notification/DeleteNotification")]
        public ActionResult DeleteNotification(int notificationId)
        {
            try
            {
                int userId = GetCurrentUserId();

                var userNotification = DB.UserNotifications
                    .FirstOrDefault(x => x.NotificationId == notificationId
                                        && x.UserId == userId
                                        && (x.IsDeleted == null || x.IsDeleted == false));  // 👈 Added IsDeleted check

                if (userNotification == null)
                {
                    return Json(new { Success = false, Message = "Notification not found" });
                }

                // 👈 Soft delete - set IsDeleted to true instead of removing
                userNotification.IsDeleted = true;
                // Or if you want permanent delete:
                // DB.UserNotifications.Remove(userNotification);

                DB.SaveChanges();

                return Json(new { Success = true, Message = "Notification deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // NEW: Get notifications by date range using CreatedDate
        [HttpPost]
        [Route("Notification/GetNotificationsByDateRange")]
        public ActionResult GetNotificationsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                int userId = GetCurrentUserId();

                var notifications = from un in DB.UserNotifications
                                    join n in DB.Notifications on un.NotificationId equals n.NotificationId
                                    where un.UserId == userId
                                          && (un.IsDeleted == null || un.IsDeleted == false)
                                          && un.CreatedDate >= fromDate
                                          && un.CreatedDate <= toDate
                                    orderby un.CreatedDate descending
                                    select new NotificationViewModel
                                    {
                                        NotificationId = n.NotificationId,
                                        UserNotificationId = un.UserNotificationId,
                                        Title = n.Title,
                                        Message = n.Message,
                                        NotificationType = n.NotificationType,
                                        CreatedDate = un.CreatedDate ?? DateTime.Now,
                                        IsRead = un.IsRead ?? false
                                    };

                return Json(new { Success = true, Data = notifications.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // NEW: Clean up old notifications (can be called by a scheduled job)
        [HttpPost]
        [Route("Notification/CleanupOldNotifications")]
        public ActionResult CleanupOldNotifications(int daysOld = 30)
        {
            try
            {
                DateTime cutoffDate = DateTime.Now.AddDays(-daysOld);

                var oldNotifications = DB.UserNotifications
                    .Where(x => x.CreatedDate < cutoffDate
                        && (x.IsDeleted == null || x.IsDeleted == false))
                    .ToList();

                foreach (var notification in oldNotifications)
                {
                    notification.IsDeleted = true; // Soft delete
                }

                DB.SaveChanges();

                return Json(new
                {
                    Success = true,
                    Message = $"Cleaned up {oldNotifications.Count} old notifications"
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            if (System.Web.HttpContext.Current.Session["EmpId"] != null)
            {
                return Convert.ToInt32(System.Web.HttpContext.Current.Session["EmpId"]);
            }
            return 0;
        }
    }
}