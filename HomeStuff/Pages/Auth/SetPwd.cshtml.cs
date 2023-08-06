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
    public class SetPwdModel : PageModel
    {
        [BindProperty, Required, DisplayName("New Password")]
        public string PasswordNew1 { get; set; } = string.Empty;
        [BindProperty, Required, DisplayName("Re-enter New Password")]
        public string PasswordNew2 { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
        public string passwordFilePath { get; set; }



        public SetPwdModel(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.passwordFilePath = Path.Combine(webHostEnvironment.ContentRootPath, configuration.GetValue<string>("PasswordFilePath"));

        }
        public void OnGet()
        {
            if (System.IO.File.Exists(passwordFilePath))
            {
                RedirectToPage("Login");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            return Page();
        }
    }
}
