using System;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class SchedulerSetupDto : Audit
    {
        public string SchedulerName { get; set; }
        public DateTime? LastExecutionDateTime { get; set; }
        public DateTime? NextExecutionDateTime { get; set; } 
        public int ExecutionIntervalMins { get; set; }
        public DateTime? BusinessStartDate { get; set; }
        public bool isFuture { get; set; }
        public bool isActive { get; set; }
        public int DaysLimit { get; set; }
        public DateTime? LastBusinessDate { get; set; }
        public string SchedulerStatus { get; set; }
    }
}
