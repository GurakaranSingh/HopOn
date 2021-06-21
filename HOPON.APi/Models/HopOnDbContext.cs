using HopOn.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace HOPON.APi.Models
{
    public class HopOnDbContext :DbContext
    {
        public HopOnDbContext(DbContextOptions<HopOnDbContext> options)
            :base(options)
        {

        }
        public DbSet<FileListModel> fileLists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Code to seed data
            modelBuilder.Entity<FileListModel>().HasData(
                 new FileListModel { ID = 1, Name = "IT", PhotoPath = "Images/file4.png" });
            modelBuilder.Entity<FileListModel>().HasData(
                new FileListModel { ID = 1, Name = "TPSReport.Xlsx", PhotoPath = "Images/file1.png" });
            modelBuilder.Entity<FileListModel>().HasData(
                new FileListModel { ID = 1, Name = "CannedReport.ppt", PhotoPath = "Images/file2.png" });
            modelBuilder.Entity<FileListModel>().HasData(
                new FileListModel { ID = 1, Name = "NewImage.jpeg", PhotoPath = "Images/file3.png" });
            modelBuilder.Entity<FileListModel>().HasData(
                new FileListModel { ID = 1, Name = "TextBook.png", PhotoPath = "Images/file4.png" });


        }
    }
}
