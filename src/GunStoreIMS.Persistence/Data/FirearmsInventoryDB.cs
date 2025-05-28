using System.Collections.Generic;
using GunStoreIMS.Domain.Models; // Ensure your models are in this namespace
using Microsoft.EntityFrameworkCore;

namespace GunStoreIMS.Persistence.Data
{
    public class FirearmsInventoryDB : DbContext
    {
        public FirearmsInventoryDB(DbContextOptions<FirearmsInventoryDB> options)
            : base(options)
        {
        }

        #region DbSets

        // --- Core A&D tables ---
        public DbSet<AcquisitionRecord> AcquisitionRecords { get; set; } = null!;
        public DbSet<DispositionRecord> DispositionRecords { get; set; } = null!;
        public DbSet<DealerRecord> DealerRecords { get; set; } = null!;

        // --- Firearms (TPH) ---
        public DbSet<Firearm> Firearms { get; set; } = null!;
        public DbSet<Silencer> Silencers { get; set; } = null!;

        // --- FFLs, NFA, and Serial Tracking ---
        public DbSet<NfaItem> NfaItems { get; set; } = null!;
        public DbSet<SerialNumberHistory> SerialNumberHistories { get; set; } = null!;

        // --- ATF Form 4473 & Transaction Tracking ---
        public DbSet<Form4473Record> Form4473Records { get; set; } = null!;
        public DbSet<Form4473FirearmLine> Form4473FirearmLines { get; set; } = null!;

        // --- Buyer Eligibility & Background Checks ---
        public DbSet<BuyerInfo> BuyerInfos { get; set; } = null!;

        // --- Ancillary Records ---
        public DbSet<Recovery> Recoveries { get; set; } = null!;
        public DbSet<Caliber> Calibers { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;

        // --- REMOVED DbSets for Owned Types ---
        // Address, PlaceOfBirth, ProhibitorAnswers, PermitDetails,
        // Certification, TransferorCertification, NICSCheck are now owned.

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Apply IEntityTypeConfiguration from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FirearmsInventoryDB).Assembly);

            // 2. TPH for Firearm
            modelBuilder.Entity<Firearm>(e => {
                e.ToTable("Firearms"); // Explicit Table Name
                e.HasDiscriminator<string>("FirearmType")
                    .HasValue<Firearm>("Firearm")
                    .HasValue<Handgun>("Handgun")
                    .HasValue<Pistol>("Pistol")
                    .HasValue<Revolver>("Revolver")
                    .HasValue<LongGun>("LongGun")
                    .HasValue<Rifle>("Rifle")
                    .HasValue<Shotgun>("Shotgun")
                    .HasValue<Silencer>("Silencer");

                // Precision & FKs
                e.Property(f => f.BarrelLength).HasPrecision(5, 2);
                e.Property(f => f.OverallLength).HasPrecision(5, 2);
                e.HasOne(f => f.Caliber).WithMany().HasForeignKey(f => f.CaliberId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(f => f.DealerRecord).WithMany().HasForeignKey(f => f.DealerRecordId).OnDelete(DeleteBehavior.Restrict);

                // Uniqueness Indexes
                e.HasIndex(f => new { f.Manufacturer, f.Model, f.SerialNumber, f.ImporterName })
                    .IsUnique()
                    .HasFilter($"\"{nameof(Firearm.ImporterName)}\" IS NOT NULL");

                e.HasIndex(f => new { f.Manufacturer, f.Model, f.SerialNumber })
                    .IsUnique()
                    .HasFilter($"\"{nameof(Firearm.ImporterName)}\" IS NULL");
            });

            // 3. Table mapping for Document
            modelBuilder.Entity<Document>().ToTable("Documents");

            #region Core Relationships

            // A&D Records <-> Firearm
            modelBuilder.Entity<AcquisitionRecord>().ToTable("AcquisitionRecords");
            modelBuilder.Entity<DispositionRecord>().ToTable("DispositionRecords");
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

            // Serial History <-> Firearm/Document
            modelBuilder.Entity<SerialNumberHistory>().ToTable("SerialNumberHistories");
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

            // Form 4473 <-> Firearm Lines
            modelBuilder.Entity<Form4473FirearmLine>().ToTable("Form4473FirearmLines");
            modelBuilder.Entity<Form4473Record>()
                .HasMany(r => r.Form4473FirearmLines)
                .WithOne(l => l.Form4473Record)
                .HasForeignKey(l => l.Form4473RecordId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Schema-Aligned Configurations

            // Configure Form4473FirearmLine based on $defs/Firearm
            modelBuilder.Entity<Form4473FirearmLine>(fl => {
                fl.Property(f => f.ManufacturerImporter).HasMaxLength(150).IsRequired();
                fl.Property(f => f.Model).HasMaxLength(60).IsRequired();
                fl.Property(f => f.SerialNumber).HasMaxLength(60).IsRequired();
                fl.Property(f => f.Type).HasMaxLength(50).IsRequired(); // Adjust length based on enum values
                fl.Property(f => f.CaliberGauge).HasMaxLength(30).IsRequired();
                // NOTE: Consider using C# enums for 'Type' and .HasConversion<string>()
            });

            // Configure DealerRecord based on $defs/DealerInfo
            modelBuilder.Entity<DealerRecord>(dr => {
                dr.ToTable("DealerRecords");
                dr.Property(d => d.TradeName).HasMaxLength(100).IsRequired();
                dr.Property(d => d.AddressLine1).HasMaxLength(100).IsRequired();
                dr.Property(d => d.AddressLine2).HasMaxLength(100); // Optional
                dr.Property(d => d.City).HasMaxLength(60).IsRequired();
                dr.Property(d => d.State).HasMaxLength(2).IsRequired();
                dr.Property(d => d.ZipCode).HasMaxLength(10).IsRequired();
                dr.Property(d => d.FFLNumber).HasMaxLength(15).IsRequired(); // X-XX-XXXXX format (10 chars) + buffer = 15
            });

            // Configure BuyerInfo and its owned types
            modelBuilder.Entity<BuyerInfo>(bi =>
            {
                bi.ToTable("BuyerInfos");

                bi.Property(b => b.LastName).HasMaxLength(60).IsRequired();
                bi.Property(b => b.FirstName).HasMaxLength(60).IsRequired();
                bi.Property(b => b.MiddleName).HasMaxLength(60);
                bi.Property(b => b.SSN).HasMaxLength(11);
                bi.Property(b => b.UPINorAMD).HasMaxLength(50);
                bi.Property(b => b.AlienOrAdmissionNumber).HasMaxLength(50);
                bi.Property(b => b.Sex).HasMaxLength(10); // "Non-Binary"
                bi.Property(b => b.Ethnicity).HasMaxLength(25).IsRequired();
                // NOTE: 'Race' and 'CountryOfCitizenship' are arrays in the schema.
                // These typically require a many-to-many relationship or storing as JSON/delimited string.
                // They are NOT configured here.

                bi.OwnsOne(b => b.ResidenceAddress, addr =>
                {
                    addr.Property(a => a.Street).HasColumnName("Address_Street").HasMaxLength(100).IsRequired();
                    addr.Property(a => a.City).HasColumnName("Address_City").HasMaxLength(60).IsRequired();
                    addr.Property(a => a.State).HasColumnName("Address_State").HasMaxLength(2).IsRequired();
                    addr.Property(a => a.Zip).HasColumnName("Address_Zip").HasMaxLength(10).IsRequired();
                    addr.Property(a => a.County).HasColumnName("Address_County").HasMaxLength(60).IsRequired();
                    addr.Property(a => a.ResideInCityLimits).HasColumnName("Address_ResideInCityLimits")
                        .HasMaxLength(7) // "Unknown"
                        .IsRequired(); // Schema uses $defs/YesNoUnknown
                    addr.WithOwner();
                });

                bi.OwnsOne(b => b.PlaceOfBirth, pob =>
                {
                    pob.Property(p => p.USCity).HasColumnName("POB_USCity").HasMaxLength(60);
                    pob.Property(p => p.USState).HasColumnName("POB_USState").HasMaxLength(2);
                    pob.Property(p => p.ForeignCountry).HasColumnName("POB_ForeignCountry").HasMaxLength(60);
                    pob.WithOwner();
                });

                bi.OwnsOne(b => b.Height, h =>
                {
                    h.Property(x => x.Feet).HasColumnName("Height_Feet");
                    h.Property(x => x.Inches).HasColumnName("Height_Inches");
                    h.WithOwner();
                });
            });

            // Configure Form4473Record and its owned types/relationships
            modelBuilder.Entity<Form4473Record>(fb =>
            {
                fb.ToTable("Form4473Records");

                // Relationships
                fb.HasOne(r => r.BuyerInfo).WithMany().HasForeignKey(r => r.BuyerInfoId).OnDelete(DeleteBehavior.Restrict);
                fb.HasOne(r => r.DealerRecord).WithMany().HasForeignKey(r => r.DealerRecordId).OnDelete(DeleteBehavior.Restrict);

                // Direct Properties
                fb.Property(r => r.TransactionNumber).HasMaxLength(50);
                fb.Property(r => r.TotalNumber).HasMaxLength(50);
                fb.Property(r => r.PawnRedemptionLines).HasMaxLength(100); // e.g., "1,3,5" etc.
                fb.Property(r => r.SupplementalDocs).HasMaxLength(200);
                fb.Property(r => r.NonImmigrantExceptionDocs).HasMaxLength(200);
                fb.Property(r => r.DealerNotes).HasMaxLength(500);
                // NOTE: 'FirearmCategory' is an array in the schema and is NOT configured here.

                // Owned Types
                fb.OwnsOne(r => r.Certification, bc =>
                {
                    bc.Property(x => x.Signature).HasColumnName("BuyerSignature"); // TODO: Decide string(Base64) or byte[]
                    bc.Property(x => x.DateSigned).HasColumnName("BuyerDateSigned");
                    bc.WithOwner();
                });

                fb.OwnsOne(r => r.BuyerRecertification, br =>
                {
                    br.Property(x => x.Signature).HasColumnName("BuyerRecertificationSignature");
                    br.Property(x => x.DateSigned).HasColumnName("BuyerRecertificationDateSigned");
                    br.WithOwner();
                });

                fb.OwnsOne(r => r.ProhibitorAnswers, pa =>
                {
                    pa.Property(p => p.IsActualTransferee).HasColumnName("Q21_IsActualTransferee");
                    // ... Map all other Q21 booleans like this ...
                    pa.Property(p => p.IntentToSellProhibitedPerson).HasColumnName("Q21_IntentToSellProhibitedPerson");
                    pa.WithOwner();
                });

                fb.OwnsOne(r => r.NICSChecks, nc =>
                {
                    // NoNicsRequiredReason enum → string
                    nc.Property(x => x.NoNicsRequiredReason)
                      .HasColumnName("Nics_NoNicsRequiredReason")
                      .HasConversion<string>()
                      .HasMaxLength(20);

                    nc.Property(x => x.DateContacted)
                      .HasColumnName("Nics_DateContacted");

                    nc.Property(x => x.TransactionNumber)
                      .HasColumnName("Nics_TransactionNumber")
                      .HasMaxLength(50);

                    // InitialResponse enum → string
                    nc.Property(x => x.InitialResponse)
                      .HasColumnName("Nics_InitialResponse")
                      .HasConversion<string>()
                      .HasMaxLength(50);

                    nc.Property(x => x.BradyTransferDate)
                      .HasColumnName("Nics_BradyTransferDate");

                    // LaterResponse enum → string
                    nc.Property(x => x.LaterResponse)
                      .HasColumnName("Nics_LaterResponse")
                      .HasConversion<string>()
                      .HasMaxLength(100);  // long enum names warrant a bit more room

                    nc.Property(x => x.LaterResponseDate)
                      .HasColumnName("Nics_LaterResponseDate");

                    // PostTransferResponse enum → string
                    nc.Property(x => x.PostTransferResponse)
                      .HasColumnName("Nics_PostTransferResponse")
                      .HasConversion<string>()
                      .HasMaxLength(50);

                    nc.Property(x => x.PostTransferResponseDate)
                      .HasColumnName("Nics_PostTransferResponseDate");

                    // Nest PermitDetails
                    nc.OwnsOne(x => x.PermitDetails, pd =>
                    {
                        // IssuingState enum → string
                        pd.Property(x => x.IssuingState)
                          .HasColumnName("Nics_Permit_IssuingState")
                          .HasConversion<string>()
                          .HasMaxLength(2);

                        pd.Property(x => x.PermitType)
                          .HasColumnName("Nics_Permit_Type")
                          .HasMaxLength(50);

                        pd.Property(x => x.IssuedDate)
                          .HasColumnName("Nics_Permit_IssuedDate");

                        pd.Property(x => x.ExpirationDate)
                          .HasColumnName("Nics_Permit_ExpirationDate");

                        pd.Property(x => x.PermitNumber)
                          .HasColumnName("Nics_Permit_Number")
                          .HasMaxLength(50);

                        pd.WithOwner();
                    });

                    nc.WithOwner();
                });


                fb.OwnsOne(r => r.TransferorCertification, tc =>
                {
                    tc.Property(x => x.Name).HasColumnName("TransferorName").HasMaxLength(100);
                    tc.Property(x => x.SignatureImage).HasColumnName("TransferorSignature"); // TODO: Decide string(Base64) or byte[]
                    tc.Property(x => x.DateTransferred).HasColumnName("TransferorDateTransferred");
                    tc.WithOwner();
                });

                fb.OwnsOne(r => r.GunShowDetails, gs =>
                {
                    gs.Property(x => x.Name).HasColumnName("GunShow_Name").HasMaxLength(100);
                    gs.Property(x => x.Address).HasColumnName("GunShow_Address").HasMaxLength(100); // Note: Simple string per schema
                    gs.Property(x => x.City).HasColumnName("GunShow_City").HasMaxLength(60);
                    gs.Property(x => x.State).HasColumnName("GunShow_State").HasMaxLength(2);
                    gs.Property(x => x.Zip).HasColumnName("GunShow_Zip").HasMaxLength(10);
                    gs.Property(x => x.County).HasColumnName("GunShow_County").HasMaxLength(60);
                    gs.WithOwner();
                    // NOTE: This owned type can be null if the C# property is nullable.
                });

                fb.OwnsOne(r => r.Identification, id =>
                {
                    id.Property(x => x.IssuingAuthorityAndType).HasColumnName("ID_IssuingAuthorityAndType").HasMaxLength(100).IsRequired();
                    id.Property(x => x.Number).HasColumnName("ID_Number").HasMaxLength(50).IsRequired();
                    id.Property(x => x.ExpirationDate).HasColumnName("ID_ExpirationDate").IsRequired();
                    id.WithOwner();
                });

                fb.OwnsOne(r => r.PCSOrders, pcs =>
                {
                    pcs.Property(x => x.PCSBaseCityState).HasColumnName("PCS_BaseCityState").HasMaxLength(150);
                    pcs.Property(x => x.PCSEffectiveDate).HasColumnName("PCS_EffectiveDate");
                    pcs.Property(x => x.PCSOrderNumber).HasColumnName("PCS_OrderNumber").HasMaxLength(50);
                    pcs.WithOwner();
                    // NOTE: This owned type can be null if the C# property is nullable.
                });

                fb.OwnsOne(r => r.SectionEDealerInfo, dealer =>
                {
                    dealer.Property(d => d.TradeName).HasColumnName("Dealer_TradeName").HasMaxLength(100).IsRequired();
                    dealer.Property(d => d.StreetAddress).HasColumnName("Dealer_StreetAddress").HasMaxLength(100).IsRequired();
                    dealer.Property(d => d.City).HasColumnName("Dealer_City").HasMaxLength(60).IsRequired();
                    dealer.Property(d => d.State).HasColumnName("Dealer_State").HasMaxLength(2).IsRequired();
                    dealer.Property(d => d.Zip).HasColumnName("Dealer_Zip").HasMaxLength(10).IsRequired();
                    dealer.Property(d => d.FFLNumber).HasColumnName("Dealer_FFLNumber").HasMaxLength(15).IsRequired();
                    dealer.WithOwner();
                });

            });

            #endregion
        }
    }
}