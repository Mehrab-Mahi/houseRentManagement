﻿using System;

namespace InvoicePro.Application.ViewModels
{
    public class DepartmentVm
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}