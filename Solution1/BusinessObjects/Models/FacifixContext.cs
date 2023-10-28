using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class FacifixContext : DbContext
{
    public FacifixContext()
    {
    }

    public FacifixContext(DbContextOptions<FacifixContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetHistory> AssetHistories { get; set; }

    public virtual DbSet<AssignedDetail> AssignedDetails { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<FixTask> FixTasks { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database=Facifix;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("Asset");

            entity.Property(e => e.AssetId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AssetName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ImportedDate).HasColumnType("datetime");
            entity.Property(e => e.ImporterId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssetHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId);

            entity.ToTable("AssetHistory");

            entity.Property(e => e.HistoryId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AssetId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetHistories)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssetHistory_Asset");
        });

        modelBuilder.Entity<AssignedDetail>(entity =>
        {
            entity.HasKey(e => e.AssignedDetailsId);

            entity.Property(e => e.AssignedDetailsId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaskId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.AssignedDetails)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignedDetails_Account");

            entity.HasOne(d => d.Task).WithMany(p => p.AssignedDetails)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignedDetails_Task");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AssetId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SubmitedDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Asset).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedback_Asset");
        });

        modelBuilder.Entity<FixTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK_Task");

            entity.ToTable("FixTask");

            entity.Property(e => e.TaskId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AuthorId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.FeedbackId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProcessedDate).HasColumnType("datetime");
            entity.Property(e => e.ReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Author).WithMany(p => p.FixTasks)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Account");

            entity.HasOne(d => d.Feedback).WithMany(p => p.FixTasks)
                .HasForeignKey(d => d.FeedbackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FixTask_Feedback");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
