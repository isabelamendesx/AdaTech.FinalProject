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

        public static ECategory ParseCategory(string category)
        {
            var CategoryMap = new Dictionary<string, ECategory>(StringComparer.OrdinalIgnoreCase)
            {
                    { "accomodation", ECategory.Accomodation },
                    { "transportation", ECategory.Transportation },
                    { "travel", ECategory.Travel },
                    { "food", ECategory.Food },
                    { "others", ECategory.Others }
            };

            CategoryMap.TryGetValue(category, out var parsedCategory);

            return parsedCategory;
        }


    }
}
