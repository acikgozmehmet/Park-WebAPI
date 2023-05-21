using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Park.Models.Dtos;
using Park.Test.Config;
using Park.Test.Helper;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;


namespace Park.Test.Tests
{
    public class NationalParkTest : BaseTest
    {
        public NationalParkTest(WebApplicationFactory<Startup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void GetNationalParkById_GivenId_ShouldReturnOK()
        {
            var token = Authentication.GetAuthToken1(_client, AuthUsername, AuthPassword);

            var Id = 1;

            _client.DefaultRequestHeaders.Add("Authorization", token);
            var response = _client.GetAsync(String.Format(ApiConfig.GetNationalParkById,Id));
            var jsonString = response.Result.Content.ReadAsStringAsync().Result;
            NationalParkDto obj = JsonSerializer.Deserialize<NationalParkDto>(jsonString);

            Assert.Equal(HttpStatusCode.OK, response.Result.StatusCode);

            obj.Id.Should().Be(1);
           
        }

        [Fact]
        public void Test_GivenId_ShouldReturnOK()
        {
                
        }


        }
    }
