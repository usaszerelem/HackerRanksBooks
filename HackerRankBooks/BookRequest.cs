﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HackerRankBooks
{
    public class BookRequest
    {
        public BookRequest()
        {
        }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public List<Book> Books { get; set; }
    }
}
