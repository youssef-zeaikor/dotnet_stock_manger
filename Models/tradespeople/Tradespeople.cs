using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_M.Models.tradespeople
{
    public class Tradespeople
    {

        [Key]
        public int TradesId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Tele { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ADRESS { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string matricule { get; set; }
    }
}
