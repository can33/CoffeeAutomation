using CoffeeAutomation.Entities;
using X.PagedList;

namespace CoffeeAutomation.Models
{
    public class CategoryVm
    {
        public Category Category{ get; set; }
        public IPagedList<Category> Categories{ get; set; }
    }
}
