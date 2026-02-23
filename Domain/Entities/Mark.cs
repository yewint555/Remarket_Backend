using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Mark : BaseEntity<Guid>
    {
        // Foreign Keys
        public Guid UserId { get; set; }
        public virtual Users User { get; set; } = default!;
        public Guid PostId { get; set; }
        public virtual Posts Post { get; set; } = default!;

        
    }
}