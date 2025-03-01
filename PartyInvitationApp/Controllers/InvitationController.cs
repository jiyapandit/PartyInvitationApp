using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationApp.Data;
using PartyInvitationApp.Models;
using PartyInvitationApp.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PartyInvitationApp.Controllers
{
    public class InvitationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly ILogger<InvitationController> _logger;

        public InvitationController(ApplicationDbContext context, EmailService emailService, ILogger<InvitationController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: Invitation/Create
        public IActionResult Create(int partyId)
        {
            ViewBag.PartyId = partyId;
            return View();
        }

        // POST: Invitation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                // Save the invitation to the database
                _context.Add(invitation);
                await _context.SaveChangesAsync();

                // Log the invitation creation
                _logger.LogInformation($"Invitation created for {invitation.GuestEmail}");

                // Send the invitation email
                var responseUrl = Url.Action("Respond", "Invitation", new { id = invitation.Id }, Request.Scheme);
                string emailBody = $"Hello {invitation.GuestName},<br/><br/>" +
                                   $"You are invited to a party. Click <a href='{responseUrl}'>here</a> to respond.";

                try
                {
                    await _emailService.SendEmailAsync(invitation.GuestEmail, "You're Invited!", emailBody);

                    // Log if email is sent successfully
                    _logger.LogInformation($"Invitation email sent to {invitation.GuestEmail}");
                }
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError($"Error sending email to {invitation.GuestEmail}: {ex.Message}");
                    ModelState.AddModelError("", "Failed to send the invitation email.");
                }

                // After saving and sending email, redirect to the Respond page to collect response
                return RedirectToAction("Respond", "Invitation", new { id = invitation.Id });
            }

            // If the model is invalid, redisplay the form
            ViewBag.PartyId = invitation.PartyId;
            return View(invitation);
        }

        // GET: Invitation/Respond/5
        public async Task<IActionResult> Respond(int id)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null) return NotFound();

            return View(invitation);
        }

        // POST: Invitation/Respond/5
        [HttpPost]
        public async Task<IActionResult> Respond(int id, InvitationStatus status)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null) return NotFound();

            // Update the invitation status based on the response
            invitation.Status = status;
            _context.Update(invitation);
            await _context.SaveChangesAsync();

            // Redirect to the party details or guest list after response
            return RedirectToAction("Index", "Party");
        }

        // GET: Invitation/GuestList/5
        public IActionResult GuestList(int partyId)
        {
            var guests = _context.Invitations
                                 .Where(i => i.PartyId == partyId)
                                 .ToList();

            return View(guests);  // Display the list of guests and their responses
        }
    }
}
