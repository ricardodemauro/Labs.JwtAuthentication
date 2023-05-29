using Labs.JwtAuthentication;
using Labs.JwtAuthentication.Endpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        var signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
        opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7004",
            ValidAudience = "https://localhost:7004",
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/public", () => "Public Hello World!").AllowAnonymous();
app.MapGet("/private", () => "Private Hello World!").RequireAuthorization();

app.MapPost("/tokens/connect", (HttpContext ctx, JwtOptions jwtOptions)
    => TokenEndpoint.Connect(ctx, jwtOptions));

//read the jwt token from header
app.MapGet("/jwt-token/headers", (HttpContext ctx) =>
{
    if (ctx.Request.Headers.TryGetValue("Authorization", out var headerAuth))
    {
        var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
        return Task.FromResult(TypedResults.Ok(new { token = jwtToken }));
    }
    return Task.FromResult(TypedResults.NotFound(new { message = "jwt not found" }));
});

//read the jwt token from authentication context
app.MapGet("/jwt-token/context", async (HttpContext ctx) =>
{
    var token = await ctx.GetTokenAsync("access_token");

    return TypedResults.Ok(new { token = token });
});

app.Run();
