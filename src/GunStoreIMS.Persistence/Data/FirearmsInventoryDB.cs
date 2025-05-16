using GunStoreIMS.Domain.Models;
using Microsoft.EntityFrameworkCore;



namespace GunStoreIMS.Persistence.Data
{
    public class FirearmsInventoryDB : DbContext
    {
        public FirearmsInventoryDB(DbContextOptions<FirearmsInventoryDB> options) : base(options)
        {
        }

        public DbSet<Form4473Record> Form4473Records { get; set; }
        public DbSet<Form4473FirearmLine> Form4473FirearmLines { get; set; }

        // Optional: override OnModelCreating to configure relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form4473Record>()
                .HasMany(r => r.Form4473FirearmLines) // ✅ Correct property name
                .WithOne(l => l.Form4473Record)
                .HasForeignKey(l => l.Form4473RecordId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }



    }

}
