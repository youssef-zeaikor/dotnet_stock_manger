using Inventory_M.Models.product;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.adjustement
{
    public class Adjustement
    {
        [Key]
        public int AdjId { get; set; }


        [Column(TypeName = "int")]
        public int Reference { get; set; }


        [Required]
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public Product Products { get; set; }


        [Column(TypeName = "int")]
        public int Qty { get; set; }


        [RegularExpression("^(add|remove)$")]
        public string Action { get; set; }



        [Column(TypeName = "varchar(max)")]
        public string Remarks { get; set; }


        [Column(TypeName = "varchar(20)")]
        public string UserAuth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateStamp { get; set; } = DateTime.Now;
    }
}
