using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Modern.Models
{
    [Table("Push_Subscriptions")]
    public class PushSubscription
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Url { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public string P256dh { get; set; }

        public string Auth { get; set; }
    }
}