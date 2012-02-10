using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;
using System.Data;

namespace service_tracker_mvc.Controllers
{
    [RequiresAuthorizationAttribute(true, "Manager")]
    public class UserController : Controller
    {
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();
        private DataContext db = new DataContext();

        [RequiresAuthorization(false)]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Home");
        }

        [RequiresAuthorization(false)]
        public ActionResult Login()
        {
            // if the user IS logged in, they probably have come here after trying to get to a page they 
            // aren't allowed to see
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Unauthorized", "Error", new { ReturnUrl = Request["returnUrl"] });
            }

            // Stage 1: display login form to user
            return View("Login");
        }

        [RequiresAuthorization(false)]
        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        var openIdRequest = openid.CreateRequest(Request.Form["openid_identifier"]);
                        
                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        openIdRequest.AddExtension(fetch);

                        openIdRequest.AddCallbackArguments("returnUrl", returnUrl);

                        return openIdRequest.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewData["Message"] = ex.Message;
                        return View("Login");
                    }
                }
                else
                {
                    ViewData["Message"] = "Invalid identifier";
                    return View("Login");
                }
            }
            else
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        FormsAuthentication.SetAuthCookie(response.ClaimedIdentifier, true);

                        var fetch = response.GetExtension<FetchResponse>();
                        string email = null;
                        if (fetch != null)
                        {
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                        }

                        if (string.IsNullOrEmpty(email))
                        {
                            throw new ApplicationException("Could not retrieve email from openID provider");
                        }

                        Session["FriendlyIdentifier"] = email; //response.FriendlyIdentifierForDisplay;


                        CreateOrUpdateDbUser(response.ClaimedIdentifier.ToString(), email);

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    case AuthenticationStatus.Canceled:
                        ViewData["Message"] = "Canceled at provider";
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        ViewData["Message"] = response.Exception.Message;
                        return View("Login");
                }
            }
            return new EmptyResult();
        }

        private void CreateOrUpdateDbUser(string claimedIdentifier, string email)
        {
            // see if user already exists
            var ExistingUser = db.Users.SingleOrDefault(u => u.ClaimedIdentifier == claimedIdentifier);
            if (ExistingUser == null)
            {
                var NewUser = new User
                {
                    ClaimedIdentifier = claimedIdentifier,
                    Email = email,
                    FirstLogin = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    LoginCount = 1,
                    RoleId = (int)RoleType.Guest
                };

                db.Users.Add(NewUser);
            }
            else
            {
                ExistingUser.LoginCount++;
                ExistingUser.LastLogin = DateTime.UtcNow;
            }
            db.SaveChanges();
        }

        public ViewResult Index()
        {
            return View(db.Users.OrderBy(u => u.Email).ToList());
        }

        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(int userId, int roleId)
        {
            db.Users.Single(u => u.UserId == userId).RoleId = roleId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}