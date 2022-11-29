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
    public class HcsGetDailyHistoryFutureService : ApmaBaseService, IHcsGetDailyFutureService
    {
        private readonly IHcsDailyFutureService _hcsDailyFutureService;

        public HcsGetDailyHistoryFutureService(IHcsDailyFutureService hcsDailyFutureService)
        {
            _hcsDailyFutureService = hcsDailyFutureService;
        }
        public async Task<Result<PrometheusResponse>> HcsGetDailyHistoryFutureAsyc(DateTime lastExecutionDate, int dayDifference)
        {
            DateTime nextExecutionDate = lastExecutionDate.AddMonths(12).AddSeconds(1);
            DateTime tempDate = lastExecutionDate;

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     Authentication pmsAuthentication = GetApmaAuthCredential();

                     List<DailyHistoryFutureDto> dto = new List<DailyHistoryFutureDto>();
                     HcsGetDailyHistoryResponse res = new HcsGetDailyHistoryResponse();
                     while (tempDate < nextExecutionDate)
                     {
                         var endDate = tempDate.AddDays(dayDifference);
                         endDate = endDate > nextExecutionDate ? nextExecutionDate : endDate;

                         var response = await GetListHcsDailyHistoryFutureAsync(tempDate, endDate);

                         if (response.IsSuccess)
                         {
                             return await _hcsDailyFutureService.BulkInsertWithProcAsync((List<DailyHistoryFutureDto>)response.Value.Data);
                         }

                         tempDate = tempDate.AddDays(dayDifference).AddSeconds(1);
                     }

                     return PrometheusResponse.Success("", "Data retrieval is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }


        public async Task<Result<PrometheusResponse>> GetListHcsDailyHistoryFutureAsync(DateTime StartDate, DateTime EndDate, string propertyCode, string pmsSegmentType)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     Authentication pmsAuthentication = GetApmaAuthCredential();
                     List<DailyHistoryFutureDto> listOfDailyHistory = new List<DailyHistoryFutureDto>();

                     HcsGetDailyHistoryResponse res = new HcsGetDailyHistoryResponse();

                     List<DailyHistoryFutureDto> dto = new List<DailyHistoryFutureDto>();

                     res = await client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "", 100, 1, "");

                     for (int j = 1; j <= res.HcsGetDailyHistoryResult.TotalPages; j++)
                     {
                         var dailyHistoryList = client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "", 100, j, "").Result.HcsGetDailyHistoryResult.DailyHistories.ToList();
                         
                         if (!string.IsNullOrEmpty(pmsSegmentType))
                             dailyHistoryList = dailyHistoryList.Where(x => x.PmsSegmentType == pmsSegmentType).ToList();

                         if (dailyHistoryList.Count > 0) FormatSummaryObject(dto, dailyHistoryList, propertyCode);
                     }

                     listOfDailyHistory.AddRange(dto);
                     dto.Clear();


                     return PrometheusResponse.Success(listOfDailyHistory, "Data retrieval is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }

        private async Task<Result<PrometheusResponse>> GetListHcsDailyHistoryFutureAsync(DateTime StartDate, DateTime EndDate)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     Authentication pmsAuthentication = GetApmaAuthCredential();
                     List<DailyHistoryFutureDto> listOfDailyHistory = new List<DailyHistoryFutureDto>();

                     HcsGetDailyHistoryResponse res = new HcsGetDailyHistoryResponse();

                     for (int i = 0; i < ApmaProperties.Length; i++)
                     {
                         List<DailyHistoryFutureDto> dto = new List<DailyHistoryFutureDto>();

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


        private void FormatSummaryObject(List<DailyHistoryFutureDto> sourceStatDtos, List<DailyHistory> dailyHistoryList, string propertyCode)
        {
            List<DailyHistoryFutureDto> sourceStatz = dailyHistoryList.Select(x => new DailyHistoryFutureDto()
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
