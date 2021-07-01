using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FLS_Accord.Models
{
    public partial class FLSContext : DbContext
    {
        public FLSContext()
        {
        }

        public FLSContext(DbContextOptions<FLSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogCategory> BlogCategory { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DepartmentBlog> DepartmentBlog { get; set; }
        public virtual DbSet<LectureSemesterRegister> LectureSemesterRegister { get; set; }
        public virtual DbSet<Lecturer> Lecturer { get; set; }
        public virtual DbSet<LecturerRating> LecturerRating { get; set; }
        public virtual DbSet<LecturerType> LecturerType { get; set; }
        public virtual DbSet<MasterPlan> MasterPlan { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<SemesterPlan> SemesterPlan { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectRegister> SubjectRegister { get; set; }
        public virtual DbSet<TeachableSubject> TeachableSubject { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
        public virtual DbSet<TimeSlotRegister> TimeSlotRegister { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:flscheduler.database.windows.net,1433;Initial Catalog=FLS;Persist Security Info=False;User ID=minhnt;Password=.yazw-EY!5hN9HS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasIndex(e => e.BlogCategoryId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ModifiledDate).HasColumnType("datetime");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.BlogCategory)
                    .WithMany(p => p.Blog)
                    .HasForeignKey(d => d.BlogCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blog_BlogCategory");
            });

            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.SemesterId);

                entity.HasIndex(e => e.SubjectId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Semester");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Subject1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<DepartmentBlog>(entity =>
            {
                entity.HasKey(e => new { e.DepartmentId, e.BlogId });

                entity.HasIndex(e => e.BlogId);

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.DepartmentBlog)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DepartmentBlog_Blog");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DepartmentBlog)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DepartmentBlog_Department");
            });

            modelBuilder.Entity<LectureSemesterRegister>(entity =>
            {
                entity.HasKey(e => new { e.LecturerId, e.SemesterId });

                entity.HasIndex(e => e.SemesterId);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.LectureSemesterRegister)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LectureSemesterRegister_Lecture");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.LectureSemesterRegister)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LectureSemesterRegister_Semester");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.HasIndex(e => e.DepartmentId);

                entity.HasIndex(e => e.LecturerTypeId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LecturerCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Lecturer)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lecture_Department");

                entity.HasOne(d => d.LecturerType)
                    .WithMany(p => p.Lecturer)
                    .HasForeignKey(d => d.LecturerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lecture_LecturerType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Lecturer)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lecture_User");
            });

            modelBuilder.Entity<LecturerRating>(entity =>
            {
                entity.HasKey(e => new { e.LecturerId, e.SemesterPlanId });

                entity.HasIndex(e => e.SemesterPlanId);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.LecturerRating)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LecturerRating_Lecture");

                entity.HasOne(d => d.SemesterPlan)
                    .WithMany(p => p.LecturerRating)
                    .HasForeignKey(d => d.SemesterPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LecturerRating_SemesterPlan");
            });

            modelBuilder.Entity<LecturerType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MasterPlan>(entity =>
            {
                entity.HasIndex(e => e.SemesterId);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.MasterPlan)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MasterPlan_Semester");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<SemesterPlan>(entity =>
            {
                entity.HasIndex(e => e.MasterPlanId);

                entity.HasIndex(e => e.SemesterId);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.MasterPlan)
                    .WithMany(p => p.SemesterPlan)
                    .HasForeignKey(d => d.MasterPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SemesterPlan_MasterPlan");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SemesterPlan)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SemesterPlan_Semester");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasIndex(e => e.DepartmentId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PreviousCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subject_Department");
            });

            modelBuilder.Entity<SubjectRegister>(entity =>
            {
                entity.HasIndex(e => e.LecturerId);

                entity.HasIndex(e => e.SemesterPlanId);

                entity.HasIndex(e => e.SubjectId);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.SubjectRegister)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectRegister_Lecture");

                entity.HasOne(d => d.SemesterPlan)
                    .WithMany(p => p.SubjectRegister)
                    .HasForeignKey(d => d.SemesterPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectRegister_SemesterPlan");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectRegister)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectRegister_Subject1");
            });

            modelBuilder.Entity<TeachableSubject>(entity =>
            {
                entity.HasKey(e => new { e.LecturerId, e.SubjectId })
                    .HasName("PK_PreferSubject");

                entity.HasIndex(e => e.SubjectId);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.TeachableSubject)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PreferSubject_Lecture");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TeachableSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeachableSubject_Subject");
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("time(0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("time(0)");
            });

            modelBuilder.Entity<TimeSlotRegister>(entity =>
            {
                entity.HasIndex(e => e.LecturerId);

                entity.HasIndex(e => e.SemesterPlanId);

                entity.HasIndex(e => e.TimeSlotId);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.TimeSlotRegister)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeSlotRegister_Lecture");

                entity.HasOne(d => d.SemesterPlan)
                    .WithMany(p => p.TimeSlotRegister)
                    .HasForeignKey(d => d.SemesterPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeSlotRegister_SemesterPlan");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.TimeSlotRegister)
                    .HasForeignKey(d => d.TimeSlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeSlotRegister_TimeSlot");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
