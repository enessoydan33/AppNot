using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotUyg.Data.Abstract;
using NotUyg.Data.Concrete.EfCore;
using NotUyg.Entity;
using NotUyg.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NotUyg.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly INotRepository _notRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(INotRepository notRepository,UserManager<User> userManager)
        {
            _notRepository = notRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var onGunOnce = DateTime.Now.AddDays(-10);
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return View();
            }
            var userId = user.Id;
            var model = _notRepository.Nots.Where(x => x.UserId == userId && x.Tarih > onGunOnce).OrderByDescending(x=> x.Tarih).ToList();
            return View(model);
        }      
       
    }
}
