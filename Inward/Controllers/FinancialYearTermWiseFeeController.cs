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
            else
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
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

        [HttpGet]
        public async Task<IActionResult> AddFinanceYearBalance()
        {
            FinanceBalanceEntity ft = new FinanceBalanceEntity();
            ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            return View(ft);
        }

        [HttpPost]
        public async Task<IActionResult> AddFinanceYearBalance(FinanceBalanceEntity ft)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SubHeadId", typeof(long));
            dataTable.Columns.Add("amount", typeof(decimal));

            foreach (var data in ft.balanceLists)
            {
                dataTable.Rows.Add( data.SubHeadId, data.amount);
            }
            ft.fdt = dataTable;
            var result = await _ifinancialYearTermWiseFee.AddFinanceYearBalance(ft);
            if (result.Msg == "Data inserted successfully.")
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
            }
            else
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
            }
            return RedirectToAction("AddFinanceYearBalance");
        }


        [HttpGet]
        public async Task<IActionResult> GetFinanceYearBalance()
        {
            var fin = await _ifinancialYearTermWiseFee.GetFinanceBalanceData();
            return View(fin);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFinanceBalanceById(string FinancialYearBalanceId)
        {
            Console.WriteLine(FinancialYearBalanceId);
            ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            var fn = await _ifinancialYearTermWiseFee.GetBalanceDataById(Convert.ToInt64(FinancialYearBalanceId));
            return View(fn);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFinanceBalanceById(FinanceBalanceEntity fn)
        {
            var result = await _ifinancialYearTermWiseFee.UpdateFinanceBalance(fn);
            if (result.Msg == "DATA UPDATED")
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
            }
            else
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
            }
            return RedirectToAction("GetFinanceYearBalance");
        }

        public async Task<IActionResult> DeleteFinanceBalance(string FinancialYearBalanceId)
        {
            var result = await _ifinancialYearTermWiseFee.DeleteFinanceBalance(Convert.ToInt64(FinancialYearBalanceId));
            if (result.Id > 0)
            {
                return Json(new { success = true, message = "Student deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete the student." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddExpense()
        {
            ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(ExpenseEntity ep)
        {
            var result = await _ifinancialYearTermWiseFee.AddExpense(ep);
            if (result.Id > 0)
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
            }
            else
            {
                TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
            }
            return RedirectToAction("AddExpense");
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenseData()
        {
            var ep =  await _ifinancialYearTermWiseFee.GetExpenseData();
            return View(ep);
        }

    }
}
