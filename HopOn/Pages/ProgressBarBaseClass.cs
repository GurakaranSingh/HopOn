using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HopOn.Pages
{
    public class ProgressBarBaseClass : ComponentBase
    {
        public List<ProgressBarList> FileLists { get; set; }
        [Inject]
        private AppDBContext _appDBContext { get; set; }
        private async Task<List<ProgressBarList>> GetAllFilesAsync()
        {
            try
            {
                List<ProgressBarList> Pbfiles = new List<ProgressBarList>();
                if (await _appDBContext.ProgressBarLists.CountAsync() > 0)
                {
                    Pbfiles = await _appDBContext.ProgressBarLists.ToListAsync();
                }
                return Pbfiles;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        protected async override Task OnInitializedAsync()
        {
           // await LoadFiles();
        }
        public async Task LoadFiles()
        {
            FileLists = await GetAllFilesAsync();
        }
    }
}
