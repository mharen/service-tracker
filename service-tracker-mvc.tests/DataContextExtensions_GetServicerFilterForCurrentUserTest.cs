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
    public class DataContextExtensions_GetServicerFilterForCurrentUserTest
    {

        [TestMethod]
        public void GetServicerFilterForCurrentUser_ByCurrentServicer_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeServicer = Statics.GetFakeServicer(servicerId: FakeUser.ServicerId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByCurrentOrganization_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeServicer = Statics.GetFakeServicer(organizationId: FakeUser.OrganizationId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeServicer = Statics.GetFakeServicer();
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUser_ByCurrentServicer_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeServicer = Statics.GetFakeServicer(servicerId: FakeUser.ServicerId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByCurrentOrganization_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeServicer = Statics.GetFakeServicer(organizationId: FakeUser.OrganizationId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeServicer = Statics.GetFakeServicer();
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUser_ByCurrentServicer_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeServicer = Statics.GetFakeServicer(servicerId: FakeUser.ServicerId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByCurrentOrganization_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeServicer = Statics.GetFakeServicer(organizationId: FakeUser.OrganizationId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeServicer = Statics.GetFakeServicer();
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUser_ByCurrentServicer_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeServicer = Statics.GetFakeServicer(servicerId: FakeUser.ServicerId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByCurrentOrganization_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeServicer = Statics.GetFakeServicer(organizationId: FakeUser.OrganizationId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeServicer = Statics.GetFakeServicer();
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUser_ByCurrentServicer_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeServicer = Statics.GetFakeServicer(servicerId: FakeUser.ServicerId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: true);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByCurrentOrganization_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeServicer = Statics.GetFakeServicer(organizationId: FakeUser.OrganizationId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeServicer = Statics.GetFakeServicer();
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUser_ByCurrentServicer_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeServicer = Statics.GetFakeServicer(servicerId: FakeUser.ServicerId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByCurrentOrganization_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeServicer = Statics.GetFakeServicer(organizationId: FakeUser.OrganizationId.Value);
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        [TestMethod]
        public void GetServicerFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeServicer = Statics.GetFakeServicer();
            VerifyServicerFilter(FakeUser, FakeServicer, expectedResult: false);
        }

        private void VerifyServicerFilter(ICurrentUser currentUser, Servicer servicer, bool expectedResult)
        {
            var filter = DataContextExtensions.GetServicerFilterForCurrentUser(currentUser);
            var actualResult = filter(servicer);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
