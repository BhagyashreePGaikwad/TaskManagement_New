using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement_April_.AuthAttribute
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorize(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Unauthorized: User is not authenticated.", isSuccess = false });
                return;
            }

            if (_roles.Any() && !_roles.Any(role => context.HttpContext.User.IsInRole(role)))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Forbidden: User does not have sufficient permissions.", isSuccess = false });
                return;
            }
        }
    
    
    }
}
