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
    public class HcsListSubSourcesService : ApmaBaseService, IHcsListSubSourcesService
    {
        private readonly IHcsListSubSourceService _hcsListSubSourceService;

        public HcsListSubSourcesService(IHcsListSubSourceService hcsListSubSourceService)
        {
            _hcsListSubSourceService = hcsListSubSourceService;
        }


        public async Task<Result<PrometheusResponse>> HcsListSubSourcesServiceAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListSubSourcesResponse res = new HcsListSubSourcesResponse();
                  List<SubSourcesDto> subsourcedto = new List<SubSourcesDto>();
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListSubSourcesAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListSubSourcesItem> listSubSources = res.HcsListSubSourcesResult.SubSources.ToList();
                      FormatSummaryObject(listSubSources, subsourcedto, propertyCode);
                  }
                  var result =await _hcsListSubSourceService.BulkInsertWithProcAsync(subsourcedto);

                  return PrometheusResponse.Success(result, "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryObject(List<ListSubSourcesItem> listSubSources, List<SubSourcesDto> subsourcedto, string propertyCode)
        {
            foreach (var item in listSubSources)
            {
                SubSourcesDto dto = new SubSourcesDto()
                {
                    PropertyCode = propertyCode,
                    Code = item.Code,
                    Description = item.Description,
                    Listorder = item.Listorder, 
                    Visible = item.Visible, 
                };
                subsourcedto.Add(dto);
            }
        }
    }
}
