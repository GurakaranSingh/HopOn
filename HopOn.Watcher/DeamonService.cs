using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.XRay.Recorder.Core.Internal.Utils;
using HopOn.Core;
using HopOn.Core.Contract;
using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace HopOn.Watcher
{
    public class DaemonService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private AmazonS3Client _s3Client;
        public IConfiguration Configuration { get; }
        public DaemonService(ILogger<DaemonService> logger, IConfiguration configuration)
        {
            _logger = logger;
            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` environment variable.
                ServiceURL = Credential.ServiceURL,//_configurationManager.GetValue<string>("MySettings:ServiceURL"),
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            this._s3Client = new AmazonS3Client(Credential.accessKey, Credential.secretKey
           , config);
            Configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            List<ProgressBarList> AMazonList = GetRecord();
            await AbortFileStatus(AMazonList);
            //_logger.LogInformation("Starting daemon: " );
        }
        public async Task<HttpStatusCode> AbortFileStatus(List<ProgressBarList> ProgressList)
        {
            try
            {
                foreach (var item in ProgressList)
                {
                    UpdateFileStatus model = new UpdateFileStatus()
                    {
                        awsUniqueId = item.AwsId,
                        Guid = item.Guid,
                    };
                    var response = await _s3Client.AbortMultipartUploadAsync(new AbortMultipartUploadRequest
                    {
                        BucketName = Credential.bucketName,
                        Key = item.Guid,
                        UploadId = item.AwsId
                    });
                    DeleteEtags(item.AwsId);
                }

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }
        public void DeleteEtags(string AwsID)
        {
            string _connectionString = "Server=127.0.0.1;database=HopOn;user id=root;password=pass@word1234";//Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "DELETE from progressbarlists where AwsId=@AwsId";
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@AwsId", AwsID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<ProgressBarList> GetRecord()
        {
            try
            {
                List<ProgressBarList> list = new List<ProgressBarList>();
                string _connectionString = "Server=127.0.0.1;database=HopOn;user id=root;password=pass@word1234";//Configuration.GetConnectionString("DefaultConnection");
                DateTime CurrentDate = DateTime.Now.AddDays(-2);
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "select * from progressbarlists where Status=@status and LastUpdateDate<= @LastUpdateDate";
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@status", FileStatus.Pending);
                        command.Parameters.AddWithValue("@LastUpdateDate", CurrentDate);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new ProgressBarList
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FileName = reader["FileName"].ToString(),
                                AwsId = reader["AwsId"].ToString(),
                                Guid = reader["Guid"].ToString()
                            });
                        }
                    }
                    conn.Close();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");

        }
    }
}
