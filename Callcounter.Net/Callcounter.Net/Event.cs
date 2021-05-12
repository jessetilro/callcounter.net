using System;
using System.Text.Json.Serialization;

namespace Callcounter.Net
{
    public class Event
    {
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("path")] public string Path { get; set; }
        [JsonPropertyName("method")] public string Method { get; set; }
        [JsonPropertyName("user_agent")] public string UserAgent { get; set; }
        [JsonPropertyName("status")] public int Status { get; set; }
        [JsonPropertyName("elapsed_time")] public int ElapsedTime { get; set; }
    }
}