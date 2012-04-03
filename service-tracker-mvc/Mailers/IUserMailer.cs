using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;
using service_tracker_mvc.Models;

namespace service_tracker_mvc.Mailers
{ 
    public interface IUserMailer
    {
        MailMessage Invitation(User user);
	}
}