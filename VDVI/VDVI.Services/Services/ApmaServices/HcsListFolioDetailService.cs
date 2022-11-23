using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using SOAPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsListFolioDetailService : ApmaBaseService, IHcsListFolioDetailService
    {
        private readonly IHcsFolioDetailService _hcsFolioDetailService;
        private readonly IHcsGetDailyHistoryService _hcsGetDailyHistoryService;

        public HcsListFolioDetailService
            (
                IHcsFolioDetailService hcsFolioDetailService,
                IHcsGetDailyHistoryService hcsGetDailyHistoryService
            )
        {
            _hcsFolioDetailService = hcsFolioDetailService;
            _hcsGetDailyHistoryService = hcsGetDailyHistoryService;
        }

        public async Task<Result<PrometheusResponse>> HcsListFolioDetailAsync(DateTime startDate, DateTime endDate)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     Authentication pmsAuthentication = GetApmaAuthCredential();


                     foreach (var propertyCode in ApmaProperties)
                     {
                         var dailyHistoryListResponse = await _hcsGetDailyHistoryService.GetListHcsDailyHistoryAsync(startDate, endDate, propertyCode);

                         if (dailyHistoryListResponse.IsSuccess)
                         {
                             var uniquePMSNumberList = ((List<DailyHistoryDto>)dailyHistoryListResponse.Value.Data).Select(x => x.PmsSegmentNumber).Distinct();

                             foreach (var uniquePMS in uniquePMSNumberList)
                             {
                                 var folioDetails = await GetFolioDetailsAsync(propertyCode, uniquePMS, pmsAuthentication);

                                 if (folioDetails.Count > 0)
                                 {
                                     await _hcsFolioDetailService.BulkInsertWithProcAsync(folioDetails);
                                 }
                             }
                         }
                         else
                         {
                             continue;
                         }
                     }

                     return PrometheusResponse.Success("", "Data saved successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }

        private async Task<List<FolioDetailDto>> GetFolioDetailsAsync(string propertyCode, string pmsNumber, Authentication pmsAuthentication)
        {
            var folioDetailsResponse = await client.HcsGetFolioDetailsAsync(pmsAuthentication, propertyCode, "", pmsNumber, "");

            List<FolioDetailDto> listOfFolioDetail = new List<FolioDetailDto>();

            if (folioDetailsResponse.HcsGetFolioDetailsResult.Success)
            {
                foreach(var folioDetail in folioDetailsResponse.HcsGetFolioDetailsResult.Folios)
                {
                    listOfFolioDetail.Add(new FolioDetailDto
                    {
                        PropertyCode = propertyCode,
                        Balance = folioDetail.Balance,
                        BillTo = folioDetail.BillTo,
                        CardOnFile = folioDetail.CardOnFile,
                        FolioID = folioDetail.FolioID,
                        InvoiceCity = folioDetail.InvoiceCity,
                        InvoiceCountry = folioDetail.InvoiceCountry,
                        InvoiceCountryIso2Code = folioDetail.InvoiceCountryIso2Code,
                        InvoiceName1 = folioDetail.InvoiceName1,
                        InvoiceName2 = folioDetail.InvoiceName2,
                        InvoiceNumber1 = folioDetail.InvoiceNumber1,
                        InvoiceNumber2 = folioDetail.InvoiceNumber2,
                        InvoiceState = folioDetail.InvoiceState,
                        InvoiceStreet1 = folioDetail.InvoiceStreet1,
                        InvoiceStreet2 = folioDetail.InvoiceStreet2,
                        InvoiceZipCode = folioDetail.InvoiceZipCode,
                        PaymentType = folioDetail.PaymentType,
                        ReadyForAR = folioDetail.ReadyForAR,
                        Reference = folioDetail.Reference,
                        RelationEmail = folioDetail.RelationEmail,
                        RelationNumber = folioDetail.RelationNumber
                    });
                }
            }

            return listOfFolioDetail;
        }
    }
}
