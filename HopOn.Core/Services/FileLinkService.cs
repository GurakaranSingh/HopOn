using HopOn.Core.Contract;
using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Core.Services
{
    public class FileLinkService : IFileLinkService
    {
        private readonly AppDBContext _appDBContext;
        public FileLinkService(AppDBContext appDBContext)
        {
            this._appDBContext = appDBContext;
        }
        public void GenrateFileLink(string FileId,LinkType _type)
        {
            try
            {
                GenratedLink DBModel = new GenratedLink()
                {
                    Expire = false,
                    FileId = FileId.TrimStart(Convert.ToChar("'"))
                            .TrimEnd(Convert.ToChar("'")),
                    Token = _type == LinkType.Token ? GenerateToken() : string.Empty,
                    Type = _type,
                    FileLink = "",
                    Guid = Guid.NewGuid().ToString()
                };
                _appDBContext.GeneratedLinks.Add(DBModel);
                _appDBContext.SaveChanges();
                DBModel.FileLink = GenerateFileLink(DBModel.Guid);
                _appDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GenerateToken()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }
        private string GenerateFileLink(string Id)
        {
            return "https://localhost:44341/Link/" + Id;
        }
    }
}
