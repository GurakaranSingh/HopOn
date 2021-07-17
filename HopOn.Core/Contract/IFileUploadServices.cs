using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HopOn.Core.Contract
{
   public interface IFileUploadServices
    {
        Task<int> GetUploadID();
    }
}
