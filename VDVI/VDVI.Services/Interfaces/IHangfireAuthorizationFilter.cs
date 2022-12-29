namespace VDVI.Services.Interfaces.Scheduler
{
    internal interface IHangfireAuthorizationFilter
    {
        bool Authorize();
    }
}
