using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeAutomation.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
