using HopOn.Core.Contract;
using HopOn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HopOn.Core.Services
{
    public class AWSApiFileHandler : IFileHandler
    {
        public Task<string> completed(FinalUpload request)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUploadID(GetUploadIdModel request)
        {
            throw new NotImplementedException();
        }

        public Task<EtagModel> UploadChunks(ChunkModel request)
        {
            throw new NotImplementedException();
        }
    }
}
