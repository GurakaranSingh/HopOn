using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model.Model
{
   public class ProgressBarList
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string AwsId { get; set; }
        public FileStatus Status { get; set; }
        public  DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Guid { get; set; }
        public int ChunkCount { get; set; }
    }
    public enum FileStatus
    { 
    Pending =0,
    Inprogress = 1,
    Succeed = 2,
    Fail = 3
    }
}
