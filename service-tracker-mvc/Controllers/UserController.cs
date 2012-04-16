using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Mvc.Mailer;
using service_tracker_mvc.Classes;
using service_tracker_mvc.Filters;
using service_tracker_mvc.Mailers;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;
using System.Net.Mail;

namespace service_tracker_mvc.Controllers
{
    [RequiresAuthorizationAttribute("Manager")]
    public class UserController : Controller
    {
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();
        private Repo repo = new Repo();

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
                return RedirectToAction("Index", "Home");
            }

            // Stage 1: display login form to user
            return View("Login");
        }

        [RequiresAuthorization(false)]
        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl = "", string invitationCode = "")
        {
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(Request["openid_identifier"], out id))
                {
                    try
                    {
                        var openIdRequest = openid.CreateRequest(id);

                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        openIdRequest.AddExtension(fetch);

                        openIdRequest.AddCallbackArguments("returnUrl", returnUrl);
                        openIdRequest.AddCallbackArguments("invitationCode", invitationCode);

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

                        Session["FriendlyIdentifier"] = email;


                        CreateOrUpdateDbUser(response.ClaimedIdentifier.ToString(), email, invitationCode);

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

        private void CreateOrUpdateDbUser(string claimedIdentifier, string email, string invitationCode)
        {
            // see if user already exists
            var ExistingUserByOpenId = repo.Users.SingleOrDefault(u => u.ClaimedIdentifier == claimedIdentifier);
            var ExistingUserByInvite = repo.Users.SingleOrDefault(u => u.InvitationCode == invitationCode);

            if (ExistingUserByOpenId != null && ExistingUserByInvite != null)
            {
                // the user that accepted the invite already had an account. Delete the invite
                repo.Remove(ExistingUserByInvite);
                ExistingUserByInvite = null;
            }

            var ExistingUser = ExistingUserByOpenId ?? ExistingUserByInvite;

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

                repo.Add(NewUser);
            }
            else
            {
                if (ExistingUser.ClaimedIdentifier == null)
                {
                    ExistingUser.ClaimedIdentifier = claimedIdentifier;
                    ExistingUser.FirstLogin = DateTime.UtcNow;
                    ExistingUser.InvitationCode = null; // clear out code since it's been redeemed

                    var log = new InvitationLog()
                    {
                        UserId = ExistingUser.UserId,
                        Action = (int)InvitationAction.Accepted,
                        LogDate = DateTime.UtcNow
                    };
                    repo.Add(log);
                }
                ExistingUser.Email = email;
                ExistingUser.LoginCount++;
                ExistingUser.LastLogin = DateTime.UtcNow;
            }
            repo.SaveChanges();
        }

        public ViewResult Index()
        {
            return View(repo.Users.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.Organizations = repo.Organizations.ToSelectListItems();
            ViewBag.Servicers = repo.Servicers.ToSelectListItems();

            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // generate a new invite code
                user.InvitationCode = Extensions.Utilities.GenerateKey();
                repo.Add(user);
                repo.SaveChanges();

                var LogEntry = new InvitationLog()
                {
                    Action = (int)InvitationAction.Created,
                    UserId = user.UserId,
                    LogDate = DateTime.UtcNow
                };

                repo.Add(LogEntry);

                try
                {
                    SendEmailInvitation(user);
                    TempData["Message"] = "Invitation Sent";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = string.Format("Invitation created, but could not be sent :( ({0})", ex.Message);
                }

                repo.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Organizations = repo.Organizations.ToSelectListItems();
            ViewBag.Servicers = repo.Servicers.ToSelectListItems();
            return View(user);
        }

        [HttpPost]
        private void SendEmailInvitation(User user)
        {
            IUserMailer mailer = new UserMailer();
            var message = mailer.Invitation(user);
            message.Send();

            var log = new InvitationLog()
            {
                UserId = user.UserId,
                Action = (int)InvitationAction.Sent,
                LogDate = DateTime.UtcNow
            };
            repo.Add(log);
        }

        public ActionResult Log(int id)
        {
            var invitationLogs = repo.InvitationLogs
                                    .Where(l => l.UserId == id)
                                    .ToList();

            return View(invitationLogs);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Organizations = repo.Organizations.ToSelectListItems();
            ViewBag.Servicers = repo.Servicers.ToSelectListItems();

            User user = repo.Users.Single(u => u.UserId == id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            // note: we can't just save off the given "user" object because it doesn't have
            // all the values from the user row (login count, date, claimed id, etc.) and to 
            // do it that way would blow those other fields out of the DB, which
            // would make you look pretty foolish, amirite?
            try
            { 
                var existingUser = repo.Users.Single(u => u.UserId == user.UserId);
                repo.Entry(existingUser).State = System.Data.EntityState.Modified;
                existingUser.OrganizationId = user.OrganizationId;
                existingUser.RoleId = user.RoleId;
                existingUser.ServicerId = user.ServicerId;
                repo.SaveChanges();
                TempData["Message"] = "User Saved";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                ViewBag.Organizations = repo.Organizations.ToSelectListItems();
                ViewBag.Servicers = repo.Servicers.ToSelectListItems();
                return View(user);
            }
        }

        public ActionResult Delete(int id)
        {
            User user = repo.Users.Single(u => u.UserId == id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = repo.Users.Single(u => u.UserId == id);

            try
            {
                var currentUser = DataContextExtensions.LoadUser();

                if (currentUser.UserId == user.UserId)
                {
                    throw new ArgumentException("You cannot delete yourself!");
                }

                repo.Remove(user);
                repo.SaveChanges();
                TempData["Message"] = "User Deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View(user);
            }
        }

        public ActionResult SendInvite(int id)
        {
            User user = repo.Users.Single(u => u.UserId == id);
            SendEmailInvitation(user);

            TempData["Message"] = "Invitation Resent";
            return RedirectToAction("Edit", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}