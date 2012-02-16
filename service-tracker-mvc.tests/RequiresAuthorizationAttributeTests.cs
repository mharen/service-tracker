using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcAuthorize.Tests.Mocks;
using service_tracker_mvc;
using System.Web.Routing;

namespace MvcAuthorize.Tests
{
    // Testing approach from http://darioquintana.com.ar/blogging/2009/05/23/aspnet-mvc-testing-a-custom-authorize-filters/
    [TestClass]
    public class RequiresAuthorizationAttributeTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);
        }

        [TestMethod]
        public void VerifyGuestCanSeeGuestAction_Test()
        {

            var controller = new FooController();
            controller.SetFakeAuthenticatedControllerContext("pepe");

            Assert.IsTrue(new ActionInvokerExpecter<ViewResult>()
                .InvokeAction(controller.ControllerContext, "PermisiveAction"));
        }

        [TestMethod]
        public void VerifyGuestCannotSeeManagerAction_Test()
        {
            // TODO
            // Perhaps this can help: http://joelabrahamsson.com/entry/performing-and-testing-redirects-with-aspnet-web-forms-mvp
        }
    }
}