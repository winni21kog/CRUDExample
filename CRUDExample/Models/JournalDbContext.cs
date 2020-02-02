using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUDExample.Models
{
    /// <summary>
    /// 日誌資料
    /// </summary>
    public class JournalDbContext:DbContext
    {
        public DbSet<DailyRecord> Records { get; set; }

        public JournalDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //建立日期 Unique Index
            modelBuilder.Entity<DailyRecord>().HasIndex(o => o.Date).IsUnique();
        }
    }
}
