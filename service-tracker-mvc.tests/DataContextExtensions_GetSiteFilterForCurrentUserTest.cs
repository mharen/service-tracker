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
    [TestClass]
    public class DataContextExtensions_GetSiteFilterForCurrentUserTest
    {

        [TestMethod]
        public void GetSiteFilterForCurrentUser_ByCurrentServicer_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeSite = Statics.GetFakeSite(servicerId: FakeUser.ServicerId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByCurrentOrganization_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeSite = Statics.GetFakeSite(organizationId: FakeUser.OrganizationId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeSite = Statics.GetFakeSite();
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUser_ByCurrentServicer_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeSite = Statics.GetFakeSite(servicerId: FakeUser.ServicerId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByCurrentOrganization_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeSite = Statics.GetFakeSite(organizationId: FakeUser.OrganizationId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeSite = Statics.GetFakeSite();
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUser_ByCurrentServicer_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeSite = Statics.GetFakeSite(servicerId: FakeUser.ServicerId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByCurrentOrganization_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeSite = Statics.GetFakeSite(organizationId: FakeUser.OrganizationId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeSite = Statics.GetFakeSite();
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUser_ByCurrentServicer_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeSite = Statics.GetFakeSite(servicerId: FakeUser.ServicerId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByCurrentOrganization_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeSite = Statics.GetFakeSite(organizationId: FakeUser.OrganizationId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeSite = Statics.GetFakeSite();
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUser_ByCurrentServicer_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeSite = Statics.GetFakeSite(servicerId: FakeUser.ServicerId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: true);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByCurrentOrganization_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeSite = Statics.GetFakeSite(organizationId: FakeUser.OrganizationId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeSite = Statics.GetFakeSite();
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUser_ByCurrentServicer_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeSite = Statics.GetFakeSite(servicerId: FakeUser.ServicerId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByCurrentOrganization_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeSite = Statics.GetFakeSite(organizationId: FakeUser.OrganizationId.Value);
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        [TestMethod]
        public void GetSiteFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeSite = Statics.GetFakeSite();
            VerifySiteFilter(FakeUser, FakeSite, expectedResult: false);
        }

        private void VerifySiteFilter(ICurrentUser currentUser, Site Site, bool expectedResult)
        {
            var filter = DataContextExtensions_Accessor.GetSiteFilterForCurrentUser(currentUser);
            var actualResult = filter(Site);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
