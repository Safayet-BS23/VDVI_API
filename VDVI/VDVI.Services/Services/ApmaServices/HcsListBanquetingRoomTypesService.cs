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
using VDVI.Repository.DB;
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
            var result = new Result<PrometheusResponse>();

            //return await TryCatchExtension.ExecuteAndHandleErrorAsync(
            //  async () =>
            //  {
            //      Authentication pmsAuthentication = GetApmaAuthCredential();
            //      HcsListBanquetingRoomTypesResponse res = new HcsListBanquetingRoomTypesResponse();
            //      List<BanquetingRoomTypesDto>  dto = new List<BanquetingRoomTypesDto>(); 

            //      for (int i = 0; i < ApmaProperties.Length; i++)
            //      {
            //          var propertyCode = ApmaProperties[i];
            //          res = await client.HcsListBanquetingRoomTypesAsync(pmsAuthentication, propertyCode, "", "");
            //          List<ListBanquetingRoomTypesItem> types = res.HcsListBanquetingRoomTypesResult.BanquetingRoomTypes.ToList();
            //          FormatSummaryObject( types, dto, propertyCode);
            //      }
            //      if (dto.Count > 0)
            //          //DB operation
            //          //var d = await _hcsListBanquetingRoomTypeService.BulkInsertWithProcAsync(dto);

            //      return PrometheusResponse.Success("", "Data retrieval is successful");
            //  },
            //  exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
            //  {
            //      DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
            //      RethrowException = false
            //  });
            return result;
        }

        private void FormatSummaryObject(List<ListBanquetingRoomTypesItem> types, List<BanquetingRoomTypesDto> BanquetingRoomTypesdto, string propertyCode)
        {
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
        }
         
    }
}
