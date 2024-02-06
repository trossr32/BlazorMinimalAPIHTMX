using BlazorMinimalApis.Slices.Data;
using BlazorMinimalApis.Lib.Routing;
using Microsoft.AspNetCore.Mvc;
using BlazorMinimalApis.Slices.Applications.Contacts.Views;
using BlazorMinimalApis.Slices.Applications.Contacts.Mappers;
using BlazorMinimalApis.Slices.Applications.Contacts.Models;

namespace BlazorMinimalApis.Slices.Applications.Contacts.Handlers;

public class EditContact : XHandler
{

	public IResult Edit(int id)
	{
		var record = Database.Contacts.First(x => x.Id == id);
		var form = new ContactMapper().ContactToEditContactForm(record);
		var model = new { Form = form };

		return View<Edit>(model);
	}

	public IResult Update(int id, [FromForm] EditContactForm form)
	{
		var validation = Validate(form);

		if (validation.HasErrors)
		{
			var model = new { Form = form };
			return View<Edit>(model);
		}

		var oldContact = Database.Contacts.First(x => x.Id == id);
		var newContact = new ContactMapper().EditContactFormToContact(form);
		newContact.Id = oldContact.Id;
		Database.Contacts.Add(newContact);
		Database.Contacts.Remove(oldContact);

		return Redirect($"/contacts/{newContact.Id}/edit");
	}
}