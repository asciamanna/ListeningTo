using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using WebApiContrib.Formatting.Jsonp;

namespace ListeningTo {
  public static class WebApiConfig {
    public static void Register(HttpConfiguration config) {
      // Web API configuration and services
      config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
      config.Formatters.Insert(0, new JsonpMediaTypeFormatter(new JsonMediaTypeFormatter()));
      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );
    }
  }
}
