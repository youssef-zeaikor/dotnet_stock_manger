using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.Brands
{
    public class Brand

    {
        
        [Key]
        public int BrandId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Brand_Name { get; set; }

    }
}
