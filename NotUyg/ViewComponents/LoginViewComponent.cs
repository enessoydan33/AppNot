using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NotUyg.Data.Abstract;
using NotUyg.Models;

namespace NotUyg.ViewComponents
{
    public class LoginViewComponent:ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View("Default",new LoginData());
        }







    }
}
