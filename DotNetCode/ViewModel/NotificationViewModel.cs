using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.ViewModel
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ShortDescription { get; set; }
        public string NotificationType { get; set; }
        public string NotificationSubType { get; set; }
        public int? ReferenceId { get; set; }
        public string ActionUrl { get; set; }
        public string NotificationIcon { get; set; }
        public string NotificationColor { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsStarred { get; set; }
        public int UserNotificationId { get; set; }
    }

    public class NotificationResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int NotificationId { get; set; }
        public int RecipientCount { get; set; }
    }

    public class NotificationFilterViewModel
    {
        public string Type { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class MarkReadViewModel
    {
        public List<int> NotificationIds { get; set; }
        public bool MarkAll { get; set; }
    }

    public class UnreadCountViewModel
    {
        public int TotalUnread { get; set; }
        public Dictionary<string, int> ModuleWiseCount { get; set; }
    }
    public partial class NotificationDTO
    {
        public int NotificationId { get; set; }
        public System.Guid NotificationGuid { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ShortDescription { get; set; }
        public string NotificationType { get; set; }
        public string NotificationSubType { get; set; }
        public Nullable<int> ReferenceId { get; set; }
        public Nullable<System.Guid> ReferenceGuid { get; set; }
        public string ActionUrl { get; set; }
        public string ActionText { get; set; }
        public string NotificationIcon { get; set; }
        public string NotificationColor { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public virtual ICollection<UserNotification> UserNotifications { get; set; }
    }

    public partial class UserNotification
    {
        public int UserNotificationId { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
        public Nullable<bool> IsStarred { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public Nullable<System.DateTime> DeliveredDate { get; set; }
        public Nullable<bool> IsActionTaken { get; set; }
        public Nullable<System.DateTime> ActionTakenDate { get; set; }

        public virtual Notification Notification { get; set; }
    }
}