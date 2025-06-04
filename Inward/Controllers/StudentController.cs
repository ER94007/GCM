using Inward.Entity;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using GCM.Services.Abstraction;
using OfficeOpenXml;
using System.Data;
using System.Text;
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCM.Entity;
using System.Security.Claims;
using System.IO;


namespace GCM.Controllers
{
	public class StudentController : Controller
	{
		// GET: StudentController
		private readonly IStudentService _studentService;
		private readonly IStudentFeeCollectionService _studentFeeCollectionService;
		private readonly ILoginService _userLoginService;
		private readonly IConfiguration _config;
		private readonly string _baseURL;
		private readonly string _Login;
		private readonly List<Student> _students = new List<Student>();

		public StudentController(IStudentService studentService, ILoginService userLoginService, IConfiguration config, IStudentFeeCollectionService feeCollectionService)
		{
			_studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
			_userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
			_config = config;
			_studentFeeCollectionService = feeCollectionService ?? throw new ArgumentNullException(nameof(feeCollectionService));
		}




		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Upload(IFormFile file, string FinancialYearId)
		//{
		//    if (file != null && file.Length > 0)
		//    {
		//        // Define the file path to save the uploaded file
		//        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

		//        // Save the file to the specified path
		//        using (var stream = new FileStream(filePath, FileMode.Create))
		//        {
		//            await file.CopyToAsync(stream); // Use async to ensure non-blocking
		//        }

		//        // Extract students from the Excel file (custom method)
		//        var students = GetStudentsFromExcel(filePath);

		//        // Convert the list of students to a DataTable
		//        DataTable dataTable = ConvertStudentsToDataTable(students);

		//        // Call the service to add students (pass the DataTable)
		//        var regResponse = await _studentService.AddStudent(dataTable, Convert.ToInt64(FinancialYearId),);

		//        }

		//    // Redirect to the ViewStudents action to display the students
		//    return RedirectToAction("ViewStudents", "Inward");
		//}



		private List<Student> GetStudentsFromExcel(string filePath)
		{
			var students = new List<Student>();
			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[0];
				var rowCount = worksheet.Dimension.Rows;
				var columnCount = worksheet.Dimension.Columns;

				// Find the column indexes based on the header row (first row)
				var headers = new Dictionary<string, int>();
				for (int col = 1; col <= columnCount; col++)
				{
					var headerText = worksheet.Cells[1, col].Text.Trim().ToLower(); // normalize to lower case
					headers[headerText] = col;
				}

				for (int row = 2; row <= rowCount; row++)
				{
					var student = new Student
					{
						name = worksheet.Cells[row, headers["name"]].Text,
						mobileno = worksheet.Cells[row, headers["mobileno"]].Text,
						email = worksheet.Cells[row, headers["email"]].Text,
						gendername = worksheet.Cells[row, headers["gendername"]].Text,
						genderid = int.Parse(worksheet.Cells[row, headers["gendername"]].Text),  // Assuming genderid is derived from gendername
						categoryid = int.Parse(worksheet.Cells[row, headers["categoryid"]].Text),
						categoryname = worksheet.Cells[row, headers["categoryname"]].Text,
						enrolmentno = worksheet.Cells[row, headers["enrolmentno"]].Text,
						applicationno = worksheet.Cells[row, headers["applicationno"]].Text,
						//userid = worksheet.Cells[row, headers["userid"]].Text
					};
					students.Add(student);
				}
			}
			return students;
		}


		private DataTable ConvertStudentsToDataTable(List<Student> students)
		{
			// Create a new DataTable object
			DataTable dataTable = new DataTable();

			// Add columns to the DataTable based on the properties of the Student class
			dataTable.Columns.Add("name", typeof(string));
			dataTable.Columns.Add("mobileno", typeof(string));
			dataTable.Columns.Add("email", typeof(string));
			//dataTable.Columns.Add("gendername", typeof(string));

			dataTable.Columns.Add("categoryid", typeof(int));
			//ataTable.Columns.Add("categoryname", typeof(string));
			dataTable.Columns.Add("enrolmentno", typeof(string));
			dataTable.Columns.Add("applicationno", typeof(string));
			//dataTable.Columns.Add("userid", typeof(string));
			// Add other columns based on your Student properties if necessary

			// Loop through the list of students and add rows to the DataTable
			foreach (var student in students)
			{
				var row = dataTable.NewRow();
				row["name"] = student.name;  // Replace with actual property names
				row["mobileno"] = student.mobileno;
				row["email"] = student.email;
				//row["gendername"] = student.gendername;
				row["categoryid"] = student.categoryid;
				// row["categoryname"] = student.categoryname;
				row["enrolmentno"] = student.enrolmentno;
				row["applicationno"] = student.applicationno;
				//row["userid"] = student.userid;
				// Add other properties as needed
				dataTable.Rows.Add(row);
			}

			return dataTable;
		}


		public IActionResult show(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				// Create a new list to store parsed student data
				List<Student> students = new List<Student>();

				// To check for duplicates, create sets to track existing email, applicationno, and enrolmentno
				HashSet<string> existingEmails = new HashSet<string>();
				HashSet<string> existingApplicationNos = new HashSet<string>();
				HashSet<string> existingEnrolmentNos = new HashSet<string>();

				// Create lists to store duplicate entries
				List<string> duplicateEmails = new List<string>();
				List<string> duplicateApplicationNos = new List<string>();
				List<string> duplicateEnrolmentNos = new List<string>();

				// Create lists to store errors for invalid gender, category, and missing fields
				List<string> invalidGenders = new List<string>();
				List<string> invalidCategories = new List<string>();
				List<string> missingFields = new List<string>();

				using (var stream = new MemoryStream())
				{
					file.CopyTo(stream);
					using (var package = new ExcelPackage(stream))
					{
						var worksheet = package.Workbook.Worksheets[0]; // Get the first sheet

						if (worksheet?.Dimension == null)
						{
							return Json(new { success = false, message = "The uploaded file is empty or has no data." });
						}

						var rowCount = worksheet.Dimension.Rows;
						var colCount = worksheet.Dimension.Columns;

						// Step 1: Read headers and map column indexes dynamically
						var headers = new Dictionary<string, int>();

						for (int col = 1; col <= colCount; col++)
						{
							string header = worksheet.Cells[1, col].Text.Trim().ToLower();
							headers[header] = col;
						}

						// Step 2: Loop through rows and extract data based on dynamic header mapping
						for (int row = 2; row <= rowCount; row++)
						{
							var student = new Student
							{
								name = worksheet.Cells[row, headers["name"]].Text,
								mobileno = worksheet.Cells[row, headers["mobileno"]].Text,
								email = worksheet.Cells[row, headers["email"]].Text,
								gendername = worksheet.Cells[row, headers["gender"]].Text,
								categoryname = worksheet.Cells[row, headers["categoryname"]].Text,
								enrolmentno = worksheet.Cells[row, headers["enrolmentno"]].Text,
								applicationno = worksheet.Cells[row, headers["applicationno"]].Text,
								//userid = worksheet.Cells[row, headers["userid"]].Text
							};

							// Check if any required field is empty
							if (string.IsNullOrEmpty(student.name))
							{
								missingFields.Add($"Name is missing for student in row {row}");
							}

							if (string.IsNullOrEmpty(student.mobileno))
							{
								missingFields.Add($"Mobile number is missing for student in row {row}");
							}

							if (string.IsNullOrEmpty(student.email))
							{
								missingFields.Add($"Email is missing for student in row {row}");
							}

							if (string.IsNullOrEmpty(student.gendername))
							{
								missingFields.Add($"Gender is missing for student in row {row}");
							}

							if (string.IsNullOrEmpty(student.categoryname))
							{
								missingFields.Add($"Category is missing for student in row {row}");
							}

							if (string.IsNullOrEmpty(student.enrolmentno))
							{
								missingFields.Add($"Enrolment number is missing for student in row {row}");
							}

							if (string.IsNullOrEmpty(student.applicationno))
							{
								missingFields.Add($"Application number is missing for student in row {row}");
							}

							// Check for invalid gender values
							if (!new[] { "female", "other", "male" }.Contains(student.gendername.ToLower()))
							{
								// Add invalid gender to the list
								invalidGenders.Add($"Invalid gender '{student.gendername}' for student {student.name} (Row {row})");
							}

							// Check for invalid category values
							if (!new[] { "general", "sc", "st", "obc", "ews", "sebc" }.Contains(student.categoryname.ToLower()))
							{
								// Add invalid category to the list
								invalidCategories.Add($"Invalid category '{student.categoryname}' for student {student.name} (Row {row})");
							}

							// Check for duplicates
							if (existingEmails.Contains(student.email))
							{
								// Add duplicate email to the list
								duplicateEmails.Add(student.email);
							}
							else
							{
								existingEmails.Add(student.email);
							}

							if (existingApplicationNos.Contains(student.applicationno))
							{
								// Add duplicate application number to the list
								duplicateApplicationNos.Add(student.applicationno);
							}
							else
							{
								existingApplicationNos.Add(student.applicationno);
							}

							//if (existingEnrolmentNos.Contains(student.enrolmentno))
							//{
							//    // Add duplicate enrolment number to the list
							//    duplicateEnrolmentNos.Add(student.enrolmentno);
							//}
							//else
							//{
							//    existingEnrolmentNos.Add(student.enrolmentno);
							//}

							// Add the student to the list (if no duplicates and no missing fields)
							students.Add(student);
						}
					}
				}

				// Check if there are any validation errors for gender, category, or missing fields
				if (invalidGenders.Count > 0 || invalidCategories.Count > 0 || missingFields.Count > 0)
				{
					return Json(new
					{
						success = false,
						errors = new
						{
							genderErrors = invalidGenders,
							categoryErrors = invalidCategories,
							missingFields = missingFields
						}
					});
				}

				// Check if there are any duplicates and return them in the response
				if (duplicateEmails.Count > 0 || duplicateApplicationNos.Count > 0 || duplicateEnrolmentNos.Count > 0)
				{
					return Json(new
					{
						success = false,
						duplicates = new
						{
							emails = duplicateEmails,
							applicationnos = duplicateApplicationNos,
							enrolmentnos = duplicateEnrolmentNos
						}
					});
				}

				// Return the parsed data as JSON if no errors
				return Json(new { success = true, students });
			}

			return BadRequest("No file uploaded.");
		}

		[HttpPost]
		public async Task<IActionResult> SaveStudents([FromBody] StudentUploadModel model)
		{
			try
			{
				if (model.Students == null || model.Students.Count == 0)
					return Json(new { success = false, message = "No student data received." });

				var studentsDataTable = ConvertToDataTable(model.Students);
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

				var regResponse = await _studentService.AddStudent(studentsDataTable, Convert.ToInt64(model.YearId), Convert.ToInt64(model.semid), Convert.ToInt64(userId));

				return Json(new
				{
					success = regResponse.Id == 1,
					message = regResponse.Id == 1 ? "Students added successfully." : "Error while adding students."
				});
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "An error occurred: " + ex.Message });
			}
		}


		public static DataTable ConvertToDataTable(List<Student> students)
		{
			var dataTable = new DataTable();

			// Define columns for DataTable based on the Student properties
			dataTable.Columns.Add("Name", typeof(string));
			dataTable.Columns.Add("MobileNo", typeof(string));
			dataTable.Columns.Add("Email", typeof(string));
			dataTable.Columns.Add("CategoryId", typeof(int));  // Adding CategoryId column
			dataTable.Columns.Add("GenderId", typeof(int));    // Adding GenderId column
			dataTable.Columns.Add("ApplicationNo", typeof(string));
			dataTable.Columns.Add("EnrollmentNo", typeof(string));

			// Populate DataTable with the student data
			foreach (var student in students)
			{
				var row = dataTable.NewRow();
				row["Name"] = student.name;
				row["MobileNo"] = student.mobileno;
				row["Email"] = student.email;

				// Set CategoryId based on category name
				if (student.categoryname.ToLower() == "sc")
				{
					row["CategoryId"] = 3; // SC Category
				}
				else if (student.categoryname.ToLower() == "st")
				{
					row["CategoryId"] = 4; // ST Category
				}
				else if (student.categoryname.ToLower() == "general")
				{
					row["CategoryId"] = 1; // General Category
				}
				else if (student.categoryname.ToLower() == "obc")
				{
					row["CategoryId"] = 2; // OBC Category
				}
				else if (student.categoryname.ToLower() == "ews")
				{
					row["CategoryId"] = 5; // EWS Category
				}
				else if (student.categoryname.ToLower() == "sebc")
				{
					row["CategoryId"] = 2; // SEBC Category
				}
				else
				{
					row["CategoryId"] = DBNull.Value; // Unknown category
				}

				// Set GenderId based on gender name
				if (student.gendername.ToLower() == "male")
				{
					row["GenderId"] = 1; // Male
				}
				else if (student.gendername.ToLower() == "female")
				{
					row["GenderId"] = 2; // Female
				}
				else if (student.gendername.ToLower() == "transgender")
				{
					row["GenderId"] = 3; // Transgender
				}
				else if (student.gendername.ToLower() == "other")
				{
					row["GenderId"] = 3; // Other
				}
				else
				{
					row["GenderId"] = DBNull.Value; // Unknown gender
				}

				row["ApplicationNo"] = student.applicationno;
				row["EnrollmentNo"] = student.enrolmentno;

				dataTable.Rows.Add(row);
			}

			return dataTable;
		}

		[HttpGet]
		public IActionResult DownloadExcel()
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "ExcelDemo.xlsx");
			var fileBytes = System.IO.File.ReadAllBytes(filePath);
			var fileName = "DemoData.xlsx";

			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}


		[HttpGet]
		public async Task<IActionResult> ViewStudents()
		{
			var students = await _userLoginService.GetStudentList();

			var report = new LocalReport();
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentReport.rdlc");
			report.ReportPath = path;

			report.DataSources.Add(new ReportDataSource("dbStudentdetails", students));

			string mimeType, encoding, fileNameExtension;
			string[] streams;
			Warning[] warnings;

			var renderedBytes = report.Render("HTML5", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
			ViewBag.ReportHtml = Encoding.UTF8.GetString(renderedBytes);

			return View(students);
		}

		// Export PDF
		[HttpGet]
		public async Task<IActionResult> ExportStudentReport()
		{
			var students = await _userLoginService.GetStudentList();

			var report = new LocalReport();
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentReport.rdlc");
			report.ReportPath = path;

			report.DataSources.Add(new ReportDataSource("dbStudentdetails", students));

			var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

			return File(result, "application/pdf", "StudentReport.pdf");
		}
		[HttpGet]
		public async Task<IActionResult> BindYear()
		{
			var yearlist = _studentFeeCollectionService.BindFinancialYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
			return Json(new { success = true, data = yearlist });
		}
		[HttpGet]
		public async Task<IActionResult> BindStudents(string yearid)
		{
			var yearlist = _studentFeeCollectionService.BindStudents(Convert.ToInt64(yearid)).Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
			return Json(new { success = true, data = yearlist });
		}
		[HttpGet]
		public async Task<IActionResult> BindTerm()
		{
			var yearlist = _studentFeeCollectionService.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
			return Json(new { success = true, data = yearlist });
		}
		[HttpGet]
		public async Task<IActionResult> BindReciept(string studentid)
		{
			var yearlist = _studentFeeCollectionService.BindReciept(Convert.ToInt64(studentid)).Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
			return Json(new { success = true, data = yearlist });
		}
		[HttpGet]
		public async Task<IActionResult> ExportStudentFeeDetailReport(int YearId, int TermId)
		{
			try
			{
				var students = await _userLoginService.GetStudentFeeDetailReport(YearId, TermId);

				var report = new LocalReport();
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StudentDetailReport.rdlc");
				report.ReportPath = path;

				report.DataSources.Add(new ReportDataSource("Report_studentsFeeMasterDetail_Rutvik", students));

				var result = report.Render("PDF", null, out var mimeType, out var encoding, out var filenameExtension, out var streams, out var warnings);

				return File(result, "application/pdf", "FeeCollectionReport(FCR).pdf");
			}
			catch (Exception ex)
			{
				

				return StatusCode(500, ex.Message);
			}

		}

		// Export PDF

	}
}
