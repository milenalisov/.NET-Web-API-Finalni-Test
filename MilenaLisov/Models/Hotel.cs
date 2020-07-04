using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MilenaLisov.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(80)]
        public string Naziv { get; set; }
        [Required]
        [Range(1950,2020)]
        public int Godina { get; set; }
        [Required]
        [Range(2, int.MaxValue)]
        public int Zaposleni { get; set; }
        [Range(10, 999)]
        public int? Sobe { get; set; }

        public int LanacId { get; set; }
        public Lanac Lanac { get; set; }
    }
}