using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotUyg.Data.Abstract;
using NotUyg.Entity;
using NotUyg.Models;

namespace NotUyg.ViewComponents
{
    public class AnketViewComponent : ViewComponent
    {
        private readonly IAnketRepository _anketRepository;
        private readonly IUserVoteRepository _userVote;
        private readonly UserManager<User> _user;

        public AnketViewComponent(IAnketRepository anketRepository, UserManager<User> user, IUserVoteRepository userVote)
        {
            _anketRepository = anketRepository;
            _user = user;
            _userVote = userVote;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _user.GetUserAsync(HttpContext.User);
            if (user == null)
                return View("Default", new List<AnketListViewModel>());

            var userId = user.Id;

            var ucGunOnce = DateTime.Now.AddDays(3);
            var polls = _anketRepository.polls.Where(x => x.Time < ucGunOnce).OrderBy(x => x.Time).Include(p => p.Options).ToList();

            var VoteCounts = _userVote.userVotes.GroupBy(x => x.OptionId)
                .Select(x => new { OptionId = x.Key, Count = x.Count() })
                .ToDictionary(x => x.OptionId, x => x.Count);

            var votedPolls = _userVote.userVotes.Where(x => x.UserId == userId)
                .Select(x => x.PollId).Distinct().ToHashSet();

            var model = polls.Select(p => new AnketListViewModel
            {
                Id = p.Id,
                Descirption = p.Description,
                Time = p.Time,
                Title = p.Title,
                HVoted = votedPolls.Contains(p.Id),
                Options = p.Options.Select(o => new OptionViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    VoteCount = VoteCounts.ContainsKey(o.Id) ? VoteCounts[o.Id] : 0
                }).ToList()
            }).ToList();

            return View("Default", model);
        }
    }
}
