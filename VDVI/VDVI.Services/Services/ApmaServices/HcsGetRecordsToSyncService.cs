using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using SOAPService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsGetRecordsToSyncService : ApmaBaseService, IHcsGetRecordsToSyncService
    {
        private readonly IHcsListGroupReservationService _hcsGroupReservationService;
        private readonly IRecordsToSyncChangedService _recordsToSyncChangedService;

        public HcsGetRecordsToSyncService
            (
                IHcsListGroupReservationService hcsGroupReservationService,
                IRecordsToSyncChangedService recordsToSyncChangedService
            )
        {
            _hcsGroupReservationService = hcsGroupReservationService;
            _recordsToSyncChangedService = recordsToSyncChangedService;
        }

        public async Task<Result<PrometheusResponse>> SyncHcsGroupReservationAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
             async () =>
             {
                 var response = await HcsGetRecordsToSync("Group");

                 if (response.IsSuccess)
                 {
                     var recordsToSync = (List<RecordsToSyncChangedDto>)response.Value.Data;

                     var result = await _recordsToSyncChangedService.BulkInsertWithProcAsync(recordsToSync);

                     if (result.IsSuccess)
                     {
                         result = await _hcsGroupReservationService.HcsSyncGroupReservationAsync(recordsToSync);
                     }

                     return result;
                 }

                 return response;
             },
             exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
             {
                 DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                 RethrowException = false
             });
        }

        private async Task<Result<PrometheusResponse>> HcsGetRecordsToSync(string type)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();

                  List<RecordsToSyncChangedDto> recordsChanged = new List<RecordsToSyncChangedDto>(); 

                  foreach (string property in ApmaProperties)
                  {
                      var isComplete = false;

                      do
                      {
                          var response = await client.HcsGetRecordsToSyncAsync(pmsAuthentication, property, type, "", "");

                          if (!response.HcsGetRecordsToSyncResult.Success)
                          {
                              continue;
                          }

                          recordsChanged.AddRange(FormatResult(response.HcsGetRecordsToSyncResult.ChangedRecords.ToList(), property));

                          isComplete = response.HcsGetRecordsToSyncResult.ProcessCompleted;

                      } while (!isComplete);                
                  }

                  return PrometheusResponse.Success(recordsChanged, "Data Sync Successfully.");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }

        private List<RecordsToSyncChangedDto> FormatResult(List<GetRecordsToSyncChangedRecord> records, string propertyCode)
        {
            return records.Select(m => new RecordsToSyncChangedDto
            {
                PropertyCode = propertyCode,
                Type = m.Type,
                Reference = m.Reference,
                NewReference = m.NewReference,
                SubReference = m.SubReference,
                Status = m.Status
            }).ToList();
        }
    }
}
