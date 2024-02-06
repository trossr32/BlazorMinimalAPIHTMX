using BlazorMinimalApis.Lib.Routing;
using BlazorMinimalApis.Slices.Applications.Home.Views;

namespace BlazorMinimalApis.Slices.Applications.Home.Handlers;

public class ShowHome : XHandler
{
	public IResult Show() => View<Show>();

    public IResult RandomNumber()
	{
		var rnd = new Random();
		var num = rnd.Next();

		return View<_RandomNumber>(new { Num = num });
	}
}