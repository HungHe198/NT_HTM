using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.BLL.Services
{
    public class VocherService : GenericService<Voucher>, IVocherService
    {
                       
        public VocherService(IGenericRepository<Voucher> repository) : base(repository)
        {
            
        }
    }
}
