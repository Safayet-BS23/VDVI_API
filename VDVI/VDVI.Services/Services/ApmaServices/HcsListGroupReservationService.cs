using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using SOAPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services.Services.ApmaServices
{
    public class HcsListGroupReservationService : ApmaBaseService, IHcsListGroupReservationService
    {
        private readonly IHcsGroupReservationService _hcsGroupReservationService;
        private readonly IHcsGetDailyFutureService _hcsGetDailyFutureService;

        public HcsListGroupReservationService
            (
                IHcsGroupReservationService hcsGroupReservationService,
                IHcsGetDailyFutureService hcsGetDailyFutureService
            )
        {
            _hcsGroupReservationService = hcsGroupReservationService;
            _hcsGetDailyFutureService = hcsGetDailyFutureService;
        }

        public async Task<Result<PrometheusResponse>> HcsGetGroupReservationsAsync(DateTime startDate, DateTime endDate)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    Authentication pmsAuthentication = GetApmaAuthCredential();

                    foreach (string property in ApmaProperties)
                    {
                        var dailyHistoryFutureUniquePmsNumbers = await GetUniquePmsSegmentNumbers(startDate, endDate, property);

                        if (dailyHistoryFutureUniquePmsNumbers.IsFailure)
                            continue;

                        var dailyHistoryFuturePMSNumberList = ((List<string>)dailyHistoryFutureUniquePmsNumbers.Value.Data);

                        List<GroupReservationDto> groupReservationDtoList = new List<GroupReservationDto>();

                        foreach(var pmsGroupReservationNumber in dailyHistoryFuturePMSNumberList)
                        {
                            var groupReservationResponse = await GetGroupReservationsAsync(startDate, endDate, property, pmsGroupReservationNumber);

                            if (groupReservationResponse.IsFailure)
                                continue;

                            groupReservationDtoList.Add((GroupReservationDto)groupReservationResponse.Value.Data);
                        }

                        if (groupReservationDtoList.Any())
                        {
                            await _hcsGroupReservationService.BulkInsertWithProcAsync(groupReservationDtoList);
                        }

                        groupReservationDtoList.Clear();
                    }
                    return PrometheusResponse.Success("", "Data saved successfully");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> HcsSyncGroupReservationAsync(List<RecordsToSyncChangedDto> changeRecords)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    Authentication pmsAuthentication = GetApmaAuthCredential();

                    if (changeRecords.Count > 0)
                    {
                        foreach(var changeRecord in changeRecords)
                        {
                            var response = await client.HcsGetGroupReservationAsync(pmsAuthentication, changeRecord.PropertyCode, changeRecord.Reference, "", "");

                            if (response.HcsGetGroupReservationResult.Success)
                            {
                                var result = await _hcsGroupReservationService.UpsertAsync(FormatGroupReservation(response.HcsGetGroupReservationResult, changeRecord.PropertyCode));
                            }
                        }

                        return PrometheusResponse.Success("", "Data saved successfully");
                    } else
                    {
                        return PrometheusResponse.Failure("Data is empty");
                    }
                                       
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        #region Private Methods 

        private async Task<Result<PrometheusResponse>> GetGroupReservationsAsync(DateTime startDate, DateTime endDate, string propertyCode, string pmsNumber)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    Authentication pmsAuthentication = GetApmaAuthCredential();

                    var response = await client.HcsGetGroupReservationAsync(pmsAuthentication, propertyCode, pmsNumber, "", "");

                    if (response.HcsGetGroupReservationResult != null)
                    {
                        return PrometheusResponse.Success(response.HcsGetGroupReservationResult, "Data retrieve successfully.");
                    }

                    return PrometheusResponse.Failure("Error Occured.");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        private async Task<Result<PrometheusResponse>> GetUniquePmsSegmentNumbers(DateTime startDate, DateTime endDate, string propertyCode)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    Authentication pmsAuthentication = GetApmaAuthCredential();

                    var dailyHistoryFutureList = await _hcsGetDailyFutureService.GetListHcsDailyHistoryFutureAsync(startDate, endDate, propertyCode, "GroupReservation");

                    var dailyHistoryFuturePMSNumberList = ((List<DailyHistoryFutureDto>)dailyHistoryFutureList.Value.Data).Select(x => x.PmsSegmentNumber).Distinct().ToList();

                    return PrometheusResponse.Success(dailyHistoryFuturePMSNumberList, "Data saved successfully");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        private GroupReservationDto FormatGroupReservation(GetGroupReservation groupReservation, string propertyCode)
        {
            return new GroupReservationDto
            {
                Balance = groupReservation.Balance,
                BlockStatus = groupReservation.BlockStatus,
                Booker = groupReservation.Booker,
                Channel = groupReservation.Channel,
                Company = groupReservation.Company,
                DepositAmount1 = groupReservation.DepositAmount1,
                DepositAmount2 = groupReservation.DepositAmount2,
                DepositDueDate1 = groupReservation.DepositDueDate1,
                DepositDueDate2 = groupReservation.DepositDueDate2,
                GroupEndDate = groupReservation.GroupEndDate,
                GroupStartDate = groupReservation.GroupStartDate,
                GroupName = groupReservation.GroupName,
                GroupReservationNumber = groupReservation.GroupReservationNumber,
                GuaranteeType = groupReservation.GuaranteeType,
                Market = groupReservation.Market,
                OptionDate = groupReservation.OptionDate,
                PaymentType = groupReservation.PaymentType,
                PropertyCode = propertyCode,
                ReleaseDate = groupReservation.ReleaseDate,
                Source = groupReservation.Source,
                Status = groupReservation.Status,
                StatusInfo = groupReservation.StatusInfo,
                TotalAdults = groupReservation.TotalAdults,
                TotalBabies = groupReservation.TotalBabies,
                TotalChildren = groupReservation.TotalChildren,
                TotalChildrenOld = groupReservation.TotalChildrenOld,
                TotalChildrenYoung = groupReservation.TotalChildrenYoung,
                TotalExcl = groupReservation.TotalExcl,
                TotalIncl = groupReservation.TotalIncl,
                TotalInfants = groupReservation.TotalInfants,
                TotalRoomsContracted = groupReservation.TotalRoomsContracted,
                TotalRoomsPickedUp = groupReservation.TotalRoomsPickedUp,
                TravelAgent = groupReservation.TravelAgent
            };
        }


        #endregion
    }
}
