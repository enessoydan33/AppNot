using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotUyg.Data.Abstract;
using NotUyg.Entity;
using NotUyg.Models;
using System.Diagnostics;
using System.Linq;


namespace NotUyg.Controllers
{
    public class NotController: Controller
    {
        private readonly INotRepository _notRepository;       
        private readonly ITagRepository _tagRepository;
        private readonly UserManager<User> _userManager;
        public NotController(INotRepository notRepository, ITagRepository tagRepository, UserManager<User> userManager)
        {
            _notRepository = notRepository;           
            _tagRepository = tagRepository;
            _userManager = userManager;            
        }
        public async Task<IActionResult> Index(int SelectedTags)
        { 
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var m = _tagRepository.Tag.ToList();
            var taglist = m.Select(g => new { Key = g.Id, Name = g.Name }).ToList();
            taglist.Insert(0, new { Key = 0, Name = "Hepsi" });
            ViewBag.Tags = new SelectList ( taglist, "Key", "Name", SelectedTags );

            if (SelectedTags>0)
            {
                var model = _notRepository.Nots.Include(m => m.Tags).Where(m => m.UserId == userId && m.Tags.Any(x => x.Id == SelectedTags)).ToList();
                return View(model);
            }

            var model2 = _notRepository.Nots.Include(m => m.Tags).Where(m => m.UserId == userId).ToList();  
            return View(model2);
        }
        
        
        public IActionResult Update(int id)
        {
            var model = _notRepository.Nots.Include(x=> x.Tags).FirstOrDefault(m => m.Id == id);

            if (model == null)
                return View("Index", "Home");

            var tags = _tagRepository.Tag.ToList();
           

            NotUpdateData data = new()
            {
                Id = model.Id,
                Baslik = model.Baslık,
                aciklama = model.acıklama,
                Durum = model.Durum,
                Tags = model.Tags.Select(x=>x.Id).ToList(),
            };

            ViewBag.Tags = new MultiSelectList(tags, "Id", "Name", data.Tags);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(NotUpdateData model)
        {
            if (ModelState.IsValid)
            {
                var m= _notRepository.Nots.FirstOrDefault(m => m.Id == model.Id);
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;
                List<Tag> tags = new();
                _notRepository.TagClear(m);

                foreach (var tagId in model.Tags)
                {
                    var tag = _tagRepository.Tag.FirstOrDefault(t => t.Id == tagId);
                    if (tag != null)
                    {
                        tags.Add(tag);
                    }
                }
                _notRepository.UpdateNot(
                new Not
                {
                    Tags = tags,
                    Id = model.Id,
                    Baslık = model.Baslik,
                    acıklama = model.aciklama,
                    Durum = model.Durum,
                    Tarih = m.Tarih,
                    UserId = userId
                });
                TempData["Update"] = "Not başarıyla güncellendi!";
                return RedirectToAction("Index");
            }
            return View(model);

        }


        public IActionResult Delete(int id)
        {
            var m = _notRepository.Nots.FirstOrDefault(a=>a.Id==id);
            if (m != null)
            {
                _notRepository.DeleteNot(m);
                TempData["Delete"] = "Not başarıyla silindi!";
            }
            if (m == null)
                return NotFound();


            return RedirectToAction("Index");
        }


        public async Task<IActionResult> TagListele(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var model = _notRepository.Nots.Where(m => m.Tags.Any(t => t.Id == id) && m.UserId ==userId).ToList();
            return View(model);

        }

    }
}
