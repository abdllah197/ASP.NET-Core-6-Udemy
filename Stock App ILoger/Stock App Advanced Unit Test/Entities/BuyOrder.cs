using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }

        [Required(ErrorMessage = "Stock symbol is required} ")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock name is required} ")]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity range 1:100000 ")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Maximum price 10000 ")]
        public double Price { get; set; }

    }
}