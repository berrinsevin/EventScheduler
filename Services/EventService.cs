using EventScheduler.Dtos;

namespace EventScheduler.Services
{
    public class EventService
    {
        private List<Event> _events;
        private List<DurationTime> _locationDurations;

        public EventService(List<Event> events, List<DurationTime> locationDurations)
        {
            _events = events;
            _locationDurations = locationDurations;
        }

        private int GetTravelTime(string startPoint, string destination)
        {
            if (startPoint == destination)
            {
                return 0;
            }

            var travelTime = _locationDurations.FirstOrDefault(d =>
                (d.CurrentLocation == startPoint && d.TargetLocation == destination) || 
                (d.CurrentLocation == destination && d.TargetLocation == startPoint))?.DurationMinutes ?? 0;

            return travelTime;
        }

        public List<Event> GetEvents()
        {
            if (_events.Count < 0)
            {
                return new List<Event>();
            }

            var sortedEvents = _events.OrderBy(e => e.StartTime).ThenByDescending(e => e.Priority).ToList();

            var selectedEvents = new List<Event>();
            var currentEvent = sortedEvents.First();
            selectedEvents.Add(currentEvent);
            sortedEvents.Remove(currentEvent);

            while (sortedEvents.Any(x => x.StartTime > currentEvent.EndTime))
            {
                var nextEvents = sortedEvents.Where(x => {
                    var travelTime = GetTravelTime(currentEvent.Location, x.Location);
                    var lastEventEnd = currentEvent.EndTime;
                    var nextEventStart = x.StartTime;
                    return nextEventStart >= lastEventEnd + TimeSpan.FromMinutes(travelTime);
                })
               .OrderByDescending(e => e.EndTime).OrderBy(e => e.StartTime).ThenByDescending(e => e.Priority).ToList();
                
                currentEvent = nextEvents.First(); 
                nextEvents.Remove(currentEvent);
                selectedEvents.Add((Event)currentEvent);

                sortedEvents = nextEvents;
            }

            return selectedEvents;
        }
    }

}
