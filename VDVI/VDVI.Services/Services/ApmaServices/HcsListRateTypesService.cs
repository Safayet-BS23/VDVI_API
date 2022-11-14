using CSharpFunctionalExtensions;
using Framework.Core.ApmaExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Extensions;
using Framework.Core.Utility;
using SOAPService;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces; 

namespace VDVI.Services
{
    public class HcsListRateTypesService : ApmaBaseService, IHcsListRateTypesService
    {
        private readonly IHcsListRateTypeService _hcsListRateTypeService;

        public HcsListRateTypesService(IHcsListRateTypeService hcsListRateTypeService)
        {
            _hcsListRateTypeService = hcsListRateTypeService;
        }


        public async Task<Result<PrometheusResponse>> HcsListRateTypeAsync()
        {
            var result = new Result<PrometheusResponse>();

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListRateTypesResponse res = new HcsListRateTypesResponse();
                  List<RateTypeDto> ratetypedto = new List<RateTypeDto>();

                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListRateTypesAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListRateType> listRateType = res.HcsListRateTypesResult.RateTypes.ToList();
                      FormatSummaryObject(listRateType, ratetypedto, propertyCode); 
                  } 
                  if(ratetypedto.Count>0)
                      //Db operation
                      result=await _hcsListRateTypeService.BulkInsertWithProcAsync(ratetypedto); 

                  return PrometheusResponse.Success(result, "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryObject(List<ListRateType> listRateType, List<RateTypeDto> ratetypedto, string propertyCode)
        {

            foreach (var item in listRateType)
            {
                RateTypeDto dto = new RateTypeDto()
                {
                    PropertyCode = propertyCode,
                    Code = item.Code,
                    Description = item.Description,                    
                    ChargeType = item.ChargeType,
                    ChargeplanCode = item.ChargeplanCode,
                    Article = item.Article,
                    IsVisible = item.IsVisible,
                    IncludesBreakfast1 = item.IncludesBreakfast1,
                    IncludesBreakfast2 = item.IncludesBreakfast2,
                    IncludesPackedLunch1 = item.IncludesPackedLunch1,
                    IncludesPackedLunch2 = item.IncludesPackedLunch2,
                    IncludesLunch1 = item.IncludesLunch1,
                    IncludesLunch2 = item.IncludesLunch2,
                    IncludesDinner1 = item.IncludesDinner1,
                    IncludesDinner2 = item.IncludesDinner2,
                    IsManualRate = item.IsManualRate,
                    IsCrsManaged = item.IsCrsManaged,
                    IsComplimentary = item.IsComplimentary,
                    IgnoreRoomsToSell = item.IgnoreRoomsToSell,
                    ListOrder = item.ListOrder,
                    NotInRateQuery = item.NotInRateQuery,
                    CrsConnector = item.CrsConnector,
                    DistributionMealplan = item.DistributionMealplan,
                    SelectableInGroupBlocks = item.SelectableInGroupBlocks,
                    IsPrepaid = item.IsPrepaid,
                    DefaultSource = item.DefaultSource,
                    DefaultSubSource =item.DefaultSubSource,
                    DerivedFrom =item.DerivedFrom,
                    DerivedType = item.DerivedType,
                    DerivedValue = item.DerivedValue,
                    RateRoundingType =item.RateRoundingType,
                    RateTypeGroup =item.RateTypeGroup,
                    IsPublished = item.IsPublished,
                    IsCwiPackage = item.IsCwiPackage,
                    Allotment = item.Allotment,
                    RatePlanCategory =item.RatePlanCategory
                };
                ratetypedto.Add(dto);
            }
        }
    }
}
