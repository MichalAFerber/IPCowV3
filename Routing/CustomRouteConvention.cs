using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Logging;

namespace IPCowV3.Routing;

public class CustomRouteConvention(ILogger<CustomRouteConvention> logger) : IPageRouteModelConvention
{
    private readonly ILogger<CustomRouteConvention> _logger = logger;

    public void Apply(PageRouteModel model)
    {
        _logger.LogInformation("Applying custom route convention");

        foreach (var selector in model.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                if (selector.AttributeRouteModel.Template != null)
                {
                    _logger.LogInformation("Original Template: {Template}", selector.AttributeRouteModel.Template);
                    selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.Replace("Home/", "");
                    _logger.LogInformation("Modified Template: {Template}", selector.AttributeRouteModel.Template);
                }
            }
        }
    }
}