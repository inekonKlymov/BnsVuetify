//namespace Bns.Api.Auth;

//using Bns.Domain.Users;
//using Bns.Domain.Users.Errors;
//using Bns.Infrastructure.Database;
//using FluentResults;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//public interface IJwtUtils
//{
//    public string GenerateJwtToken(User user);
//    public Result<User> ValidateJwtToken(string token);
//    public RefreshToken GenerateRefreshToken(string ipAddress);
//}

//public class JwtUtils(AppDbContext context,IConfiguration config) 
//    : IJwtUtils
//{
//    public string GenerateJwtToken(User user)
//    {
//        var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(
//            issuer: config["Jwt:Issuer"],
//            audience: config["Jwt:Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddHours(1),
//            signingCredentials: creds);

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }

//    public Result<User> ValidateJwtToken(string token)
//    {
//        if (token == null) return Result.Fail(UserErrors.TokenIsNull);

//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
//        try
//        {
//            tokenHandler.ValidateToken(token, new TokenValidationParameters
//            {
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(key),
//                ValidateIssuer = false,
//                ValidateAudience = false,
//                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
//                ClockSkew = TimeSpan.Zero
//            }, out SecurityToken validatedToken);

//            var jwtToken = (JwtSecurityToken)validatedToken;
//            var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
//            var user = context.Users.FirstOrDefault(u => u.UserName == username);
//            if(user == null) return Result.Fail(UserErrors.UserNotFound);

//            // return user id from JWT token if validation successful
//            return Result.Ok(user);
//        }
//        catch(Exception ex)
//        {
//            // return null if validation fails
//            return Result.Fail(UserErrors.ValidationTokenError.CausedBy(ex));

//        }
//    }

//    public RefreshToken GenerateRefreshToken(string ipAddress)
//    {
//        var refreshToken = new RefreshToken
//        {
//            Token = getUniqueToken(),
//            // token is valid for 7 days
//            Expires = DateTime.UtcNow.AddDays(7),
//            Created = DateTime.UtcNow,
//            CreatedByIp = ipAddress
//        };

//        return refreshToken;

//        string getUniqueToken()
//        {
//            // token is a cryptographically strong random sequence of values
//            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
//            // ensure token is unique by checking against db
//            var tokenIsUnique = !context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

//            if (!tokenIsUnique)
//                return getUniqueToken();
            
//            return token;
//        }
//    }
//}