using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;

namespace DevHours.CloudNative.Api.OData.Extensions
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddODataBindings(this IMvcBuilder builder)
            => builder.AddOData(o =>
                 {
                     o.Count().Filter().Expand().Select().OrderBy().SetMaxTop(25);
                     var builder = new HotelModelBuilder();
                     o.AddRouteComponents(builder.GetEdmModel());
                 });
    }
}