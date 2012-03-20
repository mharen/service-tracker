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
    public class DataContextExtensions_GetInvoiceFilterForCurrentUserTests
    {
        [TestMethod]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeInvoice = Statics.GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeInvoice = Statics.GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Administrator()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Administrator);
            var FakeInvoice = Statics.GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeInvoice = Statics.GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeInvoice = Statics.GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Manager()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Manager);
            var FakeInvoice = Statics.GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeInvoice = Statics.GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeInvoice = Statics.GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Supervisor()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Supervisor);
            var FakeInvoice = Statics.GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeInvoice = Statics.GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeInvoice = Statics.GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Customer()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Customer);
            var FakeInvoice = Statics.GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeInvoice = Statics.GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: true);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeInvoice = Statics.GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Employee()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Employee);
            var FakeInvoice = Statics.GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUser_ByCurrentServicer_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeInvoice = Statics.GetFakeInvoice(servicerId: FakeUser.ServicerId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByCurrentOrganization_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeInvoice = Statics.GetFakeInvoice(organizationId: FakeUser.OrganizationId.Value);
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        [TestMethod]
        public void GetInvoiceFilterForCurrentUserTest_ByNeitherCurrentOrganizationOrServicer_Guest()
        {
            var FakeUser = Statics.GetFakeUser(RoleType.Guest);
            var FakeInvoice = Statics.GetFakeInvoice();
            VerifyInvoiceFilter(FakeUser, FakeInvoice, expectedResult: false);
        }

        private void VerifyInvoiceFilter(ICurrentUser user, Invoice invoice, bool expectedResult){
            var filter = DataContextExtensions.GetInvoiceFilterForCurrentUser(user);
            var actualResult = filter(invoice);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
