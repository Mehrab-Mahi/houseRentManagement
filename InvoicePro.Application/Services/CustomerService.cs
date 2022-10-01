using AutoMapper;
using InvoicePro.Application.Interfaces;
using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using InvoicePro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<CustomerType> _customerTypeRepository;

        public CustomerService(IRepository<Customer> repository, IRepository<CustomerType> customerTypeRepository)
        {
            _repository = repository;
            _customerTypeRepository = customerTypeRepository;
        }

        public IEnumerable<Customer> GetAll()
        {
            try
            {
                var customerTypes = _customerTypeRepository.GetAll().ToDictionary(_ => _.Id, _ => _.Name);
                var list = _repository.GetAll().ToList();
                list = AssignCustomerTypeData(list, customerTypes);
                return list.AsEnumerable();
            }
            catch (Exception e)
            {
                var list = new List<Customer>();
                return list.AsEnumerable();
            }
        }

        private List<Customer> AssignCustomerTypeData(List<Customer> list, Dictionary<string, string> customerTypes)
        {
            foreach(var customer in list)
            {
                customer.CustomerType = customerTypes[customer.CustomerType];
            }
            return list;
        }

        public Customer GetById(string id)
        {
            return _repository.GetConditional(_ => _.Id == id);
        }

        public PayloadResponse Create(CustomerVm model)
        {
            try
            {
                var entity = new Customer();

                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.CustomerType = model.CustomerType;
                entity.MainPhone = model.MainPhone;
                entity.AlternativePhone = model.AlternativePhone;
                entity.PrimaryContact = model.PrimaryContact;
                entity.SecondaryContact = model.SecondaryContact;
                entity.Email = model.Email;
                entity.Company = model.Company;
                entity.Street1 = model.Street1;
                entity.Street2 = model.Street2;
                entity.City = model.City;
                entity.State = model.State;
                entity.Zip = model.Zip;

                _repository.Insert(entity);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Customer Creation",
                    Content = null,
                    Message = "Customer Creation has been successfull"
                };
            }
            catch (Exception e)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Customer Creation",
                    Content = null,
                    Message = $"Customer Creation become unsuccessfull! Error:-{e.Message} "
                };
            }
        }

        public PayloadResponse Update(string id, CustomerVm model)
        {
            var entity = _repository.GetConditional(_ => _.Id == id);
            try
            {
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.CustomerType = model.CustomerType;
                entity.MainPhone = model.MainPhone;
                entity.AlternativePhone = model.AlternativePhone;
                entity.PrimaryContact = model.PrimaryContact;
                entity.SecondaryContact = model.SecondaryContact;
                entity.Email = model.Email;
                entity.Company = model.Company;
                entity.Street1 = model.Street1;
                entity.Street2 = model.Street2;
                entity.City = model.City;
                entity.State = model.State;
                entity.Zip = model.Zip;

                _repository.Update(entity);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Customer Update",
                    Content = null,
                    Message = "Customer Update successfull"
                };
            }
            catch (Exception ex)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Customer Update",
                    Content = null,
                    Message = $"Customer Update become unsuccessfull! Error:-{ex.Message}"
                };
            }
        }
    }
}
