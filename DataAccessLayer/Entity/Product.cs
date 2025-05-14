using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProdoctStock { get; set; }
        public string ProductPrice { get; set; }
    }
}
