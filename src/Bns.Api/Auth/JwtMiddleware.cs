//namespace Bns.Api.Auth;

//using Bns.Domain.Common.Startup;
//using Microsoft.Extensions.Options;

//public class JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
//{
//    private readonly AppSettings _appSettings = appSettings.Value;

//    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
//    {
//        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//        var user = jwtUtils.ValidateJwtToken(token);
//        if (user.IsSuccess)
//        {
//            // attach user to context on successful jwt validation
//            context.Items["User"] = user.Value;
//        }

//        await next(context);
//    }
//}