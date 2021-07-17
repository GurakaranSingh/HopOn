using HopOn.Core.Contract;
using HopOn.Model.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Pages.Download
{
    public class DownloadFileBaseClass : ComponentBase
    {
        public bool HasToken { get; set; }
        public bool InvalidToken { get; set; }
        public bool HasLinkExpire { get; set; }
        [Parameter]
        public string Token { get; set; }
        [Parameter]
        public string FileId { get; set; }
        [Inject]
        private IFileHandler _fileHandler { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        GenratedLink Model;
        public async Task GetModel()
        {
            Model = _fileHandler.GetGeneratedLinkFile(FileId);
            if (!Model.Expire)
            {
                if (!string.IsNullOrEmpty(Model.Token))
                {
                    HasToken = true;
                }
                else
                {
                    var response = await _fileHandler.Download(Model.FileId);
                    await DownloadFile(Model.FileId, response.FileDownloadName);
                }
            }
            else
            {
                HasLinkExpire = true;
            }
        }
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
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
        public async Task ValidateToken()
        {
            if (Token == Model.Token)
            {
                var response = await _fileHandler.Download(Model.FileId);
                await DownloadFile(Model.FileId, response.FileDownloadName);
            }
            else
            {
                InvalidToken = true;
            }
        }
        //public async Task<FileStreamResult> DownloadFile()
        //{
        //    return await _fileHandler.Download(Model.FileId);
        //}
        protected override Task OnInitializedAsync()
        {
            GetModel();
            return base.OnInitializedAsync();
        }
        public void onChange(ChangeEventArgs args)
        {
            Token = (string)args.Value;
        }
    }
}
