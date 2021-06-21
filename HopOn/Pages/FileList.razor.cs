using HopOn.Data;
using HopOn.Model;
using HopOn.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Pages
{
    public partial class FileList
    {
        public IList<UploadedFile> FileLists { get; set; }
       
        protected override Task OnInitializedAsync()
        {
            LoadFiles();
            return base.OnInitializedAsync();
        }
        public async Task LoadFiles()
        {
            //FileListModel FL = new FileListModel { ID = 1, Name = "TextBook.png", PhotoPath = "Images/file4.png" };
            //FileListModel FL1 = new FileListModel { ID = 1, Name = "TPSReport.Xlsx", PhotoPath = "Images/file1.png" };
            //FileListModel FL2 = new FileListModel { ID = 1, Name = "CannedReport.ppt", PhotoPath = "Images/file2.png" };
            //FileListModel FL3 = new FileListModel { ID = 1, Name = "NewImage.jpeg", PhotoPath = "Images/file3.png" };
            //FileListModel FL4 = new FileListModel { ID = 1, Name = "TextBook.png", PhotoPath = "Images/file4.png" };
            FileLists =  await _uploadUtilityHelperService.GetAllFilesAsync();
        }
    }
}
