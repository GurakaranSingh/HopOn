using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using HopOn.Data;
using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.AspNetCore.Components.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Minio;
using Minio.Exceptions;
using HopOn.Filter;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using System.Text;
using HopOn.Model.ViewModel;
using HopOn.Core.Contract;

namespace HopOn.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        #region Declare Variable
        private IFileHandler _fileHandler;
        private IProgressBarListServices _progressBarListService;
        private IUploadUtilityHelperServices _uploadUtilityHelperService;
        private IHostingEnvironment Environment;
        private IConfiguration _configurationManager;
        private IFileLinkService _fileLinkService;

        #endregion
        public UploadController(IUploadUtilityHelperServices uploadUtilityHelperService, IFileHandler fileHandler, 
            IProgressBarListServices progressBarListService
           ,IConfiguration configurationManager, IFileLinkService fileLinkService)
        {
            #region Resolve Dependancy
            _fileHandler = fileHandler;
            _progressBarListService = progressBarListService;
            _uploadUtilityHelperService = uploadUtilityHelperService;
            _configurationManager = configurationManager;
            _fileLinkService = fileLinkService;
            #endregion
        }

        [HttpPost("SaveFileNameLocalStorage")]
        public async Task<ActionResult> SaveFileNameLocalStorage(FileModel obj)
        {
            try
            {
                List<FIleUploadModel> cats = new List<FIleUploadModel>();
                foreach (FIleUploadModel model in obj.FileUploadModel)
                {
                    {
                        cats.Add(new FIleUploadModel() { Name = model.Name, Size = model.Size });
                        HttpContext.Session.SetString("dataList", model.Name);
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new JsonResult(true);
        }
        private async Task<bool> CheckHash(string chunkData, string ClientHashKey)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                bool flag = false;
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(chunkData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string ServerHashKey = builder.ToString();
                if (ClientHashKey == ServerHashKey)
                {
                    flag = true;
                }
                return flag;
            }
        }
        [HttpPost("UploadingChunckBytes")]
        [HashValidateFilter]
        public async Task<HttpStatusCode> UploadingChunckBytes(ChunkModel obj)
        {
            try
            {
                //  var testvar = HttpContext.Request.Query["ClientHashKey"].ToString();
                //if (await CheckHash(obj.chunkData, obj.ClientHashKey))
                //{
              return  await _fileHandler.UploadChunks(obj);
                // }
                //  else
                //  {
                //      return false;
                //  }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("GetUploadProject")]
        public async Task<ActionResult> GetUploadProject(GetUploadIdModel obj)
        {
            string uploadId = await _fileHandler.GetUploadID(obj);
            return new JsonResult(new { uploadId });
        }
       
        [HttpPost("DeleteMultipleFiles")]
        public async Task DeleteMultipleFiles(DeleteUpdateModel obj)
        {
            await _uploadUtilityHelperService.DeleteListFile(obj);
        }
        [HttpPost("FinalCallFOrCHunk")]
        public async Task<HttpStatusCode> FinalCallFOrCHunk(FinalUpload obj)
        {
            return await _fileHandler.completed(obj);

        }
        [HttpGet("Watcher")]
        public async Task<HttpStatusCode> Watcher()
        {
            return await _fileHandler.DeleteUncompleteCHunks();
        }
        [HttpPost("UploadInOneCall")]
        public async Task<bool> UploadInOneCall(UploadInOneCallModel obj)
        {

            bool response = await _fileHandler.UploadInOneCall(obj);
            return response;
        }
        [HttpPost("CancleUploading")]
        public async Task<bool> CancleUploading(string AWSID)
        {
            bool response = await _fileHandler.CancleUploading(AWSID);
            return response;
        }
        [HttpPost("DeleteAWSFile/{id}")]
        public async Task<bool> DeleteAWSFile(string id)
        {
            bool response = await _fileHandler.DeleteFileFromAmazon(id);
            return response;
        }
        [HttpGet("Download/{id}")]
        public async Task<FileStreamResult> Download(string id)
        {
            return await _fileHandler.Download(id);
        }
        [HttpGet("ShareFile/{id}")]
        public async Task<FileStreamResult> ShareFile(string id)
        {
            return await _fileHandler.Download(id);
        }
        [HttpGet("GetAllProgressFile")]
        public async Task<List<ProgressBarList>> GetAllProgressFile()
        {
            return await _progressBarListService.GetAllFilesAsync();
        }
        [HttpGet("Test")]
        [Route("share/link")]
        public void Test()
        {
            
        }

    }
}
