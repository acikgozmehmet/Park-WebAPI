using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Park.Models;

namespace Park.Test.Helper
{
    public  class Authentication
    {
        //private static string username { get; }
        //private static string password { get; }

        //static Authentication()
        //{
        //    var config = new ConfigurationBuilder().AddJsonFile("CTFSettings.json").Build();
        //    username = config["API:Apigee:Authentication:username"];
        //    password = config["API:Apigee:Authentication:password"];
        //}

        //public void GetAuthToken(IApiDriver driver, IStateBag stateBag)
        //{
        //    GetAuthTokenWithUserid(driver, stateBag, null);
        //}


        public static string GetAuthToken1(HttpClient client, string username, string password)
        {
            var user  = new AuthenticationModel() { Username=username, Password=password };

            using (var content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json"))
            {
                    var combined = $"{username}:{password}";
                    var bytes = Encoding.UTF8.GetBytes(combined);
                    var auth = $"Basic {System.Convert.ToBase64String(bytes)}";

                    var url = client.BaseAddress + "api/v1/Users/authenticate";

                    HttpResponseMessage result = client.PostAsync(url, content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        return $"Bearer {result.Content.ReadAsStringAsync().Result}";

                    return null;

             }
        }


        public static string GetAuthToken2(HttpClient client, string username, string password)
        {
            var person = new AuthenticationModel();
            person.Username = "test";
            person.Password= "test";

            var json = JsonConvert.SerializeObject(person);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = client.BaseAddress + "api/v1/Users/authenticate";
            //var url = "https://httpbin.org/post";
            //using var client = new HttpClient();

            var response =  client.PostAsync(url, data);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = response.Result.Content.ReadAsStringAsync().Result;
                return $"Bearer {jsonString}";
            }
                


            return null;


        }
    }



}
