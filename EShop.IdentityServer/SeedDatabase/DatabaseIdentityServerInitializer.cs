using EShop.IdentityServer.Configuration;
using EShop.IdentityServer.Data;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EShop.IdentityServer.SeedDatabase
{
    public class DatabaseIdentityServerInitializer : ISeedDatabaseInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseIdentityServerInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void InitializeSeedRoles()
        {
            if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
            {
                //cria o perfil admin
                IdentityRole roleAdmin = new();
                roleAdmin.Name = IdentityConfiguration.Admin;
                roleAdmin.NormalizedName = IdentityConfiguration.Admin.ToUpper();
                _roleManager.CreateAsync(roleAdmin).Wait();
            }
            if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
            {
                //cria o perfil client
                IdentityRole roleClient = new();
                roleClient.Name = IdentityConfiguration.Client;
                roleClient.NormalizedName = IdentityConfiguration.Client.ToUpper();
                _roleManager.CreateAsync(roleClient).Wait();
            }
        }

        public void InitializeSeedUsers()
        {
            if (_userManager.FindByEmailAsync("admin1@com.br").Result == null)
            {
                //define os dados
                ApplicationUser admin = new()
                {
                    UserName = "admin1",
                    NormalizedUserName = "ADMIN1",
                    Email = "admin1@com.br",
                    NormalizedEmail = "ADMIN1@COM.BR",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "+55 (11) 12345-6789",
                    FirstName = "Usuario",
                    LastName = "Admin1",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                //cria o usuario
                IdentityResult resultAdmin = _userManager.CreateAsync(admin, "Numsey#2023").Result;
                if (resultAdmin.Succeeded)
                {
                    _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();

                    var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                        new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                        new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                    }).Result;
                }
            }
        }
    }
}
