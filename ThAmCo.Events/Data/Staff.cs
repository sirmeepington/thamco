
using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Staff
    {
        public int StaffId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public bool FirstAider { get; set; }

    }
}