using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models; // Add this using directive

namespace NT.BLL.Services
{
   public  class BrandService : GenericService<Brand>, IBrandService
    {
        public BrandService(Interfaces.IGenericRepository<Brand> repository) : base(repository)
        {
        }
    }
}
