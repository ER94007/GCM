using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Repository.Abstraction
{
    public interface IFinancialYearTermWiseFeeRepo  : IBaseRepository<BaseDataTableEntity>
    {
        Task<List<SelectListItem>> BindYear();
        Task<List<SelectListItem>> BindTerm();
        Task<List<SelectListItem>> BindSubhead();
    }
}
