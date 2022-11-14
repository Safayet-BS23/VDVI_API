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
    public class HcsListPackageService : ApmaBaseService, IHcsListPackageService
    {
        public readonly IHcsListPackagesService _hcsListPackagesService;
        public HcsListPackageService(IHcsListPackagesService hcsListPackagesService)
        {
            _hcsListPackagesService = hcsListPackagesService;
        }
        public async Task<Result<PrometheusResponse>> HcsListPackagesServiceeAsync()
        {
            var result = new Result<PrometheusResponse>();

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListPackagesResponse res = new HcsListPackagesResponse();
                  List<PackagesDto> PackagesDto = new List<PackagesDto>();
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListPackagesAsync(pmsAuthentication, propertyCode, "", "");
                      List<ListPackage> list = res.HcsListPackagesResult.Packages.ToList();
                      FormatSummaryObject(PackagesDto, list, propertyCode);
                  }
                  if (PackagesDto.Count > 0)
                      result = await _hcsListPackagesService.BulkInsertWithProcAsync(PackagesDto);

                  return PrometheusResponse.Success(result, "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }
        private void FormatSummaryObject(List<PackagesDto> PackagesDto, List<ListPackage> list, string propertyCode)
        {
            foreach (var item in list)
            {
                PackagesDto dto = new PackagesDto()
                {
                    PropertyCode = propertyCode,
                    Code = item.Code,
                    Description = item.Description
                };
                PackagesDto.Add(dto);
            }
        }
    }
}
