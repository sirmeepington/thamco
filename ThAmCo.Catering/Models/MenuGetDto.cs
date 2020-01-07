using System.Collections.Generic;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class MenuGetDto
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<FoodGetDto> Food { get; set; }

    }
}