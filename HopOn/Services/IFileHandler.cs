using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public interface IFileHandler
    {
        Task<string> GetUploadID(GetUploadIdModel request);
        Task UploadChunks(ChunkModel request);
        Task<HttpStatusCode> completed(FinalUpload request);
        Task<FileStreamResult> Download(string FileName);
        Task<bool> DeleteFileFromAmazon(string FileName);
        Task<bool> CancleUploading(string AWSID);
        Task<bool> UploadInOneCall(UploadInOneCallModel request);
        Task<HttpStatusCode> UpdateFileStatus(UpdateFileStatus Status);
        Task<HttpStatusCode> AbortFileStatus(UpdateFileStatus request);
        Task<HttpStatusCode> DeleteUncompleteCHunks();

    }
}
