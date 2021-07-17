﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
    public class FinalUpload
    {
        public bool lastpart { get; set; }
        public string UploadId { get; set; }
        public List<EtagModel> prevETags { get; set; }
        public string fileName { get; set; }
        public int PartNumber { get; set; }
        public int FileSize { get; set; }
        public string Guid { get; set; }
        public string FileType { get; set; }
        public int ChucksCount { get; set; }
    }
}
