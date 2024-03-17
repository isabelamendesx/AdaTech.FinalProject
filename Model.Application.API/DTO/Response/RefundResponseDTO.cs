using Model.Domain.Entities;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Model.Application.API.DTO.Response;

public class RefundResponseDTO
{
    public uint RefundId { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Value { get; set; }
    public string Status { get; set; }
    public string OwnerId { get; set; }
    public IEnumerable<RefundOperationResponseDTO>? Operations { get; set; }

}