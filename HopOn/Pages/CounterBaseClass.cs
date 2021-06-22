using HopOn.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace HopOn.Pages
{
    public class CounterBaseClass : ComponentBase
    {
        [Inject]
        private IFileUploadServices FileUploadServies { get; set; }
        public int currentCount = 0;
        public async Task IncrementCount()
        {
        int ID =  await FileUploadServies.GetUploadID();
            #region Comment
            //HttpClient Http = new HttpClient();
            //string baseUrl = "https://localhost:44306/";
            //var temp2 = await Http.GetStringAsync($"{baseUrl}api/values/GetUploadIDAsync?fileName=test");

            //var emp = new FileListModel()
            //{
            //    ID = 2,
            //    Name = "test Post",
            //    PhotoPath = "Images/File3.png"
            //};


            //var serialized = JsonConvert.SerializeObject(emp);  
            //var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            //var response = await Http.PostAsync($"{baseUrl}api/values/PostStudent", stringContent);

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/values");
            //// set request body
            //var postBody = new { Title = "Blazor POST Request Example" };
            //request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(emp), Encoding.UTF8, "application/json");
            //// add authorization header
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "my-token");
            //// add custom http header
            //request.Headers.Add("My-Custom-Header", "foobar");

            // send request
            // using var response = await HttpClient.SendAsync(request); 
            #endregion
        }
        [JSInvokable]
        public static Task<int[]> ReturnArrayAsync()
        {
            return Task.FromResult(new int[] { 1, 2, 3 });
        }
       
        [JSInvokable("startUpload")]
        public static Task<string> startUpload(string filename)
        {
            return Task.FromResult(filename);
        }
    }
}
