using Modern.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Modern.Data
{
    public class SiteContext : DbContext
    {
        public SiteContext() : base("SiteCN")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<PushSubscription> PushSubscriptions { get; set; }
    }
}