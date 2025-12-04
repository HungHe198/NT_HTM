using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class EmployeeWebService : EmployeeService
    {
        public EmployeeWebService(IGenericRepository<Employee> repository) : base(repository)
        {
        }
    }
}
