using System;
using Newtonsoft.Json;

namespace FreelancerWeb.DataLayer.Models.Server
{
    /// <summary>
    /// Response from server to PUT and POST requests
    /// </summary>
    [JsonObject, Serializable]
    public class FreelancerResponse
    {
        /// <summary>
        /// Description of reponse result
        /// </summary>
        public string Message { get; set; }
    }
}
