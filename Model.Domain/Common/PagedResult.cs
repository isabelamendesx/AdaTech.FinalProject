using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Common
{
    public class PagedResult<T> where T : class
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
