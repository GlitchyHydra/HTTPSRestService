﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer.Models.Server
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
