using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.vendor
{
    public class Vendor
    {
        [Key]
        public int VendorId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string VendorName { get; set; }

        [Column(TypeName = "text")]
        public string Address { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ContactPerson { get; set; }


        [Column(TypeName = "varchar(10)")]
        public string Telephone { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Fax { get; set; }

    }
}
