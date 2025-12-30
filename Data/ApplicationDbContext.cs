using Microsoft.EntityFrameworkCore;
using SMS.API.Models;

namespace SMS.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Example DbSet (required for migration)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.School)
                .WithMany()
                .HasForeignKey(u => u.SchoolId);
            modelBuilder.Entity<Subscription>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SchoolSubscription>()
                .HasOne(x => x.School)
                .WithMany()
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolSubscription>()
                .HasOne(x => x.Subscription)
                .WithMany()
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolAdmin>()
    .HasOne(sa => sa.User)
    .WithMany()
    .HasForeignKey(sa => sa.UserId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolAdmin>()
                .HasOne(sa => sa.School)
                .WithMany()
                .HasForeignKey(sa => sa.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
    .HasOne(t => t.User)
    .WithMany()
    .HasForeignKey(t => t.UserId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.School)
                .WithMany()
                .HasForeignKey(t => t.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
    .HasOne(u => u.Teacher)
    .WithOne(t => t.User)
    .HasForeignKey<Teacher>(t => t.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            //        modelBuilder.Entity<Student>()
            //.HasOne(s => s.User)
            //.WithMany()
            //.HasForeignKey(s => s.UserId)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany()
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
    .HasOne(a => a.Student)
    .WithMany()
    .HasForeignKey(a => a.StudentId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.StudentId, a.Date })
                .IsUnique();

            modelBuilder.Entity<Mark>()
    .HasOne(m => m.Student)
    .WithMany()
    .HasForeignKey(m => m.StudentId);

            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Exam)
                .WithMany()
                .HasForeignKey(m => m.ExamId);

            modelBuilder.Entity<Mark>()
                .Property(m => m.Score)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Mark>()
                .Property(m => m.MaxScore)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ParentStudent>()
    .HasOne(ps => ps.Student)
    .WithMany()
    .HasForeignKey(ps => ps.StudentId);

            modelBuilder.Entity<Timetable>()
       .HasOne(t => t.Subject)
       .WithMany()
       .HasForeignKey(t => t.SubjectId)
       .OnDelete(DeleteBehavior.Restrict); // 🔥 IMPORTANT

            modelBuilder.Entity<Timetable>()
                .HasOne(t => t.Class)
                .WithMany()
                .HasForeignKey(t => t.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
        .HasOne(a => a.School)
        .WithMany()
        .HasForeignKey(a => a.SchoolId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Class)
                .WithMany()
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
        .HasOne(s => s.Class)
        .WithMany(c => c.Students)
        .HasForeignKey(s => s.ClassId)
        .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<SchoolSubscription> SchoolSubscriptions { get; set; } = null!;
        public DbSet<SchoolAdmin> SchoolAdmins { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<Mark> Marks { get; set; } = null!;
        public DbSet<ParentStudent> ParentStudents { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TeacherClass> TeacherClasses { get; set; } = null!;

        //public DbSet<Teacher> Teachers => Set<Teacher>();

    }
}
