using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace GB.AspNetMvc.Models.DTO
{
    public class ProductDto
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name ="Наименование товара")]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Категория товара")]
        public Category Category { get; set; }
    }
}
