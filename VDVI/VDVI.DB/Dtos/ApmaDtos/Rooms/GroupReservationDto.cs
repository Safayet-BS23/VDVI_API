using System;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class GroupReservationDto : Audit
    {
		public string PropertyCode { get; set; }
		public string GroupReservationNumber { get; set; }
		public string GroupName { get; set; }
		public DateTime? GroupStartDate { get; set; }
		public DateTime? GroupEndDate { get; set; }
		public decimal Balance { get; set; }
		public decimal TotalExcl { get; set; }
		public decimal TotalIncl { get; set; }
		public int TotalRoomsContracted { get; set; }
		public int TotalRoomsPickedUp { get; set; }
		public int TotalAdults { get; set; }
		public int TotalChildren { get; set; }
		public int TotalInfants { get; set; }
		public string Status { get; set; }
		public string Booker { get; set; }
		public string Company { get; set; }
		public string TravelAgent { get; set; }
		public string PaymentType { get; set; }
		public string GuaranteeType { get; set; }
		public string Market { get; set; }
		public string Source { get; set; }
		public string Channel { get; set; }
		public decimal DepositAmount1 { get; set; }
		public DateTime? DepositDueDate1 { get; set; }
		public decimal DepositAmount2 { get; set; }
		public DateTime? DepositDueDate2 { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public string BlockStatus { get; set; }
		public DateTime? OptionDate { get; set; }
		public int TotalChildrenOld { get; set; }
		public int TotalChildrenYoung { get; set; }
		public int TotalBabies { get; set; }
		public string StatusInfo { get; set; }
	}
}
