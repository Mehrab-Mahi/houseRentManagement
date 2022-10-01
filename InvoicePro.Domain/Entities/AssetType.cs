using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Domain.Entities
{
    public class AssetType : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
