using VDVI.Services.Interfaces.Scheduler;

namespace VDVI.Services.Implement.Scheduler
{
    public class HangfireAuthorizationFilter : IHangfireAuthorizationFilter
    {
        private readonly string[] _roles;

        public HangfireAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }
         
        public bool Authorize()
        { 
            return true;
        }
    }
}

 
