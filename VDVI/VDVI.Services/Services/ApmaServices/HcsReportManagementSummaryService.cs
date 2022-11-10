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
    public class HcsReportManagementSummaryService : ApmaBaseService, IHcsReportManagementSummaryService
    {

        private readonly IHcsRoomSummaryService _hcsroomSummaryService;
        private readonly IHcsLedgerBalanceService _hcsLedgerBalanceService;

        public HcsReportManagementSummaryService(
            IHcsRoomSummaryService roomSummaryService,
            IHcsLedgerBalanceService hcsLedgerBalanceService)
        {
            _hcsroomSummaryService = roomSummaryService;
            _hcsLedgerBalanceService = hcsLedgerBalanceService;
        }

        public async Task<Result<PrometheusResponse>> ReportManagementSummaryAsync(DateTime StartDate, DateTime EndDate)
        { 
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    Authentication pmsAuthentication = GetApmaAuthCredential();

                    List<ReservationDashboardRoomSummaryHistoryDto> roomSummaries = new List<ReservationDashboardRoomSummaryHistoryDto>();
                    List<LedgerBalanceHistoryDto> ledgerBalances = new List<LedgerBalanceHistoryDto>();

                    for (int i = 0; i < ApmaProperties.Length; i++)
                    {
                        var propertyCode = ApmaProperties[i];
                        var res = await client.HcsReportManagementSummaryAsync(pmsAuthentication, PropertyCode: propertyCode, StartDate: StartDate, EndDate: EndDate, "");

                        List<ManagementSummary> managementSummary = res.HcsReportManagementSummaryResult.ManagementSummaries.ToList();

                       if(managementSummary.Count>0)
                            FormatSummaryObject(roomSummaries, ledgerBalances, managementSummary, propertyCode);
                    }


                    if (roomSummaries.Count > 0) 
                    { 
                        await _hcsroomSummaryService.BulkInsertWithProcAsync(roomSummaries);
                        roomSummaries.Clear();
                    }
                    if (ledgerBalances.Count > 0)
                    {
                        await _hcsLedgerBalanceService.BulkInsertWithProcAsync(ledgerBalances);
                        ledgerBalances.Clear();
                    } 

                    return PrometheusResponse.Success("", "Data retrieval is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        private void FormatSummaryObject(List<ReservationDashboardRoomSummaryHistoryDto> roomSummaries, List<LedgerBalanceHistoryDto> ledgerBalances, List<ManagementSummary> managementSummary, string propertyCode)
        {
            List<ReservationDashboardRoomSummaryHistoryDto> rooms = managementSummary.Select(x => new ReservationDashboardRoomSummaryHistoryDto()
            {
                InHouse = x.RoomSummary.InHouse,
                DayUse = x.RoomSummary.DayUse,
                LateArrival = x.RoomSummary.LateArrival,
                EarlyDeparture = x.RoomSummary.EarlyDeparture,
                Departed = x.RoomSummary.Departed,
                ToDepart = x.RoomSummary.ToDepart,
                StayOver = x.RoomSummary.StayOver,
                EarlyArrival = x.RoomSummary.EarlyArrival,
                Arrived = x.RoomSummary.Arrived,
                ToArrive = x.RoomSummary.ToArrive,
                NoShow = x.RoomSummary.NoShow,
                Complementary = x.RoomSummary.Complementary,
                WalkIns = x.RoomSummary.WalkIns,
                RoomReservationCreated = x.RoomSummary.RoomReservationCreated,
                RoomReservationCancelled = x.RoomSummary.RoomReservationCancelled,
                BusinessDate = x.BusinessDate,
                PropertyCode = propertyCode,
            }).ToList();
            roomSummaries.AddRange(rooms);


            List<LedgerBalanceHistoryDto> ledgerList = managementSummary.Select(x => new LedgerBalanceHistoryDto()
            {
                BusinessDate = x.BusinessDate,
                PropertyCode = propertyCode,
                Reservations = x.LedgerBalance.Reservations,
                InHouseReservations = x.LedgerBalance.InHouseReservations,
                GroupReservations = x.LedgerBalance.GroupReservations,
                EventReservations = x.LedgerBalance.EventReservations,
                TotalTurnover = x.LedgerBalance.TotalTurnover,
                LodgingTurnover = x.LedgerBalance.LodgingTurnover,
                PaymentsDebitor = x.LedgerBalance.PaymentsDebitor,
                PaymentsCash = x.LedgerBalance.PaymentsCash,
                CityLedger = x.LedgerBalance.CityLedger

            }).ToList();

            ledgerBalances.AddRange(ledgerList);
        }
    }
}
