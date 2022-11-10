using CSharpFunctionalExtensions;
using DutchGrit.Afas;
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
    public class DmfBeginbalaniesService : AfasBaseService, IdmfBeginbalaniesService
    {
        private readonly IdmfBeginbalansService _dmfBeginbalansService;
        private readonly IdmfAdministratiesService _idmfAdministratiesService;
        public DmfBeginbalaniesService
            (
                IdmfBeginbalansService dmfBeginbalansService,
                IdmfAdministratiesService idmfAdministratiesService,
                AfasCrenditalsDto afasCrenditalsDto
            ): base (afasCrenditalsDto)
        {
            _dmfBeginbalansService = dmfBeginbalansService;
            _idmfAdministratiesService = idmfAdministratiesService;
        }
        public async Task<Result<PrometheusResponse>> DmfBeginbalanieServiceAsync(DateTime startDate)
        {        
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    List<DMFBeginbalansDto> dto = new List<DMFBeginbalansDto>();

                    int startBusinessYear = startDate.Year;
                    int currentYear = DateTime.UtcNow.Year;

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

                        dto.AddRange(await GetDmfBeginbalaniesAsync(AfasClients.clientAA, p => true, startBusinessYear.ToString(), administrationCodes, "AA"));
                        dto.AddRange(await GetDmfBeginbalaniesAsync(AfasClients.clientAC, p => true, startBusinessYear.ToString(), administrationCodes, "AC"));
                        dto.AddRange(await GetDmfBeginbalaniesAsync(AfasClients.clientAD, p => true, startBusinessYear.ToString(), administrationCodes, "AD"));
                        dto.AddRange(await GetDmfBeginbalaniesAsync(AfasClients.clientAE, p => true, startBusinessYear.ToString(), administrationCodes, "AE"));

                        startBusinessYear++;

                        // DB operation
                       var res = _dmfBeginbalansService.BulkInsertWithProcAsync(dto);
                        dto.Clear();

                    } while (startBusinessYear<= currentYear);
                  

                    return PrometheusResponse.Success("", "Data retrieval is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                }
            );
        }

        private async Task<List<DMFBeginbalansDto>> GetDmfBeginbalaniesAsync(AfasClient Client, Func<DMFBeginbalansDto, bool> predicate, string year, List<string> administrationCodes, string environmentCode)
        {
            List<DMFBeginbalansDto> result = new List<DMFBeginbalansDto>();

            int pageIndex = 0, pageSize = 50;

            for(int i = 1; i <= administrationCodes.Count; i = i + pageSize)
            {
                var tempResult = (await Client.Query<DMFBeginbalansDto>().WhereEquals(x => x.Jaar, year).WhereEquals(x => x.Administratie_code, administrationCodes.Skip(pageIndex * pageSize).Take(pageSize).ToArray()).Skip(-1).Take(-1).OrderBy(x => x.Rekeningnummer).GetAsync()).ToList().Where(predicate).ToList();
                pageIndex++;

                result.AddRange(tempResult);
            }

            if (result.Count > 0)
                result.FormatList(x => x.Omgeving_code, environmentCode);

            return result;
        }
    }
}
