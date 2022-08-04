using FluentAssertions;
using Park.Models.Dtos;
using Park.Test.Helper;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;


namespace Park.Test.Tests
{
    public class NationalParkTest:IClassFixture<AppTestFixture>
    {
        private readonly AppTestFixture _fixture;
        private readonly HttpClient _client;

        public NationalParkTest(AppTestFixture fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public void GetNationalParkById_GivenId_ShouldReturnOK()
        {
            var token = Authentication.GetAuthToken1(_client, "test", "test");

            _client.DefaultRequestHeaders.Add("Authorization", token);
            var response = _client.GetAsync("api/v1/nationalparks/1");
            var jsonString = response.Result.Content.ReadAsStringAsync().Result;
            NationalParkDto obj = JsonSerializer.Deserialize<NationalParkDto>(jsonString);

            Assert.Equal(HttpStatusCode.OK, response.Result.StatusCode);

            obj.Id.Should().Be(1);
        }



    }
}
