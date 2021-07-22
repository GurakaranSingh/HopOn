using HopOn.Model;
using HopOn.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HopOn.Core.Contract;
using HopOn.Model.Model;

namespace HopOn.Core.Services
{
    public class UploadUtilityHelperServices : IUploadUtilityHelperServices
    {
        #region Property  
        private readonly AppDBContext _appDBContext;
        public IConfiguration _configuration { get; }
        public string _connectionString { get; set; }
        #endregion

        #region Constructor  
        public UploadUtilityHelperServices(AppDBContext appDBContext, IConfiguration Configuration)
        {
            this._appDBContext = appDBContext;
            this._configuration = Configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
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
        public async Task<bool> DeleteUploadedFileAsync(string Guid)
        {
            try
            {
                UploadedFile RemoveModel = await _appDBContext.UploadedFiles.Where(up => up.Guid == Guid).FirstOrDefaultAsync();
                List<GenratedLink> RemoveLinksList = await _appDBContext.GeneratedLinks.Where(gl => gl.FileId == Guid).ToListAsync();
                if (RemoveModel != null)
                {
                    _appDBContext.UploadedFiles.Remove(RemoveModel);
                    await _appDBContext.SaveChangesAsync();

                    if (RemoveLinksList.Count > 0)
                    {
                        _appDBContext.GeneratedLinks.RemoveRange(RemoveLinksList);
                        await _appDBContext.SaveChangesAsync();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        public async Task DeleteListFile(DeleteUpdateModel model)
        {
            List<UploadedFile> DeleteModel = await _appDBContext.UploadedFiles.Where(s => model.Ids.Contains(s.Guid)).ToListAsync();
            List<GenratedLink> LinkModel = await _appDBContext.GeneratedLinks.Where(s => model.Ids.Contains(s.FileId)).ToListAsync();
            if (DeleteModel.Count > 0)
            {
                _appDBContext.UploadedFiles.RemoveRange(DeleteModel);
                if (LinkModel.Count > 0)
                {
                    _appDBContext.GeneratedLinks.RemoveRange(LinkModel);
                }
                await _appDBContext.SaveChangesAsync();
            }
        }
        #region ETag
        public async Task InsertEtagModel(EtagModel Etagmodel)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO etags(PartNumber,ETag,AmazonID) VALUES (@PartNumber,@ETag, @AmazonID)";

                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PartNumber", Etagmodel.PartNumber);
                        command.Parameters.AddWithValue("@ETag", Etagmodel.ETag);
                        command.Parameters.AddWithValue("@AmazonID", Etagmodel.AmazonID);
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                //using (SqlConnection connection = new SqlConnection(_connectionString))
                //{
                //    connection.Open();
                //    string query = "INSERT INTO etags(PartNumber,ETag,AmazonID) VALUES (@PartNumber,@ETag, @AmazonID)";

                //    using (var command = new SqlCommand(query, connection))
                //    {
                //        command.Parameters.AddWithValue("@PartNumber",Etagmodel.PartNumber);
                //        command.Parameters.AddWithValue("@ETag", Etagmodel.ETag);
                //        command.Parameters.AddWithValue("@AmazonID", Etagmodel.AmazonID);
                //        command.ExecuteNonQuery();
                //    }
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ShowQuotaViewModel GetQuota()
        {
            User UserModel = _appDBContext.Users.Where(u => u.Id == 1).FirstOrDefault();
            ShowQuotaViewModel model = new ShowQuotaViewModel();
            model.Download_Quota = UserModel.MaxDownload;
            model.Remaining_Download_Quota = UserModel.DownloadQuota;
            model.Upload_Quota = UserModel.MaxUpload;
            model.Remaining_Upload_Quota = UserModel.UploadQuota;
            model.Storage_Quota = ((UserModel.StorageQuota / 1024) / 1024);
            model.RemainiuploadPercentage = Convert.ToDecimal((float)System.Math.Round(model.Remaining_Upload_Quota / model.Upload_Quota * 100, 2));
            model.RemainingDownloadPercentage = Convert.ToDecimal((float)System.Math.Round(model.Remaining_Download_Quota / model.Download_Quota * 100, 2));
            model.RDQ = ((model.Remaining_Download_Quota / 1024) / 1024);
            model.RUQ = ((model.Remaining_Upload_Quota / 1024) / 1024);
            model.RemainingDownloadQuota = model.RDQ < 1024 ? Convert.ToDecimal((float)System.Math.Round(model.RDQ, 2)) + "MB" : Convert.ToString((float)System.Math.Round(model.RDQ / 1024, 0)) + "GB";
            model.RemainingUploadQuota = model.RUQ < 1024 ? Convert.ToString((float)System.Math.Round(model.RUQ, 2)) + "MB" : Convert.ToString((float)System.Math.Round(model.RUQ / 1024, 0)) + "GB";
            model.Upload_Quota = ((model.Upload_Quota / 1024) / 1024);
            model.Download_Quota = ((model.Download_Quota / 1024) / 1024);
            model.UploadQuota = model.Upload_Quota < 1024 ? Convert.ToString((float)System.Math.Round(model.Upload_Quota)) + "MB" : Convert.ToString((float)System.Math.Round(model.Upload_Quota / 1024, 0)) + "GB";
            model.DownloadQuota = model.Download_Quota < 1024 ? Convert.ToString((float)System.Math.Round(model.Download_Quota)) + "MB" : Convert.ToString((float)System.Math.Round(model.Download_Quota / 1024, 0)) + "GB";
            model.StorageQuota = model.Storage_Quota < 1024 ? Convert.ToString((float)System.Math.Round(model.Storage_Quota)) + "MB" : Convert.ToString((float)System.Math.Round(model.Storage_Quota / 1024, 0)) + "GB";
            return model;
        }
        public async Task UpdateUploadQuota(User CurrentUser, int updatequota)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    
                    string query = "update users set UploadQuota = (UploadQuota - @UploadQuota) where Id = @ID";

                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@UploadQuota", updatequota);
                        command.Parameters.AddWithValue("@ID", CurrentUser.Id);
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                //using (SqlConnection connection = new SqlConnection(_connectionString))
                //{
                //    connection.Open();
                //    string query = "INSERT INTO etags(PartNumber,ETag,AmazonID) VALUES (@PartNumber,@ETag, @AmazonID)";

                //    using (var command = new SqlCommand(query, connection))
                //    {
                //        command.Parameters.AddWithValue("@PartNumber",Etagmodel.PartNumber);
                //        command.Parameters.AddWithValue("@ETag", Etagmodel.ETag);
                //        command.Parameters.AddWithValue("@AmazonID", Etagmodel.AmazonID);
                //        command.ExecuteNonQuery();
                //    }
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task DeleteEtagModel(string AwsID)
        {
            try
            {
                List<EtagModel> ETagList = await _appDBContext.ETags.Where(s => s.AmazonID == AwsID).ToListAsync();
                _appDBContext.RemoveRange(ETagList);
                await _appDBContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<EtagModel>> GetETageByID(string AWSID)
        {
            return await _appDBContext.ETags.Where(sp => sp.AmazonID == AWSID).ToListAsync();
        }
        #endregion
    }
}
