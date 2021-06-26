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

namespace HopOn.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        #region Declare Variable
        private IFileHandler _fileHandler;
        #endregion



        public UploadController(IFileHandler fileHandler)
        {
            #region Resolve Dependancy
            _fileHandler = fileHandler;
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
        //[DisableRequestSizeLimit]
        public async Task<ActionResult> UploadingChunckBytes(ChunkModel obj)
        {
            try
            {
                EtagModel model = await _fileHandler.UploadChunks(obj);
                return new JsonResult(new { model });
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

        [HttpPost("FinalCallFOrCHunk")]
        public async Task<string> FinalCallFOrCHunk(FinalUpload obj)
        {
            string response = await _fileHandler.completed(obj);
            return response;
        }
        [HttpPost("CancleUploading")]
        public async Task<bool> CancleUploading(string AWSID)
        {
            bool response = await _fileHandler.CancleUploading(AWSID);
            return response;
        }
        [HttpPost("DeleteAWSFile")]
        public async Task<bool> DeleteAWSFile(string FileName)
        {
            bool response = await _fileHandler.DeleteFileFromAmazon(FileName);
            return response;
        }
        [HttpGet("DownloadAWSFile")]
        public async Task<FileStreamResult> DownloadAWSFile(string FileName)
        {
            return await _fileHandler.DownloadAWSFile(FileName);
        }
    }
}
