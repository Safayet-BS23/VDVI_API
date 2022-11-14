using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class RoomsDto : Audit
    {   
        public string PropertyCode { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomFeatures { get; set; }
        public string ConnectingLeft { get; set; }
        public string ConnectingRight { get; set; }
        public string ExtrasNotAllowed { get; set; }
        public string HousekeepingSections { get; set; }
    }
}


