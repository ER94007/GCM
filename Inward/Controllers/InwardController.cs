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
        private readonly IConfiguration _config;
        private readonly string _baseURL;
        private readonly string _Login;

        public InwardController(ILoginService userLoginService, IConfiguration config)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _config = config;
        }
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Login");
            }


            var FarmerName = _userLoginService.FillFarmer().Result
        .Select(c => new SelectListItem() { Text = c.Farmer_Nm, Value = c.Farmer_Id.ToString() }).ToList();
            ViewBag.FarmerNames = FarmerName;

            var GradeName = _userLoginService.FillGrade().Result
        .Select(c => new SelectListItem() { Text = c.Grade_Name, Value = c.Grade_Id.ToString() }).ToList();
            ViewBag.GradeNames = GradeName;


            //    var UnitName = _userLoginService.FillUnit().Result
            //.Select(c => new SelectListItem() { Text = c.Unit_Name, Value = c.Unit_Id.ToString() }).ToList();
            //    ViewBag.UnitNames = UnitName;
            ViewBag.UnitNames = GetUnitDetailsByGrade(string.Empty);

            var InwardLastData = await _userLoginService.GetLastInwardNo();

            ViewBag.InwardNo = Convert.ToInt32(InwardLastData.InwardNo) + 1;

            //if (InwardId != null)
            //{
            //    ViewBag.InwardId = InwardId;
            //    var lstInward = await _userLoginService.GetInwardsById(InwardId);
            //    var InwardMaster = lstInward;
            //    return View(InwardMaster);

            //}
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFarmerDetailsById(string farmerId)
        {
            // Fetch details based on farmerId from your service/repository
            FarmerEntity farmerDetails = await _userLoginService.GetFarmerDetailsById(farmerId);

            return Json(new { contactNo = farmerDetails.Contact_No, panCardNo = farmerDetails.Pan_Card_No });
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
        public async Task<IActionResult> Index(InwardEntity inwardEntity)
        {
            try
            {
                var InwardLastData = await _userLoginService.GetLastInwardNo();

                ViewBag.InwardNo = Convert.ToInt32(InwardLastData.InwardNo) + 1;

                var FarmerName = _userLoginService.FillFarmer().Result
            .Select(c => new SelectListItem() { Text = c.Farmer_Nm, Value = c.Farmer_Id.ToString() }).ToList();
                ViewBag.FarmerNames = FarmerName;

                var GradeName = _userLoginService.FillGrade().Result
.Select(c => new SelectListItem() { Text = c.Grade_Name, Value = c.Grade_Id.ToString() }).ToList();
                ViewBag.GradeNames = GradeName;

                ViewBag.UnitNames = GetUnitDetailsByGrade(string.Empty);


                if (ModelState.IsValid)
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                    inwardEntity.UserId = userIdClaim.Value;
                    var lstGradeDetails = new List<GradeDetail>();
                    if (inwardEntity.GradeDetails != null)
                    {
                        // CommonUtils.WriteToLogFile("Model is valid - requestDetails is not null", LogPath);
                        foreach (var Grade in inwardEntity.GradeDetails)
                        {
                            if (Grade.GradeName != null)
                            {
                                GradeDetail objGradeDetails = new GradeDetail();
                                objGradeDetails.GradeName = Grade.GradeName;
                                objGradeDetails.TotalWeight = Grade.TotalWeight;
                                objGradeDetails.AuctionRate = Grade.AuctionRate;
                                objGradeDetails.VendorRate = Grade.VendorRate;
                                objGradeDetails.VendorCode = Grade.VendorCode;
                                objGradeDetails.UnitName = Grade.UnitName;
                                objGradeDetails.TotalCarat = Grade.TotalCarat;
                                objGradeDetails.VendorRate = Grade.VendorRate;
                                objGradeDetails.NetAmount = Grade.NetAmount;

                                lstGradeDetails.Add(objGradeDetails);
                            }
                        }
                    }

                    if (lstGradeDetails.Count > 0)
                    {
                        // CommonUtils.WriteToLogFile("Model is valid - inside if to call database is not null", LogPath);

                        DataTable dtGradeData = CommonMethods.ToDataTable(lstGradeDetails);



                        var regResponse = await _userLoginService.InsertInwardDetail(inwardEntity, dtGradeData);
                        var msg = regResponse.Msg;
                     
                        if (regResponse.Id > 0)
                        {
                            TempData["SaveStatus"] = CommonMethods.ConcatString(msg.ToString(),
                         Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");

                            return RedirectToAction("GetInwardList", "Inward");

                        }
                     
                    }
                    else
                    {
                        //  CommonUtils.WriteToLogFile("Model is valid - inside else to call database is  null", LogPath);
                        //  TempData["Message"] = CommonUtils.ConcatString("Please Fill Atleast One Reservation Details", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                        //return RedirectToAction("Index", "Inward", new { InwardId = inwardEntity.InwardId.ToString() });                        //return RedirectToAction("Index", "Inward", new { InwardId = inwardEntity.InwardId.ToString() });
                        //TempData["SaveStatus"] = CommonMethods.ConcatString(msg,
                        //    Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
                        return RedirectToAction("Index", "Inward");  

                    }

                }
                {
                    return View(inwardEntity);

                }

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
