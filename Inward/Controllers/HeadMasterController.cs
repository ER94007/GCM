using GCM.Entity;
using GCM.Services;
using GCM.Services.Abstraction;
using Inward.Common;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GCM.Controllers
{
	public class HeadMasterController : Controller
	{
		private readonly ILoginService _userLoginService;
		private readonly IConfiguration _config;
		public HeadMasterController(ILoginService userLoginService, IConfiguration config)
		{
			_userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
			_config = config;
		}

		[HttpGet]
		public async Task<IActionResult> AddHeadMaster(string? HeadMasterId)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				HeadMasterEntity sh = new HeadMasterEntity();
				if (HeadMasterId != null)
				{
					sh = await _userLoginService.GetheadById(Convert.ToInt64(HeadMasterId));
				}

				return View(sh);

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}


		[HttpGet]
		public async Task<IActionResult> GetHeadList()
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var lst = await _userLoginService.GetHeadList();
				return View(lst);

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}

		public async Task<IActionResult> Deletehead(string HeadMasterId)
		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				var result = await _userLoginService.DeleteSubhead(Convert.ToInt64(HeadMasterId));
				if (result.Id > 0)
				{
					return Json(new { success = true, message = "Head deleted successfully." });
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
		[HttpPost]
		public async Task<IActionResult> AddHead(HeadMasterEntity sh)

		{
			if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
                sh.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _userLoginService.AddUpdatehead(sh);
				if (result.Id > 0)
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.success), "||");
				}
				else if (result.Id == -1)
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
				}
				else if (result.Id == -999)
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
				}
				else
				{
					TempData["SaveStatus"] = CommonMethods.ConcatString(result.Msg.ToString(), Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
				}
				return RedirectToAction("AddHeadMaster");

			}
			else
			{
				return RedirectToAction("Login", "Login");
			}
		}
	}
}
