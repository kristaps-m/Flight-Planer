using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner
{
    public class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;
        private static readonly object ObjectLock = new object();

        public static Flight AddFlight(Flight flight)
        {
            lock (ObjectLock)
            {
                flight.Id = ++_id;
                _flights.Add(flight);
            }
            return flight;
        }

        public static PageResult FindFlightByRequest(SearchFlightsRequest req)
        {
            var Items = new List<Flight>();
            lock (ObjectLock)
            {
                foreach (var f in _flights)
                {
                    if (req.From == f.From.AirportCode && req.To == f.To.AirportCode &&
                        req.DepartureDate == f.DepartureTime.Substring(0,10))
                    {
                        Items.Add(f);
                    }
                }
            }
            
            return new PageResult(Items.Count, Items.ToArray());
        }

        public static Flight GetFlight(int id)
        {
            lock (ObjectLock)
            {
                if (id <= _flights.Count && id >= 0)
                {
                    return _flights.FirstOrDefault(f => f.Id == id);
                }
            }

            return null;
        }

        public static void Clear()
        {
            _flights.Clear();
            _id = 0;
        }

        public static void Delete(int id)
        {
            lock (ObjectLock)
            {
                var flightListRange = _flights.Count;
                var theFlightIndex = 0;

                if (id <= flightListRange && id >= 0)
                {
                    for (int i = 0; i < flightListRange; i++)
                    {
                        if (_flights[i].Id == id)
                        {
                            theFlightIndex = i;
                        }
                    }

                    _flights.RemoveAt(theFlightIndex);
                }
            }
        }
        
        public static Airport[] SearchAirports(string phrase)
        {
            var listToReturn = new List<Airport>();
            phrase = LowAndTrim(phrase);

            lock (ObjectLock)
            {
                foreach (Flight f in _flights)
                {
                    if (LowAndTrim(f.To.Country).Contains(phrase) || 
                        LowAndTrim(f.To.City).Contains(phrase) || 
                        LowAndTrim(f.To.AirportCode).Contains(phrase))
                    {
                        listToReturn.Add(f.To);
                    }
                
                    if (LowAndTrim(f.From.Country).Contains(phrase) || 
                        LowAndTrim(f.From.City).Contains(phrase) || 
                        LowAndTrim(f.From.AirportCode).Contains(phrase))
                    {
                        listToReturn.Add(f.From);
                    }
                }
            }

            return listToReturn.ToArray();
        }
        
        public static bool IsThereSameFlightInStorage(Flight flight)
        {
            lock (ObjectLock)
            {
                foreach (var f in _flights)
                {
                    if (LowAndTrim(flight.ArrivalTime) == LowAndTrim(f.ArrivalTime) &&
                        LowAndTrim(flight.DepartureTime) == LowAndTrim(f.DepartureTime) &&
                        LowAndTrim(flight.From.City) == LowAndTrim(f.From.City) &&
                        LowAndTrim(flight.From.AirportCode) == LowAndTrim(f.From.AirportCode) &&
                        LowAndTrim(flight.From.Country) == LowAndTrim(f.From.Country) &&
                        LowAndTrim(flight.Carrier) == LowAndTrim(f.Carrier) &&
                        LowAndTrim(flight.To.City) == LowAndTrim(f.To.City) &&
                        LowAndTrim(flight.To.AirportCode) == LowAndTrim(f.To.AirportCode) &&
                        LowAndTrim(flight.To.Country) == LowAndTrim(f.To.Country))
                    {
                        return true;
                    }
                }
            }
        
            return false;
        }

        private static string LowAndTrim(string word)
        {
            return word.ToLower().Trim();
        }

        public static List<Flight> GetListOfFlights()
        {
            return _flights;
        }
    }
}