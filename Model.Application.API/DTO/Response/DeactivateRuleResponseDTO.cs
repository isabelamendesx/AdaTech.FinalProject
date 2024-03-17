namespace Model.Application.API.DTO.Response
{
    public class DeactivateRuleResponseDTO
    {
        public IEnumerable<uint> DeactivatedRules { get; set; }
        public DeactivateRuleResponseDTO()
        {
            DeactivatedRules = new List<uint>();
        }

    }
}
