using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationApp.Models
{
    public class Party
    {
        //Primary ey
        public int Id { get; set; }

        //Description of the party

        [Required]
        public string Description { get; set; }

        // Date of the party on which it uis held

        [Required]
        public DateTime DateOfParty { get; set; }



        //party venue
        public string Location { get; set; }

        public List<Invitation> Invitations { get; set; } = new();
    }
}
