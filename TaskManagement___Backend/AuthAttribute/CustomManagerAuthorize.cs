using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement_April_.AuthAttribute
{
    public class CustomManagerAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Handle unauthenticated users
                context.Result = new UnauthorizedObjectResult(new { message = "Unauthorized: User is not authenticated.", isSuccess = false });
                return;
            }

            if (!context.HttpContext.User.IsInRole("Manager"))
            {
                // Handle unauthorized users
                context.Result = new UnauthorizedObjectResult(new { message = "Forbidden: User does not have sufficient permissions.", isSuccess = false });
                return;
            }
        }
   
    }
}
