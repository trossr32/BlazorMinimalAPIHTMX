using BlazorMinimalApis.Slices.Data;
using BlazorMinimalApis.Lib.Routing;
using Microsoft.AspNetCore.Mvc;
using BlazorMinimalApis.Slices.Applications.Contacts.Views;

namespace BlazorMinimalApis.Slices.Applications.Contacts.Handlers;

public class SearchContacts : XHandler
{
    public IResult Search([FromQuery] string contactSearch)
    {
        var contacts = Database.Contacts
            .Where(x => x.Name.Contains(contactSearch, StringComparison.OrdinalIgnoreCase))
            .ToList();
        var model = new { Contacts = contacts };
        return View<ContactsTable>(model);
    }
}