using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Catering.Data
{
    public class MenuFood
    {
        [Key, Column(Order= 0)]
        public int FoodId { get; set; }

        [Key, Column(Order = 1)]
        public int MenuId { get; set; }

        public Food Food { get; set; }

        public Menu Menu { get; set; }
    }
}