using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyInvitationApp.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string GuestName { get; set; }

        [Required, EmailAddress]
        public string GuestEmail { get; set; }

        [Required]
        public int PartyId { get; set; }

        [ForeignKey("PartyId")]
        public virtual Party? Party { get; set; }  // ✅ Make this nullable
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;
    }
}
