﻿using System.Data;
using System.Security.Claims;
using GCM.Entity;
using GCM.Repository;
using GCM.Repository.Abstraction;
using GCM.Services.Abstraction;
using GCM.wwwroot.Dataset;
using Inward.Common;
using Inward.Entity;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using Microsoft.SqlServer.Server;

namespace GCM.Controllers
{
	public class StudentFeeCollectionController : Controller
	{
		private readonly IStudentFeeCollectionService _studentFeeCollectionService;
		private readonly ILoginService _userLoginService;
		private readonly IConfiguration _config;
		private readonly string _baseURL;
		private readonly string _Login;
		private readonly List<Student> _students = new List<Student>();
		private readonly IWebHostEnvironment _env;

		
		public StudentFeeCollectionController(IStudentFeeCollectionService studentFeeCollectionService, ILoginService userLoginService, IConfiguration config, IWebHostEnvironment env)
		{
			_studentFeeCollectionService = studentFeeCollectionService ?? throw new ArgumentNullException(nameof(studentFeeCollectionService));
			_userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
			_config = config; 
			_env = env;

		}
		[HttpGet]
		public async Task<IActionResult> ViewStudentFeeCollection()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var studentsFeeCollection = await _studentFeeCollectionService.GetStudentFeeCollectionList();
				return View(studentsFeeCollection);

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}
		// GET: StudentFeeCollectionController
		public ActionResult AddStudentFeeCollection()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				StudentFeeCollection sft = new StudentFeeCollection();
				ViewBag.YearList = _studentFeeCollectionService.BindFinancialYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.TermList = _studentFeeCollectionService.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				ViewBag.SubheadList = _studentFeeCollectionService.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
				return View(sft);

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}


        public ActionResult AddMannualStudentFeeCollection()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                StudentFeeCollection sft = new StudentFeeCollection();
                ViewBag.YearList = _studentFeeCollectionService.BindFinancialYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
                ViewBag.TermList = _studentFeeCollectionService.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
                ViewBag.SubheadList = _studentFeeCollectionService.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
                return View(sft);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        // GET: StudentFeeCollectionController/Details/5
        public ActionResult Details(int id)
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetStudentDetails(string number)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				if (string.IsNullOrWhiteSpace(number))
				{
					return Json(new { success = false, message = "Student number is required." });
				}

				var student = await _studentFeeCollectionService.GetStudentListbynumber(number);

                if (student != null)
				{
					return Json(new { success = true, student });
				}
				else
				{
					return Json(new { success = false, message = "Student not found." });
				}

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}



		// POST: StudentFeeCollectionController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: StudentFeeCollectionController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: StudentFeeCollectionController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: StudentFeeCollectionController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: StudentFeeCollectionController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}


		[HttpGet]
		public async Task<JsonResult> GetFeesDetails(int termId, int financialYearId, int studentid)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var fees = await _studentFeeCollectionService.FeeDetails(termId, financialYearId, studentid);

				var result = fees.Select(f => new
				{
					SubHeadId = f.SubHeadId,
					subHeadName = f.subheadname,
					amount = f.fees
				});

				return Json(new { success = result.Any(), fees = result });

			}
			else
			{
				return Json(new { success = false, message = "User not authenticated." });
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddStudentFeeCollection(StudentFeeCollection model, List<FeeDetail> FeeDetails, decimal TotalFees)
		{
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				if (model == null || string.IsNullOrEmpty(model.StudentId) || model.FinancialYearId == 0 || model.TermId == 0)
				{
					var msg = "Invalid data. Please fill all required fields.";
					TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

					return RedirectToAction("ViewStudentFeeCollection", "StudentFeeCollection");
				}

				try
				{
					// Create a new entity instance

					DataTable feeDetailsTable = new DataTable();
					feeDetailsTable.Columns.Add("SubheadId", typeof(int));
					feeDetailsTable.Columns.Add("Amount", typeof(decimal));

                    if (model.FeeDetaillistMannually.Count == 0)
                    {
                        foreach (var feeDetail in FeeDetails)
                        {
                            feeDetailsTable.Rows.Add(feeDetail.SubheadId, feeDetail.Amount);
                        }
                        model.FormType = "form1";
                    }
					else
					{
                        foreach (var feeDetail in model.FeeDetaillistMannually)
                        {
                            feeDetailsTable.Rows.Add(feeDetail.SubHeadId, feeDetail.amount);
                        }
                        TotalFees =Convert.ToDecimal( model.FeeDetaillistMannually.Sum(f => f.amount));
						model.FormType = "form2";
                    }
                    // Assign DataTable to model
                    model.feesdetails = feeDetailsTable;

					
					// Ensure AddFeeCollection is asynchronous and awaited
					var regResponse = await _studentFeeCollectionService.AddFeeCollection(model, TotalFees);

					// Return a success or failure response as JSON
					if (regResponse.Id == 1)
					{
						// Success, return JSON with the success flag
						//return Json(new { success = true, message = "Data Save successfully." });
						var msg = "Data Save successfully.";
                        TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

                    }
					else if (regResponse.Id == -1)
					{
						// Success, return JSON with the success flag
						//return Json(new { success = true, message = "Record Already Exists" });
                        var msg = "Record Already Exists.";
                        TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

                    }
                    else
					{
						// Failure, return JSON with the failure flag
						//return Json(new { success = false, message = "Error while adding students." });
                        var msg = "Error while adding students.";
                        TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

                    }
					return RedirectToAction("ViewStudentFeeCollection", "StudentFeeCollection");   
                }
				catch (Exception ex)
				{

					DataTable feeDetailsTable = new DataTable();
					feeDetailsTable.Columns.Add("SubheadId", typeof(int));
					feeDetailsTable.Columns.Add("Amount", typeof(decimal));

					foreach (var feeDetail in model.FeeDetailLists)
					{
						feeDetailsTable.Rows.Add(feeDetail.SubheadId, feeDetail.Amount);
					}
					model.feesdetails = feeDetailsTable;

					// Ensure AddFeeCollection is asynchronous and awaited
					var regResponse = await _studentFeeCollectionService.AddFeeCollection(model, TotalFees);

					// Handle exception
					ModelState.AddModelError("", "An error occurred while processing your request.");
					return View("AddStudentFeeCollection", model);
				}

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}


		//[HttpPost]
		//public async Task<IActionResult> AddFinanceYearBalance(FinanceBalanceEntity ft,Decimal TotalFees)
		//{
		//    if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
		//    {
		//        DataTable dataTable = new DataTable();
		//        dataTable.Columns.Add("SubHeadId", typeof(long));
		//        dataTable.Columns.Add("amount", typeof(decimal));

		//        foreach (var data in ft.balanceLists)
		//        {
		//            dataTable.Rows.Add(data.SubHeadId, data.amount);
		//        }
		//        ft.fdt = dataTable;
		//        var result = await _studentFeeCollectionService.AddFeeCollection(ft, TotalFees);
		//        if (result.Msg == "Record Saved successfully.")
		//        {
		//            TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
		//        }
		//        else
		//        {
		//            TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.warning), "||");
		//        }
		//        return RedirectToAction("AddFinanceYearBalance");
		//    }
		//    else
		//    {
		//        return RedirectToAction("Login", "Login");
		//    }
		//}

		[HttpGet]
		public async Task<IActionResult> ExportStudentFeeCollectionReport(string? studentid, string? yearid, string? termid, string recieptno)
		{
			try
			{
				long stdid = 0;
				stdid = Convert.ToInt64(studentid);

				// Fetch the report data
				var studentsFeeCollection = await _studentFeeCollectionService.GetReport_studentFeeMaster(stdid, Convert.ToInt64(yearid), Convert.ToInt64(termid), recieptno);

				// Calculate the total GovAmount
				var totalGovAmount = studentsFeeCollection.Sum(x => x.GovAmount);

				foreach (var item in studentsFeeCollection)
				{
					item.GovernmentTotal = Convert.ToString(totalGovAmount); // Add this property in your model if not already
				}

				// Create the local report
				var report = new LocalReport();
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentFeeCollectionReport.rdlc");


				report.ReportPath = path;

				// Add the data source
				report.DataSources.Add(new ReportDataSource("studentfeecollection", studentsFeeCollection));


				
				// Render the report as PDF
				var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

				return File(result, "application/pdf", "StudentFeeCollectionReport.pdf");
			}
			catch (Exception ex)
			{
				string errorMessage;
				try
				{
					errorMessage = $"Error: {ex.Message}\n" +
						  $"Inner: {ex.InnerException?.Message}\n" +
						  $"StackTrace: {ex.StackTrace}";
				}
				catch
				{
					errorMessage = "Failed to write to log.";
				}

				return Content("An error occurred while generating the report.\n\n" + errorMessage, "text/plain");
			}
		}

        [HttpGet]
        public async Task<IActionResult> ExportFeeCollectionDetailReport(string fromdate, string todate)
        {
            var feecollectiondetail = await _userLoginService.GetFeeCollectionDetailReport(fromdate,todate);

            var report = new LocalReport();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "FeeCollectionDetailReport.rdlc");
            report.ReportPath = path;

            report.DataSources.Add(new ReportDataSource("dtFeeCollectionDetail", feecollectiondetail));

            var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

            return File(result, "application/pdf", "FeeCollectionDetailReport.pdf");

        }
    }
}

