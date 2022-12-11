using System;
using VDVI.Repository.Models.AfasModels.Dto;

namespace VDVI.Services.Rebus.Models
{
    public class AfasSchedulerEvent
    {

        public AfasSchedulerSetupDto Scheduler{ get; set; }
        public DateTime? BusinessStartDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public int LogDayLimits { get; set; }
    }
}

