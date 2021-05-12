using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Callcounter.Net
{
    public class Batch
    {
        [JsonPropertyName("project_token")] public string ProjectToken { get; set; }
        [JsonPropertyName("events")] public IList<Event> Events { get; set; }
    }
}