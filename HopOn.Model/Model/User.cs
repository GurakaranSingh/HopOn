using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model.Model
{
   public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DownloadQuota { get; set; }
        public int UploadQuota { get; set; }
        public int MaxDownload { get; set; }
        public int MaxUpload { get; set; }
        public int StorageQuota { get; set; }
        public DateTime LastUpdate { get; set; }
    }
    public enum QuotaType
    {
        Download = 0,
        Upload = 1
    }
}
