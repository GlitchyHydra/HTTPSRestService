using System;
using Newtonsoft.Json;

namespace FreelancerWeb.DataLayer.Models.Server
{
    /// <summary>
    /// 
    /// </summary>
    [JsonObject, Serializable]
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
