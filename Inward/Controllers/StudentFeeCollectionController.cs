using System.Data;
using GCM.Entity;
using GCM.Repository;
using GCM.Services.Abstraction;
using Inward.Entity;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;

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

        public StudentFeeCollectionController(IStudentFeeCollectionService studentFeeCollectionService, ILoginService userLoginService, IConfiguration config)
        {
            _studentFeeCollectionService = studentFeeCollectionService ?? throw new ArgumentNullException(nameof(studentFeeCollectionService));
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> ViewStudentFeeCollection()
        {
            var studentsFeeCollection = await _studentFeeCollectionService.GetStudentFeeCollectionList();
            return View(studentsFeeCollection);
        }
        // GET: StudentFeeCollectionController
        public ActionResult AddStudentFeeCollection()
        {
            StudentFeeCollection sft = new StudentFeeCollection();
            ViewBag.YearList = _studentFeeCollectionService.BindFinancialYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.TermList = _studentFeeCollectionService.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.SubheadList = _studentFeeCollectionService.BindSubhead().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            return View(sft);
        }

        // GET: StudentFeeCollectionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentDetails(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return Json(new { success = false, message = "Student number is required." });
            }

            var students = await _studentFeeCollectionService.GetStudentList();

            if (students == null || !students.Any())
            {
                return Json(new { success = false, message = "No students found." });
            }

            var student = students.FirstOrDefault(s => s.applicationno == number);

            if (student != null)
            {
                return Json(new { success = true, student });
            }
            else
            {
                return Json(new { success = false, message = "Student not found." });
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
            var fees = await _studentFeeCollectionService.FeeDetails(termId, financialYearId, studentid);

            var result = fees.Select(f => new
            {
                subHeadId = f.SubHeadId,
                subHeadName = f.subheadname,
                amount = f.fees
            });

            return Json(new { success = result.Any(), fees = result });
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentFeeCollection(StudentFeeCollection model, List<FeeDetail> FeeDetails, decimal TotalFees)
        {
            if (model == null || string.IsNullOrEmpty(model.StudentId) || model.FinancialYearId == 0 || model.TermId == 0)
            {
                ModelState.AddModelError("", "Invalid data. Please fill all required fields.");
                return View("AddStudentFeeCollection", model); // Return the same view with validation errors
            }

            try
            {
                // Create a new entity instance
                if (model.FormType == "form1")
                {
                    DataTable feeDetailsTable = new DataTable();
                    feeDetailsTable.Columns.Add("SubheadId", typeof(int));
                    feeDetailsTable.Columns.Add("Amount", typeof(decimal));

                    foreach (var feeDetail in FeeDetails)
                    {
                        feeDetailsTable.Rows.Add(feeDetail.subheadid, feeDetail.Amount);
                    }
                    model.feesdetails = feeDetailsTable;

                    // Ensure AddFeeCollection is asynchronous and awaited
                    var regResponse = await _studentFeeCollectionService.AddFeeCollection(model, TotalFees);
                }
                else
                {

                    DataTable feeDetailsTable = new DataTable();
                    feeDetailsTable.Columns.Add("SubheadId", typeof(int));
                    feeDetailsTable.Columns.Add("Amount", typeof(decimal));

                    foreach (var feeDetail in model.FeeDetailLists)
                    {
                        feeDetailsTable.Rows.Add(feeDetail.subheadid, feeDetail.Amount);
                    }
                    model.feesdetails = feeDetailsTable;

                    // Ensure AddFeeCollection is asynchronous and awaited
                    var regResponse = await _studentFeeCollectionService.AddFeeCollection(model, TotalFees);
                }

                return RedirectToAction("ViewStudentFeeCollection");
            }
            catch (Exception ex)
            {
                // Handle exception
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("AddStudentFeeCollection", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportStudentFeeCollectionReport1(string? studentid)
        {

            long stdid = 0;
            stdid = Convert.ToInt64(studentid);
            var studentsFeeCollection = await _studentFeeCollectionService.GetReport_studentFeeMaster(stdid);

            var report = new LocalReport();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentFeeCollectionReport.rdlc");
            report.ReportPath = path;

            report.DataSources.Add(new ReportDataSource("studentfeecollection", studentsFeeCollection));

            var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

            return File(result, "application/pdf", "StudentFeeCollectionReport.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ExportStudentFeeCollectionReport(string? studentid)
        {
            long stdid = 0;
            stdid = Convert.ToInt64(studentid);

            // Fetch the report data
            var studentsFeeCollection = await _studentFeeCollectionService.GetReport_studentFeeMaster(stdid);

            // Calculate the total GovAmount
            var totalGovAmount = studentsFeeCollection.Sum(x => x.GovAmount);
            
            foreach (var item in studentsFeeCollection)
            {
                item.GovernmentTotal =Convert.ToString(totalGovAmount); // Add this property in your model if not already
            }

            // Create the local report
            var report = new LocalReport();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentFeeCollectionReport.rdlc");
            report.ReportPath = path;

            // Add the data source
            report.DataSources.Add(new ReportDataSource("studentfeecollection", studentsFeeCollection));

            // Set the parameter for total GovAmount
            report.SetParameters(new ReportParameter("TotalGovAmount", totalGovAmount.ToString("F2")));

            // Render the report as PDF
            var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

            return File(result, "application/pdf", "StudentFeeCollectionReport.pdf");
        }
    }
}