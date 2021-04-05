using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer.Models.Server
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
        public string name { get; set; }
        /// <summary>
        /// The description of order
        /// </summary>
        /// <example>""</example>
        public string? desc { get; set; }
    }
}
