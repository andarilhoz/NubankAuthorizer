using NubankAuthorizer.Models;

namespace NubankAuthorizer.Controllers
{
    public interface IOperationController
    {
        Response ProcessOperation(Operations operation);
    }
}