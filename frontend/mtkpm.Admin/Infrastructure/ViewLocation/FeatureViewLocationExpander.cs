using Microsoft.AspNetCore.Mvc.Razor;

namespace mtkpm.Admin.Infrastructure.ViewLocation
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var locations = viewLocations.ToList();
            if (context.AreaName == null && context.ControllerName != null)
            {
                locations.Insert(0, $"/Features/{context.ControllerName}/Views/Shared/{{0}}.cshtml");
                locations.Insert(0, $"/Features/{context.ControllerName}/Views/{{1}}/{{0}}.cshtml");
            }
            return locations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["controllerName"] = context.ControllerName;
        }
    }

    public class FeatureViewLocationExpanderV2 : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var locations = viewLocations.ToList();

            if (context.AreaName == null && context.ControllerName != null)
            {
                locations.Insert(0, $"/Features/{context.ControllerName}/Views/Shared/{{0}}.cshtml");
                locations.Insert(0, $"/Features/{context.ControllerName}/Views/{{1}}/{{0}}.cshtml");
                locations.Insert(0, $"/Features/Shared/Views/{{0}}.cshtml");
            }

            return locations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["controllerName"] = context.ControllerName;
        }
    }
}
