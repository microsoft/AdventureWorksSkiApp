using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gen_skilocations
{
    class Program
    {
        static void Main(string[] args)
        {
            EventHubClient client = EventHubClient.CreateFromConnectionString(ConfigurationManager.AppSettings["EventHubConnectionString"],
                                                                              ConfigurationManager.AppSettings["EventHubConnectionPath"]);
            new SkiersSimulation(skierCount: 2500, 
                                 locationReportPeriod: 60, 
                                 oneSecond: TimeSpan.FromSeconds(1), 
                                 client: client).Start();
        }
    }

    class SkiersSimulation
    {
        static readonly ChairLift[] Lifts =
        {
            new ChairLift { Name = "Education Hill", Top = new Point { Latitude = 46.924325, Longitude = -121.501723 }, Bottom = new Point { Latitude = 46.934583, Longitude = -121.475373 } },
            new ChairLift { Name = "Bear Creek", Top = new Point { Latitude = 46.921218, Longitude = -121.500521 }, Bottom = new Point { Latitude = 46.933293, Longitude = -121.473613 } },
            new ChairLift { Name = "Overlake Jump", Top = new Point { Latitude = 46.929431, Longitude = -121.502700 }, Bottom = new Point { Latitude = 46.932098, Longitude = -121.488774 } },
            new ChairLift { Name = "Belltown Express", Top = new Point { Latitude = 46.931571, Longitude = -121.502228 }, Bottom = new Point { Latitude = 46.931878, Longitude = -121.486242 } },
            new ChairLift { Name = "Grass Lawn Caf", Top = new Point { Latitude = 46.923584, Longitude = -121.491650 }, Bottom = new Point { Latitude = 46.926325, Longitude = -121.482487 } },
            new ChairLift { Name = "Redmond Way", Top = new Point { Latitude = 46.920726, Longitude = -121.484011 }, Bottom = new Point { Latitude = 46.929065, Longitude = -121.481393 } },
            new ChairLift { Name = "Borealis", Top = new Point { Latitude = 46.920463, Longitude = -121.491864 }, Bottom = new Point { Latitude = 46.934281, Longitude = -121.475664} },
            new ChairLift { Name = "Bear Creek II", Top = new Point { Latitude = 46.935424, Longitude = -121.487358 }, Bottom = new Point { Latitude = 46.936523, Longitude = -121.476737 } }
        };

        private readonly int _skierCount;
        private readonly long _reportPeriod;
        private readonly TimeSpan _oneSecond;
        private readonly EventHubClient _eventHubClient;
        private Task _sendEventTask;

        public SkiersSimulation(int skierCount, long locationReportPeriod, TimeSpan oneSecond, EventHubClient client)
        {
            _skierCount = skierCount;
            _reportPeriod = locationReportPeriod;
            _oneSecond = oneSecond;
            _eventHubClient = client;
        }

        public void Start()
        {
            Random random = new Random();

            List<SimulatedChairlift> simulatedLifts = Lifts.Select(l => new SimulatedChairlift(l)).ToList();

            // TODO: add new skiers gradually
            List<Skier> skiers = new List<Skier>(_skierCount);
            for (int i = 0; i < _skierCount; i++)
            {
                // TODO: skew towards easier runs
                int lift = random.Next(0, Lifts.Length);
                Skier skier = new Skier { Location = simulatedLifts[lift].ChairLift.Bottom };
                simulatedLifts[lift].SkiersWaiting.Enqueue(skier);
                skiers.Add(skier);
            }

            Skier reference = simulatedLifts[0].SkiersWaiting.Peek();

            long currentTime = 0;
            Stopwatch watch = new Stopwatch();

            while (true)
            {
                watch.Restart();

                foreach (SimulatedChairlift lift in simulatedLifts)
                {
                    if (currentTime - lift.LastLoadTime >= lift.ChairLift.TimeBetweenLoads)
                    {
                        LoadUnloadSkiers(lift, currentTime);
                    }

                    // TODO: variability in time-to-bottom
                    Func<Skier, bool> doneWithRunPredicate = s => currentTime - s.StartTimeForCurrentStage >= lift.ChairLift.AverageTimeDown;

                    MoveSkiersToWaiting(currentTime, lift, doneWithRunPredicate);
                }

                currentTime++;
                watch.Stop();
                if (watch.Elapsed < _oneSecond)
                {
                    Thread.Sleep(_oneSecond - watch.Elapsed);
                }

                if (currentTime % _reportPeriod == 0)
                {
                    // Create a real timeline for events by assuming the simulation starts at 8 AM and goes on in real time
                    DateTimeOffset now = DateTimeOffset.Now;
                    DateTimeOffset eventTime = new DateTimeOffset(now.Year, now.Month, now.Day, 8, 0, 0, now.Offset);
                    eventTime = eventTime.Add(TimeSpan.FromSeconds(currentTime));
                    // Create a copy of the list and of the data since we'll send this in the background and want
                    // a stable view of the state of the simulation
                    SendLocationEvents(skiers.Select(s => new LocationEvent(s, eventTime)).ToList());
                }

                if (currentTime % 4 == 0)
                {
                    ReportOut(currentTime, simulatedLifts, reference, watch);
                }

                CheckInput(currentTime, simulatedLifts);
            }
        }

        private static void MoveSkiersToWaiting(long currentTime, SimulatedChairlift lift, Func<Skier, bool> doneWithRunPredicate)
        {
            List<Skier> remove = new List<Skier>();
            foreach (Skier skier in lift.SkiersSkiingToThisLift.Where(doneWithRunPredicate))
            {
                remove.Add(skier);
                skier.StartTimeForCurrentStage = currentTime;
                skier.Location = lift.ChairLift.Bottom;
                Debug.Assert(!lift.SkiersWaiting.Contains(skier));
                lift.SkiersWaiting.Enqueue(skier);
            }
            foreach (Skier skier in remove)
            {
                lift.SkiersSkiingToThisLift.Remove(skier);
            }
        }

        private void CheckInput(long currentTime, List<SimulatedChairlift> lifts)
        {
            int key = -1;
            while (Console.KeyAvailable)
            {
                key = Console.Read();
            }

            switch (key)
            {
                case 's':
                    lifts[1].ChairLift.TimeBetweenLoads = 60;
                    MoveSkiersToWaiting(currentTime, lifts[1], s => true);
                    Console.WriteLine("#### slow mode for lift 1");
                    break;

                case 'f':
                    lifts[1].ChairLift.TimeBetweenLoads = 1;
                    Console.WriteLine("#### fast mode for lift 1");
                    break;

                case 'n':
                    lifts[1].ChairLift.TimeBetweenLoads = 10;
                    Console.WriteLine("#### normal mode for lift 1");
                    break;
            }
        }

        private void ReportOut(long currentTime, List<SimulatedChairlift> simulatedLifts, Skier reference, Stopwatch watch)
        {
            Console.WriteLine($"------------{currentTime}");

            string ridingLift = simulatedLifts.SingleOrDefault(l => l.SkiersRiding.Contains(reference))?.ChairLift.Name ?? "<none>";
            string waitingLift = simulatedLifts.SingleOrDefault(l => l.SkiersWaiting.Contains(reference))?.ChairLift.Name ?? "<none>";
            string skiiingToLift = simulatedLifts.SingleOrDefault(l => l.SkiersSkiingToThisLift.Contains(reference))?.ChairLift.Name ?? "<none>";
            if (ridingLift != "<none>" && waitingLift != "<none>" ||
                ridingLift != "<none>" && skiiingToLift != "<none>" ||
                waitingLift != "<none>" && skiiingToLift != "<none>")
            {
                Debug.Assert(false);
            }
            Console.WriteLine($"{reference.Id}\t{reference.Location.Latitude}\t{waitingLift}\t{ridingLift}\t{skiiingToLift}\t{watch.ElapsedMilliseconds}");

            string line = "waiting";
            foreach (var lift in simulatedLifts)
            {
                line += $"\t{lift.SkiersWaiting.Count}";
            }
            Console.WriteLine(line);

            line = "riding";
            foreach (var lift in simulatedLifts)
            {
                line += $"\t{lift.SkiersRiding.Count}";
            }
            Console.WriteLine(line);
        }

        private void LoadUnloadSkiers(SimulatedChairlift lift, long currentTime)
        {
            for (int i = 0; i < lift.ChairLift.PassengerCount && lift.SkiersWaiting.Count > 0; i++)
            {
                Skier skier = lift.SkiersWaiting.Dequeue();
                skier.StartTimeForCurrentStage = currentTime;
                skier.Location = lift.ChairLift.Top; // TODO: interpolate location between bottom and top
                Debug.Assert(!lift.SkiersRiding.Contains(skier));
                lift.SkiersRiding.Enqueue(skier);
            }

            for (int i = 0; i < lift.ChairLift.PassengerCount && lift.SkiersRiding.Count > 0; i++)
            {
                Skier skier = lift.SkiersRiding.Peek();
                if (currentTime - skier.StartTimeForCurrentStage >= lift.ChairLift.TimeToTop)
                {
                    skier = lift.SkiersRiding.Dequeue();
                    skier.StartTimeForCurrentStage = currentTime;
                    skier.Location = lift.ChairLift.Top;
                    // TODO: randomize new chairlift target
                    Debug.Assert(!lift.SkiersSkiingToThisLift.Contains(skier));
                    lift.SkiersSkiingToThisLift.Add(skier);
                }

                Debug.Assert(lift.SkiersRiding.Contains(skier) ^ lift.SkiersSkiingToThisLift.Contains(skier));
            }

            lift.LastLoadTime = currentTime;
        }

        private void SendLocationEvents(List<LocationEvent> events)
        {
            if (_sendEventTask != null)
            {
                if (!_sendEventTask.IsCompleted)
                {
                    Console.WriteLine("\twaiting for previous send to complete.");
                    _sendEventTask.Wait();
                }
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();

            const int maxEvents = 1000;
            List<Task> tasks = new List<Task>();
            for (int offset = 0; offset < events.Count; offset += maxEvents)
            {
                tasks.Add(_eventHubClient.SendBatchAsync(events.Skip(offset).Take(maxEvents).Select(e => new EventData(SerializeToJsonUtf8(e)))));
            }

            _sendEventTask = Task.WhenAll(tasks.ToArray())
                                 .ContinueWith(t =>
                                 {
                                     watch.Stop();

                                     if (t.IsFaulted)
                                     {
                                         Console.WriteLine($"***** exception: {t.Exception}");
                                     }
                                     else
                                     {
                                         Console.WriteLine($"****** events sent\t{events.Count}\t{watch.ElapsedMilliseconds}");
                                     }
                                 });            
        }

        private byte[] SerializeToJsonUtf8(object o)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
        }
    }

    class SimulatedChairlift
    {
        public SimulatedChairlift(ChairLift lift)
        {
            ChairLift = lift;
            SkiersRiding = new Queue<Skier>();
            SkiersWaiting = new Queue<Skier>();
            SkiersSkiingToThisLift = new HashSet<Skier>();
        }

        public ChairLift ChairLift { get; private set; }

        public Queue<Skier> SkiersRiding { get; private set; }

        public Queue<Skier> SkiersWaiting { get; private set; }

        public HashSet<Skier> SkiersSkiingToThisLift { get; private set; }

        public long LastLoadTime { get; set; }
    }

    class Skier
    {
        public Skier()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }

        public Point Location { get; set; }

        public long StartTimeForCurrentStage { get; set; }
    }

    class ChairLift
    {
        public ChairLift()
        {
            TimeBetweenLoads = 10; // Wikipedia says 2400 people/hour in a detachable quad -> 40 ppl/minute -> 10 secs/load
            PassengerCount = 4; // all quads, fancy ski place
            TimeToTop = 240;
            AverageTimeDown = 180;
        }

        public string Name { get; set; }

        public Point Top { get; set; }

        public Point Bottom { get; set; }

        public long TimeBetweenLoads { get; set; }

        public int PassengerCount { get; set; }

        public long TimeToTop { get; set; }

        public long AverageTimeDown { get; set; }
    }

    class Point
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    class LocationEvent
    {
        public LocationEvent(Skier skier, DateTimeOffset time)
        {
            SkierId = skier.Id;
            Latitude = skier.Location.Latitude;
            Longitude = skier.Location.Longitude;
            EventTime = time;
        }

        public string SkierId { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public DateTimeOffset EventTime { get; private set; }
    }
}
