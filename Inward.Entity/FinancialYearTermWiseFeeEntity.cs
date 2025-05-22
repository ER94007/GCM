using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
    public class FinancialYearTermWiseFeeEntity
    {
        public long FinancialYearWiseTermWiseFeeDetailid { get; set; }
        public long FinancialYearId {  get; set; }
        public long TermId { get; set; }
        public string SelectedTermIds { get; set; }
        public long SubHeadId { get; set; }
        public decimal malefee { get; set; }
        public decimal femalefee { get; set; }
        public string financialYear { get; set; }
        public string termname { get; set; }
        public string subheadname { get; set; }
        public DataTable dt { get; set; }
        public long userid { get; set; } 
        public string hostname { get; set; } = System.Environment.MachineName;
        public string ipaddress { get; set; } = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        public string transactionid { get; set; } = new Guid().ToString();
        public List<FinanceList> financeLists { get; set; } = new List<FinanceList>();
    }
    public class FinanceList
    {
        public long TermId { get; set; }
        public long SubHeadId { get; set; }
        public decimal malefee { get; set; }
        public decimal femalefee { get; set; }

    }

    public class FinanceBalanceEntity
    {
        public long FinancialYearBalanceId { get; set; }

		[Required(ErrorMessage = "Please select a Finance Year.")]

		public long? FinancialYearId { get; set; }

        public string financialYear { get; set; }
        public string subheadname { get; set; }
        public long SubHeadId { get; set; }
        public decimal amount { get; set; }
        
        [Required(ErrorMessage = "At least one balance row is required.")]

		public List<BalanceList> balanceLists { get; set; } = new List<BalanceList>();
        public DataTable fdt { get; set; }
        public string updatedbalance { get; set; }
        public long userid { get; set; }
        public string hostname { get; set; } = System.Environment.MachineName;
        public string ipaddress { get; set; } = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        public string transactionid { get; set; } = new Guid().ToString();
    }

    public class BalanceList
    {

		[Required(ErrorMessage = "Please select a SubHead.")]

		public long? SubHeadId { get; set; }

		[Required(ErrorMessage = "Amount is required.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid amount.")]
		public decimal? amount { get; set; }
    }

    public class ExpenseEntity
    {
        public string ExpenseType { get; set; }
        public DateTime DateofExpense { get; set; }
        public string FinancialYear { get; set; }
        public string SubHeadName { get; set; }
        public long ExpenseMasterId { get; set; }
        [Required(ErrorMessage = "Please select a Finance Year.")]
        public long FinancialYearId { get; set; }
        [Required(ErrorMessage = "Please select a SubHead.")]
        public long SubHeadId { get; set; }
        [Required(ErrorMessage = "Amount is required.")]
        public decimal amount { get; set; }
        [Required(ErrorMessage = "Please Enter Remarks.")]
        public string Remarks { get; set; }
        [Required(ErrorMessage = "Please enter a Cheque Number.")]
        public long ChequeNo { get; set; }
        public long userid { get; set; }
        public string hostname { get; set; } = System.Environment.MachineName;
        public string ipaddress { get; set; } = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        public string transactionid { get; set; } = new Guid().ToString();
    }
}
