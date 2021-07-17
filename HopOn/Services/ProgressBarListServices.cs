using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class ProgressBarListServices : IProgressBarListServices
    {
        #region Property  
        private readonly AppDBContext _appDBContext;
        //private readonly IFileHandler _fileHandler;
        #endregion

        #region Constructor  
        public ProgressBarListServices(AppDBContext appDBContext)
        {
            this._appDBContext = appDBContext;
           // this._fileHandler = fileHandler;
        }
        #endregion
        public async Task<bool> DeleteProgressFileAsync(string AWSID)
        {
            try
            {
                ProgressBarList RemoveModel = await _appDBContext.ProgressBarLists.Where(PbL => PbL.AwsId == AWSID).FirstOrDefaultAsync();
                if (RemoveModel != null)
                {
                    _appDBContext.Remove(RemoveModel);
                    await _appDBContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return true;
        }
        public async Task<List<ProgressBarList>> GetListAsync(FileStatus status, DateTime Date)
        {
            return await _appDBContext.ProgressBarLists.Where(s => s.Status == status).Distinct().ToListAsync();
        }
        public async Task DeleteEtags(string Awsid)
        {
            List<EtagModel> DeleteModel = await _appDBContext.ETags.Where(s => s.AmazonID == Awsid).ToListAsync();
            _appDBContext.RemoveRange(DeleteModel);
            _appDBContext.SaveChanges();
        }
        public async Task<List<ProgressBarList>> GetAllFilesAsync()
        {
            return await _appDBContext.ProgressBarLists.ToListAsync();
        }

        public Task<ProgressBarList> GetProgressFileAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProgressFileAsync(ProgressBarList ProgressFile)
        {
            throw new NotImplementedException();
        }

        public void InsertProgressFileAsync(ProgressBarList ProgressFile)
        {
            try
            {
                _appDBContext.ProgressBarLists.Add(ProgressFile);
                _appDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
