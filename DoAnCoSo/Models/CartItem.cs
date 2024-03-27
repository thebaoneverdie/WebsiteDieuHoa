using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }

        public string image { get; set; }

        public int giamgia { get; set; }

        public int quantity { get; set; }

        public decimal Money
        {
            get
            {
                return quantity * price;
            }
        }
    }
}
