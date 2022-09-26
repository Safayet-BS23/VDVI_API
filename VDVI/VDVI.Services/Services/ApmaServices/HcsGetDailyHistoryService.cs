﻿using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using SOAPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
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
                     Authentication pmsAuthentication = GetApmaAuthCredential();

                     List<DailyHistoryDto> dto = new List<DailyHistoryDto>();

                     foreach (string property in ApmaProperties)
                     {
                         var res = await client.HcsGetDailyHistoryAsync(pmsAuthentication, PropertyCode: property, StartDate: StartDate, EndDate: EndDate, "", 10, 0, "");

                         var dailyHistoryList = res.HcsGetDailyHistoryResult.DailyHistories.ToList();

                         FormatSummaryObject(dto, dailyHistoryList, property);
                     }


                     var result = _hcsDailyHistoryService.BulkInsertWithProcAsync(dto);

                     return PrometheusResponse.Success("", "Data retrieval is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                     RethrowException = false
                 });
        }
        private void FormatSummaryObject(List<DailyHistoryDto> sourceStatDtos, List<DailyHistory> dailyHistoryList, string propertyCode)
        {
            List<DailyHistoryDto> sourceStatz = dailyHistoryList.Select(x => new DailyHistoryDto()
            {
                PropertyCode = propertyCode,
                Date = x.Date,
                PmsSegmentNumber = x.PmsSegmentNumber,
                PmsSegmentType = x.PmsSegmentType,
                RoomType = x.RoomType,
                Source=x.Source,
                SubSource=x.SubSource,
                RateType=x.RateType,
                Mealplan=x.Mealplan,
                Package=x.Package,
                CountryIso2Code=x.CountryIso2Code,
                PaymentDebitor=x.PaymentDebitor,
                PaymentNonDebitor=x.PaymentNonDebitor,
                Adults=x.Adults,
                Children=x.Children,
                Infants=x.Infants,
                IsDayuse=x.IsDayuse,

                RevenueInclusiveA = x.RevenuesInclusive.RevenueA,
                RevenueInclusiveB = x.RevenuesInclusive.RevenueB,
                RevenueInclusiveC = x.RevenuesInclusive.RevenueC,
                RevenueInclusiveD = x.RevenuesInclusive.RevenueD,
                RevenueInclusiveE = x.RevenuesInclusive.RevenueE,
                RevenueInclusiveF = x.RevenuesInclusive.RevenueF,
                RevenueInclusiveUndefined =x.RevenuesInclusive.RevenueUndefined,

                RevenueExclusiveA=x.RevenuesExclusive.RevenueA,
                RevenueExclusiveB=x.RevenuesExclusive.RevenueB,
                RevenueExclusiveC=x.RevenuesExclusive.RevenueC,
                RevenueExclusiveD=x.RevenuesExclusive.RevenueD,
                RevenueExclusiveE=x.RevenuesExclusive.RevenueE,
                RevenueExclusiveF=x.RevenuesExclusive.RevenueF,
                RevenueExclusiveUndefined=x.RevenuesExclusive.RevenueUndefined

            }).ToList();
            sourceStatDtos.AddRange(sourceStatz);
        }

    }
}
