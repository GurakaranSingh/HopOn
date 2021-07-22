using HopOn.Core.Contract;
using HopOn.Model;
using HopOn.Model.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Threading.Tasks;
namespace HopOn.Pages
{
    public class FileListBaseClass : ComponentBase
    {
        public List<UploadedFile> FileLists { get; set; }
        public List<DeleteFileModel> DeleteFileList { get; set; }
        public bool ShowLinkModel { get; set; }
        public bool ShowDeleteModel { get; set; }
        public string CurrentFileGuid { get; set; }
        [Inject]
        private AppDBContext _appDBContext { get; set; }
        [Inject]
        private IFileHandler _fileHandler { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        private async Task<List<UploadedFile>> GetAllFilesAsync()
        {
            try
            {
                int count = 0;
                List<UploadedFile> Upfiles = new List<UploadedFile>();
                count = _appDBContext.UploadedFiles.Count();
                if (count > 0)
                {
                    Upfiles = await _appDBContext.UploadedFiles.ToListAsync();
                }
                return Upfiles;

            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public void ShowLinkModelMethod(string Guid)
        {
            CurrentFileGuid = Guid;
            ShowLinkModel = true;
        }
        public async Task DownloadWithPreSignedUrl(string Guid)
        {
            var response = await _fileHandler.Download(Guid);
            await DownloadFile(Guid, response.FileDownloadName);
        }
        public async Task DownloadFile(string Key, string FileDownloadName)
        {
            try
            {
                string DownloadUrl = await _fileHandler.GetDownloadPreSignedUrl(Key);
                await JSRuntime.InvokeVoidAsync(
                           "downloadFromUrl",
                           new
                           {
                               url = DownloadUrl,
                               FileName = FileDownloadName,
                           });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void ShowDeleteModelMethod()
        {
            ShowLinkModel = true;
        }
        public async Task FileListRefresh()
        {
            await LoadFiles();
        }
        protected async override Task OnInitializedAsync()
        {
            await LoadFiles();
        }
        public async Task LoadFiles()
        {
            FileLists = await GetAllFilesAsync();
        }
    }
}
