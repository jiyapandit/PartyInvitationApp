using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationApp.Data;
using PartyInvitationApp.Models;
using PartyInvitationApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PartyInvitationApp.Controllers
{
    public class InvitationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public InvitationController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int partyId, Invitation invitation)
        {
            Console.WriteLine("=== InvitationController.Create CALLED ===");
            Console.WriteLine($"partyId = {partyId}, GuestName = {invitation.GuestName}, GuestEmail = {invitation.GuestEmail}");

            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.Id == partyId);

            if (party == null)
            {
                Console.WriteLine("❌ No party found with that ID!");
                return NotFound();
            }

            // ✅ Make sure the party ID is set
            invitation.PartyId = partyId;

            // ✅ Explicitly attach the party reference
            invitation.Party = party;

            // ✅ Debugging ModelState Errors
            Console.WriteLine($"✅ ModelState Valid: {ModelState.IsValid}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState Errors:");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($" - {key}: {error.ErrorMessage}");
                    }
                }
                return RedirectToAction("Manage", "Party", new { id = partyId });
            }

            // ✅ Save the Invitation to the database
            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Invitation saved successfully!");

            // ✅ Send email
            var responseUrl = Url.Action("Respond", "Invitation", new { id = invitation.Id }, Request.Scheme);
            string emailBody = $@"
        <h2>Hello {invitation.GuestName},</h2>
        <p>You are invited to <strong>{party.Description}</strong> on {party.DateOfParty.ToShortDateString()} at {party.Location}.</p>
        <p><a href='{responseUrl}' style='display: inline-block; padding: 10px; background: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Click here to RSVP</a></p>";

            bool sent = await _emailService.SendEmailAsync(invitation.GuestEmail, "You're Invited!", emailBody);
            if (sent)
            {
                invitation.Status = InvitationStatus.InviteSent;
                _context.Update(invitation);
                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Email sent successfully & invitation status updated.");
            }
            else
            {
                Console.WriteLine("❌ Email sending failed!");
            }

            return RedirectToAction("Manage", "Party", new { id = partyId });
        }




        // POST: /Invitation/SendAll?partyId=5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAll(int partyId)
        {
            Console.WriteLine("=== InvitationController.SendAll CALLED ===");
            Console.WriteLine($"partyId = {partyId}");

            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.Id == partyId);

            if (party == null)
            {
                Console.WriteLine("No party found with that ID!");
                return NotFound();
            }

            var unsentInvites = party.Invitations
                .Where(i => i.Status == InvitationStatus.InviteNotSent)
                .ToList();

            Console.WriteLine($"Found {unsentInvites.Count} unsent invites.");

            foreach (var invite in unsentInvites)
            {
                var responseUrl = Url.Action("Respond", "Invitation", new { id = invite.Id }, Request.Scheme);
                string body = $"Hello {invite.GuestName},<br/>" +
                              $"You are invited to \"{party.Description}\" on {party.DateOfParty.ToShortDateString()}, at {party.Location}.<br/>" +
                              $"<a href='{responseUrl}'>Click here</a> to respond.";

                bool success = await _emailService.SendEmailAsync(invite.GuestEmail, "You Are Invited!", body);
                Console.WriteLine($"Sending to {invite.GuestEmail}: success = {success}");

                if (success)
                {
                    invite.Status = InvitationStatus.InviteSent;
                    _context.Update(invite);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Updated unsent invites to InviteSent where successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving after SendAll: {ex.Message}");
            }

            return RedirectToAction("Manage", "Party", new { id = partyId });
        }

        // GET: /Invitation/Respond/{id}
        [HttpGet]
        [Route("Invitation/Respond/{id}")]
        public async Task<IActionResult> Respond(int id)
        {
            Console.WriteLine("=== InvitationController.Respond (GET) CALLED ===");
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invitation == null)
            {
                Console.WriteLine("No invitation found for that ID!");
                return NotFound();
            }

            return View(invitation);
        }

        // POST: /Invitation/Respond/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Invitation/Respond/{id}")]
        public async Task<IActionResult> Respond(int id, [FromForm] string rsvp)
        {
            Console.WriteLine("=== InvitationController.Respond (POST) CALLED ===");
            Console.WriteLine($"id = {id}, rsvp = {rsvp}");

            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invitation == null)
            {
                Console.WriteLine("No invitation found for that ID!");
                return NotFound();
            }

            invitation.Status = (rsvp == "yes") ? InvitationStatus.RespondedYes : InvitationStatus.RespondedNo;
            _context.Update(invitation);
            await _context.SaveChangesAsync();

            return View("ThankYou", invitation);
        }

        // GET: /Invitation/GuestList?partyId=5
        public async Task<IActionResult> GuestList(int partyId)
        {
            Console.WriteLine("=== InvitationController.GuestList CALLED ===");
            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.Id == partyId);

            if (party == null)
            {
                Console.WriteLine("No party found for that ID!");
                return NotFound();
            }

            var invites = party.Invitations;
            return View("GuestList", invites);
        }
    }
}
