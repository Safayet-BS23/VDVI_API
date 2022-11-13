using System;
using System.Collections.Generic;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class ReservationDashboardReservationFutureDto : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? DashboardDate { get; set; }
        public int CreatedReservations { get; set; }
        public int CreatedRoomNights { get; set; }
        public int StayOvers { get; set; }
        public int CancelledReservations { get; set; }
        public int CancelledRoomNights { get; set; }
        public int CheckIns { get; set; }
        public int CheckedIn { get; set; }
        public int CheckOuts { get; set; }
        public int CheckedOut { get; set; }
        public int DayUseCheckIns { get; set; }
        public int DayUseCheckOuts { get; set; }
        public decimal AverageLengthOfStay { get; set; }
        public decimal AverageLeadTime { get; set; }
        public int NoShowProcessed { get; set; }
        public int NoShowUnprocessed { get; set; }
    }
}
