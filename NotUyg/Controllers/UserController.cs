using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotUyg.Data.Abstract;
using NotUyg.Data.Concrete.EfCore;
using NotUyg.Entity;
using NotUyg.Models;
using NuGet.Protocol;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NotUyg.Controllers
{
    public class UserController:Controller
    {
        private readonly INotRepository _notRepository;
        private readonly ITagRepository _tagRepository;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinmanager;

        public UserController( INotRepository notRepository,ITagRepository tagRepository,UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _notRepository = notRepository;
            _tagRepository=tagRepository;
            _userManager = userManager;
            _signinmanager = signInManager;
        }
        public IActionResult Index() { 
            
            return View(); 
        
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginData model)
        {

            if (ModelState.IsValid)
            {
                 var user=await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signinmanager.SignOutAsync();
                    var result = await _signinmanager.PasswordSignInAsync(user, model.Password, true, false);

                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Şifre Hatalı");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu maile sahip kullanıcı yok");
                    return View("Index", "Home");
                }

            }
            return View(model);

        }



        public IActionResult Kayit()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Kayit(KayitData model)
        {
            var surname = model.Isim+"_"+ model.Soyad;
            if (ModelState.IsValid)
            {
                var a = await _userManager.FindByEmailAsync(model.Email);
                if (a == null)
                {
                    var user = new User
                    {
                        Email = model.Email,
                        UserName = surname,
                    };


                  IdentityResult result= await _userManager.CreateAsync(user,model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty,error.Description);
                        return View(model);
                    }

                    await _signinmanager.SignOutAsync();
                    await _signinmanager.PasswordSignInAsync(user, model.Password, true, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Bu mail daha önceden kullanılıyor.");
                   return View(model);
            
                }
               
            }
                   return View(model);


        }


        public async Task<IActionResult> Logout()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Not() 
        { var tags = _tagRepository.Tag.ToList();
            ViewBag.Tags = new SelectList(tags, "Id", "Name");
            return View(); 
        
        }

        [HttpPost]
        public async Task<IActionResult> Not( NotData model)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
       
            List<Tag> tags = new ();
            foreach (var tagId in model.Tags)
            {
                var tag = _tagRepository.Tag.FirstOrDefault(t => t.Id == tagId);
                if (tag != null)
                {
                    tags.Add(tag); // Tag ID'si ile ilişkilendirilen Tag nesnesi ekleniyor
                }
            }

            if (ModelState.IsValid)
            {
                _notRepository.Create(new Not
                {
                  Tarih = DateTime.Now,
                    Durum = false,
                    Baslık = model.Baslik,
                    acıklama = model.aciklama,
                    UserId = userId,
                    Tags = tags
                });
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }




        public async Task<IActionResult> UpdatePassword()
        {
            var user = await _userManager.GetUserAsync(User);
           
            if (user == null)
                return NotFound();


            var profile = new UpdatePasswordModel
            {
                UserName = user.UserName,
                ConfirmPassword = string.Empty,
                CurrentPassword = string.Empty,
                NewPassword = string.Empty,

            };

            return View(profile);
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
            
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Yeni şifreyi yanlış girdiniz.");
                    return View(model);
                }


                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return View(model); // Şifre hatası varsa göster
                }

                await _userManager.UpdateAsync(user);
                TempData["Success"] = "Profil başarıyla güncellendi!";
                return RedirectToAction("UpdatePassword");


            }
            return View(model);
        }

        
    }
}

