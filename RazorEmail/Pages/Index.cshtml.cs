using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using RazorEmail.Data;
using RazorEmail.Models;

namespace RazorEmail.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RazorEmail.Data.ApplicationDbContext _context;
        public IndexModel(RazorEmail.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Message> Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Message = await _context.Messages.ToListAsync();

            using (var client = new Pop3Client())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("pop.gmail.com", 995, true);
                client.Authenticate("smtptesteritstep@gmail.com", "167AEq!!");

                var s = new Message();
                for (int i = 0; i < client.Count; i++)
                {
                    var message = client.GetMessage(i);
                }
                //client.Disconnect(true);
                return Page();
                //return emails;
            }
        }

    }
}
