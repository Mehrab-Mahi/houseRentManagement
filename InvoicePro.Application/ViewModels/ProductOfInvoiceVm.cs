using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.ViewModels
{
    public class ProductOfInvoiceVm
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductType { get; set; }
        public string Product { get; set; }
        public decimal Unit { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
