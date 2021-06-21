using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using HopOn.Model;
using HopOn.Pages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HopOn.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string bucketName = "digitalage-users-1"; //ConfigurationManager.AppSettings["BucketName"];
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        // GET api/<ValuesController>/5
        [HttpGet]
        public async Task<string> GetUploadIDAsync(string uploadId, string fileName, int chunkIndex)
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

        // POST api/<ValuesController>
        [HttpPost]
        public void Post(FileListModel value)
        {
        }
        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<FileListModel>> PostStudent(FileListModel student)
        {
            FileListModel emp = new FileListModel();
            return emp;
        }
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
