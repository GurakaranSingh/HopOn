using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class FileUploadServices : IFileUploadServices
    {
        private readonly HttpClient httpClient;
        public FileUploadServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> GetUploadID()
        {
            try
            {
                return await httpClient.GetJsonAsync<int>("api/FileUpload");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
