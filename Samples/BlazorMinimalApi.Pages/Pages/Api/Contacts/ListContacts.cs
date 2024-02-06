﻿using BlazorMinimalApis.Pages.Data;
using BlazorMinimalApis.Lib.Routing;

namespace BlazorMinimalApis.Pages.Pages.Api.Contacts;

public class ListContacts : XPage
{
	public IResult Get()
	{
		var data = new { Contacts = Database.Contacts };
		return Results.Ok(data);
	}
}
