using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Domain.Entities
{
    public class Order : Entity
    {
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
        public string Products { get; set; }
        public string PaymentStatus { get; set; }
        public Decimal Amount { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal DueAmount { get; set; }
    }
}
