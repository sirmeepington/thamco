using System.Collections.Generic;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class FoodGetDto
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public float Cost { get; set; }
    }
}