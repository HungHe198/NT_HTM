using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class RodHardness
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Level { get; private set; } = null!;

        private RodHardness() { }

        public static RodHardness Create(string level) => new RodHardness { Level = level };

        public ICollection<ProductDetailHardness> ProductDetailHardnesses { get; private set; } = new List<ProductDetailHardness>();
    }

}