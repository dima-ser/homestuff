using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeStuff.Pages
{
    public class AboutModel : PageModel
    {
        public IWebHostEnvironment Environment;
        public AboutModel(IWebHostEnvironment env) 
        {
            this.Environment = env;
        }
        public void OnGet()
        {
        }
    }
}
