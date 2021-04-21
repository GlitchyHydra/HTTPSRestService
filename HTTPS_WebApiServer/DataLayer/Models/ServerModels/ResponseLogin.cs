using System;
using Newtonsoft.Json;

namespace FreelancerWeb.DataLayer.Models.Server
{
    [JsonObject, Serializable]
    public class ResponseLogin
    {
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
