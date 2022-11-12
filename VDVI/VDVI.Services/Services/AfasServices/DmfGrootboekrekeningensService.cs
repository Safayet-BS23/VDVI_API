using CSharpFunctionalExtensions;
using DutchGrit.Afas;
using Framework.Core.AfasExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions; 
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
    public class DmfGrootboekrekeningensService : AfasBaseService, IdmfGrootboekrekeningen
    {
        private readonly IdmfGrootboekrekeningenService _dmfGrootboekrekeningenService;
        public DmfGrootboekrekeningensService
            (
                IdmfGrootboekrekeningenService dmfGrootboekrekeningenService,
                AfasCrenditalsDto afasCrenditalsDto
            ) : base (afasCrenditalsDto)
        {
            _dmfGrootboekrekeningenService = dmfGrootboekrekeningenService;
        }        
        public async Task<Result<PrometheusResponse>> DmfGrootboekrekeningenServiceAsync()
        { 
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    List<DMFGrootboekrekeningenDto> dto = new List<DMFGrootboekrekeningenDto>();

                    //Netherlands (=Dutch)=aa  | Spain =ac| Bonaire =ad | Belgium=ae

                    dto.AddRange(await GetDMFGrootboekrekeningenAsync(AfasClients.clientAA, p => true, "AA"));
                    dto.AddRange(await GetDMFGrootboekrekeningenAsync(AfasClients.clientAC, p => true, "AC"));
                    dto.AddRange(await GetDMFGrootboekrekeningenAsync(AfasClients.clientAD, p => true, "AD"));
                    dto.AddRange(await GetDMFGrootboekrekeningenAsync(AfasClients.clientAE, p => true, "AE"));


                    // DB operation
                    if (dto.Count > 0)
                    {
                        var res = _dmfGrootboekrekeningenService.BulkInsertWithProcAsync(dto);
                    }

                    return PrometheusResponse.Success("", "Data retrieval is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                }
            );
        }

        private async Task<List<DMFGrootboekrekeningenDto>> GetDMFGrootboekrekeningenAsync(AfasClient Client, Func<DMFGrootboekrekeningenDto, bool> predicate, string environmentCode)
        {
            var result = (await Client.Query<DMFGrootboekrekeningenDto>().Skip(-1).Take(-1).OrderBy(x => x.Rekeningnummer).GetAsync()).Where(predicate).ToList();
            if (result.Count > 0)
                return result.FormatList(x => x.Omgeving_code, environmentCode);

            return new List<DMFGrootboekrekeningenDto>();
        }

    }
}
