using System;

namespace WeddingApp.Lib.Data
{
    public class EmailConfirmationCode
    {
        public Guid Code { get; }
        public string Email { get; }
        public virtual Rsvp? Rsvp { get; }

        public EmailConfirmationCode(string email, Guid code = default)
        {
            Email = email;
            Code = code == default ? Guid.NewGuid() : code;
        }
    }
}