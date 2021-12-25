using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using serssly.Data;
using serssly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace serssly.API
{
    public class FeedUpdate
    {
        public string FeedName { get; set; } = "";
    }

    [Route("api/refresh-feeds")]
    [AllowAnonymous]
    [ApiController]
    public class RefreshFeedsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RefreshFeedsController(ApplicationDbContext context) => this.context = context;

        public async Task<ActionResult<IEnumerable<FeedUpdate>>> GetAsync(CancellationToken ct)
        {
            var feeds = context.Feeds.ToArray();
            var tasks = new List<Task<FeedUpdate>>(feeds.Length);
            foreach (var f in feeds)
            {
                var lcts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                tasks.Add(UpdateFeed(f, lcts.Token));
            }

            // FIXME check the lastrefreshtime
            var result = new List<FeedUpdate>(tasks.Count);
            while (tasks.Count > 0)
            {
                var t = await Task.WhenAny(tasks);
                tasks.Remove(t);
                result.Add(t.Result);
            }

            await context.SaveChangesAsync();

            return result;
        }

        private async Task<FeedUpdate> UpdateFeed(Feed feed, CancellationToken ct)
        {
            var lastFeedItem = context.Entry(feed).Collection(f => f.FeedItems).Query()
                                      .OrderByDescending(fi => fi.PublishDateUtc).FirstOrDefault();
            var lastFeedItemPublishDate = DateTime.SpecifyKind(lastFeedItem?.PublishDateUtc ?? DateTime.MinValue, DateTimeKind.Utc);

            await foreach (var item in RssAtomFunctions.GetFeedItems(feed, lastFeedItemPublishDate.AddSeconds(1.0)))
            {
                context.FeedItems.Add(item);
            }

            feed.LastRefreshTimeUtc = DateTime.UtcNow;

            return new FeedUpdate { FeedName = feed.Link };
        }
    }
}
