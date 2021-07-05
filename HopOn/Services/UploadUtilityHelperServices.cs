using HopOn.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace HopOn.Services
{
    public class UploadUtilityHelperServices : IUploadUtilityHelperServices
    {
        #region Property  
        private readonly AppDBContext _appDBContext;
        public IConfiguration _configuration { get; }
        public string _connectionString{get;set;}
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
                if (RemoveModel != null)
                {
                    _appDBContext.Remove(RemoveModel);
                    await _appDBContext.SaveChangesAsync();

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

        #region ETag
        public async Task  InsertEtagModel(EtagModel Etagmodel)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO etags(PartNumber,ETag,AmazonID) VALUES (@PartNumber,@ETag, @AmazonID)";
                    
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PartNumber",Etagmodel.PartNumber);
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
