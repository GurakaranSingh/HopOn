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

namespace HopOn.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IConfiguration ConfigurationManager;
        private readonly string bucketName = "";
        private string accessKey = "";
        private string secretKey = "";
        private IUploadUtilityHelperServices _uploadUtilityHelperService;
        private IProgressBarListServices _ProgressBarListServices;
        public List<PartETag> eTags { get; set; } = new List<PartETag>();
        private AmazonS3Client _s3Client { get; set; }
      
       
        public UploadController(IUploadUtilityHelperServices uploadUtilityHelperService, 
            IConfiguration ConfigurationManager, IProgressBarListServices ProgressBarListServices)
        {
            this.ConfigurationManager = ConfigurationManager;
            accessKey = ConfigurationManager.GetValue<string>("MySettings:accessKey");
            secretKey = ConfigurationManager.GetValue<string>("MySettings:secretKey");
            bucketName = ConfigurationManager.GetValue<string>("MySettings:BucketName");
            _uploadUtilityHelperService = uploadUtilityHelperService;
            _ProgressBarListServices = ProgressBarListServices;
            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` environment variable.
                ServiceURL = ConfigurationManager.GetValue<string>("MySettings:ServiceURL") ,
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            this._s3Client = new AmazonS3Client(accessKey, secretKey
            , config);
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
        
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            try
            {
                // var listBucketResponse = await this._s3Client.ListBucketsAsync();

                //UploadedFile upload = new UploadedFile
                //{
                //    AwsId = "sbadjhasdjhsa",
                //    FileName = "testDemoFile",
                //    FilePath = "~/hjsds/jhds/sndsa",
                //    FileSize = "123456789",
                //};
                //bool flag = await _uploadUtilityHelperService.InsertUploadedFileAsync(upload);
                var upload =   await _uploadUtilityHelperService.GetAllFilesAsync();
                var progressBars =   await _ProgressBarListServices.GetAllFilesAsync();
                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost("UploadingChunckBytes")]
        public async Task<ActionResult> UploadingChunckBytes(ChunkModel obj)
        {
            try
            {
                obj.chunkData = obj.chunkData.Split(',')[1].ToString();
                var bytes = Convert.FromBase64String(obj.chunkData);
                using (Stream stream = new MemoryStream(bytes))
                {
                    //Step 2: upload each chunk (this is run for every chunk unlike the other steps which are run once)
                    var uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = obj.FileName,
                        UploadId = obj.awsUniqueId,
                        PartNumber = obj.chunkIndex,
                        InputStream = stream,
                        IsLastPart = false,
                        PartSize = stream.Length
                    };
                    var uploadPartResponse = await _s3Client.UploadPartAsync(uploadRequest);
                    return new JsonResult(uploadPartResponse);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("GetUploadProject")]
        public async Task<ActionResult> GetUploadProject(GetUploadIdModel obj)
        {
            string uploadId = "";
            try
            {

                // ok so minio is setup give me 10 mints i have some labor to move some furniture am coming back 
                // or till then look for how you specify host address in sdk 
                // like http://127.0.0.1:9000
                // ok ??
                //ok
                //Step 1: build and send a multi upload request
                {
                    var initiateRequest = new InitiateMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = obj.fileName
                    };
                    var initiateMultipartUploadResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
                    uploadId = initiateMultipartUploadResponse.UploadId;
                }
                if(uploadId != "")
                {
                    ProgressBarList ProgressList = new ProgressBarList()
                    {
                        AwsId = uploadId,
                        FileName = obj.fileName
                    };

                    if(await _ProgressBarListServices.InsertProgressFileAsync(ProgressList)) 
                    {
                        return new JsonResult(new { uploadId });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new JsonResult(new { uploadId });
            //return new JsonResult(new { status = true, awsUniqueId = "1234567890" });
        }
        [HttpPost("FinalCallFOrCHunk")]
        public async Task<string> FinalCallFOrCHunk(FinalUpload obj)
        {

            var response = ""; Stream stream;
            // Retreiving Previous ETags
            List<string> TagIds = new List<string>();
            try
            {
                if (obj.prevETags.Count > 0)
                {
                    foreach (EtagModel model in obj.prevETags)
                    { SetAllETags(model); }
                }
                if (obj.lastpart)
                {
                    //eTags.Add(new PartETag
                    //{
                    //    PartNumber = GPartNumber,
                    //    ETag = obj.prevETags
                    //});

                    var completeRequest = new CompleteMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = obj.fileName,
                        UploadId = obj.UploadId,
                        PartETags = eTags
                    };

                    var result = await _s3Client.CompleteMultipartUploadAsync(completeRequest);

                    if (result.HttpStatusCode == HttpStatusCode.OK)
                    {
                        #region SaVeFileMetaData
                        UploadedFile upload = new UploadedFile
                        {
                            AwsId = obj.UploadId,
                            FileName = obj.fileName,
                            FilePath = "",
                            FileSize = obj.FileSize,
                        };
                        bool flag = await _uploadUtilityHelperService.InsertUploadedFileAsync(upload);

                        #endregion
                    }

                    //Set the uploadId and fileURLs with the response.
                    return response = obj.UploadId + "|success|" + result.Location + "|";
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
        public async Task EnableAccelerationAsync()
        {
            try
            {
                var putRequest = new PutBucketAccelerateConfigurationRequest
                {
                    BucketName = bucketName,
                    AccelerateConfiguration = new AccelerateConfiguration
                    {
                        Status = BucketAccelerateStatus.Enabled
                    }
                };
                await _s3Client.PutBucketAccelerateConfigurationAsync(putRequest);
                var getRequest = new GetBucketAccelerateConfigurationRequest
                {
                    BucketName = bucketName
                };
                var response = await _s3Client.GetBucketAccelerateConfigurationAsync(getRequest);
            }
            catch (Exception ex)
            {
            }
        }
        public void SetAllETags(EtagModel prevETags)
        {
            //var partETags = new List<PartETag>();
            //var splittedPrevETags = prevETags.Split(',');

            eTags.Add(new PartETag
            {
                PartNumber = prevETags.PartNumber,
                ETag = prevETags.ETag
            });


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
