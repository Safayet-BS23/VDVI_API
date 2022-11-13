using System;
using System.Collections.Generic;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class ReservationDashboardRoomsHistoryDto : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? DashboardDate { get; set; }
        public int Inventory { get; set; }
        public int OutOfInventory { get; set; }
        public int OutOfOrder { get; set; }
    }
}
