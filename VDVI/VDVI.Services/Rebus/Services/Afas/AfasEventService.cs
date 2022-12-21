using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Enums;
using System;
using System.Threading.Tasks;
using VDVI.Repository.Models.AfasModels.Dto;
using VDVI.Services.AfasInterfaces;
using VDVI.Services.Interfaces.AFAS;
using VDVI.Services.MediatR.Services.Afas;
using VDVI.Services.Rebus.Models;

namespace VDVI.Services.Rebus.Services.Afas
{
    public class AfasEventService : IAfasEventService
    {
        private readonly IAfasSchedulerSetupService _afasschedulerSetupService;
        private readonly IAfasSchedulerLogService _afasSchedulerLogService;
        private readonly IdmfAdministratiesService _idmfAdministratiesService;
        private readonly IdmfBeginbalaniesService _idmfBeginbalaniesService;
        private readonly IdmfGrootboekrekeningen _idmfGrootboekrekeningen;
        private readonly IdmfFinancieleMutatiesService _idmfFinancieleMutatiesService;
        private readonly IdmfBoekingsdagenMutatiesService _idmfBoekingsdagenMutatiesService;
       
        public AfasEventService(IAfasSchedulerSetupService afasschedulerSetupService,
        IdmfAdministratiesService idmfAdministratiesService,
        IdmfBeginbalaniesService idmfBeginbalaniesService,
        IAfasSchedulerLogService afasSchedulerLogService,
        IdmfGrootboekrekeningen idmfGrootboekrekeningen,
        IdmfFinancieleMutatiesService idmfFinancieleMutatiesService,
        IdmfBoekingsdagenMutatiesService idmfBoekingsdagenMutatiesService)
        {
            _afasschedulerSetupService = afasschedulerSetupService;
            _idmfAdministratiesService = idmfAdministratiesService;
            _idmfBeginbalaniesService = idmfBeginbalaniesService;
            _afasSchedulerLogService = afasSchedulerLogService;
            _idmfGrootboekrekeningen = idmfGrootboekrekeningen;
            _idmfFinancieleMutatiesService = idmfFinancieleMutatiesService;
            _idmfBoekingsdagenMutatiesService = idmfBoekingsdagenMutatiesService;
        }
        public async Task ExecuteEventAsync(AfasSchedulerEvent afasSchedulerEvent)
        {
            bool flag = false;
            Result<PrometheusResponse> response;
            AfasSchedulerSetupDto dtos = new AfasSchedulerSetupDto();

            var logDayLimits = afasSchedulerEvent.LogDayLimits;

            switch (afasSchedulerEvent.Scheduler.SchedulerName)
            {
                case "DMFAdministraties":
                    response = await _idmfAdministratiesService.DmfAdministratiesAsync();
                    flag  = response.IsSuccess;
                    break;
                case "DMFBeginbalans"://Opening Balance
                    response = await _idmfBeginbalaniesService.DmfBeginbalanieServiceAsync((DateTime)afasSchedulerEvent.BusinessStartDate);
                    flag = response.IsSuccess;
                    break;
                case "DMFGrootboekrekeningen": //Ledger of Accounts
                    response = await _idmfGrootboekrekeningen.DmfGrootboekrekeningenServiceAsync();
                    flag = response.IsSuccess;
                    break;
                case "DMFFinancieleMutaties"://Financial Mutations
                    response = await _idmfFinancieleMutatiesService.DmfFinancieleMutatiesServiceAsync((DateTime)afasSchedulerEvent.BusinessStartDate);
                    flag = response.IsSuccess;
                    break;
                case "DMFBoekingsdagenMutaties"://Booking Dates Mutations
                    response = await _idmfBoekingsdagenMutatiesService.DmfBoekingsdagenMutatiesServiceAsync();
                    flag = response.IsSuccess;
                    break;
                default:
                    break;
            }

           
            if (flag)
            {
                dtos.LastExecutionDateTime = DateTime.UtcNow;
                dtos.NextExecutionDateTime = afasSchedulerEvent.Scheduler.NextExecutionDateTime.Value.AddMinutes(afasSchedulerEvent.Scheduler.ExecutionIntervalMins);
                dtos.SchedulerName = afasSchedulerEvent.Scheduler.SchedulerName;

                dtos.SchedulerStatus = SchedulerStatus.Succeed.ToString(); 
                await _afasschedulerSetupService.SaveWithProcAsync(dtos);
                await _afasSchedulerLogService.SaveWithProcAsync(afasSchedulerEvent.Scheduler.SchedulerName, logDayLimits, DateTime.UtcNow);
                afasSchedulerEvent.Scheduler.NextExecutionDateTime = dtos.NextExecutionDateTime;
            }
        }
    }
}
