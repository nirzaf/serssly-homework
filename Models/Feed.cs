using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace serssly.Models
{
    public enum FeedType
    {
        RSS,
        Atom
    }

    [Index(nameof(Link), IsUnique = true)]
    public class Feed
    {
        public int Id { get; set; }

        [Required]
        public FeedType Type { get; set; }

        [StringLength(200)]
        public string Title { get; set; } = "<invalid>";

        [Required]
        [StringLength(500)]
        public string Link { get; set; } = "<invalid>";

        public DateTime LastRefreshTimeUtc { get; set; }

        public List<UserFeed>? UserFeeds { get; set; }

        public List<FeedItem>? FeedItems { get; set; }
    }
}
