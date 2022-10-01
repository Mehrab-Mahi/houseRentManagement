using Microsoft.AspNetCore.Mvc;

namespace InvoicePro.Application.Helper
{
    public class InvoiceProAuthAttribute : TypeFilterAttribute
    {
        public InvoiceProAuthAttribute() : base(typeof(InvoiceProAuthorizeFilter))
        {
            Arguments = new object[] { };
        }
    }
}