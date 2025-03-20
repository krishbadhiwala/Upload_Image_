using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Image_manupulation.Data.Models.Dtos
{
    public class Insert_ProductDto
    {
        [Required]
        [MaxLength(30)]
        public string? ProductName { get; set; }
        [Required]
        public IFormFile? ImageFile { get; set; }
    }
}
