using System;

//using System.ServiceModel;
using System.Web.Http.Tracing;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;

using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;

//using RF.WinApp.Svc.Controllers;
using Model = RF.BL.Model;
using RF.WinApp.Svc.Extensions;

namespace RF.WinApp.Svc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Add $format support
            config.MessageHandlers.Add(new FormatQueryMessageHandler());

            // Add NavigationRoutingConvention2 to support POST, PUT, PATCH and DELETE on navigation property
            var conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new CustomNavigationRoutingConvention());

            // Enables OData support by adding an OData route and enabling querying support for OData.
            // Action selector and odata media type formatters will be registered in per-controller configuration only
            config.Routes.MapODataRoute(
                routeName: "OData",
                routePrefix: "svc",
                //routePrefix: null,
                model: GetEdmModel(),
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions);

            // Enable queryable support and allow $format query
            config.EnableQuerySupport(new QueryableAttribute {AllowedQueryOptions = AllowedQueryOptions.Supported | AllowedQueryOptions.Format});
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            // need refer to System.Web.Http.Tracing.dll
            //config.EnableSystemDiagnosticsTracing();

            //config.Filters.Add(new ModelValidationFilterAttribute());
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.ContainerName = "WebApiCtx";

            //modelBuilder.EntitySet<TestDataObj>("Values");
            var holidays = modelBuilder.EntitySet<Model.WorkCalendar>("Holidays");
            holidays.EntityType.Property(h => h.Comment).IsOptional();
            holidays.EntityType.Ignore(h => h.DayType);
            holidays.EntityType.Ignore(h => h.BankDate);
            holidays.EntityType.Ignore(h => h.BankDate_Key);

            //var companies = modelBuilder.EntitySet<Model.Company>("Companies");
            var govs = modelBuilder.EntitySet<Model.Governor>("Governors");
            govs.HasEditLink(
                entityContext => entityContext.Url.ODataLink(
                    new EntitySetPathSegment(entityContext.EntitySet.Name),
                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(entityContext.EntityInstance.Id, ODataVersion.V3))),
                followsConventions: true);

            var c = modelBuilder.ComplexType<Model.Company>();
            c.Property(o => o.lawFormValue);
            c.Ignore(o => o.FullName);
            c.Ignore(o => o.LawForm);

            govs.EntityType.ComplexProperty(g => g.Company).IsRequired();

            var assets = modelBuilder.EntitySet<Model.AssetValue>("Assets");
            assets.HasEditLink(
                entityContext => entityContext.Url.ODataLink(
                    new EntitySetPathSegment(entityContext.EntitySet.Name),
                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(entityContext.EntityInstance.Id, ODataVersion.V3))),
                followsConventions: true);

            assets.HasRequiredBinding(a => a.Governor, govs);
            assets.EntityType.Property(a => a.InsuranceTypeValue).IsRequired();
            //assets.EntityType.ComplexProperty(a => a.Governor);
            assets.EntityType.Ignore(a => a.InsuranceType);
            assets.EntityType.Ignore(a => a.InsuranceTypeString);

            assets.HasNavigationPropertiesLink(
                assets.EntityType.NavigationProperties,
                (entityContext, navigationProperty) =>
                    new Uri(entityContext.Url.ODataLink(
                        new EntitySetPathSegment(entityContext.EntitySet.Name),
                        new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(entityContext.EntityInstance.Id, ODataVersion.V3)),
                        new NavigationPathSegment(navigationProperty.Name))),
                followsConventions: true);

            govs.HasNavigationPropertiesLink(
                govs.EntityType.NavigationProperties,
                (entityContext, navigationProperty) =>
                    new Uri(entityContext.Url.ODataLink(
                        new EntitySetPathSegment(entityContext.EntitySet.Name),
                        new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(entityContext.EntityInstance.Id, ODataVersion.V3)),
                        new NavigationPathSegment(navigationProperty.Name))),
                followsConventions: true);

            var actionRep = assets.EntityType.Collection.Action("Report");
            actionRep.Parameter<DateTime>("DateBegin");
            actionRep.Parameter<DateTime>("DateEnd");
            actionRep.Parameter<byte>("InsuranceType");
            actionRep.Parameter<Guid?>("GovernorId");
            actionRep.Returns<string>();

            var actionBatch = assets.EntityType.Collection.Action("CreateBatch");
            actionBatch.CollectionParameter<string>("Values");
            actionBatch.Returns<bool>();

            IEdmModel model = modelBuilder.GetEdmModel();
            return model;
        }
    }
}
