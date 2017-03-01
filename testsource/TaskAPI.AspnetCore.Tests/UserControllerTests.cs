using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

namespace TaskAPI.AspnetCore.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        WebHostBuilder obj;

        static public IConfigurationRoot Configuration { get; set; }

        private void SetApplicationStartup()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }


        

        HttpClient client;

        [TestInitialize]
        public void SetupHttpClient()
        {
            SetApplicationStartup();
            string BaseURL = string.Empty;
//#if DEBUG
//BaseURL = Configuration.AsEnumerable().ToList().First(obj => obj.Key == "TaskAPIURL").Value;

//#else
            BaseURL = System.Environment.GetEnvironmentVariable("DOMAIN");            
//#endif

            client = new HttpClient() { BaseAddress = new Uri(BaseURL) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        [TestMethod]
        public void GetAllUsers_OK_Response()
        {
            var response = client.GetAsync("api/user").Result;
            var result = response.Content.ReadAsJsonAsync<List<User>>().Result;

            //var result = MongoDB.Bson.Serialization.BsonSerializer.Deserialize <List<User>>(response.Content.ReadAsStreamAsync().Result);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void PostUsers_OK_Response()
        {
            var rand = new Random();

            //var request = @" {'EmailAddress': 'Sample@email.com'}";
            var req = new User()
            {
                EmailAddress = "Sample@email.com" + rand.Next()
            };

            var response = client.PostAsJsonAsync<User>("api/user",req).Result;

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void DeleteUser_OK_Response()
        {
            var rand = new Random();
            var emailAddress = "Sample@email.com"+rand.Next();

            var req = new User()
            {
                EmailAddress = emailAddress
            };

            var createResponse = client.PostAsJsonAsync<User>("api/user", req).Result;

            Assert.AreEqual(createResponse.StatusCode, System.Net.HttpStatusCode.OK);

            var response = client.GetAsync("api/user/" + emailAddress).Result;
            var userObject = response.Content.ReadAsJsonAsync<User>().Result;
            var userId = userObject.userId;

            

           // var deleteResult = client.DeleteAsync("api/user/" + userId).Result;


            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent("{ \"UserId\": \"" +userId+ "\"}", Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(client.BaseAddress+"api/user")
            };
            var deleteResult =  client.SendAsync(request).Result;

            Assert.AreEqual(deleteResult.StatusCode, System.Net.HttpStatusCode.NoContent);
        }
    }
}
