using HopOn.Core.Contract;
using HopOn.Data;
using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Model.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HopOn.Pages.SharedFile
{
    public class SharedFileListBaseClass : ComponentBase
    {
        public List<FileShareModel> LinkModel { get; set; }

        [Parameter] public LinkType ShareLink { get; set; } = LinkType.Share;
        [Parameter] public LinkType GenerateToken { get; set; } = LinkType.Token;
        [Parameter]
        public string CurrentFileGuid { get; set; }
        [Inject]
        private AppDBContext _appDBContext { get; set; }
        [Inject]
        private IFileLinkService _fileLinkService { get; set; }


        private async Task<List<FileShareModel>> GetAllFilesAsync(string Id)
        {
            try
            {
                int count = 0;
                List<FileShareModel> Upfiles = new List<FileShareModel>();
                count = _appDBContext.GeneratedLinks.Count();
                if (count > 0)
                {
                    Upfiles =  _appDBContext.GeneratedLinks.Where(g => g.FileId.Equals(Id)).Select(s => new FileShareModel()
                    {
                        Expired = s.Expire,
                        FileLink = s.FileLink,
                        FileToken = s.Token,
                        FileId = s.FileId.TrimStart(Convert.ToChar("'"))
                            .TrimEnd(Convert.ToChar("'")),
                    }).ToList();
                }
                return Upfiles;

            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public void GenerateLink(ChangeEventArgs e)
        {
            LinkType linkType = e.Value.ToString() == "Share" ? LinkType.Share: LinkType.Token;
            _fileLinkService.GenrateFileLink(CurrentFileGuid,linkType);
            OnInitializedAsync();
        }
        private async Task<List<FileShareModel>> GetAllFilesAsync()
        {
            try
            {
                int count = 0;
                List<FileShareModel> Upfiles = new List<FileShareModel>();
                count = _appDBContext.GeneratedLinks.Count();
                if (count > 0)
                {
                    Upfiles = _appDBContext.GeneratedLinks.Select(s => new FileShareModel()
                    {
                        Expired = s.Expire,
                        FileLink = s.FileLink,
                        FileToken = s.Token,
                        FileId = s.FileId.TrimStart(Convert.ToChar("'"))
                            .TrimEnd(Convert.ToChar("'")),
                    }).ToList();
                }
                return Upfiles;

            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        // public async Task GeneratedLinkRefresh(string Guid)
        public async Task GeneratedLinkRefresh(string Test)
        {
          LinkModel = await GetAllFilesAsync(Test);
        }
        public async Task GeneratedLinkRefresh()
        {
            LinkModel = await GetAllFilesAsync();
        }
        protected async override Task OnInitializedAsync()
        {
            LinkModel = await GetAllFilesAsync(CurrentFileGuid);
        }
      

    }
}
