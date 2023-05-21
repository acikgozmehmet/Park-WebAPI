using Newtonsoft.Json;
using Park.Data;
using Park.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Park.Test.Mock
{
    public static class MockUtilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.NationalParks.AddRange(GetSeedingMessages_NationalPark());
            db.SaveChanges();
        }


        /// <summary>
        ///  Seed NationalPark Dara
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IEnumerable<NationalPark> GetSeedingMessages_NationalPark()
        {
            var assembly = Assembly.GetExecutingAssembly();

            //var resourceName = "Park.Test.Mock.MockData.NationalParks.json";
            //var resourceName = @"D:\VS_Projects\Park\Park.Test\Mock\MockData\NationalParks.json";
            // https://stackoverflow.com/questions/21637830/getmanifestresourcestream-returns-null
            var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames()[0];


            using Stream stream = assembly.GetManifestResourceStream(resourceName);


            
            using StreamReader reader = new StreamReader(stream);
            var testData = JsonConvert.DeserializeObject<List<NationalPark>>(reader.ReadToEnd());

            return testData;



        }
    }
}
