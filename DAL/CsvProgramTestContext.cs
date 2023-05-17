using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSV_Reader.DAL;

public partial class CsvProgramTestContext : DbContext
{
    public CsvProgramTestContext()
    {
    }

    public CsvProgramTestContext(DbContextOptions<CsvProgramTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CSV_Program_Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.CostumerNumber).HasName("PK_Accounts_1");

            entity.Property(e => e.CostumerNumber)
                .ValueGeneratedNever()
                .HasColumnName("Costumer_Number");
            entity.Property(e => e.CostumerName)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("Costumer_Name");
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
