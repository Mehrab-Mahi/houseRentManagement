using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.ViewModels
{
    public class CustomerVm
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string MainPhone { get; set; }
        public string AlternativePhone { get; set; }
        public string PrimaryContact { get; set; }
        public string SecondaryContact { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
