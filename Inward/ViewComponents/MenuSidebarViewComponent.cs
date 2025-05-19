using GCM.Entity;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

public class MenuViewComponent : ViewComponent
{
	private readonly ILoginService _loginService;

	public MenuViewComponent(ILoginService loginService)
	{
		_loginService = loginService;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
		if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
		{
			return View(new List<MenuViewModel>());
		}

		var menus = await _loginService.GetMenusByUserIdAsync(userId);

		// Convert to hierarchy
		var menuDict = menus.ToDictionary(m => m.Id);
		foreach (var menu in menus)
			menu.Children = new List<MenuViewModel>();

		var hierarchicalMenus = new List<MenuViewModel>();
		foreach (var menu in menus)
		{
			if (menu.ParentId == 0)
				hierarchicalMenus.Add(menu);
			else if (menuDict.TryGetValue(menu.ParentId.Value, out var parent))
				parent.Children.Add(menu);
		}

		return View(hierarchicalMenus);
	}
}
