namespace Model.Application.API.DTO.Response
{
    public class DeactivateRuleResponseDTO
    {
        public IEnumerable<uint> DeactivatedRulesId { get; set; }
        public DeactivateRuleResponseDTO()
        {
            DeactivatedRulesId = new List<uint>();
        }

    }
}
