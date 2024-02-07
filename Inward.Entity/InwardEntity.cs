using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inward.Entity
{
    public class InwardEntity
    {
        public int InwardId { get; set; }
        public string Farmer_Id { get; set; }
        public string? Farmer_Name { get; set; }
        public string Contact_No { get; set; }
        public string Pan_Card_No { get; set; }
        public string? UserId { get; set; }


        [Required]
        [MaxLength(50)]
        public string InwardNo { get; set; }

        [Required]
        public string InwardDate { get; set; }

        public List<GradeDetail>? GradeDetails { get; set; }

    }


    public class GradeDetail
    {
        public int GradeDetailId { get; set; }

        [Required]
        public int InwardId { get; set; }

        public int? isDeleted { get; set; }

        [Required]
        public string? GradeName { get; set; }
        public string? Grade_Id { get; set; }

        [Required]
        public string TotalWeight { get; set; }

        [Required]
        public string AuctionRate { get; set; }

        [Required]
        public string VendorCode { get; set; }

        public string? UnitName { get; set; }
        public string? Unit_Id { get; set; }

        [Required]
        public string TotalCarat { get; set; }

        [Required]
        public string VendorRate { get; set; }

        [Required]
        public string NetAmount { get; set; }

    }
}
