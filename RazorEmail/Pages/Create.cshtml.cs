using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using RazorEmail.Data;
using RazorEmail.Models;

namespace RazorEmail.Pages
{
    public class CreateModel : PageModel
    {
        private readonly RazorEmail.Data.ApplicationDbContext _context;

        public CreateModel(RazorEmail.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Message Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Joey", "joey@friends.com"));
            message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", Message.To));
            message.Subject = Message.Subject;

            message.Body = new TextPart("plain")
            {
                Text = Message.Text
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("smtptesteritstep@gmail.com", "167AEq!!");

                client.Send(message);
                client.Disconnect(true);

                return RedirectToPage("./Index");
            }
        }
    }
}