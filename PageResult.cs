using System;

namespace FlightPlanner
{
    public class PageResult
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public Array Items { get; set; }

        public PageResult(int totalItems,Array items)
        {
            Page = 0;
            TotalItems = totalItems;
            Items = items;
        }
    }
}