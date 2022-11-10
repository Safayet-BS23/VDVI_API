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
using VDVI.Services.Interfaces.AfasInterfaces.Administrators;
using VDVI.Services.Services.BaseService;

namespace VDVI.Services.AfasServices
{
    public class DmfAdministratiesService : AfasBaseService, IdmfAdministratiesService
    {
        private readonly IdmfAdministraterService _dmfAdministraterService;
        public DmfAdministratiesService
            (
                IdmfAdministraterService dmfAdministraterService,
                AfasCrenditalsDto afasCrenditalsDto
            ): base (afasCrenditalsDto)
        {
            _dmfAdministraterService = dmfAdministraterService;
        } 

        public async Task<Result<PrometheusResponse>> DmfAdministratiesAsync()
        {

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {

                    var response = await GetDmfAdministratiesAsync();

                    if (response.IsSuccess)
                    {
                        var res = _dmfAdministraterService.BulkInsertWithProcAsync(response.Value.Data as List<DMFAdministratiesDto>);
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

        public async Task<Result<PrometheusResponse>> GetDmfAdministratiesAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {

                    List<DMFAdministratiesDto> DtoList = new List<DMFAdministratiesDto>();
                    DtoList.AddRange(await GetDmfAdministratiesAsync(AfasClients.clientAA, p => true, "AA"));
                    DtoList.AddRange(await GetDmfAdministratiesAsync(AfasClients.clientAC, p => true, "AC"));
                    DtoList.AddRange(await GetDmfAdministratiesAsync(AfasClients.clientAD, p => true, "AD"));
                    DtoList.AddRange(await GetDmfAdministratiesAsync(AfasClients.clientAE, p => true, "AE"));

                    return PrometheusResponse.Success(DtoList, "Data retrieval is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                }
            );
        }

        private async Task<List<DMFAdministratiesDto>> GetDmfAdministratiesAsync(AfasClient Client, Func<DMFAdministratiesDto, bool> predicate, string environmentCode)
        {
            var result = (await Client.Query<DMFAdministratiesDto>().Skip(-1).Take(-1).OrderBy(x => x.Administratie_code).GetAsync()).Where(predicate).ToList();
            if (result.Count > 0)
                return result.FormatList(x => x.Omgeving_code, environmentCode);

            return new List<DMFAdministratiesDto>();
        }
    }
}
