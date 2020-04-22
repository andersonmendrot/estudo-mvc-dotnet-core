using System.Collections.Generic;
namespace MyWebApp.Models{
    public  class ProductListVM{
        public Discount Discount { get; set; }
        public IList<Product> Products { get; set; }
    }
}