﻿using InvoicePro.Application.ViewModels;
using System.Collections.Generic;

namespace InvoicePro.Application.Interfaces
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentVm> GetAll();
        PayloadResponse Create(DepartmentVm model);
        PayloadResponse Update(string id, DepartmentVm model);
        DepartmentVm GetById(string id);
    }
}