using System;
using Newtonsoft.Json;

namespace FreelancerWeb.DataLayer.Models.Server
{
    /// <summary>
    /// The order to insert
    /// </summary>
    [JsonObject, Serializable]
    public class OrderModel
    {
        /// <summary>
        /// The title of order
        /// </summary>
        /// <example>"example order"</example>
        public string Name { get; set; }
        /// <summary>
        /// The description of order
        /// </summary>
        /// <example>""</example>
        public string Desc { get; set; }
    }
}
