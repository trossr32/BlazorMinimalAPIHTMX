using System.ComponentModel;

namespace BlazorMinimalApis.Lib.Helpers;

public static class ObjectExtensions
{
    public static Dictionary<string, object> ToDictionary(this object? values)
    {
        var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        if (values is null) 
            return dict;

        foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(values))
        {
            var obj = propertyDescriptor.GetValue(values);
            dict.Add(propertyDescriptor.Name, obj);
        }

        return dict;
    }
}
