using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[Rooms]")]
    public class DbRooms : Audit
    {
        public string PropertyCode { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public bool RoomFeatures { get; set; }
        public string ConnectingLeft { get; set; }
        public string ConnectingRight { get; set; }
        public string ExtrasNotAllowed { get; set; }
        public string HousekeepingSections { get; set; }
    }
}


