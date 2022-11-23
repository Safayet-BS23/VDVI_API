using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[FolioDetails]")]
    public class DbFolioDetail : Audit
    {
		public string PropertyCode { get; set; }
		public string FolioID { get; set; }
		public string BillTo { get; set; }
		public string RelationNumber { get; set; }
		public string RelationEmail { get; set; }
		public string InvoiceName1 { get; set; }
		public string InvoiceName2 { get; set; }
		public string InvoiceStreet1 { get; set; }
		public string InvoiceNumber1 { get; set; }
		public string InvoiceStreet2 { get; set; }
		public string InvoiceNumber2 { get; set; }
		public string InvoiceZipCode { get; set; }
		public string InvoiceCity { get; set; }
		public string InvoiceState { get; set; }
		public string InvoiceCountry { get; set; }
		public string InvoiceCountryIso2Code { get; set; }
		public string PaymentType { get; set; }
		public bool? ReadyForAR { get; set; }
		public string Reference { get; set; }
		public string CardOnFile { get; set; }
		public decimal? Balance { get; set; }
	}
}
