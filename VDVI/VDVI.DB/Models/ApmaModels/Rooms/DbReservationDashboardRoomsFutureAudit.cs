using System;
using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[ReservationDashboard_Rooms_FutureAudit]")]
    public class DbReservationDashboardRoomsFutureAudit : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? DashboardDate { get; set; }
        public int Inventory { get; set; }
        public int OutOfInventory { get; set; }
        public int OutOfOrder { get; set; }
    }
}
