using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Pages
{
    public class ProgressBarBaseClass : ComponentBase
    {
        public List<ProgressBarList> FileLists { get; set; }
        [Inject]
        private AppDBContext DBcontetx { get; set; }
        private async Task<List<ProgressBarList>> GetAllFilesAsync()
        {
            try
            {
                int count = 0;
                List<ProgressBarList> Pbfiles = new List<ProgressBarList>();
                count = DBcontetx.ProgressBarLists.Count();
                if (count > 0)
                {
                    Pbfiles = await DBcontetx.ProgressBarLists.ToListAsync();
                }
                return Pbfiles;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task ProgressBarRefresh()
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
