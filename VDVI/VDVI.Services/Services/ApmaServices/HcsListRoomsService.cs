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
    public class HcsListRoomsService : ApmaBaseService, IHcsListRoomsService
    {    
        public HcsListRoomsService()
        {
            
        }
        public async Task<Result<PrometheusResponse>> HcsListRoomsServiceAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListRoomsResponse res = new HcsListRoomsResponse();

                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListRoomsAsync(pmsAuthentication, propertyCode, "");

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
