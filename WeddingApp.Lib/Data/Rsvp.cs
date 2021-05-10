using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingApp.Lib.Data
{
    public class Rsvp
    {
        public string? Email { get; }
        public bool Accepted { get; }
        public string? Name { get; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;
        public virtual  EmailConfirmationCode? ConfirmationCode { get; }

        private Rsvp() { }

        public Rsvp(string email, string name, bool accepted)
        {
            Email = email;
            Name = name;
            Accepted = accepted;
        }

        public override string ToString()
            => $"{Email},{Accepted},{Name},{EmailConfirmed},{CreatedOnUtc}";

        public static string ToCsv(IEnumerable<Rsvp> rsvps)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{nameof(Email)},{nameof(Accepted)},{nameof(Name)},{nameof(EmailConfirmed)},{nameof(CreatedOnUtc)}");
            foreach (var rsvp in rsvps)
            {
                builder.AppendLine(rsvp.ToString());
            }
            return builder.ToString();
        }

        public string NameAndEmail() => $"{Name} ({Email})";
    }
}
