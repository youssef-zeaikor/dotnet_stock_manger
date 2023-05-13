using Inventory_M.Models.adjustement;
using Inventory_M.Models.Brands;
using Inventory_M.Models.Categories;
using Inventory_M.Models.product;
using Inventory_M.Models.sale;
using Inventory_M.Models.stockin;
using Inventory_M.Models.tradespeople;
using Inventory_M.Models.vendor;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.recod
{
    public class Recod
    {
        [Key]
        public int RecordId { get; set; }
        [Required]
        [ForeignKey("Trades")]
        public int TradesId { get; set; }
        public Tradespeople Trades { get; set; }

        [Required]
        [ForeignKey("produits")]
        public int ProductId { get; set; }
        public Product produits { get; set; }

        [Required]
        [ForeignKey("categ")]
        public int CategoryId { get; set; }
        public Category categ { get; set; }

        [Required]
        [ForeignKey("bran")]
        public int BrandId { get; set; }
        public Brand bran { get; set; }

        [Required]
        [ForeignKey("sal")]
        public int saleId { get; set; }
        public Sale sal { get; set; }

        [Required]
        [ForeignKey("stock")]
        public int StockInId { get; set; }
        public StockIn stock { get; set; }

        [Required]
        [ForeignKey("vend")]
        public int VendorId { get; set; }
        public Vendor vend { get; set; }


        [Required]
        [ForeignKey("adj")]
        public int AdjId { get; set; }
        public Adjustement adj { get; set; }
    }
}
