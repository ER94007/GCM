using System;
using System.Collections.Generic;
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
        public long SubHeadId { get; set; }
        public decimal malefee { get; set; }
        public decimal femalefee { get; set; }
        public List<FinanceList> financeLists { get; set; } = new List<FinanceList>();
    }
    public class FinanceList
    {
        public long SubHeadId { get; set; }
        public decimal malefee { get; set; }
        public decimal femalefee { get; set; }

    }
}
