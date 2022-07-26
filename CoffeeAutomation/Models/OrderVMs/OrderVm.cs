using CoffeeAutomation.Entities;
using X.PagedList;

namespace CoffeeAutomation.Models.OrderVMs
{
    public class OrderVm
    {
        public IPagedList<Order> Orders { get; set; }
        public Order Order { get; set; }
    }
}
