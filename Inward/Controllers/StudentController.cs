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
              

        [HttpGet]
        public IActionResult ViewStudents()
        {
            // Get the list of students from TempData
            var students = TempData["Students"] as List<Student>;

            // If TempData is null, initialize an empty list
            if (students == null)
            {
                students = new List<Student>();
            }

            return View(students);
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
            return RedirectToAction(nameof(ViewStudents));
        }


        private List<Student> GetStudentsFromExcel(string filePath)
        {
            var students = new List<Student>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var student = new Student
                    {
                        name = worksheet.Cells[row, 1].Text,
                        mobileno = worksheet.Cells[row, 2].Text,
                        email = worksheet.Cells[row, 3].Text,
                        //gendername = worksheet.Cells[row, 4].Text,
                        genderid = int.Parse(worksheet.Cells[row, 4].Text),
                        categoryid = int.Parse(worksheet.Cells[row, 5].Text),
                        categoryname = worksheet.Cells[row, 6].Text,
                        enrolmentno = worksheet.Cells[row, 7].Text,
                        applicationno = worksheet.Cells[row, 8].Text,
                        userid = worksheet.Cells[row, 9].Text
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

    }
}
