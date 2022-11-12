using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDVI.Repository.Dtos.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;
using VDVI.Services.AfasInterfaces;
using VDVI.Services.Services.BaseService;
using DutchGrit.Afas;
using Framework.Core.Extensions;
using VDVI.Repository.AfasDtos;
using static Framework.Constants.Constants;
using Framework.Core.AfasExtensions;

namespace VDVI.Services.AfasServices
{
    public class DmfBoekingsdagenMutatesService : AfasBaseService, IdmfBoekingsdagenMutatiesService
    {
        private readonly IdmFFinancieleMutationService _dmFFinancieleMutationService;
        private readonly IdmFBoekingsdagenMutationService _dmFBoekingsdagenMutationService;

        public DmfBoekingsdagenMutatesService
            (
                IdmFBoekingsdagenMutationService dmFBoekingsdagenMutationService,
                IdmFFinancieleMutationService dmFFinancieleMutationService,
                AfasCrenditalsDto afasCrenditalsDto
            ) : base(afasCrenditalsDto)
        {
            _dmFFinancieleMutationService = dmFFinancieleMutationService;
            _dmFBoekingsdagenMutationService = dmFBoekingsdagenMutationService;
        }

        public async Task<Result<PrometheusResponse>> DmfBoekingsdagenMutatiesServiceAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {

                    var currentDate = DateTime.UtcNow.Date;

                    List<DMFBoekingsdagenMutatiesDto> boekingsdagenMutatieDtoList = new List<DMFBoekingsdagenMutatiesDto>();

                    boekingsdagenMutatieDtoList.AddRange(await GetBoekingsdagenMutatiesAsync(AfasClients.clientAA, p => p.Datum_boeking <= currentDate, "AA"));
                    boekingsdagenMutatieDtoList.AddRange(await GetBoekingsdagenMutatiesAsync(AfasClients.clientAC, p => p.Datum_boeking <= currentDate, "AC"));
                    boekingsdagenMutatieDtoList.AddRange(await GetBoekingsdagenMutatiesAsync(AfasClients.clientAD, p => p.Datum_boeking <= currentDate, "AD"));
                    boekingsdagenMutatieDtoList.AddRange(await GetBoekingsdagenMutatiesAsync(AfasClients.clientAE, p => p.Datum_boeking <= currentDate, "AE"));

                    if (boekingsdagenMutatieDtoList.Count > 0)
                    {
                        //DB operation
                        await _dmFBoekingsdagenMutationService.BulkInsertWithProcAsync(boekingsdagenMutatieDtoList);


                        var uniqueDatelist = boekingsdagenMutatieDtoList.Select(x => x.Datum_boeking).Distinct().ToList();
                        if (uniqueDatelist.Count > 0)
                        {
                            await GetDMFFinancieleMutatiesAsync(AfasClients.clientAA, p => true, uniqueDatelist, "AA");
                            await GetDMFFinancieleMutatiesAsync(AfasClients.clientAC, p => true, uniqueDatelist, "AC");
                            await GetDMFFinancieleMutatiesAsync(AfasClients.clientAD, p => true, uniqueDatelist, "AD");
                            await GetDMFFinancieleMutatiesAsync(AfasClients.clientAE, p => true, uniqueDatelist, "AE");
                        }
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

        private async Task<List<DMFBoekingsdagenMutatiesDto>> GetBoekingsdagenMutatiesAsync(AfasClient Client,
                                    Func<DMFBoekingsdagenMutatiesDto, bool> predicate, string environmentCode)
        {
            List<DMFBoekingsdagenMutatiesDto> result = new List<DMFBoekingsdagenMutatiesDto>();

            result = (await Client.Query<DMFBoekingsdagenMutatiesDto>().Skip(-1).Take(-1).GetAsync()).Where(predicate).ToList();
            if (result.Count > 0)
                return result.FormatList(x => x.Omgeving_code, environmentCode);

            return result;
        }
        private async Task GetDMFFinancieleMutatiesAsync(AfasClient Client, Func<DMFFinancieleMutatiesDto, bool> predicate,
                                      List<DateTime?> Listyear, string environmentCode)
        {
            List<DMFFinancieleMutatiesDto> result = new List<DMFFinancieleMutatiesDto>();

            foreach (var bookingDate in Listyear)
            {
                var bookingDateFormat = bookingDate.Value.ToString(DateFormatter.yyyy_MM_dd_Dash_DelimitedWithZeroTime);

                var financialResult = (await Client.Query<DMFFinancieleMutatiesDto>()
                        .WhereEquals(x => x.Datum_boeking, bookingDateFormat)
                        .Skip(-1).Take(-1).OrderBy(x => x.Rekeningnummer).GetAsync()).Where(predicate).ToList();


                financialResult.FormatList(x => x.Omgeving_code, environmentCode);

                if (financialResult.Count > 0)
                    await _dmFFinancieleMutationService.BulkInsertWithBoekingsdagenMutatiesAsync(financialResult);


            }

            return;
        }

    }
}
