using System;
using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[ManagementSummary_RoomSummary_History]")]
    public class DbRoomSummaryHistory : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? BusinessDate { get; set; }
        public int InHouse { get; set; }
        public int DayUse { get; set; }
        public int LateArrival { get; set; }
        public int EarlyDeparture { get; set; }
        public int Departed { get; set; }
        public int ToDepart { get; set; }
        public int StayOver { get; set; }
        public int EarlyArrival { get; set; }
        public int Arrived { get; set; }
        public int ToArrive { get; set; }
        public int NoShow { get; set; }
        public int Complementary { get; set; }
        public int WalkIns { get; set; }
        public int RoomReservationCreated { get; set; }
        public int RoomReservationCancelled { get; set; }

    }
}
