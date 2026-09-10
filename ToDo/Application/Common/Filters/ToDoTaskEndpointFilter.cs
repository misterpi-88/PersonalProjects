using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;

namespace ToDo.Application.Common.Filters;

public class ToDoTaskEndpointFilter : IEndpointFilter
{
    private readonly IFeatureManager _featureManager;

    public ToDoTaskEndpointFilter(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!await _featureManager.IsEnabledAsync("ToDoTask"))
        {
            return TypedResults.NotFound("Feature Flag 'ToDoTask' is not enabled.");
        }

        return await next(context);
    }
}
