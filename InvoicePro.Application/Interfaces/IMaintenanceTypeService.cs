using InvoicePro.Application.ViewModels;
using System.Collections.Generic;

namespace InvoicePro.Application.Interfaces
{
    public interface IMaintenanceTypeService
    {
        IEnumerable<MaintenanceTypeVm> GetAll();
        PayloadResponse Create(MaintenanceTypeVm model);
        PayloadResponse Update(string id, MaintenanceTypeVm model);
        MaintenanceTypeVm GetById(string id);
    }
}