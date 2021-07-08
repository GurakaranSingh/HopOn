using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
    public class ChunkModel
    {
        public string FileName { get; set; }
        public string chunkData { get; set; }
        public string awsUniqueId { get; set; }
        public int nextslice { get; set; }
        public int chunkMax { get; set; }
        public int chunkIndex { get; set; }
        public string Guid { get; set; }
        public string ClientHashKey { get; set; }
    }
}
