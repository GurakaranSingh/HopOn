using HopOn.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model.ViewModel
{
   public class FileShareModel
    {
        public string FileId { get; set; }
        public string FileToken { get; set; }
        public string FileLink { get; set; }
        public bool Expired { get; set; }
        public string ValidLink { get; set; }
        public LinkType Type { get; set; }
    }
}
