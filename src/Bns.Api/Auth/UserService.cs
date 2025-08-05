//namespace Bns.Api.Auth;

//using BCrypt.Net;
//using Bns.Domain.Common.Startup;
//using Bns.Domain.Users;
//using Bns.Domain.Users.Errors;
//using Bns.Infrastructure.Database;
//using FluentResults;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;

//public interface IUserService
//{
//    Task<Result<LoginResponse>> AuthenticateAsync(LoginRequest model, string ipAddress);
//    Task<Result<LoginResponse>> RefreshTokenAsync(string token, string ipAddress);
//    Task<Result> RevokeToken(string token, string ipAddress);
//}

//public class UserService(
//    AppDbContext context,
//    IJwtUtils jwtUtils,
//    IOptions<AppSettings> appSettings) 
//    : IUserService
//{
//    private readonly AppSettings _appSettings = appSettings.Value;

//    public async Task<Result<LoginResponse>> AuthenticateAsync(LoginRequest model, string ipAddress)
//    {
//        var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == model.Username);

//        // validate
//        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
//            return Result.Fail(UserErrors.UserLoginFail);

//        // authentication successful so generate jwt and refresh tokens
//        var jwtToken = jwtUtils.GenerateJwtToken(user);
//        var refreshToken = jwtUtils.GenerateRefreshToken(ipAddress);
//        user.RefreshTokens.Add(refreshToken);

//        // remove old refresh tokens from user
//        removeOldRefreshTokens(user);

//        // save changes to db
//        context.Update(user);
//        await context.SaveChangesAsync();

//        return new LoginResponse(user, jwtToken, refreshToken.Token);
//    }

//    public async Task<Result<LoginResponse>> RefreshTokenAsync(string token, string ipAddress)
//    {
//        var userResult = await getUserByRefreshTokenAsync(token);
//        if (userResult.IsFailed) return userResult.ToResult<LoginResponse>();
//        var user = userResult.Value;
//        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

//        if (refreshToken.IsRevoked)
//        {
//            // revoke all descendant tokens in case this token has been compromised
//            revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
//            context.Update(user);
//            await context.SaveChangesAsync();
//        }

//        if (!refreshToken.IsActive) return Result.Fail(UserErrors.InvalidToken);

//        // replace old refresh token with a new one (rotate token)
//        var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
//        user.RefreshTokens.Add(newRefreshToken);

//        // remove old refresh tokens from user
//        removeOldRefreshTokens(user);

//        // save changes to db
//        context.Update(user);
//        await context.SaveChangesAsync();

//        // generate new jwt
//        var jwtToken = jwtUtils.GenerateJwtToken(user);

//        return Result.Ok(new LoginResponse(user, jwtToken, newRefreshToken.Token));
//    }

//    public async Task<Result> RevokeToken(string token, string ipAddress)
//    {
//        var userResult = await getUserByRefreshTokenAsync(token);
//        if (userResult.IsFailed) return userResult.ToResult();
//        var user = userResult.Value;
//        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

//        if (!refreshToken.IsActive) return Result.Fail(UserErrors.InvalidToken);

//        // revoke token and save
//        revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
//        context.Update(user);
//        await context.SaveChangesAsync();
//        return Result.Ok();
//    }


//    private async Task<Result<User>> getUserByRefreshTokenAsync(string token)
//    {
//        var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

//        if (user == null) return Result.Fail(UserErrors.InvalidToken);

//        return Result.Ok(user);
//    }

//    private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
//    {
//        var newRefreshToken = jwtUtils.GenerateRefreshToken(ipAddress);
//        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
//        return newRefreshToken;
//    }

//    private void removeOldRefreshTokens(User user)
//    {
//        // remove old inactive refresh tokens from user based on TTL in app settings
//        user.RefreshTokens.RemoveAll(x =>
//            !x.IsActive &&
//            x.Created.AddDays(_appSettings.Jwt.RefreshTokenTTL) <= DateTime.UtcNow);
//    }

//    private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
//    {
//        // recursively traverse the refresh token chain and ensure all descendants are revoked
//        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
//        {
//            var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
//            if (childToken.IsActive)
//                revokeRefreshToken(childToken, ipAddress, reason);
//            else
//                revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
//        }
//    }

//    private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
//    {
//        token.Revoked = DateTime.UtcNow;
//        token.RevokedByIp = ipAddress;
//        token.ReasonRevoked = reason;
//        token.ReplacedByToken = replacedByToken;
//    }
//}
