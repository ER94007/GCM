using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;    
namespace Inward.Entity
{
    public class Student
    {
        public long studentid { get; set; }
        [Required]
        public string name { get; set; }
        public long FinancialYearId { get; set; }
        public string mobileno { get; set; }
        public string? FinancialYear { get; set; }
        public string email { get; set; }
        public long genderid { get; set; }
        public string? gendername { get; set; }
        public long categoryid { get; set; }
        public string? categoryname { get; set; }
        public string? enrolmentno { get; set; }
        public string? applicationno { get; set; }
        public string? userid { get; set; }
        public List<Student>? Studentslist { get; set; }
        //public IFileForm studentfile { get; set; }

    }
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

    public class StudentFeeDetailReport
    {

        public long StudentId { get; set; }
        public string RecieptNo { get; set; }
        public string StudentName { get; set; }
        public string GenderName { get; set; }
        public string TermName { get; set; }
        public string SubHeadName { get; set; }
        public Int64 Amount { get; set; }
        public Int64 GovtTotal { get; set; }
        public Int64 PrivateTotal { get; set; }
    }
 
}
