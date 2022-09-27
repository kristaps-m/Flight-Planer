using System;

namespace FlightPlanner.Validate
{
    public class ValidateClass
    {
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
        
        public static bool DoesFlightHaveSameAirport(Flight flight)
        {
            if (LowAndTrim(flight.From.AirportCode) == LowAndTrim(flight.To.AirportCode))
            {
                return true;
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

        private static string LowAndTrim(string word)
        {
            return word.ToLower().Trim();
        }
    }
}