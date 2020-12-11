using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTTPS_WebApiServer.Contracts
{
    [JsonObject, Serializable]
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
