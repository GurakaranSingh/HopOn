using HopOn.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Model
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        { }
     
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<ProgressBarList> ProgressBarLists { get; set; }
    }
}
