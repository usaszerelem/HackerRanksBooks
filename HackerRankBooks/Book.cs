using System;
using Newtonsoft.Json;

namespace HackerRankBooks
{
    public class Book
    {
        public Book()
        {
            Title = null;
            Url = null;
            Author = null;
            Comments = 0;
            StoryID = 0;
            StoryTitle = null;
            StoryUrl = null;
            ParentID = 0;
            CreatedAt = 0;
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("num_commentrs")]
        public int? Comments { get; set; }

        [JsonProperty("story_id")]
        public int? StoryID { get; set; }

        [JsonProperty("story_title")]
        public string StoryTitle { get; set; }

        [JsonProperty("story_url")]
        public string StoryUrl { get; set; }

        [JsonProperty("parent_id")]
        public int? ParentID { get; set; }

        [JsonProperty("created_at")]
        public ulong? CreatedAt { get; set; }
    }
}
