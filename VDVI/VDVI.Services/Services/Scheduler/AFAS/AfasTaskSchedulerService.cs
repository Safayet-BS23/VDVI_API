using CSharpFunctionalExtensions;
using Framework.Core.Enums;
using System.Linq;
using System.Threading.Tasks;
using VDVI.Services.Interfaces.AFAS;
using Rebus.Bus;
using Microsoft.Extensions.Configuration;
using System;
using VDVI.Services.Rebus.Models;
using VDVI.Services.Interfaces.Scheduler.AFAS;
using Ocelot.Responses;
using VDVI.Services.AfasInterfaces;
using VDVI.Repository.Models;
using VDVI.Repository.Models.AfasModels.Dto;
using Microsoft.Extensions.Options;
using Framework.Core.Base.ModelEntity;

namespace VDVI.Services.AFAS
{
    public class AfasTaskSchedulerService : IAfasTaskSchedulerService
    {
        private readonly IAfasSchedulerSetupService _afasschedulerSetupService;
        private readonly IAfasSchedulerLogService _afasSchedulerLogService;
        private readonly IAfasSchedulerSetupService _afasSchedulerSetupService;
        private readonly IdmfAdministratiesService _idmfAdministratiesService;
        private readonly IdmfBeginbalaniesService _idmfBeginbalaniesService;
        private readonly IdmfGrootboekrekeningen _idmfGrootboekrekeningen;
        private readonly IdmfFinancieleMutatiesService _idmfFinancieleMutatiesService;
        private readonly IdmfBoekingsdagenMutatiesService _idmfBoekingsdagenMutatiesService;
        private readonly SchedulerLog schedulerLog;
        //private readonly IBus _eventBus;

        AfasSchedulerSetupDto dtos = new AfasSchedulerSetupDto();

        //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        //public IConfiguration _config; 

        public AfasTaskSchedulerService(
              IAfasSchedulerSetupService afasschedulerSetupService,
              IdmfAdministratiesService idmfAdministratiesService,
              IdmfBeginbalaniesService idmfBeginbalaniesService,
              IAfasSchedulerLogService afasSchedulerLogService,
              IAfasSchedulerSetupService afasSchedulerSetupService,
              IdmfGrootboekrekeningen idmfGrootboekrekeningen,
              IdmfFinancieleMutatiesService idmfFinancieleMutatiesService,
              IdmfBoekingsdagenMutatiesService idmfBoekingsdagenMutatiesService,
              IOptions<SchedulerLog> schedulerLogOptions
            //, IBus eventBus
            )
        {
            //configurationBuilder.AddJsonFile("AppSettings.json");
            //_config = configurationBuilder.Build();
            _afasschedulerSetupService = afasschedulerSetupService;
            _idmfAdministratiesService = idmfAdministratiesService;
            _afasSchedulerLogService = afasSchedulerLogService;
            _afasSchedulerSetupService = afasSchedulerSetupService;
            _idmfBeginbalaniesService = idmfBeginbalaniesService;
            _idmfGrootboekrekeningen = idmfGrootboekrekeningen;
            _idmfFinancieleMutatiesService = idmfFinancieleMutatiesService;
            _idmfBoekingsdagenMutatiesService = idmfBoekingsdagenMutatiesService;

            schedulerLog = schedulerLogOptions.Value;
            //_eventBus = eventBus;

        }


        public async Task SummaryScheduler()
        {
            bool flag = false;
            Result<PrometheusResponse> response;
            var afasschedulers = await _afasschedulerSetupService.FindByAllScheduleAsync();
            //var logDayLimits = Convert.ToInt32(_config.GetSection("SchedulerLog").GetSection("AFASSchedulerLogLimitDays").Value);
            var logDayLimits = schedulerLog.AFASSchedulerLogLimitDays;

            var new1 = afasschedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var afasscheduler = new1[i];

                if (afasscheduler.SchedulerStatus == SchedulerStatus.Processing.ToString())
                    continue;

                if (
                        afasscheduler.NextExecutionDateTime != null
                        && afasscheduler.NextExecutionDateTime <= DateTime.UtcNow
                    )
                {
                    //Update SchedulerSetUp Status;
                    afasscheduler.SchedulerStatus = SchedulerStatus.Processing.ToString();
                    await _afasschedulerSetupService.UpdateAsync(afasscheduler);


                    //AfasSchedulerEvent afasSchedulerEvent = new AfasSchedulerEvent
                    //{
                    //    Scheduler = afasscheduler,
                    //    BusinessStartDate = afasscheduler.BusinessStartDate
                    //};

                    //// Send notification to apma handlers
                    //await _eventBus.SendLocal(afasSchedulerEvent);


                    switch (afasscheduler.SchedulerName)
                    {
                        case "DMFAdministraties":
                            response = await _idmfAdministratiesService.DmfAdministratiesAsync();
                            flag = response.IsSuccess;
                            break;
                        case "DMFBeginbalans"://Opening Balance
                            response = await _idmfBeginbalaniesService.DmfBeginbalanieServiceAsync((DateTime)afasscheduler.BusinessStartDate);
                            flag = response.IsSuccess;
                            break;
                        case "DMFGrootboekrekeningen": //Ledger of Accounts
                            response = await _idmfGrootboekrekeningen.DmfGrootboekrekeningenServiceAsync();
                            flag = response.IsSuccess;
                            break;
                        case "DMFFinancieleMutaties"://Financial Mutations
                            response = await _idmfFinancieleMutatiesService.DmfFinancieleMutatiesServiceAsync((DateTime)afasscheduler.BusinessStartDate);
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
                        //NextExecutionDateTime: 2022-10-25 15:30 ; ExecutionIntervalMins: 15 ;NextExecutionDateTime: 2022-10-25 15:45 
                        dtos.NextExecutionDateTime = afasscheduler.NextExecutionDateTime.Value.AddMinutes(afasscheduler.ExecutionIntervalMins);
                        dtos.SchedulerName = afasscheduler.SchedulerName;

                        await _afasSchedulerSetupService.SaveWithProcAsync(dtos);
                        await _afasSchedulerLogService.SaveWithProcAsync(afasscheduler.SchedulerName, logDayLimits, DateTime.UtcNow);
                    } 

                } 
            }
        }

        public async Task ResetStatusAsync()
        {
            await _afasschedulerSetupService.ResetStatusAsync();
        }
    }
}
