using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Request
{
    public class CategoryRequestDTO
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name must be at most 50 characters.")]
        public string Name { get; set; }
    }
}
