using InvoicePro.Application.ViewModels;
using System.Collections.Generic;

namespace InvoicePro.Application.Interfaces
{
    public interface IAuthService
    {
        PayloadResponse Authenticate(AuthRequest model);

        UserAuthVm ValidateToken(string authToken);

        UserAuthVm GetCurrentUser();

        List<AccessControlVm> GetUserMenu(string id);
    }
}