using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Models;
public partial class JobPortalDbContext : DbContext
{
    public JobPortalDbContext()
    {
    }

    public JobPortalDbContext(DbContextOptions<JobPortalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyReview> CompanyReviews { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<JobCategory> JobCategories { get; set; }

    public virtual DbSet<SavedJob> SavedJobs { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CAC6723486D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LogoUrl).HasMaxLength(2048);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Website).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Companies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Companies__UserI__5070F446");
        });

        modelBuilder.Entity<CompanyReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__CompanyR__74BC79CED94A4A34");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyReviews)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyRe__Compa__7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.CompanyReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyRe__UserI__7C4F7684");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Jobs__056690C2A52ED966");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExperienceRange).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.SalaryRange).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__CategoryId__619B8048");

            entity.HasOne(d => d.Company).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__CompanyId__628FA481");

            entity.HasOne(d => d.Skill).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__SkillId__6383C8BA");

            entity.HasOne(d => d.User).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__UserID__6477ECF3");
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.JobApplicationId).HasName("PK__JobAppli__BD557F85C3D27BFB");

            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResumeUrl).HasMaxLength(2048);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Applied");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Job).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__JobId__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__UserI__70DDC3D8");
        });

        modelBuilder.Entity<JobCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__JobCateg__19093A0B6FF19303");

            entity.HasIndex(e => e.CategoryName, "UQ__JobCateg__8517B2E03303CBC3").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.JobCategories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobCatego__UserI__5629CD9C");
        });

        modelBuilder.Entity<SavedJob>(entity =>
        {
            entity.HasKey(e => e.SavedJobId).HasName("PK__SavedJob__B7E5F397E3117C57");

            entity.Property(e => e.SavedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Job).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SavedJobs__JobId__75A278F5");

            entity.HasOne(d => d.User).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SavedJobs__UserI__74AE54BC");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__DFA09187D91B8461");

            entity.HasIndex(e => e.SkillName, "UQ__Skills__B63C657161FD4E71").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SkillName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Skills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Skills__UserId__5BE2A6F2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C9E914637");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343ECFD8BB").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(2048);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserSkillId).HasName("PK__UserSkil__2F28BE567D3CAA54");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSkill__Skill__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSkill__UserI__68487DD7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
