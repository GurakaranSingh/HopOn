using HopOn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class MinioFileHandler : IFileHandler
    {
        //[HttpPost("UploadMinio")]
        //public async Task<ActionResult> UploadMinio(ChunkModel obj)
        //{

        //    var location = RegionEndpoint.USEast1.ToString();
        //    var objectName = "New Update 2.zip";
        //    var filePath = "E:/";
        //    var contentType = "application/zip";

        //    try
        //    {
        //        // Make a bucket on the server, if not already present.
        //        bool found = await minio.BucketExistsAsync(bucketName);
        //        if (!found)
        //        {
        //            await minio.MakeBucketAsync(bucketName, location);
        //        }

        //        obj.chunkData = obj.chunkData.Split(',')[1].ToString();
        //        var bytes = Convert.FromBase64String(obj.chunkData);
        //        using (Stream stream = new MemoryStream(bytes))
        //        {
        //            var uploadPartResponse = minio.PutObjectAsync(bucketName, obj.FileName, stream, stream.Length);
        //            return new JsonResult(uploadPartResponse);
        //        }

        //        // Upload a file to bucket.
        //        //  await minio.PutObjectAsync(bucketName, objectName, filePath, contentType);
        //    }
        //    catch (MinioException e)
        //    {
        //        return new JsonResult("");
        //    }
        //}
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
