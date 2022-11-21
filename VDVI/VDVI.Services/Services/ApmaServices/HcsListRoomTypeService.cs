using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Extensions;
using Framework.Core.Utility;
using SOAPService;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsListRoomTypeService : ApmaBaseService, IHcsListRoomTypeService
    {
        private readonly IHcsListRoomTypesService _hcsListRoomTypesService;

        public HcsListRoomTypeService(IHcsListRoomTypesService hcsListRoomTypesService)
        {
            _hcsListRoomTypesService = hcsListRoomTypesService;
        }

        public async Task<Result<PrometheusResponse>> HcsListRoomTypeAsync()
        {
            var result = new Result<PrometheusResponse>();
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListRoomTypesResponse res = new HcsListRoomTypesResponse();
                  var dto = new List<RoomTypesDto>();
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListRoomTypesAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListRoomType> Types = res.HcsListRoomTypesResult.RoomTypes.ToList();
                      FormatSummaryobject(dto, Types, propertyCode);
                  }
                  if (dto.Count > 0)
                      result = await _hcsListRoomTypesService.BulkInsertWithProcAsync(dto);

                  return PrometheusResponse.Success("", "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryobject(List<RoomTypesDto> roomTypesDto, List<ListRoomType> Types, string propertyCode)
        {
            foreach (var item in Types)
            {
                RoomTypesDto dto = new RoomTypesDto()
                {
                    PropertyCode = propertyCode,
                    RoomType = item.RoomType,
                    //Description = string.Join(",", item.Description), 
                    //InfoText = string.Join(",", item.InfoText), 
                    ListOrder = item.ListOrder,
                    Inventory = item.Inventory,
                    BedsInRoom = item.BedsInRoom,
                    MaxOccupancy = item.MaxOccupancy,
                    Adults = item.Adults,
                    ChildOld = item.ChildOld,
                    ChildYoung = item.ChildYoung,
                    Infants = item.Infants,
                    Baby = item.Baby,
                    RoomTypeGroup = item.RoomTypeGroup,
                    UpsellRoomGroup = item.UpsellRoomGroup,
                    Publish = item.Publish,
                    Url = item.Url 
                };
                roomTypesDto.Add(dto);
            }
        }
    }
}
