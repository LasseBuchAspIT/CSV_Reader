using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSV_Reader.DAL;

public partial class CsvProgramTestContext : DbContext
{
    string connectionString;
    public CsvProgramTestContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public CsvProgramTestContext(DbContextOptions<CsvProgramTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.CustomerNumber).HasName("PK_Accounts_1");

            entity.Property(e => e.CustomerNumber)
                .ValueGeneratedNever()
                .HasColumnName("Customer_Number");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("Customer_Name");
            entity.Property(e => e.Fsa)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("FSA");
            entity.Property(e => e.OuterNumber).HasColumnName("Outer_Number");
            entity.Property(e => e.System)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Vip)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("VIP");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
