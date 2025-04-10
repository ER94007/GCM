﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCM.Entity;
using Inward.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Services.Abstraction
{
    public interface IFinancialYearTermWiseFeeService
    {
        Task<IEnumerable<ExpenseEntity>> GetExpenseData();
        Task<ResponseMessage> AddExpense(ExpenseEntity ep);
        Task<ResponseMessage> DeleteFinanceBalance(long id);
        Task<ResponseMessage> UpdateFinanceBalance(FinanceBalanceEntity fn);
        Task<FinanceBalanceEntity> GetBalanceDataById(long id);
        Task<IEnumerable<FinanceBalanceEntity>> GetFinanceBalanceData();
        Task<ResponseMessage> DeleteFinanceYearTerm(long id);
        Task<ResponseMessage> AddFinanceYearBalance(FinanceBalanceEntity ft);
        Task<ResponseMessage> UpdateFinanceData(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity);
        Task<FinancialYearTermWiseFeeEntity> GetFinanceDataById(long id);
        Task<IEnumerable<FinancialYearTermWiseFeeEntity>> GetFinanceData();
        Task<ResponseMessage> AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity);
        Task<List<SelectListItem>> BindYear();
        Task<List<SelectListItem>> BindTerm();
        Task<List<SelectListItem>> BindSubhead();
    }
}
