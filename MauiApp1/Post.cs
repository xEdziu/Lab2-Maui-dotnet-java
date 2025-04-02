using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiApp1
{
    class Post
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("body")]
        public string? Body { get; set; }

        public override string ToString()
        {
            return $"Post #{Id} by User {UserId}:\nTitle: {Title}\nBody: {Body}\n";
        }
    }
}
