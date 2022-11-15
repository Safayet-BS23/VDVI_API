using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using VDVI.Repository.Models;
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


        private readonly SchedulerLog schedulerLog;

        AfasSchedulerSetupDto dtos = new AfasSchedulerSetupDto();
        public AfasTaskSchedulerService(IAfasSchedulerSetupService afasschedulerSetupService,
                IdmfAdministratiesService idmfAdministratiesService,
                IdmfBeginbalaniesService idmfBeginbalaniesService,
                IAfasSchedulerLogService afasSchedulerLogService,
                IAfasSchedulerSetupService afasSchedulerSetupService,
                IdmfGrootboekrekeningen idmfGrootboekrekeningen,
                IdmfFinancieleMutatiesService idmfFinancieleMutatiesService,
                IdmfBoekingsdagenMutatiesService idmfBoekingsdagenMutatiesService,
                IOptions<SchedulerLog> schedulerLogOptions
            )
        {
            _afasschedulerSetupService = afasschedulerSetupService;
            _idmfAdministratiesService = idmfAdministratiesService;
            _afasSchedulerLogService = afasSchedulerLogService;
            _afasSchedulerSetupService = afasSchedulerSetupService;
            _idmfBeginbalaniesService = idmfBeginbalaniesService;
            _idmfGrootboekrekeningen = idmfGrootboekrekeningen;
            _idmfFinancieleMutatiesService = idmfFinancieleMutatiesService;
            _idmfBoekingsdagenMutatiesService = idmfBoekingsdagenMutatiesService;

            schedulerLog = schedulerLogOptions.Value;
        }


        public async Task SummaryScheduler()
        {
            bool flag = false;
            Result<PrometheusResponse> response;
            DateTime currentDateTime = DateTime.UtcNow;

            var logDayLimits = schedulerLog.AFASSchedulerLogLimitDays;

            var afasschedulers = await _afasschedulerSetupService.FindByAllScheduleAsync();

            var new1 = afasschedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var afasscheduler = new1[i];

                if (afasscheduler.SchedulerStatus == SchedulerStatus.Processing.ToString())
                    continue;

                if (
                        afasscheduler.NextExecutionDateTime != null
                        && afasscheduler.NextExecutionDateTime <= currentDateTime
                    )
                {
                    //Update SchedulerSetUp Status;
                    afasscheduler.SchedulerStatus= SchedulerStatus.Processing.ToString();
                    await _afasSchedulerSetupService.UpdateAsync( afasscheduler , afasscheduler.SchedulerName);

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

                    dtos.LastExecutionDateTime = currentDateTime;
                    //NextExecutionDateTime: 2022-10-25 15:30 ; ExecutionIntervalMins: 15 ;NextExecutionDateTime: 2022-10-25 15:45 
                    dtos.NextExecutionDateTime = afasscheduler.NextExecutionDateTime.Value.AddMinutes(afasscheduler.ExecutionIntervalMins);
                    dtos.SchedulerName = afasscheduler.SchedulerName;

                    if (flag)
                    {
                        dtos.SchedulerStatus = SchedulerStatus.Succeed.ToString();
                        await _afasSchedulerSetupService.SaveWithProcAsync(dtos);
                        await _afasSchedulerLogService.SaveWithProcAsync(afasscheduler.SchedulerName, logDayLimits);
                    } 
                }
            }
        }
    }
}
