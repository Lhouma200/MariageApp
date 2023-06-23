using System.Security.Claims;
using MariageApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MariageApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
          public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContext.HttpContext.RequestServices.GetService<IMariageRepository>();
            var user = await repo.GetUser(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}