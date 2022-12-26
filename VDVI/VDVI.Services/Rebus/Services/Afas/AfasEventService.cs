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
using Serilog;
using System.IO;
using System.Configuration;

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


            Log.Information($"Step-1=>>Afas: Afas Scheduler object: " + afasSchedulerEvent.Scheduler.SchedulerName +" NextExTime:-" + afasSchedulerEvent.Scheduler.NextExecutionDateTime+ " Current Data Time:-" + DateTime.UtcNow);

            bool flag = false;
            Result<PrometheusResponse> response;
            AfasSchedulerSetupDto dtos = new AfasSchedulerSetupDto();

            var logDayLimits = afasSchedulerEvent.LogDayLimits; 
           
            
            switch (afasSchedulerEvent.Scheduler.SchedulerName)
            {
                case "DMFAdministraties":
                    response = await _idmfAdministratiesService.DmfAdministratiesAsync();
                    flag  = response.IsSuccess;
                    dtos.SchedulerName = "DMFAdministraties";
                    break;
                case "DMFBeginbalans"://Opening Balance
                    response = await _idmfBeginbalaniesService.DmfBeginbalanieServiceAsync((DateTime)afasSchedulerEvent.BusinessStartDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "DMFBeginbalans";
                    break;
                case "DMFGrootboekrekeningen": //Ledger of Accounts
                    response = await _idmfGrootboekrekeningen.DmfGrootboekrekeningenServiceAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "DMFGrootboekrekeningen";
                    break;
                case "DMFFinancieleMutaties"://Financial Mutations
                    response = await _idmfFinancieleMutatiesService.DmfFinancieleMutatiesServiceAsync((DateTime)afasSchedulerEvent.BusinessStartDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "DMFFinancieleMutaties";
                    break;
                case "DMFBoekingsdagenMutaties"://Booking Dates Mutations
                    response = await _idmfBoekingsdagenMutatiesService.DmfBoekingsdagenMutatiesServiceAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "DMFBoekingsdagenMutaties";
                    break;
                default:
                    break;

            }

            Log.Information( $"Step-2=>>Afas: Afas Scheduler Name: " +afasSchedulerEvent.Scheduler.SchedulerName +" NextExTime:-"+ afasSchedulerEvent.Scheduler.NextExecutionDateTime+" Current UTC TIME:-"+ DateTime.UtcNow);

            if (afasSchedulerEvent.Scheduler.SchedulerName == dtos.SchedulerName && flag)
            {
                Log.Information($"Step-3=> After finished business implementations: "+ dtos.SchedulerName + " Current UTC TIME:-" + DateTime.UtcNow);
                //dtos.LastExecutionDateTime = DateTime.UtcNow;
               //dtos.NextExecutionDateTime = afasSchedulerEvent.Scheduler.NextExecutionDateTime.Value.AddMinutes(afasSchedulerEvent.Scheduler.ExecutionIntervalMins);

                //dtos.SchedulerStatus = SchedulerStatus.Succeed.ToString();
                Log.Information($"Step-4=>>Afas: Afas Scheduler Log Save Before: " + dtos.SchedulerName + " NextExTime:-" + afasSchedulerEvent.Scheduler.NextExecutionDateTime + " Current UTC TIME:-" + DateTime.UtcNow);

                await _afasschedulerSetupService.SaveWithProcAsync(dtos);

                await _afasSchedulerLogService.SaveWithProcAsync(dtos.SchedulerName, logDayLimits, DateTime.UtcNow);

                Log.Information($"Step-5=>>Afas: Afas Scheduler Log Save Afer: " + dtos.SchedulerName + " NextExTime:-" + dtos.NextExecutionDateTime+ " Current UTC TIME:-" + DateTime.UtcNow);

                //afasSchedulerEvent.Scheduler.NextExecutionDateTime = dtos.NextExecutionDateTime;
                //afasSchedulerEvent.Scheduler.SchedulerStatus = dtos.SchedulerStatus;
                flag = false;
                Log.Information($"Step-6=>>Refesh all object"+  " Current UTC TIME:-" + DateTime.UtcNow);

            }
        }
    }
}
