using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.ViewModels
{
    public class InvoiceVm
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Reference { get; set; }
        public DateTime DueDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public List<ProductOfInvoiceVm> ProductList { get; set; }
    }
}
