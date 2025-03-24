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

        public async Task<ResponseMessage> DeleteFinanceYearTerm(long id)
        {
            return await _financialYearTermWiseFeeRepo.DeleteFinanceYearTerm(id);
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
    }
}
