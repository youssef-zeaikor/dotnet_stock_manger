using Inventory_M.Models.product;
using Inventory_M.Models.tradespeople;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.sale
{
    public class Sale
    {

        [Key]
        public int saleId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string transno { get; set; }




        [Required]
        [Column(TypeName = "int")]
        public int Disc { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Price { get; set; }


        [Required]
        [Column(TypeName = "int")]
        public int Qty { get; set; }


        


        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string? status { get; set; }




        [Required]
        [ForeignKey("Trades")]
        public int TradesId { get; set; }
        public Tradespeople Trades { get; set; }

        [Required]
        [ForeignKey("produits")]
        public int ProductId { get; set; }
        public Product produits { get; set; }
    }


}





