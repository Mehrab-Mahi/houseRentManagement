﻿using InvoicePro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Application.ViewModels
{
    public class InviteUserVm : InviteUser
    {
        public string RoleName { get; set; }
        public string PublicRegisterUrl { get; set; }
    }
}
