using Model.Application.DTO.Validators;
using System.ComponentModel.DataAnnotations;


namespace Model.Application.DTO
{
    public class RefundRequestDto
    {
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set;}


        [Required(ErrorMessage = "Total is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Total must be a positive value" )]
        public decimal Total { get; set; }


        [Required(ErrorMessage = "Status is required.")]
        [ValidateStatus]
        public string Status { get; set; }
    }
}
