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
    public class HcsListSourcesService : ApmaBaseService, IHcsListSourcesService
    {
        private readonly IHcsListSourceService _hcsListSourceService;

        public HcsListSourcesService(IHcsListSourceService hcsListSourceService)
        {
            _hcsListSourceService = hcsListSourceService;
        }


        public async Task<Result<PrometheusResponse>> HcsListSourcesServiceAsync()
        {
            var result = new Result<PrometheusResponse>(); 
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListSourcesResponse res = new HcsListSourcesResponse();
                  List<SourcesDto> sourcesdto = new List<SourcesDto>();    
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListSourcesAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListSourcesItem> sourcelist=res.HcsListSourcesResult.Sources.ToList();
                      FormatSummaryObject(sourcelist, sourcesdto,propertyCode);
                  }
                  if(sourcesdto.Count>0)
                      //DB operation
                      result=await _hcsListSourceService.BulkInsertWithProcAsync(sourcesdto);

                  return PrometheusResponse.Success(result, "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryObject(List<ListSourcesItem> sourcelist, List<SourcesDto>  sourcedto,string propertyCode)
        {
            foreach (var item in sourcelist)
            {
                var dto = new SourcesDto()
                {
                    PropertyCode = propertyCode,
                    Code = item.Code,
                    Description = item.Description,
                    Listorder = item.Listorder,
                    SourceGroup = item.SourceGroup,
                };
                sourcedto.Add(dto);
            }
        }
    }
}
