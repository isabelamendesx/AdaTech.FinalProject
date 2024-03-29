﻿using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Model.Application.API.DTO.Response;

public class RefundResponseDTO
{
    public uint RefundId { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Value { get; set; }
    public string Status { get; set; }
    public string OwnerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<RefundOperationResponseDTO>? Operations { get; set; }

}