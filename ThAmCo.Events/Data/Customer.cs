using System.Collections.Generic;

namespace ThAmCo.Events.Data
{
    /// <summary>
    /// An object referencing a Customer.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The numerical Id of the customer.
        /// In the database this is represented as the identity column.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Customer's surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// The Customer's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The customer's full name created by concatenating the first name and the surname.
        /// </summary>
        public string FullName => FirstName + " " + Surname;

        /// <summary>
        /// The customer's e-mail address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A list of bookings that the customer is part of.
        /// </summary>
        public List<GuestBooking> Bookings { get; set; }

        /// <summary>
        /// Whether or not the customer has been deleted / anonymised from the database.
        /// </summary>
        public bool Deleted { get; set; }
    }
}
