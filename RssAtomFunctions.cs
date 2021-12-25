using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using serssly.Data;
using serssly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace serssly
{
    public sealed class FeedUpdate
    {
        public string FeedName { get; set; } = "";
    }

    public static class RssAtomFunctions
    {
        public static async IAsyncEnumerable<FeedItem> GetFeedItems(Feed feed, DateTime minPubDateUtc, int count = int.MaxValue)
        {
            var http = new HttpClient();
            using var resp = await http.GetAsync(feed.Link);
            using var rstream = await resp.Content.ReadAsStreamAsync();
            using var xmlReader = XmlReader.Create(rstream, new XmlReaderSettings { Async = true });
            XmlFeedReader reader = feed.Type == FeedType.RSS ? new RssFeedReader(xmlReader) : new AtomFeedReader(xmlReader);

            int n = 0;
            while (n < count && await reader.Read())
            {
                switch (reader.ElementType)
                {
                    case SyndicationElementType.Item:
                        var item = await reader.ReadItem();
                        if (item.Published >= minPubDateUtc)
                        {
                            yield return new FeedItem {
                                Id = item.Id,
                                Title = item.Title,
                                Description = item.Description,
                                FeedId = feed.Id,
                                Feed = feed,
                                PublishDateUtc = item.Published.UtcDateTime,
                                Link = item.Links.First().Uri.AbsoluteUri // should be the post link
                            };
                            n += 1;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
