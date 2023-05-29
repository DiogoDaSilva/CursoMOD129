using Microsoft.AspNetCore.Identity;

namespace CursoMOD129.Data.SeedDatabase
{
    public class SeedDatabase
    {

        public static void Seed(ApplicationDbContext context, UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager).Wait();
            SeedUsers(userManager).Wait();
        }




        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleCheck = await roleManager.RoleExistsAsync(CursoMOD129Constants.ROLES.ADMIN);

            if (!roleCheck)
            {
                var adminRole = new IdentityRole
                {
                    Name = CursoMOD129Constants.ROLES.ADMIN
                };

                await roleManager.CreateAsync(adminRole);
            }


            roleCheck = await roleManager.RoleExistsAsync(CursoMOD129Constants.ROLES.ADMINISTRATIVE);

            if (!roleCheck)
            {
                var administrativeRole = new IdentityRole
                {
                    Name = CursoMOD129Constants.ROLES.ADMINISTRATIVE
                };

                await roleManager.CreateAsync(administrativeRole);
            }
        }


        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            var dbAdmin = await userManager.FindByNameAsync(CursoMOD129Constants.USERS.ADMIN.USERNAME);

            if (dbAdmin == null)
            {
                var result = await userManager.CreateAsync(
                    new IdentityUser
                    {
                        UserName = CursoMOD129Constants.USERS.ADMIN.USERNAME,
                        Email = CursoMOD129Constants.USERS.ADMIN.USERNAME
                    },
                    CursoMOD129Constants.USERS.ADMIN.PASSWORD
                );
            }
        }
    }
}
