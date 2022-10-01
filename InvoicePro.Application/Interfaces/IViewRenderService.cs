using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}