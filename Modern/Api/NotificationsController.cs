using Modern.Data;
using Modern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Modern.Api
{
    public class NotificationsController : ApiController
    {
        private readonly SiteContext db;

        public NotificationsController()
        {
            this.db = new SiteContext();
        }

        [HttpPost]
        public IHttpActionResult Subscribe([FromBody] ClientSubscription subscription)
        {
            // save this subscription into the database
            if (ModelState.IsValid)
            {
                var pushSub = new PushSubscription
                {
                    Username = "malevolence",
                    Url = subscription.Endpoint,
                    P256dh = subscription.Keys.P256dh,
                    Auth = subscription.Keys.Auth
                };

                db.PushSubscriptions.Add(pushSub);
                db.SaveChanges();

                var data = new { data = new { success = true } };

                return Ok(data);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}