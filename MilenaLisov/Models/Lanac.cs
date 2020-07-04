using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MilenaLisov.Models
{
    public class Lanac
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(75)]
        public string Naziv { get; set; }
        [Required]
        [Range(1851, 2009)]
        public int Godina { get; set; }
    }
}