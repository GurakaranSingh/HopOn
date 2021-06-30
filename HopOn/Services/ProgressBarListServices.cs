using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
{
    public class ProgressBarListServices : IProgressBarListServices
    {
        #region Property  
        private readonly AppDBContext _appDBContext;
        #endregion

        #region Constructor  
        public ProgressBarListServices(AppDBContext appDBContext)
        {
            this._appDBContext = appDBContext;
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
