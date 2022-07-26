using CoffeeAutomation.Entities;
using X.PagedList;

namespace CoffeeAutomation.Models
{
    public class ProductVm
    {
        public IPagedList<Product> Products { get; set; }
        public Product Product { get; set; }
    }
}
