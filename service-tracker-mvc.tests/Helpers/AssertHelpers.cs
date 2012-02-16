using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace service_tracker_mvc.tests.Helpers
{
    public class AssertHelpers
    {
        public static void AssertRoute(RouteCollection routes, string url, object expectations)
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath)
                .Returns(url);

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);
            Assert.IsNotNull(routeData, "Should have found the route");

            foreach (var property in Utilities.GetProperties(expectations))
            {
                Assert.IsTrue(string.Equals(property.Value.ToString(),
                    routeData.Values[property.Name].ToString(),
                    StringComparison.OrdinalIgnoreCase)
                    , string.Format("Expected '{0}', not '{1}' for '{2}'.",
                    property.Value, routeData.Values[property.Name], property.Name));
            }
        }
    }
}
