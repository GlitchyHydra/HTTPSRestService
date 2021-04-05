using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer.Models.Server
{
    [JsonObject, Serializable]
    public class ResponseLogin
    {
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
