﻿using System;
using System.Collections.Generic;
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
        public long FinancialYearId { get; set; }
        public string financialYear { get; set; }
        public string subheadname { get; set; }
        public long SubHeadId { get; set; }
        public decimal amount { get; set; }
        public List<BalanceList> balanceLists { get; set; } = new List<BalanceList>();
        public DataTable fdt { get; set; }
    }

    public class BalanceList
    {
        public long SubHeadId { get; set; }
        public decimal amount { get; set; }
    }

    public class ExpenseEntity
    {
        public string DateofExpense { get; set; }
        public string FinancialYear { get; set; }
        public string SubHeadName { get; set; }
        public long ExpenseMasterId { get; set; }
        public long FinancialYearId { get; set; }
        public long SubHeadId { get; set; }
        public decimal amount { get; set; }
        public string Remarks { get; set; }
        public string ChequeNo { get; set; }
    }
}
