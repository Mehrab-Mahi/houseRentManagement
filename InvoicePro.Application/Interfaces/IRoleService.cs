using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Entities;
using System.Collections.Generic;

namespace InvoicePro.Application.Interfaces
{
    public interface IRoleService
    {
        List<Role> GetAll();

        List<MenuTree> GetMenuTreeData();

        bool CreateRole(RoleVm roleVm);

        bool UpdateRole(RoleVm roleVm);

        List<string> GetRoleMenuIds(string id);

        Role GetRole(string id);
    }
}