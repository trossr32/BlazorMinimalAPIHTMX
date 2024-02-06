namespace BlazorMinimalApis.Mvc.Lib;

public static class RegisterRoutesExtension
{
	public static void RegisterRoutes(this WebApplication app)
	{
		var endpointDefinitions = typeof(Program).Assembly
			.GetTypes()
			.Where(t => t.IsAssignableTo(typeof(IRouteDefinition))
                        && t is { IsAbstract: false, IsInterface: false })
			.Select(Activator.CreateInstance)
			.Cast<IRouteDefinition>();

		foreach (var endpointDef in endpointDefinitions)
		{
			endpointDef.Map(app);
		}
	}
}