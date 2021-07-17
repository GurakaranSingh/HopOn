using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
    public class GetUploadIdModel
    {
        public int fileSize { get; set; }
        public string Guid { get; set; }
        public string fileName { get; set; }
    }
}
