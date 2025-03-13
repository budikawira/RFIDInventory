using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Users.Requests;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RfidBarcode.Crm.Pages.Setup
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string Username { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public IndexModel(UserManager<ApplicationUser> userManager, IApplicationDbContext context, IMediator mediator)
        {
            _userManager = userManager;
            _context = context;
            _mediator = mediator;
        }

        public IActionResult OnGet()
        {
            if (_context.Users.Any())
            {
                return Redirect("~/");
            }

            Username = "admin";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var role = "Administrator";
            //var hasher = new PasswordHasher<IdentityUser>();
            //var user = new ApplicationUser
            //{
            //    UserName = Username,
            //    NormalizedUserName = Username.ToUpper(),
            //    PasswordHash = hasher.HashPassword(null!, Password),
            //    SecurityStamp = Guid.NewGuid().ToString()
            //};
            //var createResult = await _userManager.CreateAsync(user);
            //if (createResult.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(user, role);

            //}

            var UserVM = new UserVM
            {
                UserName = Username,
                Password = Password,
                Role = "Administrator"
            };
            var cmd = new CreateUserRequest(UserVM);
            var res = await _mediator.Send(cmd);

            return Redirect("~/Setup");
        }
    }
}
