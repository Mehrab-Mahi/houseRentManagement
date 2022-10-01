using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface ICustomerTypeService
    {
        IEnumerable<CustomerType> GetAll();
        CustomerType GetById(string id);
        PayloadResponse Create(CustomerType model);
        PayloadResponse Update(string id, CustomerType model);
    }
}
