using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_manupulation.Data.Models
{

    [Table("Product")]
     public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? ProductName { get; set; }
        [Required]
        [MaxLength(255)]
        public string? ProductImage { get; set; }
    }
}
