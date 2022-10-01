using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();
        IEnumerable<OrderStatus> GetAllOrderStatus();
        PayloadResponse CreateOrder(Order model);
        Order GetById(string id);
        PayloadResponse Update(string id, Order model);
        InvoiceVm GetInvoiceDataById(string id);
    }
}
