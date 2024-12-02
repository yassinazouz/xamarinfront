using System;
using System.Collections.Generic;
using System.Text;

namespace xamarinproject
{
    public class CartRequest
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
