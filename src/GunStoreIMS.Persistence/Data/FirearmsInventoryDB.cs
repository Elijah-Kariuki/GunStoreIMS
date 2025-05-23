using GunStoreIMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Persistence.Data
{
    public class FirearmsInventoryDB : DbContext
    {
        public FirearmsInventoryDB(DbContextOptions<FirearmsInventoryDB> options)
            : base(options)
        {
        }

        // --- Core A&D tables ---
        public DbSet<AcquisitionRecord> AcquisitionRecords { get; set; } = null!;
        public DbSet<DispositionRecord> DispositionRecords { get; set; } = null!;
        public DbSet<DealerRecord> DealerRecords { get; set; } = null!;

        // --- Firearms (TPH) ---
        public DbSet<Firearm> Firearms { get; set; } = null!;
        public DbSet<Silencer> Silencers { get; set; } = null!;

        // --- FFLs, Permits, NFA, and Serial Tracking ---
        public DbSet<FFL> FFLs { get; set; } = null!;
        public DbSet<PermitDetails> PermitDetails { get; set; } = null!;
        public DbSet<NfaItem> NfaItems { get; set; } = null!;
        public DbSet<SerialNumberHistory> SerialNumberHistories { get; set; } = null!;

        // --- ATF Form 4473 & Transaction Tracking ---
        public DbSet<Form4473Record> Form4473Records { get; set; } = null!;
        public DbSet<Form4473FirearmLine> Form4473FirearmLines { get; set; } = null!;

        // --- Buyer Eligibility & Background Checks ---
        public DbSet<BuyerInfo> BuyerInfos { get; set; } = null!;
        public DbSet<BuyerCertification> BuyerCertifications { get; set; } = null!;
        public DbSet<PlaceOfBirth> PlacesOfBirth { get; set; } = null!;
        public DbSet<ProhibitorAnswers> ProhibitorAnswers { get; set; } = null!;
        public DbSet<TransferorSignature> TransferorSignatures { get; set; } = null!;

        // --- Ancillary Records ---
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<BackgroundCheck> BackgroundChecks { get; set; } = null!;
        public DbSet<Recovery> Recoveries { get; set; } = null!;
        public DbSet<Caliber> Calibers { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Pick up any IEntityTypeConfiguration<> in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FirearmsInventoryDB).Assembly);

            // 2. TPH inheritance for Firearm hierarchy
            modelBuilder.Entity<Firearm>()
                .HasDiscriminator<string>("FirearmType")
                .HasValue<Firearm>("Firearm")
                .HasValue<Handgun>("Handgun")
                .HasValue<Pistol>("Pistol")
                .HasValue<Revolver>("Revolver")
                .HasValue<LongGun>("LongGun")
                .HasValue<Rifle>("Rifle")
                .HasValue<Shotgun>("Shotgun")
                .HasValue<Silencer>("Silencer");

            // 3. Explicit table mapping for Document (if not conventionally named)
            modelBuilder.Entity<Document>().ToTable("Documents");

            // 4. Core relationship configs

            // A&D
            modelBuilder.Entity<Firearm>()
                .HasMany(f => f.AcquisitionRecords)
                .WithOne(ar => ar.Firearm)
                .HasForeignKey(ar => ar.FirearmId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Firearm>()
                .HasMany(f => f.DispositionRecords)
                .WithOne(dr => dr.Firearm)
                .HasForeignKey(dr => dr.FirearmId)
                .OnDelete(DeleteBehavior.Restrict);

            // Serial history ↔ Document
            modelBuilder.Entity<Firearm>()
                .HasMany(f => f.SerialNumberHistories)
                .WithOne(snh => snh.Firearm)
                .HasForeignKey(snh => snh.FirearmId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SerialNumberHistory>()
                .HasOne(snh => snh.Document)
                .WithMany()
                .HasForeignKey(snh => snh.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Form4473 → lines
            modelBuilder.Entity<Form4473Record>()
                .HasMany(r => r.Form4473FirearmLines)
                .WithOne(l => l.Form4473Record)
                .HasForeignKey(l => l.Form4473RecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // Uniqueness indexes for ATF compliance
            modelBuilder.Entity<Firearm>()
                .HasIndex(f => new { f.Manufacturer, f.Model, f.SerialNumber, f.ImporterName })
                .IsUnique()
                .HasFilter($"\"{nameof(Firearm.ImporterName)}\" IS NOT NULL");

            modelBuilder.Entity<Firearm>()
                .HasIndex(f => new { f.Manufacturer, f.Model, f.SerialNumber })
                .IsUnique()
                .HasFilter($"\"{nameof(Firearm.ImporterName)}\" IS NULL");

            // Decimal precision
            modelBuilder.Entity<Firearm>(entity =>
            {
                entity.Property(e => e.BarrelLength).HasPrecision(5, 2);
                entity.Property(e => e.OverallLength).HasPrecision(5, 2);
            });

            // FKs for Caliber & FFL
            modelBuilder.Entity<Firearm>()
                .HasOne(f => f.Caliber)
                .WithMany()
                .HasForeignKey(f => f.CaliberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Firearm>()
                .HasOne(f => f.FFL)
                .WithMany()
                .HasForeignKey(f => f.FFLId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
