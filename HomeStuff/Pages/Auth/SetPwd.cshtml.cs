using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Configuration;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HomeStuff.Models;

namespace HomeStuff.Pages.Auth
{
    public class SetPwdModel : PageModel
    {
        [BindProperty, Required, DisplayName("New Password")]
        public string PasswordNew1 { get; set; } = string.Empty;
        [BindProperty, Required(ErrorMessage= "Please re-enter new password"), DisplayName("Re-enter New Password")]
        [Compare(nameof(PasswordNew1), ErrorMessage="Passwords don't match")]
        public string PasswordNew2 { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
        public string passwordFilePath { get; set; }



        public SetPwdModel(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.passwordFilePath = Utilities.GetPasswordFilePath(webHostEnvironment, configuration);

        }
        public ActionResult OnGet()
        {
            if (PasswordFileExists())
            {
                Console.WriteLine("redirecting to Login");
                return Redirect("/auth/login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (PasswordFileExists())
            {
                return Redirect("/auth/login");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string hashedPassword = Utilities.GetHash(PasswordNew1);
            using (StreamWriter outputFile = new StreamWriter(passwordFilePath))
            {
                outputFile.WriteLine(hashedPassword);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Utilities.AdminUserName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect("/");
        }

        bool PasswordFileExists()
        {
            return System.IO.File.Exists(passwordFilePath);
        }
    }
}
