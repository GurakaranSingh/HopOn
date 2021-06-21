using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteProgressFileAsync(ProgressBarList ProgressFile)
        {
            _appDBContext.Remove(ProgressFile);
            await _appDBContext.SaveChangesAsync();
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

        public async Task<bool> InsertProgressFileAsync(ProgressBarList ProgressFile)
        {
            await _appDBContext.ProgressBarLists.AddAsync(ProgressFile);
            await _appDBContext.SaveChangesAsync();
            return true;
        }

        public Task<bool> UpdateProgressFileAsync(ProgressBarList ProgressFile)
        {
            throw new NotImplementedException();
        }



        #endregion




    }
}
