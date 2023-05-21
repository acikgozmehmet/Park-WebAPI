using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Park.Models.Dtos;
using Park.Test.Config;
using Park.Test.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Park.Test.Mock
{
    public class NationalPark_MockTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public NationalPark_MockTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            //_client = _factory.CreateClient();
        }


        [Fact]
        public void GetNationalParkById_GivenId_ShouldReturnOK()
        {
            var token = Authentication.GetAuthToken1(_client, "test", "test");
            token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy9jbGFpbXMvZG5zIjoiNCIsIm5iZiI6MTY1OTg0MDUwNSwiZXhwIjoxNjU5ODU0OTA1LCJpYXQiOjE2NTk4NDA1MDV9.Mi3-u7viU1leEzTijThe66P6rJhfWQm4xHU1ITHvTg8";
            var Id = 1;

            _client.DefaultRequestHeaders.Add("Authorization", token);
            var response = _client.GetAsync(String.Format(ApiConfig.GetNationalParkById, Id));
            var jsonString = response.Result.Content.ReadAsStringAsync().Result;
            NationalParkDto obj = JsonSerializer.Deserialize<NationalParkDto>(jsonString);

            Assert.Equal(HttpStatusCode.OK, response.Result.StatusCode);

            obj.Id.Should().Be(1);


           

        }

    }
}
