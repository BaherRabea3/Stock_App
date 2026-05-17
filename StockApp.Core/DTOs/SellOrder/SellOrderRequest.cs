using StockApp.Core.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace StockApp.Core.DTOs.SellOrder
{
    public class SellOrderRequest
    {
        [Required(ErrorMessage = "Stock symbol can't be blank")]
        public string StockSymbol { get; set; } = string.Empty;
        [Required(ErrorMessage = "Stock name can't be blank")]
        public string StockName { get; set; } = string.Empty;
        [MinimumDateValidator(MinimumYear: 2000)]
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }
    }
}
