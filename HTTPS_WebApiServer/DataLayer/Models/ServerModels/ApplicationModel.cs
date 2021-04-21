using System;
using Newtonsoft.Json;

namespace FreelancerWeb.DataLayer.Models.Server
{
    [JsonObject, Serializable]
    public class ApplicationModel
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
