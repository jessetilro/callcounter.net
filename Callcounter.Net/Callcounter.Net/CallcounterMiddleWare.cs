using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Callcounter.Net
{
    public class CallcounterMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;

        private DateTime _lastFlush = DateTime.UtcNow;
        private readonly IList<Event> _eventBuffer;
        private readonly string _projectId;

        private readonly TimeSpan _maxElapsedTime = new TimeSpan(0, 5, new Random().Next(0, 59));
        private const int MaxBufferSize = 25;

        private const string
            CallcounterUrl =
                "https://api.Callcounter.eu/api/v1/events/batch.json"; 

        private const string UserAgent = "Callcounter.net";
        private const string UserAgentVersion = "1.0.0";

        public CallcounterMiddleWare(RequestDelegate next, IConfiguration configuration, HttpClient httpClient)
        {
            _next = next;
            _httpClient = httpClient;
            _projectId = configuration["CallcounterProjectToken"];
            _eventBuffer = new List<Event>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var CallcounterEvent = new Event()
            {
                CreatedAt = DateTime.UtcNow,
                Path = httpContext.Request.Path,
                Method = httpContext.Request.Method,
                UserAgent = httpContext.Request.Headers["User-Agent"]
            };

            var timer = Stopwatch.StartNew();

            await _next(httpContext);

            timer.Stop();

            CallcounterEvent.Status = httpContext.Response.StatusCode;
            CallcounterEvent.ElapsedTime = Convert.ToInt32(timer.ElapsedMilliseconds);

            _eventBuffer.Add(CallcounterEvent);

            if (IsFlushTime())
            {
                Flush();
            }
        }

        private bool IsFlushTime()
        {
            return _eventBuffer.Count > 0
                   && (_eventBuffer.Count > MaxBufferSize
                       || _lastFlush.Add(_maxElapsedTime) < DateTime.UtcNow);
        }

        private void Flush()
        {
            var request = JsonContent.Create(new
            {
                batch = new Batch()
                {
                    Events = _eventBuffer.ToImmutableList(),
                    ProjectToken = _projectId
                }
            });

            var userAgent = new ProductInfoHeaderValue(UserAgent, UserAgentVersion);

            _httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
            _httpClient.PostAsync(CallcounterUrl, request);
            _httpClient.DefaultRequestHeaders.UserAgent.Remove(userAgent);

            _eventBuffer.Clear();
            _lastFlush = DateTime.UtcNow;
        }
    }
}