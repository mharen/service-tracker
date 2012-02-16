using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using service_tracker_mvc;

namespace MvcAuthorize.Tests.Mocks
{
	public static class MvcMocks
	{
		public static HttpContextBase FakeAuthenticatedHttpContext(string username)
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new Mock<HttpSessionStateBase>();
			var server = new Mock<HttpServerUtilityBase>();
			var user = new Mock<IPrincipal>();
			var identity = new Mock<IIdentity>();

			context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Request.Url).Returns(new System.Uri("http://example.com"));
			context.Setup(ctx => ctx.Response).Returns(response.Object);
			context.Setup(ctx => ctx.Session).Returns(session.Object);
			context.Setup(ctx => ctx.Server).Returns(server.Object);
			context.Setup(ctx => ctx.User).Returns(user.Object);
			user.Setup(ctx => ctx.Identity).Returns(identity.Object);
			identity.Setup(id => id.IsAuthenticated).Returns(true);
			identity.Setup(id => id.Name).Returns(username);
			context.Setup(ctx => ctx.Response.Cache).Returns(CreateCachePolicy());

            return context.Object;
		}

		public static HttpCachePolicyBase CreateCachePolicy()
		{
			var mock = new Mock<HttpCachePolicyBase>();
			return mock.Object;
		}

		public static void SetFakeAuthenticatedControllerContext(this Controller controller, string username)
		{
			HttpContextBase httpContext = FakeAuthenticatedHttpContext(username);
			var context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
			controller.ControllerContext = context;
		}
	}
}