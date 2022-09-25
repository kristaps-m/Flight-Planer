using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner
{
    public class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = ++_id;
            _flights.Add(flight);
            return flight;
        }

        public static PageResult FindFlightByRequest(SearchFlightsRequest req)
        {
            var Items = new List<Flight>();
            foreach (var f in _flights)
            {
                if (req.From == f.From.AirportCode && req.To == f.To.AirportCode && req.DepartureDate == f.DepartureTime)
                {
                    Items.Add(f);
                }
            }

            return new PageResult(Items.Count, Items.ToArray());
        }

        public static Flight GetFlight(int id)
        {
            if (id <= _flights.Count && id >= 0)
            {
                return _flights.FirstOrDefault(f => f.Id == id);
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

        public static bool IsThereSameFlightInStorage(Flight flight)
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

            return false;
        }

        public static bool DoesFlightHaveWrongValues(Flight flight)
        {
            if (flight.From == null || 
                (flight.From.Country == null && flight.From.City == null && flight.From.AirportCode == null) ||
                (flight.From.Country == "" && flight.From.City == "" && flight.From.AirportCode == "") ||
                flight.To == null || 
                (flight.To.Country == null && flight.To.City == null && flight.To.AirportCode == null) ||
                (flight.To.Country == "" && flight.To.City == "" && flight.To.AirportCode == "") ||
                flight.Carrier == null || flight.Carrier == "" ||
                flight.DepartureTime == null ||
                flight.ArrivalTime == null)
            {
                return true;
            }

            return false;
        }

        public static bool DoesFlightHaveSameAirport(Flight flight)
        {
            if (flight.From == flight.To ||
                LowAndTrim(flight.From.AirportCode) == LowAndTrim(flight.To.AirportCode))
            {
                return true;
            }

            return false;
        }

        public static bool DoesPlaneTakeOfAndLandOnRightTime(Flight flight)
        {
            // <0 − If date1 is earlier than date2
            if (DateTime.Compare(DateTime.Parse(flight.ArrivalTime),DateTime.Parse(flight.DepartureTime)) < 0 ||
                // 0 − If date1 is the same as date2
                DateTime.Compare(DateTime.Parse(flight.ArrivalTime),DateTime.Parse(flight.DepartureTime)) == 0)
            {
                return true;
            }

            return false;
        }

        public static Airport[] SearchAirports(string phrase)
        {
            var listToReturn = new List<Airport>();
            phrase = LowAndTrim(phrase);

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

            return listToReturn.ToArray();
        }
        
        // public static void FindFlight(int id)
        // {
        //     var flightListRange = _flights.Count;
        //     var theFlightIndex = 0;
        //
        //     if (id <= flightListRange && id >= 0)
        //     {
        //         for (int i = 0; i < flightListRange; i++)
        //         {
        //             if (_flights[i].Id == id)
        //             {
        //                 theFlightIndex = i;
        //             }
        //         }
        //     
        //         _flights.RemoveAt(theFlightIndex);
        //     }
        // }

        private static string LowAndTrim(string word)
        {
            return word.ToLower().Trim();
        }
    }
}