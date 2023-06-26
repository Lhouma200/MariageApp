using MariageApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using MariageApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using MariageApp.API.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddMvc().AddNewtonsoftJson(o => 
{
    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});   

builder.Services.AddSignalR();
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
     
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IMariageRepository, MariageRepository>();
builder.Services.AddScoped<LogUserActivity>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });


builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);




var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

}
else
{
    app.UseExceptionHandler(BuilderExtensions =>
    {
        BuilderExtensions.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
                context.Response.AddApplicationError(error.Error.Message);

                await context.Response.WriteAsync(error.Error.Message);
            }
        });
    });
    // app.UseHsts();
}



app.UseHttpsRedirection();





app.UseCors("CORSPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub <ChatHub> ("/chat");
});



app.MapControllers();


app.Run();
