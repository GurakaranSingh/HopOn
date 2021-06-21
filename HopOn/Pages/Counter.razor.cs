using HopOn.Data;
using HopOn.Model;
using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Pages
{
    public partial class Counter
    {

        public Counter()
        {

        }
        private int currentCount = 0;

        public async Task  IncrementCount()
        {
            HttpClient Http = new HttpClient();
            string baseUrl = "https://localhost:44306/";
            var temp2 = await Http.GetStringAsync($"{baseUrl}api/values/GetUploadIDAsync?fileName=test");

            var emp = new FileListModel()
            {
                ID = 2,
                Name = "test Post",
                PhotoPath = "Images/File3.png"
            };


            //var serialized = JsonConvert.SerializeObject(emp);  
            //var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            //var response = await Http.PostAsync($"{baseUrl}api/values/PostStudent", stringContent);
          
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/values");
            // set request body
            var postBody = new { Title = "Blazor POST Request Example" };
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(emp), Encoding.UTF8, "application/json");
            // add authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "my-token");
            // add custom http header
            request.Headers.Add("My-Custom-Header", "foobar");

            // send request
           // using var response = await HttpClient.SendAsync(request);
        }
//        private async Task AddItem()
//        {
//#pragma warning disable IDE0090 // Use 'new(...)'
//            FileListModel addItem = new FileListModel { Name = "Test", ID = 1 };
//            await HttpClient.PostAsync("api/TodoItems", addItem);
//        }
    }
}
