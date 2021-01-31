using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WeddingApp.Lib;
using WeddingApp.Lib.Data;

namespace WeddingApp.Web.Controllers
{
    public record RsvpMessage(
        string Password,
        string Email,
        string Name
    );

    [ApiController]
    [Route("[controller]")]
    public class RsvpController : ControllerBase
    {
        private readonly WeddingDbContext _weddingDb;

        public RsvpController(WeddingDbContext weddingDb)
            => _weddingDb = weddingDb;

        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] RsvpMessage message)
        {
            var config = await _weddingDb.WebConfig();
            if (!config.RsvpPassword.Equals(message.Password, StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Incorrect password, please try again.");

            _weddingDb.Rsvps.Add(new Rsvp(message.Email, message.Name));
            try
            {
                await _weddingDb.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqliteException sqlEx
                    && SqliteErrorCodes.UniqueConstraintViolation == sqlEx.SqliteErrorCode)
                {
                    return BadRequest("There is already an RSVP for that email address.");
                }
                throw;
            }

            return Ok("RSVP submitted succesfully.");
        }
    }
}
