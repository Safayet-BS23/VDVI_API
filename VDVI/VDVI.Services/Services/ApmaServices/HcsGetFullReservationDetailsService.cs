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
    public class HcsGetFullReservationDetailsService : ApmaBaseService, IHcsGetFullReservationDetailsService
    {

        private readonly IHcsGetFullReservationDetailService _hcsGetFullReservationDetailService;
        private readonly IHcsGetDailyHistoryHistoryService _hcsGetDailyHistoryService;
        List<GetFullReservationDetailsDto> ReservationDetailsdto = new List<GetFullReservationDetailsDto>();
        public HcsGetFullReservationDetailsService(
            IHcsGetFullReservationDetailService hcsGetFullReservationDetailService, IHcsGetDailyHistoryHistoryService hcsGetDailyHistoryService)
        {
            _hcsGetFullReservationDetailService = hcsGetFullReservationDetailService;
            _hcsGetDailyHistoryService = hcsGetDailyHistoryService;
        }

        public async Task<Result<PrometheusResponse>> HcsGetFullReservationDetailsAsync(DateTime BusinessStartDate)
        {

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    Authentication pmsAuthentication = GetApmaAuthCredential();

                    int currentYear = DateTime.UtcNow.Year;
                    DateTime currentYearStartDate = new DateTime(currentYear, 01, 01);
                    int index = 1;


                    GetFullReservationDetailsDto dto = new GetFullReservationDetailsDto();
                    //while (BusinessStartDate < currentYearStartDate)
                    //{
                    //    foreach (string property in ApmaProperties)
                    //    {
                    //        var dailyHistoryListResponse = await _hcsGetDailyHistoryService.GetListHcsDailyHistoryAsync(BusinessStartDate, BusinessStartDate.AddDays(6), property);

                    //        if (dailyHistoryListResponse.IsSuccess)
                    //        {
                    //            var uniquePMSNumberList = ((List<DailyHistoryDto>)dailyHistoryListResponse.Value.Data).Select(x => x.PmsSegmentNumber).Distinct();
                    //            foreach (var uniquePMS in uniquePMSNumberList)
                    //            {
                    //                //var list = await GetFullReservationDetailsAsync(property, uniquePMS, pmsAuthentication);

                    //                //if(list.Count>0)
                    //                //{
                    //                //    //Save into DB
                    //                //    var result = _hcsGetFullReservationDetailService.BulkInsertWithProcAsync(list);
                    //                //} 

                    //            }
                    //        }
                           
                    //    }
                    //    BusinessStartDate = BusinessStartDate.AddDays(7);
                    //    index++;
                    //}
                    return PrometheusResponse.Success("", "Data retrived Successfuly");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }



        private async Task<List<GetFullReservationDetailsDto>> GetFullReservationDetailsAsync(string propertyCode, string pmsNumber, Authentication pmsAuthentication)
        {
          
            //var  res = await client.HcsGetFullReservationDetailsAsync(pmsAuthentication, PropertyCode: propertyCode, "NUL-FC193726", pmsNumber, "", "", "");
            List<GetFullReservationDetailsDto> listOfFullReservationDetail = new List<GetFullReservationDetailsDto>();


            //if(res.HcsGetFullReservationDetailsResult.Success)
            //{
            //    foreach (var item in res.HcsGetFullReservationDetailsResult.Reservation.RoomStays.)
            //    {
                    
            //    }
            //}

           return listOfFullReservationDetail;
        }
    }
}
