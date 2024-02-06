using BlazorMinimalApis.Lib.Routing;
using BlazorMinimalApis.Mvc.Views.Home;

namespace BlazorMinimalApis.Mvc.Controllers;

public class HomeController : XController
{
	public IResult Index()
	{
		return View<Home>();
	}

	public IResult RandomNumber()
	{
		Random rnd = new();
		var num = rnd.Next();
		return View<_RandomNumber>(new { Num = num });
	}
}
