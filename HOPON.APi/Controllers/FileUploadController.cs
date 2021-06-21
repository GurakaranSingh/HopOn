using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;

namespace HOPON.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string bucketName = "digitalage-users-1"; //ConfigurationManager.AppSettings["BucketName"];
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        public FileUploadController()
        {

        }

        [HttpPost("GetUploadProject")]
        public async Task<ActionResult> GetUploadProject(string fileSize,string fileName)
        {
            return new JsonResult(new { status = true, awsUniqueId = "1234567890" });
        }
        // GET api/<ValuesController>/5
        [HttpGet("GetUploadIDAsync")]
        public async Task<string> GetUploadIDAsync(string fileName)
        {
            string uploadId= "";
            try
            {
                var _s3Client = new AmazonS3Client("EDPSY04WOETI0T1OQW0B",
                             "OCG9YP2Bo7p04CRzRZeKKh5DMbgcQ5akXAAgkOEy", bucketRegion);
                //Step 1: build and send a multi upload request
              
                    var initiateRequest = new InitiateMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = fileName
                    };
                    var initiateMultipartUploadResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
                    uploadId = initiateMultipartUploadResponse.UploadId;
                
            }
            catch (Exception ex)
            {
                throw;
            }
            return uploadId;

        }

        //// POST api/<ValuesController>
        [HttpPost("SendPartialCHunk")]
        public async Task<string> SendPartialChunk(string uploadId, string fileName, int chunkIndex, int chunkMax)
        {
            try
            {
                var _s3Client = new AmazonS3Client("EDPSY04WOETI0T1OQW0B",
                             "OCG9YP2Bo7p04CRzRZeKKh5DMbgcQ5akXAAgkOEy", bucketRegion);
                var lastPart = ((chunkMax - chunkIndex) == 1) ? true : false;
                var partNumber = chunkIndex + 1;
                var ms = new MemoryStream();
                /// stream.CopyTo(ms);
                ms.Position = 0;
                //Step 2: upload each chunk (this is run for every chunk unlike the other steps which are run once)
                var uploadRequest = new UploadPartRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    UploadId = uploadId,
                    PartNumber = partNumber,
                    InputStream = ms,
                    IsLastPart = lastPart,
                    PartSize = ms.Length
                };

                var uploadPartResponse = await _s3Client.UploadPartAsync(uploadRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
            return "Success";
        }

        [HttpPost("FinalCallFOrCHunk")]
        public async Task<string> FinalCallFOrCHunk(bool lastPart, int partNumber, string prevETags, string uploadId, string fileName)
        {
            var _s3Client = new AmazonS3Client("EDPSY04WOETI0T1OQW0B",
                        "OCG9YP2Bo7p04CRzRZeKKh5DMbgcQ5akXAAgkOEy", bucketRegion);
            var response = ""; Stream stream;
            // Retreiving Previous ETags
            var eTags = new List<PartETag>();
            try
            {
                if (!string.IsNullOrEmpty(prevETags))
                {
                    eTags = SetAllETags(prevETags);
                }
                if (lastPart)
                {
                    //eTags.Add(new PartETag
                    //{
                    //    PartNumber = partNumber,
                    //    ETag = uploadPartResponse.ETag
                    //});

                    var completeRequest = new CompleteMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = fileName,
                        UploadId = uploadId,
                        PartETags = eTags
                    };

                    var result = await _s3Client.CompleteMultipartUploadAsync(completeRequest);
                    //Set the uploadId and fileURLs with the response.
                    response = uploadId + "|success|" + result.Location + "|";
                    //For image get thumbnail url
                    //if (HasImageExtension(fileName.ToLower()))
                    //{
                    //    //Send the Thumbnail URL
                    //    response += result.Location.Replace(uploadRequest.Key, "thumbnail/" + uploadRequest.Key);
                    //}
                    //else
                    //{
                    //    response += "";
                    //}
                }
                else
                {
                    //eTags.Add(new PartETag
                    //{
                    //    PartNumber = partNumber,
                    //    ETag = uploadPartResponse.ETag
                    //});

                    ////Set the uploadId and eTags with the response
                    //response = uploadRequest.UploadId + "|" + GetAllETags(eTags);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }


        private List<PartETag> SetAllETags(string prevETags)
        {
            var partETags = new List<PartETag>();
            var splittedPrevETags = prevETags.Split(',');

            for (int i = 0; i < splittedPrevETags.Length; i++)
            {
                partETags.Add(new PartETag
                {
                    PartNumber = Int32.Parse(splittedPrevETags[i]),
                    ETag = splittedPrevETags[i + 1]
                });

                i = i + 1;
            }

            return partETags;
        }
      
        private string GetAllETags(List<PartETag> newETags)
        {
            var newPartETags = "";
            var isNotFirstTag = false;

            foreach (var eTag in newETags)
            {
                newPartETags += ((isNotFirstTag) ? "," : "") + (eTag.PartNumber.ToString() + ',' + eTag.ETag);
                isNotFirstTag = true;
            }
            return newPartETags;
        }

        private bool HasImageExtension(string fileName)
        {
            return (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".bmp")
                || fileName.EndsWith(".gif") || fileName.EndsWith(".tif") || fileName.EndsWith(".tiff"));
        }
    }
}
