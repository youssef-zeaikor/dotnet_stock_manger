using Inventory_M.Models.product;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inventory_M.Models.vendor;

namespace Inventory_M.Models.stockin
{
    public class StockIn
    {
        [Key]
        public int StockInId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Reference { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Quantite { get; set; }


        [Column(TypeName = "int")]
        public int qtynew { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string stockeby { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? status { get; set; }


        [Required]
        [ForeignKey("venders")]
        public int VendorId { get; set; }
        public Vendor venders { get; set; }

        [Required]
        [ForeignKey("products")]
        public int ProductId { get; set; }
        public Product products { get; set; }
    }
}
