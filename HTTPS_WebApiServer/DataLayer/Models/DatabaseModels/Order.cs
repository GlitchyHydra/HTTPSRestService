using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLayer.Models.Database
{
    public enum OrderStatus
    {
        Open       = 0,
        Processing = 1,
        Close      = 2
    }
    [Table("orders")]
    public class Order
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("order_date")]
        public DateTime OrderDate { get; set; }
        [Column("status")]
        public OrderStatus Status { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("desc")]
        public string Desc { get; set; }

        public List<Application> Applications { get; set; }
    }
}
