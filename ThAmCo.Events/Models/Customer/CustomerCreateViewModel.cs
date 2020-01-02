using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Models
{
    /// <summary>
    /// A view model to be used in the creation of a <see cref="Data.Customer"/>.
    /// </summary>
    public class CustomerCreateViewModel
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
        public string Email { get; set; }

    }
}
