using EventScheduler.Dtos;
using EventScheduler.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var events = new List<Event>
        {
            new Event { Id = 1, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), Location = "A", Priority = 50 },
            new Event { Id = 2, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0), Location = "B", Priority = 30 },
            new Event { Id = 3, StartTime = new TimeSpan(11, 30, 0), EndTime = new TimeSpan(12, 30, 0), Location = "A", Priority = 40 },
            new Event { Id = 4, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(16, 0, 0), Location = "C", Priority = 70 },
            new Event { Id = 5, StartTime = new TimeSpan(14, 25, 0), EndTime = new TimeSpan(15, 30, 0), Location = "B", Priority = 60 },
            new Event { Id = 6, StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(14, 0, 0), Location = "D", Priority = 80 }
        };

        var locationDurations = new List<DurationTime>
        {
            new DurationTime { CurrentLocation = "A", TargetLocation = "B", DurationMinutes = 15 },
            new DurationTime { CurrentLocation = "A", TargetLocation = "C", DurationMinutes = 20 },
            new DurationTime { CurrentLocation = "A", TargetLocation = "D", DurationMinutes = 10 },
            new DurationTime { CurrentLocation = "B", TargetLocation = "C", DurationMinutes = 5 },
            new DurationTime { CurrentLocation = "B", TargetLocation = "D", DurationMinutes = 25 },
            new DurationTime { CurrentLocation = "C", TargetLocation = "D", DurationMinutes = 25 }
        };

        var eventScheduler = new EventService(events, locationDurations);
        var maxPriorityEvents = eventScheduler.GetEvents();

        Console.WriteLine("Katılınabilecek Maksimum Etkinlik Sayısı: " + maxPriorityEvents.Count);
        Console.WriteLine("Katılınabilecek Etkinliklerin ID'leri: " + string.Join(", ", maxPriorityEvents.Select(e => e.Id)));
        Console.WriteLine("Toplam Değer: " + maxPriorityEvents.Sum(e => e.Priority));
    }
}
