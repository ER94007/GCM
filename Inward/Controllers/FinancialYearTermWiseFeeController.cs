using System.Data;
using GCM.Entity;
using GCM.Services.Abstraction;
using Inward.Common;
using Inward.Entity;
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
            ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            return View(ft);
        }
        [HttpPost]
        public IActionResult AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity ft)
        {
            DataTable dataTable = new DataTable();
            var terms = ft.SelectedTermIds?.Split(',') ?? Array.Empty<string>();
            dataTable.Columns.Add("TermId", typeof(long));
            dataTable.Columns.Add("SubHeadId", typeof(long));
            dataTable.Columns.Add("MaleFee", typeof(decimal));
            dataTable.Columns.Add("FemaleFee", typeof(decimal));
            foreach (var term in terms)
            {
                foreach (var data in ft.financeLists)
                {
                    dataTable.Rows.Add(Convert.ToInt64(term), data.SubHeadId, data.malefee, data.femalefee);
                }
            }
            ft.dt = dataTable;
            var result =  _ifinancialYearTermWiseFee.AddFinancialYearTermFee(ft);
            if(result.Result.Msg == "Data inserted successfully.")
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
            }
            return RedirectToAction("AddFinancialYearTermFee");
        }

        public async Task<IActionResult> GetFinancialTermData()
        {
            var fin = await _ifinancialYearTermWiseFee.GetFinanceData();
            return View(fin);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateFinancialById(string FinancialYearWiseTermWiseFeeDetailid)
        {
            ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.TermList = _ifinancialYearTermWiseFee.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            var fn = await _ifinancialYearTermWiseFee.GetFinanceDataById(Convert.ToInt64(FinancialYearWiseTermWiseFeeDetailid));
            return View(fn);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFinancialById(FinancialYearTermWiseFeeEntity ft)
        {
            var result = await _ifinancialYearTermWiseFee.UpdateFinanceData(ft);
            if(result.Msg != null)
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
            }
            return RedirectToAction("GetFinancialTermData");
        }

        public async Task<IActionResult> DeleteFinanceYearTerm(string FinancialYearWiseTermWiseFeeDetailid)
        {
            var result = await _ifinancialYearTermWiseFee.DeleteFinanceYearTerm(Convert.ToInt64(FinancialYearWiseTermWiseFeeDetailid));
            if (result.Id > 0)
            {
                return Json(new { success = true, message = "Student deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete the student." });
            }
        }
    }
}
