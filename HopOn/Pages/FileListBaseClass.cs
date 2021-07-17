using HopOn.Model;
using HopOn.Model.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
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
