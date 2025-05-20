using System;
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
        Task<ResponseMessage> AddChequeNo(ChequeMaster ep);
        Task<ResponseMessage> DeleteFinanceBalance(long id, long id2, string name1, string name2);
        Task<long> GetBalanceData(long id1, long id2);
        Task<ChequeMaster> GetChequeDateById(long id);
        Task<ResponseMessage> CheckExpenseForCheque(long id);
        Task<ResponseMessage> UpdateCheque(ChequeMaster fn);
        Task<ResponseMessage> UpdateFinanceBalance(FinanceBalanceEntity fn);
        Task<FinanceBalanceEntity> GetBalanceDataById(long id);
        Task<IEnumerable<FinanceBalanceEntity>> GetFinanceBalanceData();
        Task<IEnumerable<ChequeMaster>> GetChequeDate();
        Task<ResponseMessage> DeleteFinanceYearTerm(long id, long id2, string name1, string name2);
        Task<ResponseMessage> AddFinanceYearBalance(FinanceBalanceEntity ft);
        Task<ResponseMessage> UpdateFinanceData(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity);
        Task<FinancialYearTermWiseFeeEntity> GetFinanceDataById(long id);
        Task<IEnumerable<FinancialYearTermWiseFeeEntity>> GetFinanceData();
        Task<ResponseMessage> AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity);
        Task<List<SelectListItem>> BindYear();
        Task<List<SelectListItem>> BindTerm();
        Task<List<SelectListItem>> BindSubhead();
        Task<List<SelectListItem>> BindCheques();
        Task<List<SelectListItem>> BindHeads();
    }
}
