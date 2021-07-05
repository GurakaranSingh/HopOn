using HopOn.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
    public class UpdateFileStatus
    {
        public FileStatus Status { get; set; }
        public string awsUniqueId { get; set; }
        public string Guid { get; set; }

    }
}
