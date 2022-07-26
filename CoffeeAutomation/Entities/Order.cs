using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeAutomation.Entities
{
    public class Order
    {
        public Order()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreatedDate{ get; set; }
        public int Amount { get; set; }

        public virtual int ProductId{ get; set; }
        public Product Product{ get; set; }
        public virtual int TableId { get; set; }
        public Table Table { get; set; }
    }
}
