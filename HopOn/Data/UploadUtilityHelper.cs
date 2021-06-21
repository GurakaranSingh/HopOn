using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[AllowAnonymous]
public class UploadUtilityHelper
{
    private readonly string bucketName = "digitalage-users-1"; //ConfigurationManager.AppSettings["BucketName"];
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

    public async Task<string> GetUploadID(string uploadId, string fileName, int chunkIndex)
    {
        try
        {
            var _s3Client = new AmazonS3Client("EDPSY04WOETI0T1OQW0B",
                         "OCG9YP2Bo7p04CRzRZeKKh5DMbgcQ5akXAAgkOEy", bucketRegion);
            //Step 1: build and send a multi upload request
            if (chunkIndex == 0)
            {
                var initiateRequest = new InitiateMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };
                var initiateMultipartUploadResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
                uploadId = initiateMultipartUploadResponse.UploadId;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return uploadId;
    }

    //public async Task<string> SendPartialChunk(string uploadId, string fileName, int chunkIndex, int chunkMax)
    //{
    //    try
    //    {
    //        var _s3Client = new AmazonS3Client("EDPSY04WOETI0T1OQW0B",
    //                     "OCG9YP2Bo7p04CRzRZeKKh5DMbgcQ5akXAAgkOEy", bucketRegion);
    //        var lastPart = ((chunkMax - chunkIndex) == 1) ? true : false;
    //        var partNumber = chunkIndex + 1;
    //        var ms = new MemoryStream();
    //        /// stream.CopyTo(ms);
    //        ms.Position = 0;
    //        //Step 2: upload each chunk (this is run for every chunk unlike the other steps which are run once)
    //        var uploadRequest = new UploadPartRequest
    //        {
    //            BucketName = bucketName,
    //            Key = fileName,
    //            UploadId = uploadId,
    //            PartNumber = partNumber,
    //            InputStream = ms,
    //            IsLastPart = lastPart,
    //            PartSize = ms.Length
    //        };

    //        var uploadPartResponse = await _s3Client.UploadPartAsync(uploadRequest);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //    return "Success";
    //}
    //public async Task<string> FinalCallFOrCHunk(bool lastPart, int partNumber, string prevETags, string uploadId, string fileName)
    //{
    //    var _s3Client = new AmazonS3Client("EDPSY04WOETI0T1OQW0B",
    //                "OCG9YP2Bo7p04CRzRZeKKh5DMbgcQ5akXAAgkOEy", bucketRegion);
    //    var response = ""; Stream stream;
    //    // Retreiving Previous ETags
    //    var eTags = new List<PartETag>();
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(prevETags))
    //        {
    //            eTags = SetAllETags(prevETags);
    //        }
    //        if (lastPart)
    //        {
    //            //eTags.Add(new PartETag
    //            //{
    //            //    PartNumber = partNumber,
    //            //    ETag = uploadPartResponse.ETag
    //            //});

    //            var completeRequest = new CompleteMultipartUploadRequest
    //            {
    //                BucketName = bucketName,
    //                Key = fileName,
    //                UploadId = uploadId,
    //                PartETags = eTags
    //            };

    //            var result = await _s3Client.CompleteMultipartUploadAsync(completeRequest);
    //            //Set the uploadId and fileURLs with the response.
    //            response = uploadId + "|success|" + result.Location + "|";
    //            //For image get thumbnail url
    //            //if (HasImageExtension(fileName.ToLower()))
    //            //{
    //            //    //Send the Thumbnail URL
    //            //    response += result.Location.Replace(uploadRequest.Key, "thumbnail/" + uploadRequest.Key);
    //            //}
    //            //else
    //            //{
    //            //    response += "";
    //            //}
    //        }
    //        else
    //        {
    //            //eTags.Add(new PartETag
    //            //{
    //            //    PartNumber = partNumber,
    //            //    ETag = uploadPartResponse.ETag
    //            //});

    //            ////Set the uploadId and eTags with the response
    //            //response = uploadRequest.UploadId + "|" + GetAllETags(eTags);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //    return response;
    //}
    
}
