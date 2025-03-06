using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyInvitationApp.Models
{
    public class Invitation
    {
        // Primary Key

        [Key]
        public int Id { get; set; }

        // Guest's full name 

        [Required]
        public string GuestName { get; set; }

        // Guest's email

        [Required, EmailAddress]
        public string GuestEmail { get; set; }

        // Foreign key 
        [Required]
        public int PartyId { get; set; }


        [ForeignKey("PartyId")]
        public virtual Party? Party { get; set; }

        // Default status: Invitation has not been sent yet
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;
    }
}
