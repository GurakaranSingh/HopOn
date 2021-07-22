using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model.ViewModel
{
   public class ShowQuotaViewModel
    {

        public decimal Upload_Quota { get; set; }
        public decimal Remaining_Download_Quota { get; set; }
        public decimal Remaining_Upload_Quota { get; set; }
        public decimal Storage_Quota { get; set; }
        public decimal RemainiuploadPercentage { get; set; }
        public decimal RemainingDownloadPercentage { get; set; }
        public decimal RDQ { get; set; }
        public decimal RUQ { get; set; }
        public string RemainingDownloadQuota { get; set; }
        public string RemainingUploadQuota { get; set; }
        public decimal upload_Quota { get; set; }
        public string UploadQuota { get; set; }
        public decimal Download_Quota { get; set; }
        public string DownloadQuota { get; set; }
        public String  StorageQuota { get; set; }
    }
}
