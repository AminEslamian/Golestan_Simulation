using Golestan_Simulation.Data;
using Golestan_Simulation.Services;
using Microsoft.EntityFrameworkCore;

namespace Golestan_Simulation.Services
{
    public static class AppDbInitializer
    {
        public static async void SeedAdminUser(ApplicationDbContext context)
        {
            if(!await context.UserRoles.AnyAsync(ur => ur.Roles.Name == Models.RolesEnum.Admin))
            {
                context.Roles.Add(new Models.Roles { Name = Models.RolesEnum.Admin });
                await context.SaveChangesAsync();
            }

            if(!await context.Users.AllAsync(u => u.UserName == "Admin"))
            {
                context.Users.Add(
                    new Models.Users
                    {
                        CreatedAt = DateTime.Now,
                        UserName = "Admin",
                        HashedPassword = PassHasher.HashPassword("@Admin1234")
                    }
                    );
                await context.SaveChangesAsync();
            }

            context.UserRoles.Add(
                new Models.UserRoles
                {
                    UserId = context.Users.First(u => u.UserName == "Admin").Id,
                    RoleId = context.Roles.First(r => r.Name == Models.RolesEnum.Admin).Id,
                }
            );

            await context.SaveChangesAsync();
        }
    }
}
