using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Core.Contract
{
   public interface IUploadUtilityHelperServices
    {
        Task<List<UploadedFile>> GetAllFilesAsync();
        Task<bool> InsertUploadedFileAsync(UploadedFile uploadedFile);
        Task<UploadedFile> GetUploadedFileAsync(int Id);
        Task<bool> UpdateUploadedFileAsync(UploadedFile uploadeFile);
        Task<bool> DeleteUploadedFileAsync(string FileName);
        Task InsertEtagModel(EtagModel Etagmodel);
        Task<List<EtagModel>> GetETageByID(string AWSID);
        Task DeleteEtagModel(string AwsID);
        Task DeleteListFile(DeleteUpdateModel model);
        Task UpdateUploadQuota(User CurrentUser, int updatequota);
        ShowQuotaViewModel GetQuota();
    }
}
