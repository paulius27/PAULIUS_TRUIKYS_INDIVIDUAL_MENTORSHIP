using DAL.Models;

namespace BL.Validation
{
    public class TimeRangeValidator : IValidator<TimeRange>
    {
        public bool Validate(TimeRange timeRange) => timeRange.Start < timeRange.End;
    }
}
