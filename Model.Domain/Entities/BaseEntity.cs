using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Entities
{
    public abstract class BaseEntity
    {
        public virtual uint Id {  get; set; } 
    }
}
