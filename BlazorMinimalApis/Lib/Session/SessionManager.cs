using Microsoft.AspNetCore.Http;

namespace BlazorMinimalApis.Lib.Session;

public class SessionManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetFlash(string key, string value) => 
        _httpContextAccessor.HttpContext?.Session.SetString(key, value);

    public void SetString(string key, string value) => 
        _httpContextAccessor.HttpContext?.Session.SetString(key, value);

    public string? GetString(string key) =>
        _httpContextAccessor.HttpContext is null 
            ? string.Empty
            : _httpContextAccessor.HttpContext.Session.GetString(key);

    public string? GetFlash(string key)
    {
        if (_httpContextAccessor.HttpContext is null) 
            return string.Empty;

        var message = _httpContextAccessor.HttpContext.Session.GetString(key);

        _httpContextAccessor.HttpContext.Session.Remove(key);

        return message;
    }

    public bool HasKey(string key)
    {
        if (_httpContextAccessor.HttpContext == null) 
            return false;

        var message = _httpContextAccessor.HttpContext.Session.GetString(key);

        return !string.IsNullOrEmpty(message);
    }
}
