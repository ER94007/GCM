﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
    public class StudentFeeCollection
    {
        public long FinancialYearWiseTermWiseFeeDetailid { get; set; }
        public long FinancialYearId { get; set; }
        public long TermId { get; set; }
        public string SelectedTermIds { get; set; }
        public long SubHeadId { get; set; }
        public decimal malefee { get; set; }
        public decimal femalefee { get; set; }
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
        public long subheadid { get; set; }
        public string SubHeadName { get; set; }
        public decimal Amount { get; set; }
    }

}
