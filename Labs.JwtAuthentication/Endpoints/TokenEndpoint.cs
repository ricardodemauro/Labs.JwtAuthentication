using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Labs.JwtAuthentication.Endpoints
{
    public static class TokenEndpoint
    {
        internal static async Task<IResult> Connect(
            HttpContext ctx,
            JwtOptions jwtOptions)
        {
            if (ctx.Request.ContentType != "application/x-www-form-urlencoded")
                return Results.BadRequest(new { Error = "Invalid Request" });

            var formCollection = await ctx.Request.ReadFormAsync();

            if (formCollection.TryGetValue("grant_type", out var grantType) == false)
                return Results.BadRequest(new { Error = "Invalid Request" });

            if (formCollection.TryGetValue("username", out var userName) == false)
                return Results.BadRequest(new { Error = "Invalid Request" });

            if (formCollection.TryGetValue("password", out var password) == false)
                return Results.BadRequest(new { Error = "Invalid Request" });

            var (accessToken, expiration) = TokenEndpoint.CreateAccessToken(
                jwtOptions,
                userName,
                new[] { "read_todo", "create_todo" });

            var exp = expiration - DateTime.Now;
            return Results.Ok(new
            {
                access_token = accessToken,
                expiration = (int)double.Round(exp.TotalSeconds, 0, MidpointRounding.ToZero),
                type = "bearer"
            });
        }

        static (string, DateTime) CreateAccessToken(
            JwtOptions jwtOptions,
            string username,
            string[] permissions)
        {
            var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);
            var expiration = DateTime.Now.AddMinutes(15);

            var signingCredentials = new SigningCredentials(
                symmetricKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtClaimTypes.Subject, username),
                new Claim(JwtClaimTypes.Name, username),
                new Claim(JwtClaimTypes.Role, permissions[0]),
                new Claim(JwtClaimTypes.Audience, jwtOptions.Audience)
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials);

            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (rawToken, expiration);
        }
    }
}
