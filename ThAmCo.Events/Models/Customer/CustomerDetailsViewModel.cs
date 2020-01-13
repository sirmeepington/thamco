using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used when viewing the details of a <see cref="Data.Customer"/>.
    /// </summary>
    public class CustomerDetailsViewModel
    {
        /// <inheritdoc cref="Data.Customer.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="Data.Customer.Surname"/>
        [Required]
        public string Surname { get; set; }

        /// <inheritdoc cref="Data.Customer.FirstName"/>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <inheritdoc cref="Data.Customer.Email"/>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// A list of <see cref="GuestBookingDetailsViewModel"/>. <para />
        /// Unlike <see cref="Data.Customer.Bookings"/> this uses the details 
        /// view model instead of the raw entity as it is more suited in this case.
        /// </summary>
        public IEnumerable<GuestBookingDetailsViewModel> Bookings { get; set; }

        /// <inheritdoc cref="Data.Customer.FirstName"/>
        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + Surname;


    }
}
