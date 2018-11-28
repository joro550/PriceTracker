using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace PriceFinder.Tests.Stubs
{
    public class TimerStub : TimerInfo
    {
        public TimerStub() 
            : base(new DailySchedule(),new ScheduleStatus())
        {
        }
    }
}