// API/Filters/ValidateRouteAndBodyIdAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace GunStoreIMS.API.Filters 
{ 

    public class ValidateRouteAndBodyIdAttribute : ActionFilterAttribute
    {
        private readonly string _routeParam, _bodyParam;
        public ValidateRouteAndBodyIdAttribute(string routeParam, string bodyParam)
        {
            _routeParam = routeParam;
            _bodyParam = bodyParam;
        }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            if (ctx.ActionArguments.TryGetValue(_bodyParam, out var dto) &&
                ctx.RouteData.Values.TryGetValue(_routeParam, out var idObj) &&
                Guid.TryParse(idObj?.ToString(), out var routeId))
            {
                // reflect on DTO to find a Guid property named FirearmId or Id:
                var prop = dto!.GetType()
                               .GetProperty("FirearmId") ??
                           dto!.GetType()
                               .GetProperty("Id");
                if (prop != null && prop.GetValue(dto) is Guid bodyId && bodyId != routeId)
                {
                    ctx.Result = new BadRequestObjectResult($"Route ID ({routeId}) ≠ DTO ID ({bodyId}).");
                }
            }
        }
    }
}