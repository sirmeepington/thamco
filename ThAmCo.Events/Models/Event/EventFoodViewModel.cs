using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Models;

namespace ThAmCo.Events.Models
{
    public class EventFoodViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Event Type")]
        public string EventType { get; set; }

        public MenuGetDto Menu { get; set; }

        public List<MenuGetDto> AvailableMenus { get; set; }

    }
}