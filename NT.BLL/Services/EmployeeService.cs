using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class EmployeeService : GenericService<Employee>
    {
        public EmployeeService(IGenericRepository<Employee> repository) : base(repository)
        {
        }
    }
}
