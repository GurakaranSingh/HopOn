using HopOn.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class UploadUtilityHelperServices : IUploadUtilityHelperServices
    {
        #region Property  
        private readonly AppDBContext _appDBContext;
        #endregion

        #region Constructor  
        public UploadUtilityHelperServices(AppDBContext appDBContext)
        {
            this._appDBContext = appDBContext;
        }

        #endregion


        #region Get List of Files  
        public async Task<List<UploadedFile>> GetAllFilesAsync()
        {
            return await _appDBContext.UploadedFiles.ToListAsync();
        }
        #endregion

        #region Insert Uploaded File  
        public async Task<bool> InsertUploadedFileAsync(UploadedFile uploadedFile)
        {
            await _appDBContext.UploadedFiles.AddAsync(uploadedFile);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Uploaded file by Id  
        public async Task<UploadedFile> GetUploadedFileAsync(int Id)
        {
            UploadedFile uploadedfile = await _appDBContext.UploadedFiles.FirstOrDefaultAsync(c => c.Id.Equals(Id));
            return uploadedfile;
        }
        #endregion

        #region Update Uploaded File  
        public async Task<bool> UpdateUploadedFileAsync(UploadedFile uploadeFile)
        {
            _appDBContext.UploadedFiles.Update(uploadeFile);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeleteFile
        public async Task<bool> DeleteUploadedFileAsync(UploadedFile uploadeFile)
        {
            _appDBContext.Remove(uploadeFile);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}
