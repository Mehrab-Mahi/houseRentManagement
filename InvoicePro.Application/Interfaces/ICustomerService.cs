using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(string id);
        PayloadResponse Create(CustomerVm model);
        PayloadResponse Update(string id, CustomerVm model);
    }
}
