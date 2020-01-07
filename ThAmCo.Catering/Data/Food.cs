using System.Collections.Generic;

namespace ThAmCo.Catering.Data
{
    public class Food
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Cost { get; set; }

        public List<MenuFood> Menus { get; set; }
    }
}