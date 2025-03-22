using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Services.Abstraction
{
    public interface IFinancialYearTermWiseFeeService
    {
        Task<List<SelectListItem>> BindYear();
        Task<List<SelectListItem>> BindTerm();
        Task<List<SelectListItem>> BindSubhead();
    }
}
