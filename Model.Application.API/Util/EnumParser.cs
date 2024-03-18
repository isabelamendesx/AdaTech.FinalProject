using Model.Domain.Entities;

namespace Model.Application.API.Util
{
    public static class EnumParser
    {      
        public static EStatus ParseStatus(string status)
        {
            var StatusMap = new Dictionary<string, EStatus>(StringComparer.OrdinalIgnoreCase)
            {
                    { "submitted", EStatus.Submitted },
                    { "approved", EStatus.Approved },
                    { "rejected", EStatus.Rejected },
                    { "underevaluation", EStatus.UnderEvaluation },
                    { "paid", EStatus.Paid }
            };

            StatusMap.TryGetValue(status, out var parsedStatus);

            return parsedStatus;
        }



    }
}
