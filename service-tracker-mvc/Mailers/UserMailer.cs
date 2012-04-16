using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;
using System.Configuration;
using service_tracker_mvc.Models;
using service_tracker_mvc.Data;

namespace service_tracker_mvc.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer() :
            base()
        {
            MasterName = "_Layout";
        }


        public virtual MailMessage Invitation(User user)
        {
            using (var repo = new Repo())
            {
                var profile = repo.Profiles.Single();

                var from = profile.EmailFromAddress ?? "noreply@domain";

                var mailMessage = new MailMessage(from, user.Email)
                {
                    Subject = "Invitation to Service Tracker"
                };

                var bcc = profile.EmailBccAddress;
                if (!string.IsNullOrEmpty(bcc))
                {
                    mailMessage.Bcc.Add(bcc);
                }

                ViewBag.User = user;
                PopulateBody(mailMessage, viewName: "Invitation");

                return mailMessage;
            }
        }


    }
}