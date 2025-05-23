﻿using System.Data;
using GCM.Entity;
using GCM.Services.Abstraction;
using System.Security.Claims;
using Inward.Common;
using Inward.Entity;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

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
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				FinancialYearTermWiseFeeEntity ft = new FinancialYearTermWiseFeeEntity();
				//ViewBag.GenderList = _userLoginService.BindGender().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.TermList = _ifinancialYearTermWiseFee.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				return View(ft);
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}
		[HttpPost]
		public IActionResult AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity ft)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
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
				ft.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = _ifinancialYearTermWiseFee.AddFinancialYearTermFee(ft);
				if (result.Result.Msg == "Data inserted successfully.")
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
				}
				else
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
				}
				return RedirectToAction("AddFinancialYearTermFee");
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}

		}

		public async Task<IActionResult> GetFinancialTermData()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var fin = await _ifinancialYearTermWiseFee.GetFinanceData();
				return View(fin);
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}
		[HttpGet]
		public async Task<IActionResult> UpdateFinancialById(string FinancialYearWiseTermWiseFeeDetailid)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.TermList = _ifinancialYearTermWiseFee.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				var fn = await _ifinancialYearTermWiseFee.GetFinanceDataById(Convert.ToInt64(FinancialYearWiseTermWiseFeeDetailid));
				return View(fn);
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFinancialById(FinancialYearTermWiseFeeEntity ft)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				ft.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _ifinancialYearTermWiseFee.UpdateFinanceData(ft);
				if (result.Msg == "-1")
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString("Fee Already Collected so you can not Update Record", Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
				}
				else if(result.Msg == "1")
				{
                    TempData["SaveStatus"] = CommonMethods.ConcatString("Data Updated Successfully", Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
                }
					return RedirectToAction("GetFinancialTermData");
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}

		}

		public async Task<IActionResult> DeleteFinanceYearTerm(string FinancialYearWiseTermWiseFeeDetailid)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				long userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
				var hostname = System.Environment.MachineName;
				var ipaddress = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                var result = await _ifinancialYearTermWiseFee.DeleteFinanceYearTerm(Convert.ToInt64(FinancialYearWiseTermWiseFeeDetailid), userid, hostname, ipaddress);
				if (result.Id > 0)
				{
					return Json(new { success = true, message = "Record deleted successfully." });
				}
				else
				{
					return Json(new { success = false, message = "Fee Already Collected so you can not delete Record." });
				}
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}

		}

		[HttpGet]
		public async Task<IActionResult> AddFinanceYearBalance()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				FinanceBalanceEntity ft = new FinanceBalanceEntity();
				ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				return View(ft);
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}

		}

		[HttpPost]
		public async Task<IActionResult> AddFinanceYearBalance(FinanceBalanceEntity ft)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add("SubHeadId", typeof(long));
				dataTable.Columns.Add("amount", typeof(decimal));

				foreach (var data in ft.balanceLists)
				{
					dataTable.Rows.Add(data.SubHeadId, data.amount);
				}
				ft.fdt = dataTable;
				ft.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _ifinancialYearTermWiseFee.AddFinanceYearBalance(ft);
				if (result.Msg == "Record Saved successfully.")
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
				}
				else
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
				}
				return RedirectToAction("AddFinanceYearBalance");
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}

		}


		[HttpGet]
		public async Task<IActionResult> GetFinanceYearBalance()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var fin = await _ifinancialYearTermWiseFee.GetFinanceBalanceData();
				return View(fin);
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		[HttpGet]
		public async Task<IActionResult> UpdateFinanceBalanceById(string FinancialYearBalanceId)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				var fn = await _ifinancialYearTermWiseFee.GetBalanceDataById(Convert.ToInt64(FinancialYearBalanceId));
				return View(fn);
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}

		}

		[HttpPost]
		public async Task<IActionResult> UpdateFinanceBalanceById(FinanceBalanceEntity fn)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
                fn.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		public async Task<IActionResult> DeleteFinanceBalance(string FinancialYearBalanceId)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
                long userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var hostname = System.Environment.MachineName;
                var ipaddress = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                var result = await _ifinancialYearTermWiseFee.DeleteFinanceBalance(Convert.ToInt64(FinancialYearBalanceId), userid, hostname, ipaddress);
				if (result.Id > 0)
				{
					return Json(new { success = true, message = "Student deleted successfully." });
				}
				else
				{
					return Json(new { success = false, message = "Failed to delete the student." });
				}
			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetBalance(string subheadid, string finyearid)
		{
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var result = await _ifinancialYearTermWiseFee.GetBalanceData(Convert.ToInt64(subheadid), Convert.ToInt64(finyearid));
                if(result >= 0)
				{
					return Json(new { data = result , success=true});
				}
				else
				{
                    return Json(new { data = result, success = false });
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
		public async Task<IActionResult> AddExpense()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				ViewBag.YearList = _ifinancialYearTermWiseFee.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.SubheadList = _ifinancialYearTermWiseFee.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.ChequeList = _ifinancialYearTermWiseFee.BindCheques().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
                

                return View();

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddExpense(ExpenseEntity ep)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
                ep.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetExpenseData()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var ep = await _ifinancialYearTermWiseFee.GetExpenseData();
				return View(ep);

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}
		[HttpGet]
		public async Task<IActionResult> AddChequeNo(string? chequeid)
		{
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{ 
				ChequeMaster chequeMaster = new ChequeMaster();
                return View(chequeMaster);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
		[HttpPost]
        public async Task<IActionResult> AddCheque(ChequeMaster cm)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("chequeno", typeof(string));
                foreach (var data in cm.chequenolist)
                {
                    dataTable.Rows.Add(data.chequeno);
                }
                cm.dt = dataTable;
                cm.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _ifinancialYearTermWiseFee.AddChequeNo(cm);
                if (result.Id > 0)
                {
                    TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
                }
                else
                {
                    TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
                }
                return RedirectToAction("AddChequeNo");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

		[HttpGet]
		public async Task<IActionResult> GetChequeMaster()
		{
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var ep = await _ifinancialYearTermWiseFee.GetChequeDate();
                return View(ep);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
           
		}

		public async Task<IActionResult> UpdateChequeById(string chequeid)
		{
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                
                    var result = await _ifinancialYearTermWiseFee.CheckExpenseForCheque(Convert.ToInt64(chequeid));
                    if (result.Id > 0)
                    {
                        TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
                        return RedirectToAction("GetChequeMaster");
                    }
                    else
                    {
                        var ck = await _ifinancialYearTermWiseFee.GetChequeDateById(Convert.ToInt64(chequeid));
                        return View(ck);
                    }
                
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

		[HttpPost]
        public async Task<IActionResult> UpdateChequeById(ChequeMaster cm)
		{
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                cm.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _ifinancialYearTermWiseFee.UpdateCheque(cm);
                if (result.Id > 0)
                {
                    TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
                }
                else
                {
                    TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
                }
                return RedirectToAction("GetChequeMaster");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

		public async Task<IActionResult> ParameterForm()
		{
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

	}
}
