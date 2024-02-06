using BlazorMinimalApis.Lib.Routing;

namespace BlazorMinimalApis.Pages.Pages.Home;

public class Home : XPage 
{
    public int Num;

    public IResult Get() => Page<_Home>();

    public IResult RandomNumber()
    {
        Random rnd = new();
		Num = rnd.Next();
        return Page<_RandomNumber>();
    }
}