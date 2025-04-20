using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
    public class StudentFeeCollection
    {
        public long FinancialYearId { get; set; }
        public long TermId { get; set; }
        public long SubHeadId { get; set; }
        public string subheadname { get; set; }
        public long subheadid { get; set; }
        public string fees { get; set; }
        public string StudentId { get; set; }
        public string FormType { get; set; }
        public DataTable feesdetails { get; set; }
        public string FinancialYear { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public string RecieptNo { get; set; }
        public string FeeModeDescription { get; set; }
        public string name { get; set; }
        public string GovernmentFee { get; set; }
        public long GovAmount { get; set; }
        public string PrivateFee { get; set; }
        public long PrivAmount { get; set; }
        public string GovernmentTotal { get; set; }
        public List<FeeDetail> FeeDetailLists { get; set; } = new List<FeeDetail>();
        public string EnrolmentNo { get; set; }
        public string ApplicationNo { get; set; }
        public string CreatedDate { get; set; }



        [Required(ErrorMessage = "Please select a Finance Year.")]

       // public long? FinancialYearId { get; set; }
        //public long? TermId { get; set; }

        public string financialYear { get; set; }
        public decimal amount { get; set; }

        [Required(ErrorMessage = "At least one balance row is required.")]

        public List<FeeDetaillistMannually> FeeDetaillistMannually { get; set; } = new List<FeeDetaillistMannually>();
        public DataTable fdt { get; set; }

    }

       public class StudentFeeDetail
    {
        public int Id { get; set; }
        public int StudentFeeCollectionId { get; set; }
        public int SubHeadId { get; set; }
        public decimal MaleFee { get; set; }
        public decimal FemaleFee { get; set; }
    }
    public class FeeDetail
    {
        public long SubheadId { get; set; }
        public string SubHeadName { get; set; }
        public decimal Amount { get; set; }
    }

    public class MannualStudentFeeCollection
    {
        public string StudentId { get; set; }
        public long FinancialYearBalanceId { get; set; }

        [Required(ErrorMessage = "Please select a Finance Year.")]

        public long? FinancialYearId { get; set; }
        public long? TermId { get; set; }

        public string financialYear { get; set; }
        public string subheadname { get; set; }
        public long SubHeadId { get; set; }
        public decimal amount { get; set; }

        [Required(ErrorMessage = "At least one balance row is required.")]

        public List<FeeDetaillistMannually> FeeDetaillistMannually { get; set; } = new List<FeeDetaillistMannually>();
        public DataTable fdt { get; set; }
    }

    public class FeeDetaillistMannually
    {

        [Required(ErrorMessage = "Please select a SubHead.")]

        public long? SubHeadId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid amount.")]
        public decimal? amount { get; set; }
    }
}
