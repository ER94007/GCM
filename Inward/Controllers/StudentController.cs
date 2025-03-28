using Inward.Common;
using Inward.Entity;
using System.Security.Claims;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GCM.Services.Abstraction;
using GCM.Repository.Abstraction;
using System.Text.Json;
using OfficeOpenXml;
using System.Data;

namespace GCM.Controllers
{
    public class StudentController : Controller
    {
        // GET: StudentController
        private readonly IStudentService _studentService;
        private readonly ILoginService _userLoginService;
        private readonly IConfiguration _config;
        private readonly string _baseURL;
        private readonly string _Login;
        private readonly List<Student> _students = new List<Student>();

        public StudentController(IStudentService studentService, ILoginService userLoginService, IConfiguration config)
        {
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _config = config;
        }
              

                     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Define the file path to save the uploaded file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Use async to ensure non-blocking
                }

                // Extract students from the Excel file (custom method)
                var students = GetStudentsFromExcel(filePath);

                // Convert the list of students to a DataTable
                DataTable dataTable = ConvertStudentsToDataTable(students);

                // Call the service to add students (pass the DataTable)
                var regResponse = await _studentService.AddStudent(dataTable);

                }

            // Redirect to the ViewStudents action to display the students
            return RedirectToAction("ViewStudents", "Inward");
        }

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
                        userid = worksheet.Cells[row, headers["userid"]].Text
                    };
                    students.Add(student);
                }
            }
            return students;
        }


        //private List<Student> GetStudentsFromExcel(string filePath)
        //{
        //    var students = new List<Student>();
        //    using (var package = new ExcelPackage(new FileInfo(filePath)))
        //    {
        //        var worksheet = package.Workbook.Worksheets[0];
        //        var rowCount = worksheet.Dimension.Rows;

        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            var student = new Student
        //            {
        //                name = worksheet.Cells[row, 1].Text,
        //                mobileno = worksheet.Cells[row, 2].Text,
        //                email = worksheet.Cells[row, 3].Text,
        //                gendername = worksheet.Cells[row, 4].Text,
        //                // Set genderid based on gendername
        //                genderid = worksheet.Cells[row, 4].Text.ToLower() == "male" ? 1 :
        //                           worksheet.Cells[row, 4].Text.ToLower() == "female" ? 2 : 0,
        //                // Set categoryid based on categoryname
        //                categoryname = worksheet.Cells[row, 5].Text,
        //                categoryid = worksheet.Cells[row, 5].Text.ToLower() == "general" ? 1 :
        //                             worksheet.Cells[row, 5].Text.ToLower() == "ews" ? 2 :
        //                             worksheet.Cells[row, 5].Text.ToLower() == "sc" ? 3 :
        //                             worksheet.Cells[row, 5].Text.ToLower() == "st" ? 4 : 0,
        //                enrolmentno = worksheet.Cells[row, 6].Text,
        //                applicationno = worksheet.Cells[row, 7].Text,
        //                userid = worksheet.Cells[row, 8].Text
        //            };
        //            students.Add(student);
        //        }
        //    }
        //    return students;
        //}

        //private List<Student> GetStudentsFromExcel(string filePath)
        //{
        //    var students = new List<Student>();
        //    using (var package = new ExcelPackage(new FileInfo(filePath)))
        //    {
        //        var worksheet = package.Workbook.Worksheets[0];
        //        var rowCount = worksheet.Dimension.Rows;

        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            var student = new Student
        //            {
        //                name = worksheet.Cells[row, 1].Text,
        //                mobileno = worksheet.Cells[row, 2].Text,
        //                email = worksheet.Cells[row, 3].Text,
        //                gendername = worksheet.Cells[row, 4].Text,
        //                genderid = int.Parse(worksheet.Cells[row, 4].Text),
        //                categoryid = int.Parse(worksheet.Cells[row, 5].Text),
        //                categoryname = worksheet.Cells[row, 6].Text,
        //                enrolmentno = worksheet.Cells[row, 7].Text,
        //                applicationno = worksheet.Cells[row, 8].Text,
        //                userid = worksheet.Cells[row, 9].Text
        //            };
        //            students.Add(student);
        //        }
        //    }
        //    return students;
        //}

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
            dataTable.Columns.Add("userid", typeof(string));
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
                row["userid"] = student.userid;
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
                                userid = worksheet.Cells[row, headers["userid"]].Text
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

                            if (existingEnrolmentNos.Contains(student.enrolmentno))
                            {
                                // Add duplicate enrolment number to the list
                                duplicateEnrolmentNos.Add(student.enrolmentno);
                            }
                            else
                            {
                                existingEnrolmentNos.Add(student.enrolmentno);
                            }

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
        public async Task<IActionResult> SaveStudents(List<Student> students)
        {
            try
            {
                // Convert List<Student> to DataTable
                var studentsDataTable = ConvertToDataTable(students);

                // Call the AddStudent method and pass the DataTable
                var regResponse = await _studentService.AddStudent(studentsDataTable);

                // Handle the response and return a JSON result
                if (regResponse.Id == 1)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false});
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helper method to convert List<Student> to DataTable
        public static DataTable ConvertToDataTable(List<Student> students)
        {
            var dataTable = new DataTable();

            // Define columns for DataTable based on the Student properties
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("MobileNo", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Category", typeof(string));
            dataTable.Columns.Add("Gender", typeof(string));
            dataTable.Columns.Add("ApplicationNo", typeof(string));
            dataTable.Columns.Add("EnrollmentNo", typeof(string));

            // Populate DataTable with the student data
            foreach (var student in students)
            {
                var row = dataTable.NewRow();
                row["Name"] = student.name;
                row["MobileNo"] = student.mobileno;
                row["Email"] = student.email;
                row["Category"] = student.categoryname;
                row["Gender"] = student.gendername;
                row["ApplicationNo"] = student.applicationno;
                row["EnrollmentNo"] = student.enrolmentno;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

    }
}
