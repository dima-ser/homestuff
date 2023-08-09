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
    public class LoginModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }
        [BindProperty, Required]
        public string Password { get; set; } = string.Empty;
        [BindProperty]
        public bool RememberMe { get; set; } = false;

        public string? ErrorMessage { get; set; }
        public string PasswordFilePath { get; set; }



        public LoginModel(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.PasswordFilePath = Utilities.GetPasswordFilePath(webHostEnvironment, configuration);

            
           
        }
        public ActionResult OnGet()
        {
            if (!System.IO.File.Exists(PasswordFilePath))
            {
                return Redirect("/auth/setpwd");
            }
            else
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!System.IO.File.Exists(PasswordFilePath))
            {
                return Redirect("/auth/setpwd");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string correctHash = string.Empty;
            try
            {
                using (StreamReader sr = new(PasswordFilePath))
                {
                    correctHash = sr.ReadLine()!;
                }
                string hash = Utilities.GetHash(Password);

                if (correctHash == hash)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Utilities.AdminUserName)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        IsPersistent = RememberMe
                    });
                    
                    return Redirect(!string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl : "/");
                }
                ErrorMessage = "Login failed";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

        }
    }
}
