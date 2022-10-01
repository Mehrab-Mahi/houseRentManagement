using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Domain.Entities
{
    public class Product : Entity
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductType { get; set; }
        public string Vendor { get; set; }
        public int? Available { get; set; }
        public decimal? AverageCosting { get; set; }
        public decimal? SalesPrice { get; set; }
        public string ImagePath { get; set; }
    }
}
