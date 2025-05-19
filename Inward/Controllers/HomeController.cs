using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inward.Models;
using System.Data.SqlClient;
using GCM.Entity;
using System.IdentityModel.Claims;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Inward.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly ILoginService _loginService;


	public HomeController(ILogger<HomeController> logger, ILoginService loginService)
	{
		_logger = logger;
		_loginService = loginService;

	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
	


	//[HttpGet]
	//public async Task<IActionResult> Sidebar()
	//{
	//	if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
	//	{
	//		int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
	//		var menus = await _loginService.GetMenusByUserIdAsync(userId);
	//		return PartialView("_SidebarPartial", menus);
	//	}
	//	else
	//	{
	//		return RedirectToAction("Login", "Login");
	//	}
	//}
}
