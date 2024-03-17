namespace Model.Application.API.DTO.Response
{
    public class RuleResponseDTO
    {
        public string CategoryName { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public string Action {  get; set; }
        public bool IsActived { get; set; } 

    }
}
