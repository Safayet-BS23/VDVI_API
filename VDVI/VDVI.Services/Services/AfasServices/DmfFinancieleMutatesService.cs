using CSharpFunctionalExtensions;
using DutchGrit.Afas;
using Framework.Core.AfasExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Extensions;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDVI.Repository.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;
using VDVI.Services.AfasInterfaces;
using VDVI.Services.Services.BaseService;

namespace VDVI.Services.AfasServices
{
    public class DmfFinancieleMutatesService : AfasBaseService, IdmfFinancieleMutatiesService
    {
        private readonly IdmFFinancieleMutationService _dmFFinancieleMutationService;
        private readonly IdmfAdministratiesService _idmfAdministratiesService;
        public DmfFinancieleMutatesService
            (
                IdmFFinancieleMutationService dmFFinancieleMutationService,
                IdmfAdministratiesService idmfAdministratiesService,
                AfasCrenditalsDto afasCrenditalsDto
            ) : base (afasCrenditalsDto)
        {
            _dmFFinancieleMutationService = dmFFinancieleMutationService;
            _idmfAdministratiesService = idmfAdministratiesService;
        }

        public async Task<Result<PrometheusResponse>> DmfFinancieleMutatiesServiceAsync(DateTime startDate)
        {

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    int startBusinessYear = startDate.Year;
                    int currentYear = DateTime.UtcNow.Year;

                    //List<DMFFinancieleMutatiesDto> dto = new List<DMFFinancieleMutatiesDto>();

                    do
                    {
                        List<string> administrationCodes = new List<string>();

                        var admininstrationCodesResponse = await _idmfAdministratiesService.GetDmfAdministratiesAsync();

                        if (admininstrationCodesResponse.IsSuccess)
                        {
                            administrationCodes = (admininstrationCodesResponse.Value.Data as List<DMFAdministratiesDto>)
                                                            .Select(p => p.Administratie_code.ToString()).Distinct().ToList();
                        }

                        //Netherlands (=Dutch)=aa  | Spain =ac| Bonaire =ad | Belgium=ae

                        await GetDMFFinancieleMutatiesAsync(AfasClients.clientAA, p => true, startBusinessYear.ToString(), administrationCodes, "AA");
                        await GetDMFFinancieleMutatiesAsync(AfasClients.clientAC, p => true, startBusinessYear.ToString(), administrationCodes, "AC");
                        await GetDMFFinancieleMutatiesAsync(AfasClients.clientAD, p => true, startBusinessYear.ToString(), administrationCodes, "AD");
                        await GetDMFFinancieleMutatiesAsync(AfasClients.clientAE, p => true, startBusinessYear.ToString(), administrationCodes, "AE");

                        startBusinessYear++;

                    } while (startBusinessYear <= currentYear);

                    return PrometheusResponse.Success("", "Data retrieval is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                }
            );

        }

        private async Task<List<DMFFinancieleMutatiesDto>> GetDMFFinancieleMutatiesAsync(AfasClient Client, Func<DMFFinancieleMutatiesDto, bool> predicate, string year, List<string> administrationCodes, string environmentCode)
        {
            List<DMFFinancieleMutatiesDto> result = new List<DMFFinancieleMutatiesDto>();

            int pageIndex = 0, pageSize = 1;

            for (int i = 1; i <= administrationCodes.Count; i = i + pageSize)
            {
                try
                {
                    var codes = administrationCodes.Skip(pageIndex * pageSize).Take(pageSize).ToArray();

                    var tempResult = (await Client.Query<DMFFinancieleMutatiesDto>()
                        .WhereEquals(x => x.Jaar, year)
                        .WhereEquals(x => x.Administratie_code, codes)
                        .Skip(-1).Take(-1).OrderBy(x => x.Rekeningnummer).GetAsync()).Where(predicate).ToList();
                    pageIndex++;

                    tempResult.FormatList(x => x.Omgeving_code, environmentCode);

                    if (tempResult.Count > 0)
                        await _dmFFinancieleMutationService.BulkInsertWithProcAsync(tempResult);

                } catch
                {
                    continue;
                }
            }

            return result;
        }

    }
}
