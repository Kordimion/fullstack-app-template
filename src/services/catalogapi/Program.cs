using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.RequireHttpsMetadata = false;
        options.Authority = "http://keycloak:8080/identity/realms/shop";
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy  =>
        {
            policy.WithOrigins("https://localhost/")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();


app.UseForwardedHeaders(fhOptions);
app.UsePathBase("/api/catalog");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else {
    
}


app.UseCors();

app.UseAuthentication();
app.Use(async (context, next) =>
    {
        // Do loging
        // Do work that doesn't write to the Response.
        System.Console.WriteLine(string.Join(", ",context.User.Claims.Select(i => i.ToString())));
        await next.Invoke();
        // Do logging or other work that doesn't write to the Response.
    });
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "hello world!");

app.Run();
