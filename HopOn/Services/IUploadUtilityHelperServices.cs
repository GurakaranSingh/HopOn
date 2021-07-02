using HopOn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Services
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
    }
}
