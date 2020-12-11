using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLayer.Models
{
    public enum OrderStatus
    {
        OPEN = 0,
        IN_WORK = 1,
        CLOSE = 2
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
    }
}
