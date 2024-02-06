using BlazorMinimalApis.Lib.Views;
using BlazorMinimalApis.Lib.Helpers;
using BlazorMinimalApis.Lib.Validation;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlazorMinimalApis.Lib.Routing;

public abstract class XController
{
    public ValidationResponse Validation = new();

    public IResult View<TComponent>(object data)
    {
        var componentData = data.ToDictionary();
        var errors = new List<ValidationError>();

        if (Validation is { HasErrors: true, Errors.Count: > 0 })
            errors = Validation.Errors;

        var componentType = typeof(TComponent);

        return new RazorComponentResult(typeof(PageComponent), new { ComponentType = componentType, ComponentParameters = componentData, Errors = errors });
    }

    public IResult View<TComponent>() => View<TComponent>(new { });

    public ValidationResponse Validate<TData>(TData data)
    {
        var ctx = new ValidationContext(data);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(data, ctx, results, true))
            return new ValidationResponse { HasErrors = false };

        ValidationResponse validationResponse = new();

        foreach (var ve in results
                     .Select(error => new ValidationError
                     {
                         Message = error.ErrorMessage,
                         MemberName = error.MemberNames.First(),
                     }))
        {
            validationResponse.Errors.Add(ve);
        }

        Validation = validationResponse;
        return validationResponse;
    }

    public ValidationResponse Validate<TData, TValidator>(TData data, TValidator validator) where TValidator : AbstractValidator<TData>
    {
        var results = validator.Validate(data);

        if (results.IsValid)
            return new ValidationResponse { HasErrors = false };

        ValidationResponse validationResponse = new();

        foreach (var ve in results.Errors
                     .Select(error => new ValidationError
                     {
                         Message = error.ErrorMessage,
                         MemberName = error.PropertyName
                     }))
        {
            validationResponse.Errors.Add(ve);
        }

        Validation = validationResponse;
        return validationResponse;
    }

    public async Task<ValidationResponse> ValidateAsync<TData, TValidator>(TData data, TValidator validator) where TValidator : AbstractValidator<TData>
    {
        var results = await validator.ValidateAsync(data);

        if (results.IsValid)
            return new ValidationResponse { HasErrors = false };

        ValidationResponse validationResponse = new();

        foreach (var ve in results.Errors
                     .Select(error => new ValidationError
                     {
                         Message = error.ErrorMessage,
                         MemberName = error.PropertyName
                     }))
        {
            validationResponse.Errors.Add(ve);
        }

        Validation = validationResponse;
        return validationResponse;
    }

    public ValidationResponse GetErrors() => Validation;

    public void AddError(string key, string message)
    {
        Validation.HasErrors = true;
        Validation.Errors.Add(new ValidationError { MemberName = key, Message = message });
    }

    public IResult Redirect(string url) => Results.Redirect(url);

    public XController Flash(string key, string message) => this;
}
