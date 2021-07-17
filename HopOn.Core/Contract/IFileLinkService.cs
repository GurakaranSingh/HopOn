using HopOn.Model.Model;
using HopOn.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Core.Contract
{
  public  interface IFileLinkService
    {
        void GenrateFileLink(string FileId, LinkType _type);
    }
}
