// Models/Invitation.cs
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationApp.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        [Required]
        public string GuestName { get; set; }

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

        public int PartyId { get; set; }
        public Party Party { get; set; }
        
    }
}