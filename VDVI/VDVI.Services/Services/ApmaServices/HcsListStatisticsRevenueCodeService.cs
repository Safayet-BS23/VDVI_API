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
    public class HcsListStatisticsRevenueCodeService : ApmaBaseService,  IHcsListStatisticsRevenueCodeService
    {
        private readonly IHcsStatisticsRevenueCodeService _hcsStatisticsRevenueCodeService;

        public HcsListStatisticsRevenueCodeService(IHcsStatisticsRevenueCodeService hcsStatisticsRevenueCodeService)
            : base ()
        {
            _hcsStatisticsRevenueCodeService = hcsStatisticsRevenueCodeService;
        }

        public async Task<Result<PrometheusResponse>> HcsListStatisticsRevenueCodeAsyc()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
              async () =>
              {
                  Authentication pmsAuthentication = GetApmaAuthCredential();
                  HcsListStatisticsRevenueCodesResponse res = new HcsListStatisticsRevenueCodesResponse();
                  var dto = new List<StatisticsRevenueCodesDto>();
                  for (int i = 0; i < ApmaProperties.Length; i++)
                  {
                      var propertyCode = ApmaProperties[i];
                      res = await client.HcsListStatisticsRevenueCodesAsync(pmsAuthentication, propertyCode, "");
                      List<StatisticsRevenueCode> Types = res.HcsListStatisticsRevenueCodesResult.StatisticsRevenueCodes.ToList();
                      FormatSummaryobject(dto, Types, propertyCode);
                  }
                  if (dto.Count > 0)
                  {
                      var result = await _hcsStatisticsRevenueCodeService.BulkInsertWithProcAsync(dto);
                  }

                  return PrometheusResponse.Success("", "Data retrieval is successful");
              },
              exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
              {
                  DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                  RethrowException = false
              });
        }

        private void FormatSummaryobject(List<StatisticsRevenueCodesDto> statisticsRevenueCodesList, List<StatisticsRevenueCode> Types, string propertyCode)
        {
            foreach (var item in Types)
            {
                StatisticsRevenueCodesDto dto = new StatisticsRevenueCodesDto()
                {
                    PropertyCode = propertyCode,
                    Type = item?.Type,
                    Description = item?.Description
                };

                statisticsRevenueCodesList.Add(dto);
            }
        }
    }

}
