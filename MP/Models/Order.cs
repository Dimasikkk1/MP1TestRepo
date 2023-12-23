using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MP.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int OrderNumber { get; set; }
        public DateTime ReleaseMonth { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
