using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClientToMyServer.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ClientToMyServer.Services.ServerConfigs
{
    public class HttpCodeWriter : IHttpCodeWriter
    {
        private readonly ServerConfig _conf;
        private readonly ILogger _logger;
        private static HttpClient _client = new HttpClient();
        private string Token;
        public HttpCodeWriter(IOptions<ServerConfig> _conf, ILogger<HttpCodeWriter> _logger)
        {
            this._conf = _conf.Value;
            this._logger = _logger;
            _client.BaseAddress = new Uri(_conf.Value.Url);
        }
        public async Task<string> GetCodeById(int id)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
            HttpResponseMessage response = await _client.GetAsync($"api/code/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError(System.Net.HttpStatusCode.OK.ToString());
                return String.Empty;
            }
        }
        public async Task Authentificate()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
            _client.DefaultRequestHeaders.Add("username", _conf.username);
            _client.DefaultRequestHeaders.Add("password", _conf.password);
            HttpResponseMessage response = await _client.PostAsync("token/", null);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic data = JObject.Parse(await response.Content.ReadAsStringAsync());
                Token = data.access_token;
            }
            else
            {
                _logger.LogError(System.Net.HttpStatusCode.OK.ToString());
                Token= String.Empty;
            }
        }
    }
}
