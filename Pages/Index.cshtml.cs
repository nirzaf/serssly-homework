using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using serssly.Data;
using serssly.Models;

namespace serssly.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public Feed[] UserFeeds { get; set; } = null!;

        public void OnGet()
        {
            static int FeedComparison(Feed f1, Feed f2)
            {
                var f1pubdate = f1.FeedItems?.FirstOrDefault()?.PublishDateUtc ?? DateTime.MinValue;
                var f2pubdate = f2.FeedItems?.FirstOrDefault()?.PublishDateUtc ?? DateTime.MinValue;

                return -f1pubdate.CompareTo(f2pubdate); // the order must be inverted (latest first)
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var feeds =
                context.UserFeeds.Include(uf => uf.Feed).ThenInclude(
                    f => f.FeedItems!.OrderByDescending(it => it.PublishDateUtc).Take(8))
                    .Where(uf => uf.UserId == userId)
                    .Select(uf => uf.Feed).ToArray();

            Array.Sort(feeds, FeedComparison);
            UserFeeds = feeds;
        }
    }
}
