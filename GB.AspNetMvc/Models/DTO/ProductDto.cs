using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace GB.AspNetMvc.Models.DTO
{
    public class ProductDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Category Category { get; set; }
    }
}
