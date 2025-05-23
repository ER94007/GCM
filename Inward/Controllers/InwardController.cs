﻿using GCM.Entity;
using GCM.Services.Abstraction;
using Inward.Common;
using Inward.Entity;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Web;

namespace Inward.Controllers
{
    public class InwardController : Controller
    {
        private readonly ILoginService _userLoginService;
        private readonly IFinancialYearTermWiseFeeService _ifinancialYearTermWiseFeeService;
        private readonly IConfiguration _config;
        private readonly string _baseURL;
        private readonly string _Login;

        public InwardController(ILoginService userLoginService, IConfiguration config, IFinancialYearTermWiseFeeService financialYearTermWiseFeeService)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _config = config;
            _ifinancialYearTermWiseFeeService = financialYearTermWiseFeeService;
        }
        public async Task<IActionResult> Index(string? studentid)
        {

           

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Student student = new Student();

            
            int stdid = 0;
            if (!string.IsNullOrEmpty(studentid))
            {
                try
                {
                    // Decode the URL-encoded string before decryption
                    string decryptedId = EncryptionHelper.Decrypt(Uri.UnescapeDataString(studentid));
                    stdid = int.Parse(decryptedId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error decrypting student ID: " + ex.Message);
                    return BadRequest("Invalid student ID.");
                }
            }

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (studentid != null)
            {
                student = await _userLoginService.GetStudentByid(Convert.ToInt64(stdid));
            }
            ViewBag.GenderList = _userLoginService.BindGender().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.CategoryList = _userLoginService.BindCategory().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList(); ;
            ViewBag.YearList = _ifinancialYearTermWiseFeeService.BindYear().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            ViewBag.TermList = _ifinancialYearTermWiseFeeService.BindTerm().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();
            
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudents()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                    var students = await _userLoginService.GetStudentList();
                return View(students);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(FileStream studentfile, string FinancialYearId)
        {
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteStudent(long studentid)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                    var result = _userLoginService.DeleteStudent(studentid);
                if(result.Id > 0)
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

        public async Task<IActionResult> AddSubHead(string? subheadid)
        {

            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                    SubHeadEntity sh = new SubHeadEntity();
                if (subheadid != null)
                {

                     sh = await _userLoginService.GetSubheadById(Convert.ToInt64(subheadid));
                }
				ViewBag.HeadList = _ifinancialYearTermWiseFeeService.BindHeads().Result.Select(c => new SelectListItem() { Text = c.Text, Value = c.Value.ToString() }).ToList();

				return View(sh);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSubHead(SubHeadEntity sh)
        {
            sh.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var result = await _userLoginService.AddUpdateSubhead(sh);
                if (result.Id > 0)
                {
                    TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
                }
                else if (result.Id==-1)
                {
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");

				}
				else if (result.Id==-999)
                {
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");

				}
				else
                {
                    TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
                }
                    return RedirectToAction("AddSubHead");

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSubHeadList()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                    var lst = await _userLoginService.GetSubHeadList();
                return View(lst);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetFarmerDetailsById(string farmerId)
        {
            // Fetch details based on farmerId from your service/repository
            FarmerEntity farmerDetails = await _userLoginService.GetFarmerDetailsById(farmerId);

            return Json(new { contactNo = farmerDetails.Contact_No, panCardNo = farmerDetails.Pan_Card_No });
        }
        public async Task<IActionResult> DeleteSubhead(string subheadid)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                    var result = await _userLoginService.DeleteSubhead(Convert.ToInt64(subheadid));
                if (result.Id > 0)
                {
                    return Json(new { success = true, message = "Subhead deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "This subhead contains dependencies." });
                }

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public List<SelectListItem> GetUnitDetailsByGrade(string gradeId)
        {
            // Ensure gradeId is not null or empty before proceeding
            if (string.IsNullOrEmpty(gradeId))
            {
                return new List<SelectListItem>();
            }

            List<FarmerEntity> unitDetails = _userLoginService.FillUnit(gradeId).Result;

            var unitList = unitDetails.Select(u => new SelectListItem { Value = u.Unit_Id.ToString(), Text = u.Unit_Name }).ToList();

            return unitList;
        }

        [HttpPost]
        public async Task<IActionResult> Index(Student st)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                        st.userid= userIdClaim.Value;
                        var regResponse = await _userLoginService.AddUpdateStudent(st);
                        var msg = regResponse.Msg;
                        if (regResponse.Id > 0)
                        {
                        TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(),Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

                        }
                     
                 }
                 
                       
                        return RedirectToAction("Index", "Inward");   

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> GetInwardList()
        {
            var modelList = await _userLoginService.GetInwardList();
            return View(modelList);
        }
    }
}
