using InvoicePro.Application.Interfaces;
using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using InvoicePro.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<ProductType> _productTyperepository;
        private readonly IHostingEnvironment _env;

        public ProductService(IRepository<Product> repository, IHostingEnvironment env, IRepository<ProductType> productTyperepository)
        {
            _repository = repository;
            _env = env;
            _productTyperepository = productTyperepository;
        }

        public IEnumerable<ProductVm> GetAll()
        {
            var products = _repository.GetAll().ToList();

            var productList = new List<ProductVm>();

            foreach(var p in products)
            {
                p.ProductType = AssignProductType(p.ProductType);
                productList.Add(AssignProduct(p));
            }

            return productList.AsEnumerable();
        }

        private string AssignProductType(string productTypeId)
        {
            return _productTyperepository.GetConditional(_ => _.Id == productTypeId).Name;
        }

        public Product GetById(string id)
        {
            var entity = _repository.GetConditional(_ => _.Id == id);

            entity.ImagePath = entity.ImagePath == null ? null : GetImageString(entity.ImagePath);

            return entity;
        }

        private string GetImageString(string path)
        {
            var relativePath = path != null ? (GetImageFolder() + path) : "";

            if (string.IsNullOrEmpty(relativePath))
            {
                return "";
            }

            byte[] AsBytes = File.ReadAllBytes(relativePath);
            return "data:image/jpeg;base64," + Convert.ToBase64String(AsBytes);
        }

        public PayloadResponse Create(Product model)
        {
            try
            {
                model.ImagePath = model.ImagePath == null ? null : SaveImage(model.ImagePath);
                _repository.Insert(model);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Product Creation",
                    Content = null,
                    Message = "Product Creation has been successfull"
                };
            }
            catch(Exception ex)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Product Creation",
                    Content = null,
                    Message = $"Product Creation become unsuccessfull! Error:-{ex.Message} "
                };
            }
        }

        private string SaveImage(string imagePath)
        {
            var name = Guid.NewGuid().ToString("N") + ".jpg";
            try
            {
                var path = GetImageFolder();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var valid64 = imagePath.Split(',');
                var bytes = Convert.FromBase64String(valid64[1]);
                File.WriteAllBytes(Path.Combine(path, name), bytes);

                return name;
            }
            catch (Exception ex)
            {
                return imagePath;
            }
        }

        private string GetImageFolder()
        {
            return Path.Combine(_env.ContentRootPath, "wwwroot/Images/");
        }

        private ProductVm AssignProduct(Product p)
        {
            return new ProductVm() { 
                Id = p.Id,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductType = p.ProductType,
                Vendor = p.Vendor,
                Available = p.Available,
                OnSalesOrder = 0,
                AverageCosting = p.AverageCosting,
                SalesPrice = p.SalesPrice,
                SalesPerWeek = 0,
                ImagePath = GetImageString(p.ImagePath)
            };
        }

        public PayloadResponse Update(string id, Product model)
        {
            var entity = _repository.GetConditional(_ => _.Id == id);
            try
            {
                entity.ProductCode = model.ProductCode;
                entity.ProductName = model.ProductName;
                entity.ProductDescription = model.ProductDescription;
                entity.ProductType = model.ProductType;
                entity.Vendor = model.Vendor;
                entity.Available = model.Available;
                entity.AverageCosting = model.AverageCosting;
                entity.SalesPrice = model.SalesPrice;

                if(model.ImagePath == null)
                {
                    entity.ImagePath = null;
                }
                else if(model.ImagePath.Split(',').Length > 1)
                {
                    if(entity.ImagePath != null)
                    {
                        var file = new FileInfo(Path.Combine(GetImageFolder(), entity.ImagePath));
                        file.Delete();
                    }
                    entity.ImagePath = SaveImage(model.ImagePath);
                }

                _repository.Update(entity);
                _repository.SaveChanges();

                return new PayloadResponse
                {
                    IsSuccess = true,
                    PayloadType = "Product Update",
                    Content = null,
                    Message = "Product Update successfull"
                };
            }
            catch (Exception e)
            {
                return new PayloadResponse
                {
                    IsSuccess = false,
                    PayloadType = "Product Update",
                    Content = null,
                    Message = $"Product Update become unsuccessfull! Error:-{e.Message}"
                };
            }
        }
    }
}
