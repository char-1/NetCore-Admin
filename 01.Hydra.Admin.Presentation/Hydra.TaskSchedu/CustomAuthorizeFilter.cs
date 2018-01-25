using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Hydra.Admin.UI
{
    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
