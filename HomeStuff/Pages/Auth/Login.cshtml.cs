using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Configuration;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
        public string passwordFilePath { get; set; }



        public LoginModel(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.passwordFilePath = Path.Combine(webHostEnvironment.ContentRootPath, configuration.GetValue<string>("PasswordFilePath"));
            Console.WriteLine(passwordFilePath);
           
        }
        public ActionResult OnGet()
        {
            if (!System.IO.File.Exists(passwordFilePath))
            {
                Console.WriteLine("file doesn't exist, redirecting to SetPwd");
                return Redirect("/auth/setpwd");
            }
            else
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string correctPassword = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(passwordFilePath))
                {
                    correctPassword = sr.ReadLine()!;
                }
                //var passwordHasher = new PasswordHasher<string>();
                //if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                if (!string.IsNullOrEmpty(correctPassword) && correctPassword == Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "HomeStuff Admin")
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
