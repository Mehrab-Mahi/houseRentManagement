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
    public class CustomerTypeService : ICustomerTypeService
    {
        private readonly IRepository<CustomerType> _repository;
        private readonly IMapper _mapper;

        public CustomerTypeService(IRepository<CustomerType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<CustomerType> GetAll()
        {
            try
            {
                var test = _repository.GetAll().ToList();
                return test.AsEnumerable();
            }
            catch(Exception e)
            {
                var list = new List<CustomerType>();
                return list.AsEnumerable();
            }
        }

        public CustomerType GetById(string id)
        {
            return _repository.GetConditional(_ => _.Id == id);
        }

        public PayloadResponse Create(CustomerType model)
        {
            try
            {
                var entity = new CustomerType();
                entity = _mapper.Map(model, entity);
                if (string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = Guid.NewGuid().ToString("N");
                }

                _repository.Insert(entity);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Customer Type Creation",
                    Content = null,
                    Message = "Customer Type Creation has been successfull"
                };
            }
            catch(Exception e)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Customer Type Creation",
                    Content = null,
                    Message = $"Customer Type Creation become unsuccessfull! Error:-{e.Message} "
                };
            }
        }

        public PayloadResponse Update(string id, CustomerType model)
        {
            var entity = _repository.GetConditional(_ => _.Id == id);
            try
            {
                entity.Name = model.Name;
                entity.Description = model.Description;

                _repository.Update(entity);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Customer Type Update",
                    Content = null,
                    Message = "Customer Type Update successfull"
                };
            }
            catch (Exception e)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Customer Type Update",
                    Content = null,
                    Message = $"Customer Type Update become unsuccessfull! Error:-{e.Message}"
                };
            }
        }
    }
}
