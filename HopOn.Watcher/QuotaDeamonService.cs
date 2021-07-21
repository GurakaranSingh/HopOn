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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HopOn.Watcher
{
    class QuotaDeamonService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private AmazonS3Client _s3Client;
        public IConfiguration Configuration { get; }
        private readonly string _connectionString;
        public QuotaDeamonService(ILogger<QuotaDeamonService> logger, IConfiguration configuration)
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
             _connectionString = "Server=127.0.0.1;database=HopOn;user id=root;password=pass@word1234";//Configuration.GetConnectionString("DefaultConnection");

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void RefillQuota()
        {
            try
            {
                DateTime LastUpdateDate = DateTime.Now.AddDays(-1);
                DateTime TodayDate = DateTime.Now;
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "UPDATE users set DownloadQuota = MaxDownload , UploadQuota= MaxUpload,LastUpdate = @Today  where LastUpdate = @CurrentDate";
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@CurrentDate", LastUpdateDate);
                        command.Parameters.AddWithValue("@Today", TodayDate);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
