﻿using Entities;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    public class BuyOrderRequest: IValidatableObject
    {        

        [Required(ErrorMessage = "Stock symbol is required")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock name is required")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity range 1:100000")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Maximum price 10000")]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {			
			return new BuyOrder() { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity };
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }
            return results;
        }
    }
}
