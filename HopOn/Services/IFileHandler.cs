using HopOn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public interface IFileHandler
    {
        Task<string> GetUploadID(GetUploadIdModel request);
        Task<EtagModel> UploadChunks(ChunkModel request);
        Task<string>completed(FinalUpload request);
    }
}
