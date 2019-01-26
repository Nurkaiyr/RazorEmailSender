using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorEmail.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public List<Message> To{ get; set; }
        public List<Message> From { get; set; }
        public string Text { get; set; }
        public string Subject { get; set; }

        public Message()
        {
            To = new List<Message>();
            From = new List<Message>();
        }
    }
}
