using InvoicePro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePro.Infra.Data.Context
{
   public class AssetProDbContext : DbContext
    {
        public AssetProDbContext(DbContextOptions<AssetProDbContext> options)
          : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MenuCrud> MenuCruds { get; set; }
        public DbSet<AccessControl> AccessControls { get; set; }
        public DbSet<InviteUser> InviteUsers { get; set; }       
        public DbSet<Department> Departments { get; set; }       
        public DbSet<Designation> Designations { get; set; }       
        public DbSet<Employee> Employees { get; set; }       
        public DbSet<AssetType> AssetTypes { get; set; }       
        public DbSet<AssetStatus> AssetStatuses { get; set; }       
        public DbSet<MaintenanceType> MaintenanceTypes { get; set; }       
        public DbSet<MailHost> MailHosts { get; set; }       
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<InvoiceNumber> InvoiceNumbers { get; set; }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
