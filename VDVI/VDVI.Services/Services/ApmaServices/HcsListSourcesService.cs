using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Extensions;
using Framework.Core.Utility;
using SOAPService; 
using System.Threading.Tasks;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsListSourcesService : ApmaBaseService, IHcsListSourcesService
    {    
        public HcsListSourcesService()
        {
            
        }
        public async Task<Result<PrometheusResponse>> HcsListSourcesServiceAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListSourcesResponse res = new HcsListSourcesResponse();

                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListSourcesAsync(pmsAuthentication, propertyCode, "", "");

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
    }
}
