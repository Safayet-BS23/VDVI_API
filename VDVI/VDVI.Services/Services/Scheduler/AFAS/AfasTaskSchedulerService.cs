﻿using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using VDVI.Repository.Models.AfasModels.Dto;
using VDVI.Services.AfasInterfaces;
using VDVI.Services.Interfaces.AFAS;

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

        
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        public IConfiguration _config;

        AfasSchedulerSetupDto dtos = new AfasSchedulerSetupDto();
        public AfasTaskSchedulerService(IAfasSchedulerSetupService afasschedulerSetupService,
            IdmfAdministratiesService idmfAdministratiesService,
            IdmfBeginbalaniesService idmfBeginbalaniesService,
            IAfasSchedulerLogService afasSchedulerLogService,
            IAfasSchedulerSetupService afasSchedulerSetupService,
            IdmfGrootboekrekeningen idmfGrootboekrekeningen,
            IdmfFinancieleMutatiesService idmfFinancieleMutatiesService,
            IdmfBoekingsdagenMutatiesService idmfBoekingsdagenMutatiesService)
        {
            _afasschedulerSetupService = afasschedulerSetupService;
            _idmfAdministratiesService = idmfAdministratiesService;
            _afasSchedulerLogService = afasSchedulerLogService;
            _afasSchedulerSetupService = afasSchedulerSetupService;
            _idmfBeginbalaniesService = idmfBeginbalaniesService;
            _idmfGrootboekrekeningen = idmfGrootboekrekeningen;
            _idmfFinancieleMutatiesService = idmfFinancieleMutatiesService;
            _idmfBoekingsdagenMutatiesService = idmfBoekingsdagenMutatiesService;

            configurationBuilder.AddJsonFile("AppSettings.json");
            _config = configurationBuilder.Build();
        }


        public async Task SummaryScheduler()
        {
            bool flag = false;
            Result<PrometheusResponse> response;
            DateTime currentDateTime = DateTime.UtcNow;
            var logDayLimits = Convert.ToInt32(_config.GetSection("SchedulerLog").GetSection("AFASSchedulerLogLimitDays").Value);

            var afasschedulers = await _afasschedulerSetupService.FindByAllScheduleAsync();
            var new1 = afasschedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var afasscheduler = new1[i];

                if (afasscheduler.NextExecutionDateTime != null || afasscheduler.NextExecutionDateTime <= currentDateTime)
                {
                    switch (afasscheduler.SchedulerName)
                    {
                        case "DMFAdministraties":
                            response = await _idmfAdministratiesService.DmfAdministratiesAsync();
                            flag = response.IsSuccess;
                            break; 
                        case "DMFBeginbalans":
                            response = await _idmfBeginbalaniesService.DmfBeginbalanieServiceAsync((DateTime)afasscheduler.BusinessStartDate);
                            flag = response.IsSuccess;
                            break;
                        case "DMFGrootboekrekeningen": 
                            response = await _idmfGrootboekrekeningen.DmfGrootboekrekeningenServiceAsync();
                            flag = response.IsSuccess;
                            break;
                        case "DMFFinancieleMutaties":
                            response = await _idmfFinancieleMutatiesService.DmfFinancieleMutatiesServiceAsync((DateTime)afasscheduler.BusinessStartDate);
                            flag = response.IsSuccess;
                            break;
                        case "DMFBoekingsdagenMutaties":
                            response = await _idmfBoekingsdagenMutatiesService.DmfBoekingsdagenMutatiesServiceAsync((DateTime)afasscheduler.BusinessStartDate);
                            flag = response.IsSuccess;
                            break;
                        default:
                            break;
                    } 

                    dtos.LastExecutionDateTime = currentDateTime;
                    //NextExecutionDateTime: 2022-10-25 15:30 ; ExecutionIntervalMins: 15 ;NextExecutionDateTime: 2022-10-25 15:45 
                    dtos.NextExecutionDateTime = afasscheduler.NextExecutionDateTime.Value.AddMinutes(afasscheduler.ExecutionIntervalMins);                  
                    dtos.SchedulerName = afasscheduler.SchedulerName;

                    if (flag)
                    {
                        await _afasSchedulerSetupService.SaveWithProcAsync(dtos);
                        await _afasSchedulerLogService.SaveWithProcAsync(afasscheduler.SchedulerName, logDayLimits);
                    }
                }
            }
        }
    }
}
