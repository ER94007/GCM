using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GCM.Entity;
using GCM.Repository.Abstraction;
using GCM.Services.Abstraction;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Services
{
    public class FinancialYearTermWiseFeeService : IFinancialYearTermWiseFeeService
    {
        private readonly IFinancialYearTermWiseFeeRepo _financialYearTermWiseFeeRepo;
        public FinancialYearTermWiseFeeService(IFinancialYearTermWiseFeeRepo financialYearTermWiseFeeRepo)
        {
            _financialYearTermWiseFeeRepo = financialYearTermWiseFeeRepo ?? throw new ArgumentNullException(nameof(financialYearTermWiseFeeRepo));
        }
        public async Task<IEnumerable<ChequeMaster>> GetChequeDate()
        {
            return await _financialYearTermWiseFeeRepo.GetChequeDate();
        }
        public async Task<IEnumerable<ExpenseEntity>> GetExpenseData()
        {
            return await _financialYearTermWiseFeeRepo.GetExpenseData();
        }
        public async Task<ResponseMessage> AddChequeNo(ChequeMaster ep)
        {
            return await _financialYearTermWiseFeeRepo.AddChequeNo(ep);
        }
        public async Task<ResponseMessage> AddExpense(ExpenseEntity ep)
        {
            return await _financialYearTermWiseFeeRepo.AddExpense(ep);
        }
        public async Task<long> GetBalanceData(long id1,long id2)
        {
            return await _financialYearTermWiseFeeRepo.GetBalanceData(id1,id2);
        }
        public async Task<ResponseMessage> CheckExpenseForCheque(long id)
        {
            return await _financialYearTermWiseFeeRepo.CheckExpenseForCheque(id);
        }
        public async Task<ChequeMaster> GetChequeDateById(long id)
        {
            return await _financialYearTermWiseFeeRepo.GetChequeDateById(id);
        }
        public async Task<ResponseMessage> DeleteFinanceBalance(long id, long id2, string name1, string name2)
        {
            return await _financialYearTermWiseFeeRepo.DeleteFinanceBalance(id, id2, name1, name2);
        }
        public async Task<ResponseMessage> UpdateFinanceBalance(FinanceBalanceEntity fn)
        {
            return await _financialYearTermWiseFeeRepo.UpdateFinanceBalance(fn);
        }
        public async Task<FinanceBalanceEntity> GetBalanceDataById(long id)
        {
            return await _financialYearTermWiseFeeRepo.GetBalanceDataById(id);
        }
        public async Task<IEnumerable<FinanceBalanceEntity>> GetFinanceBalanceData()
        {
            return await _financialYearTermWiseFeeRepo.GetFinanceBalanceData();
        }
        public async Task<ResponseMessage> UpdateCheque(ChequeMaster ft )
        {
            return await _financialYearTermWiseFeeRepo.UpdateCheque(ft);
        }
        public async Task<ResponseMessage> AddFinanceYearBalance(FinanceBalanceEntity ft )
        {
            return await _financialYearTermWiseFeeRepo.AddFinanceYearBalance(ft);
        }
        public async Task<ResponseMessage> DeleteFinanceYearTerm(long id, long id2, string name1, string name2)
        {
            return await _financialYearTermWiseFeeRepo.DeleteFinanceYearTerm(id, id2, name1, name2);
        }
        public async Task<ResponseMessage> UpdateFinanceData(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity)
        {
            return await _financialYearTermWiseFeeRepo.UpdateFinanceData(financialYearTermWiseFeeEntity);
        }
        public async Task<FinancialYearTermWiseFeeEntity> GetFinanceDataById(long id)
        {
            return await _financialYearTermWiseFeeRepo.GetFinanceDataById(id);
        }
        public async Task<IEnumerable<FinancialYearTermWiseFeeEntity>> GetFinanceData()
        {
            return await _financialYearTermWiseFeeRepo.GetFinanceData();
        }
        public async Task<ResponseMessage> AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity)
        {
            return await _financialYearTermWiseFeeRepo.AddFinancialYearTermFee(financialYearTermWiseFeeEntity);
        }
        public async Task<List<SelectListItem>> BindYear()
        {
            return await _financialYearTermWiseFeeRepo.BindYear();
        }
        public async Task<List<SelectListItem>> BindTerm()
        {
            return await _financialYearTermWiseFeeRepo.BindTerm();
        }
        public async Task<List<SelectListItem>> BindSubhead()
        {
            return await _financialYearTermWiseFeeRepo.BindSubhead();
        }
        public async Task<List<SelectListItem>> BindCheques()
        {
            return await _financialYearTermWiseFeeRepo.BindCheques();
        }
		public async Task<List<SelectListItem>> BindHeads()
		{
			return await _financialYearTermWiseFeeRepo.BindHeads();
		}

		
	}
}
