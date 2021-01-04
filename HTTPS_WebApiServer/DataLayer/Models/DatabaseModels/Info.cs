using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Database
{
    [Table("info")]
    public class Info
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("second_name")]
        public string SecondName { get; set; }
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
        [Column("additional")]
        public string Additional { get; set; }
    }
}
