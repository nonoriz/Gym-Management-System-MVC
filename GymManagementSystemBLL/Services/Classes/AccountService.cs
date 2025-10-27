using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.AccountViewModels;
using GymManagementSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var User = userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if(User is null) return null;
            var IsPasswordValid = userManager.CheckPasswordAsync(User, loginViewModel.Password).Result;
            return IsPasswordValid ? User : null;
        }
    }
}
