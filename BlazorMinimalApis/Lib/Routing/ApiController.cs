using BlazorMinimalApis.Lib.Validation;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BlazorMinimalApis.Lib.Routing;

public abstract class ApiController
{
    public ValidationResponse Validation = new();

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

    public ValidationResponse Validate<TData>(TData data, AbstractValidator<TData> validator)
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

    public ValidationResponse GetErrors() => Validation;

    public void AddError(string key, string message)
    {
        Validation.HasErrors = true;
        Validation.Errors.Add(new ValidationError { MemberName = key, Message = message });
    }

    public IResult Redirect(string url) => Results.Redirect(url);
}
