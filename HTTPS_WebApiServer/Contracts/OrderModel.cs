using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTTPS_WebApiServer.Contracts
{
    [JsonObject, Serializable]
    public class OrderModel
    {
        public string name { get; set; }
        public string desc { get; set; }
    }
}
