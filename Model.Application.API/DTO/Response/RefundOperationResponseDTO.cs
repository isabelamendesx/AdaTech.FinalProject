using System.Text.Json.Serialization;

namespace Model.Application.API.DTO.Response
{
    public class RefundOperationResponseDTO
    {
        public string Date { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RuleResponseDTO? Rule { get; set; }
        public string ApprovedBy {  get; set; } 
    }
}
