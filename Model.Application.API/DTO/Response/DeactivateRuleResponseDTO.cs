namespace Model.Application.API.DTO.Response
{
    public class DeactivateRuleResponseDTO
    {
        public List<uint> DeactivatedRulesId { get; set; }
        public DeactivateRuleResponseDTO()
        {
            DeactivatedRulesId = new List<uint>();
        }

    }
}
