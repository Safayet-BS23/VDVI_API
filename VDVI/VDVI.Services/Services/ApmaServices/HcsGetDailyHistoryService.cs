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
    public class HcsGetDailyHistoryService : ApmaBaseService, IHcsGetDailyHistoryService
    {
        private readonly IHcsDailyHistoryService _hcsDailyHistoryService;

        public HcsGetDailyHistoryService(IHcsDailyHistoryService hcsDailyHistoryService)
        {
            _hcsDailyHistoryService = hcsDailyHistoryService;
        }
        public async Task<Result<PrometheusResponse>> HcsGetDailyHistoryAsyc(DateTime StartDate, DateTime EndDate)
        {            
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     var response = await GetListHcsDailyHistoryAsync(StartDate, EndDate);

                     if (response.IsSuccess)
                     {
                         return await _hcsDailyHistoryService.BulkInsertWithProcAsync((List<DailyHistoryDto>)response.Value.Data);
                     }

                     return PrometheusResponse.Failure(response.Value.Message);
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }

        public async Task<Result<PrometheusResponse>> GetListHcsDailyHistoryAsync(DateTime StartDate, DateTime EndDate, string propertyCode)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     Authentication pmsAuthentication = GetApmaAuthCredential();
                     List<DailyHistoryDto> listOfDailyHistory = new List<DailyHistoryDto>();

                     HcsGetDailyHistoryResponse res = new HcsGetDailyHistoryResponse();

                     res = await client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "", 100, 1, "");

                     for (int j = 1; j <= res.HcsGetDailyHistoryResult.TotalPages; j++)
                     {
                         List<DailyHistoryDto> dto = new List<DailyHistoryDto>();

                         var dailyHistoryList = client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "", 100, j, "").Result.HcsGetDailyHistoryResult.DailyHistories.ToList();
                         if (dailyHistoryList.Count > 0) 
                             FormatSummaryObject(dto, dailyHistoryList, propertyCode);

                         listOfDailyHistory.AddRange(dto);
                         dto.Clear();
                     }

                     return PrometheusResponse.Success(listOfDailyHistory, "Data retrieval is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }

        private async Task<Result<PrometheusResponse>> GetListHcsDailyHistoryAsync(DateTime StartDate, DateTime EndDate)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     Authentication pmsAuthentication = GetApmaAuthCredential();
                     List<DailyHistoryDto> listOfDailyHistory = new List<DailyHistoryDto>();

                     HcsGetDailyHistoryResponse res = new HcsGetDailyHistoryResponse();

                     for (int i = 0; i < ApmaProperties.Length; i++)
                     {
                         List<DailyHistoryDto> dto = new List<DailyHistoryDto>();

                         var propertyCode = ApmaProperties[i];
                         res = await client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "", 100, 1, "");

                         for (int j = 1; j <= res.HcsGetDailyHistoryResult.TotalPages; j++)
                         {
                             var dailyHistoryList = client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "", 100, j, "").Result.HcsGetDailyHistoryResult.DailyHistories.ToList();
                             if (dailyHistoryList.Count > 0) FormatSummaryObject(dto, dailyHistoryList, propertyCode);
                         }

                         listOfDailyHistory.AddRange(dto);
                         dto.Clear();
                     }
                     return PrometheusResponse.Success(listOfDailyHistory, "Data retrieval is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }

        private void FormatSummaryObject(List<DailyHistoryDto> sourceStatDtos, List<DailyHistory> dailyHistoryList, string propertyCode)
        {
            if (dailyHistoryList.Count > 0)
            {
                List<DailyHistoryDto> sourceStatz = dailyHistoryList.Select(x => new DailyHistoryDto()
                {
                    PropertyCode = propertyCode,
                    Date = x.Date,
                    PmsSegmentNumber = x.PmsSegmentNumber,
                    PmsSegmentType = x.PmsSegmentType,
                    RoomType = x.RoomType,
                    Source = x.Source,
                    SubSource = x.SubSource,
                    RateType = x.RateType,
                    Mealplan = x.Mealplan,
                    Package = x.Package,
                    CountryIso2Code = x.CountryIso2Code,
                    PaymentDebitor = x.PaymentDebitor,
                    PaymentNonDebitor = x.PaymentNonDebitor,
                    Adults = x.Adults,
                    Children = x.Children,
                    Infants = x.Infants,
                    IsDayuse = x.IsDayuse,

                    RevenueInclusiveA = x.RevenuesInclusive.RevenueA,
                    RevenueInclusiveB = x.RevenuesInclusive.RevenueB,
                    RevenueInclusiveC = x.RevenuesInclusive.RevenueC,
                    RevenueInclusiveD = x.RevenuesInclusive.RevenueD,
                    RevenueInclusiveE = x.RevenuesInclusive.RevenueE,
                    RevenueInclusiveF = x.RevenuesInclusive.RevenueF,
                    RevenueInclusiveUndefined = x.RevenuesInclusive.RevenueUndefined,

                    RevenueExclusiveA = x.RevenuesExclusive.RevenueA,
                    RevenueExclusiveB = x.RevenuesExclusive.RevenueB,
                    RevenueExclusiveC = x.RevenuesExclusive.RevenueC,
                    RevenueExclusiveD = x.RevenuesExclusive.RevenueD,
                    RevenueExclusiveE = x.RevenuesExclusive.RevenueE,
                    RevenueExclusiveF = x.RevenuesExclusive.RevenueF,
                    RevenueExclusiveUndefined = x.RevenuesExclusive.RevenueUndefined

                }).ToList();
                sourceStatDtos.AddRange(sourceStatz);
            }

        }

    }
}
