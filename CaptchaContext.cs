using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Captcha
{
    public class CaptchaContext : DbContext
    {
        public CaptchaContext(DbContextOptions<CaptchaContext> options)
                : base(options)
        { }

        public DbSet<CaptchaRecord> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaptchaRecord>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });
        }
    }

    public class CaptchaRecord
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public long CreatedTime { get; set; }
    }
}