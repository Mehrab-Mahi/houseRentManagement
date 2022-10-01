using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface IProductTypeService
    {
        IEnumerable<ProductType> GetAll();
        ProductType GetById(string id);
        PayloadResponse Create(ProductType model);
        PayloadResponse Update(string id, ProductType model);
    }
}
