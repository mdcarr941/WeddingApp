using Microsoft.AspNetCore.Mvc;
using WeddingApp.Lib.Data;
using System;
using System.Threading.Tasks;

namespace WeddingApp.Web.Controllers
{
    public record SubmissionViewModel(bool Success);

    [Route("[controller]")]
    public class EmailConfirmationController : Controller
    {
        private readonly WeddingDbContext _weddingDb;

        public EmailConfirmationController(
            WeddingDbContext weddingDb)
        {
            _weddingDb = weddingDb;
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
            return View(new SubmissionViewModel(Success: true));
        }
    }
}