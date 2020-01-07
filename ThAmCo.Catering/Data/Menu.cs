using System.Collections.Generic;

namespace ThAmCo.Catering.Data
{
    public class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public List<MenuFood> Food { get; set; }
    }
}