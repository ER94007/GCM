using System.Data;
using System.Security.Claims;
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
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                    if (model == null || string.IsNullOrEmpty(model.StudentId) || model.FinancialYearId == 0 || model.TermId == 0)
                {
                    ModelState.AddModelError("", "Invalid data. Please fill all required fields.");
                    return View("AddStudentFeeCollection", model); // Return the same view with validation errors
                }

                try
                {
                    // Create a new entity instance

                    DataTable feeDetailsTable = new DataTable();
                    feeDetailsTable.Columns.Add("SubheadId", typeof(int));
                    feeDetailsTable.Columns.Add("Amount", typeof(decimal));

                    foreach (var feeDetail in FeeDetails)
                    {
                        feeDetailsTable.Rows.Add(feeDetail.subheadid, feeDetail.Amount);
                    }

                    // Assign DataTable to model
                    model.feesdetails = feeDetailsTable;

                    // Ensure AddFeeCollection is asynchronous and awaited
                    var regResponse = await _studentFeeCollectionService.AddFeeCollection(model, TotalFees);

                    return RedirectToAction("ViewStudentFeeCollection");
                }
                catch (Exception ex)
                {
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


        [HttpGet]
        public async Task<IActionResult> ExportStudentReport()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null) { 
                    var studentsFeeCollection = await _studentFeeCollectionService.GetStudentFeeCollectionList();

                var report = new LocalReport();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentFeeCollectionReport.rdlc");
                report.ReportPath = path;

                report.DataSources.Add(new ReportDataSource("dbStudentdetails", studentsFeeCollection));

                var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

                return File(result, "application/pdf", "StudentReport.pdf");

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

    }
}
