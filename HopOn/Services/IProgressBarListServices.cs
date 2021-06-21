using HopOn.Model;
using HopOn.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
{
   public interface IProgressBarListServices
    {
        Task<List<ProgressBarList>> GetAllFilesAsync();
        Task<bool> InsertProgressFileAsync(ProgressBarList ProgressFile);
        Task<ProgressBarList> GetProgressFileAsync(int Id);
        Task<bool> UpdateProgressFileAsync(ProgressBarList ProgressFile);
        Task<bool> DeleteProgressFileAsync(ProgressBarList ProgressFile);
    }
}
