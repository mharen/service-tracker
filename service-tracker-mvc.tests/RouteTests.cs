using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using Moq;
using System.Web;
using service_tracker_mvc.tests.Helpers;

namespace service_tracker_mvc.tests
{
    [TestClass]
    public class RouteTests
    {
        static RouteCollection Routes = new RouteCollection();

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            MvcApplication.RegisterRoutes(Routes);
        }

        [TestMethod]
        public void CustomerIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Customer/Index", new { controller = "Customer", action = "Index" }); }
        [TestMethod]
        public void CustomerDetailsRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Customer/Details", new { controller = "Customer", action = "Details" }); }
        [TestMethod]
        public void CustomerCreateRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Customer/Create", new { controller = "Customer", action = "Create" }); }
        [TestMethod]
        public void CustomerEditRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Customer/Edit", new { controller = "Customer", action = "Edit" }); }
        [TestMethod]
        public void CustomerDeleteRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Customer/Delete", new { controller = "Customer", action = "Delete" }); }

        [TestMethod]
        public void InvoiceIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Invoice/Index", new { controller = "Invoice", action = "Index" }); }
        [TestMethod]
        public void InvoiceExcelRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Invoice/Excel", new { controller = "Invoice", action = "Excel" }); }
        [TestMethod]
        public void InvoiceDetailsRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Invoice/Details", new { controller = "Invoice", action = "Details" }); }
        [TestMethod]
        public void InvoiceCreateRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Invoice/Create", new { controller = "Invoice", action = "Create" }); }
        [TestMethod]
        public void InvoiceEditRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Invoice/Edit", new { controller = "Invoice", action = "Edit" }); }
        [TestMethod]
        public void InvoiceDeleteRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Invoice/Delete", new { controller = "Invoice", action = "Delete" }); }

        [TestMethod]
        public void ProductIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Product/Index", new { controller = "Product", action = "Index" }); }
        [TestMethod]
        public void ProductDetailsRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Product/Details", new { controller = "Product", action = "Details" }); }
        [TestMethod]
        public void ProductCreateRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Product/Create", new { controller = "Product", action = "Create" }); }
        [TestMethod]
        public void ProductEditRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Product/Edit", new { controller = "Product", action = "Edit" }); }
        [TestMethod]
        public void ProductDeleteRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Product/Delete", new { controller = "Product", action = "Delete" }); }

        [TestMethod]
        public void ServiceIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Service/Index", new { controller = "Service", action = "Index" }); }
        [TestMethod]
        public void ServiceDetailsRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Service/Details", new { controller = "Service", action = "Details" }); }
        [TestMethod]
        public void ServiceCreateRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Service/Create", new { controller = "Service", action = "Create" }); }
        [TestMethod]
        public void ServiceEditRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Service/Edit", new { controller = "Service", action = "Edit" }); }
        [TestMethod]
        public void ServiceDeleteRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Service/Delete", new { controller = "Service", action = "Delete" }); }

        [TestMethod]
        public void ServicerIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Servicer/Index", new { controller = "Servicer", action = "Index" }); }
        [TestMethod]
        public void ServicerDetailsRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Servicer/Details", new { controller = "Servicer", action = "Details" }); }
        [TestMethod]
        public void ServicerCreateRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Servicer/Create", new { controller = "Servicer", action = "Create" }); }
        [TestMethod]
        public void ServicerEditRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Servicer/Edit", new { controller = "Servicer", action = "Edit" }); }
        [TestMethod]
        public void ServicerDeleteRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Servicer/Delete", new { controller = "Servicer", action = "Delete" }); }

        [TestMethod]
        public void SettingIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/Setting/Index", new { controller = "Setting", action = "Index" }); }

        [TestMethod]
        public void UserIndexRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Index", new { controller = "User", action = "Index" }); }
        [TestMethod]
        public void UserDetailsRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Details", new { controller = "User", action = "Details" }); }
        [TestMethod]
        public void UserLoginRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Login", new { controller = "User", action = "Login" }); }
        [TestMethod]
        public void UserLoginAuthenticate_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Authenticate", new { controller = "User", action = "Authenticate" }); }
        [TestMethod]
        public void UserLogoutRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Logout", new { controller = "User", action = "Logout" }); }
        [TestMethod]
        public void UserEditRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Edit", new { controller = "User", action = "Edit" }); }
        [TestMethod]
        public void UserDeleteRoute_Test() { AssertHelpers.AssertRoute(Routes, "~/User/Delete", new { controller = "User", action = "Delete" }); }

    }
}
