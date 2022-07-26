using CoffeeAutomation.Entities;

namespace CoffeeAutomation.Models.OrderVMs
{
    public class OrderCreateVm
    {
        public ICollection<Order> Orders { get; set; }
        public Order Order { get; set; }
    }
}
