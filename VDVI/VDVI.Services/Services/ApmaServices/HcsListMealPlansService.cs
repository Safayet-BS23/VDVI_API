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
    public class HcsListMealPlansService : ApmaBaseService, IHcsListMealPlansService
    {
        private readonly IHcsListMealsPlansService _hcsListMealsPlansService;

        public HcsListMealPlansService(IHcsListMealsPlansService hcsListMealsPlansService)
        {
            _hcsListMealsPlansService = hcsListMealsPlansService;
        }


        public async Task<Result<PrometheusResponse>> HcsListMealPlansAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListMealPlansResponse res = new HcsListMealPlansResponse();
                  List<MealPlansDto> MealPlansDto=new List<MealPlansDto>();
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListMealPlansAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListMealPlan> ListMealPlans=res.HcsListMealPlansResult.MealPlans.ToList();
                      FormatObjectSummary(ListMealPlans, MealPlansDto, propertyCode);
                  }
                  var jsonresult=res.SerializeToJson();

                  return PrometheusResponse.Success("", "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }

        private void FormatObjectSummary(List<ListMealPlan> ListMealPlans, List<MealPlansDto> MealPlansDto,string propertyCode)
        {
            foreach (var item in ListMealPlans)
            {
                MealPlansDto dtos = new MealPlansDto()
                {
                    PropertyCode = propertyCode,
                    Code = item.Code,
                    Description = item.Description
                };
                MealPlansDto.Add(dtos);
            }

        }
        
    }
}
