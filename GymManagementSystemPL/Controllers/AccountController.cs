using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.AccountViewModels;
using GymManagementSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            this.accountService = accountService;
            this.signInManager = signInManager;
        }

        #region Login

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var user = accountService.ValidateUser(model);
            if(user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }
           var result= signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false).Result;
            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Is Not Allowed");
            if(result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Is Locked Out");
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(model);
        }

        #endregion


        #region Logout

        [HttpPost]
        public ActionResult Logout()
        {
            signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region AccessDenied

        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
