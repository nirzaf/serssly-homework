using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serssly.Models
{
    public class UserFeed
    {
        private Feed? feed;

        public string UserId { get; set; } = "";

        public int FeedId { get; set; }

        public Feed Feed
        {
            set => feed = value;
            get => feed ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Feed));
        }
    }
}
