﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace CafeteriaManagement.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Logic to send email
            return Task.CompletedTask;
        }
    }
}
