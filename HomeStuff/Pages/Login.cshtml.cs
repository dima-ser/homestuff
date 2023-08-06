using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Configuration;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace HomeStuff.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuration;
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }
        [BindProperty, Required]
        public string Password { get; set; }= string.Empty;
        public bool RememberMe { get; set; } = false;
        public string? Message { get; set; }

        public LoginModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var correctPassword = configuration.GetValue<string>("Password");

                //var passwordHasher = new PasswordHasher<string>();
                //if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                if (correctPassword == Password) 
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "HomeStuff Admin")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return Redirect(!string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl : "/");
               }
            Message = "Login failed";
            return Page();
        }
    }
}
