using GCM.Entity;
using GCM.Services.Abstraction;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Controllers
{
    public class FinancialYearTermWiseFeeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _baseURL;
        private readonly string _Login;
        private readonly IFinancialYearTermWiseFeeService _ifinancialYearTermWiseFee;

        public FinancialYearTermWiseFeeController(IFinancialYearTermWiseFeeService financialYearTermWiseFee, IConfiguration config)
        {
            _ifinancialYearTermWiseFee = financialYearTermWiseFee ?? throw new ArgumentNullException(nameof(financialYearTermWiseFee));
            _config = config;
        }
        [HttpGet]
        public IActionResult AddFinancialYearTermFee()
        {
            FinancialYearTermWiseFeeEntity ft = new FinancialYearTermWiseFeeEntity();
            //ViewBag.GenderList = _userLoginService.BindGender().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.TermList = _ifinancialYearTermWiseFee.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            //ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            return View(ft);
        }
        [HttpPost]
        public IActionResult AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity ft)
        {
            
            return View(ft);
        }
    }
}
