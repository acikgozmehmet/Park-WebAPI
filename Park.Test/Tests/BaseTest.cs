using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using Xunit;

namespace Park.Test.Tests
{
    public class BaseTest : IClassFixture<WebApplicationFactory<Startup>>
    {

        protected readonly WebApplicationFactory<Startup> _fixture;
        protected readonly HttpClient _client;

        protected static string env { get; }
        protected static string BaseAddress { get; }

        protected static string AuthUsername { get; }
        protected static string AuthPassword { get; }


        static BaseTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("TSettings.json").Build();
            AuthUsername = config["API:Authentication:username"];
            AuthPassword = config["API:Authentication:password"];

            env = config.GetSection("Env").Value.Trim().ToLower();
            if (!env.Equals("local"))
                BaseAddress = config.GetSection("API").GetSection(env).GetSection("Host").Value.Trim().ToLower();
        }


        public BaseTest(WebApplicationFactory<Startup> fixture)
        {
            if (env == "local")
            {
                _fixture = fixture;
                _client = _fixture.CreateClient();
            }
            else
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri(BaseAddress);
            }
        }


    }



}

