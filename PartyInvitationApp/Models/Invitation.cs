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
        public int PartyId { get; set; }  // This should not be nullable

        [ForeignKey("PartyId")]
        public virtual Party Party { get; set; }  // This allows EF to map it properly

        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;
    }
}
