using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
namespace NT.BLL.Services
{
    internal class PaymentMethodServices : GenericService<PaymentMethod>, IPaymentMethodService
    {
        public PaymentMethodServices(IGenericRepository<PaymentMethod> repository) : base(repository)
        {
        }
    }
}
