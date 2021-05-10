using Microsoft.AspNetCore.Mvc;
using WeddingApp.Lib.Data;
using WeddingApp.Lib.Services;
using System;
using System.Threading.Tasks;

namespace WeddingApp.Web.Controllers
{
    public record SubmissionViewModel(bool Success);

    [Route("[controller]")]
    public class EmailConfirmationController : Controller
    {
        private readonly WeddingDbContext _weddingDb;
        private readonly EmailService _emailService;

        public EmailConfirmationController(
            WeddingDbContext weddingDb,
            EmailService emailService)
        {
            _weddingDb = weddingDb;
            _emailService = emailService;
        }

        [HttpGet, Route("submission/{code}")]
        public async Task<IActionResult> Submission(Guid code)
        {
            var confirmation = await _weddingDb.EmailConfirmationCodes.FindAsync(code);
            if (confirmation is null)
            {
                return View(new SubmissionViewModel(Success: false));
            }

            var rsvp = await _weddingDb.Rsvps.FindAsync(confirmation.Email);
            rsvp.EmailConfirmed = true;
            await _weddingDb.SaveChangesAsync();
            await _emailService.SendMeetingLink(rsvp.Name, rsvp.Email);
            return View(new SubmissionViewModel(Success: true));
        }
    }
}