using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCM.Entity;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Repository.Abstraction
{
    public interface IFinancialYearTermWiseFeeRepo  : IBaseRepository<BaseDataTableEntity>
    {
        Task<ResponseMessage> DeleteFinanceYearTerm(long id);
        Task<ResponseMessage> UpdateFinanceData(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity);
        Task<FinancialYearTermWiseFeeEntity> GetFinanceDataById(long id);
        Task<IEnumerable<FinancialYearTermWiseFeeEntity>> GetFinanceData();
        Task<ResponseMessage> AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity);
        Task<List<SelectListItem>> BindYear();
        Task<List<SelectListItem>> BindTerm();
        Task<List<SelectListItem>> BindSubhead();
    }
}
