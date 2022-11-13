using System;
using System.Collections.Generic;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class ReservationDashboardOccupancyFutureAuditDto : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? DashboardDate { get; set; }
        public decimal Percentage { get; set; }
        public int RoomsSold { get; set; }
        public int AvailableRooms { get; set; }
    }
}
