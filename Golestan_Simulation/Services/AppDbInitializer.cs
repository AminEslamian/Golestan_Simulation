using Golestan_Simulation.Data;
using Golestan_Simulation.Services;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Services
{
    public static class AppDbInitializer
    {
        private static readonly IPassHasherService _passHasher = new PassHasherService();
        public static async Task SeedAdminAsync(ApplicationDbContext context)
        {
            if(!context.Roles.Any(r => r.Name == Models.RolesEnum.Admin))
            {
                context.Roles.Add(new Models.Roles { Name = Models.RolesEnum.Admin });
                await context.SaveChangesAsync();
            }

            if(!context.Users.Any(u => u.UserName == "Admin"))
            {
                context.Users.Add(
                    new Models.Users
                    {
                        CreatedAt = DateTime.Now,
                        UserName = "Admin",
                        HashedPassword = _passHasher.HashPassword("@Admin1234")
                    }
                );
                await context.SaveChangesAsync();
            }

            if(!context.UserRoles.Any(ur => ur.User.UserName == "Admin" && ur.Role.Name == Models.RolesEnum.Admin))
            {
                context.UserRoles.Add(
                    new Models.UserRoles
                    {
                        UserId = context.Users.First(u => u.UserName == "Admin").Id,
                        RoleId = context.Roles.First(r => r.Name == Models.RolesEnum.Admin).Id
                    }
                );

                await context.SaveChangesAsync();
            }

        }
    }
}
