using MediatR;
using System;
using VDVI.DB.Dtos;

namespace VDVI.Services.MediatR.Models
{
    public class ApmaSchedulerEvent : IRequest
    {
        public SchedulerSetupDto Scheduler { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysLimit { get; set; }
        public DateTime CurrentDate { get; set; }
        public int LogDayLimits { get; set; }
    }
}
