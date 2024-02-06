using BlazorMinimalApis.Pages.Data;
using BlazorMinimalApis.Lib.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMinimalApis.Pages.Pages.Contacts;

public class ListContacts : XPage
{
    public List<Contact> Contacts = new();

    public IResult Get(HttpContext context)
	{
		Contacts = Database.Contacts;
		return Page<_ListContacts>();
	}

	public IResult GetSearch([FromQuery] string contactSearch)
	{
		Contacts = Database.Contacts
			.Where(x => x.Name.Contains(contactSearch, StringComparison.OrdinalIgnoreCase))
			.ToList();

		return Page<_SearchContacts>();
	}
}