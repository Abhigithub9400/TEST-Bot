﻿namespace MediAssist.UI.Models
{
    public class UpdateClinicViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public string? Email { get; set; }
        public string? Website { get; set; }

        public string Logo { get; set; }

        public string FavIcon { get; set; }
    }
}
