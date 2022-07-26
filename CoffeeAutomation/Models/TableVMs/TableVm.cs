using CoffeeAutomation.Entities;

namespace CoffeeAutomation.Models.TableVMs
{
    public class TableVm
    {
        public Table Table { get; set; }
        public ICollection<Table> Tables { get; set; }
    }
}
