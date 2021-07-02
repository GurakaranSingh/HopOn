﻿using HopOn.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public interface IFileHandler
    {
        Task<string> GetUploadID(GetUploadIdModel request);
        Task UploadChunks(ChunkModel request);
        Task<string>completed(FinalUpload request);
        Task<FileStreamResult> DownloadAWSFile(string FileName);
        Task<bool> DeleteFileFromAmazon(string FileName);
        Task<bool> CancleUploading(string AWSID);
    }
}
