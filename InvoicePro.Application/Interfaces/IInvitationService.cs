using InvoicePro.Application.ViewModels;
using System.Collections.Generic;

namespace InvoicePro.Application.Interfaces
{
    public interface IInvitationService
    {
        List<InviteUserVm> GetAll();

        bool Invite(InviteUserVm model);

        InviteUserVm GetByEmail(string email);

        bool IsExists(string email);

        void Update(InviteUserVm inviteModel);

        bool Approve(string id);
    }
}