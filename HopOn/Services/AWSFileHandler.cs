using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class AWSFileHandler : IFileHandler
    {
        private AmazonS3Client _s3Client;
        private IConfiguration _configurationManager;
        private IProgressBarListServices _ProgressBarListServices;
        private IUploadUtilityHelperServices _uploadUtilityHelperService;
        public List<PartETag> eTags { get; set; } = new List<PartETag>();


        private readonly string bucketName = "";
        private string accessKey = "";
        private string secretKey = "";

        public AWSFileHandler(IUploadUtilityHelperServices uploadUtilityHelperService,
            IConfiguration configurationManager, IProgressBarListServices ProgressBarListServices)
        {
            _configurationManager = configurationManager;
            _ProgressBarListServices = ProgressBarListServices;
            _uploadUtilityHelperService = uploadUtilityHelperService;

            accessKey = _configurationManager.GetValue<string>("MySettings:accessKey");
            secretKey = _configurationManager.GetValue<string>("MySettings:secretKey");
            bucketName = _configurationManager.GetValue<string>("MySettings:BucketName");

            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` environment variable.
                ServiceURL = _configurationManager.GetValue<string>("MySettings:ServiceURL"),
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            this._s3Client = new AmazonS3Client(accessKey, secretKey
            , config);
        }

        public async Task<string> GetUploadID(GetUploadIdModel request)
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
                        Key = request.fileName
                    };
                    var initiateMultipartUploadResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
                    uploadId = initiateMultipartUploadResponse.UploadId;
                }
                if (uploadId != "")
                {
                    ProgressBarList ProgressList = new ProgressBarList()
                    {
                        AwsId = uploadId,
                        FileName = request.fileName
                    };

                    if (await _ProgressBarListServices.InsertProgressFileAsync(ProgressList))
                    {
                        return uploadId;
                    }
                }
                return uploadId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<EtagModel> UploadChunks(ChunkModel request)
        {
            try
            {

                request.chunkData = request.chunkData.Split(',')[1].ToString();
                var bytes = Convert.FromBase64String(request.chunkData);
                using (Stream stream = new MemoryStream(bytes))
                {
                    //Step 2: upload each chunk (this is run for every chunk unlike the other steps which are run once)
                    var uploadRequest = new UploadPartRequest
                    {
                        BucketName =bucketName,
                        Key = request.FileName,
                        UploadId = request.awsUniqueId,
                        PartNumber = request.chunkIndex,
                        InputStream = stream,
                        IsLastPart = false,
                        PartSize = stream.Length
                    };
                    var uploadPartResponse = await _s3Client.UploadPartAsync(uploadRequest);
                    EtagModel ReturnModel = new EtagModel()
                    {
                        ETag = uploadPartResponse.ETag,
                        PartNumber = uploadPartResponse.PartNumber
                    };

                    return ReturnModel;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> completed(FinalUpload request)
        {
            string response = "";
            List<string> TagIds = new List<string>();
            try
            {
                if (request.prevETags.Count > 0)
                {
                    foreach (EtagModel model in request.prevETags)
                    { SetAllETags(model); }
                }
                if (request.lastpart)
                {
                    var completeRequest = new CompleteMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = request.fileName,
                        UploadId = request.UploadId,
                        PartETags = eTags
                    };

                    var result = await _s3Client.CompleteMultipartUploadAsync(completeRequest);

                    if (result.HttpStatusCode == HttpStatusCode.OK)
                    {
                        #region SaVeFileMetaData
                        UploadedFile upload = new UploadedFile
                        {
                            AwsId = request.UploadId,
                            FileName = request.fileName,
                            FilePath = "",
                            FileSize = request.FileSize,
                        };
                        bool flag = await _uploadUtilityHelperService.InsertUploadedFileAsync(upload);
                        if (flag)
                        {
                            await _ProgressBarListServices.DeleteProgressFileAsync(request.UploadId);
                        }
                        #endregion
                    }
                    //Set the uploadId and fileURLs with the response.
                    response = request.UploadId + "|success|" + result.Location + "|";
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> CancleUploading(string AWSID)
        {
           return await _ProgressBarListServices.DeleteProgressFileAsync(AWSID);
        }
        public async Task<bool> DeleteFileFromAmazon(string FileName)
        {
            try
            {
                bool flag = false;
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = FileName
                };
                var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                if (response.HttpStatusCode == HttpStatusCode.NoContent)
                {
                    if (await _uploadUtilityHelperService.DeleteUploadedFileAsync(FileName))
                        flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<FileStreamResult> DownloadAWSFile(string FileName)
        {
            try
            {
                string responseBody = "";
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = FileName
                };
                GetObjectResponse response = await _s3Client.GetObjectAsync(request);
                return new FileStreamResult(response.ResponseStream, response.Headers.ContentType)
                { FileDownloadName = FileName };
            }
            catch (Exception ex)
            {
                throw;
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
    }
}
