using Model.Domain.Entities;
using System.ComponentModel;

namespace Model.Application.API.DTO.Response
{
    public class RefundOperationResponseDTO
    {
        public uint OperationId { get; set; }
        public string Date { get; set; }       
        public RuleResponseDTO? Rule { get; set; }
        public string ApprovedBy {  get; set; } 
    }
}
