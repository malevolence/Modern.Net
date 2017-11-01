using Modern.Data;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modern.Utils;
using System.Threading.Tasks;
using System.Net.Http;
using Modern.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Modern.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteContext db;

        public HomeController()
        {
            this.db = new SiteContext();
        }

        public ActionResult Index()
        {
            var subs = db.PushSubscriptions.OrderBy(x => x.CreateDate).ToList();

            return View(subs);
        }

        public ActionResult Keys()
        {
            var vapidDetails = WebPush.VapidHelper.GenerateVapidKeys();

            return View(vapidDetails);
        }

        [HttpPost]
        public async Task<ActionResult> SendNotification(int id)
        {
            var sub = db.PushSubscriptions.Find(id);
            if (sub == null)
            {
                return HttpNotFound();
            }
            else
            {
                var publicKey = ConfigurationManager.AppSettings["VapidPublicKey"];
                var privateKey = ConfigurationManager.AppSettings["VapidPrivateKey"];
                var subject = @"mailto: webmaster@productionhub.com";

                Uri uri = new Uri(sub.Url);
                string audience = $"{uri.Scheme}{Uri.SchemeDelimiter}{uri.Host}";

                var subscription = new WebPush.PushSubscription(sub.Url, sub.P256dh, sub.Auth);
                var vapidDetails = new WebPush.VapidDetails(subject, publicKey, privateKey);

                var client = new WebPush.WebPushClient();
                try
                {
                    var opts = new Dictionary<string, object>();
                    opts["vapidDetails"] = vapidDetails;

                    var notification = new PushNotification();
                    notification.Title = "Something New Happened!";
                    notification.Body = "There was some activity on your account and you should go do something about it.";
                    notification.Icon = "https://s3.amazonaws.com/images.productionhub.com/icons/asterisk.png";
                    notification.Data.Url = "/";
                    notification.Data.ListingId = "142484";
                    notification.Actions.Add(new NotificationAction { Action = "ignore", Title = "Ignore Lead", Icon = "https://s3.amazonaws.com/images.productionhub.com/icons/icon_remove_red.png" });
                    notification.Actions.Add(new NotificationAction { Action = "buy", Title = "Purchase Lead", Icon = "https://s3.amazonaws.com/images.productionhub.com/icons/icon_credit.png" });

                    var payload = JsonConvert.SerializeObject(notification, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                    var requestDetails = client.GenerateRequestDetails(subscription, payload, opts);


                    //await client.SendNotificationAsync(subscription, "payload", vapidDetails);
                    var httpClient = new HttpClient();
                    await httpClient.SendAsync(requestDetails);
                    return Json(true);
                }
                catch (WebPush.WebPushException exc)
                {
                    return new HttpStatusCodeResult(exc.StatusCode);
                }
            }
        }
    }
}