using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
    public class UploadInOneCallModel
    {
        public string FileName { get; set; }
        //public string FilePath { get; set; }
        public string File { get; set; }
        public string awsUniqueId { get; set; }
        public string ContentType { get; set; }
    }
}
