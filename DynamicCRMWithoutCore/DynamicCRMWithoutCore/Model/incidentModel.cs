using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRMWithoutCore.Model
{
    class incidentModel
    {
        public string incidentName { get; set; }

        public Guid incidentId { get; set; }

        public Guid incidentOwner { get; set; }

    }
}
