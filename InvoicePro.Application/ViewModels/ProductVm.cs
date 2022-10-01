using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.ViewModels
{
    public class ProductVm
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductType { get; set; }
        public string Vendor { get; set; }
        public int? Available { get; set; }
        public int? OnSalesOrder { get; set; } = 0;
        public decimal? AverageCosting { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? SalesPerWeek { get; set; } = 0;
        public string ImagePath { get; set; }
    }
}
