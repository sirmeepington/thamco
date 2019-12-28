using System.Collections.Generic;

namespace ThAmCo.Events.Data
{
    public class Customer
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string FullName => FirstName + " " + Surname;

        public string Email { get; set; }

        public List<GuestBooking> Bookings { get; set; }

        public bool Deleted { get; set; }
    }
}
