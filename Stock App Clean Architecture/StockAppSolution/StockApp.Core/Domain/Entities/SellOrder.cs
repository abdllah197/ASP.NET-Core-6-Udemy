using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        [Key]     
        public Guid SellOrderID { get; set; }
        [Required(ErrorMessage = "Stock symbol is required} ")]        
        public string? StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name is required} ")]
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }

    }
}