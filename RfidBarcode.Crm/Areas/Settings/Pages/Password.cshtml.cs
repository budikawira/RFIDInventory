using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Domain.Entities.Identities;
using RfidBarcode.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RfidBarcode.Crm.Areas.Settings.Pages
{
    public class PasswordModel : PageModel
    {
        [BindProperty]
        public string CurrentPassword { get; set; } = null!;

        [BindProperty]
        public string NewPassword { get; set; } = null!;

        [BindProperty]
        public string ConfirmPassword { get; set; } = null!;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserResolverService _user;
        private readonly ApplicationDbContext _context;

        public PasswordModel(UserManager<ApplicationUser> userManager, IUserResolverService user, ApplicationDbContext context)
        {
            _userManager = userManager;
            _user = user;
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ConfirmPassword != NewPassword)
            {
                TempData["Error"] = "Password baru tidak sama dengan konfirmasi password baru!";
                return Page();
            }

            var user = await _userManager.FindByNameAsync(_user.GetUser());
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Password berhasil diubah!";
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["Error"] = errors;
                }
                return Page();
            }
            TempData["Error"] = "User tidak ditemukan!";

            return Page();
        }
    }
}
