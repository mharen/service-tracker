using service_tracker_mvc.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using service_tracker_mvc.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using service_tracker_mvc;
using Moq;
using System.Web;
using service_tracker_mvc.Classes;

namespace service_tracker_mvc.tests
{
    
    
    /// <summary>
    ///This is a test class for DataContextExtensionsTest and is intended
    ///to contain all DataContextExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataContextExtensionsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Administrator()
        {
            var FakeUser = GetFakeUser(RoleType.Administrator);
            var FakeInvoice = GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Administrator()
        {
            var FakeUser = GetFakeUser(RoleType.Administrator);
            var FakeInvoice = GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Administrator()
        {
            var FakeUser = GetFakeUser(RoleType.Administrator);
            var FakeInvoice = GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Manager()
        {
            var FakeUser = GetFakeUser(RoleType.Manager);
            var FakeInvoice = GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Manager()
        {
            var FakeUser = GetFakeUser(RoleType.Manager);
            var FakeInvoice = GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Manager()
        {
            var FakeUser = GetFakeUser(RoleType.Manager);
            var FakeInvoice = GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Supervisor()
        {
            var FakeUser = GetFakeUser(RoleType.Supervisor);
            var FakeInvoice = GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Supervisor()
        {
            var FakeUser = GetFakeUser(RoleType.Supervisor);
            var FakeInvoice = GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Supervisor()
        {
            var FakeUser = GetFakeUser(RoleType.Supervisor);
            var FakeInvoice = GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Customer()
        {
            var FakeUser = GetFakeUser(RoleType.Customer);
            var FakeInvoice = GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Customer()
        {
            var FakeUser = GetFakeUser(RoleType.Customer);
            var FakeInvoice = GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Customer()
        {
            var FakeUser = GetFakeUser(RoleType.Customer);
            var FakeInvoice = GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Employee()
        {
            var FakeUser = GetFakeUser(RoleType.Employee);
            var FakeInvoice = GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Employee()
        {
            var FakeUser = GetFakeUser(RoleType.Employee);
            var FakeInvoice = GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Employee()
        {
            var FakeUser = GetFakeUser(RoleType.Employee);
            var FakeInvoice = GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Guest()
        {
            var FakeUser = GetFakeUser(RoleType.Guest);
            var FakeInvoice = GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Guest()
        {
            var FakeUser = GetFakeUser(RoleType.Guest);
            var FakeInvoice = GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod()]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Guest()
        {
            var FakeUser = GetFakeUser(RoleType.Guest);
            var FakeInvoice = GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        private void VerifyInvoiceFilter(CurrentUser user, Invoice invoice, bool expectedResult){
            var filter = DataContextExtensions.GetInvoiceFilterForCurrentUser(user);
            var actualResult = filter(invoice);
            Assert.AreEqual(expectedResult, actualResult);
        }

        private CurrentUser GetFakeUser(RoleType roleType)
        {
            return new CurrentUser()
            {
                Role = roleType,
                Name = "name",
                ServicerId = 1,
                OrganizationId = 2
            };
        }

        private Invoice GetFakeInvoice(int servicerId = 0, int organizationId = 0){
            return new Invoice()
            {
                ServicerId = servicerId,
                Site = new Site()
                {
                    OrganizationId = organizationId
                }
            };
        }

        ///// <summary>
        /////A test for GetServicerFilterForCurrentUser
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Users\\haren.AURORA_NT\\Documents\\My Dropbox\\Projects\\service-tracker\\service-tracker-mvc", "/")]
        //[UrlToTest("http://localhost:60595/")]
        //[DeploymentItem("service-tracker-mvc.dll")]
        //public void GetServicerFilterForCurrentUserTest()
        //{
        //    Func<Servicer, bool> expected = null; // TODO: Initialize to an appropriate value
        //    Func<Servicer, bool> actual;
        //    actual = DataContextExtensions_Accessor.GetServicerFilterForCurrentUser();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetSiteFilterForCurrentUser
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Users\\haren.AURORA_NT\\Documents\\My Dropbox\\Projects\\service-tracker\\service-tracker-mvc", "/")]
        //[UrlToTest("http://localhost:60595/")]
        //[DeploymentItem("service-tracker-mvc.dll")]
        //public void GetSiteFilterForCurrentUserTest()
        //{
        //    Func<Site, bool> expected = null; // TODO: Initialize to an appropriate value
        //    Func<Site, bool> actual;
        //    actual = DataContextExtensions_Accessor.GetSiteFilterForCurrentUser();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ToSelectListItems
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Users\\haren.AURORA_NT\\Documents\\My Dropbox\\Projects\\service-tracker\\service-tracker-mvc", "/")]
        //[UrlToTest("http://localhost:60595/")]
        //public void ToSelectListItemsTest()
        //{
        //    DbSet<Organization> organizations = null; // TODO: Initialize to an appropriate value
        //    bool includeAllOption = false; // TODO: Initialize to an appropriate value
        //    IEnumerable<SelectListItem> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<SelectListItem> actual;
        //    actual = DataContextExtensions.ToSelectListItems(organizations, includeAllOption);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ToSelectListItems
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Users\\haren.AURORA_NT\\Documents\\My Dropbox\\Projects\\service-tracker\\service-tracker-mvc", "/")]
        //[UrlToTest("http://localhost:60595/")]
        //public void ToSelectListItemsTest1()
        //{
        //    DbSet<Service> services = null; // TODO: Initialize to an appropriate value
        //    bool includeAllOption = false; // TODO: Initialize to an appropriate value
        //    IEnumerable<ExtendedSelectListItem> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<ExtendedSelectListItem> actual;
        //    actual = DataContextExtensions.ToSelectListItems(services, includeAllOption);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ToSelectListItems
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Users\\haren.AURORA_NT\\Documents\\My Dropbox\\Projects\\service-tracker\\service-tracker-mvc", "/")]
        //[UrlToTest("http://localhost:60595/")]
        //public void ToSelectListItemsTest2()
        //{
        //    DbSet<Servicer> servicers = null; // TODO: Initialize to an appropriate value
        //    bool includeAllOption = false; // TODO: Initialize to an appropriate value
        //    IEnumerable<SelectListItem> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<SelectListItem> actual;
        //    actual = DataContextExtensions.ToSelectListItems(servicers, includeAllOption);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ToSelectListItems
        /////</summary>
        //// TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        //// http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        //// whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\Users\\haren.AURORA_NT\\Documents\\My Dropbox\\Projects\\service-tracker\\service-tracker-mvc", "/")]
        //[UrlToTest("http://localhost:60595/")]
        //public void ToSelectListItemsTest3()
        //{
        //    DbSet<Site> sites = null; // TODO: Initialize to an appropriate value
        //    bool includeAllOption = false; // TODO: Initialize to an appropriate value
        //    IEnumerable<SelectListItem> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<SelectListItem> actual;
        //    actual = DataContextExtensions.ToSelectListItems(sites, includeAllOption);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
