using InvoicePro.Application.Interfaces;
using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using InvoicePro.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicePro.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<OrderStatus> _orderStatusRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerType> _customerTypeRepository;
        private readonly IRepository<InvoiceNumber> _invoiceNumberRepository;

        public OrderService(IRepository<Order> repository, IRepository<Customer> customerRepository, IRepository<OrderStatus> orderStatusRepository, IRepository<CustomerType> customerTypeRepository, IRepository<InvoiceNumber> invoiceNumberRepository)
        {
            _repository = repository;
            _customerRepository = customerRepository;
            _orderStatusRepository = orderStatusRepository;
            _customerTypeRepository = customerTypeRepository;
            _invoiceNumberRepository = invoiceNumberRepository;
        }

        public IEnumerable<Order> GetAll()
        {
            var orders = _repository.GetAll().ToList();

            foreach(var order in orders)
            {
                order.CustomerId = GetCustomerNameById(order.CustomerId);
                order.OrderStatus = GetOrderStatusById(order.OrderStatus);
            }

            return orders.AsEnumerable();
        }

        private string GetOrderStatusById(string orderStatus)
        {
            return _orderStatusRepository.GetConditional(_ => _.Id == orderStatus).Name;
        }

        private string GetCustomerNameById(string customerId)
        {
            var customer = _customerRepository.GetConditional(_ => _.Id == customerId);

            if(customer == null)
            {
                return "";
            }
            else
            {
                return customer.FirstName + " " + customer.LastName;
            }
        }

        public IEnumerable<OrderStatus> GetAllOrderStatus()
        {
            return _orderStatusRepository.GetAll().AsEnumerable();
        }

        public PayloadResponse CreateOrder(Order model)
        {
            try
            {
                model.Id = GenerateOrderId();
                _repository.Insert(model);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Order Creation",
                    Content = null,
                    Message = "Order Creation has been successfull"
                };
            }
            catch(Exception ex)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Order Creation",
                    Content = null,
                    Message = $"Order Creation become unsuccessfull! Error:-{ex.Message} "
                };
            }
        }

        private string GenerateOrderId()
        {
            var date = DateTime.Now;

            return "ODR-" +
                date.Year.ToString() + "-" +
                date.Month.ToString() + "-" +
                date.Day.ToString() + "-" +
                date.Hour.ToString() + "-" +
                date.Minute.ToString() + "-" +
                date.Second.ToString() + "-" +
                date.Millisecond.ToString();
        }

        public Order GetById(string id)
        {
            return _repository.GetConditional(_ => _.Id == id);
        }

        public PayloadResponse Update(string id, Order model)
        {
            var entity = _repository.GetConditional(_ => _.Id == id);

            try
            {
                entity.CustomerId = model.CustomerId;
                entity.OrderStatus = model.OrderStatus;
                entity.Products = model.Products;
                entity.Amount = model.Amount;
                entity.DueAmount = model.Amount - entity.PaidAmount;

                _repository.Update(entity);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Order Update",
                    Content = null,
                    Message = "Order Update successfull"
                };
            }
            catch(Exception ex)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Order Update",
                    Content = null,
                    Message = $"Order Update become unsuccessfull! Error:-{ex.Message}"
                };
            }
        }

        public InvoiceVm GetInvoiceDataById(string id)
        {
            var order = _repository.GetConditional(_ => _.Id == id);

            var customer = _customerRepository.GetConditional(_ => _.Id == order.CustomerId);

            return new InvoiceVm()
            {
                InvoiceNumber = GetInvoiceNumber(),
                InvoiceDate = DateTime.Now,
                DueDate = order.LastModifiedTime,
                CustomerName = customer.FirstName + " " + customer.LastName,
                CustomerType = _customerTypeRepository.GetConditional(_ => _.Id == customer.CustomerType).Name,
                Address = customer.Street1 + "," + customer.City + "," + customer.State + "," + customer.Zip,
                Email = customer.Email,
                Mobile = customer.MainPhone,
                PaymentStatus = order.PaymentStatus,
                Total = order.Amount,
                PaidAmount = order.PaidAmount,
                DueAmount = order.DueAmount,
                ProductList = JsonConvert.DeserializeObject<List<ProductOfInvoiceVm>>(order.Products)
            };
        }

        private string GetInvoiceNumber()
        {
            var number = _invoiceNumberRepository.GetAll().FirstOrDefault();

            number.Number = number.Number + 1;

            _invoiceNumberRepository.Update(number);
            _invoiceNumberRepository.SaveChanges();

            var newNumber = number.Number;

            if(newNumber > 999)
            {
                return newNumber.ToString();
            }
            else
            {
                var newNumberStr = newNumber.ToString();
                var newInvoiceNumber = string.Empty;

                for(var i = newNumberStr.Length; i < 4; i++)
                {
                    newInvoiceNumber += "0";
                }

                newInvoiceNumber += newNumberStr;

                return newInvoiceNumber;
            }
        }
    }
}
