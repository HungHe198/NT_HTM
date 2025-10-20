using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class PermissionService : GenericService<Permission>, IPermissionService
    {
        public PermissionService(IGenericRepository<Permission> repository) : base(repository)
        {
        }
    }
}
