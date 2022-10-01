using InvoicePro.Application.ViewModels;
using System.Collections.Generic;

namespace InvoicePro.Application.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeVm> GetAll();

        EmployeeVm GetById(string id);

        PayloadResponse Create(EmployeeVm model);

        PayloadResponse Update(string id, EmployeeVm model);
    }
}