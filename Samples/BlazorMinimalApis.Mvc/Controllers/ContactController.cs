﻿using BlazorMinimalApis.Lib.Routing;
using BlazorMinimalApis.Lib.Session;
using BlazorMinimalApis.Mvc.Data;
using BlazorMinimalApis.Mvc.Views.Contacts;
using Microsoft.AspNetCore.Mvc;
using Riok.Mapperly.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BlazorMinimalApis.Mvc.Controllers;

public class ContactController : XController
{
	public IResult List(HttpContext context)
	{
		var parameters = new { Contacts = Database.Contacts };
		return View<List>(parameters);
	}

	public IResult Search([FromQuery] string contactSearch)
	{
		var contacts = Database.Contacts
			.Where(x => x.Name.Contains(contactSearch, StringComparison.OrdinalIgnoreCase))
			.ToList();
		var model = new { Contacts = contacts };
		return View<_ContactsTable>(model);
	}

	public IResult Create()
	{
		var model = new { Form = new CreateContactForm() };
		return View<Create>(model);
	}

	public IResult Store([FromForm] CreateContactForm form, SessionManager session)
	{
		if (Validate(form).HasErrors)
		{
			var model = new { Form = form };
			return View<Create>(model);
		}
		var newContact = new CreateContactMapper().FormToContact(form);
		newContact.Id = Database.Contacts.Count() + 1;
		Database.Contacts.Add(newContact);

		session.SetFlash("success", "Contact successfully added.");

		return Redirect("/contacts/create");
	}

	public IResult Edit(int id)
	{
		var record = Database.Contacts.First(x => x.Id == id);
		var form = new EditContactMapper().ContactToForm(record);
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
		var newContact = new EditContactMapper().FormToContact(form);
		newContact.Id = oldContact.Id;
		Database.Contacts.Add(newContact);
		Database.Contacts.Remove(oldContact);

		return Redirect($"/contacts/{newContact.Id}/edit");
	}

	public IResult Delete(int id)
	{
		var contact = Database.Contacts.First(x => x.Id == id);
		Database.Contacts.Remove(contact);
		return Redirect($"/contacts");
	}
}

public class CreateContactForm
{
	[Required] public string Name { get; set; }
	[Required, EmailAddress] public string Email { get; set; }
	[Required] public string City { get; set; }
	[Required, Phone] public string Phone { get; set; }
}

[Mapper]
public partial class CreateContactMapper
{
	public partial CreateContactForm ContactToForm(Contact contact);
	public partial Contact FormToContact(CreateContactForm contact);
}

public class EditContactForm
{
	public int Id { get; set; }
	[Required] public string Name { get; set; }
	[Required, EmailAddress] public string Email { get; set; }
	[Required] public string City { get; set; }
	[Required, Phone] public string Phone { get; set; }
}


[Mapper]
public partial class EditContactMapper
{
	public partial EditContactForm ContactToForm(Contact contact);
	public partial Contact FormToContact(EditContactForm contact);
}