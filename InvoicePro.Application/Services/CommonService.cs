using InvoicePro.Application.Interfaces;
using InvoicePro.Domain.Entities;
using InvoicePro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<Entity> _repo;
        public CommonService(IRepository<Entity> repo)
        {
            _repo = repo;
        }
        public bool Delete(string id, string table)
        {
            try
            {
                var query = $"delete from {table}s where Id='{id}'";
                _repo.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
