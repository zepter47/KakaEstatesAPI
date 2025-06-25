using System;
using System.Collections.Generic;
using JamilNativeAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace JamilNativeAPI.Context;

public partial class KakaireEstatesContext : DbContext
{
    public KakaireEstatesContext()
    {
    }

    public KakaireEstatesContext(DbContextOptions<KakaireEstatesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sequence> Sequences { get; set; }

    public virtual DbSet<TblHouse> TblHouses { get; set; }

    public virtual DbSet<TblMaritalstatus> TblMaritalstatuses { get; set; }

    public virtual DbSet<TblNokRelationship> TblNokRelationships { get; set; }

    public virtual DbSet<TblPayment> TblPayments { get; set; }

    public virtual DbSet<TblPaymentDetail> TblPaymentDetails { get; set; }

    public virtual DbSet<TblTenant> TblTenants { get; set; }

    public virtual DbSet<TblWaterFactor> TblWaterFactors { get; set; }

    public virtual DbSet<TblWaterbill> TblWaterbills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:DefaultDb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_unicode_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Sequence>(entity =>
        {
            entity.HasKey(e => e.SequenceName).HasName("PRIMARY");

            entity.ToTable("sequences");

            entity.Property(e => e.SequenceName).HasColumnName("sequence_name");
            entity.Property(e => e.CurrentValue).HasColumnName("current_value");
        });

        modelBuilder.Entity<TblHouse>(entity =>
        {
            entity.HasKey(e => e.HouseId).HasName("PRIMARY");

            entity.ToTable("tbl_house");

            entity.Property(e => e.HouseId).HasColumnName("house_id");
            entity.Property(e => e.HouseNumber)
                .HasMaxLength(20)
                .HasColumnName("house_number");
        });

        modelBuilder.Entity<TblMaritalstatus>(entity =>
        {
            entity.HasKey(e => e.MaritalstatusId).HasName("PRIMARY");

            entity.ToTable("tbl_maritalstatus");

            entity.Property(e => e.MaritalstatusId).HasColumnName("maritalstatus_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<TblNokRelationship>(entity =>
        {
            entity.HasKey(e => e.NokRelationshipId).HasName("PRIMARY");

            entity.ToTable("tbl_nok_relationship");

            entity.Property(e => e.NokRelationshipId).HasColumnName("nok_relationship_id");
            entity.Property(e => e.Relatioship)
                .HasMaxLength(50)
                .HasColumnName("relatioship");
        });

        modelBuilder.Entity<TblPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.ToTable("tbl_payment");

            entity.HasIndex(e => e.WaterbillId, "tbl_payment_waterbill_id_key").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.AddedOn)
                .HasColumnType("datetime")
                .HasColumnName("added_on");
            entity.Property(e => e.AmountOwed)
                .HasPrecision(12, 2)
                .HasColumnName("amount_owed");
            entity.Property(e => e.WaterbillId).HasColumnName("waterbill_id");

            entity.HasOne(d => d.Waterbill).WithOne(p => p.TblPayment)
                .HasForeignKey<TblPayment>(d => d.WaterbillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_payment_waterbill_id_fkey");
        });

        modelBuilder.Entity<TblPaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentDetailsId).HasName("PRIMARY");

            entity.ToTable("tbl_payment_details");

            entity.HasIndex(e => e.PaymentId, "tbl_payment_details_payment_id_fkey");

            entity.Property(e => e.PaymentDetailsId).HasColumnName("payment_details_id");
            entity.Property(e => e.AddedOn)
                .HasColumnType("datetime")
                .HasColumnName("added_on");
            entity.Property(e => e.AmountPaid)
                .HasPrecision(12, 2)
                .HasColumnName("amount_paid");
            entity.Property(e => e.AmountRemaining)
                .HasPrecision(12, 2)
                .HasColumnName("amount_remaining");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");

            entity.HasOne(d => d.Payment).WithMany(p => p.TblPaymentDetails)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_payment_details_payment_id_fkey");
        });

        modelBuilder.Entity<TblTenant>(entity =>
        {
            entity.HasKey(e => e.TenantId).HasName("PRIMARY");

            entity.ToTable("tbl_tenant");

            entity.HasIndex(e => e.HouseId, "tbl_tenant_house_id_fkey");

            entity.HasIndex(e => e.MaritalstatusId, "tbl_tenant_maritalstatus_id_fkey");

            entity.HasIndex(e => e.NokRelationshipId, "tbl_tenant_nok_relationship_id_fkey");

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.AddedOn)
                .HasColumnType("datetime")
                .HasColumnName("added_on");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .HasColumnName("gender");
            entity.Property(e => e.HouseId).HasColumnName("house_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("last_name");
            entity.Property(e => e.MaritalstatusId).HasColumnName("maritalstatus_id");
            entity.Property(e => e.NextofkinName)
                .HasMaxLength(50)
                .HasColumnName("nextofkin_name");
            entity.Property(e => e.NinNumber)
                .HasMaxLength(30)
                .HasColumnName("nin_number");
            entity.Property(e => e.NokPhonenumber)
                .HasMaxLength(30)
                .HasColumnName("nok_phonenumber");
            entity.Property(e => e.NokRelationshipId).HasColumnName("nok_relationship_id");
            entity.Property(e => e.OccupantsNumber).HasColumnName("occupants_number");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.House).WithMany(p => p.TblTenants)
                .HasForeignKey(d => d.HouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_tenant_house_id_fkey");

            entity.HasOne(d => d.Maritalstatus).WithMany(p => p.TblTenants)
                .HasForeignKey(d => d.MaritalstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_tenant_maritalstatus_id_fkey");

            entity.HasOne(d => d.NokRelationship).WithMany(p => p.TblTenants)
                .HasForeignKey(d => d.NokRelationshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_tenant_nok_relationship_id_fkey");
        });

        modelBuilder.Entity<TblWaterFactor>(entity =>
        {
            entity.HasKey(e => e.WaterFactorId).HasName("PRIMARY");

            entity.ToTable("tbl_water_factor");

            entity.Property(e => e.WaterFactorId).HasColumnName("water_factor_id");
            entity.Property(e => e.FactorAmount)
                .HasPrecision(12, 2)
                .HasColumnName("factor_amount");
            entity.Property(e => e.UnitFactor).HasColumnName("unit_factor");
        });

        modelBuilder.Entity<TblWaterbill>(entity =>
        {
            entity.HasKey(e => e.WaterbillId).HasName("PRIMARY");

            entity.ToTable("tbl_waterbill");

            entity.HasIndex(e => e.HouseId, "tbl_waterbill_house_id_fkey");

            entity.HasIndex(e => e.TenantId, "tbl_waterbill_tenant_id_fkey");

            entity.Property(e => e.WaterbillId).HasColumnName("waterbill_id");
            entity.Property(e => e.AddedOn)
                .HasColumnType("datetime")
                .HasColumnName("added_on");
            entity.Property(e => e.CurrentReading)
                .HasPrecision(7, 2)
                .HasColumnName("current_reading");
            entity.Property(e => e.CustomInvoice)
                .HasMaxLength(50)
                .HasColumnName("custom_invoice");
            entity.Property(e => e.HouseId).HasColumnName("house_id");
            entity.Property(e => e.PreviousReading)
                .HasPrecision(7, 2)
                .HasColumnName("previous_reading");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.House).WithMany(p => p.TblWaterbills)
                .HasForeignKey(d => d.HouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_waterbill_house_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.TblWaterbills)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_waterbill_tenant_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
