using System;

namespace serssly.Models
{
    public class FeedItem
    {
        private Feed? feed;

        public string Id { get; set; } = "";

        public string Link { get; set; } = "";

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime PublishDateUtc { get; set; }

        public int FeedId { get; set; }

        public Feed Feed
        {
            set => feed = value;
            get => feed ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Feed));
        }
    }
}
