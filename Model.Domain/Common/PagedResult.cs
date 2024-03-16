using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Common
{
    public class PagedResult<T> where T : class
    {
        public required int TotalCount { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }
}
