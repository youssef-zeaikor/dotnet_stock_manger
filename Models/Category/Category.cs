using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.Categories
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Category_Name { get; set; }




    }
}
