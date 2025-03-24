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
       
        //[HttpPost]
        //public async Task<IActionResult> AddStudents(Student st)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //            st.userid = userIdClaim.Value;
        //            var regResponse = await _studentService.AddStudent(st);
        //            var msg = regResponse.Msg;
        //            if (regResponse.Id > 0)
        //            {
        //                TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

        //            }
        //        }
        //        return RedirectToAction("Index", "Inward");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> ViewStudents()
            {
            // Check if there are students in TempData
            if (TempData.ContainsKey("Students"))
            {
                var studentsJson = TempData["Students"].ToString();
                var students = JsonSerializer.Deserialize<List<Student>>(studentsJson); // Assuming Student is your model
                return View(students);
            }

            // If no students are in TempData, fetch from the database/service
            var defaultStudents = await _userLoginService.GetStudentList();
            return View(defaultStudents);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var students = GetStudentsFromExcel(filePath);

                //var studentTable = new DataTable();
                //foreach (var student in students)
                //{
                //    studentTable.Rows.Add(student.studentid, student.name, student.mobileno, student.email,
                //                          student.gendername, student.categoryid, student.categoryname,
                //                          student.enrolmentno, student.applicationno, student.userid);
                //}
                
                //var regResponse = await _studentService.AddStudent(studentTable);
                
                _students.AddRange(students);

                TempData["Students"] = JsonSerializer.Serialize(students);
            }
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
                        studentid = int.Parse(worksheet.Cells[row, 1].Text),
                        name = worksheet.Cells[row, 2].Text,
                        mobileno = worksheet.Cells[row, 3].Text,
                        email = worksheet.Cells[row, 4].Text,
                        gendername = worksheet.Cells[row, 5].Text,
                        categoryid = int.Parse(worksheet.Cells[row, 6].Text),
                        categoryname = worksheet.Cells[row, 7].Text,
                        enrolmentno = worksheet.Cells[row, 8].Text,
                        applicationno = worksheet.Cells[row, 9].Text,
                        userid = worksheet.Cells[row, 10].Text
                    };
                    students.Add(student);
                }
            }
            return students;
        }
    }
}
