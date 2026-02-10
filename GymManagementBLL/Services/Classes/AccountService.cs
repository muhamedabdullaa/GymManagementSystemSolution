using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementBLL.Services.Classes
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
			if (User == null) return null;
			var IsPasswordValid = userManager.CheckPasswordAsync(User,loginViewModel.Password).Result;


			return IsPasswordValid ? User : null;
		}
	}
}
