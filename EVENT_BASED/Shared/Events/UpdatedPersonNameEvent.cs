using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class UpdatedPersonNameEvent
    {
        public string PersonId { get; set; }
        public string NewName { get; set; }
    }
}
