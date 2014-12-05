
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_SSO_AccessGraph.Models
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<PerWebUserCache> PerUserCacheList { get; set; }
    }

    public class PerWebUserCache
    {
        [Key]
        public int EntryId { get; set; }
        public string webUserUniqueId { get; set; }
        public byte[] cacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }

}