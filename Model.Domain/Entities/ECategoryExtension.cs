using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public static class ECategoryExtension
    {
        public static EStatus CheckStatusByRules(this ECategory category, decimal value)
        {
            if (RejectAny().Any(x => x(value)))
                return EStatus.Rejected;

            if (ApproveAny().Any(x => x(value)))
                return EStatus.Approved;

            if(SpecificConditionsToApproveByCategory().TryGetValue(category, out var list))
                if (list.Any(x => x(value)))
                    return EStatus.Approved;
            
                
            return EStatus.UnderEvaluation;
        }
        private static List<Func<decimal, bool>> ApproveAny()
        {
            return new List<Func<decimal, bool>>
            {
                (value) => { return value <= 100; },
            };
        }

        private static List<Func<decimal, bool>> RejectAny()
        {
            return new List<Func<decimal, bool>>
            {
                (value) => { return value > 1000; },
            };
        }

        private static Dictionary<ECategory, List<Func<decimal, bool>>> SpecificConditionsToApproveByCategory()
        {
            return new Dictionary<ECategory, List<Func<decimal, bool>>>
            {
                { ECategory.Transportation, TransportationConditionsToApprove()},
                { ECategory.Food, FoodConditionsToApprove()},
            };
        }

        private static List<Func<decimal, bool>> FoodConditionsToApprove()
        {
            return new List<Func<decimal, bool>>
            {
                (value) => { return value > 100 && value <= 500; },
            };
        }

        private static List<Func<decimal, bool>> TransportationConditionsToApprove()
        {
            return new List<Func<decimal, bool>>
            {
                (value) => { return value > 100 && value <= 500; },
            };
        }

        
    }
}
