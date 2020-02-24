using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string AdminUser = "Admin";
        private const string AdminPassword = "Secret123$";

        public static async Task<IApplicationBuilder> EnsureIdentityPopulatedAsync(this IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(AdminUser).ConfigureAwait(false);

            if (user != null) return app;

            user = new IdentityUser(AdminUser);
            await userManager.CreateAsync(user, AdminPassword).ConfigureAwait(false);

            return app;
        }
    }
}