using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Entities.Dto
{
    public class ProductAddDto
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }

    }
}
