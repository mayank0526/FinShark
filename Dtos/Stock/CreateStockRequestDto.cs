using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {

        [Required]
        [MaxLength(6, ErrorMessage = "Symbol can not exceed 6 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(64, ErrorMessage = "Company Name can not exceed 64 characters")]
        public string CompanyName { get; set; } = string.Empty;


        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001,1000)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(12, ErrorMessage = "Industry cannot exceed 12 character")]
        public string Industry { get; set; } = string.Empty;


        [Required]
        [Range(1,500000000000000)]
        public long MarketCap { get; set; }

    }
}