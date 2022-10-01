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
    public class ProductTypeService : IProductTypeService
    {
        private readonly IRepository<ProductType> _repository;
        private readonly IMapper _mapper;

        public ProductTypeService(IRepository<ProductType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<ProductType> GetAll()
        {
            try
                {
                return _repository.GetAll().AsEnumerable();
            }
                catch (Exception e)
            {
                var list = new List<ProductType>();
                return list.AsEnumerable();
            }
        }

        public ProductType GetById(string id)
        {
            return _repository.GetConditional(_ => _.Id == id);
        }

        public PayloadResponse Create(ProductType model)
        {
            try
            {
                var entity = new ProductType();
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
                    PayloadType = "Product Type Creation",
                    Content = null,
                    Message = "Product Type Creation has been successfull"
                };
            }
            catch (Exception e)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Product Type Creation",
                    Content = null,
                    Message = $"Product Type Creation become unsuccessfull! Error:-{e.Message} "
                };
            }
        }

        public PayloadResponse Update(string id, ProductType model)
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
                    PayloadType = "Product Type Update",
                    Content = null,
                    Message = "Product Type Update successfull"
                };
            }
            catch (Exception e)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Product Type Update",
                    Content = null,
                    Message = $"Product Type Update become unsuccessfull! Error:-{e.Message}"
                };
            }
        }
    }
}
