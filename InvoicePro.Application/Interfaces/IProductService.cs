using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductVm> GetAll();
        Product GetById(string id);
        PayloadResponse Create(Product model);
        PayloadResponse Update(string id, Product model);
    }
}
