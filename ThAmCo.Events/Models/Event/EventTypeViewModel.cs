namespace ThAmCo.Events.Models
{

    /// <summary>
    /// A view model which maps the <see cref="Venues.Data.EventType"/> into a view model./>.
    /// </summary>
    public class EventTypeViewModel
    {
        /// <inheritdoc cref="Venues.Data.EventType.Id" />
        public string Id { get; set; }

        /// <inheritdoc cref="Venues.Data.EventType.Title" />
        public string Title { get; set; }
    }

}