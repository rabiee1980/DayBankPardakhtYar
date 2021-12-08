using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Transfer.Domain.Models
{
    public partial class TSS_DBContext : DbContext
    {
        public TSS_DBContext()
        {
        }

        public TSS_DBContext(DbContextOptions<TSS_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountStatement> AccountStatement { get; set; }
        public virtual DbSet<ErrorCodes> ErrorCodes { get; set; }
        public virtual DbSet<Errors> Errors { get; set; }
        public virtual DbSet<IbanInfo> IbanInfo { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<TransferStatus> TransferStatus { get; set; }
        public virtual DbSet<TransferStatusWS> TransferStatusWS { get; set; }
        public virtual DbSet<TransferType> TransferType { get; set; }
        public virtual DbSet<TurnoverType> TurnoverType { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountStatement>(entity =>
            {
                entity.ToTable("AccountStatement", "Transfer");

                entity.Property(e => e.AgentBranchCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgentBranchName).HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BranchName).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatementId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransferAmount).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<ErrorCodes>(entity =>
            {
                entity.ToTable("ErrorCodes", "Transfer");

                entity.Property(e => e.EnDescription)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ExtraData).HasMaxLength(250);

                entity.Property(e => e.FaDescription)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Errors>(entity =>
            {
                entity.ToTable("Errors", "Transfer");

                entity.Property(e => e.ErrorDescription)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ExtraData)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.ErrorCodeNavigation)
                    .WithMany(p => p.Errors)
                    .HasForeignKey(d => d.ErrorCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Errors_ErrorCodes");
            });

            modelBuilder.Entity<IbanInfo>(entity =>
            {
                entity.ToTable("IbanInfo", "Transfer");

                entity.Property(e => e.Cif)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DspositNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ForeignName).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.ToTable("Transactions", "Transfer");

                entity.Property(e => e.Acceptable).HasDefaultValueSql("((0))");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 0)");

                entity.Property(e => e.Cancelable).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreationJalaliDateTime)
                    .IsRequired()
                    .HasMaxLength(19)
                    .IsUnicode(false)
                    .HasDefaultValueSql("([dbo].[GETDateTimeJalali]())");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DestinationIban)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.OwnerNationalId)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReferenceId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Resumeable).HasDefaultValueSql("((0))");

                entity.Property(e => e.SourceIban)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsUnicode(false);

                entity.Property(e => e.Suspendable).HasDefaultValueSql("((0))");

                entity.Property(e => e.TrackingNumber)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransferStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Error)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ErrorId)
                    .HasConstraintName("FK_Transfer_Errors");

                entity.HasOne(d => d.TransferStatusNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransferStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfer_TransferStatus");

                entity.HasOne(d => d.TransferType)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransferTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfer_TransferType");
            });

            modelBuilder.Entity<TransferStatus>(entity =>
            {
                entity.ToTable("TransferStatus", "Transfer");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TransferStatusWS>(entity =>
            {
                entity.ToTable("TransferStatusWS", "Transfer");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.TransferStatus)
                    .WithMany(p => p.TransferStatusWS)
                    .HasForeignKey(d => d.TransferStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransferStatusWS_TransferStatus");
            });

            modelBuilder.Entity<TransferType>(entity =>
            {
                entity.ToTable("TransferType", "Transfer");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TurnoverType>(entity =>
            {
                entity.ToTable("TurnoverType", "Transfer");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
