using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingApp.Lib.Data
{
    public class Rsvp
    {
        public string? Email { get; }
        public string? Name { get; }
        public bool EmailConfirmed { get; }
        public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;

        private Rsvp() { }

        public Rsvp(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string ToCsv()
            => $"{Email},{Name},{EmailConfirmed},{CreatedOnUtc}";

        public static string ToCsv(IEnumerable<Rsvp> rsvps)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{nameof(Email)},{nameof(Name)},{nameof(EmailConfirmed)},{nameof(CreatedOnUtc)}");
            foreach (var rsvp in rsvps)
            {
                builder.AppendLine(rsvp.ToCsv());
            }
            return builder.ToString();
        }
    }
}
