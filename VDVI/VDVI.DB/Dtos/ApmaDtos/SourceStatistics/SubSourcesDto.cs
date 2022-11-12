using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class SubSourcesDto : Audit
    {
        public string PropertyCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Listorder { get; set; }
        public bool? Visible { get; set; } 

    }
}
