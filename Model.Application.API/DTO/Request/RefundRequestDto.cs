using Model.Application.DTO.Validators;
using System.ComponentModel.DataAnnotations;


namespace Model.Application.API.DTO.Request
{
    public class RefundRequestDto
    {
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Category is required.")]
        public uint CategoryId { get; set; }


        [Required(ErrorMessage = "Total is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Total must be a positive value")]
        public decimal Total { get; set; }


        [Required(ErrorMessage = "Status is required.")]
        [ValidateSubmittedStatus]
        public string Status { get; set; }
    }
}
