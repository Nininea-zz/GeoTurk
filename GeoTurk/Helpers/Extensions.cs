using GeoTurk.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace GeoTurk.Helpers
{
    public static class Extensions
    {
        public static string GetPropertyName<T>(this T dbset, Expression<Func<T>> exp)
        {
            return (((MemberExpression)(exp.Body)).Member).Name;
        }

        public static string ToGeorgianDate(this DateTime date, CultureInfo culture = null, string format = "dd/MM/yyyy")
        {
            if (culture == null)
                culture = CultureInfo.InvariantCulture;

            return date.ToString(format, culture);
        }
        public static string ToGeorgianDateTime(this DateTime date, CultureInfo culture = null, string format = "dd/MM/yyyy HH:mm:ss")
        {
            if (culture == null)
                culture = CultureInfo.InvariantCulture;

            return date.ToString(format, culture);
        }

        public static string ToJson(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(obj, settings);
        }
        public static string RenderViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}