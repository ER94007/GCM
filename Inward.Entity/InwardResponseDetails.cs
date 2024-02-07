using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inward.Entity
{
    public class InwardResponseDetails
    {
        public int InwardId { get; set; }
        public string Farmer_Id { get; set; }
        public string Farmer_Name { get; set; }

        public string? Reg_TravelRequestTypeId { get; set; }


        public string InwardNo { get; set; }

        public DateTime InwardDate { get; set; }

        public List<GradeDetail>? GradeDetails { get; set; }
    }
}
