using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ProductService : GenericService<Product>, IProductService
    {
        public ProductService(IGenericRepository<Product> repository) : base(repository)
        {
        }
    }
}
