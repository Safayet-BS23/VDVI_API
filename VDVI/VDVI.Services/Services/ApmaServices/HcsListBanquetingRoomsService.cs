using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Extensions;
using Framework.Core.Utility;
using SOAPService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsListBanquetingRoomsService : ApmaBaseService, IHcsListBanquetingRoomsService
    {
        private readonly IHcsBanquetingRoomService _hcsBanquetingRoomService;

        public HcsListBanquetingRoomsService(IHcsBanquetingRoomService hcsBanquetingRoomService)
        {
            _hcsBanquetingRoomService = hcsBanquetingRoomService;
        }  

        public async Task<Result<PrometheusResponse>> HcsListBanquetingRoomsAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListBanquetingRoomsResponse res = new HcsListBanquetingRoomsResponse();

                  List<BanquetingRoomsDto> banquetingRoomsDtos = new List<BanquetingRoomsDto>(); 

                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListBanquetingRoomsAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListBanquetingRoom> BanquetingRooms = res.HcsListBanquetingRoomsResult.BanquetingRooms.ToList(); 
                      FormatSummaryObject(BanquetingRooms, banquetingRoomsDtos,  propertyCode);
                  }

                  //DB operation
                  await _hcsBanquetingRoomService.BulkInsertWithProcAsync(banquetingRoomsDtos);
                  var jsonresult = res.SerializeToJson();

                  return PrometheusResponse.Success("", "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryObject(List<ListBanquetingRoom> BanquetingRooms, List<BanquetingRoomsDto> banquetingRoomsDtos, string propertyCode)
        {
            foreach(var room in BanquetingRooms)
            {
                var dto = new BanquetingRoomsDto()
                {
                    PropertyCode = propertyCode,
                    Code = room.Code,
                    Description = room.Description,
                    BanquetingRoomType = room.BanquetingRoomType,
                    IsCombination = room.IsCombination,
                    Combination= string.Join(",",room.Combination),
                    ListOrder = room.ListOrder
                };
                banquetingRoomsDtos.Add(dto);

                 
            }
        }
    }
}
