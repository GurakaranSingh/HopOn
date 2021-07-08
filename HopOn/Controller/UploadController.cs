using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using HopOn.Data;
using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Services;
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

namespace HopOn.Controller
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
        #endregion



        public UploadController(IUploadUtilityHelperServices uploadUtilityHelperService, IFileHandler fileHandler, IProgressBarListServices progressBarListService
           )
        {
            #region Resolve Dependancy
            _fileHandler = fileHandler;
            _progressBarListService = progressBarListService;
            _uploadUtilityHelperService = uploadUtilityHelperService;
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
       
        [HttpPost("UploadingChunckBytes")]
        //[HashValidateFilter()]
        //  [RequestFormLimits(MultipartBodyLengthLimit = 3147483648)]
        public async Task<bool> UploadingChunckBytes(ChunkModel obj)
        {
            try
            {
                //var testvar = HttpContext.Request.Query["ClientHashKey"].ToString();
                //if (await ChechHash(obj.chunkData, obj.ClientHashKey))
                //{
                    await _fileHandler.UploadChunks(obj);
                //}
                //else
                //{
                //    return false;
                //}
                return true;
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
        [HttpGet("DownloadAWSFile/{id}")]
        public async Task<FileStreamResult> DownloadAWSFile(string id)
        {
            return await _fileHandler.DownloadAWSFile(id);
        }

        [HttpGet("GetAllProgressFile")]
        public async Task<List<ProgressBarList>> GetAllProgressFile()
        {
            return await _progressBarListService.GetAllFilesAsync();
        }

    }
}
