using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modern.Models
{
    public class ClientSubscription
    {
        public string Endpoint { get; set; }
        public string ExpirationTime { get; set; }
        public ClientSubscriptionKeys Keys { get; set; }
    }

    public class ClientSubscriptionKeys
    {
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}