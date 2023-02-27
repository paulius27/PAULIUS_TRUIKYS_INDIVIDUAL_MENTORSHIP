using System;

namespace BL.Models
{
    public class TimeInterval
    {
        public TimeInterval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
