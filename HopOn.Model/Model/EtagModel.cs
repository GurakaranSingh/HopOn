using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
   public class EtagModel
    {
        [Key]
        public int Id { get; set; }
        public int PartNumber { get; set; }
        public string ETag { get; set; }
        public string AmazonID { get; set; }
    }
}
