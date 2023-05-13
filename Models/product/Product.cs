using Inventory_M.Models.Brands;
using Inventory_M.Models.Categories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.product
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Product_Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Barcode { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string description { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Price { get; set; }


        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [Column(TypeName = "int")]
        public int Recorder { get; set; }


        [Required]
        [ForeignKey("Brands")]
        public int BrandId { get; set; }
        public Brand Brands { get; set; }


        [Required]
        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
    }
}
