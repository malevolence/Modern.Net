using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modern.Models
{
    public class PushNotification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public string Tag { get; set; }
        public NotificationData Data { get; set; } = new NotificationData();
        public List<NotificationAction> Actions { get; set; } = new List<NotificationAction>();
    }

    public class NotificationData
    {
        public string ListingId { get; set; }
        public string Url { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }

    public class NotificationAction
    {
        public string Action { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}