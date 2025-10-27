using GymManagementSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.DataSeeding
{
    public static class IdentityDbContextSeeding
    {
        public static async Task<bool> DataSeed(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers=userManager.Users.Any();
                var HasRoles=roleManager.Roles.Any();
                if (HasUsers && HasRoles) return false;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new(){Name="SuperAdmin"},
                        new(){Name="Admin"}
                    };
                    foreach (var Role in Roles)
                    {
                        if(!roleManager.RoleExistsAsync(Role.Name!).Result)
                             roleManager.CreateAsync(Role).Wait();

                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Nour",
                        LastName = "Ahmed",
                        UserName = "NourAhmed",
                        Email = "nouryahmed607@gmail.com",
                        PhoneNumber = "01101943883"
                    };
                    userManager.CreateAsync(MainAdmin,"P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();


                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Omar",
                        LastName = "Ahmed",
                        UserName = "OmarAhmed",
                        Email = "omarahmed607@gmail.com",
                        PhoneNumber = "01101943882"
                    };
                    userManager.CreateAsync( Admin , "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;

            }catch(Exception ex)
            {
                Console.WriteLine($"Seeding Failed :{ex.Message}");
                return false;
            }
        }
    }
}
