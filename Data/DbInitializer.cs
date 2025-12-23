using SMS.API.Models;
using SMS.API.Models.Enums;

namespace SMS.API.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    FullName = "Super Admin",
                    Email = "admin@sms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.SuperAdmin
                });
                context.Users.Add(new User
                {
                    FullName = "Admin",
                    Email = "adm@sms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.SchoolAdmin,
                    //SchoolId = 1
                });
                context.Subscriptions.Add(new Subscription
                {
                    PlanName = "testplan",
                    Price = 3000,
                    DurationInMonths = 1,
                    IsActive = true,
                    MaxStudents = 60,
                    MaxTeachers = 10
                });

                context.SaveChanges();
            }
        }
    }
}
