using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var pathbase = builder.Configuration["PathBase"];
var authority = builder.Configuration["Auth:Authority"];
var spaHref = builder.Configuration["Cors:SpaHref"] ?? "";


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.RequireHttpsMetadata = false;
        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Catalog", policy =>
    {
        policy.RequireAuthenticatedUser();

        var scopes = new[] { "catalog.api.read" };
        policy.RequireAssertion(context => {
            var claim = context.User.FindFirst("scope");
            if(claim == null) { return false; }
            return claim.Value.Split(' ').Any(s =>
                scopes.Contains(s, StringComparer.Ordinal)
            );
        });
    });

var fhOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost

};

fhOptions.KnownProxies.Clear();
fhOptions.KnownNetworks.Clear();

IdentityModelEventSource.ShowPII = true;
if(!spaHref.IsNullOrEmpty() && spaHref != "/") {
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy  =>
        {
            policy.WithOrigins(spaHref)
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
}


var app = builder.Build();


app.UseForwardedHeaders(fhOptions);
if(!pathbase.IsNullOrEmpty())
    app.UsePathBase("/api/catalog");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "hello world!");

app.Run();
