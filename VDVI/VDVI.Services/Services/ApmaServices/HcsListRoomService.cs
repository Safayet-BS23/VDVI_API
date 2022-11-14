using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Extensions;
using Framework.Core.Utility;
using SOAPService;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsListRoomService : ApmaBaseService, IHcsListRoomService
    {
        private readonly IHcsRoomsService _hcsRoomsService;

        public HcsListRoomService(IHcsRoomsService hcsRoomsService)
        {
            _hcsRoomsService = hcsRoomsService;
        }
         
        public async Task<Result<PrometheusResponse>> HcsListRoomsServiceAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListRoomsResponse res = new HcsListRoomsResponse();
                  List<RoomsDto> roomsDtos= new List<RoomsDto>();
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListRoomsAsync(pmsAuthentication, propertyCode, "");
                      List<ListRoomRoom> rooms=res.HcsListRoomsResult.Rooms.ToList();
                  }
                  await _hcsRoomsService.BulkInsertWithProcAsync(roomsDtos);
                  var jsonresult=res.SerializeToJson();

                  return PrometheusResponse.Success("", "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryobject(List<RoomsDto> roomsDtos, List<ListRoomRoom> rooms,string propertyCode)
        {
            foreach (var item in rooms)
            {
                RoomsDto dto = new RoomsDto()
                {
                    PropertyCode = propertyCode,
                    ConnectingLeft = item.ConnectingLeft,
                    ConnectingRight = item.ConnectingRight,
                    ExtrasNotAllowed = item.ExtrasNotAllowed,
                    HousekeepingSections = item.HousekeepingSections,
                    RoomFeatures = string.Join(",", item.RoomFeatures),
                    RoomType = item.RoomType,
                    RoomNumber = item.RoomNumber
                };
                roomsDtos.Add(dto);
            }
        }
    }
}
