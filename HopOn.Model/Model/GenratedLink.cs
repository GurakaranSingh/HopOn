using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model.Model
{
    public enum LinkType
    {
        [Display(Name ="Share File")]
        Share = 0,
        [Display(Name = "Genrate Token Link")]
        Token = 1
    }
    public class GenratedLink
    {
        [Key]
        public int Id { get; set; }
        public string Guid { get; set; }
        public string FileLink { get; set; }
        public LinkType Type { get; set; }
        public string Token { get; set; }
        public bool Expire { get; set; }
        public string FileId { get; set; }
    }
}
