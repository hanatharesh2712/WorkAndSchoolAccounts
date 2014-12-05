
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_MultiOrg_AccessGraph.Models
{
    public class PerWebUserCache
    {
        [Key]
        public int EntryId { get; set; }
        public string webUserUniqueId { get; set; }
        public byte[] cacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }

    public class TenantDbContext : DbContext
    {
        public TenantDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<PerWebUserCache> PerUserCacheList { get; set; }
    }
}