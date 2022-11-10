using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.AfasInterfaces
{
    public interface IdmfBoekingsdagenMutatiesService 
    {
        Task<Result<PrometheusResponse>> DmfBoekingsdagenMutatiesServiceAsync();
    }
}
