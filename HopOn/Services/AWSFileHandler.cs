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
using System.Threading;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class AWSFileHandler : IFileHandler
    {
        private AmazonS3Client _s3Client;
        private IConfiguration _configurationManager;
        private IProgressBarListServices _ProgressBarListServices;
        private IUploadUtilityHelperServices _uploadUtilityHelperService;
        private readonly AppDBContext _appDBContext;

        public List<PartETag> eTags { get; set; } = new List<PartETag>();


        private readonly string bucketName = "";
        private string accessKey = "";
        private string secretKey = "";

        public AWSFileHandler(IUploadUtilityHelperServices uploadUtilityHelperService,
            IConfiguration configurationManager, IProgressBarListServices ProgressBarListServices, AppDBContext appDBContext)
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
            this._appDBContext = appDBContext;
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
                        Key = request.Guid
                    };
                    var initiateMultipartUploadResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
                    uploadId = initiateMultipartUploadResponse.UploadId;
                }
                if (uploadId != "")
                {
                    ProgressBarList ProgressList = new ProgressBarList()
                    {
                        AwsId = uploadId,
                        FileName = request.fileName,
                        Status = FileStatus.Pending,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        Guid = ""

                    };
                    _ProgressBarListServices.InsertProgressFileAsync(ProgressList);
                    return uploadId;
                }
                return uploadId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<HttpStatusCode> DeleteUncompleteCHunks()
        {
            try
            {
                HttpStatusCode response = HttpStatusCode.NoContent;
                DateTime CurrentDate = DateTime.Now.AddDays(-2);
                List<ProgressBarList> AmazonID = await _ProgressBarListServices.GetListAsync(FileStatus.Fail, CurrentDate);
                if (AmazonID.Count > 0)
                {
                    foreach (var item in AmazonID)
                    {
                        UpdateFileStatus model = new UpdateFileStatus()
                        {
                            awsUniqueId = item.AwsId,
                            Guid = item.Guid,
                        };
                        response = await AbortFileStatus(model);

                        if (response == HttpStatusCode.OK)
                        {
                            await _ProgressBarListServices.DeleteEtags(item.AwsId);
                        }
                    }
                    return response;
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }
        private void InsertProgressBar(ProgressBarList ProgressList)
        {
            _ProgressBarListServices.InsertProgressFileAsync(ProgressList);
        }
        public async Task<HttpStatusCode> AbortFileStatus(UpdateFileStatus request)
        {
            try
            {
                var response = await _s3Client.AbortMultipartUploadAsync(new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = request.Guid,
                    UploadId = request.awsUniqueId
                });
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> UpdateFileStatus(UpdateFileStatus Status)
        {
            try
            {
                ProgressBarList UpdateModel = _appDBContext.ProgressBarLists.Where(p => p.AwsId == Status.awsUniqueId).FirstOrDefault();
                if (UpdateModel != null)
                {
                    UpdateModel.Status = Status.Status;
                    UpdateModel.LastUpdateDate = DateTime.Now;
                    UpdateModel.Guid = Status.Guid;
                    UpdateModel.ChunkCount = UpdateModel.ChunkCount + 1;
                    _appDBContext.SaveChanges();
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }
        public async Task UploadChunks(ChunkModel request)
        {
            try
            {
                UpdateFileStatus statusModel = new UpdateFileStatus() { awsUniqueId = request.awsUniqueId, Status = FileStatus.Inprogress, Guid = request.Guid };
                await UpdateFileStatus(statusModel);
                request.chunkData = request.chunkData.Split(',')[1].ToString();
                byte[] bytes = Convert.FromBase64String(request.chunkData);
                Thread Background = new Thread(() => BackTask(request, bytes));
                Background.IsBackground = true;
                Background.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async void BackTask(ChunkModel request, byte[] bytes)
        {
            EtagModel ReturnModel;
            try
            {
                using (Stream stream = new MemoryStream(bytes))
                {
                    //Step 2: upload each chunk (this is run for every chunk unlike the other steps which are run once)
                    var uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = request.Guid,
                        UploadId = request.awsUniqueId,
                        PartNumber = request.chunkIndex,
                        InputStream = stream,
                        IsLastPart = false,
                        PartSize = stream.Length
                    };
                    var uploadPartResponse = await _s3Client.UploadPartAsync(uploadRequest);
                    ReturnModel = new EtagModel()
                    {
                        ETag = uploadPartResponse.ETag,
                        PartNumber = uploadPartResponse.PartNumber,
                        AmazonID = request.awsUniqueId
                    };
                    if (ReturnModel != null)
                        await _uploadUtilityHelperService.InsertEtagModel(ReturnModel).ConfigureAwait(false);
                }
               
               
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<HttpStatusCode> completed(FinalUpload request)
        {
            List<string> TagIds = new List<string>();
            try
            {
                UpdateFileStatus statusModel;
                request.ChucksCount = _appDBContext.ProgressBarLists.Where(p => p.AwsId == request.UploadId).Select(s=>s.ChunkCount).FirstOrDefault();
                request.prevETags = await _uploadUtilityHelperService.GetETageByID(request.UploadId);
               if(request.ChucksCount != request.prevETags.Count)
                {
                    return HttpStatusCode.Conflict;
                    statusModel = new UpdateFileStatus() { awsUniqueId = request.UploadId, Status = FileStatus.Fail , Guid = request.Guid };
                    await UpdateFileStatus(statusModel);
                }
                else
                {
                    foreach (EtagModel model in request.prevETags)
                    { SetAllETags(model); }
                    
                    var completeRequest = new CompleteMultipartUploadRequest
                    {
                        BucketName = bucketName,
                        Key = request.Guid,
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
                            FilePath = "Images/" + GetFileName(request.fileName.Split(".")[1].ToLower()),
                            FileSize = request.FileSize,
                            Guid = request.Guid,
                        };
                        bool flag = await _uploadUtilityHelperService.InsertUploadedFileAsync(upload);
                        if (flag)
                        {
                            await _uploadUtilityHelperService.DeleteEtagModel(request.UploadId);
                        }
                        #endregion
                        statusModel = new UpdateFileStatus() { awsUniqueId = request.UploadId, Status = FileStatus.Succeed, Guid = request.Guid };
                         await UpdateFileStatus(statusModel);
                        return HttpStatusCode.OK;
                    }
                    else
                    {
                        statusModel = new UpdateFileStatus() { awsUniqueId = request.UploadId, Status = FileStatus.Fail, Guid = request.Guid };
                        await UpdateFileStatus(statusModel);
                        return HttpStatusCode.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> UploadInOneCall(UploadInOneCallModel request)
        {
            bool response = false; UpdateFileStatus statusModel;
            try
            {
                request.File = request.File.Split(',')[1].ToString();
                byte[] bytes = Convert.FromBase64String(request.File);
                using (Stream stream = new MemoryStream(bytes))
                {
                    var putRequest = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = request.Guid,
                        InputStream = stream,
                        ContentType = request.ContentType
                    };

                    PutObjectResponse result = await _s3Client.PutObjectAsync(putRequest);

                    if (result.HttpStatusCode == HttpStatusCode.OK)
                    {
                        #region SaVeFileMetaData
                        UploadedFile upload = new UploadedFile
                        {
                            AwsId = request.awsUniqueId,
                            FileName = request.FileName,
                            FileSize = request.ContentType,
                            Guid = request.Guid,
                            FilePath = "Images/"+ GetFileName(request.FileName.Split(".")[1].ToLower())
                        };
                        bool flag = await _uploadUtilityHelperService.InsertUploadedFileAsync(upload);
                        response = flag;


                        statusModel = new UpdateFileStatus() { awsUniqueId = request.awsUniqueId, Status = FileStatus.Succeed, Guid = request.Guid };
                        await UpdateFileStatus(statusModel);
                        #endregion
                    }
                }
                //Set the uploadId and fileURLs with the response.
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GetFileName(string ext)
        {
            switch(ext)
            {
                case "jpg":
                case "jpeg":
                    return "image.png";
                case "zip":
                    return "Zip.jpg";
                case "mp4":
                    return "mp4.png";
                case "pdf.png":
                    return "pdf.png";
                default:
                    return "Default.png";
            }
        }
        public async Task<bool> CancleUploading(string AWSID)
        {
            return await _ProgressBarListServices.DeleteProgressFileAsync(AWSID);
        }
        public async Task<bool> DeleteFileFromAmazon(string Guid)
        {
            try
            {
                bool flag = false;
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = Guid
                };
                var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                if (response.HttpStatusCode == HttpStatusCode.NoContent)
                {
                    if (await _uploadUtilityHelperService.DeleteUploadedFileAsync(Guid))
                        flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<FileStreamResult> Download(string Guid)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = Guid
                };
                string _fileName = _appDBContext.UploadedFiles.Where(sp => sp.Guid == Guid).Select(sp => sp.FileName).FirstOrDefault();
                GetObjectResponse response = await _s3Client.GetObjectAsync(request);
                return new FileStreamResult(response.ResponseStream, response.Headers.ContentType)
                {
                    FileDownloadName = _fileName,
                };
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
