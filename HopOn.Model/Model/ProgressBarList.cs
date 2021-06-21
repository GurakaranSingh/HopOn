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
    }
}
