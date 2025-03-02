using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationApp.Data;
using PartyInvitationApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PartyInvitationApp.Controllers
{
    public class PartyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Party
        public async Task<IActionResult> Index()
        {
            // Cookie track first visit
            if (!Request.Cookies.ContainsKey("FirstVisit"))
            {
                var cookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddYears(1) };
                Response.Cookies.Append("FirstVisit", DateTime.UtcNow.ToString(), cookieOptions);
            }
            ViewBag.FirstVisit = Request.Cookies["FirstVisit"];

            var parties = await _context.Parties
                .Include(p => p.Invitations)
                .ToListAsync();
            return View(parties);
        }

        // GET: /Party/Create
        public IActionResult Create() => View();

        // POST: /Party/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Party party)
        {
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: /Party/Manage/5
        public async Task<IActionResult> Manage(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (party == null) return NotFound();

            ViewBag.NotSent = party.Invitations.Count(i => i.Status == InvitationStatus.InviteNotSent);
            ViewBag.Sent = party.Invitations.Count(i => i.Status == InvitationStatus.InviteSent);
            ViewBag.YesCount = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedYes);
            ViewBag.NoCount = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedNo);

            return View(party);
        }

        // GET: /Party/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null) return NotFound();
            return View(party);
        }

        // POST: /Party/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Party party)
        {
            if (id != party.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(party);
                await _context.SaveChangesAsync();
                return RedirectToAction("Manage", new { id = party.Id });
            }
            return View(party);
        }

        // GET: /Party/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null) return NotFound();
            return View(party);
        }

        // POST: /Party/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null) return NotFound();

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
