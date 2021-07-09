using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HopOn.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HashValidateFilter : ActionFilterAttribute
    {
        public string _chunkData { get; set; }
        public string _clientHashKey{ get; set; }
        //public HashValidateFilter(string chuckData, string ClientHashKey)
        //{
        //    _chunkData = chuckData;
        //    _clientHashKey = ClientHashKey;
        //}
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CheckHash();
        }
        public async Task<bool> CheckHash()
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                bool flag = false;
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(_chunkData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string ServerHashKey = builder.ToString();
                if (_clientHashKey == ServerHashKey)
                {
                    flag = true;
                }
                return flag;
            }
        }
    }
}
