// Controllers/PartyController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationApp.Data;
using PartyInvitationApp.Models;
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

        // List all parties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parties.Include(p => p.Invitations).ToListAsync());
        }

        // Show details
        public async Task<IActionResult> Details(int id)
        {
            var party = await _context.Parties.Include(p => p.Invitations)
                                              .FirstOrDefaultAsync(p => p.Id == id);
            if (party == null) return NotFound();
            return View(party);
        }

        // Show Edit Page
        public async Task<IActionResult> Edit(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null) return NotFound();
            return View(party);
        }

        // Handle Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Party party)
        {
            if (id != party.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }
    }
}
