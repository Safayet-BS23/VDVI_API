using System;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class ReservationDashboardRoomsFutureDto : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? DashboardDate { get; set; }
        public int Inventory { get; set; }
        public int OutOfInventory { get; set; }
        public int OutOfOrder { get; set; }
    }
}
