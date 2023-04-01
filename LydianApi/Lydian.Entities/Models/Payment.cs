using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Entities.Models
{
    public class Payment
    {
        public Payment()
        {
            Code = Guid.NewGuid().ToString();
        }

        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public bool PaymentStatus { get; set; }
        public String Code { get; set; }
        

    }
}
