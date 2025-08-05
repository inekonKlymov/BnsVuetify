//using Ardalis.ApiEndpoints;
//using Bns.Domain.Users;
//using FluentResults;
//using FluentResults.Extensions.AspNetCore;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;

//namespace Bns.Api.Auth;

//public record LoginRequest(string Username, string Password);

////public record LoginResponse(string Token);
//public record LoginResponse(Guid Id, string FullName, string Username, string JwtToken, [property: JsonIgnore] string RefreshToken)
//{
//    public LoginResponse(User user, string jwtToken, string refreshToken) : this(user.Id.Value, user.FullName, user.UserName, jwtToken, refreshToken)
//    {
//    }
//}

//public class LoginEndpoint(IUserService userService, IConfiguration config) : EndpointBaseAsync.WithRequest<LoginRequest>.WithActionResult<Result<LoginResponse>>
//{
//    [HttpPost("api/auth/login")]
//    public override async Task<ActionResult<Result<LoginResponse>>> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
//    {

//        var response = await userService.AuthenticateAsync(request, IpAddress());
//        if (response.IsFailed) return response.ToActionResult();
//        SetTokenCookie(response.Value.RefreshToken);
//        return response.ToActionResult();
//    }

   

  
//    private string IpAddress()
//    {
//        // get source ip address for the current request
//        if (Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
//            return value;
//        else if (HttpContext.Connection.RemoteIpAddress != null)
//            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
//        else
//            return "Unknown"; // Fallback value if RemoteIpAddress is null
//    }
//    private void SetTokenCookie(string token)
//    {
//        // append cookie with refresh token to the http response
//        var cookieOptions = new CookieOptions
//        {
//            HttpOnly = true,
//            Expires = DateTime.UtcNow.AddDays(7)
//        };
//        Response.Cookies.Append("refreshToken", token, cookieOptions);
//    }
//}
//public class RefreshTokenEndpoint(IUserService userService) : EndpointBaseAsync.WithoutRequest.WithActionResult<Result<LoginResponse>>
//{
//    public override async Task<ActionResult<Result<LoginResponse>>> HandleAsync(CancellationToken cancellationToken = default)
//    {
//        var refreshToken = Request.Cookies["refreshToken"];
//        var response = await userService.RefreshTokenAsync(refreshToken, IpAddress());
//        if (response.IsFailed) return response.ToActionResult();
//        SetTokenCookie(response.Value.RefreshToken);
//        return Ok(response);
//    }
//    private void SetTokenCookie(string token)
//    {
//        // append cookie with refresh token to the http response
//        var cookieOptions = new CookieOptions
//        {
//            HttpOnly = true,
//            Expires = DateTime.UtcNow.AddDays(7)
//        };
//        Response.Cookies.Append("refreshToken", token, cookieOptions);
//    }
//    private string IpAddress()
//    {
//        // get source ip address for the current request
//        if (Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
//            return value;
//        else if (HttpContext.Connection.RemoteIpAddress != null)
//            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
//        else
//            return "Unknown"; // Fallback value if RemoteIpAddress is null
//    }
//}
//public record RevokeTokenRequest(string Token);

//public class RevokeTokenEndpoint(IUserService userService) : EndpointBaseAsync.WithRequest<RevokeTokenRequest>.WithActionResult<Result>
//{
//    [HttpPost("api/auth/revoke-token")]
//    public override async Task<ActionResult<Result>> HandleAsync(RevokeTokenRequest request, CancellationToken cancellationToken = default)
//    {
//        // accept refresh token in request body or cookie
//        var token = request.Token ?? Request.Cookies["refreshToken"];

//        if (string.IsNullOrEmpty(token))
//            return BadRequest(new { message = "Token is required" });

//        await userService.RevokeToken(token, IpAddress());
//        return Ok(new { message = "Token revoked" });
//    }
//    private string IpAddress()
//    {
//        // get source ip address for the current request
//        if (Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
//            return value;
//        else if (HttpContext.Connection.RemoteIpAddress != null)
//            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
//        else
//            return "Unknown"; // Fallback value if RemoteIpAddress is null
//    }
//}