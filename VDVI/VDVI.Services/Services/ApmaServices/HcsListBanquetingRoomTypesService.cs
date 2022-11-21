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
    public class HcsListBanquetingRoomTypesService : ApmaBaseService, IHcsListBanquetingRoomTypesService
    {
        private readonly IHcsListBanquetingRoomTypeService _hcsListBanquetingRoomTypeService;

        public HcsListBanquetingRoomTypesService(IHcsListBanquetingRoomTypeService hcsListBanquetingRoomTypeService)
        {
            _hcsListBanquetingRoomTypeService = hcsListBanquetingRoomTypeService;
        }

        public async Task<Result<PrometheusResponse>> HcsListBanquetingRoomTypesAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListBanquetingRoomTypesResponse res = new HcsListBanquetingRoomTypesResponse();
                  List<BanquetingRoomTypesDto> dto = new List<BanquetingRoomTypesDto>();

                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListBanquetingRoomTypesAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListBanquetingRoomTypesItem> types = res.HcsListBanquetingRoomTypesResult.BanquetingRoomTypes.ToList();
                      dto = FormatSummaryObject(types, propertyCode);
                  }
                  if (dto.Count > 0)
                  {
                      //DB operation
                      var result = await _hcsListBanquetingRoomTypeService.BulkInsertWithProcAsync(dto);
                  }        
                      
                  return PrometheusResponse.Success("", "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }

        private List<BanquetingRoomTypesDto> FormatSummaryObject(List<ListBanquetingRoomTypesItem> types, string propertyCode)
        {
            List<BanquetingRoomTypesDto> BanquetingRoomTypesdto = new List<BanquetingRoomTypesDto>();

            foreach (var item in BanquetingRoomTypesdto)
            {
                var dto = new BanquetingRoomTypesDto()
                {
                    PropertyCode = propertyCode,
                    Description = item.Description,
                    Code=item.Code
                };
                BanquetingRoomTypesdto.Add(dto);
            }

            return BanquetingRoomTypesdto;
        }
         
    }
}
