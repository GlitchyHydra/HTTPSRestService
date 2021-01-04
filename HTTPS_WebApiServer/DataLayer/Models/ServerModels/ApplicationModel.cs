using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer.Models.Server
{
    [JsonObject, Serializable]
    public class ApplicationModel
    {
        public string name { get; set; }
        public string? desc { get; set; }
    }
}
