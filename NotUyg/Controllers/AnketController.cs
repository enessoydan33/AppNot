using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotUyg.Entity;
using NotUyg.Models;
using NotUyg.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace NotUyg.Controllers
{
    public class AnketController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly IAnketRepository _anketRepository;
        private readonly IUserVoteRepository _userVote;
        public AnketController(UserManager<User> usermanager, IAnketRepository anketRepository, IUserVoteRepository userVote)
        {
            _usermanager = usermanager;
            _anketRepository = anketRepository;
            _userVote = userVote;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(AnketKayit model)
        {
            var user = await _usermanager.GetUserAsync(User);
            var userId = user.Id;
            if (ModelState.IsValid)
            {
                Poll poll = new ()
                {
                    Description = model.Description,
                    Time = DateTime.UtcNow,
                    Title = model.Title,
                    UserId = userId,
                    Options = new List<Option>()
                };

                foreach (var option in model.OptName)
                {
                    poll.Options.Add(new Option
                    {
                        Name = option
                    });
                }

                _anketRepository.Create(poll);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var user = await _usermanager.GetUserAsync(User);
            var userId = user.Id;
            var ucGunOnce = DateTime.Now.AddDays(3);
            var polls = _anketRepository.polls.Where(x=> x.Time< ucGunOnce).OrderBy(x => x.Time).Include(p=> p.Options).ToList();

            var voteCounts = _userVote.userVotes
        .GroupBy(v => v.OptionId)
        .Select(g => new { OptionId = g.Key, Count = g.Count() })
        .ToDictionary(x => x.OptionId, x => x.Count);

            // Bu kullanıcının oy verdiği anket ID'leri
            var votedPollIds = _userVote.userVotes
                .Where(v => v.UserId == userId)
                .Select(v => v.PollId)
                .Distinct()
                .ToHashSet();

            var model = polls.Select(p => new AnketListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Descirption = p.Description,
                Time = p.Time,
                HVoted = votedPollIds.Contains(p.Id),
                Options = p.Options.Select(o => new OptionViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    VoteCount = voteCounts.ContainsKey(o.Id) ? voteCounts[o.Id] : 0
                }).ToList()

            }).ToList();

           return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(VoteInputModel model)
        {
            var user = await _usermanager.GetUserAsync(User);
            var userId = user.Id;
            var ucGunOnce = DateTime.Now.AddDays(3);
       
            var polls = _anketRepository.polls.Where(x => x.Time < ucGunOnce).OrderBy(x => x.Time).Include(p => p.Options).ToList();
      
            var voteCounts = _userVote.userVotes
        .GroupBy(v => v.OptionId)
        .Select(g => new { OptionId = g.Key, Count = g.Count() })
        .ToDictionary(x => x.OptionId, x => x.Count);

            // Bu kullanıcının oy verdiği anket ID'leri
            var votedPollIds = _userVote.userVotes
                .Where(v => v.UserId == userId)
                .Select(v => v.PollId)
                .Distinct()
                .ToHashSet();


            var ListModel = polls.Select(p => new AnketListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Descirption = p.Description,
                Time = p.Time,
                HVoted = votedPollIds.Contains(p.Id),
                Options = p.Options.Select(o => new OptionViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    VoteCount = voteCounts.ContainsKey(o.Id) ? voteCounts[o.Id] : 0
                }).ToList()
            }).ToList();



            if (ModelState.IsValid)
            {
          
                var OyVerme = await _userVote.userVotes.Where(x => x.UserId == userId && x.PollId == model.PollId).FirstOrDefaultAsync();
                if(OyVerme!= null)
                {
                    ModelState.AddModelError("", "Bu ankete oy verdiniz");
                    return View("List",ListModel);
                }              
                UserVote m = new ()
                {
                    PollId = model.PollId,
                    OptionId = model.OptionId,
                    UserId = userId,
                };                
                _userVote.Create(m);
                return RedirectToAction("Index","Home");
            }
            return View("List",ListModel);
        }
    }
}
