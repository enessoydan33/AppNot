using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotUyg.Data.Abstract;
using NotUyg.Entity;
using NotUyg.Models;

namespace NotUyg.Controllers
{
    [Authorize]
    public class NotController : Controller
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
            if (user == null)
                return Challenge();

            var userId = user.Id;
            var m = _tagRepository.Tag.ToList();
            var taglist = m.Select(g => new { Key = g.Id, Name = g.Name }).ToList();
            taglist.Insert(0, new { Key = 0, Name = "Hepsi" });
            ViewBag.Tags = new SelectList(taglist, "Key", "Name", SelectedTags);

            if (SelectedTags > 0)
            {
                var model = _notRepository.Nots.Include(n => n.Tags)
                    .Where(n => n.UserId == userId && n.Tags.Any(x => x.Id == SelectedTags))
                    .ToList();
                return View(model);
            }

            var model2 = _notRepository.Nots.Include(n => n.Tags)
                .Where(n => n.UserId == userId)
                .ToList();
            return View(model2);
        }

        public async Task<IActionResult> Update(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var model = _notRepository.Nots.Include(x => x.Tags)
                .FirstOrDefault(n => n.Id == id && n.UserId == user.Id);

            if (model == null)
                return NotFound();

            var tags = _tagRepository.Tag.ToList();

            NotUpdateData data = new()
            {
                Id = model.Id,
                Baslik = model.Baslık,
                aciklama = model.acıklama,
                Durum = model.Durum,
                Tags = model.Tags.Select(x => x.Id).ToList(),
            };

            ViewBag.Tags = new MultiSelectList(tags, "Id", "Name", data.Tags);

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(NotUpdateData model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var existing = _notRepository.Nots.FirstOrDefault(n => n.Id == model.Id && n.UserId == user.Id);
            if (existing == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                List<Tag> tags = new();
                _notRepository.TagClear(existing);

                if (model.Tags != null)
                {
                    foreach (var tagId in model.Tags)
                    {
                        var tag = _tagRepository.Tag.FirstOrDefault(t => t.Id == tagId);
                        if (tag != null)
                        {
                            tags.Add(tag);
                        }
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
                        Tarih = existing.Tarih,
                        UserId = user.Id
                    });
                TempData["Update"] = "Not başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            ViewBag.Tags = new MultiSelectList(_tagRepository.Tag.ToList(), "Id", "Name", model.Tags);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var note = _notRepository.Nots.FirstOrDefault(a => a.Id == id && a.UserId == user.Id);
            if (note == null)
                return NotFound();

            _notRepository.DeleteNot(note);
            TempData["Delete"] = "Not başarıyla silindi!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TagListele(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var userId = user.Id;
            var model = _notRepository.Nots
                .Where(n => n.Tags.Any(t => t.Id == id) && n.UserId == userId)
                .ToList();
            return View(model);
        }
    }
}
