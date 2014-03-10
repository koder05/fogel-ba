using Microsoft.Data.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;

using System.IO;
using System.Threading.Tasks;

namespace RF.WinApp.Svc.Extensions
{
    /// <summary>
    /// Custom convention to support GET/POST/PUT/PATCH/DELETE routing on navigation property
    /// </summary>
    public class CustomNavigationRoutingConvention : EntitySetRoutingConvention
    {
        private void A(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                string contents = reader.ReadToEnd();
                //var o = Newtonsoft.Json.JsonConvert.DeserializeObject(contents);
            }
        }

        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            //if (controllerContext.Request.Method == System.Net.Http.HttpMethod.Put) controllerContext.Request.Content.ReadAsStreamAsync().ContinueWith((t) => A(t.Result));
            
            if (odataPath == null)
            {
                throw new ArgumentNullException("odataPath");
            }

            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (actionMap == null)
            {
                throw new ArgumentNullException("actionMap");
            }

            string prefix = GetHttpPrefix(controllerContext.Request.Method.ToString());
            if (string.IsNullOrEmpty(prefix))
            {
                return null;
            }

            //~/entityset/key/$links/navigation
            if ((odataPath.PathTemplate == "~/entityset/key/navigation") 
                || (odataPath.PathTemplate == "~/entityset/key/cast/navigation")
                    || (odataPath.PathTemplate == "~/entityset/key/$links/navigation"))
            {
                NavigationPathSegment segment = odataPath.Segments.Last() as NavigationPathSegment;
                IEdmNavigationProperty navigationProperty = segment.NavigationProperty;
                IEdmEntityType declaringType = navigationProperty.DeclaringType as IEdmEntityType;

                if (declaringType != null)
                {
                    KeyValuePathSegment keySegment = odataPath.Segments[1] as KeyValuePathSegment;
                    controllerContext.RouteData.Values[ODataRouteConstants.Key] = keySegment.Value;
                    string actionName = prefix + navigationProperty.Name + "From" + declaringType.Name;
                    return (actionMap.Contains(actionName) ? actionName : (prefix + navigationProperty.Name));
                }
            }

            return null;
        }

        public static string GetHttpPrefix(string httpMethod)
        {
            switch (httpMethod)
            {
                case "GET":
                    return "Get";
                case "POST":
                    return "Post";
                case "PUT":
                    return "Put";
                case "MERGE":
                case "PATCH":
                    return "Patch";
                case "DELETE":
                    return "Delete";
                default:
                    return null;
            }
        }
    }
}